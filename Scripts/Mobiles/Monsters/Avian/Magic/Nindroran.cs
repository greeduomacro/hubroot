using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a nindroran corpse" )]
	public class Nindroran : BaseCreature
	{
		[Constructable]
		public Nindroran()
			: base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a nindroran";
			BodyValue = 243;
			BaseSoundID = 402;

			if( Utility.Random( 5 ) == 1 )
			{
				Female = true;
				Hue = 2014;

				SetStr( 720, 750 );
				SetDex( 100, 130 );
				SetInt( 385, 425 );

				SetHits( 430, 475 );

				SetDamage( 13, 19 );

				SetResistance( ResistanceType.Physical, 55, 70 );
				SetResistance( ResistanceType.Fire, 30, 40 );
				SetResistance( ResistanceType.Cold, 35, 45 );
				SetResistance( ResistanceType.Poison, 40, 50 );
				SetResistance( ResistanceType.Energy, 40, 50 );

				SetSkill( SkillName.Wrestling, 90, 100 );
				SetSkill( SkillName.Tactics, 98, 110 );
				SetSkill( SkillName.MagicResist, 95, 115 );
				SetSkill( SkillName.EvalInt, 100, 120 );
				SetSkill( SkillName.Magery, 95, 110 );
				SetSkill( SkillName.Meditation, 75, 90 );

				Fame = 12500;
				Karma = 0;
			}
			else
			{
				Hue = 2012;

				SetStr( 500, 580 );
				SetDex( 100, 125 );
				SetInt( 300, 350 );

				SetHits( 350, 395 );

				SetDamage( 10, 16 );

				SetResistance( ResistanceType.Physical, 40, 50 );
				SetResistance( ResistanceType.Fire, 20, 30 );
				SetResistance( ResistanceType.Cold, 20, 35 );
				SetResistance( ResistanceType.Poison, 35, 45 );
				SetResistance( ResistanceType.Energy, 35, 40 );

				SetSkill( SkillName.Wrestling, 80, 90 );
				SetSkill( SkillName.Tactics, 85, 95 );
				SetSkill( SkillName.MagicResist, 95, 100 );
				SetSkill( SkillName.EvalInt, 95, 105 );
				SetSkill( SkillName.Magery, 90, 100 );
				SetSkill( SkillName.Meditation, 70, 80 );

				Fame = 10000;
				Karma = 0;
			}

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			VirtualArmor = 45;
		}

		public override int TreasureMapLevel { get { return 5; } }
		public override Poison HitPoison { get { return Poison.Greater; } }
		public override double HitPoisonChance { get { return 70; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 4, 7 ) );
		}

		public Nindroran( Serial serial )
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
