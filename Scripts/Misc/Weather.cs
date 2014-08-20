using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Misc
{
	public class Weather
	{
		private static Dictionary<Map, List<Weather>> m_WeatherByFacet = new Dictionary<Map, List<Weather>>();

		public static void Initialize()
		{
			/* Static weather format:
			 *   AddWeather( temperature, chanceOfPercipitation, chanceOfExtremeTemperature, <area ...> );
			 * 
			 * Note: chanceOfExtremeTemperature reverses neg/pos of temperature
			 */
			//AddWeather( Map.Tykari, +10, 60, 0,
			//    new Rectangle2D( 20, 12, 1868, 764 ), new Rectangle2D( 1831, 2307, 619, 143 ),
			//    new Rectangle2D( 4554, 547, 333, 546 ) );

			//AddWeather( Map.Tykari, +45, 40, 5,
			//    new Rectangle2D( 606, 756, 1426, 596 ), new Rectangle2D( 3626, 940, 938, 248 ),
			//    new Rectangle2D( 2218, 1540, 194, 268 ), new Rectangle2D( 1564, 2174, 342, 194 ) );

			//AddWeather( Map.Tykari, +70, 30, 1,
			//    new Rectangle2D( 820, 1320, 582, 308 ), new Rectangle2D( 2290, 310, 664, 1178 ),
			//    new Rectangle2D( 1168, 1748, 1040, 436 ), new Rectangle2D( 3056, 1362, 1920, 746 ),
			//    new Rectangle2D( 2422, 1578, 650, 1070 ), new Rectangle2D( 764, 2082, 782, 974 ),
			//    new Rectangle2D( 3996, 518, 548, 408 ), new Rectangle2D( 1600, 2444, 844, 540 ) );

			//AddWeather( Map.Tykari, +110, 0, 0,
			//    new Rectangle2D( 2970, 552, 1020, 250 ), new Rectangle2D( 2968, 802, 888, 554 ),
			//    new Rectangle2D( 126, 1390, 692, 452 ), new Rectangle2D( 784, 1654, 346, 306 ),
			//    new Rectangle2D( 8, 1860, 732, 1176 ) );

			/* Dynamic weather format:
			 *   AddDynamicWeather( temperature, chanceOfPercipitation, chanceOfExtremeTemperature, moveSpeed, width, height, bounds );
			 */
			//for( int i = 0; i < 25; i++ )
			//    AddDynamicWeather( Map.Tykari, +60, 15, 1, 8, 300, 300, new Rectangle2D( 0, 0, 1600, 1325 ) );
		}

		public static List<Weather> GetWeatherList( Map facet )
		{
			if( facet == null )
				return null;

			List<Weather> list = null;
			m_WeatherByFacet.TryGetValue( facet, out list );

			if( list == null )
				m_WeatherByFacet[facet] = list = new List<Weather>();

			return list;
		}

		public static void AddDynamicWeather( Map map, int temperature, int chanceOfPercipitation, int chanceOfExtremeTemperature, int moveSpeed, int width, int height, Rectangle2D bounds )
		{
			Rectangle2D area = new Rectangle2D();
			bool isValid = false;

			for( int j = 0; j < 10; ++j )
			{
				area = new Rectangle2D( bounds.X + Utility.Random( bounds.Width - width ), bounds.Y + Utility.Random( bounds.Height - height ), width, height );

				if( !CheckWeatherConflict( map, null, area ) )
					isValid = true;

				if( isValid )
					break;
			}

			if( !isValid )
				return;

			Weather w = new Weather( map, new Rectangle2D[] { area }, temperature, chanceOfPercipitation, chanceOfExtremeTemperature, TimeSpan.FromSeconds( 30.0 ) );

			w.m_Bounds = bounds;
			w.m_MoveSpeed = moveSpeed;
		}

		public static void AddWeather( Map map, int temperature, int chanceOfPercipitation, int chanceOfExtremeTemperature, params Rectangle2D[] area )
		{
			new Weather( map, area, temperature, chanceOfPercipitation, chanceOfExtremeTemperature, TimeSpan.FromSeconds( 30.0 ) );
		}

		public static bool CheckWeatherConflict( Map facet, Weather exclude, Rectangle2D area )
		{
			List<Weather> list = GetWeatherList( facet );

			if( list == null )
				return false;

			for( int i = 0; i < list.Count; ++i )
			{
				Weather w = list[i];

				if( w != exclude && w.IntersectsWith( area ) )
					return true;
			}

			return false;
		}

		private Map m_Facet;
		private Rectangle2D[] m_Area;
		private int m_Temperature;
		private int m_ChanceOfPercipitation;
		private int m_ChanceOfExtremeTemperature;

		public Map Facet { get { return m_Facet; } }
		public Rectangle2D[] Area { get { return m_Area; } set { m_Area = value; } }
		public int Temperature { get { return m_Temperature; } set { m_Temperature = value; } }
		public int ChanceOfPercipitation { get { return m_ChanceOfPercipitation; } set { m_ChanceOfPercipitation = value; } }
		public int ChanceOfExtremeTemperature { get { return m_ChanceOfExtremeTemperature; } set { m_ChanceOfExtremeTemperature = value; } }

		// For dynamic weather:
		private Rectangle2D m_Bounds;
		private int m_MoveSpeed;
		private int m_MoveAngleX, m_MoveAngleY;

		public Rectangle2D Bounds { get { return m_Bounds; } set { m_Bounds = value; } }
		public int MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }
		public int MoveAngleX { get { return m_MoveAngleX; } set { m_MoveAngleX = value; } }
		public int MoveAngleY { get { return m_MoveAngleY; } set { m_MoveAngleY = value; } }

		public static bool CheckIntersection( Rectangle2D r1, Rectangle2D r2 )
		{
			if( r1.X >= (r2.X + r2.Width) )
				return false;

			if( r2.X >= (r1.X + r1.Width) )
				return false;

			if( r1.Y >= (r2.Y + r2.Height) )
				return false;

			if( r2.Y >= (r1.Y + r1.Height) )
				return false;

			return true;
		}

		public static bool CheckContains( Rectangle2D big, Rectangle2D small )
		{
			if( small.X < big.X )
				return false;

			if( small.Y < big.Y )
				return false;

			if( (small.X + small.Width) > (big.X + big.Width) )
				return false;

			if( (small.Y + small.Height) > (big.Y + big.Height) )
				return false;

			return true;
		}

		public virtual bool IntersectsWith( Rectangle2D area )
		{
			for( int i = 0; i < m_Area.Length; ++i )
			{
				if( CheckIntersection( area, m_Area[i] ) )
					return true;
			}

			return false;
		}

		public Weather( Map facet, Rectangle2D[] area, int temperature, int chanceOfPercipitation, int chanceOfExtremeTemperature, TimeSpan interval )
		{
			m_Facet = facet;
			m_Area = area;
			m_Temperature = temperature;
			m_ChanceOfPercipitation = chanceOfPercipitation;
			m_ChanceOfExtremeTemperature = chanceOfExtremeTemperature;

			List<Weather> list = GetWeatherList( facet );

			if( list != null )
				list.Add( this );

			Timer.DelayCall( TimeSpan.FromSeconds( (0.2 + (Utility.RandomDouble() * 0.8)) * interval.TotalSeconds ), interval, new TimerCallback( OnTick ) );
		}

		public virtual void Reposition()
		{
			if( m_Area.Length == 0 )
				return;

			int width = m_Area[0].Width;
			int height = m_Area[0].Height;

			Rectangle2D area = new Rectangle2D();
			bool isValid = false;

			for( int j = 0; j < 10; ++j )
			{
				area = new Rectangle2D( m_Bounds.X + Utility.Random( m_Bounds.Width - width ), m_Bounds.Y + Utility.Random( m_Bounds.Height - height ), width, height );

				if( !CheckWeatherConflict( m_Facet, this, area ) )
					isValid = true;

				if( isValid )
					break;
			}

			if( !isValid )
				return;

			m_Area[0] = area;
		}

		public virtual void RecalculateMovementAngle()
		{
			double angle = Utility.RandomDouble() * Math.PI * 2.0;

			double cos = Math.Cos( angle );
			double sin = Math.Sin( angle );

			m_MoveAngleX = (int)(100 * cos);
			m_MoveAngleY = (int)(100 * sin);
		}

		public virtual void MoveForward()
		{
			if( m_Area.Length == 0 )
				return;

			for( int i = 0; i < 5; ++i ) // try 5 times to find a valid spot
			{
				int xOffset = (m_MoveSpeed * m_MoveAngleX) / 100;
				int yOffset = (m_MoveSpeed * m_MoveAngleY) / 100;

				Rectangle2D oldArea = m_Area[0];
				Rectangle2D newArea = new Rectangle2D( oldArea.X + xOffset, oldArea.Y + yOffset, oldArea.Width, oldArea.Height );

				if( !CheckWeatherConflict( m_Facet, this, newArea ) && CheckContains( m_Bounds, newArea ) )
				{
					m_Area[0] = newArea;
					break;
				}

				RecalculateMovementAngle();
			}
		}

		private int m_Stage;
		private bool m_Active;
		private bool m_ExtremeTemperature;

		public virtual void OnTick()
		{
			if( m_Stage == 0 )
			{
				m_Active = (m_ChanceOfPercipitation > Utility.Random( 100 ));
				m_ExtremeTemperature = (m_ChanceOfExtremeTemperature > Utility.Random( 100 ));

				if( m_MoveSpeed > 0 )
				{
					Reposition();
					RecalculateMovementAngle();
				}
			}

			if( m_Active )
			{
				if( m_Stage > 0 && m_MoveSpeed > 0 )
					MoveForward();

				int type, density, temperature;

				temperature = m_Temperature;

				if( m_ExtremeTemperature )
					temperature *= -1;

				if( m_Stage < 15 )
				{
					density = m_Stage * 5;
				}
				else
				{
					density = 150 - (m_Stage * 5);

					if( density < 10 )
						density = 10;
					else if( density > 70 )
						density = 70;
				}

				if( density == 0 )
					type = 0xFE;
				else if( temperature > 0 )
					type = 0;
				else
					type = 2;

				List<NetState> states = NetState.Instances;

				Packet weatherPacket = null;

				for( int i = 0; i < states.Count; ++i )
				{
					NetState ns = states[i];
					Mobile mob = ns.Mobile;

					if( mob == null || mob.Map != m_Facet || mob.Z <= -15 )
						continue;

					bool contains = (m_Area.Length == 0);

					for( int j = 0; !contains && j < m_Area.Length; ++j )
						contains = m_Area[j].Contains( mob.Location );

					if( !contains )
						continue;

					if( weatherPacket == null )
						weatherPacket = Packet.Acquire( new Server.Network.Weather( type, density, temperature ) );

					ns.Send( weatherPacket );
				}

				Packet.Release( weatherPacket );
			}

			m_Stage++;
			m_Stage %= 30;
		}
	}

	public class WeatherMap : MapItem
	{
		public override string DefaultName
		{
			get { return "weather map"; }
		}

		[Constructable]
		public WeatherMap()
		{
			SetDisplay( 0, 0, 5119, 4095, 400, 400 );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Map facet = from.Map;

			if( facet == null )
				return;

			List<Weather> list = Weather.GetWeatherList( facet );

			ClearPins();

			for( int i = 0; i < list.Count; ++i )
			{
				Weather w = list[i];

				for( int j = 0; j < w.Area.Length; ++j )
					AddWorldPin( w.Area[j].X + (w.Area[j].Width / 2), w.Area[j].Y + (w.Area[j].Height / 2) );
			}

			base.OnDoubleClick( from );
		}

		public WeatherMap( Serial serial )
			: base( serial )
		{
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