using System;
using Server.Mobiles;
using Server.Gumps;
using Server.Perks;
using Server.SkillCapSelection;

namespace Server.Gumps
{
    public class StatusGump : Gump
    {
        private static int LabelHue = 1152;

        public StatusGump( Player from )
            : base(200, 200)
        {
            StatusEntry[] entries = new StatusEntry[]
            {
                new StatusEntry("Race", from.Race),
                new StatusEntry("Skills", String.Format("{0}", (from.SkillsTotal / 10).ToString("F1"))),
                new StatusEntry("E.o.C", from.EssenceOfCharacter),
                
                new StatusEntry("bar"),
                
                new StatusEntry("Knockout Counts", from.KOCount),

                new StatusEntry("bar"),

                new StatusEntry("Hunger", String.Format("({0}/20)", from.Hunger )),
                new StatusEntry("Thirst", String.Format("({0}/20)", from.Thirst ))
            };

            AddPage(1);
            AddBackground(0, 0, 255, 330, 9250);
            AddAlphaRegion(15, 15, 225, 300);

            AddButton(225, 15, 3, 4, 0, GumpButtonType.Reply, 0); // X

            AddButton(45, 235, 234, 234, 1, GumpButtonType.Reply, 0);
            AddLabel(57, 295, 1154, "Perks");

            AddButton(150, 235, 229, 229, 2, GumpButtonType.Reply, 0);
            AddLabel(162, 295, 1154, "Skills");

            AddButton(200, 125, 252, 253, 3, GumpButtonType.Reply, 0); //Reduce KO's


            AddLabel(20, 15, 1152, "Character Overview");
            AddBar(40);

            for( int i = 0, y = 50; i < entries.Length; i++ )
            {
                if( entries[i].Prompt == "bar" )
                {
                    y += 5;

                    AddBar(y);

                    y += 10;
                }
                else
                {
                    AddLabel(20, y, LabelHue, entries[i].Prompt + "");
                    AddLabel(160, y, LabelHue, entries[i].Value.ToString());

                    y += 20;
                }
            }
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            if( sender.Mobile == null )
                return;

            sender.Mobile.CloseGump(typeof(StatusGump));

            if (info.ButtonID == 1)
            {
                sender.Mobile.SendGump(new PerkOverviewGump((Player)sender.Mobile));
            }

            if (info.ButtonID == 2)
            {
                sender.Mobile.SendGump(new SkillCapSelectionGump((Player)sender.Mobile));
            }

            if (info.ButtonID == 3)
            {
                if (((Player)sender.Mobile).EoC >= 5000 && ((Player)sender.Mobile).KOCount > 0)
                {
                    ((Player)sender.Mobile).ConsumeEoC(5000);
                    ((Player)sender.Mobile).KOCount--;

                    sender.Mobile.CloseGump(typeof(StatusGump));
                    sender.Mobile.SendGump(new StatusGump(((Player)sender.Mobile)));
                }

                else if (((Player)sender.Mobile).EoC < 5000)
                {
                    sender.Mobile.SendMessage("You do not have the required essence to do that.");
                    sender.Mobile.SendGump(new StatusGump(((Player)sender.Mobile)));
                }

                else if (((Player)sender.Mobile).KOCount < 1)
                {
                    sender.Mobile.SendMessage("You can not lower your KO Counts any further.");
                    sender.Mobile.SendGump(new StatusGump(((Player)sender.Mobile)));
                }
            }
        }

        private void AddBar( int y )
        {
            AddImageTiled(15, y, 225, 4, 9151);
        }

        class StatusEntry
        {
            public string Prompt { get; set; }
            public object Value { get; set; }

            public StatusEntry( string prompt, object value = null )
            {
                Prompt = prompt;
                Value = value;
            }
        }
    }
}
