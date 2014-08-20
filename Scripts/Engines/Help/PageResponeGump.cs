using System;
using Server;
using Server.Gumps;

namespace Server.Help
{
	public class PageResponseGump : Gump
	{
		private string _text;
		private Mobile _mobile, _staff;

		public PageResponseGump( Mobile mobile, Mobile staff, string text )
			: base( 0, 0 )
		{
			_mobile = mobile;
			_staff = staff;
			_text = text;

			AddPage( 0 );
			AddBackground( 20, 20, 345, 370, 9250 );
			AddLabel( 90, 35, 0, "Staff Help Response" );

			AddButton( 290, 350, 4023, 4025, 0, GumpButtonType.Reply, 0 );

			AddHtml( 35, 60, 314, 285, String.Format( "Message from [{0}] {1}: {2}", staff.AccessLevel.ToString(), staff.RawName, text ), true, false );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			_mobile.CloseGump( typeof( PageResponseGump ) );
		}
	}
}