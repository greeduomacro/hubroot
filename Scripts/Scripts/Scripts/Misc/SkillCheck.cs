using System;
using Server;
using Server.Mobiles;
using Server.Perks;

namespace Server.Misc
{
	public class SkillCheck
	{
		private const bool AntiMacroCode = true;		//Change this to false to disable anti-macro code

		public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes( 3.0 ); //How long do we remember targets/locations?
		public const int Allowance = 3;	//How many times may we use the same location/target for gain
		private const int LocationSize = 3; //The size of eeach location, make this smaller so players dont have to move as far
		private static bool[] UseAntiMacro = new bool[]
		{
			// true if this skill uses the anti-macro code, false if it does not
			true,// Alchemy = 0,
			true,// Anatomy = 1,
			true,// AnimalLore = 2,
			true,// ItemID = 3,
			true,// ArmsLore = 4,
			true,// Parry = 5,
			true,// Begging = 6,
			true,// Blacksmith = 7,
			true,// Fletching = 8,
			true,// Peacemaking = 9,
			true,// Camping = 10,
			true,// Carpentry = 11,
			true,// Cartography = 12,
			true,// Cooking = 13,
			true,// DetectHidden = 14,
			true,// Discordance = 15,
			true,// EvalInt = 16,
			true,// Healing = 17,
			true,// Fishing = 18,
			true,// Forensics = 19,
			true,// Herding = 20,
			true,// Hiding = 21,
			true,// Provocation = 22,
			true,// Inscribe = 23,
			true,// Lockpicking = 24,
			true,// Magery = 25,
			true,// MagicResist = 26,
			true,// Tactics = 27,
			true,// Snooping = 28,
			true,// Musicianship = 29,
			true,// Poisoning = 30,
			true,// Archery = 31,
			true,// SpiritSpeak = 32,
			true,// Stealing = 33,
			true,// Tailoring = 34,
			true,// AnimalTaming = 35,
			true,// TasteID = 36,
			true,// Tinkering = 37,
			true,// Tracking = 38,
			true,// Veterinary = 39,
			true,// Swords = 40,
			true,// Macing = 41,
			true,// Fencing = 42,
			true,// Wrestling = 43,
			true,// Lumberjacking = 44,
			true,// Mining = 45,
			true,// Meditation = 46,
			true,// Stealth = 47,
			true,// RemoveTrap = 48,
			true,// Necromancy = 49,
			true,// Focus = 50,
			true,// Chivalry = 51
			true,// Bushido = 52
			true,//Ninjitsu = 53
			true // Spellweaving
		};

		public static void Initialize()
		{
			Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler( Mobile_SkillCheckLocation );
			Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler( Mobile_SkillCheckDirectLocation );

			Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler( Mobile_SkillCheckTarget );
			Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler( Mobile_SkillCheckDirectTarget );
		}

		public static bool Mobile_SkillCheckLocation( Mobile from, SkillName skillName, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			double value = skill.Value;

			if( value < minSkill )
				return false; // Too difficult
			else if( value >= maxSkill )
				return true; // No challenge

			double chance = (value - minSkill) / (maxSkill - minSkill);

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool Mobile_SkillCheckDirectLocation( Mobile from, SkillName skillName, double chance )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			if( chance < 0.0 )
				return false; // Too difficult
			else if( chance >= 1.0 )
				return true; // No challenge

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool CheckSkill( Mobile from, Skill skill, object amObj, double chance )
		{
			if( from.Skills.Cap == 0 )
				return false;

			bool success = (chance >= Utility.RandomDouble());
			double gc = (double)(from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;
			gc += (skill.Cap - skill.Base) / skill.Cap;
			gc /= 5;

			gc += (1.0 - chance) * (success ? 0.5 : 0.2);
			gc /= 5;

            gc *= ((from.Hunger + from.Thirst) / 40);

			//gc *= skill.Info.GainFactor;

			if( gc < 0.01 )
				gc = 0.01;

            if (from is BaseCreature && ((BaseCreature)from).Controlled)
            {
                if (((BaseCreature)from).ControlMaster is Player)
                {
                    Player master = ((BaseCreature)from).ControlMaster as Player;
                    Beastmaster bmr = Perk.GetByType<Beastmaster>(master);

                    if (bmr != null && bmr.GuidingHand())
                    {
                        gc *= 3;
                    }
                }
            }

			if( from.Alive && ((gc >= Utility.RandomDouble() && AllowGain( from, skill, amObj )) || skill.Base < 10.0) )
				Gain( from, skill );

			return success;
		}

		public static bool Mobile_SkillCheckTarget( Mobile from, SkillName skillName, object target, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			double value = skill.Value;

			if( value < minSkill )
				return false; // Too difficult
			else if( value >= maxSkill )
				return true; // No challenge

			double chance = (value - minSkill) / (maxSkill - minSkill);

			return CheckSkill( from, skill, target, chance );
		}

		public static bool Mobile_SkillCheckDirectTarget( Mobile from, SkillName skillName, object target, double chance )
		{
			Skill skill = from.Skills[skillName];

			if( skill == null )
				return false;

			if( chance < 0.0 )
				return false; // Too difficult
			else if( chance >= 1.0 )
				return true; // No challenge

			return CheckSkill( from, skill, target, chance );
		}

		private static bool AllowGain( Mobile from, Skill skill, object obj )
		{
			if( AntiMacroCode && from is PlayerMobile && UseAntiMacro[skill.Info.SkillID] )
				return ((PlayerMobile)from).AntiMacroCheck( skill, obj );
			else
				return true;
		}

		public enum Stat { Str, Dex, Int }

		public static void Gain( Mobile from, Skill skill )
		{
			if( from.Region.IsPartOf( typeof( Regions.Jail ) ) )
				return;

			if( from is BaseCreature && ((BaseCreature)from).IsDeadPet )
				return;


			if( skill.Base < skill.Cap && skill.Lock == SkillLock.Up )
			{
				int toGain = 1;

				Skills skills = from.Skills;

				if( (skills.Total / skills.Cap) >= Utility.RandomDouble() )//( skills.Total >= skills.Cap )
				{
					for( int i = 0; i < skills.Length; ++i )
					{
						Skill toLower = skills[i];

						if( toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain )
						{
							toLower.BaseFixedPoint -= toGain;
							break;
						}
					}
				}

				if( (skills.Total + toGain) <= skills.Cap )
				{
					skill.BaseFixedPoint += toGain;
				}
			}

			if( skill.Lock == SkillLock.Up )
			{
				SkillInfo info = skill.Info;

				if( from.StrLock == StatLockType.Up && (info.StrGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Str );
				else if( from.DexLock == StatLockType.Up && (info.DexGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Dex );
				else if( from.IntLock == StatLockType.Up && (info.IntGain / 33.3) > Utility.RandomDouble() )
					GainStat( from, Stat.Int );
			}
		}

		public static bool CanLower( Mobile from, Stat stat )
		{
			switch( stat )
			{
				case Stat.Str: return (from.StrLock == StatLockType.Down && from.RawStr > 25);
				case Stat.Dex: return (from.DexLock == StatLockType.Down && from.RawDex > 25);
				case Stat.Int: return (from.IntLock == StatLockType.Down && from.RawInt > 25);
			}

			return false;
		}

		public static bool CanRaise( Mobile from, Stat stat )
		{
			if( !(from is BaseCreature && ((BaseCreature)from).Controlled) )
			{
				if( from.RawStatTotal >= from.StatCap )
					return false;
			}

            int strCap = 125;
            int dexCap = 125;
            int intCap = 125;

            if (from is Player)
            {
                switch (((Player)from).Race)
                {
                    case Race.Ogre:
                        {
                            strCap = 250;
                            dexCap = 100;
                            intCap = 100;
                        }
                        break;

                    case Race.Terathan:
                         {
                             strCap = 200;
                             dexCap = 200;
                             intCap = 150;
                         }
                         break;

                    case Race.Liche:
                         {
                             strCap = 150;
                             dexCap = 100;
                             intCap = 250;
                         }
                         break;

                    case Race.HalfDaemon:
                         {
                            strCap = 300;
                            dexCap = 150;
                            intCap = 150;
                         }
                         break;

                    case Race.Shapeshifter:
                         {
                             strCap = 150;
                             dexCap = 250;
                             intCap = 200;
                         }
                         break;

                    case Race.Elemental:
                         {
                             strCap = 150;
                             dexCap = 150;
                             intCap = 300;
                         }
                         break;

                    default: break;
                }
            }

			switch( stat )
			{
				case Stat.Str: return (from.StrLock == StatLockType.Up && from.RawStr < strCap);
				case Stat.Dex: return (from.DexLock == StatLockType.Up && from.RawDex < dexCap);
				case Stat.Int: return (from.IntLock == StatLockType.Up && from.RawInt < intCap);
			}

			return false;
		}

		public static void IncreaseStat( Mobile from, Stat stat, bool atrophy )
		{
			atrophy = atrophy || (from.RawStatTotal >= from.StatCap);

			switch( stat )
			{
				case Stat.Str:
					{
						if( atrophy )
						{
							if( CanLower( from, Stat.Dex ) && (from.RawDex < from.RawInt || !CanLower( from, Stat.Int )) )
								--from.RawDex;
							else if( CanLower( from, Stat.Int ) )
								--from.RawInt;
						}

						if( CanRaise( from, Stat.Str ) )
							++from.RawStr;

						break;
					}
				case Stat.Dex:
					{
						if( atrophy )
						{
							if( CanLower( from, Stat.Str ) && (from.RawStr < from.RawInt || !CanLower( from, Stat.Int )) )
								--from.RawStr;
							else if( CanLower( from, Stat.Int ) )
								--from.RawInt;
						}

						if( CanRaise( from, Stat.Dex ) )
							++from.RawDex;

						break;
					}
				case Stat.Int:
					{
						if( atrophy )
						{
							if( CanLower( from, Stat.Str ) && (from.RawStr < from.RawDex || !CanLower( from, Stat.Dex )) )
								--from.RawStr;
							else if( CanLower( from, Stat.Dex ) )
								--from.RawDex;
						}

						if( CanRaise( from, Stat.Int ) )
							++from.RawInt;

						break;
					}
			}
		}

		private static TimeSpan m_StatGainDelay = TimeSpan.FromMinutes( 1.5 );

		public static void GainStat( Mobile from, Stat stat )
		{
			TimeSpan delay = m_StatGainDelay;

			Region reg = Region.Find( from.Location, from.Map );

			switch( stat )
			{
				case Stat.Str:
					{
						if( (from.LastStrGain + delay) >= DateTime.Now )
							return;

						from.LastStrGain = DateTime.Now;
						break;
					}
				case Stat.Dex:
					{
						if( (from.LastDexGain + delay) >= DateTime.Now )
							return;

						from.LastDexGain = DateTime.Now;
						break;
					}
				case Stat.Int:
					{
						if( (from.LastIntGain + delay) >= DateTime.Now )
							return;

						from.LastIntGain = DateTime.Now;
						break;
					}
			}

			bool atrophy = ((from.RawStatTotal / (double)from.StatCap) >= Utility.RandomDouble());

			IncreaseStat( from, stat, atrophy );
		}
	}
}