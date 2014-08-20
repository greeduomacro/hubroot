using Server.Mobiles;
using Server;

namespace Server.Perks
{
    public class Summoner : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Summoner( Player player )
            : base(player)
        {
        }

        public bool IntelligentDesign()
        {
            return (Level >= PerkLevel.First);
        }

        public bool SecondNature()
        {
            return (Level >= PerkLevel.Second);
        }

        public bool Puppeteer()
        {
            return (Level >= PerkLevel.Third);
        }

        public bool Remanence()
        {
            return (Level >= PerkLevel.Fourth);
        }

        public bool Horde()
        {
            return (Level >= PerkLevel.Fifth);
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
        public Summoner( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "Master of commiting thought to reality."; } }
        public override int GumpID { get { return 2279; } }
        public override string Label { get { return "Summoner"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Intelligent Design", "Your knowledge of how ether constructs living creatures allows you to to summon more skilled minions."),
                    new LabelEntry(PerkLevel.Second, "Second Nature", "The summoner's instictual grasp of transmuting ether to matter allows your summons to take more damage."),
                    new LabelEntry(PerkLevel.Third, "Puppeteer", "The summoner is capable of manipulating large groups of summonings at a time, with little effort."),
                    new LabelEntry(PerkLevel.Fourth, "Remanence", "Your experience with summoning allows you to sustain the energy of summoned creatures for longer periods of time."),
                    new LabelEntry(PerkLevel.Fifth, "The Horde", "While most struggle to create even a single entity, the summoner is capable of constructing multiple creatures at once.")
                });
            }
        }
    }
}