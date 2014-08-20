using System;
using Server;

namespace Server.Items
{
	public class MandrakeCrop : Item
	{
		[Constructable]
		public MandrakeCrop()
			: base( 0x18DF )
		{
			Movable = false;
			Name = "mandrake";
		}

		public MandrakeCrop( Serial serial ) : base( serial ) { }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "You pull up the mandrake root." );
			from.AddToBackpack( new MandrakeRootCrop() );

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

		private class MandrakeRootCrop : Item, ICarvable
		{
			[Constructable]
			public MandrakeRootCrop()
				: base( 0x18DE )
			{
				Name = "mandrake root";
				Weight = 0.1;
			}

			public MandrakeRootCrop( Serial serial ) : base( serial ) { }

			public void Carve( Mobile from, Item item )
			{
				from.SendMessage( "You carve the root into several usable tendrils." );
				from.AddToBackpack( new MandrakeRoot( Utility.RandomMinMax( 8, 15 ) ) );

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