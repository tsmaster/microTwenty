using System;

namespace MicroTwenty
{
    [Serializable]
    public class ArmorRow : IInventoryDesc
    {
        // from InventoryItem
        public string InventoryCode;
        public string Name;
        public int Cost;

        public int DR;
        public float THR;
        public string Location;

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