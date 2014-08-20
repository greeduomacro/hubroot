using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class PerkIncreaseExpenditure : EoCExpenditure
    {
        public PerkIncreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.PerkIncrease) { }

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
        public PerkIncreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
