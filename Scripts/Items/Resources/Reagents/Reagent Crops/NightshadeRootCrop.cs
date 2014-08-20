using System;
using Server;

namespace Server.Items
{
	public class NightshadeCrop : Item, ICarvable
	{
		[Constructable]
		public NightshadeCrop()
			: base( 0x18E6 )
		{
			Movable = false;
			Name = "nightshade plant";
		}

		public NightshadeCrop( Serial serial ) : base( serial ) { }

		public void Carve( Mobile from, Item item )
		{
			from.SendMessage( "You slice several of the leaves off the nightshade plant." );
			from.AddToBackpack( new Nightshade( Utility.RandomMinMax( 8, 15 ) ) );

			Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "You might do better with a bladed item!" );
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