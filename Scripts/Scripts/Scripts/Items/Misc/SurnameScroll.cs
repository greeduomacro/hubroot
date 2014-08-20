using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.SkillSelection;
using Server.Surnames;

namespace Server.Items
{
    public class SurnameScroll : Item
    {
        [Constructable]
        public SurnameScroll()
            : this(1)
        {
        }

        [Constructable]
        public SurnameScroll(int amount)
            : base(0x1F4F)
        {
            Name = "a rolled scroll";
        }

        public SurnameScroll(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack.");
            }

            else if (this != null && from is Player)
            {
                from.SendGump(new SurnameRegistrarGump((Player)from, GumpType.Scroll));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}