using System;
using UnityEngine;

namespace MicroTwenty
{
    public class PartyUi : BuildingUi
    {
        enum PartyMenuId
        {

            EXIT,
        }

        private string _name;
        private GameMgr _gameMgr;
        private Texture2D _targetTexture;
        private Texture2D _fontTexture;
        private int _margin;
        private int _textureWidth;
        private int _textureHeight;
        private Texture2D _partyTexture;
        private MenuManager _menuMgr;
        private MenuObject _partyMenu;
        private MenuObject _inventoryMenu;

        public PartyUi (GameMgr gameMgr)
        {
            _name = "Party";
            _gameMgr = gameMgr;
            _targetTexture = _gameMgr.GetMapManager ().GetTargetTexture ();
            _fontTexture = _gameMgr.GetMapManager ().GetFontBitmap ();

            _margin = 5;
            _textureWidth = _targetTexture.width;
            _textureHeight = _targetTexture.height;

            _partyTexture = Resources.Load<Texture2D> ("Sprites/Dialogs/party");

            var mapManager = _gameMgr.GetMapManager ();
            var menuBitmap = mapManager.GetMenuBitmap ();
            var fontBitmap = mapManager.GetFontBitmap ();

            _menuMgr = new MenuManager (menuBitmap, fontBitmap);
            _partyMenu = new MenuObject ("party menu", menuBitmap, fontBitmap);
            _partyMenu.SetWindow (1, 6);
            var charMenu = _partyMenu.AddItem ("Character");
            AddPartySubMenu (charMenu, (Character c) => { ShowPaperDoll (c); });
            _partyMenu.AddItem ("Cast Spell");
            _partyMenu.AddItem ("Marching Orders");
            _inventoryMenu = _partyMenu.AddItem ("Show Inventory");
            _inventoryMenu.SetWindow (1, 5);
            UpdateInventoryMenu ();
            _partyMenu.AddItem ("Sleep");
            _partyMenu.AddItem ("Save Game");
            _partyMenu.AddItem ("Exit Game");
            _partyMenu.AddItem ("Exit Party Menu");
            _partyMenu ["Exit Party Menu"].SetItemId ((int)PartyMenuId.EXIT);

            _partyMenu.Build ();

            _menuMgr.OpenMenu (_partyMenu);
        }

        private void ShowPaperDoll (Character c)
        {
            _gameMgr.ExitBuilding ();
            //_gameMgr.AddCommand (new ExitBuildingCommand (_gameMgr));
            _gameMgr.AddCommand (new ShowPaperDollCommand (c, _gameMgr));
        }

        private void UpdateInventoryMenu ()
        {
            _inventoryMenu.ClearItems ();

            if (_gameMgr.Party.inventory.Count == 0) {
                _inventoryMenu.SetEnabled (false);
                return;
            } else {
                _inventoryMenu.SetEnabled (true);
            }

            foreach (var invItem in _gameMgr.Party.inventory) {
                var invCount = invItem.Count;
                var invCode = invItem.Item.GetInventoryCode ();
                var invName = invItem.Item.GetName ();

                var itemMenu = _inventoryMenu.AddItem (string.Format ("{0} ({1})", invName, invCount));
                itemMenu.SetWindow (1, 4);
                var equipMenuItem = itemMenu.AddItem ("Equip");
                AddPartySubMenu (equipMenuItem, (Character c) => { ShowCharEquipScreen (c, invItem.Item); });
                var useMenuItem = itemMenu.AddItem ("Use");
                AddPartySubMenu (useMenuItem, (Character c) => { UseMenuItem (c, invItem.Item); });
                itemMenu.AddItem ("Drop").SetAction (() => { DropItem (invItem.Item); });
            }

            _inventoryMenu.Build ();
        }

        private void DropItem (IInventoryDesc item)
        {
            throw new NotImplementedException ();
        }

        private void UseMenuItem (Character c, IInventoryDesc item)
        {
            throw new NotImplementedException ();
        }

        private void ShowCharEquipScreen (Character c, IInventoryDesc item)
        {
            throw new NotImplementedException ();
        }

        private void AddPartySubMenu (MenuObject menuItem, Action<Character> p)
        {
            menuItem.ClearItems ();
            menuItem.SetWindow (1, 6);
            foreach (var character in _gameMgr.Party.characters) {
                menuItem.AddItem (character.Name).SetAction(() => { p (character); });
            }
            menuItem.Build ();
        }

        public override void Draw ()
        {
            TextureDrawing.DrawRect (_targetTexture,
                _margin, _margin,
                _textureWidth - 2 * _margin, _textureHeight - 2 * _margin,
                Color.white, Color.green, true, true);

            var bgWidth = _partyTexture.width;
            var bgHeight = _partyTexture.height;

            var bgLeft = (_targetTexture.width - bgWidth) / 2;
            var bgBottom = (_targetTexture.height - bgHeight) / 2;

            TextureDrawing.DrawPartialSprite (_targetTexture, _partyTexture,
                bgLeft, bgBottom,
                0, 0,
                bgWidth, bgHeight);

            TextureDrawing.DrawRect (_targetTexture,
                bgLeft, bgBottom, bgWidth, bgHeight, new Color (1.0f, 1.0f, 1.0f, 0.75f), Color.white, true, false);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                _name,
                _textureWidth / 2, _textureHeight - 20,
                Color.black);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                string.Format("{0}g", _gameMgr.Party.Gold),
                _textureWidth / 2, _textureHeight - 30,
                Color.black);


            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                "X - eXit",
                _textureWidth / 2, 20,
                Color.black);

            // draw party grid
            for (int i = 0; i < _gameMgr.Party.characters.Count; ++i) {
                var c = _gameMgr.Party.characters [i];
                var s = string.Format ("{0} {1} {2} {3} {4}", i, c.Name, c.hitPoints, c.manaPoints, c.GetStatusString());

                TextureDrawing.DrawStringAt (_targetTexture, _fontTexture,
                    s,
                    _margin, _textureHeight - 40 - 8 * i,
                    Color.black);
            }

            _menuMgr.Draw (_targetTexture, _margin * 2, 100);

            _targetTexture.Apply ();
        }

        public override void Update (float deltaSeconds)
        {
            if (Input.GetKeyDown (KeyCode.X)) {
                _gameMgr.ExitBuilding ();
            }

            if (Input.GetKeyDown (KeyCode.UpArrow)) {
                _menuMgr.OnUp ();
            } else if (Input.GetKeyDown (KeyCode.DownArrow)) {
                _menuMgr.OnDown ();
            } else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                // show menu
                _menuMgr.OnLeft ();
            } else if (Input.GetKeyDown (KeyCode.RightArrow)) {
                // show menu
                _menuMgr.OnRight ();
            } else if ((Input.GetKeyDown (KeyCode.Space)) ||
                (Input.GetKeyDown (KeyCode.Return)) ||
                (Input.GetButtonDown ("Submit"))) {

                var result = _menuMgr.OnActivate ();
                if (result != null) {
                    var resId = result.GetItemId ();
                    switch (resId) {
                    case (int)PartyMenuId.EXIT:
                        _gameMgr.ExitBuilding ();
                        break;
                    default:
                        Debug.LogFormat ("got select for resID {0}", resId);
                        break;
                    }
                }
            }
        }
    }
}