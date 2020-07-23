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

        public int currentHitPoints;
        public int currentManaPoints;

        // stats
        public int stat_str;
        public int stat_int;
        public int stat_wis;
        public int stat_dex;
        public int stat_con;
        public int stat_cha;

        public SpriteId SpriteID { get; set; }


        public Dictionary<ItemEquipLocation, IInventoryDesc> equippedItems;

        public Character (string name, int hitPoints)
        {
            this.Name = name;
            this.hitPoints = hitPoints;
            this.currentHitPoints = hitPoints;
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

            int spriteIndex = UnityEngine.Random.Range (0, 6);

            switch (spriteIndex) {
                case 0: SpriteID = SpriteId.SPRITE_COMBAT_GUY_1; break;
                case 1: SpriteID = SpriteId.SPRITE_COMBAT_GUY_2; break;
                case 2: SpriteID = SpriteId.SPRITE_COMBAT_GUY_3; break;
                case 3: SpriteID = SpriteId.SPRITE_COMBAT_GUY_4; break;
                case 4: SpriteID = SpriteId.SPRITE_COMBAT_GUY_5; break;
                case 5: SpriteID = SpriteId.SPRITE_COMBAT_GUY_6; break;
            }
        }

        private int Roll3d6 ()
        {
            return UnityEngine.Random.Range (0, 6) +
                UnityEngine.Random.Range (0, 6) +
                UnityEngine.Random.Range (0, 6) +
                3;
        }

        // TODO add status flags, buffs, curses
        public string GetStatusString ()
        {
            if (currentHitPoints <= 0) {
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

            return Mathf.CeilToInt (armorValue);
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

        public int GetCombatMove ()
        {
            // TODO determine from stats
            return 3;
        }

        internal List<WeaponRow> GetWeapons ()
        {
            List<WeaponRow> weapons = new List<WeaponRow> ();

            foreach (IInventoryDesc equippedItem in equippedItems.Values) {
                WeaponRow weaponItem = equippedItem as WeaponRow;
                if (weaponItem != null) {
                    weapons.Add (weaponItem);
                }
            }
            return weapons;
        }

        public List<ArmorRow> GetArmors ()
        {
            List<ArmorRow> armors = new List<ArmorRow> ();

            foreach (IInventoryDesc equippedItem in equippedItems.Values) {
                ArmorRow armorItem = equippedItem as ArmorRow;
                if (armorItem != null) {
                    armors.Add (armorItem);
                }
            }
            return armors;
        }

        public List<SpellRep> GetSpells ()
        {
            List<SpellRep> spells = new List<SpellRep>();
            return spells;
        }

        public void SetCurrentHP (int hp)
        {
            currentHitPoints = hp;
        }

        public void SetCurrentMP (int mp)
        {
            currentManaPoints = mp;
        }
    }
}