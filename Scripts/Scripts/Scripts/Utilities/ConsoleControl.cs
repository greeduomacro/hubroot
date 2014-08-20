using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Server;
using Server.Accounting;
using Server.Network;
using Server.Utilities.Logging;

namespace Server.Utilities
{
    public class ConsoleControl
    {
        public const bool Enabled = true;

        [CallPriority(-10)]
        public static void Initialize()
        {
            Console.WriteLine("Console Control: System {0}", (Enabled ? "enabled" : "disabled"));

            if( Enabled )
                EventSink.ServerStarted += new ServerStartedEventHandler(OnServerStart);
        }

        private static void OnServerStart()
        {
            Thread th = new Thread(new ThreadStart(InitialThread));
            th.Start();
        }

        private static void InitialThread()
        {
            try { DoCommand(Console.ReadLine(), true); }
            catch( Exception e ) { ExceptionManager.LogException("ConsoleControl.cs", e); }
        }

        public static void DoCommand( string line, bool restartThread )
        {
            bool cmdUsed = false;

            for( int i = 0; !cmdUsed && i < ConsoleCommand.AllCommands.Length; i++ )
            {
                if( ConsoleCommand.AllCommands[i].Command.ToLower() == line.ToLower() )
                {
                    cmdUsed = true;
                    ConsoleCommand.AllCommands[i].OnUse();
                }
                else if( ConsoleCommand.AllCommands[i].Aliases != null )
                {
                    for( int j = 0; j < ConsoleCommand.AllCommands[i].Aliases.Length; j++ )
                    {
                        if( ConsoleCommand.AllCommands[i].Aliases[j].ToLower() == line.ToLower() )
                        {
                            cmdUsed = true;
                            ConsoleCommand.AllCommands[i].OnUse();
                        }
                    }
                }
            }

            if( !cmdUsed )
                Console.WriteLine("Invalid console command specified.");

            //lil bit buggy - sometimes this thread will capture input before others (before kick/ban thread, for instance)
            //ToDo: solve by waiting for other threads to finish, or restart this thread in each callback
            if( restartThread )
            {
                Thread thread = new Thread(new ThreadStart(InitialThread));
                thread.Start();
            }
        }
    }

    public class ConsoleCommand
    {
        public delegate void CommandCallback();

        private string _command;
        private string[] _aliases;
        private string _description;
        private CommandCallback _callback;

        public string Command { get { return _command; } }
        public string[] Aliases { get { return _aliases; } }
        public string Description { get { return _description; } }

        public ConsoleCommand( string command, string[] aliases, string description, CommandCallback callback )
        {
            _command = command;
            _aliases = aliases;
            _description = description;
            _callback = callback;
        }

        public void OnUse()
        {
            if( _callback != null )
                _callback();
        }

        public static ConsoleCommand[] AllCommands = new ConsoleCommand[]
				{
					new ConsoleCommand( "Shutdown",		new string[]{ "SNS", "Exit" },	"Shuts down the server without saving",						new CommandCallback( ShutdownNoSaveCallback ) ),
					new ConsoleCommand( "Restart",		new string[]{ "RNS" },			"Restarts the server without saving",						new CommandCallback( RestartNoSaveCallback ) ),
					new ConsoleCommand( "Who",			new string[]{ "W", "Online" },	"Displays the user count and status",						new CommandCallback( WhoCallback ) ),
					new ConsoleCommand( "Broadcast",	new string[]{ "BCast", "B" },	"Broadcasts a message to all online users",					new CommandCallback( BroadcastCallback ) ),
					new ConsoleCommand( "Clear",		new string[]{ "cls" },			"Clears the console buffer",								new CommandCallback( ClearCallback ) ),
					new ConsoleCommand( "Kick",			null,							"Prompts to kick an online account",						new CommandCallback( KickCallback ) ),
					new ConsoleCommand( "Ban",			null,							"Prompts to ban an online account",							new CommandCallback( BanCallback ) ),
					new ConsoleCommand( "StartListen",	null,							"Enables console display of world speech",					new CommandCallback( StartListenCallback ) ),
					new ConsoleCommand( "StopListen",	null,							"Disables console display of world speech",					new CommandCallback( StopListenCallback ) ),
					new ConsoleCommand( "DocGen",		null,							"Generates documentation for all types in this assembly",	new CommandCallback( DocGenCallback ) ),
					new ConsoleCommand( "Help",			new string[]{ "H", "?" },		"Displays all console commands and their descriptions",		new CommandCallback( HelpCallback ) ),
					new ConsoleCommand( "ResetPW",		null,							"Resets the password for the given account",				new CommandCallback( ResetPasswordCallback ) )
				};

        #region ShutdownNoSave
        private static void ShutdownNoSaveCallback()
        {
            Core.Process.Kill();
        }
        #endregion

        #region RestartNoSave
        private static void RestartNoSaveCallback()
        {
            Process.Start(Core.ExePath);
            Core.Process.Kill();
        }
        #endregion

        #region Who
        private static void WhoCallback()
        {
            List<NetState> list = NetState.Instances;

            if( list.Count <= 0 )
                Console.WriteLine("There are no users currently online.");
            else
            {
                Console.WriteLine("Online user(s):");

                for( int i = 0; i < list.Count; i++ )
                {
                    Account acct = ((Account)list[i].Account);
                    Mobile m = list[i].Mobile;

                    if( acct == null || m == null )
                        continue;

                    string region = (m.Region == null ? "an unknown region" : m.Region.Name == null ? "an unknown region" : m.Region.Name);

                    Console.WriteLine("  ({0}) {1} in {2}", acct.Username, m.RawName, (region == "" ? "an unknown region" : region));
                }
            }
        }
        #endregion

        #region Broadcast
        private static void BroadcastCallback()
        {
            Console.Write("Enter the message to broadcast: ");

            Thread th = new Thread(new ThreadStart(BroadcastMessage));
            th.Start();
        }

        private static void BroadcastMessage()
        {
            string toSend = Console.ReadLine();

            foreach( NetState state in NetState.Instances )
            {
                if( state.Mobile != null )
                    state.Mobile.SendMessage(0x482, toSend);
            }

            Console.WriteLine("Message sent.");
        }
        #endregion

        #region Clear
        private static void ClearCallback()
        {
            Console.Clear();
        }
        #endregion

        #region Kick
        private static void KickCallback()
        {
            Console.Write("Enter the account name of the user you wish to kick: ");

            Thread th = new Thread(new ThreadStart(DoKick));
            th.Start();
        }

        private static void DoKick()
        {
            string acct = Console.ReadLine();
            bool success = false;

            foreach( NetState state in NetState.Instances )
            {
                if( state.Mobile != null )
                {
                    if( String.Compare(acct, ((Account)state.Mobile.Account).Username, true) == 0 )
                    {
                        state.Dispose(true);

                        Console.WriteLine("Account {0} has been disconnected.", ((Account)state.Mobile.Account).Username);

                        success = true;
                        break;
                    }
                }
            }

            if( !success )
                Console.WriteLine("Failed to kick account {0}.", acct);
        }
        #endregion

        #region Ban
        private static void BanCallback()
        {
            Console.Write("Enter the account username to ban: ");

            Thread th = new Thread(new ThreadStart(DoBan));
            th.Start();
        }

        private static void DoBan()
        {
            string acct = Console.ReadLine();
            bool success = false;

            foreach( NetState state in NetState.Instances )
            {
                if( state.Mobile != null )
                {
                    if( String.Compare(acct, ((Account)state.Mobile.Account).Username, true) == 0 )
                    {
                        ((Account)state.Mobile.Account).Banned = true;
                        state.Dispose(true);

                        Console.WriteLine("Account {0} has been banned and disconnected.", ((Account)state.Mobile.Account).Username);

                        success = true;
                        break;
                    }
                }
            }

            if( !success )
                Console.WriteLine("Failed to ban account {0}.", acct);
        }
        #endregion

        #region StartListen
        private static void StartListenCallback()
        {
            SpeechLogging.SetConsoleListen(true);
        }
        #endregion

        #region StopListen
        private static void StopListenCallback()
        {
            SpeechLogging.SetConsoleListen(false);
        }
        #endregion

        #region DocGen
        private static void DocGenCallback()
        {
            Console.WriteLine("Generating assembly documentation...");
            World.Broadcast(0x35, true, "Server documentation is being generated. Please wait...");

            NetState.FlushAll();
            NetState.Pause();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            if( Server.Commands.Docs.Document() )
                Console.WriteLine("done ({0:F1} seconds)", (sw.ElapsedMilliseconds / 1000));
            else
                Console.WriteLine("failed ({0:F1} seconds)", (sw.ElapsedMilliseconds / 1000));

            NetState.Resume();

            World.Broadcast(0x35, true, "Documentation complete.");
        }
        #endregion

        private static void HelpCallback()
        {
            Utility.PushColor(ConsoleColor.White);
            Console.WriteLine("Available console commands:");

            for( int i = 0; i < AllCommands.Length; i++ )
                Console.WriteLine("{0,-15} {1,-15} - {2,-30}", AllCommands[i].Command, GetAliases(AllCommands[i]), AllCommands[i].Description);
            Utility.PopColor();
        }

        private static string GetAliases( ConsoleCommand cmd )
        {
            if( cmd.Aliases == null )
                return "[no aliases]";

            string str = "";

            for( int i = 0; i < cmd.Aliases.Length; i++ )
            {
                str += cmd.Aliases[i];

                if( i != (cmd.Aliases.Length - 1) )
                    str += ",";
            }

            return String.Format("[{0}]", str);
        }

        #region ResetPW
        private static void ResetPasswordCallback()
        {
            Console.WriteLine("Enter the account username and new password, separated by a pipe: ");

            Thread th = new Thread(new ThreadStart(DoResetPassword));
            th.Start();
        }

        private static void DoResetPassword()
        {
            string[] input = Console.ReadLine().Split(new char[] { '|' }, 2);

            if( input.Length == 2 )
            {
                string username = input[0], password = input[1].Trim();

                IAccount account = Accounts.GetAccount(username);

                if( account != null )
                {
                    account.SetPassword(password);
                    Accounts.Save(null);
                    Console.WriteLine("Password reset completed for '{0}'", username);
                }
            }
            else
            {
                Console.WriteLine("Error: bad input exception");
            }
        }
        #endregion
    }
}