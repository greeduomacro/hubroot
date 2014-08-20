using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BindingCrystal : Item
	{
		private Mobile m_To;

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public Mobile Target { get { return m_To; } set { m_To = value; } }

		public override bool DisplayLootType { get { return false; } }

		[Constructable]
		public BindingCrystal()
			: base( 0x1F19 )
		{
			Name = "an enchanted crystal";
			Hue = 1156;
			LootType = LootType.Blessed;
		}

		public BindingCrystal( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack to use it.
			}
			else if( Target == null || Target.Map == null || Target.Map == Map.Internal )
			{
				from.SendMessage( "The target map is unavailable." );
			}
			else
			{
				from.MoveToWorld( Target.Location, Target.Map );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if( Target != null && Target.Map != null )
				list.Add( 1049644, Target.Map.ToString() ); // [~stuff~]
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( (Mobile)m_To );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_To = reader.ReadMobile();
		}
	}
}