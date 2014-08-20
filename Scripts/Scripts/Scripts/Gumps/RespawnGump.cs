using System;
using Server.Mobiles;

namespace Server.Gumps
{
    public class RespawnGump : Gump
    {
        private Player _from;

        public RespawnGump( Player from, TimeSpan minToRespawn )
            : base(200, 100)
        {
            _from = from;

            Closable = false;

            Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerStateCallback(Resend), Tuple.Create<Player, TimeSpan>(from, (minToRespawn.Subtract(TimeSpan.FromMinutes(1)))));

            AddPage(1);
            AddBackground(0, 0, 245, 190, 9250);

            AddHtml(15, 15, 215, 100, "<Center>You have been knocked out! You must wait here for another adventurer to assist you, or wait to regain consciousness on your own.</Center>", false, false);
            AddHtml(15, 115, 215, 20, String.Format("<Center>Current Knockout Count: {0}</Center>", from.KOCount), false, false);
            AddHtml(15, 155, 215, 20, String.Format("<CENTER>Automatic recovery in {0} minute{1}.</CENTER>", minToRespawn.Minutes, (minToRespawn.Minutes == 1 ? "" : "s")), false, false);
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
               //if( info.ButtonID == 1 )
                //Suicide Maybe?
        }

        private void Resend( object state )
        {
            Tuple<Player, TimeSpan> args = (Tuple<Player, TimeSpan>)state;
            Player pm = args.Item1;
            TimeSpan minToRespawn = args.Item2;

            if( pm == null || pm.Alive )
                return;

            pm.CloseGump(typeof(RespawnGump));

            if( minToRespawn.Minutes <= 0 )
                pm.AutoRespawn();
            else
               pm.SendGump(new RespawnGump(pm, minToRespawn));
        }
    }
}