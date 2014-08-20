using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
	public class FollowCommand
	{
		private static Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();

		[CommandAttribute( "Follow", AccessLevel.Counselor )]
		public static void FollowCommand_OnCommand( Server.Commands.CommandEventArgs args )
		{
			Mobile m = args.Mobile;
			Timer t = null;
			bool foundValue = false;

			if( m_Table.ContainsKey( m ) )
				foundValue = m_Table.TryGetValue( m, out t );

			if( foundValue || t != null )
			{
				if( t != null )
					t.Stop();

				if( m_Table.Remove( m ) )
					m.SendMessage( "Follow procedure successfully halted." );
			}
			else
			{
				m.SendMessage( "Select the mobile to begin following." );
				m.Target = new InternalTarget( m_Table );
			}
		}

		private class InternalTarget : Target
		{
			private Dictionary<Mobile, Timer> table;

			public InternalTarget( Dictionary<Mobile, Timer> dict )
				: base( 12, false, TargetFlags.None )
			{
				table = dict;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( targeted is Mobile )
				{
					Mobile to = targeted as Mobile;
					Timer t = null;

					t = new InternalTimer( from, to );
					t.Start();

					table.Add( from, t );

					from.SendMessage( "You are now following {0}.", to.RawName );
				}
				else
				{
					from.SendMessage( "This command only works on Mobiles." );
					from.Target = new InternalTarget( table );
				}
			}

			private class InternalTimer : Timer
			{
				private Mobile m_GM;
				private Mobile m_Target;

				public InternalTimer( Mobile gm, Mobile target )
					: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
				{
					m_GM = gm;
					m_Target = target;

					Priority = TimerPriority.OneSecond;
				}

				protected override void OnTick()
				{
					if( m_GM == null || m_Target == null )
					{
						m_Table.Remove( m_GM );

						Stop();
						return;
					}
					else if( m_GM.NetState == null || (m_Target is PlayerMobile && ((PlayerMobile)m_Target).NetState == null) )
					{
						m_Table.Remove( m_GM );

						Stop();
						return;
					}

					if( !m_GM.InRange( m_Target, 5 ) || m_GM.Z != m_Target.Z || m_GM.Map != m_Target.Map )
						m_GM.MoveToWorld( m_Target.Location, m_Target.Map );
				}
			}
		}
	}
}