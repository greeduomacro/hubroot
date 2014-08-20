namespace Server.Items
{
	public class WomanStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new WomanStatueAddonDeed(); } }

		[Constructable]
		public WomanStatueAddon()
		{
			AddComponent( new AddonComponent( 0x2CB0 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0x2CB1 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x2CB2 ), 1, -1, 0 );

			Components.ForEach(
				delegate( AddonComponent comp )
				{
					comp.Name = "a statue";
				} );
		}

		public WomanStatueAddon( Serial serial ) : base( serial ) { }

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

	public class WomanStatueAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new WomanStatueAddon(); } }

		[Constructable]
		public WomanStatueAddonDeed()
		{
			Name = "statue addon deed";
		}

		public WomanStatueAddonDeed( Serial serial ) : base( serial ) { }

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