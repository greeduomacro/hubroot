using System;
using System.Collections;
using Server;
using Server.Gumps;

namespace Server.Transport
{
	public class FlyingCarpetControlGump : Gump
	{
		private BaseFlyingCarpet _carpet;

		internal int labelHue = 1152;
		internal int incrementArrow = 5600;
		internal int incrementPressedArrow = 5604;
		internal int decrementArrow = 5602;
		internal int decrementPressedArrow = 5606;

		public FlyingCarpetControlGump( BaseFlyingCarpet carpet )
			: base( 0, 0 )
		{
			_carpet = carpet;

			AddPage( 1 );
			AddBackground( 0, 0, 295, 245, 9250 );
			AddBackground( 215, 85, 40, 20, 9200 );

			AddLabel( 15, 10, labelHue, "Directional Movement" );
			AddButton( 57, 29, 4500, 4500, GetButtonID( 0, 0 ), GumpButtonType.Reply, 0 ); //Up
			AddButton( 87, 40, 4501, 4501, GetButtonID( 0, 1 ), GumpButtonType.Reply, 0 ); //North
			AddButton( 100, 70, 4502, 4502, GetButtonID( 0, 2 ), GumpButtonType.Reply, 0 ); //Right
			AddButton( 87, 100, 4503, 4503, GetButtonID( 0, 3 ), GumpButtonType.Reply, 0 ); //East
			AddButton( 59, 113, 4504, 4504, GetButtonID( 0, 4 ), GumpButtonType.Reply, 0 ); //Down
			AddButton( 27, 100, 4505, 4505, GetButtonID( 0, 5 ), GumpButtonType.Reply, 0 ); //South
			AddButton( 15, 70, 4506, 4506, GetButtonID( 0, 6 ), GumpButtonType.Reply, 0 ); //Left
			AddButton( 27, 41, 4507, 4507, GetButtonID( 0, 7 ), GumpButtonType.Reply, 0 ); //West
			AddButton( 69, 84, 4020, 4022, GetButtonID( 0, 8 ), GumpButtonType.Reply, 0 ); //Stop

			AddLabel( 15, 165, labelHue, "Adjust Altitude" );
			AddButton( 30, 190, incrementArrow, incrementPressedArrow, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0 );
			AddButton( 30, 210, decrementArrow, decrementPressedArrow, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0 );
			AddButton( 55, 195, 4020, 4022, GetButtonID( 1, 2 ), GumpButtonType.Reply, 0 ); //Stop

			AddLabel( 170, 85, labelHue, "Speed:" );
			AddLabel( 230, 85, labelHue, _carpet.Speed.ToString() );
			AddButton( 260, 78, incrementArrow, incrementPressedArrow, GetButtonID( 2, 0 ), GumpButtonType.Reply, 0 );
			AddButton( 260, 98, decrementArrow, decrementPressedArrow, GetButtonID( 2, 1 ), GumpButtonType.Reply, 0 );

			AddLabel( 190, 120, labelHue, "Entity Control" );
			AddButton( 215, 140, 4008, 4010, GetButtonID( 4, 0 ), GumpButtonType.Reply, 0 );
			AddButton( 250, 140, 4002, 4004, GetButtonID( 4, 1 ), GumpButtonType.Reply, 0 );

			AddLabel( 205, 10, labelHue, "Rotate" );
			AddButton( 195, 30, 4014, 4016, GetButtonID( 3, 0 ), GumpButtonType.Reply, 0 ); //Turn left
			AddButton( 230, 30, 4005, 4007, GetButtonID( 3, 1 ), GumpButtonType.Reply, 0 ); //Turn right

			AddItem( 210, 196, 2770 );
			AddItem( 188, 178, 2772 );
			AddItem( 210, 160, 2771 );
			AddItem( 232, 178, 2773 );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			int val = (info.ButtonID - 1);

			if( val < 0 ) return;

			int type = (val % 10);
			int index = (val / 10);

			switch( type )
			{
				case 0: //directional movement
					{
						switch( index )
						{
							case 0: _carpet.StartMove( Direction.Up ); break;
							case 1: _carpet.StartMove( Direction.North ); break;
							case 2: _carpet.StartMove( Direction.Right ); break;
							case 3: _carpet.StartMove( Direction.East ); break;
							case 4: _carpet.StartMove( Direction.Down ); break;
							case 5: _carpet.StartMove( Direction.South ); break;
							case 6: _carpet.StartMove( Direction.Left ); break;
							case 7: _carpet.StartMove( Direction.West ); break;
							case 8:
							default: _carpet.Stop(); break;
						}

						break;
					}
				case 1: //altitude adjustment
					{
						switch( index )
						{
							case 0: _carpet.StartAdjustAltitude( Direction.Up ); break;
							case 1: _carpet.StartAdjustAltitude( Direction.Down ); break;
							case 2:
							default: _carpet.StopAltitudeChange(); break;
						}

						break;
					}
				case 2: //speed adjustment
					{
						switch( index )
						{
							case 0: _carpet.Speed++; break;
							case 1: _carpet.Speed--; break;
						}

						break;
					}
				case 3: //turning
					{
						switch( index )
						{
							case 0: _carpet.SetFacing( (Direction)(((int)_carpet.Facing + -2) & 0x7) ); break;
							case 1: _carpet.SetFacing( (Direction)(((int)_carpet.Facing + 2) & 0x7) ); break;
						}

						break;
					}
				case 4:
					{
						switch( index )
						{
							case 0:
								{
									sender.Mobile.BeginTarget( 10, false, Server.Targeting.TargetFlags.None, new TargetStateCallback( carpetEntity_selectionCallback ), true );
									sender.Mobile.SendMessage( "Select an entity to include in this carpet's movement region." );

									break;
								}
							case 1:
								{
									sender.Mobile.BeginTarget( 10, false, Server.Targeting.TargetFlags.None, new TargetStateCallback( carpetEntity_selectionCallback ), false );
									sender.Mobile.SendMessage( "Select an entity to exclude from this carpet's movement region." );

									break;
								}
						}

						break;
					}
			}

			Resend( sender.Mobile );
		}

		private int GetButtonID( int type, int index )
		{
			return (1 + (index * 10) + type);
		}

		private void Resend( Mobile m )
		{
			m.CloseGump( typeof( FlyingCarpetControlGump ) );
			m.SendGump( new FlyingCarpetControlGump( _carpet ) );
		}

		private void carpetEntity_selectionCallback( Mobile from, object target, object state )
		{
			bool add = (bool)state;

			if( target is IEntity )
			{
				if( add )
					_carpet.IncludeEntity( (IEntity)target );
				else
					_carpet.RemoveEntity( (IEntity)target );
			}

			from.BeginTarget( 10, false, Server.Targeting.TargetFlags.None, new TargetStateCallback( carpetEntity_selectionCallback ), state );
		}
	}
}