using System;

namespace Server.Games
{
	public class PokerHand
	{
		private Card[] _cards;
		private CardDeck _deck;
		private byte _maxCards;

		public Card[] Cards { get { return _cards; } }
		/// <summary>
		/// The deck that delt this hand. Used to return the cards to the right place at the end of the round
		/// </summary>
		public CardDeck FromDeck { get { return _deck; } set { _deck = value; } }
		public byte MaxCards { get { return _maxCards; } set { SetMaxCards( value ); } }
		public byte Size { get { return (byte)_cards.Length; } }

		public PokerHand( byte maxCards )
		{
			_cards = new Card[maxCards];
		}

		#region +virtual bool GiveCard( Card )
		/// <summary>
		/// Inserts the new card into the first open position of this hand's array
		/// </summary>
		public virtual bool GiveCard( Card card )
		{
			bool cardGiven = false;

			for( byte i = 0; !cardGiven && i < Size; i++ )
			{
				if( _cards[i] == null )
				{
					_cards[i] = card;
					cardGiven = true;
				}
			}

			return cardGiven;
		}
		#endregion

		#region +virtual void ReturnToDeck()
		/// <summary>
		/// Returns all of the cards in this hand to the original <code>CardDeck</code>
		/// </summary>
		public virtual void ReturnToDeck()
		{
			if( FromDeck == null )
				return;

			for( byte i = 0; i < Size; i++ )
			{
				if( _cards[i] != null )
					FromDeck.ReturnCard( _cards[i] );
			}

			_cards = new Card[MaxCards];
		}
		#endregion

		#region ~virtual void SetMaxCards( byte )
		/// <summary>
		/// Invoked when the MaxCards property is changed, this method ensures the <code>Cards</code> array is the right size.
		/// </summary>
		protected virtual void SetMaxCards( byte val )
		{
			if( Size != val )
			{
				Card[] cardCopy = new Card[val];
				Array.ConstrainedCopy( _cards, 0, cardCopy, 0, Math.Min( _cards.Length, cardCopy.Length ) );

				_cards = cardCopy;
				_maxCards = val;
			}
		}
		#endregion

		#region +override string ToString()
		public override string ToString()
		{
			string res = "";

			for( byte i = 0; i < Size; i++ )
			{
				if( _cards[i] != null )
					res += _cards[i].ToSimpleString();
				else
					res += "-";

				if( i != (Size - 1) )
					res += ",";
			}

			return res;
		}
		#endregion
	}
}