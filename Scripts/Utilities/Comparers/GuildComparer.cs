using System.Collections.Generic;
using Server;
using Server.Guilds;

namespace Server.Utilities
{
	public class GuildComparer : IComparer<Guild>
	{
		public static readonly IComparer<Guild> Instance = new GuildComparer();

		public int Compare( Guild x, Guild y )
		{
			if( x == null && y == null )
				return 0;
			else if( x == null )
				return -1;
			else if( y == null )
				return 1;

			if( x.Id == y.Id )
				return 0;
			else if( x.Id > y.Id )
				return 1;
			else if( x.Id < y.Id )
				return -1;
			else
				return Insensitive.Compare( x.Name, y.Name );
		}
	}
}