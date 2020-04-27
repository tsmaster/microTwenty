using System;
using UnityEngine;

namespace MicroTwenty
{
    public class MainMenuScreen : IIntroScreen
    {
        private MapManager _mapManager;
        private MenuManager _menuMgr;
        private MenuObject _mainMenu;
        private Texture2D _titleTexture;

        private enum MenuOptions {
            LoadSavedGame,
            StartNewGame,
            Credits,
            Quit
        }

        public MainMenuScreen (MapManager mapManager, Texture2D microTwentyTexture)
        {
            _mapManager = mapManager;

            _menuMgr = new MenuManager (mapManager.GetMenuBitmap (), mapManager.GetFontBitmap ());
            _mainMenu = new MenuObject("main_menu", mapManager.GetMenuBitmap (), mapManager.GetFontBitmap ());
            _mainMenu.SetWindow (1, 3);
            _mainMenu.AddItem ("Load Saved Game").SetItemId((int)MenuOptions.LoadSavedGame).SetEnabled (false);
            _mainMenu.AddItem ("Start New Game").SetItemId ((int)MenuOptions.StartNewGame);
            _mainMenu.AddItem ("Credits").SetItemId ((int)MenuOptions.Credits);
            _mainMenu.AddItem ("Quit to BDGos").SetItemId ((int)MenuOptions.Quit).SetEnabled (false);
            _mainMenu.Build ();

            _titleTexture = microTwentyTexture;

            _menuMgr.OpenMenu (_mainMenu);
        }

        public void Draw ()
        {
            var targetTexture = _mapManager.GetTargetTexture ();

            // draw title texture into BG

            TextureDrawing.DrawPartialSprite (targetTexture, _titleTexture, 0, 0, 0, 0, _titleTexture.width, _titleTexture.height);

            // draw a gray rectangle over it

            TextureDrawing.DrawRect (targetTexture, 0, 0,
                targetTexture.width, targetTexture.height,
                new Color (0, 0, 0, 0.3f), Color.white, true, false);

            // draw menu
            _menuMgr.Draw (targetTexture, 50, 120);
            targetTexture.Apply ();
        }

        public void UpdateScreen (float deltaSeconds)
        {
            // update the menu

            if (Input.GetKeyDown (KeyCode.UpArrow)) {
                _menuMgr.OnUp ();
            }
            if (Input.GetKeyDown (KeyCode.DownArrow)) {
                _menuMgr.OnDown ();
            }
            if ((Input.GetKeyDown (KeyCode.Return)) ||
                (Input.GetKeyDown (KeyCode.Space))) {
                var res = _menuMgr.OnActivate ();

                if (res != null) {
                    switch (res.GetItemId ()) {
                    case (int)MenuOptions.StartNewGame:
                        _mapManager.ShowScreen (ScreenId.NoIntroGameScreen);
                        break;
                    case (int)MenuOptions.LoadSavedGame:
                        break;
                    case (int)MenuOptions.Credits:
                        _mapManager.ShowScreen (ScreenId.CreditsScreen);
                        break;
                    case (int)MenuOptions.Quit:
                        break;
                    }
                }
            }
        }
    }
}
