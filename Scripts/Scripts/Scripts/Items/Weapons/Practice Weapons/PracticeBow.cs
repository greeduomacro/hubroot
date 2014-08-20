using System;

using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class PracticeBow : BaseRanged
	{
		public override int EffectID { get { return 0xF42; } }
		public override Type AmmoType { get { return typeof( Arrow ); } }
		public override Item Ammo { get { return new Arrow(); } }

		public override int DefHitSound { get { return 0x234; } }
		public override int DefMissSound { get { return 0x238; } }

		public override int DefMaxRange{ get{ return 6; } }

		public override SkillName DefSkill { get { return SkillName.Archery; } }
		public override WeaponType DefType { get { return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.ShootBow; } }

		public override int InitMinHits { get { return Utility.RandomMinMax( 50, 150 ); } }
		public override int InitMaxHits { get { return InitMinHits; } }

		[Constructable]
		public PracticeBow() : base( 0x13B2 )
		{
			Layer = Layer.TwoHanded;
			LootType = LootType.Newbied;
			MinDamage = MaxDamage = 1;
			Name = ItemData.Name + " [practice weapon]";
			Speed = 38;
			StrRequirement = 35;
			Weight = 5.0;
		}

		public PracticeBow( Serial serial ) : base( serial )
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