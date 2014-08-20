using System;
using Server;
using Server.Items;

namespace Khazman.TravelBySea
{
	public class SailBoatWestEast : BaseAddon
	{
		[Constructable]
		public SailBoatWestEast()
		{
			AddComponent( new AddonComponent( 15977 ), 6, 0, 0 );
			AddComponent( new AddonComponent( 15975 ), 5, -1, 0 );
			AddComponent( new AddonComponent( 15994 ), 5, 0, 0 );
			AddComponent( new AddonComponent( 15996 ), 4, 1, 0 );
			AddComponent( new AddonComponent( 15998 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 15997 ), 4, -1, 0 );
			AddComponent( new AddonComponent( 15999 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 15993 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 16005 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16008 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16010 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 3, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 4, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16012 ), -3, 1, 0 );
			AddComponent( new AddonComponent( 16013 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 16014 ), -4, 1, 0 );
			AddComponent( new AddonComponent( 16015 ), -4, 0, 0 );
			AddComponent( new AddonComponent( 16016 ), -4, -1, 0 );
			AddComponent( new AddonComponent( 16017 ), -5, 1, 0 );
			AddComponent( new AddonComponent( 16018 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 16020 ), -5, -1, 0 );
			AddComponent( new AddonComponent( 16011 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 15971 ), -6, 0, 0 );
			AddComponent( new AddonComponent( 15957 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 15962 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 15963 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 15964 ), 3, -2, 0 );
			AddComponent( new AddonComponent( 15961 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16009 ), -1, -2, 0 );
		}

		public SailBoatWestEast( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatWestEast2 : BaseAddon
	{
		[Constructable]
		public SailBoatWestEast2()
		{
			AddComponent( new AddonComponent( 15977 ), 6, 0, 0 );
			AddComponent( new AddonComponent( 15975 ), 5, -1, 0 );
			AddComponent( new AddonComponent( 15994 ), 5, 0, 0 );
			AddComponent( new AddonComponent( 15996 ), 4, 1, 0 );
			AddComponent( new AddonComponent( 15998 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 15997 ), 4, -1, 0 );
			AddComponent( new AddonComponent( 15999 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 15993 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 16005 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16008 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16010 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 3, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 4, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16012 ), -3, 1, 0 );
			AddComponent( new AddonComponent( 16013 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 16014 ), -4, 1, 0 );
			AddComponent( new AddonComponent( 16015 ), -4, 0, 0 );
			AddComponent( new AddonComponent( 16016 ), -4, -1, 0 );
			AddComponent( new AddonComponent( 16017 ), -5, 1, 0 );
			AddComponent( new AddonComponent( 16018 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 16020 ), -5, -1, 0 );
			AddComponent( new AddonComponent( 16011 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 15971 ), -6, 0, 0 );
			AddComponent( new AddonComponent( 15957 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 15962 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 15963 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 15964 ), 3, -2, 0 );
			AddComponent( new AddonComponent( 15961 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16010 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16004 ), -1, 2, 0 );
		}

		public SailBoatWestEast2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatSouthNorth : BaseAddon
	{
		[Constructable]
		public SailBoatSouthNorth()
		{
			AddComponent( new AddonComponent( 16084 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16095 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16094 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16092 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16093 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16024 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -3, 0 );
			AddComponent( new AddonComponent( 16045 ), 0, -4, 0 );
			AddComponent( new AddonComponent( 16026 ), 0, -6, 0 );
			AddComponent( new AddonComponent( 16027 ), -1, -5, 0 );
			AddComponent( new AddonComponent( 16028 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 16029 ), -1, -4, 0 );
			AddComponent( new AddonComponent( 16030 ), 1, -4, 0 );
			AddComponent( new AddonComponent( 16031 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 16032 ), 1, -3, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16042 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 15950 ), 1, 5, 0 );
			AddComponent( new AddonComponent( 16054 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16037 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 16038 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 16039 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 16040 ), 1, 4, 0 );
			AddComponent( new AddonComponent( 16060 ), 0, 6, 0 );
		}

		public SailBoatSouthNorth( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatSouthNorth2 : BaseAddon
	{
		[Constructable]
		public SailBoatSouthNorth2()
		{
			AddComponent( new AddonComponent( 16085 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16095 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16094 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16092 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16093 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16024 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -3, 0 );
			AddComponent( new AddonComponent( 16045 ), 0, -4, 0 );
			AddComponent( new AddonComponent( 16026 ), 0, -6, 0 );
			AddComponent( new AddonComponent( 16027 ), -1, -5, 0 );
			AddComponent( new AddonComponent( 16028 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 16029 ), -1, -4, 0 );
			AddComponent( new AddonComponent( 16030 ), 1, -4, 0 );
			AddComponent( new AddonComponent( 16031 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 16032 ), 1, -3, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16042 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 15950 ), 1, 5, 0 );
			AddComponent( new AddonComponent( 16054 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 16048 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16037 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 16038 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 16039 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 16040 ), 1, 4, 0 );
			AddComponent( new AddonComponent( 16060 ), 0, 6, 0 );
		}

		public SailBoatSouthNorth2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatNorthSouth : BaseAddon
	{
		[Constructable]
		public SailBoatNorthSouth()
		{
			AddComponent( new AddonComponent( 16033 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16032 ), 1, -3, 0 );
			AddComponent( new AddonComponent( 16031 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 16030 ), 1, -4, 0 );
			AddComponent( new AddonComponent( 16029 ), -1, -4, 0 );
			AddComponent( new AddonComponent( 16028 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 16027 ), -1, -5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -3, 0 );
			AddComponent( new AddonComponent( 16045 ), 0, -4, 0 );
			AddComponent( new AddonComponent( 16047 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16068 ), 0, -6, 0 );
			AddComponent( new AddonComponent( 15947 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16098 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16097 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16099 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16084 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16052 ), 0, 6, 0 );
			AddComponent( new AddonComponent( 16053 ), 1, 5, 0 );
			AddComponent( new AddonComponent( 16054 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 16055 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 16040 ), 1, 4, 0 );
			AddComponent( new AddonComponent( 16039 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 16038 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 16037 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 16042 ), 0, 5, 3 );
			AddComponent( new AddonComponent( 16044 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 16096 ), -2, 3, 0 );
		}

		public SailBoatNorthSouth( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatNorthSouth2 : BaseAddon
	{
		[Constructable]
		public SailBoatNorthSouth2()
		{
			AddComponent( new AddonComponent( 16033 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16033 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16049 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16044 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16032 ), 1, -3, 0 );
			AddComponent( new AddonComponent( 16031 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 16030 ), 1, -4, 0 );
			AddComponent( new AddonComponent( 16029 ), -1, -4, 0 );
			AddComponent( new AddonComponent( 16028 ), 1, -5, 0 );
			AddComponent( new AddonComponent( 16027 ), -1, -5, 0 );
			AddComponent( new AddonComponent( 16048 ), 0, -3, 0 );
			AddComponent( new AddonComponent( 16045 ), 0, -4, 0 );
			AddComponent( new AddonComponent( 16047 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16068 ), 0, -6, 0 );
			AddComponent( new AddonComponent( 15947 ), 0, -5, 0 );
			AddComponent( new AddonComponent( 16098 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16050 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 16097 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16099 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16085 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16052 ), 0, 6, 0 );
			AddComponent( new AddonComponent( 16053 ), 1, 5, 0 );
			AddComponent( new AddonComponent( 16054 ), -1, 5, 0 );
			AddComponent( new AddonComponent( 16055 ), 0, 5, 0 );
			AddComponent( new AddonComponent( 16040 ), 1, 4, 0 );
			AddComponent( new AddonComponent( 16039 ), -1, 4, 0 );
			AddComponent( new AddonComponent( 16038 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 16037 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 16042 ), 0, 5, 3 );
			AddComponent( new AddonComponent( 16044 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 16044 ), 0, 4, 0 );
			AddComponent( new AddonComponent( 16096 ), -2, 3, 0 );
		}

		public SailBoatNorthSouth2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatEastWest : BaseAddon
	{
		[Constructable]
		public SailBoatEastWest()
		{
			AddComponent( new AddonComponent( 16021 ), -6, 0, 0 );
			AddComponent( new AddonComponent( 16020 ), -5, -1, 0 );
			AddComponent( new AddonComponent( 16018 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 16017 ), -5, 1, 0 );
			AddComponent( new AddonComponent( 16016 ), -4, -1, 0 );
			AddComponent( new AddonComponent( 16015 ), -4, 0, 0 );
			AddComponent( new AddonComponent( 16014 ), -4, 1, 0 );
			AddComponent( new AddonComponent( 16013 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 16012 ), -3, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -5, 0, 3 );
			AddComponent( new AddonComponent( 16007 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16008 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16010 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16005 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 15999 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 15998 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 15997 ), 4, -1, 0 );
			AddComponent( new AddonComponent( 15996 ), 4, 1, 0 );
			AddComponent( new AddonComponent( 15995 ), 5, -1, 0 );
			AddComponent( new AddonComponent( 15993 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 15990 ), 6, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 3, 0, 0 );
			AddComponent( new AddonComponent( 15952 ), 5, 0, 0 );
			AddComponent( new AddonComponent( 15991 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 4, 0, 0 );
			AddComponent( new AddonComponent( 15980 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 15979 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 15978 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 15981 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16004 ), 1, 2, 0 );
		}

		public SailBoatEastWest( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SailBoatEastWest2 : BaseAddon
	{
		[Constructable]
		public SailBoatEastWest2()
		{
			AddComponent( new AddonComponent( 16021 ), -6, 0, 0 );
			AddComponent( new AddonComponent( 16020 ), -5, -1, 0 );
			AddComponent( new AddonComponent( 16018 ), -5, 0, 0 );
			AddComponent( new AddonComponent( 16017 ), -5, 1, 0 );
			AddComponent( new AddonComponent( 16016 ), -4, -1, 0 );
			AddComponent( new AddonComponent( 16015 ), -4, 0, 0 );
			AddComponent( new AddonComponent( 16014 ), -4, 1, 0 );
			AddComponent( new AddonComponent( 16013 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 16012 ), -3, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -5, 0, 3 );
			AddComponent( new AddonComponent( 16007 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 16008 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 16008 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 16010 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 16010 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 16005 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 16005 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 15999 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 15998 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 15997 ), 4, -1, 0 );
			AddComponent( new AddonComponent( 15996 ), 4, 1, 0 );
			AddComponent( new AddonComponent( 15995 ), 5, -1, 0 );
			AddComponent( new AddonComponent( 15993 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 15990 ), 6, 0, 0 );
			AddComponent( new AddonComponent( 16011 ), 3, 0, 0 );
			AddComponent( new AddonComponent( 15952 ), 5, 0, 0 );
			AddComponent( new AddonComponent( 15991 ), 5, 1, 0 );
			AddComponent( new AddonComponent( 16011 ), 4, 0, 0 );
			AddComponent( new AddonComponent( 15980 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 15979 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 15978 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 15981 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 16009 ), 1, -2, 0 );
		}

		public SailBoatEastWest2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}