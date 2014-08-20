using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DaemonBait : Item
	{
		private int m_UsesRemaining;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get{ return m_UsesRemaining; }
			set{ m_UsesRemaining = value; InvalidateProperties(); }
		}

		[Constructable]
		public DaemonBait() : this( Utility.RandomMinMax( 8, 12 ) )
		{
		}
		
		[Constructable]
		public DaemonBait( int amount ) : base( 0x1CF0 )
		{
			Name = "daemon bait";
			Hue = 1157;
			LootType = LootType.Newbied;
			
			UsesRemaining = amount;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 );
			else if( this.UsesRemaining < 1 )
			{
				from.SendMessage( "There is no more use for this item." );
				this.Delete();
			}
			else if( from.BeginAction( typeof( DaemonBait ) ) )
			{
				new InternalTimer( from ).Start();
				
				SpawnDaemon( from );
				Effects.SendLocationEffect( new Point3D( from.X, from.Y, from.Z + 1 ), from.Map, 0x37B9, 13 );
				Effects.SendLocationEffect( new Point3D( from.X, from.Y, from.Z ), from.Map, 0x37C3, 13 );
				
				this.UsesRemaining -= 1;
				
				if( this.UsesRemaining <= 0 )
					this.Delete();
			}
			else
				from.SendMessage( "You must wait before another daemon is ready to be called forth." );
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			list.Add( 1060584, "{0}", this.UsesRemaining );
		}
		
		public DaemonBait( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
			
			writer.Write( (int) m_UsesRemaining );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			m_UsesRemaining = (int)reader.ReadInt();
		}
		
		private void SpawnDaemon( Mobile owner )
		{
			Daemon daemon = new Daemon();
			Point3D point = new Point3D( owner.X + 2, owner.Y + 2, owner.Z );
			
			daemon.MoveToWorld( point, owner.Map );
			daemon.Controlled = true;
			daemon.ControlMaster = owner;
			daemon.ControlOrder = OrderType.Follow;
			daemon.ControlTarget = owner;

			Effects.SendLocationEffect( new Point3D( daemon.X, daemon.Y, daemon.Z + 1 ), daemon.Map, 0x3709, 13 );
			Effects.SendLocationEffect( new Point3D( daemon.X, daemon.Y, daemon.Z ), daemon.Map, 0x37CC, 13 );
			daemon.BoltEffect( 3 );
		}
		
		private class InternalTimer : Timer
		{
			private Mobile m_From;
			
			public InternalTimer( Mobile from ) : base( TimeSpan.FromMinutes( 15 ) )
			{
				m_From = from;
				Priority = TimerPriority.OneSecond;
			}
			
			protected override void OnTick()
			{
				Stop();
				m_From.EndAction( typeof( DaemonBait ) );
			}
		}
	}
}
