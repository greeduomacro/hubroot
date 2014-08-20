using System;
using Server;
using Server.Gumps;

namespace Server.Help
{
	public class PagePromptGump : Gump
	{
		private const int LabelHue = 1152;

		private Mobile _from;
		private PageType _type;

		public PagePromptGump( Mobile from, PageType type )
			: base( 0, 0 )
		{
			Closable = true;

			_from = from;
			_type = type;

			AddPage( 0 );
			AddBackground( 85, 65, 485, 285, 9250 );

			AddBackground( 97, 130, 460, 185, 3500 );
			AddTextEntry( 120, 153, 414, 138, 1153, 0, "" );

			AddLabel( 275, 80, LabelHue, "Page Description" );
			AddLabel( 105, 110, LabelHue, "Enter a brief description of your problem:" );

			AddButton( 325, 315, 4023, 4025, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if( info.ButtonID == 0 )
			{
				_from.SendLocalizedMessage( 501235, "", 0x35 ); //Help request aborted.
				return;
			}
			else
			{
				TextRelay txtEntry = info.GetTextEntry( 0 );
				string text = (txtEntry == null ? "" : txtEntry.Text);

				if( text.Length == 0 )
				{
					_from.SendMessage( 0x35, "You must enter a description of your problem." );
					_from.SendGump( new PagePromptGump( _from, _type ) );
				}
				else
				{
					_from.SendMessage( 0x35, "The next available staff member will be with you as soon as possible. If your issue is resolved early, please revoke it from the page queue. To check the statue of your page, type \'[CheckPage\'." );

					PageQueue.AddNewPage( new PageEntry( _from, text, _type ) );
				}
			}
		}
	}
}