using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0xF43, 0xF44 )]
	public class YewAxe : BaseAxe, IUsesRemaining
	{
		public override HarvestSystem HarvestSystem { get { return Lumberjacking.System; } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int AosStrengthReq { get { return 50; } }
		public override int AosMinDamage { get { return 13; } }
		public override int AosMaxDamage { get { return 15; } }
		public override int AosSpeed { get { return 35; } }

		public override int OldStrengthReq { get { return 25; } }
		public override int OldMinDamage { get { return 1; } }
		public override int OldMaxDamage { get { return 15; } }
		public override int OldSpeed { get { return 35; } }

		[Constructable]
		public YewAxe()
			: this( Utility.RandomMinMax( 60, 80 ) )
		{
		}

		[Constructable]
		public YewAxe( int uses )
			: base( 0xF43 )
		{
			Hue = 0x973;
			Name = "a yew axe";
			ShowUsesRemaining = true;
			UsesRemaining = uses;
			Weight = 4.0;
		}

		public YewAxe( Serial serial )
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