using System;
using Server;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0x14F0, 0x14EF )]
	public class DecorationArmorDeed : Item
	{
		private int m_WestID;
		private int m_NorthID;

		[CommandProperty( AccessLevel.GameMaster )]
		public int WestID { get { return m_WestID; } set { m_WestID = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int NorthID { get { return m_NorthID; } set { m_NorthID = value; } }

		[Constructable]
		public DecorationArmorDeed( int westID, int northID )
			: base( 0x14F0 )
		{
			m_WestID = westID;
			m_NorthID = northID;
		}

		public DecorationArmorDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
			writer.Write( (int)m_WestID );
			writer.Write( (int)m_NorthID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_WestID = reader.ReadInt();
			m_NorthID = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if( house != null && house.IsCoOwner( from ) )
				{
					bool northWall = BaseAddon.IsWall( from.X, from.Y - 1, from.Z, from.Map );
					bool westWall = BaseAddon.IsWall( from.X - 1, from.Y, from.Z, from.Map );

					if( northWall && westWall )
					{
						switch( from.Direction & Direction.Mask )
						{
							case Direction.North:
							case Direction.South: northWall = true; westWall = false; break;

							case Direction.East:
							case Direction.West: northWall = false; westWall = true; break;

							default: from.SendMessage( "Turn to face the wall on which to hang the trophy." ); return;
						}
					}

					int itemID = 0;

					if( northWall )
						itemID = m_NorthID;
					else if( westWall )
						itemID = m_WestID;
					else
						from.SendMessage( "This must be placed next to a wall." );

					if( itemID > 0 )
					{
						house.Addons.Add( new DecorationArmorAddon( from, itemID, m_WestID, m_NorthID ) );
						Delete();
					}
				}
				else
					from.SendLocalizedMessage( 502092 ); //You must be in your house to do this.
			}
			else
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
		}
	}

	public class DecorationArmorAddon : Item, IAddon
	{
		private int m_WestID;
		private int m_NorthID;

		[CommandProperty( AccessLevel.GameMaster )]
		public int WestID { get { return m_WestID; } set { m_WestID = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int NorthID { get { return m_NorthID; } set { m_NorthID = value; } }

		[Constructable]
		public DecorationArmorAddon( Mobile from, int itemID, int westID, int northID )
			: base( itemID )
		{
			m_WestID = westID;
			m_NorthID = northID;

			Movable = true;

			MoveToWorld( from.Location, from.Map );
		}

		public DecorationArmorAddon( Serial serial )
			: base( serial )
		{
		}

		public bool CouldFit( IPoint3D p, Map map )
		{
			if( !map.CanFit( p.X, p.Y, p.Z, this.ItemData.Height ) )
				return false;

			if( this.ItemID == m_NorthID )
				return BaseAddon.IsWall( p.X, p.Y - 1, p.Z, map ); //North wall
			else
				return BaseAddon.IsWall( p.X - 1, p.Y, p.Z, map ); //West wall
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
			writer.Write( (int)m_WestID );
			writer.Write( (int)m_NorthID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_WestID = reader.ReadInt();
			m_NorthID = reader.ReadInt();
		}

		public Item Deed
		{
			get { return new DecorationArmorDeed( m_WestID, m_NorthID ); }
		}

		public override void OnDoubleClick( Mobile from )
		{
			BaseHouse house = BaseHouse.FindHouseAt( from );

			if( house != null && house.IsCoOwner( from ) )
			{
				if( from.InRange( GetWorldLocation(), 1 ) )
				{
					from.AddToBackpack( this.Deed );
					Delete();
				}
				else
					from.SendLocalizedMessage( 500295 ); //You are too far away to do that.
			}
		}
	}
}
