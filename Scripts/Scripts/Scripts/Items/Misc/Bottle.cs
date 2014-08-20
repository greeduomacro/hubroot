using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class Bottle : Item
	{
		[Constructable]
		public Bottle()
			: this( 1 )
		{
		}

		[Constructable]
		public Bottle( int amount )
			: base( 0xF0E )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public Bottle( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "What would you like to fill this with?" );
			from.Target = new InternalTarget( this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		private class InternalTarget : Target
		{
			private Bottle _bottle;

			public InternalTarget( Bottle bottle )
				: base( 4, false, TargetFlags.None )
			{
				_bottle = bottle;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				from.SendMessage( "You probably can\'t fit that into a bottle!" );
			}
		}
	}
}