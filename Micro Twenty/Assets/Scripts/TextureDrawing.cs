using System;
using UnityEngine;

namespace MicroTwenty
{
    public static class TextureDrawing
    {
        static public void DrawRect (Texture2D targetTexture,
            int left, int top,
            int width, int height,
            Color fillColor, Color strokeColor, bool fill, bool stroke)
        {
            if (left < 0) {
                width += left;
                left = 0;
            }
            if (left + width >= targetTexture.width) {
                width = targetTexture.width - left - 1;
            }

            if (width <= 0) {
                return;
            }

            if (top < 0) {
                height += top;
                top = 0;
            }
            if (top + height >= targetTexture.height) {
                height = targetTexture.height - top - 1;
            }

            if (height <= 0) {
                return;
            }


            var texWidth = targetTexture.width;
            var texHeight = targetTexture.height;

            if (fill) {
                //var bits = targetTexture.GetPixels (left, top, width, height);
                Color [] bits = new Color [width * height];
                for (int i = 0; i < bits.Length; ++i) {
                    bits [i] = fillColor;
                    // todo support 1 bit alpha
                    // todo support 8 bit alpha
                }
                targetTexture.SetPixels (left, top, width, height, bits);
            }
            if (stroke) {
                for (int x = 0; x <= width; ++x) {
                    targetTexture.SetPixel (left + x, top, strokeColor);
                    targetTexture.SetPixel (left + x, top + height, strokeColor);
                }
                for (int y = 0; y <= height; ++y) {
                    targetTexture.SetPixel (left, top + y, strokeColor);
                    targetTexture.SetPixel (left + width, top + y, strokeColor);
                }
            }
        }
    }
}
