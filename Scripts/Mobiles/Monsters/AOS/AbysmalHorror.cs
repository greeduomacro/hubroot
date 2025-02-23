using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an abyssmal horror corpse" )]
	public class AbysmalHorror : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.MortalStrike : WeaponAbility.WhirlwindAttack;
		}

		[Constructable]
		public AbysmalHorror()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an abyssmal horror";
			Body = 312;
			BaseSoundID = 0x451;

			SetStr( 401, 420 );
			SetDex( 81, 90 );
			SetInt( 401, 420 );

			SetHits( 6000 );

			SetDamage( 13, 17 );

			SetDamageType( ResistanceType.Physical, 50, 65 );
			SetDamageType( ResistanceType.Poison, 50, 65 );

			SetResistance( ResistanceType.Physical, 30, 75 );
			SetResistance( ResistanceType.Fire, 80, 120 );
			SetResistance( ResistanceType.Cold, 50, 75 );
			SetResistance( ResistanceType.Poison, 60, 85 );
			SetResistance( ResistanceType.Energy, 77, 120 );

			SetSkill( SkillName.EvalInt, 200.0 );
			SetSkill( SkillName.Magery, 112.6, 190.5 );
			SetSkill( SkillName.Meditation, 150.0, 200.0 );
			SetSkill( SkillName.MagicResist, 117.6, 180.0 );
			SetSkill( SkillName.Tactics, 100.0, 120.0 );
			SetSkill( SkillName.Wrestling, 84.1, 98.0 );

			Fame = 26000;
			Karma = -26000;

			VirtualArmor = 54;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override bool BardImmune { get { return !Core.SE; } }
		public override bool Unprovokable { get { return Core.SE; } }
		public override bool AreaPeaceImmune { get { return Core.SE; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 1; } }

		public AbysmalHorror( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if( BaseSoundID == 357 )
				BaseSoundID = 0x451;
		}
	}
}