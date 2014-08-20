using System;
using System.Collections.Generic;
using Server.Items;
using System.IO;

namespace Server
{
	[Flags]
	public enum GrabFlag
	{
		Everything	= 0x0001,
		Coins		= 0x0002,
		Armor		= 0x0004,
		Clothing	= 0x0008,
		Jewelry		= 0x0010,
		Weapons		= 0x0020,
		Gems		= 0x0040,
		Instruments	= 0x0060,
		Potions		= 0x0080,
		Reagents	= 0x0100,
		Scrolls		= 0x0200
	}

	public class Grab
	{
		internal static string PersistencePath = "Saves\\Grab";
		internal static string PersistenceFile = Path.Combine( PersistencePath, "grab.bin" );

		private static Dictionary<Mobile, GrabOptions> _optTable = new Dictionary<Mobile, GrabOptions>();

		public static Dictionary<Mobile, GrabOptions> OptionsTable { get { return _optTable; } }

		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler( event_worldLoad );
			EventSink.WorldSave += new WorldSaveEventHandler( event_worldSave );
		}

		#region +static string GetContainerName( Mobile, GrabFlag )
		public static string GetContainerName( Mobile m, GrabFlag flag )
		{
			GrabOptions options = GetOptions( m );
			Container cont = options.GetPlacementContainer( flag );
			string res = "(not set)";

			if( cont == null || !cont.IsChildOf( m.Backpack ) )
				res = "(not set)";
			else if( cont == m.Backpack )
				res = "(main backpack)";
			else if( !String.IsNullOrEmpty( cont.Name ) )
				res = cont.Name;
			else
				res = cont.ItemData.Name;

			return res;
		} 
		#endregion

		#region +static GrabFlag ParseInt32( int )
		public static GrabFlag ParseInt32( int num )
		{
			GrabFlag flag = (GrabFlag)0;

			switch( num )
			{
				case 1: flag = GrabFlag.Everything; break;
				case 2: flag = GrabFlag.Coins; break;
				case 3: flag = GrabFlag.Armor; break;
				case 4: flag = GrabFlag.Clothing; break;
				case 5: flag = GrabFlag.Jewelry; break;
				case 6: flag = GrabFlag.Weapons; break;
				case 7: flag = GrabFlag.Gems; break;
				case 8: flag = GrabFlag.Instruments; break;
				case 9: flag = GrabFlag.Potions; break;
				case 10: flag = GrabFlag.Reagents; break;
				case 11: flag = GrabFlag.Scrolls; break;
			}

			return flag;
		} 
		#endregion

		#region +static GrabFlag ParseType( Item )
		public static GrabFlag ParseType( Item i )
		{
			GrabFlag flag = GrabFlag.Everything;

			if( i is Server.Currency.BaseCoin )
				flag = GrabFlag.Coins;
			else if( i is BaseArmor )
				flag = GrabFlag.Armor;
			else if( i is BaseClothing )
				flag = GrabFlag.Clothing;
			else if( i is BaseJewel )
				flag = GrabFlag.Jewelry;
			else if( i is BaseWeapon )
				flag = GrabFlag.Weapons;
			else if( i.ItemID >= 3855 && i.ItemID <= 3888 )
				flag = GrabFlag.Gems;
			else if( i is BaseInstrument )
				flag = GrabFlag.Instruments;
			else if( i is BasePotion )
				flag = GrabFlag.Potions;
			else if( i is BaseReagent )
				flag = GrabFlag.Reagents;
			else if( i is SpellScroll )
				flag = GrabFlag.Scrolls;

			return flag;
		} 
		#endregion

		#region GrabOptions operations
		public static GrabOptions GetOptions( Mobile m )
		{
			if( !OptionsTable.ContainsKey( m ) || OptionsTable[m] == null )
				OptionsTable[m] = new GrabOptions( m );

			return OptionsTable[m];
		}

		public static void SaveOptions( Mobile m, GrabOptions options )
		{
			OptionsTable[m] = options;
		} 
		#endregion

		#region persistence
		private static void event_worldLoad()
		{
			if( !File.Exists( PersistenceFile ) )
				return;

			using( FileStream stream = new FileStream( PersistenceFile, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				BinaryFileReader reader = new BinaryFileReader( new BinaryReader( stream ) );

				int count = reader.ReadInt();

				for( int i = 0; i < count; i++ )
				{
					int serial = reader.ReadInt();

					if( serial > -1 )
					{
						Mobile m = World.FindMobile( serial );
						GrabOptions options = new GrabOptions( reader );

						if( m != null && !m.Deleted )
						{
							if( options != null && !OptionsTable.ContainsKey( m ) )
								OptionsTable.Add( m, options );
						}
					}
				}

				reader.Close();
			}
		}

		private static void event_worldSave( WorldSaveEventArgs args )
		{
			if( !Directory.Exists( PersistencePath ) )
				Directory.CreateDirectory( PersistencePath );

			BinaryFileWriter writer = new BinaryFileWriter( PersistenceFile, true );

			writer.Write( OptionsTable.Count );

			if( OptionsTable.Count > 0 )
			{
				foreach( KeyValuePair<Mobile, GrabOptions> kvp in OptionsTable )
				{
					if( kvp.Key == null || kvp.Key.Deleted || kvp.Value == null )
					{
						writer.Write( (int)-1 );
					}
					else
					{
						writer.Write( (int)kvp.Key.Serial );
						kvp.Value.Serialize( writer );
					}
				}
			}

			writer.Close();
		} 
		#endregion
	}
}