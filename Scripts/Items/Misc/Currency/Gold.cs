using System;
using Server;

namespace Server.Currency
{
	public class Gold : BaseCoin
	{
		public override CurrencyType CurrencyType { get { return CurrencyType.Gold; } }

		[Constructable]
		public Gold()
			: this( 1 )
		{
		}

		[Constructable]
		public Gold( int amountFrom, int amountTo )
			: this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Gold( int amount )
			: base( 3824 )
		{
            Hue = 2213;
			Amount = amount;
            Name = "gold coin";

            if(amount > 1)
            Name = "gold coins";
		}

		public Gold( Serial serial )
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