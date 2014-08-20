using System;
using Server;

namespace Server.Items
{
	public class PearlOyster : Item, ICarvable
	{
		[Constructable]
		public PearlOyster()
			: base( 0x1F13 )
		{
			Hue = 943;
			Name = "a black-shelled oyster";
			Weight = 0.5;
		}

		public PearlOyster( Serial serial ) : base( serial ) { }

		public void Carve( Mobile from, Item item )
		{
			from.SendMessage( "You pry open the oyster and extract several small black pearls from within." );
			from.AddToBackpack( new BlackPearl( Utility.RandomMinMax( 4, 8 ) ) );

			Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "You will need to pry it open with a bladed item." );
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
	}
}