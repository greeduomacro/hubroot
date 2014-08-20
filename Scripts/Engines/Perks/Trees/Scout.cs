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
using Server.Traps;


namespace Server.Perks
{
    public class Scout : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Scout( Player player )
            : base(player)
        {
        }

        public int StamRegenBonus()
        {
            if (Level >= PerkLevel.Third)
                return 7;
            else
                return 0;
        }

        public bool Undetectable()
        {
            return (Level >= PerkLevel.Fourth);
        }

        ///<Summary>
        ///Allows the scout to see hidden players within proximity.
        /// </Summary>
        public bool CanSeeHidden()
        {
            return (Level >= PerkLevel.Fifth);
        }

        ///<summary>
        ///Allows the scout to hide in heavily lit areas.
        /// </summary>
        public bool HideInLight()
        {
            return (Level >= PerkLevel.Second);
        }

        /// <summary>
        /// Adds a [trapped] label to a trapped item at <code>PerkLevel.First</code>
        /// </summary>
        public bool LabelTrappedDoor(BaseDoor door)
        {
            if (Level < PerkLevel.First)
                return false;

            if (door.TrapType != DoorTrapType.None)
                return true;

            return false;
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
        public Scout( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "Highly experienced behind enemy lines."; } }
        public override int GumpID { get { return 2249; } }
        public override string Label { get { return "Scout"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Spotter", "Being the first in has tought the scout how to easily spot traps on doors."),
                    new LabelEntry(PerkLevel.Second, "Camoflauge", "Spending much time behind enemy lines you develop the ability to hide in any light."),
                    new LabelEntry(PerkLevel.Third, "Endurance", "Your body becomes more adept at metabolising the acid produced by muscles under stress."),
                    new LabelEntry(PerkLevel.Fourth, "Undetectable", "With great understanding of the art hiding, you are nearly impossible to detect."),
                    new LabelEntry(PerkLevel.Fifth, "Keen Eyes", "Ever-vigilant, the scout is capable of spotting hidden players in close proximity.")
                });
            }
        }
    }
}