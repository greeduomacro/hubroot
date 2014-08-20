using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LeatherBelt : Item
	{
		[Constructable]
		public LeatherBelt()
			: this( 0 )
		{
		}

		[Constructable]
		public LeatherBelt( int hue )
			: base( 0x2790 )
		{
			Hue = hue;
			Layer = Layer.Waist;
			Name = "a leather belt";
			Weight = 0.5;
		}

		public LeatherBelt( Serial serial )
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