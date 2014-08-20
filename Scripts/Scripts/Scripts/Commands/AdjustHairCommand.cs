using System;
using Server;

namespace Server.Commands
{
	public class AdjustHair
	{
		[CommandAttribute( "AdjustHair", AccessLevel.Player )]
		public static void AdjustHair_OnCommand( Server.Commands.CommandEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m.Alive )
				m.SendGump( new Server.Gumps.AdjustHairGump( m ) );
			else
				m.SendMessage( "You have no hair to adjust in your current state." );
		}
	}
}
