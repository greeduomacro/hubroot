using System;
using Server;

namespace Server.Items
{
	public class DisplayCaseEast : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DisplayCaseEastDeed(); } }

		[Constructable]
		public DisplayCaseEast()
		{

			AddComponent( new AddonComponent( 2824 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 2821 ), 0, 0, 3 );
			AddComponent( new AddonComponent( 2823 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 2820 ), 0, 1, 3 );
			AddComponent( new AddonComponent( 2822 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 2819 ), 0, 2, 3 );
			AddComponent( new AddonComponent( 2819 ), 0, 2, 3 );

		}

		public DisplayCaseEast( Serial serial )
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

	public class DisplayCaseEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DisplayCaseEast(); } }

		[Constructable]
		public DisplayCaseEastDeed()
		{
			Name = "A Display Case (East) Deed";

		}

		public DisplayCaseEastDeed( Serial serial )
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

namespace Server.Items
{
	public class DisplayCaseSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DisplayCaseSouthDeed(); } }

		[Constructable]
		public DisplayCaseSouth()
		{

			AddComponent( new AddonComponent( 2818 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 2815 ), 0, 0, 3 );
			AddComponent( new AddonComponent( 2817 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 2814 ), 1, 0, 3 );
			AddComponent( new AddonComponent( 2816 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 2813 ), 2, 0, 3 );
			AddComponent( new AddonComponent( 2813 ), 2, 0, 3 );

		}

		public DisplayCaseSouth( Serial serial )
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

	public class DisplayCaseSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DisplayCaseSouth(); } }

		[Constructable]
		public DisplayCaseSouthDeed()
		{
			Name = "A Display Case (South) Deed";

		}

		public DisplayCaseSouthDeed( Serial serial )
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

namespace Server.Items
{
	public class HammockEast : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new HammockEastDeed(); } }

		[Constructable]
		public HammockEast()
		{

			AddComponent( new AddonComponent( 4595 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 4594 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 4594 ), 0, 2, 0 );

		}

		public HammockEast( Serial serial )
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

	public class HammockEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new HammockEast(); } }

		[Constructable]
		public HammockEastDeed()
		{
			Name = "A Hammock (East) Deed";

		}

		public HammockEastDeed( Serial serial )
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

namespace Server.Items
{
	public class HammockSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new HammockSouthDeed(); } }

		[Constructable]
		public HammockSouth()
		{

			AddComponent( new AddonComponent( 4592 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 4593 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 4593 ), 2, 0, 0 );

		}

		public HammockSouth( Serial serial )
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

	public class HammockSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new HammockSouth(); } }

		[Constructable]
		public HammockSouthDeed()
		{
			Name = "A Hammock (South) Deed";

		}

		public HammockSouthDeed( Serial serial )
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

namespace Server.Items
{
	public class LightWoodTableEast : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LightWoodTableEastDeed(); } }

		[Constructable]
		public LightWoodTableEast()
		{

			AddComponent( new AddonComponent( 2924 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 2925 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 2923 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 2923 ), 0, 2, 0 );

		}

		public LightWoodTableEast( Serial serial )
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

	public class LightWoodTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LightWoodTableEast(); } }

		[Constructable]
		public LightWoodTableEastDeed()
		{
			Name = "A Light Wood Table (East) Deed";

		}

		public LightWoodTableEastDeed( Serial serial )
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

namespace Server.Items
{
	public class LightWoodTableSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LightWoodTableSouthDeed(); } }

		[Constructable]
		public LightWoodTableSouth()
		{

			AddComponent( new AddonComponent( 2943 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 2944 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 2942 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 2942 ), 2, 0, 0 );

		}

		public LightWoodTableSouth( Serial serial )
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

	public class LightWoodTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LightWoodTableSouth(); } }

		[Constructable]
		public LightWoodTableSouthDeed()
		{
			Name = "A Light Wood Table (South) Deed";

		}

		public LightWoodTableSouthDeed( Serial serial )
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

namespace Server.Items
{
	public class LogTableEast : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LogTableEastDeed(); } }

		[Constructable]
		public LogTableEast()
		{

			AddComponent( new AddonComponent( 4576 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 4577 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 4575 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 4575 ), 0, 2, 0 );

		}

		public LogTableEast( Serial serial )
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

	public class LogTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LogTableEast(); } }

		[Constructable]
		public LogTableEastDeed()
		{
			Name = "A Log Table (East) Deed";

		}

		public LogTableEastDeed( Serial serial )
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

namespace Server.Items
{
	public class LogTableSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LogTableSouthDeed(); } }

		[Constructable]
		public LogTableSouth()
		{

			AddComponent( new AddonComponent( 4573 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 4574 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 4572 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 4572 ), 2, 0, 0 );

		}

		public LogTableSouth( Serial serial )
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

	public class LogTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LogTableSouth(); } }

		[Constructable]
		public LogTableSouthDeed()
		{
			Name = "A Log Table (South) Deed";

		}

		public LogTableSouthDeed( Serial serial )
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

namespace Server.Items
{
	public class MarbleTableSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new MarbleTableSouthDeed(); } }

		[Constructable]
		public MarbleTableSouth()
		{

			AddComponent( new AddonComponent( 7621 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 7622 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 7620 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 7620 ), 2, 0, 0 );

		}

		public MarbleTableSouth( Serial serial )
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

	public class MarbleTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new MarbleTableSouth(); } }

		[Constructable]
		public MarbleTableSouthDeed()
		{
			Name = "A Marble Table (South) Deed";

		}

		public MarbleTableSouthDeed( Serial serial )
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

namespace Server.Items
{
	public class RedGreenTableEast : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new RedGreenTableEastDeed(); } }

		[Constructable]
		public RedGreenTableEast()
		{

			AddComponent( new AddonComponent( 5737 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 5736 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 5735 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 5735 ), 0, 2, 0 );

		}

		public RedGreenTableEast( Serial serial )
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

	public class RedGreenTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new RedGreenTableEast(); } }

		[Constructable]
		public RedGreenTableEastDeed()
		{
			Name = "A Covered Table (East) Deed";

		}

		public RedGreenTableEastDeed( Serial serial )
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

namespace Server.Items
{
	public class RedGreenTableSouth : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new RedGreenTableSouthDeed(); } }

		[Constructable]
		public RedGreenTableSouth()
		{

			AddComponent( new AddonComponent( 5738 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 5739 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 5740 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 5740 ), 2, 0, 0 );

		}

		public RedGreenTableSouth( Serial serial )
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

	public class RedGreenTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new RedGreenTableSouth(); } }

		[Constructable]
		public RedGreenTableSouthDeed()
		{
			Name = "A Covered Table (South) Deed";

		}

		public RedGreenTableSouthDeed( Serial serial )
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
