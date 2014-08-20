using System;
using System.Reflection;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Commands
{
	public class Particle
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Particle", AccessLevel.Counselor, new CommandEventHandler( Particle_OnCommand ) );
		}

		[Usage( "Particle [itemid] [speed] [duration] [effect] [hue] [rendermode]" )]
		[Description( "Shows a particle effect" )]
		private static void Particle_OnCommand( CommandEventArgs e )
		{
			int partID = 0;
			int speed = 0;
			int duration = 0;
			int effect = 0;
			int hue = 0;
			int rendermode = 0;

			if( e.Length >= 6 )
			{
				partID = e.GetInt32( 0 );
				speed = e.GetInt32( 1 );
				duration = e.GetInt32( 2 );
				effect = e.GetInt32( 3 );
				hue = e.GetInt32( 4 );
				rendermode = e.GetInt32( 5 );
			}
			else
			{
				e.Mobile.SendMessage( "Particle [itemid] [speed] [duration] [effect] [hue] [rendermode]" );
			}
			e.Mobile.FixedParticles( partID, speed, duration, effect, hue, rendermode, EffectLayer.Waist );
		}
	}
}