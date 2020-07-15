using UnityEngine;
using System.Collections.Generic;
using System;

namespace MicroTwenty
{
    public class EquipmentBuildingUi : BaseStoreUi
    {
        public EquipmentBuildingUi (string name, GameMgr gameMgr) : base (name, gameMgr)
        {
            LoadBackgroundTexture ("Sprites/Dialogs/axe");

            InitializeInventory ();

            ShowMenu ();
        }

        void InitializeInventory ()
        {
            var idm = GameMgr.GetInventoryDataManager ();

            AddInventoryItem (idm.GetByCode ("BPCK"), 10);
            AddInventoryItem (idm.GetByCode ("PL10"), 10);
            AddInventoryItem (idm.GetByCode ("RTNS"), 20);
            AddInventoryItem (idm.GetByCode ("SACK"), -1);
            AddInventoryItem (idm.GetByCode ("TORC"), -1);
        }
    }
}
