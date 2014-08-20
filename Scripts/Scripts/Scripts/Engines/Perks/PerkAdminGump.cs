using System;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Perks
{
    public class PerkAdminGump : Gump
    {
        [CommandAttribute("PerkAdmin", AccessLevel.Administrator)]
        public static void Command( CommandEventArgs args )
        {
            Mobile m = args.Mobile;

            if( args.Length > 0 )
            {
                m.BeginTarget(18, false, TargetFlags.None, ( from, targ ) => m.SendGump(new PerkAdminGump(m, Page.Manage_Targeted, null, targ)));
            }
            else
            {
                m.SendGump(new PerkAdminGump(m, Page.Information, null, null));
            }
        }

        public enum Page
        {
            Information,
            Manage,
            Manage_Targeted
        }

        private const int LabelColor32 = 0xFFFFFF;
        private const int SelectedColor32 = 0x8080FF;
        private const int DisabledColor32 = 0x808080;

        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;
        private const int RedHue = 0x20;

        public Mobile From { get; private set; }
        public Page PageType { get; private set; }
        public object State { get; private set; }

        public PerkAdminGump( Mobile from, Page page, string notice, object state )
            : base(50, 40)
        {
            from.CloseGump(typeof(PerkAdminGump));

            From = from;
            PageType = page;
            State = state;

            AddPage(0);
            AddBackground(0, 0, 420, 440, 5054);

            AddBlackAlpha(10, 10, 170, 100);
            AddBlackAlpha(190, 10, 220, 100);
            AddBlackAlpha(10, 120, 400, 260);
            AddBlackAlpha(10, 390, 400, 40);

            AddPageButton(10, 10, GetButtonId(0, 1), "INFORMATION", Page.Information);
            AddPageButton(10, 30, GetButtonId(0, 2), "MANAGE", Page.Manage);

            if( notice != null )
                AddHtml(12, 392, 396, 36, Color(notice, LabelColor32), false, false);

            switch( page )
            {
                case Page.Information:
                    AddLabel(20, 130, LabelHue, "Active Perks:");
                    AddLabel(150, 130, LabelHue, Perk.AllPerks.Count.ToString());

                    AddLabel(20, 150, LabelHue, "Initial Cost:");
                    AddLabel(150, 150, LabelHue, Perk.FirstLevelCost.ToString("N0"));

                    AddLabel(20, 170, LabelHue, "Players with Perks:");
                    AddLabel(150, 170, LabelHue, Perk.PerkTable.Count.ToString("N0"));
                    break;
                case Page.Manage:
                    AddPageButton(200, 20, GetButtonId(1, 0), "Select a player", Page.Manage_Targeted);
                    break;
                case Page.Manage_Targeted:
                    AddPageButton(200, 20, GetButtonId(1, 0), "Select a different player", Page.Manage);

                    if( state != null && state is Player )
                    {
                        Player target = (Player)state;
                        Tuple<Perk, Perk> targetPerks = Perk.GetPerks(target);

                        AddLabel(200, 50, LabelHue, String.Format("Player: {0}", target.RawName));
                        AddLabel(200, 70, LabelHue, String.Format("EoC: {0}", target.EssenceOfCharacter.ToString("N0")));

                        AddLabelCropped(20, 130, 110, 20, LabelHue, "Perk");
                        AddLabelCropped(132, 130, 110, 20, LabelHue, "Level");

                        //first perk
                        AddLabelCropped(20, 150, 110, 20, LabelHue, targetPerks.Item1.Label);
                        AddLabelCropped(132, 150, 110, 20, LabelHue, targetPerks.Item1.Level.ToString());

                        if( targetPerks.Item1.Level < PerkLevel.Fifth )
                            AddButton(345, 150, 2435, 2436, GetButtonId(3, (int)PerkPosition.Primary), GumpButtonType.Reply, 0);

                        if( targetPerks.Item1.Level > PerkLevel.First )
                            AddButton(360, 150, 2437, 2438, GetButtonId(4, (int)PerkPosition.Primary), GumpButtonType.Reply, 0);

                        AddButton(380, 150, 4017, 4018, GetButtonId(2, (int)PerkPosition.Primary), GumpButtonType.Reply, 0);

                        //second perk
                        AddLabelCropped(20, 175, 110, 20, LabelHue, targetPerks.Item2.Label);
                        AddLabelCropped(132, 175, 110, 20, LabelHue, targetPerks.Item2.Level.ToString());

                        if( targetPerks.Item2.Level < PerkLevel.Fifth )
                            AddButton(345, 175, 2435, 2436, GetButtonId(3, (int)PerkPosition.Secondary), GumpButtonType.Reply, 0);

                        if( targetPerks.Item2.Level > PerkLevel.First )
                            AddButton(360, 175, 2437, 2438, GetButtonId(4, (int)PerkPosition.Secondary), GumpButtonType.Reply, 0);

                        AddButton(380, 175, 4017, 4018, GetButtonId(2, (int)PerkPosition.Secondary), GumpButtonType.Reply, 0);
                    }
                    else
                    {
                        AddHtml(12, 392, 396, 36, Color("Only players have perks!", RedHue), false, false);
                    }
                    break;
            }
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            int val, type, index;
            DecodeButtonId(info.ButtonID, out val, out type, out index);

            if( val < 0 )
                return;

            Player target = State as Player;

            switch( type )
            {
                case 0: //change main page
                    Page page;

                    switch( index )
                    {
                        case 1: page = Page.Information; break;
                        case 2: page = Page.Manage; break;
                        default: return;
                    }

                    From.SendGump(new PerkAdminGump(From, page, null, null));
                    break;
                case 1: //select player
                    switch( index )
                    {
                        case 0:
                            From.SendMessage("Select a player to view perks.");
                            From.BeginTarget(18, false, TargetFlags.None, ( from, targ ) => From.SendGump(new PerkAdminGump(From, Page.Manage_Targeted, null, targ)));
                            break;
                    }
                    break;
                case 2: //remove perk
                    if( target == null )
                    {
                        From.SendGump(new PerkAdminGump(From, Page.Manage_Targeted, "The targeted player was lost! Please try again.", State));
                    }
                    else
                    {
                        Perk.Remove(target, (PerkPosition)index);
                        From.SendGump(new PerkAdminGump(From, Page.Manage_Targeted, null, State));
                    }
                    break;
                case 3: //increase level
                case 4: //decrease level
                    if( target == null )
                    {
                        From.SendGump(new PerkAdminGump(From, Page.Manage_Targeted, "The targeted player was lost! Please try again.", State));
                    }
                    else
                    {
                        Tuple<Perk, Perk> targetPerks = Perk.GetPerks(target);
                        Perk targetPerk;

                        switch( (PerkPosition)index )
                        {
                            default:
                            case PerkPosition.Primary:
                                targetPerk = targetPerks.Item1;
                                break;
                            case PerkPosition.Secondary:
                                targetPerk = targetPerks.Item2;
                                break;
                        }

                        if( !(targetPerk is Empty) )
                        {
                            if( type == 3 && targetPerk.Level < PerkLevel.Fifth )
                                targetPerk.Level++;
                            else if( targetPerk.Level > PerkLevel.First )
                                targetPerk.Level--;
                        }

                        From.SendGump(new PerkAdminGump(From, Page.Manage_Targeted, null, State));
                    }
                    break;
            }
        }

        private void AddPageButton( int x, int y, int buttonID, string text, Page page, params Page[] subPages )
        {
            bool isSelection = (PageType == page);

            for( int i = 0; !isSelection && i < subPages.Length; i++ )
                isSelection = (PageType == subPages[i]);

            AddSelectedButton(x, y, buttonID, text, isSelection);
        }

        private void AddSelectedButton( int x, int y, int buttonID, string text, bool isSelection )
        {
            AddButton(x, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
            AddHtml(x + 35, y, 200, 20, Color(text, isSelection ? SelectedColor32 : LabelColor32), false, false);
        }
    }
}