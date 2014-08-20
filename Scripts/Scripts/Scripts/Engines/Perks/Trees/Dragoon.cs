using Server.Mobiles;
using Server;
using Server.Events;
using Server.Items;
using System;
using Server.Misc;

namespace Server.Perks
{
    public class Dragoon : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Dragoon( Player player )
            : base(player)
        {
        }

        public bool SaddleBound()
        {
            return (Level >= PerkLevel.First);
        }

        public bool MountedRival()
        {
            return (Level >= PerkLevel.Second);
        }

        public int MomentousStrike(Mobile attacker)
        {

            if (Level >= PerkLevel.Third)
            {  
                Player aggressor = attacker as Player;
                aggressor.Delta(MobileDelta.Direction);
                Direction d = aggressor.Direction;

                 EventDispatcher.InvokePlayerMove(new PlayerMoveEventArgs(aggressor, d));
                 bool running = ((d & Direction.Running) != 0);

                 if (running && aggressor.Mounted)
                 {
                     if(Player.lastMove > DateTime.Now - TimeSpan.FromSeconds(0.5))
                     return 50;
                 }
            }

            return 0;
        }

        public double LongArm(BaseWeapon weapon) 
        {
            if (Level >= PerkLevel.Fourth)
            {
                if (weapon.Layer == Layer.TwoHanded)
                    return 0.33;
            }

            return 0;
        }

        public int Symbiosis()
        {
            if (Level >= PerkLevel.Fifth)
            {
                if (Player.Mounted)
                {
                    return 8;
                }
            }
            return 0;
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
        public Dragoon( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "A fierce warrior; unmatched in mounted combat."; } }
        public override int GumpID { get { return 20745; } }
        public override string Label { get { return "Dragoon"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Saddle Bound", "It is nearly impossible to separate a dragoon from their steed."),
                    new LabelEntry(PerkLevel.Second, "Mounted Rival", "The dragoon is capable of dismounting opponents from horseback."),
                    new LabelEntry(PerkLevel.Third, "Momentous Strike", "Using the momentum of their mount, the dragoon is capable of dealing more damage with melee weapons when running."),
                    new LabelEntry(PerkLevel.Fourth, "The Longarm", "The dragoon is far more accurate when using two-handed weapons."),
                    new LabelEntry(PerkLevel.Fifth, "Symbiotic", "The bond between the dragoon and their mount is so strong, it alters their physiology.")
                });
            }
        }
    }
}