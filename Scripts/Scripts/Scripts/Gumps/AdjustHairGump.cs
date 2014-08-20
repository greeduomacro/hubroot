using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class AdjustHairGump : Gump
	{
		private Mobile m_From;

		public AdjustHairGump( Mobile from )
			: base( 0, 0 )
		{
			m_From = from;

			AddPage( 1 );
			AddBackground( 10, 10, 430, 390, 83 );
			AddAlphaRegion( 20, 22, 410, 365 );
			AddHtml( 25, 40, 400, 95, String.Format( "<CENTER>Character Hair Adjustment</CENTER>\n To choose " +
													"a new hair or facial hair style, simply click the checkbox " +
													"beneath the option that you would like. Don\'t forget to " +
													"consider time when choosing hair of varying lengths (for example, " +
													"it would generally take a while for short hair to grow long, " +
													"but in the meantime you could select a medium-length hairstyle)." ), true, true );

			AddButton( 415, 23, 3, 4, 0, GumpButtonType.Reply, 0 ); //close

			if( !from.Female )
			{
				AddLabel( 275, 363, 1150, "Adjust Facial Hair" );
				AddButton( 395, 365, 5601, 5605, 0, GumpButtonType.Page, 2 );
			}

			for( int i = 1; i <= 9; i++ )
			{
				int x = 50;
				int y = 140;

				if( i == 2 || i == 6 )
					x += 95;
				else if( i == 3 || i == 7 )
					x += (95 * 2);
				else if( i == 4 || i == 8 )
					x += (95 * 3);

				if( i > 4 && i < 9 )
					y = 225;
				else if( i >= 9 )
					y = 310;

				AddHairBackground( x, y );
				AddButton( x + 16, y + 55, 2714, 2715, i, GumpButtonType.Reply, i );
			}

			AddImage( -38, 84, 1875 ); //short hair
			AddImage( 64, 92, 1840 ); //topknot
			AddImage( 149, 77, 1836 ); //pigtails
			AddImage( 248, 84, 1871 ); //pageboy
			AddImage( -37, 172, 1873 ); //afro
			AddImage( 58, 165, 1876 ); //long hair
			AddImage( 153, 166, 1845 ); //pony tail
			AddImage( 247, 169, 1880 ); //receding

			AddPage( 2 );
			AddBackground( 10, 10, 430, 225, 83 );
			AddAlphaRegion( 20, 22, 410, 200 );

			AddLabel( 45, 205, 1150, "Adjust Hair" );
			AddButton( 20, 208, 5603, 5607, 0, GumpButtonType.Page, 1 );

			for( int i = 1; i <= 8; i++ )
			{
				int x = 55;
				int y = 30;

				if( i == 2 || i == 6 )
					x += 95;
				else if( i == 3 || i == 7 )
					x += (95 * 2);
				else if( i == 4 || i == 8 )
					x += (95 * 3);

				if( i > 4 )
					y = 115;

				AddHairBackground( x, y );
				AddButton( x + 16, y + 55, 2714, 2715, i + 9, GumpButtonType.Reply, i + 9 );
			}

			AddImage( -33, -32, 1881 ); //goatee
			AddImage( 62, -32, 1882 ); //long beard/moustache
			AddImage( 157, -32, 1883 ); //long beard
			AddImage( 253, -34, 1884 ); //moustache
			AddImage( -32, 60, 1885 ); //short beard
			AddImage( 62, 60, 1886 ); //short beard/moustache
			AddImage( 158, 55, 1887 ); //vandyke
		}

		private void AddHairBackground( int x, int y )
		{
			AddBackground( x, y, 50, 50, 2620 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int button = info.ButtonID;

			if( button == 0 )
				return;

			int newHair = 0;
			int newBeard = 0;

			switch( button )
			{
				default: break;
				case 1: newHair = Hair.ShortHair; break;
				case 2: newHair = Hair.KrisnaHair; break;
				case 3: newHair = Hair.TwoPigTails; break;
				case 4: newHair = Hair.PageboyHair; break;
				case 5: newHair = Hair.Afro; break;
				case 6: newHair = Hair.LongHair; break;
				case 7: newHair = Hair.PonyTail; break;
				case 8: newHair = Hair.ReceedingHair; break;
				case 10: newBeard = Beard.Goatee; break;
				case 11: newBeard = Beard.MediumLongBeard; break;
				case 12: newBeard = Beard.LongBeard; break;
				case 13: newBeard = Beard.Mustache; break;
				case 14: newBeard = Beard.ShortBeard; break;
				case 15: newBeard = Beard.MediumShortBeard; break;
				case 16: newBeard = Beard.Vandyke; break;
			}

			if( button < 10 )
			{
				m_From.HairItemID = newHair;
			}
			else
			{
				m_From.FacialHairItemID = newBeard;
			}
		}
	}
}
