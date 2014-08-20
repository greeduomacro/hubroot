namespace Server.Items
{
	public class AngelStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new AngelStatueAddonDeed(); } }

		[Constructable]
		public AngelStatueAddon()
		{
			AddComponent( new AddonComponent( 0x2CAD ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0x2CAE ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x2CAF ), 1, -1, 0 );

			Components.ForEach(
				delegate( AddonComponent comp )
				{
					comp.Name = "a statue";
				} );
		}

		public AngelStatueAddon( Serial serial ) : base( serial ) { }

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

	public class AngelStatueAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new AngelStatueAddon(); } }

		[Constructable]
		public AngelStatueAddonDeed()
		{
			Name = "statue addon deed";
		}

		public AngelStatueAddonDeed( Serial serial ) : base( serial ) { }

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