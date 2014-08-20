namespace Server.Items
{
	public class DwarvenStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new DwarvenStatueAddonDeed(); } }

		[Constructable]
		public DwarvenStatueAddon()
		{
			AddComponent( new AddonComponent( 0x237D ), -1, 1, 0 );
			AddComponent( new AddonComponent( 0x237E ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x237F ), 1, -1, 0 );

			Components.ForEach(
				delegate( AddonComponent comp )
				{
					comp.Name = "a large dwarven statue";
				} );
		}

		public DwarvenStatueAddon( Serial serial ) : base( serial ) { }

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

	public class DwarvenStatueAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new DwarvenStatueAddon(); } }

		[Constructable]
		public DwarvenStatueAddonDeed()
		{
			Name = "dwarven statue addon deed";
		}

		public DwarvenStatueAddonDeed( Serial serial ) : base( serial ) { }

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