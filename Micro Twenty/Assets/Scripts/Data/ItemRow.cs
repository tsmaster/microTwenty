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

        public string CANUSE;
        public string CE_HAND;

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
            return CE_HAND == "yes";
        }

        public bool CanEquipHead ()
        {
            return false;
        }

        public bool CanUseCombat ()
        {
            return CANUSE == "yes";
        }

        public bool CanUseNonCombat ()
        {
            return CANUSE == "yes";
        }

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