using System;
using Server;
using Server.Gumps;
using System.Collections.Generic;
using System.IO;
using Server.Network;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Server.Misc;
using Server.Mobiles;

//Must Be applied to CharacterCreation.cs
 //using System.Text.RegularExpressions;
 //private static void SetName( Mobile m, string name )
        //{
        //    bool badName = false;
        //    name = name.Trim();

        //    for( int i = 0; i < m.Account.Length; i++ )
        //    {
        //        if( m.Account[i] != null )
        //        {
        //            if( Insensitive.Compare(m.Account[i].RawName, name) == 0 )
        //                badName = true;
        //        }
        //    }

        //    Regex rx = new Regex(" ");
        //    Match match = rx.Match(name);

        //    if (match.Success)
        //        badName = true;

        //    if( badName || !NameVerification.Validate(name, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote) )
        //        name = NameList.RandomName(m.Female ? "female" : "male");

        //    m.Name = name;
        //}

namespace Server.Surnames
{
    public class NameRegistryScroll : Item
    {
        [Constructable]
        public NameRegistryScroll()
            : base(0x14F0)
        {
            Name = "a name registry application";
            Movable = true;
        }

        public NameRegistryScroll(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this != null && from is PlayerMobile && from.Alive)
            {
                from.SendGump(new SurnameRegistrarGump(from, GumpType.Scroll));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NameRegistry : Item
    {
        [Constructable]
        public NameRegistry() : base(4029)
        {
            Name = "a name registry";
            Movable = false;
        }

        public NameRegistry(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this != null && from is PlayerMobile && from.Alive)
            {
                from.SendGump(new SurnameRegistrarGump(from, GumpType.Book));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public enum GumpType
    {
        Invalid,
        Scroll,
        Book
    }

	public class SurnameRegistrarGump : Gump
	{
        List<string> surnames = new List<string>();
        List<string> origins = new List<string>();

        string originatingSerial;

        Mobile caller;
        GumpType gumpType;

        string nameList = "";

        private static readonly string savePath = "Surnames";
        private static readonly string surnameFile = Path.Combine(savePath, "surnames.xml");

        public SurnameRegistrarGump( Mobile from, GumpType gtype)
            : base(0, 0)
		{
            caller = from;
            gumpType = gtype;

            GenerateNameList();

            if (gumpType == GumpType.Scroll)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                this.AddPage(0);
                this.AddBackground(68, 2, 367, 298, 9380);
                this.AddTextEntry(262, 186, 138, 18, 0, (int)Buttons.NameEntry, @"");
                this.AddButton(335, 216, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
                this.AddHtml(275, 66, 121, 106, @"If you'd like to register your character's surname, please type it in below and hit the okay button. If it is currently in use, you will have to wait for the acceptance of the name's originator.", true, true);
                this.AddHtml(107, 62, 136, 179, @"" + nameList, true, true);
            }

            if (gumpType == GumpType.Book)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                this.AddPage(0);
                this.AddImage(79, 48, 2200);
                this.AddTextEntry(262, 186, 138, 18, 0, (int)Buttons.NameEntry, @"");
                this.AddButton(335, 216, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
                this.AddHtml(275, 66, 121, 106, @"<BASEFONT COLOR=#101010>If you'd like to register your character's surname, please type it in below and hit the okay button. If it is currently in use, you will have to wait for the acceptance of the name's originator.</font>", false, true);
                this.AddHtml(107, 62, 136, 179, @"<BASEFONT COLOR=#101010>" + nameList, false, true);
            }
		}
		
		public enum Buttons
		{
			NameEntry,
			Okay,
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Okay &&
                !String.IsNullOrEmpty(info.GetTextEntry((int)Buttons.NameEntry).Text))
            {
                Mobile from = sender.Mobile;

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                String desiredSurname = info.GetTextEntry((int)Buttons.NameEntry).Text;
                desiredSurname = (string)(char.ToUpper(desiredSurname[0]) + desiredSurname.Substring(1));

                Regex rx = new Regex(" ");
                Match match = rx.Match(from.Name);
                bool hasSurname = match.Success;

                rx = new Regex(" |1|2|3|4|5|6|7|8|9|0");
                match = rx.Match(desiredSurname);              
              
                bool invalidName = !NameVerification.Validate
                    (desiredSurname, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote) || match.Success;

                if (desiredSurname.Length > 16)
                {
                    from.SendMessage("Your surname may be no longer than (16) characters.");
                    return;
                }

                if (invalidName)
                {
                    from.SendMessage("That is not an acceptable surname.");
                    return;
                }

                if (hasSurname)
                {
                    from.SendMessage("You already have a surname and may not register another.");
                    return;
                }

                if (surnames.Contains(desiredSurname))
                {
                    DetermineOrigin(desiredSurname);
                    InstantiateRequest(desiredSurname, caller as PlayerMobile);
                }

                if (!String.IsNullOrEmpty(desiredSurname) && !surnames.Contains(desiredSurname)
                    && !invalidName && !hasSurname)
                {
                    from.Name += " " + desiredSurname;

                    using (StreamWriter writer = new StreamWriter(surnameFile))
                    {
                        XmlTextWriter xml = new XmlTextWriter(writer);

                        xml.Formatting = Formatting.Indented;
                        xml.IndentChar = '\t';
                        xml.Indentation = 1;

                        xml.WriteStartDocument(true);
                        xml.WriteStartElement("surnames");

                        for (int n = 0; n < surnames.Count; n++)
                        {
                            xml.WriteStartElement("surname");
                            xml.WriteAttributeString("origin", origins[n].ToString());
                            xml.WriteString(surnames[n]);
                            xml.WriteEndElement();
                        }

                        xml.WriteStartElement("surname");
                        xml.WriteAttributeString("origin", caller.Serial.ToString());
                        xml.WriteString(desiredSurname);
                        xml.WriteEndElement();

                        xml.WriteEndElement();
                        xml.WriteEndDocument();
                        xml.Close();
                    }
                }
            }
        }

        public void GenerateNameList()
        {
            if (!File.Exists(surnameFile))
                return;

            XmlDocument doc = new XmlDocument();
            XmlElement root;

            try
            {
                doc.Load(surnameFile);
                root = doc["surnames"];

                foreach( XmlElement ele in root.GetElementsByTagName("surname") )
                {
                    surnames.Add(ele.InnerText.ToString());
                    origins.Add(ele.Attributes["origin"].Value);
                }
            }

            //Just for looks most likely. Chances are anything that'll throw this will crash the server.
            catch (Exception e)
            {
                Console.WriteLine("\nSurname load exception: " + e + "(" + caller.Name + ")");
            }

            StringBuilder builder = new StringBuilder();

            foreach (string surname in surnames)
            {               
                builder.Append(surname).Append(", ");
            }

            nameList = builder.ToString();
            nameList = nameList.Remove(nameList.Length - 2);
        }

        public void DetermineOrigin(string surnameRequested)
        {
            if (!File.Exists(surnameFile))
                return;

            XmlDocument doc = new XmlDocument();
            XmlElement root;

            try
            {
                doc.Load(surnameFile);
                root = doc["surnames"];

                foreach (XmlElement ele in root.GetElementsByTagName("surname"))
                {
                    if (ele.InnerText == surnameRequested)
                        originatingSerial = ele.Attributes["origin"].Value;
                }
            }

            catch { }
        }

        public void InstantiateRequest(string surnameRequested, PlayerMobile requestee)
        {
            bool found = false;

            for (int i = 0; i < NetState.Instances.Count; i++)
            {
                if (NetState.Instances[i].Mobile.Serial.ToString() == originatingSerial)
                {
                    Mobile origin = NetState.Instances[i].Mobile;
                    origin.SendGump(new SurnameRequestGump(surnameRequested, requestee));
                    found = true;
                }
            }

            if (found)
            {
                requestee.SendMessage("This surname has already been registered. You will need permission from the originating player...");
                requestee.SendMessage("The originating player is online and your request has been sent.");
            }

            else
            {
                requestee.SendMessage("This surname has already been registered. You will need permission from the originating player...");
                requestee.SendMessage("The originating player is offine or no-longer exists, please try again later.");
            }
        }
	}

    public class SurnameRequestGump : Gump
    {
        PlayerMobile p;
        string nameReq; 

        public SurnameRequestGump( string surnameRequested, PlayerMobile requestee )
            : base(0, 0)
        {
            nameReq = surnameRequested;
            p = requestee;

            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(111, 49, 261, 267, 9250);
            this.AddHtml(145, 78, 200, 80, String.Format(@"{0} is requesting to take on the name {1} and join your clan. Do you accept?", requestee.Name, surnameRequested), true, false);
            this.AddButton(272, 199, 12018, 12019, (int)Buttons.Refuse, GumpButtonType.Reply, 0);
            this.AddButton(271, 243, 12000, 12001, (int)Buttons.Accept, GumpButtonType.Reply, 0);

        }

        public enum Buttons
        {
            Refuse,
            Accept
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == (int)Buttons.Accept)
            {
                p.Name += " " + nameReq;
                p.SendMessage("{0} has accepted your request and you take on the surname {1}.", sender.Mobile.Name, nameReq);
            }

            if (info.ButtonID == (int)Buttons.Refuse)
            {
                p.SendMessage("{0} has refused your request to join the {1} clan.", sender.Mobile.Name, nameReq);
            }
        }
    }
}