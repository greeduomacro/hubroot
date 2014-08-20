using System;
using Server;
using Server.Targeting;
using Server.Gumps;

namespace Server.Commands
{
	public class SkillCaps
	{
		public static void Initialize()
		{
			CommandSystem.Register( "SkillCaps", AccessLevel.GameMaster, new CommandEventHandler( SkillCaps_OnCommand ) );
		}

		private class SkillCapsTarget : Target
		{
			public SkillCapsTarget()
				: base( -1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if( o is Mobile )
					from.SendGump( new SkillsGump( from, (Mobile)o, true, null ) );
			}
		}

		[Usage( "SkillCaps" )]
		[Description( "Opens a menu where you can view or edit skill caps of a targeted mobile." )]
		private static void SkillCaps_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new SkillCapsTarget();
		}
	}
}