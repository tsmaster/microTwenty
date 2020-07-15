using UnityEngine;
using System.Collections.Generic;
using System;

namespace MicroTwenty
{
    public class MagicBuildingUi : BaseStoreUi
    {
        public MagicBuildingUi (string name, GameMgr gameMgr) : base (name, gameMgr)
        {
            LoadBackgroundTexture ("Sprites/Dialogs/books");

            InitializeInventory ();

            ShowMenu ();
        }

        void InitializeInventory ()
        {
            var idm = GameMgr.GetInventoryDataManager ();

            AddInventoryItem (idm.GetByCode ("CAND"), 10);
            AddInventoryItem (idm.GetByCode ("FBSC"), 10);
            AddInventoryItem (idm.GetByCode ("SKMI"), 10);
            AddInventoryItem (idm.GetByCode ("BOOK"), 10);
        }
    }
}
