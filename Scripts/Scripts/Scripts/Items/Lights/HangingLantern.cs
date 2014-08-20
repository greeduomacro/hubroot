using System;
using Server;

namespace Server.Items
{
	public class HangingLantern : BaseLight
	{
		public override int LitItemID { get { return 0xA1A; } }
		public override int UnlitItemID { get { return 0xA1D; } }

		public TimeSpan duration = TimeSpan.FromMinutes( 60.0 );

		[Constructable]
		public HangingLantern()
			: base( 0xA1D )
		{
			if( Burnout )
				Duration = duration;
			else
				Duration = TimeSpan.Zero;

			Movable = false;
			Burning = false;
			Light = LightType.Circle300;
			Weight = 40.0;
		}

		public HangingLantern( Serial serial )
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
