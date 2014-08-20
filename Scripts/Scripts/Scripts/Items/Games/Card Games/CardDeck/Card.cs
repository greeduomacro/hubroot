using System;

namespace Server.Games
{
	#region +enum CardSuit : byte
	/// <summary>
	/// Defines the colored suit of a <code>Card</code>
	/// </summary>
	public enum CardSuit : byte
	{
		Clubs = 0,
		Diamonds,
		Hearts,
		Spades
	}
	#endregion

	#region +enum CardValue : byte
	/// <summary>
	/// Defines the numeric value of a <code>Card</code>
	/// </summary>
	public enum CardValue : byte
	{
		Ace = 1,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King = 13
	}
	#endregion

	public class Card : IComparable<Card>
	{
		public static readonly string[] CardTable = new string[52]
			{
				"Ac", "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc",
				"Ad", "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd",
				"Ah", "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh",
				"As", "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks"
			};

		#region unicode suit symbols
		public static readonly string[] UnicodeOutlineSuitSymbols = new string[4]
			{
				"\u2667",	//clubs
				"\u2662",	//diamonds
				"\u2661",	//hearts
				"\u2664"	//spades
			};

		public static readonly string[] UnicodeSolidSuitSymbols = new string[4]
			{
				"\u2663",	//clubs
				"\u2666",	//diamonds
				"\u2665",	//hearts
				"\u2660"	//spades
			};
		#endregion

		private CardSuit _suit;
		private CardValue _value;

		public CardSuit Suit { get { return _suit; } }
		public CardValue Value { get { return _value; } }

		public Card( CardSuit suit, CardValue value )
		{
			_suit = suit;
			_value = value;
		}

		#region +static Card Parse( string descriptor )
		/// <summary>
		/// Creates a new card from a two-char card descriptor, as seen in the <code>Card.CardTable</code> array.
		/// </summary>
		/// <param name="descriptor">two-char string representation describing the new card's suit and value</param>
		public static Card Parse( string descriptor )
		{
			if( descriptor.Length != 2 )
				return new Card( CardSuit.Spades, CardValue.Ace );

			CardSuit newSuit = CardSuit.Spades;
			CardValue newVal = CardValue.Ace;

			switch( descriptor[1] )
			{
				case 'c': newSuit = CardSuit.Clubs; break;
				case 'd': newSuit = CardSuit.Diamonds; break;
				case 'h': newSuit = CardSuit.Hearts; break;
				case 's': newSuit = CardSuit.Spades; break;
			}

			switch( descriptor[0] )
			{
				case 'A': newVal = CardValue.Ace; break;
				case 'T': newVal = CardValue.Ten; break;
				case 'J': newVal = CardValue.Jack; break;
				case 'Q': newVal = CardValue.Queen; break;
				case 'K': newVal = CardValue.King; break;
				default:
					{
						newVal = (CardValue)Int32.Parse( descriptor[0].ToString() );
						break;
					}
			}

			return new Card( newSuit, newVal );
		}
		#endregion

		#region +int CompareTo( Card )
		public int CompareTo( Card card )
		{
			if( card.Value == this.Value )
				return 0;

			if( card.Value == this.Value )
				return this.Suit.CompareTo( card.Suit );

			return this.Value.CompareTo( card.Value );
		}
		#endregion

		#region +string ToSimpleString()
		/// <summary>
		/// Converts the card suit and value to a simple string,
		/// such as "As" for Ace of Spades or "9c" for the Nine of Clubs
		/// </summary>
		public string ToSimpleString()
		{
			string res = "";

			switch( _value )
			{
				case CardValue.Ace: res = "A"; break;
				case CardValue.Ten: res = "T"; break;
				case CardValue.Jack: res = "J"; break;
				case CardValue.Queen: res = "Q"; break;
				case CardValue.King: res = "K"; break;
				default:
					{
						res = Enum.Format( typeof( CardValue ), _value, "d" );
						break;
					}
			}

			switch( _suit )
			{
				case CardSuit.Clubs: res += "c"; break;
				case CardSuit.Diamonds: res += "d"; break;
				case CardSuit.Hearts: res += "h"; break;
				case CardSuit.Spades: res += "s"; break;
			}

			return res;
		}
		#endregion

		#region +override string ToString()
		public override string ToString()
		{
			return String.Format( "{0} of {1}", _value, _suit );
		}
		#endregion
	}
}