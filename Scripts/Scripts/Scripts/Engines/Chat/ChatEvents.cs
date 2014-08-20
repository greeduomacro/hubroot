using System;
using Server;

namespace Server.Chat
{
	public delegate void MessageSentHandler( MessageSentArgs args );

	#region +class MessageSentArgs : EventArgs
	public class MessageSentArgs : EventArgs
	{
		private ChatMessage _msg;

		public ChatMessage Message { get { return _msg; } }

		public MessageSentArgs( ChatMessage message )
		{
			_msg = message;
		}
	}
	#endregion

	public static class ChatEvents
	{
		public static event MessageSentHandler MessageSent;

		public static void InvokeMessageSent( MessageSentArgs args )
		{
			if( MessageSent != null )
				MessageSent( args );
		}
	}
}