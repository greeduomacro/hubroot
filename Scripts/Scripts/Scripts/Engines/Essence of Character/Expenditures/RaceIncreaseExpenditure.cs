using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class RaceIncreaseExpenditure : EoCExpenditure
    {
        public RaceIncreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.RaceIncrease) { }

        public override bool Use<T>( T state )
        {
            return false;
        }

        public override void Undo()
        {
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
        public RaceIncreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
