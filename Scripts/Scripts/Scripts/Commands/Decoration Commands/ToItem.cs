using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Commands
{
	public class ToItemCommand : BaseCommand
	{
		public ToItemCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Single | CommandSupport.Area | CommandSupport.Multi;
			Commands = new string[] { "ToItem" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "ToItem";
			Description = "Easily converts objects into their movable item equivalents.";
		}

		public static void Initialize()
		{
			TargetCommands.Register( new ToItemCommand() );
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
			if( o is AddonComponent )
			{
				BaseAddon addon = ((AddonComponent)o).Addon;

				if( addon.Components.Count > 0 )
				{
					for( int i = 0; i < addon.Components.Count; i++ )
					{
						AddonComponent component = addon.Components[i];
						Item newItem = new Item( component.ItemID );

						newItem.Hue = component.Hue;
						newItem.Light = component.Light;
						newItem.Movable = true;
						newItem.Name = component.Name;

						newItem.MoveToWorld( component.Location, component.Map );
					}
				}

				addon.Delete();

				AddResponse( "The add-on has been converted into individual items." );
			}
			else if( o is Item )
			{
				Item i = (Item)o;
				Item newItem = new Item( i.ItemID );

				newItem.Hue = i.Hue;
				newItem.Layer = i.Layer;
				newItem.Light = i.Light;
				newItem.Movable = true;
				newItem.Name = i.Name;

				newItem.MoveToWorld( i.Location, i.Map );

				if( i.Parent == args.Mobile )
					newItem.Bounce( args.Mobile );

				if( i is Container )
					((Container)i).Destroy();
				else
					i.Delete();

				AddResponse( "The item has been converted to an item." );
			}
			else
			{
				LogFailure( "This command only works with item objects." );
			}
		}
	}
}
