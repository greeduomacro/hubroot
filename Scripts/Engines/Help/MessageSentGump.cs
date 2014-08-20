using System;
using Server;
using Server.Gumps;

namespace Server.Help
{
	public class MessageSentGump : Gump
	{
		private string _text;
		private Mobile _mobile, _staff;

		public MessageSentGump( Mobile mobile, Mobile staff, string text )
			: base( 0, 0 )
		{
			_staff = staff;
			_text = text;
			_mobile = mobile;

			Closable = false;

			AddPage( 0 );
			AddBackground( 10, 10, 92, 75, 0xA3C );
			AddImageTiled( 15, 17, 82, 61, 0xA40 );
			AddAlphaRegion( 15, 17, 82, 61 );

			AddImageTiled( 19, 21, 21, 53, 0xBBC );

			AddButton( 20, 22, 0x7D2, 0x7D2, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 44, 38, 65, 24, 3001002, 0xFFFFFF, false, false ); //Message
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			_mobile.SendGump( new PageResponseGump( _mobile, _staff, _text ) );
		}
	}
}