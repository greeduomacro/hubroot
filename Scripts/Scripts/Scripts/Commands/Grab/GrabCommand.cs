using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Commands
{
	public class GrabCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Grab", AccessLevel.Counselor, new CommandEventHandler( Grab_OnCommand ) );
		}

		[Usage( "Grab" )]
		[Description( "Retrieves all of the available resources from the targeted container, provided the container is owned by the user" )]
		private static void Grab_OnCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;

			if( args.Length > 0 )
			{
				m.SendGump( new GrabOptionsGump( m ) );
			}
			else
			{
				m.SendMessage( "Select the container to grab from." );
				m.BeginTarget( 2, false, TargetFlags.None, new TargetCallback( Grab_OnTarget ) );
			}
		}

		private static void Grab_OnTarget( Mobile from, object target )
		{
			if( !from.Alive || !from.CanSee( target ) || !(target is Container) )
				return;

			bool canLoot = false;
			Container cont = (Container)target;

			if( target is Corpse )
			{
				Corpse c = (Corpse)target;

				if( c.Owner == null || c.Killer == null ) //unable to determine cause of death
				{
					canLoot = true;
				}
				else if( c.Owner == from && (c.Killer == null || !c.Killer.Player) ) //your corpse, not killed by a player (PvE death)
				{
					canLoot = true;
				}
				else if( !c.IsCriminalAction( from ) )
				{
					canLoot = true;
				}
				else if( c.Owner is BaseCreature ) //it's a monster corpse: do you have looting rights?
				{
					BaseCreature bc = (BaseCreature)c.Owner;
					List<DamageStore> lootingRights = BaseCreature.GetLootingRights( bc.DamageEntries, bc.HitsMax );
					Mobile master = bc.GetMaster();

					if( master != null && master == from ) //if it's your pet, you always have the right
						canLoot = true;

					for( int i = 0; !canLoot && i < lootingRights.Count; i++ )
					{
						if( lootingRights[i].m_Mobile != from )
							continue;

						canLoot = lootingRights[i].m_HasRight;
					}

					if( !canLoot )
						from.SendMessage( "You do not have the right to grab from that corpse." );
				}
				else
				{
					canLoot = false;
					from.SendMessage( "You cannot grab from that corpse." );
				}
			}
			else
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if( house != null )
				{
					if( house.IsOwner( from ) )
						canLoot = house.Secures.Contains( cont );

					if( !canLoot )
						from.SendMessage( "You can only grab from secure containers in your own house." );
				}
				else if( cont is BaseTreasureChest && !((BaseTreasureChest)cont).Locked )
				{
					canLoot = true;
				}
				else
				{
					canLoot = false;
					from.SendMessage( "You cannot grab from that container." );
				}
			}

			if( canLoot )
				GrabLoot( from, cont );
		}

		private static void GrabLoot( Mobile from, Container cont )
		{
			if( !from.Alive || cont == null )
				return;

			if( cont is Corpse && from == ((Corpse)cont).Owner )
			{
				Corpse corpse = (Corpse)cont;

				if( corpse.Killer == null || corpse.Killer is BaseCreature )
					corpse.Open( from, true );
				else
					corpse.Open( from, false );
			}
			else
			{
				bool fullPack = false;
				List<Item> items = new List<Item>( cont.Items );
				GrabOptions options = Grab.GetOptions( from );

				for( int i = 0; !fullPack && i < items.Count; i++ )
				{
					Item item = items[i];

					if( options.IsLootable( item ) )
					{
						Container dropCont = options.GetPlacementContainer( Grab.ParseType( item ) );

						if( dropCont == null || dropCont.Deleted || !dropCont.IsChildOf( from.Backpack ) )
							dropCont = from.Backpack;

						if( !item.DropToItem( from, dropCont, new Point3D( -1, -1, 0 ) ) )
							fullPack = true;
					}
				}

				if( fullPack )
					from.SendMessage( "You grabbed as many of the items as you could. The rest remain {0}.", (cont is Corpse ? "on the corpse" : "in the container") );
				else
					from.SendMessage( "You retrieve all of the items from the {0}.", (cont is Corpse ? "body" : "container") );

				from.RevealingAction();
			}
		}
	}
}