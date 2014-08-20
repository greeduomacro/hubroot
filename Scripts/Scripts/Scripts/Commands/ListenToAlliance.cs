using System;
using System.Collections.Generic;
using Server;
using Server.Guilds;

namespace Server.Commands
{
	public class ListenToAlliance
	{
		private static Dictionary<Mobile, List<Guild>> _table = new Dictionary<Mobile, List<Guild>>();

		public static Dictionary<Mobile, List<Guild>> Table { get { return _table; } }

		[CommandAttribute( "ListenToAlliance", AccessLevel.GameMaster )]
		public static void ListenToAlliance_OnCommand( Server.Commands.CommandEventArgs args )
		{
			args.Mobile.SendMessage( "Target an aligned guild member." );
			args.Mobile.BeginTarget( -1, false, Server.Targeting.TargetFlags.None, delegate( Mobile from, object obj )
			{
				if( obj is Mobile )
				{
					Mobile m = obj as Mobile;

					if( m.Guild != null )
					{
						Guild g = m.Guild as Guild;

						if( g != null && g.Alliance != null )
						{
							if( _table.ContainsKey( from ) && _table[from].Contains( g ) )
							{
								_table[from].Remove( g );

								if( _table[from].Count < 1 )
									_table.Remove( from );

								from.SendMessage( "You are no longer listening to that alliance\'s private chat." );
							}
							else
							{
								if( _table.ContainsKey( from ) )
								{
									_table[from].Add( g );
								}
								else
								{
									List<Guild> list = new List<Guild>();
									list.Add( g );

									_table.Add( from, list );
								}

								from.SendMessage( "You are now listening to that alliance\'s private chat." );
							}
						}
						else
						{
							from.SendMessage( "Their guild is not part of an alliance." );
						}
					}
					else
					{
						from.SendMessage( "They are not in a guild." );
					}
				}
			} );
		}

		[CommandAttribute( "ClearAllianceListeners", AccessLevel.GameMaster )]
		public static void ClearAllianceListeners_OnCommand( Server.Commands.CommandEventArgs args )
		{
			if( _table.ContainsKey( args.Mobile ) )
				_table.Remove( args.Mobile );

			args.Mobile.SendMessage( "You are no longer listening to any private alliance chat." );
		}
	}
}