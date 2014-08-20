using Server.Mobiles;
using Sk = Server.Misc.SkillCheck;

namespace Server.EssenceOfCharacter
{
    public class StatIncreaseExpenditure : EoCExpenditure
    {
        public static void Initialize()
        {
            Register(EoCExpenditureType.StatIncrease, typeof(StatIncreaseExpenditure));
        }

        private Sk.Stat Stat { get; set; }

        public StatIncreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.StatIncrease) { }

        public override bool Use<T>( T state )
        {
            Stat = (Sk.Stat)((object)state);

            if( !Sk.CanRaise(Player, Stat) )
                return false;

            int toConsume = 0;

            switch( Stat )
            {
                case Sk.Stat.Dex:
                    toConsume = Player.RawDex;
                    break;
                case Sk.Stat.Int:
                    toConsume = Player.RawInt;
                    break;
                case Sk.Stat.Str:
                    toConsume = Player.RawStr;
                    break;
            }

            toConsume *= 1000;

            if( ConsumeEoC(toConsume) )
            {
                Sk.IncreaseStat(Player, Stat, false);

                return true;
            }

            return false;
        }

        public override void Undo()
        {
            base.Undo();

            if( !Sk.CanLower(Player, Stat) )
                return;

            switch( Stat )
            {
                case Sk.Stat.Dex:
                    --Player.RawDex;
                    break;
                case Sk.Stat.Int:
                    --Player.RawInt;
                    break;
                case Sk.Stat.Str:
                    --Player.RawStr;
                    break;
            }
        }

        /// <summary>
        /// Serialization
        /// </summary>
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public StatIncreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
