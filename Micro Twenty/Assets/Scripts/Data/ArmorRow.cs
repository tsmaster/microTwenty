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

        public string CE_BODY;
        public string CE_HEAD;
        public string CE_HAND;
        public string CE_FEET;

        public bool CanEquipBody ()
        {
            return CE_BODY == "yes";
        }

        public bool CanEquipFeet ()
        {
            return CE_FEET == "yes";
        }

        public bool CanEquipHands ()
        {
            return CE_HAND == "yes";
        }

        public bool CanEquipHead ()
        {
            return CE_HEAD == "yes";
        }

        public bool CanUseCombat ()
        {
            return false;
        }

        public bool CanUseNonCombat ()
        {
            return false;
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