using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BasePracticeWeapon : BaseWeapon
	{
		public override int InitMinHits { get { return Utility.RandomMinMax( 50, 150 ); } }
		public override int InitMaxHits { get { return InitMinHits; } }

		public BasePracticeWeapon( int itemID )
			: base( itemID ) {
			Layer = Layer.OneHanded;
			LootType = LootType.Newbied;
			MaxDamage = 15;
			MinDamage = 5;
			Name = ItemData.Name + " [practice weapon]";
			Speed = 38;
			StrRequirement = 35;
			Weight = 5.0;
		}

		public BasePracticeWeapon( Serial serial )
			: base( serial ) {
		}

		public override int AbsorbDamage( Mobile attacker, Mobile defender, int damage ) {
			return AbsorbDamageAOS( attacker, defender, damage );
		}

		public override int AbsorbDamageAOS( Mobile attacker, Mobile defender, int damage ) {
			if( attacker.Guild != null && defender.Guild != null ) {
				if( attacker.Guild == defender.Guild ) {
					defender.FixedEffect( 0x37B9, 10, 16 );
					damage = (int)(damage * 0.2);
				} else
					damage = (int)(damage * 1.2);
			}

			return damage;
		}

		public override void Serialize( GenericWriter writer ) {
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader ) {
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}