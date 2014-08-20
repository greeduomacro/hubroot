using System;
using Server;

namespace Server.Items
{
	public class SmallTreasurePileAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new SmallTreasurePileAddonDeed();
			}
		}

		[Constructable]
		public SmallTreasurePileAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 6987 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 6996 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 6986 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 6984 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 6982 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 6983 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 6980 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 6979 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 6978 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 6977 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 6976 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 6975 );
			AddComponent( ac, 1, 1, 0 );

		}

		public SmallTreasurePileAddon( Serial serial )
			: base( serial )
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

	public class SmallTreasurePileAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SmallTreasurePileAddon();
			}
		}

		[Constructable]
		public SmallTreasurePileAddonDeed()
		{
			Name = "SmallTreasurePile";
		}

		public SmallTreasurePileAddonDeed( Serial serial )
			: base( serial )
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
}