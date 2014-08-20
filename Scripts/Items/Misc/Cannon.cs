using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public enum CannonDirection
	{
		North,
		East,
		South,
		West
	}

	public class Cannon : BaseAddon
	{
		private CannonDirection m_CannonDirection;
		private Mobile m_Cannoneer;

		[CommandProperty( AccessLevel.GameMaster )]
		public CannonDirection CannonDirection { get { return m_CannonDirection; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Cannoneer { get { return m_Cannoneer; } set { m_Cannoneer = value; } }

		[Constructable]
		public Cannon( CannonDirection direction )
		{
			m_CannonDirection = direction;

			switch( direction )
			{
				case CannonDirection.North:
					{
						AddComponent( new CannonComponent( 0xE8D, this ), 0, 0, 0 );
						AddComponent( new CannonComponent( 0xE8C, this ), 0, 1, 0 );
						AddComponent( new CannonComponent( 0xE8B, this ), 0, 2, 0 );

						break;
					}
				case CannonDirection.East:
					{
						AddComponent( new CannonComponent( 0xE96, this ), 0, 0, 0 );
						AddComponent( new CannonComponent( 0xE95, this ), -1, 0, 0 );
						AddComponent( new CannonComponent( 0xE94, this ), -2, 0, 0 );

						break;
					}
				case CannonDirection.South:
					{
						AddComponent( new CannonComponent( 0xE91, this ), 0, 0, 0 );
						AddComponent( new CannonComponent( 0xE92, this ), 0, -1, 0 );
						AddComponent( new CannonComponent( 0xE93, this ), 0, -2, 0 );

						break;
					}
				default:
					{
						AddComponent( new CannonComponent( 0xE8E, this ), 0, 0, 0 );
						AddComponent( new CannonComponent( 0xE8F, this ), 1, 0, 0 );
						AddComponent( new CannonComponent( 0xE90, this ), 2, 0, 0 );

						break;
					}
			}
		}

		public Cannon( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from is Player && from.Skills.ArmsLore.Value >= 60.0)
            {
                if (from.InRange(this.GetWorldLocation(), 1))
                {
                    if (from.Backpack.GetAmount(typeof(CannonBall), true) > 0)
                    {
                        if (from.Backpack.GetAmount(typeof(SulfurousAsh), true) >= 5)
                        {
                            if (Utility.RandomDouble() > (from.Skills.ArmsLore.Value / 100))
                            {
                                Effects.SendLocationEffect(from, Map, 0x36B0, 16, 1);
                                Effects.PlaySound(from, Map, 0x11D);

                                from.Damage(Utility.RandomMinMax(30, 60));
                                from.SendMessage("Your lack of understanding how to use the cannon causes a miss-fire!");
                            }

                            else
                            {
                                from.SendMessage("Select the point to fire at.");
                                from.Target = new InternalTarget(((PlayerMobile)from), this);
                            }
                        }
                        else
                        {
                            from.SendMessage("You need to have at least five pieces of sulfurous ash to fire the cannon.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You need at least one cannon ball!");
                    }
                }
                else
                {
                    from.SendMessage("You are not close enough to use this.");
                }
            }

            else
            {
                from.SendMessage("You do not posess the required armslore skill needed to use this");
            }
		}

		public void DoFireEffect( IPoint3D target )
		{
			Point3D from;

			switch( m_CannonDirection )
			{
				case CannonDirection.North: from = new Point3D( X, Y - 1, Z ); break;
				case CannonDirection.East: from = new Point3D( X + 1, Y, Z ); break;
				case CannonDirection.South: from = new Point3D( X, Y + 1, Z ); break;
				default: from = new Point3D( X - 1, Y, Z ); break;
			}

			Effects.SendLocationEffect( from, Map, 0x36B0, 16, 1 );
			Effects.PlaySound( from, Map, 0x11D );

			Effects.SendLocationEffect( target, Map, 0x36B0, 16, 1 );
			Effects.PlaySound( target, Map, 0x11D );

			System.Collections.Generic.List<Mobile> list = new System.Collections.Generic.List<Mobile>();
			IPooledEnumerable eable = this.GetMobilesInRange( 16 );

			foreach( Mobile m in eable )
			{
				if( Utility.InRange( m.Location, new Point3D( target ), 3 ) && m.Location != target )
					list.Add( m );
			}

			eable.Free();

			for( int i = 0; i < list.Count; i++ )
			{
				list[i].Damage( Utility.RandomMinMax( 30, 45 ) );
			}

			list.Clear();
		}

		public void Fire( Mobile from, Mobile target )
		{
			DoFireEffect( target );

			target.Damage( ((target.HitsMax / 2) + 50), from );
		}

		public override bool HandlesOnMovement { get { return m_Cannoneer != null && !m_Cannoneer.Deleted && m_Cannoneer.Alive; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m_Cannoneer == null || m_Cannoneer.Deleted || !m_Cannoneer.Alive )
				return;

			bool canFire, isEnemy;

			switch( m_CannonDirection )
			{
				case CannonDirection.North:
					canFire = m.X >= X - 7 && m.X <= X + 7 && m.Y == Y - 7 && oldLocation.Y < Y - 7;
					break;
				case CannonDirection.East:
					canFire = m.Y >= Y - 7 && m.Y <= Y + 7 && m.X == X + 7 && oldLocation.X > X + 7;
					break;
				case CannonDirection.South:
					canFire = m.X >= X - 7 && m.X <= X + 7 && m.Y == Y + 7 && oldLocation.Y > Y + 7;
					break;
				default:
					canFire = m.Y >= Y - 7 && m.Y <= Y + 7 && m.X == X - 7 && oldLocation.X < X - 7;
					break;
			}

			if( m_Cannoneer is BaseCreature )
				isEnemy = ((BaseCreature)m_Cannoneer).IsEnemy( m );
			else
				isEnemy = true;

			if( canFire && isEnemy )
				Fire( m_Cannoneer, m );
		}

		public override void Serialize( GenericWriter writer )
		{
			if( m_Cannoneer != null && m_Cannoneer.Deleted )
				m_Cannoneer = null;

			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.WriteEncodedInt( (int)m_CannonDirection );
			writer.Write( (Mobile)m_Cannoneer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_CannonDirection = (CannonDirection)reader.ReadEncodedInt();
			m_Cannoneer = (Mobile)reader.ReadMobile();
		}

		private class InternalTarget : Target
		{
			private PlayerMobile m_From;
			private Cannon m_Cannon;

			public InternalTarget( PlayerMobile from, Cannon c )
				: base( 16, true, TargetFlags.Harmful )
			{
				m_From = from;
				m_Cannon = c;

				CheckLOS = true;
			}

            protected override void OnTarget(Mobile from, object target)
            {
                if (from.InRange(m_Cannon.GetWorldLocation(), 1))
                {
                    if (target is Mobile)
                    {
                        if (!from.CanBeHarmful(((Mobile)target), false, false))
                            return;
                    }

                    if (target is IPoint3D)
                    {
                        Point3D p = new Point3D(((IPoint3D)target));

                        bool canFire;

                        switch (m_Cannon.CannonDirection)
                        {
                            case CannonDirection.North:
                                canFire = (p.X >= m_Cannon.X - 1 && p.X <= m_Cannon.X + 1) && (p.Y < m_Cannon.Y - 2);
                                break;
                            case CannonDirection.East:
                                canFire = (p.Y >= m_Cannon.Y - 1 && p.Y <= m_Cannon.Y + 1) && (p.X > m_Cannon.X + 2);
                                break;
                            case CannonDirection.South:
                                canFire = (p.X >= m_Cannon.X - 1 && p.X <= m_Cannon.X + 1) && (p.Y > m_Cannon.Y + 2);
                                break;
                            default:
                                canFire = (p.Y >= m_Cannon.Y - 1 && p.Y <= m_Cannon.Y + 1) && (p.X < m_Cannon.X - 2);
                                break;
                        }

                        if (canFire)
                        {
                            if (target is Mobile)
                                m_Cannon.Fire(from, ((Mobile)target));
                            else
                                m_Cannon.DoFireEffect(p);

                            from.Backpack.ConsumeTotal(new Type[] { typeof(CannonBall), typeof(SulfurousAsh) }, new int[] { 1, 5 }, true);
                        }
                    }
                }
            }
		}
	}

	public class CannonComponent : AddonComponent
	{
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Cannoneer
		{
			get { return Addon is Cannon ? ((Cannon)Addon).Cannoneer : null; }
			set { if( Addon is Cannon ) ((Cannon)Addon).Cannoneer = value; }
		}

		private Cannon m_Cannon;

		public CannonComponent( int itemID, Cannon cannon )
			: base( itemID )
		{
			m_Cannon = cannon;
		}

		public CannonComponent( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( m_Cannon != null )
				m_Cannon.OnDoubleClick( from );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( (Mobile)Cannoneer );
			writer.Write( (Cannon)m_Cannon );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			Cannoneer = reader.ReadMobile() as Mobile;
			m_Cannon = reader.ReadItem() as Cannon;
		}
	}
}