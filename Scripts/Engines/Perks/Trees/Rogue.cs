using Server.Mobiles;
using Server;
using Server.Items;

namespace Server.Perks
{
    public class Rogue : Perk
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Rogue( Player player )
            : base(player)
        {
        }

        public bool SafeCracker()
        {
            return (Level >= PerkLevel.First);
        }

        public bool Ambush;

        public bool ShieldEquipped
        {
            get
            {
                return Player != null && (Player.FindItemOnLayer(Layer.TwoHanded) is BaseShield);
            }
        }

        public bool CloakAndDagger(Mobile attacker)
        {

            if (Level >= PerkLevel.Second)
            {
                BaseWeapon weapon = Player.Weapon as BaseWeapon;
                Item cloak = Player.FindItemOnLayer(Layer.Cloak);

                if (weapon is BaseKnife && cloak is BaseCloak && !ShieldEquipped)
                {
                    if (Utility.RandomDouble() <= 0.21)
                    {
                        Player.FixedEffect(0x37B9, 10, 16);
                        Player.SendMessage("You redirect your opponent's attack with your cloak!");
                        attacker.SendMessage("Your opponent redirects your attack.");
                        return true;
                    }
                }                  
            }

            return false;
        }

        public bool Unseen()
        {
            return (Level >= PerkLevel.Fourth);
        }

        public bool CanRunHidden()
        {
            return (Level >= PerkLevel.Third);
        }

        public bool CanJumpHidden()
        {
            return (Level >= PerkLevel.Third);
        }

        public void SurpriseAttack(Mobile defender, int damage)
        {
            if (Level >= PerkLevel.Fifth)
            {
                if (Player.Hidden)
                {
                    Player.SendMessage("You catch your opponent off-guard!");
                    defender.SendMessage("Your opponent catches you off-guard!");
                    defender.Damage(damage, Player);
                }
            }
        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void Serialize( Server.GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)1); //version
            writer.Write(Ambush);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public Rogue( GenericReader reader )
            : base(reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    break;

                case 1:
                    Ambush = reader.ReadBool();
                    goto case 0;
            }
        }

        public override string Description { get { return "The professional taker. Be it your life, or your goods."; } }
        public override int GumpID { get { return 2274; } }
        public override string Label { get { return "Rogue"; } }

        public override LabelEntryList LabelEntries
        {
            get
            {
                return new LabelEntryList(new LabelEntry[]
                {
                    new LabelEntry(PerkLevel.First, "Safe-Cracker", "As such a master at technique, you produce no noise when manipulating lock tumblers, and never break a tool."),
                    new LabelEntry(PerkLevel.Second, "Cloak And Dagger", "The rogue is capable of using a cloak and dagger to redirect an opponents attack."),
                    new LabelEntry(PerkLevel.Third, "Stalking Prey", "Having stalked many prey, you have perfected the art of running and jumping unnoticed."),
                    new LabelEntry(PerkLevel.Fourth, "The Unseen", "The rogue is capable of lifting objects from prey without their notice."),
                    new LabelEntry(PerkLevel.Fifth, "Assassination", "The rogue deals bonus damage when breaking from stealth to launch an attack.")
                });
            }
        }
    }
}