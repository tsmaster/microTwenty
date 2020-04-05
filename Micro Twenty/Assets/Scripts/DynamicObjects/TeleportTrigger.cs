using System;
using UnityEngine;

namespace MicroTwenty
{
    public class TeleportTrigger : DynamicObject
    {
        private string destMapName;
        private HexCoord destCoord;

        public TeleportTrigger (GameMgr gameMgr, HexCoord hexCoord, string destMapName, HexCoord destCoord) : base(gameMgr, hexCoord, DynamicObjectType.TRIGGER, false)
        {
            this.destMapName = destMapName;
            this.destCoord = destCoord;
        }

        public override void OnMoveOver ()
        {
            Debug.Log ("I've been moved over!");
            gameMgr.AddCommand (new TeleportCommand ("teleport", gameMgr, destMapName, destCoord));
        }
    }
}
