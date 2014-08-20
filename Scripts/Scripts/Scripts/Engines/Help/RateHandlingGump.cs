using System;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Help
{
    public class RateHandlingGump : Gump
    {
        private Mobile _sender;
        private Mobile _handler;
        private PageEntry _entry;

        public RateHandlingGump(Mobile sender, Mobile handler, PageEntry entry)
            : base(0, 0)
        {
            _sender = sender;
            _handler = handler;
            _entry = entry;

            Closable = false;

            AddPage(0);
            AddBackground(20, 20, 400, 345, 9250);

            //AddLabel(120, 35, 0, "The Genesis Roleplaying Shard");
            AddLabel(105, 55, 0, "Staff Reputation and Voting System");

            AddLabel(40, 125, 0, "Please rate your experience");
            AddLabelCropped(40, 145, 195, 20, 0, String.Format("with {0} today:", _handler.RawName));

            int x = 240, y = 100;

            for (int i = 0; i < 5; i++, y += 20)
            {
                AddRadio(x, y, 208, 209, false, i + 1);
                AddLabel(x + 25, y, 1152, GetLabel(i));
            }

            AddLabel(35, 198, 0, "Comments or Notes:");
            AddBackground(32, 215, 375, 110, 9200);
            AddTextEntry(35, 220, 368, 100, 0, 0, "");

            AddButton(375, 328, 4011, 4013, 0, GumpButtonType.Reply, 0);
            AddLabel(340, 330, 0, "Done");
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Player handler = _handler as Player;
            string comments = info.GetTextEntry(0).Text;
            int points = 0;

            if (handler == null)
                return;

            if (info.IsSwitched(1))
                points = 2;
            else if (info.IsSwitched(2))
                points = 1;
            else if (info.IsSwitched(3))
                points = 0;
            else if (info.IsSwitched(4))
                points = -1;
            else if (info.IsSwitched(5))
                points = -2;

            if (points > 0)
                handler.RepGood += points;
            else
                handler.RepBad += points;

            if (_entry != null)
            {
                try
                {
                    _entry.WriteLine();
                    _entry.WriteLine(String.Format("### --- Vote: {0} --- ###", points.ToString()));
                    _entry.WriteLine("### --- Comments: --- ###");
                    _entry.WriteLine(comments);
                }
                catch { }
            }
        }

        private string GetLabel(int index)
        {
            string str = "";

            switch (index)
            {
                case 0:
                    {
                        str = String.Format("Excellent  [+2]");
                    } break;
                case 1:
                    {
                        str = String.Format("Good  [+1]");
                    } break;
                case 2:
                    {
                        str = String.Format("Mediocre/Fair  [+0]");
                    } break;
                case 3:
                    {
                        str = String.Format("Poor  [-1]");
                    } break;
                case 4:
                    {
                        str = String.Format("Awful  [-2]");
                    } break;
            }

            return str;
        }
    }
}