using System;
using Server;

namespace Server.Items
{
	public class Spine : Item
	{
		[Constructable]
		public Spine() : base( 0x1B1B )
		{
		}

		public Spine( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}