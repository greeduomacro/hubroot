using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Barter
{
	public class BarterTradeGump : Gump
	{
		private List<Item> _clientItems;
		private List<Item> _vendorItems;

		public List<Item> ClientItems { get { return _clientItems; } }
		public List<Item> VendorItems { get { return _vendorItems; } }

		public BarterTradeGump( List<Item> vendorItems, List<Item> clientItems ) : base( 265, 200 )
		{
			_clientItems = clientItems;
			_vendorItems = vendorItems;

			AddPage( 1 );
			AddBackground( 0, 0, 240, 320, 9250 );
			AddLabel( 20, 15, BarterGump.LabelHue, "Prospective Trade" );
			AddItem( 155, 20, 6226 );
			AddItem( 190, 5, 3651 );

			int x = 20, y = 40;

			for( byte i = 0; i < vendorItems.Count; i++ )
			{
				AddAlphaRegion( x, y, 200, 20 );
				AddLabelCropped( (x + 5), y, 170, 20, BarterGump.LabelHue, (vendorItems[i].Name == null ? "unknown" : vendorItems[i].Name) );

				//if( !IsSelected( list[i] ) )
				//    AddButton( (x + 180), y, 4005, 4006, GetButtonId( btnReplyType, i ), GumpButtonType.Reply, 0 );

				y += 25;
			}
		}
	}
}