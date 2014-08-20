using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;

namespace Server.Utilities
{
	public class NetStateComparer : IComparer<NetState>
	{
		public static readonly IComparer<NetState> Instance = new NetStateComparer();

		public int Compare( NetState x, NetState y )
		{
			if( x == null && y == null )
				return 0;
			else if( x == null )
				return -1;
			else if( y == null )
				return 1;

			if( x.Mobile == null && y.Mobile == null )
				return 0;
			else if( x.Mobile == null )
				return -1;
			else if( y.Mobile == null )
				return 1;

			if( x.Mobile.AccessLevel > y.Mobile.AccessLevel )
				return -1;
			else if( x.Mobile.AccessLevel < y.Mobile.AccessLevel )
				return 1;
			else
				return Insensitive.Compare( x.Mobile.RawName, y.Mobile.RawName );
		}
	}
}