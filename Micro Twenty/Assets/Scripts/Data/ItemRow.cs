using System;

namespace MicroTwenty
{
    [Serializable]
    public class ItemRow : IInventoryDesc
    {
        // from InventoryItem
        public string InventoryCode;
        public string Name;
        public int Cost;

        public int GetCost ()
        {
            return Cost;
        }

        public string GetInventoryCode ()
        {
            return InventoryCode;
        }

        public string GetName ()
        {
            return Name;
        }
    }
}