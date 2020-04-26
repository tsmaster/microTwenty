using System;
namespace MicroTwenty
{
    public static class BdgMath
    {
        public static float Map (float q, float in_min, float in_max, float out_min, float out_max) {
            var frac = (q - in_min) / (in_max - in_min);
            return frac * (out_max - out_min) + out_min;
        }

        /// <summary>
        /// Clamps the value to be between min and max, inclusive.
        /// </summary>
        /// <returns>The clamped value.</returns>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Maximum.</param>
        public static int ClampInt (int value, int min, int max)
        {
            return Math.Min (Math.Max (value, min), max);
        }
    }
}
