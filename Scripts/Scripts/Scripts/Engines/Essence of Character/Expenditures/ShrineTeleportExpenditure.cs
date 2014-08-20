using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class ShrineTeleportExpenditure : EoCExpenditure
    {
        public ShrineTeleportExpenditure( Player pl ) : base(pl, EoCExpenditureType.ShrineTeleport) { }

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
        public ShrineTeleportExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
