using System;
using System.Text;
using Server.Utilities;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Curr = Server.Currency.CurrencySystem;

namespace Server.Market
{
    public class MarketDetailsGump : Gump
    {
        private MarketEntry _entry;

        public MarketDetailsGump( MarketEntry entry )
            : base(0, 0)
        {
            _entry = entry;

            AddPage(1);
            AddBackground(155, 65, 535, 355, 9250);
            AddLabel(380, 75, 1152, "Item Details");

            IEntity entity = World.FindEntity(entry.ObjectSerial);

            if( entity.Serial.IsItem )
                AddItem(615, 85, ((Item)entity).ItemID, ((Item)entity).Hue);

            AddAlphaRegion(170, 105, 415, 300);
            AddHtml(170, 105, 415, 300, String.Format("<basefont color=#{0:X6}>{1}</basefont>", 0xFFFFFF, GetDescription(entry)), false, true);

            AddButton(625, 275, 4023, 4025, GetButtonId(1, 0), GumpButtonType.Reply, 0);
            AddLabel(610, 295, 1152, "Purchase");

            AddButton(625, 340, 4020, 4022, GetButtonId(1, 1), GumpButtonType.Reply, 0);
            AddLabel(615, 360, 1152, "Go back");
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            int val, type, index;
            DecodeButtonId(info.ButtonID, out val, out type, out index);

            if( val < 0 )
                return;

            switch( type )
            {
                case 1:
                    {
                        switch( index )
                        {
                            case 0: //purchase
                                {
                                    Market.BuyItem(sender.Mobile, _entry);
                                    break;
                                }
                            case 1: //go back
                                {
                                    sender.Mobile.CloseGump(typeof(MarketDetailsGump));
                                    break;
                                }
                        }

                        break;
                    }
            }
        }

        private string GetDescription( MarketEntry entry )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format("Seller: {0}", (entry.Seller == null ? "(private)" : entry.Seller.RawName)));

            int[] cost = Curr.Compress(entry.Cost, 0, 0);
            sb.AppendLine(String.Format("Total cost: {0} copper, {1} silver, {2} gold", cost[0], cost[1], cost[2]));

            sb.AppendLine(String.Format("Seller's description: {0}", entry.Description));

            sb.AppendLine();
            sb.Append("<center>---------------</center>");
            sb.Append(String.Format("<center>{0} Details</center>", (entry.ObjectSerial.IsItem ? "Item" : "Mobile")));

            IEntity entity = World.FindEntity(entry.ObjectSerial);

            if( entity is Item )
            {
                Item item = (Item)entity;

                sb.AppendLine("Name: " + item.Name);

                if( item.Amount > 1 )
                    sb.AppendLine("Quantity: " + item.Amount);

                if( item is BaseWeapon )
                {
                    BaseWeapon weap = (BaseWeapon)item;

                    if( weap.Crafter != null )
                        sb.AppendLine("Crafted by: " + weap.Crafter.RawName);

                    sb.AppendLine("Quality: " + weap.Quality);

                    if( weap.Identified )
                    {
                        sb.AppendLine("Durability: " + weap.DurabilityLevel);
                        sb.AppendLine("Accuracy level: " + weap.AccuracyLevel);
                        sb.AppendLine("Damage level: " + weap.DamageLevel);

                        if( weap.Slayer != SlayerName.None )
                        {
                            SlayerEntry se = SlayerGroup.GetEntryByName(weap.Slayer);

                            if( se != null )
                                sb.AppendLine("Slayer enhancement: " + se.Title);
                        }

                        if( weap.Slayer2 != SlayerName.None )
                        {
                            SlayerEntry se = SlayerGroup.GetEntryByName(weap.Slayer2);

                            if( se != null )
                                sb.AppendLine("Slayer enhancement: " + se.Title);
                        }
                    }
                    else
                    {
                        sb.AppendLine("<i>Unidentified</i>");
                    }
                }
            }
            else if( entity is Mobile )
            {
                Mobile mob = (Mobile)entity;

                sb.AppendLine("Name: " + mob.Name);

                if( mob is BaseCreature )
                {
                    BaseCreature bc = (BaseCreature)mob;

                    sb.AppendLine("Creature type: " + Util.SplitString(bc.GetType().ToString()));
                    sb.AppendLine("Minimum Taming required: " + bc.MinTameSkill.ToString("F2"));
                    sb.AppendLine("Follower Slots required: " + bc.ControlSlots);
                }
            }

            return sb.ToString();
        }
    }
}