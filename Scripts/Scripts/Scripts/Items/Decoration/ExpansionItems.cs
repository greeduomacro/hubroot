using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x2A58, 0x2A59 )]
	public class BoneThrone : Item
	{
		[Constructable]
		public BoneThrone()
			: base( 0x2A59 )
		{
			Name = "bone throne";
			Weight = 5.0;
		}

		public BoneThrone( Serial serial )
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

	[Furniture]
	[Flipable( 0x2AC0, 0x2AC3 )]
	public class WallFountain : Item
	{
		[Constructable]
		public WallFountain()
			: base( 0x2AC0 )
		{
			Name = "a fountain";
			Weight = 15.0;
		}

		public WallFountain( Serial serial )
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

	[Furniture]
	[Flipable( 0x2D0B, 0x2D0C )]
	public class LargeWashBasin : Item
	{
		[Constructable]
		public LargeWashBasin()
			: base( 0x2D0B )
		{
			Name = "a large wash basin";
			Weight = 20.0;
		}

		public LargeWashBasin( Serial serial )
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

	public class FruitBowl : Item
	{
		[Constructable]
		public FruitBowl()
			: base( 0x2D4F )
		{
			Name = "a bowl of fruit";
			Weight = 1.0;
		}

		public FruitBowl( Serial serial )
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

	[Flipable( 0x2D61, 0x2D62 )]
	public class Inkwell : Item
	{
		[Constructable]
		public Inkwell()
			: base( 0x2D61 )
		{
			Name = "an inkwell";
			Weight = 1.0;
		}

		public Inkwell( Serial serial )
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

	[Flipable( 0x2D73, 0x2D74 )]
	public class WallMap : Item
	{
		[Constructable]
		public WallMap()
			: base( 0x2D74 )
		{
			Name = "a cloth wall map";
		}

		public WallMap( Serial serial )
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

	[Furniture]
	[Flipable( 0x2DE1, 0x2DE2 )]
	public class RoundedOrnateTable : Item
	{
		[Constructable]
		public RoundedOrnateTable()
			: base( 0x2DE1 )
		{
			Name = "a rounded ornate table";
			Weight = 10.0;
		}

		public RoundedOrnateTable( Serial serial )
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

	[Furniture]
	[Flipable( 0x2DE7, 0x2DE8 )]
	public class FlatOrnateTable : Item
	{
		[Constructable]
		public FlatOrnateTable()
			: base( 0x2DE7 )
		{
			Name = "a flat ornate table";
			Weight = 10.0;
		}

		public FlatOrnateTable( Serial serial )
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
