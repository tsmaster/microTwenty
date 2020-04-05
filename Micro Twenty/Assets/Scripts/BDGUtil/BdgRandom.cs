using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class BdgRandom
    {
        public static void ShuffleList<T> (List<T> list)
        {
            for (int i = 0; i < list.Count; ++i) {
                int j = UnityEngine.Random.Range (0, list.Count);
                var temp = list [i];
                list [i] = list [j];
                list [j] = temp;
            }
        }
    }
}
