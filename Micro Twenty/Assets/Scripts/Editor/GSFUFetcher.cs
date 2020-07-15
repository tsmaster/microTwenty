using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GoogleSheetsForUnity;

namespace MicroTwenty
{
    public class TwentyItemImporter : Editor
    {
        private const string _weaponTableName = "Weapons";
        private const string _armorTableName = "Armor";
        private const string _monsterTableName = "Monsters";
        private const string _itemTableName = "Items";

        [MenuItem ("MicroTwenty/Import Weapons from Google Sheets")]
        static void ImportWeapons ()
        {
            Debug.Log ("Importing Weapons");
            GoogleSheetsForUnity.Drive.responseCallback += HandleDriveResponse;
            GoogleSheetsForUnity.Drive.GetTable (_weaponTableName, false);
        }

        [MenuItem ("MicroTwenty/Import Armor from Google Sheets")]
        static void ImportArmor ()
        {
            Debug.Log ("Importing Armor");
            GoogleSheetsForUnity.Drive.responseCallback += HandleDriveResponse;
            GoogleSheetsForUnity.Drive.GetTable (_armorTableName, false);
        }

        [MenuItem ("MicroTwenty/Import Monsters from Google Sheets")]
        static void ImportMonsters ()
        {
            Debug.Log ("Importing Monsters");
            GoogleSheetsForUnity.Drive.responseCallback += HandleDriveResponse;
            GoogleSheetsForUnity.Drive.GetTable (_monsterTableName, false);
        }

        [MenuItem ("MicroTwenty/Import Items from Google Sheets")]
        static void ImportItems ()
        {
            Debug.Log ("Importing Items");
            GoogleSheetsForUnity.Drive.responseCallback += HandleDriveResponse;
            GoogleSheetsForUnity.Drive.GetTable (_itemTableName, false);
        }

        public static void HandleDriveResponse (Drive.DataContainer dataContainer)
        {
            Debug.Log (dataContainer.msg);
            GoogleSheetsForUnity.Drive.responseCallback -= HandleDriveResponse;

            if (dataContainer.QueryType == Drive.QueryType.getTable) {
                string rawJSon = dataContainer.payload;
                //Debug.Log (rawJSon);

                // find table
                if (string.Compare (dataContainer.objType, _weaponTableName) == 0) {
                    Debug.Log ("got weapon table");

                    var wrappedJSON = "{\"weapons\": " + rawJSon + "}";

                    //Debug.Log ("Wrapped JSON");
                    //Debug.Log (wrappedJSON);

                    // Parse from json to the desired object type.
                    var weaponSheet = JsonUtility.FromJson<WeaponSheet> (wrappedJSON);

                    if (weaponSheet == null) {
                        Debug.LogError ("no weapon sheet");

                    } else {
                        Debug.Log ("Weapon 0:" + weaponSheet.weapons [0].Name);
                    }

                    var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, "Resources/JSON/weapons.json");
                    System.IO.File.WriteAllText (path, wrappedJSON);
                    Debug.Log ("Wrote JSON");
                } else if (string.Compare (dataContainer.objType, _armorTableName) == 0) {
                    Debug.Log ("got armor table");

                    var wrappedJSON = "{\"armor\": " + rawJSon + "}";

                    //Debug.Log ("Wrapped JSON");
                    Debug.Log (wrappedJSON);

                    // Parse from json to the desired object type.
                    var armor = JsonUtility.FromJson<ArmorSheet> (wrappedJSON);

                    Debug.Log ("Armor 0:" + armor.armor [0].Name);
                    var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, "Resources/JSON/armor.json");
                    System.IO.File.WriteAllText (path, wrappedJSON);
                    Debug.Log ("Wrote JSON");
                } else if (string.Compare (dataContainer.objType, _monsterTableName) == 0) {
                    Debug.Log ("got monster table");

                    var wrappedJSON = "{\"monsters\": " + rawJSon + "}";

                    //Debug.Log ("Wrapped JSON");
                    Debug.Log (wrappedJSON);

                    // Parse from json to the desired object type.
                    var monsters = JsonUtility.FromJson<MonsterSheet> (wrappedJSON);

                    Debug.Log ("Monster 0:" + monsters.monsters [0].Name);
                    var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, "Resources/JSON/monsters.json");
                    System.IO.File.WriteAllText (path, wrappedJSON);
                    Debug.Log ("Wrote JSON");
                } else if (string.Compare (dataContainer.objType, _itemTableName) == 0) {
                    Debug.Log ("got item table");

                    var wrappedJSON = "{\"items\": " + rawJSon + "}";

                    //Debug.Log ("Wrapped JSON");
                    Debug.Log (wrappedJSON);

                    // Parse from json to the desired object type.
                    var items = JsonUtility.FromJson<ItemSheet> (wrappedJSON);

                    Debug.Log ("Item 0:" + items.items [0].Name);
                    var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, "Resources/JSON/items.json");
                    System.IO.File.WriteAllText (path, wrappedJSON);
                    Debug.Log ("Wrote JSON");
                }
            }
        }
    }
}