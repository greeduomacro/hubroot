using System;
using Server;
using Server.Gumps;

namespace Server.Chat
{
	public class ChatMessageAlertGump : Gump
	{
		public ChatMessageAlertGump( byte messages )
			: base( 10, 10 )
		{
			AddPage( 1 );
			AddBackground( 0, 0, 185, 75, 2620 );
			AddImage( 10, 10, 2002 );
			AddLabel( 35, 25, ChatManager.GumpLabelHue, String.Format( "{0} Unread Message{1}", messages, (messages == 1 ? "" : "s") ) );
			AddButton( 160, 48, 2714, 2715, 1, GumpButtonType.Reply, 0 );
		}

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if( info.ButtonID == 1 )
				ChatMessageManager.DisplayMessages( sender.Mobile );
		}
		#endregion
	}
}