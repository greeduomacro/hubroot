using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class PerkUnlockExpenditure : EoCExpenditure
    {
        public PerkUnlockExpenditure( Player pl ) : base(pl, EoCExpenditureType.PerkUnlock) { }

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
        public PerkUnlockExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
