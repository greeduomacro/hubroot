using System;
using Server.Items;

namespace Server
{
    public class Loot
    {
        #region List definitions
        private static Type[] m_AosWeaponTypes = new Type[]
			{
				typeof( Scythe ),				typeof( BoneHarvester ),		typeof( Scepter ),
				typeof( BladedStaff ),			typeof( Pike ),					typeof( DoubleBladedStaff ),
				typeof( Lance ),				typeof( CrescentBlade )
			};

        public static Type[] AosWeaponTypes { get { return m_AosWeaponTypes; } }

        private static Type[] m_WeaponTypes = new Type[]
			{
				typeof( Axe ),					typeof( BattleAxe ),			typeof( DoubleAxe ),
				typeof( ExecutionersAxe ),		typeof( Hatchet ),				typeof( LargeBattleAxe ),
				typeof( TwoHandedAxe ),			typeof( WarAxe ),				typeof( Club ),
				typeof( Mace ),					typeof( Maul ),					typeof( WarHammer ),
				typeof( WarMace ),				typeof( Bardiche ),				typeof( Halberd ),
				typeof( Spear ),				typeof( ShortSpear ),			typeof( Pitchfork ),
				typeof( WarFork ),				typeof( BlackStaff ),			typeof( GnarledStaff ),
				typeof( QuarterStaff ),			typeof( Broadsword ),			typeof( Cutlass ),
				typeof( Katana ),				typeof( Kryss ),				typeof( Longsword ),
				typeof( Scimitar ),				typeof( VikingSword ),			typeof( Pickaxe ),
				typeof( HammerPick ),			typeof( ButcherKnife ),			typeof( Cleaver ),
				typeof( Dagger ),				typeof( SkinningKnife ),		typeof( ShepherdsCrook )
			};

        public static Type[] WeaponTypes { get { return m_WeaponTypes; } }

        private static Type[] m_AosRangedWeaponTypes = new Type[]
			{
				typeof( CompositeBow ),			typeof( RepeatingCrossbow )
			};

        public static Type[] AosRangedWeaponTypes { get { return m_AosRangedWeaponTypes; } }

        private static Type[] m_RangedWeaponTypes = new Type[]
			{
				typeof( Bow ),					typeof( Crossbow ),				typeof( HeavyCrossbow )
			};

        public static Type[] RangedWeaponTypes { get { return m_RangedWeaponTypes; } }

        private static Type[] m_ArmorTypes = new Type[]
			{
				typeof( BoneArms ),				typeof( BoneChest ),			typeof( BoneGloves ),
				typeof( BoneLegs ),				typeof( BoneHelm ),				typeof( ChainChest ),
				typeof( ChainLegs ),			typeof( ChainCoif ),			typeof( Bascinet ),
				typeof( CloseHelm ),			typeof( Helmet ),				typeof( NorseHelm ),
				typeof( OrcHelm ),				typeof( FemaleLeatherChest ),	typeof( LeatherArms ),
				typeof( LeatherBustierArms ),	typeof( LeatherChest ),			typeof( LeatherGloves ),
				typeof( LeatherGorget ),		typeof( LeatherLegs ),			typeof( LeatherShorts ),
				typeof( LeatherSkirt ),			typeof( LeatherCap ),			typeof( FemalePlateChest ),
				typeof( PlateArms ),			typeof( PlateChest ),			typeof( PlateGloves ),
				typeof( PlateGorget ),			typeof( PlateHelm ),			typeof( PlateLegs ),
				typeof( RingmailArms ),			typeof( RingmailChest ),		typeof( RingmailGloves ),
				typeof( RingmailLegs ),			typeof( FemaleStuddedChest ),	typeof( StuddedArms ),
				typeof( StuddedBustierArms ),	typeof( StuddedChest ),			typeof( StuddedGloves ),
				typeof( StuddedGorget ),		typeof( StuddedLegs )
			};

        public static Type[] ArmorTypes { get { return m_ArmorTypes; } }

        private static Type[] m_AosShieldTypes = new Type[]
			{
				typeof( ChaosShield ),			typeof( OrderShield )
			};

        public static Type[] AosShieldTypes { get { return m_AosShieldTypes; } }

        private static Type[] m_ShieldTypes = new Type[]
			{
				typeof( BronzeShield ),			typeof( Buckler ),				typeof( HeaterShield ),
				typeof( MetalShield ),			typeof( MetalKiteShield ),		typeof( WoodenKiteShield ),
				typeof( WoodenShield )
			};

        public static Type[] ShieldTypes { get { return m_ShieldTypes; } }

        private static Type[] m_GemTypes = new Type[]
			{
				typeof( Amber ),				typeof( Amethyst ),				typeof( Citrine ),
				typeof( Diamond ),				typeof( Emerald ),				typeof( Ruby ),
				typeof( Sapphire ),				typeof( StarSapphire ),			typeof( Tourmaline )
			};

        public static Type[] GemTypes { get { return m_GemTypes; } }

        private static Type[] m_JewelryTypes = new Type[]
			{
				typeof( GoldRing ),				typeof( GoldBracelet ),
				typeof( SilverRing ),			typeof( SilverBracelet )
			};

        public static Type[] JewelryTypes { get { return m_JewelryTypes; } }

        private static Type[] m_RegTypes = new Type[]
			{
				typeof( BlackPearl ),			typeof( Bloodmoss ),			typeof( Garlic ),
				typeof( Ginseng ),				typeof( MandrakeRoot ),			typeof( Nightshade ),
				typeof( SulfurousAsh ),			typeof( SpidersSilk )
			};

        public static Type[] RegTypes { get { return m_RegTypes; } }

        private static Type[] m_PotionTypes = new Type[]
			{
				typeof( AgilityPotion ),		typeof( StrengthPotion ),		typeof( RefreshPotion ),
				typeof( LesserCurePotion ),		typeof( LesserHealPotion ),		typeof( LesserPoisonPotion )
			};

        public static Type[] PotionTypes { get { return m_PotionTypes; } }

        private static Type[] m_InstrumentTypes = new Type[]
			{
				typeof( Drums ),				typeof( Harp ),					typeof( LapHarp ),
				typeof( Lute ),					typeof( Tambourine ),			typeof( TambourineTassel )
			};

        public static Type[] InstrumentTypes { get { return m_InstrumentTypes; } }

        private static Type[] m_StatueTypes = new Type[]
		{
			typeof( StatueSouth ),			typeof( StatueSouth2 ),			typeof( StatueNorth ),
			typeof( StatueWest ),			typeof( StatueEast ),			typeof( StatueEast2 ),
			typeof( StatueSouthEast ),		typeof( BustSouth ),			typeof( BustEast )
		};

        public static Type[] StatueTypes { get { return m_StatueTypes; } }

        private static Type[] m_RegularScrollTypes = new Type[]
			{
				typeof( ClumsyScroll ),			typeof( CreateFoodScroll ),		typeof( FeeblemindScroll ),		typeof( HealScroll ),
				typeof( MagicArrowScroll ),		typeof( NightSightScroll ),		typeof( ReactiveArmorScroll ),	typeof( WeakenScroll ),
				typeof( AgilityScroll ),		typeof( CunningScroll ),		typeof( CureScroll ),			typeof( HarmScroll ),
				typeof( MagicTrapScroll ),		typeof( MagicUnTrapScroll ),	typeof( ProtectionScroll ),		typeof( StrengthScroll ),
				typeof( BlessScroll ),			typeof( FireballScroll ),		typeof( MagicLockScroll ),		typeof( PoisonScroll ),
				typeof( TelekinisisScroll ),	typeof( TeleportScroll ),		typeof( UnlockScroll ),			typeof( WallOfStoneScroll ),
				typeof( ArchCureScroll ),		typeof( ArchProtectionScroll ),	typeof( CurseScroll ),			typeof( FireFieldScroll ),
				typeof( GreaterHealScroll ),	typeof( LightningScroll ),		typeof( ManaDrainScroll ),		typeof( RecallScroll ),
				typeof( BladeSpiritsScroll ),	typeof( DispelFieldScroll ),	typeof( IncognitoScroll ),		typeof( MagicReflectScroll ),
				typeof( MindBlastScroll ),		typeof( ParalyzeScroll ),		typeof( PoisonFieldScroll ),	typeof( SummonCreatureScroll ),
				typeof( DispelScroll ),			typeof( EnergyBoltScroll ),		typeof( ExplosionScroll ),		typeof( InvisibilityScroll ),
				typeof( MarkScroll ),			typeof( MassCurseScroll ),		typeof( ParalyzeFieldScroll ),	typeof( RevealScroll ),
				typeof( ChainLightningScroll ), typeof( EnergyFieldScroll ),	typeof( FlamestrikeScroll ),	typeof( GateTravelScroll ),
				typeof( ManaVampireScroll ),	typeof( MassDispelScroll ),		typeof( MeteorSwarmScroll ),	typeof( PolymorphScroll ),
				typeof( EarthquakeScroll ),		typeof( EnergyVortexScroll ),	typeof( ResurrectionScroll ),	typeof( SummonAirElementalScroll ),
				typeof( SummonDaemonScroll ),	typeof( SummonEarthElementalScroll ),	typeof( SummonFireElementalScroll ),	typeof( SummonWaterElementalScroll )
			};

        public static Type[] RegularScrollTypes { get { return m_RegularScrollTypes; } }

        private static Type[] m_GrimmochJournalTypes = new Type[]
		{
			typeof( GrimmochJournal1 ),		typeof( GrimmochJournal2 ),		typeof( GrimmochJournal3 ),
			typeof( GrimmochJournal6 ),		typeof( GrimmochJournal7 ),		typeof( GrimmochJournal11 ),
			typeof( GrimmochJournal14 ),	typeof( GrimmochJournal17 ),	typeof( GrimmochJournal23 )
		};

        public static Type[] GrimmochJournalTypes { get { return m_GrimmochJournalTypes; } }

        private static Type[] m_LysanderNotebookTypes = new Type[]
		{
			typeof( LysanderNotebook1 ),		typeof( LysanderNotebook2 ),		typeof( LysanderNotebook3 ),
			typeof( LysanderNotebook7 ),		typeof( LysanderNotebook8 ),		typeof( LysanderNotebook11 )
		};

        public static Type[] LysanderNotebookTypes { get { return m_LysanderNotebookTypes; } }

        private static Type[] m_TavarasJournalTypes = new Type[]
		{
			typeof( TavarasJournal1 ),		typeof( TavarasJournal2 ),		typeof( TavarasJournal3 ),
			typeof( TavarasJournal6 ),		typeof( TavarasJournal7 ),		typeof( TavarasJournal8 ),
			typeof( TavarasJournal9 ),		typeof( TavarasJournal11 ),		typeof( TavarasJournal14 ),
			typeof( TavarasJournal16 ),		typeof( TavarasJournal16b ),	typeof( TavarasJournal17 ),
			typeof( TavarasJournal19 )
		};

        public static Type[] TavarasJournalTypes { get { return m_TavarasJournalTypes; } }


        private static Type[] m_WandTypes = new Type[]
			{
				typeof( ClumsyWand ),               typeof( FeebleWand ),           typeof( FireballWand ),
				typeof( GreaterHealWand ),          typeof( HarmWand ),             typeof( HealWand ),
				typeof( IDWand ),                   typeof( LightningWand ),        typeof( MagicArrowWand ),
				typeof( ManaDrainWand ),            typeof( WeaknessWand )          
                
			};
        public static Type[] WandTypes { get { return m_WandTypes; } }

        private static Type[] m_AosClothingTypes = new Type[]
			{
				typeof( FurSarong ),			typeof( FurCape ),				typeof( FlowerGarland ),
				typeof( GildedDress ),			typeof( FurBoots ),				typeof( FormalShirt ),
		};

        public static Type[] AosClothingTypes { get { return m_AosClothingTypes; } }

        private static Type[] m_ClothingTypes = new Type[]
			{
				typeof( Cloak ),				
				typeof( Bonnet ),               typeof( Cap ),		            typeof( FeatheredHat ),
				typeof( FloppyHat ),            typeof( JesterHat ),			typeof( Surcoat ),
				typeof( SkullCap ),             typeof( StrawHat ),	            typeof( TallStrawHat ),
				typeof( TricorneHat ),			typeof( WideBrimHat ),          typeof( WizardsHat ),
				typeof( BodySash ),             typeof( Doublet ),              typeof( Boots ),
				typeof( FullApron ),            typeof( JesterSuit ),           typeof( Sandals ),
				typeof( Tunic ),				typeof( Shoes ),				typeof( Shirt ),
				typeof( Kilt ),                 typeof( Skirt ),				typeof( FancyShirt ),
				typeof( FancyDress ),			typeof( ThighBoots ),			typeof( LongPants ),
				typeof( PlainDress ),           typeof( Robe ),					typeof( ShortPants ),
				typeof( HalfApron )
			};
        public static Type[] ClothingTypes { get { return m_ClothingTypes; } }

        private static Type[] m_AosHatTypes = new Type[]
			{
				typeof( FlowerGarland ),	typeof( BearMask ),		typeof( DeerMask )	//Are Bear& Deer mask inside the Pre-AoS loottables too?
			};

        public static Type[] AosHatTypes { get { return m_AosHatTypes; } }

        private static Type[] m_HatTypes = new Type[]
			{
				typeof( SkullCap ),			typeof( Bandana ),		typeof( FloppyHat ),
				typeof( Cap ),				typeof( WideBrimHat ),	typeof( StrawHat ),
				typeof( TallStrawHat ),		typeof( WizardsHat ),	typeof( Bonnet ),
				typeof( FeatheredHat ),		typeof( TricorneHat ),	typeof( JesterHat )
			};

        public static Type[] HatTypes { get { return m_HatTypes; } }

        private static Type[] m_LibraryBookTypes = new Type[]
			{
				typeof( GrammarOfOrcish ),		typeof( CallToAnarchy ),				typeof( ArmsAndWeaponsPrimer ),
				typeof( SongOfSamlethe ),		typeof( TaleOfThreeTribes ),			typeof( GuideToGuilds ),
				typeof( BirdsOfBritannia ),		typeof( BritannianFlora ),				typeof( ChildrenTalesVol2 ),
				typeof( TalesOfVesperVol1 ),	typeof( DeceitDungeonOfHorror ),		typeof( DimensionalTravel ),
				typeof( EthicalHedonism ),		typeof( MyStory ),						typeof( DiversityOfOurLand ),
				typeof( QuestOfVirtues ),		typeof( RegardingLlamas ),				typeof( TalkingToWisps ),
				typeof( TamingDragons ),		typeof( BoldStranger ),					typeof( BurningOfTrinsic ),
				typeof( TheFight ),				typeof( LifeOfATravellingMinstrel ),	typeof( MajorTradeAssociation ),
				typeof( RankingsOfTrades ),		typeof( WildGirlOfTheForest ),			typeof( TreatiseOnAlchemy ),
				typeof( VirtueBook )
			};

        public static Type[] LibraryBookTypes { get { return m_LibraryBookTypes; } }

        #endregion

        #region Accessors

        public static BaseWand RandomWand()
        {
            return Construct(m_WandTypes) as BaseWand;
        }

        public static BaseClothing RandomClothing()
        {
            if( Core.AOS )
                return Construct(m_AosClothingTypes, m_ClothingTypes) as BaseClothing;

            return Construct(m_ClothingTypes) as BaseClothing;
        }

        public static BaseWeapon RandomRangedWeapon()
        {
            if( Core.AOS )
                return Construct(m_AosRangedWeaponTypes, m_RangedWeaponTypes) as BaseWeapon;

            return Construct(m_RangedWeaponTypes) as BaseWeapon;
        }

        public static BaseWeapon RandomWeapon()
        {
            if( Core.AOS )
                return Construct(m_AosWeaponTypes, m_WeaponTypes) as BaseWeapon;

            return Construct(m_WeaponTypes) as BaseWeapon;
        }

        public static Item RandomWeaponOrJewelry()
        {
            if( Core.AOS )
                return Construct(m_AosWeaponTypes, m_WeaponTypes, m_JewelryTypes);

            return Construct(m_WeaponTypes, m_JewelryTypes);
        }

        public static BaseJewel RandomJewelry()
        {
            return Construct(m_JewelryTypes) as BaseJewel;
        }

        public static BaseArmor RandomArmor()
        {
            return Construct(m_ArmorTypes) as BaseArmor;
        }

        public static BaseHat RandomHat()
        {
            if( Core.AOS )
                return Construct(m_AosHatTypes, m_HatTypes) as BaseHat;

            return Construct(m_HatTypes) as BaseHat;
        }

        public static Item RandomArmorOrHat()
        {
            if( Core.AOS )
                return Construct(m_ArmorTypes, m_AosHatTypes, m_HatTypes);

            return Construct(m_ArmorTypes, m_HatTypes);
        }

        public static BaseShield RandomShield()
        {
            if( Core.AOS )
                return Construct(m_AosShieldTypes, m_ShieldTypes) as BaseShield;

            return Construct(m_ShieldTypes) as BaseShield;
        }

        public static BaseArmor RandomArmorOrShield()
        {
            if( Core.AOS )
                return Construct(m_ArmorTypes, m_AosShieldTypes, m_ShieldTypes) as BaseArmor;

            return Construct(m_ArmorTypes, m_ShieldTypes) as BaseArmor;
        }

        public static Item RandomArmorOrShieldOrJewelry()
        {
            if( Core.AOS )
                return Construct(m_ArmorTypes, m_AosHatTypes, m_HatTypes, m_AosShieldTypes, m_ShieldTypes, m_JewelryTypes);

            return Construct(m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes);
        }

        public static Item RandomArmorOrShieldOrWeapon()
        {
            if( Core.AOS )
                return Construct(m_AosWeaponTypes, m_WeaponTypes, m_AosRangedWeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_AosHatTypes, m_HatTypes, m_AosShieldTypes, m_ShieldTypes);

            return Construct(m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes);
        }

        public static Item RandomArmorOrShieldOrWeaponOrJewelry()
        {
            if( Core.AOS )
                return Construct(m_AosWeaponTypes, m_WeaponTypes, m_AosRangedWeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_AosHatTypes, m_HatTypes, m_AosShieldTypes, m_ShieldTypes, m_JewelryTypes);

            return Construct(m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes);
        }

        public static Item RandomGem()
        {
            return Construct(m_GemTypes);
        }

        public static Item RandomReagent()
        {
            return Construct(m_RegTypes);
        }

        public static Item RandomPossibleReagent()
        {
            return Construct(m_RegTypes);
        }

        public static Item RandomPotion()
        {
            return Construct(m_PotionTypes);
        }

        public static BaseInstrument RandomInstrument()
        {
            return Construct(m_InstrumentTypes) as BaseInstrument;
        }

        public static Item RandomStatue()
        {
            return Construct(m_StatueTypes);
        }

        public static SpellScroll RandomScroll( int minIndex, int maxIndex, SpellbookType type )
        {
            Type[] types;

            switch( type )
            {
                default:
                case SpellbookType.Regular: types = m_RegularScrollTypes; break;
            }

            return Construct(types, Utility.RandomMinMax(minIndex, maxIndex)) as SpellScroll;
        }

        public static BaseBook RandomGrimmochJournal()
        {
            return Construct(m_GrimmochJournalTypes) as BaseBook;
        }

        public static BaseBook RandomLysanderNotebook()
        {
            return Construct(m_LysanderNotebookTypes) as BaseBook;
        }

        public static BaseBook RandomTavarasJournal()
        {
            return Construct(m_TavarasJournalTypes) as BaseBook;
        }

        public static BaseBook RandomLibraryBook()
        {
            return Construct(m_LibraryBookTypes) as BaseBook;
        }
        #endregion

        #region Construction methods
        public static Item Construct( Type type )
        {
            try
            {
                return Activator.CreateInstance(type) as Item;
            }
            catch
            {
                return null;
            }
        }

        public static Item Construct( Type[] types )
        {
            if( types.Length > 0 )
                return Construct(types, Utility.Random(types.Length));

            return null;
        }

        public static Item Construct( Type[] types, int index )
        {
            if( index >= 0 && index < types.Length )
                return Construct(types[index]);

            return null;
        }

        public static Item Construct( params Type[][] types )
        {
            int totalLength = 0;

            for( int i = 0; i < types.Length; ++i )
                totalLength += types[i].Length;

            if( totalLength > 0 )
            {
                int index = Utility.Random(totalLength);

                for( int i = 0; i < types.Length; ++i )
                {
                    if( index >= 0 && index < types[i].Length )
                        return Construct(types[i][index]);

                    index -= types[i].Length;
                }
            }

            return null;
        }
        #endregion
    }
}