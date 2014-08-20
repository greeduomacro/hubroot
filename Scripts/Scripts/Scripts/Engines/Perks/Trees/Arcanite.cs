using Server.Mobiles;

namespace Server.Perks
{
    public class Arcanite : Perk
    {
        /// <summary>
        /// Indicates if a spell animation should be played
        /// </summary>
        public bool EtherealKenetics()
        {
            return Level >= PerkLevel.Fourth;
        }

        public int RegenBonus()
        {
            if (Level >= PerkLevel.Fourth)
            {
                return 8;
            }

            return 0;
        }

        /// <summary>
        /// Indicates if a spell mantra should be spoken
        /// </summary>
        public bool SayMantra
        {
            get { return Level < PerkLevel.Fifth; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Arcanite( Player player )
            : base(player)
        {
        }

        /// <summary>
        /// Gets the Cast Delay modifier (in seconds) that is applied at <code>PerkLevel.Third</code>
        /// </summary>
        public double GetFCBonus()
        {
            if( Level < PerkLevel.Third )
                return 0;

            return 2;
        }

        /// <summary>
        /// Gets the Faster Cast Recovery (in seconds) value that is applied at <code>PerkLevel.Second</code>
        /// </summary>
        public double GetFCRBonus()
        {
            if( Level < PerkLevel.Second )
                return 0;

            return 3;
        }

        /// <summary>
        /// Applies a Lower Mana scalar at <code>PerkLevel.First</code>
        /// </summary>
        public int GetLMCBonus()
        {
            if( Level < PerkLevel.First )
                return 0;

            return 25;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void Serialize( Server.GenericWriter writer )
        {
            base.Serialize(writer);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public Arcanite( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "Capable of manipulating ether with thought alone."; } }
        public override int GumpID { get { return 2270; } }
        public override string Label { get { return "Arcanite"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Ethereal Command", "The Arcanite is capable of modifying ether without expending much of its own energy."),
                    new LabelEntry(PerkLevel.Second, "Arcane Reflex", "With such an instinctual understanding ether, your body recovers faster from ethereal interactions."),
                    new LabelEntry(PerkLevel.Third, "Neural Circuitry", "After channeling massive amounts energy through your body, you are capable of affecting ether at great speeds."),
                    new LabelEntry(PerkLevel.Fourth, "Ethereal Kinetics", "The Arcanite is capable of comitting spells to reality without the use of hand gestures."),
                    new LabelEntry(PerkLevel.Fifth, "Arcane Telepathy", "At the height of their telepathic connection to the ether, the Arcanite need not speak words of power in order to transmute spells.")
                });
            }
        }
    }
}