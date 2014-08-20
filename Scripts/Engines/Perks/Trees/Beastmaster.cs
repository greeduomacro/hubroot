using Server.Mobiles;
using Server;

namespace Server.Perks
{
    public class Beastmaster : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Beastmaster( Player player )
            : base(player)
        {
        }

        public bool GuidingHand()
        {
            return (Level >= PerkLevel.Fourth);
        }

        public bool IsItimidating()
        {
            return (Level > PerkLevel.First);          
        }

        public int PackRegenBonus()
        {
            if (Level > PerkLevel.Second)
                return 8;
            else
                return 0;
        }

        public bool FollowerBonus()
        {
            if (Level == PerkLevel.Fifth)
                return true;
            else
                return false;
        }

        public int MasterRegenBonus()
        {

            int packsize = 0;
            int bonus = 0;


            if (Level > PerkLevel.Third)
            {
                    foreach (Mobile m in Player.GetMobilesInRange(10))
                    {
                        if (m != Player)
                        {
                            BaseCreature bc = m as BaseCreature;
                            if (bc.ControlMaster == Player)
                            {
                                packsize++;
                            }

                        }

                    }

                    bonus = (packsize * 1);

                    return bonus;

                }

            return bonus;
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
        public Beastmaster( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "A master of imposing will and intepreting body language."; } }
        public override int GumpID { get { return 2295; } }
        public override string Label { get { return "Beastmaster"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Intimidation", "The Beastmaster is capable of starring down even the fiercest of creatures."),
                    new LabelEntry(PerkLevel.Second, "Pack Leader", "You inspire a level in confidence in your pack that invigorates, unlocking hidden potential within each member."),
                    new LabelEntry(PerkLevel.Third, "Of The Pack", "Your sense of duty to your pack makes you a fierce opponent on the battlefield."),
                    new LabelEntry(PerkLevel.Fourth, "Guiding Hand", "Creatures under the Beastmaster's command and tutelage learn skills at a higher rate."),
                    new LabelEntry(PerkLevel.Fifth, "The Heard", "Through sheer will-power, the Beastmaster is capable of commanding more creatures than most on the battlefield.")
                });
            }
        }
    }
}