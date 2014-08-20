using System;
using Server;
using Server.Guilds;
using Server.Gumps;
using Server.Network;

namespace Server.Chat
{
	public class ChatMessageGump : Gump
	{
		private Mobile _client;
		private bool _isGuildMsg;
		private Mobile _recipient;

		public ChatMessageGump( Mobile client, Mobile recipient ) : this( client, recipient, false ) { }

		public ChatMessageGump( Mobile client, Mobile recipient, bool isGuildMessage )
			: base( 10, 10 )
		{
			_client = client;
			_isGuildMsg = isGuildMessage;
			_recipient = recipient;

			AddPage( 1 );
			AddBackground( 0, 0, 400, 200, 9250 );

			AddAlphaRegion( 15, 15, 368, 20 );
			AddHtml( 15, 15, 368, 20, String.Format( "<basefont color='white'><center>Message to {0}</center></basefont>", (isGuildMessage ? "your guild" : recipient.RawName) ), false, false );

			AddAlphaRegion( 15, 40, 368, 145 );
			AddLabelCropped( 20, 40, 175, 20, ChatManager.GumpLabelHue, String.Format( "Name: {0}", (isGuildMessage ? "N/A" : recipient.RawName) ) );
			AddLabelCropped( 20, 60, 175, 20, ChatManager.GumpLabelHue, String.Format( "Guild: {0}", (isGuildMessage ? client.Guild.Name : (recipient.Guild == null ? "N/A" : recipient.Guild.Name)) ) );

			if( !isGuildMessage )
			{
				ChatInfo info = ChatManager.GetInfo( client );
				bool isBuddy = info.BuddyList.Contains( recipient );
				bool isIgnore = info.IgnoreList.Contains( recipient );

				AddButton( 200, 40, (isBuddy ? 4020 : 4008), (isBuddy ? 4022 : 4010), 2, GumpButtonType.Reply, 0 );
				AddLabel( 235, 40, ChatManager.GumpLabelHue, String.Format( "{0} buddy list", (isBuddy ? "Remove from" : "Add to") ) );

				AddButton( 200, 60, (isIgnore ? 4020 : 4002), (isIgnore ? 4022 : 4004), 3, GumpButtonType.Reply, 0 );
				AddLabel( 235, 60, ChatManager.GumpLabelHue, String.Format( "{0} ignore list", (isIgnore ? "Remove from" : "Add to") ) );
			}

			AddTextEntry( 20, 82, 357, 80, 0, 5, "", 255 );

			AddButton( 15, 165, 4029, 4031, 1, GumpButtonType.Reply, 0 );
			AddLabel( 50, 167, ChatManager.GumpLabelHue, "Send" );
		}

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			ChatInfo chatInfo = ChatManager.GetInfo( _client );

			switch( info.ButtonID )
			{
				case 1: //send message
					{
						string msg = Utility.FixHtml( info.GetTextEntry( 5 ).Text );

						if( _isGuildMsg && _client.Guild is Guild )
							((Guild)_client.Guild).GuildChat( _client, msg );
						else
							ChatManager.SendMessage( _client, _recipient, msg );

						break;
					}
				case 2: //adjust buddy list
					{
						if( chatInfo.BuddyList.Contains( _recipient ) )
						{
							chatInfo.Remove( ChatInfo.UpdateType.Buddy, _recipient );
							_client.SendMessage( "You have removed {0} from your buddy list.", _recipient.RawName );
						}
						else
						{
							chatInfo.Add( ChatInfo.UpdateType.Buddy, _recipient );
							_client.SendMessage( "You have added {0} to your buddy list.", _recipient.RawName );
						}

						Resend();

						break;
					}
				case 3: //adjust ignore list
					{
						if( chatInfo.IgnoreList.Contains( _recipient ) )
						{
							chatInfo.Remove( ChatInfo.UpdateType.Ignore, _recipient );
							_client.SendMessage( "You have removed {0} from your ignore list.", _recipient.RawName );
						}
						else
						{
							chatInfo.Add( ChatInfo.UpdateType.Ignore, _recipient );
							_client.SendMessage( "You have added {0} to your ignore list.", _recipient.RawName );
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
			_client.SendGump( new ChatMessageGump( _client, _recipient, _isGuildMsg ) );
		}
		#endregion
	}
}