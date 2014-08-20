using System;
using Server;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Utilities
{
	public class CombatLogging
	{
		private const bool Enabled = true;
		private static string LogDir = @"Combat\";

		[CallPriority( -5 )]
		public static void Initialize()
		{
			Console.WriteLine( "Logging: Combat recording {0}", (Enabled ? "enabled" : "disabled") );

			if( Enabled )
			{
				EventSink.AggressiveAction += new AggressiveActionEventHandler( EventSink_AggressiveAction );
				EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );

				LogManager.GenerateLogFile( LogDir, LogManager.DefaultFileName, LogManager.DefaultFileHeader );
			}
		}

		public static void EventSink_AggressiveAction( AggressiveActionEventArgs args )
		{
			Mobile aggressor = args.Aggressor;
			Mobile aggressed = args.Aggressed;

			if( aggressor == null || aggressed == null )
				return;

			if( !Server.Misc.AttackMessage.CheckAggressions( aggressor, aggressed ) )
			{
				if( aggressor is PlayerMobile && aggressed is PlayerMobile )
				{
					WriteLine( aggressor, "{0}: {1} {2}", LogManager.Format( aggressor ), "initiated aggressive action against", LogManager.Format( aggressed ) );
				}
			}
		}

		public static void EventSink_PlayerDeath( PlayerDeathEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m == null )
				return;

			if( m.LastKiller != null && m.LastKiller is PlayerMobile )
			{
				if( m.LastKiller.Serial == m.Serial )
					WriteLine( m, "{0}: {1}", LogManager.Format( m ), "may have been killed by poison" );
				else
					WriteLine( m, "{0}: {1} {2}", LogManager.Format( m ), "was killed by", LogManager.Format( m.LastKiller ) );
			}
		}

		public static void WriteLine( Mobile from, string format, params object[] args )
		{
			WriteLine( from, String.Format( format, args ) );
		}

		public static void WriteLine( Mobile from, string text )
		{
			if( !Enabled )
				return;

			LogManager.LogMessage( LogDir, LogManager.DefaultFileName, String.Format( "{0}: {1}: {2}", DateTime.Now, from.NetState, text ) );

			string name = (((Account)from.Account) == null ? from.RawName : ((Account)from.Account).Username);

			LogManager.LogMessage( System.IO.Path.Combine( LogDir, from.AccessLevel.ToString() ), String.Format( "{0}.log", name ),
				String.Format( "{0}: {1}: {2}", DateTime.Now, from.NetState, text ) );
		}
	}
}