using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Barter
{
	/**
	 * sort items by priority
	 * color items in trade gump based on likelihood of trade success
	 *		-green-named items would be traded for sure, based on what the player wants to trade, etc
	 **/
	public class BarterGump : Gump
	{
		public static readonly int LabelHue = 1152;

		private List<Gump> _children;
		private Mobile _client;
		private List<Item> _clientItems;
		private byte _clientPage;
		private BarterVendor _vendor;
		private List<Item> _vendorItems;
		private byte _vendorPage;

		public BarterGump( BarterVendor vendor, Mobile client ) : this( vendor, client, 1, 1 ) { }

		public BarterGump( BarterVendor vendor, Mobile client, byte vendorItemPage, byte clientItemPage ) : base( 10, 10 )
		{
			_children = new List<Gump>();
			_client = client;
			_vendor = vendor;

			_clientPage = Math.Max( clientItemPage, (byte)1 );
			_vendorPage = Math.Max( vendorItemPage, (byte)1 );

			AddPage( 1 );

			//vendor goods
			AddBackground( 0, 0, 240, 320, 9250 );
			AddLabel( 20, 15, LabelHue, "Vendor Goods" );

			_vendorItems = vendor.BankBox.Items;
			int rangeIdx = ((_vendorPage * 10) - 10);

			AddItemRange( 20, 40, 1, _vendorItems.GetRange( rangeIdx, Math.Min( (_vendorItems.Count - rangeIdx), 10 ) ) );

			if( _vendorPage > 1 )
				AddButton( 190, 15, 9766, 9767, GetButtonId( 0, 0 ), GumpButtonType.Reply, 0 );

			if( _vendorItems.Count > (rangeIdx + 10) )
				AddButton( 210, 15, 9762, 9763, GetButtonId( 0, 1 ), GumpButtonType.Reply, 0 );

			//client goods
			AddBackground( 510, 0, 240, 320, 9250 );
			AddLabel( 530, 15, LabelHue, "Your Goods" );

			_clientItems = client.Backpack.Items;
			rangeIdx = ((_clientPage * 10) - 10);

			AddItemRange( 530, 40, 2, _clientItems.GetRange( rangeIdx, Math.Min( (_clientItems.Count - rangeIdx), 10 ) ) );

			if( _clientPage > 1 )
				AddButton( 700, 15, 9766, 9767, GetButtonId( 0, 2 ), GumpButtonType.Reply, 0 );

			if( _clientItems.Count > (rangeIdx + 10) )
				AddButton( 720, 15, 9762, 9763, GetButtonId( 0, 3 ), GumpButtonType.Reply, 0 );
		}

		#region -void AddItemRange( short, short, byte, List<Item> )
		private void AddItemRange( short x, short startY, byte btnReplyType, List<Item> list )
		{
			if( list.Count > 10 )
				list = list.GetRange( 0, 10 );

			short y = startY;

			for( byte i = 0; i < list.Count; i++ )
			{
				AddAlphaRegion( x, y, 200, 20 );
				AddLabelCropped( (x + 5), y, 170, 20, LabelHue, (list[i].Name == null ? "unknown" : list[i].Name) );

				//if( !IsSelected( list[i] ) )
					AddButton( (x + 180), y, 4005, 4006, GetButtonId( btnReplyType, i ), GumpButtonType.Reply, 0 );

				y += 25;
			}
		}
		#endregion

		#region -bool IsSelected( Item )
		private bool IsSelected( Item item )
		{
			BarterTradeGump tradeGump = (BarterTradeGump)_children.Find( delegate( Gump g ) { return (g is BarterTradeGump); } );

			if( tradeGump == null )
				return false;

			bool isSelected = false;

			for( int i = 0; !isSelected && i < tradeGump.ClientItems.Count; i++ )
			{
				if( tradeGump.ClientItems[i] == item )
					isSelected = true;
			}

			for( int i = 0; !isSelected && i < tradeGump.VendorItems.Count; i++ )
			{
				if( tradeGump.VendorItems[i] == item )
					isSelected = true;
			}

			return isSelected;
		} 
		#endregion

		#region +override void OnResponse( NetState, RelayInfo )
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int val = (info.ButtonID - 1);

			if( val < 0 )
			{
				CloseChildren();
				_vendor.DisposeTrade( _client, false );
				return;
			}

			int type = (val % 10);
			int index = (val / 10);

			/** known types:
			 * 0 = page switching (index 0-1 for vendor paging, 2-3 for client paging)
			 * 1 = vendor item details (index is index in array)
			 * 2 = client item details (index is index in array)
			 **/
			switch( type )
			{
				case 0:
					{
						switch( index )
						{
							case 0:
								{
									_client.SendGump( new BarterGump( _vendor, _client, (byte)(_vendorPage - 1), _clientPage ) );
									break;
								}
							case 1:
								{
									_client.SendGump( new BarterGump( _vendor, _client, (byte)(_vendorPage + 1), _clientPage ) );
									break;
								}
							case 2:
								{
									_client.SendGump( new BarterGump( _vendor, _client, _vendorPage, (byte)(_clientPage - 1) ) );
									break;
								}
							case 3:
								{
									_client.SendGump( new BarterGump( _vendor, _client, _vendorPage, (byte)(_clientPage + 1) ) );
									break;
								}
						}
						break;
					}
				case 1:
				case 2:
					{
						_client.CloseGump( typeof( BarterDetailGump ) );
						_client.SendGump( this );

						Gump child = new BarterDetailGump( this, (type == 1 ? _vendorItems[index] : _clientItems[index]), (type == 1) );

						if( _client.SendGump( child ) )
							AddChild( child );
						break;
					}
			}
		}
		#endregion

		#region +override void OnServerClose( NetState )
		public override void OnServerClose( NetState owner )
		{
			base.OnServerClose( owner );

			CloseChildren();
			_vendor.DisposeTrade( _client, false );
		}
		#endregion

		#region +void AddChild( Gump )
		public void AddChild( Gump child )
		{
			if( !_children.Contains( child ) )
				_children.Add( child );
		}
		#endregion

		#region +void CloseChildren()
		public void CloseChildren()
		{
			_children.ForEach(
				delegate( Gump g )
				{
					_client.CloseGump( g.GetType() );
				} );
		}
		#endregion
	}
}