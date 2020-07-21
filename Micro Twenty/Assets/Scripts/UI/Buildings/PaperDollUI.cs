using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class PaperDollUi : BuildingUi
    {
        private string _name;
        private GameMgr _gameMgr;
        private Texture2D _targetTexture;
        private Texture2D _fontTexture;
        private int _margin;
        private int _textureWidth;
        private int _textureHeight;
        private Texture2D _backgroundTexture;
        private Character _character;
        private MenuManager _menuMgr;
        private MenuObject _charMenu;
        private MenuObject _equipItemMenu;

        public PaperDollUi (GameMgr gameMgr)
        {
            _name = "Paper Doll";
            _gameMgr = gameMgr;
            _targetTexture = _gameMgr.GetMapManager ().GetTargetTexture ();
            _fontTexture = _gameMgr.GetMapManager ().GetFontBitmap ();

            _margin = 5;
            _textureWidth = _targetTexture.width;
            _textureHeight = _targetTexture.height;

            _backgroundTexture = Resources.Load<Texture2D> ("Sprites/Dialogs/paperdoll");

            _character = null;

            var mapManager = _gameMgr.GetMapManager ();
            var menuBitmap = mapManager.GetMenuBitmap ();
            var fontBitmap = mapManager.GetFontBitmap ();

            _menuMgr = new MenuManager (menuBitmap, fontBitmap);
            _charMenu = new MenuObject ("character menu", menuBitmap, fontBitmap);
            _charMenu.SetWindow (1, 6);

            _charMenu.AddItem ("Stats");
            _equipItemMenu = _charMenu.AddItem ("Equip Item");
            _equipItemMenu.SetWindow (1, 4);
            UpdateEquipItemMenu ();

            _charMenu.AddItem ("Use Item");
            _charMenu.AddItem ("Play Song");
            _charMenu.AddItem ("Cast Spell");
            _charMenu.AddItem ("Exit").SetAction (() => { _gameMgr.ExitBuilding (); });

            _charMenu.Build ();

            _menuMgr.OpenMenu (_charMenu);
        }

        private void UpdateEquipItemMenu ()
        {
            _equipItemMenu.ClearItems ();
            if (_character == null) {
                _equipItemMenu.SetEnabled (false);
                return;
            }
            _equipItemMenu.SetEnabled (true);

            var rHandEquipMenu = _equipItemMenu.AddItem ("Right Hand");
            AddEquipMenu (rHandEquipMenu, Character.ItemEquipLocation.HAND_RIGHT);
            var lHandEquipMenu = _equipItemMenu.AddItem ("Left Hand");
            AddEquipMenu (lHandEquipMenu, Character.ItemEquipLocation.HAND_LEFT);
            var bodyEquipMenu = _equipItemMenu.AddItem ("Body");
            AddEquipMenu (bodyEquipMenu, Character.ItemEquipLocation.BODY);
            var headEquipMenu = _equipItemMenu.AddItem ("Head");
            AddEquipMenu (headEquipMenu, Character.ItemEquipLocation.HEAD);
            var feetEquipMenu = _equipItemMenu.AddItem ("Feet");
            AddEquipMenu (feetEquipMenu, Character.ItemEquipLocation.FEET);

            _equipItemMenu.Build ();
        }

        private void AddEquipMenu (MenuObject locationEquipMenu, Character.ItemEquipLocation loc)
        {
            locationEquipMenu.ClearItems ();

            var itemList = GetItemsForLocation (loc);
            if (itemList.Count == 0) {
                locationEquipMenu.SetEnabled (false);
                return;
            }

            locationEquipMenu.SetWindow (1, 5);

            foreach (var item in itemList) {
                locationEquipMenu.AddItem (item.GetName ()).SetAction (() => {
                    _gameMgr.Party.RemoveInventoryItem (item);
                    _character.Equip (loc, item, _gameMgr); });
            }

            locationEquipMenu.Build ();
        }

        private List<IInventoryDesc> GetItemsForLocation (Character.ItemEquipLocation loc)
        {
            List<IInventoryDesc> items = new List<IInventoryDesc> ();

            foreach (var partyItem in _gameMgr.Party.inventory) {
                if (CanEquipItemAtLocation (partyItem.Item, loc)) {
                    items.Add (partyItem.Item);
                }
            }

            return items;
        }

        private bool CanEquipItemAtLocation (IInventoryDesc item, Character.ItemEquipLocation loc) {
            switch (loc) {
            case Character.ItemEquipLocation.BODY:
                return item.CanEquipBody ();
            case Character.ItemEquipLocation.HEAD:
                return item.CanEquipHead ();
            case Character.ItemEquipLocation.HAND_LEFT:
            case Character.ItemEquipLocation.HAND_RIGHT:
                return item.CanEquipHands ();
            case Character.ItemEquipLocation.FEET:
                return item.CanEquipFeet ();
            default:
                Debug.LogErrorFormat ("unknown loc {0}", loc);
                return false;
            }
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

            if (_character != null) {
                TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                    string.Format ("STR {0}  INT {1}  WIS {2}",
                        _character.stat_str,
                        _character.stat_int,
                        _character.stat_wis),
                    _textureWidth / 2, _textureHeight - 30,
                    Color.black);
                TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                    string.Format ("DEX {0}  CON {1}  CHA {2}",
                        _character.stat_dex,
                        _character.stat_con,
                        _character.stat_cha),
                    _textureWidth / 2, _textureHeight - 40,
                    Color.black);
                TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                    string.Format ("AC {0}  HP {1}  MP {2}",
                        _character.GetArmorClass(),
                        _character.hitPoints,
                        _character.manaPoints),
                    _textureWidth / 2, _textureHeight - 50,
                    Color.black);
                TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                    string.Format ("LVL {0}  EXP {1}  ST {2}",
                        _character.level,
                        _character.experiencePoints,
                        _character.GetStatusString()),
                    _textureWidth / 2, _textureHeight - 60,
                    Color.black);

            }

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                "X - eXit",
                _textureWidth / 2, 20,
                Color.black);

            _menuMgr.Draw (_targetTexture, _margin * 2, 100);

            _targetTexture.Apply ();
        }

        public override void Update (float deltaSeconds)
        {
            if (_character == null) {
                var c = _gameMgr.GetMapManager ().GetCharacterForPaperDoll ();
                if (c != null) {
                    SetCharacter (c);
                }
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
                    default:
                        Debug.LogFormat ("got select for resID {0}", resId);
                        break;
                    }
                }
            }


            if (Input.GetKeyDown (KeyCode.X)) {
                _gameMgr.ExitBuilding ();
            }
        }

        private void SetCharacter (Character c)
        {
            _character = c;
            _name = c.Name;
            UpdateEquipItemMenu ();
        }
    }
}
