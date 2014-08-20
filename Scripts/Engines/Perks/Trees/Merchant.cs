using Server.Mobiles;
using Server;

namespace Server.Perks
{
    public class Merchant : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Merchant( Player player )
            : base(player)
        {
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
        public Merchant( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return ""; } }
        public override int GumpID { get { return 2253; } }
        public override string Label { get { return "Merchant"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "", ""),
                    new LabelEntry(PerkLevel.Second, "", ""),
                    new LabelEntry(PerkLevel.Third, "", ""),
                    new LabelEntry(PerkLevel.Fourth, "", ""),
                    new LabelEntry(PerkLevel.Fifth, "", "")
                });
            }
        }
    }
}