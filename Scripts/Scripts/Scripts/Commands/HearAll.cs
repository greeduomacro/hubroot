using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Misc
{
	public class HearAll
	{
		private static ArrayList m_HearAll = new ArrayList();

		public static void Initialize()
		{
			CommandSystem.Register( "HearAll", AccessLevel.GameMaster, new CommandEventHandler( HearAll_OnCommand ) );

			EventSink.Speech += new SpeechEventHandler( OnSpeech );
		}

		private static void OnSpeech( SpeechEventArgs args )
		{
			string msg;

			if( args.Mobile == null )
				return;

			if( args.Mobile.Region != null && (args.Mobile.Region.Name != null && args.Mobile.Region.Name.Length > 0) )
				msg = String.Format( "{0} ({1}): {2}", args.Mobile.RawName, args.Mobile.Region.Name, args.Speech );
			else
				msg = String.Format( "{0}: {1}", args.Mobile.RawName, args.Speech );

			ArrayList rem = null;

			for( int i = 0; i < m_HearAll.Count; i++ )
			{
				if( m_HearAll[i] is Mobile )
				{
					Mobile m = (Mobile)m_HearAll[i];

					if( m.NetState == null )
					{
						if( rem == null )
							rem = new ArrayList( 1 );

						rem.Add( m );
					}
					else
					{
						if( m.InRange( args.Mobile.Location, 14 ) )
							continue;

						m.SendMessage( msg );
					}
				}
			}

			if( rem != null )
			{
				for( int i = 0; i < rem.Count; i++ )
					m_HearAll.Remove( rem[i] );
			}
		}

		[Usage( "HearAll" )]
		[Description( "Toggles listening to global player chat." )]
		private static void HearAll_OnCommand( CommandEventArgs args )
		{
			if( m_HearAll.Contains( args.Mobile ) )
			{
				m_HearAll.Remove( args.Mobile );
				args.Mobile.SendMessage( "\'Hear all\' disabled." );
			}
			else
			{
				m_HearAll.Add( args.Mobile );
				args.Mobile.SendMessage( "\'Hear all\' enabled. Type [HearAll again to disable it." );
			}
		}

		public static void RemoveMobile( Mobile m )
		{
			if( m_HearAll != null && m_HearAll.Contains( m ) )
			{
				m_HearAll.Remove( m );
			}
		}

		public static void ClearList()
		{
			if( m_HearAll != null )
				m_HearAll.Clear();
		}
	}
}
