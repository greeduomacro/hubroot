using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class WardTeleporter : Item
	{
		#region props
		private bool m_Active, m_Creatures;
		private Point3D m_PointDest;
		private Map m_MapDest;
		private TimeSpan m_Delay;

		private int m_WardItemID;
		private int m_WardItemHue;
		private string m_WardItemName;

		#region StandardTeleporter
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get { return m_Delay; }
			set { m_Delay = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D PointDest
		{
			get { return m_PointDest; }
			set { m_PointDest = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map MapDest
		{
			get { return m_MapDest; }
			set { m_MapDest = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Creatures
		{
			get { return m_Creatures; }
			set { m_Creatures = value; }
		}

		public override int LabelNumber { get { return 1026095; } } // teleporter
		#endregion

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
		#endregion

		#region ctors
		[Constructable]
		public WardTeleporter()
			: this( new Point3D( 0, 0, 0 ), null, false )
		{
		}

		[Constructable]
		public WardTeleporter( Point3D pointDest, Map mapDest )
			: this( pointDest, mapDest, false )
		{
		}

		[Constructable]
		public WardTeleporter( Point3D pointDest, Map mapDest, bool creatures )
			: base( 0x1BC3 )
		{
			Movable = false;
			Visible = false;

			m_Active = true;
			m_PointDest = pointDest;
			m_MapDest = mapDest;
			m_Creatures = creatures;

			m_WardItemID = 0;
			m_WardItemHue = 0;
			m_WardItemName = "";
		}
		#endregion

		public virtual void StartTeleport( Mobile m )
		{
			if( m_Delay == TimeSpan.Zero )
				DoTeleport( m );
			else
				Timer.DelayCall( m_Delay, new TimerStateCallback( DoTeleport_Callback ), m );
		}

		private void DoTeleport_Callback( object state )
		{
			DoTeleport( (Mobile)state );
		}

		public virtual void DoTeleport( Mobile m )
		{
			Map map = m_MapDest;

			if( map == null || map == Map.Internal )
				map = m.Map;

			Point3D p = m_PointDest;

			if( p == Point3D.Zero )
				p = m.Location;

			Server.Mobiles.BaseCreature.TeleportPets( m, p, map );

			m.MoveToWorld( p, map );
		}

		public override bool OnMoveOver( Mobile m )
		{
			if( m_Active && ValidateItem( m ) )
			{
				if( !m_Creatures && !m.Player )
					return base.OnMoveOver( m );

				StartTeleport( m );
				return false;
			}

			return base.OnMoveOver( m );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel >= AccessLevel.GameMaster )
			{
				from.Target = new InternalTarget( this );
				from.SendMessage( "Select the item that has the properties (name, itemID, and hue) that you want to protect this teleporter." );
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

		public WardTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			writer.Write( (TimeSpan)m_Delay );
			writer.Write( (bool)m_Creatures );
			writer.Write( (bool)m_Active );

			writer.Write( m_PointDest );
			writer.Write( m_MapDest );

			//version 1
			writer.Write( (int)m_WardItemID );
			writer.Write( (int)m_WardItemHue );
			writer.Write( (string)m_WardItemName );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Delay = reader.ReadTimeSpan();
			m_Creatures = reader.ReadBool();
			m_Active = reader.ReadBool();

			m_PointDest = reader.ReadPoint3D();
			m_MapDest = reader.ReadMap();

			if( version >= 1 )
			{
				m_WardItemID = reader.ReadInt();
				m_WardItemHue = reader.ReadInt();
				m_WardItemName = reader.ReadString();
			}
		}

		private class InternalTarget : Target
		{
			private WardTeleporter m_Tele;

			public InternalTarget( WardTeleporter tele )
				: base( 12, true, TargetFlags.None )
			{
				m_Tele = tele;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is Item )
				{
					Item i = (Item)target;

					m_Tele.WardItemID = i.ItemID;
					m_Tele.WardItemHue = i.Hue;
					m_Tele.WardItemName = i.Name;

					from.SendMessage( "The properties of the teleporter have been set. Please double-check the teleporter to be sure before final use." );
				}
			}
		}
	}
}