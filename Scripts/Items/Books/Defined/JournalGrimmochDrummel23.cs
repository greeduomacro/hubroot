using System;
using Server;

namespace Server.Items
{
	public class JournalGrimmochDrummel23 : BrownBook
	{
		[Constructable]
		public JournalGrimmochDrummel23() : base( "The daily journal of Grimmoch Drummel", "Grimmoch ", 1, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
		int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"Day 23:", 
				"", 
				"We no longer bury the dead.", 
				"", 
				"", 
				"",
				"",
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		}

		public JournalGrimmochDrummel23( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}
	}
}