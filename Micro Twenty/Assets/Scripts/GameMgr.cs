using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class GameMgr
    {
        private MapManager mapManager;

        private List<Command> commands;

        public GameMgr (MapManager mapManager)
        {
            this.mapManager = mapManager;
            this.commands = new List<Command> ();
        }

        internal void TeleportPlayer (string destMapName, HexCoord destMapCoord)
        {
            mapManager.TeleportPlayer (destMapName, destMapCoord);
        }

        internal void AddCommand (Command command)
        {
            commands.Add (command);
        }

        internal void EnterCombat ()
        {
            mapManager.EnterCombat ();
        }

        internal void Update (float deltaSeconds)
        {
            // maybe should do more than one command per frame?

            if (commands.Count > 0) {
                commands [0].Update (deltaSeconds);
                if (commands [0].IsDone ()) {
                    commands.RemoveAt (0);
                }
            }
        }
    }
}
