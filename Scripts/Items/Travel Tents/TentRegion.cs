using System;
using Server.Items;

namespace Server.Regions
{
	public class TravelTentRegion : Region
	{
		private Mobile m_Owner;
		private SecureTentChest m_Chest;
		private Rectangle2D m_Bounds;
		private int m_Height;

		public TravelTentRegion( Mobile owner, SecureTentChest chest, Map map, Rectangle2D bounds, int height )
			: base( String.Format( "travel tent of {0}", (owner == null ? "null" : owner.RawName) ), map, 100, bounds )
		{
			m_Owner = owner;
			m_Chest = chest;
			m_Bounds = bounds;
			m_Height = height;

			Register();
		}

		//Suppress default messages
		public override void OnEnter( Mobile m )
		{
		}

		public override void OnExit( Mobile m )
		{
		}

		public override bool AllowSpawn()
		{
			return false;
		}

		public override TimeSpan GetLogoutDelay( Mobile from )
		{
			if( from == m_Owner || m_Chest.IsFriend( from ) )
			{
				return TimeSpan.Zero;
			}
			else
			{
				return TimeSpan.FromMinutes( 2 );
			}
		}
	}
}
