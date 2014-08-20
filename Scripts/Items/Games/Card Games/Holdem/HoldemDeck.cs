using Server;

namespace Server.Games.Holdem
{
	public class HoldemDeck : GameItem
	{
		[Constructable]
		public HoldemDeck()
			: base( DeckSize.One )
		{
			CardGame = new HoldemGame( this );
		}

		public HoldemDeck( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}