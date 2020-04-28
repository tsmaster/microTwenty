using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class CombatTrigger : DynamicObject
    {
        private string destMapName;
        private HexCoord destCoord;

        public CombatTrigger (GameMgr gameMgr, HexCoord hexCoord, string destMapName, HexCoord destCoord) : base (gameMgr, hexCoord, DynamicObjectType.RATMAN, false)
        {
            this.destMapName = destMapName;
            this.destCoord = destCoord;
        }

        public override void OnMoveOver ()
        {
            Debug.Log ("To Combat!");
            gameMgr.AddCommand (new DialogCommand (gameMgr, "Combat", new List<string>{
                "The Rat King says",
                "'I send you to fight!'" }, gameMgr.GetMapManager().GetRatKingBitmap()));
            gameMgr.AddCommand (new CombatCommand ("combat", gameMgr, destMapName, destCoord));
        }
    }
}
