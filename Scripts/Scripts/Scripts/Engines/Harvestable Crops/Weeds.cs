using System;
using Server;
using Server.Items;

namespace Server.Engines.Crops
{
	public class Weeds : Item
	{
		public Weeds()
			: base( Utility.Random( 0xCB0, 5 ) )
		{
			Movable = false;
			Name = "weeds";

			Timer.DelayCall( TimeSpan.FromHours( 1.0 ), new TimerCallback( Delete ) );
		}

		public Weeds( Serial serial )
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

			Timer.DelayCall( TimeSpan.FromMinutes( 2.0 ), new TimerCallback( Delete ) );
		}
	}
}