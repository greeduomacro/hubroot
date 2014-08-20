using System;
using Server;
using Server.Network;
using Server.Multis;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Regions;
using Server.Mobiles;
using Server.Spells;

namespace Server.Spells
{
	public class RiftSpell : Spell
	{
		public override bool ClearHandsOnCast { get { return false; } }

		public override int GetMana()
		{
			return 15;
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = 0;
			max = 0;
		}

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(5.0);
            }
        }

        private static SpellInfo m_Info = new SpellInfo(
                "Rift", "In Sanct Vas Por",
                263,
                9032

            );

		private RunebookEntry _entry;

		public RiftSpell( Mobile caster, Item scroll )
			: this( caster, scroll, null )
		{
		}

		public RiftSpell( Mobile caster, Item scroll, RunebookEntry entry )
			: base( caster, scroll, m_Info )
		{
			_entry = entry;
		}

		public override void OnCast()
		{
			if( _entry == null )
				Caster.Target = new InternalTarget( this );
			else
				Effect( _entry.Location, _entry.Map, true );
		}

		public override bool CheckCast()
		{
			if( SpellHelper.CheckCombat( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}

			return SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom );
		}

		public void Effect( Point3D loc, Map map, bool checkMulti )
		{
			if( map == null || (!Core.AOS && Caster.Map != map) )
			{
				Caster.SendLocalizedMessage( 1005570 ); // You can not gate to another facet.
			}
			else if( !SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom ) )
			{
			}
			else if( !SpellHelper.CheckTravel( Caster, map, loc, TravelCheckType.GateTo ) )
			{
			}

			else if( Caster.Kills >= 5 && map != Map.Felucca )
			{
				Caster.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if( SpellHelper.CheckCombat( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
			}
			else if( !map.CanSpawnMobile( loc.X, loc.Y, loc.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if( (checkMulti && SpellHelper.CheckMulti( loc, map )) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if( CheckSequence() )
			{
				Caster.PublicOverheadMessage( Server.Network.MessageType.Regular, Caster.EmoteHue, true, "*slashes through the air, cutting through the fabric of space and time itself*" );

				Effects.PlaySound( Caster.Location, Caster.Map, 476 );

				InternalItem firstGate = new InternalItem( loc, map );
				firstGate.Hue = 1;
				firstGate.MoveToWorld( Caster.Location, Caster.Map );

				Effects.PlaySound( loc, map, 476 );

				InternalItem secondGate = new InternalItem( Caster.Location, Caster.Map );
				secondGate.Hue = 1;
				secondGate.MoveToWorld( loc, map );
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Moongate
		{
			public InternalItem( Point3D target, Map map )
				: base( target, map )
			{
				Map = map;

				Dispellable = true;

				InternalTimer t = new InternalTimer( this );
				t.Start();
			}

			public InternalItem( Serial serial )
				: base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				Delete();
			}

			private class InternalTimer : Timer
			{
				private Item _item;

				public InternalTimer( Item item )
					: base( TimeSpan.FromSeconds( 10.0 ) )
				{
					_item = item;

					Priority = TimerPriority.OneSecond;
				}

				protected override void OnTick()
				{
					if( _item != null )
						_item.Delete();
				}
			}
		}

		private class InternalTarget : Target
		{
			private RiftSpell _spell;

			public InternalTarget( RiftSpell owner )
				: base( 12, false, TargetFlags.None )
			{
				_spell = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if( o is RecallRune )
				{
					RecallRune rune = (RecallRune)o;

					if( rune.Marked )
						_spell.Effect( rune.Target, rune.TargetMap, true );
					else
					{
						rune.Mark( from );

						from.SendMessage( "You use the dagger to inscribe your location on the rune." );
					}
				}
				else if( o is Runebook )
				{
					RunebookEntry e = ((Runebook)o).Default;

					if( e != null )
						_spell.Effect( e.Location, e.Map, true );
					else
						from.SendLocalizedMessage( 502354 ); // Target is not marked.
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501030, from.Name, "" ) ); // I can not gate travel from that object.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				_spell.FinishSequence();
			}
		}
	}
}