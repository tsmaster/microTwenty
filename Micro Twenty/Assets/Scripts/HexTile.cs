using System;
namespace MicroTwenty
{
    public class HexTile
    {
        private bool canMove;
        public HexCoord HexCoord;

        public bool CanMove {
            get { return canMove; }
            set { canMove = value; }
        }

        private HexMap.TileType tileType;
        public HexMap.TileType TileType {
            get { return tileType; }
        }

        public int x, y, z;

        public HexTile (HexMap.TileType tileType, int x, int y, int z, bool canMove)
        {
            this.tileType = tileType;
            this.x = x;
            this.y = y;
            this.z = z;
            this.HexCoord = new HexCoord (x, y, z);
            this.canMove = canMove;
        }
    }
}
