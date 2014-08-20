using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Help
{
	public class PageEntryGump : Gump
	{
		private PageEntry _entry;
		private Mobile _mobile;

		private static int[] m_AccessLevelHues = new int[]
			{
				0,
				3,		//counselor
				38,		//gamemaster
				467,	//seer
				1535,	//administrator
			};

		public PageEntryGump( Mobile m, PageEntry entry )
			: base( 0, 0 )
		{
			_mobile = m;
			_entry = entry;

			int buttons = 0;
			int bottom = 366;

			AddPage( 0 );

			AddImageTiled( 10, 10, 410, 456, 0xA40 );
			AddAlphaRegion( 11, 11, 408, 454 );

			AddPage( 1 );

			AddLabel( 28, 28, 2100, "Sent:" );
			AddLabelCropped( 138, 28, 264, 20, 2100, entry.TimeSent.ToString() );

			AddLabel( 28, 48, 2100, "Sender:" );
			AddLabelCropped( 138, 48, 264, 20, 2100, String.Format( "{0} {1} [{2}]", entry.Sender.RawName, entry.Sender.Location, entry.Sender.Map ) );

			AddButton( 28, bottom - (buttons * 22), 0xFAB, 0xFAD, 8, GumpButtonType.Reply, 0 );
			AddImageTiled( 62, bottom - (buttons * 22) + 1, 340, 80, 0xA40 );
			AddImageTiled( 63, bottom - (buttons * 22) + 2, 338, 78, 0xBBC );
			AddTextEntry( 65, bottom - (buttons++ * 22) + 2, 336, 78, 0x480, 0, "" );

			if( entry.Sender != m )
			{
				AddButton( 28, bottom - (buttons * 22), 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddLabel( 62, bottom - (buttons++ * 22), 2100, "Go to Sender" );
			}

			AddLabel( 28, 68, 2100, "Handler:" );

			if( entry.Handler == null )
			{
				AddLabelCropped( 138, 68, 264, 20, 2100, "Unhandled" );

				AddButton( 28, bottom - (buttons * 22), 0xFB1, 0xFB3, 5, GumpButtonType.Reply, 0 );
				AddLabel( 62, bottom - (buttons++ * 22), 2100, "Delete Page" );

				AddButton( 28, bottom - (buttons * 22), 0xFB7, 0xFB9, 4, GumpButtonType.Reply, 0 );
				AddLabel( 62, bottom - (buttons++ * 22), 2100, "Handle Page" );
			}
			else
			{
				try
				{
					AddLabelCropped( 138, 68, 264, 20, m_AccessLevelHues[(int)entry.Handler.AccessLevel], entry.Handler.RawName );
				}
				catch( Exception e )
				{
					Server.Utilities.ExceptionManager.LogException( "HelpEngine.cs", e );
				}

				if( entry.Handler != m )
				{
					AddButton( 28, bottom - (buttons * 22), 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
					AddLabel( 62, bottom - (buttons++ * 22), 2100, "Go to Handler" );
				}
				else
				{
					AddButton( 28, bottom - (buttons * 22), 0xFA2, 0xFA4, 6, GumpButtonType.Reply, 0 );
					AddLabel( 62, bottom - (buttons++ * 22), 2100, "Abandon Page" );

					AddButton( 28, bottom - (buttons * 22), 0xFB7, 0xFB9, 7, GumpButtonType.Reply, 0 );
					AddLabel( 62, bottom - (buttons++ * 22), 2100, "Page Handled" );
				}
			}

			AddLabel( 28, 88, 2100, "Page Location:" );
			AddLabelCropped( 138, 88, 264, 20, 2100, String.Format( "{0} [{1}]", entry.OriginLocation, entry.OriginMap ) );

			AddButton( 28, bottom - (buttons * 22), 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0 );
			AddLabel( 62, bottom - (buttons++ * 22), 2100, "Go to Page Location" );

			if( entry.SpeechLog != null )
			{
				AddButton( 28, bottom - (buttons * 22), 0xFA5, 0xFA7, 9, GumpButtonType.Reply, 0 );
				AddLabel( 62, bottom - (buttons++ * 22), 2100, "View Speech Log" );
			}

			AddLabel( 28, 108, 2100, "Page Type:" );
			AddLabelCropped( 138, 108, 264, 20, 2100, PageQueue.GetPageTypeName( entry.Type ) );

			AddLabel( 28, 128, 2100, "Message:" );
			AddHtml( 138, 128, 250, 100, entry.Message, true, true );
		}

		public void Resend( NetState state )
		{
			PageEntryGump g = new PageEntryGump( _mobile, _entry );

			g.SendTo( state );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;

			switch( info.ButtonID )
			{
				default:
				case 0:
					{
						if( _entry.Handler != state.Mobile )
						{
							PageQueueGump g = new PageQueueGump();

							g.SendTo( state );
						}

						break;
					}
				case 1: //go to sender
					{
						if( _entry.Sender.Deleted )
						{
							m.SendMessage( "That character no longer exists." );
						}
						else if( _entry.Sender.Map == null || _entry.Sender.Map == Map.Internal )
						{
							m.SendMessage( "That character is not in this world." );
						}
						else
						{
							m.MoveToWorld( _entry.Sender.Location, _entry.Sender.Map );

							m.SendMessage( "You have been teleported to that page's sender." );

							Resend( state );
						}

						break;
					}
				case 2: //go to handler
					{
						Mobile h = _entry.Handler;

						if( h != null )
						{
							if( h.Deleted )
							{
								m.SendMessage( "That character no longer exists." );
							}
							else if( h.Map == null || h.Map == Map.Internal )
							{
								m.SendMessage( "That character is not in this world." );
							}
							else
							{
								m.MoveToWorld( h.Location, h.Map );

								m.SendMessage( "You have been teleported to that page's handler." );

								Resend( state );
							}
						}

						break;
					}
				case 3: //go to page location
					{
						if( _entry.OriginMap == null || _entry.OriginMap == Map.Internal )
						{
							m.SendMessage( "That location is not in this world." );
						}
						else
						{
							m.MoveToWorld( _entry.OriginLocation, _entry.OriginMap );

							m.SendMessage( "You have been teleported to the original page location." );

							Resend( state );
						}

						break;
					}
				case 4: //handle page
					{
						if( _entry.Handler == null )
						{
							_entry.Handler = m;

							m.SendMessage( "You are now handling this page." );

							ChatGump.SendTo( _entry.Sender, _entry.Sender, m, false, _entry );
							ChatGump.SendTo( m, _entry.Sender, m, true, _entry );

							_entry.WriteLine( "##############################" );
							_entry.WriteLine( String.Format( "--------Page Type: {0}--------", PageQueue.GetPageTypeName( _entry.Type ) ) );
							_entry.WriteLine( String.Format( "--- Description: {0} ---", _entry.Message ) );
							_entry.WriteLine( "##############################" );
							_entry.WriteLine();
						}
						else
							m.SendMessage( "{0} is already handling this page.", _entry.Handler.RawName );

						break;
					}
				case 5: //delete page
					{
						if( _entry.Handler == null )
						{
							PageQueue.Remove( _entry );

							m.SendMessage( "You delete the page." );

							PageQueueGump g = new PageQueueGump();
							g.SendTo( state );
						}
						else
						{
							m.SendMessage( "Someone is handling that page, and it cannot be deleted." );

							Resend( state );
						}

						break;
					}
				case 6: //abandon page
					{
						if( _entry.Handler == state.Mobile )
						{
							m.SendMessage( "You have abandoned the page." );

							ChatGump.Close( _entry.Sender );
							ChatGump.Close( m );

							_entry.Handler = null;
						}
						else
							m.SendMessage( "You are not handling that page." );

						Resend( state );

						break;
					}
				case 7: //page handled
					{
						if( _entry.Handler == state.Mobile )
						{
							PageQueue.Remove( _entry );

							if( _entry.Sender.HasGump( typeof( ChatGump ) ) )
							{
								m.SendMessage( 0x35, String.Format( "{0} still has the chat gump open.", _entry.Sender.RawName ) );

								Resend( state );
							}

							_entry.Handler = null;

							m.SendMessage( "You mark the page as handled, and remove it from the queue." );

							PageQueueGump g = new PageQueueGump();
							g.SendTo( state );
						}
						else
						{
							m.SendMessage( "You are not handling that page." );

							Resend( state );
						}

						break;
					}
				case 8: //send message
					{
						TextRelay text = info.GetTextEntry( 0 );

						if( text != null )
						{
							_entry.Sender.SendGump( new MessageSentGump( _entry.Sender, m, text.Text ) );
						}

						Resend( state );

						break;
					}
				case 9: //view speech log
					{
						if( _entry.SpeechLog != null )
						{
							m.SendGump( new Server.Engines.Help.SpeechLogGump( _entry.Sender, _entry.SpeechLog ) );
						}

						Resend( state );

						break;
					}
			}
		}
	}
}