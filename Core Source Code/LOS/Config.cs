//--------------------------------------------------------------------------------
// Copyright Joe Kraska, 2006. This file is restricted according to the GPL.
// Terms and conditions can be found in COPYING.txt.
//--------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Xml;
using Server;
using Server.Items;
using System.Collections.Generic;
//--------------------------------------------------------------------------------
namespace Server.LOS
{
	//--------------------------------------------------------------------------------
	//  Reinitialize Timer: checks periodically to see if the config file has
	//     been written; reinitializes system if it has.
	//--------------------------------------------------------------------------------
	// 
	//  Left out because an auto read in of the config would cause a matrix cache
	//  flush. On larger shards, that will cause an immediate lag that would better
	//  confined to a server restart.
	//
	//    public class ReinitializeTimer : Timer
	//    {
	//        private DateTime        m_lastWriteTime;
	//
	//        public ReinitializeTimer( ) : base( TimeSpan.FromSeconds( 0 ), TimeSpan.FromSeconds( 15 ) )
	//        { 
	//            m_lastWriteTime = File.GetLastWriteTime( RandomEncounterEngine.EncountersFile );
	//        }
	//
	//        protected override void OnTick()
	//        {
	//            DateTime        checkTime = File.GetLastWriteTime( Config.ConfigFile );
	//
	//            if( checkTime > m_lastWriteTime )
	//            {
	//                Console.WriteLine("LOS: rereading config file at {0}", Config.ConfigFile);
	//                
	//                // need to do foreach map map.Los.Clear() here.
	//
	//                Config.Clear();
	//
	//                Config config = Config.GetInstance();
	//
	//                m_lastWriteTime = checkTime;
	//            }
	//        }
	//    }
	//--------------------------------------------------------------------------------
	//  Config class; parses the xml file and establishes a hastable of tiles that
	//   are not to block line of sight.
	//--------------------------------------------------------------------------------
	public class Config
	{
		private static string m_ConfigFile = "./Data/LOS/Config.xml";
		private static Config m_OnlyInstance = null;

		private bool m_RTFM;
		private bool m_On;
		private bool m_Mobiles;
		private bool m_Items;
		private bool m_LosForMobs;
		private bool m_Symmetric;
		private bool m_HighWalls;
		private int m_SquelchNames;
		private int m_EdgeRange;
		private int m_WindowRange;
		private int m_TreeRange;
		private int m_CacheRatio;
		private bool m_BackingStore;

		private Dictionary<int, int> m_NotLossed;
		private Dictionary<int, int> m_WhiteListed;
		private Dictionary<int, int> m_BlackListed;
		private Dictionary<int, int> m_Trees;
		private Dictionary<int, int> m_Mountains;

		private Dictionary<string, Dictionary<string, string>> m_Warmups;

		private Dictionary<string, string> m_FacetsOn;

		public static string ConfigFile { get { return m_ConfigFile; } }

		public bool RTFM { get { return m_RTFM; } set { m_RTFM = value; } }
		public bool On { get { return m_On; } set { m_On = value; } }
		public bool Mobiles { get { return m_Mobiles; } }
		public bool Items { get { return m_Items; } }
		public bool LosForMobs { get { return m_LosForMobs; } }
		public bool Symmetric { get { return m_Symmetric; } }
		public bool HighWalls { get { return m_HighWalls; } }
		public int SquelchNames { get { return m_SquelchNames; } }
		public int EdgeRange { get { return m_EdgeRange; } }
		public int WindowRange { get { return m_WindowRange; } }
		public int TreeRange { get { return m_TreeRange; } }
		public int CacheRatio { get { return m_CacheRatio; } }
		public bool BackingStore { get { return m_BackingStore; } }

		public bool NotLossed( int tile ) { if( m_NotLossed.ContainsKey( tile ) ) return true; else return false; }
		public bool WhiteListed( int tile ) { if( m_WhiteListed.ContainsKey( tile ) ) return true; else return false; }
		public bool BlackListed( int tile ) { if( m_BlackListed.ContainsKey( tile ) ) return true; else return false; }
		public bool Tree( int tile ) { if( m_Trees.ContainsKey( tile ) ) return true; else return false; }
		public bool Mountain( int tile ) { if( m_Mountains.ContainsKey( tile ) ) return true; else return false; }

		public bool WarmupFacet( string facet )
		{
			if( !m_FacetsOn.ContainsKey( facet ) ) return false;

			if( m_Warmups.ContainsKey( facet ) ) return true;

			return false;
		}

		public bool WarmupRegion( string facet, string region )
		{
			if( !WarmupFacet( facet ) ) return false;

			if( region == null ) return false;

			if( m_Warmups[facet].ContainsKey( region ) ) return true;

			return false;
		}

		public bool FacetOn( string name )
		{
			return m_FacetsOn.ContainsKey( name );
		}

		public static Config GetInstance()
		{
			if( m_OnlyInstance == null ) m_OnlyInstance = new Config();

			return m_OnlyInstance;
		}

		public static void Clear()
		{
			m_OnlyInstance = null;
		}

		private Config()
		{
			m_FacetsOn = new Dictionary<string, string>();
			m_NotLossed = new Dictionary<int, int>();
			m_WhiteListed = new Dictionary<int, int>();
			m_BlackListed = new Dictionary<int, int>();
			m_Trees = new Dictionary<int, int>();
			m_Mountains = new Dictionary<int, int>();
			m_Warmups = new Dictionary<string, Dictionary<string, string>>();

			Console.Write( "LOS: Configuration system initializing..." );

			if( !MaybeLoadXml() )
				Console.WriteLine( "failed!" );
			else
				Console.WriteLine( "done" );

			if( m_RTFM == false )
			{
				Console.WriteLine(
					"\n" +
					"#### LOS: FATAL. *drum roll*.... could it BE??!?!\n" +
					"####             You haven't read and removed the RTFM=\"false\" tag in the config file!!!!\n" +
					"####             It is suggested you go read and modify your config file now.\n"
					);

				throw new Exception( "### LOS FATAL--RTFM" );
			}
		}
		private bool MaybeLoadXml()
		{
			if( !System.IO.File.Exists( m_ConfigFile ) )
			{
				Console.WriteLine(
					"\n" +
					"#### LOS: FATAL. You haven't put the Config.xml file in the right spot.\n" +
					"####             It was being looked for in Data/Los/Config.xml\n"
					);

				throw new Exception( "### LOS FATAL--CONFIG" );
			}

			return LoadXml();
		}
		//----------------------------------------------------------------------
		// Actual XML load-out and node iteration
		//----------------------------------------------------------------------
		private bool LoadXml()
		{
			XmlLinePreservingDocument xmlDoc = new XmlLinePreservingDocument( m_ConfigFile );

			try
			{
				xmlDoc.DoLoad();

				string rtfm = "true";
				string on = "false";
				string facetsOn = "Felucca";
				string mobiles = "true";
				string items = "true";
				string losForMobs = "true";
				string symmetric = "false";
				string highWalls = "true";
				string squelchNames = "-1";
				string edgeRange = "1";
				string windowRange = "1";
				string treeRange = "1";
				string cacheRatio = "100";
				string backingStore = "true";

				XmlNode root = xmlDoc["LOS"];

				try { rtfm = root.Attributes["RTFM"].Value; }
				catch { }
				try { on = root.Attributes["on"].Value; }
				catch { }
				try { facetsOn = root.Attributes["facets"].Value; }
				catch { }
				try { mobiles = root.Attributes["mobiles"].Value; }
				catch { }
				try { items = root.Attributes["items"].Value; }
				catch { }
				try { losForMobs = root.Attributes["losForMobs"].Value; }
				catch { }
				try { symmetric = root.Attributes["symmetric"].Value; }
				catch { }
				try { highWalls = root.Attributes["highWalls"].Value; }
				catch { }
				try { squelchNames = root.Attributes["squelchNames"].Value; }
				catch { }
				try { edgeRange = root.Attributes["edgeRange"].Value; }
				catch { }
				try { windowRange = root.Attributes["windowRange"].Value; }
				catch { }
				try { treeRange = root.Attributes["treeRange"].Value; }
				catch { }
				try { cacheRatio = root.Attributes["cacheRatio"].Value; }
				catch { }
				try { backingStore = root.Attributes["backingStore"].Value; }
				catch { }

				m_RTFM = bool.Parse( rtfm );
				m_On = bool.Parse( on );
				m_Mobiles = bool.Parse( mobiles );
				m_Items = bool.Parse( items );
				m_LosForMobs = bool.Parse( losForMobs );
				m_Symmetric = bool.Parse( symmetric );
				m_HighWalls = bool.Parse( highWalls );
				m_SquelchNames = int.Parse( squelchNames );
				m_EdgeRange = int.Parse( edgeRange );
				m_WindowRange = int.Parse( windowRange );
				m_TreeRange = int.Parse( treeRange );
				m_CacheRatio = int.Parse( cacheRatio );
				m_BackingStore = bool.Parse( backingStore );

				string[] facets = facetsOn.Split( new Char[] { ',' } );

				foreach( string facet in facets )
				{
					m_FacetsOn.Add( facet, facet );
				}

				//--------------------------------------------------------------
				// Load various tile sets
				//--------------------------------------------------------------

				LoadTileList( root, "NotLossed", NumberStyles.HexNumber, m_NotLossed );
				LoadTileList( root, "WhiteList", NumberStyles.HexNumber, m_WhiteListed );
				LoadTileList( root, "BlackList", NumberStyles.HexNumber, m_BlackListed );
				LoadTileList( root, "Trees", NumberStyles.HexNumber, m_Trees );
				LoadTileList( root, "Mountains", NumberStyles.None, m_Mountains );

				LoadWarmups( root );

				return true;
			}
			catch( Exception e )
			{
				Console.WriteLine( "LOS: Exception encountered attempting to load file: " + m_ConfigFile );
				Console.WriteLine( "{0}", e );
				xmlDoc.Close();
				return false;
			}
		}
		//----------------------------------------------------------------------
		//----------------------------------------------------------------------
		private void LoadTileList( XmlNode parent, string listName, NumberStyles style, Dictionary<int, int> loadInto )
		{
			XmlNodeList treeNodes = parent.SelectNodes( "./" + listName );

			if( treeNodes.Count == 1 )
			{
				XmlNode whiteNode = treeNodes[0];

				XmlNodeList listedTileNodes = whiteNode.SelectNodes( "./Tile" );

				//if (listedTileNodes.Count==0) Console.WriteLine( "LOS: There appeared to be NO {0} TILES defined in the configuration file!", listName);
				//------------------------------------------------------------------
				// Iterate over tiles
				//------------------------------------------------------------------
				foreach( XmlNode tileNode in listedTileNodes )
				{
					string str_id = "";

					try { str_id = tileNode.Attributes["id"].Value; }
					catch
					{
						Console.WriteLine(
							"LOS: Tile at {0} had no id. Ignoring tile entry!",
							tileNode.Attributes["lineNumber"].Value
							);
						continue;
					}

					int id = Int16.Parse( str_id, style );

					if( loadInto.ContainsKey( id ) )
						Console.WriteLine( "LOS: ignoring duplicate key \"{0}\" in list {1}", id, listName );
					else
						loadInto.Add( id, id );
				}
			}
			//else Console.WriteLine("LOS: Warning, there appeared to be no {0} defined in the config file.", listName);
		}
		//----------------------------------------------------------------------
		//----------------------------------------------------------------------
		private void LoadWarmups( XmlNode parent )
		{
			XmlNodeList warmupNodes = parent.SelectNodes( "./Warmups" );

			if( warmupNodes.Count == 1 )
			{
				XmlNode warmupNode = warmupNodes[0];

				XmlNodeList warmups = warmupNode.SelectNodes( "./Warmup" );

				//if (warmups.Count==0) Console.WriteLine( "LOS: There appeared to be NO WARMUP defined in the configuration file!");
				//------------------------------------------------------------------
				// Iterate over tiles
				//------------------------------------------------------------------
				foreach( XmlNode warmup in warmups )
				{
					string map = "";
					string region = "";

					try { map = warmup.Attributes["map"].Value; }
					catch
					{
						Console.WriteLine(
							"LOS: Warmup at {1} had no \"map\" attr. THIS IS ILLEGAL. IGNORED ENTIRE WARMUP!",
							warmup.Attributes["lineNumber"].Value
							);
						continue;
					}

					try { region = warmup.Attributes["region"].Value; }
					catch
					{
						Console.WriteLine(
							"LOS: Warmup at {1} had no \"region\" attr. THIS IS ILLEGAL. IGNORED ENTIRE WARMUP!",
							warmup.Attributes["lineNumber"].Value
							);
						continue;
					}

					Dictionary<string, string> regionHash;

					if( m_Warmups.ContainsKey( map ) )
					{
						regionHash = m_Warmups[map];
					}
					else
					{
						regionHash = new Dictionary<string, string>();
						m_Warmups.Add( map, regionHash );
					}

					regionHash.Add( region, region );
				}
			}
			//else Console.WriteLine("LOS: Warning, there appeared to be no Warmups section defined in the config file.");
		}
	}
	//--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------
