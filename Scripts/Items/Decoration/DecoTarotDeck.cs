using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x12AB, 0x12AC )]
	public class DecoTarotDeck : Item
	{
		[Constructable]
		public DecoTarotDeck()
			: base( 0x12AB )
		{
			Name = "tarot deck";
		}

		public DecoTarotDeck( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			switch( ((Item)this).ItemID )
			{
				case 0x12AB: // Closed north
					if( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A5;
					else
						((Item)this).ItemID = 0x12A8;
					break;
				case 0x12AC: // Closed east
					if( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A6;
					else
						((Item)this).ItemID = 0x12A7;
					break;
				case 0x12A5: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A6: ((Item)this).ItemID = 0x12AC; break; // Open east
				case 0x12A8: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A7: ((Item)this).ItemID = 0x12AC; break; // Open east
			}
		}
	}
}