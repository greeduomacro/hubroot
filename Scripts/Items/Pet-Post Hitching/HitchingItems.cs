using Server.Targeting;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HitchingRope : Item
    {
        private int m_Uses;

        [Constructable]
        public HitchingRope()
            : base(0x1EA0)
        {
            Name = "a strong rope";
            Weight = 2.0;

            m_Uses = Utility.RandomMinMax(20, 50);
        }

        public override void OnDoubleClick( Mobile from )
        {
            if( !IsChildOf(from.Backpack) )
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                if( --m_Uses <= 0 )
                {
                    this.Delete();
                    from.SendMessage("As you work with the rope, you realize it is in no condition to hold your pet.");
                }
                else
                {
                    from.Target = new HitchToPostTarget(this);
                    from.SendMessage("Target the post you wish to attach the rope to.");
                }
            }
        }

        public HitchingRope( Serial serial )
            : base(serial)
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)1);

            writer.Write((int)m_Uses);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if( version >= 1 )
                m_Uses = reader.ReadInt();
            else
                m_Uses = Utility.RandomMinMax(20, 50);
        }
    }

    [FlipableAttribute(0x14E7, 0x14E8)]
    public class HitchingPost : Item
    {
        [Constructable]
        public HitchingPost()
            : base(0x14E8)
        {
            Name = "a hitching post";
            Movable = true;
            Weight = 10.0;
        }

        public override void OnDoubleClick( Mobile m )
        {
            if( !m.InRange(this.GetWorldLocation(), 2) )
                m.SendLocalizedMessage(500295);
            else
            {
                m.SendMessage("Target the pet you wish to unhitch.");
                m.Target = new unHitchTarget(this);
            }
        }

        public HitchingPost( Serial serial )
            : base(serial)
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}