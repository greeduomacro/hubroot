using System;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Commands
{
	public class BecomeCommand
	{
		[CommandAttribute( "Become", AccessLevel.Administrator )]
		public static void Become_OnCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;

			if( args.Arguments.Length != 1 )
				m.SendMessage( "Format: Become <targetPersona>" );
			else
				Transform( m, args.Arguments[0] );
		}

		private static void Transform( Mobile m, string target )
		{
			if( m.BodyValue != 0x190 )
				m.BodyValue = 0x190;
			if( m.BodyMod != 0 )
				m.BodyMod = 0;

			m.Female = false;
			m.Title = null;
			m.HueMod = -1;
			m.Hue = 33774;

			m.Criminal = false;
			m.Kills = 0;
			m.ShortTermMurders = 0;

			m.Fame = 0;
			m.Karma = 0;

			switch( target.ToLower() )
			{
				case "tykiim":
					{
						m.Name = "Tykiim";

						m.RawStr = 65000;
						m.Hits = 32550;
						m.RawInt = 65000;
						m.Mana = 65000;
						m.RawDex = 65000;
						m.Stam = 65000;

						m.HairItemID = Hair.TopKnot;
						m.FacialHairItemID = Beard.ShortBeard;
						m.HairHue = m.FacialHairHue = 1157;

						for( int i = 0; i < SkillInfo.Table.Length; i++ )
							m.Skills[i].Base = 6000;

						m.Skills.Tactics.Base = 6500;

						break;
					}
				case "playermage":
					{
						m.RawStr = 110;
						m.RawInt = 110;
						m.RawDex = 40;

						for( int i = 0; i < SkillInfo.Table.Length; i++ )
							m.Skills[i].Base = 0;

						m.Skills[SkillName.EvalInt].Base = 100;
						m.Skills[SkillName.Inscribe].Base = 100;
						m.Skills[SkillName.Magery].Base = 100;
						m.Skills[SkillName.MagicResist].Base = 100;
						m.Skills[SkillName.Meditation].Base = 100;
						m.Skills[SkillName.Wrestling].Base = 100;

						break;
					}
				case "playerwarrior":
					{
						m.RawStr = 110;
						m.RawInt = 40;
						m.RawDex = 110;

						for( int i = 0; i < SkillInfo.Table.Length; i++ )
							m.Skills[i].Base = 0;

						m.Skills[SkillName.Anatomy].Base = 100;
						m.Skills[SkillName.Archery].Base = 100;
						m.Skills[SkillName.Fencing].Base = 100;
						m.Skills[SkillName.Focus].Base = 100;
						m.Skills[SkillName.Healing].Base = 100;
						m.Skills[SkillName.Macing].Base = 100;
						m.Skills[SkillName.MagicResist].Base = 100;
						m.Skills[SkillName.Parry].Base = 100;
						m.Skills[SkillName.Swords].Base = 100;
						m.Skills[SkillName.Tactics].Base = 100;

						break;
					}
			}
		}
	}
}