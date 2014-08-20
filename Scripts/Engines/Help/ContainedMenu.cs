using System;
using Server;
using Server.Menus;
using Server.Menus.Questions;

namespace Server.Help
{
	public class ContainedMenu : QuestionMenu
	{
		private Mobile _from;

		public ContainedMenu( Mobile from )
			: base( "You already have an open help request. A member of the staff team will assist you as soon as possible. What would you like to do?",
												new string[] { "Leave my help request as it is", "Remove and cancel my help request" } )
		{
			_from = from;
		}

		public override void OnCancel( Server.Network.NetState state )
		{
			_from.SendLocalizedMessage( 1005306, "", 0x35 ); //Help request unchanged.
		}

		public override void OnResponse( Server.Network.NetState state, int index )
		{
			if( index == 0 )
			{
				OnCancel( state );
			}
			else if( index == 1 )
			{
				PageEntry entry = PageQueue.GetEntry( _from );

				if( entry != null )
				{
					if( entry.Handler != null )
					{
						_from.SendMessage( "Your page is currently being handled by a staff member, and cannot be cancelled at this time." );
					}
					else
					{
						_from.SendLocalizedMessage( 1005307, "", 0x35 ); //Removed help request.

						PageQueue.Remove( entry );
					}
				}
				else
				{
					OnCancel( state );
				}
			}
		}
	}
}