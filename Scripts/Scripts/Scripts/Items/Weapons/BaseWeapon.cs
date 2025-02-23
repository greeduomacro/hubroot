using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Network;
using Server.Perks;
using Server.Spells;

namespace Server.Items
{
    public interface ISlayer
    {
        SlayerName Slayer { get; set; }
        SlayerName Slayer2 { get; set; }
    }

    public abstract class BaseWeapon : Item, IWeapon, ICraftable, ISlayer, IDurability
    {
        /* Weapon internals work differently now (Mar 13 2003)
         *
         * The attributes defined below default to -1.
         * If the value is -1, the corresponding virtual 'Aos/Old' property is used.
         * If not, the attribute value itself is used. Here's the list:
         *  - MinDamage
         *  - MaxDamage
         *  - Speed
         *  - HitSound
         *  - MissSound
         *  - StrRequirement, DexRequirement, IntRequirement
         *  - WeaponType
         *  - WeaponAnimation
         *  - MaxRange
         */

        #region Var declarations

        // Instance values. These values are unique to each weapon.
        private WeaponDamageLevel m_DamageLevel;
        private WeaponAccuracyLevel m_AccuracyLevel;
        private WeaponDurabilityLevel m_DurabilityLevel;
        private WeaponQuality m_Quality;
        private Mobile m_Crafter;
        private Poison m_Poison;
        private int m_PoisonCharges;
        private bool m_Identified;
        private int m_Hits;
        private int m_MaxHits;
        private SlayerName m_Slayer;
        private SlayerName m_Slayer2;
        private SkillMod m_SkillMod, m_MageMod;
        private CraftResource m_Resource;
        private bool m_PlayerConstructed;

        private bool m_Cursed; // Is this weapon cursed via Curse Weapon necromancer spell? Temporary; not serialized.
        private bool m_Consecrated; // Is this weapon blessed via Consecrate Weapon paladin ability? Temporary; not serialized.

        private AosAttributes m_AosAttributes;
        private AosWeaponAttributes m_AosWeaponAttributes;
        private AosSkillBonuses m_AosSkillBonuses;

        // Overridable values. These values are provided to override the defaults which get defined in the individual weapon scripts.
        private int m_StrReq, m_DexReq, m_IntReq;
        private int m_MinDamage, m_MaxDamage;
        private int m_HitSound, m_MissSound;
        private float m_Speed;
        private int m_MaxRange;
        private SkillName m_Skill;
        private WeaponType m_Type;
        private WeaponAnimation m_Animation;
        #endregion

        #region Virtual Properties
        public virtual WeaponAbility PrimaryAbility { get { return null; } }
        public virtual WeaponAbility SecondaryAbility { get { return null; } }

        public virtual int DefMaxRange { get { return 1; } }
        public virtual int DefHitSound { get { return 0; } }
        public virtual int DefMissSound { get { return 0; } }
        public virtual SkillName DefSkill { get { return SkillName.Swords; } }
        public virtual WeaponType DefType { get { return WeaponType.Slashing; } }
        public virtual WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash1H; } }

        public virtual int AosStrengthReq { get { return 0; } }
        public virtual int AosDexterityReq { get { return 0; } }
        public virtual int AosIntelligenceReq { get { return 0; } }
        public virtual int AosMinDamage { get { return 0; } }
        public virtual int AosMaxDamage { get { return 0; } }
        public virtual int AosSpeed { get { return 0; } }
        public virtual float MlSpeed { get { return 0.0f; } }
        public virtual int AosMaxRange { get { return DefMaxRange; } }
        public virtual int AosHitSound { get { return DefHitSound; } }
        public virtual int AosMissSound { get { return DefMissSound; } }
        public virtual SkillName AosSkill { get { return DefSkill; } }
        public virtual WeaponType AosType { get { return DefType; } }
        public virtual WeaponAnimation AosAnimation { get { return DefAnimation; } }

        public virtual int OldStrengthReq { get { return 0; } }
        public virtual int OldDexterityReq { get { return 0; } }
        public virtual int OldIntelligenceReq { get { return 0; } }
        public virtual int OldMinDamage { get { return 0; } }
        public virtual int OldMaxDamage { get { return 0; } }
        public virtual int OldSpeed { get { return 0; } }
        public virtual int OldMaxRange { get { return DefMaxRange; } }
        public virtual int OldHitSound { get { return DefHitSound; } }
        public virtual int OldMissSound { get { return DefMissSound; } }
        public virtual SkillName OldSkill { get { return DefSkill; } }
        public virtual WeaponType OldType { get { return DefType; } }
        public virtual WeaponAnimation OldAnimation { get { return DefAnimation; } }

        public virtual int InitMinHits { get { return 0; } }
        public virtual int InitMaxHits { get { return 0; } }

        //public override int PhysicalResistance { get { return m_AosWeaponAttributes.ResistPhysicalBonus; } }
        //public override int FireResistance { get { return m_AosWeaponAttributes.ResistFireBonus; } }
        //public override int ColdResistance { get { return m_AosWeaponAttributes.ResistColdBonus; } }
        //public override int PoisonResistance { get { return m_AosWeaponAttributes.ResistPoisonBonus; } }
        //public override int EnergyResistance { get { return m_AosWeaponAttributes.ResistEnergyBonus; } }

        public virtual SkillName AccuracySkill { get { return SkillName.Tactics; } }
        #endregion

        #region Getters & Setters
        [CommandProperty(AccessLevel.Administrator)]
        public AosAttributes Attributes
        {
            get { return m_AosAttributes; }
            set { }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public AosWeaponAttributes WeaponAttributes
        {
            get { return m_AosWeaponAttributes; }
            set { }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public AosSkillBonuses SkillBonuses
        {
            get { return m_AosSkillBonuses; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Cursed
        {
            get { return m_Cursed; }
            set { m_Cursed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Consecrated
        {
            get { return m_Consecrated; }
            set { m_Consecrated = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified
        {
            get { return m_Identified; }
            set { m_Identified = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get { return m_Hits; }
            set
            {
                if( m_Hits == value )
                    return;

                if( value > m_MaxHits )
                    value = m_MaxHits;

                m_Hits = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get { return m_MaxHits; }
            set { m_MaxHits = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonCharges
        {
            get { return m_PoisonCharges; }
            set { m_PoisonCharges = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison
        {
            get { return m_Poison; }
            set { m_Poison = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponQuality Quality
        {
            get { return m_Quality; }
            set { UnscaleDurability(); m_Quality = value; ScaleDurability(); InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return m_Crafter; }
            set { m_Crafter = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SlayerName Slayer
        {
            get { return m_Slayer; }
            set { m_Slayer = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SlayerName Slayer2
        {
            get { return m_Slayer2; }
            set { m_Slayer2 = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { UnscaleDurability(); m_Resource = value; Hue = CraftResources.GetHue(m_Resource); InvalidateProperties(); ScaleDurability(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponDamageLevel DamageLevel
        {
            get { return m_DamageLevel; }
            set { m_DamageLevel = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponDurabilityLevel DurabilityLevel
        {
            get { return m_DurabilityLevel; }
            set { UnscaleDurability(); m_DurabilityLevel = value; InvalidateProperties(); ScaleDurability(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get { return m_PlayerConstructed; }
            set { m_PlayerConstructed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxRange
        {
            get { return (m_MaxRange == -1 ? Core.AOS ? AosMaxRange : OldMaxRange : m_MaxRange); }
            set { m_MaxRange = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponAnimation Animation
        {
            get { return (m_Animation == (WeaponAnimation)(-1) ? Core.AOS ? AosAnimation : OldAnimation : m_Animation); }
            set { m_Animation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponType Type
        {
            get { return (m_Type == (WeaponType)(-1) ? Core.AOS ? AosType : OldType : m_Type); }
            set { m_Type = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName Skill
        {
            get { return (m_Skill == (SkillName)(-1) ? Core.AOS ? AosSkill : OldSkill : m_Skill); }
            set { m_Skill = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitSound
        {
            get { return (m_HitSound == -1 ? Core.AOS ? AosHitSound : OldHitSound : m_HitSound); }
            set { m_HitSound = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MissSound
        {
            get { return (m_MissSound == -1 ? Core.AOS ? AosMissSound : OldMissSound : m_MissSound); }
            set { m_MissSound = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage
        {
            get { return (m_MinDamage == -1 ? Core.AOS ? AosMinDamage : OldMinDamage : m_MinDamage); }
            set { m_MinDamage = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage
        {
            get { return (m_MaxDamage == -1 ? Core.AOS ? AosMaxDamage : OldMaxDamage : m_MaxDamage); }
            set { m_MaxDamage = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public float Speed
        {
            get
            {
                if( m_Speed != -1 )
                    return m_Speed;

                if( Core.ML )
                    return MlSpeed;
                else if( Core.AOS )
                    return AosSpeed;

                return m_Speed;
            }
            set { m_Speed = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrRequirement
        {
            get { return (m_StrReq == -1 ? Core.AOS ? AosStrengthReq : OldStrengthReq : m_StrReq); }
            set { m_StrReq = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexRequirement
        {
            get { return (m_DexReq == -1 ? Core.AOS ? AosDexterityReq : OldDexterityReq : m_DexReq); }
            set { m_DexReq = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntRequirement
        {
            get { return (m_IntReq == -1 ? Core.AOS ? AosIntelligenceReq : OldIntelligenceReq : m_IntReq); }
            set { m_IntReq = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WeaponAccuracyLevel AccuracyLevel
        {
            get
            {
                return m_AccuracyLevel;
            }
            set
            {
                if( m_AccuracyLevel != value )
                {
                    m_AccuracyLevel = value;

                    if( UseSkillMod )
                    {
                        if( m_AccuracyLevel == WeaponAccuracyLevel.Regular )
                        {
                            if( m_SkillMod != null )
                                m_SkillMod.Remove();

                            m_SkillMod = null;
                        }
                        else if( m_SkillMod == null && Parent is Mobile )
                        {
                            m_SkillMod = new DefaultSkillMod(AccuracySkill, true, (int)m_AccuracyLevel * 5);
                            ((Mobile)Parent).AddSkillMod(m_SkillMod);
                        }
                        else if( m_SkillMod != null )
                        {
                            m_SkillMod.Value = (int)m_AccuracyLevel * 5;
                        }
                    }

                    InvalidateProperties();
                }
            }
        }

        #endregion

        public virtual void UnscaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_Hits = ((m_Hits * 100) + (scale - 1)) / scale;
            m_MaxHits = ((m_MaxHits * 100) + (scale - 1)) / scale;
            InvalidateProperties();
        }

        public virtual void ScaleDurability()
        {
            int scale = 100 + GetDurabilityBonus();

            m_Hits = ((m_Hits * scale) + 99) / 100;
            m_MaxHits = ((m_MaxHits * scale) + 99) / 100;
            InvalidateProperties();
        }

        public int GetDurabilityBonus()
        {
            int bonus = 0;

            if( m_Quality == WeaponQuality.Exceptional )
                bonus += 20;

            switch( m_DurabilityLevel )
            {
                case WeaponDurabilityLevel.Durable: bonus += 20; break;
                case WeaponDurabilityLevel.Substantial: bonus += 50; break;
                case WeaponDurabilityLevel.Massive: bonus += 70; break;
                case WeaponDurabilityLevel.Fortified: bonus += 100; break;
                case WeaponDurabilityLevel.Indestructible: bonus += 120; break;
            }

            if( Core.AOS )
            {
                bonus += m_AosWeaponAttributes.DurabilityBonus;

                CraftResourceInfo resInfo = CraftResources.GetInfo(m_Resource);
                CraftAttributeInfo attrInfo = null;

                if( resInfo != null )
                    attrInfo = resInfo.AttributeInfo;

                if( attrInfo != null )
                    bonus += attrInfo.WeaponDurability;
            }

            return bonus;
        }

        public static void BlockEquip( Mobile m, TimeSpan duration )
        {
            if( m.BeginAction(typeof(BaseWeapon)) )
                new ResetEquipTimer(m, duration).Start();
        }

        private class ResetEquipTimer : Timer
        {
            private Mobile m_Mobile;

            public ResetEquipTimer( Mobile m, TimeSpan duration )
                : base(duration)
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_Mobile.EndAction(typeof(BaseWeapon));
            }
        }

        public override bool CheckConflictingLayer( Mobile m, Item item, Layer layer )
        {
            if( base.CheckConflictingLayer(m, item, layer) )
                return true;

            if( this.Layer == Layer.TwoHanded && layer == Layer.OneHanded )
            {
                m.SendLocalizedMessage(500214); // You already have something in both hands.
                return true;
            }
            else if( this.Layer == Layer.OneHanded && layer == Layer.TwoHanded && !(item is BaseShield) && !(item is BaseEquipableLight) )
            {
                m.SendLocalizedMessage(500215); // You can only wield one weapon at a time.
                return true;
            }

            return false;
        }

        public override bool CanEquip( Mobile from )
        {
            if( from.Dex < DexRequirement )
            {
                from.SendMessage("You are not coordinated enough to wield that.");
                return false;
            }
            else if( from.Str < StrRequirement )
            {
                from.SendLocalizedMessage(500213); // You are not strong enough to equip that.
                return false;
            }
            else if( from.Int < IntRequirement )
            {
                from.SendMessage("You are not smart enough to use that.");
                return false;
            }
            else if( !from.CanBeginAction(typeof(BaseWeapon)) )
            {
                return false;
            }
            else
            {
                return base.CanEquip(from);
            }
        }

        public virtual bool UseSkillMod { get { return !Core.AOS; } }

        public override bool OnEquip( Mobile from )
        {
            if( Core.AOS )
            {
                int strBonus = m_AosAttributes.BonusStr;
                int dexBonus = m_AosAttributes.BonusDex;
                int intBonus = m_AosAttributes.BonusInt;

                if( (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
                {
                    Mobile m = from;

                    string modName = this.Serial.ToString();

                    if( strBonus != 0 )
                        m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                    if( dexBonus != 0 )
                        m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                    if( intBonus != 0 )
                        m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
                }
            }

            from.NextCombatTime = DateTime.Now + GetDelay(from);

            if( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular )
            {
                if( m_SkillMod != null )
                    m_SkillMod.Remove();

                m_SkillMod = new DefaultSkillMod(AccuracySkill, true, (int)m_AccuracyLevel * 5);
                from.AddSkillMod(m_SkillMod);
            }

            if( Core.ML && m_AosWeaponAttributes.MageWeapon != 0 && m_AosWeaponAttributes.MageWeapon != 30 )
            {
                if( m_MageMod != null )
                    m_MageMod.Remove();

                m_MageMod = new DefaultSkillMod(SkillName.Magery, true, -30 + m_AosWeaponAttributes.MageWeapon);
                from.AddSkillMod(m_MageMod);
            }

            return true;
        }

        public override void OnAdded( object parent )
        {
            base.OnAdded(parent);

            if( parent is Mobile )
            {
                Mobile from = (Mobile)parent;

                if( Core.ML )
                    m_AosSkillBonuses.AddTo(from);

                from.CheckStatTimers();
                from.Delta(MobileDelta.WeaponDamage);
            }
        }

        public override void OnRemoved( object parent )
        {
            if( parent is Mobile )
            {
                Mobile m = (Mobile)parent;
                BaseWeapon weapon = m.Weapon as BaseWeapon;

                string modName = this.Serial.ToString();

                m.RemoveStatMod(modName + "Str");
                m.RemoveStatMod(modName + "Dex");
                m.RemoveStatMod(modName + "Int");

                if( weapon != null )
                    m.NextCombatTime = DateTime.Now + weapon.GetDelay(m);

                if( UseSkillMod && m_SkillMod != null )
                {
                    m_SkillMod.Remove();
                    m_SkillMod = null;
                }

                if( m_MageMod != null )
                {
                    m_MageMod.Remove();
                    m_MageMod = null;
                }

                if( Core.AOS )
                    m_AosSkillBonuses.Remove();

                m.CheckStatTimers();

                m.Delta(MobileDelta.WeaponDamage);
            }
        }

        public virtual SkillName GetUsedSkill( Mobile m, bool checkSkillAttrs )
        {
            SkillName sk;

            if( checkSkillAttrs )//&& m_AosWeaponAttributes.UseBestSkill != 0 )
            {
                double swrd = m.Skills[SkillName.Swords].Value;
                double fenc = m.Skills[SkillName.Fencing].Value;
                double mcng = m.Skills[SkillName.Macing].Value;
                double arch = m.Skills[SkillName.Archery].Value;
                double val;

                sk = SkillName.Swords;
                val = swrd;

                if( fenc > val ) { sk = SkillName.Fencing; val = fenc; }
                if( mcng > val ) { sk = SkillName.Macing; val = mcng; }
                if( arch > val ) { sk = SkillName.Archery; val = arch; }
            }
            else if( Core.ML && m_AosWeaponAttributes.MageWeapon != 0 )
            {
                if( m.Skills[SkillName.Magery].Value > m.Skills[Skill].Value )
                    sk = SkillName.Magery;
                else
                    sk = Skill;
            }
            else
            {
                sk = Skill;

                if( sk != SkillName.Wrestling && !m.Player && !m.Body.IsHuman && m.Skills[SkillName.Wrestling].Value > m.Skills[sk].Value )
                    sk = SkillName.Wrestling;
            }

            return sk;
        }

        public virtual double GetAttackSkillValue( Mobile attacker, Mobile defender )
        {
            return attacker.Skills[GetUsedSkill(attacker, false)].Value;
        }

        public virtual double GetDefendSkillValue( Mobile attacker, Mobile defender )
        {
            return defender.Skills[GetUsedSkill(defender, false)].Value;
        }

        public virtual bool CheckHit( Mobile attacker, Mobile defender )
        {
            BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
            BaseWeapon defWeapon = defender.Weapon as BaseWeapon;

            Skill atkSkill = attacker.Skills[atkWeapon.Skill];

            double atkValue = atkWeapon.GetAttackSkillValue(attacker, defender);
            double defValue = defWeapon.GetDefendSkillValue(attacker, defender);
            double ourValue, theirValue;

            int bonus = GetHitChanceBonus();

            if( Core.AOS )
            {
                if( atkValue <= -20.0 )
                    atkValue = -19.9;

                if( defValue <= -20.0 )
                    defValue = -19.9;

                // Hit Chance Increase = 45%
                int atkChance = AosAttributes.GetValue(attacker, AosAttribute.AttackChance);
                if( atkChance > 45 )
                    atkChance = 45;

                bonus += atkChance;

                if( HitLower.IsUnderAttackEffect(attacker) )
                    bonus -= 25; // Under Hit Lower Attack effect -> 25% malus

                ourValue = (atkValue + 20.0) * (100 + bonus);

                // Defense Chance Increase = 45%
                bonus = AosAttributes.GetValue(defender, AosAttribute.DefendChance);
                if( bonus > 45 )
                    bonus = 45;

                if( HitLower.IsUnderDefenseEffect(defender) )
                    bonus -= 25; // Under Hit Lower Defense effect -> 25% malus

                int blockBonus = 0;

                if( Block.GetBonus(defender, ref blockBonus) )
                    bonus += blockBonus;

                int discordanceEffect = 0;

                // Defender loses -0/-28% if under the effect of Discordance.
                if( SkillHandlers.Discordance.GetEffect(attacker, ref discordanceEffect) )
                    bonus -= discordanceEffect;

                theirValue = (defValue + 20.0) * (100 + bonus);

                bonus = 0;
            }
            else
            {
                //if( atkValue <= -50.0 )
                //    atkValue = -49.9;

                //if( defValue <= -50.0 )
                //    defValue = -49.9;

                //ourValue = (atkValue + 50.0);
                //theirValue = (defValue + 50.0);
                ourValue = ((atkValue + 20.0) * 100);
                theirValue = ((defValue + 20.0) * 100);
            }

            double chance = ourValue / (theirValue * 1.4);

            chance *= 1.0 + ((double)bonus / 100);

            if( Core.AOS && chance < 0.02 )
                chance = 0.02;

            WeaponAbility ability = WeaponAbility.GetCurrentAbility(attacker);

            if( ability != null )
                chance += ability.AccuracyBonus;

            SpecialMove move = SpecialMove.GetCurrentMove(attacker);

            if( move != null )
                chance += move.GetAccuracyBonus(attacker);

            if( attacker.Player )
            {
                Acrobat acr = Perk.GetByType<Acrobat>((Player)attacker);
                if( acr != null )
                    chance += acr.GetHitChanceBonus();

                Dragoon drg = Perk.GetByType<Dragoon>((Player)attacker);
                if (drg != null)
                    chance += drg.LongArm(this);

                Marksman mm = Perk.GetByType<Marksman>((Player)attacker);
                if (mm != null)
                    chance += mm.DeadAim(attacker, defender);
            }

            if( defender.Player )
            {
                Pugilist pug = Perk.GetByType<Pugilist>((Player)defender);
                if (pug != null && pug.TryDodge())
                    chance = 0;

                Legionnaire leg = Perk.GetByType<Legionnaire>((Player)defender);
                if( leg != null && leg.TryDodge(this) )
                    chance = 0;

                Acrobat acr = Perk.GetByType<Acrobat>((Player)defender);
                if( acr != null && acr.TryDodge() )
                    chance = 0;

                Rogue rge = Perk.GetByType<Rogue>((Player)defender);
                if (rge != null && rge.CloakAndDagger(attacker))
                     chance = 0;
            }

            return attacker.CheckSkill(atkSkill.SkillName, chance);
        }

        public virtual TimeSpan GetDelay( Mobile m )
        {
            double speed = this.Speed;

            if( speed == 0 )
                return TimeSpan.FromHours(1.0);

            double delayInSeconds;

            if( Core.ML )
            {
                /*
                 * This is likely true for Core.AOS as well... both guides report the same
                 * formula, and both are wrong.
                 * The old formula left in for AOS for legacy & because we aren't quite 100%
                 * Sure that AOS has THIS formula
                 */
                int bonus = AosAttributes.GetValue(m, AosAttribute.WeaponSpeed);

                if( DualWield.Registry.Contains(m) )
                    bonus += ((DualWield.DualWieldTimer)DualWield.Registry[m]).BonusSwingSpeed;

                if( Feint.Registry.Contains(m) )
                    bonus -= ((Feint.FeintTimer)Feint.Registry[m]).SwingSpeedReduction;

                int discordanceEffect = 0;

                // Discordance gives a malus of -0/-28% to swing speed.
                if( SkillHandlers.Discordance.GetEffect(m, ref discordanceEffect) )
                    bonus -= discordanceEffect;

                if( bonus > 75 )
                    bonus = 75;

                double ticks;

                if( Core.AOS )
                {
                    int stamTicks = m.Stam / 30;

                    ticks = speed * 4;
                    ticks = Math.Floor((ticks - stamTicks) * (100.0 / (100 + bonus)));
                }
                else
                {
                    speed = Math.Floor(speed * (bonus + 100.0) / 100.0);

                    if( speed <= 0 )
                        speed = 1;

                    ticks = Math.Floor((80000.0 / ((m.Stam + 100) * speed)) - 2);
                }

                // Swing speed currently capped at one swing every 1.25 seconds (5 ticks).
                if( ticks < 5 )
                    ticks = 5;

                delayInSeconds = ticks * 0.25;
            }

            else if( Core.AOS )
            {
                int v = (m.Stam + 100) * (int)speed;

                int bonus = AosAttributes.GetValue(m, AosAttribute.WeaponSpeed);

                if (m is Player)
                {
                    Marksman mm = Perk.GetByType<Marksman>((Player)m);

                    if (mm != null)
                    {
                        bonus += mm.QuickDraw(this);
                    }

                    Pugilist pug = Perk.GetByType<Pugilist>((Player)m);

                    if (pug != null)
                    {
                        bonus += pug.Brawler(this);
                    }
                }

                int discordanceEffect = 0;

                // Discordance gives a malus of -0/-28% to swing speed.
                if( SkillHandlers.Discordance.GetEffect(m, ref discordanceEffect) )
                    bonus -= discordanceEffect;

                v += AOS.Scale(v, bonus);

                if( v <= 0 )
                    v = 1;

                delayInSeconds = Math.Floor(40000.0 / v) * 0.5;

                // Maximum swing rate capped at one swing per second
                // OSI dev said that it has and is supposed to be 1.25
                if( delayInSeconds < 1.25 )
                    delayInSeconds = 1.25;
            }
            else
            {
                int v = (m.Stam + 100) * (int)speed;

                if( v <= 0 )
                    v = 1;

                delayInSeconds = 15000.0 / v;
            }

            return TimeSpan.FromSeconds(delayInSeconds);
        }

        public virtual void OnBeforeSwing( Mobile attacker, Mobile defender )
        {
            if (attacker is Player)
            {
                Rogue rge = Perk.GetByType<Rogue>((Player)attacker);

                if (rge != null)
                {
                    rge.Ambush = true;
                }
            }

            WeaponAbility a = WeaponAbility.GetCurrentAbility(attacker);

            if( a != null && !a.OnBeforeSwing(attacker, defender) )
                WeaponAbility.ClearCurrentAbility(attacker);

            SpecialMove move = SpecialMove.GetCurrentMove(attacker);

            if( move != null && !move.OnBeforeSwing(attacker, defender) )
                SpecialMove.ClearCurrentMove(attacker);
        }

        public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender )
        {
            return OnSwing(attacker, defender, 1.0);
        }

        public bool canSwing = true;

        public virtual TimeSpan OnSwing( Mobile attacker, Mobile defender, double damageBonus )
        {

            if (Core.AOS)
            {
                canSwing = (!attacker.Paralyzed && !attacker.Frozen);

                if (canSwing)
                {
                    Spell sp = attacker.Spell as Spell;

                    canSwing = (sp == null || !sp.IsCasting || !sp.BlocksMovement);
                }

                if (canSwing)
                {
                    PlayerMobile p = attacker as PlayerMobile;

                    canSwing = (p == null || p.PeacedUntil <= DateTime.Now);
                }
            }

            if (attacker is Player && attacker.Weapon is Fists )
            {
                Item weapon = attacker.Weapon as Fists;

                if (attacker is Player)
                {
                    Pugilist pug = Perk.GetByType<Pugilist>((Player)attacker);

                    if (pug != null)
                    {
                        pug.MartialArt(attacker, defender);
                    }
                }

                if (attacker != null)
                {
                    if (attacker.Stam < 2)
                    {
                        attacker.SendMessage("You do not have the stamina to swing your fists.");
                        canSwing = false;
                    }
                    else
                    {
                        attacker.Stam -= 2;
                    }
                }
            }

            if (this.Parent is Player && this is BaseWeapon)
            {
                Item weapon = attacker.Weapon as BaseWeapon;

                if (weapon != null)
                {
                    if (attacker.Stam < (int)(((weapon.Weight + 2) / 2) + 2))
                        {
                            canSwing = false;
                            attacker.SendMessage("You do not have the stamina to swing your weapon.");
                        }
                        else
                        {
                            attacker.Stam -= (int)(((weapon.Weight + 2) / 2) + 2);
                        }             
                }
            }

            if( canSwing && attacker.HarmfulCheck(defender) )
            {
                attacker.DisruptiveAction();

                if( attacker.NetState != null )
                    attacker.Send(new Swing(0, attacker, defender));

                if( attacker is BaseCreature )
                {
                    BaseCreature bc = (BaseCreature)attacker;
                    WeaponAbility ab = bc.GetWeaponAbility();

                    if( ab != null )
                    {
                        if( bc.WeaponAbilityChance > Utility.RandomDouble() )
                            WeaponAbility.SetCurrentAbility(bc, ab);
                        else
                            WeaponAbility.ClearCurrentAbility(bc);
                    }
                }

                if( CheckHit(attacker, defender) )
                    OnHit(attacker, defender, damageBonus);
                else
                    OnMiss(attacker, defender);

                if (attacker.NetState != null)
                    attacker.Send(new Swing(0, attacker, defender));
            }

            return GetDelay(attacker);
        }

        #region Sounds
        public virtual int GetHitAttackSound( Mobile attacker, Mobile defender )
        {
            int sound = attacker.GetAttackSound();

            if( sound == -1 )
                sound = HitSound;

            return sound;
        }

        public virtual int GetHitDefendSound( Mobile attacker, Mobile defender )
        {
            return defender.GetHurtSound();
        }

        public virtual int GetMissAttackSound( Mobile attacker, Mobile defender )
        {
            if( attacker.GetAttackSound() == -1 )
                return MissSound;
            else
                return -1;
        }

        public virtual int GetMissDefendSound( Mobile attacker, Mobile defender )
        {
            return -1;
        }
        #endregion

        public static bool CheckParry( Mobile defender )
        {
            if( defender == null )
                return false;

            BaseShield shield = defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield;

            double parry = defender.Skills[SkillName.Parry].Value;

            if( shield != null )
            {
                double chance = parry / 300.0;

                if( parry >= 100.0 )
                    chance += (parry - 100) / 100;

                // Low dexterity lowers the chance.
                if( defender.Dex < 80 )
                    chance = chance * (20 + defender.Dex) / 100;

                return defender.CheckSkill(SkillName.Parry, chance);
            }
            else if( !(defender.Weapon is Fists) && !(defender.Weapon is BaseRanged) )
            {
                BaseWeapon weapon = defender.Weapon as BaseWeapon;

                double divisor = (weapon.Layer == Layer.OneHanded) ? 48000.0 : 41140.0;
                double chance = (parry / divisor);
                double aosChance = parry / 800.0;

                // Parry or Bushido over 100 grant a 5% bonus.
                if( parry >= 100.0 )
                {
                    chance += (parry - 100) / 100;
                    aosChance += (parry - 100) / 100;
                }

                // Low dexterity lowers the chance.
                if( defender.Dex < 80 )
                    chance = chance * (20 + defender.Dex) / 100;

                if( chance > aosChance )
                    return defender.CheckSkill(SkillName.Parry, chance);
                else
                    return (aosChance > Utility.RandomDouble()); // Only skillcheck if wielding a shield & there's no effect from Bushido
            }

            return false;
        }

        public virtual int AbsorbDamageAOS( Mobile attacker, Mobile defender, int damage )
        {
            bool blocked = false;

            if( defender.Player || defender.Body.IsHuman )
            {
                blocked = CheckParry(defender);

                if( blocked )
                {
                    defender.FixedEffect(0x37B9, 10, 16);
                    damage = 0;

                    BaseShield shield = defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield;

                    if( shield != null )
                    {
                        shield.OnHit(this, damage);
                    }
                }
            }

            if( !blocked )
            {
                double positionChance = Utility.RandomDouble();

                Item armorItem;

                if( positionChance < 0.07 )
                    armorItem = defender.NeckArmor;
                else if( positionChance < 0.14 )
                    armorItem = defender.HandArmor;
                else if( positionChance < 0.28 )
                    armorItem = defender.ArmsArmor;
                else if( positionChance < 0.43 )
                    armorItem = defender.HeadArmor;
                else if( positionChance < 0.65 )
                    armorItem = defender.LegsArmor;
                else
                    armorItem = defender.ChestArmor;

                IWearableDurability armor = armorItem as IWearableDurability;

                if( armor != null )
                    armor.OnHit(this, damage); // call OnHit to lose durability
            }

            return damage;
        }

        public virtual int AbsorbDamage( Mobile attacker, Mobile defender, int damage )
        {
            if( Core.AOS )
                return AbsorbDamageAOS(attacker, defender, damage);

            double chance = Utility.RandomDouble();

            Item armorItem;

            if( chance < 0.07 )
                armorItem = defender.NeckArmor;
            else if( chance < 0.14 )
                armorItem = defender.HandArmor;
            else if( chance < 0.28 )
                armorItem = defender.ArmsArmor;
            else if( chance < 0.43 )
                armorItem = defender.HeadArmor;
            else if( chance < 0.65 )
                armorItem = defender.LegsArmor;
            else
                armorItem = defender.ChestArmor;

            IWearableDurability armor = armorItem as IWearableDurability;

            if( armor != null )
                damage = armor.OnHit(this, damage);

            BaseShield shield = defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield;
            if( shield != null )
                damage = shield.OnHit(this, damage);

            int virtualArmor = defender.VirtualArmor + defender.VirtualArmorMod;

            if( virtualArmor > 0 )
            {
                double scalar;

                if( chance < 0.14 )
                    scalar = 0.07;
                else if( chance < 0.28 )
                    scalar = 0.14;
                else if( chance < 0.43 )
                    scalar = 0.15;
                else if( chance < 0.65 )
                    scalar = 0.22;
                else
                    scalar = 0.35;

                int from = (int)(virtualArmor * scalar) / 2;
                int to = (int)(virtualArmor * scalar);

                damage -= Utility.Random(from, (to - from) + 1);
            }

            return damage;
        }

        public virtual int GetPackInstinctBonus( Mobile attacker, Mobile defender )
        {
            if( attacker.Player || defender.Player )
                return 0;

            BaseCreature bc = attacker as BaseCreature;

                   if (bc == null || bc.PackInstinct == PackInstinct.None || (!bc.Controlled && !bc.Summoned))
                        return 0;

            Mobile master = bc.ControlMaster;

            if( master == null )
                master = bc.SummonMaster;

            if( master == null )
                return 0;

            int inPack = 1;

            foreach( Mobile m in defender.GetMobilesInRange(3) )
            {
                if( m != attacker && m is BaseCreature )
                {
                    BaseCreature tc = (BaseCreature)m;

                    if ((tc.PackInstinct & bc.PackInstinct) == 0 || (!tc.Controlled && !tc.Summoned) )
                        continue;

                    Mobile theirMaster = tc.ControlMaster;

                    if( theirMaster == null )
                        theirMaster = tc.SummonMaster;

                    if( master == theirMaster && tc.Combatant == defender )
                        ++inPack;
                }
            }

            if( inPack >= 5 )
                return 100;
            else if( inPack >= 4 )
                return 75;
            else if( inPack >= 3 )
                return 50;
            else if( inPack >= 2 )
                return 25;

            return 0;
        }

        private static bool m_InDoubleStrike;

        public static bool InDoubleStrike
        {
            get { return m_InDoubleStrike; }
            set { m_InDoubleStrike = value; }
        }

        public virtual void OnHit( Mobile attacker, Mobile defender )
        {
            OnHit(attacker, defender, 1.0);
        }

        public virtual void OnHit( Mobile attacker, Mobile defender, double damageBonus )
        {
            int damage = ComputeDamage(attacker, defender);

            if (attacker is Player)
            {
                Adventurer adv = Perk.GetByType<Adventurer>((Player)attacker);

                if (adv != null)
                {
                    adv.TorchAttack(attacker, defender);
                }

                Rogue rge = Perk.GetByType<Rogue>((Player)attacker);

                if (rge != null && rge.Ambush)
                {
                    rge.SurpriseAttack(defender, damage);
                }

                Pugilist pug = Perk.GetByType<Pugilist>((Player)attacker);

                if (pug != null)
                {
                    pug.ComboAttack(attacker, defender, this);
                }

                Monk mk = Perk.GetByType<Monk>((Player)attacker);

                if (mk != null)
                {
                    mk.QiStrike( defender, this, damage );
                    mk.Purge(defender, this);
                    mk.ShenStrike(attacker, defender, this);
                }

                Warlock wlk = Perk.GetByType<Warlock>((Player)attacker);

                if (wlk != null)
                {
                    wlk.Capacitor(attacker, defender, this);
                    wlk.SoulEater(attacker, defender, this);
                }

            }

            PlaySwingAnimation(attacker);
            PlayHurtAnimation(defender);

            attacker.PlaySound(GetHitAttackSound(attacker, defender));
            defender.PlaySound(GetHitDefendSound(attacker, defender));

            #region Damage Multipliers

            int percentageBonus = 0;

            WeaponAbility a = WeaponAbility.GetCurrentAbility(attacker);
            SpecialMove move = SpecialMove.GetCurrentMove(attacker);

            if( a != null )
            {
                percentageBonus += (int)(a.DamageScalar * 100) - 100;
            }

            if( move != null )
            {
                percentageBonus += (int)(move.GetDamageScalar(attacker, defender) * 100) - 100;
            }

            percentageBonus += (int)(damageBonus * 100) - 100;

            CheckSlayerResult cs = CheckSlayers(attacker, defender);

            if( cs != CheckSlayerResult.None )
            {
                if( cs == CheckSlayerResult.Slayer )
                    defender.FixedEffect(0x37B9, 10, 5);

                percentageBonus += 100;
            }

            int packInstinctBonus = GetPackInstinctBonus(attacker, defender);

            if( packInstinctBonus != 0 )
            {
                percentageBonus += packInstinctBonus;
            }

            if( m_InDoubleStrike )
            {
                percentageBonus -= 5;
            }

            if ( defender is Player ) 
            {
                Adventurer adv = Perk.GetByType<Adventurer>((Player)defender);

                if (adv != null)
                    percentageBonus -= adv.GetDamageReduction();

                if (((Player)defender).Race == Race.Ogre)
                {
                    percentageBonus -= 15;
                }
            }

            if (attacker is Player)
            {
                damage += ((Player)attacker).BodyDamageBonus;

                Dragoon drg = Perk.GetByType<Dragoon>((Player)attacker);

                if (drg != null)
                {
                    percentageBonus += drg.MomentousStrike(attacker);
                }

                Pugilist pug = Perk.GetByType<Pugilist>((Player)attacker);

                if (pug != null && pug.Hardened() && attacker.Weapon is Fists)
                {
                    damage += 12;
                }
            }


            if( attacker is PlayerMobile && !(Core.ML && defender is PlayerMobile) )
            {
                PlayerMobile pmAttacker = (PlayerMobile)attacker;

                if( pmAttacker.HonorActive && pmAttacker.InRange(defender, 1) )
                {
                    percentageBonus += 25;
                }

                if( pmAttacker.SentHonorContext != null && pmAttacker.SentHonorContext.Target == defender )
                {
                    percentageBonus += pmAttacker.SentHonorContext.PerfectionDamageBonus;
                }
            }

            percentageBonus = Math.Min(percentageBonus, 300);

            damage = AOS.Scale(damage, 100 + percentageBonus);

            #endregion

            damage = AbsorbDamage(attacker, defender, damage);
            damage = (int)((damage * 0.95) + this.Weight);

            if( Core.ML && damage < 1 )
                damage = 0;

            else if( Core.AOS && damage == 0 ) // parried
            {
                if( a != null && a.Validate(attacker) )
                {
                    a = null;
                    WeaponAbility.ClearCurrentAbility(attacker);

                    attacker.SendLocalizedMessage(1061140); // Your attack was parried!
                }
            }

            AddBlood(attacker, defender, damage);

            int phys, fire, cold, pois, nrgy;

            GetDamageTypes(attacker, out phys, out fire, out cold, out pois, out nrgy);

            if( m_Consecrated )
            {
                phys = defender.PhysicalResistance;
                fire = defender.FireResistance;
                cold = defender.ColdResistance;
                pois = defender.PoisonResistance;
                nrgy = defender.EnergyResistance;

                int low = phys, type = 0;

                if( fire < low ) { low = fire; type = 1; }
                if( cold < low ) { low = cold; type = 2; }
                if( pois < low ) { low = pois; type = 3; }
                if( nrgy < low ) { low = nrgy; type = 4; }

                phys = fire = cold = pois = nrgy = 0;

                if( type == 0 ) phys = 100;
                else if( type == 1 ) fire = 100;
                else if( type == 2 ) cold = 100;
                else if( type == 3 ) pois = 100;
                else if( type == 4 ) nrgy = 100;
            }

            int damageGiven = damage;

            if( a != null && !a.OnBeforeDamage(attacker, defender) )
            {
                WeaponAbility.ClearCurrentAbility(attacker);
                a = null;
            }

            if( move != null && !move.OnBeforeDamage(attacker, defender) )
            {
                SpecialMove.ClearCurrentMove(attacker);
                move = null;
            }

            bool ignoreArmor = (a is ArmorIgnore || (move != null && move.IgnoreArmor(attacker)));

            damageGiven = AOS.Damage(defender, attacker, damage, ignoreArmor, phys, fire, cold, pois, nrgy);

            double propertyBonus = (move == null) ? 1.0 : move.GetPropertyBonus(attacker);

            if( Core.AOS )
            {
                int lifeLeech = 0;
                int stamLeech = 0;
                int manaLeech = 0;

                if( (int)(m_AosWeaponAttributes.HitLeechHits * propertyBonus) > Utility.Random(100) )
                    lifeLeech += 50; // HitLeechHits% chance to leech 50% of damage as hit points

                if( (int)(m_AosWeaponAttributes.HitLeechStam * propertyBonus) > Utility.Random(100) )
                    stamLeech += 50; // HitLeechStam% chance to leech 50% of damage as stamina

                if( (int)(m_AosWeaponAttributes.HitLeechMana * propertyBonus) > Utility.Random(100) )
                    manaLeech += 50; // HitLeechMana% chance to leech 50% of damage as mana

                if( m_Cursed )
                    lifeLeech += 50; // Additional 50% life leech for cursed weapons (necro spell)

                if( lifeLeech != 0 )
                    attacker.Hits += AOS.Scale(damageGiven, lifeLeech);

                if( stamLeech != 0 )
                    attacker.Stam += AOS.Scale(damageGiven, stamLeech);

                if( manaLeech != 0 )
                    attacker.Mana += AOS.Scale(damageGiven, manaLeech);

                if( lifeLeech != 0 || stamLeech != 0 || manaLeech != 0 )
                    attacker.PlaySound(0x44D);
            }

            if( m_MaxHits > 0 && ((MaxRange <= 1 && (defender is Slime || defender is ToxicElemental)) || Utility.Random(25) == 0) ) // Stratics says 50% chance, seems more like 4%..
            {
                if( MaxRange <= 1 && (defender is Slime || defender is ToxicElemental) )
                    attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500263); // *Acid blood scars your weapon!*

                if( Core.AOS && m_AosWeaponAttributes.SelfRepair > Utility.Random(10) )
                {
                    HitPoints += 2;
                }
                else
                {
                    if( m_Hits > 0 )
                    {
                        --HitPoints;
                    }
                    else if( m_MaxHits > 1 )
                    {
                        --MaxHitPoints;

                        if( Parent is Mobile )
                            ((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                    }
                    else
                    {
                        Delete();
                    }
                }
            }

            if( Core.AOS )
            {
                int physChance = (int)(m_AosWeaponAttributes.HitPhysicalArea * propertyBonus);
                int fireChance = (int)(m_AosWeaponAttributes.HitFireArea * propertyBonus);
                int coldChance = (int)(m_AosWeaponAttributes.HitColdArea * propertyBonus);
                int poisChance = (int)(m_AosWeaponAttributes.HitPoisonArea * propertyBonus);
                int nrgyChance = (int)(m_AosWeaponAttributes.HitEnergyArea * propertyBonus);

                if( physChance != 0 && physChance > Utility.Random(100) )
                    DoAreaAttack(attacker, defender, 0x10E, 150, 0, 0, 0, 0, 0);

                if( fireChance != 0 && fireChance > Utility.Random(100) )
                    DoAreaAttack(attacker, defender, 0x11D, 1160, 0, 100, 0, 0, 0);

                if( coldChance != 0 && coldChance > Utility.Random(100) )
                    DoAreaAttack(attacker, defender, 0x0FC, 2100, 0, 0, 100, 0, 0);

                if( poisChance != 0 && poisChance > Utility.Random(100) )
                    DoAreaAttack(attacker, defender, 0x205, 1166, 0, 0, 0, 100, 0);

                if( nrgyChance != 0 && nrgyChance > Utility.Random(100) )
                    DoAreaAttack(attacker, defender, 0x1F1, 120, 0, 0, 0, 0, 100);

                int maChance = (int)(m_AosWeaponAttributes.HitMagicArrow * propertyBonus);
                int harmChance = (int)(m_AosWeaponAttributes.HitHarm * propertyBonus);
                int fireballChance = (int)(m_AosWeaponAttributes.HitFireball * propertyBonus);
                int lightningChance = (int)(m_AosWeaponAttributes.HitLightning * propertyBonus);
                int dispelChance = (int)(m_AosWeaponAttributes.HitDispel * propertyBonus);

                if( maChance != 0 && maChance > Utility.Random(100) )
                    DoMagicArrow(attacker, defender);

                if( harmChance != 0 && harmChance > Utility.Random(100) )
                    DoHarm(attacker, defender);

                if( fireballChance != 0 && fireballChance > Utility.Random(100) )
                    DoFireball(attacker, defender);

                if( lightningChance != 0 && lightningChance > Utility.Random(100) )
                    DoLightning(attacker, defender);

                if( dispelChance != 0 && dispelChance > Utility.Random(100) )
                    DoDispel(attacker, defender);

                int laChance = (int)(m_AosWeaponAttributes.HitLowerAttack * propertyBonus);
                int ldChance = (int)(m_AosWeaponAttributes.HitLowerDefend * propertyBonus);

                if( laChance != 0 && laChance > Utility.Random(100) )
                    DoLowerAttack(attacker, defender);

                if( ldChance != 0 && ldChance > Utility.Random(100) )
                    DoLowerDefense(attacker, defender);
            }

            if( attacker is BaseCreature )
                ((BaseCreature)attacker).OnGaveMeleeAttack(defender);

            if( defender is BaseCreature )
                ((BaseCreature)defender).OnGotMeleeAttack(attacker);

            if( a != null )
                a.OnHit(attacker, defender, damage);

            if( move != null )
                move.OnHit(attacker, defender, damage);

            if( defender is IHonorTarget && ((IHonorTarget)defender).ReceivedHonorContext != null )
                ((IHonorTarget)defender).ReceivedHonorContext.OnTargetHit(attacker);
        }

        public virtual double GetAosDamage( Mobile attacker, int bonus, int dice, int sides )
        {
            int damage = Utility.Dice(dice, sides, bonus) * 100;
            int damageBonus = 0;

            // Inscription bonus
            int inscribeSkill = attacker.Skills[SkillName.Inscribe].Fixed;

            damageBonus += inscribeSkill / 200;

            if( inscribeSkill >= 1000 )
                damageBonus += 5;

            if( attacker.Player )
            {
                // Int bonus
                damageBonus += (attacker.Int / 10);

                // SDI bonus
                damageBonus += AosAttributes.GetValue(attacker, AosAttribute.SpellDamage);
            }

            damage = AOS.Scale(damage, 100 + damageBonus);

            return damage / 100;
        }

        #region Do<AoSEffect>
        public virtual void DoMagicArrow( Mobile attacker, Mobile defender )
        {
            if( !attacker.CanBeHarmful(defender, false) )
                return;

            attacker.DoHarmful(defender);

            double damage = GetAosDamage(attacker, 10, 1, 4);

            attacker.MovingParticles(defender, 0x36E4, 5, 0, false, true, 3006, 4006, 0);
            attacker.PlaySound(0x1E5);

            SpellHelper.Damage(TimeSpan.FromSeconds(1.0), defender, attacker, damage, 0, 100, 0, 0, 0);
        }

        public virtual void DoHarm( Mobile attacker, Mobile defender )
        {
            if( !attacker.CanBeHarmful(defender, false) )
                return;

            attacker.DoHarmful(defender);

            double damage = GetAosDamage(attacker, 17, 1, 5);

            if( !defender.InRange(attacker, 2) )
                damage *= 0.25; // 1/4 damage at > 2 tile range
            else if( !defender.InRange(attacker, 1) )
                damage *= 0.50; // 1/2 damage at 2 tile range

            defender.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
            defender.PlaySound(0x0FC);

            SpellHelper.Damage(TimeSpan.Zero, defender, attacker, damage, 0, 0, 100, 0, 0);
        }

        public virtual void DoFireball( Mobile attacker, Mobile defender )
        {
            if( !attacker.CanBeHarmful(defender, false) )
                return;

            attacker.DoHarmful(defender);

            double damage = GetAosDamage(attacker, 19, 1, 5);

            attacker.MovingParticles(defender, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160);
            attacker.PlaySound(0x15E);

            SpellHelper.Damage(TimeSpan.FromSeconds(1.0), defender, attacker, damage, 0, 100, 0, 0, 0);
        }

        public virtual void DoLightning( Mobile attacker, Mobile defender )
        {
            if( !attacker.CanBeHarmful(defender, false) )
                return;

            attacker.DoHarmful(defender);

            double damage = GetAosDamage(attacker, 23, 1, 4);

            defender.BoltEffect(0);

            SpellHelper.Damage(TimeSpan.Zero, defender, attacker, damage, 0, 0, 0, 0, 100);
        }

        public virtual void DoDispel( Mobile attacker, Mobile defender )
        {
            bool dispellable = false;

            if( defender is BaseCreature )
                dispellable = ((BaseCreature)defender).Summoned;

            if( !dispellable )
                return;

            if( !attacker.CanBeHarmful(defender, false) )
                return;

            attacker.DoHarmful(defender);

            MagerySpell sp = new Spells.Sixth.DispelSpell(attacker, null);

            if( sp.CheckResisted(defender) )
            {
                defender.FixedEffect(0x3779, 10, 20);
            }
            else
            {
                Effects.SendLocationParticles(EffectItem.Create(defender.Location, defender.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                Effects.PlaySound(defender, defender.Map, 0x201);

                defender.Delete();
            }
        }

        public virtual void DoLowerAttack( Mobile from, Mobile defender )
        {
            if( HitLower.ApplyAttack(defender) )
            {
                defender.PlaySound(0x28E);
                Effects.SendTargetEffect(defender, 0x37BE, 1, 4, 0xA, 3);
            }
        }

        public virtual void DoLowerDefense( Mobile from, Mobile defender )
        {
            if( HitLower.ApplyDefense(defender) )
            {
                defender.PlaySound(0x28E);
                Effects.SendTargetEffect(defender, 0x37BE, 1, 4, 0x23, 3);
            }
        }

        public virtual void DoAreaAttack( Mobile from, Mobile defender, int sound, int hue, int phys, int fire, int cold, int pois, int nrgy )
        {
            Map map = from.Map;

            if( map == null )
                return;

            List<Mobile> list = new List<Mobile>();

            foreach( Mobile m in from.GetMobilesInRange(10) )
            {
                if( from != m && defender != m && SpellHelper.ValidIndirectTarget(from, m) && from.CanBeHarmful(m, false) && (!Core.ML || from.InLOS(m)) )
                    list.Add(m);
            }

            if( list.Count == 0 )
                return;

            Effects.PlaySound(from.Location, map, sound);

            // TODO: What is the damage calculation?

            for( int i = 0; i < list.Count; ++i )
            {
                Mobile m = list[i];

                from.DoHarmful(m, true);
                m.FixedEffect(0x3779, 1, 15, hue, 0);
                AOS.Damage(m, from, (int)((GetBaseDamage(from) + 5)), phys, fire, cold, pois, nrgy);
            }
        }
        #endregion

        public virtual CheckSlayerResult CheckSlayers( Mobile attacker, Mobile defender )
        {
            BaseWeapon atkWeapon = attacker.Weapon as BaseWeapon;
            SlayerEntry atkSlayer = SlayerGroup.GetEntryByName(atkWeapon.Slayer);
            SlayerEntry atkSlayer2 = SlayerGroup.GetEntryByName(atkWeapon.Slayer2);

            if( atkSlayer != null && atkSlayer.Slays(defender) || atkSlayer2 != null && atkSlayer2.Slays(defender) )
                return CheckSlayerResult.Slayer;

            if( !Core.SE )
            {
                ISlayer defISlayer = Spellbook.FindEquippedSpellbook(defender);

                if( defISlayer == null )
                    defISlayer = defender.Weapon as ISlayer;

                if( defISlayer != null )
                {
                    SlayerEntry defSlayer = SlayerGroup.GetEntryByName(defISlayer.Slayer);
                    SlayerEntry defSlayer2 = SlayerGroup.GetEntryByName(defISlayer.Slayer2);

                    if( defSlayer != null && defSlayer.Group.OppositionSuperSlays(attacker) || defSlayer2 != null && defSlayer2.Group.OppositionSuperSlays(attacker) )
                        return CheckSlayerResult.Opposition;
                }
            }

            return CheckSlayerResult.None;
        }

        public virtual void AddBlood( Mobile attacker, Mobile defender, int damage )
        {
            if( damage > 0 )
            {
                new Blood().MoveToWorld(defender.Location, defender.Map);

                int extraBlood = (Core.SE ? Utility.RandomMinMax(3, 4) : Utility.RandomMinMax(0, 1));

                for( int i = 0; i < extraBlood; i++ )
                {
                    new Blood().MoveToWorld(new Point3D(
                                                         defender.X + Utility.RandomMinMax(-1, 1),
                                                         defender.Y + Utility.RandomMinMax(-1, 1),
                                                         defender.Z), defender.Map);
                }
            }
        }

        public virtual void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
        {
            if( wielder is BaseCreature )
            {
                BaseCreature bc = (BaseCreature)wielder;

                phys = bc.PhysicalDamage;
                fire = bc.FireDamage;
                cold = bc.ColdDamage;
                pois = bc.PoisonDamage;
                nrgy = bc.EnergyDamage;
            }
            else
            {
                phys = 100;
                fire = 0;
                cold = 0;
                pois = 0;
                nrgy = 0;
            }
        }

        public virtual void OnMiss( Mobile attacker, Mobile defender )
        {
            PlaySwingAnimation(attacker);
            attacker.PlaySound(GetMissAttackSound(attacker, defender));
            defender.PlaySound(GetMissDefendSound(attacker, defender));

            WeaponAbility ability = WeaponAbility.GetCurrentAbility(attacker);

            if( ability != null )
                ability.OnMiss(attacker, defender);

            SpecialMove move = SpecialMove.GetCurrentMove(attacker);

            if( move != null )
                move.OnMiss(attacker, defender);

            if( defender is IHonorTarget && ((IHonorTarget)defender).ReceivedHonorContext != null )
                ((IHonorTarget)defender).ReceivedHonorContext.OnTargetMissed(attacker);
        }

        public virtual void GetBaseDamageRange( Mobile attacker, out int min, out int max )
        {
            if( attacker is BaseCreature )
            {
                BaseCreature c = (BaseCreature)attacker;

                if( c.DamageMin >= 0 )
                {
                    min = c.DamageMin;
                    max = c.DamageMax;
                    return;
                }

                if( this is Fists && !attacker.Body.IsHuman )
                {
                    min = attacker.Str / 28;
                    max = attacker.Str / 28;
                    return;
                }
            }

            min = MinDamage;
            max = MaxDamage;
        }

        public virtual double GetBaseDamage( Mobile attacker )
        {
            int min, max;

            GetBaseDamageRange(attacker, out min, out max);

            return Utility.RandomMinMax(min, max);
        }

        public virtual double GetBonus( double value, double scalar, double threshold, double offset )
        {
            double bonus = value * scalar;

            if( value >= threshold )
                bonus += offset;

            return bonus / 100;
        }

        public virtual int GetHitChanceBonus()
        {
            int bonus = 0;

            switch( m_AccuracyLevel )
            {
                case WeaponAccuracyLevel.Accurate: bonus += 02; break;
                case WeaponAccuracyLevel.Surpassingly: bonus += 04; break;
                case WeaponAccuracyLevel.Eminently: bonus += 06; break;
                case WeaponAccuracyLevel.Exceedingly: bonus += 08; break;
                case WeaponAccuracyLevel.Supremely: bonus += 10; break;
            }

            return bonus;
        }

        public virtual int GetDamageBonus()
        {
            int bonus = VirtualDamageBonus;

            switch( m_Quality )
            {
                case WeaponQuality.Low: bonus -= 20; break;
                case WeaponQuality.Exceptional: bonus += 20; break;
            }

            switch( m_DamageLevel )
            {
                case WeaponDamageLevel.Ruin: bonus += 15; break;
                case WeaponDamageLevel.Might: bonus += 20; break;
                case WeaponDamageLevel.Force: bonus += 25; break;
                case WeaponDamageLevel.Power: bonus += 30; break;
                case WeaponDamageLevel.Vanq: bonus += 35; break;
            }

            return bonus;
        }

        public virtual void GetStatusDamage( Mobile from, out int min, out int max )
        {
            int baseMin, baseMax;

            GetBaseDamageRange(from, out baseMin, out baseMax);

            if( Core.AOS )
            {
                min = Math.Max((int)ScaleDamageAOS(from, baseMin, false), 1);
                max = Math.Max((int)ScaleDamageAOS(from, baseMax, false), 1);
            }
            else
            {
                min = Math.Max((int)ScaleDamageOld(from, baseMin, false), 1);
                max = Math.Max((int)ScaleDamageOld(from, baseMax, false), 1);
            }
        }

        public virtual double ScaleDamageAOS( Mobile attacker, double damage, bool checkSkills )
        {
            if( checkSkills )
            {
                attacker.CheckSkill(SkillName.Tactics, 0.0, attacker.Skills[SkillName.Tactics].Cap); // Passively check tactics for gain
                attacker.CheckSkill(SkillName.Anatomy, 0.0, attacker.Skills[SkillName.Anatomy].Cap); // Passively check Anatomy for gain

                if( Type == WeaponType.Axe )
                    attacker.CheckSkill(SkillName.Lumberjacking, 0.0, 100.0); // Passively check Lumberjacking for gain
            }

            #region Physical bonuses
            /*
			 * These are the bonuses given by the physical characteristics of the mobile.
			 * No caps apply.
			 */
            double strengthBonus = GetBonus(attacker.Str, 0.300, 100.0, 5.00);
            double anatomyBonus = GetBonus(attacker.Skills[SkillName.Anatomy].Value, 0.500, 100.0, 5.00);
            double tacticsBonus = GetBonus(attacker.Skills[SkillName.Tactics].Value, 0.625, 100.0, 6.25);
            double lumberBonus = GetBonus(attacker.Skills[SkillName.Lumberjacking].Value, 0.200, 100.0, 10.00);

            if( Type != WeaponType.Axe )
                lumberBonus = 0.0;
            #endregion

            #region Modifiers
            /*
			 * The following are damage modifiers whose effect shows on the status bar.
			 * Capped at 100% total.
			 */
            int damageBonus = AosAttributes.GetValue(attacker, AosAttribute.WeaponDamage);
            int defenseMasteryMalus = 0;

            // Defense Mastery gives a -50%/-80% malus to damage.
            if( Server.Items.DefenseMastery.GetMalus(attacker, ref defenseMasteryMalus) )
                damageBonus -= defenseMasteryMalus;

            int discordanceEffect = 0;

            // Discordance gives a -2%/-48% malus to damage.
            if( SkillHandlers.Discordance.GetEffect(attacker, ref discordanceEffect) )
                damageBonus -= discordanceEffect * 2;

            if( damageBonus > 100 )
                damageBonus = 100;
            #endregion

            double totalBonus = strengthBonus + anatomyBonus + tacticsBonus + lumberBonus + ((double)(GetDamageBonus() + damageBonus) / 100.0);

            return damage + (int)(damage * totalBonus);
        }

        public virtual int VirtualDamageBonus { get { return 0; } }

        public virtual int ComputeDamageAOS( Mobile attacker, Mobile defender )
        {
            return (int)ScaleDamageAOS(attacker, GetBaseDamage(attacker), true);
        }

        public virtual double ScaleDamageOld( Mobile attacker, double damage, bool checkSkills )
        {
            if( checkSkills )
            {
                attacker.CheckSkill(SkillName.Tactics, 0.0, attacker.Skills[SkillName.Tactics].Cap); // Passively check tactics for gain
                attacker.CheckSkill(SkillName.Anatomy, 0.0, attacker.Skills[SkillName.Anatomy].Cap); // Passively check Anatomy for gain

                if( Type == WeaponType.Axe )
                    attacker.CheckSkill(SkillName.Lumberjacking, 0.0, 100.0); // Passively check Lumberjacking for gain
            }

            /* Compute tactics modifier
             * :   0.0 = 50% loss
             * :  50.0 = unchanged
             * : 100.0 = 50% bonus
             */
            double tacticsBonus = (attacker.Skills[SkillName.Tactics].Value - 50.0) / 100.0;

            /* Compute strength modifier
             * : 1% bonus for every 5 strength
             */
            double strBonus = (attacker.Str / 5.0) / 100.0;

            /* Compute anatomy modifier
             * : 1% bonus for every 5 points of anatomy
             * : +10% bonus at Grandmaster or higher
             */
            double anatomyValue = attacker.Skills[SkillName.Anatomy].Value;
            double anatomyBonus = (anatomyValue / 5.0) / 100.0;

            if( anatomyValue >= 100.0 )
                anatomyBonus += 0.1;

            /* Compute lumberjacking bonus
             * : 1% bonus for every 5 points of lumberjacking
             * : +10% bonus at Grandmaster or higher
             */
            double lumberBonus;

            if( Type == WeaponType.Axe )
            {
                double lumberValue = attacker.Skills[SkillName.Lumberjacking].Value;

                lumberBonus = (lumberValue / 5.0) / 100.0;

                if( lumberValue >= 100.0 )
                    lumberBonus += 0.1;
            }
            else
            {
                lumberBonus = 0.0;
            }

            // New quality bonus:
            double qualityBonus = ((int)m_Quality - 1) * 0.2;

            // Apply bonuses
            damage += (damage * tacticsBonus) + (damage * strBonus) + (damage * anatomyBonus) + (damage * lumberBonus) + (damage * qualityBonus) + ((damage * VirtualDamageBonus) / 100);

            // Old quality bonus:
#if false
			/* Apply quality offset
			 * : Low         : -4
			 * : Regular     :  0
			 * : Exceptional : +4
			 */
			damage += ((int)m_Quality - 1) * 4.0;
#endif

            /* Apply damage level offset
			 * : Regular : 0
			 * : Ruin    : 1
			 * : Might   : 3
			 * : Force   : 5
			 * : Power   : 7
			 * : Vanq    : 9
			 */
            if( m_DamageLevel != WeaponDamageLevel.Regular )
                damage += (2.0 * (int)m_DamageLevel) - 1.0;

            return ScaleDamageByDurability((int)damage);
        }

        public virtual int ScaleDamageByDurability( int damage )
        {
            int scale = 100;

            if( m_MaxHits > 0 && m_Hits < m_MaxHits )
                scale = 50 + ((50 * m_Hits) / m_MaxHits);

            return AOS.Scale(damage, scale);
        }

        public virtual int ComputeDamage( Mobile attacker, Mobile defender )
        {
            if( Core.AOS )
                return ComputeDamageAOS(attacker, defender);

            return (int)ScaleDamageOld(attacker, GetBaseDamage(attacker), true);
        }

        public virtual void PlayHurtAnimation( Mobile from )
        {
            int action;
            int frames;

            switch( from.Body.Type )
            {
                case BodyType.Sea:
                case BodyType.Animal:
                    {
                        action = 7;
                        frames = 5;
                        break;
                    }
                case BodyType.Monster:
                    {
                        action = 10;
                        frames = 4;
                        break;
                    }
                case BodyType.Human:
                    {
                        action = 20;
                        frames = 5;
                        break;
                    }
                default: return;
            }

            if( from.Mounted )
                return;

            from.Animate(action, frames, 1, true, false, 0);
        }

        public virtual void PlaySwingAnimation( Mobile from )
        {
            int action;

            switch( from.Body.Type )
            {
                case BodyType.Sea:
                case BodyType.Animal:
                    {
                        action = Utility.Random(5, 2);
                        break;
                    }
                case BodyType.Monster:
                    {
                        switch( Animation )
                        {
                            default:
                            case WeaponAnimation.Wrestle:
                            case WeaponAnimation.Bash1H:
                            case WeaponAnimation.Pierce1H:
                            case WeaponAnimation.Slash1H:
                            case WeaponAnimation.Bash2H:
                            case WeaponAnimation.Pierce2H:
                            case WeaponAnimation.Slash2H: action = Utility.Random(4, 3); break;
                            case WeaponAnimation.ShootBow: return; // 7
                            case WeaponAnimation.ShootXBow: return; // 8
                        }

                        break;
                    }
                case BodyType.Human:
                    {
                        if( !from.Mounted )
                        {
                            action = (int)Animation;
                        }
                        else
                        {
                            switch( Animation )
                            {
                                default:
                                case WeaponAnimation.Wrestle:
                                case WeaponAnimation.Bash1H:
                                case WeaponAnimation.Pierce1H:
                                case WeaponAnimation.Slash1H: action = 26; break;
                                case WeaponAnimation.Bash2H:
                                case WeaponAnimation.Pierce2H:
                                case WeaponAnimation.Slash2H: action = 29; break;
                                case WeaponAnimation.ShootBow: action = 27; break;
                                case WeaponAnimation.ShootXBow: action = 28; break;
                            }
                        }

                        break;
                    }
                default: return;
            }

            from.Animate(action, 7, 1, true, false, 0);
        }

        #region Serialization/Deserialization
        private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
        {
            if( setIf )
                flags |= toSet;
        }

        private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
        {
            return ((flags & toGet) != 0);
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)11); // version

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.DamageLevel, m_DamageLevel != WeaponDamageLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.AccuracyLevel, m_AccuracyLevel != WeaponAccuracyLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.DurabilityLevel, m_DurabilityLevel != WeaponDurabilityLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Quality, m_Quality != WeaponQuality.Regular);
            SetSaveFlag(ref flags, SaveFlag.Hits, m_Hits != 0);
            SetSaveFlag(ref flags, SaveFlag.MaxHits, m_MaxHits != 0);
            SetSaveFlag(ref flags, SaveFlag.Slayer, m_Slayer != SlayerName.None);
            SetSaveFlag(ref flags, SaveFlag.Poison, m_Poison != null);
            SetSaveFlag(ref flags, SaveFlag.PoisonCharges, m_PoisonCharges != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Identified, m_Identified != false);
            SetSaveFlag(ref flags, SaveFlag.StrReq, m_StrReq != -1);
            SetSaveFlag(ref flags, SaveFlag.DexReq, m_DexReq != -1);
            SetSaveFlag(ref flags, SaveFlag.IntReq, m_IntReq != -1);
            SetSaveFlag(ref flags, SaveFlag.MinDamage, m_MinDamage != -1);
            SetSaveFlag(ref flags, SaveFlag.MaxDamage, m_MaxDamage != -1);
            SetSaveFlag(ref flags, SaveFlag.HitSound, m_HitSound != -1);
            SetSaveFlag(ref flags, SaveFlag.MissSound, m_MissSound != -1);
            SetSaveFlag(ref flags, SaveFlag.Speed, m_Speed != -1);
            SetSaveFlag(ref flags, SaveFlag.MaxRange, m_MaxRange != -1);
            SetSaveFlag(ref flags, SaveFlag.Skill, m_Skill != (SkillName)(-1));
            SetSaveFlag(ref flags, SaveFlag.Type, m_Type != (WeaponType)(-1));
            SetSaveFlag(ref flags, SaveFlag.Animation, m_Animation != (WeaponAnimation)(-1));
            SetSaveFlag(ref flags, SaveFlag.Resource, m_Resource != CraftResource.Iron);
            SetSaveFlag(ref flags, SaveFlag.xAttributes, !m_AosAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.xWeaponAttributes, !m_AosWeaponAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PlayerConstructed, m_PlayerConstructed);
            SetSaveFlag(ref flags, SaveFlag.SkillBonuses, !m_AosSkillBonuses.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.Slayer2, m_Slayer2 != SlayerName.None);

            writer.Write((int)flags);

            if( GetSaveFlag(flags, SaveFlag.DamageLevel) )
                writer.Write((int)m_DamageLevel);

            if( GetSaveFlag(flags, SaveFlag.AccuracyLevel) )
                writer.Write((int)m_AccuracyLevel);

            if( GetSaveFlag(flags, SaveFlag.DurabilityLevel) )
                writer.Write((int)m_DurabilityLevel);

            if( GetSaveFlag(flags, SaveFlag.Quality) )
                writer.Write((int)m_Quality);

            if( GetSaveFlag(flags, SaveFlag.Hits) )
                writer.Write((int)m_Hits);

            if( GetSaveFlag(flags, SaveFlag.MaxHits) )
                writer.Write((int)m_MaxHits);

            if( GetSaveFlag(flags, SaveFlag.Slayer) )
                writer.Write((int)m_Slayer);

            if( GetSaveFlag(flags, SaveFlag.Poison) )
                Poison.Serialize(m_Poison, writer);

            if( GetSaveFlag(flags, SaveFlag.PoisonCharges) )
                writer.Write((int)m_PoisonCharges);

            if( GetSaveFlag(flags, SaveFlag.Crafter) )
                writer.Write((Mobile)m_Crafter);

            if( GetSaveFlag(flags, SaveFlag.Identified) )
                writer.Write((bool)m_Identified);

            if( GetSaveFlag(flags, SaveFlag.StrReq) )
                writer.Write((int)m_StrReq);

            if( GetSaveFlag(flags, SaveFlag.DexReq) )
                writer.Write((int)m_DexReq);

            if( GetSaveFlag(flags, SaveFlag.IntReq) )
                writer.Write((int)m_IntReq);

            if( GetSaveFlag(flags, SaveFlag.MinDamage) )
                writer.Write((int)m_MinDamage);

            if( GetSaveFlag(flags, SaveFlag.MaxDamage) )
                writer.Write((int)m_MaxDamage);

            if( GetSaveFlag(flags, SaveFlag.HitSound) )
                writer.Write((int)m_HitSound);

            if( GetSaveFlag(flags, SaveFlag.MissSound) )
                writer.Write((int)m_MissSound);

            if( GetSaveFlag(flags, SaveFlag.Speed) )
                writer.Write((float)m_Speed);

            if( GetSaveFlag(flags, SaveFlag.MaxRange) )
                writer.Write((int)m_MaxRange);

            if( GetSaveFlag(flags, SaveFlag.Skill) )
                writer.Write((int)m_Skill);

            if( GetSaveFlag(flags, SaveFlag.Type) )
                writer.Write((int)m_Type);

            if( GetSaveFlag(flags, SaveFlag.Animation) )
                writer.Write((int)m_Animation);

            if( GetSaveFlag(flags, SaveFlag.Resource) )
                writer.Write((int)m_Resource);

            if( GetSaveFlag(flags, SaveFlag.xAttributes) )
                m_AosAttributes.Serialize(writer);

            if( GetSaveFlag(flags, SaveFlag.xWeaponAttributes) )
                m_AosWeaponAttributes.Serialize(writer);

            if( GetSaveFlag(flags, SaveFlag.SkillBonuses) )
                m_AosSkillBonuses.Serialize(writer);

            if( GetSaveFlag(flags, SaveFlag.Slayer2) )
                writer.Write((int)m_Slayer2);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            DamageLevel = 0x00000001,
            AccuracyLevel = 0x00000002,
            DurabilityLevel = 0x00000004,
            Quality = 0x00000008,
            Hits = 0x00000010,
            MaxHits = 0x00000020,
            Slayer = 0x00000040,
            Poison = 0x00000080,
            PoisonCharges = 0x00000100,
            Crafter = 0x00000200,
            Identified = 0x00000400,
            StrReq = 0x00000800,
            DexReq = 0x00001000,
            IntReq = 0x00002000,
            MinDamage = 0x00004000,
            MaxDamage = 0x00008000,
            HitSound = 0x00010000,
            MissSound = 0x00020000,
            Speed = 0x00040000,
            MaxRange = 0x00080000,
            Skill = 0x00100000,
            Type = 0x00200000,
            Animation = 0x00400000,
            Resource = 0x00800000,
            xAttributes = 0x01000000,
            xWeaponAttributes = 0x02000000,
            PlayerConstructed = 0x04000000,
            SkillBonuses = 0x08000000,
            Slayer2 = 0x10000000,
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch( version )
            {
                case 11:
                case 10:
                case 9:
                case 8:
                    {
                        if( version <= 9 )
                        {
                            reader.ReadMobile();
                            reader.ReadString();
                            reader.ReadMobile();
                        }

                        goto case 7;
                    }
                case 7:
                case 6:
                case 5:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadInt();

                        if( GetSaveFlag(flags, SaveFlag.DamageLevel) )
                        {
                            m_DamageLevel = (WeaponDamageLevel)reader.ReadInt();

                            if( m_DamageLevel > WeaponDamageLevel.Vanq )
                                m_DamageLevel = WeaponDamageLevel.Ruin;
                        }

                        if( GetSaveFlag(flags, SaveFlag.AccuracyLevel) )
                        {
                            m_AccuracyLevel = (WeaponAccuracyLevel)reader.ReadInt();

                            if( m_AccuracyLevel > WeaponAccuracyLevel.Supremely )
                                m_AccuracyLevel = WeaponAccuracyLevel.Accurate;
                        }

                        if( GetSaveFlag(flags, SaveFlag.DurabilityLevel) )
                        {
                            m_DurabilityLevel = (WeaponDurabilityLevel)reader.ReadInt();

                            if( m_DurabilityLevel > WeaponDurabilityLevel.Indestructible )
                                m_DurabilityLevel = WeaponDurabilityLevel.Durable;
                        }

                        if( GetSaveFlag(flags, SaveFlag.Quality) )
                            m_Quality = (WeaponQuality)reader.ReadInt();
                        else
                            m_Quality = WeaponQuality.Regular;

                        if( GetSaveFlag(flags, SaveFlag.Hits) )
                            m_Hits = reader.ReadInt();

                        if( GetSaveFlag(flags, SaveFlag.MaxHits) )
                            m_MaxHits = reader.ReadInt();

                        if( GetSaveFlag(flags, SaveFlag.Slayer) )
                            m_Slayer = (SlayerName)reader.ReadInt();

                        if( GetSaveFlag(flags, SaveFlag.Poison) )
                            m_Poison = Poison.Deserialize(reader);

                        if( GetSaveFlag(flags, SaveFlag.PoisonCharges) )
                            m_PoisonCharges = reader.ReadInt();

                        if( GetSaveFlag(flags, SaveFlag.Crafter) )
                            m_Crafter = reader.ReadMobile();

                        if( GetSaveFlag(flags, SaveFlag.Identified) )
                        {
                            if( version <= 10 )
                                m_Identified = true;
                            else
                                m_Identified = reader.ReadBool();
                        }

                        if( GetSaveFlag(flags, SaveFlag.StrReq) )
                            m_StrReq = reader.ReadInt();
                        else
                            m_StrReq = -1;

                        if( GetSaveFlag(flags, SaveFlag.DexReq) )
                            m_DexReq = reader.ReadInt();
                        else
                            m_DexReq = -1;

                        if( GetSaveFlag(flags, SaveFlag.IntReq) )
                            m_IntReq = reader.ReadInt();
                        else
                            m_IntReq = -1;

                        if( GetSaveFlag(flags, SaveFlag.MinDamage) )
                            m_MinDamage = reader.ReadInt();
                        else
                            m_MinDamage = -1;

                        if( GetSaveFlag(flags, SaveFlag.MaxDamage) )
                            m_MaxDamage = reader.ReadInt();
                        else
                            m_MaxDamage = -1;

                        if( GetSaveFlag(flags, SaveFlag.HitSound) )
                            m_HitSound = reader.ReadInt();
                        else
                            m_HitSound = -1;

                        if( GetSaveFlag(flags, SaveFlag.MissSound) )
                            m_MissSound = reader.ReadInt();
                        else
                            m_MissSound = -1;

                        if( GetSaveFlag(flags, SaveFlag.Speed) )
                        {
                            if( version < 9 )
                                m_Speed = reader.ReadInt();
                            else
                                m_Speed = reader.ReadFloat();
                        }
                        else
                            m_Speed = -1;

                        if( GetSaveFlag(flags, SaveFlag.MaxRange) )
                            m_MaxRange = reader.ReadInt();
                        else
                            m_MaxRange = -1;

                        if( GetSaveFlag(flags, SaveFlag.Skill) )
                            m_Skill = (SkillName)reader.ReadInt();
                        else
                            m_Skill = (SkillName)(-1);

                        if( GetSaveFlag(flags, SaveFlag.Type) )
                            m_Type = (WeaponType)reader.ReadInt();
                        else
                            m_Type = (WeaponType)(-1);

                        if( GetSaveFlag(flags, SaveFlag.Animation) )
                            m_Animation = (WeaponAnimation)reader.ReadInt();
                        else
                            m_Animation = (WeaponAnimation)(-1);

                        if( GetSaveFlag(flags, SaveFlag.Resource) )
                            m_Resource = (CraftResource)reader.ReadInt();
                        else
                            m_Resource = CraftResource.Iron;

                        if( GetSaveFlag(flags, SaveFlag.xAttributes) )
                            m_AosAttributes = new AosAttributes(this, reader);
                        else
                            m_AosAttributes = new AosAttributes(this);

                        if( GetSaveFlag(flags, SaveFlag.xWeaponAttributes) )
                            m_AosWeaponAttributes = new AosWeaponAttributes(this, reader);
                        else
                            m_AosWeaponAttributes = new AosWeaponAttributes(this);

                        if( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular && Parent is Mobile )
                        {
                            m_SkillMod = new DefaultSkillMod(AccuracySkill, true, (int)m_AccuracyLevel * 5);
                            ((Mobile)Parent).AddSkillMod(m_SkillMod);
                        }

                        if( version < 7 && m_AosWeaponAttributes.MageWeapon != 0 )
                            m_AosWeaponAttributes.MageWeapon = 30 - m_AosWeaponAttributes.MageWeapon;

                        if( Core.SE && m_AosWeaponAttributes.MageWeapon != 0 && m_AosWeaponAttributes.MageWeapon != 30 && Parent is Mobile )
                        {
                            m_MageMod = new DefaultSkillMod(SkillName.Magery, true, -30 + m_AosWeaponAttributes.MageWeapon);
                            ((Mobile)Parent).AddSkillMod(m_MageMod);
                        }

                        if( GetSaveFlag(flags, SaveFlag.PlayerConstructed) )
                            m_PlayerConstructed = true;

                        if( GetSaveFlag(flags, SaveFlag.SkillBonuses) )
                            m_AosSkillBonuses = new AosSkillBonuses(this, reader);
                        else
                            m_AosSkillBonuses = new AosSkillBonuses(this);

                        if( GetSaveFlag(flags, SaveFlag.Slayer2) )
                            m_Slayer2 = (SlayerName)reader.ReadInt();

                        break;
                    }
                case 4:
                    {
                        m_Slayer = (SlayerName)reader.ReadInt();

                        goto case 3;
                    }
                case 3:
                    {
                        m_StrReq = reader.ReadInt();
                        m_DexReq = reader.ReadInt();
                        m_IntReq = reader.ReadInt();

                        goto case 2;
                    }
                case 2:
                    {
                        m_Identified = reader.ReadBool();

                        goto case 1;
                    }
                case 1:
                    {
                        m_MaxRange = reader.ReadInt();

                        goto case 0;
                    }
                case 0:
                    {
                        if( version == 0 )
                            m_MaxRange = 1; // default

                        if( version < 5 )
                        {
                            m_Resource = CraftResource.Iron;
                            m_AosAttributes = new AosAttributes(this);
                            m_AosWeaponAttributes = new AosWeaponAttributes(this);
                            m_AosSkillBonuses = new AosSkillBonuses(this);
                        }

                        m_MinDamage = reader.ReadInt();
                        m_MaxDamage = reader.ReadInt();

                        m_Speed = reader.ReadInt();

                        m_HitSound = reader.ReadInt();
                        m_MissSound = reader.ReadInt();

                        m_Skill = (SkillName)reader.ReadInt();
                        m_Type = (WeaponType)reader.ReadInt();
                        m_Animation = (WeaponAnimation)reader.ReadInt();
                        m_DamageLevel = (WeaponDamageLevel)reader.ReadInt();
                        m_AccuracyLevel = (WeaponAccuracyLevel)reader.ReadInt();
                        m_DurabilityLevel = (WeaponDurabilityLevel)reader.ReadInt();
                        m_Quality = (WeaponQuality)reader.ReadInt();

                        m_Crafter = reader.ReadMobile();

                        m_Poison = Poison.Deserialize(reader);
                        m_PoisonCharges = reader.ReadInt();

                        if( m_StrReq == OldStrengthReq )
                            m_StrReq = -1;

                        if( m_DexReq == OldDexterityReq )
                            m_DexReq = -1;

                        if( m_IntReq == OldIntelligenceReq )
                            m_IntReq = -1;

                        if( m_MinDamage == OldMinDamage )
                            m_MinDamage = -1;

                        if( m_MaxDamage == OldMaxDamage )
                            m_MaxDamage = -1;

                        if( m_HitSound == OldHitSound )
                            m_HitSound = -1;

                        if( m_MissSound == OldMissSound )
                            m_MissSound = -1;

                        if( m_Speed == OldSpeed )
                            m_Speed = -1;

                        if( m_MaxRange == OldMaxRange )
                            m_MaxRange = -1;

                        if( m_Skill == OldSkill )
                            m_Skill = (SkillName)(-1);

                        if( m_Type == OldType )
                            m_Type = (WeaponType)(-1);

                        if( m_Animation == OldAnimation )
                            m_Animation = (WeaponAnimation)(-1);

                        if( UseSkillMod && m_AccuracyLevel != WeaponAccuracyLevel.Regular && Parent is Mobile )
                        {
                            m_SkillMod = new DefaultSkillMod(AccuracySkill, true, (int)m_AccuracyLevel * 5);
                            ((Mobile)Parent).AddSkillMod(m_SkillMod);
                        }

                        break;
                    }
            }

            if( Core.AOS && Parent is Mobile )
                m_AosSkillBonuses.AddTo((Mobile)Parent);

            int strBonus = m_AosAttributes.BonusStr;
            int dexBonus = m_AosAttributes.BonusDex;
            int intBonus = m_AosAttributes.BonusInt;

            if( this.Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0) )
            {
                Mobile m = (Mobile)this.Parent;

                string modName = this.Serial.ToString();

                if( strBonus != 0 )
                    m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if( dexBonus != 0 )
                    m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if( intBonus != 0 )
                    m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            if( Parent is Mobile )
                ((Mobile)Parent).CheckStatTimers();

            if( m_Hits <= 0 && m_MaxHits <= 0 )
            {
                m_Hits = m_MaxHits = Utility.RandomMinMax(InitMinHits, InitMaxHits);
            }

            if( version < 6 )
                m_PlayerConstructed = true; // we don't know, so, assume it's crafted
        }
        #endregion

        public BaseWeapon( int itemID )
            : base(itemID)
        {
            Layer = (Layer)ItemData.Quality;

            m_Quality = WeaponQuality.Regular;
            m_StrReq = -1;
            m_DexReq = -1;
            m_IntReq = -1;
            m_MinDamage = -1;
            m_MaxDamage = -1;
            m_HitSound = -1;
            m_MissSound = -1;
            m_Speed = -1;
            m_MaxRange = -1;
            m_Skill = (SkillName)(-1);
            m_Type = (WeaponType)(-1);
            m_Animation = (WeaponAnimation)(-1);

            m_Hits = m_MaxHits = Utility.RandomMinMax(InitMinHits, InitMaxHits);

            m_Resource = CraftResource.Iron;

            m_AosAttributes = new AosAttributes(this);
            m_AosWeaponAttributes = new AosWeaponAttributes(this);
            m_AosSkillBonuses = new AosSkillBonuses(this);
        }

        public BaseWeapon( Serial serial )
            : base(serial)
        {
        }

        private string GetNameString()
        {
            string name = this.Name;

            if( name == null )
                name = String.Format("#{0}", LabelNumber);

            return name;
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get { return base.Hue; }
            set { base.Hue = value; InvalidateProperties(); }
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            int oreType;

            switch( m_Resource )
            {
                case CraftResource.DullCopper: oreType = 1053108; break; // dull copper
                case CraftResource.ShadowIron: oreType = 1053107; break; // shadow iron
                case CraftResource.Copper: oreType = 1053106; break; // copper
                case CraftResource.Bronze: oreType = 1053105; break; // bronze
                case CraftResource.Gold: oreType = 1053104; break; // golden
                case CraftResource.Agapite: oreType = 1053103; break; // agapite
                case CraftResource.Verite: oreType = 1053102; break; // verite
                case CraftResource.Valorite: oreType = 1053101; break; // valorite
                case CraftResource.SpinedLeather: oreType = 1061118; break; // spined
                case CraftResource.HornedLeather: oreType = 1061117; break; // horned
                case CraftResource.BarbedLeather: oreType = 1061116; break; // barbed
                case CraftResource.RedScales: oreType = 1060814; break; // red
                case CraftResource.YellowScales: oreType = 1060818; break; // yellow
                case CraftResource.BlackScales: oreType = 1060820; break; // black
                case CraftResource.GreenScales: oreType = 1060819; break; // green
                case CraftResource.WhiteScales: oreType = 1060821; break; // white
                case CraftResource.BlueScales: oreType = 1060815; break; // blue
                default: oreType = 0; break;
            }

            if( oreType != 0 )
                list.Add(1053099, "#{0}\t{1}", oreType, GetNameString()); // ~1_oretype~ ~2_armortype~
            else if( Name == null )
                list.Add(LabelNumber);
            else
                list.Add(Name);
        }

        public override bool AllowEquipedCast( Mobile from )
        {
            if( base.AllowEquipedCast(from) )
                return true;

            if (from is Player)
            {
                Warlock wlk = Perk.GetByType<Warlock>((Player)from);

                if (wlk != null && wlk.Conduit())
                    return true;
            }

            return (Core.AOS && m_AosAttributes.SpellChanneling != 0);
        }

        public virtual int ArtifactRarity
        {
            get { return 0; }
        }

        /*public override void GetProperties( ObjectPropertyList list )
        {
            base.GetProperties(list);

            if( !Core.AOS)
            {
                if( m_Crafter != null )
                    list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~

                if( m_AosSkillBonuses != null )
                    m_AosSkillBonuses.GetProperties(list);

                if( m_Quality == WeaponQuality.Exceptional )
                    list.Add(1060636); // exceptional

                if( ArtifactRarity > 0 )
                    list.Add(1061078, ArtifactRarity.ToString()); // artifact rarity ~1_val~

                if( this is IUsesRemaining && ((IUsesRemaining)this).ShowUsesRemaining )
                    list.Add(1060584, ((IUsesRemaining)this).UsesRemaining.ToString()); // uses remaining: ~1_val~

                if( m_Poison != null && m_PoisonCharges > 0 )
                    list.Add(1062412 + m_Poison.Level, m_PoisonCharges.ToString());

                if( m_Slayer != SlayerName.None )
                {
                    SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer);
                    if( entry != null )
                        list.Add(entry.Title);
                }

                if( m_Slayer2 != SlayerName.None )
                {
                    SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer2);
                    if( entry != null )
                        list.Add(entry.Title);
                }
                base.AddResistanceProperties(list);

                int prop;

                if( (prop = m_AosWeaponAttributes.UseBestSkill) != 0 )
                    list.Add(1060400); // use best weapon skill

                if( (prop = (GetDamageBonus() + m_AosAttributes.WeaponDamage)) != 0 )
                    list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

                if( (prop = m_AosAttributes.DefendChance) != 0 )
                    list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

                if( (prop = m_AosAttributes.EnhancePotions) != 0 )
                    list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

                if( (prop = (GetHitChanceBonus() + m_AosAttributes.AttackChance)) != 0 )
                    list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitColdArea) != 0 )
                    list.Add(1060416, prop.ToString()); // hit cold area ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitDispel) != 0 )
                    list.Add(1060417, prop.ToString()); // hit dispel ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitEnergyArea) != 0 )
                    list.Add(1060418, prop.ToString()); // hit energy area ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitFireArea) != 0 )
                    list.Add(1060419, prop.ToString()); // hit fire area ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitFireball) != 0 )
                    list.Add(1060420, prop.ToString()); // hit fireball ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitHarm) != 0 )
                    list.Add(1060421, prop.ToString()); // hit harm ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLeechHits) != 0 )
                    list.Add(1060422, prop.ToString()); // hit life leech ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLightning) != 0 )
                    list.Add(1060423, prop.ToString()); // hit lightning ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLowerAttack) != 0 )
                    list.Add(1060424, prop.ToString()); // hit lower attack ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLowerDefend) != 0 )
                    list.Add(1060425, prop.ToString()); // hit lower defense ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitMagicArrow) != 0 )
                    list.Add(1060426, prop.ToString()); // hit magic arrow ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLeechMana) != 0 )
                    list.Add(1060427, prop.ToString()); // hit mana leech ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitPhysicalArea) != 0 )
                    list.Add(1060428, prop.ToString()); // hit physical area ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitPoisonArea) != 0 )
                    list.Add(1060429, prop.ToString()); // hit poison area ~1_val~%

                if( (prop = m_AosWeaponAttributes.HitLeechStam) != 0 )
                    list.Add(1060430, prop.ToString()); // hit stamina leech ~1_val~%

                if( (prop = m_AosAttributes.BonusDex) != 0 )
                    list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

                if( (prop = m_AosAttributes.BonusHits) != 0 )
                    list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

                if( (prop = m_AosAttributes.BonusInt) != 0 )
                    list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

                if( (prop = m_AosAttributes.LowerManaCost) != 0 )
                    list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

                if( (prop = m_AosAttributes.LowerRegCost) != 0 )
                    list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

                if( (prop = m_AosWeaponAttributes.MageWeapon) != 0 )
                    list.Add(1060438, (30 - prop).ToString()); // mage weapon -~1_val~ skill

                if( (prop = m_AosAttributes.BonusMana) != 0 )
                    list.Add(1060439, prop.ToString()); // mana increase ~1_val~

                if( (prop = m_AosAttributes.NightSight) != 0 )
                    list.Add(1060441); // night sight

                if( (prop = m_AosAttributes.ReflectPhysical) != 0 )
                    list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

                if( (prop = m_AosWeaponAttributes.SelfRepair) != 0 )
                    list.Add(1060450, prop.ToString()); // self repair ~1_val~

                if( (prop = m_AosAttributes.SpellChanneling) != 0 )
                    list.Add(1060482); // spell channeling

                if( (prop = m_AosAttributes.SpellDamage) != 0 )
                    list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

                if( (prop = m_AosAttributes.BonusStam) != 0 )
                    list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

                if( (prop = m_AosAttributes.BonusStr) != 0 )
                    list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

                if( (prop = m_AosAttributes.WeaponSpeed) != 0 )
                    list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

                if( MinDamage > 0 && MaxDamage > 0 )
                    list.Add(1061168, "{0}\t{1}", MinDamage.ToString(), MaxDamage.ToString()); // weapon damage ~1_val~ - ~2_val~

                if( Speed > -1 )
                    list.Add(1061167, Speed.ToString()); // weapon speed ~1_val~

                if( MaxRange > 1 )
                    list.Add(1061169, MaxRange.ToString()); // range ~1_val~

                if( StrRequirement > 0 )
                    list.Add(1061170, StrRequirement.ToString()); // strength requirement ~1_val~

                if( Layer == Layer.TwoHanded )
                    list.Add(1061171); // two-handed weapon
                else
                    list.Add(1061824); // one-handed weapon

                if( Core.SE || m_AosWeaponAttributes.UseBestSkill == 0 )
                {
                    switch( Skill )
                    {
                        case SkillName.Swords: list.Add(1061172); break; // skill required: swordsmanship
                        case SkillName.Macing: list.Add(1061173); break; // skill required: mace fighting
                        case SkillName.Fencing: list.Add(1061174); break; // skill required: fencing
                        case SkillName.Archery: list.Add(1061175); break; // skill required: archery
                    }
                }

                if( m_Hits >= 0 && m_MaxHits > 0 )
                    list.Add(1060639, "{0}\t{1}", m_Hits, m_MaxHits); // durability ~1_val~ / ~2_val~
            }
            else
            {
                if( DisplayLootType )
                {
                    if( LootType == LootType.Blessed )
                        list.Add(1038021);
                    else if( LootType == LootType.Cursed )
                        list.Add(1049643);
                }

                //if( m_Quality == WeaponQuality.Exceptional )
                    //list.Add((1018305 - (int)m_Quality));

                if( m_Crafter != null )
                    list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~

                if( m_Identified )
                {

                    if (m_Crafter != null)
                        list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~

                    if( m_DurabilityLevel != WeaponDurabilityLevel.Regular )
                        list.Add(1038000 + (int)m_DurabilityLevel);

                    if( m_AccuracyLevel != WeaponAccuracyLevel.Regular )
                        list.Add(1038010 + (int)m_AccuracyLevel);

                    if( m_DamageLevel != WeaponDamageLevel.Regular )
                        list.Add(1038015 + (int)m_DamageLevel);

                    if( m_Slayer != SlayerName.None )
                    {
                        SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer);
                        if( entry != null )
                            list.Add(entry.Title);
                    }

                    if( m_Slayer2 != SlayerName.None )
                    {
                        SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer2);
                        if( entry != null )
                            list.Add(entry.Title);
                    }
                }
                else if( m_Slayer != SlayerName.None || m_Slayer2 != SlayerName.None || m_DurabilityLevel != WeaponDurabilityLevel.Regular || m_DamageLevel != WeaponDamageLevel.Regular || m_AccuracyLevel != WeaponAccuracyLevel.Regular )
                {
                    list.Add(1038000); //Unidentified
                }

                if( m_Poison != null && m_PoisonCharges > 0 )
                    list.Add(1017383, m_PoisonCharges.ToString());

                //if( m_Hits >= 0 && m_MaxHits > 0 )
                //    list.Add( 1060639, "{0}\t{1}", m_Hits, m_MaxHits ); // durability ~1_val~ / ~2_val~
            }
        }*/

        public override void OnSingleClick( Mobile from )
        {
            List<EquipInfoAttribute> attrs = new List<EquipInfoAttribute>();

            if( DisplayLootType )
            {
                if( LootType == LootType.Blessed )
                    attrs.Add(new EquipInfoAttribute(1038021)); // blessed
                else if( LootType == LootType.Cursed )
                    attrs.Add(new EquipInfoAttribute(1049643)); // cursed
            }

            if( m_Quality == WeaponQuality.Exceptional )
                attrs.Add(new EquipInfoAttribute(1018305 - (int)m_Quality));

            if( m_Identified || from.AccessLevel >= AccessLevel.GameMaster )
            {
                if( m_Slayer != SlayerName.None )
                {
                    SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer);
                    if( entry != null )
                        attrs.Add(new EquipInfoAttribute(entry.Title));
                }

                if( m_Slayer2 != SlayerName.None )
                {
                    SlayerEntry entry = SlayerGroup.GetEntryByName(m_Slayer2);
                    if( entry != null )
                        attrs.Add(new EquipInfoAttribute(entry.Title));
                }

                if( m_DurabilityLevel != WeaponDurabilityLevel.Regular )
                    attrs.Add(new EquipInfoAttribute(1038000 + (int)m_DurabilityLevel));

                if( m_DamageLevel != WeaponDamageLevel.Regular )
                    attrs.Add(new EquipInfoAttribute(1038015 + (int)m_DamageLevel));

                if( m_AccuracyLevel != WeaponAccuracyLevel.Regular )
                    attrs.Add(new EquipInfoAttribute(1038010 + (int)m_AccuracyLevel));
            }
            else if( m_Slayer != SlayerName.None || m_Slayer2 != SlayerName.None || m_DurabilityLevel != WeaponDurabilityLevel.Regular || m_DamageLevel != WeaponDamageLevel.Regular || m_AccuracyLevel != WeaponAccuracyLevel.Regular )
                attrs.Add(new EquipInfoAttribute(1038000)); // Unidentified

            if( m_Poison != null && m_PoisonCharges > 0 )
                attrs.Add(new EquipInfoAttribute(1017383, m_PoisonCharges));

            int number;

            if( Name == null )
            {
                number = LabelNumber;
            }
            else
            {
                this.LabelTo(from, Name);
                number = 1041000;
            }

            if( attrs.Count == 0 && Crafter == null && Name != null )
                return;

            EquipmentInfo eqInfo = new EquipmentInfo(number, m_Crafter, false, attrs.ToArray());

            from.Send(new DisplayEquipmentInfo(this, eqInfo));
        }

        private static BaseWeapon m_Fists; // This value holds the default--fist--weapon

        public static BaseWeapon Fists
        {
            get { return m_Fists; }
            set { m_Fists = value; }
        }

        #region ICraftable Members
        public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
        {
            if( !Identified )
                Identified = true;

            Quality = (WeaponQuality)quality;

            if( makersMark )
                Crafter = from;

            PlayerConstructed = true;

            Type resourceType = typeRes;

            if( resourceType == null )
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Resource = CraftResources.GetFromType(resourceType);
            CraftContext context = craftSystem.GetContext(from);

            if( context != null && context.DoNotColor )
                Hue = 0;

            if( Core.AOS )
            {
                if( tool is BaseRunicTool )
                    ((BaseRunicTool)tool).ApplyAttributesTo(this);

                if( Quality == WeaponQuality.Exceptional )
                {
                    if( Attributes.WeaponDamage > 35 )
                        Attributes.WeaponDamage -= 20;
                    else
                        Attributes.WeaponDamage = 15;
                }
            }
            else if( tool is BaseRunicTool )
            {
                CraftResource thisResource = CraftResources.GetFromType(resourceType);

                if( thisResource == ((BaseRunicTool)tool).Resource )
                {
                    switch( thisResource )
                    {
                        case CraftResource.DullCopper:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Durable;
                                AccuracyLevel = WeaponAccuracyLevel.Accurate;
                                break;
                            }
                        case CraftResource.ShadowIron:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Durable;
                                DamageLevel = WeaponDamageLevel.Ruin;
                                break;
                            }
                        case CraftResource.Copper:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Fortified;
                                DamageLevel = WeaponDamageLevel.Ruin;
                                AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                                break;
                            }
                        case CraftResource.Bronze:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Fortified;
                                DamageLevel = WeaponDamageLevel.Might;
                                AccuracyLevel = WeaponAccuracyLevel.Surpassingly;
                                break;
                            }
                        case CraftResource.Gold:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                                DamageLevel = WeaponDamageLevel.Force;
                                AccuracyLevel = WeaponAccuracyLevel.Eminently;
                                break;
                            }
                        case CraftResource.Agapite:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                                DamageLevel = WeaponDamageLevel.Power;
                                AccuracyLevel = WeaponAccuracyLevel.Eminently;
                                break;
                            }
                        case CraftResource.Verite:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                                DamageLevel = WeaponDamageLevel.Power;
                                AccuracyLevel = WeaponAccuracyLevel.Exceedingly;
                                break;
                            }
                        case CraftResource.Valorite:
                            {
                                DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                                DamageLevel = WeaponDamageLevel.Vanq;
                                AccuracyLevel = WeaponAccuracyLevel.Supremely;
                                break;
                            }
                    }
                }
            }

            return quality;
        }

        #endregion

        public int A { get; set; }
    }

    public enum CheckSlayerResult
    {
        None,
        Slayer,
        Opposition
    }
}