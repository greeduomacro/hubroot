using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class PortingCrystal : Item
	{
		private Point3D m_Point;
		private Map m_Map;

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Destination { get { return m_Point; } set { m_Point = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Map DestMap { get { return m_Map; } set { m_Map = value; } }

		[Constructable]
		public PortingCrystal()
			: base( 0x1F19 )
		{
			Name = "an enchanted crystal";
			Hue = 205;
			LootType = LootType.Blessed;
			Weight = 10.0;
		}

		public override bool DisplayLootType { get { return false; } }

		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else if( m_Point == Point3D.Zero || m_Map == null || m_Map == Map.Internal )
			{
				from.SendMessage( "The crystal glimmers brightly, but nothing seems to happen." );
			}
			else
			{
				from.MoveToWorld( m_Point, m_Map );
				from.PlaySound( 0x1E3 );

				if( from.RawName == "Asisa" )
					from.SendMessage( "Be safe and well, fair one." );
			}
		}

		public PortingCrystal( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Point );
			writer.Write( m_Map );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Point = reader.ReadPoint3D();
			m_Map = reader.ReadMap();
		}
	}
}
