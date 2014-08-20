using System;
using System.Collections.Generic;
using System.IO;
using Server.Engines.Craft;
using Server.Events;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells;

namespace Server.EssenceOfCharacter
{
    public class EoCGenerator
    {
        private static string DataPath = "Saves/EoC";
        private static string DataFile = Path.Combine(DataPath, "eoc.bin");

        private static Dictionary<Player, EoCContext> EoCTable { get; set; }
        private static Dictionary<Player, HitsTimer> HitsTable { get; set; }

        public static void Configure()
        {
            EoCTable = new Dictionary<Player, EoCContext>();
            HitsTable = new Dictionary<Player, HitsTimer>();

            EventSink.WorldLoad += new WorldLoadEventHandler(OnWorldLoad);
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        public static void Initialize()
        {
            EventDispatcher.CorpseAction += new CorpseActionEventHandler(OnCorpseAction);

            EventDispatcher.CreatureDamaged += new CreatureDamagedEventHandler(OnCreatureDamaged);
            EventDispatcher.CreatureKilled += new CreatureKilledEventHandler(OnCreatureKilled);
            EventDispatcher.CreatureTameChange += new CreatureTameStateChangeHandler(OnCreatureTameChange);

            EventDispatcher.ItemCraft += new ItemCraftEventHandler(OnItemCraft);
            EventDispatcher.ItemPurchased += new ItemPurchasedEventHandler(OnItemPurchased);
            EventDispatcher.ItemSold += new ItemSoldEventHandler(AddOne);

            EventDispatcher.PlayerDamaged += new PlayerDamagedEventHandler(OnDamaged);
            EventDispatcher.PlayerDismounted += new PlayerDismountedEventHandler(AddOne);
            EventDispatcher.PlayerDrinking += new PlayerDrinkingEventHandler(AddOne);
            EventDispatcher.PlayerEating += new PlayerEatingEventHandler(AddOne);
            EventDispatcher.PlayerMove += new PlayerMoveEventHandler(OnMove);
            EventDispatcher.PlayerSkillChanged += new PlayerSkillChangedEventHandler(OnSkillChanged);
            EventDispatcher.PlayerSteal += new PlayerStealEventHandler(OnSteal);

            EventDispatcher.SpellFailed += new SpellFailedEventHandler(OnSpellEvent);
            EventDispatcher.SpellFinished += new SpellFinishedEventHandler(OnSpellEvent);

            EventSink.Login += new LoginEventHandler(OnLogin);
            EventSink.Logout += new LogoutEventHandler(OnLogout);
            EventSink.Speech += new SpeechEventHandler(OnSpeech);
            EventSink.SkillUsed += new SkillUsedEventHandler(OnSkillUsed);
        }

        /// <summary>
        /// Generic handler to add one EoC point
        /// </summary>
        private static void AddOne( PlayerEventArgs args )
        {
            args.Player.EssenceOfCharacter++;
        }

        /// <summary>
        /// Grants EoC for certain corpse actions, such as looting an item
        /// </summary>
        private static void OnCorpseAction( CorpseActionEventArgs args )
        {
            Corpse c = args.Corpse;
            Mobile corpseOwner = c.Owner;
            Player p = args.Player;

            if( c == null || corpseOwner == null || p == null )
                return;

            switch( args.Action )
            {
                case CorpseActionEventArgs.CorpseAction.Opened:
                    p.EssenceOfCharacter++;
                    EoCTable[p].OpenCorpses.Add(c);

                    break;
                case CorpseActionEventArgs.CorpseAction.Looted:
                    if( corpseOwner != null && corpseOwner.Player )
                    {
                        p.EssenceOfCharacter++;

                        if( EoCTable[p].OpenCorpses.Contains(c) )
                        {
                            EoCTable[p].OpenCorpses.Remove(c);
                        }
                    }

                    break;
                case CorpseActionEventArgs.CorpseAction.Closed:
                case CorpseActionEventArgs.CorpseAction.Deleted:
                    if( EoCTable[p].OpenCorpses.Contains(c) )
                    {
                        if( corpseOwner != null && corpseOwner.Player )
                        {
                            p.EssenceOfCharacter += 10;
                        }

                        EoCTable[p].OpenCorpses.Remove(c);
                    }

                    break;
                case CorpseActionEventArgs.CorpseAction.Carved:
                    if( corpseOwner != null && corpseOwner is BaseCreature )
                    {
                        p.EssenceOfCharacter++;
                    }

                    break;
            }
        }

        /// <summary>
        /// Grants EoC for damaging a <code>BaseCreature</code>
        /// </summary>
        private static void OnCreatureDamaged( CreatureDamagedEventArgs args )
        {
            if( args.Aggressor == null || !args.Aggressor.Player )
                return;

            Player p = (Player)args.Aggressor;

            p.EssenceOfCharacter += Math.Max(1, (args.DamageAmount / 5));
        }

        /// <summary>
        /// Grants EoC for killing a <code>BaseCreature</code>
        /// </summary>
        private static void OnCreatureKilled( CreatureKilledEventArgs args )
        {
            if( args.Creature == null || args.Creature.Fame == 0 )
                return;

            args.Player.EssenceOfCharacter += Math.Max(1, (args.Creature.Fame / 50));
        }

        /// <summary>
        /// Grants EoC when a <code>BaseCreature</code> is tamed or released
        /// </summary>
        private static void OnCreatureTameChange( CreatureTameStateChangeArgs args )
        {
            if( args.Player == null )
                return;

            args.Player.EssenceOfCharacter++;
        }

        /// <summary>
        /// Grants EoC for crafting items
        /// </summary>
        private static void OnItemCraft( ItemCraftEventArgs args )
        {
            CraftItem item = args.CraftItem;
            CraftSystem system = args.CraftSystem;
            Player p = args.Player;

            double minSkill = 1.0;

            for( int i = 0; i < item.Skills.Count; i++ )
            {
                if( item.Skills[i].SkillToMake != system.MainSkill )
                    continue;

                minSkill = item.Skills[i].MinSkill;
                break;
            }

            if( args.Success )
            {
                p.EssenceOfCharacter += (int)(minSkill / 10);
            }
            else
            {
                p.EssenceOfCharacter += (int)(minSkill / 5);
            }
        }

        /// <summary>
        /// Grants EoC for purchasing boats
        /// </summary>
        private static void OnItemPurchased( ItemPurchasedEventArgs args )
        {
            if( args.Entity is BaseBoat || args.Entity is BaseBoatDeed )
            {
                EoCTable[args.Player].Boats++;
            }
            else
            {
                args.Player.EssenceOfCharacter++;
            }
        }

        /// <summary>
        /// Grants EoC for:
        /// <para>getting damaged by a player or NPC</para>
        /// <para>getting knocked out</para>
        /// <para>knocking out another player</para>
        /// <para> </para>
        /// <para>Also starts the <code>HitsTable</code> to trigger &lt;5% HP EoC generation</para>
        /// </summary>
        private static void OnDamaged( PlayerDamagedEventArgs args )
        {
            Mobile aggr = args.Aggressor;
            Player p = args.Player;

            if( aggr != null )
            {
                if( p.Guild != null && !p.Guild.Disbanded && p.Guild == aggr.Guild )
                    return;

                if( p.Party != null && p.Party == aggr.Party )
                    return;

                p.EssenceOfCharacter += Math.Max(1, (args.DamageAmount / 10));

                if( args.WillKill )
                {
                    if( aggr is Player )
                    {
                        int change = (int)(p.SkillsTotal / aggr.SkillsTotal) * 10;

                        ((Player)aggr).EssenceOfCharacter += change;
                        p.EssenceOfCharacter += change;
                    }
                    else
                    {
                        int count = 0;

                        for( int i = 0; i < p.Aggressed.Count; i++ )
                        {
                            if( AttackMessage.CheckAggressions(p, p.Aggressed[i].Defender) )
                                count++;
                        }

                        for( int i = 0; i < p.Aggressors.Count; i++ )
                        {
                            if( AttackMessage.CheckAggressions(p, p.Aggressors[i].Attacker) )
                                count++;
                        }

                        p.EssenceOfCharacter += Math.Max(50, (50 * count));
                    }
                }
            }

            if( p.Hits < (int)(p.HitsMax * 0.05) )
            {
                if( !HitsTable.ContainsKey(p) )
                {
                    HitsTable[p] = new HitsTimer(p);
                    HitsTable[p].Start();
                }
                else if( !HitsTable[p].Running )
                {
                    HitsTable[p].Start();
                }
            }
            else if( HitsTable.ContainsKey(p) )
            {
                HitsTable[p].Stop();
                HitsTable.Remove(p);
            }
        }

        /// <summary>
        /// Starts pending generators on player re-login
        /// </summary>
        private static void OnLogin( LoginEventArgs args )
        {
            Player p = (Player)args.Mobile;

            if( !EoCTable.ContainsKey(p) )
            {
                EoCTable[p] = new EoCContext(p);
            }

            EoCTable[p].LastLogin = DateTime.Now;

            if( HitsTable.ContainsKey(p) )
            {
                HitsTable[p].Start();
            }
        }

        /// <summary>
        /// Stops any running generator processes
        /// </summary>
        private static void OnLogout( LogoutEventArgs args )
        {
            Player p = (Player)args.Mobile;

            if( HitsTable.ContainsKey(p) )
            {
                HitsTable[p].Stop();
            }
        }

        /// <summary>
        /// Grants EoC based on player movement
        /// </summary>
        private static void OnMove( PlayerMoveEventArgs args )
        {
            Player p = args.Player;

            if( p.Mounted )
            {
                EoCTable[p].MountedSteps++;
            }
            else if( (args.Direction & Direction.Running) != 0 )
            {
                EoCTable[p].WalkingSteps += 2;
            }
            else
            {
                EoCTable[p].WalkingSteps++;
            }
        }

        /// <summary>
        /// Grants EoC for speaking and emoting
        /// </summary>
        private static void OnSpeech( SpeechEventArgs args )
        {
            Player p = args.Mobile as Player;

            if( p != null && p.Map != null && p.Map != Map.Internal )
            {
                int playerCount = 0;
                IPooledEnumerable eable = p.Map.GetClientsInRange(p.Location);

                foreach( NetState ns in eable )
                {
                    if( ns.Mobile != null && ns.Mobile != p && ns.Mobile.CanSee(p) && ns.Mobile.Alive == p.Alive )
                    {
                        playerCount++;
                    }
                }

                eable.Free();

                if( args.Type == MessageType.Emote || args.Type == MessageType.Regular )
                {
                    p.EssenceOfCharacter += (1 * playerCount);
                }
            }
        }

        /// <summary>
        /// Grants EoC when a skill has changed
        /// </summary>
        private static void OnSkillChanged( PlayerSkillChangedEventArgs args )
        {
        }

        /// <summary>
        /// Grants EoC for using a skill
        /// </summary>
        private static void OnSkillUsed( SkillUsedEventArgs args )
        {
            if( args.Mobile.Player )
            {
                ((Player)args.Mobile).EssenceOfCharacter++;
            }
        }

        /// <summary>
        /// Grants EoC for casting spells
        /// </summary>
        private static void OnSpellEvent( SpellEventArgs args )
        {
            int eoc = 0;
            Spell spell = args.Spell;

            if( spell is MagerySpell )
            {
                eoc = (1 * ((int)((MagerySpell)spell).Circle + 1));
            }

            args.Player.EssenceOfCharacter += eoc;
        }

        /// <summary>
        /// Grants EoC when an item is stolen
        /// </summary>
        private static void OnSteal( PlayerStealEventArgs args )
        {
            if( args.StolenItem == null )
                return;

            Player p = args.Player;

            if( args.Caught )
            {
                p.EssenceOfCharacter += (10 * args.Witnesses.Count);
            }
            else
            {
                p.EssenceOfCharacter += (5 * args.Witnesses.Count);
            }
        }

        /// <summary>
        /// Loads serialized data
        /// </summary>
        private static void OnWorldLoad()
        {
            if( !File.Exists(DataFile) )
                return;

            using( FileStream stream = new FileStream(DataFile, FileMode.Open, FileAccess.Read, FileShare.Read) )
            {
                BinaryFileReader reader = new BinaryFileReader(new BinaryReader(stream));

                int tableCount = reader.ReadInt();
                EoCTable = new Dictionary<Player, EoCContext>(tableCount);

                for( int i = 0; i < tableCount; i++ )
                {
                    if( !reader.ReadBool() )
                        continue;

                    Player pl = reader.ReadMobile<Player>();

                    if( pl != null && !pl.Deleted )
                        EoCTable[pl] = new EoCContext(reader);
                }

                int hitsCount = reader.ReadInt();
                HitsTable = new Dictionary<Player, HitsTimer>(hitsCount);

                for( int i = 0; i < hitsCount; i++ )
                {
                    if( !reader.ReadBool() )
                        continue;

                    Player player = reader.ReadMobile<Player>();

                    if( player == null || player.Deleted )
                        continue;

                    HitsTable[player] = new HitsTimer(player);

                    DateTime next = reader.ReadDateTime();

                    if( next < DateTime.Now )
                        next = DateTime.Now;

                    HitsTable[player].Delay = (next - DateTime.Now);
                }

                reader.Close();
            }
        }

        /// <summary>
        /// Saves persistent data
        /// </summary>
        private static void OnWorldSave( WorldSaveEventArgs args )
        {
            if( !Directory.Exists(DataPath) )
                Directory.CreateDirectory(DataPath);

            BinaryFileWriter writer = new BinaryFileWriter(DataFile, true);

            writer.Write((int)EoCTable.Count);

            foreach( KeyValuePair<Player, EoCContext> kvp in EoCTable )
            {
                if( kvp.Key == null || kvp.Key.Deleted )
                {
                    writer.Write(false);
                }
                else
                {
                    writer.Write(true);

                    writer.WriteMobile<Player>(kvp.Key);
                    kvp.Value.Serialize(writer);
                }
            }

            writer.Write((int)HitsTable.Count);

            foreach( KeyValuePair<Player, HitsTimer> kvp in HitsTable )
            {
                if( kvp.Key != null && !kvp.Key.Deleted && kvp.Value.Running )
                {
                    writer.Write(true);

                    writer.WriteMobile<Player>(kvp.Key);
                    writer.Write(kvp.Value.Next);
                }
                else
                {
                    writer.Write(false);
                }
            }

            writer.Close();
        }

        private class HitsTimer : Timer
        {
            public Player Player { get; set; }

            public HitsTimer( Player player )
                : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
            {
                Player = player;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if( Player.Alive && Player.NetState != null && Player.Hits < (int)(Player.HitsMax * 0.05) )
                {
                    Player.EssenceOfCharacter++;
                }
                else
                {
                    Stop();
                }
            }
        }

        private class EoCContext
        {
            private int _boats;
            private DateTime _lastLogin;
            private int _mountedSteps;
            private int _walkingSteps;

            /// <summary>
            /// List of corpses the player has or had open
            /// </summary>
            public List<Corpse> OpenCorpses { get; set; }
            public Player Player { get; set; }

            /// <summary>
            /// Number of boats the player has purchased
            /// </summary>
            public int Boats
            {
                get { return _boats; }
                set
                {
                    _boats = value;

                    if( _boats == 1 )
                    {
                        Player.EssenceOfCharacter += 100;
                    }
                    else
                    {
                        Player.EssenceOfCharacter += 10;
                    }
                }
            }

            /// <summary>
            /// Timestamp of the player's last login
            /// </summary>
            public DateTime LastLogin
            {
                get { return _lastLogin; }
                set
                {
                    DateTime old = _lastLogin;
                    _lastLogin = value;

                    if( old <= (DateTime.Now - TimeSpan.FromHours(24)) )
                    {
                        Player.EssenceOfCharacter += 100;
                    }
                }
            }

            /// <summary>
            /// Number of steps taken while mounted
            /// </summary>
            public int MountedSteps
            {
                get { return _mountedSteps; }
                set
                {
                    _mountedSteps = value;
                    OnStepsChanged();
                }
            }

            /// <summary>
            /// Number of walking or running steps taken
            /// </summary>
            public int WalkingSteps
            {
                get { return _walkingSteps; }
                set
                {
                    _walkingSteps = value;
                    OnStepsChanged();
                }
            }

            /// <summary>
            /// ctor
            /// </summary>
            public EoCContext( Player player )
            {
                _mountedSteps = 0;
                _walkingSteps = 0;

                OpenCorpses = new List<Corpse>();
                Player = player;
            }

            /// <summary>
            /// Called when the steps have changed to automatically add EoC
            /// </summary>
            private void OnStepsChanged()
            {
                if( MountedSteps >= 12 )
                {
                    Player.EssenceOfCharacter++;
                    MountedSteps = 0;
                }

                if( WalkingSteps >= 24 )
                {
                    Player.EssenceOfCharacter++;
                    WalkingSteps = 0;
                }
            }

            /// <summary>
            /// Serializes the context data
            /// </summary>
            public void Serialize( GenericWriter writer )
            {
                writer.Write((int)0);

                writer.WriteMobile<Player>(Player);

                writer.Write(_boats);
                writer.Write(_lastLogin);
                writer.Write(_mountedSteps);
                writer.Write(_walkingSteps);
            }

            /// <summary>
            /// Deserializes the context data
            /// </summary>
            public EoCContext( GenericReader reader )
            {
                int version = reader.ReadInt();

                switch( version )
                {
                    case 0:
                        Player = reader.ReadMobile<Player>();

                        _boats = reader.ReadInt();
                        _lastLogin = reader.ReadDateTime();
                        _mountedSteps = reader.ReadInt();
                        _walkingSteps = reader.ReadInt();
                        break;
                }

                OpenCorpses = new List<Corpse>();
            }
        }
    }
}