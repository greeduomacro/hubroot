using System;

namespace Server.Items
{
	public class EmptyDeskAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new EmptyDeskAddonDeed( false ); } }

		[Constructable]
		public EmptyDeskAddon( bool southFacing )
		{
			if( southFacing )
			{
				AddComponent( new AddonComponent( 0x2CA4 ), 0, 0, 0 );
				AddComponent( new AddonComponent( 0x2CA5 ), 1, 0, 0 );
			}
			else
			{
				AddComponent( new AddonComponent( 0x2CA2 ), 0, 0, 0 );
				AddComponent( new AddonComponent( 0x2CA3 ), 0, 1, 0 );
			}
		}

		public EmptyDeskAddon( Serial serial ) : base( serial ) { }

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

	public class EmptyDeskAddonDeed : BaseAddonDeed
	{
		private bool _southFacing;

		[CommandProperty( AccessLevel.Counselor )]
		public bool SouthFacing { get { return _southFacing; } set { _southFacing = value; } }

		public override BaseAddon Addon { get { return new EmptyDeskAddon( this.SouthFacing ); } }

		[Constructable]
		public EmptyDeskAddonDeed( bool southFacing )
		{
			_southFacing = southFacing;

			Name = String.Format( "empty desk addon [{0}]", (southFacing ? "South" : "East") );
		}

		public EmptyDeskAddonDeed( Serial serial ) : base( serial ) { }

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