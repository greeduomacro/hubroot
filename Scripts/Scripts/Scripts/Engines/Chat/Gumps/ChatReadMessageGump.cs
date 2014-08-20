using System;
using Server;
using Server.Guilds;
using Server.Gumps;
using Server.Network;

namespace Server.Chat
{
	public class ChatReadMessageGump : Gump
	{
		private Mobile _sender;
		private ChatMessage _message;
		private Mobile _recipient;

		public ChatReadMessageGump( Mobile sender, Mobile recipient, ChatMessage message )
			: base( 10, 10 )
		{
			_sender = sender;
			_message = message;
			_recipient = recipient;

			AddPage( 1 );
			AddBackground( 0, 0, 400, 200, 9250 );

			AddAlphaRegion( 15, 15, 368, 20 );
			AddHtml( 15, 15, 368, 20, String.Format( "<basefont color='white'><center>Message from {0}</center></basefont>", sender.RawName ), false, false );

			AddAlphaRegion( 15, 40, 368, 145 );
			AddLabelCropped( 20, 40, 175, 20, ChatManager.GumpLabelHue, String.Format( "Name: {0}", sender.RawName ) );
			AddLabelCropped( 20, 60, 175, 20, ChatManager.GumpLabelHue, String.Format( "Guild: {0}", (sender.Guild == null ? "N/A" : sender.Guild.Name) ) );

			ChatInfo info = ChatManager.GetInfo( recipient );
			bool isBuddy = info.BuddyList.Contains( sender );
			bool isIgnore = info.IgnoreList.Contains( sender );

			AddButton( 200, 40, (isBuddy ? 4020 : 4008), (isBuddy ? 4022 : 4010), 2, GumpButtonType.Reply, 0 );
			AddLabel( 235, 40, ChatManager.GumpLabelHue, String.Format( "{0} buddy list", (isBuddy ? "Remove from" : "Add to") ) );

			AddButton( 200, 60, (isIgnore ? 4020 : 4002), (isIgnore ? 4022 : 4004), 3, GumpButtonType.Reply, 0 );
			AddLabel( 235, 60, ChatManager.GumpLabelHue, String.Format( "{0} ignore list", (isIgnore ? "Remove from" : "Add to") ) );

			AddHtml( 20, 82, 357, 80, message.Message, false, true );

			AddButton( 15, 165, 4014, 4016, 1, GumpButtonType.Reply, 0 );
			AddLabel( 50, 167, ChatManager.GumpLabelHue, "Reply" );
		}

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			ChatInfo chatInfo = ChatManager.GetInfo( _recipient );

			switch( info.ButtonID )
			{
				case 1: //reply to message
					{
						_recipient.SendGump( new ChatMessageGump( _recipient, _sender, false ) );

						break;
					}
				case 2: //adjust buddy list
					{
						if( chatInfo.BuddyList.Contains( _sender ) )
						{
							chatInfo.Remove( ChatInfo.UpdateType.Buddy, _sender );
							_recipient.SendMessage( "You have removed {0} from your buddy list.", _sender.RawName );
						}
						else
						{
							chatInfo.Add( ChatInfo.UpdateType.Buddy, _sender );
							_recipient.SendMessage( "You have added {0} to your buddy list.", _sender.RawName );
						}

						Resend();

						break;
					}
				case 3: //adjust ignore list
					{
						if( chatInfo.IgnoreList.Contains( _recipient ) )
						{
							chatInfo.Remove( ChatInfo.UpdateType.Ignore, _sender );
							_recipient.SendMessage( "You have removed {0} from your ignore list.", _sender.RawName );
						}
						else
						{
							chatInfo.Add( ChatInfo.UpdateType.Ignore, _sender );
							_recipient.SendMessage( "You have added {0} to your ignore list.", _sender.RawName );
						}

						Resend();

						break;
					}
			}
		}
		#endregion

		#region -void Resend()
		private void Resend()
		{
			_recipient.SendGump( new ChatReadMessageGump( _sender, _recipient, _message ) );
		}
		#endregion
	}
}