using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Network;
using Server.Regions;

namespace Server.Utilities
{
	public class RegionPopularityWatcher : Timer
	{
		public static readonly bool Enabled = false;

		private static string PersistanceDirectory = "Data/Stats";
		private static string PersistancePath = Path.Combine( PersistanceDirectory, "regionWatcher.xml" );
		private static string RootElementName = "regionWatcher";
		private static string ChildElementName = "region";
		private static Map[] ExcludedMaps = new Map[] { };

		private List<RegionWatcherObject> _regions = new List<RegionWatcherObject>();
		private int _totalRegionChecks = 0;

		public List<RegionWatcherObject> Regions { get { return _regions; } set { _regions = value; } }
		public int TotalChecks { get { return _totalRegionChecks; } set { _totalRegionChecks = value; } }

		public static void Initialize()
		{
			if( Enabled )
				RegionPopularityWatcher.Load( true );
		}

		public RegionPopularityWatcher()
			: base( TimeSpan.FromMinutes( 5.0 ), TimeSpan.FromMinutes( 30.0 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			List<NetState> users = new List<NetState>( NetState.Instances );
			Mobile m;
			Region reg;
			RegionWatcherObject obj;

			for( int i = 0; i < users.Count; i++ )
			{
				reg = null;
				obj = null;

				m = users[i].Mobile;

				if( m == null || IgnoreMap( m.Map ) )
					continue;

				reg = FindRegion( m.Location, m.Map );

				if( reg == null )
					continue;

				_totalRegionChecks++;

				if( RegionCollected( reg, out obj ) )
					obj.Count++;
				else
				{
					obj = new RegionWatcherObject( reg.Name, reg.Map, 1 );
					_regions.Add( obj );
				}
			}

			CalculatePopularity();

			try
			{
				SortStats( _regions );
				SaveStats( _regions, _totalRegionChecks );
			}
			catch( Exception e )
			{
				ExceptionManager.LogException( "RegionPopularityWatcher", e );
			}
		}

		#region -Region FindRegion( Point3D, Map )
		private Region FindRegion( Point3D location, Map map )
		{
			Region reg = Region.Find( location, map );

			if( reg is CustomRegion )
			{
				Sector s = map.GetSector( location );
				int highestPriority = 0;

				for( int i = 0; i < s.RegionRects.Count; i++ )
				{
					if( s.RegionRects[i].Region.Priority > highestPriority )
					{
						highestPriority = s.RegionRects[i].Region.Priority;
						reg = s.RegionRects[i].Region;
					}
				}
			}

			if( reg == null || String.IsNullOrEmpty( reg.Name ) || reg.Name == "world" )
				return null;

			return reg;
		}
		#endregion

		#region -bool RegionCollected( Region, out RegionWatcherObject )
		private bool RegionCollected( Region r, out RegionWatcherObject foundObject )
		{
			RegionWatcherObject found = null;

			if( r.Name == "Isles of Jun'kol" )
			{
				found = _regions.Find(
					delegate( RegionWatcherObject obj )
					{
						return (obj.Name == "Jun'kol");
					} );
			}
			else
			{
				for( int i = 0; found == null && i < _regions.Count; i++ )
				{
					if( _regions[i].Name == r.Name && _regions[i].Map == r.Map )
						found = _regions[i];
				}
			}

			foundObject = found;
			return (found != null);
		}
		#endregion

		#region -void CalculatePopularity()
		private void CalculatePopularity()
		{
			for( int i = 0; i < _regions.Count; i++ )
			{
				_regions[i].PopularityPercent = (int)((_regions[i].Count / (double)_totalRegionChecks) * 100);
			}
		}
		#endregion

		#region +void SaveStats( List<RegionWatcherObject>, int )
		public void SaveStats( List<RegionWatcherObject> list, int totalChecks )
		{
			if( !Directory.Exists( PersistanceDirectory ) )
				Directory.CreateDirectory( PersistanceDirectory );

			using( XmlTextWriter writer = new XmlTextWriter( new StreamWriter( PersistancePath, false ) ) )
			{
				writer.Formatting = Formatting.Indented;
				writer.IndentChar = '\t';
				writer.Indentation = 1;

				writer.WriteStartDocument();
				writer.WriteStartElement( RootElementName );
				writer.WriteAttributeString( "totalChecks", totalChecks.ToString() );

				for( int i = 0; i < list.Count; i++ )
				{
					writer.WriteStartElement( ChildElementName );

					writer.WriteStartElement( "name" );
					writer.WriteString( list[i].Name );
					writer.WriteEndElement();

					writer.WriteStartElement( "map" );
					writer.WriteString( list[i].Map.Name );
					writer.WriteEndElement();

					writer.WriteStartElement( "count" );
					writer.WriteString( XmlConvert.ToString( list[i].Count ) );
					writer.WriteEndElement();

					writer.WriteStartElement( "popularityPercentage" );
					writer.WriteString( XmlConvert.ToString( list[i].PopularityPercent ) );
					writer.WriteEndElement();

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
		#endregion

		#region +static List<RegionWatcherObject> Load( bool )
		public static List<RegionWatcherObject> Load( bool autoStart )
		{
			RegionPopularityWatcher watcher = new RegionPopularityWatcher();
			watcher.Regions = new List<RegionWatcherObject>();
			watcher.TotalChecks = 0;

			if( File.Exists( PersistancePath ) )
			{
				XmlDocument doc = new XmlDocument();
				XmlElement root;
				RegionWatcherObject obj;

				try
				{
					doc.Load( PersistancePath );
					root = doc[RootElementName];

					watcher.TotalChecks = Convert.ToInt32( root.GetAttribute( "totalChecks" ) );

					foreach( XmlElement node in root.GetElementsByTagName( ChildElementName ) )
					{
						obj = new RegionWatcherObject( node["name"].InnerText, Util.GetMapByName( node["map"].InnerText ) );
						obj.Count = Convert.ToInt32( node["count"].InnerText );
						obj.PopularityPercent = Convert.ToInt32( node["popularityPercentage"].InnerText );

						if( IgnoreMap( obj.Map ) )
							watcher.TotalChecks -= obj.Count;
						else
							watcher.Regions.Add( obj );
					}
				}
				catch( Exception e )
				{
					ExceptionManager.LogException( "RegionPopularityWatcher", e );
				}
			}

			SortStats( watcher.Regions );

			if( autoStart )
				watcher.Start();

			return watcher.Regions;
		}
		#endregion

		#region +static void SortStats( List<RegionWatcherObject> )
		public static void SortStats( List<RegionWatcherObject> list )
		{
			list.Sort(
				delegate( RegionWatcherObject x, RegionWatcherObject y )
				{
					return x.Count.CompareTo( y.Count );
				} );
		}
		#endregion

		#region -static bool IgnoreMap( Map )
		private static bool IgnoreMap( Map map )
		{
			return (Array.IndexOf<Map>( ExcludedMaps, map ) > -1);
		}
		#endregion
	}

	#region +sealed class RegionWatcherObject
	public sealed class RegionWatcherObject
	{
		private string _name;
		private Map _map;
		private int _count;
		private int _popularityPercent;

		public string Name { get { return _name; } set { _name = value; } }
		public Map Map { get { return _map; } set { _map = value; } }
		public int Count { get { return _count; } set { _count = value; } }
		public int PopularityPercent { get { return _popularityPercent; } set { _popularityPercent = value; } }

		public RegionWatcherObject( string name, Map map ) : this( name, map, 0 ) { }

		public RegionWatcherObject( string name, Map map, int count )
		{
			_name = name;
			_map = map;
			_count = count;
			_popularityPercent = 0;
		}
	}
	#endregion
}