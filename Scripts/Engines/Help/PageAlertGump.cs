using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Help
{
	public class PageAlertGump : Gump
	{
		private static List<Mobile> _list = new List<Mobile>();

		public PageAlertGump()
			: base( 0, 0 )
		{
			AddPage( 0 );
			AddBackground( 20, 20, 305, 80, 9250 );

			AddLabel( 45, 35, 0, String.Format( "There {0} currently {1} page{2} in the queue.", PageQueue.List.Count > 1 ? "are" : "is", PageQueue.List.Count.ToString(), PageQueue.List.Count > 1 ? "s" : "" ) );

			AddButton( 155, 60, 4023, 4025, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if( info.ButtonID == 1 )
			{
				state.Mobile.SendGump( new PageQueueGump() );
			}
		}

		public static void SendToStaff()
		{
			CloseAll();

			for( int i = 0; i < NetState.Instances.Count; i++ )
			{
				Mobile m = NetState.Instances[i].Mobile;

				if( m != null && m.AccessLevel >= AccessLevel.Counselor )
				{
					if( m.AutoPageNotify && !PageQueue.IsHandling( m ) )
					{
						m.SendGump( new PageAlertGump() );
						m.SendMessage( "A new page has been placed in the queue." );

						if( !_list.Contains( m ) )
							_list.Add( m );
					}
				}
			}
		}

		public static void CloseAll()
		{
			if( _list.Count <= 0 )
				return;

			for( int i = 0; i < _list.Count; i++ )
			{
				Mobile m = _list[i];

				if( m != null && m.NetState != null )
					m.CloseGump( typeof( PageAlertGump ) );
			}

			_list.Clear();
		}
	}
}