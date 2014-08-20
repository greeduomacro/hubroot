using Server.Searches;
using Server.Mobiles;

namespace Server.Items
{
    public class RespawnBell : Item
    {
        private Point3D _respawnLoc;

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Point3D RespawnLocation { get { return _respawnLoc; } set { _respawnLoc = value; } }

        [Constructable]
        public RespawnBell()
            : base(0x1C12)
        {
            Hue = 0x48D;
            Movable = false;
            Name = "a mystical bell";

            RespawnLocation = this.Location;
        }

        public RespawnBell(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            Player pm = from as Player;

            if (!from.InRange(this, 2))
            {
                from.SendLocalizedMessage(CommonLocs.ThatTooFar);
            }
            else if (pm != null && pm.RespawnLocation == RespawnLocation && pm.RespawnMap == this.Map)
            {
                from.SendMessage("This is already set as your home locale.");
            }
            else
            {
                Effects.PlaySound(this.Location, this.Map, 0x507);
                Effects.SendTargetParticles(from, 0x375A, 1, 30, 0, EffectLayer.CenterFeet);

                int i = 0;
                Tour tour = delegate(Server.Map map, int x, int y)
                {
                    int xLoc = from.X + x;
                    int yLoc = from.Y + y;
                    LandTile landTile = map.Tiles.GetLandTile(xLoc, yLoc);

                    Effects.SendLocationParticles(EffectItem.Create(new Point3D(xLoc, yLoc, landTile.Z), map, EffectItem.DefaultDuration), 0x376A, 1, 30, (i % 2 == 0 ? 1163 : 1164), 3, 0, 0);
                    i++;

                    return false;
                };

                Search.Circle(Map, 3, tour);

                from.SendMessage("This bell now marks your home locale. If you are rendered unconcious during battle, you can choose to be revived here for a minor skill penalty.");

                if (from is Player)
                {
                    ((Player)from).RespawnLocation = RespawnLocation;
                    ((Player)from).RespawnMap = this.Map;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((Point3D)_respawnLoc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            _respawnLoc = reader.ReadPoint3D();
        }
    }
}