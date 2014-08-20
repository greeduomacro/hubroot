using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
	public class ConfirmTentPlacementGump : Gump
	{
		private Mobile m_Owner;
		private TentAddon m_Tent;
		private TentFlap m_Flap;
		private TentBedroll m_Bedroll;
		private SecureTentChest m_Chest;
		private Rectangle2D m_RegionBounds;
		private TravelTentRegion m_Region;

		public ConfirmTentPlacementGump( Mobile owner, TentAddon tent, TentFlap flap, TentBedroll roll, SecureTentChest chest, Rectangle2D bounds )
			: base( 10, 10 )
		{
			m_Owner = owner;
			m_Tent = tent;
			m_Flap = flap;
			m_Bedroll = roll;
			m_Chest = chest;
			m_RegionBounds = bounds;

			Closable = false;
			Disposable = false;
			Resizable = false;
			Dragable = true;

			AddPage( 1 );
			AddBackground( 10, 10, 325, 305, 9250 );
			AddImageTiled( 25, 25, 295, 11, 50 );
			AddLabel( 90, 35, 0, "Confirm Tent Placement" );

			AddButton( 35, 275, 4020, 4022, 0, GumpButtonType.Reply, 1 ); //Cancel
			AddButton( 280, 275, 4023, 4025, 1, GumpButtonType.Reply, 1 ); //Ok

			AddHtml( 27, 75, 290, 200, String.Format( "<center>You are about to place a travel tent.</center><br> Within, you will find a bedroll "
													 + "and a secure wooden chest.<br> To repack your tent, or to logout safely, double-click "
													 + "your bedroll. When doing so, please make sure that all items are removed from the chest."
													 + "<br> Please press okay to continue, or press cancel to stop tent placement." ), false, false );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch( info.ButtonID )
			{
				default:
				case 0:
					{
						from.CloseGump( typeof( ConfirmTentPlacementGump ) );

						if( m_Tent != null )
							m_Tent.Delete();
						if( m_Flap != null )
							m_Flap.Delete();
						if( m_Bedroll != null )
							m_Bedroll.Delete();
						if( m_Chest != null )
							m_Chest.Delete();

						m_Owner.AddToBackpack( new TravelTent() );

						break;
					}
				case 1:
					{
						from.CloseGump( typeof( ConfirmTentPlacementGump ) );

						m_Region = new TravelTentRegion( m_Owner, m_Chest, m_Owner.Map, m_RegionBounds, m_Owner.Z );
						m_Region.Register();

						m_Owner.AddToBackpack( new TentValidator( m_Owner, m_Tent, m_Bedroll, m_Chest, m_Region, m_RegionBounds ) );

						break;
					}
			}
		}
	}

	public class TentManagementGump : Gump
	{
		private Mobile m_Owner;
		private TentBedroll m_Bedroll;

		public TentManagementGump( Mobile owner, TentBedroll bedroll )
			: base( 10, 10 )
		{
			m_Owner = owner;
			m_Bedroll = bedroll;

			Closable = true;
			Disposable = true;
			Dragable = true;

			AddPage( 1 );
			AddBackground( 10, 10, 295, 290, 9250 );
			AddImageTiled( 25, 25, 265, 11, 50 );
			AddLabel( 80, 35, 0, "Travel Tent Management" );

			AddButton( 35, 260, 4020, 4022, 0, GumpButtonType.Reply, 1 ); //Cancel
			AddButton( 250, 260, 4023, 4025, 1, GumpButtonType.Reply, 1 ); //Ok

			AddRadio( 35, 125, 210, 211, false, 2 );
			AddLabel( 65, 125, 0, "Deconstruct Tent" );

			AddRadio( 35, 170, 210, 211, false, 3 );
			AddLabel( 65, 170, 0, "Safe Logout" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			Point3D fireLoc = new Point3D( from.X + 5, from.Y + 2, from.Z );

			bool deconstruct = info.IsSwitched( 2 );
			bool logout = info.IsSwitched( 3 );

			if( deconstruct )
			{
				m_Bedroll.Delete();

				from.AddToBackpack( new TravelTent() );

				Campfire fire = new Campfire();
				fire.Status = CampfireStatus.Off;
				fire.MoveToWorld( fireLoc, from.Map );
			}

			if( logout )
			{
				m_Bedroll.Delete();

				if( from.Backpack != null && from.Backpack.GetAmount( typeof( TentValidator ) ) > 0 )
				{
					from.Backpack.ConsumeTotal( typeof( TentValidator ), 1 );
				}

				from.AddToBackpack( new TravelTent() );

				Campfire fire = new Campfire();
				fire.Status = CampfireStatus.Off;
				fire.MoveToWorld( fireLoc, from.Map );

				((PlayerMobile)from).BedrollLogout = true;
				state.Dispose();
			}

			switch( info.ButtonID )
			{
				default:
				case 0:
					{
						from.CloseGump( typeof( TentManagementGump ) );
						break;
					}
			}
		}
	}
}
