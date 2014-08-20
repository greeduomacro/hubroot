using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class RuneBeetle : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public RuneBeetle() : base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a rune beetle";
			BodyValue = 244;
			
			SetStr( 400, 465 );
			SetDex( 125, 170 );
			SetInt( 375, 450 );
			
			SetHits( 310, 360 );
			
			SetDamage( 12, 17 );
			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 70 );
			
			SetResistance( ResistanceType.Physical, 40, 65 );
			SetResistance( ResistanceType.Fire, 35, 50 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 75, 95 );
			SetResistance( ResistanceType.Energy, 40, 60 );
			
			SetSkill( SkillName.Wrestling, 70, 80 );
			SetSkill( SkillName.Tactics, 80, 95 );
			SetSkill( SkillName.Anatomy, 20, 30 );
			SetSkill( SkillName.MagicResist, 95, 110 );
			SetSkill( SkillName.Poisoning, 120, 140 );
			SetSkill( SkillName.Magery, 100, 110 );
			SetSkill( SkillName.EvalInt, 100, 125 );
			SetSkill( SkillName.Meditation, 95, 110 );
			
			Fame = 20000;
			Karma = -20000;
			
			VirtualArmor = 75;
			
			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 98.1;
		}
		
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 35; } }
		public override bool CanAngerOnTame { get { return true; } }
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 6 );
		}
		
		public override int GetAngerSound()
		{
			return 1256;
		}
		
		public override int GetAttackSound()
		{
			return 1257;
		}
		
		public override int GetIdleSound()
		{
			return 1255;
		}
		
		public override int GetHurtSound()
		{
			return 1254;
		}
		
		public override int GetDeathSound()
		{
			return 1253;
		}
		
		public RuneBeetle( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}
