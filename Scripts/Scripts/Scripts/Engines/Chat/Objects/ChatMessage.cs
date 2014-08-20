using System;
using Server;

namespace Server.Chat
{
	public class ChatMessage
	{
		private Mobile _client;
		private string _message;
		private Mobile _recipient;
		private DateTime _timestamp;

		public Mobile Client { get { return _client; } }
		public string Message { get { return _message; } }
		public Mobile Recipient { get { return _recipient; } }
		public DateTime Timestamp { get { return _timestamp; } set { _timestamp = value; } }

		public ChatMessage( Mobile client, Mobile recipient, string message )
		{
			_client = client;
			_message = message;
			_recipient = recipient;
			_timestamp = DateTime.Now;
		}

		#region +virtual void Serialize( GenericWriter )
		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( (int)0 );

			//version 0
			writer.Write( _client );
			writer.Write( _message );
			writer.Write( _recipient );
			writer.Write( _timestamp );
		}
		#endregion

		#region +ChatMessage( BinaryFileReader )
		public ChatMessage( BinaryFileReader reader )
		{
			int version = reader.ReadInt();

			switch( version )
			{
				case 1:
				case 0:
					{
						_client = reader.ReadMobile();
						_message = reader.ReadString();
						_recipient = reader.ReadMobile();
						_timestamp = reader.ReadDateTime();

						break;
					}
			}
		}
		#endregion
	}
}