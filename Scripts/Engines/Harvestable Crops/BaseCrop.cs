
namespace Server.Engines.Crops
{
	public class BaseCrop : Item
	{
		public BaseCrop( int itemID )
			: base( itemID )
		{
			Movable = false;
		}

		public BaseCrop( Serial serial )
			: base( serial )
		{
		}

		public virtual bool CanGrow( Point3D location, Map map )
		{
			return (map != null && ValidateTiles( location, map ));
		}

		private static int[] validTiles = new int[]
			{
				//furrows
				0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0x10,
				0x11, 0x12, 0x13, 0x14, 0x15, 0x150, 0x151,
				0x152, 0x153, 0x154, 0x155, 0x156, 0x157,
				0x158, 0x159, 0x15A, 0x15B, 0x15C, 0x32C9,
				0x32CA,
				//grass/ground tiles
				0x3, 0x4, 0x5, 0x6, 0x37B, 0x37C, 0x37D,
				0x37E
			};

		public static bool ValidateTiles( Point3D loc, Map map )
		{
			StaticTile[] statics = map.Tiles.GetStaticTiles( loc.X, loc.Y );
			int tileID = (map.Tiles.GetLandTile( loc.X, loc.Y ).ID);
			bool valid = false;

			for( int i = 0; !valid && i < validTiles.Length; i++ )
				valid = (tileID == validTiles[i]);

			for( int i = 0; !valid && i < validTiles.Length; i++ )
			{
				for( int j = 0; !valid && j < statics.Length; j++ )
					valid = (validTiles[i] == statics[j].ID);
			}

			return valid;
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