using System;
using System.Collections.Generic;
using Server;

namespace Server.Chat
{
	public class ChatInfo
	{
		public enum UpdateType : byte
		{
			Buddy = 0x01,
			Ignore = 0x02
		}

		private List<Mobile> _buddyList;
		private Mobile _client;
		private List<Mobile> _ignoreList;
		private bool _visible;

		public List<Mobile> BuddyList { get { return _buddyList; } }
		public Mobile Client { get { return _client; } }
		public List<Mobile> IgnoreList { get { return _ignoreList; } }
		public bool Visible { get { return _visible; } set { _visible = value; } }

		public ChatInfo( Mobile client ) : this( client, new List<Mobile>(), new List<Mobile>() ) { }

		public ChatInfo( Mobile client, List<Mobile> buddyList, List<Mobile> ignoreList )
		{
			_client = client;
			_buddyList = buddyList;
			_ignoreList = ignoreList;
			_visible = true;
		}

		#region +virtual void Add( UpdateType, Mobile )
		public virtual void Add( UpdateType type, Mobile user )
		{
			switch( type )
			{
				case UpdateType.Buddy:
					{
						Remove( UpdateType.Ignore, user );

						if( !_buddyList.Contains( user ) )
							_buddyList.Add( user );

						break;
					}
				case UpdateType.Ignore:
					{
						Remove( UpdateType.Buddy, user );

						if( !_ignoreList.Contains( user ) )
							_ignoreList.Add( user );

						break;
					}
			}
		}
		#endregion

		#region +virtual List<Mobile> GetOnlineList( ListPage )
		public virtual List<Mobile> GetOnlineList( ListPage listType )
		{
			List<Mobile> list = new List<Mobile>();
			Action<Mobile> onlineCheck =
				delegate( Mobile m )
				{
					if( m != null && m.NetState != null )
						list.Add( m );
				};

			switch( listType )
			{
				case ListPage.Buddy:
					{
						_buddyList.ForEach( onlineCheck );
						break;
					}
				case ListPage.Ignore:
					{
						_ignoreList.ForEach( onlineCheck );
						break;
					}
			}

			return list;
		}
		#endregion

		#region +virtual void Remove( UpdateType, Mobile )
		public virtual void Remove( UpdateType type, Mobile user )
		{
			switch( type )
			{
				case UpdateType.Buddy:
					{
						if( _buddyList.Contains( user ) )
							_buddyList.Remove( user );

						break;
					}
				case UpdateType.Ignore:
					{
						if( _ignoreList.Contains( user ) )
							_ignoreList.Remove( user );

						break;
					}
			}
		}
		#endregion

		#region +virtual void Serialize( BinaryFileWriter )
		public virtual void Serialize( BinaryFileWriter writer )
		{
			writer.Write( (int)0 );

			//version 0
			writer.WriteMobileList<Mobile>( _buddyList, true );
			writer.Write( _client );
			writer.WriteMobileList<Mobile>( _ignoreList, true );
			writer.Write( _visible );
		}
		#endregion

		#region +ChatInfo( BinaryFileReader )
		public ChatInfo( BinaryFileReader reader )
		{
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						_buddyList = reader.ReadStrongMobileList<Mobile>();
						_client = reader.ReadMobile();
						_ignoreList = reader.ReadStrongMobileList<Mobile>();
						_visible = reader.ReadBool();
						break;
					}
			}
		}
		#endregion
	}
}