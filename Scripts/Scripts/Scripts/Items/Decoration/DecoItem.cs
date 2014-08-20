using System;
using Server;

namespace Server.Items
{
	public class DecoItem : Item
	{
		private static int[] allDecoItems = new int[]
		{
			0x1b0b,
			0x1b0f,
			0x1b1b,
			0x1cdd,
			0x1cde,
			0x1ce0,
			0x1ce1,
			0x1ce5,
			0x1ce8,
			0x1cf0,
			0x1cf3,
			0x1cf6,
			0x1cf7,
			0x1d11,
			0x1d9f,
			0x1da0,
			0x1da1,
			0x1da2,
			0x1da3,
			0x1da4
		};

		[Constructable]
		public DecoItem()
			: base( Utility.RandomList( allDecoItems ) )
		{
			Movable = false;
		}

		public DecoItem( Serial serial )
			: base( serial )
		{
		}

		public override bool Decays
		{
			get { return false; }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}