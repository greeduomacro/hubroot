using System;
using System.IO;
using Server;

namespace Server.Utilities
{
	public class LogManager
	{
		/// <summary>
		/// Returns the parent logging directory (RunUO\Logs\)
		/// </summary>
		public static string RootLogDir = @"Logs\";

		/// <summary>
		/// Returns the default file name (long date string [i.e. Saturday, January 01, 2000])
		/// </summary>
		public static string DefaultFileName = String.Format( "{0}.log", DateTime.Now.ToLongDateString() );

		/// <summary>
		/// Returns the default file header decoration
		/// </summary>
		public static string DefaultFileHeader = String.Format( "###################################\x0D\x0ALogging started on {0}\x0D\x0A", DateTime.Now );

		public static void Initialize()
		{
			if( !Directory.Exists( RootLogDir ) )
				Directory.CreateDirectory( RootLogDir );
		}

		/// <summary>
		/// Creates a new log file in the specified path
		/// </summary>
		/// <param name="logDirectory">the Logs\ subdirectory of the new log file</param>
		/// <param name="fileName">the name of the logfile</param>
		/// <param name="header">text to be printed at the top of the logfile</param>
		public static void GenerateLogFile( string logDirectory, string fileName, string header )
		{
			if( !Directory.Exists( RootLogDir + logDirectory ) )
				Directory.CreateDirectory( RootLogDir + logDirectory );

			using( StreamWriter writer = new StreamWriter( RootLogDir + Path.Combine( logDirectory, fileName ), true, System.Text.Encoding.UTF8 ) )
			{
				writer.WriteLine( header );
			}
		}

		/// <summary>
		/// Logs a text string to the specified log file.
		/// </summary>
		/// <param name="logDirectory">the Logs\ subdirectory of the log file</param>
		/// <param name="fileName">the name of the logfile</param>
		/// <param name="text">the text to store</param>
		public static void LogMessage( string logDirectory, string fileName, string text )
		{
			if( !Directory.Exists( RootLogDir + logDirectory ) )
				Directory.CreateDirectory( RootLogDir + logDirectory );

			using( StreamWriter writer = new StreamWriter( RootLogDir + Path.Combine( logDirectory, fileName ), true, System.Text.Encoding.UTF8 ) )
			{
				writer.WriteLine( text );
			}
		}

		/// <summary>
		/// Provides a standard object format for message logging
		/// </summary>
		/// <param name="obj">the object to be formatted</param>
		/// <returns>the object parameter as a formatted string</returns>
		public static string Format( object obj )
		{
			if( obj is Mobile )
			{
				Mobile m = obj as Mobile;

				if( m.Account == null )
					return String.Format( "{0} (no account)", m );
				else
					return String.Format( "{0} ('{1}')", m, (m.Account as Server.Accounting.Account).Username );
			}
			else if( obj is Item )
				return String.Format( "0x{0:X} ({1})", (obj as Item).Serial.Value, (obj as Item).GetType().Name );

			return obj.ToString();
		}
	}
}