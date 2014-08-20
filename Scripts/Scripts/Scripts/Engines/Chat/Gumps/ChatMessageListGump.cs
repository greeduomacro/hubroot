using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Chat
{
	public class ChatMessageListGump : Gump
	{
		private Mobile _client;
		private byte _currentPage;
		private List<ChatMessage> _messages;

		public ChatMessageListGump( Mobile client ) : this( client, 0 ) { }

		public ChatMessageListGump( Mobile client, byte page )
			: base( 10, 10 )
		{
			_client = client;
			_currentPage = Math.Max( page, (byte)1 );

			List<ChatMessage> fullMessageList = ChatMessageManager.GetMessages( client );
			int rangeIdx = ((_currentPage * 10) - 10);
			int count = Math.Min( (fullMessageList.Count - rangeIdx), 10 );

			List<ChatMessage> messages = fullMessageList.GetRange( rangeIdx, count );
			_messages = messages;

			int backSize = ((messages.Count > 0 ? 55 : 50) + (21 * messages.Count));

			AddPage( 1 );
			AddBackground( 0, 0, 400, backSize, 9250 );

			AddAlphaRegion( 15, 15, 370, 20 );
			AddHtml( 15, 15, 370, 20, String.Format( "<basefont color='white'><center>Message List - Unread: {0}</center></basefont>", fullMessageList.Count ), false, false );

			if( page > 1 )
				AddButton( 347, 18, 9706, 9607, ChatManager.GetButtonId( 0, 0 ), GumpButtonType.Reply, 0 );

			if( fullMessageList.Count > (rangeIdx + 10) )
				AddButton( 365, 18, 9702, 9703, ChatManager.GetButtonId( 0, 1 ), GumpButtonType.Reply, 0 );

			for( int i = 0, y = 40; i < messages.Count; i++, y += 22 )
			{
				AddAlphaRegion( 15, y, 370, 20 );
				AddButton( 15, y, 4011, 4013, ChatManager.GetButtonId( 1, i ), GumpButtonType.Reply, 0 );
				AddLabelCropped( 50, y, 155, 20, ChatManager.GetGumpNameColor( _client, messages[i].Client ), messages[i].Client.RawName );
				AddLabelCropped( 210, y, 184, 20, ChatManager.GumpLabelHue, ChatMessageManager.FormatTimestamp( messages[i] ) );
			}
		}

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int val = (info.ButtonID - 1);

			if( val < 0 )
				return;

			int type = (val % 10);
			int index = (val / 10);

			switch( type )
			{
				case 0:
					{
						switch( index )
						{
							case 0:
								{
									_client.SendGump( new ChatMessageListGump( _client, (byte)(_currentPage - 1) ) );
									break;
								}
							case 1:
								{
									_client.SendGump( new ChatMessageListGump( _client, (byte)(_currentPage + 1) ) );
									break;
								}
						}

						break;
					}
				case 1: //read message at index
					{
						ChatMessageManager.RemoveMessage( _client, _messages[index] );
						_client.SendGump( new ChatReadMessageGump( _messages[index].Client, _client, _messages[index] ) );
						break;
					}
			}
		}
		#endregion
	}
}