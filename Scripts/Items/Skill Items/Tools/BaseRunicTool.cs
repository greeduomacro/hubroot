using System;
using System.Collections;

namespace Server.Items
{
	public abstract class BaseRunicTool : BaseTool
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; Hue = CraftResources.GetHue( m_Resource ); InvalidateProperties(); }
		}

		public BaseRunicTool( CraftResource resource, int itemID )
			: base( itemID )
		{
			m_Resource = resource;
		}

		public BaseRunicTool( CraftResource resource, int uses, int itemID )
			: base( uses, itemID )
		{
			m_Resource = resource;
		}

		public BaseRunicTool( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
			writer.Write( (int)m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						m_Resource = (CraftResource)reader.ReadInt();
						break;
					}
			}
		}

		private static bool m_IsRunicTool;
		private static int m_LuckChance;

		private static int Scale( int min, int max, int low, int high )
		{
			int percent;

			if( m_IsRunicTool )
			{
				percent = Utility.RandomMinMax( min, max );
			}
			else
			{
				// Behold, the worst system ever!
				int v = Utility.RandomMinMax( 0, 10000 );

				v = (int)Math.Sqrt( v );
				v = 100 - v;

				if( LootPack.CheckLuck( m_LuckChance ) )
					v += 10;

				if( v < min )
					v = min;
				else if( v > max )
					v = max;

				percent = v;
			}

			int scaledBy = Math.Abs( high - low ) + 1;

			if( scaledBy != 0 )
				scaledBy = 10000 / scaledBy;

			percent *= (10000 + scaledBy);

			return low + (((high - low) * percent) / 1000001);
		}

		private static void ApplyAttribute( AosAttributes attrs, int min, int max, AosAttribute attr, int low, int high )
		{
			ApplyAttribute( attrs, min, max, attr, low, high, 1 );
		}

		private static void ApplyAttribute( AosAttributes attrs, int min, int max, AosAttribute attr, int low, int high, int scale )
		{
			if( attr == AosAttribute.CastSpeed )
				attrs[attr] += Scale( min, max, low / scale, high / scale ) * scale;
			else
				attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;

			//if( attr == AosAttribute.SpellChanneling )
				//attrs[AosAttribute.CastSpeed] -= 1;
		}

		private static void ApplyAttribute( AosArmorAttributes attrs, int min, int max, AosArmorAttribute attr, int low, int high )
		{
			attrs[attr] = Scale( min, max, low, high );
		}

		private static void ApplyAttribute( AosArmorAttributes attrs, int min, int max, AosArmorAttribute attr, int low, int high, int scale )
		{
			attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static void ApplyAttribute( AosWeaponAttributes attrs, int min, int max, AosWeaponAttribute attr, int low, int high )
		{
			attrs[attr] = Scale( min, max, low, high );
		}

		private static void ApplyAttribute( AosWeaponAttributes attrs, int min, int max, AosWeaponAttribute attr, int low, int high, int scale )
		{
			attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static void ApplyAttribute( AosElementAttributes attrs, int min, int max, AosElementAttribute attr, int low, int high )
		{
			attrs[attr] = Scale( min, max, low, high );
		}

		private static void ApplyAttribute( AosElementAttributes attrs, int min, int max, AosElementAttribute attr, int low, int high, int scale )
		{
			attrs[attr] = Scale( min, max, low / scale, high / scale ) * scale;
		}

		private static SkillName[] m_PossibleBonusSkills = new SkillName[]
			{
				SkillName.Swords,
				SkillName.Fencing,
				SkillName.Macing,
				SkillName.Archery,
				SkillName.Wrestling,
				SkillName.Parry,
				SkillName.Tactics,
				SkillName.Anatomy,
				SkillName.Healing,
				SkillName.Magery,
				SkillName.Meditation,
				SkillName.EvalInt,
				SkillName.MagicResist,
				SkillName.AnimalTaming,
				SkillName.AnimalLore,
				SkillName.Veterinary,
				SkillName.Musicianship,
				SkillName.Provocation,
				SkillName.Discordance,
				SkillName.Peacemaking,
				SkillName.Focus,
				SkillName.Stealing,
				SkillName.Stealth,
				SkillName.SpiritSpeak 
			};

		private static SkillName[] m_PossibleSpellbookSkills = new SkillName[]
			{
				SkillName.Magery,
				SkillName.Meditation,
				SkillName.EvalInt,
				SkillName.MagicResist
			};

		private static void ApplySkillBonus( AosSkillBonuses attrs, int min, int max, int index, int low, int high )
		{
			SkillName[] possibleSkills = (attrs.Owner is Spellbook ? m_PossibleSpellbookSkills : m_PossibleBonusSkills);
			int count = (Core.SE ? possibleSkills.Length : possibleSkills.Length - 2);

			SkillName sk, check;
			double bonus;
			bool found;

			do
			{
				found = false;
				sk = possibleSkills[Utility.Random( count )];

				for( int i = 0; !found && i < 5; ++i )
					found = (attrs.GetValues( i, out check, out bonus ) && check == sk);
			} while( found );

			attrs.SetValues( index, sk, Scale( min, max, low, high ) );
		}

		private static void ApplyResistance( BaseArmor ar, int min, int max, ResistanceType res, int low, int high )
		{
			switch( res )
			{
				case ResistanceType.Physical: ar.PhysicalBonus += Scale( min, max, low, high ); break;
				case ResistanceType.Fire: ar.FireBonus += Scale( min, max, low, high ); break;
				case ResistanceType.Cold: ar.ColdBonus += Scale( min, max, low, high ); break;
				case ResistanceType.Poison: ar.PoisonBonus += Scale( min, max, low, high ); break;
				case ResistanceType.Energy: ar.EnergyBonus += Scale( min, max, low, high ); break;
			}
		}

		private const int MaxProperties = 32;
		private static BitArray m_Props = new BitArray( MaxProperties );
		private static int[] m_Possible = new int[MaxProperties];

		public static int GetUniqueRandom( int count )
		{
			int avail = 0;

			for( int i = 0; i < count; ++i )
			{
				if( !m_Props[i] )
					m_Possible[avail++] = i;
			}

			if( avail == 0 )
				return -1;

			int v = m_Possible[Utility.Random( avail )];

			m_Props.Set( v, true );

			return v;
		}

		public void ApplyAttributesTo( BaseWeapon weapon )
		{
			CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

			if( resInfo == null )
				return;

			CraftAttributeInfo attrs = resInfo.AttributeInfo;

			if( attrs == null )
				return;

			int attributeCount = Utility.RandomMinMax( attrs.RunicMinAttributes, attrs.RunicMaxAttributes );
			int min = attrs.RunicMinIntensity;
			int max = attrs.RunicMaxIntensity;

			ApplyAttributesTo( weapon, true, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseWeapon weapon, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( weapon, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseWeapon weapon, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			AosAttributes primary = weapon.Attributes;
			AosWeaponAttributes secondary = weapon.WeaponAttributes;

			m_Props.SetAll( false );

			if( weapon is BaseRanged )
				m_Props.Set( 2, true ); // ranged weapons cannot be ubws or mageweapon

			for( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 24 );

				if( random == -1 )
					break;

				switch( random )
				{
					case 0:
						{
							switch( Utility.Random( 5 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitPhysicalArea, 25, 100, 2 ); break;
								case 1: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitFireArea, 25, 100, 2 ); break;
								case 2: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitColdArea, 25, 100, 2 ); break;
								case 3: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitPoisonArea, 25, 100, 2 ); break;
								case 4: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitEnergyArea, 25, 100, 2 ); break;
							}

							break;
						}
					case 1:
						{
							switch( Utility.Random( 4 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitMagicArrow, 2, 50, 2 ); break;
								case 1: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitHarm, 2, 50, 2 ); break;
								case 2: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitFireball, 2, 50, 2 ); break;
								case 3: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLightning, 2, 50, 2 ); break;
							}

							break;
						}
					/*case 2:
						{
							switch( Utility.Random( 2 ) )
							{
								case 0: ApplyAttribute( secondary, min, max, AosWeaponAttribute.UseBestSkill, 1, 1 ); break;
								case 1: ApplyAttribute( secondary, min, max, AosWeaponAttribute.MageWeapon, 1, 10 ); break;
							}

							break;
						}*/
					case 3: ApplyAttribute( primary, min, max, AosAttribute.WeaponDamage, 25, 100 ); break;
					case 4: ApplyAttribute( primary, min, max, AosAttribute.DefendChance, 5, 35 ); break;
					//case 5: ApplyAttribute( primary, min, max, AosAttribute.CastSpeed, 1, 1 ); break;
					case 6: ApplyAttribute( primary, min, max, AosAttribute.AttackChance, 5, 35 ); break;
					//case 7: ApplyAttribute( primary, min, max, AosAttribute.Luck, 1, 100 ); break;
					case 8: ApplyAttribute( primary, min, max, AosAttribute.WeaponSpeed, 5, 50, 5 ); break;
					case 10: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitDispel, 2, 50, 2 ); break;
					case 11: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLeechHits, 2, 50, 2 ); break;
					case 12: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLowerAttack, 2, 50, 2 ); break;
					case 13: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLowerDefend, 2, 50, 2 ); break;
					case 14: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLeechMana, 2, 50, 2 ); break;
					case 15: ApplyAttribute( secondary, min, max, AosWeaponAttribute.HitLeechStam, 2, 50, 2 ); break;
					//case 16: ApplyAttribute( secondary, min, max, AosWeaponAttribute.LowerStatReq, 10, 100, 10 ); break;
					//case 17: ApplyAttribute( secondary, min, max, AosWeaponAttribute.ResistPhysicalBonus, 1, 15 ); break;
					//case 18: ApplyAttribute( secondary, min, max, AosWeaponAttribute.ResistFireBonus, 1, 15 ); break;
					//case 19: ApplyAttribute( secondary, min, max, AosWeaponAttribute.ResistColdBonus, 1, 15 ); break;
					//case 20: ApplyAttribute( secondary, min, max, AosWeaponAttribute.ResistPoisonBonus, 1, 15 ); break;
					//case 21: ApplyAttribute( secondary, min, max, AosWeaponAttribute.ResistEnergyBonus, 1, 15 ); break;
					//case 22: ApplyAttribute( secondary, min, max, AosWeaponAttribute.DurabilityBonus, 10, 100, 10 ); break;
					//case 23: weapon.Slayer = GetRandomSlayer(); break;
				}
			}
		}

		public static SlayerName GetRandomSlayer()
		{
			// TODO: Check random algorithm on OSI

			SlayerGroup[] groups = SlayerGroup.Groups;

			if( groups.Length == 0 )
				return SlayerName.None;

			SlayerGroup group = groups[Utility.Random( groups.Length - 1 )]; //-1 To Exclude the Fey Slayer which appears ONLY on a certain artifact.
			SlayerEntry entry;

			if( 10 > Utility.Random( 100 ) ) // 10% chance to do super slayer
			{
				entry = group.Super;
			}
			else
			{
				SlayerEntry[] entries = group.Entries;

				if( entries.Length == 0 )
					return SlayerName.None;

				entry = entries[Utility.Random( entries.Length )];
			}

			return entry.Name;
		}

		public void ApplyAttributesTo( BaseArmor armor )
		{
			CraftResourceInfo resInfo = CraftResources.GetInfo( m_Resource );

			if( resInfo == null )
				return;

			CraftAttributeInfo attrs = resInfo.AttributeInfo;

			if( attrs == null )
				return;

			int attributeCount = Utility.RandomMinMax( attrs.RunicMinAttributes, attrs.RunicMaxAttributes );
			int min = attrs.RunicMinIntensity;
			int max = attrs.RunicMaxIntensity;

			ApplyAttributesTo( armor, true, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseArmor armor, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( armor, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseArmor armor, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			AosAttributes primary = armor.Attributes;
			AosArmorAttributes secondary = armor.ArmorAttributes;

			m_Props.SetAll( false );

			bool isShield = (armor is BaseShield);
			int baseCount = (isShield ? 7 : 20);
			int baseOffset = (isShield ? 0 : 4);

			if( !isShield && armor.MeditationAllowance == ArmorMeditationAllowance.All )
				m_Props.Set( 3, true ); // remove mage armor from possible properties

			for( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( baseCount );

				if( random == -1 )
					break;

				random += baseOffset;

				switch( random )
				{
					/* Begin Sheilds */
					//case 0: ApplyAttribute( primary, min, max, AosAttribute.SpellChanneling, 1, 1 ); break;
					case 1: ApplyAttribute( primary, min, max, AosAttribute.DefendChance, 5, 25 ); break;
					case 2: ApplyAttribute( primary, min, max, AosAttribute.AttackChance, 5, 10 ); break;
					//case 3: ApplyAttribute( primary, min, max, AosAttribute.CastSpeed, 1, 1 ); break;
					/* Begin Armor */
					//case 4: ApplyAttribute( secondary, min, max, AosArmorAttribute.LowerStatReq, 10, 100, 10 ); break;
					//case 5: ApplyAttribute( secondary, min, max, AosArmorAttribute.SelfRepair, 1, 5 ); break;
					//case 6: ApplyAttribute( secondary, min, max, AosArmorAttribute.DurabilityBonus, 10, 100, 10 ); break;
					/* End Shields */
					//case 7: ApplyAttribute( secondary, min, max, AosArmorAttribute.MageArmor, 1, 1 ); break;
					case 8: ApplyAttribute( primary, min, max, AosAttribute.BonusHits, 3, 10 ); break;
					case 9: ApplyAttribute( primary, min, max, AosAttribute.BonusStam, 3, 10 ); break;
					case 10: ApplyAttribute( primary, min, max, AosAttribute.BonusMana, 3, 10 ); break;
					//case 11: ApplyAttribute( primary, min, max, AosAttribute.NightSight, 1, 1 ); break;
					case 12: ApplyAttribute( primary, min, max, AosAttribute.BonusHits, 3, 10 ); break;
					case 13: ApplyAttribute( primary, min, max, AosAttribute.BonusStam, 3, 10 ); break;
					case 14: ApplyAttribute( primary, min, max, AosAttribute.BonusMana, 3, 10 ); break;
					case 15: ApplyAttribute( primary, min, max, AosAttribute.LowerManaCost, 4, 8 ); break;
					//case 16: ApplyAttribute( primary, min, max, AosAttribute.LowerRegCost, 1, 20 ); break;
					//case 17: ApplyAttribute( primary,	min, max, AosAttribute.Luck, 1, 100 ); break;
					//case 18: ApplyAttribute( primary, min, max, AosAttribute.ReflectPhysical, 1, 15 ); break;
					case 19: ApplyResistance( armor, min, max, ResistanceType.Physical, 13, 15 ); break;
					case 20: ApplyResistance( armor, min, max, ResistanceType.Fire, 3, 15 ); break;
					case 21: ApplyResistance( armor, min, max, ResistanceType.Cold, 3, 15 ); break;
					case 22: ApplyResistance( armor, min, max, ResistanceType.Poison, 3, 15 ); break;
					case 23: ApplyResistance( armor, min, max, ResistanceType.Energy, 3, 15 ); break;
					/* End Armor */
				}
			}
		}

		public static void ApplyAttributesTo( BaseHat hat, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( hat, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseHat hat, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			AosAttributes primary = hat.Attributes;
			AosArmorAttributes secondary = hat.ClothingAttributes;
			AosElementAttributes resists = hat.Resistances;

			m_Props.SetAll( false );

			for( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 19 );

				if( random == -1 )
					break;

				switch( random )
				{
					//case 0: ApplyAttribute( primary, min, max, AosAttribute.ReflectPhysical, 1, 15 ); break;
					case 1: ApplyAttribute( primary, min, max, AosAttribute.BonusHits, 3, 8 ); break;
					case 2: ApplyAttribute( primary, min, max, AosAttribute.BonusStam, 3, 8 ); break;
					case 3: ApplyAttribute( primary, min, max, AosAttribute.BonusMana, 3, 8 ); break;
					//case 4: ApplyAttribute( primary, min, max, AosAttribute.NightSight, 1, 1 ); break;
					case 5: ApplyAttribute( primary, min, max, AosAttribute.BonusHits, 3, 8 ); break;
					case 6: ApplyAttribute( primary, min, max, AosAttribute.BonusStam, 3, 8 ); break;
					case 7: ApplyAttribute( primary, min, max, AosAttribute.BonusMana, 3, 8 ); break;
					case 8: ApplyAttribute( primary, min, max, AosAttribute.LowerManaCost, 3, 8 ); break;
					//case 9: ApplyAttribute( primary, min, max, AosAttribute.LowerRegCost, 1, 20 ); break;
					//case 10: ApplyAttribute( primary,	min, max, AosAttribute.Luck, 1, 100 ); break;
					//case 11: //ApplyAttribute( secondary,	min, max, AosArmorAttribute.LowerStatReq,		10, 100, 10 ); break;
					//case 12: //ApplyAttribute( secondary,	min, max, AosArmorAttribute.SelfRepair,			1, 5 ); break;
					//case 13: ApplyAttribute( secondary, min, max, AosArmorAttribute.DurabilityBonus, 10, 100, 10 ); break;
					case 14: ApplyAttribute( resists, min, max, AosElementAttribute.Physical, 3, 15 ); break;
					case 15: ApplyAttribute( resists, min, max, AosElementAttribute.Fire, 3, 15 ); break;
					case 16: ApplyAttribute( resists, min, max, AosElementAttribute.Cold, 3, 15 ); break;
					case 17: ApplyAttribute( resists, min, max, AosElementAttribute.Poison, 3, 15 ); break;
					case 18: ApplyAttribute( resists, min, max, AosElementAttribute.Energy, 3, 15 ); break;
				}
			}
		}

		public static void ApplyAttributesTo( BaseJewel jewelry, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( jewelry, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( BaseJewel jewelry, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			AosElementAttributes resists = jewelry.Resistances;
			AosSkillBonuses skills = jewelry.SkillBonuses;

            AosAttributes primary = jewelry.Attributes;


			m_Props.SetAll( false );

			for( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 18 );

				if( random == -1 )
					break;

				switch( random )
				{
					case 0: ApplyAttribute( resists, min, max, AosElementAttribute.Physical, 3, 15 ); break;
					case 1: ApplyAttribute( resists, min, max, AosElementAttribute.Fire, 3, 15 ); break;
					case 2: ApplyAttribute( resists, min, max, AosElementAttribute.Cold, 3, 15 ); break;
					case 3: ApplyAttribute( resists, min, max, AosElementAttribute.Poison, 3, 15 ); break;
					case 4: ApplyAttribute( resists, min, max, AosElementAttribute.Energy, 3, 15 ); break;
					//case 5: ApplySkillBonus( skills, min, max, 0, 1, 15 ); break;
					//case 6: ApplySkillBonus( skills, min, max, 1, 1, 15 ); break;
					//case 7: ApplySkillBonus( skills, min, max, 2, 1, 15 ); break;
					//case 8: ApplySkillBonus( skills, min, max, 3, 1, 15 ); break;
					//case 9: ApplySkillBonus( skills, min, max, 4, 1, 15 ); break;
                    case 10: ApplyAttribute(primary, min, max, AosAttribute.BonusHits, 3, 10); break;
                    case 12: ApplyAttribute(primary, min, max, AosAttribute.BonusStam, 3, 10); break;
                    case 13: ApplyAttribute(primary, min, max, AosAttribute.BonusMana, 3, 10); break;
                    case 15: ApplyAttribute(primary, min, max, AosAttribute.BonusHits, 3, 8); break;
                    case 16: ApplyAttribute(primary, min, max, AosAttribute.BonusStam, 3, 8); break;
                    case 17: ApplyAttribute(primary, min, max, AosAttribute.BonusMana, 3, 8); break;
                    case 18: ApplyAttribute(primary, min, max, AosAttribute.LowerManaCost, 4, 10); break;
				}
			}
		}

		public static void ApplyAttributesTo( Spellbook spellbook, int attributeCount, int min, int max )
		{
			ApplyAttributesTo( spellbook, false, 0, attributeCount, min, max );
		}

		public static void ApplyAttributesTo( Spellbook spellbook, bool isRunicTool, int luckChance, int attributeCount, int min, int max )
		{
			m_IsRunicTool = isRunicTool;
			m_LuckChance = luckChance;

			AosAttributes primary = spellbook.Attributes;
			AosSkillBonuses skills = spellbook.SkillBonuses;

			m_Props.SetAll( false );

			for( int i = 0; i < attributeCount; ++i )
			{
				int random = GetUniqueRandom( 16 );

				if( random == -1 )
					break;

				switch( random )
				{
					//case 0:
					//case 1:
					//case 2:
					case 3:
						{
							ApplyAttribute( primary, min, max, AosAttribute.BonusInt, 4, 8 );

							for( int j = 0; j < 4; ++j )
								m_Props.Set( j, true );

							break;
						}
					case 4: ApplyAttribute( primary, min, max, AosAttribute.BonusMana, 1, 8 ); break;
					case 5: ApplyAttribute( primary, min, max, AosAttribute.CastSpeed, 1, 1 ); break;
					case 6: ApplyAttribute( primary, min, max, AosAttribute.CastRecovery, 1, 3 ); break;
					case 7: ApplyAttribute( primary, min, max, AosAttribute.SpellDamage, 1, 12 ); break;
					case 8: ApplySkillBonus( skills, min, max, 0, 1, 15 ); break;
					case 9: ApplySkillBonus( skills, min, max, 1, 1, 15 ); break;
					case 10: ApplySkillBonus( skills, min, max, 2, 1, 15 ); break;
					case 11: ApplySkillBonus( skills, min, max, 3, 1, 15 ); break;
					//case 12: ApplyAttribute( primary, min, max, AosAttribute.LowerRegCost, 1, 20 ); break;
					case 13: ApplyAttribute( primary, min, max, AosAttribute.LowerManaCost, 1, 8 ); break;
					case 14: ApplyAttribute( primary, min, max, AosAttribute.RegenMana, 1, 2 ); break;
					case 15: spellbook.Slayer = GetRandomSlayer(); break;
				}
			}
		}
	}
}
