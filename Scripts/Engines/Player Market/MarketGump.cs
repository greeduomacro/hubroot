using System;
using System.Collections.Generic;
using Server.Utilities;
using Server;
using Server.Gumps;

namespace Server.Market
{
    public enum Category
    {
        Armor,
        Clothing,
        Misc,
        Resources,
        SkillItems,
        Weaponry
    }

    public class MarketGump : Gump
    {
        internal int LabelHue = 1152;
        internal int LabelColor32 = 0xFFFFFF;

        protected List<MarketEntry> entries;
        protected Category selectedCategory;
        protected byte selectedPage;

        public MarketGump() : this(Category.Misc, 0) { }

        public MarketGump( Category category, byte page )
            : base(0, 0)
        {
            selectedCategory = category;
            selectedPage = page;

            AddPage(1);
            AddBackground(10, 10, 780, 490, 9250);
            AddLabel(355, 20, LabelHue, "Player Market");

            AddAlphaRegion(25, 40, 125, 20);
            AddHtml(25, 40, 125, 20, String.Format("<basefont color=#{0:X6}><center>Category</center></basefont>", LabelColor32), false, false);
            AddAlphaRegion(155, 40, 535, 20);
            AddHtml(155, 40, 535, 20, String.Format("<basefont color=#{0:X6}><center>Item Description</center></basefont>", LabelColor32), false, false);
            AddAlphaRegion(695, 40, 80, 20);
            AddHtml(695, 40, 80, 20, String.Format("<basefont color=#{0:X6}><center>Cost</center></basefont>", LabelColor32), false, false);
            AddImageTiled(25, 60, 750, 4, 9151);

            string[] categories = Enum.GetNames(typeof(Category));

            for( int i = 0, y = 70; i < categories.Length; i++, y += 25 )
            {
                AddAlphaRegion(45, y, 105, 20);

                if( (int)selectedCategory != i )
                {
                    AddButton(25, (y + 5), 5601, 5605, GetButtonId(1, i), GumpButtonType.Reply, 0);
                    AddLabel(45, y, LabelHue, Util.SplitString(categories[i]));
                }
                else
                {
                    AddImage(25, (y + 5), 5605);
                    AddHtml(45, y, 105, 20, String.Format("<basefont color=#{0:X6}>{1}</basefont>", LabelColor32, Util.SplitString(categories[i])), false, false);
                }
            }

            entries = Market.EntriesByCategory(selectedCategory);

            if( entries.Count > 0 )
            {
                for( int i = 0, index = (selectedPage * 10), y = 65; i < 10 && index >= 0 && index < entries.Count; i++, index++, y += 40 )
                {
                    MarketEntry entry = entries[index];

                    if( entry == null )
                        continue;

                    AddAlphaRegion(155, y, 535, 20);
                    AddLabelCropped(155, y, 535, 20, LabelHue, entry.Description);

                    int[] cost = Currency.CurrencySystem.Compress(entry.Cost, 0, 0);
                    AddAlphaRegion(695, y, 80, 20);
                    AddLabel(695, y, LabelHue, String.Format("{0}c, {1}s, {2}g", cost[0], cost[1], cost[2]));

                    AddButton(645, (y + 21), 5411, 5411, GetButtonId(2, index), GumpButtonType.Reply, 0);
                    AddButton(675, (y + 22), 2117, 2118, GetButtonId(3, index), GumpButtonType.Reply, 0);
                }
            }

            if( selectedPage > 0 )
                AddButton(380, 470, 5603, 5607, GetButtonId(4, 1), GumpButtonType.Reply, 0);
            if( entries.Count > ((selectedPage + 1) * 10) )
                AddButton(405, 470, 5601, 5605, GetButtonId(4, 2), GumpButtonType.Reply, 0);
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile from = sender.Mobile;
            int val, type, index;
            DecodeButtonId(info.ButtonID, out val, out type, out index);

            if( val <= 0 || from == null || !from.Alive )
                return;

            from.CloseGump(typeof(MarketDetailsGump));

            switch( type )
            {
                case 1: //select category
                    {
                        from.SendGump(new MarketGump((Category)index, 0));

                        break;
                    }
                case 2: //view details
                    {
                        from.SendGump(new MarketGump(selectedCategory, selectedPage));

                        if( index >= 0 && index < entries.Count && entries[index] != null )
                            from.SendGump(new MarketDetailsGump(entries[index]));

                        break;
                    }
                case 3: //buy now
                    {
                        if( index >= 0 && index < entries.Count && entries[index] != null )
                            Market.BuyItem(from, entries[index]);

                        break;
                    }
                case 4: //change page
                    {
                        switch( index )
                        {
                            case 1:
                                {
                                    if( selectedPage > 0 )
                                        from.SendGump(new MarketGump(selectedCategory, (byte)(selectedPage - 1)));

                                    break;
                                }
                            case 2:
                                {
                                    from.SendGump(new MarketGump(selectedCategory, (byte)(selectedPage + 1)));
                                    break;
                                }
                        }

                        break;
                    }
            }
        }
    }
}