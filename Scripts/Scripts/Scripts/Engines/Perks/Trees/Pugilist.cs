using Server.Mobiles;
using Server;
using Server.Items;
using Server.Spells;
using System.Collections;
using System;

namespace Server.Perks
{


    public class Pugilist : Perk
    {
        public void ComboAttack(Mobile attacker, Mobile defender, BaseWeapon weapon)
        {
            if (Level >= PerkLevel.Fifth)
            {
                if (weapon is Fists)
                {
                    int hitChance = (attacker.Dex / 8);
                    if (hitChance > Utility.RandomMinMax(0, 100))
                    {
                        int hits = Utility.RandomMinMax(1, 2);
                        int damage = Utility.RandomMinMax(1, 3) + (attacker.Str / 10);

                        for (int i = 1; i <= hits; i++)
                        {
                            attacker.DoHarmful(defender);
                            defender.Damage(damage);

                            int sound = Utility.RandomMinMax(1, 3);

                            switch (sound)
                            {
                                case 1:
                                    {
                                        attacker.PlaySound(0x13E);
                                        break;
                                    }
                                case 2:
                                    {
                                        attacker.PlaySound(0x145);
                                        break;
                                    }
                                case 3:
                                    {
                                        attacker.PlaySound(0x142);
                                        break;
                                    }
                            }
                        }

                        attacker.SendMessage("You strike your opponent multiple times in a combination-attack!");
                    }
                }
            }
        }

        public int Brawler(BaseWeapon weapon)
        {
            if (Level >= PerkLevel.Third)
            {
                if (weapon is Fists)
                {                 
                    return 25;
                }
            }             

            return 0;
 
        }
        public bool TryDodge()
        {
            if (Level >= PerkLevel.Second)
            {
                if (Player.Weapon is Fists)
                {
                    if (Utility.RandomDouble() <= 0.33)
                    {
                        Player.Emote("*Dodges*");
                        Player.SendMessage("You dodge your opponents attack!");
                        Player.FixedEffect(0x37B9, 10, 16);
                        return true;
                    }
                }

                else
                {
                    if (Utility.RandomDouble() <= 0.16)
                    {
                        Player.Emote("*Dodges*");
                        Player.SendMessage("You dodge your opponents attack!");
                        Player.FixedEffect(0x37B9, 10, 16);
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void Concussion(Mobile attacker, Mobile defender)
        {
            BaseWeapon weapon = attacker.Weapon as BaseWeapon;

            attacker.SendLocalizedMessage(1060165); // You have delivered a concussion!
            defender.SendLocalizedMessage(1060166); // You feel disoriented!

            defender.PlaySound(0x213);
            defender.FixedParticles(0x377A, 1, 32, 9949, 1153, 0, EffectLayer.Head);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 10), defender.Map), new Entity(Serial.Zero, new Point3D(defender.X, defender.Y, defender.Z + 20), defender.Map), 0x36FE, 1, 0, false, false, 1133, 3, 9501, 1, 0, EffectLayer.Waist, 0x100);

            int damage = 5;

            if (defender.HitsMax > 0)
            {
                double hitsPercent = ((double)defender.Hits / (double)defender.HitsMax) * 100.0;

                double manaPercent = 0;

                if (defender.ManaMax > 0)
                    manaPercent = ((double)defender.Mana / (double)defender.ManaMax) * 100.0;

                damage += Math.Min((int)(Math.Abs(hitsPercent - manaPercent) / 4), 20);
            }

            defender.Damage(damage, attacker);
        }

        public bool Hardened()
        {
            return Level >= PerkLevel.Second;
        }

        public virtual void MartialArt(Mobile attacker, Mobile defender)
        {
            if (Level >= PerkLevel.Fourth)
            {
                if (0.10 >= Utility.RandomDouble())
                {
                   Concussion(attacker, defender);
                }
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Pugilist( Player player )
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
        public Pugilist( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "The master of hand to hand combat."; } }
        public override int GumpID { get { return 2255; } }
        public override string Label { get { return "Pugilist"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Footwork", "Without a weapon, even the most meager of weapons pose grave danger. The first thing you learn as a fist-fighter is not to get hit."),
                    new LabelEntry(PerkLevel.Second, "Hardened", "Conditioning your bones by smashing them against any hard surface you can find has made them more resilient and more damaging."),
                    new LabelEntry(PerkLevel.Third, "Brawler", "The muscles in your arms are formed by beating your foes to death with your bare hands, granting you an agile strength-to-weight ratio."),
                    new LabelEntry(PerkLevel.Fourth, "Martial Art", "Years of study, practice, and application pushes your capacity for hand to hand combat more in to the realm of art."),
                    new LabelEntry(PerkLevel.Fifth, "Flash and Awe", "With hand speeds nearing the unseen, and no weight in hand to slow them, the pugilist is capable of unleashing rapid combination attacks.")
                });
            }
        }
    }
}