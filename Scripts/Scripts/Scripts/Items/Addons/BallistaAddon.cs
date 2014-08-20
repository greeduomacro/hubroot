using System;
using Server;

namespace Server.Items
{
	public class BallistaEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new BallistaEastAddonDeed();
			}
		}

		[Constructable]
		public BallistaEastAddon()
		{
			AddComponent( new AddonComponent( 16227 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16191 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16228 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16214 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16226 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16225 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16196 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16208 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16212 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16210 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16216 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16218 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16220 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16222 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16224 ), 0, -2, 0 );
		}

		public BallistaEastAddon( Serial serial )
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

	public class BallistaEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new BallistaEastAddon();
			}
		}

		[Constructable]
		public BallistaEastAddonDeed()
		{
			Name = "Ballista";
		}

		public BallistaEastAddonDeed( Serial serial )
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

	public class BallistaSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new BallistaSouthAddonDeed();
			}
		}

		[Constructable]
		public BallistaSouthAddon()
		{
			AddComponent( new AddonComponent( 16263 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16249 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16257 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16265 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16230 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16253 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16261 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16259 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16251 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16270 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16267 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16269 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16235 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16247 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16255 ), 2, 0, 0 );
		}

		public BallistaSouthAddon( Serial serial )
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

	public class BallistaSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new BallistaSouthAddon();
			}
		}

		[Constructable]
		public BallistaSouthAddonDeed()
		{
			Name = "BallistaSouth";
		}

		public BallistaSouthAddonDeed( Serial serial )
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