using System;
using Server;
using Server.Accounting;
using Server.Commands;

namespace Server.Commands
{
	public class CharacterAccessLevelToggle
	{
		public static void Initialize()
		{
			CommandSystem.Register( "CALT", AccessLevel.Player, new CommandEventHandler( CALT_onCommand ) );
		}

		[CommandAttribute( "CALT", AccessLevel.Player )]
		[Usage( "CALT [<level>]" )]
		[Description( "Toggles your character AccessLevel between Player and your account\'s maximum. Sets your AccessLevel to the optional <level> parameter, if given." )]
		public static void CALT_onCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;
			AccessLevel acctLevel = (m.Account == null ? AccessLevel.Player : ((Account)m.Account).AccessLevel);
			AccessLevel newLevel = m.AccessLevel;

			if( acctLevel == AccessLevel.Player )
			{
				m.SendMessage( "This command has no effect on your status." );
				return;
			}

			if( args.Length == 0 )
			{
				newLevel = (m.AccessLevel == AccessLevel.Player ? acctLevel : AccessLevel.Player);
			}
			else if( args.Length == 1 )
			{
				switch( args.GetString( 0 ).ToLower() )
				{
					case "player":
						{
							newLevel = AccessLevel.Player;
							break;
						}
					case "counselor":
						{
							newLevel = AccessLevel.Counselor;
							break;
						}
					case "gm":
					case "gamemaster":
						{
							newLevel = AccessLevel.GameMaster;
							break;
						}
					case "seer":
						{
							newLevel = AccessLevel.Seer;
							break;
						}
					case "admin":
					case "administrator":
						{
							newLevel = AccessLevel.Administrator;
							break;
						}
					default:
						{
							m.SendMessage( "Invalid paramater: {0}", args.GetString( 0 ) );
							break;
						}
				}
			}

			if( newLevel > acctLevel )
				m.SendMessage( "You can only raise your level to '{0}'", acctLevel );
			else if( newLevel == m.AccessLevel )
				m.SendMessage( "Your accesslevel is already set to '{0}'", newLevel );
			else
				m.AccessLevel = newLevel;
		}
	}
}
