using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x9BD, 0x9BE, 0x9D5, 0x9D4 )]
	public class Silverware : Item
	{
		[Constructable]
		public Silverware()
			: base( 0x9BD )
		{
			this.Weight = 5.0;
		}

		public Silverware( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}