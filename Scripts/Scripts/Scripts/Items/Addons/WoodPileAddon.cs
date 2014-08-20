using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WoodPileAddon : BaseAddon
	{
		[Constructable]
		public WoodPileAddon()
		{
			AddComponent( new AddonComponent( 7046 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 7042 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 7043 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 7050 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 7058 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 7041 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 7044 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 7049 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 7053 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 7052 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 7046 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 7047 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 7055 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 7054 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 7051 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 7059 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 7055 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 7048 ), -1, -2, 2 );
			AddComponent( new AddonComponent( 7066 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 7054 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 7045 ), -2, 1, 0 );

		}

		public WoodPileAddon( Serial serial )
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
