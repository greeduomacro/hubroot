using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Commands
{
	public class ExplodeCommand : BaseKillCommand
	{
		public static void Initialize()
		{
			TargetCommands.Register( new ExplodeCommand() );
		}

		public ExplodeCommand()
		{
			Commands = new string[] { "Explode" };
			Description = "Annihilates the targeted mobile(s) in a gory explosion.";
			Usage = "Explode";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Mobile )
			{
				Mobile m = obj as Mobile;

				if( !m.Alive || m.Blessed || !m.CanBeDamaged() )
					LogFailure( "This command cannot work on that in its current state." );
				else
					Explode( m );
			}
			else
				LogFailure( "This command only works on mobiles." );
		}

		public void Explode( Mobile m )
		{
			if( m.Body.IsHuman )
			{
				int[] parts = new int[] { 7389, 7390, 7395, 7396, 7397, 7405, 7406, 7408, 7409 };

				for( int i = 0; i < parts.Length; i++ )
				{
					int itemID = parts[i];

					Point3D end = new Point3D( m.X + Utility.RandomMinMax( -8, 8 ), m.Y + Utility.RandomMinMax( -8, 8 ), m.Z );
					Effects.SendMovingEffect( m, new Entity( Serial.Zero, end, m.Map ), itemID, 5, 10, false, false );

					Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishExplosion ), new object[] { m.Map, end, itemID } );
				}
			}

			m.FixedParticles( 0x36D0, 20, 10, 5044, EffectLayer.Waist );
			m.PlaySound( 0x307 );

			Kill( m, true, false, true );
		}

		private void FinishExplosion( object info )
		{
			object[] args = (object[])info;

			if( args == null || args.Length <= 0 )
				return;

			Map map = (Map)args[0];
			Point3D p = (Point3D)args[1];
			int itemID = (int)args[2];

			new Server.Items.TimedItem( 120.0, itemID ).MoveToWorld( p, map );
		}
	}
}