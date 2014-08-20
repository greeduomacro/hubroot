using System;
using System.IO;
using Server.Utilities;
using Server.Accounting;

namespace Server.Commands
{
	public class CommandLogging
	{
		public static bool Enabled = true;
		private static string LogDir = @"Commands\";

		[CallPriority( -5 )]
		public static void Initialize()
		{
			Console.WriteLine( "Logging: Command logging {0}", (Enabled ? "enabled" : "disabled") );

			if( Enabled )
			{
				EventSink.Command += new CommandEventHandler( EventSink_Command );

				LogManager.GenerateLogFile( LogDir, LogManager.DefaultFileName, LogManager.DefaultFileHeader );
			}
		}

		public static void AppendPath( ref string path, string toAppend )
		{
			path = Path.Combine( path, toAppend );

			if( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		public static void EventSink_Command( CommandEventArgs e )
		{
			WriteLine( e.Mobile, "{0} {1} used command '{2} {3}'", e.Mobile.AccessLevel, LogManager.Format( e.Mobile ), e.Command, e.ArgString );
		}

		public static void LogChangeProperty( Mobile from, object o, string name, string value )
		{
			WriteLine( from, "{0} {1} set property '{2}' of {3} to '{4}'", from.AccessLevel, LogManager.Format( from ), name, LogManager.Format( o ), value );
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

		public static string Format( object obj )
		{
			return LogManager.Format( obj );
		}
	}
}