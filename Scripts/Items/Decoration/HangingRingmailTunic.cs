using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x13E7, 0x13E8 )]
	public class HangingRingmailTunic : Item
	{
		[Constructable]
		public HangingRingmailTunic()
			: base( 0x13E7 )
		{
		}

		public HangingRingmailTunic( Serial serial )
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