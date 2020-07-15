using UnityEngine;
using System.Collections.Generic;
using System;

namespace MicroTwenty
{
    public class HealingBuildingUi : BaseStoreUi
    {
        public HealingBuildingUi (string name, GameMgr gameMgr) : base (name, gameMgr)
        {
            LoadBackgroundTexture ("Sprites/Dialogs/potion");

            InitializeInventory ();

            ShowMenu ();
        }

        void InitializeInventory ()
        {
            var idm = GameMgr.GetInventoryDataManager ();

            AddInventoryItem (idm.GetByCode ("ANTO"), 10);
            AddInventoryItem (idm.GetByCode ("BAND"), 10);
            AddInventoryItem (idm.GetByCode ("FAKT"), 10);
            AddInventoryItem (idm.GetByCode ("HPOT"), 10);
        }
    }
}
