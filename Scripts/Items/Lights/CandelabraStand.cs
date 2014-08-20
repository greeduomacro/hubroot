using System;
using Server;

namespace Server.Items
{
	public class CandelabraStand : BaseLight
	{
		public override int LitItemID { get { return 0xB26; } }
		public override int UnlitItemID { get { return 0xA29; } }

		public TimeSpan duration = TimeSpan.FromHours( 3.0 );

		[Constructable]
		public CandelabraStand()
			: base( 0xA29 )
		{
			if( Burnout )
				Duration = duration;
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Circle225;
			Weight = 20.0;
		}

		public CandelabraStand( Serial serial )
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
