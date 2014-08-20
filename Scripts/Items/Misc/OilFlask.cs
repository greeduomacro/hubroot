using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class OilFlask : Item
	{
		[Constructable]
		public OilFlask()
			: base( 0x1C18 )
		{
			Name = "an oil flask";
			Weight = 0.5;
		}

		public OilFlask( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "Select the lantern you wish to fill." );
				from.Target = new InternalTarget( this );
			}
			else
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
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
			private OilFlask m_Flask;

			public InternalTarget( OilFlask flask )
				: base( 12, false, TargetFlags.None )
			{
				m_Flask = flask;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is Lantern || target is HangingLantern )
				{
					BaseLight light = (BaseLight)target;

					light.HasFuel = true;
					light.BurntOut = false;
					SetDuration( light );

					from.SendMessage( "You fill the lantern with oil." );
					m_Flask.Delete();
				}
				else
					from.SendMessage( "Target a lantern." );
			}

			private static void SetDuration( BaseLight light )
			{
				if( light is Lantern )
					light.Duration = ((Lantern)light).duration;
				else if( light is HangingLantern )
					light.Duration = ((HangingLantern)light).duration;
			}
		}
	}
}
