using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x1565, 0x1566 )]
	public class SwordRack : Item
	{
		[Constructable]
		public SwordRack()
			: base( 0x1565 )
		{
			Name = "a rack of swords";
			Weight = 8.0;
		}

		public SwordRack( Serial serial )
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