
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class KnightSarcophogusSAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new KnightSarcophogusSAddonDeed(); } }

		[ Constructable ]
		public KnightSarcophogusSAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 7335 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 7336 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 7337 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 7338 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 7339 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 7340 );
			AddComponent( ac, 0, 1, 0 );

		}

		public KnightSarcophogusSAddon( Serial serial ) : base( serial )
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

	public class KnightSarcophogusSAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new KnightSarcophogusSAddon(); } }

		[Constructable]
		public KnightSarcophogusSAddonDeed()
		{
			Name = "KnightSarcophogusS";
		}

		public KnightSarcophogusSAddonDeed( Serial serial ) : base( serial )
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