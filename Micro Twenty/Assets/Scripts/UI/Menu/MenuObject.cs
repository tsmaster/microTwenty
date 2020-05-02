using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class MenuObject
    {
        // number of child items displayed in our window
        private int _cw_width;
        private int _cw_height;

        private int _cell_x_padding;
        private int _cell_y_padding;

        // number of patches across our window
        private int _patch_width;
        private int _patch_height;

        // number of patches used by an individual cell
        private int _cell_patch_width;
        private int _cell_patch_height;

        Dictionary<string, int> childLookup;
        List<MenuObject> children;
        private Texture2D _menuSprite;
        private Texture2D _fontSprite;

        private const int FONT_WIDTH = 6;
        public const int FONT_HEIGHT = 8;

        private string _name;

        private int _topRow = 0;
        // cursor position in absolute (not scrolled) position
        private int _cursorRow = 0;
        private int _cursorCol = 0;
        private int _cursorIndex = 0;

        private int _numRows = 0;
        private int _itemId = -1;

        public bool IsEnabled { get; set; }

        public MenuObject (string name, Texture2D menuSprite, Texture2D fontSprite)
        {
            _name = name;
            _cw_width = 1;
            _cw_height = 1;

            _cell_x_padding = 1;
            _cell_y_padding = 0;

            childLookup = new Dictionary<string, int> ();
            children = new List<MenuObject> ();

            _menuSprite = menuSprite;
            _fontSprite = fontSprite;

            IsEnabled = true;
        }


        private void CalcCursorIndex ()
        {
            _cursorIndex = _cursorRow * _cw_width + _cursorCol;
            Debug.Assert (_cursorIndex <= children.Count);
        }

        internal MenuObject AddItem (string name)
        {
            if (childLookup.ContainsKey (name)) {
                return children [childLookup [name]];
            }

            var newChild = new MenuObject (name, _menuSprite, _fontSprite);
            children.Add (newChild);
            childLookup [name] = children.Count - 1;
            return children [children.Count - 1];
        }

        public MenuObject ClearItems ()
        {
            childLookup.Clear ();
            children.Clear ();
            return this;
        }

        public bool HasChildren ()
        {
            return children.Count > 0;
        }

        public void Build ()
        {
            int maxWidth = 0;

            foreach (var child in children) {
                if (child.HasChildren ()) {
                    child.Build ();
                }
                var cname = child.GetName ();
                maxWidth = Math.Max (maxWidth, cname.Length);
            }

            _cell_patch_width = maxWidth + 2 * _cell_x_padding;
            _cell_patch_height = 1 + 2 * _cell_y_padding;

            _patch_width = _cw_width * _cell_patch_width + 2;
            _patch_height = _cw_height * _cell_patch_height + 2;

            _numRows = children.Count / _cw_width;
            if (children.Count % _cw_width != 0) {
                _numRows += 1;
            }
        }

        private string GetName ()
        {
            return _name;
        }

        #region Drawing

        internal void Draw (Texture2D destTexture, int x, int y)
        {
            int source_x, source_y;

            // upper left
            GetSpriteLoc (_menuSprite, 0, out source_x, out source_y);
            TextureDrawing.DrawPartialSprite (destTexture, _menuSprite, 
                x, y, source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

            // upper right
            GetSpriteLoc (_menuSprite, 2, out source_x, out source_y);
            TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                x + (_patch_width - 1) * FONT_WIDTH, y,
                source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

            // Lower Left
            GetSpriteLoc (_menuSprite, 6, out source_x, out source_y);
            TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                x, y - (_patch_height - 1) * FONT_HEIGHT, 
                source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

            // Lower Right
            GetSpriteLoc (_menuSprite, 8, out source_x, out source_y);
            TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                x + (_patch_width - 1) * FONT_WIDTH, y - (_patch_height - 1) * FONT_HEIGHT,
                source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

            for (int px = 1; px < _patch_width - 1; ++px) {
                // top
                GetSpriteLoc (_menuSprite, 1, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + px * FONT_WIDTH, y, source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

                // bottom
                GetSpriteLoc (_menuSprite, 7, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + px * FONT_WIDTH, y - (_patch_height - 1) * FONT_HEIGHT, 
                    source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

                // middle
                GetSpriteLoc (_menuSprite, 4, out source_x, out source_y);
                for (int py = 1; py < _patch_height - 1; ++py) {
                    TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                        x + px * FONT_WIDTH, y - py * FONT_HEIGHT,
                        source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
                }
            }

            for (int py = 1; py < _patch_height - 1; ++py) {
                // left
                GetSpriteLoc (_menuSprite, 3, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x, y - py * FONT_HEIGHT, source_x, source_y, FONT_WIDTH, FONT_HEIGHT);

                // right
                GetSpriteLoc (_menuSprite, 5, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + (_patch_width - 1) * FONT_WIDTH, y - py * FONT_HEIGHT,
                    source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
            }

            int firstItemToDraw = _topRow * _cw_width;
            int lastItemToDraw = Mathf.Min (firstItemToDraw + (_cw_width * _cw_height) - 1, children.Count - 1);

            for (int itemIndex = firstItemToDraw; itemIndex <= lastItemToDraw; ++itemIndex) {
                int col = (itemIndex - firstItemToDraw) % _cw_width;
                int row = (itemIndex - firstItemToDraw) / _cw_width;

                DrawChildItem (destTexture, x, y, itemIndex, col, row);

                // cursor
                if (itemIndex == _cursorIndex) {
                    GetSpriteLoc (_menuSprite, 12, out source_x, out source_y);

                    TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                        x + (col * _cell_patch_width) * FONT_WIDTH + 4,
                        y - ((row * _cell_patch_height) + 1) * FONT_HEIGHT,
                        source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
                }
            }

            // up indicator
            if (firstItemToDraw > 0) {
                GetSpriteLoc (_menuSprite, 9, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + (_patch_width - 2) * FONT_WIDTH,
                    y,
                    source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
            }

            // down indicator
            if (lastItemToDraw < children.Count - 1) {
                GetSpriteLoc (_menuSprite, 10, out source_x, out source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + (_patch_width - 2) * FONT_WIDTH,
                    y - (_patch_height - 1) * FONT_HEIGHT,
                    source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
            }
        }

        private void DrawChildItem (Texture2D destTexture, int x, int y, int itemIndex, int col, int row) {
            MenuObject childObject = children [itemIndex];

            TextureDrawing.DrawStringAt (destTexture, _fontSprite, childObject._name,
                x + ((col * _cell_patch_width) + _cell_x_padding + 1) * FONT_WIDTH,
                y - ((row * _cell_patch_height) + 1) * FONT_HEIGHT,
                childObject.IsEnabled ? UnityEngine.Color.white : UnityEngine.Color.gray);

            if (childObject.HasChildren ()) {
                GetSpriteLoc (_menuSprite, 11, out int source_x, out int source_y);
                TextureDrawing.DrawPartialSprite (destTexture, _menuSprite,
                    x + ((col * _cell_patch_width) + childObject.GetName().Length + 2) * FONT_WIDTH + 4,
                    y - ((row * _cell_patch_height) + 1) * FONT_HEIGHT,
                    source_x, source_y, FONT_WIDTH, FONT_HEIGHT);
            }
        }

        private void GetSpriteLoc (Texture2D texture, int v, out int source_x, out int source_y)
        {
            var fontBitmapColumns = texture.width / FONT_WIDTH;
            var fontBitmapRows = texture.height / FONT_HEIGHT;

            var col = v % fontBitmapColumns;
            var row_from_top = (v - col) / fontBitmapColumns;

            var row_from_bottom = fontBitmapRows - row_from_top - 1;

            source_x = col * FONT_WIDTH;
            source_y = row_from_bottom * FONT_HEIGHT;
        }

        #endregion // Drawing

        public MenuObject this [string name] {
            get {
                return AddItem (name);
            }
            set {
                throw new NotImplementedException();
            }
        }

        #region Layout Parameters

        public MenuObject SetEnabled (bool enabled)
        {
            this.IsEnabled = enabled;
            return this;
        }

        internal void SetWindow (int width, int height)
        {
            _cw_width = width;
            _cw_height = height;
        }

        public MenuObject SetItemId (int itemId)
        {
            _itemId = itemId;
            return this;
        }

        public int GetItemId ()
        {
            return _itemId;
        }

        #endregion // Layout Parameters

        #region Navigation

        internal void OnUp ()
        {
            _cursorRow -= 1;
            _cursorRow = Math.Max (0, _cursorRow);

            _topRow = Math.Min (_cursorRow, _topRow);
            _topRow = Math.Max (_topRow, 0);

            CalcCursorIndex ();
        }

        internal void OnLeft ()
        {
            _cursorCol -= 1;
            _cursorCol = Math.Max (0, _cursorCol);

            CalcCursorIndex ();
        }

        internal void OnRight ()
        {
            _cursorCol += 1;
            _cursorCol = Math.Min (_cw_width - 1, _cursorCol);

            CalcCursorIndex ();
        }

        internal void OnDown ()
        {
            _cursorRow += 1;
            _cursorRow = Math.Min (_numRows - 1, _cursorRow);

            _topRow = Math.Max (_cursorRow - _cw_height + 1, _topRow);
            _topRow = Math.Min (_topRow, _numRows - 1);

            CalcCursorIndex ();
        }

        public MenuObject OnActivate ()
        {
            var child = children [_cursorIndex];

            if (child.IsEnabled) {
                return child;
            } else {
                return null;
            }
        }
        #endregion // Navigation
    }
}
