using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class Candle : BaseEquipableLight
	{
		public override int LitItemID { get { return 0xA0F; } }
		public override int UnlitItemID { get { return 0xA28; } }

		public TimeSpan duration = TimeSpan.FromMinutes( 60.0 );

		[Constructable]
		public Candle()
			: base( 0xA28 )
		{
			if( Burnout )
				Duration = duration;
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Circle150;
			Weight = 1.0;
		}

		public Candle( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "Where would you like to put this candle?" );
				from.Target = new InternalTarget( this );
			}
			else
				base.OnDoubleClick( from );
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

		private class InternalTarget : Target
		{
			private Candle m_Candle;

			public InternalTarget( Candle candle )
				: base( 2, false, TargetFlags.None )
			{
				m_Candle = candle;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is BaseLight )
				{
					BaseLight light = (BaseLight)target;

					if( light is Candelabra || light is CandelabraStand || light is CandleSkull || light is HeatingStand || light is WallSconce )
					{
						light.HasFuel = true;
						light.BurntOut = false;

						SetDuration( light );

						from.SendMessage( "You mount the candle in the stand." );
						m_Candle.Delete();
					}
					else
						from.SendMessage( "This will not work with that." );
				}
				else if( target is Mobile && ((Mobile)target) == from )
				{
					m_Candle.Ignite();
				}
				else
					from.SendMessage( "This will not work with that." );
			}

			private static void SetDuration( BaseLight light )
			{
				if( light is Candelabra )
					light.Duration = ((Candelabra)light).duration;
				else if( light is CandelabraStand )
					light.Duration = ((CandelabraStand)light).duration;
				else if( light is CandleSkull )
					light.Duration = ((CandleSkull)light).duration;
				else if( light is HeatingStand )
					light.Duration = ((HeatingStand)light).duration;
				else if( light is WallSconce )
					light.Duration = ((WallSconce)light).duration;
			}
		}
	}
}
