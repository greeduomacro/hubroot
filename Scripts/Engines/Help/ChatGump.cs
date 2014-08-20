using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Utilities;

namespace Server.Help
{
	public class ChatGump
	{
		public static void SendTo( Mobile to, Mobile sender, Mobile handler, bool staff, PageEntry entry )
		{
			SendTo( to, sender, handler, staff, entry, true );
		}

		public static void SendTo( Mobile to, Mobile sender, Mobile handler, bool staff, PageEntry entry, bool resendEntryGump )
		{
			to.SendGump( new ChatGumpDisplay( sender, handler, staff, entry ) );

			if( resendEntryGump )
				to.SendGump( new ChatGumpEntry( sender, handler, entry ) );
		}

		public static void Close( Mobile m )
		{
			Close( m, true );
		}

		public static void Close( Mobile m, bool closeAll )
		{
			m.CloseGump( typeof( ChatGumpDisplay ) );

			if( closeAll )
				m.CloseGump( typeof( ChatGumpEntry ) );
		}

		public static string GetFileName( Mobile sender, Mobile handler )
		{
			return String.Format( Path.Combine( LogManager.RootLogDir + HelpEngine.LogDir, String.Format( "{0} - {1}.ChatGump_text.txt", sender.RawName, handler.RawName ) ) );
		}

		public static string FormatAccessLevel( Mobile m )
		{
			string level = "";

			switch( m.AccessLevel )
			{
				default: break;
				case AccessLevel.Counselor: level = "Cnslr"; break;
				case AccessLevel.GameMaster: level = "GM"; break;
				case AccessLevel.Seer: level = "Seer"; break;
				case AccessLevel.Administrator: level = "Admin"; break;
			}

			return level;
		}

		#region ChatGumpDisplay
		internal class ChatGumpDisplay : Gump
		{
			private Mobile _sender;
			private Mobile _handler;
			private PageEntry _entry;

			public ChatGumpDisplay( Mobile sender, Mobile handler, bool staff, PageEntry entry )
				: base( 0, 0 )
			{
				Closable = true;

				_sender = sender;
				_handler = handler;
				_entry = entry;

				AddPage( 1 );
				AddBackground( 10, 10, 360, 245, 9250 );
				AddLabelCropped( 25, 20, 320, 25, 1152, String.Format( "Page Assistance Chat - {0} {1} : {2}", FormatAccessLevel( handler ), handler.RawName, sender.RawName ) );

				AddHtml( 25, 40, 330, 200, LoadFile( GetFileName( sender, handler ) ), false, true );

				if( staff )
					AddButton( 10, 242, 3, 4, 1, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
			{
				if( info.ButtonID == 1 )
				{
					ChatGump.Close( _sender );
					ChatGump.Close( _handler );

					if( File.Exists( GetFileName( _sender, _handler ) ) )
						File.Delete( GetFileName( _sender, _handler ) );

					_sender.SendMessage( "{0} has determined that your problem is resolved, and has removed your page from the queue. If this is not the case, please file a separate page with more information.", _handler.RawName );
					_handler.SendMessage( "You have closed the page, and deleted it from the queue." );

					_sender.SendGump( new RateHandlingGump( _sender, _handler, _entry ) );

					PageQueue.Remove( _entry );
					_entry = null;
				}
			}

			protected string LoadFile( string filename )
			{
				string contents = "Welcome to Live Chat. You may begin chatting at any time, and this display window will automatically refresh with your text, as well as your assisting staff member\'s text.\n\n";

				if( File.Exists( filename ) )
				{
					string[] fileContents = File.ReadAllLines( filename );

					for( int i = (fileContents.Length - 1); i >= 0; i-- )
					{
						contents += fileContents[i] + "\n";
					}
				}

				return Utility.FixHtml( contents );
			}
		}
		#endregion

		#region ChatGumpEntry
		internal class ChatGumpEntry : Gump
		{
			private Mobile _sender;
			private Mobile _handler;
			private PageEntry _entry;

			public ChatGumpEntry( Mobile sender, Mobile handler, PageEntry entry )
				: base( 0, 0 )
			{
				Closable = false;

				_sender = sender;
				_handler = handler;
				_entry = entry;

				AddPage( 1 );
				AddBackground( 10, 260, 360, 200, 5170 );
				AddLabel( 30, 262, 0, "Message:" );

				AddTextEntry( 35, 285, 310, 140, 0, 100, "" );

				AddLabel( 296, 435, 0, "Send" );
				AddButton( 335, 436, 4033, 4033, 1, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
			{
				if( _entry != null && ((sender == null || _handler == null) || (_sender.NetState == null || _handler.NetState == null)) )
					return;
				else if( sender == null || sender.Mobile == null )
					return;

				if( info.ButtonID == 1 )
				{
					TextRelay tr = info.GetTextEntry( 100 );

					if( tr.Text == null || tr.Text.Length <= 0 || tr.Text == "" )
					{
						sender.Mobile.SendMessage( "You must enter a message to send." );
						Resend( sender.Mobile, true );
					}
					else
					{
						WriteToFile( sender.Mobile, tr.Text );

						Resend( _sender, (_sender == sender.Mobile) );
						Resend( _handler, (_handler == sender.Mobile) );
					}
				}
			}

			protected void Resend( Mobile m, bool resendEntryGump )
			{
				ChatGump.Close( m, false );

				if( m == _sender )
					ChatGump.SendTo( m, _sender, _handler, false, _entry, resendEntryGump );
				else
					ChatGump.SendTo( m, _sender, _handler, true, _entry, resendEntryGump );
			}

			protected void WriteToFile( Mobile m, string text )
			{
				string safeText = Utility.FixHtml( String.Format( "{0}{1}: {2}", (m == _handler ? FormatAccessLevel( m ) + " " : ""), m.RawName, text ) );

				using( StreamWriter writer = new StreamWriter( GetFileName( _sender, _handler ), true ) )
				{
					writer.WriteLine( safeText );
				}

				if( _entry != null )
					_entry.WriteLine( String.Format( "({0}) " + safeText, ((Server.Accounting.Account)m.Account).Username ) );
			}
		}
		#endregion
	}
}