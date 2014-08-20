using Server.Gumps;

namespace Server.Items
{
	public class TravelTent : Item
	{
		private Rectangle2D m_Bounds;

		public Rectangle2D Bounds { get { return m_Bounds; } }

		[Constructable]
		public TravelTent()
			: base( 0xA58 )
		{
			Name = "a rolled up travel tent";
			Weight = 35.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			Point2D start = new Point2D( from.X - 3, from.Y - 3 );
			Point2D end = new Point2D( from.X + 3, from.Y + 3 );

			m_Bounds = new Rectangle2D( start, end );

			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else if( AlreadyOwnTent( from ) )
			{
				from.SendMessage( "You already have a tent established." );
			}
			else if( from.HasGump( typeof( ConfirmTentPlacementGump ) ) )
			{
				from.CloseGump( typeof( ConfirmTentPlacementGump ) );
			}
			else if( from.Combatant != null )
			{
				from.SendMessage( "You can't place a tent while fighting!" );
			}
			else if( VerifyPlacement( from, m_Bounds ) )
			{
				TentAddon tent = new TentAddon();
				tent.MoveToWorld( new Point3D( from.X, from.Y, from.Z ), from.Map );

				TentFlap flap = new TentFlap( from, this );
				flap.MoveToWorld( new Point3D( from.X + 2, from.Y, from.Z ), from.Map );

				SecureTentChest chest = new SecureTentChest( from );
				chest.MoveToWorld( new Point3D( from.X - 1, from.Y - 2, from.Z ), from.Map );

				TentBedroll roll = new TentBedroll( from, tent, flap, chest );
				roll.MoveToWorld( new Point3D( from.X, from.Y + 1, from.Z ), from.Map );

				from.SendGump( new ConfirmTentPlacementGump( from, tent, flap, roll, chest, m_Bounds ) );

				this.Delete();
			}
			else
			{
				from.SendMessage( "You cannot place a tent in this area." );
			}
		}

		public bool AlreadyOwnTent( Mobile from )
		{
			int count = 0;

			foreach( Item item in from.Backpack.Items )
			{
				if( item is TentValidator )
					count++;
			}

			//for more tents allowed, raise 0
			if( count > 0 && from.AccessLevel == AccessLevel.Player )
				return true;

			return false;
		}

		public bool VerifyPlacement( Mobile from, Rectangle2D area )
		{
			if( !from.CheckAlive() )
				return false;

			foreach( Item i in from.GetItemsInRange( 12 ) )
			{
				if( (i is TravelTent || i is TentAddon) && area.Contains( i ) )
					return false;
			}

			Region region = Region.Find( from.Location, from.Map );

			if( from.AccessLevel >= AccessLevel.GameMaster || region.AllowHousing( from, from.Location ) )
				return true;
			else if( !from.Map.CanFit( from.Location, 16 ) )
				return false;
			else if( region is TreasureRegion )
				return false;

			return (from.AccessLevel >= AccessLevel.GameMaster || region.AllowHousing( from, from.Location ));
		}

		public TravelTent( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
