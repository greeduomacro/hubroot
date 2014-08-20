using System;
using Server;
using Server.Gumps;
using Curr = Server.Currency.CurrencySystem;
using System.Text.RegularExpressions;

namespace Server.Market
{
	public class OrderManagementGump : Gump
	{
		internal int LabelHue = 1152;

		private MarketEntry _entry;

		public OrderManagementGump( MarketEntry entry )
			: base( 0, 0 )
		{
			_entry = entry;

			AddPage( 1 );
			AddBackground( 10, 10, 500, 430, 9250 );
			AddLabel( 170, 20, LabelHue, "Personal Market Management" );
			AddItem( 445, 30, 6226 );
			AddItem( 431, 53, 3826 );
			AddItem( 446, 62, 3820 );
			AddItem( 422, 45, 3822 );

			if( entry == null )
				AddLabel( 225, 40, LabelHue, "New Order" );
			else
				AddLabel( 215, 40, LabelHue, "Order Update" );

			AddLabel( 25, 80, LabelHue, "Item cost:" );

			int[] cost = new int[] { 0, 0, 0 };

			if( entry != null )
				cost = Curr.Compress( entry.Cost, 0, 0 );

			AddImageTiled( 30, 100, 80, 20, 3004 );
			AddTextEntry( 30, 100, 80, 20, 0, 1, Convert.ToString( cost[0] ) );
			AddLabel( 115, 100, LabelHue, "copper" );
			AddImageTiled( 30, 125, 80, 20, 3004 );
			AddTextEntry( 30, 125, 80, 20, 0, 2, Convert.ToString( cost[1] ) );
			AddLabel( 115, 125, LabelHue, "silver" );
			AddImageTiled( 30, 150, 80, 20, 3004 );
			AddTextEntry( 30, 150, 80, 20, 0, 3, Convert.ToString( cost[2] ) );
			AddLabel( 115, 150, LabelHue, "gold" );

			AddLabel( 275, 80, LabelHue, "Category:" );

			AddRadio( 310, 100, 9020, 9021, (entry == null ? false : entry.Category == Category.Armor), 1 );
			AddLabel( 335, 100, LabelHue, "Armor" );
			AddRadio( 310, 125, 9020, 9021, (entry == null ? false : entry.Category == Category.Clothing), 2 );
			AddLabel( 335, 125, LabelHue, "Clothing" );
			AddRadio( 310, 150, 9020, 9021, (entry == null ? true : entry.Category == Category.Misc), 3 );
			AddLabel( 335, 150, LabelHue, "Miscellaneous" );
			AddRadio( 310, 175, 9020, 9021, (entry == null ? false : entry.Category == Category.Resources), 4 );
			AddLabel( 335, 175, LabelHue, "Resources (commodities)" );
			AddRadio( 310, 200, 9020, 9021, (entry == null ? false : entry.Category == Category.SkillItems), 5 );
			AddLabel( 335, 200, LabelHue, "Skill Items (training aid)" );
			AddRadio( 310, 225, 9020, 9021, (entry == null ? false : entry.Category == Category.Weaponry), 6 );
			AddLabel( 335, 225, LabelHue, "Weaponry" );

			AddLabel( 25, 225, LabelHue, "Description:" );
			AddImageTiled( 25, 250, 355, 175, 3004 );
			AddTextEntry( 25, 250, 355, 175, 0, 4, (entry == null ? "" : entry.Description) );

			AddButton( 420, 265, 4023, 4025, GetButtonId( 1, 1 ), GumpButtonType.Reply, 0 );
			AddLabel( 395, 285, LabelHue, "Submit Order" );
			AddButton( 420, 335, 4017, 4019, GetButtonId( 1, 2 ), GumpButtonType.Reply, 0 );
			AddLabel( 395, 355, LabelHue, "Cancel Order" );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int val, type, index;
			DecodeButtonId( info.ButtonID, out val, out type, out index );

			if( val < 0 )
				return;

			Mobile from = sender.Mobile;

			if( type == 1 )
			{
				if( index == 1 )
				{
					bool newOrder = (_entry == null);

					if( newOrder )
						_entry = new MarketEntry( from );

					_entry.Category = (Category)(info.Switches[0] - 1);

					string cc = info.GetTextEntry( 1 ).Text;
					string cs = info.GetTextEntry( 2 ).Text;
					string cg = info.GetTextEntry( 3 ).Text;

					Regex rx = new Regex( "[^0-9]" );
					cc = rx.Replace( cc, "" ); cc.Trim();
					cs = rx.Replace( cs, "" ); cs.Trim();
					cg = rx.Replace( cg, "" ); cg.Trim();

					int[] cost = new int[]
					{
						(String.IsNullOrEmpty( cc ) ? 0 : Convert.ToInt32( cc )),
						(String.IsNullOrEmpty( cs ) ? 0 : Convert.ToInt32( cs )),
						(String.IsNullOrEmpty( cg ) ? 0 : Convert.ToInt32( cg ))
					};
					_entry.ChangeCost( cost[0], cost[1], cost[2] );
					_entry.Description = info.GetTextEntry( 4 ).Text;

					if( _entry.Cost < 10 )
					{
						from.SendMessage( "Please enter a price of at least 10 copper." );
						from.SendGump( new OrderManagementGump( _entry ) );
					}
					else if( String.IsNullOrEmpty( _entry.Description ) )
					{
						from.SendMessage( "Please enter a description for your order." );
						from.SendGump( new OrderManagementGump( _entry ) );
					}
					else
					{
						if( newOrder )
							Market.AddNewOrder( from, _entry );
						else
							Market.UpdateOrder( from, _entry );
					}
				}
				else if( index == 2 && _entry != null ) //cancel order
				{
					Market.CloseOrder( _entry, false );
				}
			}
		}
	}
}