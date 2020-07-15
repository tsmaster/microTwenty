using UnityEngine;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class InventoryDataManager
    {
        const string ITEMS_JSON_PATH = "Resources/JSON/items.json";
        Dictionary<string, IInventoryDesc> _inventoryDict;

        public InventoryDataManager ()
        {
            _inventoryDict = new Dictionary<string, IInventoryDesc> ();
        }

        public void Load ()
        {
            var path = System.IO.Path.Combine (UnityEngine.Application.dataPath, ITEMS_JSON_PATH);
            var itemJson = System.IO.File.ReadAllText (path);
            Debug.Log ("Read Items JSON");
            var itemSheet = JsonUtility.FromJson<ItemSheet> (itemJson);
            Debug.Log ("Loaded Items JSON Sheet");

            foreach (IInventoryDesc desc in itemSheet.items) {
                var code = desc.GetInventoryCode();

                if ((code != null) &&
                    (code.Length > 1)) {
                    _inventoryDict [code] = desc;
                } else {
                    Debug.LogWarningFormat ("No inventory code for {0}", desc.GetName());
                }
            }
            Debug.Log ("made inventory dict");
        }

        public IInventoryDesc GetByCode (string code)
        {
            if (_inventoryDict.ContainsKey (code)) {
                Debug.LogFormat ("found code {0}", code);
                return _inventoryDict [code];
            }
            Debug.LogWarningFormat ("looking for item {0}, but not found", code);
            return null;
        }

        public void AddDesc (IInventoryDesc desc)
        {
            var code = desc.GetInventoryCode ();

            if ((code != null) &&
                (code.Length > 1)) {
                _inventoryDict [code] = desc;
            } else {
                Debug.LogWarningFormat ("No inventory code for {0}", desc.GetName ());
            }
        }
    }
}
