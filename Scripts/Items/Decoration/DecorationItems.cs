using System;
using Server.Network;
namespace Server.Items
{

	[FlipableAttribute( 0x14F3, 0x14F4 )]
	public class ModelBoat : Item
	{
		[Constructable]
		public ModelBoat()
			: base( 0x14F3 )
		{
			Movable = true;
			Weight = 0.1;
		}

		public ModelBoat( Serial serial )
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

	[FlipableAttribute( 0xDF6, 0xDF7 )]
	public class Knitting : Item
	{
		[Constructable]
		public Knitting()
			: base( 0xDF6 )
		{
			Movable = true;
			Weight = 0.1;
		}

		public Knitting( Serial serial )
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

	public class Kettle : Item
	{
		[Constructable]
		public Kettle()
			: base( 0x9ED )
		{
			Movable = true;
			Weight = 5.0;
		}

		public Kettle( Serial serial )
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

	public class Frypan : Item
	{
		[Constructable]
		public Frypan()
			: base( 0x97F )
		{
			Movable = true;
			Weight = 1.0;
		}

		public Frypan( Serial serial )
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

	[FlipableAttribute( 0xDCC, 0xDCF, 0xDCD, 0xDCE )]
	public class FishingWeight : Item
	{
		[Constructable]
		public FishingWeight()
			: base( 0xDCC )
		{
			Movable = true;
			Weight = 0.1;
		}

		public FishingWeight( Serial serial )
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

	public class DoughBowl : Item
	{
		[Constructable]
		public DoughBowl()
			: base( 0x10E3 )
		{
			Movable = true;
			Weight = 2.0;
		}

		public DoughBowl( Serial serial )
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

	public class CrystalBall : Item
	{
		[Constructable]
		public CrystalBall()
			: base( 0xE2E )
		{
			Movable = true;
			Weight = 1.0;
			Light = LightType.Circle225;
		}

		public CrystalBall( Serial serial )
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

	public class Rose : Item
	{
		[Constructable]
		public Rose()
			: base( 0x18E9 )
		{
			Movable = true;
			Weight = 0.1;
			Name = "Long Stem Rose";
		}

		public Rose( Serial serial )
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
