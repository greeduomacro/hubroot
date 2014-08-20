using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Regions;

namespace Server.Transport
{
    public abstract class BaseCarpetRoll : Item
    {
        private int _multiId;
        private Point3D _offset;

        protected abstract BaseFlyingCarpet Carpet { get; }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MultiID { get { return _multiId; } set { _multiId = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public Point3D Offset { get { return _offset; } set { _offset = value; } }

        public BaseCarpetRoll( int id, Point3D offset )
            : base( 0xA57 )
        {
            _multiId = id & 0x3FFF;
            _offset = offset;

            Name = "a rolled up carpet";
            Weight = 1.0;
        }

        public BaseCarpetRoll( Serial serial ) : base( serial ) { }

        /// <summary>
        /// Completes placement of the carpet
        /// </summary>
        public void OnPlacement( Mobile from, Point3D p )
        {
            Map map = from.Map;

            if( map == null || map == Map.Internal )
                return;

            BaseFlyingCarpet carpet = Carpet;

            if( carpet == null )
                return;

            p = new Point3D( p.X - Offset.X, p.Y - Offset.Y, p.Z - Offset.Z );

            if( CanFit( ref p ) )
            {
                Delete();

                carpet.Owner = from;
                carpet.MoveToWorld( p, map );

                from.Z = carpet.Z + 1;
            }
            else
            {
                carpet.Delete();
                from.Z -= 8;
                from.SendMessage( "The carpet cannot be placed there." );
            }
        }

        /// <summary>
        /// Determines if a new carpet can be placed at the given point. If a carpet cannot fit because
        /// of Z-index clashes, the Z-index is modified until the carpet can fit.
        /// </summary>
        public bool CanFit( ref Point3D p )
        {
            if( Map == null || Map == Map.Internal || Deleted || Carpet == null || Carpet.Deleted )
                return false;

            MultiComponentList mcl = MultiData.GetComponents( Carpet.ItemID );
            int topZ = p.Z;

            for( int x = 0; x < mcl.Width; x++ )
            {
                for( int y = 0; y < mcl.Height; y++ )
                {
                    int tx = (p.X + mcl.Min.X + x);
                    int ty = (p.Y + mcl.Min.Y + y);

                    LandTile landTile = Map.Tiles.GetLandTile( tx, ty );
                    StaticTile[] statics = Map.Tiles.GetStaticTiles( tx, ty, true );

                    topZ = Math.Max( topZ, Map.GetAverageZ( tx, ty ) );

                    for( int i = 0; i < statics.Length; i++ )
                    {
                        StaticTile t = statics[i];

                        if( t.Z > p.Z && (p.Z + 12) > t.Z ) //if it's above, and our top would hit its bottom
                            return false;
                        else if( t.Z < p.Z && (t.Z + t.Height) > p.Z ) //if it's below, and its top would hit our bottom
                            return false;
                    }

                    object obj = Map.GetTopSurface( p );

                    if( obj is LandTile )
                    {
                        LandTile t = (LandTile)obj;

                        if( t.Z > p.Z )
                         return false;
                    }
                    else if( obj is StaticTile )
                    {
                        StaticTile t = (StaticTile)obj;

                        if( t.Z  > p.Z )
                            return false;
                    }
                    else if( obj is Item )
                    {
                        Item i = obj as Item;

                        if( i.GetWorldTop().Z > p.Z )
                            return false;
                    }

                    if( mcl.Tiles[x][y].Length == 0 || Carpet.Contains( tx, ty ) )
                        continue;

                    if( !Map.CanFit( tx, ty, Z, 12, false, true, false ) )
                        return false;
                }
            }

            IPooledEnumerable eable = Map.GetItemsInBounds( new Rectangle2D( (p.X + mcl.Min.X), (p.Y + mcl.Min.Y), mcl.Width, mcl.Height ) );

            foreach( Item i in eable )
            {
                if( i.ItemID >= 0x4000 || i.Z < p.Z || !i.Visible )
                    continue;

                int x = (i.X - p.X + mcl.Min.X);
                int y = (i.Y - p.Y + mcl.Min.Y);

                if( x >= 0 && x < mcl.Width && y >= 0 && y < mcl.Height && mcl.Tiles[x][y].Length == 0 )
                    continue;
                else if( Carpet.Contains( i ) )
                    continue;

                eable.Free();
                return false;
            }

            eable.Free();

            p.Z = topZ + 1;

            return true;
        }

        public override void OnDoubleClick( Mobile from )
        {
            Region reg = Region.Find( from.Location, from.Map );

            if( !IsChildOf( from.Backpack ) )
            {
                from.SendLocalizedMessage( CommonLocs.MustBeInPack );
            }
            else if( reg.IsPartOf( typeof( DungeonRegion ) ) )
            {
                from.SendMessage( "You cannot place this inside a dungeon." );
            }
            else if( reg.IsPartOf( typeof( HouseRegion ) ) )
            {
                from.SendMessage( "You cannot place this inside a house." );
            }
            else
            {
                from.Z += 8;
                OnPlacement( from, from.Location );
            }
        }

        #region serialization
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 );

            writer.Write( (int)MultiID );
            writer.Write( (Point3D)Offset );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();

            MultiID = reader.ReadInt();
            Offset = reader.ReadPoint3D();
        }
        #endregion
    }
}