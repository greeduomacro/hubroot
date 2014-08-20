using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Currency;

namespace Khazman.TravelBySea
{
	public class SailDestinationGump : Gump
	{
		private const int fieldsPerPage = 14;
		
		private Mobile m_From;
		private Point3D m_StartPoint;
		private Point3D m_SailTo;
		private Point3D m_TravelBoat = new Point3D( 1401, 828, -2 );
		
		public SailDestinationGump( Mobile from, Point3D startPoint ) : base( 10, 10 )
		{
			m_From = from;
			m_StartPoint = startPoint;
			
			AddPage( 1 );
			AddBackground( 10, 10, 410, 260, 9250 );
			AddLabel( 125, 30, 0, "Where would you like to sail?" );
			
			//AddButton( 30, 70, 5601, 5605, 13, GumpButtonType.Reply, 1 );
			AddLabel( 55, 68, 0, "Raivac - Port Closed" );
			AddButton( 30, 95, 5601, 5605, 14, GumpButtonType.Reply, 1 );
			AddLabel( 55, 93, 0, "Caer Nital" );
			AddButton( 30, 120, 5601, 5605, 15, GumpButtonType.Reply, 1 );
			AddLabel( 55, 118, 0, "Alivraz" );
			AddButton( 30, 145, 5601, 5605, 16, GumpButtonType.Reply, 1 );
			AddLabel( 55, 143, 0, "Brelk" );
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			
			int xPoint;
			int yPoint;
			int totalDistance;
			
			Container pack = from.Backpack;
			
			switch( info.ButtonID )
			{
				default:
					{
						break;
					}
				case 1:
					{
						xPoint = 1466 - m_StartPoint.X;
						yPoint = 1765 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 1466, 1765, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 2:
					{
						xPoint = 3025 - m_StartPoint.X;
						yPoint = 836 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 3025, 836, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 3:
					{
						xPoint = 2252 - m_StartPoint.X;
						yPoint = 1171 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 2252, 1171, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 4:
					{
						xPoint = 2088 - m_StartPoint.X;
						yPoint = 2854 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 2088, 2854, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 5:
					{
						xPoint = 656 - m_StartPoint.X;
						yPoint = 2244 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 656, 2244, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 6:
					{
						xPoint = 3810 - m_StartPoint.X;
						yPoint = 1278 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 3810, 1278, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 7:
					{
						xPoint = 4410 - m_StartPoint.X;
						yPoint = 1035 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 4410, 1035, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 8:
					{
						xPoint = 3665 - m_StartPoint.X;
						yPoint = 22997 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 3665, 2297, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 9:
					{
						xPoint = 3665 - m_StartPoint.X;
						yPoint = 2678 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 3665, 2678, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 10:
					{
						xPoint = 1503 - m_StartPoint.X;
						yPoint = 3705 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 1503, 3705, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 11:
					{
						xPoint = 2937 - m_StartPoint.X;
						yPoint = 3415 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 2937, 3415, 0 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 12:
					{
						xPoint = 5831 - m_StartPoint.X;
						yPoint = 3252 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 5831, 3252, 0 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Felucca ) );
						
						break;
					}
				case 13:
					{
						xPoint = 1085 - m_StartPoint.X;
						yPoint = 634 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 1068, 627, 0 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Backtrol ) );
						
						break;
					}
				case 14:
					{
						xPoint = 1298 - m_StartPoint.X;
						yPoint = 1273 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 1298, 1273, -4 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Backtrol ) );
						
						break;
					}
				case 15:
					{
						xPoint = 600 - m_StartPoint.X;
						yPoint = 446 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 600, 446, 0 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Backtrol ) );
						
						break;
					}
				case 16:
					{
						xPoint = 1212 - m_StartPoint.X;
						yPoint = 890 - m_StartPoint.Y;
						totalDistance = (int)Math.Sqrt( (xPoint * xPoint) + (yPoint * yPoint) );
						
						m_SailTo = new Point3D( 1212, 890, 1 );
						
						from.SendGump( new SailConfirmGump( from, totalDistance, m_TravelBoat, m_SailTo, Map.Backtrol ) );
						
						break;
					}
			}
		}
	}
	
	public class SailConfirmGump : Gump
	{
		int m_Cost;
		Point3D m_SendTo;
		Point3D m_SailTo;
		Map m_SailMap;
		
		public SailConfirmGump( Mobile from, int cost, Point3D sendTo, Point3D sailTo, Map map ) : base( 10, 10 )
		{
			m_Cost = CalcCost( cost );
			m_SendTo = sendTo;
			m_SailTo = sailTo;
			m_SailMap = map;
			
			AddPage( 0 );
			AddBackground( 10, 10, 335, 255, 9250 );
			AddImageTiled( 25, 25, 305, 25, 3004 );
			AddLabel( 55, 27, 0, String.Format( "Sailing there will require {0} copper coins.", m_Cost ) );
			
			AddButton( 30, 85, 5601, 5605, 1, GumpButtonType.Reply, 1 );
			AddLabel( 55, 83, 0, "Pay the fee" );
			AddButton( 30, 120, 5601, 5605, 2, GumpButtonType.Reply, 1 );
			AddLabel( 55, 118, 0, "Present a membership card" );
			AddButton( 30, 155, 5601, 5605, 3, GumpButtonType.Reply, 1 );
			AddLabel( 55, 153, 0, "Cancel trip" );
		}

		private static int CalcCost( int startCost )
		{
			if( startCost > 650 )
				return Utility.RandomMinMax( 600, 700 );
			else
				return startCost;
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			Container pack = from.Backpack;
			
			SailTimer waitTime;
			int travelTime = 5;
			
			switch( info.ButtonID )
			{
				default:
					{
						from.SendMessage( "Another time, perhaps." );
						break;
					}
				case 1:
					{
						if( pack.ConsumeTotal( typeof( Copper ), m_Cost ) )
						{
							if( from.Map != m_SailMap )
								travelTime += 30;

							travelTime += (m_Cost / 10);

							if( travelTime > 90 && from.Map == m_SailMap )
								travelTime = Utility.RandomMinMax( 75, 90 );
							else if( travelTime > 120 && from.Map != m_SailMap )
								travelTime = Utility.RandomMinMax( 110, 120 );

							from.Location = m_SendTo;
							from.Map = Map.Backtrol;

							BaseCreature.TeleportPets( from, m_SendTo, Map.Backtrol );
							
							waitTime = new SailTimer( from, m_SailTo, m_SailMap, TimeSpan.FromSeconds( travelTime ) );
							waitTime.Start();

							from.AddToBackpack( new SailTimerCheck() );

							//from.SendMessage( "Cost: {0} | Travel Time: {1}", m_Cost, TimeSpan.FromSeconds(travelTime) );
						}
						else
						{
							from.SendMessage( "Please come back with the fare price." );
						}
						
						break;
					}
				case 2:
					{
						if( pack.ConsumeTotal( typeof( SailingMembershipCard ), 0 ) )
						{
							if( from.Map != m_SailMap )
								travelTime += 30;

							travelTime += (m_Cost / 10);

							if( travelTime > 90 && from.Map == m_SailMap )
								travelTime = Utility.RandomMinMax( 75, 90 );
							else if( travelTime > 120 && from.Map != m_SailMap )
								travelTime = Utility.RandomMinMax( 110, 120 );

							from.Location = m_SendTo;
							from.Map = Map.Backtrol;

							BaseCreature.TeleportPets( from, m_SendTo, Map.Backtrol );
							
							waitTime = new SailTimer( from, m_SailTo, m_SailMap, TimeSpan.FromSeconds( travelTime ) );
							waitTime.Start();

							from.AddToBackpack( new SailTimerCheck() );

							//from.SendMessage( "Cost: {0} | Travel Time: {1}", m_Cost, TimeSpan.FromSeconds(travelTime) );
						}
						else
						{
							from.SendMessage( "That's not a membership card!" );
						}
						
						break;
					}
				case 3:
					{
						from.SendMessage( "Another time, perhaps." );
						break;
					}
			}
		}
	}
	
	public class SailTimer : Timer
	{
		Mobile m_From;
		Point3D m_Destination;
		Map m_DestMap;
		
		public SailTimer( Mobile from, Point3D destination, Map destMap, TimeSpan duration ) : base( duration )
		{
			m_From = from;
			m_Destination = destination;
			m_DestMap = destMap;
			
			Priority = TimerPriority.OneSecond;
		}
		
		protected override void OnTick()
		{
			if( m_From.Backpack.ConsumeTotal( typeof( SailTimerCheck ), 1 ) )
			{
				BaseCreature.TeleportPets( m_From, m_Destination, m_DestMap );

				m_From.Location = m_Destination;
				m_From.Map = m_DestMap;
			
				m_From.SendMessage( "I hope you enjoyed the trip!" );
			}

			Stop();
		}
	}

	public class SailTimerCheck : Item
	{
		[Constructable]
		public SailTimerCheck() : base( 0x14F4 )
		{
			Name = "Sail Timer Check Item : Do not delete!";
			Visible = false;
			Movable = false;
			Weight = 0.0;
		}

		public SailTimerCheck( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}