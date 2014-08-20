using System;
using System.Collections.Generic;
using Server;
using Server.Commands;

namespace Server.Commands
{
	public class CleanNullMap
	{
		[CommandAttribute( "CleanNullMap", AccessLevel.Administrator )]
		public static void cleanNullMap_onCommand( CommandEventArgs args )
		{
			List<IEntity> toDelete = new List<IEntity>();
			int errorCount = 0;

			foreach( KeyValuePair<Serial, Item> kvp in World.Items )
			{
				if( kvp.Value != null && !kvp.Value.Deleted && kvp.Value.Map == null )
					toDelete.Add( kvp.Value );
			}

			foreach( KeyValuePair<Serial, Mobile> kvp in World.Mobiles )
			{
				if( kvp.Value != null && !kvp.Value.Deleted && kvp.Value.Map == null )
					toDelete.Add( kvp.Value );
			}

			toDelete.ForEach(
				delegate( IEntity entity )
				{
					try
					{
						entity.Delete();
					}
					catch
					{
						errorCount++;
					}
				} );

			args.Mobile.SendMessage( "Deleted {0} entities ({1} error{2})", toDelete.Count, errorCount, (errorCount == 1 ? "s" : "") );
			
			toDelete.Clear();
		}
	}
}
