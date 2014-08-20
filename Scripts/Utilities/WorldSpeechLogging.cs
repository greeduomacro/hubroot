using System;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;

namespace Server.Utilities.Logging
{
	public class SpeechLogging
	{
		private const bool Enabled = true;
		private static bool ConsoleEnabled = false;
		private static string LogDir = @"Speech\";

		[CallPriority( -5 )]
		public static void Initialize()
		{
			Console.WriteLine( "Logging: Speech logging {0}", (Enabled ? "enabled" : "disabled") );

			if( Enabled )
			{
				EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
				CommandSystem.Register( "ConsoleListen", AccessLevel.Administrator, new CommandEventHandler( ConsoleListen_OnCommand ) );
				LogManager.GenerateLogFile( LogDir, LogManager.DefaultFileName, LogManager.DefaultFileHeader );
			}
		}

		public static void EventSink_Speech( SpeechEventArgs e )
		{
			WriteLine( e.Mobile, "{0}: {1}", LogManager.Format( e.Mobile ), e.Speech );

			if( ConsoleEnabled )
				Console.WriteLine( e.Mobile.Name + String.Format( " ({0}): ", ((Account)e.Mobile.Account).Username ) + e.Speech );
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

		[Usage( "ConsoleListen <true | false>" )]
		[Description( "Enables or disables outputting speech to the console." )]
		public static void ConsoleListen_OnCommand( CommandEventArgs e )
		{
			if( e.Length == 1 )
			{
				SetConsoleListen( e.GetBoolean( 0 ) );
				e.Mobile.SendMessage( "Console speech output has been {0}.", ConsoleEnabled ? "enabled" : "disabled" );
			}
			else
			{
				e.Mobile.SendMessage( "Format: ConsoleListen <true | false >" );
			}
		}

		public static void SetConsoleListen( bool state )
		{
			ConsoleEnabled = state;
			Console.WriteLine( "Console output for world listen has been {0}.", ConsoleEnabled ? "enabled" : "disabled" );
		}
	}
}
