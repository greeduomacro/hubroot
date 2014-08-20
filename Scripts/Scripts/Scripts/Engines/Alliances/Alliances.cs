using System;
using Server;
using Server.Guilds;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Misc;
using Server.Commands;
using Server.Targeting;
using System.IO;
using System.Xml;
using System.Text;
using Server.Accounting;
using System.Globalization;
using Server.Regions;

//Add to Player.cs If using UOAberration, PlayerMobile.cs if using RunUO

//using Aberration.Alliances
//private Alliance currentAlliance;

 //       public Alliance CurrentAlliance
 //       {
 //           set { currentAlliance = value; }

 //           get { return currentAlliance; }
 //       }

 //       [CommandProperty(AccessLevel.GameMaster)]
 //       public String AllianceName
 //       {      
 //           get 
 //           {
 //               if (currentAlliance != null)
 //                   return currentAlliance.AllianceName;

 //               else return null;
 //           }
 //       }

//Add to Notoriety.cs

//using Aberration.Alliances;
//Add to Notoriety.MobileNotoriety( Mobile source, Mobile target )

//            if(source is Player && target is Player)
//            {
//                Alliance sourceAlliance = ((Player)source).CurrentAlliance;
//                Alliance targetAlliance = ((Player)target).CurrentAlliance;

//                if(sourceAlliance != null && targetAlliance != null)
//                {
//                    if (sourceAlliance == targetAlliance)
//                    {
//                        return Notoriety.Ally;
//                    }

//                    if (sourceAlliance != targetAlliance || targetAlliance != sourceAlliance)
//                    {
//                        return Notoriety.Enemy;
//                    }
//                }
//            }

namespace Aberration.Alliances
{
    public enum Rank
    {
        None,
        Henchman,
        Warrior,
        Soldier,
        Lieutenant,
        Knight,
    }

    public class BaseAlliance
    {
        string serialString;

        Serial serial;

        public List<Guild> childGuilds = new List<Guild>();
        public List<Mobile> membersOf = new List<Mobile>();

        public List<string> memberSerials = new List<string>();

        string allianceName;

        Player allianceLeader;
        Player allianceSheriff;
        Player allianceMinister;

        public string leaderSerial;
        public string sheriffSerial;
        public string ministerSerial;

        public int primaryHue = 0;
        public int secondaryHue = 0;
        public int mountBody = 0;
        public int mountID = 0;
        public int emblemID = 0;

        public int CitiesControlled;

        public int TotalSilverGathered
        {
            get
            {
                int total = 0;

                for (int x = 0; x < membersOf.Count; x++)
                {
                    if (membersOf[x] is Player)
                    {
                        Player p = membersOf[x] as Player;
                        total += p.allyState.totalSilver;
                    }
                }

                return total;
            }
        }

        public int CollectivePoints
        {
            get
            {
                int total = 0;

                for (int i = 0; i < membersOf.Count; i++ )
                {
                    if (membersOf[i] is Player)
                    {
                        Player p = membersOf[i] as Player;
                        total += p.allyState.killPoints;
                    }
                }

                return total;
            }
        }

        public Player AllianceSheriff
        {
            get { return allianceSheriff; }
            set { allianceSheriff = value; }
        }

        public Player AllianceMinister
        {
            get { return allianceMinister; }
            set { allianceMinister = value; }
        }

        public Player AllianceLeader
        {
            get { return allianceLeader; }
            set { allianceLeader = value; }
        }

        public String AllianceName
        {
            get { return allianceName; }
            set { allianceName = value; }
        }

        public Serial Serial
        {
            get { return serial; }
            set { serial = value; }
        }

        public string SerialString
        {
            get { return serialString; }
            set { serialString = value; }
        }

        ~BaseAlliance()
        {
            Console.WriteLine("The Alliance '{0}' has been disbanded.", allianceName);
        }

        public void Serialize(BinaryFileWriter writer)
        {
            writer.Write((int)0); //Version

            writer.Write((string)serialString.ToString());
            writer.WriteMobile(allianceLeader);

            writer.Write((int)membersOf.Count);

            foreach (Mobile m in membersOf)
            {
                writer.WriteMobile(m);
            }

            writer.Write((int)childGuilds.Count);

            foreach (Guild g in childGuilds)
            {
                writer.WriteGuild(g);
            }

            writer.Write((string)allianceName);
            writer.Write((int)primaryHue);
            writer.Write((int)secondaryHue);
            writer.Write((int)mountBody);
            writer.Write((int)mountID);

        }

        public void Deserialize(BinaryFileReader reader)
        {
            int version = reader.ReadInt();

            if (version >= 0)
            {
                serialString = reader.ReadString();

                allianceLeader = reader.ReadMobile() as Player;

                int count = reader.ReadInt();

                for (int n = 1; n <= count; n++)
                {
                    membersOf.Add(reader.ReadMobile());
                }

                int guildCount = reader.ReadInt();

                for (int x = 1; x <= guildCount; x++)
                {
                    childGuilds.Add(reader.ReadGuild() as Guild);
                }

                allianceName = reader.ReadString();
                primaryHue = reader.ReadInt();
                secondaryHue = reader.ReadInt();
                mountBody = reader.ReadInt();
                mountID = reader.ReadInt();

                foreach (Mobile m in membersOf)
                {
                    if (m is Player)
                    {
                        Player p = m as Player;
                        p.CurrentAlliance = this;
                    }
                }
            }
        }

        public void Add(Mobile m)
        {
            if (m is Player && ((Player)m).CurrentAlliance == null)
            {
                ((Player)m).CurrentAlliance = this;
                membersOf.Add(m);
                ((PlayerMobile)m).ValidateEquipment();
            }
        }

        public void AssimilateGuild(Mobile m)
        {
            if (m.Guild is Guild)
            {
                Guild g = m.Guild as Guild;

                childGuilds.Add(g);

                foreach (Mobile mob in g.Members)
                {
                    if (m != mob && m is Player && ((Player)m).CurrentAlliance == null)
                    {
                        membersOf.Add(m);

                        AllianceState alState = new AllianceState();

                        if (m is Player)
                        {
                            ((Player)m).CurrentAlliance = this;
                            ((Player)m).allyState = alState;
                        }

                        m.SendMessage("Your guild has joined the {0} alliance.", this.AllianceName);
                    }
                }
            }
        }

        public void QueryMember(Mobile m)
        {
            if (memberSerials.Count > 0)
            {
                foreach (string s in memberSerials)
                {
                    if (m.Serial.ToString() == s && m is Player && ((Player)m).CurrentAlliance == null)
                    {
                        membersOf.Add(m);

                        ((Player)m).CurrentAlliance = this;
                    }
                }

                if (m.Serial.ToString() == leaderSerial && m is Player)
                    AllianceLeader = m as Player;

                if (m.Serial.ToString() == ministerSerial && m is Player)
                    AllianceMinister = m as Player;

                if (m.Serial.ToString() == sheriffSerial && m is Player)
                    allianceSheriff = m as Player;

                AssimilateGuild(m);
            }
        }

        public void Sanitize()
        {
            foreach (PlayerMobile p in membersOf)
            {
                if (p is Player)
                {
                    ((Player)p).CurrentAlliance = null;
                    p.SendMessage("The alliance you claim fealty to has been dissolved.");
                }
            }

            childGuilds.Clear();
            membersOf.Clear();

            AllianceDefinition.Alliances.Remove(this);
        }
    }    

    public class AllianceDefinition
    {
        public static List<BaseAlliance> Alliances = new List<BaseAlliance>();

        private static readonly string dataPath = "Data";
        private static readonly string allianceXML = Path.Combine(dataPath, "alliances.xml");

        private static readonly string SavePath = "Saves\\Alliances";
        private static readonly string SaveFile = Path.Combine(SavePath, "alliances.bin");

        public static bool useXML = false;
        public static int AllianceLimit = 11;

        private static void Event_WorldLoad()
        {
            if (!File.Exists(SaveFile) || useXML)
                return;

            try
            {

                using (FileStream stream = new FileStream(SaveFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryFileReader reader = new BinaryFileReader(new BinaryReader(stream));

                    Deserialize(reader);
                }
            }

            catch { Console.WriteLine("Error: Event_WorldLoad Failed in Alliance Definition."); }
        }

        private static void Event_WorldSave(WorldSaveEventArgs args)
        {
            try
            {
                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);

                BinaryFileWriter writer = new BinaryFileWriter(SaveFile, true);

                Serialize(writer);

                ConstructXML();
            }

            catch { Console.WriteLine("Error: Event_WorldSave Failed in Alliance Definition."); }
        }

        private static void ConstructXML()
        {
            if (Alliances.Count > 0)
            {
                if (!Directory.Exists(dataPath))
                {
                    try { Directory.CreateDirectory(dataPath); }

                    catch { Console.WriteLine("Error: Unable to create new directory."); }
                }

                using (StreamWriter writer = new StreamWriter(allianceXML))
                {
                    try
                    {
                        XmlTextWriter xml = new XmlTextWriter(writer);

                        xml.Formatting = Formatting.Indented;
                        xml.IndentChar = '\t';
                        xml.Indentation = 1;

                        xml.WriteStartDocument(true);
                        xml.WriteStartElement("Alliances");
                        xml.WriteAttributeString("limit", AllianceLimit.ToString());

                        foreach (BaseAlliance a in Alliances)
                        {
                            xml.WriteStartElement("alliance");

                            xml.WriteAttributeString("serial", a.Serial.ToString());
                            xml.WriteAttributeString("name", a.AllianceName.ToString());
                            xml.WriteAttributeString("leader", a.AllianceLeader.Serial.ToString());

                            if (a.AllianceSheriff != null)
                                xml.WriteAttributeString("general", a.AllianceSheriff.Serial.ToString());

                            if (a.AllianceMinister != null)
                                xml.WriteAttributeString("magistrate", a.AllianceMinister.Serial.ToString());

                            if (a.primaryHue != 0)
                                xml.WriteAttributeString("hue", a.primaryHue.ToString());

                            if (a.mountID != 0)
                                xml.WriteAttributeString("mountID", a.mountID.ToString());

                            if (a.membersOf.Count >= 1)
                            {
                                StringBuilder builder = new StringBuilder();

                                foreach (Mobile m in a.membersOf)
                                {
                                    builder.Append(m.Serial.ToString()).Append(", ");
                                }

                                string memberList = builder.ToString();
                                memberList = memberList.Remove(memberList.Length - 2);

                                builder.Clear();

                                xml.WriteString(memberList);
                            }

                            xml.WriteEndElement();
                        }

                        xml.WriteEndElement();
                        xml.WriteEndDocument();
                        xml.Close();
                    }

                    catch
                    {
                        //Pointless, Will Crash
                        Console.WriteLine("Error: Unable to create new xml file.");
                    }

                }
            }
        }  

        public static void Configure()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(Event_WorldSave);
            EventSink.WorldLoad += new WorldLoadEventHandler(Event_WorldLoad);
        }

        public static void Initialize()
        {
            if (useXML)
            {
                Console.Write("Alliances: Constructing from {0}... ", allianceXML);
                ConstructAlliances();
            }
        }

        private static void ConstructAlliances()
        {
            if (!File.Exists(allianceXML))
                return;

            XmlDocument doc = new XmlDocument();
            XmlElement root;

            try
            {
                doc.Load(allianceXML);
                root = doc["Alliances"];

                AllianceLimit = Convert.ToInt32(root.GetAttribute("limit"));

                foreach (XmlElement ele in root.GetElementsByTagName("alliance"))
                {
                    BaseAlliance alliance = new BaseAlliance();

                    alliance.AllianceName = ele.Attributes["name"].Value;
                    alliance.SerialString = ele.Attributes["serial"].Value;
                    alliance.leaderSerial = ele.Attributes["leader"].Value;

                    string serials = ele.InnerText.ToString();

                    string[] serialStrings = serials.Split(',');

                    foreach (string s in serialStrings)
                    {
                        string serial = s.Trim();

                        alliance.memberSerials.Add(serial);
                    }

                    Alliances.Add(alliance);
                }
            }

            catch { }
        }

        public static void Serialize(BinaryFileWriter writer)
        {
            writer.Write((int)0); //Version

            writer.Write((int)AllianceLimit);
            writer.Write((bool)useXML);
            writer.Write((int)Alliances.Count);

            foreach (BaseAlliance a in Alliances)
            {
                a.Serialize(writer);
            }

            writer.Close();
        }

        public static void Deserialize(BinaryFileReader reader)
        {
            int version = reader.ReadInt();

            if (version >= 0)
            {
                AllianceLimit = reader.ReadInt();
                useXML = reader.ReadBool();
                int count = reader.ReadInt();

                for (int i = 1; i <= count; i++)
                {
                    BaseAlliance alliance = new BaseAlliance();
                    alliance.Deserialize(reader);
                    Alliances.Add(alliance);
                }
            }

            reader.Close();
        }
    }

    public class AllianceHandler
    {
        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(HookSpeech);
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            if (AllianceDefinition.useXML)
            {
                foreach (BaseAlliance a in AllianceDefinition.Alliances)
                {
                    a.QueryMember(args.Mobile);
                }
            }
        }

        public static void HookSpeech(SpeechEventArgs e)
        {
            if (e.Speech.ToLower().IndexOf("i wish to form an alliance") >= 0)
            {
                AttemptInitialization(e);
            }

            if (e.Speech.ToLower().IndexOf("i wish to disband my alliance") >= 0)
            {
                CheckLeader(e);
            }

            if (e.Speech.ToLower().IndexOf("i wish to form a pact") >= 0)
            {
                AttemptPact(e);
            }
        }    

        public static void AttemptPact(SpeechEventArgs e)
        {
            if (e.Mobile is Player && ((Player)e.Mobile).CurrentAlliance == null)
                return;

            if (e.Mobile is Player && ((Player)e.Mobile).CurrentAlliance.AllianceLeader == e.Mobile)
            {
                e.Mobile.Target = new PactTarget(e.Mobile);
            }

            else e.Mobile.SendMessage("You must be the leader of an alliance to form pacts with other guilds.");
        }

        public static void AttemptInitialization(SpeechEventArgs e)
        {
            Player caller = e.Mobile as Player;

            if (caller.CurrentAlliance == null)
            {
                QueryGuild(caller);
            }

            else caller.SendMessage("You are already in an alliance!");
        }

        public static void CheckLeader(SpeechEventArgs e)
        {
            Player caller = e.Mobile as Player;

            if (caller.CurrentAlliance == null)
            {
                caller.SendMessage("You are not in an alliance.");
                return;
            }

            if (caller.CurrentAlliance.AllianceLeader == caller)
            {
                caller.SendGump(new ConfirmSanitize(caller));
            }

            else caller.SendMessage("You must be the leader of your alliance to disband it.");
        }

        public static void QueryGuild(PlayerMobile m)
        {
            PlayerMobile p = m;

            if (p.Guild == null && p.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("You must be a guild leader to form an alliance.");
                return;
            }

            if (((p.Guild != null && p.Guild is Guild) && ((Guild)p.Guild).Leader == p)
                || p.AccessLevel > AccessLevel.Counselor)
            {
                if (AllianceDefinition.Alliances.Count >= AllianceDefinition.AllianceLimit)
                {
                    m.SendMessage("There are currently too many alliances to form another. Please try again later");
                    return;
                }

                if (AllianceDefinition.Alliances.Count < AllianceDefinition.AllianceLimit)
                    p.SendGump(new AllianceNameQuery());
            }
        }
        
        private class PactTarget : Target
        {
            public PactTarget(Mobile m)
                : base(12, true, TargetFlags.None)
            {
                m.SendMessage("Select the guild leader you wish to invite into your alliance.");
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Player && ((Player)o).Guild is Guild)
                {
                    Player target = o as Player;

                    if (((Guild)target.Guild).Leader == target)
                    {
                        if (from is Player)
                        {
                            target.SendGump(new JoinAllianceQuery(from));
                        }
                    }

                    else from.SendMessage("You must target a guild leader.");
                }

                else from.SendMessage("You must target a guild leader.");
            }
        }
    }

    public class InitiateInterface
    {
        [CommandAttribute("Alliances", AccessLevel.Administrator)]
        public static void AllyInterface_Command(CommandEventArgs args)
        {
            InterfaceKey key = new InterfaceKey();
            args.Mobile.SendGump(new AllianceInterface(args.Mobile, key));
        }
    }

    public class InterfaceKey
    {
        int val;

        public Int32 Value
        {
            get { return val; }
            set { val = value; }
        }
    }

    public class AllianceState
    {
        Rank playerRank;
        BaseAlliance alliance;

        public int killPoints;
        public int totalSilver;
        public int pointDeficit;

        public BaseAlliance Alliance
        {
            get
            {   return alliance;    }

            set
            {   alliance = value;   }
        }
    }


    public class SigilRegion : GuardedRegion
    {
        string region_Name = "";
        Rectangle3D[] region_Area;
        Map region_map;

        public SigilRegion( string name, Map map, int priority, Rectangle3D[] area ) 
            : base (name, map, priority, area)
        {
            region_Name = name;
            region_Area = area;
            region_map = map;
        }

        public override void OnExit(Mobile m)
        {
            foreach (Item i in m.Backpack.AcquireItems())
            {
                if (i is TownSigil)
                {
                    ((TownSigil)i).ReturnHome();
                    m.SendMessage("By leaving " + region_Name + ", you return the town's sigil to it's base location.");
                }
            }

            base.OnExit(m);
        }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage("You have entered " + region_Name + ".");
            base.OnEnter(m);
        }
    }

    public class TownSigil : Item
    {
        public int home_X, home_Y, home_Z;

        int min_z = -120, max_z = 120;

        BaseAlliance controlling_Alliance;

        SigilRegion region;
        Rectangle3D[] region_Area;

        string name = "";

        [CommandProperty(AccessLevel.GameMaster)]
        public string RegionName
        {
            get { return name; }
            set { name = value; } 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle3D[] RegionArea
        {
            get { return region_Area; }
            set { region_Area = value; }
        }

        [Constructable]
        public TownSigil()
        {
            ItemID = 1801;
            Movable = false;
        }

        public void TargetArea(Mobile m)
        {
            BoundingBoxPicker.Begin(m, new BoundingBoxCallback(DefineArea), this);
        }

        public void DefineArea(Mobile from, Map map, Point3D start, Point3D end, object control)
        {
            if (this != null)
            {
                List<Rectangle3D> areas = new List<Rectangle3D>();

                if (RegionArea != null)
                {
                    foreach (Rectangle3D rect in RegionArea)
                        areas.Add(rect);
                }

                Rectangle3D newrect = new Rectangle3D(new Point3D(start, min_z), new Point3D(end, min_z));
                areas.Add(newrect);

                RegionArea = areas.ToArray();

                UpdateRegion();
            }
        }

        public void UpdateRegion()
        {
            if (region != null)
                region.Unregister();

            if (this.Map != null)
            {
                if (this != null && this.RegionArea != null && this.RegionArea.Length > 0)
                {
                    region = new SigilRegion(RegionName, this.Map, 255, RegionArea);
                    region.Register();
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.GameMaster)
            {
                TargetArea(from);
            }
        }

        public void ReturnHome()
        {
            if(!Deleted)
            MoveToWorld(new Point3D(new Point2D(home_X, home_Y), home_Z));
        }
    }


    public class ConfirmSanitize   : Gump
    {
        Player caller;

        public ConfirmSanitize(Player p)
            : base(50, 50)
        {
            caller = p;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(139, 46, 119, 107, 9200);
            this.AddLabel(154, 60, 0, @"Are You Sure?");
            this.AddButton(168, 121, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
            this.AddButton(168, 89, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            Okay,
            Cancel,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Okay)
            {
                caller.CurrentAlliance.Sanitize();
                caller.SendMessage("You have successfully disbanded your alliance.");
            }
        }
    }

    public class AllianceNameQuery : Gump
    {
        public AllianceNameQuery()
            : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddBackground(65, 62, 344, 104, 9200);
            this.AddLabel(85, 77, 0, @"Name Your Alliance");
            this.AddButton(335, 107, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
            this.AddTextEntry(123, 109, 200, 20, 0, (int)Buttons.TextEntry, @"");
            this.AddImage(69, 116, 52);

        }

        public enum Buttons
        {
            Invalid,
            Okay,
            TextEntry,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Okay && sender.Mobile is Player &&
                !String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.TextEntry).Text))
            {
                String nameRequested = info.GetTextEntry((int)Buttons.TextEntry).Text;

                if (NameVerification.Validate(nameRequested, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote)
                    && AllianceDefinition.Alliances.Count <= AllianceDefinition.AllianceLimit)
                {

                    BaseAlliance alliance = new BaseAlliance();
                    alliance.AllianceName = nameRequested;

                    PlayerMobile pm = sender.Mobile as PlayerMobile;

                    AllianceState alState = new AllianceState();
                    ((Player)pm).allyState = alState;

                    alliance.membersOf.Add(pm);
                    alliance.AllianceLeader = pm as Player;
                    alliance.Serial = pm.Serial;
                    alliance.SerialString = pm.Serial.ToString();

                    if ((Guild)pm.Guild != null)
                        alliance.childGuilds.Add((Guild)pm.Guild);

                    AllianceDefinition.Alliances.Add(alliance);

                    ((Player)pm).CurrentAlliance = alliance;

                    if (pm.Guild != null)
                    {
                        foreach (Mobile m in ((Guild)pm.Guild).Members)
                        {
                            if (m is Player && ((Player)m).CurrentAlliance == null)
                            {
                                Player p = m as Player;

                                p.CurrentAlliance = alliance;
                                alliance.membersOf.Add(p);

                                alState = new AllianceState();
                                p.allyState = alState;

                                p.SendMessage("Your guild has joined a new alliance.");
                            }
                        }
                    }

                    sender.Mobile.SendMessage("You have sucessfully created a new alliance.");
                }

                else sender.Mobile.SendMessage("This is not an acceptable alliance name.");
            }
        }
    }

    public class JoinAllianceQuery : Gump
    {
        BaseAlliance alliance;
        Mobile from;

        public JoinAllianceQuery(Mobile m)
            : base(50, 50)
        {
            if (m is Player)
                alliance = ((Player)m).CurrentAlliance;

            from = m;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(87, 53, 323, 164, 9350);
            this.AddHtml(104, 85, 284, 45, m.Name + @" Is inviting you to join an alliance. Do you accept?", true, true);
            this.AddButton(214, 162, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
            this.AddButton(303, 162, 241, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

        }

        public enum Buttons
        {
            Invalid,
            Okay,
            Cancel,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Okay)
            {
                Player target = sender.Mobile as Player;

                target.CurrentAlliance = alliance;
                Guild g = target.Guild as Guild;

                alliance.childGuilds.Add(g);
                alliance.membersOf.Add(target);

                AllianceState alState = new AllianceState();
                target.allyState = alState;

                alliance.AssimilateGuild(target);

                target.SendMessage("You enter into a pact with the {0}.", alliance.AllianceName);
                from.SendMessage("{0} has accepted your alliance invitation.", target.Name);
            }
        }
    }

    public class AllianceInterface : Gump
    {
        BaseAlliance currentAllianceParsed;

        List<BaseAlliance> alliancesToParse = new List<BaseAlliance>();

        InterfaceKey key;

        public AllianceInterface(Mobile from, InterfaceKey interfaceKey) : base(50, 50)
        {
             key = interfaceKey;

            if (AllianceDefinition.Alliances.Count > 0)
            {
                foreach (BaseAlliance a in AllianceDefinition.Alliances)
                    alliancesToParse.Add(a);

                currentAllianceParsed = alliancesToParse[key.Value];

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                this.AddPage(0);
                this.AddBackground(23, 15, 459, 278, 9250);
                this.AddAlphaRegion(39, 32, 194, 54);
                this.AddLabel(111, 48, currentAllianceParsed.primaryHue -1, currentAllianceParsed.AllianceName);
                this.AddButton(230, 258, 2468, 2466, (int)Buttons.Previous, GumpButtonType.Reply, 0);
                this.AddButton(406, 257, 2469, 2470, (int)Buttons.Next, GumpButtonType.Reply, 0);
                this.AddImage(41, 36, 8481);
                this.AddLabel(47, 257, 0, @"Alliance Limit: ");
                this.AddTextEntry(135, 257, 29, 20, 0, (int)Buttons.AllianceLimitEntry, AllianceDefinition.AllianceLimit.ToString());
                this.AddLabel(85, 98, 0, @"Member Count: " + currentAllianceParsed.membersOf.Count);
                this.AddButton(325, 258, 2463, 2461, (int)Buttons.Delete, GumpButtonType.Reply, 0);
                this.AddLabel(71, 119, 0, @"Cities Controlled: " + currentAllianceParsed.CitiesControlled);
                this.AddLabel(71, 141, 0, @"Collective Points: " + currentAllianceParsed.CollectivePoints);
                this.AddLabel(44, 162, 0, @"Total Silver Amassed: " + currentAllianceParsed.TotalSilverGathered);
                this.AddLabel(291, 37, 0, @"Primary Hue:");
                this.AddTextEntry(382, 36, 52, 20, 0, (int)Buttons.PrimeHueEntry, currentAllianceParsed.primaryHue.ToString());
                this.AddLabel(277, 63, 0, @"Secondary Hue:");
                this.AddTextEntry(382, 63, 52, 20, 0, (int)Buttons.SecondHueEntry, currentAllianceParsed.secondaryHue.ToString());
                this.AddButton(444, 37, 1209, 1210, (int)Buttons.PrimeHueButton, GumpButtonType.Reply, 0);
                this.AddButton(444, 67, 1209, 1210, (int)Buttons.SecondHueButton, GumpButtonType.Reply, 0);
                this.AddButton(173, 261, 1209, 1210, (int)Buttons.AllianceLimitButton, GumpButtonType.Reply, 0);
                this.AddLabel(41, 223, 0, @"Total Alliances: " + alliancesToParse.Count);
                this.AddImage(432, 188, 9005);
            }

            else from.SendMessage("There are currently no alliances with which to interface.");
        }

        public enum Buttons      
        {
            Invalid,
            Previous,
            Next,
            AllianceLimitEntry,
            Delete,
            PrimeHueEntry,
            SecondHueEntry,
            PrimeHueButton,
            SecondHueButton,
            AllianceLimitButton,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Delete)
            {
                currentAllianceParsed.Sanitize();

                if (AllianceDefinition.Alliances.Count > 0)
                {
                    foreach (BaseAlliance a in AllianceDefinition.Alliances)
                        alliancesToParse.Add(a);

                    currentAllianceParsed = alliancesToParse[key.Value];
                    sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));             
                }

                sender.Mobile.SendMessage("Selected alliance has been purged.");
            }

            if (info.ButtonID == (int)Buttons.Next)
            {
                if (key.Value < alliancesToParse.Count -1)
                {
                    sender.Mobile.CloseGump(typeof(AllianceInterface));
                    key.Value++;
                    sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                }
            }

            if (info.ButtonID == (int)Buttons.Previous)
            {            
                if (key.Value > 0)
                {
                    sender.Mobile.CloseGump(typeof(AllianceInterface));
                    key.Value--;
                    sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                }
            }

            if (info.ButtonID == (int)Buttons.PrimeHueButton)
            {
                if(!String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.PrimeHueEntry).Text))
                {
                    int newHue = 0;

                    if (!String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.PrimeHueEntry).Text))
                    {
                        try { newHue = Int32.Parse(info.GetTextEntry((int)Buttons.PrimeHueEntry).Text); }

                        catch
                        {
                            sender.Mobile.SendMessage("Invalid hue entry, please try again.");
                            sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                            return;
                        }

                        if (newHue <= 0 || newHue > 3000)
                        {
                            sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                            sender.Mobile.SendMessage("Hue value out of range (1-3000)");
                            return;
                        }

                        if (newHue > 0 && newHue < 3000)
                            currentAllianceParsed.primaryHue = newHue;

                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                    }
                }
            }

            if (info.ButtonID == (int)Buttons.SecondHueButton)
            {
                if (!String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.SecondHueEntry).Text))
                {
                    int newHue = 0;

                    if (!String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.SecondHueEntry).Text))
                    {
                        try { newHue = Int32.Parse(info.GetTextEntry((int)Buttons.SecondHueEntry).Text); }

                        catch
                        {
                            sender.Mobile.SendMessage("Invalid hue entry, please try again.");
                            sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                            return;
                        }

                        if (newHue <= 0 || newHue > 3000)
                        {
                            sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                            sender.Mobile.SendMessage("Hue value out of range (1-3000)");
                            return;
                        }

                        if (newHue > 0 && newHue < 3000)
                            currentAllianceParsed.secondaryHue = newHue;

                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                    }
                }
            }

            if (info.ButtonID == (int)Buttons.AllianceLimitButton)
            {
                int newLimit = 0;

                if (!String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.AllianceLimitEntry).Text))
                {
                    try
                    {
                        newLimit = Int32.Parse(info.GetTextEntry((int)Buttons.AllianceLimitEntry).Text);
                    }

                    catch
                    {
                        sender.Mobile.SendMessage("You have entered invalid characters.");
                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                        return;
                    }

                    if (newLimit <= 0)
                    {
                        sender.Mobile.SendMessage("The alliance limit must be a number larger than 0");
                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                        return;
                    }

                    if (newLimit < AllianceDefinition.Alliances.Count)
                    {
                        sender.Mobile.SendMessage("The alliance limit can not be less than the number of active alliances.");
                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                        return;
                    }

                    if (newLimit > 0 && newLimit >= AllianceDefinition.Alliances.Count)
                    {
                        AllianceDefinition.AllianceLimit = newLimit;
                        sender.Mobile.SendMessage("The maximum number of alliances has been changed.");
                        sender.Mobile.SendGump(new AllianceInterface(sender.Mobile, key));
                    }
                }
            }
        }
    }

    public class SteedSelection    : Gump
    {
        public SteedSelection()
            : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(9, 11, 411, 177, 9270);
            this.AddAlphaRegion(22, 56, 82, 74);
            this.AddImage(40, 75, 8479);
            this.AddAlphaRegion(123, 56, 82, 74);
            this.AddImage(141, 74, 8480);
            this.AddAlphaRegion(224, 56, 82, 74);
            this.AddImage(242, 74, 8481);
            this.AddAlphaRegion(324, 56, 82, 74);
            this.AddImage(342, 74, 8484);
            this.AddButton( 29, 143, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            this.AddButton(132, 143, 247, 248, (int)Buttons.Button2, GumpButtonType.Reply, 0);
            this.AddButton(235, 143, 247, 248, (int)Buttons.Button3, GumpButtonType.Reply, 0);
            this.AddButton(336, 143, 247, 248, (int)Buttons.Button4, GumpButtonType.Reply, 0);
            this.AddLabel(154, 27, 0, @"Select Thine Steed");
        }

        public enum Buttons
        {
            Ivalid,
            Button1,
            Button2,
            Button3,
            Button4,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (sender.Mobile is Player && ((Player)sender.Mobile).CurrentAlliance != null
                && ((Player)sender.Mobile).CurrentAlliance.AllianceLeader == sender.Mobile)
            {
                Player p = sender.Mobile as Player;
                BaseAlliance a = p.CurrentAlliance;

                switch (info.ButtonID)
                {
                    case (int)Buttons.Button1:
                        {
                            
                        } 
                        break;

                    case (int)Buttons.Button2:
                        {

                        }
                        break;

                    case (int)Buttons.Button3:
                        {

                        }
                        break;

                    case (int)Buttons.Button4:
                        {

                        }
                        break;

                    default: break;
                }
            }
        }

    }

}