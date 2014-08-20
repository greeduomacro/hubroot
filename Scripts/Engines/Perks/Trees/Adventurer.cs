using System;
using Server.Commands;
using Server.EssenceOfCharacter;
using Server.Events;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Multis;
using Server.Network;
using Server.Mobiles;

namespace Server.Perks
{
    public class Adventurer : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Adventurer( Player player )
            : base(player)
        {
        }

        /// <summary>
        /// Lowers an applied poison level by one degree at <code>PerkLevel.Second</code>
        /// </summary>
        public Poison GetLowerPoison( Poison p )
        {
            if( Level < PerkLevel.Second )
                return p;

            return Poison.GetPoison(p.Level - 1);
        }

        /// <summary>
        /// Lowers damage received by 15% at <code>PerkLevel.Third</code>
        /// </summary>
        public int GetDamageReduction()
        {
            if (Level < PerkLevel.Third)
                return 0;

            return 15;
        }


        /// <summary>
        /// Applies a bonus to stam regeneration at <code>PerkLevel.Fourth</code>
        /// </summary>
        public int GetStamRegenBonus()
        {
            if( Level < PerkLevel.Fourth )
                return 0;

            return 8;
        }

        /// <summary>
        /// Slows hunger and thirst decay at <code>PerkLevel.Fourth</code>
        /// </summary>
        public int HungerThirstBonus()
        {
            if (Level < PerkLevel.First)
                return 0;

            return 1;
        }

        /// <summary>
        /// Allows the adventurer to use a torch as a second weapon at <code>PerkLevel.Fifth</code>
        /// </summary>
        public void TorchAttack( Mobile attacker, Mobile defender )
        {
            if (Level == PerkLevel.Fifth)
            {
                Item torch = attacker.FindItemOnLayer(Layer.TwoHanded);
                if (torch != null && torch is Items.Torch)
                {
                    int hitChance = (attacker.Dex / 6);
                    if (hitChance > Utility.RandomMinMax(0, 100))
                    {
                            int torchDamage = Utility.RandomMinMax(1, 5) + (attacker.Str / 10);

                            attacker.DoHarmful(defender);
                            defender.Damage(torchDamage);
                            attacker.SendMessage("You follow through with your torch, using it to strike your opponent.");
                            defender.SendMessage("{0} strikes you with {1} torch.", attacker, attacker.Female ? "her" : "his");
                            attacker.PlaySound(0x142);
                    }                   
                }
            }
        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public Adventurer( GenericReader reader )
            : base(reader)
        {

        }

        public override string Description { get { return "Well-traveled, well-worn veteran."; } }
        public override int GumpID { get { return 2245; } }
        public override string Label { get { return "Adventurer"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Metabolic Efficiency", "Having gone long durations of time without food or water has tought your body to use sustenance more efficiently."),
                    new LabelEntry(PerkLevel.Second, "Resilience", "Being bit, stung, cut, and clawed in various adventures, you have become much more capable of fending off environmental toxins."),
                    new LabelEntry(PerkLevel.Third, "Thick Skin", "Continuous wear and tear, cuts, and bruises gives you a higher normal pain tolerance."),
                    new LabelEntry(PerkLevel.Fourth, "Mental Fortitude", "Many days and nights on the road has lead your body to recover more quickly."),
                    new LabelEntry(PerkLevel.Fifth, "Torch Mastery", "With as much time spent in dark places as you have, the need for light and the ability to defend yourself often go hand-in-hand, allowing you to use a torch as a bludgeon.")
                });
            }
        }
    }
}