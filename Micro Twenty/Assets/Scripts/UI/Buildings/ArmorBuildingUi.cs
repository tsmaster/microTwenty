using UnityEngine;

namespace MicroTwenty
{
    public class ArmorBuildingUi : BaseStoreUi
    {
        public ArmorBuildingUi (string name, GameMgr gameMgr) : base(name, gameMgr)
        {
            LoadBackgroundTexture ("Sprites/Dialogs/helmets");

            InitializeInventory ();

            ShowMenu ();
        }

        void InitializeInventory ()
        {
            var adm = GameMgr.GetArmorDataManager ();

            AddInventoryItem (adm.GetByCode ("CLOTH"), -1);
            AddInventoryItem (adm.GetByCode ("LEATH"), 5);
            AddInventoryItem (adm.GetByCode ("CHAIN"), 5);
            AddInventoryItem (adm.GetByCode ("PLATE"), 2);
            AddInventoryItem (adm.GetByCode ("SHIELD"), 6);
            AddInventoryItem (adm.GetByCode ("HELM"), 3);
        }
    }
}
