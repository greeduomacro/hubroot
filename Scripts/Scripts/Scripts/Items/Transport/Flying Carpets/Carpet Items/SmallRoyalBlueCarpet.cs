using System;
using Server;

namespace Server.Transport
{
	public class SmallRoyalBlueCarpet : BaseFlyingCarpet
	{
		public override int NorthId { get { return 0x218; } }
		public override int EastId { get { return 0x219; } }
		public override int SouthId { get { return 0x21A; } }
		public override int WestId { get { return 0x21B; } }

		public SmallRoyalBlueCarpet() : base() { Shadow = new Shadow( 0x223, 0x224, 0x225, 0x226 ); }

		public SmallRoyalBlueCarpet( Serial serial ) : base( serial ) { }

		public override void OnDestroy()
		{
			if( Owner != null )
				Owner.AddToBackpack( new SmallRoyalBlueCarpetRoll() );
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

	public class SmallRoyalBlueCarpetRoll : BaseCarpetRoll
	{
		protected override BaseFlyingCarpet Carpet { get { return new SmallRoyalBlueCarpet(); } }

		[Constructable]
		public SmallRoyalBlueCarpetRoll() : base( 0x218, Point3D.Zero ) { }

		public SmallRoyalBlueCarpetRoll( Serial serial ) : base( serial ) { }

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