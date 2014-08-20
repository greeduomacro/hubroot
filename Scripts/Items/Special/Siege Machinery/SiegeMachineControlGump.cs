using System;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Gumps
{
	public class SiegeMachineControlGump : Gump
	{
		private SiegeMachine m_Tower;

		public SiegeMachineControlGump( SiegeMachine tower )
			: base( 10, 10 )
		{
			m_Tower = tower;

			AddPage( 0 );

			AddImage( 5, 5, 9007 );

			AddButton( 82, 32, 4500, 4500, 1, GumpButtonType.Reply, 0 );
			AddButton( 115, 48, 4501, 4501, 2, GumpButtonType.Reply, 0 );
			AddButton( 128, 81, 4502, 4502, 3, GumpButtonType.Reply, 0 );
			AddButton( 111, 114, 4503, 4503, 4, GumpButtonType.Reply, 0 );
			AddButton( 81, 129, 4504, 4504, 5, GumpButtonType.Reply, 0 );
			AddButton( 46, 114, 4505, 4505, 6, GumpButtonType.Reply, 0 );
			AddButton( 33, 81, 4506, 4506, 7, GumpButtonType.Reply, 0 );
			AddButton( 46, 46, 4507, 4507, 8, GumpButtonType.Reply, 0 );

			AddImage( 66, 66, 1417 );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			Mobile m = sender.Mobile;

			if( m == null || (m_Tower == null || m_Tower.Deleted) )
				return;

			if( info.ButtonID == 0 )
			{
				m.CloseGump( typeof( SiegeMachineControlGump ) );
				return;
			}

			switch( info.ButtonID )
			{
				default:
				case 0:
					{
						m.CloseGump( typeof( SiegeMachineControlGump ) );
					} break;
				case 1:
					{
						m_Tower.Y -= 1;
						m_Tower.X -= 1;
					} break;
				case 2:
					{
						m_Tower.Y -= 1;
					} break;
				case 3:
					{
						m_Tower.Y -= 1;
						m_Tower.X += 1;
					} break;
				case 4:
					{
						m_Tower.X += 1;
					} break;
				case 5:
					{
						m_Tower.Y += 1;
						m_Tower.X += 1;
					} break;
				case 6:
					{
						m_Tower.Y += 1;
					} break;
				case 7:
					{
						m_Tower.Y += 1;
						m_Tower.X -= 1;
					} break;
				case 8:
					{
						m_Tower.X -= 1;
					} break;
			}

			Effects.PlaySound( m_Tower, m_Tower.Map, m_Tower.MoveSound );
			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( StopSound ) );

			m.CloseGump( typeof( SiegeMachineControlGump ) );
			m.SendGump( new SiegeMachineControlGump( m_Tower ) );
		}

		private void StopSound()
		{
			Effects.PlaySound( m_Tower, m_Tower.Map, m_Tower.StopSound );
		}
	}
}