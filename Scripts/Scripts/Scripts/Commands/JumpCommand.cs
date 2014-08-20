using Server.Misc;
using Server.Mobiles;
using Server.Searches;
using Server.Spells;
using Server.Targeting;
using Server.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Server.Accounting;
using Server.Collections;
using Server.Commands;
using Server.ContextMenus;
using Server.Guilds;
using Server.Gumps;
using Server.HuePickers;
using Server.Items;
using Server.Menus;
using Server.Network;
using Server.Prompts;
using Server.Perks;

namespace Server.Commands
{
    public class JumpCommand
    {
        [CommandAttribute("Jump", AccessLevel.Counselor)]
        public static void Jump_OnCommand( CommandEventArgs args )
        {
            BeginJump(args.Mobile as Player);
        }

        public static void BeginJump( Player pm )
        {
            if( !pm.Alive || pm.Frozen || pm.Paralyzed || pm.Stam <= 0 || ( pm.Mounted && (pm.Race != Race.Elemental && !pm.AbilityActive)))
            {
                pm.SendMessage("You cannot jump in your current state.");
            }
            else if( pm.JumpRange == 0 )
            {
                pm.SendMessage("You cannot jump right now!");
            }
            else
            {
                pm.BeginTarget(pm.JumpRange, true, Targeting.TargetFlags.None, OnJumpTarget);
            }
        }

        public static bool ElementalJump( Player pm, IPoint3D p )
        {
            bool isWater = false;

            Map map = pm.Map;
            LandTile land = map.Tiles.GetLandTile(p.X, p.Y);
            StaticTile[] tiles = map.Tiles.GetStaticTiles(p.X, p.Y);

            isWater = (land.Z == p.Z && ((land.ID >= 168 && land.ID <= 171) || (land.ID >= 310 && land.ID <= 311)));

            for (int i = 0; i < tiles.Length; ++i)
            {
                StaticTile tile = tiles[i];
                isWater = (tile.ID >= 0x1796 && tile.ID <= 0x17B2);
            }

            return (pm.Race == Race.Elemental && pm.AbilityActive && isWater);
        }

        private static void OnJumpTarget( Mobile from, object targeted )
        {
            Player pm = from as Player;
            IPoint3D target = targeted as IPoint3D;

            if( target == null )
            {
                pm.SendLocalizedMessage(CommonLocs.LocationBlocked);
                return;
            }

            SpellHelper.GetSurfaceTop(ref target);
            Point3D p = new Point3D(target);

            if( WeightOverloading.IsOverloaded(pm) )
            {
                pm.SendLocalizedMessage(502359, "", 0x22); //Thou art too encumbered to move.
            }

            else if( (!pm.Map.CanSpawnMobile(p) || SpellHelper.CheckMulti(p, pm.Map)) 
                && !ElementalJump(pm, p))
            {
                pm.SendLocalizedMessage(CommonLocs.LocationBlocked);
            }
            else
            {
                Tour tour = delegate( Map map, int x, int y )
                {
                    Sector sector = map.GetSector(x, y);

                    for( int i = 0; i < sector.Mobiles.Count; i++ )
                    {
                        Mobile mob = sector.Mobiles[i];

                        if( mob == pm )
                            continue;

                        if( mob.X == x && mob.Y == y && ((mob.Z + Mobile.Height) > pm.JumpHeight) )
                            return true;
                    }

                    for( int i = 0; i < sector.Items.Count; i++ )
                    {
                        Item item = sector.Items[i];

                        if( item.AtWorldPoint(x, y) && ((item.Z + item.ItemData.Height) > pm.JumpHeight) )
                            return true;
                    }

                    StaticTile[] statics = map.Tiles.GetStaticTiles(x, y, true);

                    for( int i = 0; i < statics.Length; i++ )
                    {
                        StaticTile st = statics[i];

                        if( (st.Z + st.Height) > pm.JumpHeight )
                            return true;
                    }

                    LandTile land = map.Tiles.GetLandTile(x, y);

                    if( (land.Z + land.Height) > pm.JumpHeight )
                        return true;

                    return false;
                };

                if( Search.Line(pm.Map, new Point2D(pm.Location), new Point2D(p), tour) )
                {
                    pm.SendMessage("You cannot jump over that obstacle!");
                }
                else if (!ConsumeJumpStamina(pm, pm.Location, p))
                {
                    pm.SendMessage("You do not have the stamina to jump that far!");
                }
                else
                {

                    int emote = Utility.RandomMinMax(1, 4);
                    switch (emote)
                    {
                        case 1:  pm.PublicOverheadMessage(MessageType.Regular, pm.EmoteHue, true, "*Jumps!*");
                            break;

                        case 2: pm.PublicOverheadMessage(MessageType.Regular, pm.EmoteHue, true, "*Bounds!*");
                            break;

                        case 3: pm.PublicOverheadMessage(MessageType.Regular, pm.EmoteHue, true, "*Hops!*");
                            break;

                        case 4: pm.PublicOverheadMessage(MessageType.Regular, pm.EmoteHue, true, "*Leaps!*");
                            break;

                    }

                    Rogue rge = Perk.GetByType<Rogue>((Player)pm);

                    if (rge == null || !rge.CanJumpHidden())
                        pm.RevealingAction();

                    pm.JumpTo(p);
                }
            }
        }

        private static bool ConsumeJumpStamina( Player pm, IPoint2D start, IPoint2D end )
        {
            double weightRatio = (pm.TotalWeight / pm.MaxWeight);
            int weightInducedLoss = (int)(5 * weightRatio);

            int dist = Util.DistanceBetween(start, end);
            int toConsume = (int)(dist * 5);
            toConsume += weightInducedLoss;

            if( (pm.Stam - toConsume) <= 0 )
                return false;

            pm.Stam -= toConsume;

            return true;
        }
    }
}