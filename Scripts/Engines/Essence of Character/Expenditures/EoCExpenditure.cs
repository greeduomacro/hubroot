using System;
using Server.Mobiles;

namespace Server.EssenceOfCharacter
{
    public enum EoCExpenditureType : byte
    {
        SkillIncrease,
        SkillCapIncrease,
        StatIncrease,
        PerkIncrease,
        PerkUnlock,
        RaceIncrease,
        RaceChange,
        ShrineTeleport,

        KOCountDecrease,
        KORespawn,
        MurderCountDecrease,
    }

    public abstract class EoCExpenditure
    {
        private static Type[] ExpenseTypes = new Type[0xFF];

        /// <summary>
        /// Create an expenditure of the given type
        /// </summary>
        public static EoCExpenditure Create( Player pl, EoCExpenditureType type )
        {
            EoCExpenditure exp = (EoCExpenditure)Activator.CreateInstance(ExpenseTypes[(int)type], pl);

            return exp;
        }

        /// <summary>
        /// Creates a new expenditure, uses it, and - if successful - appends the expenditure to the player's EoCLedger
        /// </summary>
        public static bool Use<T>( Player pl, EoCExpenditureType type, T state )
        {
            EoCExpenditure exp = Create(pl, type);

            if( exp.Use<T>(state) )
            {
                pl.EoCLedger.Append(exp);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Registers an expenditure type
        /// </summary>
        public static void Register( EoCExpenditureType type, Type expenditure )
        {
            int id = (int)type;

            if( ExpenseTypes[id] != null )
                throw new Exception("Duplicate type registration " + type);

            ExpenseTypes[id] = expenditure;
        }

        /// <summary>
        /// Amount of EoC that was consumed by using this expenditure
        /// </summary>
        public int ConsumedEoC { get; protected set; }
        /// <summary>
        /// Reference to the player that owns this expenditure
        /// </summary>
        public Player Player { get; protected set; }
        /// <summary>
        /// Flag that indicates if spent EoC should be recovered when the player is recreated
        /// </summary>
        public bool RecoverOnRecreate { get; protected set; }
        /// <summary>
        /// Type of expenditure category
        /// </summary>
        public EoCExpenditureType Type { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        public EoCExpenditure( Player pl, EoCExpenditureType type ) : this(pl, type, true) { }

        /// <summary>
        /// ctor
        /// </summary>
        public EoCExpenditure( Player pl, EoCExpenditureType type, bool recoverOnRecreate )
        {
            Player = pl;
            RecoverOnRecreate = recoverOnRecreate;
            Type = type;
        }

        /// <summary>
        /// Uses the expenditure and deducts the required EoC from the given <code>Player</code>
        /// </summary>
        public abstract bool Use<T>( T state );

        /// <summary>
        /// Undoes an expenditure and restores spent EoC if <code>RecoverOnRecreate == true</code>
        /// </summary>
        public virtual void Undo()
        {
            if( RecoverOnRecreate && ConsumedEoC > 0 )
                Player.EssenceOfCharacter += ConsumedEoC;
        }

        /// <summary>
        /// Tries to consume an amount of <code>EssenceOfCharacter</code>
        /// </summary>
        protected bool ConsumeEoC( int toConsume )
        {
            bool res = Player.ConsumeEoC(toConsume);

            if( res )
                ConsumedEoC = toConsume;

            return res;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        public virtual void Serialize( GenericWriter writer )
        {
            writer.Write((byte)Type);
            writer.Write(ConsumedEoC);
            writer.WriteMobile<Player>(Player);
            writer.Write(RecoverOnRecreate);
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        public EoCExpenditure( GenericReader reader )
        {
            Type = (EoCExpenditureType)reader.ReadByte();
            ConsumedEoC = reader.ReadInt();
            Player = reader.ReadMobile<Player>();
            RecoverOnRecreate = reader.ReadBool();
        }
    }
}