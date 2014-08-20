using System;
using Server.Network;
using Server.Multis;

namespace Server.Items
{
	public class BloodBasin : Item
	{
		[Constructable]
		public BloodBasin()
			: base( 0xE23 )
		{
			Movable = true;
			Weight = 1.0;
		}

		public BloodBasin( Serial serial )
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