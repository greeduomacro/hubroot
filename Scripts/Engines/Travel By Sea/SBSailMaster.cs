using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic; 

namespace Khazman.TravelBySea
{
	public class SBSailMaster : SBInfo
	{
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBSailMaster()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

        public class InternalBuyInfo : List<GenericBuyInfo> 
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Sailing Membership", typeof( SailingMembershipCard ), 2000, 5, 0x14F0, 0x4F1 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( SailingMembershipCard ), 1200 );
			}
		}
	}
}