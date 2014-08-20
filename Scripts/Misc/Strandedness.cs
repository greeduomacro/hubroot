using System;
using System.IO;
using Server;

namespace Server.Misc
{
	public class Strandedness
	{
		private static Point2D[] Backtrol = new Point2D[]
			{
				new Point2D( 2853, 2291 ), new Point2D( 2479, 2112 ), new Point2D( 2142, 1868 ),
				new Point2D( 1632, 1940 ), new Point2D( 1355, 2277 ), new Point2D( 2415, 2672 ),
				new Point2D( 3006, 2588 ), new Point2D( 3915, 2335 ), new Point2D( 4957, 1742 ),
				new Point2D( 4056, 1584 ), new Point2D( 3276, 1715 ), new Point2D( 2503, 1654 ),
				new Point2D( 2410, 930 ), new Point2D( 277, 1398 ), new Point2D( 981, 1078 ),
				new Point2D( 845, 1096 ), new Point2D( 573, 2755 )
			};

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static bool IsStranded( Mobile from )
		{
			Map map = from.Map;

			if( map == null )
				return false;

			object surface = map.GetTopSurface( from.Location );

			if( surface is LandTile )
			{
				int id = ((LandTile)surface).ID;

				return (id >= 168 && id <= 171)
					|| (id >= 310 && id <= 311);
			}
			else if( surface is StaticTile )
			{
				int id = ((StaticTile)surface).ID;

				return (id >= 0x1796 && id <= 0x17B2);
			}

			return false;
		}

		public static void EventSink_Login( LoginEventArgs e )
		{
			Mobile from = e.Mobile;

			if( !IsStranded( from ) )
				return;

			Map map = from.Map;
			Point2D[] list = Backtrol;
			Point2D p = Point2D.Zero;
			double pdist = double.MaxValue;

			for( int i = 0; i < list.Length; ++i )
			{
				double dist = from.GetDistanceToSqrt( list[i] );

				if( dist < pdist )
				{
					p = list[i];
					pdist = dist;
				}
			}

			int x = p.X, y = p.Y;
			int z;
			bool canFit = false;

			z = map.GetAverageZ( x, y );
			canFit = map.CanSpawnMobile( x, y, z );

			for( int i = 1; !canFit && i <= 40; i += 2 )
			{
				for( int xo = -1; !canFit && xo <= 1; ++xo )
				{
					for( int yo = -1; !canFit && yo <= 1; ++yo )
					{
						if( xo == 0 && yo == 0 )
							continue;

						x = p.X + (xo * i);
						y = p.Y + (yo * i);
						z = map.GetAverageZ( x, y );
						canFit = map.CanSpawnMobile( x, y, z );
					}
				}
			}

			if( canFit )
				from.Location = new Point3D( x, y, z );
		}
	}
}