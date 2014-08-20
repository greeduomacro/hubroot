using System.Collections.Generic;
using Server.Items;

namespace Server
{
	public class GrabOptions
	{
		private Dictionary<GrabFlag, Container> _containerTable;
		private GrabFlag _flags;
		private Mobile _mobile;

		public Dictionary<GrabFlag, Container> ContainerTable { get { return _containerTable; } protected set { _containerTable = value; } }
		public GrabFlag Flags { get { return _flags; } protected set { _flags = value; } }
		public Mobile Mobile { get { return _mobile; } protected set { _mobile = value; } }

		public GrabOptions( Mobile m ) : this( m, GrabFlag.Everything ) { }

		public GrabOptions( Mobile m, GrabFlag flags )
		{
			_containerTable = new Dictionary<GrabFlag, Container>();
			_flags = flags;
			_mobile = m;
		}

		#region +bool IsLootable( Item )
		public bool IsLootable( Item i )
		{
			if( GetFlag( GrabFlag.Everything ) )
				return true;
			else if( i is Server.Currency.BaseCoin && GetFlag( GrabFlag.Coins ) )
				return true;
			else if( i is BaseArmor && GetFlag( GrabFlag.Armor ) )
				return true;
			else if( i is BaseClothing && GetFlag( GrabFlag.Clothing ) )
				return true;
			else if( i is BaseJewel && GetFlag( GrabFlag.Jewelry ) )
				return true;
			else if( i is BaseWeapon && GetFlag( GrabFlag.Weapons ) )
				return true;
			else if( (i.ItemID >= 3855 && i.ItemID <= 3888) && GetFlag( GrabFlag.Gems ) )
				return true;
			else if( i is BaseInstrument && GetFlag( GrabFlag.Instruments ) )
				return true;
			else if( i is BasePotion && GetFlag( GrabFlag.Potions ) )
				return true;
			else if( i is BaseReagent && GetFlag( GrabFlag.Reagents ) )
				return true;
			else if( i is SpellScroll && GetFlag( GrabFlag.Scrolls ) )
				return true;

			return false;
		} 
		#endregion

		#region GrabFlag operations
		public bool GetFlag( GrabFlag flag )
		{
			return ((Flags & flag) != 0);
		}

		public void SetFlag( GrabFlag flag, bool value )
		{
			if( value )
			{
				if( flag == GrabFlag.Everything )
					Flags = GrabFlag.Everything;
				else
					Flags |= flag;
			}
			else
			{
				Flags &= flag;
			}
		}

		public void ResetFlags()
		{
			Flags = 0;
		} 
		#endregion

		#region PlacementContainer operations
		public Container GetPlacementContainer( GrabFlag flag )
		{
			if( ContainerTable.ContainsKey( flag ) )
				return ContainerTable[flag];

			return null;
		}

		public void SetPlacementContainer( GrabFlag flag, Container cont )
		{
			ContainerTable[flag] = cont;
		} 
		#endregion

		public virtual void Serialize( BinaryFileWriter writer )
		{
			writer.Write( (int)0 );

			//version 0
			writer.Write( (int)ContainerTable.Count );

			if( ContainerTable.Count > 0 )
			{
				foreach( KeyValuePair<GrabFlag, Container> kvp in ContainerTable )
				{
					writer.Write( (int)kvp.Key );
					writer.Write( kvp.Value );
				}
			}

			writer.Write( (int)Flags );
			writer.Write( Mobile );
		}

		public GrabOptions( BinaryFileReader reader )
		{
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						int tblCount = reader.ReadInt();
						ContainerTable = new Dictionary<GrabFlag, Container>( tblCount );

						for( int i = 0; i < tblCount; i++ )
						{
							ContainerTable.Add( (GrabFlag)reader.ReadInt(), (Container)reader.ReadItem() );
						}

						Flags = (GrabFlag)reader.ReadInt();
						Mobile = reader.ReadMobile();

						break;
					}
			}
		}
	}
}