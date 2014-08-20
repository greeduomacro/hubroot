using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.Games
{
	#region +enum DeckSize : byte
	/// <summary>
	/// Defines the number of decks in the poker deck
	/// </summary>
	public enum DeckSize : byte
	{
		One = 52,
		Two = 104,
		Three = 156,
		Four = 208
	}
	#endregion

	public class CardDeck
	{
#if false
		//ToDo Remove later -- for testing only
		[Server.CallPriority( -1 )]
		public static void Configure()
		{
			Server.Utility.PushColor( ConsoleColor.White );

			Card[] testHand = new Card[7]
		    {
		        new Card( CardSuit.Clubs, CardValue.Two ),
		        new Card( CardSuit.Diamonds, CardValue.Two ),
		        new Card( CardSuit.Hearts, CardValue.Three ),
		        new Card( CardSuit.Spades, CardValue.Three ),
		        new Card( CardSuit.Diamonds, CardValue.Three ),
		        new Card( CardSuit.Hearts, CardValue.Seven ),
		        new Card( CardSuit.Spades, CardValue.King )
		    };
			CardValue highCard;

			Console.WriteLine( "\n===== hand eval result: {0} [high card={1}]\n", HandEvaluator.EvaluateHand( testHand, out highCard ), highCard );

			#region old tests
			CardDeck deck = new CardDeck( DeckSize.One );
			Console.WriteLine( "\n===== single deck contents:\n{0}\n", deck.PrintContents() );

			Card c = new Card( CardSuit.Diamonds, CardValue.Ten );
			Console.WriteLine( "\n===== Ten of Diamonds: {0} | {1}\n", c.ToSimpleString(), c.ToString() );

			PokerHand hand1 = new PokerHand( 5 );
			PokerHand hand2 = new PokerHand( 5 );
			deck.DealCards( new PokerHand[] { hand1, hand2 }, 6 );

			Console.WriteLine( "\n===== hand1: {0}", hand1.ToString() );
			Console.WriteLine( "===== hand2: {0}", hand2.ToString() );
			Console.WriteLine( "\n===== new deck contents:\n{0}\n", deck.PrintContents() );

			hand1.ReturnToDeck();
			hand2.ReturnToDeck();

			Stopwatch sw = new Stopwatch();
			byte shuffleAmt = byte.MaxValue;
			Console.WriteLine( "===== hands returned to deck...shuffling {0} times", shuffleAmt );
			sw.Start();
			deck.ShuffleDeck( shuffleAmt );
			sw.Stop();
			Console.WriteLine( "===== deck shuffled, time taken: {0} milliseconds | {1} seconds", sw.ElapsedMilliseconds, (sw.ElapsedMilliseconds / 1000) );
			Console.WriteLine( "\n===== deck contents:\n{0}\n", deck.PrintContents() );
			Console.WriteLine( "===== deck length={0}", deck.deck.Count );

			PokerHand pkh = new PokerHand( 7 );
			deck.DealCards( new PokerHand[] { pkh }, 7 );
			Console.WriteLine( "\n===== unsorted: {0}", pkh.ToString() );
			Array.Sort<Card>( pkh.Cards );
			Console.WriteLine( "===== sorted: {0}", pkh.ToString() );
			#endregion

			Server.Utility.PopColor();
		}
#endif

		private Stack<Card> _deck;
		private DeckSize _size;
		private bool _useJokers;

		protected Stack<Card> deck { get { return _deck; } set { _deck = value; } }

		public DeckSize Size { get { return _size; } protected set { _size = value; } }
		public bool UseJokers { get { return _useJokers; } protected set { _useJokers = value; } }

		public CardDeck( DeckSize size ) : this( size, false ) { }

		public CardDeck( DeckSize size, bool useJokers )
		{
			_size = size;
			_useJokers = useJokers;

			_deck = BuildDeck();
			ShuffleDeck( 4 );
		}

		#region +virtual void DealCards( PokerHand[], byte )
		/// <summary>
		/// Deals a specified amount of cards to each <code>PokerHand</code>
		/// </summary>
		/// <param name="playerHands">array of players' <code>PokerHand</code>s</param>
		/// <param name="dealAmt">amount to deal to each <code>PokerHand</code></param>
		public virtual void DealCards( PokerHand[] playerHands, byte dealAmt )
		{
			Card card;
			List<Card> recycleCards = new List<Card>();

			for( byte i = 0; i < dealAmt; i++ )
			{
				for( byte j = 0; j < (byte)playerHands.Length; j++ )
				{
					card = DrawOne();

					if( card == null )
						break;

					if( playerHands[j].GiveCard( card ) )
						playerHands[j].FromDeck = this;
					else
						recycleCards.Add( card );
				}
			}

			recycleCards.Reverse();
			recycleCards.ForEach(
				delegate( Card c )
				{
					deck.Push( c );
				} );
			recycleCards.Clear();
		}
		#endregion

		#region +virtual Card DrawOne()
		/// <summary>
		/// Draws the top <code>Card</code> and returns it
		/// </summary>
		public virtual Card DrawOne()
		{
			if( deck.Count >= 1 )
				return deck.Pop();

			return null;
		}
		#endregion

		#region +virtual Card Peek()
		/// <summary>
		/// Peeks at the top <code>Card</code> in the deck
		/// </summary>
		public virtual Card Peek()
		{
			return deck.Peek();
		}
		#endregion

		#region +virtual string PrintContents()
		/// <summary>
		/// Returns a string loaded with comma-separated descriptors of each <code>Card</code> in the deck
		/// </summary>
		public virtual string PrintContents()
		{
			string res = "";
			Card[] deck = this.deck.ToArray();

			for( int i = 0; i < deck.Length; i++ )
			{
				res += deck[i].ToSimpleString();

				if( i != (deck.Length - 1) )
					res += ",";
			}

			return res;
		}
		#endregion

		#region +virtual void ReturnCard( Card )
		/// <summary>
		/// Pushes the given card back onto the deck, if it is not already there
		/// </summary>
		public virtual void ReturnCard( Card card )
		{
			if( !deck.Contains( card ) )
				deck.Push( card );
		}
		#endregion

		#region +virtual void ShuffleDeck( byte )
		/// <summary>
		/// Randomly shuffles the deck
		/// </summary>
		/// <param name="numShuffles">number of times to re-shuffle the deck</param>
		public virtual void ShuffleDeck( byte numShuffles )
		{
			Card[] deck = this.deck.ToArray();
			Random rdm = new Random();
			int rdmIdx;
			Card temp;

			for( byte i = 0; i < numShuffles; i++ )
			{
				for( byte j = 0; j < (byte)deck.Length; j++ )
				{
					rdmIdx = (rdm.Next( 0, deck.Length ) % (deck.Length - 1) + 1);

					temp = deck[j];
					deck[j] = deck[rdmIdx];
					deck[rdmIdx] = temp;
				}

				//ToDo possibly add more randomization by cutting the deck here, then continuing with the next shuffle as above
			}

			this.deck.Clear();

			for( int i = 0; i < deck.Length; i++ )
				this.deck.Push( deck[i] );
		}
		#endregion

		#region ~virtual Stack<Card> BuildDeck()
		protected virtual Stack<Card> BuildDeck()
		{
			Stack<Card> intDeck = new Stack<Card>( (int)_size );
			byte deckCount = (byte)((byte)_size / 52);

			for( byte i = 0; i < deckCount; i++ )
			{
				for( sbyte j = (sbyte)(Card.CardTable.Length - 1); j >= 0; j-- )
					intDeck.Push( Card.Parse( Card.CardTable[j] ) );
			}

			return intDeck;
		}
		#endregion
	}
}