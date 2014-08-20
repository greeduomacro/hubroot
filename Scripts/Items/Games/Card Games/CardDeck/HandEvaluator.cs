using System;
using System.Collections.Generic;

namespace Server.Games
{
	#region +enum HandEvalResult : byte
	/// <summary>
	/// Describes the types of hand results a <code>PokerHand</code> can have
	/// </summary>
	public enum HandEvalResult : byte
	{
		None = 0,
		/// <summary>
		/// A single pair of cards, e.g. 2c,2h
		/// </summary>
		OnePair,
		/// <summary>
		/// Two individual pairs of cards, e.g. Qc,Qd,4d,4h
		/// </summary>
		TwoPair,
		/// <summary>
		/// Three suits of the same numeric card, e.g. Ad,Ah,As
		/// </summary>
		ThreeKind,
		/// <summary>
		/// Five-card sequence, but not in the same suit, e.g. 7h,8d,9d,10c,Js
		/// </summary>
		Straight,
		/// <summary>
		/// Any five cards in the same suit, e.g. 2h,4h,8h,10h,Kh
		/// </summary>
		Flush,
		/// <summary>
		/// Three of a kind plus a single pair, e.g. 10c,10d,10s,Qh,Qs
		/// </summary>
		FullHouse,
		/// <summary>
		/// All four suits of the same numeric card, e.g. 5c,5d,5h,5s
		/// </summary>
		FourKind,
		/// <summary>
		/// Five-card sequence in the same suit, e.g. 2s,3s,4s,5s,6s
		/// </summary>
		StraightFlush,
		/// <summary>
		/// Best possible hand: top five cards of the same suit, e.g. Ac,Kc,Qc,Jc,Tc
		/// </summary>
		RoyalFlush
	}
	#endregion

	public static class HandEvaluator
	{
		#region +static HandEvalResult EvaluateHand( Card[] )
		/// <summary>
		/// Evaluates the worth of the given card hand
		/// </summary>
		/// <returns>a valid HandEvalResult value</returns>
		public static HandEvalResult EvaluateHand( Card[] cards, out CardValue highCardValue )
		{
			if( cards.Length != 5 && cards.Length != 7 )
			{
				Console.WriteLine( "Warning: Invalid card hand length ({0}) passed to HandEvaluator.EvaluateHand.", cards.Length );

				highCardValue = (CardValue)0;
				return HandEvalResult.None;
			}

			Array.Sort<Card>( cards );
			HandEvalResult res = HandEvalResult.None;
			highCardValue = (CardValue)0;

			bool hasStraight = HasStraight( cards );
			bool hasFlush = HasFlush( cards );
			Card[] checkArray;

			if( !hasStraight && cards.Length == 7 )
			{
				for( byte i = 0; !hasStraight && i < 3; i++ )
				{
					checkArray = new Card[5];
					Array.Copy( cards, i, checkArray, 0, 5 );

					hasStraight = HasStraight( checkArray );
				}
			}

			//not a regular straight, so let's check for a "royal straight" (Ten through Ace)
			if( !hasStraight && cards[0].Value == CardValue.Ace )
			{
				checkArray = new Card[4];
				Array.Copy( cards, (cards.Length == 5 ? 1 : (cards.Length == 7 ? 3 : 0)), checkArray, 0, 4 );

				hasStraight = (HasStraight( checkArray ) && checkArray[checkArray.Length - 1].Value == CardValue.King);

				//copy the Ace into the checkArray in order to keep the HasFlush method working (checks for 5 of the same suit)
				Card[] copy = checkArray;
				checkArray = new Card[5];
				Array.Copy( copy, checkArray, 4 );

				checkArray.SetValue( cards[0], 4 );

				if( hasStraight && HasFlush( checkArray ) )
				{
					highCardValue = CardValue.Ace;

					if( HasFlush( checkArray ) )
						res = HandEvalResult.RoyalFlush;
					else
						res = HandEvalResult.Straight;
				}
			}

			//not a royal flush or royal straight, so what else is it...
			if( res == HandEvalResult.None )
			{
				if( hasStraight && hasFlush )
					res = HandEvalResult.StraightFlush;
				else if( HasFourKind( cards, out highCardValue ) )
					res = HandEvalResult.FourKind;
				else if( HasFullHouse( cards, out highCardValue ) )
					res = HandEvalResult.FullHouse;
				else if( hasFlush )
					res = HandEvalResult.Flush;
				else if( hasStraight )
					res = HandEvalResult.Straight;
				else if( HasThreeKind( cards, out highCardValue ) )
					res = HandEvalResult.ThreeKind;
				else if( HasTwoPair( cards, out highCardValue ) )
					res = HandEvalResult.TwoPair;
				else if( HasSinglePair( cards, out highCardValue ) )
					res = HandEvalResult.OnePair;
			}

			//other methods did not determine the high card, just grab the last card (or the Ace)
			if( highCardValue == (CardValue)0 )
			{
				if( cards[0].Value == CardValue.Ace )
					highCardValue = CardValue.Ace;
				else
					highCardValue = cards[cards.Length - 1].Value;
			}

			return res;
		}
		#endregion

		#region -static byte[] FindSameValues( Card[], byte )
		/// <summary>
		/// Checks the given <code>Card</code> array for matching values. Returns a two-column byte array,
		/// where the first index is the pair value and the second index is the number of matches in the hand.
		/// For example, for a hand with three nines, this method returns {9, 3}
		/// </summary>
		/// <param name="cards">the <code>Card</code> array representing a player's hand</param>
		/// <param name="sameCount">number of matches to search for (such as '4' for four-of-a-kind)</param>
		private static byte[] FindSameValues( Card[] cards, byte sameCount )
		{
			byte[] res = new byte[2] { 0, 0 };
			byte highCard = (byte)(cards[0].Value == CardValue.Ace ? CardValue.Ace : cards[cards.Length - 1].Value);

			for( byte i = (byte)cards[cards.Length - 1].Value; i > 0; i-- )
			{
				res[0] = i;
				res[1] = 0;

				for( byte j = 0; j < cards.Length; j++ )
				{
					if( (byte)cards[j].Value == i && ++res[1] >= sameCount )
						return res;
				}
			}

			res[0] = 0;
			res[1] = 0;

			return res;
		}
		#endregion

		#region -static bool HasFlush( Card[] )
		/// <summary>
		/// Determines if the given card array contains a flush of five or more of the same suit
		/// </summary>
		private static bool HasFlush( Card[] cards )
		{
			if( cards.Length < 5 )
				return false;

			Dictionary<CardSuit, byte> flushTable = new Dictionary<CardSuit, byte>( 4 );
			flushTable[CardSuit.Clubs] = 0;
			flushTable[CardSuit.Diamonds] = 0;
			flushTable[CardSuit.Hearts] = 0;
			flushTable[CardSuit.Spades] = 0;

			for( byte i = 0; i < cards.Length; i++ )
			{
				flushTable[cards[i].Suit]++;
			}

			bool hasFlush = false;
			Dictionary<CardSuit, byte>.Enumerator itr = flushTable.GetEnumerator();

			while( !hasFlush && itr.MoveNext() )
			{
				hasFlush = (itr.Current.Value >= 5);
			}

			return hasFlush;
		}
		#endregion

		#region -static bool HasFourKind( Card[], out CardValue )
		/// <summary>
		/// Determines if the given card array has a four-of-a-kind. If so, <code>highCard</code> is set to the match value.
		/// </summary>
		private static bool HasFourKind( Card[] cards, out CardValue highCard )
		{
			byte[] res = FindSameValues( cards, 4 );
			highCard = (CardValue)res[0];

			return (res[1] == 4);
		}
		#endregion

		#region -static bool HasFullHouse( Card[], out CardValue )
		/// <summary>
		/// Determines if the given card array has a Full House; that is, a three-of-a-kind and a single pair.
		/// </summary>
		private static bool HasFullHouse( Card[] cards, out CardValue highCard )
		{
			highCard = (cards[0].Value == CardValue.Ace ? CardValue.Ace : cards[cards.Length - 1].Value);

			byte[] searchRes = FindSameValues( cards, 3 );

			if( searchRes[0] != 0 && searchRes[1] >= 3 ) //we have a three-of-a-kind
			{
				List<Card> list = new List<Card>();

				for( int i = 0; i < cards.Length; i++ )
					list.Add( cards[i] );

				list.Sort();
				list.RemoveRange( list.FindIndex( delegate( Card c ) { return (byte)c.Value == searchRes[0]; } ), (int)searchRes[1] );

				searchRes = FindSameValues( cards, 2 );

				if( searchRes[0] != 0 && searchRes[1] >= 2 )
					return true;
			}

			return false;
		}
		#endregion

		#region -static bool HasSinglePair( Card[], out CardValue )
		/// <summary>
		/// Determines if the given card array has a single pair of values. If so, <code>highCard</code> is set to the pair value.
		/// </summary>
		private static bool HasSinglePair( Card[] cards, out CardValue highCard )
		{
			byte[] res = FindSameValues( cards, 2 );
			highCard = (CardValue)res[0];

			return (res[1] == 2);
		}
		#endregion

		#region -static bool HasStraight( Card[] )
		/// <summary>
		/// Tests to see if the given array is a straight set
		/// </summary>
		private static bool HasStraight( Card[] cards )
		{
			return ((byte)(cards[cards.Length - 1].Value - cards[0].Value) == (cards.Length - 1));
		}
		#endregion

		#region -static bool HasThreeKind( Card[], out CardValue )
		/// <summary>
		/// Determines if the given card array has a three-of-a-kind. If so, <code>highCard</code> is set to the match value.
		/// </summary>
		private static bool HasThreeKind( Card[] cards, out CardValue highCard )
		{
			byte[] res = FindSameValues( cards, 3 );
			highCard = (CardValue)res[0];

			return (res[1] == 3);
		}
		#endregion

		#region -static bool HasTwoPair( Card[], out CardValue )
		/// <summary>
		/// Determines if the given card array has at least two pairs of cards. If so, <code>highCard</code> is set to the highest value of pairs.
		/// </summary>
		private static bool HasTwoPair( Card[] cards, out CardValue highCard )
		{
			byte numPairs = 0;
			highCard = (CardValue)0;

			for( byte i = 0; i < cards.Length; i++ )
			{
				for( byte j = (byte)(i + 1); j < cards.Length; j++ )
				{
					if( cards[i].Value == cards[j].Value )
					{
						numPairs++;

						if( cards[i].Value > highCard && highCard != CardValue.Ace )
							highCard = cards[i].Value;
					}
				}
			}

			return (numPairs >= 2);
		}
		#endregion
	}
}