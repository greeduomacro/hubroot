using System;
using System.Collections.Generic;
using Server;

namespace Server.Games
{
	public abstract class BaseCardGame : ICardGame
	{
		public static readonly byte GlobalMaxPlayers = 6;
		public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes( 2.0 );

		private ICardGameItem _gameItem;
		private byte _handSize;
		private bool _init;
		private byte _maxPlayers;
		private Dictionary<Mobile, PokerHand> _players;
		private TimeSpan _waitTimeout;

		public ICardGameItem GameItem { get { return _gameItem; } protected set { _gameItem = value; } }
		public byte HandSize { get { return _handSize; } protected set { _handSize = value; } }
		public bool Initialized { get { return _init; } protected set { _init = value; } }
		public byte MaxPlayers { get { return _maxPlayers; } protected set { _maxPlayers = Math.Min( value, GlobalMaxPlayers ); } }
		public Dictionary<Mobile, PokerHand> Players { get { return _players; } protected set { _players = value; } }
		public TimeSpan PlayerWaitTimeout { get { return _waitTimeout; } protected set { _waitTimeout = value; } }

		public BaseCardGame( ICardGameItem gameItem, byte maxPlayers, TimeSpan playerTimeout )
		{
			_gameItem = gameItem;
			_maxPlayers = Math.Min( maxPlayers, GlobalMaxPlayers );
			_players = new Dictionary<Mobile, PokerHand>( _maxPlayers );
			_waitTimeout = playerTimeout;
		}

		#region +virtual void DealFirstRound()
		public virtual void DealFirstRound()
		{
		}
		#endregion

		#region +virtual void Initialize()
		/// <summary>
		/// Initializes the card game and its deck
		/// </summary>
		public virtual void Initialize()
		{
			_gameItem.Deck.ShuffleDeck( 4 );

			_init = true;
		}
		#endregion

		#region +virtual bool AddPlayer( Mobile, byte )
		/// <summary>
		/// Attempts to add the given <code>Mobile</code> to the list of players
		/// </summary>
		/// <param name="numCards">maximum number of cards in the player's hand</param>
		public virtual bool AddPlayer( Mobile m, byte numCards )
		{
			if( Players.Count >= MaxPlayers || Players.ContainsKey( m ) )
				return false;

			Players.Add( m, new PokerHand( numCards ) );

			return Players.ContainsKey( m );
		}
		#endregion

		#region +virtual bool IsPlaying( Mobile )
		/// <summary>
		/// Determines whether or not the given <code>Mobile</code> is a participant in this game
		/// </summary>
		public virtual bool IsPlaying( Mobile m )
		{
			return Players.ContainsKey( m );
		}
		#endregion

		#region +virtual bool RemovePlayer( Mobile )
		/// <summary>
		/// Removes the given <code>Mobile</code> from the player list
		/// </summary>
		public virtual bool RemovePlayer( Mobile m )
		{
			if( !Players.ContainsKey( m ) )
				return false;

			return Players.Remove( m );
		}
		#endregion
	}
}