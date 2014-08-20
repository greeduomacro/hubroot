using System;
using System.Collections.Generic;
using Server;
using Server.Commands;

namespace Server.Commands
{
	public class RepeatSoundCommand
	{
		private static Dictionary<int, Timer> m_Table = new Dictionary<int, Timer>();

		[CommandAttribute( "RepeatSound", AccessLevel.Seer )]
		public static void RepeatSoundCommand_OnCommand( CommandEventArgs args )
		{
			if( args.Length == 2 )
			{
				Timer timer = null;
				bool foundValue = false;
				int soundID = args.GetInt32( 0 );
				double interval = args.GetDouble( 1 );

				if( m_Table.ContainsKey( soundID ) )
					foundValue = m_Table.TryGetValue( soundID, out timer );

				if( foundValue || timer != null )
				{
					if( timer != null )
						timer.Stop();

					if( m_Table.Remove( soundID ) )
						args.Mobile.SendMessage( "RepeatSound process with sound index {0} halted.", soundID );
				}
				else
				{
					timer = new InternalTimer( args.Mobile, soundID, interval );
					timer.Start();

					m_Table.Add( soundID, timer );
				}
			}
			else
			{
				args.Mobile.SendMessage( "Usage: RepeatSound <int soundID> <double intervalDelay>" );
			}
		}

		private class InternalTimer : Timer
		{
			Mobile m_From;
			private int SoundID;

			public InternalTimer( Mobile from, int soundID, double interval )
				: base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( interval ) )
			{
				m_From = from;
				SoundID = soundID;

				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				CommandSystem.Handle( m_From, String.Format( "[Sound {0} true", SoundID ) );
			}
		}
	}
}