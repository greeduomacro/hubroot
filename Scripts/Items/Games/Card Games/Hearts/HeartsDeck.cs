using System;
using Server;

namespace Server.Games.Hearts
{
	public class HeartsDeck : GameItem
	{
		[Constructable]
		public HeartsDeck()
			: base( DeckSize.One )
		{
			this.CardGame = new HeartsGame( this );
		}

		public HeartsDeck( Serial serial ) : base( serial ) { }

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