using System;
using Server;
using Server.Events;
using Server.Items;
using Server.Multis;
using Server.Network;
using Server.Perks;
using Server.Mobiles;

namespace Server.SkillHandlers
{
    public class Hiding
    {
        private static bool m_CombatOverride;

        public static bool CombatOverride
        {
            get { return m_CombatOverride; }
            set { m_CombatOverride = value; }
        }

        public static void Initialize()
        {
            SkillInfo.Table[21].Callback = new SkillUseCallback(OnUse);
        }

        /// <summary>
        /// Determines if the Mobile is in an area with too much lighting to hide.
        /// </summary>
        /// <param name="m">Mobile to check lighting for</param>
        /// <returns>true if safe to hide, false otherwise</returns>
        public static bool CheckLighting( Mobile m )
        {
            Scout sct = Perk.GetByType<Scout>((Player)m);

            if (sct != null)
            {
                if (sct.HideInLight())
                    return true;
            }

            if( m.AccessLevel >= AccessLevel.Counselor )
                return true;

            bool safe = true;

            foreach( Item i in m.GetItemsInRange(3) )
            {
                if( (i is BaseLight && ((BaseLight)i).Burning) && m.InLOS(i) )
                {
                    safe = false;
                    break;
                }
            }

            if( safe )
            {
                foreach( Mobile mob in m.GetMobilesInRange(3) )
                {
                    if( !m.InLOS(mob) )
                        continue;

                    if( mob is IIlluminatingObject )
                        safe = false;
                    else
                    {
                        Item lightOH = mob.FindItemOnLayer(Layer.OneHanded);
                        Item lightTH = mob.FindItemOnLayer(Layer.TwoHanded);

                        if( lightOH != null )
                        {
                            if( lightOH is BaseLight && (lightOH as BaseLight).Burning )
                                safe = false;
                            else if( lightOH is IIlluminatingObject && (lightOH as IIlluminatingObject).IsIlluminating )
                                safe = false;
                        }
                        else if( lightTH != null )
                        {
                            if( lightTH is BaseLight && (lightTH as BaseLight).Burning )
                                safe = false;
                            else if( lightTH is IIlluminatingObject && (lightTH as IIlluminatingObject).IsIlluminating )
                                safe = false;
                        }
                    }

                    if( !safe )
                        break;
                }
            }

            return safe;
        }

        public static TimeSpan OnUse( Mobile m )
        {
            if( m.Target != null || m.Spell != null )
            {
                m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
                return TimeSpan.FromSeconds(1.0);
            }
            else if( !CheckLighting(m) )
            {
                m.SendMessage("You cannot hide in so much light.");
                return TimeSpan.FromSeconds(1.0);
            }

            double bonus = 0.0;

            BaseHouse house = BaseHouse.FindHouseAt(m);

            if( house != null && house.IsFriend(m) )
            {
                bonus = 100.0;
            }
            else if( !Core.SE )
            {
                if( house == null )
                    house = BaseHouse.FindHouseAt(new Point3D(m.X - 1, m.Y, 127), m.Map, 16);

                if( house == null )
                    house = BaseHouse.FindHouseAt(new Point3D(m.X + 1, m.Y, 127), m.Map, 16);

                if( house == null )
                    house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y - 1, 127), m.Map, 16);

                if( house == null )
                    house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y + 1, 127), m.Map, 16);

                if( house != null )
                    bonus = 50.0;
            }

            //int range = 18 - (int)(m.Skills[SkillName.Hiding].Value / 10);
            int range = Math.Min((int)((100 - m.Skills[SkillName.Hiding].Value) / 2) + 8, 18);	//Cap of 18 not OSI-exact, intentional difference

            bool badCombat = (!m_CombatOverride && m.Combatant != null && m.InRange(m.Combatant.Location, range) && m.Combatant.InLOS(m));
            bool ok = (!badCombat /*&& m.CheckSkill( SkillName.Hiding, 0.0 - bonus, 100.0 - bonus )*/ );

            if( ok )
            {
                if( !m_CombatOverride )
                {
                    foreach( Mobile check in m.GetMobilesInRange(range) )
                    {
                        if( check.InLOS(m) && check.Combatant == m )
                        {
                            badCombat = true;
                            ok = false;
                            break;
                        }
                    }
                }

                ok = (!badCombat && m.CheckSkill(SkillName.Hiding, 0.0 - bonus, 100.0 - bonus));
            }

            if( badCombat )
            {
                m.RevealingAction();

                m.LocalOverheadMessage(MessageType.Regular, 0x22, 501237); // You can't seem to hide right now.

                return TimeSpan.FromSeconds(1.0);
            }
            else
            {
                if( ok )
                {
                    m.Hidden = true;
                    m.LocalOverheadMessage(MessageType.Regular, 0x1F4, 501240); // You have hidden yourself well.

                    EventSink.InvokeSkillUsed(new SkillUsedEventArgs(m, m.Skills[SkillName.Hiding]));
                }
                else
                {
                    m.RevealingAction();

                    m.LocalOverheadMessage(MessageType.Regular, 0x22, 501241); // You can't seem to hide here.
                }

                return TimeSpan.FromSeconds(10.0);
            }
        }
    }
}