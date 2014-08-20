using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class KORespawnExpenditure : EoCExpenditure
    {
        public KORespawnExpenditure( Player pl ) : base(pl, EoCExpenditureType.KORespawn, false) { }

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
        public KORespawnExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
