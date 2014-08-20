using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseBeverageBottleGroup : Item, IHasGroupQuantity
	{
		private BeverageGroupType m_Content;
		private int m_Quantity;
		private Mobile m_Poisoner;
		private Poison m_Poison;

		public override int LabelNumber
		{
			get
			{
				int num = BaseLabelNumber;

				if( IsEmpty || num == 0 )
					return EmptyLabelNumber;

				return BaseLabelNumber + (int)m_Content;
			}
		}

		public virtual bool ShowQuantity { get { return (MaxQuantity > 1); } }
		public virtual bool Fillable { get { return false; } }
		public virtual bool Pourable { get { return true; } }

		public virtual int EmptyLabelNumber { get { return base.LabelNumber; } }
		public virtual int BaseLabelNumber { get { return 0; } }

		public abstract int MaxQuantity { get; }
		public abstract int ComputeItemID();

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsEmpty { get { return (m_Quantity <= 0); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsFull { get { return (m_Quantity >= MaxQuantity); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner
		{
			get { return m_Poisoner; }
			set { m_Poisoner = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BeverageGroupType Content
		{
			get { return m_Content; }
			set
			{
				m_Content = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if( itemID > 0 )
					ItemID = itemID;
				else
					Delete();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Quantity
		{
			get { return m_Quantity; }
			set
			{
				if( value < 0 )
					value = 0;
				else if( value > MaxQuantity )
					value = MaxQuantity;

				m_Quantity = value;

				InvalidateProperties();

				int itemID = ComputeItemID();

				if( itemID > 0 )
					ItemID = itemID;
				else
					Delete();
			}
		}

		public virtual bool ValidateUse( Mobile from, bool message )
		{
			if( Deleted )
				return false;

			if( !Movable && !Fillable )
			{
				Multis.BaseHouse house = Multis.BaseHouse.FindHouseAt( this );

				if( house == null || !house.IsLockedDown( this ) )
				{
					if( message )
						from.SendLocalizedMessage( 502946, "", 0x59 ); // That belongs to someone else.

					return false;
				}
			}

			if( from.Map != Map || !from.InRange( GetWorldLocation(), 2 ) || !from.InLOS( this ) )
			{
				if( message )
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.

				return false;
			}

			return true;
		}

		#region Effects of achohol
		private static Hashtable m_Table = new Hashtable();

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			CheckHeaveTimer( e.Mobile );
		}

		public static void CheckHeaveTimer( Mobile from )
		{
			if( from.BAC > 0 && from.Map != Map.Internal && !from.Deleted )
			{
				Timer t = (Timer)m_Table[from];

				if( t == null )
				{
					if( from.BAC > 60 )
						from.BAC = 60;

					t = new HeaveTimer( from );
					t.Start();

					m_Table[from] = t;
				}
			}
			else
			{
				Timer t = (Timer)m_Table[from];

				if( t != null )
				{
					t.Stop();
					m_Table.Remove( from );

					from.SendLocalizedMessage( 500850 ); // You feel sober.
				}
			}
		}

		private class HeaveTimer : Timer
		{
			private Mobile m_Drunk;

			public HeaveTimer( Mobile drunk )
				: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Drunk = drunk;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if( m_Drunk.Deleted || m_Drunk.Map == Map.Internal )
				{
					Stop();
					m_Table.Remove( m_Drunk );
				}
				else if( m_Drunk.Alive )
				{
					if( m_Drunk.BAC > 60 )
						m_Drunk.BAC = 60;

					// chance to get sober
					if( 10 > Utility.Random( 100 ) )
						--m_Drunk.BAC;

					// lose some stats
					m_Drunk.Stam -= 1;
					m_Drunk.Mana -= 1;

					if( Utility.Random( 1, 4 ) == 1 )
					{
						if( !m_Drunk.Mounted )
						{
							// turn in a random direction
							m_Drunk.Direction = (Direction)Utility.Random( 8 );

							// heave
							m_Drunk.Animate( 32, 5, 1, true, false, 0 );
						}

						// *hic*
						m_Drunk.PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, 500849 );
						m_Drunk.RevealingAction();
					}

					if( m_Drunk.BAC <= 0 )
					{
						Stop();
						m_Table.Remove( m_Drunk );

						m_Drunk.SendLocalizedMessage( 500850 ); // You feel sober.
					}
				}
			}
		}
		#endregion

		public virtual void PourOut_OnTarget( Mobile from, object target )
		{
			if( IsEmpty || !Pourable || !ValidateUse( from, false ) )
				return;

			if( from == target )
			{
				if( from.Thirst < 20 )
					from.Thirst += 1;

				int bac = 0;

				switch( this.Content )
				{
					case BeverageGroupType.Ale: bac = 4; break;
					case BeverageGroupType.Wine: bac = 5; break;
					case BeverageGroupType.Cider: bac = 6; break;
					case BeverageGroupType.Liquor: bac = 7; break;
				}

				from.BAC += bac;

				if( from.BAC > 60 )
					from.BAC = 60;

				CheckHeaveTimer( from );

				from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );

				if( m_Poison != null )
					from.ApplyPoison( m_Poisoner, m_Poison );

				--Quantity;

				CalculateItemID();
			}
		}

		public virtual int CalculateItemID()
		{
			int perc = (m_Quantity * 100) / MaxQuantity;

			if( perc <= 0 )
			{
				switch( this.Content )
				{
					case BeverageGroupType.Ale: return 0x99F;
					case BeverageGroupType.Wine: return 0x9C7;
					case BeverageGroupType.Cider: return 0x9C8;
					case BeverageGroupType.Liquor: return 0x99B;
				}
			}
			else if( perc <= 33 )
			{
				switch( this.Content )
				{
					case BeverageGroupType.Ale: return 0x9A0;
					case BeverageGroupType.Wine: return 0x9C6;
					case BeverageGroupType.Cider: return 0x98E;
					case BeverageGroupType.Liquor: return 0x99C;
				}
			}
			else if( perc <= 66 )
			{
				switch( this.Content )
				{
					case BeverageGroupType.Ale: return 0x9A1;
					case BeverageGroupType.Wine: return 0x9C5;
					case BeverageGroupType.Cider: return 0x98D;
					case BeverageGroupType.Liquor: return 0x99D;
				}
			}
			else
			{
				switch( this.Content )
				{
					case BeverageGroupType.Ale: return 0x9A2;
					case BeverageGroupType.Wine: return 0x9C4;
					case BeverageGroupType.Cider: return 0x98D;
					case BeverageGroupType.Liquor: return 0x99E;
				}
			}

			return 0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsEmpty )
			{
				if( !ValidateUse( from, true ) )
					return;

				Delete();
			}
			else if( Pourable && ValidateUse( from, true ) )
			{
				from.BeginTarget( -1, true, TargetFlags.None, new TargetCallback( PourOut_OnTarget ) );
				from.SendLocalizedMessage( 1010086 ); //What do you want to use this on?
			}
		}

		public BaseBeverageBottleGroup()
		{
			ItemID = ComputeItemID();
			//ItemID = CalculateItemID();
		}

		public BaseBeverageBottleGroup( BeverageGroupType type )
		{
			m_Content = type;
			m_Quantity = MaxQuantity;
			ItemID = ComputeItemID();
			//ItemID = CalculateItemID();
		}

		public BaseBeverageBottleGroup( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 ); // version

			writer.Write( (Mobile)m_Poisoner );

			Poison.Serialize( m_Poison, writer );
			writer.Write( (int)m_Content );
			writer.Write( (int)m_Quantity );
		}

		protected bool CheckType( string name )
		{
			return (World.LoadingType == String.Format( "Server.Items.{0}", name ));
		}

		public override void Deserialize( GenericReader reader )
		{
			InternalDeserialize( reader, true );
		}

		public void InternalDeserialize( GenericReader reader, bool read )
		{
			base.Deserialize( reader );

			if( !read )
				return;

			CalculateItemID();

			int version = reader.ReadInt();

			switch( version )
			{
				case 1:
					{
						m_Poisoner = reader.ReadMobile();
						goto case 0;
					}
				case 0:
					{
						m_Poison = Poison.Deserialize( reader );
						m_Content = (BeverageGroupType)reader.ReadInt();
						m_Quantity = reader.ReadInt();
						break;
					}
			}
		}
	}

	public enum BeverageGroupType
	{
		Ale,
		Wine,
		Cider,
		Liquor
	}

	public interface IHasGroupQuantity
	{
		int Quantity { get; set; }
	}

	[TypeAlias( "Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine" )]
	public class BottleGroup : BaseBeverageBottleGroup
	{
		public override int BaseLabelNumber { get { return 1042959; } } //A bottle of ale
		public override int MaxQuantity { get { return 5; } }
		public override bool Fillable { get { return true; } }

		public override int ComputeItemID()
		{
			return CalculateItemID();
		}

		[Constructable]
		public BottleGroup( BeverageGroupType type )
			: base( type )
		{
			Weight = 1.0;
		}

		public BottleGroup( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						if( CheckType( "BottleAle" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageGroupType.Ale;
						}
						else if( CheckType( "BottleLiquor" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageGroupType.Liquor;
						}
						else if( CheckType( "BottleWine" ) )
						{
							Quantity = MaxQuantity;
							Content = BeverageGroupType.Wine;
						}
						else
						{
							throw new Exception( World.LoadingType );
						}

						break;
					}
			}
		}
	}
}
