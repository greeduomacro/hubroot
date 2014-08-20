using System;
using System.Text;
using Server.Gumps;
using Server.Network;
using System.Collections;
using Server.Mobiles;
using Server.Commands;

namespace Server.Language
{
    public class Language
    {
        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
        }

        public static void EventSink_Speech(SpeechEventArgs args)
        {
            if (args.Mobile is Player)
            {
                MessageType mt = args.Type;
                Player player = args.Mobile as Player;
                string playerSpeech = args.Speech;
                int speechRange;

                int[] FontHue = new int[] { 0, 0, 1175, 1157, 1155, 1152 };

                if ((player.AccessLevel > AccessLevel.Player) && player.Hidden)
                    return;

                if ((!player.Alive) || String.IsNullOrEmpty(playerSpeech))
                    return;

                if (player.CurrentLanguage == Player.PlayerLanguage.Common)
                    return;

                if (args.Type == MessageType.Emote)
                {
                    player.Emote(playerSpeech);
                    return;
                }

                switch (args.Type)
                {
                    case MessageType.Yell:
                        speechRange = 20;
                        break;

                    case MessageType.Regular:
                        speechRange = 10;
                        break;

                    case MessageType.Whisper:
                        speechRange = 1;
                        break;

                    default:
                        speechRange = 10;
                        break;
                }
              
                ArrayList list = new ArrayList();

                foreach (Mobile m in player.Map.GetMobilesInRange(player.Location, speechRange))
                {
                    list.Add(m);
                }

                for (int x = 0; x < list.Count; x++)
                {
                    Mobile m = list[x] as Mobile;
                    if (m.Player)
                    {
                        Player listener = m as Player;

                        if (listener.LevelofUnderstanding[(int)player.CurrentLanguage] < Utility.RandomMinMax(90, 100))
                        {
                            args.Blocked = true;
                            listener.Send(new AsciiMessage(player.Serial, -1, MessageType.Regular, FontHue[(int)player.CurrentLanguage], 8, "Runes", playerSpeech.ToUpper()));
                            player.RevealingAction();

                            if (Utility.RandomDouble() <= 0.02)
                            {
                                listener.LevelofUnderstanding[(int)player.CurrentLanguage] += 1;
                            }
                        }

                        else
                        {
                            args.Blocked = true;
                            listener.Send(new UnicodeMessage(player.Serial, -1, MessageType.Regular, player.SpeechHue, 3, "", "normal", playerSpeech));
                            player.RevealingAction();

                            if (Utility.RandomDouble() <= 0.005)
                            {
                                listener.LevelofUnderstanding[(int)player.CurrentLanguage] += 1;
                            }
                        }
                    }
                }
            }
        }
    }

    public class BookOfLanguage : Item
    {
        public enum Language
        {
            Invalid,
            Common,
            Ancient,
            Tribal,
            Pagan,
            Glyph
        }

        private Language WritLanguage;

        [CommandProperty(AccessLevel.GameMaster)]
        public Language BookType
        {
            get { return WritLanguage; }
            set { WritLanguage = value; }
        }

        private int nameRef = Utility.RandomMinMax(1,5);

        public string GenerateBookName()
        {
            switch (nameRef)
            {
                case 1:
                    return "Articles on the " + BookType.ToString() + " Language";
                case 2:
                    return BookType.ToString() + " Syntax and Grammar";
                case 3:
                    return BookType.ToString() + " Language Structure";
                case 4:
                    return "Conjugating Verbs in " + BookType.ToString();
                case 5:
                    return "A History of the " + BookType.ToString() + " Language";

                default: return "A book on" + BookType.ToString() + " dialects";
            }
        }

        public override string DefaultName
        {
            get { return GenerateBookName(); }
        }

        [Constructable]
        public BookOfLanguage()
            : base(0xFBE)
        {
            Weight = 1.0;
        }

        public BookOfLanguage(Serial serial)
            : base(serial)
        {
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write((int)WritLanguage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            WritLanguage = (Language)reader.ReadInt();

        }

        public override void OnDoubleClick(Mobile from)
        {
            Player pm = from as Player;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        
            else if (pm.LevelofUnderstanding[(int)BookType] >= 100)
            {
                pm.SendMessage("You have already learned all you can about this language.");
            }
            else
            {
                pm.LevelofUnderstanding[(int)BookType] += Utility.RandomMinMax(1, 2);
                pm.SendMessage("Your knowledge of the {0} language has increased.", BookType.ToString());

                if (pm.LevelofUnderstanding[(int)BookType] > 100)
                    pm.LevelofUnderstanding[(int)BookType] = 100;

                Delete();
            }
        }
    }

    public class SpeakCommand
    {
        [CommandAttribute("Speak", AccessLevel.Player)]
        public static void SayCommand_OnCommand(CommandEventArgs args)
        {
            string text = args.ArgString.Trim();
            Player p = args.Mobile as Player;
            Player.PlayerLanguage newLanguage;
            Enum.TryParse<Player.PlayerLanguage>(text, true, out newLanguage);

            //Debug
            //Console.Write(p.Name + "attempting to speak:");
            //Console.Write(newLanguage);
            //Console.Write((int)newLanguage);
            //Console.WriteLine(p.LevelofUnderstanding[(int)newLanguage]);

            switch (newLanguage)
            {
                case Player.PlayerLanguage.Common:
                    {
                        if (p.LevelofUnderstanding[(int)newLanguage] == 100)
                        {
                            p.CurrentLanguage = Player.PlayerLanguage.Common;
                            p.SendMessage("You're now speaking a Common language.");
                        }
                        else
                        {
                            p.SendMessage("You do not posess the level of understanding needed to speak that language.");
                        }

                        break;
                    }

                case Player.PlayerLanguage.Ancient:
                    {
                        if (p.LevelofUnderstanding[(int)newLanguage] == 100)
                        {
                            p.CurrentLanguage = Player.PlayerLanguage.Ancient;
                            p.SendMessage("You're now speaking in Ancient tongues.");
                        }
                        else
                        {
                            p.SendMessage("You do not posess the level of understanding needed to speak that language.");
                        }

                        break;
                    }

                case Player.PlayerLanguage.Tribal:
                    {
                        if (p.LevelofUnderstanding[(int)newLanguage] == 100)
                        {
                            p.CurrentLanguage = Player.PlayerLanguage.Tribal;
                            p.SendMessage("You're now speaking in a Tribal dialect.");
                        }
                        else
                        {
                            p.SendMessage("You do not posess the level of understanding needed to speak that language.");
                        }

                        break;
                    }

                case Player.PlayerLanguage.Pagan:
                    {
                        if (p.LevelofUnderstanding[(int)newLanguage] == 100)
                        {
                            p.CurrentLanguage = Player.PlayerLanguage.Pagan;
                            p.SendMessage("You are now using a Pagan vernacular.");
                        }
                        else
                        {
                            p.SendMessage("You do not posess the level of understanding needed to speak that language.");
                        }

                        break;
                    }

                case Player.PlayerLanguage.Glyph:
                    {
                        if (p.LevelofUnderstanding[(int)newLanguage] == 100)
                        {
                            p.CurrentLanguage = Player.PlayerLanguage.Tribal;
                            p.SendMessage("You're now speaking in phonetic Glyph.");
                        }
                        else
                        {
                            p.SendMessage("You do not posess the level of understanding needed to speak that language.");
                        }

                        break;
                    }


            }
        }
    }
}