
namespace Server.Events
{
    public delegate void CorpseActionEventHandler( CorpseActionEventArgs args );

    public delegate void CreatureDamagedEventHandler( CreatureDamagedEventArgs args );
    public delegate void CreatureKilledEventHandler( CreatureKilledEventArgs args );
    public delegate void CreatureTameStateChangeHandler( CreatureTameStateChangeArgs args );

    public delegate void ItemCraftEventHandler( ItemCraftEventArgs args );
    public delegate void ItemPurchasedEventHandler( ItemPurchasedEventArgs args );
    public delegate void ItemSoldEventHandler( ItemSoldEventArgs args );

    public delegate void PlayerDamagedEventHandler( PlayerDamagedEventArgs args );
    public delegate void PlayerDismountedEventHandler( PlayerEventArgs args );
    public delegate void PlayerDrinkingEventHandler( PlayerDrinkingEventArgs args );
    public delegate void PlayerEatingEventHandler( PlayerEatingEventArgs args );
    public delegate void PlayerKarmaChangeEventHandler( PlayerKarmaChangeEventArgs args );
    public delegate void PlayerMoveEventHandler( PlayerMoveEventArgs args );
    public delegate void PlayerRawStatChangeEventHandler( PlayerRawStatChangeEventArgs args );
    public delegate void PlayerSkillChangedEventHandler( PlayerSkillChangedEventArgs args );
    public delegate void PlayerStealEventHandler( PlayerStealEventArgs args );

    public delegate void SpellCastEventHandler( SpellCastEventArgs args );
    public delegate void SpellFailedEventHandler( SpellEventArgs args );
    public delegate void SpellFinishedEventHandler( SpellEventArgs args );

    public class EventDispatcher
    {
        /// <summary>
        /// Occurs when an action is taken on a <code>Corpse</code>
        /// </summary>
        public static event CorpseActionEventHandler CorpseAction;

        public static void InvokeCorpseAction( CorpseActionEventArgs args )
        {
            if( CorpseAction != null )
                CorpseAction(args);
        }

        /// <summary>
        /// Occurs when a <code>BaseCreature</code> is damaged
        /// </summary>
        public static event CreatureDamagedEventHandler CreatureDamaged;
        /// <summary>
        /// Occurs when a <code>BaseCreature</code> is killed by a player
        /// </summary>
        public static event CreatureKilledEventHandler CreatureKilled;
        /// <summary>
        /// Occurs when a <code>BaseCreature</code> is tamed or released
        /// </summary>
        public static event CreatureTameStateChangeHandler CreatureTameChange;

        public static void InvokeCreatureDamaged( CreatureDamagedEventArgs args )
        {
            if( CreatureDamaged != null )
                CreatureDamaged(args);
        }

        public static void InvokeCreatureKilled( CreatureKilledEventArgs args )
        {
            if( CreatureKilled != null )
                CreatureKilled(args);
        }

        public static void InvokeCreatureTameStateChange( CreatureTameStateChangeArgs args )
        {
            if( CreatureTameChange != null )
                CreatureTameChange(args);
        }

        /// <summary>
        /// Occurs when a player attempts to craft an item
        /// </summary>
        public static event ItemCraftEventHandler ItemCraft;
        /// <summary>
        /// Occurs when an item is purchased from a vendor
        /// </summary>
        public static event ItemPurchasedEventHandler ItemPurchased;
        /// <summary>
        /// Occurs when an item is sold to a vendor
        /// </summary>
        public static event ItemSoldEventHandler ItemSold;

        public static void InvokeItemCraft( ItemCraftEventArgs args )
        {
            if( ItemCraft != null )
                ItemCraft(args);
        }

        public static void InvokeItemPurchased( ItemPurchasedEventArgs args )
        {
            if( ItemPurchased != null )
                ItemPurchased(args);
        }

        public static void InvokeItemSold( ItemSoldEventArgs args )
        {
            if( ItemSold != null )
                ItemSold(args);
        }

        /// <summary>
        /// Occurs when a player is damaged
        /// </summary>
        public static event PlayerDamagedEventHandler PlayerDamaged;
        /// <summary>
        /// Occurs when a player is dismounted
        /// </summary>
        public static event PlayerDismountedEventHandler PlayerDismounted;
        /// <summary>
        /// Occurs when a player consumes drink
        /// </summary>
        public static event PlayerDrinkingEventHandler PlayerDrinking;
        /// <summary>
        /// Occurs when a player consumes food
        /// </summary>
        public static event PlayerEatingEventHandler PlayerEating;
        /// <summary>
        /// Occurs after a player's karma has changed
        /// </summary>
        public static event PlayerKarmaChangeEventHandler PlayerKarmaChange;
        /// <summary>
        /// Occurs before a player takes a step
        /// </summary>
        public static event PlayerMoveEventHandler PlayerMove;
        /// <summary>
        /// Occurs after a player's base stat value has changed
        /// </summary>
        public static event PlayerRawStatChangeEventHandler PlayerRawStatChange;
        /// <summary>
        /// Occurs after a player's base skill value has changed
        /// </summary>
        public static event PlayerSkillChangedEventHandler PlayerSkillChanged;
        /// <summary>
        /// Occurs when a player tries to steal an item
        /// </summary>
        public static event PlayerStealEventHandler PlayerSteal;

        public static void InvokePlayerDamaged( PlayerDamagedEventArgs args )
        {
            if( PlayerDamaged != null )
                PlayerDamaged(args);
        }

        public static void InvokePlayerDismounted( PlayerEventArgs args )
        {
            if( PlayerDismounted != null )
                PlayerDismounted(args);
        }

        public static void InvokePlayerDrinking( PlayerDrinkingEventArgs args )
        {
            if( PlayerDrinking != null )
                PlayerDrinking(args);
        }

        public static void InvokePlayerEating( PlayerEatingEventArgs args )
        {
            if( PlayerEating != null )
                PlayerEating(args);
        }

        public static void InvokePlayerKarmaChange( PlayerKarmaChangeEventArgs args )
        {
            if( PlayerKarmaChange != null )
                PlayerKarmaChange(args);
        }

        public static void InvokePlayerMove( PlayerMoveEventArgs args )
        {
            if( PlayerMove != null )
                PlayerMove(args);
        }

        public static void InvokePlayerRawStatChange( PlayerRawStatChangeEventArgs args )
        {
            if( PlayerRawStatChange != null )
                PlayerRawStatChange(args);
        }

        public static void InvokePlayerSkillChanged( PlayerSkillChangedEventArgs args )
        {
            if( PlayerSkillChanged != null )
                PlayerSkillChanged(args);
        }

        public static void InvokePlayerSteal( PlayerStealEventArgs args )
        {
            if( PlayerSteal != null )
                PlayerSteal(args);
        }

        /// <summary>
        /// Occurs when a spell is cast
        /// </summary>
        public static event SpellCastEventHandler SpellCast;
        /// <summary>
        /// Occurs when a spell has failed
        /// </summary>
        public static event SpellFailedEventHandler SpellFailed;
        /// <summary>
        /// Occurs when a spell has been successfully cast
        /// </summary>
        public static event SpellFinishedEventHandler SpellFinished;

        public static void InvokeSpellCast( SpellCastEventArgs args )
        {
            if( SpellCast != null )
                SpellCast(args);
        }

        public static void InvokeSpellFailed( SpellEventArgs args )
        {
            if( SpellFailed != null )
                SpellFailed(args);
        }

        public static void InvokeSpellFinished( SpellEventArgs args )
        {
            if( SpellFinished != null )
                SpellFinished(args);
        }
    }
}