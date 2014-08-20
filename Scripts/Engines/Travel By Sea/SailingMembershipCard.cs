using System;
using Server;
using Server.Items;

namespace Khazman.TravelBySea
{
	public class SailingMembershipCard : Item
	{
		[Constructable]
		public SailingMembershipCard() : base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 1154;
			Name = "Sailing Membership Card";
		}

		public SailingMembershipCard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}