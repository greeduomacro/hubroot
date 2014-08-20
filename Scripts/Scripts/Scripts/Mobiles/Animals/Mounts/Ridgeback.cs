using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a ridgeback corpse" )]
	public class Ridgeback : BaseMount
	{
		[Constructable]
		public Ridgeback() : this( "a ridgeback" )
		{
		}

		[Constructable]
		public Ridgeback( string name ) : base( name, 187, 0x3EBA, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x3F3;

            SetStr(258, 300);
            SetDex(156, 175);
            SetInt(16, 30);

            SetHits(241, 254);
            SetMana(0);

            SetDamage(13, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 60);
            SetResistance(ResistanceType.Fire, 30, 45);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 40, 55);
            SetResistance(ResistanceType.Energy, 30, 55);

            SetSkill(SkillName.MagicResist, 95.3, 99.0);
            SetSkill(SkillName.Tactics, 89.3, 94.0);
            SetSkill(SkillName.Wrestling, 95.1, 95.0);

			Fame = 300;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 83.1;
		}

		public override bool OverrideBondingReqs()
		{
			return true;
		}

		public override double GetControlChance( Mobile m, bool useBaseSkill )
		{
			return 1.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Ridgeback( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}