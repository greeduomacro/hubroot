using Server.Commands.Generic;
using Server.Items;

namespace Server.Commands
{
    public class ToSiegeMachineComponentCommand : BaseCommand
    {
        public ToSiegeMachineComponentCommand()
        {
            AccessLevel = AccessLevel.GameMaster;
            Supports = CommandSupport.Single | CommandSupport.Area | CommandSupport.Multi;
            Commands = new string[] { "ToSiegeMachineComponent", "ToSMC" };
            ObjectTypes = ObjectTypes.Items;
            Usage = "ToSiegeMachineComponent";
            Description = "Easily converts items into siege machine components";
        }

        public static void Initialize()
        {
            TargetCommands.Register(new ToSiegeMachineComponentCommand());
        }

        public override void ExecuteList( CommandEventArgs args, System.Collections.ArrayList list )
        {
            for( int i = 0; i < list.Count; i++ )
            {
                if( list[i] is AddonComponent || list[i] is BaseAddon )
                    continue;

                Execute(args, list[i]);
            }
        }

        public override void Execute( CommandEventArgs args, object o )
        {
            if( o is Item && !(o is AddonComponent) && !(o is BaseAddon) )
            {
                Item i = (Item)o;
                SiegeMachineComponent newComponent = new SiegeMachineComponent(i.ItemID);

                newComponent.Hue = i.Hue;
                newComponent.Light = i.Light;
                newComponent.Movable = false;
                newComponent.Name = i.Name;
                newComponent.MoveToWorld(i.Location, i.Map);

                if( i.Parent == args.Mobile )
                    newComponent.Bounce(args.Mobile);

                if( i is Container )
                    ((Container)i).Destroy();
                else
                    i.Delete();

                AddResponse("The item has been converted to a siege machine component.");
            }
            else
            {
                LogFailure("This command only works with items (no addons).");
            }
        }
    }
}
