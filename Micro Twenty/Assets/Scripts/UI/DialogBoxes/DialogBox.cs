using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class DialogBox
    {
        private MapManager _mapManager;
        private string _title;
        private List<string> _message;
        private Texture2D _icon;
        private float _elapsedSeconds;
        private const float MIN_DISPLAY_TIME = 0.3f;

        public DialogBox (MapManager mapManager, string title, List<string> message, Texture2D icon)
        {
            _mapManager = mapManager;
            _title = title;
            _message = message;
            _icon = icon;
            _elapsedSeconds = 0.0f;
        }

        public void Update (float deltaSeconds, out bool isDone)
        {
            _elapsedSeconds += deltaSeconds;

            if ((_elapsedSeconds > MIN_DISPLAY_TIME) && (Input.anyKeyDown)) {
                Debug.Log ("dialog is done");
                isDone = true;
            } else {
                isDone = false;
            }
        }

        public void Draw ()
        {
            // if you make it translucent, that means the icon has to be translucent
            var fillColor = Color.black;
            var targetTexture = _mapManager.GetTargetTexture ();
            var fontTexture = _mapManager.GetFontBitmap ();

            TextureDrawing.DrawRect (targetTexture, 10, 10,
                targetTexture.width - 20, targetTexture.height - 20,
                fillColor, Color.white,
                true, true);

            // draw title
            var titleWidthPixels = _title.Length * 6;
            var titleX = (targetTexture.width - titleWidthPixels) / 2;

            var currentY = targetTexture.height - 24;

            var lineSpacing = 12;

            TextureDrawing.DrawStringAt (targetTexture, fontTexture, _title,
                titleX, currentY,
                Color.white);

            currentY -= lineSpacing;

            // draw icon

            if (_icon != null) {
                var iconX = (targetTexture.width - _icon.width) / 2;
                var iconY = currentY - _icon.height;

                TextureDrawing.DrawPartialSprite (targetTexture, _icon, iconX, iconY, 0, 0, _icon.width, _icon.height);

                currentY -= (_icon.height + lineSpacing);
            }

            // draw msg


            for (int i = 0; i < _message.Count; ++i) {
                var msgWidthPixels = _message[i].Length * 6;
                var msgX = (targetTexture.width - msgWidthPixels) / 2;

                TextureDrawing.DrawStringAt (targetTexture, fontTexture, _message[i],
                    msgX, currentY,
                    Color.white);

                currentY -= lineSpacing;
            }

            // draw OK

            string okMsg = "OK";
            var okWidthPixels = okMsg.Length * 6;
            var okX = (targetTexture.width - okWidthPixels) / 2;
            var okY = 20;
            TextureDrawing.DrawStringAt (targetTexture, fontTexture, "OK",
                okX, okY,
                Color.white);



            targetTexture.Apply ();
        }
    }
}
