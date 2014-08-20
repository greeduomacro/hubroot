using System;
using Server;

namespace Server.Items
{
	public class LethalPoisonPotion : BasePoisonPotion
	{
		public override Poison Poison{ get{ return Poison.Lethal; } }

		public override double MinPoisoningSkill{ get{ return 95.0; } }
		public override double MaxPoisoningSkill{ get{ return 120.0; } }

		[Constructable]
		public LethalPoisonPotion() : base( PotionEffect.PoisonGreater )
		{
		}

		public LethalPoisonPotion( Serial serial ) : base( serial )
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
