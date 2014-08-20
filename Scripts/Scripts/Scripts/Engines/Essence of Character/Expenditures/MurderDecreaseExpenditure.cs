using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class MurderDecreaseExpenditure : EoCExpenditure
    {
        public MurderDecreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.MurderCountDecrease, false) { }

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
        public MurderDecreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
