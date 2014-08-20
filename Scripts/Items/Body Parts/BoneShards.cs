using System;
using Server;

namespace Server.Items
{
	public class BoneShards : Item
	{
		[Constructable]
		public BoneShards() : base( 0x1B19 )
		{
		}

		public BoneShards( Serial serial ) : base( serial )
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