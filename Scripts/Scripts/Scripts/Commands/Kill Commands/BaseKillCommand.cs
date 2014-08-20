using System.Collections.Generic;
using Server.Commands.Generic;
using Server.Items;

namespace Server.Commands
{
    public abstract class BaseKillCommand : BaseCommand
    {
        private TimedItem _timedCorpse;

        public TimedItem TimedCorpse { get { return _timedCorpse; } }

        public BaseKillCommand()
        {
            AccessLevel = AccessLevel.Seer;
            ObjectTypes = ObjectTypes.Mobiles;
            Supports = CommandSupport.AllMobiles;
        }

        #region +virtual void Kill( Mobile, bool, bool, bool )
        /// <summary>
        /// Kills the given Mobile
        /// </summary>
        /// <param name="m">Mobile to kill</param>
        /// <param name="packToBank">true to move the Mobile's items to the bank</param>
        /// <param name="createTimedCorpse">true to create a temporary corpse</param>
        /// <param name="deleteCorpse">true to delete the Mobile's corpse after death</param>
        public virtual void Kill( Mobile m, bool packToBank, bool createTimedCorpse, bool deleteCorpse )
        {
            if( m == null )
                return;

            if( packToBank )
                PackToBank(m);

            if( createTimedCorpse )
            {
                _timedCorpse = new TimedItem(120.0, Utility.Random(0xECA, 8));
                _timedCorpse.MoveToWorld(m.Location, m.Map);
            }

            m.Kill();

            if( deleteCorpse && m.Corpse != null )
                m.Corpse.Delete();
        }
        #endregion

        #region +virtual void PackToBank( Mobile )
        /// <summary>
        /// Moves all of the given Mobile's items to their bank.
        /// </summary>
        public virtual void PackToBank( Mobile m )
        {
            if( m == null || !m.Player )
                return;

            BankBox bank = m.FindBankNoCreate();
            Backpack pack = new Backpack();
            pack.Hue = 1157;

            List<Item> items = new List<Item>();

            for( int i = 0; i < m.Items.Count; i++ )
            {
                if( m.Items[i] is BankBox || (m.Backpack != null && m.Items[i].Serial == m.Backpack.Serial) )
                    continue;

                items.Add(m.Items[i]);
            }

            if( m.Backpack != null )
            {
                for( int i = 0; i < m.Backpack.Items.Count; i++ )
                    items.Add(m.Backpack.Items[i]);
            }

            items.ForEach(
                delegate( Item i )
                {
                    pack.AddItem(i);
                });
            items.Clear();

            if( bank == null )
                pack.MoveToWorld(m.Location, m.Map);
            else
                bank.AddItem(pack);
        }
        #endregion
    }
}