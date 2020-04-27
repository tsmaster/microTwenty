using System;
using UnityEngine;

namespace MicroTwenty
{
    public class BigDiceGamesScreen : IIntroScreen
    {
        private MapManager _mapManager;
        private Texture2D _bdgTexture;
        private float _elapsedSeconds;
        private const float SHOW_DURATION = 3.14f;

        public BigDiceGamesScreen (MapManager mapManager, Texture2D bdgTexture)
        {
            _mapManager = mapManager;
            _bdgTexture = bdgTexture;

            _elapsedSeconds = 0.0f;
        }

        public void UpdateScreen (float deltaSeconds)
        {
            _elapsedSeconds += deltaSeconds;

            if ((_elapsedSeconds >= SHOW_DURATION) || (Input.anyKeyDown)) {
                _mapManager.ShowScreen (ScreenId.TitleScreen);
            }
        }

        public void Draw ()
        {
            var targetTexture = _mapManager.GetTargetTexture ();
            TextureDrawing.DrawPartialSprite (targetTexture, _bdgTexture, 0, 0, 0, 0, _bdgTexture.width, _bdgTexture.height);
            targetTexture.Apply ();
        }
    }
}
