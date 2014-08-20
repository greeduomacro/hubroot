using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.SkillSelection {
	public class PointsRemainingWarningGump : Gump {
		private Mobile _from;
		private Gump _gumpToClose;

		public PointsRemainingWarningGump( Mobile from, Gump gump, int pointsLeft )
			: base( 0, 0 ) {
			Closable = false;

			_from = from;
			_gumpToClose = gump;

			AddPage( 1 );
			AddBackground( 330, 230, 310, 195, 9250 );
			AddAlphaRegion( 345, 245, 280, 140 );

			AddHtml( 345, 245, 280, 140, String.Format( "<BASEFONT COLOR=white>You have {0} remaining points available. " +
													 "Are you sure you wish to continue without " +
													 "alotting these points?</BASEFONT>", pointsLeft ), false, false );

			AddLabel( 460, 395, 0, "Continue?" );
			AddButton( 425, 390, 4020, 4022, 0, GumpButtonType.Reply, 0 ); //cancel
			AddButton( 525, 390, 4023, 4025, 1, GumpButtonType.Reply, 0 ); //okay
		}

		public override void OnResponse( NetState sender, RelayInfo info ) {
            if (info.ButtonID != 0)
            {
                _from.CloseGump(_gumpToClose.GetType());

                SkillScroll scroll = new SkillScroll();
                _from.AddToBackpack(scroll);
            }
            else
            {
                _from.SendGump(_gumpToClose);
            }

		}
	}
}
