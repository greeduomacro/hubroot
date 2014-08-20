using System;
using Server;

namespace Server.Engines.Crops
{
	public class AppleSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( AppleTree ); } }

		public AppleSapling( Mobile sower )
			: base( sower )
		{
		}

		public AppleSapling( Serial serial )
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

	public class BananaSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( BananaTree ); } }

		public BananaSapling( Mobile sower )
			: base( sower )
		{
		}

		public BananaSapling( Serial serial )
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

	public class CoconutPalmSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( CoconutPalmTree ); } }

		public CoconutPalmSapling( Mobile sower )
			: base( sower )
		{
		}

		public CoconutPalmSapling( Serial serial )
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

	public class DatePalmSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( DatePalmTree ); } }

		public DatePalmSapling( Mobile sower )
			: base( sower )
		{
		}

		public DatePalmSapling( Serial serial )
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

	public class LemonSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( LemonTree ); } }

		public LemonSapling( Mobile sower )
			: base( sower )
		{
		}

		public LemonSapling( Serial serial )
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

	public class LimeSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( LimeTree ); } }

		public LimeSapling( Mobile sower )
			: base( sower )
		{
		}

		public LimeSapling( Serial serial )
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

	public class PeachSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( PeachTree ); } }

		public PeachSapling( Mobile sower )
			: base( sower )
		{
		}

		public PeachSapling( Serial serial )
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

	public class PearSapling : BaseTreeSapling
	{
		public override Type FullTreeType { get { return typeof( PearTree ); } }

		public PearSapling( Mobile sower )
			: base( sower )
		{
		}

		public PearSapling( Serial serial )
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