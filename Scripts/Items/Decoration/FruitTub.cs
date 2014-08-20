namespace Server.Items
{
	public class FruitTub : BaseContainer
	{
		public enum FruitType : byte
		{
			Apples,
			Bananas,
			Bread,
			Gourds,
			Grapes,
			Lemons,
			Tomatoes,
			Vegetables1,
			Vegetables2,
			Vegetables3
		}

		private FruitType _type;

		[CommandProperty( AccessLevel.GameMaster )]
		public FruitType Type { get { return _type; } set { SetType( value ); } }

		[Constructable]
		public FruitTub() : this( FruitType.Apples ) { }

		[Constructable]
		public FruitTub( FruitType type )
			: base( 0x2D7A )
		{
			Name = GetName( type );
			Weight = 5.0;

			InitContents( type );
			SetType( type );
		}

		public FruitTub( Serial serial ) : base( serial ) { }

		#region -string GetName( FruitType )
		private string GetName( FruitType type )
		{
			string name = "";

			switch( type )
			{
				case FruitType.Apples: name = "a tub of apples"; break;
				case FruitType.Bananas: name = "a tub of bananas"; break;
				case FruitType.Bread: name = "a tub of bread loaves"; break;
				case FruitType.Gourds: name = "a tub of gourds"; break;
				case FruitType.Grapes: name = "a tub of grapes"; break;
				case FruitType.Lemons: name = "a tub of lemons"; break;
				case FruitType.Tomatoes: name = "a tub of tomatoes"; break;
				case FruitType.Vegetables1: name = "a tub of vegetables"; break;
				case FruitType.Vegetables2: name = "a tub of vegetables"; break;
				case FruitType.Vegetables3: name = "a tub of vegetables"; break;
			}

			return name;
		} 
		#endregion

		#region -void InitContents( FruitType )
		private void InitContents( FruitType type )
		{
			Food item = null;
			byte count = (byte)Utility.RandomMinMax( 10, 30 );

			for( byte i = 0; i < count; i++ )
			{
				switch( type )
				{
					default:
					case FruitType.Apples: item = new Apple(); break;
					case FruitType.Bananas: item = new Banana(); break;
					case FruitType.Bread: item = new BreadLoaf(); break;
					case FruitType.Gourds: item = new Gourd(); break;
					case FruitType.Grapes: item = new Grapes(); break;
					case FruitType.Lemons: item = new Lemon(); break;
					case FruitType.Tomatoes: item = new Tomato(); break;
					case FruitType.Vegetables1:
					case FruitType.Vegetables2:
					case FruitType.Vegetables3:
						{
							switch( Utility.Random( 4 ) )
							{
								case 0: item = new Carrot(); break;
								case 1: item = new Onion(); break;
								case 2: item = new Pumpkin(); break;
								case 3: item = new Gourd(); break;
							}
							break;
						}
				}

				if( item != null )
					DropItem( item );
			}
		} 
		#endregion

		#region -void SetType( FruitType )
		private void SetType( FruitType type )
		{
			int itemId = 0;

			switch( type )
			{
				case FruitType.Apples: itemId = 0x2D7A; break;
				case FruitType.Bananas: itemId = 0x2D7B; break;
				case FruitType.Bread: itemId = 0x2D76; break;
				case FruitType.Gourds: itemId = 0x2D7E; break;
				case FruitType.Grapes: itemId = 0x2D7D; break;
				case FruitType.Lemons: itemId = 0x2D7C; break;
				case FruitType.Tomatoes: itemId = 0x2D75; break;
				case FruitType.Vegetables1: itemId = 0x2D77; break;
				case FruitType.Vegetables2: itemId = 0x2D78; break;
				case FruitType.Vegetables3: itemId = 0x2D79; break;
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

			Type = (FruitType)reader.ReadByte();
		}
		#endregion
	}
}