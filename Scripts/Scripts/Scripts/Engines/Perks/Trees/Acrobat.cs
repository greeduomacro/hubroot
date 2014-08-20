using Server.Mobiles;

namespace Server.Perks
{
    public class Acrobat : Perk
    {
        public bool Olympian()
        {
            if (Level == PerkLevel.Fifth)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Acrobat( Player player )
            : base(player)
        {
        }

        /// <summary>
        /// Decreases falling damage by 90% at <code>PerkLevel.First</code>
        /// </summary>
        public int GetFallingDamageBonus( int damage )
        {
            if( Level < PerkLevel.First )
                return 0;

            return (int)(damage * 0.90);
        }

        /// <summary>
        /// Applies a bonus to hit chance at <code>PerkLevel.Fourth</code>
        /// </summary>
        public double GetHitChanceBonus()
        {
            if( Level < PerkLevel.Fourth )
                return 0;

            return 0.20;
        }

        /// <summary>
        /// Gets the bonus jump range that is applied at <code>PerkLevel.First</code>
        /// </summary>
        /// <returns></returns>
        public int GetJumpRangeBonus()
        {
            if( Level < PerkLevel.First )
                return 0;

            return 2;
        }

        /// <summary>
        /// Applies a bonus to stam regeneration at <code>PerkLevel.Fifth</code>
        /// </summary>
        public int GetStamRegenBonus()
        {
            if( Level < PerkLevel.Third )
                return 0;

            return 8;
        }

        /// <summary>
        /// Applies a 25% chance to dodge weapon attacks at <code>PerkLevel.Second</code>
        /// </summary>
        public bool TryDodge()
        {
            if (Level >= PerkLevel.Second)
            {
                if (Utility.RandomDouble() <= 0.25)
                {
                    Player.SendMessage("You dodge your opponents attack!");
                    Player.Emote("*Dodges*");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void Serialize( Server.GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)1); //version
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public Acrobat( GenericReader reader )
            : base(reader)
        {
            int version = reader.ReadInt();
        }

        public override string Description { get { return "Well-versed in the art of defying physics."; } }
        public override int GumpID { get { return 2248; } }
        public override string Label { get { return "Acrobat"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Leaps and Bounds", "With well-developed leg muscles from continuous exercise, in combination with your natural athleticism, you are able to jump greater than average distances and fall more gracefully."),
                    new LabelEntry(PerkLevel.Second, "Evasion Mastery", "You've long practiced the art of being limber and more nimble, and you are able to contort your body in ways that seem to defy physics."),
                    new LabelEntry(PerkLevel.Third, "Endurance", "Years of training allows the Acrobat to recover more quickly from long periods of stress and other feats of endurance."),
                    new LabelEntry(PerkLevel.Fourth, "Sure Footing", "Having developed such sure footing, you are now able to strike from unexpected positions and angles some might not expect."),
                    new LabelEntry(PerkLevel.Fifth, "Olympian", "The acrobats natural abilties allow him or her to surpass average physical limitations.")
                });
            }
        }
    }
}