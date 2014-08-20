using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Utilities
{
	public class NetStateAnnouncement
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
			EventSink.Logout += new LogoutEventHandler( EventSink_Logout );
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m.AccessLevel < AccessLevel.Administrator )
				BroadcastMessage( AccessLevel.GameMaster, 0x5D, String.Format( "{0} has entered the realm.", m.RawName ) );

			Console.WriteLine( "Login: {0}: Logged in with character \'{1}\'", m.NetState, m.RawName );

            ((Player)m).AdjustBody();
		}

		private static void EventSink_Logout( LogoutEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m.AccessLevel < AccessLevel.Administrator )
				BroadcastMessage( AccessLevel.GameMaster, 0x1BB, String.Format( "{0} has left the realm.", m.RawName ) );
		}

		public static void BroadcastMessage( AccessLevel ac, int hue, string message )
		{
			foreach( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if( m != null && m.AccessLevel >= ac )
					m.SendMessage( hue, message );
			}
		}
	}
}
