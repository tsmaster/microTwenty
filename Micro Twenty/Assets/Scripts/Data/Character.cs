using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    [Serializable]
    public class Character
    {
        public enum ItemEquipLocation
        {
            NONE,
            HAND_LEFT,
            HAND_RIGHT,
            HEAD,
            BODY,
            FEET
        };

        public string Name;
        public int hitPoints;
        public int manaPoints;
        public string WeaponCode;
        public string ArmorCode;
        public int experiencePoints;
        public int level;
        public string Class;

        public Dictionary<ItemEquipLocation, IInventoryDesc> equippedItems;

        public Character (string name, int hitPoints)
        {
            this.Name = name;
            this.hitPoints = hitPoints;
            equippedItems = new Dictionary<ItemEquipLocation, IInventoryDesc> ();
        }

        // TODO add status flags, buffs, curses
        public string GetStatusString ()
        {
            if (hitPoints <= 0) {
                return "DE";
            } else {
                return "OK";
            }
        }

        public void Equip (ItemEquipLocation loc, IInventoryDesc item, GameMgr gameMgr)
        {
            IInventoryDesc oldItem = null;

            // TODO unequip old item

            equippedItems [loc] = item;
        }
    }
}