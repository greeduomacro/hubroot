using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Games
{
	public abstract class GameItem : Item, ICardGameItem
	{
		private ICardGame _game;
		private CardDeck _deck;
		private Mobile _dealer;

		public ICardGame CardGame { get { return _game; } protected set { _game = value; } }
		public CardDeck Deck { get { return _deck; } protected set { _deck = value; } }
		public Mobile Dealer { get { return _dealer; } protected set { _dealer = value; } }

		public GameItem( DeckSize deckSize )
			: base( 0x12AB )
		{
			_deck = new CardDeck( deckSize );
		}

		public GameItem( Serial serial ) : base( serial ) { }

		#region +override void GetContextMenuEntries( Mobile, List<ContextMenuEntry> )
		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if( Dealer == from )
			{
				if( CardGame.Players.Count < CardGame.MaxPlayers )
					list.Add( new CallbackEntry( 464, 1, new ContextCallback( BeginAddPlayer ) ) );

				if( CardGame.Players.Count > 0 )
				{
					list.Add( new CallbackEntry( 465, 1, new ContextCallback( BeginRemovePlayer ) ) );
					list.Add( new CallbackEntry( 6510, 1, new ContextCallback( CardGame.DealFirstRound ) ) );
				}
			}
		}
		#endregion

		#region +override void OnDoubleClick( Mobile )
		public override void OnDoubleClick( Mobile from )
		{
			if( Dealer == null )
			{
				Dealer = from;
				PublicOverheadMessage( Server.Network.MessageType.Regular, 901, false, "The dealer has been changed to: " + from.RawName );
			}

			if( Dealer == from && CardGame != null )
			{
				if( !CardGame.Initialized )
					CardGame.Initialize();
			}
		}
		#endregion

		#region +override void Serialize( GenericWriter )
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}
		#endregion

		#region +override void Deserialize( GenericReader )
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
		#endregion

		#region ~virtual void BeginAddPlayer()
		protected virtual void BeginAddPlayer()
		{
			Dealer.BeginTarget( 6, false, TargetFlags.None, new TargetCallback( EndAddPlayer ) );
			Dealer.SendMessage( "Select the player to add to this game." );
		}
		#endregion

		#region ~virtual void BeginRemovePlayer()
		protected virtual void BeginRemovePlayer()
		{
			Dealer.BeginTarget( 6, false, TargetFlags.None, new TargetCallback( EndRemovePlayer ) );
			Dealer.SendMessage( "Select the player to remove from the game." );
		}
		#endregion

		#region ~virtual void EndAddPlayer( Mobile, object )
		protected virtual void EndAddPlayer( Mobile from, object target )
		{
			if( target is PlayerMobile )
			{
				if( !CardGame.IsPlaying( (PlayerMobile)target ) )
					CardGame.AddPlayer( (PlayerMobile)target, CardGame.HandSize );
			}
			else
			{
				BeginAddPlayer();
				from.SendMessage( "Only players can join this game." );
			}
		}
		#endregion

		#region ~virtual void EndRemovePlayer( Mobile, object )
		protected virtual void EndRemovePlayer( Mobile from, object target )
		{
			if( target is PlayerMobile )
			{
				if( CardGame.IsPlaying( (PlayerMobile)target ) )
				{
					PokerHand hand = CardGame.Players[(PlayerMobile)target];
					hand.ReturnToDeck();

					CardGame.RemovePlayer( (PlayerMobile)target );
				}
			}
			else
			{
				BeginRemovePlayer();
				from.SendMessage( "That cannot be a player of this game." );
			}
		}
		#endregion
	}
}