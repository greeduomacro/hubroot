using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xE41, 0xE40 )]   // metal & gold 
	public class BankChest : Item
	{
		[Constructable]
		public BankChest()
			: this( 0xE40 )
		{
		}

		[Constructable]
		public BankChest( int itemID )
			: base( itemID )
		{
			Movable = false;
			Name = "a bank chest";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( this.GetWorldLocation(), 4 ) )
			{
				BankBox box = from.BankBox;
				if( from.Criminal )
				{
					from.SendMessage( "Thou canst not deposit or withdraw from thy bank box if thou art a criminal!" );
				}
				else if( box != null )
				{
					box.Open();
				}
				else
				{
					from.SendMessage( "Error! Thy bank box was not found!" );
				}
			}
			else
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away. 
			}
		}

		public BankChest( Serial serial )
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
