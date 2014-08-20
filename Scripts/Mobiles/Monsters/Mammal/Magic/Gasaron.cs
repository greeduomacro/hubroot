using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	public class Gasaron : BaseCreature
	{
		[Constructable]
		public Gasaron()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gasaron";
			BodyValue = 250;
			BaseSoundID = 1323;

			SetStr( 400, 450 );
			SetDex( 150, 200 );
			SetInt( 65, 75 );

			SetHits( 380, 440 );

			SetDamage( 12, 15 );
			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.EvalInt, 65, 85 );
			SetSkill( SkillName.Magery, 60, 75 );
			SetSkill( SkillName.Meditation, 30, 45 );
			SetSkill( SkillName.MagicResist, 65, 70 );
			SetSkill( SkillName.Wrestling, 95, 110 );
			SetSkill( SkillName.Tactics, 95, 110 );
			SetSkill( SkillName.Anatomy, 40, 50 );

			Fame = 8500;
			Karma = -10000;

			VirtualArmor = 25;
		}

		public override HideType HideType { get { return HideType.Horned; } }
		public override int Hides { get { return 8; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if( 0.05 >= Utility.RandomDouble() )
				SummonDires( attacker );
		}

		private void SummonDires( Mobile target )
		{
			Map map = this.Map;

			if( map == null )
				return;

			int newWolves = Utility.RandomMinMax( 2, 3 );

			for( int i = 0; i < newWolves; ++i )
			{
				DireWolf wolf = new DireWolf();

				wolf.Team = this.Team;
				wolf.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				wolf.MoveToWorld( loc, map );
				wolf.Combatant = target;
			}
		}

		public Gasaron( Serial serial )
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
