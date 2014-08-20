using System;
using System.IO;
using System.Threading;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Utilities
{
    public class MessageOfTheDay : Gump
    {
        private const string _path = "Data/Message of the Day/";
        private const string _archives = _path + "Archives/";
        private const int _maxArchives = 15;

        private const string DefaultBody = "There is no current news available.";
        private const int LabelHue = 0x480;

        private static volatile string[] _archiveNameCache;
        private static volatile string _msgCache;
        private static volatile Mutex _mutex = new Mutex();

        #region Initialization
        public static void Initialize()
        {
            try
            {
                if( !Directory.Exists(_archives) )
                    Directory.CreateDirectory(_archives);

                LoadMessage();
                RetrieveArchives();
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("MessageOfTheDay.cs", e);
            }

            EventSink.Login += new LoginEventHandler(OnLogin);
            CommandSystem.Register("MotD", AccessLevel.Player, new CommandEventHandler(MOTD_OnCommand));
        }

        private static void OnLogin( LoginEventArgs args )
        {
            if( args.Mobile != null && args.Mobile.Account != null && args.Mobile.Account is Account )
            {
                if( Convert.ToBoolean(((Account)args.Mobile.Account).GetTag("MotD")) )
                {
                    SendGump(args.Mobile);
                }
            }
        }
        #endregion

        #region LoadMessage
        private static bool LoadMessage()
        {
            string str = "";

            try
            {
                if( File.Exists(Path.Combine(_path, "message.txt")) )
                {
                    using( StreamReader reader = new StreamReader(Path.Combine(_path, "message.txt")) )
                    {
                        str = reader.ReadToEnd();
                        reader.Close();
                    }
                }
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("MessageOfTheDay.cs", e);
            }
            finally
            {
                _msgCache = (str == "" ? DefaultBody : str);
            }

            return !(_msgCache == DefaultBody);
        }
        #endregion

        #region ReloadMotD
        private static void ReloadMotD()
        {
            bool showMsg = true;

            _mutex.WaitOne();

            try
            {
                showMsg = LoadMessage();
                RetrieveArchives();
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("MessageOfTheDay.cs", e);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            Account acct;

            foreach( Mobile m in World.Mobiles.Values )
            {
                if( m == null || !(m is PlayerMobile) )
                    continue;
                else if( (acct = ((Account)m.Account)) == null )
                    continue;

                if( acct.GetTag("MotD") != null )
                    acct.RemoveTag("MotD");

                acct.SetTag("MotD", showMsg.ToString());
            }

            World.Broadcast(0x482, false, "Notice: the message of the day has been updated. Type \"[MotD\" to view this message.");
        }
        #endregion

        #region Archiving
        private static void RetrieveArchives()
        {
            _archiveNameCache = null;
            _archiveNameCache = Directory.GetFiles(_archives, "*.txt");
        }

        private static void ArchiveMotD()
        {
            bool showMsg = true;

            _mutex.WaitOne();

            try
            {
                PerformArchive();
                showMsg = LoadMessage();
                RetrieveArchives();
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("MessageOfTheDay.cs", e);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            Account acct;

            foreach( Mobile m in World.Mobiles.Values )
            {
                if( m == null || !(m is PlayerMobile) )
                    continue;
                else if( (acct = ((Account)m.Account)) == null )
                    continue;

                if( acct.GetTag("MotD") != null )
                    acct.RemoveTag("MotD");

                acct.SetTag("MotD", showMsg.ToString());
            }
        }

        private static void PerformArchive()
        {
            string[] files = Directory.GetFiles(_archives, "*.txt");
            int totalFiles = files.Length;
            string src;

            for( int i = totalFiles, j = i + 1; i > 0; i--, j-- )
            {
                if( File.Exists((src = _archives + i + ".txt")) )
                    File.Move(src, _archives + j + ".txt");
            }

            if( File.Exists((src = _path + "message.txt")) )
                File.Move(src, _archives + "1" + ".txt");

            if( File.Exists((src = _path + "newMsg.txt")) )
                File.Move(src, _path + "message.txt");
        }
        #endregion

        #region MotD Command
        [Usage("MotD")]
        [Description("Loads the current Message of the Day for the calling user.")]
        private static void MOTD_OnCommand( CommandEventArgs args )
        {
            SendGump(args.Mobile);
        }

        private static void SendGump( Mobile m )
        {
            MessageOfTheDay gump = null;

            _mutex.WaitOne();

            try
            {
                int size = _archiveNameCache.Length >= _maxArchives ? _maxArchives : _archiveNameCache.Length;

                gump = new MessageOfTheDay(m, _msgCache, _archiveNameCache, size);
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("MessageOfTheDay.cs", e);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            if( gump != null && (Account)m.Account != null )
            {
                Account acct = (Account)m.Account;

                gump.BuildGump();
                m.SendGump(gump);

                if( acct.GetTag("MotD") != null )
                    acct.RemoveTag("MotD");

                acct.SetTag("MotD", "false");
            }
        }
        #endregion

        #region Gump construction
        private Mobile _from;
        private string _body;
        private string[] _archiveList;
        private int _archiveCount;
        private int _archivesLoaded;
        private int _totalPages;

        public MessageOfTheDay( Mobile m, string body, string[] archiveList, int count )
            : base(0, 0)
        {
            _from = m;
            _from.CloseGump(typeof(MessageOfTheDay));

            _body = body;
            _archiveList = archiveList;
            _archiveCount = (archiveList == null ? 0 : count);
            _archivesLoaded = 0;
            _totalPages = 0;
        }

        private void BuildGump()
        {
            AddNewPage(_body, true);

            while( _archivesLoaded < _archiveCount )
            {
                AddNewPage(GrabNextArchive(), false);
            }
        }

        private void AddNewPage( string text, bool mainPage )
        {
            AddPage(++_totalPages);

            AddBackground(10, 10, 400, 370, 9250);
            //AddLabel( 75, 25, LabelHue, "The Genesis Roleplaying Shard" );
            AddLabel(135, 43, LabelHue, "Announcement Center");

            if( _from.AccessLevel >= AccessLevel.Administrator )
            {
                AddButton(25, 25, 4011, 4013, 1, GumpButtonType.Reply, 0);	//reload
                AddButton(25, 50, 4014, 4016, 2, GumpButtonType.Reply, 0);	//archive
            }

            if( mainPage )
                AddLabel(25, 70, LabelHue, "Current Shard News:");
            else
                AddLabel(25, 70, LabelHue, "Previous News:");

            AddLabel(315, 21, LabelHue, String.Format("Page {0} of {1}", _totalPages, _archiveCount + 1));

            AddHtml(25, 90, 370, 245, String.Format(text), false, true);
            AddAlphaRegion(25, 90, 370, 245);

            if( _archiveCount > 0 )
            {
                if( _totalPages < (_archiveCount + 1) )
                {
                    AddLabel(280, 342, LabelHue, "Previous News");
                    AddButton(375, 345, 5601, 5605, 0, GumpButtonType.Page, _totalPages + 1);
                }

                if( _totalPages > 1 )
                    AddButton(30, 345, 5603, 5607, 0, GumpButtonType.Page, _totalPages - 1);
            }
        }

        private string GrabNextArchive()
        {
            string nextArchive = "";
            string text = "";

            if( _archiveList != null && _archivesLoaded < _archiveCount && ((nextArchive = _archiveList[_archivesLoaded].ToString()) != null)
               && File.Exists(nextArchive) )
            {
                StreamReader reader = new StreamReader(nextArchive);

                text = reader.ReadToEnd();

                reader.Close();
                _archivesLoaded++;
            }

            return (text == "" ? DefaultBody : text);
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            if( info.ButtonID == 1 )
            {
                try
                {
                    ReloadMotD();
                    SendGump(sender.Mobile);
                }
                catch( Exception e )
                {
                    Console.WriteLine("OnResponse reload error: {0}", e);
                }
            }
            else if( info.ButtonID == 2 )
            {
                try
                {
                    ArchiveMotD();
                    SendGump(sender.Mobile);
                }
                catch( Exception e )
                {
                    Console.WriteLine("OnResponse archive error: {0}", e);
                }
            }
        }
        #endregion
    }
}
