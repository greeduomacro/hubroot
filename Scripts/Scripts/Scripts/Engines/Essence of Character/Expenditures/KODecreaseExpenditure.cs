using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class KODecreaseExpenditure : EoCExpenditure
    {
        public KODecreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.KOCountDecrease, false) { }

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
        public KODecreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
