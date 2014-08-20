using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChoppingBlockAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ChoppingBlockAddonDeed();
			}
		}

		[Constructable]
		public ChoppingBlockAddon()
		{
			AddComponent( new AddonComponent( 4715 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 4716 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 4717 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 4722 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 4723 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 4720 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 4721 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 4719 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 4718 ), 0, -1, 0 );
			AddonComponent ac = null;
			ac = new AddonComponent( 4723 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 4722 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 4719 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 4718 );
			AddComponent( ac, 0, -1, 0 );

		}

		public ChoppingBlockAddon( Serial serial )
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

	public class ChoppingBlockAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ChoppingBlockAddon();
			}
		}

		[Constructable]
		public ChoppingBlockAddonDeed()
		{
			Name = "ChoppingBlock";
		}

		public ChoppingBlockAddonDeed( Serial serial )
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
