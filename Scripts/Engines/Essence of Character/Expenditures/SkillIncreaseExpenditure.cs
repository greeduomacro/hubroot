using Server.Commands;
using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public class SkillIncreaseExpenditure : EoCExpenditure
    {
        [CommandAttribute("SkillInc", Server.AccessLevel.Counselor)]
        public static void OnCommand( CommandEventArgs args )
        {
            if( args.Arguments.Length != 1 )
            {
                args.Mobile.SendMessage("Format: SkillInc <SkillName | undo>");
                return;
            }

            string arg = (string)args.Arguments[0];
            Player pl = args.Mobile as Player;

            if( arg == "undo" )
            {
                pl.EoCLedger.Undo();
            }
            else
            {
                SkillName sn = (SkillName)System.Enum.Parse(typeof(SkillName), arg, true);
                EoCExpenditure.Use<Skill>(pl, EoCExpenditureType.SkillIncrease, pl.Skills[sn]);
            }
        }

        public static void Initialize()
        {
            Register(EoCExpenditureType.SkillIncrease, typeof(SkillIncreaseExpenditure));
        }

        private int SkillID { get; set; }

        public SkillIncreaseExpenditure( Player pl ) : base(pl, EoCExpenditureType.SkillIncrease) { }

        public override bool Use<T>( T state )
        {
            Skill sk = state as Skill;

            if( sk.BaseFixedPoint >= sk.CapFixedPoint )
                return false;

            int toConsume = (int)((sk.Base / 0.1) * 10);

            if( ConsumeEoC(toConsume) )
            {
                SkillID = sk.SkillID;
                sk.BaseFixedPoint += 1;

                return true;
            }

            return false;
        }

        public override void Undo()
        {
            base.Undo();

            Player.Skills[SkillID].BaseFixedPoint -= 1;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)SkillID);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public SkillIncreaseExpenditure( GenericReader reader )
            : base(reader)
        {
            SkillID = reader.ReadInt();
        }
    }
}
