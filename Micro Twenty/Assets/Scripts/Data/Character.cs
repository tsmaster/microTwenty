using System;

namespace MicroTwenty
{
    [Serializable]
    public class Character
    {
        public string Name;
        public int hitPoints;
        public int manaPoints;
        public string WeaponCode;
        public string ArmorCode;
        public int experiencePoints;
        public int level;
        public string Class;
        public InventoryItem [] inventoryItems;
    }
}