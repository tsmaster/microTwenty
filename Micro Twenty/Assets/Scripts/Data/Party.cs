using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    [Serializable]
    public class Party
    {
        public Character [] characters;

        public int Gold;

        public List<InventoryQuantity> inventory;

        public Party ()
        {
            inventory = new List<InventoryQuantity> ();
        }

        internal void AddInventoryItem (IInventoryDesc desc)
        {
            foreach (var invQuant in inventory) {
                if (invQuant.Item.GetInventoryCode () == desc.GetInventoryCode()) {
                    invQuant.Count += 1;
                    return;
                }
            }

            inventory.Add (new InventoryQuantity {
                Count = 1,
                Item = desc
            });
        }

        internal void RemoveInventoryItem (IInventoryDesc desc)
        {
            foreach (var invQuant in inventory) {
                if (invQuant.Item.GetInventoryCode () == desc.GetInventoryCode ()) {
                    invQuant.Count -= 1;
                    return;
                }
            }

            Debug.LogErrorFormat ("trying to remove item {0} from inventory, but could not find it", desc.GetName ());
        }


        public int GetQuantity (string inventoryCode)
        {
            foreach (var invQuant in inventory) {
                if (invQuant.Item.GetInventoryCode() == inventoryCode) {
                    return invQuant.Count;
                }
            }
            return 0;
        }
    }
}
