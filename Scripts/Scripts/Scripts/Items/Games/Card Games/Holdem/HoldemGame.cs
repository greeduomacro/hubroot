

namespace Server.Games.Holdem
{
	public class HoldemGame : BaseCardGame
	{
		public HoldemGame( ICardGameItem gameItem )
			: base( gameItem, 6, BaseCardGame.DefaultTimeout )
		{
			HandSize = 2;
		}

		public override void DealFirstRound()
		{

		}
	}
}