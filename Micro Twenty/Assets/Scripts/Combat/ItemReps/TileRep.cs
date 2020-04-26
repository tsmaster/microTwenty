using System;
namespace MicroTwenty
{
    public class TileRep
    {
        public HexCoord HexCoord { get; private set; }
        public bool IsWalkable { get; private set; }

        public TileRep (HexCoord coord, bool isWalkable)
        {
            HexCoord = coord;
            IsWalkable = isWalkable;
        }
    }
}
