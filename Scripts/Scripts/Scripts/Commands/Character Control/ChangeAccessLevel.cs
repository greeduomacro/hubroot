using System;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;

namespace Server.Commands
{
	public class ChangeAccessLevel
	{
		[CommandAttribute( "ChangeAccessLevel", AccessLevel.Administrator )]
		public static void ChangeAccessLevel_OnCommand( CommandEventArgs args )
		{
			if( args.Length == 1 )
			{
				string newLevel = args.GetString( 0 );

				if( newLevel != null )
				{
					args.Mobile.SendMessage( "Select the mobile." );
					args.Mobile.Target = new InternalTarget( newLevel );
				}
				else
					args.Mobile.SendMessage( "Format: ChangeAccessLevel <newLevel>" );
			}
			else
				args.Mobile.SendMessage( "Format: ChangeAccessLevel <newLevel>" );
		}

		private class InternalTarget : Target
		{
			private string newLevel;

			public InternalTarget( string level )
				: base( 14, false, TargetFlags.None )
			{
				newLevel = level;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is Mobile )
				{
					Mobile targ = target as Mobile;
					AccessLevel acc;

					switch( newLevel.ToLower() )
					{
						case "player":
							{
								acc = AccessLevel.Player;
							} break;
						case "counselor":
							{
								acc = AccessLevel.Counselor;
							} break;
						case "gm":
						case "gamemaster":
							{
								acc = AccessLevel.GameMaster;
							} break;
						case "seer":
							{
								acc = AccessLevel.Seer;
							} break;
						case "admin":
						case "administrator":
							{
								acc = AccessLevel.Administrator;
							} break;
						default:
							{
								acc = targ.AccessLevel;
							} break;
					}

					FinishTarget( targ, from, acc );
				}
				else
				{
					from.SendMessage( "This will only work on mobiles." );
					from.Target = new InternalTarget( newLevel );
				}
			}

			private void FinishTarget( Mobile m, Mobile from, AccessLevel level )
			{
				Account acct = m.Account as Account;

				if( acct != null )
					acct.AccessLevel = level;

				m.AccessLevel = level;

				from.SendMessage( String.Format( "The accesslevel for {0} has been changed to {1}.", m.RawName, level.ToString() ) );
			}
		}
	}
}