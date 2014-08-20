using System;
using Server;
using Server.Gumps;

namespace Server.Help
{
	public enum PageType
	{
		GeneralQuestion = 1,
		Harrassment = 2,
		Other = 3
	}

	public class HelpGump : Gump
	{
		private const int LabelHue = 1152;
		private Mobile _from;

		public HelpGump( Mobile from )
			: base( 0, 0 )
		{
			_from = from;

			if( from.HasGump( typeof( HelpGump ) ) )
				from.CloseGump( typeof( HelpGump ) );

			AddPage( 0 );
			AddBackground( 10, 10, 430, 350, 9250 );
			AddAlphaRegion( 25, 50, 400, 265 );
			AddLabel( 100, 30, LabelHue, "Help Menu" );

			AddLabel( 345, 320, LabelHue, "Cancel" );
			AddButton( 395, 320, 4020, 4022, 0, GumpButtonType.Reply, 0 );

			AddHtml( 70, 60, 350, 75, @"<u>General Question:</u> Select this option if you have a general gameplay question. This encompasses questions about the shard's custom systems, command usage, and skill usage.", true, true );
			AddHtml( 70, 145, 350, 75, @"<u>Harrassment:</u> Use this option if another player is harrassing you, verbally or with game mechanics (i.e. luring monsters to attack you). Please be aware that we take all harrassment claims seriously, and it is inadvisable to report illegitimate claims of this type.", true, true );
			AddHtml( 70, 230, 350, 75, @"<u>Other:</u> Select this page type if your question or concern does not fall under any of the others. Use this category for suggestions, bug reports, and account management questions as well.", true, true );

			int x = 35, y = 85;

			for( int i = 1; i < 4; i++, y += 85 )
			{
				AddButton( x, y, 4005, 4007, i, GumpButtonType.Reply, 0 );
			}
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if( info.ButtonID == 0 )
			{
				_from.SendLocalizedMessage( 501235, "", 0x35 ); //Help request aborted.
				return;
			}

			PageType type = (PageType)info.ButtonID;

			if( ((int)type) > -1 && PageQueue.AllowedToPage( _from ) )
				_from.SendGump( new PagePromptGump( _from, type ) );
		}
	}
}