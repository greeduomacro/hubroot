using System;
using System.Diagnostics;

namespace Server.Utilities
{
    public class ExceptionManager
    {
        public const string LogCategory = "Application";
        public const string Source = "UO Server";

        public static void Configure()
        {
            Console.WriteLine("Exception Manager: Enabled to log exceptions to the system event log.");
        }

        public static void LogException( string sourceFile, Exception e )
        {
            try
            {
                if( !System.Diagnostics.EventLog.SourceExists(Source) )
                    System.Diagnostics.EventLog.CreateEventSource(Source, LogCategory);

                string eventString = "#############\n";
                eventString += String.Format("[{0}]: Exception caught in {1}:\n\n", sourceFile, e.TargetSite);
                eventString += e.ToString();
                eventString += "\n#############";

                System.Diagnostics.EventLog.WriteEntry(Source, eventString, EventLogEntryType.Warning);
            }
            catch { }
        }
    }
}