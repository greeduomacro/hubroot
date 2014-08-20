using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;

namespace Server.Commands
{
	public class ToRaisableItemCommand : BaseCommand
	{
		public ToRaisableItemCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Single | CommandSupport.Area | CommandSupport.Multi;
			Commands = new string[] { "ToRaisableItem" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "ToRaisableItem";
			Description = "Easily converts items into raisable items.";
		}

		public static void Initialize()
		{
			TargetCommands.Register( new ToRaisableItemCommand() );
		}

		public override void ExecuteList( CommandEventArgs args, System.Collections.ArrayList list )
		{
			for( int i = 0; i < list.Count; i++ )
			{
				if( list[i] is AddonComponent || list[i] is BaseAddon )
					continue;

				Execute( args, list[i] );
			}
		}

		public override void Execute( CommandEventArgs args, object o )
		{
			if( o is Item && !(o is AddonComponent) && !(o is BaseAddon) )
			{
				Item i = (Item)o;
				RaisableItem newItem = new RaisableItem( i.ItemID );

				newItem.Hue = i.Hue;
				newItem.Light = i.Light;
				newItem.Movable = false;
				newItem.Name = i.Name;

				newItem.MoveToWorld( i.Location, i.Map );

				if( i.Parent == args.Mobile )
					newItem.Bounce( args.Mobile );

				if( i is Container )
					((Container)i).Destroy();
				else
					i.Delete();

				AddResponse( "The item has been converted to a raisable item." );
			}
			else
			{
				LogFailure( "This command only works with basic items (no addons)." );
			}
		}
	}
}
