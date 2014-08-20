using System;
using Server;
using Server.Items;

namespace Server.Barter
{
	public class BlacksmithBarterVendor : BarterVendor
	{
		[Constructable]
		public BlacksmithBarterVendor()
			: base( "the blacksmith" )
		{
			SetInterestLevel( TradeGoods.Armor, TradeGoodInterest.VeryHigh );
			SetInterestLevel( TradeGoods.Clothing, TradeGoodInterest.Moderate );
			SetInterestLevel( TradeGoods.Harvest, TradeGoodInterest.Moderate );
			SetInterestLevel( TradeGoods.Shields, TradeGoodInterest.High );
			SetInterestLevel( TradeGoods.Tools, TradeGoodInterest.Moderate );
			SetInterestLevel( TradeGoods.Weapons, TradeGoodInterest.High );

			Longsword sword;

			for( byte i = 0; i < 9; i++ )
			{
				sword = new Longsword();
				sword.Name = i + ": sword";

				BankBox.AddItem( sword );
			}
		}

		public BlacksmithBarterVendor( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (byte)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			byte version = reader.ReadByte();
		}
	}
}