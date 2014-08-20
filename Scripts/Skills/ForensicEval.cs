using System;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class ForensicEvaluation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Forensics].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse( Mobile m )
        {
            m.Target = new ForensicTarget();

            m.SendLocalizedMessage(500906); // What would you like to evaluate?

            return TimeSpan.FromSeconds(1.0);
        }

        public class ForensicTarget : Target
        {
            public ForensicTarget()
                : base(10, false, TargetFlags.None)
            {
            }

            protected override void OnTarget( Mobile from, object target )
            {
                if( target is Mobile )
                {
                    if( from.CheckTargetSkill(SkillName.Forensics, target, 40.0, 100.0) )
                    {
                        if (target is PlayerMobile && ((PlayerMobile)target).NpcGuild == NpcGuild.ThievesGuild)
                        {
                            if (from.Skills.Forensics.Value > ((PlayerMobile)target).Skills.Forensics.Value)
                                from.SendMessage("Your intuition tells you this person displays all the signs of a thief.");
                            else
                                from.SendLocalizedMessage(501003); //You notice nothing unusual.
                        }

                        else if (target is PlayerMobile && ((PlayerMobile)target).DisguiseTimeLeft > TimeSpan.FromSeconds(0))
                        {
                            if (from.Skills.Forensics.Value > ((PlayerMobile)target).Skills.Forensics.Value)
                                from.SendMessage("This person appears to be wearing a disguise.");
                            else
                                from.SendLocalizedMessage(501003); //You notice nothing unusual.
                        }

                        else
                            from.SendLocalizedMessage(501003); //You notice nothing unusual.
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001); //You cannot determine anything useful.
                    }
                }
                else if( target is Corpse )
                {
                    if( from.CheckTargetSkill(SkillName.Forensics, target, 0.0, 100.0) )
                    {
                        Corpse c = (Corpse)target;

                        if (((Body)c.Amount).IsHuman)
                        {
                            if (c.Killer.NameMod != null)
                                c.LabelTo(from, 1042751, (c.Killer == null ? "no one" : c.Killer.NameMod));

                            else 
                            {
                                c.LabelTo(from, 1042751, (c.Killer == null ? "no one" : c.Killer.RawName));
                            }

                        }

                        if( c.Looters.Count > 0 )
                        {
                            StringBuilder sb = new StringBuilder();
                            for( int i = 0; i < c.Looters.Count; i++ )
                            {
                                if( i > 0 ) 
                                    sb.Append(", ");
                                if (c.Looters[i].NameMod != null)
                                {
                                    sb.Append(((Mobile)c.Looters[i]).NameMod);
                                }
                                else 
                                {
                                    sb.Append(((Mobile)c.Looters[i]).RawName);
                                }
                            }

                            c.LabelTo(from, 1042752, sb.ToString()); //This body has been distrubed by ~1_PLAYER_NAMES~
                        }
                        else
                        {
                            c.LabelTo(from, 501002); //The corpse has not be desecrated.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001); //You cannot determine anything useful.
                    }
                }
                else if( target is ILockpickable )
                {
                    ILockpickable p = (ILockpickable)target;
                    if( p.Picker != null && p.Picker.NameMod != null )
                        from.SendLocalizedMessage(1042749, p.Picker.NameMod);

                    else if (p.Picker != null) 
                    {
                        from.SendLocalizedMessage(1042749, p.Picker.RawName);
                    }
                    else
                        from.SendLocalizedMessage(501003);//You notice nothing unusual.
                }
                else
                {
                    from.SendLocalizedMessage(501003); //You notice nothing unusual.
                }

                EventSink.InvokeSkillUsed(new SkillUsedEventArgs(from, from.Skills[SkillName.Forensics]));
            }
        }
    }
}
