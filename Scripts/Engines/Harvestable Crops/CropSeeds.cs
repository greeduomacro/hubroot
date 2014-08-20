using System;
using Server;

namespace Server.Engines.Crops
{
	public class CabbageSeed : BaseCropSeed
	{
		[Constructable]
		public CabbageSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CabbageSeed( int amount )
			: base( 0xC5E, typeof( CabbageCrop ) )
		{
			this.Amount = amount;
			Name = "cabbage seed";
		}

		public CabbageSeed( Serial serial )
			: base( serial )
		{
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

	public class CarrotSeed : BaseCropSeed
	{
		[Constructable]
		public CarrotSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CarrotSeed( int amount )
			: base( 0xC68, typeof( CarrotCrop ) )
		{
			this.Amount = amount;
			Name = "carrot seed";
		}

		public CarrotSeed( Serial serial )
			: base( serial )
		{
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

	public class CantaloupeSeed : BaseCropSeed
	{
		[Constructable]
		public CantaloupeSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CantaloupeSeed( int amount )
			: base( 0xC5E, typeof( CantaloupeCrop ) )
		{
			this.Amount = amount;
			Name = "canteloupe seed";
		}

		public CantaloupeSeed( Serial serial )
			: base( serial )
		{
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

	public class CornSeed : BaseCropSeed
	{
		[Constructable]
		public CornSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CornSeed( int amount )
			: base( 0xC7E, typeof( CornCrop ) )
		{
			this.Amount = amount;
			Name = "corn seed";
		}

		public CornSeed( Serial serial )
			: base( serial )
		{
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

	public class CottonSeed : BaseCropSeed
	{
		private static int[] seedlingIDs = new int[]
			{
				0xC51, 0xC52, 0xC53, 0xC54
			};

		[Constructable]
		public CottonSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public CottonSeed( int amount )
			: base( Utility.RandomList( seedlingIDs ), typeof( CottonCrop ) )
		{
			this.Amount = amount;
			Name = "cotton seed";
		}

		public CottonSeed( Serial serial )
			: base( serial )
		{
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

	public class FlaxSeed : BaseCropSeed
	{
		[Constructable]
		public FlaxSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public FlaxSeed( int amount )
			: base( 0x1A99, typeof( FlaxCrop ) )
		{
			this.Amount = amount;
			Name = "flax seed";
		}

		public FlaxSeed( Serial serial )
			: base( serial )
		{
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

	public class GourdSeed : BaseCropSeed
	{
		[Constructable]
		public GourdSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public GourdSeed( int amount )
			: base( 0xC60, typeof( GourdCrop ) )
		{
			this.Amount = amount;
			Name = "gourd seed";
		}

		public GourdSeed( Serial serial )
			: base( serial )
		{
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

	public class HoneydewMelonSeed : BaseCropSeed
	{
		[Constructable]
		public HoneydewMelonSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public HoneydewMelonSeed( int amount )
			: base( 0xC5E, typeof( HoneydewMelonCrop ) )
		{
			this.Amount = amount;
			Name = "honeydew melon seed";
		}

		public HoneydewMelonSeed( Serial serial )
			: base( serial )
		{
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

	public class LettuceSeed : BaseCropSeed
	{
		[Constructable]
		public LettuceSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public LettuceSeed( int amount )
			: base( 0xC5E, typeof( LettuceCrop ) )
		{
			this.Amount = amount;
			Name = "lettuce seed";
		}

		public LettuceSeed( Serial serial )
			: base( serial )
		{
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

	public class OnionSeed : BaseCropSeed
	{
		[Constructable]
		public OnionSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public OnionSeed( int amount )
			: base( 0xC68, typeof( OnionCrop ) )
		{
			this.Amount = amount;
			Name = "onion seed";
		}

		public OnionSeed( Serial serial )
			: base( serial )
		{
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

	public class PotatoSeed : BaseCropSeed
	{
		[Constructable]
		public PotatoSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public PotatoSeed( int amount )
			: base( 0xC68, typeof( PotatoCrop ) )
		{
			this.Amount = amount;
			Name = "potato seed";
		}

		public PotatoSeed( Serial serial )
			: base( serial )
		{
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

	public class PumpkinSeed : BaseCropSeed
	{
		[Constructable]
		public PumpkinSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public PumpkinSeed( int amount )
			: base( 0xC6C, typeof( PumpkinCrop ) )
		{
			this.Amount = amount;
			Name = "pumpkin seed";
		}

		public PumpkinSeed( Serial serial )
			: base( serial )
		{
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

	public class TomatoSeed : BaseCropSeed
	{
		[Constructable]
		public TomatoSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public TomatoSeed( int amount )
			: base( 0xC60, typeof( TomatoCrop ) )
		{
			this.Amount = amount;
			Name = "tomato seed";
		}

		public TomatoSeed( Serial serial )
			: base( serial )
		{
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

	public class RoseBud : BaseCropSeed
	{
		[Constructable]
		public RoseBud()
			: this( 1 )
		{
		}

		[Constructable]
		public RoseBud( int amount )
			: base( 0x18E0, typeof( RosePlant ) )
		{
			this.Amount = amount;
			Name = "rose bud";
		}

		public RoseBud( Serial serial )
			: base( serial )
		{
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

	public class SquashSeed : BaseCropSeed
	{
		[Constructable]
		public SquashSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public SquashSeed( int amount )
			: base( 0xC5E, typeof( SquashCrop ) )
		{
			this.Amount = amount;
			Name = "squash seed";
		}

		public SquashSeed( Serial serial )
			: base( serial )
		{
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

	public class TurnipSeed : BaseCropSeed
	{
		[Constructable]
		public TurnipSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public TurnipSeed( int amount )
			: base( 0xC61, typeof( TurnipCrop ) )
		{
			this.Amount = amount;
			Name = "turnip seed";
		}

		public TurnipSeed( Serial serial )
			: base( serial )
		{
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

	public class WatermelonSeed : BaseCropSeed
	{
		[Constructable]
		public WatermelonSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public WatermelonSeed( int amount )
			: base( 0xC5E, typeof( WatermelonCrop ) )
		{
			this.Amount = amount;
			Name = "watermelon seed";
		}

		public WatermelonSeed( Serial serial )
			: base( serial )
		{
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

	public class WheatSeed : BaseCropSeed
	{
		private static int[] seedlingIDs = new int[]
			{
				0xC55, 0xC56, 0xC57, 0xC58, 0xC59
			};

		[Constructable]
		public WheatSeed()
			: this( 1 )
		{
		}

		[Constructable]
		public WheatSeed( int amount )
			: base( Utility.RandomList( seedlingIDs ), typeof( WheatCrop ) )
		{
			this.Amount = amount;
			Name = "wheat seed";
		}

		public WheatSeed( Serial serial )
			: base( serial )
		{
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