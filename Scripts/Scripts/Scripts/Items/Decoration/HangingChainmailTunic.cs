using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x13BD, 0x13C2 )]
	public class HangingChainmailTunic : Item
	{
		[Constructable]
		public HangingChainmailTunic()
			: base( 0x13BD )
		{
			Name = "hanging chainmail tunic";
			Weight = 8.0;
		}

		public HangingChainmailTunic( Serial serial )
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