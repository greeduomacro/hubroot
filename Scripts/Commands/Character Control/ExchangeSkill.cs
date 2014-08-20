using System;
using Server;
using Server.Commands;
using Server.Targeting;

namespace Server.Commands
{
	public class ExchangeSkillCommand
	{
		[CommandAttribute( "ExchangeSkill", AccessLevel.GameMaster )]
		public static void ExchangeSkill_OnCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;

			if( args.Length != 2 )
			{
				m.SendMessage( "ExchangeSkill <skill to lower> <skill to raise>" );
			}
			else
			{
				SkillName toLower = SkillName.Swords;
				SkillName toRaise = SkillName.Swords;

				if( TryParse( m, args.GetString( 0 ), ref toLower ) && TryParse( m, args.GetString( 1 ), ref toRaise ) )
				{
					m.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( OnTarget ), new object[] { toLower, toRaise } );
					m.SendMessage( "Select the mobile." );
				}
			}
		}

		private static void OnTarget( Mobile from, object targeted, object state )
		{
			object[] args = state as object[];
			SkillName toLower = (SkillName)args[0];
			SkillName toRaise = (SkillName)args[1];

			if( targeted is Mobile )
			{
				Mobile targ = (Mobile)targeted;
				Skill targLower = targ.Skills[toLower];
				Skill targRaise = targ.Skills[toRaise];

				if( targLower == null || targRaise == null )
					return;

				double swapBase = targRaise.Base, swapCap = targRaise.Cap;

				targRaise.Base = targLower.Base;
				targRaise.Cap = targLower.Cap;
				targLower.Base = swapBase;
				targLower.Cap = swapCap;

				CommandLogging.LogChangeProperty( from, targ, String.Format( "{0} was exchanged for", toLower ), toRaise.ToString() );

				from.SendMessage( "{0}\'s {1} skill has been exchanged for {2} {3} skill.",
					targ.RawName, toLower.ToString(), (targ.Female ? "her" : "his"), toRaise.ToString() );
			}
			else
			{
				from.SendMessage( "This only works for mobiles." );
			}
		}

		public static bool TryParse( Mobile m, string toParse, ref SkillName skill )
		{
			try
			{
				skill = (SkillName)Enum.Parse( typeof( SkillName ), toParse, true );
			}
			catch
			{
				m.SendMessage( "{0} is an invalid skill name.", skill.ToString() );
				return false;
			}

			return true;
		}
	}
}