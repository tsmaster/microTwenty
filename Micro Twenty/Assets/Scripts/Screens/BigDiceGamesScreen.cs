using System;
using UnityEngine;

namespace MicroTwenty
{
    public class MicroTwentyTitleScreen : IIntroScreen
    {
        private MapManager _mapManager;
        private Texture2D _titleTexture;
        private float _elapsedSeconds;
        private const float SHOW_DURATION = 3.14f;

        public MicroTwentyTitleScreen (MapManager mapManager, Texture2D titleTexture)
        {
            _mapManager = mapManager;
            _titleTexture = titleTexture;

            _elapsedSeconds = 0.0f;
        }

        public void UpdateScreen (float deltaSeconds)
        {
            _elapsedSeconds += deltaSeconds;

            if ((_elapsedSeconds >= SHOW_DURATION) || (Input.anyKeyDown)) {
                _mapManager.ShowScreen (ScreenId.MenuScreen);
            }
        }

        public void Draw ()
        {
            var targetTexture = _mapManager.GetTargetTexture ();
            TextureDrawing.DrawPartialSprite (targetTexture, _titleTexture, 0, 0, 0, 0, _titleTexture.width, _titleTexture.height);
            targetTexture.Apply ();
        }
    }
}
