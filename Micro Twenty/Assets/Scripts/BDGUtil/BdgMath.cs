using System;
namespace MicroTwenty
{
    public static class BdgMath
    {
        public static float Map (float q, float in_min, float in_max, float out_min, float out_max) {
            var frac = (q - in_min) / (in_max - in_min);
            return frac * (out_max - out_min) + out_min;
        }
    }
}
