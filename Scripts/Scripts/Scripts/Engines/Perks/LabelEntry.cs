using System.Collections.Generic;

namespace Server.Perks
{
    public class LabelEntry
    {
        public string Description { get; set; }
        public string Label { get; set; }
        public PerkLevel Level { get; set; }

        public LabelEntry( PerkLevel level, string label, string description )
        {
            Level = level;
            Label = label;
            Description = description;
        }
    }

    public class LabelEntryList : List<LabelEntry>
    {
        public LabelEntryList()
        {
        }

        public LabelEntryList( LabelEntry[] initialList )
            : base(initialList)
        {
        }

        public LabelEntry this[PerkLevel level]
        {
            get
            {
                return Find(entry => entry.Level == level);
            }
        }
    }
}