using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Accounting;

namespace Server.Utilities
{
	public class AccountComparer : IComparer<Account>
	{
		public static readonly IComparer<Account> Instance = new AccountComparer();

		public int Compare( Account x, Account y )
		{
			if( x == null && y == null )
				return 0;
			else if( x == null )
				return -1;
			else if( y == null )
				return 1;

			AccessLevel xLevel, yLevel;
			bool xOnline, yOnline;

			Server.Gumps.AdminGump.GetAccountInfo( x, out xLevel, out xOnline );
			Server.Gumps.AdminGump.GetAccountInfo( y, out yLevel, out yOnline );

			if( xOnline && !yOnline )
				return -1;
			else if( yOnline && !xOnline )
				return 1;
			else if( xLevel > yLevel )
				return -1;
			else if( xLevel < yLevel )
				return 1;
			else
				return Insensitive.Compare( x.Username, y.Username );
		}
	}
}