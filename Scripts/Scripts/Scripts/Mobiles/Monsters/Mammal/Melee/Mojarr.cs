using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Mojarr : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public Mojarr()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a mojarr";
			BodyValue = 241;
			BaseSoundID = 1249;

			SetStr( 400, 430 );
			SetDex( 130, 165 );
			SetInt( 100, 140 );

			SetHits( 240, 260 );

			SetDamage( 14, 18 );
			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 55, 70 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65, 80 );
			SetSkill( SkillName.Wrestling, 65, 80 );
			SetSkill( SkillName.Tactics, 75, 90 );
			SetSkill( SkillName.Anatomy, 30, 45 );

			Fame = 5500;
			Karma = -4000;

			VirtualArmor = 25;
		}

		public override Poison HitPoison { get { return Poison.Greater; } }
		public override double HitPoisonChance { get { return 0.45; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override int Hides { get { return Utility.RandomMinMax( 5, 11 ); } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public Mojarr( Serial serial )
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
		}
	}
}
