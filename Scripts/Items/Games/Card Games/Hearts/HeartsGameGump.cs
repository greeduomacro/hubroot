using System;
using Server;
using Server.Gumps;

namespace Server.Games.Hearts
{
	public class HeartsGameGump : Gump
	{
		private HeartsGame _game;
		private PokerHand _hand;

		public HeartsGameGump( HeartsGame game, PokerHand hand )
			: base( 10, 10 )
		{
			_game = game;
			_hand = hand;

			AddPage( 1 );
			AddBackground( 0, 0, 640, 480, 9250 );
		}
	}
}