
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class OpenKnightSarcophogusSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OpenKnightSarcophogusSAddonDeed(); } }

		[ Constructable ]
		public OpenKnightSarcophogusSAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 7356 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 7354 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 7355 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 7353 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 7352 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 7358 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 7359 );
			AddComponent( ac, 0, 3, 0 );
			ac = new AddonComponent( 7357 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 7351 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 7350 );
			AddComponent( ac, 1, 3, 0 );
		}

		public OpenKnightSarcophogusSAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class OpenKnightSarcophogusSAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OpenKnightSarcophogusSAddon(); } }

		[Constructable]
		public OpenKnightSarcophogusSAddonDeed()
		{
			Name = "OpenKnightSarcophogusS";
		}

		public OpenKnightSarcophogusSAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}