using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LighthouseAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LighthouseAddonDeed(); } }

		[Constructable]
		public LighthouseAddon()
		{
			AddComponent( new AddonComponent( 6845 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 6849 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 6843 ), -3, -3, 0 );
			AddComponent( new AddonComponent( 6844 ), -3, -2, 0 );
			AddComponent( new AddonComponent( 6832 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 6860 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 6820 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 6829 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 6822 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 6821 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 6828 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 6823 ), 2, 3, 0 );
			AddComponent( new AddonComponent( 6825 ), 3, 2, 0 );
			AddComponent( new AddonComponent( 6824 ), 3, 3, 0 );
			AddComponent( new AddonComponent( 6827 ), 3, 0, 0 );
			AddComponent( new AddonComponent( 6826 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 6855 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 6852 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 6838 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 6835 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 6841 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 6863 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 6859 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 6858 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 6862 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 6842 ), -2, -3, 0 );
			AddComponent( new AddonComponent( 6846 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 6861 ), -2, -1, 0 );

			AddonComponent ac = new AddonComponent( 6864 );
			ac.Light = LightType.Circle300;
			AddComponent( ac, 2, 2, 0 );
		}

		public LighthouseAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class LighthouseAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LighthouseAddon(); } }

		[Constructable]
		public LighthouseAddonDeed()
		{
			Name = "lighthouse";
		}

		public LighthouseAddonDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}