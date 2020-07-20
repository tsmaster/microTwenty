using System;

namespace MicroTwenty
{
    public class ExitBuildingCommand : Command
    {
        public ExitBuildingCommand (GameMgr gameMgr) : base ("Exit Building", gameMgr)
        {
        }

        public override void Update (float deltaSeconds)
        {
            gameMgr.ExitBuilding ();
            this.isDone = true;
        }
    }
}
