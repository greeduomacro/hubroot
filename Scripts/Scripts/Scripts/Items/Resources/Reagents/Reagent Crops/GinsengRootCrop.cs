using System;
using Server;

namespace Server.Items
{
	public class GinsengCrop : Item
	{
		[Constructable]
		public GinsengCrop()
			: base( 0x18EA )
		{
			Movable = false;
			Name = "ginseng";
		}

		public GinsengCrop( Serial serial ) : base( serial ) { }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "You yank the ginseng from the ground." );
			from.AddToBackpack( new GinsengRootCrop() );

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

		private class GinsengRootCrop : Item, ICarvable
		{
			[Constructable]
			public GinsengRootCrop()
				: base( 0x18EC )
			{
				Name = "ginseng root";
				Weight = 0.1;
			}

			public GinsengRootCrop( Serial serial ) : base( serial ) { }

			public void Carve( Mobile from, Item item )
			{
				from.SendMessage( "You chop up the roots of the ginseng plant, gathering a handful of useful lengths." );
				from.AddToBackpack( new Ginseng( Utility.RandomMinMax( 8, 15 ) ) );

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