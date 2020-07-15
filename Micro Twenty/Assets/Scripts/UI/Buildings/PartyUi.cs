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
            _partyMenu.AddItem ("Character");
            _partyMenu.AddItem ("Cast Spell");
            _partyMenu.AddItem ("Use Item");
            _partyMenu.AddItem ("Sing Song");
            _partyMenu.AddItem ("Marching Orders");
            _partyMenu.AddItem ("Sleep");
            _partyMenu.AddItem ("Save Game");
            _partyMenu.AddItem ("Exit Game");
            _partyMenu.AddItem ("Exit Party Menu");
            _partyMenu ["Exit Party Menu"].SetItemId ((int)PartyMenuId.EXIT);

            _partyMenu.Build ();

            _menuMgr.OpenMenu (_partyMenu);
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
                "X - eXit",
                _textureWidth / 2, 20,
                Color.black);

            _menuMgr.Draw (_targetTexture, _margin * 2, _targetTexture.height - _margin * 8);

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