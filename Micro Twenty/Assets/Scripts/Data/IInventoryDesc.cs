using System;

namespace MicroTwenty
{
    public interface IInventoryDesc
    {
        string GetInventoryCode ();
        string GetName ();
        int GetCost ();
    }

    public class InventoryQuantity
    {
        public int Count;
        public IInventoryDesc Item;
    }
}