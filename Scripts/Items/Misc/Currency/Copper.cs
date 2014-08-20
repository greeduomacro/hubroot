using System;
using Server;

namespace Server.Currency
{
	public class Copper : BaseCoin
	{
		public override CurrencyType CurrencyType { get { return CurrencyType.Copper; } }

		[Constructable]
		public Copper()
			: this( 1 )
		{
		}

		[Constructable]
		public Copper( int amountFrom, int amountTo )
			: this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Copper( int amount )
			: base( 3824 )
		{
			Amount = amount;
			Hue = 2413;
			Name = "copper coin";

            if (amount > 1)
                Name = "copper coins";
		}

		public Copper( Serial serial )
			: base( serial )
		{
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
	}
}