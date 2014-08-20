using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;

namespace Server.Commands
{
	public class DisintegrateCommand : BaseKillCommand
	{
		public static void Initialize()
		{
			TargetCommands.Register( new DisintegrateCommand() );
		}

		public DisintegrateCommand()
		{
			Commands = new string[] { "Disintegrate" };
			Description = "Disintegrates the targeted mobile(s), leaving ash and a charred body part.";
			Usage = "Disintegrate";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Mobile )
			{
				Mobile m = obj as Mobile;

				if( !m.Alive || m.Blessed || !m.CanBeDamaged() )
					LogFailure( "This command cannot work on that in its current state." );
				else
				{
					Disintegrate( m );
					e.Mobile.SendMessage( "{0} has been disintegrated.", m.RawName );
				}
			}
			else
				LogFailure( "This command only works on mobiles." );
		}

		public void Disintegrate( Mobile m )
		{
			TimedItem dust = new TimedItem( 300.0, 0xF91 );
			dust.Hue = 934;
			dust.Name = String.Format( "the charred remains of {0}", m.RawName );
			dust.MoveToWorld( m.Location, m.Map );

			TimedItem scatterings = new TimedItem( 300.0, 0xF35 );
			scatterings.Hue = 934;
			scatterings.Name = "burnt dust scatterings";
			scatterings.MoveToWorld( m.Location, m.Map );

			if( m.Body.IsHuman )
			{
				TimedItem gore = new TimedItem( 300.0, Utility.Random( 0x1D9F, 5 ) );
				gore.Hue = 1140;
				gore.Name = String.Format( "a charred piece of {0}", m.RawName );
				gore.MoveToWorld( m.Location, m.Map );
			}

			Effects.PlaySound( m.Location, m.Map, 0x307 );
			Effects.SendLocationEffect( m.Location, m.Map, 14000, 10, 934, 0 );

			Kill( m, true, false, true );

			m.SendMessage( "You have been disintegrated. All of your belongings have been placed in your bankbox." );
		}
	}
}