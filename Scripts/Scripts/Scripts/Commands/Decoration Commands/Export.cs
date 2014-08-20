using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;
using System.Reflection;
using Server.Targeting;

namespace Server.Commands
{
	public class Export
	{
		private static string path = string.Format( @".\Exports" );
		private static string name;

		public static void Initialize()
		{
			CommandSystem.Register( "ExportItems", AccessLevel.Administrator, new CommandEventHandler( ExportItems_OnCommand ) );
			CommandSystem.Register( "ExportStatics", AccessLevel.Administrator, new CommandEventHandler( ExportStatics_OnCommand ) );
		}

		[Usage( "ExportItems <filename>" )]
		[Description( "Exports items in the targeted area to a script." )]
		private static void ExportItems_OnCommand( CommandEventArgs e )
		{
			if( e.Arguments.Length == 1 )
			{
				name = e.Arguments[0];
				ExportItems( e.Mobile );
			}
			else
				e.Mobile.SendMessage( "Usage: ExportItems <filename>" );
		}

		[Usage( "ExportStatics <filename>" )]
		[Description( "Exports statics in the targeted area to a script." )]
		private static void ExportStatics_OnCommand( CommandEventArgs e )
		{
			if( e.Arguments.Length == 1 )
			{
				name = e.Arguments[0];
				ExportStatics( e.Mobile );
			}
			else
				e.Mobile.SendMessage( "Usage: ExportStatics <filename>" );
		}

		////////////////////////////////////ITEMS///////////////////////////////////////////// 
		private static void ExportItems( Mobile from )
		{
			BoundingBoxPicker.Begin( from, new BoundingBoxCallback( ExportItems1 ), 1 );
		}

		private static void ExportItems1( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			//World.Broadcast( 0x35, true, "Export file is being generated, please wait." );
			from.SendMessage( "Export file is being generated. Please wait." );

			DateTime startTime = DateTime.Now;
			DirectoryInfo di = null;
			if( !Directory.Exists( path ) )
				di = Directory.CreateDirectory( path );
			path = string.Format( @".\Exports\" + name + ".cs" );

			using( StreamWriter sw = File.CreateText( path ) )
			{
				int z1 = 0;
				ArrayList statics = new ArrayList();
				StaticTile[] tiles;
				//======================================================================================
				sw.WriteLine( "using System;" );
				sw.WriteLine( "using Server;" );
				sw.WriteLine( "using Server.Items;" );
				sw.WriteLine( "using Server.Targeting;" );
				sw.WriteLine();
				sw.WriteLine( "namespace Server.Commands" );
				sw.WriteLine( "{" );
				sw.WriteLine( "\tpublic class " + name );
				sw.WriteLine( "\t{" );
				sw.WriteLine( "\t\tpublic static void Initialize()" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\tCommandSystem.Register( \"" + name + "\", AccessLevel.Administrator, new CommandEventHandler( " + name + "_OnCommand ) );" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\t[Usage( \"" + name + "\" )]" );
				sw.WriteLine( "\t\t[Description( \"Creates a replica of an exported static\" )]" );
				sw.WriteLine( "\t\tprivate static void " + name + "_OnCommand( CommandEventArgs e )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\te.Mobile.Target = new InternalTarget();" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\tprivate class InternalTarget : Target" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\tpublic InternalTarget() : base( 6, true, TargetFlags.None )" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\t}" );
				sw.WriteLine( "\t\t\tprotected override void OnTarget( Mobile from, object o )" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\tStatic item = null;" );
				//======================================================================================
				tiles = from.Map.Tiles.GetStaticTiles( start.X, start.Y, true );
				for( int a = 0; a < tiles.Length; a++ )
				{
					try
					{
						z1 = tiles[a].Z;
					}
					catch { sw.WriteLine( "No location for tile" ); }
				}
				IPooledEnumerable eable = from.Map.GetItemsInBounds( new Rectangle2D( start, end ) );
				{
					foreach( Item item in eable )
					{
						sw.WriteLine( "\t\t\titem = new Static(" + item.ItemID + ");" );
						sw.WriteLine( "\t\t\titem.Movable = false;" );
						sw.WriteLine( "\t\t\titem.Location = new Point3D( ((IPoint3D)o).X + " + (item.X - start.X) + ", ((IPoint3D)o).Y + " + (item.Y - start.Y) + ", ((IPoint3D)o).Z + " + (item.Z - z1) + ");" );
						sw.WriteLine( "\t\t\titem.Hue = " + item.Hue + ";" );
						sw.WriteLine( "\t\t\titem.Map = from.Map;" );
					}
				}
				sw.WriteLine( "\t\t\t}" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t}" );
				sw.WriteLine( "}" );
				DateTime endTime = DateTime.Now;

				//World.Broadcast( 0x35, true, "Export file has been completed. The entire process took {0:F1} seconds.", (endTime - startTime).TotalSeconds );
				from.SendMessage( "Export file has been completed. The entire process took {0:F1}seconds.", (endTime - startTime).TotalSeconds );
				path = string.Format( @".\Exports" );
			}
		}
		////////////////////////////////////STATICS///////////////////////////////////////////// 
		private static void ExportStatics( Mobile from )
		{
			BoundingBoxPicker.Begin( from, new BoundingBoxCallback( ExportStatics1 ), 1 );
		}
		private static void ExportStatics1( Mobile from, Map map, Point3D start, Point3D end, object state )
		{
			//World.Broadcast( 0x35, true, "Export file is being generated, please wait." );
			from.SendMessage( "Export file is being generated. Please wait." );

			DateTime startTime = DateTime.Now;
			DirectoryInfo di = null;
			if( !Directory.Exists( path ) )
				di = Directory.CreateDirectory( path );
			path = string.Format( @".\Exports\" + name + ".cs" );

			using( StreamWriter sw = File.CreateText( path ) )
			{
				int x = end.X - start.X;
				int y = end.Y - start.Y;
				int z1 = 0;
				StaticTarget st = null;
				StaticTile[] statics;
				LandTile[] tiles;
				//======================================================================================
				sw.WriteLine( "using System;" );
				sw.WriteLine( "using Server;" );
				sw.WriteLine( "using Server.Items;" );
				sw.WriteLine( "using Server.Targeting;" );
				sw.WriteLine( "namespace Server.Scripts.Commands" );
				sw.WriteLine( "{" );
				sw.WriteLine( "\tpublic class " + name );
				sw.WriteLine( "\t{" );
				sw.WriteLine( "\t\tpublic static void Initialize()" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\tCommandSystem.Register( \"" + name + "\", AccessLevel.Administrator, new CommandEventHandler( " + name + "_OnCommand ) );" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\t[Usage( \"" + name + "\" )]" );
				sw.WriteLine( "\t\t[Description( \"Creates a replica of an exported static\" )]" );
				sw.WriteLine( "\t\tprivate static void " + name + "_OnCommand( CommandEventArgs e )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\te.Mobile.Target = new InternalTarget();" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\tprivate class InternalTarget : Target" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\tpublic InternalTarget() : base( 6, true, TargetFlags.None )" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\t}" );
				sw.WriteLine( "\t\t\tprotected override void OnTarget( Mobile from, object o )" );
				sw.WriteLine( "\t\t\t{" );
				sw.WriteLine( "\t\t\tStatic item = null;" );
				//======================================================================================
				tiles = from.Map.Tiles.GetLandBlock( start.X, start.Y );
				for( int a = 0; a < tiles.Length; a++ )
				{
					try
					{
						z1 = tiles[a].Z;
					}
					catch { sw.WriteLine( "No location for tile" ); }
				}
				for( int i = 0; i < x; i++ )
				{
					for( int j = 0; j < y; j++ )
					{
						statics = from.Map.Tiles.GetStaticTiles( start.X + i, start.Y + j, true );
						for( int z = 0; z < statics.Length; z++ )
						{
							st = new StaticTarget( new Point3D( start.X + i, start.Y + j, statics[z].Z ), statics[z].ID );
							sw.WriteLine( "\t\t\titem = new Static(" + st.ItemID + ");" );
							sw.WriteLine( "\t\t\titem.Movable = false;" );
							sw.WriteLine( "\t\t\titem.Location = new Point3D( ((IPoint3D)o).X + " + i + ", ((IPoint3D)o).Y + " + j + ", ((IPoint3D)o).Z + " + (statics[z].Z - z1) + ");" );
							sw.WriteLine( "\t\t\titem.Map = from.Map;" );
							sw.WriteLine( "" );
						}
					}
				}
				sw.WriteLine( "\t\t\t}" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t}" );
				sw.WriteLine( "}" );
				DateTime endTime = DateTime.Now;

				//World.Broadcast( 0x35, true, "Export file has been completed. The entire process took {0:F1} seconds.", (endTime - startTime).TotalSeconds );
				from.SendMessage( "Export file has been completed. The entire process took {0:F1} seconds.", (endTime - startTime).TotalSeconds );
				path = string.Format( @".\Exports" );
			}
		}
	}
}
