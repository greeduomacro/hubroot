using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x14ED, 0x14EE )]
	public class Scroll : BaseBook
	{
		[Constructable]
		public Scroll()
			: base( 0x14ED )
		{
			Name = "a scroll";
		}

		[Constructable]
		public Scroll( int pageCount, bool writable )
			: base( 0x14ED, pageCount, writable )
		{
			Name = "a scroll";
		}

		[Constructable]
		public Scroll( string title, string author, int pageCount, bool writable )
			: base( 0x14ED, title, author, pageCount, writable )
		{
			Name = "a scroll";
		}

		public Scroll( Serial serial )
			: base( serial )
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
