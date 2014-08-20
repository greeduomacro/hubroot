using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x151A, 0x1512 )]
	public class StandingArmor : Item
	{
		[Constructable]
		public StandingArmor()
			: base( 0x1512 )
		{
			Name = "decorative armor";
			Weight = 20.0;
		}

		public StandingArmor( Serial serial )
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