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

        public bool CanEquipBody ()
        {
            return false;
        }

        public bool CanEquipFeet ()
        {
            return false;
        }

        public bool CanEquipHands ()
        {
            return true;
        }

        public bool CanEquipHead ()
        {
            return false;
        }

        public bool CanUseCombat ()
        {
            // TODO what about one-use items, like grenades?
            return false;
        }

        public bool CanUseNonCombat ()
        {
            return false;
        }

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