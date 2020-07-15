using UnityEngine;
using System.Collections.Generic;
using System;

namespace MicroTwenty
{
    public class WeaponBuildingUi : BaseStoreUi
    {
        public WeaponBuildingUi (string name, GameMgr gameMgr) : base (name, gameMgr)
        {
            LoadBackgroundTexture ("Sprites/Dialogs/axe");

            InitializeInventory ();

            ShowMenu ();
        }

        void InitializeInventory ()
        {
            var wdm = GameMgr.GetWeaponDataManager ();

            AddInventoryItem (wdm.GetByCode ("SW"), 10);
            AddInventoryItem (wdm.GetByCode ("AXE"), 20);
            AddInventoryItem (wdm.GetByCode ("BOW"), 8);
            AddInventoryItem (wdm.GetByCode ("DAGGER"), -1);
            AddInventoryItem (wdm.GetByCode ("CLUB"), 25);
            AddInventoryItem (wdm.GetByCode ("MACE"), 20);
            AddInventoryItem (wdm.GetByCode ("HALBRD"), 4);
        }
    }
}
