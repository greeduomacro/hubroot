using System;
using Server;

namespace Server.Gumps {
	public delegate void WarningGump1Callback( Mobile from, bool okay, object state );

	public class WarningGump1 : Gump {
		private WarningGump1Callback m_Callback;
		private object m_State;

		public WarningGump1( int header, int headerColor, object content, int contentColor, int width, int height, WarningGump1Callback callback, object state )
			: base( (640 - width) / 2, (480 - height) / 2 ) {
			m_Callback = callback;
			m_State = state;

			Closable = false;

			AddPage( 0 );

			AddBackground( 0, 0, width, height, 5054 );

			AddImageTiled( 10, 10, width - 20, 20, 2624 );
			AddAlphaRegion( 10, 10, width - 20, 20 );
			AddHtmlLocalized( 10, 10, width - 20, 20, header, headerColor, false, false );

			AddImageTiled( 10, 40, width - 20, height - 80, 2624 );
			AddAlphaRegion( 10, 40, width - 20, height - 80 );

			if( content is int )
				AddLabel( 10, 40, 33, "You are about to place a new house." );
			AddLabel( 10, 60, 33, "You will not be able to place another house or " );
			AddLabel( 10, 80, 33, "have one transferred to you for one (1) real-life week." );
			AddLabel( 10, 100, 33, "Once you accept these terms," );
			AddLabel( 10, 120, 33, " these effects cannot be reversed." );
			AddLabel( 10, 140, 33, "If you are absolutely certain you wish to proceed, " );
			AddLabel( 10, 160, 33, "click the button next to OKAY below." );
			AddLabel( 10, 180, 33, "If you do not wish to trade for this house, click CANCEL." );

			AddImageTiled( 10, height - 30, width - 20, 20, 2624 );
			AddAlphaRegion( 10, height - 30, width - 20, 20 );

			AddButton( 10, height - 30, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40, height - 30, 170, 20, 1011036, 32767, false, false ); // OKAY

			AddButton( 10 + ((width - 20) / 2), height - 30, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40 + ((width - 20) / 2), height - 30, 170, 20, 1011012, 32767, false, false ); // CANCEL
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info ) {
			if( info.ButtonID == 1 && m_Callback != null )
				m_Callback( sender.Mobile, true, m_State );
			else if( m_Callback != null )
				m_Callback( sender.Mobile, false, m_State );
		}
	}
}