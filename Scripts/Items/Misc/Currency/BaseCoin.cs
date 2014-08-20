using System;
using Server;

namespace Server.Currency
{
	public abstract class BaseCoin : Item
	{
		private CoinMint _mint;

		public CoinMint Mint { get { return _mint; } set { _mint = value; } }

		public abstract CurrencyType CurrencyType { get; }

		public BaseCoin() : this( 0xEED ) { }

		public BaseCoin( int itemID )
			: base( itemID )
		{
			_mint = new CoinMint( CoinMint.Minters.Unknown, "", "" );
			Stackable = true;
		}

		public BaseCoin( Serial serial ) : base( serial ) { }

		public override double DefaultWeight { get { return 0.02; } }

		public override int GetDropSound()
		{
			if( Amount <= 1 )
				return 0x2E4;
			else if( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
		}

		public override int GetTotal( TotalType type )
		{
			int baseTotal = base.GetTotal( type );

			if( type == TotalType.Gold )
				baseTotal += CurrencySystem.ConvertTo( CurrencyType, CurrencyType.Gold, this.Amount );

			return baseTotal;
		}

		protected override void OnAmountChange( int oldValue )
		{
			oldValue = CurrencySystem.ConvertTo( CurrencyType, CurrencyType.Gold, oldValue );
			int newValue = CurrencySystem.ConvertTo( CurrencyType, CurrencyType.Gold, this.Amount );

			UpdateTotal( this, TotalType.Gold, newValue - oldValue );
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