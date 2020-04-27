﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class CreditsScreen : IIntroScreen
    {
        private MapManager _mapManager;
        private float _elapsedSeconds;
        private Texture2D _fontTexture;
        private const float SLIDE_RATE = 16.0f;
        private float _slideY;
        private List<string> _credits;
        private float _duration;

        private const int LINE_SPACING = 12;

        public CreditsScreen (MapManager mapManager)
        {
            _mapManager = mapManager;
            _elapsedSeconds = 0.0f;
            _fontTexture = mapManager.GetFontBitmap ();

            _slideY = 0.0f;

            _credits = new List<string>{
                "MicroTwenty",
                "is a Big Dice Games Joint",
                " ",
                "Programming: Dave LeCompte",
                "Art: Dave LeCompte",
                "Music: <TBD>",
                "Craft Services: Alan Turing",
                "",
                "Thanks To:",
                "Red Blob Games",
                "Kenney.nl",
                "OneLoneCoder",
                "S. John Ross",
                "Dyson Logos",
                "David Churchill"
            };

            _duration = (LINE_SPACING * _credits.Count + mapManager.GetTargetTexture().height) / SLIDE_RATE + 3.14f;
        }

        public void UpdateScreen (float deltaSeconds)
        {
            _elapsedSeconds += deltaSeconds;

            if ((_elapsedSeconds >= _duration) || (Input.anyKeyDown)) {
                _mapManager.ShowScreen (ScreenId.MenuScreen);
            }

            _slideY += deltaSeconds * SLIDE_RATE;
        }

        public void Draw ()
        {
            var targetTexture = _mapManager.GetTargetTexture ();

            TextureDrawing.DrawRect (targetTexture, 0, 0, targetTexture.width, targetTexture.height, Color.black, Color.black, true, false);

            for (int i = 0; i < _credits.Count; ++i) {
                int creditY = (int)(_slideY - i * LINE_SPACING);

                string msg = _credits [i];

                int creditX = (targetTexture.width - 6 * msg.Length) / 2;
                TextureDrawing.DrawStringAt (targetTexture, _fontTexture, msg, creditX, creditY, Color.yellow);
            }

            targetTexture.Apply ();
        }
    }
}
