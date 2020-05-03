using System;
namespace MicroTwenty
{
    [Serializable]
    public class Party
    {
        public Character [] characters;

        public int money;

        public InventoryItem [] inventory;
    }
}
