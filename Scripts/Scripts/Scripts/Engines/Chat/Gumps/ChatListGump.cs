using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Chat
{
	public enum ListPage : byte
	{
		Everyone = 0x01,
		Buddy = 0x02,
		Ignore = 0x03
	}

	public sealed class ChatListGump : Gump
	{
		private byte _currentPage;
		private ListPage _pageType;
		private Mobile _client;

		public ChatListGump( ListPage pageType, Mobile user ) : this( 1, pageType, user ) { }

		public ChatListGump( byte page, ListPage pageType, Mobile client )
			: base( 10, 10 )
		{
			_currentPage = Math.Max( page, (byte)1 );
			_pageType = pageType;
			_client = client;

			ChatInfo info = ChatManager.GetInfo( client );
			bool hasNextPage = false;
			List<Mobile> userList = ChatManager.GatherUsers( _currentPage, pageType, client, out hasNextPage );
			int backSize = ((userList.Count > 0 ? 55 : 50) + (21 * userList.Count));
			
			uint onlineCount = 0;

			for( int i = 0; i < NetState.Instances.Count; i++ )
			{
				if( NetState.Instances[i] != null && NetState.Instances[i].Running && NetState.Instances[i].Mobile != null )
					onlineCount++;
			}

			AddPage( 1 );
			AddBackground( 0, 0, 265, backSize, 9250 );

			AddAlphaRegion( 15, 15, 234, 20 );
			AddHtml( 15, 15, 234, 20, String.Format( "<basefont color='white'><center>{0} List - Online: {1}</center></basefont>", (pageType == ListPage.Everyone ? "User" : (pageType == ListPage.Buddy ? "Buddy" : "Ignore")), onlineCount ), false, false );

			if( _currentPage > 1 )
				AddButton( 215, 18, 9706, 9707, ChatManager.GetButtonId( 5, 0 ), GumpButtonType.Reply, 0 );

			if( hasNextPage )
				AddButton( 235, 18, 9702, 9703, ChatManager.GetButtonId( 5, 1 ), GumpButtonType.Reply, 0 );

			for( int i = 0, y = 40; i < userList.Count; i++, y += 22 )
			{
				AddAlphaRegion( 15, y, 234, 20 );
				AddLabelCropped( 20, y, 200, 20, ChatManager.GetGumpNameColor( _client, userList[i] ), userList[i].RawName );
				AddButton( 220, y, (info.IgnoreList.Contains( userList[i] ) ? 4002 : 4011), (info.IgnoreList.Contains( userList[i] ) ? 4004 : 4013), ChatManager.GetButtonId( 0, i ), GumpButtonType.Reply, 0 );
			}

			AddButton( 15, (backSize + 3), 4008, 4010, ChatManager.GetButtonId( (int)ListPage.Buddy, 0 ), GumpButtonType.Reply, 0 );
			AddLabel( 50, (backSize + 3), ChatManager.GumpLabelHue, String.Format( "{0} List", (pageType == ListPage.Buddy ? "User" : "Buddy") ) );

			AddButton( 225, (backSize + 3), (info.Visible ? 4017 : 4005), (info.Visible ? 4019 : 4007), ChatManager.GetButtonId( (int)ListPage.Everyone, 1 ), GumpButtonType.Reply, 0 );
			AddLabel( (info.Visible ? 180 : 175), (backSize + 3), (info.Visible ? ChatManager.GumpVisibleLabelHue : ChatManager.GumpInvisibleLabelHue), (info.Visible ? "Visible" : "Invisible") );

			AddButton( 15, (backSize + 28), 4029, 4031, ChatManager.GetButtonId( (int)ListPage.Everyone, 2 ), GumpButtonType.Reply, 0 );
			AddLabel( 50, (backSize + 28), ChatManager.GumpLabelHue, "Guild Message" );

			AddButton( 225, (backSize + 28), 4002, 4004, ChatManager.GetButtonId( (int)ListPage.Ignore, 0 ), GumpButtonType.Reply, 0 );
			AddLabel( 152, (backSize + 28), ChatManager.GumpLabelHue, String.Format( "{0} List", (pageType == ListPage.Ignore ? "User" : "Ignore") ) );
		}

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int val = (info.ButtonID - 1);

			if( val < 0 )
				return;

			int type = (val % 10);
			int index = (val / 10);

			ChatInfo chatInfo = ChatManager.GetInfo( _client );
			List<Mobile> userList = ChatManager.GatherUsers( _currentPage, _pageType, _client );
			
			switch( type )
			{
				case 0: //send message - index represents position in ChatManager.GatherUsers list
					{
						if( ChatManager.CanChat( _client, userList[index] ) )
						{
							_client.SendGump( new ChatMessageGump( _client, userList[index] ) );
						}
						else
						{
							if( chatInfo.IgnoreList.Contains( userList[index] ) )
							{
								if( _pageType == ListPage.Ignore )
								{
									chatInfo.Remove( ChatInfo.UpdateType.Ignore, userList[index] );
									_client.SendMessage( "{0} has been removed from your ignore list.", userList[index].RawName );
								}
								else
								{
									_client.SendMessage( "That user is on your ignore list!" );
								}
							}
							else
							{
								_client.SendMessage( "You cannot chat with that user." );
							}

							Resend();
						}

						break;
					}
				case (int)ListPage.Everyone:
					{
						switch( index )
						{
							case 1: //toggle visibility
								{
									chatInfo.Visible = !chatInfo.Visible;
									Resend();

									break;
								}
							case 2: //send guild message
								{
									if( _client.Guild == null )
									{
										_client.SendMessage( "You are not in a guild!" );
										Resend();
									}
									else
									{
										_client.SendGump( new ChatMessageGump( _client, null, true ) );
									}

									break;
								}
						}

						break;
					}
				case (int)ListPage.Buddy:
					{
						switch( index )
						{
							case 0: //display buddy list
								{
									if( _pageType == ListPage.Buddy )
									{
										_client.SendGump( new ChatListGump( ListPage.Everyone, _client ) );
									}
									else
									{
										if( chatInfo.BuddyList.Count == 0 )
										{
											_client.SendMessage( "You do not have any chat buddies!" );
											Resend();
										}
										else
										{
											_client.SendGump( new ChatListGump( ListPage.Buddy, _client ) );
										}
									}
									
									break;
								}
						}

						break;
					}
				case (int)ListPage.Ignore:
					{
						switch( index )
						{
							case 0: //display ignore list
								{
									if( _pageType == ListPage.Ignore )
									{
										_client.SendGump( new ChatListGump( ListPage.Everyone, _client ) );
									}
									else
									{
										if( chatInfo.IgnoreList.Count == 0 )
										{
											_client.SendMessage( "You do not have anyone on your ignore list!" );
											Resend();
										}
										else
										{
											_client.SendGump( new ChatListGump( ListPage.Ignore, _client ) );
										}
									}
								

									break;
								}
						}

						break;
					}
				case 5:
					{
						switch( index )
						{
							case 0:
								{
									Resend( (byte)(_currentPage - 1) );
									break;
								}
							case 1:
								{
									Resend( (byte)(_currentPage + 1) );
									break;
								}
						}

						break;
					}
			}
		}
		#endregion

		#region -void Resend()
		private void Resend()
		{
			Resend( _currentPage );
		}
		#endregion

		#region -void Resend( byte )
		private void Resend( byte page )
		{
			_client.SendGump( new ChatListGump( page, _pageType, _client ) );
		}
		#endregion
	}
}