using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Barter
{
	public class BarterDetailGump : Gump
	{
		private Item _item;
		private BarterGump _parent;
		private bool _vendorItem;

		public bool IsVendorItem { get { return _vendorItem; } }

		public BarterDetailGump( BarterGump parent, Item item, bool isVendorItem ) : base( 240, 10 )
		{
			_item = item;
			_parent = parent;
			_vendorItem = isVendorItem;

			AddPage( 1 );
			AddBackground( 0, 0, 300, 175, 9250 );
			AddLabel( (IsVendorItem ? 95 : 100), 15, BarterGump.LabelHue, String.Format( "{0} Item Details", (IsVendorItem ? "Vendor" : "Your") ) );

			AddHtml( 15, 35, 270, 100, String.Format( "<basefont color=black>{0}</basefont>", GetItemDetails( item ) ), false, true );

			AddButton( 15, 140, 4020, 4022, 0, GumpButtonType.Reply, 0 );

			AddLabel( 180, 140, BarterGump.LabelHue, "Trade item" );
			AddButton( 255, 140, 4011, 4013, 1, GumpButtonType.Reply, 0 );
		}

		#region -string GetItemDetails( Item )
		private string GetItemDetails( Item item )
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine( String.Format( "Name: {0}", (item.Name == null ? "unknown" : item.Name) ) );
			builder.AppendLine( String.Format( "Type: {0}", Utilities.Util.SplitString( item.GetType().Name ) ) );

			return builder.ToString();
		}
		#endregion

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if( info.ButtonID == 1 )
			{
				BarterTradeGump tradeGump = (BarterTradeGump)sender.Mobile.FindGump( typeof( BarterTradeGump ) );
				List<Item> clientItems;
				List<Item> vendorItems;

				if( tradeGump != null )
				{
					clientItems = tradeGump.ClientItems;
					vendorItems = tradeGump.VendorItems;
				}
				else
				{
					clientItems = new List<Item>();
					vendorItems = new List<Item>();
				}

				if( _vendorItem )
					vendorItems.Add( _item );
				else
					clientItems.Add( _item );

				tradeGump = new BarterTradeGump( vendorItems, clientItems );

				_parent.CloseChildren();
				_parent.AddChild( tradeGump );
				sender.Mobile.SendGump( tradeGump );
			}
		}
		#endregion
	}
}