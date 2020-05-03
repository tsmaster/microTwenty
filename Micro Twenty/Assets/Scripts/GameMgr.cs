using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MicroTwenty
{
    public class GameMgr
    {
        private MapManager mapManager;

        private List<Command> commands;

        public Party Party {get; set;}

        public GameMgr (MapManager mapManager)
        {
            this.mapManager = mapManager;
            this.commands = new List<Command> ();
            Party = new Party ();
        }

        internal void TeleportPlayer (string destMapName, HexCoord destMapCoord)
        {
            mapManager.TeleportPlayer (destMapName, destMapCoord);
        }

        internal MapManager GetMapManager ()
        {
            return mapManager;
        }

        internal void AddCommand (Command command)
        {
            commands.Add (command);
            Debug.LogFormat ("commands count: {0}", commands.Count);
        }

        internal void EnterCombat (string destMapName)
        {
            mapManager.EnterCombat (destMapName);
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

        public void LoadGame ()
        {
            Debug.Log ("Loading Game");
            if (SaveGameAvailable ()) {
                BinaryFormatter bf = new BinaryFormatter ();
                FileStream file = File.Open (Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Party = (Party)bf.Deserialize (file);
                file.Close ();
                Debug.Log ("Game Loaded");
                Debug.LogFormat ("Party funds are {0}", Party.money);
            } else {
                Debug.Log ("No game available");
            }
        }

        public bool SaveGameAvailable ()
        {
            return (File.Exists (Application.persistentDataPath + "/gamesave.save"));
        }

        public void SaveGame ()
        {
            Debug.Log ("Saving Game");
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = File.Create (Application.persistentDataPath + "/gamesave.save");
            bf.Serialize (file, Party);
            file.Close ();
            Debug.Log ("Game Saved");
        }
    }
}
