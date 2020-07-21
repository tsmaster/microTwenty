using System;

namespace MicroTwenty
{
    public interface IInventoryDesc
    {
        string GetInventoryCode ();
        string GetName ();
        int GetCost ();

        bool CanEquipBody ();
        bool CanEquipHead ();
        bool CanEquipHands ();
        bool CanEquipFeet ();
        bool CanUseNonCombat ();
        bool CanUseCombat ();
    }

    public class InventoryQuantity
    {
        public int Count;
        public IInventoryDesc Item;
    }
}