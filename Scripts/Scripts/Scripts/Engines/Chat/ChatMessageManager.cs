using System;
using System.Collections.Generic;
using System.IO;
using Server;

namespace Server.Chat
{
	public static class ChatMessageManager
	{
		private static string SavePath = "Saves/Chat";
		private static string SaveFile = Path.Combine( SavePath, "chatMessages.bin" );

		private static Dictionary<Mobile, List<ChatMessage>> _messageTable;

		#region +static void Configure()
		public static void Configure()
		{
			_messageTable = new Dictionary<Mobile, List<ChatMessage>>();

			ChatEvents.MessageSent += new MessageSentHandler(
				delegate( MessageSentArgs args )
				{
					AddPendingMessage( args.Message.Recipient, args.Message );
				} );
		}
		#endregion

		#region +static void AddPendingMessage( Mobile, ChatMessage )
		public static void AddPendingMessage( Mobile m, ChatMessage message )
		{
			if( !_messageTable.ContainsKey( m ) )
				_messageTable.Add( m, new List<ChatMessage>() );

			_messageTable[m].Add( message );

			m.CloseGump( typeof( ChatMessageAlertGump ) );
			m.SendGump( new ChatMessageAlertGump( (byte)_messageTable[m].Count ) );
		}
		#endregion

		#region +static void DisplayMessages( Mobile )
		public static void DisplayMessages( Mobile m )
		{
			m.SendGump( new ChatMessageListGump( m ) );
		}
		#endregion

		#region +static string FormatTimestamp( ChatMessage )
		public static string FormatTimestamp( ChatMessage message )
		{
			return (message.Timestamp.ToString( "MMMM dd, yyyy | HH:mm" ) );
		}
		#endregion

		#region +static List<ChatMessage> GetMessages( Mobile )
		public static List<ChatMessage> GetMessages( Mobile m )
		{
			List<ChatMessage> messages;

			if( !_messageTable.TryGetValue( m, out messages ) )
				_messageTable[m] = messages = new List<ChatMessage>();

			return messages;
		}
		#endregion

		#region +static void RemoveMessage( Mobile, ChatMessage )
		public static void RemoveMessage( Mobile m, ChatMessage message )
		{
			if( !_messageTable.ContainsKey( m ) )
				return;

			if( _messageTable[m].Contains( message ) )
				_messageTable[m].Remove( message );
		}
		#endregion

		#region +static void SerializeMessages( string )
		public static void SerializeMessages()
		{
			if( !Directory.Exists( SavePath ) )
				Directory.CreateDirectory( SavePath );

			BinaryFileWriter writer = new BinaryFileWriter( SaveFile, true );

			writer.Write( (int)_messageTable.Count );

			if( _messageTable.Count > 0 )
			{
				foreach( KeyValuePair<Mobile, List<ChatMessage>> kvp in _messageTable )
				{
					if( kvp.Key == null || kvp.Key.Deleted || kvp.Value == null )
					{
						writer.Write( (int)-1 );
					}
					else
					{
						writer.Write( (int)kvp.Key.Serial );

						writer.Write( (int)kvp.Value.Count );
						kvp.Value.ForEach(
							delegate( ChatMessage message )
							{
								message.Serialize( writer );
							} );
					}
				}
			}

			writer.Close();
		}
		#endregion

		#region +static void DeserializeMessages()
		public static void DeserializeMessages()
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
						Mobile m = World.FindMobile( serial );
						int messages = reader.ReadInt();
						List<ChatMessage> msgList = new List<ChatMessage>( messages );

						for( int j = 0; j < messages; j++ )
							msgList.Add( new ChatMessage( reader ) );

						if( !_messageTable.ContainsKey( m ) )
							_messageTable.Add( m, msgList );
					}
				}
			}
		}
		#endregion
	}
}