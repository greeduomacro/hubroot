using System;

using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x143B, 0x143A )]
	public class PracticeMaul : BasePracticeWeapon
	{
		public override int DefHitSound { get { return 0x233; } }
		public override int DefMissSound { get { return 0x239; } }

		public override SkillName DefSkill { get { return SkillName.Macing; } }
		public override WeaponType DefType { get { return WeaponType.Bashing; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Bash2H; } }

		[Constructable]
		public PracticeMaul() : base( 0x143B )
		{
			Layer = Layer.TwoHanded;
		}

		public PracticeMaul( Serial serial ) : base( serial )
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