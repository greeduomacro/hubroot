using System;
using Server.Items;

namespace Server.Gumps
{
	public class GrabOptionsGump : Gump
	{
		internal int LabelHue = 1152;

		public GrabOptionsGump( Mobile m )
			: base( 10, 10 )
		{
			GrabOptions options = Grab.GetOptions( m );

			AddPage( 1 );
			AddBackground( 0, 0, 330, 405, 9250 );
			AddLabel( 120, 15, LabelHue, "Grab Options" );
			AddImageTiled( 15, 35, 300, 4, 9151 );

			AddAlphaRegion( 15, 45, 160, 20 );
			AddLabel( 15, 45, LabelHue, "Loot to Grab" );
			AddAlphaRegion( 180, 45, 135, 20 );
			AddLabel( 180, 45, LabelHue, "Placement Container" );

			string[] types = Enum.GetNames( typeof( GrabFlag ) );
			
			for( int i = 0, y = 75; i < types.Length; i++, y += 25 )
			{
				GrabFlag flag = (GrabFlag)Enum.Parse( typeof( GrabFlag ), types[i], true );

				AddCheck( 15, y, 210, 211, options.GetFlag( flag ), (i + 1) );
				AddLabel( 40, y, LabelHue, types[i] );

				AddLabelCropped( 185, y, 100, 20, LabelHue, Grab.GetContainerName( m, flag ) );
				AddButton( 295, y, 9762, 9763, (i + 1), GumpButtonType.Reply, 0 );
			}

			AddButton( 15, 370, 4020, 4022, 100, GumpButtonType.Reply, 0 );
			AddLabel( 50, 370, LabelHue, "Cancel" );
			AddButton( 285, 370, 4023, 4025, 105, GumpButtonType.Reply, 0 );
			AddLabel( 190, 370, LabelHue, "Apply Changes" );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			Mobile m = sender.Mobile;
			GrabOptions options = Grab.GetOptions( m );

			if( m == null || info.ButtonID <= 0 || info.ButtonID == 100 )
				return;

			//store flags
			options.ResetFlags();

			if( info.Switches.Length > 0 )
			{
				for( int i = 0; i < info.Switches.Length; i++ )
				{
					if( info.Switches[i] == 1 )
					{
						options.SetFlag( GrabFlag.Everything, true );
						break;
					}

					options.SetFlag( Grab.ParseInt32( info.Switches[i] ), true );
				}
			}
			else
			{
				options.SetFlag( GrabFlag.Everything, true );
			}

			//handle buttons
			if( info.ButtonID == 105 ) //OK
			{
				Grab.SaveOptions( m, options );
				m.SendMessage( "You have updated your Grab options." );
			}
			else //placement container selection
			{
				m.SendMessage( "Select the container to place this type of loot in." );
				m.BeginTarget( -1, false, Server.Targeting.TargetFlags.None, new TargetStateCallback( OnContainerSelect ), info.ButtonID );
			}
		}

		private void OnContainerSelect( Mobile from, object target, object state )
		{
			if( target is Container )
			{
				Container cont = (Container)target;

				if( !cont.IsChildOf( from.Backpack ) && cont != from.Backpack )
				{
					from.SendMessage( "You may only drop grabbed loot into containers in your pack." );
				}
				else
				{
					GrabOptions options = Grab.GetOptions( from );
					GrabFlag containerFlag = Grab.ParseInt32( (int)state );

					options.SetPlacementContainer( containerFlag, cont );

					from.SendMessage( "You have selected a new container for '{0}'.", Enum.GetName( typeof( GrabFlag ), containerFlag ) );
				}
			}
			else
			{
				from.SendMessage( "Loot can only be dropped into containers." );
			}

			from.SendGump( new GrabOptionsGump( from ) );
		}
	}
}