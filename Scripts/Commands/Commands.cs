using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells;

namespace Server.Commands.Generic
{
	public class TargetCommands
	{
		public static void Initialize()
		{
			Register( new KillCommand( true ) );
			Register( new KillCommand( false ) );
			Register( new KickCommand( true ) );
			Register( new KickCommand( false ) );
			Register( new FirewallCommand() );
			Register( new TeleCommand() );
			Register( new SetCommand() );

			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Immortal", "blessed", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Invul", "blessed", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Mortal", "blessed", "false", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "NoInvul", "blessed", "false", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Squelch", "squelched", "true", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "Unsquelch", "squelched", "false", ObjectTypes.Mobiles ) );

			Register( new AliasedSetCommand( AccessLevel.GameMaster, "ShaveHair", "HairItemID", "0", ObjectTypes.Mobiles ) );
			Register( new AliasedSetCommand( AccessLevel.GameMaster, "ShaveBeard", "FacialHairItemID", "0", ObjectTypes.Mobiles ) );

			Register( new GetCommand() );
			Register( new GetTypeCommand() );
			Register( new DeleteCommand() );
			Register( new RestockCommand() );
			Register( new DismountCommand() );
			Register( new AddCommand() );
			Register( new AddToPackCommand() );
			Register( new TellCommand() );
			Register( new PrivSoundCommand() );
			Register( new IncreaseCommand() );
			Register( new OpenBrowserCommand() );
			Register( new CountCommand() );
			Register( new InterfaceCommand() );
			Register( new RespawnCommand() );
			Register( new RefreshHouseCommand() );
			Register( new ConditionCommand() );
			Register( new BringToPackCommand() );
			Register( new AnimateCommand() );

			Register( new EditItemDescriptionCommand() );
			Register( new TraceLockdownCommand() );
		}

		private static List<BaseCommand> m_AllCommands = new List<BaseCommand>();

		public static List<BaseCommand> AllCommands { get { return m_AllCommands; } }

		public static void Register( BaseCommand command )
		{
			m_AllCommands.Add( command );

			List<BaseCommandImplementor> impls = BaseCommandImplementor.Implementors;

			for( int i = 0; i < impls.Count; ++i )
			{
				BaseCommandImplementor impl = impls[i];

				if( (command.Supports & impl.SupportRequirement) != 0 )
					impl.Register( command );
			}
		}
	}

	public class AnimateCommand : BaseCommand
	{
		public AnimateCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Animate", "Anim" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Animate <int action> <int frameCount> <int repeatCount> <bool forward> <bool repeat> <int delay>";
			Description = "Causes the target to animate in the designated fashion.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Mobile )
			{
				if( e.Arguments.Length == 1 )
				{
					if( ((Mobile)obj).Alive )
						((Mobile)obj).Animate( e.GetInt32( 0 ), 5, 1, true, false, 0 );
				}
				else if( e.Arguments.Length == 6 )
				{
					int action = e.GetInt32( 0 );
					int frames = e.GetInt32( 1 );
					int repeats = e.GetInt32( 2 );
					bool forward = e.GetBoolean( 3 );
					bool repeat = e.GetBoolean( 4 );
					int delay = e.GetInt32( 5 );

					if( ((Mobile)obj).Alive )
						((Mobile)obj).Animate( action, frames, repeats, forward, repeat, delay );
				}
				else
				{
					LogFailure( Usage );
				}
			}
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if( e.Arguments.Length == 6 )
			{
				int action = e.GetInt32( 0 );
				int frames = e.GetInt32( 1 );
				int repeats = e.GetInt32( 2 );
				bool forward = e.GetBoolean( 3 );
				bool repeat = e.GetBoolean( 4 );
				int delay = e.GetInt32( 5 );

				for( int i = 0; i < list.Count; i++ )
				{
					if( list[i] is Mobile && ((Mobile)list[i]).Alive )
						((Mobile)list[i]).Animate( action, frames, repeats, forward, repeat, delay );
				}
			}
			else
			{
				LogFailure( Usage );
			}
		}
	}

	public class KillCommand : BaseCommand
	{
		private bool Resurrect;

		public KillCommand( bool resurrect )
		{
			Resurrect = resurrect;

			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { resurrect ? "Res" : "Kill" };
			ObjectTypes = ObjectTypes.Mobiles;

			if( resurrect )
			{
				Usage = "Res";
				Description = "Resurrects the targeted mobile(s).";
			}
			else
			{
				Usage = "Kill";
				Description = "Kills the targeted mobile(s).";
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Mobile )
			{
				Mobile targ = obj as Mobile;

				if( Resurrect )
				{
					if( targ.Alive )
						LogFailure( "That mobile is not dead!" );
					else
					{
						targ.Resurrect();

						AddResponse( "The mobile has been resurrected." );
						CommandLogging.WriteLine( e.Mobile, "{0} {1} resurrected {2}.", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( targ ) );
					}
				}
				else
				{
					if( !targ.Alive )
						LogFailure( "That mobile is already dead!" );
					else
					{
						targ.Kill();

						AddResponse( "The mobile has been killed." );
						CommandLogging.WriteLine( e.Mobile, "{0} {1} killed {2}.", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( targ ) );
					}
				}
			}
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			int count = 0;
			DateTime start = DateTime.Now;

			for( int i = 0; i < list.Count; i++ )
			{
				if( list[i] is Mobile )
				{
					Mobile targ = list[i] as Mobile;
					count++;

					if( Resurrect && !targ.Alive )
					{
						targ.Resurrect();

						CommandLogging.WriteLine( e.Mobile, "{0} {1} resurrected {2}.", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( targ ) );
					}
					else if( !Resurrect && targ.Alive )
					{
						targ.Kill();

						CommandLogging.WriteLine( e.Mobile, "{0} {1} killed {2}.", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( targ ) );
					}
				}
			}

			if( count == 1 )
				AddResponse( String.Format( "The mobile has been {0}.", Resurrect ? "resurrected" : "killed" ) );
			else if( count > 1 )
			{
				AddResponse( String.Format( "The process is complete. {0} mobiles have been {1} ({2:F1} seconds).", count, Resurrect ? "resurrected" : "killed", (DateTime.Now - start).TotalSeconds ) );

				CommandLogging.WriteLine( e.Mobile, "{0} {1} killed {2} mobiles.", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), count );
			}
		}
	}

	public class ConditionCommand : BaseCommand
	{
		public ConditionCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple | CommandSupport.Complex | CommandSupport.Self;
			Commands = new string[] { "Condition" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Condition <condition>";
			Description = "Checks that the given condition matches a targeted object.";
			ListOptimized = true;
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			try
			{
				string[] args = e.Arguments;
				ObjectConditional condition = ObjectConditional.Parse( e.Mobile, ref args );

				for( int i = 0; i < list.Count; ++i )
				{
					if( condition.CheckCondition( list[i] ) )
						AddResponse( "True - that object matches the condition." );
					else
						AddResponse( "False - that object does not match the condition." );
				}
			}
			catch( Exception ex )
			{
				e.Mobile.SendMessage( ex.Message );
			}
		}
	}

	public class BringToPackCommand : BaseCommand
	{
		public BringToPackCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllItems;
			Commands = new string[] { "BringToPack" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "BringToPack";
			Description = "Brings a targeted item to your backpack.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Item item = obj as Item;

			if( item != null )
			{
				if( e.Mobile.PlaceInBackpack( item ) )
					AddResponse( "The item has been placed in your backpack." );
				else
					AddResponse( "Your backpack could not hold the item." );
			}
		}
	}

	public class RefreshHouseCommand : BaseCommand
	{
		public RefreshHouseCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple;
			Commands = new string[] { "RefreshHouse" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "RefreshHouse";
			Description = "Refreshes a targeted house sign.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is HouseSign )
			{
				BaseHouse house = ((HouseSign)obj).Owner;

				if( house == null )
				{
					LogFailure( "That sign has no house attached." );
				}
				else
				{
					house.RefreshDecay();
					AddResponse( "The house has been refreshed." );
				}
			}
			else
			{
				LogFailure( "That is not a house sign." );
			}
		}
	}

	public class RespawnCommand : BaseCommand
	{
		public RespawnCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllItems;
			Commands = new string[] { "Respawn" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "Respawn";
			Description = "Manually triggers the Respawn method of targeted spawners.";
			ListOptimized = true;
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			int count = 0;
			DateTime start = DateTime.Now;

			for( int i = 0; i < list.Count; i++ )
			{
				if( list[i] is Spawner )
					++count;

				if( list[i] is Spawner )
					((Spawner)list[i]).Respawn();
			}

			TimeSpan span = DateTime.Now - start;

			if( count == 1 )
				AddResponse( "The spawner has been manually restarted." );
			else if( count > 1 )
				AddResponse( string.Format( "The process is complete. {0} spawner{1} restarted. The entire process took {2:F1} seconds.", count, count > 1 ? "s were" : " was", span.TotalSeconds ) );
		}
	}

	public class CountCommand : BaseCommand
	{
		public CountCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Complex;
			Commands = new string[] { "Count" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Count";
			Description = "Counts the number of objects that a command modifier would use. Generally used with condition arguments.";
			ListOptimized = true;
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if( list.Count == 1 )
				AddResponse( "There is one matching object." );
			else
				AddResponse( String.Format( "There are {0} matching objects.", list.Count ) );
		}
	}

	public class OpenBrowserCommand : BaseCommand
	{
		public OpenBrowserCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "OpenBrowser", "OB" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "OpenBrowser <url>";
			Description = "Opens the web browser of a targeted player to a specified url.";
		}

		public static void OpenBrowser_Callback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			Mobile gm = (Mobile)states[0];
			string url = (string)states[1];

			if( okay )
			{
				gm.SendMessage( "{0} : has opened their web browser to : {1}", from.Name, url );
				from.LaunchBrowser( url );
			}
			else
			{
				from.SendMessage( "You have chosen not to open your web browser." );
				gm.SendMessage( "{0} : has chosen not to open their web browser to : {1}", from.Name, url );
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( e.Length == 1 )
			{
				Mobile mob = (Mobile)obj;
				Mobile from = e.Mobile;

				if( mob.Player )
				{
					NetState ns = mob.NetState;

					if( ns == null )
					{
						LogFailure( "That player is not online." );
					}
					else
					{
						string url = e.GetString( 0 );

						CommandLogging.WriteLine( from, "{0} {1} requesting to open web browser of {2} to {3}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ), url );
						AddResponse( "Awaiting user confirmation..." );
						mob.SendGump( new WarningGump( 1060637, 30720, String.Format( "A game master is requesting to open your web browser to the following URL:<br>{0}", url ), 0xFFC000, 320, 240, new WarningGumpCallback( OpenBrowser_Callback ), new object[] { from, url } ) );
					}
				}
				else
				{
					LogFailure( "That is not a player." );
				}
			}
			else
			{
				LogFailure( "Format: OpenBrowser <url>" );
			}
		}
	}

	public class IncreaseCommand : BaseCommand
	{
		public IncreaseCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "Increase", "Inc" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Increase {<propertyName> <offset> ...}";
			Description = "Increases the value of a specified property by the specified offset.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is BaseMulti )
			{
				LogFailure( "This command does not work on multis." );
			}
			else if( e.Length >= 2 )
			{
				string result = Properties.IncreaseValue( e.Mobile, obj, e.Arguments );

				if( result == "The property has been increased." || result == "The properties have been increased." || result == "The property has been decreased." || result == "The properties have been decreased." || result == "The properties have been changed." )
					AddResponse( result );
				else
					LogFailure( result );
			}
			else
			{
				LogFailure( "Format: Increase {<propertyName> <offset> ...}" );
			}
		}
	}

	public class PrivSoundCommand : BaseCommand
	{
		public PrivSoundCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "PrivSound" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "PrivSound <index>";
			Description = "Plays a sound to a given target.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;

			if( e.Length == 1 )
			{
				int index = e.GetInt32( 0 );
				Mobile mob = (Mobile)obj;

				CommandLogging.WriteLine( from, "{0} {1} playing sound {2} for {3}", from.AccessLevel, CommandLogging.Format( from ), index, CommandLogging.Format( mob ) );
				mob.Send( new PlaySound( index, mob.Location ) );
			}
			else
			{
				from.SendMessage( "Format: PrivSound <index>" );
			}
		}
	}

	public class TellCommand : BaseCommand
	{
		public TellCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			ObjectTypes = ObjectTypes.Mobiles;

			Commands = new string[] { "Tell" };
			Usage = "Tell \"text\"";
			Description = "Sends a system message to a targeted player.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile)obj;
			Mobile from = e.Mobile;

			CommandLogging.WriteLine( from, "{0} {1} {2} {3} \"{4}\"", from.AccessLevel, CommandLogging.Format( from ), "telling", CommandLogging.Format( mob ), e.ArgString );

			mob.SendMessage( e.ArgString );
		}
	}

	public class AddToPackCommand : BaseCommand
	{
		public AddToPackCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.All;
			Commands = new string[] { "AddToPack", "AddToCont" };
			ObjectTypes = ObjectTypes.Both;
			ListOptimized = true;
			Usage = "AddToPack <name> [params] [set {<propertyName> <value> ...}]";
			Description = "Adds an item by name to the backpack of a targeted player or npc, or a targeted container. Optional constructor parameters. Optional set property list.";
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if( e.Arguments.Length == 0 )
				return;

			List<Container> packs = new List<Container>( list.Count );

			for( int i = 0; i < list.Count; ++i )
			{
				object obj = list[i];
				Container cont = null;

				if( obj is Mobile )
					cont = ((Mobile)obj).Backpack;
				else if( obj is Container )
					cont = (Container)obj;

				if( cont != null )
					packs.Add( cont );
				else
					LogFailure( "That is not a container." );
			}

			Add.Invoke( e.Mobile, e.Mobile.Location, e.Mobile.Location, e.Arguments, packs );
		}
	}

	public class AddCommand : BaseCommand
	{
		public AddCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple | CommandSupport.Self;
			Commands = new string[] { "Add" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Add [<name> [params] [set {<propertyName> <value> ...}]]";
			Description = "Adds an item or npc by name to a targeted location. Optional constructor parameters. Optional set property list. If no arguments are specified, this brings up a categorized add menu.";
		}

		public override bool ValidateArgs( BaseCommandImplementor impl, CommandEventArgs e )
		{
			if( e.Length >= 1 )
			{
				Type t = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

				if( t == null )
				{
					e.Mobile.SendMessage( "No type with that name was found." );

					string match = e.GetString( 0 ).Trim();

					if( match.Length < 3 )
					{
						e.Mobile.SendMessage( "Invalid search string." );
						e.Mobile.SendGump( new AddGump( e.Mobile, match, 0, Type.EmptyTypes, false ) );
					}
					else
					{
						e.Mobile.SendGump( new AddGump( e.Mobile, match, 0, AddGump.Match( match ).ToArray(), true ) );
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				e.Mobile.SendGump( new CategorizedAddGump( e.Mobile ) );
			}

			return false;
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			IPoint3D p = obj as IPoint3D;

			if( p == null )
				return;

			if( p is Item )
				p = ((Item)p).GetWorldTop();
			else if( p is Mobile )
				p = ((Mobile)p).Location;

			Add.Invoke( e.Mobile, new Point3D( p ), new Point3D( p ), e.Arguments );
		}
	}

	public class TeleCommand : BaseCommand
	{
		public TeleCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.Simple;
			Commands = new string[] { "Teleport", "Tele" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Teleport";
			Description = "Teleports your character to a targeted location.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			IPoint3D p = obj as IPoint3D;

			if( p == null )
				return;

			Mobile from = e.Mobile;

			SpellHelper.GetSurfaceTop( ref p );

			Point3D fromLoc = from.Location;
			Point3D toLoc = new Point3D( p );

			from.Location = toLoc;
			from.ProcessDelta();
		}
	}

	public class DismountCommand : BaseCommand
	{
		public DismountCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Dismount" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Dismount";
			Description = "Forcefully dismounts a given target.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			Mobile mob = (Mobile)obj;

			CommandLogging.WriteLine( from, "{0} {1} dismounting {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( mob ) );

			bool takenAction = false;

			for( int i = 0; i < mob.Items.Count; ++i )
			{
				Item item = mob.Items[i];

				if( item is IMountItem )
				{
					IMount mount = ((IMountItem)item).Mount;

					if( mount != null )
					{
						mount.Rider = null;
						takenAction = true;
					}

					if( mob.Items.IndexOf( item ) == -1 )
						--i;
				}
			}

			for( int i = 0; i < mob.Items.Count; ++i )
			{
				Item item = mob.Items[i];

				if( item.Layer == Layer.Mount )
				{
					takenAction = true;
					item.Delete();
					--i;
				}
			}

			if( takenAction )
				AddResponse( "They have been dismounted." );
			else
				LogFailure( "They were not mounted." );
		}
	}

	public class RestockCommand : BaseCommand
	{
		public RestockCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs;
			Commands = new string[] { "Restock" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Restock";
			Description = "Manually restocks a targeted vendor, refreshing the quantity of every item the vendor sells to the maximum. This also invokes the maximum quantity adjustment algorithms.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is BaseVendor )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} restocking {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );

				((BaseVendor)obj).Restock();
				AddResponse( "The vendor has been restocked." );
			}
			else
			{
				AddResponse( "That is not a vendor." );
			}
		}
	}

	public class GetTypeCommand : BaseCommand
	{
		public GetTypeCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "GetType" };
			ObjectTypes = ObjectTypes.All;
			Usage = "GetType";
			Description = "Gets the type name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj == null )
			{
				AddResponse( "The object is null." );
			}
			else
			{
				Type type = obj.GetType();

				if( type.DeclaringType == null )
					AddResponse( String.Format( "The type of that object is {0}.", type.Name ) );
				else
					AddResponse( String.Format( "The type of that object is {0}.", type.FullName ) );
			}
		}
	}

	public class GetCommand : BaseCommand
	{
		public GetCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "Get" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Get <propertyName>";
			Description = "Gets one or more property values by name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( e.Length >= 1 )
			{
				for( int i = 0; i < e.Length; ++i )
				{
					string result = Properties.GetValue( e.Mobile, obj, e.GetString( i ) );

					if( result == "Property not found." || result == "Property is write only." || result.StartsWith( "Getting this property" ) )
						LogFailure( result );
					else
						AddResponse( result );
				}
			}
			else
			{
				LogFailure( "Format: Get <propertyName>" );
			}
		}
	}

	public class AliasedSetCommand : BaseCommand
	{
		private string m_Name;
		private string m_Value;

		public AliasedSetCommand( AccessLevel level, string command, string name, string value, ObjectTypes objects )
		{
			m_Name = name;
			m_Value = value;

			AccessLevel = level;

			if( objects == ObjectTypes.Items )
				Supports = CommandSupport.AllItems;
			else if( objects == ObjectTypes.Mobiles )
				Supports = CommandSupport.AllMobiles;
			else
				Supports = CommandSupport.All;

			Commands = new string[] { command };
			ObjectTypes = objects;
			Usage = command;
			Description = String.Format( "Sets the {0} property to {1}.", name, value );
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			string result = Properties.SetValue( e.Mobile, obj, m_Name, m_Value );

			if( result == "Property has been set." )
				AddResponse( result );
			else
				LogFailure( result );
		}
	}

	public class SetCommand : BaseCommand
	{
		public SetCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.All;
			Commands = new string[] { "Set" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Set <propertyName> <value> [...]";
			Description = "Sets one or more property values by name of a targeted object.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( e.Length >= 2 )
			{
				for( int i = 0; (i + 1) < e.Length; i += 2 )
				{
					string result = Properties.SetValue( e.Mobile, obj, e.GetString( i ), e.GetString( i + 1 ) );

					if( result == "Property has been set." )
						AddResponse( result );
					else
						LogFailure( result );
				}
			}
			else
			{
				LogFailure( "Format: Set <propertyName> <value>" );
			}
		}
	}

	public class DeleteCommand : BaseCommand
	{
		public DeleteCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs | CommandSupport.AllItems;
			Commands = new string[] { "Delete", "Remove" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Delete";
			Description = "Deletes a targeted item or mobile. Does not delete players.";
		}

		private void OnConfirmCallback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			CommandEventArgs e = (CommandEventArgs)states[0];
			ArrayList list = (ArrayList)states[1];

			bool flushToLog = false;

			if( okay )
			{
				AddResponse( "Delete command confirmed." );

				if( list.Count > 20 )
				{
					CommandLogging.Enabled = false;
					NetState.Pause();
				}

				base.ExecuteList( e, list );

				if( list.Count > 20 )
				{
					NetState.Resume();
					flushToLog = true;
					CommandLogging.Enabled = true;
				}
			}
			else
			{
				AddResponse( "Delete command aborted." );
			}

			Flush( from, flushToLog );
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if( list.Count > 1 )
			{
				e.Mobile.SendGump( new WarningGump( 1060637, 30720, String.Format( "You are about to delete {0} objects. This cannot be undone without a full server revert.<br><br>Continue?", list.Count ), 0xFFC000, 420, 280, new WarningGumpCallback( OnConfirmCallback ), new object[] { e, list } ) );
				AddResponse( "Awaiting confirmation..." );
			}
			else
			{
				base.ExecuteList( e, list );
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			if( obj is Item )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );
				((Item)obj).Delete();
				//AddResponse( "The item has been deleted." );
			}
			else if( obj is Mobile && !((Mobile)obj).Player )
			{
				CommandLogging.WriteLine( e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format( e.Mobile ), CommandLogging.Format( obj ) );
				((Mobile)obj).Delete();
				//AddResponse( "The mobile has been deleted." );
			}
			else
			{
				LogFailure( "That cannot be deleted." );
			}
		}
	}

	public class FirewallCommand : BaseCommand
	{
		public FirewallCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Firewall" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Firewall";
			Description = "Adds a targeted player to the firewall (list of blocked IP addresses). This command does not ban or kick.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			Mobile targ = (Mobile)obj;
			NetState state = targ.NetState;

			if( state != null )
			{
				CommandLogging.WriteLine( from, "{0} {1} firewalling {2}", from.AccessLevel, CommandLogging.Format( from ), CommandLogging.Format( targ ) );

				try
				{
					Firewall.Add( state.Address );
					AddResponse( "They have been firewalled." );
				}
				catch( Exception ex )
				{
					LogFailure( ex.Message );
				}
			}
			else
			{
				LogFailure( "They are not online." );
			}
		}
	}

	public class KickCommand : BaseCommand
	{
		private bool m_Ban;

		public KickCommand( bool ban )
		{
			m_Ban = ban;

			AccessLevel = (ban ? AccessLevel.Administrator : AccessLevel.GameMaster);
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { ban ? "Ban" : "Kick" };
			ObjectTypes = ObjectTypes.Mobiles;

			if( ban )
			{
				Usage = "Ban";
				Description = "Bans the account of a targeted player.";
			}
			else
			{
				Usage = "Kick";
				Description = "Disconnects a targeted player.";
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile from = e.Mobile;
			Mobile targ = (Mobile)obj;

			if( from.AccessLevel > targ.AccessLevel )
			{
				NetState fromState = from.NetState, targState = targ.NetState;

				if( fromState != null && targState != null )
				{
					Account fromAccount = fromState.Account as Account;
					Account targAccount = targState.Account as Account;

					if( fromAccount != null && targAccount != null )
					{
						CommandLogging.WriteLine( from, "{0} {1} {2} {3}", from.AccessLevel, CommandLogging.Format( from ), m_Ban ? "banning" : "kicking", CommandLogging.Format( targ ) );

						//targ.Say( "I've been {0}!", m_Ban ? "banned" : "kicked" );

						AddResponse( String.Format( "They have been {0}.", m_Ban ? "banned" : "kicked" ) );

						targState.Dispose();

						if( m_Ban )
						{
							targAccount.Banned = true;
							targAccount.SetUnspecifiedBan( from );
							from.SendGump( new BanDurationGump( targAccount ) );
						}
					}
				}
				else if( targState == null )
				{
					LogFailure( "They are not online." );
				}
			}
			else
			{
				LogFailure( "You do not have the required access level to do this." );
			}
		}
	}

	public class EditItemDescriptionCommand : BaseCommand
	{
		public EditItemDescriptionCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllItems;
			Commands = new string[] { "DescribeItem" };
			ObjectTypes = ObjectTypes.Items;
		}

		public override void Execute( CommandEventArgs args, object obj )
		{
			Mobile m = args.Mobile;

			if( obj is IDescriptiveItem )
			{
				m.Prompt = new Server.Prompts.DescriptiveItemPrompt( (IDescriptiveItem)obj );
				m.SendMessage( "Enter a description for this item:" );
			}
			else
			{
				m.SendMessage( "That item cannot hold a description." );
			}
		}
	}

	public class TraceLockdownCommand : BaseCommand
	{
		public TraceLockdownCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.Simple;
			Commands = new string[] { "TraceLockdown" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "TraceLockdown";
			Description = "Finds the BaseHouse for which a targeted item is locked down or secured.";
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Item item = obj as Item;

			if( item == null )
				return;

			if( !item.IsLockedDown && !item.IsSecure )
			{
				LogFailure( "That is not locked down." );
				return;
			}

			foreach( BaseHouse house in BaseHouse.AllHouses )
			{
				if( house.IsSecure( item ) || house.IsLockedDown( item ) )
				{
					e.Mobile.SendGump( new PropertiesGump( e.Mobile, house ) );
					return;
				}
			}

			LogFailure( "No house was found." );
		}
	}
}