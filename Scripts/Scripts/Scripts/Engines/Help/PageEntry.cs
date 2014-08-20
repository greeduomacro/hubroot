using System;
using System.Collections;
using Server;
using Server.Engines.Help;
using Server.Mobiles;
using Server.Utilities;

namespace Server.Help
{
	public class PageEntry
	{
		private Mobile _sender, _handler;
		private DateTime _timeSent;
		private string _message;
		private PageType _type;
		private Point3D _location;
		private Map _map;
		private SpeechLog _speechLog;

		public Mobile Sender { get { return _sender; } }

		public Mobile Handler
		{
			get { return _handler; }
			set
			{
				if( _handler != value )
				{
					Mobile old = _handler;
					_handler = value;

					PageQueue.OnHandlerChanged( old, value, this );
				}
			}
		}

		public DateTime TimeSent { get { return _timeSent; } }
		public string Message { get { return _message; } }
		public PageType Type { get { return _type; } }

		public Point3D OriginLocation { get { return _location; } }
		public Map OriginMap { get { return _map; } }

		public SpeechLog SpeechLog { get { return _speechLog; } }

		public PageEntry( Mobile sender, string message, PageType type )
		{
			_sender = sender;
			_timeSent = DateTime.Now;
			_message = message;
			_type = type;

			_location = sender.Location;
			_map = sender.Map;

			if( Server.Engines.Help.SpeechLog.Enabled )
			{
				PlayerMobile pm = sender as PlayerMobile;

				if( pm != null && pm.SpeechLog != null )
				{
					_speechLog = pm.SpeechLog;
				}
			}
		}

		public void WriteLine()
		{
			WriteLine( "" );
		}

		public void WriteLine( string toWrite )
		{
			if( HelpEngine.LoggingEnabled )
			{
				DateTime now = DateTime.Now;
				string filename = String.Format( "({0}-{1}-{2}) {3} - {4}.log", now.Day, now.Month, now.Year, _sender.RawName, _handler == null ? "(no handler)" : _handler.RawName );
				LogManager.LogMessage( HelpEngine.LogDir, filename, toWrite );
			}
		}
	}
}