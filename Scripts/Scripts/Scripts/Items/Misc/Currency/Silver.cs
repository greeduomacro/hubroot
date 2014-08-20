using System;
using Server;

namespace Server.Currency
{
	public class Silver : BaseCoin
	{
		public override CurrencyType CurrencyType { get { return CurrencyType.Silver; } }

		[Constructable]
		public Silver()
			: this( 1 )
		{
		}

		[Constructable]
		public Silver( int amountFrom, int amountTo )
			: this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public Silver( int amount )
			: base( 0xEF0 )
		{
			Amount = amount;
		}

		public Silver( Serial serial )
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