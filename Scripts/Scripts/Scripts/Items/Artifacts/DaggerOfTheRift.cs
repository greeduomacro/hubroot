using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Items
{
    public class DaggerOfTheRift : BaseKnife
    {
        private DateTime _lastUsed;

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.InfectiousStrike; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ShadowStrike; } }

        public override int AosStrengthReq { get { return 10; } }
        public override int AosMinDamage { get { return 10; } }
        public override int AosMaxDamage { get { return 11; } }
        public override int AosSpeed { get { return 56; } }

        public override int OldStrengthReq { get { return 1; } }
        public override int OldMinDamage { get { return 3; } }
        public override int OldMaxDamage { get { return 15; } }
        public override int OldSpeed { get { return 55; } }

        public override int InitMinHits { get { return 0; } }
        public override int InitMaxHits { get { return 0; } }

        public override SkillName DefSkill { get { return SkillName.Fencing; } }
        public override WeaponType DefType { get { return WeaponType.Piercing; } }
        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Pierce1H; } }

        [Constructable]
        public DaggerOfTheRift()
            : base(0xF52)
        {
            Hue = 1342;
            Layer = Layer.OneHanded;
            LootType = LootType.Blessed;
            Name = "H'slamirs Dagger of the Rift";
            Weight = 1.0;
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damage)
        {
            base.OnHit(attacker, defender, damage);

            if (DateTime.Now >= (_lastUsed + TimeSpan.FromSeconds(4.0)))
            {
                attacker.Target = new InternalTarget(this, defender);
            }
        }

        public void Target(IPoint3D p, Mobile defender)
        {
            Map map = defender.Map;

            if (this.Parent == null || !(this.Parent is Mobile))
                return;

            Mobile attacker = (Mobile)this.Parent;

            if (p is RecallRune)
            {
                RecallRune rune = (RecallRune)p;

                if (rune.Marked)
                {
                    defender.PublicOverheadMessage(Server.Network.MessageType.Regular, defender.SpeechHue, true, "*the earth trembles and time slows, a small rift appearing and drawing you both into its depth*");
                    attacker.SendMessage("You and your combatant have been sent to the rune\'s location.");

                    defender.MoveToWorld(rune.Target, rune.TargetMap);
                    attacker.MoveToWorld(rune.Target, rune.TargetMap);

                    Effects.PlaySound(defender, map, 510);
                    Effects.SendLocationParticles(defender, 0x37C4, 10, 10, 2023);
                    Effects.PlaySound(attacker, map, 510);
                    Effects.SendLocationParticles(attacker, 0x37C4, 10, 10, 2023);
                }
                else
                    attacker.SendMessage("That is not a valid target.");
            }
            else if (map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z))
            {
                attacker.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (Server.Spells.SpellHelper.CheckMulti(new Point3D(p), map))
            {
                attacker.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (defender.BodyValue != 402 && defender.BodyValue != 403)
            {
                _lastUsed = DateTime.Now;

                Point3D to = new Point3D(p);

                defender.MoveToWorld(to, map);
                defender.ProcessDelta();

                Effects.PlaySound(defender, map, 510);
                Effects.SendLocationParticles(defender, 0x37C4, 10, 10, 2023);
                Effects.SendLocationParticles(EffectItem.Create(to, map, EffectItem.DefaultDuration), 0x37C4, 10, 10, 5023);
            }
        }

        public class InternalTarget : Target
        {
            private DaggerOfTheRift _owner;
            private Mobile _defender;

            public InternalTarget(DaggerOfTheRift owner, Mobile defender)
                : base(12, true, TargetFlags.None)
            {
                _owner = owner;
                _defender = defender;

                CheckLOS = false;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D point = o as IPoint3D;

                if (point != null)
                    _owner.Target(point, _defender);
            }
        }

        public override void OnDoubleClick(Mobile m)
        {
            DaggerOfTheRift dagger = m.FindItemOnLayer(Layer.OneHanded) as DaggerOfTheRift;

            if (dagger == null)
                m.SendMessage("You must have this equipped to use it.");
            else
            {
                RiftSpell spell = new RiftSpell(m, null);
                spell.Cast();
            }
        }

        public DaggerOfTheRift(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}