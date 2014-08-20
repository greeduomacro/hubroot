using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x1508, 0x151C )]
	public class DecoratedStandingArmor : Item
	{
		[Constructable]
		public DecoratedStandingArmor()
			: base( 0x1508 )
		{
			Name = "decorative armor";
			Weight = 20.0;
		}

		public DecoratedStandingArmor( Serial serial )
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