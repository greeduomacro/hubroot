using System;
using Server;

namespace Server.Items
{
	public class DresserEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DresserEastDeed(); } }

		[Constructable]
		public DresserEastAddon()
		{
			AddComponent( new AddonComponent( 0xA45 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xA44 ), 0, 1, 0 );
		}

		public DresserEastAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DresserEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DresserEastAddon(); } }

		[Constructable]
		public DresserEastDeed()
		{
			Name = "Dresser [East]";
		}

		public DresserEastDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DresserSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DresserSouthDeed(); } }

		[Constructable]
		public DresserSouthAddon()
		{
			AddComponent( new AddonComponent( 0xA3D ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0xA3C ), 0, 0, 0 );
		}

		public DresserSouthAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class DresserSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DresserSouthAddon(); } }

		[Constructable]
		public DresserSouthDeed()
		{
			Name = "Dresser [South]";
		}

		public DresserSouthDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
