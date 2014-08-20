using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Utilities
{
	public class CloseGumpTimer : Timer
	{
		private Mobile m_From;
		private Gump m_Gump;

		public CloseGumpTimer( Mobile from, Gump g, TimeSpan delay )
			: base( delay )
		{
			m_From = from;
			m_Gump = g;

			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			if( m_From.NetState != null )
			{
				IEnumerable<Gump> eable = m_From.NetState.Gumps;
				List<Gump> toRemove = new List<Gump>();

				lock( eable )
				{
					foreach( Gump g in eable )
					{
						if( g != null && g.GetType() == m_Gump.GetType() )
						{
							toRemove.Add( g );
						}
					}
				}

				for( int i = 0; i < toRemove.Count; i++ )
				{
					m_From.CloseGump( m_Gump.GetType() );
					m_From.NetState.RemoveGump( toRemove[i] );
				}

				toRemove.Clear();
			}
		}
	}
}