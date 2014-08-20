using System;
using Server;

namespace Server.Games
{
	public interface ICardGameItem
	{
		ICardGame CardGame { get; }
		CardDeck Deck { get; }
		Mobile Dealer { get; }
	}
}