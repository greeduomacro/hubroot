using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class IzkarksRing : BaseRing
	{
		private DateTime _lastUsed;

		[Constructable]
		public IzkarksRing()
			: base( 0x108A ) {
			Hue = 1409;
			Name = "A golden, jewel-encrusted ring";
			Weight = 0;
		}

		public override bool OnEquip( Mobile m ) {
			m.PublicOverheadMessage( Server.Network.MessageType.Regular, m.EmoteHue, true, "*as you place the ring on your hand, you feel a sudden jolt of painful energy course through your body and the ring becomes heavy.*" );
			m.PlaySound( 0x5BF );
			m.Hits = 0;
			m.Stam = 0;

			this.Hue = 1175;
			this.LootType = LootType.Cursed;
			this.Name = "a blackened, jewel-encrusted ring";

			return base.OnEquip( m );
		}

		public override void OnDoubleClick( Mobile m ) {
			if( this.IsChildOf( m ) && (m.Serial == 0x363 || m.AccessLevel > AccessLevel.Player) ) {

				if( (DateTime.Now - _lastUsed) < TimeSpan.FromSeconds( 6.0 ) ) {
					m.SendMessage( "You must allow the ring time to cool down." );
				} else {
					m.Target = new InternalTarget( this );
				}

			} else {
				m.SendMessage( "The ring emits a soft glow, becoming warm to the touch. It eventually stops, rendering no noticable effect." );
			}
		}

		public void OnTarget( IPoint3D p ) {
			if( this.Parent == null || !(this.Parent is Mobile) )
				return;

			Mobile m = (Mobile)this.Parent;

			if( m.Mana >= 10 ) {
				Map map = m.Map;

				if( p is RecallRune ) {
					RecallRune rune = p as RecallRune;

					if( rune.Marked ) {
						m.MoveToWorld( rune.Target, rune.TargetMap );
						m.SendMessage( "You have been moved to the rune's location." );
					} else {
						rune.Mark( m );

						m.SendMessage( "You use the ring's power to etch your location into the rune." );
					}
				} else if( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) ) {
					m.SendMessage( "The ring rejects your command to travel." );
				} else if( m.Hidden ) {
					int armorRating = Server.SkillHandlers.Stealth.GetArmorRating( m );

					if( m.CheckSkill( SkillName.Stealth, -20.0 + (armorRating * 2), (Core.AOS ? 60.0 : 80.0) + (armorRating * 2) ) ) {
						_lastUsed = DateTime.Now;

						m.Mana -= 10;
						m.MoveToWorld( new Point3D( p ), map );

						m.SendMessage( "You manage to silently use the ring's power." );
					} else {
						_lastUsed = DateTime.Now;

						m.Mana -= 10;
						m.RevealingAction();
						m.MoveToWorld( new Point3D( p ), map );
						m.SendMessage( "You fail in your attempt to travel silently." );

						Effects.PlaySound( p, map, 0x20E );
						Effects.SendLocationParticles( m, 0x1FCB, 10, 10, 2023 );
						Effects.SendLocationParticles( EffectItem.Create( new Point3D( p ), map, EffectItem.DefaultDuration ), 0x1FCB, 10, 10, 5023 );
					}
				} else {
					_lastUsed = DateTime.Now;

					m.Mana -= 10;
					m.MoveToWorld( new Point3D( p ), map );

					Effects.PlaySound( p, map, 0x20E );
					Effects.SendLocationParticles( m, 0x1FCB, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create( new Point3D( p ), map, EffectItem.DefaultDuration ), 0x1FCB, 10, 10, 5023 );
				}
			} else {
				m.PublicOverheadMessage( Server.Network.MessageType.Regular, m.EmoteHue, true, "*the ring becomes warm, emitting a soft glow, but it is unable to function.*" );
				m.SendMessage( "You do not have enough mana to channel the power of the ring." );
				m.RevealingAction();
				m.PlaySound( 0x5BF );
			}
		}

		public class InternalTarget : Target
		{
			private IzkarksRing _owner;

			public InternalTarget( IzkarksRing owner )
				: base( 16, true, TargetFlags.None ) {
				_owner = owner;

				CheckLOS = false;

			}

			protected override void OnTarget( Mobile from, object o ) {
				IPoint3D p = o as IPoint3D;

				if( p != null )
					_owner.OnTarget( p );
			}
		}

		public IzkarksRing( Serial serial )
			: base( serial ) {
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