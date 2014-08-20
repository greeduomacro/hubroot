using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items {
	public abstract class BaseThrowingWeap : BaseMeleeWeapon {
		public abstract int EffectID { get; }
		public abstract Type AmmoType { get; }
		public abstract Item Ammo { get; }

		public override int AosHitSound { get { return 0x232; } }
		public override int AosMissSound { get { return 0x23A; } }

		public override SkillName AosSkill { get { return SkillName.Swords; } }
		public override WeaponType AosType { get { return WeaponType.Ranged; } }
		public override WeaponAnimation AosAnimation { get { return WeaponAnimation.Slash2H; } }

		public override SkillName AccuracySkill { get { return SkillName.Swords; } }

		public BaseThrowingWeap( int itemID )
			: base( itemID ) {
		}

		public BaseThrowingWeap( Serial serial )
			: base( serial ) {
		}

		public override TimeSpan OnSwing( Mobile attacker, Mobile defender ) {
			// Make sure we've been standing still for one second
			if( DateTime.Now > (attacker.LastMoveTime + TimeSpan.FromSeconds( Core.AOS ? 0.5 : 1.0 )) || (Core.AOS && WeaponAbility.GetCurrentAbility( attacker ) is MovingShot) ) {
				bool canSwing = true;

				if( Core.AOS ) {
					canSwing = (!attacker.Paralyzed && !attacker.Frozen);

					if( canSwing ) {
						Spell sp = attacker.Spell as Spell;

						canSwing = (sp == null || !sp.IsCasting || !sp.BlocksMovement);
					}
				}

				if( canSwing && attacker.HarmfulCheck( defender ) ) {
					attacker.DisruptiveAction();
					attacker.Send( new Swing( 0, attacker, defender ) );

					if( OnFired( attacker, defender ) ) {
						if( CheckHit( attacker, defender ) )
							OnHit( attacker, defender );
						else
							OnMiss( attacker, defender );
					}
				}

				return GetDelay( attacker );
			} else {
				return TimeSpan.FromSeconds( 0.25 );
			}
		}

		public override void OnHit( Mobile attacker, Mobile defender ) {
			if( attacker.Player && !defender.Player && (defender.Body.IsAnimal || defender.Body.IsMonster) && 0.4 >= Utility.RandomDouble() )
				defender.AddToBackpack( Ammo );

			base.OnHit( attacker, defender );
		}

		public override void OnMiss( Mobile attacker, Mobile defender ) {
			if( attacker.Player && 0.4 >= Utility.RandomDouble() )
				Ammo.MoveToWorld( new Point3D( defender.X + Utility.RandomMinMax( -1, 1 ), defender.Y + Utility.RandomMinMax( -1, 1 ), defender.Z ), defender.Map );

			base.OnMiss( attacker, defender );
		}

		public virtual bool OnFired( Mobile attacker, Mobile defender ) {
			Container pack = attacker.Backpack;

			if( attacker.Player && (pack == null || !pack.ConsumeTotal( AmmoType, 1 )) )
				return false;

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

		public override void Serialize( GenericWriter writer ) {
			base.Serialize( writer );

			writer.Write( (int)2 ); // version
		}

		public override void Deserialize( GenericReader reader ) {
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version ) {
				case 2:
				case 1: {
						break;
					}
				case 0: {
						/*m_EffectID =*/
						reader.ReadInt();
						break;
					}
			}

		}
	}
}
