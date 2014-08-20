using System;
using System.Collections;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0xE42, 0xE43 )]
	public class SecureWoodenChest : BaseContainer
	{
		private Mobile m_Owner;
		private bool m_OthersCanDrop;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool OthersCanDrop { get { return m_OthersCanDrop; } set { m_OthersCanDrop = value; } }

		public override int DefaultGumpID { get { return 0x49; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override TimeSpan DecayTime { get { return TimeSpan.FromDays( 30 ); } }

		public override Rectangle2D Bounds { get { return new Rectangle2D( 18, 105, 144, 73 ); } }

		[Constructable]
		public SecureWoodenChest()
			: base( 0xE42 )
		{
			Name = "a wooden chest";
			Movable = false;
		}

		public SecureWoodenChest( Serial serial )
			: base( serial )
		{
		}

		public override bool IsDecoContainer { get { return false; } }

		public override void OnDoubleClick( Mobile from )
		{
			if( Owner == null )
				Owner = from;

			base.OnDoubleClick( from );
		}

		public override void DisplayTo( Mobile to )
		{
			if( to == m_Owner || to.AccessLevel >= AccessLevel.GameMaster )
			{
				base.DisplayTo( to );
			}
			else
				return;
		}

		public override bool IsAccessibleTo( Mobile from )
		{
			if( from == m_Owner || from.AccessLevel >= AccessLevel.GameMaster )
				return true;
			else if( OthersCanDrop )
				return true;

			return false;
		}

		public override bool CheckHold( Mobile from, Item item, bool msg, bool checkItems, int plusItems, int plusWeight )
		{
			return ((from == m_Owner || from.AccessLevel >= AccessLevel.GameMaster || OthersCanDrop)
				&& base.CheckHold( from, item, msg, checkItems, plusItems, plusWeight ));
		}

		public override void OnDelete()
		{
			ArrayList toMove = new ArrayList();

			foreach( Item i in this.Items )
			{
				toMove.Add( i );
			}

			if( m_Owner != null && m_Owner.Backpack != null )
			{
				foreach( Item item in toMove )
				{
					m_Owner.PlaceInBackpack( item );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			writer.Write( (Mobile)m_Owner );
			writer.Write( (bool)m_OthersCanDrop );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();

			if( version >= 1 )
				m_OthersCanDrop = reader.ReadBool();
		}
	}
}