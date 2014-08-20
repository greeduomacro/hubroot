using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class SBHunter : SBInfo
	{
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();
		
		public SBHunter()
		{
		}
		
		public override List<GenericBuyInfo> BuyInfo{ get{ return m_BuyInfo; } }
		public override IShopSellInfo SellInfo { get{ return m_SellInfo; } }
		
		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "kindling", typeof( Kindling ), 25, 20, 0xDE1, 0 ) );
				Add( new GenericBuyInfo( "bedroll", typeof( Bedroll ), 100, 20, 0xA59, 0 ) );
				Add( new GenericBuyInfo( "flint", typeof( FlintAndSteel ), 450, 20, 0x27F9, 942 ) );
				//Add( new GenericBuyInfo( "traveler's tent", typeof( TravelTent ), 3500, 20, 0xA58, 801 ) );
				Add( new GenericBuyInfo( "strong rope", typeof( Rope ), 500, 5, 0x14F8, 0 ) );
				Add( new GenericBuyInfo( "arrow", typeof( Arrow ), 10, 20, 0xF3F, 0 ) );
				Add( new GenericBuyInfo( "torch", typeof( Torch ), 15, 20, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( "iron wire", typeof( IronWire ), 250, 10, 0x1876, 0 ) );
				Add( new GenericBuyInfo( "forged metal", typeof( ForgedMetal ), 400, 10, 0xFB8, 0 ) );
			}
		}
		
		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Kindling ), 10 );
				Add( typeof( Bedroll ), 50 );
				Add( typeof( FlintAndSteel ), 230 );
				Add( typeof( TravelTent ), 2000 );
				Add( typeof( Rope ), 200 );
				Add( typeof( Arrow ), 5 );
				Add( typeof( Torch ), 8 );
				Add( typeof( IronWire ), 125 );
				Add( typeof( ForgedMetal ), 200 );
			}
		}
	}
}
