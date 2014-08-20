using System;
using System.Collections.Generic;
using Server;

namespace Server.Games
{
	public interface ICardGame
	{
		ICardGameItem GameItem { get; }
		byte HandSize { get; }
		bool Initialized { get; }
		byte MaxPlayers { get; }
		Dictionary<Mobile, PokerHand> Players { get; }
		TimeSpan PlayerWaitTimeout { get; }

		void DealFirstRound();
		void Initialize();

		bool AddPlayer( Mobile m, byte numCards );
		bool IsPlaying( Mobile m );
		bool RemovePlayer( Mobile m );
	}
}