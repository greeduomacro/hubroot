using System.Collections.Generic;
using System.Reflection;
using Server.Mobiles;
using Server.Utilities;

namespace Server.EssenceOfCharacter
{
    /// <summary>
    /// The <code>EoCLedger</code> is a structure to keep records of what
    /// a player spends EoC on, in order to be able to roll back these expenditures
    /// </summary>
    public class EoCLedger
    {
        public Player Player { get; set; }
        public Stack<EoCExpenditure> Expenditures { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        public EoCLedger( Player pl )
        {
            Player = pl;
            Expenditures = new Stack<EoCExpenditure>();
        }

        /// <summary>
        /// Adds an expenditure to this ledger
        /// </summary>
        public virtual void Append( EoCExpenditure exp )
        {
            Expenditures.Push(exp);
        }

        /// <summary>
        /// Rolls back the last expenditure
        /// </summary>
        public virtual void Undo()
        {
            if( Expenditures.Count == 0 )
                return;

            Expenditures.Pop().Undo();
        }

        /// <summary>
        /// Serialization
        /// </summary>
        public virtual void Serialize( GenericWriter writer )
        {
            writer.Write((int)0); //version

            writer.WriteMobile<Player>(Player);
            writer.Write((int)Expenditures.Count);

            foreach( EoCExpenditure exp in Expenditures )
            {
                writer.Write(exp.GetType().FullName);
                exp.Serialize(writer);
            }
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public EoCLedger( GenericReader reader )
        {
            int version = reader.ReadInt();

            Player = reader.ReadMobile<Player>();

            int count = reader.ReadInt();
            Expenditures = new Stack<EoCExpenditure>(count);

            if( count > 0 )
            {
                EoCExpenditure exp;
                ConstructorInfo ctor;

                for( int i = 0; i < count; i++ )
                {
                    ctor = Util.GetDeserializationCtor(reader.ReadString());
                    exp = ctor.Invoke(new object[] { reader }) as EoCExpenditure;

                    Expenditures.Push(exp);
                }
            }
        }
    }
}