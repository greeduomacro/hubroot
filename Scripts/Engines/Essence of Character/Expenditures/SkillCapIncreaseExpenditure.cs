using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class SkillCapIncreaseExpenditure : EoCExpenditure
    {
        public SkillCapIncreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.SkillCapIncrease) { }

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
        public SkillCapIncreaseExpenditure( GenericReader reader )
            : base(reader)
        {
        }
    }
}
