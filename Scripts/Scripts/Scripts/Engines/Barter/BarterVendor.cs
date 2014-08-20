using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Barter
{
	#region +enum TradeGoodInterest
	public enum TradeGoodInterest : byte
	{
		None,
		Minimal,
		Moderate,
		High,
		VeryHigh
	}
	#endregion

	#region +enum TradeGoods [Flags]
	[Flags]
	public enum TradeGoods
	{
		None			= 0x0000,
		Armor			= 0x0001,
		Books			= 0x0002,
		Clothing		= 0x0004,
		Construction	= 0x0008,
		Food			= 0x0010,
		Gems			= 0x0020,
		Harvest			= 0x0040,
		Jewelry			= 0x0080,
		Resources		= 0x0100,
		Shields			= 0x0200,
		Special			= 0x0400,
		Tools			= 0x0800,
		Weapons			= 0x1000
	}
	#endregion

	public class BarterVendor : BaseCreature
	{
		private bool _busyInTrade;
		private TradeGoods _interestingGoods;
		private Dictionary<TradeGoods, TradeGoodInterest> _interestLevelTable;

		protected TradeGoods interestingGoods { get { return _interestingGoods; } private set { _interestingGoods = value; } }
		protected Dictionary<TradeGoods, TradeGoodInterest> interestLevelTable { get { return _interestLevelTable; } private set { _interestLevelTable = value; } }

		[CommandProperty( AccessLevel.Counselor, true )]
		public bool BusyInTrade { get { return _busyInTrade; } set { _busyInTrade = value; } }

		public BarterVendor( string title )
			: base( AIType.AI_Vendor, FightMode.Aggressor, 10, 1, 4.0, 2.0 )
		{
			_interestLevelTable = new Dictionary<TradeGoods, TradeGoodInterest>();

			Female = Utility.RandomBool();

			Body = (Female ? 0x191 : 0x190);
			Hue = Utility.RandomSkinHue();
			Name = NameList.RandomName( (Female ? "female" : "male") );
			Title = title;

			InitStats( 100, 80, 80 );
			InitOutfit();

			SetInterestLevel( TradeGoods.None, TradeGoodInterest.None );
		}

		public BarterVendor( Serial serial ) : base( serial ) { }

		#region ~virtual void InitTrade( Mobile )
		protected virtual void InitTrade( Mobile m )
		{
			if( BusyInTrade )
			{
				PrivateOverheadMessage( MessageType.Regular, this.SpeechHue, true, "Good day! Please wait until I finish with my other client.", m.NetState );
			}
			else
			{
				BusyInTrade = true;
				Direction = GetDirectionTo( m );
				Frozen = true;

				m.SendGump( new BarterGump( this, m ) );
			}
		}
		#endregion

		#region +override bool HandlesOnSpeech( Mobile )
		public override bool HandlesOnSpeech( Mobile from )
		{
			return (from is PlayerMobile);
		}
		#endregion

		#region +override void OnSpeech( SpeechEventArgs )
		public override void OnSpeech( SpeechEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m is PlayerMobile && m.Backpack != null )
			{
				if( m.InRange( this, 3 ) && m.CanSee( this ) )
				{
					if( args.Speech.ToLower().IndexOf( "trade" ) > -1 )
					{
						InitTrade( m );

						args.Handled = true;
					}
				}
			}

			base.OnSpeech( args );
		}
		#endregion

		#region +virtual void DisposeTrade( Mobile, bool )
		public virtual void DisposeTrade( Mobile m, bool sendThanksMessage )
		{
			BusyInTrade = false;
			Frozen = false;

			if( sendThanksMessage )
				PublicOverheadMessage( MessageType.Regular, this.SpeechHue, true, String.Format( "{0}, I thank thee for the visit. Please come again!" ), false );
		}
		#endregion

		#region +virtual int GetRandomClothingHue()
		public virtual int GetRandomClothingHue()
		{
			switch( Utility.Random( 5 ) )
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomGreenHue();
				case 2: return Utility.RandomRedHue();
				case 3: return Utility.RandomNeutralHue();
				case 4: return Utility.RandomDyedHue();
			}
		}
		#endregion

		#region +virtual void InitOutfit()
		public virtual void InitOutfit()
		{
			switch( Utility.Random( 3 ) )
			{
				case 0: AddItem( new Shirt( GetRandomClothingHue() ) ); break;
				case 1:
				case 2: AddItem( new FancyShirt( GetRandomClothingHue() ) ); break;
			}

			switch( Utility.Random( 4 ) )
			{
				case 0: AddItem( new Boots() ); break;
				case 1: AddItem( new Sandals() ); break;
				case 2:
				case 3: AddItem( new Shoes( Utility.RandomNeutralHue() ) ); break;
			}

			Utility.AssignRandomHair( this, Utility.RandomHairHue() );
			Utility.AssignRandomFacialHair( this, this.HairHue );

			if( Female )
			{
				switch( Utility.Random( 5 ) )
				{
					case 0: AddItem( new ShortPants( GetRandomClothingHue() ) ); break;
					case 1: AddItem( new Skirt( GetRandomClothingHue() ) ); break;
					case 2:
					case 3:
					case 4: AddItem( new PlainDress( GetRandomClothingHue() ) ); break;
				}
			}
			else
			{
				switch( Utility.Random( 4 ) )
				{
					case 0: AddItem( new ShortPants( GetRandomClothingHue() ) ); break;
					case 1: AddItem( new Kilt( GetRandomClothingHue() ) ); break;
					case 2:
					case 3: AddItem( new LongPants( GetRandomClothingHue() ) ); break;
				}
			}
		}
		#endregion

		#region +bool IsInterestedIn( TradeGoods, out TradeGoodInterest )
		/// <summary>
		/// Determines if this vendor is interested in a good type
		/// </summary>
		/// <param name="goodType">TradeGoods type to check</param>
		public bool IsInterestedIn( TradeGoods goodType, out TradeGoodInterest currentInterest )
		{
			currentInterest = TradeGoodInterest.None;

			if( (interestingGoods & goodType) != 0 )
			{
				currentInterest = interestLevelTable[goodType];
				return true;
			}

			return false;
		}
		#endregion

		#region +void SetInterestLevel( TradeGoods, TradeGoodInterest )
		/// <summary>
		/// Sets the level of interest this vendor has for a given good type
		/// </summary>
		/// <param name="goodType">TradeGoods type to apply the interest level to</param>
		/// <param name="interestLevel">new interest level for the given good type</param>
		public void SetInterestLevel( TradeGoods goodType, TradeGoodInterest interestLevel )
		{
			if( interestLevel == TradeGoodInterest.None )
				interestingGoods &= ~goodType;
			else
				interestingGoods |= goodType;

			interestLevelTable[goodType] = interestLevel;
		}
		#endregion

		#region +override void Serialize( GenericWriter )
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (byte)0 );

			//version 0
			writer.Write( (int)_interestingGoods );

			if( _interestLevelTable.Count > 0 )
			{
				writer.Write( true );
				writer.Write( (int)_interestLevelTable.Count );

				foreach( KeyValuePair<TradeGoods, TradeGoodInterest> kvp in _interestLevelTable )
				{
					writer.Write( (int)kvp.Key );
					writer.Write( (byte)kvp.Value );
				}
			}
			else
			{
				writer.Write( false );
			}
		}
		#endregion

		#region +override void Deserialize( GenericReader )
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			byte version = reader.ReadByte();

			switch( version )
			{
				case 0:
					{
						_interestingGoods = (TradeGoods)reader.ReadInt();

						if( reader.ReadBool() )
						{
							int capacity = reader.ReadInt();
							_interestLevelTable = new Dictionary<TradeGoods, TradeGoodInterest>( capacity );

							for( int i = 0; i < capacity; i++ )
							{
								_interestLevelTable.Add( (TradeGoods)reader.ReadInt(), (TradeGoodInterest)reader.ReadByte() );
							}
						}
						break;
					}
			}
		}
		#endregion
	}
}