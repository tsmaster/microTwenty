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
                Color [] bits = new Color [width * height];

                if (fillColor.a <= 254.0f / 255.0f) {
                    var a = fillColor.a;
                    var oma = 1.0f - a; // one minus a

                    bits = targetTexture.GetPixels (left, top, width, height);

                    for (int i = 0; i < bits.Length; ++i) {
                        bits [i] = oma * bits [i] + a * fillColor;
                    }
                } else {
                    for (int i = 0; i < bits.Length; ++i) {
                        bits [i] = fillColor;
                    }
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


        static public void DrawPartialSprite (Texture2D targetTexture, Texture2D sourceTexture, 
            int target_x, int target_y, int source_x, int source_y, int width, int height)
        {
            if (target_x < 0) {
                width += target_x;
                source_x -= target_x;
                target_x = 0;
            }
            if (target_x + width >= targetTexture.width) {
                width = targetTexture.width - target_x - 1;
            }

            if (width <= 0) {
                return;
            }

            if (target_y < 0) {
                height += target_y;
                source_y -= target_y;
                target_y = 0;
            }
            if (target_y + height >= targetTexture.height) {
                height = targetTexture.height - target_y - 1;
            }

            if (height <= 0) {
                return;
            }


            var targpixels = targetTexture.GetPixels (target_x, target_y, width, height);
            var sourcePixels = sourceTexture.GetPixels (source_x, source_y, width, height);

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    Color c = sourcePixels [x + y * width];
                    if (c.a < 1.0f / 256) {
                        continue;
                    }
                    targpixels [x + y * width] = c;
                }
            }
            targetTexture.SetPixels (target_x, target_y, width, height, targpixels);
        }

        public static void DrawTintedPartialSprite (Texture2D targetTexture, Texture2D sourceTexture, 
            int tx, int ty, int sx, int sy, int width, int height, Color tint)
        {
            if (tx < 0) {
                width += tx;
                sx -= tx;
                tx = 0;
            }
            if (tx + width >= targetTexture.width) {
                width = targetTexture.width - tx;
            }

            if (width <= 0) {
                return;
            }

            if (ty < 0) {
                height += ty;
                sy -= ty;
                ty = 0;
            }
            if (ty + height >= targetTexture.height) {
                height = targetTexture.height - ty;
            }

            if (height <= 0) {
                return;
            }

            var targpixels = targetTexture.GetPixels (tx, ty, width, height);
            var sourcePixels = sourceTexture.GetPixels (sx, sy, width, height);

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    Color c = sourcePixels [x + y * width];
                    if (c.a < 1.0f / 256) {
                        continue;
                    }
                    targpixels [x + y * width] = c * tint;
                }
            }
            targetTexture.SetPixels (tx, ty, width, height, targpixels);
        }


        static public void DrawStringAt (Texture2D targetTexture, Texture2D fontTexture, string message, int x, int y, Color color)
        {
            for (int i = 0; i < message.Length; ++i) {
                var pos_x = i * 6 + x;
                var pos_y = y;

                char c = message [i];
                int coff = (int)c - 32;

                var c_col = coff % 8;
                var c_row = 11 - coff / 8;

                DrawTintedPartialSprite (targetTexture, fontTexture, pos_x, pos_y, c_col * 8, c_row * 8, 8, 8, color);
            }
        }
    }
}
