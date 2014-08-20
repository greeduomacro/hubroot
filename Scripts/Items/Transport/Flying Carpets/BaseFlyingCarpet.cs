using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Transport
{
    public abstract class BaseFlyingCarpet : BaseMulti
    {
        private Timer _altTimer;
        private Direction _facing;
        private List<IEntity> _includes;
        private Timer _moveTimer;
        private Direction _moving;
        private Mobile _owner;
        private Shadow _shadow;
        private byte _speed;

        [CommandProperty(AccessLevel.GameMaster)]
        public Direction Facing { get { return _facing; } set { SetFacing(value); } }

        public List<IEntity> IncludedEntities { get { return _includes; } protected set { _includes = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsMoving { get { return _moveTimer != null; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Direction Moving { get { return _moving; } set { _moving = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return _owner; } set { _owner = value; } }

        public Shadow Shadow { get { return _shadow; } protected set { _shadow = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public byte Speed { get { return _speed; } set { _speed = Math.Max((byte)1, Math.Min((byte)2, value)); } }

        public virtual int NorthId { get { return 0; } }
        public virtual int EastId { get { return 0; } }
        public virtual int SouthId { get { return 0; } }
        public virtual int WestId { get { return 0; } }

        public override bool HandlesOnSpeech { get { return true; } }

        public BaseFlyingCarpet()
            : base(0x0)
        {
            _shadow = new Shadow(NorthId, EastId, SouthId, WestId);

            Facing = Direction.North;
            IncludedEntities = new List<IEntity>();
            Movable = false;
            Speed = 0;
        }

        public BaseFlyingCarpet( Serial serial ) : base(serial) { }

        public abstract void OnDestroy();

        #region +virtual void DestroyCarpet()
        public virtual void DestroyCarpet()
        {
            OnDestroy();

            if( Owner != null )
            {
                Owner.CloseGump(typeof(FlyingCarpetControlGump));

                if( Contains(Owner) )
                {
                    int z = Map.GetAverageZ(Owner.X, Owner.Y);
                    LandTile t = Map.Tiles.GetLandTile(Owner.X, Owner.Y);
                    StaticTile[] tiles = Map.Tiles.GetStaticTiles(Owner.X, Owner.Y);

                    if( t.Z > z )
                        z = t.Z;

                    for( int i = 0; i < tiles.Length; i++ )
                    {
                        if( tiles[i].Z > z )
                            z = tiles[i].Z;
                    }

                    Owner.Z = z;
                }
            }

            Delete();
        }
        #endregion

        #region +virtual void IncludeEntity( IEntity )
        public virtual void IncludeEntity( IEntity ent )
        {
            if( !IncludedEntities.Contains(ent) )
                IncludedEntities.Add(ent);
        }
        #endregion

        #region +virtual bool HasEntity( IEntity )
        public virtual bool HasEntity( IEntity ent )
        {
            return IncludedEntities.Contains(ent);
        }
        #endregion

        #region +virtual void RemoveEntity( IEntity )
        public virtual void RemoveEntity( IEntity ent )
        {
            if( IncludedEntities.Contains(ent) )
                IncludedEntities.Remove(ent);
        }
        #endregion

        public bool AdjustAltitude( Direction dir )
        {
            if( Map == null || Deleted )
                return false;

            Point3D p = Point3D.Zero;

            switch( dir )
            {
                case Direction.Up: p = new Point3D(X, Y, (Z + 1)); break;
                case Direction.Down: p = new Point3D(X, Y, (Z - 1)); break;
            }

            if( CanFit(p, Map, ItemID, dir, true) )
            {
                Teleport(0, 0, (dir == Direction.Up ? 1 : -1));
                return true;
            }

            return false;
        }

        public bool CanFit( Point3D p, Map map, int itemId, Direction dir, bool altitudeChange )
        {
            if( map == null || map == Map.Internal || Deleted )
                return false;

            if( (p.Z + Mobile.Height) >= Map.TopZ )
                return false;

            MultiComponentList mcl = MultiData.GetComponents(itemId);
            Shadow.Visible = true;

            for( int x = 0; x < mcl.Width; x++ )
            {
                for( int y = 0; y < mcl.Height; y++ )
                {
                    int tx = (p.X + mcl.Min.X + x);
                    int ty = (p.Y + mcl.Min.Y + y);

                    LandTile landTile = map.Tiles.GetLandTile(tx, ty);
                    StaticTile[] statics = map.Tiles.GetStaticTiles(tx, ty);

                    if ((landTile.Z + 2) == p.Z && (!altitudeChange || (altitudeChange && dir == Direction.Down)))
                        Shadow.Visible = false;

                    if (landTile.Z == p.Z && (!altitudeChange || (altitudeChange && dir == Direction.Down)))
                        return false;

                    for( int i = 0; i < statics.Length; i++ )
                    {
                        StaticTile t = statics[i];

                        if( t.Z > p.Z && (p.Z + Mobile.Height) > t.Z ) //if it's above, and our top would hit its bottom
                            return false;
                        else if( t.Z < p.Z && (t.Z + t.Height) > p.Z ) //if it's below, and its top would hit our bottom
                            return false;
                    }

                    object obj = map.GetTopSurface(p);

                    if( obj is LandTile )
                    {
                        LandTile t = (LandTile)obj;

                        if( (t.Z + t.Height) > p.Z )
                            return false;
                    }
                    else if( obj is StaticTile )
                    {
                        StaticTile t = (StaticTile)obj;

                        if( (t.Z + t.Height) > p.Z )
                            return false;
                    }
                    else if( obj is Item )
                    {
                        Item i = obj as Item;

                        if( i.GetWorldTop().Z > p.Z )
                            return false;
                    }

                    if( mcl.Tiles[x][y].Length == 0 || Contains(tx, ty) )
                        continue;

                    if( !map.CanFit(tx, ty, Z, 12, false, true, false) )
                        return false;
                }
            }

            IPooledEnumerable eable = map.GetItemsInBounds(new Rectangle2D((p.X + mcl.Min.X), (p.Y + mcl.Min.Y), mcl.Width, mcl.Height));

            foreach( Item i in eable )
            {
                if( i.ItemID >= 0x4000 || i.Z < p.Z || !i.Visible )
                    continue;

                int x = (i.X - p.X + mcl.Min.X);
                int y = (i.Y - p.Y + mcl.Min.Y);

                if( x >= 0 && x < mcl.Width && y >= 0 && y < mcl.Height && mcl.Tiles[x][y].Length == 0 )
                    continue;
                else if( Contains(i) )
                    continue;

                eable.Free();
                return false;
            }

            eable.Free();

            return true;
        }

        public bool Move( Direction dir, int speed )
        {
            if( Map == null || Deleted || !(Owner.InRange(this.Location, 12)))
                return false;

            int rx = 0, ry = 0;
            //Movement.Offset( (Direction)(((int)Facing + (int)dir) & 0x7), ref rx, ref ry );
            Movement.Movement.Offset(dir, ref rx, ref ry);

            for( int i = 1; i <= speed; i++ )
            {
                if( !CanFit(new Point3D((X + (i * rx)), (Y + (i * ry)), Z), Map, ItemID, dir, false) )
                {
                    if( i == 1 )
                        return false;

                    speed = (i - 1);
                    break;
                }
            }

            int xOffset = (speed * rx);
            int yOffset = (speed * ry);

            Teleport(xOffset, yOffset, 0);
            return true;
        }

        #region +Point3D Rotate( Point3D, int )
        public Point3D Rotate( Point3D p, int count )
        {
            int rx = (p.X - Location.X);
            int ry = (p.Y - Location.Y);

            for( int i = 0; i < count; i++ )
            {
                int tmp = rx;
                rx = -ry;
                ry = tmp;
            }

            return new Point3D((Location.X + rx), (Location.Y + ry), p.Z);
        }
        #endregion

        #region +void SendGump( Mobile )
        public void SendGump( Mobile from )
        {
            if( Contains(from) && from.Z == Z + 1 )
            {
                from.CloseGump(typeof(FlyingCarpetControlGump));
                from.SendGump(new FlyingCarpetControlGump(this));
            }

            if (from.InRange(this.Location, 16))
            {
                from.CloseGump(typeof(FlyingCarpetControlGump));
                from.SendGump(new FlyingCarpetControlGump(this));
            }
        }
        #endregion

        public bool SetFacing( Direction facing )
        {
            if( Parent != null || Map == null )
                return false;

            if( Map != Map.Internal )
            {
                switch( facing )
                {
                    case Direction.North: if( !CanFit(Location, Map, NorthId, facing, false) ) return false; break;
                    case Direction.East: if( !CanFit(Location, Map, EastId, facing, false) ) return false; break;
                    case Direction.South: if( !CanFit(Location, Map, SouthId, facing, false) ) return false; break;
                    case Direction.West: if( !CanFit(Location, Map, WestId, facing, false) ) return false; break;
                }
            }

            Map.OnLeave(this);

            Direction old = Facing;
            _facing = facing;

            MultiComponentList mcl = Components;
            List<IEntity> toMove = new List<IEntity>();
            IPooledEnumerable eable = Map.GetObjectsInBounds(new Rectangle2D((X + mcl.Min.X), (Y + mcl.Min.Y), mcl.Width, mcl.Height));

            foreach( object obj in eable )
            {
                if( obj is Item && Contains((Item)obj) )
                {
                    Item i = obj as Item;

                    if( i != this && i.Z >= Z )
                        toMove.Add(i);
                }
                else if( obj is Mobile && Contains((Mobile)obj) )
                {
                    toMove.Add((Mobile)obj);
                    ((Mobile)obj).Direction = (Direction)((int)((Mobile)obj).Direction - (int)old + (int)facing);
                }
            }

            eable.Free();

            int count = (int)(Facing - old) & 0x7;
            count /= 2;

            for( int i = 0; i < toMove.Count; i++ )
            {
                IEntity ent = toMove[i];

                if( ent is Item )
                    ((Item)ent).Location = Rotate(ent.Location, count);
                else if( ent is Mobile )
                    ((Mobile)ent).Location = Rotate(ent.Location, count);
            }

            switch( Facing )
            {
                case Direction.North: ItemID = NorthId; break;
                case Direction.East: ItemID = EastId; break;
                case Direction.South: ItemID = SouthId; break;
                case Direction.West: ItemID = WestId; break;
            }

            if( Shadow != null && !Shadow.Deleted )
                Shadow.OnRotate(old, Facing);

            Map.OnEnter(this);
            return true;
        }

        public void StartAdjustAltitude( Direction dir )
        {
            if( dir != Direction.Up && dir != Direction.Down )
                return;

            if( _altTimer != null )
                _altTimer.Stop();

            _altTimer = new AltitudeChangeTimer(this, dir);
            _altTimer.Start();
        }

        #region +void StartMove( Direction )
        public void StartMove( Direction dir )
        {
            _moving = dir;

            StopAltitudeChange();

            if( _moveTimer != null )
                _moveTimer.Stop();

            _moveTimer = new MoveTimer(this);
            _moveTimer.Start();
        }
        #endregion

        #region +void Stop()
        public void Stop()
        {
            Moving = Direction.North;
            Speed = 0;

            StopAltitudeChange();

            if( _moveTimer != null )
            {
                _moveTimer.Stop();
                _moveTimer = null;
            }
        }
        #endregion

        #region +void StopAltitudeChange()
        public void StopAltitudeChange()
        {
            if( _altTimer != null )
            {
                _altTimer.Stop();
                _altTimer = null;
            }
        }
        #endregion

        #region +void Teleport~
        public void Teleport( int xOffset, int yOffset, int zOffset )
        {
            Teleport(xOffset, yOffset, zOffset, Map);
        }

        public void Teleport( int xOffset, int yOffset, int zOffset, Map map )
        {
            for( int i = 0; i < IncludedEntities.Count; i++ )
            {
                if( Contains(IncludedEntities[i].Location) )
                {
                    Point3D loc = new Point3D(IncludedEntities[i].X + xOffset, IncludedEntities[i].Y + yOffset, IncludedEntities[i].Z + zOffset);

                    if (IncludedEntities[i] is Item && IncludedEntities[i].Z == (this.Z +1))
                        ((Item)IncludedEntities[i]).MoveToWorld(loc, map);
                    else if (IncludedEntities[i] is Mobile && IncludedEntities[i].Z == (this.Z + 1))
                        ((Mobile)IncludedEntities[i]).MoveToWorld(loc, map);
                }
            }

            if( Owner != null && Contains(Owner) && Owner.Z == (this.Z + 1))
                Owner.MoveToWorld(new Point3D(Owner.X + xOffset, Owner.Y + yOffset, Owner.Z + zOffset), Owner.Map);

            Point3D locationPoint = new Point3D(X + xOffset, Y + yOffset, Z + zOffset);

            Location = locationPoint;
            Map = map;

            if( Shadow != null && !Shadow.Deleted )
            {
                object top = Map.GetTopSurface(locationPoint);
                int z = Map.GetAverageZ(locationPoint.X, locationPoint.Y);

                if( top is LandTile )
                    z = ((LandTile)top).Z + ((LandTile)top).Height;
                else if( top is StaticTile )
                    z = ((StaticTile)top).Z + ((StaticTile)top).Height;
                else if( top is Item )
                    z = ((Item)top).GetWorldTop().Z;

                Shadow.MoveToWorld(new Point3D(locationPoint, z), map);
            }
        }
        #endregion

        #region +override void OnAfterDelete()
        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if( _moveTimer != null )
                _moveTimer.Stop();
        }
        #endregion

        #region +override void OnDelete()
        public override void OnDelete()
        {
            base.OnDelete();

            if( _shadow != null )
                _shadow.Delete();
        }
        #endregion

        #region +override void OnLocationChange( Point3D )
        public override void OnLocationChange( Point3D oldLocation )
        {
            base.OnLocationChange(oldLocation);

            if( Shadow != null && !Shadow.Deleted )
            {
                if( Shadow.Z == Z )
                    Shadow.Z -= 3;
            }
        }
        #endregion

        #region +override void OnMapChange()
        public override void OnMapChange()
        {
            base.OnMapChange();

            if( Shadow != null && !Shadow.Deleted )
                Shadow.MoveToWorld(new Point3D(X, Y, Z - 3), Map);
        }
        #endregion

        #region +override void OnSpeech( SpeechEventArgs )
        public override void OnSpeech( SpeechEventArgs args )
        {
            if( args.Mobile == Owner )
            {
                if (args.Speech.ToLower().IndexOf("in ort por") > -1)
                    SendGump(args.Mobile);
                if (args.Speech.ToLower().IndexOf("ort rel ylem") > -1)
                    DestroyCarpet();
            }
        }
        #endregion

        public override void OnDoubleClick(Mobile from)
        {
            if (from != Owner || !IncludedEntities.Contains(from))
                from.SendMessage("You are not permitted to board this carpet.");

            if (!from.InRange(this.Location, 1))
                from.SendMessage("You must be closer if you wish to board this.");

            if ((from == Owner || IncludedEntities.Contains(from)) && from.InRange(this.Location, 1))
            {
                from.MoveToWorld(this.Location, this.Map);
            }
        }

        #region serialization
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((int)Facing);
            writer.Write((Mobile)Owner);
            writer.Write((Item)Shadow);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        Facing = (Direction)reader.ReadInt();
                        Owner = reader.ReadMobile();
                        Shadow = (Shadow)reader.ReadItem();
                        break;
                    }
            }

            IncludedEntities = new List<IEntity>();
            Speed = 0;
        }
        #endregion

        #region -class AltitudeChangeTimer : Timer
        private class AltitudeChangeTimer : Timer
        {
            private BaseFlyingCarpet _carpet;
            private Direction _dir;

            public AltitudeChangeTimer( BaseFlyingCarpet carpet, Direction dir )
                : base(TimeSpan.FromSeconds(0.25), TimeSpan.FromSeconds(0.5))
            {
                _carpet = carpet;
                _dir = dir;
                Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if( !_carpet.AdjustAltitude(_dir) )
                    _carpet.Stop();
            }
        }
        #endregion

        #region -class MoveTimer : Timer
        private class MoveTimer : Timer
        {
            private BaseFlyingCarpet _carpet;

            public MoveTimer( BaseFlyingCarpet carpet )
                : base(TimeSpan.FromSeconds(0.25), TimeSpan.FromSeconds(0.30))
            {
                _carpet = carpet;
                Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if( !_carpet.Move(_carpet.Moving, _carpet.Speed) )
                    _carpet.Stop();
            }
        }
        #endregion
    }
}