using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x1569, 0x156A )]
	public class WeaponRack : Item
	{
		[Constructable]
		public WeaponRack()
			: base( 0x1569 )
		{
			Name = "a rack of weapons";
			Weight = 10.0;
		}

		public WeaponRack( Serial serial )
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