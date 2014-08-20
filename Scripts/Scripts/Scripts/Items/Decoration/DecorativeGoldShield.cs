using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x156C, 0x156D )]
	public class DecorativeGoldShield : Item
	{
		[Constructable]
		public DecorativeGoldShield()
			: base( 0x156C )
		{
			Name = "decorative shield";
			Weight = 10.0;
		}

		public DecorativeGoldShield( Serial serial )
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