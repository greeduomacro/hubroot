using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
	public class TentBedroll : Item
	{
		private Mobile m_Owner;
		private TentAddon m_Tent;
		private TentFlap m_Flap;
		private SecureTentChest m_Chest;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } }

		public TentBedroll( Mobile owner, TentAddon tent, TentFlap flap, SecureTentChest chest )
			: base( 0xA55 )
		{
			Name = "a bedroll";
			Movable = false;

			m_Owner = owner;
			m_Tent = tent;
			m_Flap = flap;
			m_Chest = chest;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( m_Owner == from && !from.HasGump( typeof( TentManagementGump ) ) && !from.HasGump( typeof( ConfirmTentPlacementGump ) ) )
			{
				from.SendGump( new TentManagementGump( from, this ) );
			}
			else
			{
				from.SendMessage( "This is not your tent!" );
			}
		}

		public override void OnDelete()
		{
			if( m_Flap != null )
				m_Flap.Delete();

			if( m_Owner != null && m_Owner.Backpack != null )
			{
				for( int i = 0; i < m_Owner.Backpack.Items.Count; i++ )
				{
					Item item = m_Owner.Backpack.Items[i];

					if( item != null && item is TentValidator && ((TentValidator)item).TravelTentArea.Contains( this ) )
						item.Delete();
				}
			}
		}

		public TentBedroll( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( (Mobile)m_Owner );
			writer.Write( m_Tent );
			writer.Write( m_Flap );
			writer.Write( m_Chest );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = (Mobile)reader.ReadMobile();
			m_Tent = (TentAddon)reader.ReadItem();
			m_Flap = (TentFlap)reader.ReadItem();
			m_Chest = (SecureTentChest)reader.ReadItem();
		}
	}
}
