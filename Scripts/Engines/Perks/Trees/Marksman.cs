using Server.Mobiles;
using Server;
using Server.Items;
using System;

namespace Server.Perks
{
    public class Marksman : Perk
    {

        public void WoundingShot(Mobile attacker, Mobile defender)
        {
            if (Level >= PerkLevel.Fifth)
            {
                int distance = 0;

                for (int x = 0; x <= 12; x++)
                {
                    if (attacker.InRange(defender, x))
                    {
                        distance = x;
                    }
                }

                int woundChance = (int)((attacker.Skills.Anatomy.Value / 8) - (distance / 2));

                if (woundChance >= Utility.RandomMinMax(0, 100))
                {
                    BleedAttack.BeginBleed(defender, attacker);

                    attacker.SendMessage("You wound your opponent, causing them to bleed!");
                    defender.SendMessage("You've been wounded!");
                }
            }
        }

        public int QuickDraw(BaseWeapon weapon)
        {
            if (Level >= PerkLevel.Fourth)
            {
                if (weapon is BaseRanged)
                {                 
                    return 25;
                }
            }             

            return 0;
        }

        public int DeadAim(Mobile attacker, Mobile defender)
        {
            if (Level >= PerkLevel.Third)
            {
                int range;

                for (range = 1; range <= 10; range++)
                {
                    if(attacker.InRange(defender, range))
                    {
                        return ((80 / range) / 100);
                    }
                }             
            }

            return 0;
        }

        public bool RunAndGun()
        {
            return (Level >= PerkLevel.First);
        }

        public bool BowConverted;

        public bool LongShot()
        {
            return (Level >= PerkLevel.Second);
        }
        /// <summary>
        /// ctor
        /// </summary>
        public Marksman( Player player )
            : base(player)
        {
        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void Serialize( Server.GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)1); //version
            writer.Write(BowConverted);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public Marksman( GenericReader reader )
            : base(reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    break;

                case 1:
                    BowConverted = reader.ReadBool();
                    goto case 0;
            }
        }

        public override string Description { get { return "Master of ranged weaponry and bow tactics."; } }
        public override int GumpID { get { return 2244; } }
        public override string Label { get { return "Marksman"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "The Hunter", "Having not only chased, but run from many foe, the Marksman is capable of firing on the run."),
                    new LabelEntry(PerkLevel.Second, "Eagle-Eye", "Years of practice gives the marksman the ability to hit targets are great distances."),
                    new LabelEntry(PerkLevel.Third, "Precision", "As such a skilled Marksman, hitting targets at close range is little challenge."),
                    new LabelEntry(PerkLevel.Fourth, "Quick Draw", "Having fired countless arrows, the Marksman is capable of bringing arrow to bow in record time."),
                    new LabelEntry(PerkLevel.Fifth, "Wounding Shot", "The Marksman is capable of placing arrows in vital areas, wounding the victim.")
                });
            }
        }
    }
}