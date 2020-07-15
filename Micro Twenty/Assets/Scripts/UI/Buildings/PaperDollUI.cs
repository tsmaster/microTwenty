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
        private Texture2D _axeTexture;

        public PaperDollUi (GameMgr gameMgr)
        {
            _name = "Paper Doll";
            _gameMgr = gameMgr;
            _targetTexture = _gameMgr.GetMapManager ().GetTargetTexture ();
            _fontTexture = _gameMgr.GetMapManager ().GetFontBitmap ();

            _margin = 5;
            _textureWidth = _targetTexture.width;
            _textureHeight = _targetTexture.height;

            _axeTexture = Resources.Load<Texture2D> ("Sprites/Dialogs/paperdoll");
        }

        public override void Draw ()
        {
            TextureDrawing.DrawRect (_targetTexture,
                _margin, _margin,
                _textureWidth - 2 * _margin, _textureHeight - 2 * _margin,
                Color.white, Color.green, true, true);

            TextureDrawing.DrawPartialSprite (_targetTexture, _axeTexture,
                2 * _margin,
                _targetTexture.height - 2 * _margin - _axeTexture.height,
                0, 0,
                _axeTexture.width, _axeTexture.height);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                _name,
                _textureWidth / 2, _textureHeight - 20,
                Color.black);

            TextureDrawing.DrawCenteredStringAt (_targetTexture, _fontTexture,
                "X - eXit",
                _textureWidth / 2, 20,
                Color.black);

            _targetTexture.Apply ();
        }

        public override void Update (float deltaSeconds)
        {
            if (Input.GetKeyDown (KeyCode.X)) {
                _gameMgr.ExitBuilding ();
            }
        }
    }
}
