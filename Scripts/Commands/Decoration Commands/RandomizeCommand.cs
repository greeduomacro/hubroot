using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Commands
{
	public class RandomizeCommand
	{
		[CommandAttribute( "Randomize", AccessLevel.GameMaster )]
		public static void Randomize_OnCommand( Server.Commands.CommandEventArgs args )
		{
			Mobile m = args.Mobile;

			if( args.Length != 4 )
			{
				m.SendMessage( "Format: Randomize <starting itemID> <ending itemID> <Z level> <bool includeNonFlooring>" );
				return;
			}

			int start = args.GetInt32( 0 );
			int end = args.GetInt32( 1 );
			int z = args.GetInt32( 2 );
			bool walls = args.GetBoolean( 3 );

			object[] state = new object[4] { start, end, z, walls };

			BoundingBoxPicker.Begin( m, new BoundingBoxCallback( BoxPickerCallback ), state );
		}

		private static void BoxPickerCallback( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			object[] args = state as object[];

			if( args == null || args.Length != 4 )
				return;

			if( start.X > end.X )
			{
				int x = start.X;
				start.X = end.X;
				end.X = x;
			}

			if( start.Y > end.Y )
			{
				int y = start.Y;
				start.Y = end.Y;
				end.Y = y;
			}

			IPooledEnumerable eable = map.GetItemsInBounds( new Rectangle2D( start.X, start.Y, ((end.X - start.X) + 1), ((end.Y - start.Y) + 1) ) );
			List<Static> selection = new List<Static>();
			bool fail = false;
			int startID = (int)args[0];
			int endID = (int)args[1];
			int zLevel = (int)args[2];
			bool includeWalls = (bool)args[3];

			if( startID > endID )
			{
				int temp = startID;
				startID = endID;
				endID = startID;
			}

			try
			{
				foreach( object o in eable )
				{
					if( o is Static )
					{
						Static st = o as Static;

						if( st.Z != zLevel )
							continue;
						else if( !includeWalls && !IsFlooring( st.ItemID ) )
							continue;

						selection.Add( st );
					}
				}
			}
			catch( Exception e )
			{
				Server.Utilities.ExceptionManager.LogException( "RandomizeCommand.cs", e );
				from.SendMessage( "The targeted items were modified during assembly. Please try again." );
				fail = true;
			}
			finally
			{
				eable.Free();
			}

			if( fail )
				return;

			#region Random list creation
			List<int> list = new List<int>();

			while( startID <= endID )
			{
				int i = startID;
				list.Add( i );

				startID++;
			}

			int[] IDlist = new int[list.Count];

			for( int i = 0; i < IDlist.Length; i++ )
			{
				IDlist[i] = list[i];
			}

			list.Clear();
			#endregion

			int count = 0;

			for( int i = 0; i < selection.Count; i++ )
			{
				selection[i].ItemID = Utility.RandomList( IDlist );
				count++;
			}

			from.SendMessage( "Randomization complete: {0} tiles were processed.", count );
		}

		private static bool IsFlooring( int itemID )
		{
			if( itemID >= 1035 && itemID <= 1054 )
				return true;
			else if( itemID >= 1169 && itemID <= 1413 )
				return true;
			else if( itemID >= 1993 && itemID <= 2000 )
				return true;
			else if( itemID >= 6013 && itemID <= 6092 )
				return true;
			else if( itemID >= 12788 && itemID <= 12955 )
				return true;

			return false;
		}
	}
}