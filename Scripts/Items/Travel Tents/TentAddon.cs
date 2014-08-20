using System;
using Server;

namespace Server.Items
{
	public class TentAddon : BaseAddon
	{
		[Constructable]
		public TentAddon()
		{
			AddComponent( new AddonComponent( 2729 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 1536 ), 0, -1, 19 );
			AddComponent( new AddonComponent( 2729 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 1544 ), 0, 0, 27 );
			AddComponent( new AddonComponent( 2729 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 1538 ), 1, 0, 19 );
			AddComponent( new AddonComponent( 2729 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 1541 ), 1, -1, 19 );
			AddComponent( new AddonComponent( 2730 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 1536 ), 1, -2, 16 );
			AddComponent( new AddonComponent( 502 ), 1, -3, 0 );
			AddComponent( new AddonComponent( 1548 ), 1, -3, 13 );
			AddComponent( new AddonComponent( 2730 ), -1, -2, 0 );
			AddComponent( new AddonComponent( 1536 ), -1, -2, 16 );
			AddComponent( new AddonComponent( 498 ), 3, 2, 0 );
			AddComponent( new AddonComponent( 1549 ), 3, 2, 13 );
			AddComponent( new AddonComponent( 498 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 1549 ), 3, 1, 13 );
			AddComponent( new AddonComponent( 2731 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 1538 ), 2, 1, 16 );
			AddComponent( new AddonComponent( 2736 ), 2, 2, 0 );
			AddComponent( new AddonComponent( 1540 ), 2, 2, 16 );
			AddComponent( new AddonComponent( 2729 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 1537 ), 0, 1, 19 );
			AddComponent( new AddonComponent( 2729 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 1543 ), -1, 1, 19 );
			AddComponent( new AddonComponent( 2733 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 1535 ), -2, 1, 16 );
			AddComponent( new AddonComponent( 503 ), -3, 0, 0 );
			AddComponent( new AddonComponent( 1547 ), -3, 0, 13 );
			AddComponent( new AddonComponent( 2733 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 1535 ), -2, 0, 16 );
			AddComponent( new AddonComponent( 502 ), -2, -3, 0 );
			AddComponent( new AddonComponent( 1548 ), -2, -3, 13 );
			AddComponent( new AddonComponent( 2734 ), -2, -2, 0 );
			AddComponent( new AddonComponent( 1542 ), -2, -2, 16 );
			AddComponent( new AddonComponent( 502 ), 0, -3, 0 );
			AddComponent( new AddonComponent( 1548 ), 0, -3, 13 );
			AddComponent( new AddonComponent( 503 ), -3, -1, 0 );
			AddComponent( new AddonComponent( 1547 ), -3, -1, 13 );
			AddComponent( new AddonComponent( 503 ), -3, -2, 0 );
			AddComponent( new AddonComponent( 1547 ), -3, -2, 13 );
			AddComponent( new AddonComponent( 499 ), -3, -3, 0 );
			AddComponent( new AddonComponent( 1552 ), -3, -3, 13 );
			AddComponent( new AddonComponent( 2731 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 1538 ), 2, -1, 16 );
			AddComponent( new AddonComponent( 2731 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 1538 ), 2, 0, 16 );
			AddComponent( new AddonComponent( 502 ), 2, -3, 0 );
			AddComponent( new AddonComponent( 1548 ), 2, -3, 13 );
			AddComponent( new AddonComponent( 2735 ), 2, -2, 0 );
			AddComponent( new AddonComponent( 1541 ), 2, -2, 16 );
			AddComponent( new AddonComponent( 503 ), -3, 1, 0 );
			AddComponent( new AddonComponent( 1547 ), -3, 1, 13 );
			AddComponent( new AddonComponent( 496 ), 3, 3, 0 );
			AddComponent( new AddonComponent( 1551 ), 3, 3, 13 );
			AddComponent( new AddonComponent( 2732 ), -1, 2, 0 );
			AddComponent( new AddonComponent( 1537 ), -1, 2, 16 );
			AddComponent( new AddonComponent( 2732 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 1537 ), 0, 2, 16 );
			AddComponent( new AddonComponent( 503 ), -3, 2, 0 );
			AddComponent( new AddonComponent( 1547 ), -3, 2, 13 );
			AddComponent( new AddonComponent( 2737 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 1543 ), -2, 2, 16 );
			AddComponent( new AddonComponent( 497 ), 2, 3, 0 );
			AddComponent( new AddonComponent( 1550 ), 2, 3, 13 );
			AddComponent( new AddonComponent( 497 ), 1, 3, 0 );
			AddComponent( new AddonComponent( 1550 ), 1, 3, 13 );
			AddComponent( new AddonComponent( 2732 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 1537 ), 1, 2, 16 );
			AddComponent( new AddonComponent( 2729 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 1540 ), 1, 1, 19 );
			AddComponent( new AddonComponent( 497 ), -2, 3, 0 );
			AddComponent( new AddonComponent( 1550 ), -2, 3, 13 );
			AddComponent( new AddonComponent( 500 ), -3, 3, 0 );
			AddComponent( new AddonComponent( 1554 ), -3, 3, 13 );
			AddComponent( new AddonComponent( 497 ), 0, 3, 0 );
			AddComponent( new AddonComponent( 1550 ), 0, 3, 13 );
			AddComponent( new AddonComponent( 497 ), -1, 3, 0 );
			AddComponent( new AddonComponent( 1550 ), -1, 3, 13 );
			AddComponent( new AddonComponent( 502 ), -1, -3, 0 );
			AddComponent( new AddonComponent( 1548 ), -1, -3, 13 );
			AddComponent( new AddonComponent( 2729 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 1535 ), -1, 0, 19 );
			AddComponent( new AddonComponent( 2729 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 1542 ), -1, -1, 19 );
			AddComponent( new AddonComponent( 498 ), 3, -2, 0 );
			AddComponent( new AddonComponent( 1549 ), 3, -2, 13 );
			AddComponent( new AddonComponent( 501 ), 3, -3, 0 );
			AddComponent( new AddonComponent( 1553 ), 3, -3, 13 );
			AddComponent( new AddonComponent( 1549 ), 3, 0, 13 );
			AddComponent( new AddonComponent( 498 ), 3, -1, 0 );
			AddComponent( new AddonComponent( 1549 ), 3, -1, 13 );
			AddComponent( new AddonComponent( 2733 ), -2, -1, 0 );
			AddComponent( new AddonComponent( 1535 ), -2, -1, 16 );
			AddComponent( new AddonComponent( 2730 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 1536 ), 0, -2, 16 );
			AddComponent( new AddonComponent( 2868 ), -2, -1, 0 );  //small table
			AddComponent( new AddonComponent( 2868 ), -1, 1, 0 );   //sm table, pillow 'support'
			AddComponent( new AddonComponent( 5028 ), -1, 1, 8 );   //bed pillow
			AddComponent( new AddonComponent( 2903 ), 0, -2, 0 );   //wooden chair
			AddComponent( new AddonComponent( 4604 ), 4, 2, 0 );    //bamboo stool
			AddComponent( new AddonComponent( 4604 ), 5, 3, 0 );    //second bamboo stool

			AddonComponent ac = new AddonComponent( 4012 );        //fire pit
			ac.Light = LightType.Circle150;
			AddComponent( ac, 5, 2, 0 );
		}

		public TentAddon( Serial serial )
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
