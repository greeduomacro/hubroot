using System;

namespace Server.Items
{
	public class FullDeskAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new FullDeskAddonDeed( false ); } }

		[Constructable]
		public FullDeskAddon( bool southFacing )
		{
			if( southFacing )
			{
				AddComponent( new AddonComponent( 0x2CA8 ), 0, 0, 0 );
				AddComponent( new AddonComponent( 0x2CA9 ), 1, 0, 0 );
			}
			else
			{
				AddComponent( new AddonComponent( 0x2CA6 ), 0, 0, 0 );
				AddComponent( new AddonComponent( 0x2CA7 ), 0, 1, 0 );
			}
		}

		public FullDeskAddon( Serial serial ) : base( serial ) { }

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

	public class FullDeskAddonDeed : BaseAddonDeed
	{
		private bool _southFacing;

		[CommandProperty( AccessLevel.Counselor )]
		public bool SouthFacing { get { return _southFacing; } set { _southFacing = value; } }

		public override BaseAddon Addon { get { return new FullDeskAddon( this.SouthFacing ); } }

		[Constructable]
		public FullDeskAddonDeed( bool southFacing )
		{
			_southFacing = southFacing;

			Name = String.Format( "desk addon [{0}]", (southFacing ? "South" : "East") );
		}

		public FullDeskAddonDeed( Serial serial ) : base( serial ) { }

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