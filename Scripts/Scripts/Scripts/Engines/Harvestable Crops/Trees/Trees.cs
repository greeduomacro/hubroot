using System;
using Server;

namespace Server.Engines.Crops
{
	public class AppleTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Apple ); } }
		public override int TrunkID { get { return 0xD94; } }

		public AppleTree( Mobile sower )
			: base( sower, 0xD96 )
		{
			Name = "an apple tree";
		}

		public AppleTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xD95;

			base.OnHarvest( from );
		}

		public override void ResetYieldingFruit()
		{
			this.ItemID = 0xD96;

			base.ResetYieldingFruit();
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

	public class BananaTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Banana ); } }
		public override int TrunkID { get { return 0; } }

		public BananaTree( Mobile sower )
			: base( sower, 0xCAA )
		{
			Name = "a banana tree";
		}

		public BananaTree( Serial serial )
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

	public class CoconutPalmTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Coconut ); } }
		public override int TrunkID { get { return 0; } }

		public CoconutPalmTree( Mobile sower )
			: base( sower, 0xC95 )
		{
			Name = "a coconut tree";
		}

		public CoconutPalmTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xD95;

			base.OnHarvest( from );
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

	public class DatePalmTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Dates ); } }
		public override int TrunkID { get { return 0; } }

		public DatePalmTree( Mobile sower )
			: base( sower, 0xC96 )
		{
			Name = "a date tree";
		}

		public DatePalmTree( Serial serial )
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

	public class LemonTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Lemon ); } }
		public override int TrunkID { get { return 0xD9C; } }

		public LemonTree( Mobile sower )
			: base( sower, 0xD9E )
		{
			Name = "a lemon tree";
		}

		public LemonTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xD9D;

			base.OnHarvest( from );
		}

		public override void ResetYieldingFruit()
		{
			this.ItemID = 0xD9E;

			base.ResetYieldingFruit();
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

	public class LimeTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Lime ); } }
		public override int TrunkID { get { return 0xDA4; } }

		public LimeTree( Mobile sower )
			: base( sower, 0xDA6 )
		{
			Name = "a lime tree";
		}

		public LimeTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xDA5;

			base.OnHarvest( from );
		}

		public override void ResetYieldingFruit()
		{
			this.ItemID = 0xDA6;

			base.ResetYieldingFruit();
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

	public class PeachTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Peach ); } }
		public override int TrunkID { get { return 0xDA0; } }

		public PeachTree( Mobile sower )
			: base( sower, 0xDA2 )
		{
			Name = "a peach tree";
		}

		public PeachTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xDA1;

			base.OnHarvest( from );
		}

		public override void ResetYieldingFruit()
		{
			this.ItemID = 0xDA2;

			base.ResetYieldingFruit();
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

	public class PearTree : HarvestableTree
	{
		public override Type CropType { get { return typeof( Server.Items.Pear ); } }
		public override int TrunkID { get { return 0xDA8; } }

		public PearTree( Mobile sower )
			: base( sower, 0xDAA )
		{
			Name = "a pear tree";
		}

		public PearTree( Serial serial )
			: base( serial )
		{
		}

		public override void OnHarvest( Mobile from )
		{
			this.ItemID = 0xDA9;

			base.OnHarvest( from );
		}

		public override void ResetYieldingFruit()
		{
			this.ItemID = 0xDAA;

			base.ResetYieldingFruit();
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