using System;

namespace Server.Items
{
	public class RottenHeart : Item
	{
		private Mobile m_Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

		[Constructable]
		public RottenHeart()
			: base( 3985 )
		{
			Weight = 1.0;
			Name = "rotten heart";
			Hue = 996;
		}

		public RottenHeart( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
			writer.Write( (Mobile)m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Owner = (Mobile)reader.ReadMobile();
		}
	}
}