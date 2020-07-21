using System;
using System.Collections.Generic;
using UnityEngine;

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

        // stats
        public int stat_str;
        public int stat_int;
        public int stat_wis;
        public int stat_dex;
        public int stat_con;
        public int stat_cha;

        public Dictionary<ItemEquipLocation, IInventoryDesc> equippedItems;

        public Character (string name, int hitPoints)
        {
            this.Name = name;
            this.hitPoints = hitPoints;
            equippedItems = new Dictionary<ItemEquipLocation, IInventoryDesc> ();

            RollStats ();
        }

        private void RollStats ()
        {
            stat_str = Roll3d6 ();
            stat_int = Roll3d6 ();
            stat_wis = Roll3d6 ();
            stat_dex = Roll3d6 ();
            stat_con = Roll3d6 ();
            stat_cha = Roll3d6 ();
        }

        private int Roll3d6 ()
        {
            return UnityEngine.Random.Range (0,6) +
                UnityEngine.Random.Range (0, 6) +
                UnityEngine.Random.Range (0, 6) +
                3;
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

        // TODO revisit what armor class really means
        public int GetArmorClass ()
        {
            float armorValue = 0;
            foreach (IInventoryDesc equippedItem in equippedItems.Values) {
                ArmorRow armorItem = equippedItem as ArmorRow;
                if (armorItem != null) {
                    armorValue += armorItem.THR;
                    armorValue += armorItem.DR;
                }
            }

            return Mathf.CeilToInt(armorValue);
        }

        public void Equip (ItemEquipLocation loc, IInventoryDesc item, GameMgr gameMgr)
        {
            IInventoryDesc oldItem = null;
            if (equippedItems.ContainsKey (loc)) {
                oldItem = equippedItems [loc];
            }

            if (oldItem != null) {
                gameMgr.Party.AddInventoryItem (oldItem);
            }

            equippedItems [loc] = item;
        }
    }
}