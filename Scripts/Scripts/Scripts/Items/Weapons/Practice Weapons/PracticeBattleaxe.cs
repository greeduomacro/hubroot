using System;

using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF47, 0xF48 )]
	public class PracticeBattleaxe : BasePracticeWeapon
	{
		public override int DefHitSound { get { return 0x232; } }
		public override int DefMissSound { get { return 0x23A; } }

		public override SkillName DefSkill { get { return SkillName.Swords; } }
		public override WeaponType DefType { get { return WeaponType.Slashing; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash2H; } }

		[Constructable]
		public PracticeBattleaxe() : base( 0xF47 )
		{
			Layer = Layer.TwoHanded;
		}

		public PracticeBattleaxe( Serial serial ) : base( serial )
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