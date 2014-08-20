//Created by Ethereal 5/10/2003
using System;
using Server;

namespace Server.Items
{
	public class OrecartEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new OrecartEastDeed(); } }

		[Constructable]
		public OrecartEastAddon()
		{
			AddComponent( new AddonComponent( 0x1A8B ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0x1A88 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0x1A87 ), 2, 0, 0 );
		}

		public OrecartEastAddon( Serial serial )
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

	public class OrecartEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new OrecartEastAddon(); } }
		public override int LabelNumber { get { return 0; } } // Ore Cart (east)

		[Constructable]
		public OrecartEastDeed()
		{
			Name = "a deed for an Orecart (east)";
		}

		public OrecartEastDeed( Serial serial )
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