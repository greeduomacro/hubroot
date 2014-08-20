using System;
using System.Collections.Generic;
using Server;
using Server.Guilds;

namespace Server.Commands
{
	public class ListenToGuild
	{
		private static Dictionary<Mobile, List<BaseGuild>> _table = new Dictionary<Mobile, List<BaseGuild>>();

		public static Dictionary<Mobile, List<BaseGuild>> Table { get { return _table; } }

		[CommandAttribute( "ListenToGuild", AccessLevel.GameMaster )]
		public static void ListenToGuild_OnCommand( Server.Commands.CommandEventArgs args )
		{
			args.Mobile.SendMessage( "Target a guild member." );
			args.Mobile.BeginTarget( -1, false, Server.Targeting.TargetFlags.None, delegate( Mobile from, object obj )
			{
				if( obj is Mobile )
				{
					Mobile m = obj as Mobile;

					if( m.Guild == null )
					{
						from.SendMessage( "They are not in a guild." );
					}
					else if( _table.ContainsKey( from ) && _table[from].Contains( m.Guild ) )
					{
						_table[from].Remove( m.Guild );

						if( _table[from].Count < 1 )
							_table.Remove( from );

						from.SendMessage( "You are no longer listening to that guild\'s private chat." );
					}
					else
					{
						if( _table.ContainsKey( from ) )
						{
							_table[from].Add( m.Guild );
						}
						else
						{
							List<BaseGuild> list = new List<BaseGuild>();
							list.Add( m.Guild );

							_table.Add( from, list );
						}

						from.SendMessage( "You are now listening to that guild\'s private chat." );
					}
				}
			} );
		}

		[CommandAttribute( "ClearGuildListeners", AccessLevel.GameMaster )]
		public static void ClearGuildListeners_OnCommand( Server.Commands.CommandEventArgs args )
		{
			if( _table.ContainsKey( args.Mobile ) )
				_table.Remove( args.Mobile );

			args.Mobile.SendMessage( "You are no longer listening to any private guild chat." );
		}
	}
}