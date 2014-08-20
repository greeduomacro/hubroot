using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class RockPile : Item
	{

		[Constructable]
		public RockPile()
			: base( 0x1367 )
		{
			Name = "a pile of rocks";
			Weight = 10;
		}

		public RockPile( Serial serial )
			: base( serial )
		{

		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "You must have that pile more accessible before throwing." );
				return;
			}
			else
			{
				if( from.CanBeginAction( typeof( RockPile ) ) )
				{
					from.Target = new RockTarget( from );
					from.SendMessage( "You chip off a large piece of the rock pile." );
				}
				else
				{
					from.SendMessage( "That piece is too solid. Try another." );
				}

			}

		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from )
				: base( TimeSpan.FromSeconds( 0.25 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.EndAction( typeof( RockPile ) );
			}
		}

		private class RockTarget : Target
		{
			private Mobile m_Thrower;

			public RockTarget( Mobile thrower )
				: base( 10, false, TargetFlags.None )
			{
				m_Thrower = thrower;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target == from )
				{
					from.SendMessage( "Don't you think that would hurt?" );
				}
				else if( target is Mobile )
				{
					Mobile m = (Mobile)target;

					if( from.Hidden && from.AccessLevel < AccessLevel.GameMaster )
						from.RevealingAction();

					if( from.AccessLevel < m.AccessLevel )
					{
						from.Blessed = false;
						from.Kill();

						return;
					}

					from.BeginAction( typeof( RockPile ) );
					from.PlaySound( 0x2F3 );
					from.Animate( 9, 1, 1, true, false, 0 );
					from.SendMessage( "You throw a rock and hit the target!" );

					int hits = m.Hits;
					int str = m.RawStr;

					if( m.Alive )
					{
						if( m.Hits > 15 )
							m.Hits /= 2;
						else
							m.Kill();
					}

					Effects.SendMovingEffect( from, m, 0x11B6, 8, 0, false, false, 0, 0 );
				}
				else
				{
					from.SendMessage( "Wouldn't be nice to throw that at someone so weak, would it?" );
				}
				new InternalTimer( from ).Start();
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.EndAction( typeof( RockPile ) );
			}
		}
	}
}
