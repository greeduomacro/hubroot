using System;
using Server;
using Server.Commands;
using Server.Network;

namespace Server.Chat
{
	public sealed class ChatCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Msg", AccessLevel.Player, new CommandEventHandler( msg_invoke ) );
			CommandSystem.Register( "ViewMessages", AccessLevel.Player, new CommandEventHandler( viewMessages_invoke ) );
		}

		[Usage( "Msg [<player name>]" )]
		[Description( "Displays the chat list for the calling user. If <player name> is given and is found, opens a new message window to that player." )]
		private static void msg_invoke( CommandEventArgs args )
		{
			if( args.Length == 1 && args.GetString( 0 ) != null )
			{
				string name = args.GetString( 0 );
				bool found = false;

				for( int i = 0; !found && i < NetState.Instances.Count; i++ )
				{
					if( NetState.Instances[i].Mobile != null && NetState.Instances[i].Mobile.RawName.ToLower() == name.ToLower() )
					{
						if( ChatManager.CanChat( args.Mobile, NetState.Instances[i].Mobile ) )
						{
							args.Mobile.SendGump( new ChatMessageGump( args.Mobile, NetState.Instances[i].Mobile ) );
							found = true;
						}
					}
				}

				if( !found )
					args.Mobile.SendGump( new ChatListGump( ListPage.Everyone, args.Mobile ) );
			}
			else
			{
				args.Mobile.SendGump( new ChatListGump( ListPage.Everyone, args.Mobile ) );
			}
		}

		[Usage( "ViewMessages" )]
		[Description( "Displays a list of any unread messages" )]
		private static void viewMessages_invoke( CommandEventArgs args )
		{
			args.Mobile.SendGump( new ChatMessageListGump( args.Mobile ) );
		}
	}
}