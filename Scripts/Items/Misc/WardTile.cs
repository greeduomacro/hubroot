using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WardTile : Item
	{
		#region Properties
		private bool m_Active, m_Creatures;

		private int m_WardItemID;
		private int m_WardItemHue;
		private string m_WardItemName;

		private string m_Message;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Creatures
		{
			get { return m_Creatures; }
			set { m_Creatures = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int WardItemID
		{
			get { return m_WardItemID; }
			set { m_WardItemID = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int WardItemHue
		{
			get { return m_WardItemHue; }
			set { m_WardItemHue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string WardItemName
		{
			get { return m_WardItemName; }
			set { m_WardItemName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Message
		{
			get { return m_Message; }
			set { m_Message = value; }
		}
		#endregion

		[Constructable]
		public WardTile()
			: base( 0x51D )
		{
			Name = "a ward";
			Visible = false;
			Movable = false;

			m_Active = true;
			m_Creatures = false;

			m_WardItemID = 0;
			m_WardItemHue = 0;
			m_WardItemName = "";

			m_Message = "";
		}

		public WardTile( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile from )
		{
			if( m_Active && ValidateItem( from ) )
			{
				if( !m_Creatures && !from.Player )
					return base.OnMoveOver( from );

				return true;
			}
			else
			{
				if( m_Message != null && m_Message != "" )
					from.SendMessage( m_Message );

				return false;
			}
		}

		private bool ValidateItem( Mobile m )
		{
			if( WardItemID == 0 )
				return false;

			bool hasItem = false;

			List<Item> items = new List<Item>( m.Items );
			for( int i = 0; !hasItem && i < items.Count; i++ )
			{
				if( items[i] != null && !items[i].Deleted )
				{
					if( items[i].ItemID == WardItemID && items[i].Hue == WardItemHue && items[i].Name == WardItemName )
						hasItem = true;
				}
			}

			if( !hasItem && m.Backpack != null )
			{
				List<Item> packItems = new List<Item>( m.Backpack.Items );
				for( int i = 0; !hasItem && i < packItems.Count; i++ )
				{
					if( packItems[i] != null && !packItems[i].Deleted )
					{
						if( packItems[i].ItemID == WardItemID && packItems[i].Hue == WardItemHue && packItems[i].Name == WardItemName )
							hasItem = true;
					}
				}
			}

			return hasItem;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( (bool)m_Active );
			writer.Write( (bool)m_Creatures );

			writer.Write( (int)m_WardItemID );
			writer.Write( (int)m_WardItemHue );
			writer.Write( (string)m_WardItemName );

			writer.Write( (string)m_Message );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Active = reader.ReadBool();
			m_Creatures = reader.ReadBool();

			m_WardItemID = reader.ReadInt();
			m_WardItemHue = reader.ReadInt();
			m_WardItemName = reader.ReadString();

			m_Message = reader.ReadString();
		}
	}
}