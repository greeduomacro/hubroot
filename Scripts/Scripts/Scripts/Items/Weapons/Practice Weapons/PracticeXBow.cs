using System;

using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF50, 0xF4F )]
	public class PracticeXBow : BaseRanged
	{
		public override int EffectID { get { return 0x1BFE; } }
		public override Type AmmoType { get { return typeof( Bolt ); } }
		public override Item Ammo { get { return new Bolt(); } }

		public override int DefHitSound { get { return 0x234; } }
		public override int DefMissSound { get { return 0x238; } }

		public override int DefMaxRange { get { return 5; } }

		public override SkillName DefSkill { get { return SkillName.Archery; } }
		public override WeaponType DefType { get { return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.ShootXBow; } }

		public override int InitMinHits { get { return Utility.RandomMinMax( 50, 150 ); } }
		public override int InitMaxHits { get { return InitMinHits; } }

		[Constructable]
		public PracticeXBow() : base( 0xF50 )
		{
			Layer = Layer.TwoHanded;
			LootType = LootType.Newbied;
			MinDamage = MaxDamage = 1;
			Name = ItemData.Name + " [practice weapon]";
			Speed = 38;
			StrRequirement = 35;
			Weight = 5.0;
		}

		public PracticeXBow( Serial serial ) : base( serial )
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