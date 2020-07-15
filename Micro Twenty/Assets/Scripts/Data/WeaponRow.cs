using System;

namespace MicroTwenty
{
    [Serializable]
    public class WeaponRow : IInventoryDesc
    {
        // from InventoryItem
        public string InventoryCode;
        public string Name;
        public int Cost;

        public int numDice;
        public int diceSides;
        public int dmgMod;
        public int thMod;
        public string Tags;
        public string Range;

        int IInventoryDesc.GetCost ()
        {
            return Cost;
        }

        string IInventoryDesc.GetInventoryCode ()
        {
            return InventoryCode;
        }

        string IInventoryDesc.GetName ()
        {
            return Name;
        }
    }
}