using UnityEngine;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class ArmorDataManager
    {
        const string ARMOR_JSON_PATH = "Resources/JSON/armor.json";
        Dictionary<string, ArmorRow> _armorDict;
        private InventoryDataManager _inventoryDataManager;

        public ArmorDataManager (InventoryDataManager invDataMgr)
        {
            _inventoryDataManager = invDataMgr;
        }

        public void Load ()
        {
            var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, ARMOR_JSON_PATH);
            var armorJson = System.IO.File.ReadAllText (path);
            Debug.Log ("Read Armor JSON");
            var armorSheet = JsonUtility.FromJson<ArmorSheet> (armorJson);
            Debug.Log ("Loaded Armor JSON Sheet");

            _armorDict = new Dictionary<string, ArmorRow> ();

            foreach (ArmorRow ar in armorSheet.armor) {
                var code = ar.InventoryCode;

                if ((code != null) &&
                    (code.Length > 1)) {
                    _armorDict [code] = ar;
                    _inventoryDataManager.AddDesc (ar);
                } else {
                    Debug.LogWarningFormat ("No inventory code for {0}", ar.Name);
                }
            }
            Debug.Log ("made armor dict");
        }

        public ArmorRow GetByCode (string code)
        {
            if (_armorDict.ContainsKey (code)) {
                Debug.LogFormat ("found code {0}", code);
                return _armorDict [code];
            }
            Debug.LogWarningFormat ("looking for armor {0}, but not found", code);
            return null;
        }
    }
}
