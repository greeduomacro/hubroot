using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;

namespace Server.Market
{
	public class PersonalMarketGump : Gump
	{
		internal int LabelHue = 1152;
		internal int InactiveLabelHue = 943;

		protected List<MarketEntry> entries;
		protected byte selectedPage;

		public PersonalMarketGump( Mobile from ) : this( from, 0 ) { }

		public PersonalMarketGump( Mobile from, byte page )
			: base( 0, 0 )
		{
			selectedPage = page;

			AddPage( 1 );
			AddBackground( 10, 10, 500, 430, 9250 );
			AddLabel( 170, 20, LabelHue, "Personal Market Management" );
			AddItem( 445, 30, 6226 );
			AddItem( 431, 53, 3826 );
			AddItem( 446, 62, 3820 );
			AddItem( 422, 45, 3822 );

			AddLabel( 30, 55, LabelHue, "Orders (active/total): " + Market.PrintOrderData( from ) );
			AddLabel( 30, 80, LabelHue, "Total earnings to date: " + Market.PrintOrderEarnings( from ) );

			AddLabel( 350, 145, LabelHue, "Create new order" );
			AddButton( 465, 145, 4011, 4013, GetButtonId( 3, 1 ), GumpButtonType.Reply, 0 );

			AddBackground( 25, 180, 470, 245, 9350 );
			AddLabel( 225, 160, LabelHue, "My Orders" );

			entries = Market.EntriesBySeller( from );

			for( int i = 0, y = 185, index = (page * 10); i < 10; i++, y += 22 )
			{
				AddAlphaRegion( 30, y, 355, 20 );
				AddAlphaRegion( 387, y, 85, 20 );

				if( entries.Count >= (index + 1) )
				{
					MarketEntry entry = entries[index];

					if( entry == null )
						continue;

					AddLabelCropped( 30, y, 355, 20, (entry.Active ? LabelHue : InactiveLabelHue), entry.Description );

					int[] cost = Currency.CurrencySystem.Compress( entry.Cost, 0, 0 );
					AddLabel( 387, y, LabelHue, String.Format( "{0}c, {1}s, {2}g", cost[0], cost[1], cost[2] ) );

					if( entry.Active )
						AddButton( 478, (y + 5), 2103, 2104, GetButtonId( 1, index ), GumpButtonType.Reply, 0 );

					index++;
				}
			}

			if( page > 0 )
				AddButton( 240, 405, 5603, 5607, GetButtonId( 2, 1 ), GumpButtonType.Reply, 0 );
			if( entries.Count > ((page + 1) * 10) )
				AddButton( 265, 405, 5601, 5605, GetButtonId( 2, 2 ), GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int val, type, index;
			DecodeButtonId( info.ButtonID, out val, out type, out index );

			if( val < 0 )
				return;

			Mobile from = sender.Mobile;

			switch( type )
			{
				case 1: //specific order edit
					{
						if( index >= 0 && index < entries.Count && entries[index] != null )
							from.SendGump( new OrderManagementGump( entries[index] ) );

						break;
					}
				case 2: //change page
					{
						switch( index )
						{
							case 1:
								{
									if( selectedPage > 0 )
										from.SendGump( new PersonalMarketGump( from, (byte)(selectedPage - 1) ) );

									break;
								}
							case 2:
								{
									from.SendGump( new PersonalMarketGump( from, (byte)(selectedPage + 1) ) );
									break;
								}
						}

						break;
					}
				case 3: //new order
					{
						from.SendGump( new OrderManagementGump( null ) );
						break;
					}
			}
		}
	}
}