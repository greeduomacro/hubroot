using Server.Regions;

namespace Server.Items
{
	public class TentValidator : Item
	{
		private Mobile m_Owner;
		private TentAddon m_Tent;
		private TentBedroll m_Bedroll;
		private SecureTentChest m_Chest;
		private TravelTentRegion m_Region;
		private Rectangle2D m_Bounds;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Rectangle2D TravelTentArea { get { return m_Bounds; } }

		[Constructable]
		public TentValidator( Mobile owner, TentAddon tent, TentBedroll bedroll, SecureTentChest chest, TravelTentRegion region, Rectangle2D bounds )
			: base( 0x12B3 )
		{
			Name = "travel tent validator";
			Movable = false;
			Visible = false;

			m_Owner = owner;
			m_Tent = tent;
			m_Bedroll = bedroll;
			m_Chest = chest;
			m_Region = region;
			m_Bounds = bounds;
		}

		public override void OnDelete()
		{
			if( m_Tent != null )
				m_Tent.Delete();

			if( m_Chest != null )
				m_Chest.Delete();

			if( m_Region != null )
			{
				m_Region.Unregister();
				m_Region = null;
			}
		}

		public TentValidator( Serial serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			#region v1
			writer.Write( m_Owner );
			writer.Write( m_Tent );
			writer.Write( m_Bedroll );
			writer.Write( m_Chest );
			writer.Write( m_Bounds );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version >= 1 )
			{
				m_Owner = reader.ReadMobile();
				m_Tent = reader.ReadItem() as TentAddon;
				m_Bedroll = reader.ReadItem() as TentBedroll;
				m_Chest = reader.ReadItem() as SecureTentChest;
				m_Bounds = reader.ReadRect2D();

				if( m_Owner != null && m_Chest != null && m_Tent != null )
					m_Region = new TravelTentRegion( m_Owner, m_Chest, m_Tent.Map, m_Bounds, 16 );
			}
		}
	}
}
