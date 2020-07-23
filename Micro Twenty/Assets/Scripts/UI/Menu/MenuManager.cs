using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class MenuManager
    {
        private Texture2D _menuSprite;
        private Texture2D _fontSprite;
        private bool _isOpen;
        private List<MenuObject> _menuStack;

        private bool _dismissOnAction = true;
        public bool DismissOnAction {
            get { return _dismissOnAction; }
            set { _dismissOnAction = value; }
        }

        public bool CanBackOutOfMenu { get; set; }

        public MenuManager (Texture2D menuSprite, Texture2D fontSprite)
        {
            _menuSprite = menuSprite;
            _fontSprite = fontSprite;

            _menuStack = new List<MenuObject> ();
            CanBackOutOfMenu = true;
        }

        public void OpenMenu (MenuObject menuObject)
        {
            _menuStack.Clear();
            _menuStack.Add (menuObject);
            _isOpen = true;
        }

        public void CloseMenu ()
        {
            _menuStack.Clear ();
            _isOpen = false;
        }

        public bool IsOpen ()
        {
            return _isOpen;
        }

        public void Draw (Texture2D destTexture, int x, int y)
        {
            if (!_isOpen) {
                return;
            }

            foreach (var mo in _menuStack) {
                mo.Draw (destTexture, x, y);
                x += 10;
                y -= 10;
            }
            destTexture.Apply ();
        }

        internal void OnUp ()
        {
            _menuStack [_menuStack.Count - 1].OnUp ();
        }

        internal void OnDown ()
        {
            _menuStack [_menuStack.Count - 1].OnDown ();
        }

        internal void OnLeft ()
        {
            _menuStack [_menuStack.Count - 1].OnLeft ();
        }

        internal void OnRight ()
        {
            _menuStack [_menuStack.Count - 1].OnRight ();
        }

        public MenuObject OnActivate ()
        {
            var curTop = _menuStack [_menuStack.Count - 1];
            var obj = curTop.OnActivate ();

            if (obj == null) {
                return null;
            }

            if (obj.HasChildren()) {
                _menuStack.Add (obj);
                return null;
            } else {
                if (DismissOnAction) {
                    CloseMenu ();
                }
                return obj;
            }

        }

        internal void OnBack ()
        {
            _menuStack.RemoveAt (_menuStack.Count - 1);

            if (CanBackOutOfMenu && (_menuStack.Count == 0)) {
                CloseMenu ();
            }
        }
    }
}
