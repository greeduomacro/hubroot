using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
	public class FlintAndSteel : Item
	{
		private int m_UsesRemaining;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining{ get{ return m_UsesRemaining; } set{ m_UsesRemaining = value; InvalidateProperties(); } }
		
		[Constructable]
		public FlintAndSteel() : base( 0x27F9 )
		{
			Name = "flint and steel";
			Hue = 942;
			
			UsesRemaining = 15;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 );
			else if( !from.Backpack.ConsumeTotal( typeof( Kindling ), 0 ) )
				from.SendMessage( "You do not have any kindling to ignite." );
			else			
			{
				if( Utility.RandomBool() )
				{
					Point3D fireLocation = GetFireLocation( from );
					
					if( fireLocation == Point3D.Zero )
						from.SendLocalizedMessage( 501695 ); //There is not a spot nearby to place your campfire.
					else
					{
						from.Backpack.ConsumeTotal( typeof( Kindling ), 1 );
						from.LocalOverheadMessage( MessageType.Regular, from.EmoteHue, false, "*strikes the steel and flint, sparking the kindling to life*" );
						
						new Campfire().MoveToWorld( fireLocation, from.Map );

						if( --this.UsesRemaining <= 0 )
							this.Delete();
					}
				}
				else
					from.SendMessage( "You strike the flint, but the kindling fails to ignite." );
			}
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			list.Add( 1060584, "{0}", this.UsesRemaining );
		}
		
		public FlintAndSteel( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
			writer.Write( (int) m_UsesRemaining );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			m_UsesRemaining = reader.ReadInt();
		}
		
		private Point3D GetFireLocation( Mobile from )
		{
			if ( from.Region is DungeonRegion )
				return Point3D.Zero;
			
			if ( this.Parent == null )
				return this.Location;
			
			ArrayList list = new ArrayList( 4 );
			
			AddOffsetLocation( from,  0, -1, list );
			AddOffsetLocation( from, -1,  0, list );
			AddOffsetLocation( from,  0,  1, list );
			AddOffsetLocation( from,  1,  0, list );
			
			if ( list.Count == 0 )
				return Point3D.Zero;
			
			int idx = Utility.Random( list.Count );
			return (Point3D) list[idx];
		}
		
		private void AddOffsetLocation( Mobile from, int offsetX, int offsetY, ArrayList list )
		{
			Map map = from.Map;
			
			int x = from.X + offsetX;
			int y = from.Y + offsetY;
			
			Point3D loc = new Point3D( x, y, from.Z );
			
			if ( map.CanFit( loc, 1 ) && from.InLOS( loc ) )
			{
				list.Add( loc );
			}
			else
			{
				loc = new Point3D( x, y, map.GetAverageZ( x, y ) );
				
				if ( map.CanFit( loc, 1 ) && from.InLOS( loc ) )
					list.Add( loc );
			}
		}
	}
}
