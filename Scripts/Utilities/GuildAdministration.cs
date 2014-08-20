using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Utilities
{
	public class GuildAdminGump : Gump
	{
		public enum Page
		{
			Information,
			Guilds,
			GuildInfo,
			GuildMemberList,
			Alliances,
			AllianceDetails,
			Wars,
			WarDetails
		}

		private const string Version = "1.3.0";

		private Mobile _from;
		private Page _pageType;
		private int _listPage;
		private object _state;

		private List<Guild> _list;
		private Dictionary<AllianceInfo, List<Guild>> _allianceTable;
		private Dictionary<Guild, WarDeclaration> _warTable;

		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;
		private const int DisabledColor = 0x4210;

		private const int LabelColor32 = 0xFFFFFF;
		private const int SelectedColor32 = 0x8080FF;
		private const int DisabledColor32 = 0x808080;

		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;

		public static void Initialize()
		{
			CommandSystem.Register( "GuildAdmin", AccessLevel.Seer, new CommandEventHandler( GuildAdmin_OnCommand ) );
		}

		[Usage( "GuildAdmin" )]
		[Description( "Opens an interface for server administrators to interact with and perform tasks upon the guild system." )]
		public static void GuildAdmin_OnCommand( CommandEventArgs args )
		{
			if( args.Length == 1 )
			{
				string notice = "";

				List<Guild> foundList = SearchForGuild( args.GetString( 0 ), false, out notice );

				if( foundList.Count == 1 )
					args.Mobile.SendGump( new GuildAdminGump( args.Mobile, Page.GuildInfo, 0, "One match found.", null, foundList[0] ) );
				else
					args.Mobile.SendGump( new GuildAdminGump( args.Mobile, Page.Guilds, 0, (notice == null ? (foundList.Count == 0 ? "Nothing matched your search terms." : null) : notice), foundList, null ) );
			}
			else
				args.Mobile.SendGump( new GuildAdminGump( args.Mobile, Page.Information, 0, null, null, null ) );
		}

		#region Collection Population
		private void PopulateGuildList()
		{
			if( _list != null )
				_list.Clear();
			else
				_list = new List<Guild>();

			foreach( KeyValuePair<int, BaseGuild> kvp in Guild.List )
			{
				if( kvp.Value is Guild )
					_list.Add( (Guild)kvp.Value );
			}

			_list.Sort( GuildComparer.Instance );
		}

		private void PopulateAllianceTable()
		{
			if( _allianceTable != null )
				_allianceTable.Clear();
			else
				_allianceTable = new Dictionary<AllianceInfo, List<Guild>>();

			foreach( KeyValuePair<string, AllianceInfo> kvp in AllianceInfo.Alliances )
			{
				if( kvp.Value != null )
				{
					List<Guild> memberList = new List<Guild>();

					foreach( BaseGuild bg in Guild.List.Values )
					{
						Guild g = (Guild)bg;

						if( kvp.Value.IsMember( g ) && !memberList.Contains( g ) )
						{
							memberList.Add( g );
						}
					}

					memberList.Sort( GuildComparer.Instance );
					_allianceTable.Add( kvp.Value, memberList );
				}
			}
		}

		private void PopulateWarTable()
		{
			if( _warTable != null )
				_warTable.Clear();
			else
				_warTable = new Dictionary<Guild, WarDeclaration>();

			foreach( KeyValuePair<int, BaseGuild> kvp in Guild.List )
			{
				if( kvp.Value is Guild && ((Guild)kvp.Value).AcceptedWars != null )
				{
					Guild g = (Guild)kvp.Value;

					for( int i = 0; i < g.AcceptedWars.Count; i++ )
					{
						if( !_warTable.ContainsKey( g ) && !_warTable.ContainsValue( g.AcceptedWars[i] ) )
						{
							if( !_warTable.ContainsKey( g.AcceptedWars[i].Guild ) && !_warTable.ContainsKey( g.AcceptedWars[i].Opponent ) )
								_warTable.Add( g, g.AcceptedWars[i] );
						}
					}
				}
			}
		}
		#endregion

		#region Gump Layout Helpers
		private void AddPageButton( int x, int y, int buttonID, string text, Page page, params Page[] subPages )
		{
			bool isSelection = (_pageType == page);

			for( int i = 0; !isSelection && i < subPages.Length; i++ )
				isSelection = (_pageType == subPages[i]);

			AddSelectedButton( x, y, buttonID, text, isSelection );
		}

		private void AddSelectedButton( int x, int y, int buttonID, string text, bool isSelection )
		{
			AddButton( x, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 200, 20, Color( text, isSelection ? SelectedColor32 : LabelColor32 ), false, false );
		}

		private void AddButtonLabeled( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 240, 20, Color( text, LabelColor32 ), false, false );
		}

		private int GetButtonID( int type, int index )
		{
			return (1 + (index * 10) + type);
		}

		private void AddTextField( int x, int y, int width, int height, int index )
		{
			AddBackground( x - 2, y - 2, width + 4, height + 4, 0x2486 );
			AddTextEntry( x + 2, y + 2, width - 4, height - 4, 0, index, "" );
		}

		private void AddGuildHeader()
		{
			AddTextField( 200, 20, 200, 20, 0 );
			AddButtonLabeled( 200, 50, GetButtonID( 4, 0 ), "Search by Name" );
			AddButtonLabeled( 200, 80, GetButtonID( 4, 1 ), "Search by Abbreviation" );
		}
		#endregion

		private string GetStatus( WarStatus status )
		{
			string str = "";

			switch( status )
			{
				case WarStatus.InProgress: str = "In Progress"; break;
				case WarStatus.Win: str = "Winning / Won"; break;
				case WarStatus.Lose: str = "Losing / Lost"; break;
				case WarStatus.Draw: str = "Draw"; break;
				case WarStatus.Pending: str = "Status Pending"; break;
			}

			return str;
		}

		private string GetRemainingTime( WarDeclaration warDecl )
		{
			TimeSpan timeRemaining = TimeSpan.Zero;

			if( warDecl.WarLength != TimeSpan.Zero && (warDecl.WarBeginning + warDecl.WarLength) > DateTime.Now )
				timeRemaining = (warDecl.WarBeginning + warDecl.WarLength) - DateTime.Now;

			return String.Format( "{0:D2}:{1:mm}", timeRemaining.Hours, DateTime.MinValue + timeRemaining );
		}

		public GuildAdminGump( Mobile from, Page page, int listPage, string notice, List<Guild> list, object state )
			: base( 50, 40 )
		{
			from.CloseGump( typeof( GuildAdminGump ) );

			if( list == null )
				PopulateGuildList();
			else
				_list = list;

			PopulateAllianceTable();
			PopulateWarTable();

			_from = from;
			_pageType = page;
			_listPage = listPage;
			_state = state;

			AddPage( 0 );
			AddBackground( 0, 0, 420, 440, 5054 );

			AddBlackAlpha( 10, 10, 170, 100 );
			AddBlackAlpha( 190, 10, 220, 100 );
			AddBlackAlpha( 10, 120, 400, 260 );
			AddBlackAlpha( 10, 390, 400, 40 );

			if( !Guild.NewGuildSystem )
			{
				AddLabel( 20, 130, LabelHue, "This menu does not support old guild systems." );
				return;
			}

			AddPageButton( 10, 10, GetButtonID( 0, 0 ), "INFORMATION", Page.Information );
			AddPageButton( 10, 30, GetButtonID( 0, 1 ), "GUILD LIST", Page.Guilds, Page.GuildInfo );
			AddPageButton( 10, 50, GetButtonID( 0, 2 ), "ALLIANCES", Page.Alliances, Page.AllianceDetails );

			if( notice != null )
				AddHtml( 12, 392, 396, 36, Color( notice, LabelColor32 ), false, false );

			switch( page )
			{
				case Page.Information:
					{
						AddGuildHeader();

						AddLabel( 20, 130, LabelHue, "GuildAdmin Version:" );
						AddLabel( 150, 130, LabelHue, Version );

						AddLabel( 20, 150, LabelHue, "Registration Fee:" );
						AddLabel( 150, 150, LabelHue, Guild.RegistrationFee.ToString( "#,0" ) + " gp" );

						AddLabel( 20, 170, LabelHue, "Total Guilds:" );
						AddLabel( 150, 170, LabelHue, _list.Count.ToString() );

						AddLabel( 20, 190, LabelHue, " Active Alliances:" );
						AddLabel( 150, 190, LabelHue, _allianceTable.Count.ToString() );

						AddLabel( 20, 210, LabelHue, " Active Wars:" );
						AddLabel( 150, 210, LabelHue, _warTable.Count.ToString() );

						break;
					}
				case Page.Guilds:
					{
						AddGuildHeader();

						if( _list == null )
						{
							AddHtml( 12, 140, 250, 60, Color( "There was a problem building the list of guilds on the server. This page cannot be displayed.", LabelColor32 ), false, false );
							break;
						}

						AddLabelCropped( 12, 120, 81, 20, LabelHue, "Guild ID" );
						AddLabelCropped( 95, 120, 81, 20, LabelHue, "Abbreviaton" );
						AddLabelCropped( 178, 120, 172, 20, LabelHue, "Name" );

						if( listPage > 0 )
							AddButton( 375, 122, 0x15E3, 0x15E7, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 375, 122, 0x25EA );

						if( (listPage + 1) * 12 < _list.Count )
							AddButton( 392, 122, 0x15E1, 0x15E5, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 392, 122, 0x25E6 );

						if( _list.Count == 0 )
							AddLabel( 12, 140, LabelHue, "There are no guilds to display." );

						for( int i = 0, index = (listPage * 12); i < 12 && index >= 0 && index < _list.Count; i++, index++ )
						{
							Guild g = _list[index];

							if( g == null )
								continue;

							int offset = (140 + (i * 20));

							AddLabelCropped( 12, offset, 81, 20, LabelHue, g.Id.ToString() );
							AddLabelCropped( 95, offset, 81, 20, LabelHue, g.Abbreviation );
							AddLabelCropped( 178, offset, 172, 20, LabelHue, g.Name );

							AddButton( 380, offset - 1, 0xFA5, 0xFA7, GetButtonID( 4, index + 2 ), GumpButtonType.Reply, 0 );
						}

						break;
					}
				case Page.GuildInfo:
					{
						Guild g = state as Guild;

						if( g == null )
							break;

						AddGuildHeader();
						AddHtml( 10, 125, 400, 20, Color( Center( "Guild Information" ), LabelColor32 ), false, false );

						int y = 146;

						AddLabel( 20, y, LabelHue, "Name:" );
						AddLabel( 200, y, LabelHue, g.Name );
						y += 20;

						AddLabel( 20, y, LabelHue, "Abbreviation:" );
						AddLabel( 200, y, LabelHue, g.Abbreviation );
						y += 20;

						AddLabel( 20, y, LabelHue, "Guild Leader:" );
						if( g.Leader.Account != null )
							AddLabelCropped( 200, y, 150, 20, LabelHue, String.Format( "{0} [{1}]", g.Leader.RawName,
								((Server.Accounting.Account)g.Leader.Account).Username ) );
						else
							AddLabelCropped( 200, y, 150, 20, LabelHue, g.Leader.RawName );
						y += 20;

						AddLabel( 20, y, LabelHue, "Active Members:" );
						AddLabel( 200, y, LabelHue, g.Members.Count.ToString() );
						y += 44;

						AddButtonLabeled( 20, y, GetButtonID( 7, 0 ), "Disband" );
						AddButtonLabeled( 200, y, GetButtonID( 7, 1 ), "Active Alliance" );
						y += 20;

						AddButtonLabeled( 20, y, GetButtonID( 7, 2 ), "Member List" );
						AddButtonLabeled( 200, y, GetButtonID( 7, 3 ), "Active Wars" );
						y += 20;

						AddButtonLabeled( 20, y, GetButtonID( 7, 4 ), "Add Member" );
						AddButtonLabeled( 200, y, GetButtonID( 7, 5 ), "Guild Properties" );

						break;
					}
				case Page.GuildMemberList:
					{
						Guild g = state as Guild;

						if( g == null )
							break;

						AddGuildHeader();
						AddLabelCropped( 12, 120, 120, 20, LabelHue, "Player Name" );
						AddLabelCropped( 132, 120, 120, 20, LabelHue, "Account Username" );
						AddLabelCropped( 252, 120, 120, 20, LabelHue, "Status" );

						if( listPage > 0 )
							AddButton( 375, 122, 0x15E3, 0x15E7, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 375, 122, 0x25EA );

						if( (listPage + 1) * 12 < g.Members.Count )
							AddButton( 392, 122, 0x15E1, 0x15E5, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 392, 122, 0x25E6 );

						if( g.Members.Count == 0 )
							AddLabel( 12, 140, LabelHue, "This guild has no members." );

						for( int i = 0, index = (listPage * 12); i < 12 && index >= 0 && index < g.Members.Count; i++, index++ )
						{
							Mobile m = g.Members[index];

							if( m == null || m.Account == null )
								continue;

							int offset = (140 + (i * 20));

							AddLabelCropped( 12, offset, 120, 20, LabelHue, m.RawName );
							AddLabelCropped( 132, offset, 120, 20, LabelHue, ((Server.Accounting.Account)m.Account).Username );

							if( m.NetState != null )
								AddLabelCropped( 252, offset, 120, 20, 0x40, "Online" );
							else
								AddLabelCropped( 252, offset, 120, 20, 0x20, "Offline" );
						}

						break;
					}
				case Page.Alliances:
					{
						AddGuildHeader();

						if( _allianceTable == null )
						{
							AddHtml( 12, 140, 250, 60, Color( "There was a problem building the table of alliances. This page cannot be displayed.", LabelColor32 ), false, false );
							break;
						}

						AddLabelCropped( 12, 120, 170, 20, LabelHue, "Name" );
						AddLabelCropped( 184, 120, 81, 20, LabelHue, "Member Count" );
						AddLabelCropped( 291, 120, 61, 20, LabelHue, "Leader" );

						if( listPage > 0 )
							AddButton( 375, 122, 0x15E3, 0x15E7, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 375, 122, 0x25EA );

						if( (listPage + 1) * 12 < _allianceTable.Count )
							AddButton( 392, 122, 0x15E1, 0x15E5, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 392, 122, 0x25E6 );

						if( _allianceTable.Count == 0 )
							AddLabel( 12, 140, LabelHue, "There are no alliances to display." );

						List<AllianceInfo> allianceList = new List<AllianceInfo>( _allianceTable.Keys );

						for( int i = 0, index = (listPage * 12); i < 12 && index >= 0 && index < allianceList.Count; i++, index++ )
						{
							AllianceInfo info = allianceList[index];

							if( info == null )
								continue;

							List<Guild> tempList = null;
							_allianceTable.TryGetValue( info, out tempList );

							int offset = (140 + (i * 20));

							AddLabelCropped( 12, offset, 170, 20, LabelHue, info.Name );
							AddLabelCropped( 204, offset, 61, 20, LabelHue, (tempList == null ? "N/A" : tempList.Count.ToString()) );
							AddLabelCropped( 291, offset, 61, 20, LabelHue, info.Leader.Abbreviation );

							AddButton( 380, (offset - 1), 0xFA5, 0xFA7, GetButtonID( 5, index ), GumpButtonType.Reply, 0 );
						}

						break;
					}
				case Page.AllianceDetails:
					{
						AllianceInfo info = state as AllianceInfo;

						if( info == null || !_allianceTable.ContainsKey( info ) )
							break;

						AddGuildHeader();
						AddHtml( 10, 125, 400, 20, Color( Center( "Alliance Details" ), LabelColor32 ), false, false );

						int y = 146;

						AddLabel( 20, y, LabelHue, "Name:" );
						AddLabel( 200, y, LabelHue, info.Name );
						y += 20;

						AddLabel( 20, y, LabelHue, "Leader:" );
						AddLabelCropped( 200, y, 180, 20, LabelHue, String.Format( "[{0}] {1}", info.Leader.Abbreviation, info.Leader.Name ) );
						y += 20;

						AddLabel( 20, y, LabelHue, "Member Count:" );
						AddLabel( 200, y, LabelHue, _allianceTable[info].Count.ToString() );
						y += 20;

						AddLabel( 20, y, LabelHue, "Status:" );
						AddLabel( 200, y, LabelHue, (_allianceTable[info].Count < 2 ? "Pending Acceptance" : "Active") );
						y += 20;

						y = 270;

						AddButtonLabeled( 20, y, GetButtonID( 8, 0 ), "Disband" );
						AddButtonLabeled( 200, y, GetButtonID( 8, 1 ), "Member List" );

						break;
					}
				case Page.Wars:
					{
						Guild g = state as Guild;

						if( g == null )
							break;

						AddGuildHeader();

						AddLabelCropped( 12, 120, 120, 20, LabelHue, "Guild Name" );
						AddLabelCropped( 134, 120, 120, 20, LabelHue, "Opponent Guild" );
						AddLabelCropped( 246, 120, 90, 20, LabelHue, "Date Started" );

						if( listPage > 0 )
							AddButton( 375, 122, 0x15E3, 0x15E7, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 375, 122, 0x25EA );

						if( (listPage + 1) * 12 < g.AcceptedWars.Count )
							AddButton( 392, 122, 0x15E1, 0x15E5, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0 );
						else
							AddImage( 392, 122, 0x25E6 );

						if( g.AcceptedWars.Count == 0 )
							AddLabel( 12, 140, LabelHue, "This guild has not accepted any war declarations." );

						for( int i = 0, index = (listPage * 12); i < 12 && index >= 0 && index < g.AcceptedWars.Count; i++, index++ )
						{
							WarDeclaration warDecl = g.AcceptedWars[index];

							if( warDecl == null )
								continue;

							int offset = (140 + (i * 20));

							AddLabelCropped( 12, offset, 120, 20, LabelHue, g.Name );
							AddLabelCropped( 134, offset, 120, 20, LabelHue, warDecl.Opponent.Name );
							AddLabelCropped( 246, offset, 120, 20, LabelHue, warDecl.WarBeginning.ToShortDateString() );

							AddButton( 380, (offset - 1), 0xFA5, 0xFA7, GetButtonID( 6, index ), GumpButtonType.Reply, 0 );
						}

						break;
					}
				case Page.WarDetails:
					{
						WarDeclaration warDecl = state as WarDeclaration;

						if( warDecl == null )
							break;

						AddGuildHeader();
						AddHtml( 10, 125, 400, 20, Color( Center( String.Format( "War Details for {0}", warDecl.Guild.Abbreviation ) ), LabelColor32 ), false, false );

						int y = 146;

						AddLabel( 20, y, LabelHue, "Current Status:" );
						AddLabel( 200, y, LabelHue, GetStatus( warDecl.Status ) );
						y += 20;

						AddLabel( 20, y, LabelHue, "Initiated at:" );
						AddLabel( 200, y, LabelHue, warDecl.WarBeginning.ToShortTimeString() + " on " + warDecl.WarBeginning.ToShortDateString() );
						y += 40;

						AddHtml( 10, y, 400, 20, Color( Center( "Conditions of War:" ), LabelColor32 ), false, false );
						y += 30;

						AddLabel( 20, y, LabelHue, "Kills [Current/Max]:" );
						AddLabel( 200, y, LabelHue, String.Format( "{0}/{1}", warDecl.Kills.ToString(), warDecl.MaxKills.ToString() ) );
						y += 20;

						AddLabel( 20, y, LabelHue, "Time [Remaining/Length]:" );
						AddLabel( 200, y, LabelHue, String.Format( "{0}/{1}", GetRemainingTime( warDecl ), String.Format( "{0:D2}:{1:mm}", warDecl.WarLength.Hours, DateTime.MinValue + warDecl.WarLength ) ) );

						y = 290;

						AddButtonLabeled( 20, y, GetButtonID( 9, 0 ), "Guild Details" );
						AddButtonLabeled( 200, y, GetButtonID( 9, 1 ), "Opponent Details" );

						break;
					}
			}
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int val = (info.ButtonID - 1);

			if( val < 0 )
				return;

			int type = (val % 10);
			int index = (val / 10);

			switch( type )
			{
				case 0:
					{
						Page page;

						switch( index )
						{
							case 0: page = Page.Information; break;
							case 1: page = Page.Guilds; break;
							case 2: page = Page.Alliances; break;
							default: return;
						}

						_from.SendGump( new GuildAdminGump( _from, page, 0, null, null, null ) );
					} break;
				case 1: //change page
					{
						switch( index )
						{
							case 0:
								{
									if( _list != null && _listPage > 0 )
										_from.SendGump( new GuildAdminGump( _from, _pageType, (_listPage - 1), null, null, _state ) );
								} break;
							case 1:
								{
									if( _list != null )
										_from.SendGump( new GuildAdminGump( _from, _pageType, (_listPage + 1), null, null, _state ) );
								} break;
						}
					} break;
				case 4: //search or show guild details
					{
						switch( index )
						{
							case 0:
							case 1:
								{
									TextRelay relay = info.GetTextEntry( 0 );
									string toFind = (relay == null ? null : relay.Text.Trim().ToLower());
									string notice = "";

									List<Guild> foundList = SearchForGuild( toFind, (index == 0), out notice );

									if( foundList.Count == 1 )
										_from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, "One match found.", null, foundList[0] ) );
									else
										_from.SendGump( new GuildAdminGump( _from, Page.Guilds, 0, (notice == null ? (foundList.Count == 0 ? "Nothing matched your search terms." : null) : notice), foundList, null ) );

									break;
								}
							default:
								{
									index -= 2;

									if( _list != null && index >= 0 && index < _list.Count )
									{
										Guild g = _list[index];

										if( g == null )
											break;

										_from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, null, null, g ) );
									}

									break;
								}
						}
					} break;
				case 5: //show alliance details
					{
						List<AllianceInfo> allianceList = new List<AllianceInfo>( _allianceTable.Keys );

						if( allianceList != null && index >= 0 && index < allianceList.Count )
						{
							AllianceInfo aInfo = allianceList[index];

							if( aInfo == null )
								break;

							_from.SendGump( new GuildAdminGump( _from, Page.AllianceDetails, 0, null, null, aInfo ) );
						}
					} break;
				case 6: //show war details
					{
						if( _state is Guild )
							_from.SendGump( new GuildAdminGump( _from, Page.WarDetails, 0, null, null, ((Guild)_state).AcceptedWars[index] ) );
					} break;
				case 7: //show guild details
					{
						switch( index )
						{
							case 0: //disband
								{
									string warning = String.Format( "You are about to disband the guild \"{0}.\" Are you sure you want to do this?", ((Guild)_state).Name );
									_from.SendGump( new WarningGump( 1060635, 30720, warning, 0xFFC000, 420, 200, new WarningGumpCallback( DisbandGuild_Callback ), _state ) );

									break;
								}
							case 1: //alliance details
								{
									if( ((Guild)_state).Alliance == null )
										_from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, "This guild is not a member of an alliance.", null, _state ) );
									else
										_from.SendGump( new GuildAdminGump( _from, Page.AllianceDetails, 0, null, null, ((Guild)_state).Alliance ) );

									break;
								}
							case 2: //member list
								{
									_from.SendGump( new GuildAdminGump( _from, Page.GuildMemberList, 0, null, null, (Guild)_state ) );

									break;
								}
							case 3: //war list
								{
									if( ((Guild)_state).AcceptedWars == null || ((Guild)_state).AcceptedWars.Count == 0 )
										_from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, "This guild is not involved in any wars.", null, _state ) );
									else
										_from.SendGump( new GuildAdminGump( _from, Page.Wars, 0, null, null, _state ) );

									break;
								}
							case 4: //add member
								{
									_from.Target = new InternalRecruitTarget( (Guild)_state );
									_from.SendMessage( "Select the player to recruit into \"{0}.\"", ((Guild)_state).Name );

									_from.SendGump( new GuildAdminGump( _from, _pageType, 0, null, null, _state ) );

									break;
								}
							case 5: //guild properties [same as the GuildProps command]
								{
									_from.SendGump( new PropertiesGump( _from, (Guild)_state ) );
									_from.SendGump( new GuildInfoGump( (Server.Mobiles.PlayerMobile)_from, (Guild)_state ) );

									_from.SendGump( new GuildAdminGump( _from, _pageType, 0, null, null, _state ) );

									break;
								}
						}
					} break;
				case 8: //alliance details
					{
						switch( index )
						{
							case 0: //disband
								{
									string warning = String.Format( "You are about to disband the alliance \"{0}.\" Are you sure you want to do this?", ((AllianceInfo)_state).Name );
									_from.SendGump( new WarningGump( 1060635, 30720, warning, 0xFFC000, 420, 200, new WarningGumpCallback( DisbandAlliance_Callback ), _state ) );

									break;
								}
							case 1: //member list
								{
									AllianceInfo aInfo = _state as AllianceInfo;

									if( _allianceTable.ContainsKey( aInfo ) )
									{
										_from.SendGump( new GuildAdminGump( _from, Page.Guilds, 0, null, _allianceTable[aInfo], null ) );
									}
									else
									{
										_from.SendGump( new GuildAdminGump( _from, _pageType, 0, "The list of members for this alliance could not be found.", null, _state ) );
									}

									break;
								}
						}
					} break;
				case 9: //war details
					{
						switch( index )
						{
							case 0: _from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, null, null, ((WarDeclaration)_state).Guild ) ); break;
							case 1: _from.SendGump( new GuildAdminGump( _from, Page.GuildInfo, 0, null, null, ((WarDeclaration)_state).Opponent ) ); break;
						}
					} break;
			}
		}

		public static List<Guild> SearchForGuild( string toFind, bool searchNamesOnly, out string notice )
		{
			List<Guild> foundGuildList = new List<Guild>();

			if( toFind == null || toFind.Length == 0 )
				notice = String.Format( "You must enter {0} to search for.", searchNamesOnly ? "a guild name" : "a guild abbreviation" );
			else
			{
				notice = "";

				foreach( KeyValuePair<int, BaseGuild> kvp in Guild.List )
				{
					bool isMatch = false;

					if( kvp.Value.Name.ToLower().IndexOf( toFind.ToLower() ) >= 0 )
						isMatch = true;
					else if( !searchNamesOnly && kvp.Value.Abbreviation.ToLower().IndexOf( toFind.ToLower() ) >= 0 )
						isMatch = true;

					if( isMatch && kvp.Value is Guild )
						foundGuildList.Add( (Guild)kvp.Value );
				}

				foundGuildList.Sort( GuildComparer.Instance );
			}

			return foundGuildList;
		}

		#region WarningGump Callbacks
		private static void DisbandGuild_Callback( Mobile from, bool okay, object state )
		{
			string notice = "";

			if( okay )
			{
				((Guild)state).Disband();

				notice = String.Format( "\"{0}\" has been disbanded.", ((Guild)state).Name );
			}
			else
			{
				notice = String.Format( "You have chosen not to disband \"{0}.\"", ((Guild)state).Name );
			}

			from.SendGump( new GuildAdminGump( from, (okay ? Page.Guilds : Page.GuildInfo), 0, notice, null, (okay ? null : state) ) );
		}

		private static void DisbandAlliance_Callback( Mobile from, bool okay, object state )
		{
			string notice = "";

			if( okay )
			{
				((AllianceInfo)state).Disband();

				notice = String.Format( "The alliance, \"{0},\" has been disbanded.", ((AllianceInfo)state).Name );
			}
			else
			{
				notice = String.Format( "You have chosen not to disband \"{0}.\"", ((AllianceInfo)state).Name );
			}

			from.SendGump( new GuildAdminGump( from, (okay ? Page.Alliances : Page.AllianceDetails), 0, notice, null, (okay ? null : state) ) );
		}
		#endregion

		private class InternalRecruitTarget : Target
		{
			private Guild m_Guild;

			public InternalRecruitTarget( Guild guild )
				: base( 16, false, TargetFlags.None )
			{
				m_Guild = guild;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( targeted is PlayerMobile )
				{
					PlayerMobile m = targeted as PlayerMobile;

					if( !m.Alive )
					{
						from.SendLocalizedMessage( 501162 ); // Only the living may be recruited.
					}
					else if( m_Guild.IsMember( m ) )
					{
						from.SendLocalizedMessage( 501163 ); // They are already a guildmember!
					}
					else if( m_Guild.Candidates.Contains( m ) )
					{
						from.SendLocalizedMessage( 501164 ); // They are already a candidate.
					}
					else if( m_Guild.Accepted.Contains( m ) )
					{
						from.SendLocalizedMessage( 501165 ); // They have already been accepted for membership, and merely need to use the Guildstone to gain full membership.
					}
					else if( m.Guild != null )
					{
						from.SendLocalizedMessage( 501166 ); // You can only recruit candidates who are not already in a guild.
					}
					else
					{
						m_Guild.AddMember( m );

						from.SendMessage( "{0} has been recruited into the guild, \"{1}.\"", m.RawName, m_Guild.Name );
						m.SendMessage( "You have been recruited into the guild, \"{0}!\"", m_Guild.Name );
					}
				}
				else
				{
					from.SendMessage( "Only players can belong to a guild!" );
				}
			}
		}
	}
}