using System;
using Server;

namespace Server.Items
{
	public class Saddle : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SaddleDeed(); } }

		[Constructable]
		public Saddle()
		{

			AddComponent( new AddonComponent( 0x0F38 ), 0, 0, 0 );

		}

		public Saddle( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			Mobile m_Placer = from;

			if( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
				{
					from.AddToBackpack( new SaddleDeed() );

					this.Delete();

					from.SendMessage( "A deed for the item has been placed in your backpack." );
				}
				else
				{
					from.SendMessage( "You cannot redeed this item." ); // You cannot take this tree down.
				}
			}
			else
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
			}
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

	public class SaddleDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new Saddle(); } }

		[Constructable]
		public SaddleDeed()
		{
			Name = "Saddle";
		}

		public SaddleDeed( Serial serial )
			: base( serial )
		{

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