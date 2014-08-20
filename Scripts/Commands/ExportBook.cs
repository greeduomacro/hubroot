using System;
using System.IO;
using System.Text.RegularExpressions;
using Server.Items;
using Server;
using System.Reflection;
using Server.Targeting;

namespace Server.Commands
{
	public class BookExport
	{
		private static string path = string.Format( @".\Exports" );
		private static string name;

		public static void Initialize()
		{
			CommandSystem.Register( "ExportBook", AccessLevel.Administrator, new CommandEventHandler( ExportBook_OnCommand ) );
		}

		[Usage( "ExportBook <filename>" )]
		[Description( "Exports the book targeted to a script." )]
		private static void ExportBook_OnCommand( CommandEventArgs e )
		{
			if( e.Arguments.Length == 1 )
			{
				name = e.Arguments[0];
				e.Mobile.Target = new TargetBook();
			}
			else
				e.Mobile.SendMessage( "Usage: ExportBook <filename>" );
		}

		private class TargetBook : Target
		{

			public TargetBook()
				: base( 5, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( targeted is BaseBook )
					BookExport.ExportBook( (BaseBook)targeted );
				else
					from.SendMessage( "You must target a book." );
			}
		}

		private static void ExportBook( BaseBook book )
		{
			World.Broadcast( 0x35, true, "Export file is being generated, please wait." );

			DateTime startTime = DateTime.Now;
			DirectoryInfo di = null;
			if( !Directory.Exists( path ) )
				di = Directory.CreateDirectory( path );
			path = string.Format( @".\Exports\" + name + ".cs" );

			using( StreamWriter sw = File.CreateText( path ) )
			{
				//======================================================================================
				sw.WriteLine( "using System;" );
				sw.WriteLine( "using Server;" );
				sw.WriteLine( "namespace Server.Items" );
				sw.WriteLine( "{" );
				sw.WriteLine( "\tpublic class " + name + " : BaseBook" );
				sw.WriteLine( "\t{" );
				sw.WriteLine( "\t\t[Constructable]" );
				sw.WriteLine( "\t\tpublic " + name + "() : base( " + book.ItemID + ", " + (book.Writable.ToString()).ToLower() + " )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\tTitle = \"" + book.Title + "\";" );
				sw.WriteLine( "\t\t\tAuthor = \"" + book.Author + "\";" );
				sw.WriteLine( "\t\t\tint lastline = 0;" );
				string body = null;
				for( int i = 0; i < book.PagesCount; i++ )
				{
					foreach( string line in book.Pages[i].Lines )
					{
						if( line == "" )
							body += "\n";
						body += line;
					}
				}
				body = Regex.Replace( body, "\n", " \\n " );
				body = Regex.Replace( body, "  ", " " );
				sw.WriteLine( "\t\t\tstring body = \"" + body + "\";" );
				sw.WriteLine( "\t\t\tint p = GetPages(body, ref lastline);" );
				sw.WriteLine( "\t\t\tPages = new BookPageInfo[p];" );
				sw.WriteLine( "\t\t\tfor ( int i = 0; i < Pages.Length; ++i )" );
				sw.WriteLine( "\t\t\t\tPages[i] = new BookPageInfo();" );
				sw.WriteLine( "\t\t\tFormatPages(body, p, lastline);" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\tpublic " + name + "(Serial serial) : base( serial )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\tpublic override void Serialize( GenericWriter writer )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\tbase.Serialize( writer );" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t\tpublic override void Deserialize( GenericReader reader )" );
				sw.WriteLine( "\t\t{" );
				sw.WriteLine( "\t\t\tbase.Deserialize( reader );" );
				sw.WriteLine( "\t\t}" );
				sw.WriteLine( "\t}" );
				sw.WriteLine( "}" );

				DateTime endTime = DateTime.Now;

				World.Broadcast( 0x35, true, "Export file has been completed. The entire process took {0:F1} seconds.", (endTime - startTime).TotalSeconds );
				path = string.Format( @".\Exports" );
			}
		}
	}
}
