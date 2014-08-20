using System;
using Server;

namespace Server.Items
{
	public class GarlicCrop : Item
	{
		[Constructable]
		public GarlicCrop()
			: base( 0x18E2 )
		{
			Movable = false;
			Name = "a garlic plant";
		}

		public GarlicCrop( Serial serial ) : base( serial ) { }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "You pull up a handful of garlic bulbs." );
			from.AddToBackpack( new GarlicRootCrop() );

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

		private class GarlicRootCrop : Item, ICarvable
		{
			[Constructable]
			public GarlicRootCrop()
				: base( 0x18E3 )
			{
				Name = "garlic bulbs";
				Weight = 0.01;
			}

			public GarlicRootCrop( Serial serial ) : base( serial ) { }

			public void Carve( Mobile from, Item item )
			{
				from.SendMessage( "You chop off the bulbs, and peel out some of the larger cloves." );
				from.AddToBackpack( new Garlic( Utility.RandomMinMax( 8, 15 ) ) );

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
}