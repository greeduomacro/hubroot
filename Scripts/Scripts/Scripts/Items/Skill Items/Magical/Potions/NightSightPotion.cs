using System;
using Server;

namespace Server.Items
{
	public class NightSightPotion : BasePotion
	{
		[Constructable]
		public NightSightPotion() : base( 0xF06, PotionEffect.Nightsight )
		{
		}

		public NightSightPotion( Serial serial ) : base( serial )
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

		public override void Drink( Mobile from )
		{
			if ( from.BeginAction( typeof( LightCycle ) ) )
			{
				new LightCycle.NightSightTimer( from ).Start();

				if( Server.Utilities.StartupSettings.LightingChanged )
					from.LightLevel = 15;
				else if( from.Map != Map.Trammel )
					from.LightLevel = LightCycle.DungeonLevel / 2;
				else
					from.LightLevel = 6;

				from.FixedParticles( 0x376A, 9, 32, 5007, EffectLayer.Waist );
				from.PlaySound( 0x1E3 );

				BasePotion.PlayDrinkEffect( from );

				this.Consume();
			}
			else
			{
				from.SendMessage( "You already have nightsight." );
			}
		}
	}
}
