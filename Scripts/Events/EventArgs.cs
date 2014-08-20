using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Events
{
    public class PlayerEventArgs : EventArgs
    {
        public Player Player { get; private set; }

        public PlayerEventArgs( Player player )
        {
            Player = player;
        }
    }

    public class CorpseActionEventArgs : PlayerEventArgs
    {
        public enum CorpseAction
        {
            Carved,
            Channeled,
            Closed,
            Deleted,
            Looted,
            Opened
        }

        public CorpseAction Action { get; private set; }
        public Corpse Corpse { get; private set; }

        public CorpseActionEventArgs( Player player, Corpse c, CorpseAction action )
            : base(player)
        {
            Action = action;
            Corpse = c;
        }
    }

    public class CreatureDamagedEventArgs : EventArgs
    {
        public Mobile Aggressor { get; private set; }
        public BaseCreature Creature { get; private set; }
        public int DamageAmount { get; private set; }
        public bool WillKill { get; private set; }

        public CreatureDamagedEventArgs( BaseCreature bc, int amount, Mobile from, bool willKill )
        {
            Aggressor = from;
            Creature = bc;
            DamageAmount = amount;
            WillKill = willKill;
        }
    }

    public class CreatureKilledEventArgs : PlayerEventArgs
    {
        public BaseCreature Creature { get; private set; }

        public CreatureKilledEventArgs( Player player, BaseCreature bc )
            : base(player)
        {
            Creature = bc;
        }
    }

    public class CreatureTameStateChangeArgs : PlayerEventArgs
    {
        public BaseCreature Creature { get; private set; }
        public bool Tamed { get; private set; }

        public CreatureTameStateChangeArgs( Player player, BaseCreature bc, bool tamed )
            : base(player)
        {
            Creature = bc;
            Tamed = tamed;
        }
    }

    public class ItemCraftEventArgs : PlayerEventArgs
    {
        public CraftItem CraftItem { get; private set; }
        public CraftSystem CraftSystem { get; private set; }
        public bool Success { get; private set; }

        public ItemCraftEventArgs( Player player, bool success, CraftItem item, CraftSystem system )
            : base(player)
        {
            CraftItem = item;
            CraftSystem = system;
            Success = success;
        }
    }

    public class ItemPurchasedEventArgs : PlayerEventArgs
    {
        public IEntity Entity { get; private set; }

        public ItemPurchasedEventArgs( Player player, IEntity entity )
            : base(player)
        {
            Entity = entity;
        }
    }

    public class ItemSoldEventArgs : PlayerEventArgs
    {
        public IEntity Entity { get; private set; }

        public ItemSoldEventArgs( Player player, IEntity entity )
            : base(player)
        {
            Entity = entity;
        }
    }

    public class PlayerDamagedEventArgs : PlayerEventArgs
    {
        public int DamageAmount { get; private set; }
        public Mobile Aggressor { get; private set; }
        public bool WillKill { get; private set; }

        public PlayerDamagedEventArgs( Player player, int amount, Mobile from, bool willKill )
            : base(player)
        {
            DamageAmount = amount;
            Aggressor = from;
            WillKill = willKill;
        }
    }

    public class PlayerDrinkingEventArgs : PlayerEventArgs
    {
        public BaseBeverage BeverageItem { get; set; }

        public PlayerDrinkingEventArgs( Player player, BaseBeverage bvgItem )
            : base(player)
        {
            BeverageItem = bvgItem;
        }
    }

    public class PlayerEatingEventArgs : PlayerEventArgs
    {
        public Food FoodItem { get; set; }

        public PlayerEatingEventArgs( Player player, Food foodItem )
            : base(player)
        {
            FoodItem = foodItem;
        }
    }

    public class PlayerKarmaChangeEventArgs : PlayerEventArgs
    {
        public int OldValue { get; private set; }
        public int NewValue { get; private set; }

        public PlayerKarmaChangeEventArgs( Player player, int oldVal, int newVal )
            : base(player)
        {
            OldValue = oldVal;
            NewValue = newVal;
        }
    }

    public class PlayerMoveEventArgs : PlayerEventArgs
    {
        public Direction Direction { get; private set; }

        public PlayerMoveEventArgs( Player player, Direction dir )
            : base(player)
        {
            Direction = dir;
        }
    }

    public class PlayerRawStatChangeEventArgs : PlayerEventArgs
    {
        public StatType Type { get; private set; }
        public int OldValue { get; private set; }

        public PlayerRawStatChangeEventArgs( Player player, StatType type, int oldVal )
            : base(player)
        {
            Type = type;
            OldValue = oldVal;
        }
    }

    public class PlayerSkillChangedEventArgs : PlayerEventArgs
    {
        public double OldValue { get; private set; }
        public Skill Skill { get; private set; }

        public PlayerSkillChangedEventArgs( Player player, Skill skill, double oldValue )
            : base(player)
        {
            OldValue = oldValue;
            Skill = skill;
        }
    }

    public class PlayerStealEventArgs : PlayerEventArgs
    {
        public bool Caught { get; private set; }
        public Item StolenItem { get; private set; }
        public Mobile Victim { get; private set; }
        public List<Mobile> Witnesses { get; private set; }

        public PlayerStealEventArgs( Player player, Item stolenItem, Mobile victim, bool caught, List<Mobile> witnesses )
            : base(player)
        {
            Caught = caught;
            StolenItem = stolenItem;
            Victim = victim;
            Witnesses = witnesses;
        }
    }

    public class SpellEventArgs : PlayerEventArgs
    {
        public Spell Spell { get; private set; }

        public SpellEventArgs( Player player, Spell spell )
            : base(player)
        {
            Spell = spell;
        }
    }

    public class SpellCastEventArgs : SpellEventArgs
    {
        public bool Blocked { get; set; }

        public SpellCastEventArgs( Player player, Spell spell )
            : base(player, spell)
        {
        }
    }
}