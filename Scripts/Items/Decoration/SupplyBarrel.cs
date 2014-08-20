namespace Server.Items
{
	public class SupplyBarrel : Container
	{
		public enum BarrelType : byte
		{
			Arrows,
			Bolts,
			Farming,
			LargeMining,
			LargeWeapon1,
			LargeWeapon2,
			LargeWeapon3,
			Mining,
			Spears,
			Weapons
		}

		private BarrelType _type;

		[CommandProperty( AccessLevel.GameMaster )]
		public BarrelType Type { get { return _type; } set { SetType( value ); } }

		[Constructable]
		public SupplyBarrel() : this( BarrelType.Arrows ) { }

		[Constructable]
		public SupplyBarrel( BarrelType type )
			: base( 0x2CDD )
		{
			Name = GetName( type );
			Weight = 10.0;

			InitContents( type );
			SetType( type );
		}

		public SupplyBarrel( Serial serial ) : base( serial ) { }

		#region -string GetName( BarrelType )
		private string GetName( BarrelType type )
		{
			string name = "";

			switch( type )
			{
				case BarrelType.Arrows: name = "a barrel of arrows"; break;
				case BarrelType.Bolts: name = "a barrel of bolts"; break;
				case BarrelType.Farming: name = "a farming supply barrel"; break;
				case BarrelType.LargeMining: name = "a large mining supply barrel"; break;
				case BarrelType.LargeWeapon1: name = "a large weapon barrel"; break;
				case BarrelType.LargeWeapon2: name = "a large weapon barrel"; break;
				case BarrelType.LargeWeapon3: name = "a large weapon barrel"; break;
				case BarrelType.Mining: name = "a mining supply barrel"; break;
				case BarrelType.Spears: name = "a barrel of spears"; break;
				default:
				case BarrelType.Weapons: name = "a barrel of weapons"; break;
			}

			return name;
		}
		#endregion

		#region -void InitContents( BarrelType )
		private void InitContents( BarrelType type )
		{
			Item item = null;
			byte count = (byte)Utility.RandomMinMax( 10, 30 );

			for( byte i = 0; i < count; i++ )
			{
				switch( type )
				{
					default:
					case BarrelType.Arrows: item = new Arrow( Utility.RandomMinMax( 2, 6 ) ); break;
					case BarrelType.Bolts: item = new Bolt( Utility.RandomMinMax( 2, 6 ) ); break;
					case BarrelType.Farming:
						{
							if( i > 3 )
								return;

							switch( Utility.Random( 3 ) )
							{
								case 0: item = new Shovel(); break;
								case 1: item = new Scythe(); break;
								case 2: item = new Pitchfork(); break;
							}
							break;
						}
					case BarrelType.LargeMining:
						{
							if( i > 5 )
								return;

							switch( Utility.Random( 5 ) )
							{
								case 0: item = new Pitchfork(); break;
								case 1:
								case 2: item = new Shovel(); break;
								case 3:
								case 4: item = new Pickaxe(); break;
							}
							break;
						}
					case BarrelType.LargeWeapon1:
					case BarrelType.LargeWeapon2:
					case BarrelType.LargeWeapon3:
						{
							if( i > 6 )
								return;

							switch( Utility.Random( 6 ) )
							{
								case 0: item = new Spear(); break;
								case 1: item = new Halberd(); break;
								case 2: item = new Axe(); break;
								case 3: item = new ThinLongsword(); break;
								case 4: item = new WarAxe(); break;
								case 5: item = new VikingSword(); break;
							}
							break;
						}
					case BarrelType.Mining:
						{
							if( i > 3 )
								return;

							item = new Pickaxe();
							break;
						}
					case BarrelType.Spears:
						{
							if( i > 4 )
								return;

							item = new Spear();
							break;
						}
					case BarrelType.Weapons:
						{
							if( i > 3 )
								return;

							switch( Utility.Random( 3 ) )
							{
								case 0: item = new WarAxe(); break;
								case 1: item = new WarMace(); break;
								case 2: item = new Maul(); break;
							}
							break;
						}
				}

				if( item != null )
					DropItem( item );
			}
		}
		#endregion

		#region -void SetType( BarrelType )
		private void SetType( BarrelType type )
		{
			int itemId = 0;

			switch( type )
			{
				case BarrelType.Arrows: itemId = 0x2CDD; break;
				case BarrelType.Bolts: itemId = 0x2CDE; break;
				case BarrelType.Farming: itemId = 0x2CE5; break;
				case BarrelType.LargeMining: itemId = 0x2CE4; break;
				case BarrelType.LargeWeapon1: itemId = 0x2CE1; break;
				case BarrelType.LargeWeapon2: itemId = 0x2CE2; break;
				case BarrelType.LargeWeapon3: itemId = 0x2CE3; break;
				case BarrelType.Mining: itemId = 0x2CDC; break;
				case BarrelType.Spears: itemId = 0x2CE0; break;
				default:
				case BarrelType.Weapons: itemId = 0x2CDF; break;
			}

			ItemID = itemId;
			_type = type;
		} 
		#endregion

		#region serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );

			writer.Write( (byte)Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			Type = (BarrelType)reader.ReadByte();
		} 
		#endregion
	}
}