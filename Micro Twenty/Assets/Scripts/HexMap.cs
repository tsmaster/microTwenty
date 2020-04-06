using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

namespace MicroTwenty
{
    public abstract class HexMap
    {
        public enum TileType
        {
            MOUNTAIN,
            WATER_MOUNT,
            PLAIN,
            PORTAL,
            CITY,
            WATER,
            ROCKWALL,
            WOODWALL,
            BUILDING,
            BUILDING_ENTRANCE,
            DOCK,
        };

        public List<HexTile> tiles;
        public List<DynamicObject> dynamicObjects;
        private GameMgr gameMgr;

        public HexMap (GameMgr gameMgr)
        {
            tiles = new List<HexTile> ();
            dynamicObjects = new List<DynamicObject> ();
            this.gameMgr = gameMgr;
        }

        public abstract void LayoutMap ();

        public abstract string Name ();

        public void Mountain (int x, int y, int z)
        {
            AddTile (TileType.MOUNTAIN, x, y, z, false);
        }

        public void WaterMount (int x, int y, int z)
        {
            AddTile (TileType.WATER_MOUNT, x, y, z, false);
        }

        public void Plain (int x, int y, int z)
        {
            AddTile (TileType.PLAIN, x, y, z, true);
        }

        public void Portal (int x, int y, int z)
        {
            AddTile (TileType.PORTAL, x, y, z, true);
        }

        public void Water (int x, int y, int z)
        {
            AddTile (TileType.WATER, x, y, z, false);
        }

        public void City (int x, int y, int z)
        {
            AddTile (TileType.CITY, x, y, z, true);
        }

        public void RockWall (int x, int y, int z)
        {
            AddTile (TileType.ROCKWALL, x, y, z, false);
        }

        public void WoodWall (int x, int y, int z)
        {
            AddTile (TileType.WOODWALL, x, y, z, false);
        }

        public void Building (int x, int y, int z)
        {
            AddTile (TileType.BUILDING, x, y, z, false);
        }

        public void BuildingEntrance (int x, int y, int z)
        {
            AddTile (TileType.BUILDING_ENTRANCE, x, y, z, true);
        }

        public void Dock (int x, int y, int z)
        {
            AddTile (TileType.DOCK, x, y, z, true);
        }

        public void Ship (int x, int y, int z)
        {
            AddTile (TileType.WATER, x, y, z, true);
            AddObject (DynamicObject.DynamicObjectType.SHIP, x, y, z, false);
        }


        private void AddTile (TileType tileType, int x, int y, int z, bool canMove)
        {
            var tile = new HexTile (tileType, x, y, z, canMove);
            tiles.Add (tile);
        }

        public void Tree (int x, int y, int z)
        {
            AddObject (DynamicObject.DynamicObjectType.TREE, x, y, z, true);
        }

        private void AddObject (DynamicObject.DynamicObjectType objectType, int x, int y, int z, bool blocksMovement)
        {
            dynamicObjects.Add (new DynamicObject (gameMgr, new HexCoord (x, y, z), objectType, blocksMovement));
        }

        public List<HexTile> GetTilesAt (HexCoord hc)
        {
            var outList = new List<HexTile> ();
            foreach (var tile in tiles) {
                if (tile.HexCoord.Equals(hc)) {
                    outList.Add (tile);
                }
            }
            return outList;
        }

        public List<HexTile> GetTiles ()
        {
            return tiles;
        }
    }
}
