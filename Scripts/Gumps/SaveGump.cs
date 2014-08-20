using System;
using Server;
using Server.Gumps;

namespace Server.Gumps
{
	public class SaveGump : Gump
	{
		public SaveGump()
			: base( 200, 200 )
		{
			Closable = false;

			AddPage( 1 );
			AddBackground( 0, 0, 350, 220, 9250 );
			AddAlphaRegion( 15, 15, 320, 190 );

			AddItem( 10, 145, 2923 );
			AddItem( 25, 125, 2924 );
			AddItem( 16, 144, 2450 ); //serving tray
			AddItem( 15, 155, 2515 ); //ham
			AddItem( 30, 125, 2466 ); //bottles of ale

			AddItem( 180, 55, 31 );
			AddItem( 160, 75, 32 );
			AddItem( 179, 95, 4171 ); //wall clock
			AddItem( 140, 135, 4168 ); //globe

			AddItem( 271, 101, 4590 );
			AddItem( 290, 90, 4591 );
			AddItem( 277, 157, 3555 ); //small fire
			AddItem( 230, 135, 11766 ); //reading chair
			AddItem( 195, 145, 7717 ); //stack of books

			AddHtml( 20, 15, 300, 60, String.Format( "<basefont color=#E0AF19>Please wait while world data is saved. This can take a few minutes, so sit back with an ale or a book and enjoy the wait.</basefont>" ), false, false );
		}
	}
}