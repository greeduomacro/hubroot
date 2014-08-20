using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;

namespace Server.Help
{
	public class PageQueueGump : Gump
	{
		private PageEntry[] _list;

		public PageQueueGump()
			: base( 0, 0 )
		{
			AddPage( 0 );
			AddImageTiled( 10, 10, 410, 448, 0xA40 );
			AddAlphaRegion( 11, 11, 408, 446 );

			AddLabel( 190, 22, 2100, "Page Queue" );

			List<PageEntry> list = PageQueue.List;

			for( int i = 0; i < list.Count; )
			{
				if( list[i].Sender == null || list[i].Sender.Deleted )
					PageQueue.Remove( list[i] );
				else
					i++;
			}

			_list = list.ToArray();

			if( _list.Length > 0 )
			{
				AddPage( 1 );

				for( int i = 0; i < _list.Length; ++i )
				{
					PageEntry entry = _list[i];

					if( i >= 5 && (i % 5) == 0 )
					{
						AddLabel( 308, 22, 2100, "Next Page" );
						AddLabel( 58, 22, 2100, "Previous Page" );
						AddButton( 378, 22, 0xFA5, 0xFA7, 0, GumpButtonType.Page, (i / 5) + 1 );
						AddButton( 22, 22, 0xFAE, 0xFB0, 0, GumpButtonType.Page, (i / 5) );

						AddPage( (i / 5) + 1 );
					}

					string typeString = PageQueue.GetPageTypeName( entry.Type );

					string html = String.Format( "[{0}] {1} <basefont color=#{2:X6}>[<u>{3}</u>]</basefont>", typeString, entry.Message, entry.Handler == null ? 0xFF0000 : 0xFF, entry.Handler == null ? "Unhandled" : "Handling" );

					AddHtml( 22, 54 + ((i % 5) * 80), 350, 70, html, true, true );
					AddButton( 380, 54 + ((i % 5) * 80) + 24, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0 );
				}
			}
			else
			{
				AddLabel( 22, 54, 2100, "The page queue is empty." );
			}
		}

		public override void OnResponse( Server.Network.NetState state, RelayInfo info )
		{
			if( info.ButtonID >= 1 && info.ButtonID <= _list.Length )
			{
				PageEntryGump g = new PageEntryGump( state.Mobile, _list[info.ButtonID - 1] );

				g.SendTo( state );
			}
		}
	}
}