using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class RaceChangeExpenditure : EoCExpenditure
    {
        public RaceChangeExpenditure( Player pl ) : base(pl, EoCExpenditureType.RaceChange) { }

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
        public RaceChangeExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
