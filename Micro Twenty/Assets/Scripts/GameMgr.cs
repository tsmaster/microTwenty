using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace MicroTwenty
{
    public class GameMgr
    {
        private MapManager _mapManager;

        private WeaponDataManager _weaponDataManager;
        private ArmorDataManager _armorDataManager;
        private InventoryDataManager _inventoryDataManager;

        private List<Command> commands;

        public Party Party {get; set;}

        internal void ShowPaperDoll (Character character)
        {
            EnterBuilding ("Paper Doll", BuildingTrigger.BuildingType.PAPERDOLL);
            _mapManager.SetPaperDollCharacter (character);
        }

        public GameMgr (MapManager mapManager)
        {
            this._mapManager = mapManager;
            _inventoryDataManager = new InventoryDataManager ();
            _weaponDataManager = new WeaponDataManager (_inventoryDataManager);
            _armorDataManager = new ArmorDataManager (_inventoryDataManager);
            commands = new List<Command> ();

            LoadData ();

            Party = new Party ();
            Party.Gold = 6000;


            Character char0 = new Character ("Stan", 10);
            Character char1 = new Character ("Kim", 10);
            Character char2 = new Character ("Flexo", 10);
            Character char3 = new Character ("Mags", 10);
            Character char4 = new Character ("Torso", 10);
            Character char5 = new Character ("Belto", 10);

            /*
            AddUnit ("Stan", t0Starts [0], SpriteId.SPRITE_COMBAT_GUY_1, 0, 3).AddWeapon (WeaponRep.MakeBow ()).AddArmor (ArmorRep.MakeLeatherArmor ());
            AddUnit ("Kim", t0Starts [1], SpriteId.SPRITE_COMBAT_GUY_2, 0, 3).AddWeapon (WeaponRep.MakeBow ()).AddArmor (ArmorRep.MakeLeatherArmor ());
            AddUnit ("Flexo", t0Starts [2], SpriteId.SPRITE_COMBAT_GUY_3, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakeLeatherArmor ());
            AddUnit ("Mags", t0Starts [3], SpriteId.SPRITE_COMBAT_GUY_4, 0, 3).
                AddWeapon (WeaponRep.MakeStaff ()).AddArmor (ArmorRep.MakeClothArmor ()).
                AddSpell (SpellRep.MakeFireballSpell ()).AddSpell (SpellRep.MakeHealSpell ());
            AddUnit ("Torso", t0Starts [4], SpriteId.SPRITE_COMBAT_GUY_5, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakePlateArmor ());
            AddUnit ("Belto", t0Starts [5], SpriteId.SPRITE_COMBAT_GUY_6, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakeChainArmor ());
            */

            Party.AddCharacter (char0);
            Party.AddCharacter (char1);
            Party.AddCharacter (char2);
            Party.AddCharacter (char3);
            Party.AddCharacter (char4);
            Party.AddCharacter (char5);
        }

        public void LoadData ()
        {
            _weaponDataManager.Load ();
            _armorDataManager.Load ();
            _inventoryDataManager.Load ();
        }

        internal void EnterBuilding (String name, BuildingTrigger.BuildingType buildingType)
        {
            _mapManager.EnterBuilding (name, buildingType);
        }

        internal void ExitBuilding ()
        {
            _mapManager.ExitBuilding ();
        }

        internal void TeleportPlayer (string destMapName, HexCoord destMapCoord)
        {
            _mapManager.TeleportPlayer (destMapName, destMapCoord);
        }

        public WeaponDataManager GetWeaponDataManager ()
        {
            return _weaponDataManager;
        }

        public ArmorDataManager GetArmorDataManager ()
        {
            return _armorDataManager;
        }

        public InventoryDataManager GetInventoryDataManager ()
        {
            return _inventoryDataManager;
        }

        internal MapManager GetMapManager ()
        {
            return _mapManager;
        }

        internal void AddCommand (Command command)
        {
            commands.Add (command);
            Debug.LogFormat ("commands count: {0}", commands.Count);
        }

        internal void EnterCombat (string destMapName)
        {
            _mapManager.EnterCombat (destMapName);
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
                var compressedSaveGameString = PlayerPrefs.GetString ("savegame");
                var jsonStr = DecompressString (compressedSaveGameString);

                Party = JsonUtility.FromJson<Party> (jsonStr);

                Debug.Log ("Game Loaded");
                Debug.LogFormat ("Party funds are {0}", Party.Gold);

            } else {
                Debug.Log ("No game available");
            }
        }

        public bool SaveGameAvailable ()
        {
            //return (File.Exists (Application.persistentDataPath + "/gamesave.save"));

            var saveGameString = PlayerPrefs.GetString ("savegame");
            return ((saveGameString != null) &&
                (saveGameString.Length > 0));
        }

        public void SaveGame ()
        {
            Debug.Log ("Saving Game");

            var json = JsonUtility.ToJson (Party);

            var gzippedStr = CompressString (json);

            Debug.LogFormat ("json len: {0} zip len: {1}", json.Length, gzippedStr.Length);

            PlayerPrefs.SetString ("savegame", gzippedStr);

            Debug.Log ("Game Saved");
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString (string text)
        {
            byte [] buffer = Encoding.UTF8.GetBytes (text);
            var memoryStream = new MemoryStream ();
            using (var gZipStream = new GZipStream (memoryStream, CompressionMode.Compress, true)) {
                gZipStream.Write (buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte [memoryStream.Length];
            memoryStream.Read (compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte [compressedData.Length + 4];
            Buffer.BlockCopy (compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy (BitConverter.GetBytes (buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String (gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString (string compressedText)
        {
            byte [] gZipBuffer = Convert.FromBase64String (compressedText);
            using (var memoryStream = new MemoryStream ()) {
                int dataLength = BitConverter.ToInt32 (gZipBuffer, 0);
                memoryStream.Write (gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte [dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream (memoryStream, CompressionMode.Decompress)) {
                    gZipStream.Read (buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString (buffer);
            }
        }
    }
}

