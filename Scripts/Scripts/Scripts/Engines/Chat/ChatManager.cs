using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Mobiles;
using Server.Network;
using System.Xml;

namespace Server.Chat
{
	public static class ChatManager
	{
		#region Gump label hues
		public static int GumpLabelHue
		{
			get
			{
				int color = 1152;
				Int32.TryParse( _configTable["gumpLabelHue"], out color );

				return color;
			}
		}

		public static int GumpVisibleLabelHue
		{
			get
			{
				int color = GumpLabelHue;
				Int32.TryParse( _configTable["gumpVisibleLabelHue"], out color );

				return color;
			}
		}

		public static int GumpInvisibleLabelHue
		{
			get
			{
				int color = GumpLabelHue;
				Int32.TryParse( _configTable["gumpInvisibleLabelHue"], out color );

				return color;
			}
		}
		#endregion

		private static string ConfigPath = "Data";
		private static string ConfigFileName = "chat.cfg";
		private static string ConfigFile = Path.Combine( ConfigPath, ConfigFileName );
		private static string ConfigRootElement = "chatConfig";
		private static string ConfigChildElement = "chat";
		private static string SavePath = "Saves/Chat";
		private static string SaveFile = Path.Combine( SavePath, "chat.bin" );

		private static Dictionary<string, string> _configTable;
		private static Dictionary<Mobile, ChatInfo> _userListTable;

		#region -static bool CanChat( Mobile, Mobile )
		public static bool CanChat( Mobile client, Mobile recipient )
		{
			ChatInfo clientInfo = GetInfo( client );
			ChatInfo recipInfo = GetInfo( recipient );

			return (!clientInfo.IgnoreList.Contains( recipient ) && !recipInfo.IgnoreList.Contains( client ) && recipInfo.Visible);
		}
		#endregion

		#region +static void Configure()
		public static void Configure()
		{
			_configTable = new Dictionary<string, string>();
			_userListTable = new Dictionary<Mobile, ChatInfo>();

			LoadConfig();

			EventSink.WorldLoad += new WorldLoadEventHandler(
				delegate
				{
					LoadLists();
					SetupConfigWatcher();
				} );
			EventSink.WorldSave += new WorldSaveEventHandler(
				delegate
				{
					SaveLists();
				} );
		}
		#endregion

		#region +static List<Mobile> GatherUsers( byte, ListPage, Mobile )
		public static List<Mobile> GatherUsers( byte page, ListPage pageType, Mobile client )
		{
			bool emptyCatch;

			return GatherUsers( page, pageType, client, out emptyCatch );
		}
		#endregion

		#region +static List<Mobile> GatherUsers( byte, ListPage, Mobile, out bool )
		public static List<Mobile> GatherUsers( byte page, ListPage pageType, Mobile client, out bool moreUsersPending )
		{
			ChatInfo info = GetInfo( client );
			List<Mobile> list;

			switch( pageType )
			{
				default:
				case ListPage.Everyone:
					{
						list = new List<Mobile>();

						NetState.Instances.ForEach(
							delegate( NetState state )
							{
								if( state == client.NetState )
									return;

								Mobile m = state.Mobile;

								if( m != null && m.Account != null )
								{
									ChatInfo userInfo = GetInfo( m );

									if( userInfo.Visible )
										list.Add( m );
								}
							} );
						break;
					}
				case ListPage.Buddy:
				case ListPage.Ignore:
					{
						list = info.GetOnlineList( pageType );
						break;
					}
			}

			int rangeIdx = ((page * 10) - 10);
			list.Sort( ChatListComparer.Instance );

			moreUsersPending = (list.Count > (rangeIdx + 10));

			return list.GetRange( rangeIdx, Math.Min( (list.Count - rangeIdx), 10 ) );
		}
		#endregion

		#region +static int GetButtonId( int, int )
		public static int GetButtonId( int type, int index )
		{
			return (1 + (index * 10) + type);
		}
		#endregion

		#region +static int GetGumpNameColor( Mobile, Mobile )
		public static int GetGumpNameColor( Mobile client, Mobile user )
		{
			int color = GumpLabelHue;
			ChatInfo info = GetInfo( client );

			if( info.BuddyList.Contains( user ) )
				Int32.TryParse( _configTable["buddyColor"], out color );
			else if( info.IgnoreList.Contains( user ) )
				Int32.TryParse( _configTable["ignoreColor"], out color );

			return color;
		}
		#endregion

		#region +static ChatInfo GetInfo( Mobile )
		public static ChatInfo GetInfo( Mobile m )
		{
			ChatInfo info;

			if( !_userListTable.TryGetValue( m, out info ) )
			{
				_userListTable[m] = info = new ChatInfo( m );
				SaveLists();
			}

			return info;
		}
		#endregion

		#region +static void SendMessage( Mobile, Mobile, string )
		public static void SendMessage( Mobile client, Mobile recipient, string message )
		{
			ChatEvents.InvokeMessageSent( new MessageSentArgs( new ChatMessage( client, recipient, message ) ) );
		}
		#endregion

		#region -static void LoadConfig()
		private static void LoadConfig()
		{
			if( !File.Exists( ConfigFile ) )
			{
				Utility.PushColor( ConsoleColor.Red );
				Console.WriteLine( "ChatManager: Error: Config file '{0}' cannot be found. Reverting to default configuration values.", ConfigFile );
				Utility.PopColor();

				_configTable["buddyColor"] = "1154";
				_configTable["ignoreColor"] = "1156";
				_configTable["gumpLabelHue"] = "1152";
				_configTable["gumpVisibleLabelHue"] = "1154";
				_configTable["gumpInvisibleLabelHue"] = "1156";
			}
			else
			{

				XmlDocument doc = new XmlDocument();
				XmlElement root;

				try
				{
					doc.Load( ConfigFile );
					root = doc[ConfigRootElement];
					string key = "";

					foreach( XmlElement node in root.GetElementsByTagName( ConfigChildElement ) )
					{
						key = node.GetAttribute( "key" );
						_configTable[key] = node.GetAttribute( "value" );
					}
				}
				catch { }
			}
		}
		#endregion

		#region -static void LoadLists()
		private static void LoadLists()
		{
			if( !File.Exists( SaveFile ) )
				return;

			using( FileStream stream = new FileStream( SaveFile, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				BinaryFileReader reader = new BinaryFileReader( new BinaryReader( stream ) );

				int count = reader.ReadInt();

				for( int i = 0; i < count; i++ )
				{
					int serial = reader.ReadInt();

					if( serial > -1 )
					{
						Mobile m = World.FindMobile( (Serial)serial );
						ChatInfo info = new ChatInfo( reader );

						if( m != null && !m.Deleted )
						{
							if( info != null && !_userListTable.ContainsKey( m ) )
								_userListTable.Add( m, info );
						}
					}
				}
			}

			ChatMessageManager.DeserializeMessages();
		}
		#endregion

		#region -static void SaveLists()
		private static void SaveLists()
		{
			if( !Directory.Exists( SavePath ) )
				Directory.CreateDirectory( SavePath );

			BinaryFileWriter writer = new BinaryFileWriter( SaveFile, true );

			writer.Write( (int)_userListTable.Count );

			if( _userListTable.Count > 0 )
			{
				foreach( KeyValuePair<Mobile, ChatInfo> kvp in _userListTable )
				{
					if( kvp.Key == null || kvp.Key.Deleted || kvp.Value == null )
					{
						writer.Write( (int)-1 );
					}
					else
					{
						writer.Write( (int)kvp.Key.Serial );
						kvp.Value.Serialize( writer );
					}
				}
			}

			writer.Close();

			ChatMessageManager.SerializeMessages();
		}
		#endregion

		#region -static void SetupConfigWatcher()
		private static void SetupConfigWatcher()
		{
			FileSystemWatcher watcher = new FileSystemWatcher( ConfigPath, ConfigFileName );
			watcher.NotifyFilter = (NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size);

			watcher.Changed += new FileSystemEventHandler( delegate { LoadConfig(); } );
			watcher.Renamed += new RenamedEventHandler( delegate { LoadConfig(); } );

			watcher.EnableRaisingEvents = true;
		}
		#endregion

		#region -class ChatListComparer : IComparer<Mobile>
		private class ChatListComparer : IComparer<Mobile>
		{
			public static readonly IComparer<Mobile> Instance = new ChatListComparer();

			public int Compare( Mobile x, Mobile y )
			{
				if( x == null || y == null )
					return 0;
				else if( x == null )
					return -1;
				else if( y == null )
					return 1;

				if( x.AccessLevel > y.AccessLevel )
					return -1;
				else if( x.AccessLevel < y.AccessLevel )
					return 1;
				else
					return Insensitive.Compare( x.RawName, y.RawName );
			}
		}
		#endregion
	}
}