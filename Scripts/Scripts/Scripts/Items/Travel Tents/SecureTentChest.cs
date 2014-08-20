using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Targeting;
using Server.Utilities;

namespace Server.Items
{
	public class SecureTentChest : BaseContainer
	{
		private Mobile m_Owner;
		private List<Mobile> m_Friends;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } }

		public List<Mobile> Friends { get { return m_Friends; } set { m_Friends = value; } }

		public override int DefaultGumpID { get { return 0x49; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override TimeSpan DecayTime { get { return TimeSpan.FromMinutes( 30 ); } }

		public override Rectangle2D Bounds { get { return new Rectangle2D( 18, 105, 144, 73 ); } }

		public SecureTentChest( Mobile owner )
			: base( 0xE43 )
		{
			Movable = false;

			m_Owner = owner;
			m_Friends = new List<Mobile>();
		}

		public SecureTentChest( Serial serial )
			: base( serial )
		{
		}

		public override bool IsDecoContainer { get { return false; } }

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if( m_Owner != null )
				list.Add( "a secure chest" );
		}

		public override void OnSingleClick( Mobile from )
		{
			if( m_Owner != null )
			{
				LabelTo( from, "a secure chest" );

				if( CheckContentDisplay( from ) )
					LabelTo( from, "({0} items, {1} stones)", this.TotalItems, this.TotalWeight );
			}

			base.OnSingleClick( from );
		}

		public override bool IsAccessibleTo( Mobile from )
		{
			if( from == m_Owner || IsFriend( from ) || from.AccessLevel >= AccessLevel.GameMaster )
				return base.IsAccessibleTo( from );
			else
				return false;
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			if( !base.CheckHold( m, item, message, checkItems, plusItems, plusWeight ) )
				return base.CheckHold( m, item, message );
			else if( m == m_Owner || IsFriend( m ) || m.AccessLevel >= AccessLevel.GameMaster )
				return true;
			else
				return false;
		}

		public override void OnDelete()
		{
			List<Item> items = new List<Item>();

			if( m_Owner != null && m_Owner.Backpack != null )
			{
				try
				{
					for( int i = 0; i < Items.Count; i++ )
						items.Add( Items[i] );

					items.ForEach(
						delegate( Item i )
						{
							m_Owner.PlaceInBackpack( i );
						} );
				}
				catch( Exception e )
				{
					ExceptionManager.LogException( "SecureTentChest", e );
				}
			}
		}

		public void AddFriend( Mobile from, Mobile targ )
		{
			if( m_Friends == null || m_Owner == targ )
				return;

			if( !targ.Player )
			{
				from.SendMessage( "That can't be a friend of the tent." );
			}
			else if( m_Friends.Contains( targ ) )
			{
				from.SendLocalizedMessage( 501376 ); //This person is already on your friends list.
			}
			else
			{
				m_Friends.Add( targ );

				targ.Delta( MobileDelta.Noto );
				targ.SendMessage( "You have been granted access to the tent chest." );
			}
		}

		public void RemoveFriend( Mobile from, Mobile targ )
		{
			if( m_Friends == null )
				return;

			if( m_Friends.Contains( targ ) )
			{
				m_Friends.Remove( targ );

				targ.Delta( MobileDelta.Noto );

				from.SendMessage( "Chest access for {0} has been removed.", targ.RawName );
				targ.SendMessage( "Your access to this tent's secure chest has been removed." );
			}
		}

		public bool IsFriend( Mobile m )
		{
			if( m == null || m_Friends == null )
				return false;

			return m_Friends.Contains( m );
		}

		public override void GetContextMenuEntries( Mobile from, System.Collections.Generic.List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if( from == m_Owner )
			{
				list.Add( new ChestAddFriendEntry( from, this ) );
				list.Add( new ChestRemoveFriendEntry( from, this ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( (Mobile)m_Owner );
			writer.WriteMobileList( m_Friends );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = (Mobile)reader.ReadMobile();
			m_Friends = reader.ReadStrongMobileList();
		}
	}

	public class ChestAddFriendEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private SecureTentChest m_Chest;

		public ChestAddFriendEntry( Mobile from, SecureTentChest chest )
			: base( 6110, 5 )
		{
			m_From = from;
			m_Chest = chest;
		}

		public override void OnClick()
		{
			m_From.Target = new TentChestFriendTarget( true, m_Chest );
		}
	}

	public class ChestRemoveFriendEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private SecureTentChest m_Chest;

		public ChestRemoveFriendEntry( Mobile from, SecureTentChest chest )
			: base( 6099, 5 )
		{
			m_From = from;
			m_Chest = chest;
		}

		public override void OnClick()
		{
			m_From.Target = new TentChestFriendTarget( false, m_Chest );
		}
	}

	public class TentChestFriendTarget : Target
	{
		private bool m_toAdd;
		private SecureTentChest m_Chest;

		public TentChestFriendTarget( bool add, SecureTentChest chest )
			: base( 12, false, TargetFlags.None )
		{
			CheckLOS = false;

			m_Chest = chest;
			m_toAdd = add;
		}

		protected override void OnTarget( Mobile from, object target )
		{
			if( !from.Alive || m_Chest.Deleted )
				return;

			if( target is Mobile )
			{
				if( m_toAdd )
					m_Chest.AddFriend( from, (Mobile)target );
				else
					m_Chest.RemoveFriend( from, (Mobile)target );
			}
			else
			{
				from.SendMessage( "You cannot grant access to that." );
			}
		}
	}
}