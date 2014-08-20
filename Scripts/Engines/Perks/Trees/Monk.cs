using Server.Mobiles;
using Server;
using Server.Items;

namespace Server.Perks
{
    public class Monk : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Monk( Player player )
            : base(player)
        {
        }

        public void ShenStrike(Mobile attacker, Mobile defender, BaseWeapon weapon)
        {
            if (Level >= PerkLevel.Fifth)
            {
                if (weapon is Fists && Utility.RandomDouble() <= 0.10)
                {
                    int element = Utility.RandomMinMax(2, 4);

                    switch (element)
                    {
                        case 1:
                            {
                                weapon.DoLightning(attacker, defender);
                                break;
                            }
                        case 2:
                            {
                                weapon.DoMagicArrow(attacker, defender);
                                break;
                            }
                        case 3:
                            {
                                weapon.DoHarm(attacker, defender);
                                break;
                            }
                        case 4:
                            {
                                weapon.DoFireball(attacker, defender);
                                break;
                            }

                    }
                }
            }
        }

        public int MeditationMastery()
        {
            if (Level >= PerkLevel.Fourth && Player.Meditating)
            {
                return 8;
            }

            return 0;
        }

        public static bool HasFreeHands(Mobile m)
        {
            Item handOne = m.FindItemOnLayer(Layer.OneHanded);
            Item handTwo = m.FindItemOnLayer(Layer.TwoHanded);

            if (handTwo is BaseWeapon)
                handOne = handTwo;

            return (handOne == null && handTwo == null);
        }

        public bool ParrySpell()
        {
            if (Level >= PerkLevel.Third)
            {
                if ( HasFreeHands((Mobile)Player) && Utility.RandomDouble() < 0.25)
                {
                    Player.FixedEffect(0x37B9, 10, 16);
                    Player.SendMessage("You deflect your opponent's spell!");
                    return true;
                }
            }

            return false;
        }

        public void Purge(Mobile defender, BaseWeapon weapon)
        {
            if (Level >= PerkLevel.Second)
            {
                if (defender is BaseCreature & Utility.RandomBool())
                {
                    BaseCreature creature = defender as BaseCreature;
                    if (creature.Summoned && weapon is Fists)
                    {
                        creature.Dispel(defender);
                    }
                }
            }
        }

        public void QiStrike(Mobile defender, BaseWeapon weapon, int damage)
        {
            if (Level >= PerkLevel.First)
            {
                if (weapon is Fists)
                {
                    int manaDamage = (int)(damage * 0.5) + 1;

                    if (defender.Mana >= manaDamage)
                    {
                        defender.Mana -= manaDamage;
                    }

                    else if (defender.Mana < manaDamage)
                        defender.Mana = 0;

                    defender.FixedEffect(0x37BE, 10, 16);

                }
            }
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
        public Monk( GenericReader reader )
            : base(reader)
        {
        }

        public override string Description { get { return "Disciplined warrior capable of manipulating ether by hand."; } }
        public override int GumpID { get { return 2273; } }
        public override string Label { get { return "Monk"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Qi-jing", "The Monk has to capacity to affect other living creature's life force through touch."),
                    new LabelEntry(PerkLevel.Second, "Exorcist", "The Monk is capable of reaching inside summoned creatures and unraveling the fabric of their being."),
                    new LabelEntry(PerkLevel.Third, "Spellbreaker", "Being well-versed in all worldly energies, the Monk is able to parry spells with only their hands."),
                    new LabelEntry(PerkLevel.Fourth, "Mantra", "Using mantra meditation the Monk is capable of altering their physiology through thought alone."),
                    new LabelEntry(PerkLevel.Fifth, "The Five Shen", "By tapping into the mana stored in an individual organ, the Monk is able to unleash elemental attacks.")
                });
            }
        }
    }
}