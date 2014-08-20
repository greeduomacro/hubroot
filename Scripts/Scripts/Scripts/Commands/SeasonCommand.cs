using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Network;

namespace Server.Misc
{
	public enum SeasonList
	{
		Spring,
		Summer,
		Fall,
		Winter,
		Desolation
	}

	public class SeasonWeather
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Season", AccessLevel.Administrator, new CommandEventHandler( Season_OnCommand ) );
		}

		[Usage( "Season spring | summer | fall | winter | desolation" )]
		[Description( "Changes seasons." )]
		private static void Season_OnCommand( CommandEventArgs e )
		{
			Mobile m = e.Mobile;

			if( e.Length == 1 )
			{
				string seasonType = e.GetString( 0 ).ToLower();
				SeasonList season;

				try
				{
					season = (SeasonList)Enum.Parse( typeof( SeasonList ), seasonType, true );
				}
				catch
				{
					m.SendMessage( "Usage: Season spring | summer | fall | winter | desolation" );
					return;
				}

				m.SendMessage( "Setting season to: " + seasonType + "." );
				SetSeason( m, season );
			}
			else
				m.SendMessage( "Usage: Season spring | summer | fall | winter | desolation" );
		}

		public static void SetSeason( Mobile m, SeasonList season )
		{
			m.Send( new SeasonChange( (int)season ) );
		}

		public static void SetGlobalSeason( SeasonList season )
		{
			for( int i = 0; i < NetState.Instances.Count; i++ )
			{
				Mobile m = NetState.Instances[i].Mobile;

				if( m != null )
					SetSeason( m, season );
			}
		}
	}
}