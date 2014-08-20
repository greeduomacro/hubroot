using System;
using Server;

namespace Server.Items
{
	public class SpiderSac : Item, ICarvable
	{
		[Constructable]
		public SpiderSac()
			: base( 0x10D9 )
		{
			Name = "spider silk egg case";
			Weight = 0.1;
		}

		public SpiderSac( Serial serial ) : base( serial ) { }

		public void Carve( Mobile from, Item item )
		{
			from.SendMessage( "You slice off many strands of the silky material and bundle them up for later use." );
			from.AddToBackpack( new SpidersSilk( Utility.Random( 10, 30 ) ) );

			Delete();
		}

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