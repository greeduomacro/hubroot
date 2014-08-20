using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Targeting;
using System.IO;

namespace Server.Commands
{
	public class GenMulti
	{
		[CommandAttribute( "GenMulti", AccessLevel.Administrator )]
		public static void GenMulti_OnCommand( CommandEventArgs args )
		{
			Mobile m = args.Mobile;
			string file = args.GetString( 0 );

			if( String.IsNullOrEmpty( file ) )
			{
				m.SendMessage( "Format: GenMulti <fileName>" );
				return;
			}

			BoundingBoxPicker.Begin( m, new BoundingBoxCallback( boxPicker_callback ), file + ".txt" );
		}

		private static void boxPicker_callback( Mobile m, Map map, Point3D start, Point3D end, object state )
		{
			string filename = (string)state;

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
			List<Item> items = new List<Item>();

			try
			{
				foreach( Item i in eable )
				{
					items.Add( i );
				}
			}
			catch
			{
				m.SendMessage( "The targeted area was modified during collection. Please try again." );
				return;
			}
			finally
			{
				eable.Free();
			}

			if( items.Count > 0 )
			{
				m.BeginTarget( 12, true, TargetFlags.None, new TargetStateCallback( originTarget_callback ), new object[] { filename, items } );
				m.SendMessage( "Select the point of origin." );
			}
			else
			{
				m.SendMessage( "No items were found in the targeted area." );
			}
		}

		private static void originTarget_callback( Mobile m, object target, object state )
		{
			object[] args = (object[])state;
			string filename = (string)args[0];
			List<Item> itemList = (List<Item>)args[1];
			IPoint3D p = target as IPoint3D;

			if( p != null )
			{
				if( p is Item )
					p = ((Item)p).GetWorldTop();
				else if( p is Mobile )
					p = ((Mobile)p).Location;

				using( StreamWriter writer = new StreamWriter( filename, false ) )
				{
					writer.WriteLine( "0x1 0 0 0 0x0 0" );

					for( int i = 0; i < itemList.Count; i++ )
					{
						writer.WriteLine( "0x{0:X} {1} {2} {3} 0x{4:X} 1", itemList[i].ItemID, (itemList[i].X - p.X), (itemList[i].Y - p.Y), ((itemList[i].Z - p.Z) + 1), itemList[i].Hue );
					}

					writer.Close();
				}

				m.SendMessage( "The multi data has been output to \"{0}\"", filename );
			}
		}
	}
}