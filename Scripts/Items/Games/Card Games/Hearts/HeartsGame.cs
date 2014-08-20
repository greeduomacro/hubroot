using System;
using System.Collections.Generic;
using Server;

namespace Server.Games.Hearts
{
	public class HeartsGame : BaseCardGame
	{
		public HeartsGame( ICardGameItem gameItem ) : base( gameItem, 4, BaseCardGame.DefaultTimeout )
		{
			HandSize = 13;
		}

		#region +override void DealFirstRound()
		public override void DealFirstRound()
		{
			for( byte i = 0; i < 13; i++ )
			{
				foreach( KeyValuePair<Mobile, PokerHand> kvp in Players )
				{
					if( kvp.Value.FromDeck == null )
						kvp.Value.FromDeck = GameItem.Deck;

					if( kvp.Value.MaxCards != 13 )
						kvp.Value.MaxCards = 13;

					kvp.Value.GiveCard( GameItem.Deck.DrawOne() );
				}
			}

			foreach( KeyValuePair<Mobile, PokerHand> kvp in Players )
			{
				kvp.Key.SendGump( new HeartsGameGump( this, kvp.Value ) );
			}
		}
		#endregion
	}
}