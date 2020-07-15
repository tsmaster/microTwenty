using UnityEngine;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class WeaponDataManager
    {
        const string WEAPON_JSON_PATH = "Resources/JSON/weapons.json";
        Dictionary<string, WeaponRow> _weaponDict;
        private InventoryDataManager _inventoryDataManager;

        public WeaponDataManager (InventoryDataManager invDataMgr)
        {
            _inventoryDataManager = invDataMgr;
        }

        public void Load ()
        {
            var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, WEAPON_JSON_PATH);
            var weaponJson = System.IO.File.ReadAllText (path);
            Debug.Log ("Read Weapon JSON");
            var weaponSheet = JsonUtility.FromJson<WeaponSheet> (weaponJson);
            Debug.Log ("Loaded Weapon JSON Sheet");

            _weaponDict = new Dictionary<string, WeaponRow> ();

            foreach (WeaponRow wr in weaponSheet.weapons) {
                var code = wr.InventoryCode;

                if ((code != null) &&
                    (code.Length > 1)) {
                    _weaponDict [code] = wr;
                    _inventoryDataManager.AddDesc (wr);
                } else {
                    Debug.LogWarningFormat ("No inventory code for {0}", wr.Name);
                }
            }
            Debug.Log ("made weapon dict");
        }

        public WeaponRow GetByCode (string code)
        {
            if (_weaponDict.ContainsKey (code)) {
                Debug.LogFormat ("found code {0}", code);
                return _weaponDict [code];
            }
            Debug.LogWarningFormat ("looking for weapon {0}, but not found", code);
            return null;
        }
    }
}
