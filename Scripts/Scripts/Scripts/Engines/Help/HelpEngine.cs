using Server;
using Server.Commands;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Help
{
    public class HelpEngine
    {
        public const bool LoggingEnabled = true;
        public const string LogDir = @"Pages\";

        public static void Initialize()
        {
            EventSink.HelpRequest += new HelpRequestEventHandler(EventSink_HelpRequest);
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            EventSink.Logout += new LogoutEventHandler(EventSink_Logout);

            CommandSystem.Register("CheckPage", AccessLevel.Player, new CommandEventHandler(CheckPage_OnCommand));
            CommandSystem.Register("GetRep", AccessLevel.Player, new CommandEventHandler(GetRep_OnCommand));
            CommandSystem.Register("Pages", AccessLevel.Counselor, new CommandEventHandler(Pages_OnCommand));
        }

        #region Events
        private static void EventSink_HelpRequest(HelpRequestEventArgs args)
        {
            Mobile m = args.Mobile;

            if (m.HasGump(typeof(HelpGump)) || !PageQueue.AllowedToPage(m))
                return;

            if (PageQueue.Contains(m))
                m.SendMenu(new ContainedMenu(m));
            else
                m.SendGump(new HelpGump(m));
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile m = args.Mobile;

            if (m != null && m.AccessLevel >= AccessLevel.Counselor)
            {
                if (PageQueue.List.Count > 0)
                {
                    if (m.HasGump(typeof(PageAlertGump)))
                        m.CloseGump(typeof(PageAlertGump));

                    m.SendGump(new PageAlertGump());
                }
            }
        }

        private static void EventSink_Logout(LogoutEventArgs args)
        {
            Mobile m = args.Mobile;

            if (PageQueue.Contains(m))
                PageQueue.Cancel(m);
        }
        #endregion

        #region Commands
        [Usage("CheckPage")]
        [Description("Retrieves the queue status of your most recent help page, if available.")]
        private static void CheckPage_OnCommand(CommandEventArgs args)
        {
            Mobile m = args.Mobile;
            PageEntry entry = PageQueue.GetEntry(m);
            int index = PageQueue.IndexOf(entry);

            if (entry == null)
            {
                m.SendMessage("You have not filed a page for help.");
            }
            else if (entry.Sender.NetState != null && index > -1)
            {
                m.SendMessage("Queue status: {0}", (index + 1).ToString());
            }
        }

        [Usage("GetRep")]
        [Description("Reports the player-voted reputation of the targeted staff member.")]
        private static void GetRep_OnCommand(CommandEventArgs args)
        {
            args.Mobile.SendMessage("Select the staff member.");
            args.Mobile.Target = new InternalTarget();
        }

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Player && ((Mobile)target).AccessLevel >= AccessLevel.Counselor)
                {
                    Player pm = target as Player;

                    from.SendMessage("Positive reputation points: {0}\nNegative reputation points: {1}", pm.RepGood, pm.RepBad);
                }
                else
                {
                    from.SendMessage("This command will only work on staff members.");
                }
            }
        }

        [Usage("Pages")]
        [Description("Displays all open help pages.")]
        private static void Pages_OnCommand(CommandEventArgs args)
        {
            Mobile m = args.Mobile;
            PageEntry entry = PageQueue.GetEntry(m);

            if (entry != null)
            {
                m.SendGump(new PageEntryGump(m, entry));
            }
            else if (PageQueue.List.Count > 0)
            {
                m.SendGump(new PageQueueGump());
            }
            else
            {
                m.SendMessage("The page queue is empty.");
            }
        }
        #endregion
    }
}