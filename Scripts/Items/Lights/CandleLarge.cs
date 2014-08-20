using System;
using Server;

namespace Server.Items
{
	public class CandleLarge : BaseLight
	{
		public override int LitItemID { get { return 0xB1A; } }
		public override int UnlitItemID { get { return 0xA26; } }

		public TimeSpan duration = TimeSpan.FromMinutes( 90.0 );

		[Constructable]
		public CandleLarge()
			: base( 0xA26 )
		{
			if( Burnout )
				Duration = duration;
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Circle150;
			Weight = 2.0;
		}

		public CandleLarge( Serial serial )
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