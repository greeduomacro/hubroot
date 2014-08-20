using System;
using Server;

namespace Server.Engines.Crops
{
	public class AppleTreeSeed : BaseCropSeed
	{
		[Constructable]
		public AppleTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public AppleTreeSeed( int amount )
			: base( 0x913, typeof( AppleSapling ) )
		{
			this.Amount = amount;
			Name = "apple seed";
		}

		public AppleTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class BananaTreeSeed : BaseCropSeed
	{
		[Constructable]
		public BananaTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public BananaTreeSeed( int amount )
			: base( 0x913, typeof( BananaSapling ) )
		{
			this.Amount = amount;
			Name = "banana seed";
		}

		public BananaTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class CoconutPalmTreeSeed : BaseCropSeed
	{
		[Constructable]
		public CoconutPalmTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CoconutPalmTreeSeed( int amount )
			: base( 0x913, typeof( CoconutPalmSapling ) )
		{
			this.Amount = amount;
			Name = "coconut seed";
		}

		public CoconutPalmTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class DatePalmTreeSeed : BaseCropSeed
	{
		[Constructable]
		public DatePalmTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public DatePalmTreeSeed( int amount )
			: base( 0x913, typeof( DatePalmSapling ) )
		{
			this.Amount = amount;
			Name = "date seed";
		}

		public DatePalmTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class LemonTreeSeed : BaseCropSeed
	{
		[Constructable]
		public LemonTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public LemonTreeSeed( int amount )
			: base( 0x913, typeof( LemonSapling ) )
		{
			this.Amount = amount;
			Name = "lemon seed";
		}

		public LemonTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class LimeTreeSeed : BaseCropSeed
	{
		[Constructable]
		public LimeTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public LimeTreeSeed( int amount )
			: base( 0x913, typeof( LimeSapling ) )
		{
			this.Amount = amount;
			Name = "lime seed";
		}

		public LimeTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class PeachTreeSeed : BaseCropSeed
	{
		[Constructable]
		public PeachTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public PeachTreeSeed( int amount )
			: base( 0x913, typeof( PeachSapling ) )
		{
			this.Amount = amount;
			Name = "peach seed";
		}

		public PeachTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}

	public class PearTreeSeed : BaseCropSeed
	{
		[Constructable]
		public PearTreeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public PearTreeSeed( int amount )
			: base( 0x913, typeof( PearSapling ) )
		{
			this.Amount = amount;
			Name = "pear seed";
		}

		public PearTreeSeed( Serial serial )
			: base( serial )
		{
		}

		public override bool CheckPlantSeed( Point3D location, Map map, int range )
		{
			return base.CheckPlantSeed( location, map, 1 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}