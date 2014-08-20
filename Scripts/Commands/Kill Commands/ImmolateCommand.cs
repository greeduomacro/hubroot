using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Commands
{
	public class ImmolateCommand : BaseKillCommand
	{
		public static void Initialize()
		{
			TargetCommands.Register( new ImmolateCommand() );
		}

		public ImmolateCommand()
		{
			Commands = new string[] { "Immolate" };
			Description = "Engulfs the targeted mobile(s) in flames for a few seconds, burning them to death.";
			Usage = "Immolate";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Mobile )
			{
				Mobile m = obj as Mobile;

				if( !m.Alive || m.Blessed || !m.CanBeDamaged() )
				{
					LogFailure( "This command will not work on that in its current state." );
				}
				else
				{
					new InternalTimer( this, m ).Start();
					e.Mobile.SendMessage( "The immolation process has begun." );
				}
			}
			else
				LogFailure( "This command only works on mobiles." );
		}

		private class InternalTimer : Timer
		{
			private BaseKillCommand _cmd;
			private Mobile _mobile;
			private int _count;

			public InternalTimer( BaseKillCommand cmd, Mobile m )
				: base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				_cmd = cmd;
				_mobile = m;
				_count = 0;

				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if( _count++ <= 10 && (_mobile != null && _mobile.Alive) )
				{
					_mobile.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );

					if( (_count % 2) == 0 )
						_mobile.PlaySound( (_mobile.Female ? 814 : 1088) );
					else
						_mobile.PlaySound( 0x208 );

					AOS.Damage( _mobile, null, Utility.RandomMinMax( 6, 12 ), 0, 100, 0, 0, 0, true );
				}
				else
				{
					_mobile.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
					_mobile.PlaySound( (_mobile.Female ? 814 : 1088) );

					_mobile.PublicOverheadMessage( Server.Network.MessageType.Emote, 0x38A, false, "*screams in total agony*" );

					_cmd.Kill( _mobile, true, true, true );
					_cmd.TimedCorpse.Hue = 1175;
					_cmd.TimedCorpse.Name = String.Format( "the charred remains of {0}", _mobile.RawName );

					Stop();
				}
			}
		}
	}
}