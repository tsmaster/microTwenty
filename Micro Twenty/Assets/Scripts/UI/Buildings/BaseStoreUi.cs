using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty 
{
    abstract public class BaseStoreUi : BuildingUi
    {
        private string _name;
        private GameMgr _gameMgr;

        private Texture2D _targetTexture;
        private Texture2D _fontTexture;
        private int _margin;
        private int _textureWidth;
        private int _textureHeight;
        private Texture2D _backgroundTexture;

        private List<IInventoryDesc> _inventoryItems;
        private Dictionary<string, int> _inventoryQuantities;

        private MenuManager _menuMgr;
        private MenuObject _storeMenu;
        private MenuObject _buyItemsMenu;
        private MenuObject _sellItemsMenu;

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public GameMgr GameMgr {
            get { return _gameMgr; }
            set { _gameMgr = value; }
        }

        public BaseStoreUi (string name, GameMgr gameMgr)
        {
            Name = name;
            GameMgr = gameMgr;

            _targetTexture = _gameMgr.GetMapManager ().GetTargetTexture ();
            _fontTexture = _gameMgr.GetMapManager ().GetFontBitmap ();

            _margin = 5;
            _textureWidth = _targetTexture.width;
            _textureHeight = _targetTexture.height;

            // TODO store inventory should be replaced with dynamic supply/demand
            _inventoryItems = new List<IInventoryDesc> ();
            // TODO replace this system with dynamic supply/demand system
            _inventoryQuantities = new Dictionary<string, int> ();
        }

        protected void ShowMenu ()
        {
            MakeMenu ();
            _menuMgr.OpenMenu (_storeMenu);
        }

        protected void LoadBackgroundTexture (string textureName)
        {
            _backgroundTexture = Resources.Load<Texture2D> (textureName);
        }

        protected void AddInventoryItem (IInventoryDesc desc, int quantity)
        {
            _inventoryItems.Add (desc);
            _inventoryQuantities [desc.GetInventoryCode ()] = quantity;
        }


        private void MakeMenu ()
        {
            Debug.Log ("Making menu");
            var menuBitmap = _gameMgr.GetMapManager ().GetMenuBitmap ();
            var fontBitmap = _gameMgr.GetMapManager ().GetFontBitmap ();
            _menuMgr = new MenuManager (menuBitmap, fontBitmap);
            _menuMgr.DismissOnAction = false;
            _menuMgr.CanBackOutOfMenu = false;

            _storeMenu = new MenuObject ("TODO store menu", menuBitmap, fontBitmap);
            _storeMenu.SetWindow (1, 4);
            _buyItemsMenu = _storeMenu.AddItem ("Buy Items");
            _buyItemsMenu.SetWindow (1, 4);
            UpdateBuyMenu ();
            _sellItemsMenu = _storeMenu.AddItem ("Sell Inventory");
            _sellItemsMenu.SetWindow (1, 4);
            UpdateSellMenu ();
            _storeMenu.AddItem ("Identify Item");
            _storeMenu.AddItem ("Exit Store").SetAction (_gameMgr.ExitBuilding);
            _storeMenu.Build ();
        }

        private void UpdateBuyMenu ()
        {
            _buyItemsMenu.ClearItems ();

            foreach (IInventoryDesc item in _inventoryItems) {
                string invCode = item.GetInventoryCode ();
                string invName = item.GetName ();
                //Debug.LogFormat ("adding {0} {1} {2} to menu", item, invCode, invName);
                if ((invName != null) &&
                    (invName.Length > 0)) {
                    var quant = _inventoryQuantities [invCode];
                    if (quant == 0) {
                        continue;
                    }
                    var costLine = MakeCostLine (invName, item.GetCost(), quant, 24);
                    _buyItemsMenu.AddItem (costLine).SetEnabled (item.GetCost() <= _gameMgr.Party.Gold).
                        SetAction (() => {
                            BuyItem (invCode);
                            UpdateBuyMenu ();
                            UpdateSellMenu ();
                        });
                }
            }
            _buyItemsMenu.Build ();
        }

        private void UpdateSellMenu ()
        {
            _sellItemsMenu.ClearItems ();

            foreach (InventoryQuantity itemQuant in _gameMgr.Party.inventory) {
                var item = itemQuant.Item;
                var count = itemQuant.Count;
                var itemName = item.GetName ();
                var itemCode = item.GetInventoryCode ();

                // TODO roll this in to sell calculation, elsewhere
                var itemSellPrice = item.GetCost () / 2;

                Debug.LogFormat ("adding {0} {1} {2} to menu", item, itemCode, itemName);
                if ((itemName != null) &&
                    (itemName.Length > 0)) {
                    if (count == 0) {
                        continue;
                    }
                    var costLine = MakeCostLine (itemName, itemSellPrice, count, 24);
                    _sellItemsMenu.AddItem (costLine).
                        SetAction (() => {
                            SellItem (itemCode);
                            UpdateBuyMenu ();
                            UpdateSellMenu ();
                        });
                }
            }
            _sellItemsMenu.Build ();
        }

        private IInventoryDesc GetDescFromCode (string inventoryCode)
        {
            foreach (var desc in _inventoryItems) {
                if (desc.GetInventoryCode () == inventoryCode) {
                    return desc;
                }
            }

            Debug.LogErrorFormat ("Can't find desc for code {0}", inventoryCode);
            return null;
        }

        private void BuyItem (string inventoryCode)
        {
            IInventoryDesc desc = GetDescFromCode (inventoryCode);
            Debug.Assert (desc != null);

            int quantityInStore = _inventoryQuantities [inventoryCode];
            int newQuantity = quantityInStore;
            if (quantityInStore == -1) {
                // it's fine, we've got arbitrary amounts of this
            } else if (quantityInStore > 0) {
                newQuantity = quantityInStore - 1;
            } else {
                // trying to buy something that there's zero of?
                Debug.LogErrorFormat ("Trying to buy from zero stock for {0}", inventoryCode);
                return;
            }

            Debug.LogFormat ("Bought one {0}, new quantity {1}", inventoryCode, newQuantity);
            _inventoryQuantities [inventoryCode] = newQuantity;

            _gameMgr.Party.Gold -= desc.GetCost();
            _gameMgr.Party.AddInventoryItem (desc);
        }

        private void SellItem (string inventoryCode)
        {
            IInventoryDesc desc = GetDescFromCode(inventoryCode);
            Debug.Assert (desc != null);

            int quantityInPartyInventory = _gameMgr.Party.GetQuantity (inventoryCode);

            if (quantityInPartyInventory == 0) {
                Debug.LogErrorFormat ("Trying to sell {0} when the party has none", inventoryCode);
                return;
            }

            // TODO make this dependant on CHA?
            var sellPrice = desc.GetCost() / 2;

            if (_inventoryQuantities.ContainsKey (inventoryCode)) {
                if (_inventoryQuantities [inventoryCode] >= 0) {
                    _inventoryQuantities [inventoryCode] += 1;
                } else {
                    // do nothing, this is the infinite supply case
                }
            } else {
                _inventoryQuantities [inventoryCode] = 1;
            }

            _gameMgr.Party.Gold += sellPrice;
            _gameMgr.Party.RemoveInventoryItem (desc);
        }


        private string MakeCostLine (string name, int cost, int count, int len)
        {
            var costStr = cost.ToString ();

            var countStr = count.ToString ();
            if (count == -1) {
                countStr = "--";
            }

            var endingString = string.Format ("{0}g [{1}]", costStr, countStr);

            if (name.Length + endingString.Length > len) {
                return name.Substring (0, len - endingString.Length) + costStr;
            }

            var padding = "";
            while (name.Length + padding.Length + endingString.Length < len) {
                padding = padding + " ";
            }

            return name + padding + endingString;
        }

        public override void Draw ()
        {
            TextureDrawing.DrawRect (_targetTexture,
                _margin, _margin,
                _textureWidth - 2 * _margin, _textureHeight - 2 * _margin,
                Color.white, Color.green, true, true);

            TextureDrawing.DrawPartialSprite (_targetTexture, _backgroundTexture,
                2 * _margin,
                _targetTexture.height - 2 * _margin - _backgroundTexture.height,
                0, 0,
                _backgroundTexture.width, _backgroundTexture.height);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                _name,
                _textureWidth / 2, _textureHeight - 20,
                Color.black);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                "X - eXit",
                _textureWidth / 2, 20,
                Color.black);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                string.Format ("{0}g", _gameMgr.Party.Gold),
                _textureWidth / 2, _textureHeight - 40,
                Color.black);

            _menuMgr.Draw (_targetTexture, 0, 100);

            _targetTexture.Apply ();
        }

        public override void Update (float deltaSeconds)
        {
            if (Input.GetKeyDown (KeyCode.UpArrow)) {
                Debug.Log ("Up arrow");
                _menuMgr.OnUp ();
            }
            if (Input.GetKeyDown (KeyCode.DownArrow)) {
                _menuMgr.OnDown ();
            }
            if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                _menuMgr.OnLeft ();
            }
            if (Input.GetKeyDown (KeyCode.RightArrow)) {
                _menuMgr.OnRight ();
            }
            if ((Input.GetKeyDown (KeyCode.Space)) ||
                (Input.GetKeyDown (KeyCode.Return)) ||
                (Input.GetButtonDown ("Submit"))) {

                var result = _menuMgr.OnActivate ();

                // TODO use menu item
            }
            if ((Input.GetKeyDown (KeyCode.Escape)) ||
                (Input.GetButtonDown ("Cancel"))) {
                _menuMgr.OnBack ();
            }

            if (Input.GetKeyDown (KeyCode.X)) {
                // TODO remove this, once the menu properly exits the store and doesn't exit the menu
                _gameMgr.ExitBuilding ();
            }
        }

    }
}
