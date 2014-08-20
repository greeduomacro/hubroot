using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class SiegeMachineComponent : Item
	{
		#region Properties
		private SiegeMachine m_Tower;
		private List<Mobile> m_Mobiles;
		private List<Item> m_Items;

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public SiegeMachine Tower { get { return m_Tower; } set { m_Tower = value; } }
		#endregion

		[Constructable]
		public SiegeMachineComponent( int itemID )
			: base( itemID )
		{
			m_Mobiles = new List<Mobile>();
			m_Items = new List<Item>();

			Movable = false;
		}

		public SiegeMachineComponent( Serial serial )
			: base( serial )
		{
		}

		#region Collection access
		public bool MobileListContains( Mobile m )
		{
			if( m_Mobiles == null || m == null )
				return false;

			return m_Mobiles.Contains( m );
		}

		public void AddMobile( Mobile m )
		{
			if( m_Mobiles == null )
				m_Mobiles = new List<Mobile>();

			if( m_Mobiles.Contains( m ) )
				m_Mobiles.Remove( m );

			m_Mobiles.Add( m );
		}

		public void RemoveMobile( Mobile m )
		{
			if( m_Mobiles != null && m_Mobiles.Contains( m ) )
				m_Mobiles.Remove( m );
		}

		public bool ItemListContains( Item i )
		{
			if( m_Items == null || i == null )
				return false;

			return m_Items.Contains( i );
		}

		public override void AddItem( Item i )
		{
			if( m_Items == null )
				m_Items = new List<Item>();

			if( m_Items.Contains( i ) )
				m_Items.Remove( i );

			m_Items.Add( i );
		}

		new public void RemoveItem( Item i )
		{
			if( m_Items != null && m_Items.Contains( i ) )
				m_Items.Remove( i );
		}

		public void ClearLists()
		{
			if( m_Mobiles != null )
				m_Mobiles.Clear();

			if( m_Items != null )
				m_Items.Clear();
		}
		#endregion

		#region Overrides
		public override bool HandlesOnMovement { get { return true; } }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if( from.AccessLevel >= AccessLevel.GameMaster )
				list.Add( new InternalContextMenu( from, this ) );
		}

		public override void OnDelete()
		{
			base.OnDelete();

			ClearLists();

			if( m_Tower != null )
				m_Tower.RemoveTowerPart( this );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( m_Tower != null )
			{
				m_Tower.OnDoubleClick( from );
			}
		}

		public override void OnLocationChange( Point3D oldLocation )
		{
			base.OnLocationChange( oldLocation );

			if( m_Mobiles == null )
			{
				m_Mobiles = new List<Mobile>();
			}

			if( m_Items == null )
			{
				m_Items = new List<Item>();
			}

			for( int i = 0; i < m_Mobiles.Count; i++ )
			{
				if( m_Mobiles[i] != null )
					m_Mobiles[i].MoveToWorld( new Point3D( this.X, this.Y, m_Mobiles[i].Z ), this.Map );
			}

			for( int i = 0; i < m_Items.Count; i++ )
			{
				if( m_Items[i] != null && !m_Items[i].Deleted )
					m_Items[i].MoveToWorld( new Point3D( this.X, this.Y, m_Items[i].Z ), this.Map );
			}
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if( MobileListContains( m ) && (m.X != this.X && m.Y != this.Y) )
			{
				RemoveMobile( m );
			}
		}
		#endregion

		#region Serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)2 );

			writer.Write( m_Tower );

			#region v1
			if( m_Items != null )
			{
				writer.Write( true );
				writer.Write( (int)m_Items.Count );

				for( int i = 0; i < m_Items.Count; i++ )
					writer.Write( m_Items[i] );
			}
			else
			{
				writer.Write( false );
			}
			#endregion

			#region v2
			if( m_Mobiles != null )
			{
				writer.Write( true );
				writer.Write( (int)m_Mobiles.Count );

				for( int i = 0; i < m_Mobiles.Count; i++ )
					writer.Write( m_Mobiles[i] );
			}
			else
			{
				writer.Write( false );
			}
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version >= 0 )
			{
				m_Tower = (SiegeMachine)reader.ReadItem();
			}

			if( version >= 1 )
			{
				bool notNull = reader.ReadBool();

				if( notNull )
				{
					int count = reader.ReadInt();

					m_Items = new List<Item>( count );

					for( int i = 0; i < count; i++ )
					{
						Item item = reader.ReadItem();

						if( item != null )
							m_Items.Add( item );
					}
				}
				else
				{
					m_Items = new List<Item>();
				}
			}

			if( version >= 2 )
			{
				bool notNull = reader.ReadBool();

				if( notNull )
				{
					int count = reader.ReadInt();

					m_Mobiles = new List<Mobile>( count );

					for( int i = 0; i < count; i++ )
					{
						Mobile m = reader.ReadMobile();

						if( m != null )
							m_Mobiles.Add( m );
					}
				}
				else
				{
					m_Mobiles = new List<Mobile>();
				}
			}
		}
		#endregion

		private class InternalContextMenu : ContextMenuEntry
		{
			private Mobile m_From;
			private SiegeMachineComponent m_Component;

			public InternalContextMenu( Mobile from, SiegeMachineComponent smc )
				: base( 175, 12 )
			{
				m_From = from;
				m_Component = smc;
			}

			public override void OnClick()
			{
				if( m_From != null && m_Component != null )
				{
					m_From.SendMessage( "Select the item(s) and/or mobile(s) to insert into or remove from this component\'s cache." );
					m_From.Target = new InternalTarget( m_Component );
				}
			}
		}

		private class InternalTarget : Target
		{
			private SiegeMachineComponent m_Component;

			public InternalTarget( SiegeMachineComponent smc )
				: base( 12, false, TargetFlags.None )
			{
				m_Component = smc;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target == m_Component )
				{
					m_Component.ClearLists();

					from.SendMessage( "The item and mobile caches for this component have been cleared." );
				}
				else if( target is Mobile )
				{
					Mobile m = target as Mobile;

					if( m == null || !m.Alive )
						return;

					if( m_Component.MobileListContains( m ) )
					{
						m_Component.RemoveMobile( m );

						from.SendMessage( "The mobile has been removed from the cache." );
					}
					else
					{
						CheckOtherComponents( m );

						m_Component.AddMobile( m );

						from.SendMessage( "The mobile has been inserted into the cache." );
					}
				}
				else if( target is Item && !(target is SiegeMachine || target is SiegeMachineComponent) )
				{
					Item item = target as Item;

					if( item == null || item.Deleted )
						return;

					if( m_Component.ItemListContains( item ) )
					{
						m_Component.RemoveItem( item );

						from.SendMessage( "The item has been removed from the cache." );
					}
					else
					{
						CheckOtherComponents( item );

						item.Movable = false;
						m_Component.AddItem( item );

						from.SendMessage( "The item has been inserted into the cache." );
					}
				}
				else
				{
					from.SendMessage( "This will only work for other items or mobiles." );
					from.Target = new InternalTarget( m_Component );
				}
			}

			private void CheckOtherComponents( object toCheck )
			{
				if( m_Component == null || m_Component.Tower == null )
					return;

				ICollection<SiegeMachineComponent> coll = m_Component.Tower.GetKeys();

				foreach( SiegeMachineComponent c in coll )
				{
					if( toCheck is Mobile )
					{
						if( c.MobileListContains( (Mobile)toCheck ) )
							c.RemoveMobile( (Mobile)toCheck );
					}
					else if( toCheck is Item )
					{
						if( c.ItemListContains( (Item)toCheck ) )
							c.RemoveItem( (Item)toCheck );
					}
				}
			}
		}
	}
}