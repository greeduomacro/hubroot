using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x11F4, 0x11F8 )]
	public class DarkFur1 : Item
	{
		[Constructable]
		public DarkFur1()
			: base( 0x11F8 )
		{
			Name = "a dark fur";
			Weight = 7.0;
		}

		public DarkFur1( Serial serial )
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
	}

	[FlipableAttribute( 0x11F5, 0x11F9 )]
	public class DarkFur2 : Item
	{
		[Constructable]
		public DarkFur2()
			: base( 0x11F9 )
		{
			Name = "a dark fur";
			Weight = 7.0;
		}

		public DarkFur2( Serial serial )
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
	}

	[FlipableAttribute( 0x11F6, 0x11FA )]
	public class LightFur1 : Item
	{
		[Constructable]
		public LightFur1()
			: base( 0x11FA )
		{
			Name = "a light fur";
			Weight = 7.0;
		}

		public LightFur1( Serial serial )
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
	}

	[FlipableAttribute( 0x11F7, 0x11FB )]
	public class LightFur2 : Item
	{
		[Constructable]
		public LightFur2()
			: base( 0x11FB )
		{
			Name = "a light fur";
			Weight = 7.0;
		}

		public LightFur2( Serial serial )
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
	}
}
