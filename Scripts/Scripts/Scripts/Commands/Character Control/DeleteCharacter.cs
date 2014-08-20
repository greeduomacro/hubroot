using System;
using System.Collections;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
	public class DeleteCharacter
	{
		public static void Initialize()
		{
			CommandSystem.Register( "DeleteCharacter", AccessLevel.Administrator, new CommandEventHandler( DeleteCharacter_OnCommand ) );
		}

		[Usage( "DeleteCharacter" )]
		[Description( "Deletes a targeted character." )]
		public static void DeleteCharacter_OnCommand( CommandEventArgs arg )
		{
			arg.Mobile.SendMessage( "Which character would you like to delete?" );
			arg.Mobile.Target = new DeleteCharTarget();
		}
	}

	public class DeleteCharTarget : Target
	{
		public DeleteCharTarget()
			: base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( targeted is PlayerMobile )
			{
				Mobile mob = (Mobile)targeted;
				Account mobAccount = (Account)mob.Account;
				from.SendGump( new WarningGump( 1060635, 30720, String.Format( "You are about to delete the character <u>{0}</u> on account <u>{1}</u>. Any items on this character's person or bankbox will be removed, as will any pets in their ownership. If this character owns a house, this will also be removed from the game.<br>Are you sure you wish to continue?", mob.Name, mobAccount.Username ), 0xFFC000, 320, 240, new WarningGumpCallback( DeleteChar_WarningGumpCallback ), targeted ) );
			}
			else
			{
				from.SendMessage( "You can only remove player characters using this command. Try {0}remove instead.", Server.Commands.CommandSystem.Prefix );
			}
		}
		public static void DeleteChar_WarningGumpCallback( Mobile from, bool okay, object state )
		{
			Mobile mob = (Mobile)state;
			NetState ns = mob.NetState;
			ArrayList pets = new ArrayList();
			int petCount = 0;

			if( !okay )
				return;
			CommandLogging.WriteLine( from, "{0} {1} deleting character {2}.", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );

			foreach( Mobile m in World.Mobiles.Values )
			{
				if( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;
					if( (bc.Controlled && bc.ControlMaster == mob) || (bc.Summoned && bc.SummonMaster == mob) )
					{
						petCount++;
						pets.Add( bc );
					}
				}
			}

			for( int i = 0; i < pets.Count; ++i )
			{
				Mobile pet = (Mobile)pets[i];
				pet.Delete();
			}

			from.SendMessage( "{0} pet{1} deleted.", petCount, petCount != 1 ? "s" : "" );
			//mob.Say( "I've been deleted!" );

			if( mob.NetState != null )
				mob.NetState.Dispose();
			mob.Delete();
			from.SendMessage( "This character has been disposed of thoroughly." );
		}
	}
}