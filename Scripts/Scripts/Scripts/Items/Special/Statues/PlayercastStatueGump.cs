using System;
using System.Collections.Generic;
using Server.Utilities;
using Server;
using Server.Gumps;

namespace Server.PlayercastStatues
{
    public class PlayercastStatueGump : Gump
    {
        private Mobile m_Owner;
        private PlayercastStatue m_Statue;

        private int LabelHue = 1152;

        public PlayercastStatueGump( Mobile owner, PlayercastStatue statue )
            : base(10, 10)
        {
            m_Owner = owner;
            m_Statue = statue;

            Closable = true;
            Disposable = true;
            Dragable = true;

            int i = 0, j = 0;
            int x = 20, y = 55;

            #region Page 1 //Main
            AddPage(1);
            AddDefaultBackround(170, 245);

            string[] labels0 = new string[] { "Adjust Pose", "Change Material", "Engrave", "Turn", "Change Name" };

            for( i = 0, j = 0; i < labels0.Length; i++, j += 30 )
            {
                if( i == 2 || i == 4 )
                    AddButton(x, y + j, 2103, 2104, i + 1, GumpButtonType.Reply, 0);
                else
                    AddButton(x, y + j, 2103, 2104, i + 1, GumpButtonType.Page, i < 2 ? i + 2 : i + 1);

                AddLabel(x + 20, (y - 5) + j, LabelHue, (string)labels0[i]);
            }

            AddButton(20, 215, 3, 4, 6, GumpButtonType.Reply, 0);
            AddLabel(45, 210, LabelHue, "Re-deed");
            #endregion

            #region Page 2 //Adjust Pose
            AddPage(2);
            AddDefaultBackround(170, 250);

            string[] labels1 = new string[] { "All Praise Me", "Casting", "Fighting", "Hands on Hips", "Ready", "Salute" };

            for( i = 0, j = 0; i < labels1.Length; i++, j += 30 )
            {
                AddButton(x, y + j, 2103, 2104, i + 10, GumpButtonType.Reply, 0);
                AddLabel(x + 20, (y - 5) + j, LabelHue, (string)labels1[i]);
            }

            AddButton(125, 215, 4014, 4016, 101, GumpButtonType.Page, 1);
            #endregion

            #region Page 3 //Change Material
            AddPage(3);
            AddDefaultBackround(325, 480);

            SortedDictionary<string, int> materialTable = new SortedDictionary<string, int>(StringComparer.CurrentCulture);

            foreach( int v in Enum.GetValues(typeof(MaterialType)) )
            {
                if( !materialTable.ContainsValue(v) && v != 0 )
                    materialTable.Add(Enum.GetName(typeof(MaterialType), v), v);
            }

            i = j = 0;
            int k = 0;

            foreach( KeyValuePair<string, int> kvp in materialTable )
            {
                if( i <= (materialTable.Count / 2) )
                {
                    AddButton(x, y + j, 2103, 2104, kvp.Value, GumpButtonType.Reply, 0);
                    AddLabel(x + 20, (y - 5) + j, LabelHue, Util.SplitString(kvp.Key));

                    j += 30;
                }
                else
                {
                    AddButton(x + 145, y + k, 2103, 2104, kvp.Value, GumpButtonType.Reply, 0);
                    AddLabel(x + 145 + 20, (y - 5) + k, LabelHue, Util.SplitString(kvp.Key));

                    k += 30;
                }

                i++;
            }

            AddButton(280, 445, 4014, 4016, 101, GumpButtonType.Page, 1);
            #endregion

            #region Page 4 //Turn
            AddPage(4);
            AddDefaultBackround(185, 280);

            AddButton(65, 55, 4500, 4500, 450, GumpButtonType.Reply, 0);
            AddButton(100, 80, 4501, 4501, 451, GumpButtonType.Reply, 0);
            AddButton(120, 115, 4502, 4502, 452, GumpButtonType.Reply, 0);
            AddButton(100, 150, 4503, 4503, 453, GumpButtonType.Reply, 0);
            AddButton(65, 165, 4504, 4504, 454, GumpButtonType.Reply, 0);
            AddButton(30, 150, 4505, 4505, 455, GumpButtonType.Reply, 0);
            AddButton(15, 115, 4506, 4506, 456, GumpButtonType.Reply, 0);
            AddButton(30, 80, 4507, 4507, 457, GumpButtonType.Reply, 0);

            AddButton(140, 245, 4014, 4016, 101, GumpButtonType.Page, 1);
            #endregion
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile m = sender.Mobile;
            bool resend = true;

            if( m == null || (m_Statue == null || m_Statue.Deleted) )
                return;

            switch( info.ButtonID )
            {
                case 3: //engrave
                    {
                        m.SendMessage("What would you like to engrave on the plinth?");
                        m.Prompt = new EngraveStatuePrompt(m_Statue);
                    } break;
                case 5: //change name
                    {
                        m.SendMessage("What would you like the name of this statue to be?");
                        m.Prompt = new StatueRenamePrompt(m_Statue);
                    } break;
                case 6: //re-deed
                    {
                        m.AddToBackpack(new StatueDeed(m_Statue.HasPlinth));

                        m_Statue.Delete();

                        resend = false;
                    } break;
                //change pose
                case 10:
                    {
                        m_Statue.Pose = Poses.AllPraiseMe;
                    } break;
                case 11:
                    {
                        m_Statue.Pose = Poses.Casting;
                    } break;
                case 12:
                    {
                        m_Statue.Pose = Poses.Fighting;
                    } break;
                case 13:
                    {
                        m_Statue.Pose = Poses.HandsOnHips;
                    } break;
                case 14:
                    {
                        m_Statue.Pose = Poses.Ready;
                    } break;
                case 15:
                    {
                        m_Statue.Pose = Poses.Salute;
                    } break;
                //turn
                case 450:
                    {
                        m_Statue.Direction = Direction.Up;
                    } break;
                case 451:
                    {
                        m_Statue.Direction = Direction.North;
                    } break;
                case 452:
                    {
                        m_Statue.Direction = Direction.Right;
                    } break;
                case 453:
                    {
                        m_Statue.Direction = Direction.East;
                    } break;
                case 454:
                    {
                        m_Statue.Direction = Direction.Down;
                    } break;
                case 455:
                    {
                        m_Statue.Direction = Direction.South;
                    } break;
                case 456:
                    {
                        m_Statue.Direction = Direction.Left;
                    } break;
                case 457:
                    {
                        m_Statue.Direction = Direction.West;
                    } break;
                default:
                    {
                        resend = false;
                    } break;
            }

            List<int> material = new List<int>();

            foreach( int i in Enum.GetValues(typeof(MaterialType)) )
                material.Add(i);

            if( material.Contains(info.ButtonID) && info.ButtonID != 0 )
            {
                m_Statue.Material = (MaterialType)material[material.IndexOf(info.ButtonID)];

                resend = true;
            }

            if( resend )
            {
                m.CloseGump(typeof(PlayercastStatueGump));
                m.SendGump(new PlayercastStatueGump(m, m_Statue));
            }
        }

        private void AddDefaultBackround( int width, int height )
        {
            AddBackground(0, 0, width, height, 9250);

            AddLabel(20, 15, LabelHue, "Statue Modification");
        }
    }
}