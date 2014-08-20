using System;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
	public class SayCommand
	{
		[CommandAttribute( "Say", AccessLevel.Counselor )]
		public static void SayCommand_OnCommand( CommandEventArgs args )
		{
			string text = args.ArgString.Trim();

			if( text.Length > 0 )
				args.Mobile.Target = new InternalTarget( text );
			else
				args.Mobile.SendMessage( "Format: Say \'text\'" );
		}

		private class InternalTarget : Target
		{
			private string m_toSay;

			public InternalTarget( string text )
				: base( 12, false, TargetFlags.None )
			{
				m_toSay = text;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is Mobile )
				{
					Mobile speaker = (Mobile)target;

					if( from != speaker && from.AccessLevel > speaker.AccessLevel )
					{
						speaker.Say( m_toSay );

						CommandLogging.WriteLine( from, "{0} {1} forcing speech on {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( speaker ) );
					}
				}
				else if( target is Item )
				{
					Item item = (Item)target;

					item.PublicOverheadMessage( MessageType.Regular, from.SpeechHue, false, m_toSay );
				}
				else
					from.SendMessage( "Invalid type." );
			}
		}
	}
}
