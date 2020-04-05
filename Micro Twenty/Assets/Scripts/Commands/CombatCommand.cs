using System;

namespace MicroTwenty
{
    public class CombatCommand : Command
    {
        private string destMapName;
        private HexCoord destMapCoord;

        public CombatCommand (string name, GameMgr gameMgr, string destMapName, HexCoord destCoord) : base (name, gameMgr)
        {
            this.destMapName = destMapName;
            this.destMapCoord = destCoord;
        }

        public override void Update (float deltaSeconds)
        {
            gameMgr.TeleportPlayer (destMapName, destMapCoord);
            gameMgr.EnterCombat ();
            this.isDone = true;
        }
    }
}
