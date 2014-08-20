using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Commands
{
	public class ImplodeCommand : BaseKillCommand
	{
		public static void Initialize()
		{
			TargetCommands.Register( new ImplodeCommand() );

			EventSink.Movement += new MovementEventHandler( OnMovement );
		}

		public ImplodeCommand()
		{
			Commands = new string[] { "Implode" };
			Description = "Causes the target to implode upon itself.";
			Usage = "Implode";
		}

		public override void Execute( Server.Commands.CommandEventArgs args, object o )
		{
			if( o is Mobile )
			{
				Mobile m = (Mobile)o;

				if( !m.Alive || m.Blessed || !m.CanBeDamaged() )
				{
					LogFailure( "This command cannot work on that in its current state." );
				}
				else
				{
					m.BodyMod = 305;
					m.HueMod = 0;
					new Server.Items.Blood().MoveToWorld( m.Location, m.Map );

					Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( PlayEffects ), new object[] { m, m.Map } );

					new InternalTimer( this, m ).Start();
				}
			}
			else
				LogFailure( "This command only works on mobiles." );
		}

		private static void PlayEffects( object info )
		{
			object[] args = (object[])info;

			if( args == null || args.Length <= 0 )
				return;

			Mobile m = (Mobile)args[0];
			Map map = (Map)args[1];

			Effects.PlaySound( m, map, 834 );

			m.PublicOverheadMessage( Server.Network.MessageType.Regular, m.SpeechHue, true, String.Format( "*shudders inwardly, {0} body convulsing violently as {1} skin reverses itself, showing the muscle and innards to the world*", m.Body.IsHuman ? m.Female ? "her" : "his" : "its", m.Body.IsHuman ? m.Female ? "her" : "his" : "its" ) );
		}

		private static void OnMovement( MovementEventArgs args )
		{
			Mobile m = args.Mobile;

			if( m == null || m.Map == null || m.Map == Map.Internal )
				return;

			if( m.BodyMod == 305 )
				new Server.Items.Blood( Utility.RandomList( 4650, 4651, 4652, 4653, 4654, 4655, 7428, 7410, 7418 ) ).MoveToWorld( m.Location, m.Map );
		}

		private class InternalTimer : Timer
		{
			private BaseKillCommand _cmd;
			private Mobile _mobile;

			public InternalTimer( BaseKillCommand cmd, Mobile m )
				: base( TimeSpan.FromSeconds( 7.0 ) )
			{
				_cmd = cmd;
				_mobile = m;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if( _mobile == null )
				{
					Stop();
					return;
				}
				else if( !_mobile.Alive || _mobile.Blessed || _mobile.Map == null || _mobile.Map == Map.Internal )
				{
					Stop();
					return;
				}

				_cmd.Kill( _mobile, true, false, true );

				new Server.Items.TimedItem( 300.0, 4650 ).MoveToWorld( _mobile.Location, _mobile.Map ); //blood
				new Server.Items.TimedItem( 300.0, 7407 ).MoveToWorld( _mobile.Location, _mobile.Map ); //entrails

				Stop();
			}
		}
	}
}