using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class HexCoord : IEqualityComparer<HexCoord>
    {
        public readonly int x, y, z;

        public HexCoord (int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public HexCoord Add (HexCoord other)
        {
            return new HexCoord (
                this.x + other.x,
                this.y + other.y,
                this.z + other.z);
        }

        public HexCoord Sub (HexCoord other)
        {
            return new HexCoord (
                this.x - other.x,
                this.y - other.y,
                this.z - other.z);
        }

        public HexCoord Mul (int count)
        {
            return new HexCoord (
                this.x * count,
                this.y * count,
                this.z * count);
        }

        public override string ToString ()
        {
            return string.Format ("<{0} {1} {2}>", x, y, z);
        }

        public HexCoord Rotate (int facingsCount)
        {
            if (facingsCount == 0) {
                return this;
            }
            if (facingsCount < 0) {
                return Rotate (facingsCount + 6);
            }
            if (facingsCount > 1) {
                return Rotate (1).Rotate (facingsCount - 1);
            }
            var nx = -this.y;
            var ny = -this.z;
            var nz = -this.x;
            return new HexCoord (nx, ny, nz);
        }

        public static List<HexCoord> GetAtRange (int range)
        {
            List<HexCoord> outList = new List<HexCoord> ();

            if (range == 0) {
                outList.Add (new HexCoord(0,0,0));
                return outList;
            }

            HexCoord startVectorUnit = new HexCoord (1, -1, 0);
            for (int facing = 0; facing < 6; ++facing) {
                var startVector = startVectorUnit.Mul (range);
                outList.Add (startVector);
                var step = startVectorUnit.Rotate (2);
                for (int i = 1; i < range; ++i) {
                    outList.Add (startVector.Add (step.Mul (i)));
                }
                startVectorUnit = startVectorUnit.Rotate (1);
            }
            return outList;
        }

        /// <summary>
        /// Gets hexes at ranges 0 to the specified range.
        /// </summary>
        /// <returns>The within range.</returns>
        /// <param name="range">Range.</param>
        public static List<HexCoord> GetWithinRange (int range)
        {
            return GetBetweenRanges (0, range);
        }

        /// <summary>
        /// Gets hexes that are at least innerRange and at most outerRange away
        /// </summary>
        /// <returns>The within range.</returns>
        /// <param name="innerRange">Range.</param>
        /// <param name="outerRange">Range.</param>
        public static List<HexCoord> GetBetweenRanges (int innerRange, int outerRange)
        {
            List<HexCoord> outList = new List<HexCoord> ();

            for (int i = innerRange; i <= outerRange; ++i) {
                outList.AddRange (GetAtRange (i));
            }
            return outList;
        }

        internal static List<HexCoord> GetAtRangeFromLoc (int v, HexCoord startCoord)
        {
            List<HexCoord> outList = new List<HexCoord> ();
            foreach (var hc in GetAtRange (v)) {
                outList.Add (hc.Add (startCoord));
            }
            return outList;
        }

        internal static List<HexCoord> GetWithinRangeFromLoc (int v, HexCoord startCoord)
        {
            List<HexCoord> outList = new List<HexCoord> ();
            foreach (var hc in GetWithinRange (v)) {
                outList.Add (hc.Add (startCoord));
            }
            return outList;
        }


        public bool Equals (HexCoord a, HexCoord b)
        {
            UnityEngine.Debug.LogFormat ("(2) testing {0} == {1}", a.ToString (), b.ToString ());

            return a == b;
        }

        public int GetHashCode (HexCoord obj)
        {
            return obj.GetHashCode ();
        }

        public override int GetHashCode ()
        {
            return 17 * x + 11 * y + z;
        }

        public override bool Equals (object other)
        {
            var otherHc = other as HexCoord;
            if (otherHc == null) {
                return false;
            }
            return ((this.x == otherHc.x) &&
                (this.y == otherHc.y) &&
                (this.z == otherHc.z));
        }

        internal int DistanceTo (HexCoord destCoord)
        {
            return (Math.Abs (this.x - destCoord.x) +
                Math.Abs (this.y - destCoord.y) +
                Math.Abs (this.z - destCoord.z)) / 2;
        }

        public static List<HexCoord> DrawLine (HexCoord start, HexCoord end)
        {
            var numPoints = start.DistanceTo (end);
            var outList = new List<HexCoord> ();

            for (int i = 0; i <= numPoints; ++i) {
                float frac = i / (float)numPoints;
                FloatHexCoord fhc = FloatHexCoord.CubeLerp (start, end, frac);
                HexCoord hc = FloatHexCoord.CubeRound (fhc);
                outList.Add (hc);
            }
            return outList;
        }


    }

    public class FloatHexCoord
    {
        public float x;
        public float y;
        public float z;

        public FloatHexCoord (float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static FloatHexCoord CubeLerp (HexCoord start, HexCoord end, float frac)
        {
            return new FloatHexCoord (
                BdgMath.Map (frac, 0, 1, start.x, end.x),
                BdgMath.Map (frac, 0, 1, start.y, end.y),
                BdgMath.Map (frac, 0, 1, start.z, end.z));
        }

        public static HexCoord CubeRound (FloatHexCoord fhc)
        {
            var rx = Mathf.RoundToInt (fhc.x);
            var ry = Mathf.RoundToInt (fhc.y);
            var rz = Mathf.RoundToInt (fhc.z);

            var xDiff = Mathf.Abs (rx - fhc.x);
            var yDiff = Mathf.Abs (ry - fhc.y);
            var zDiff = Mathf.Abs (rz - fhc.z);

            if ((xDiff > yDiff) && (xDiff > zDiff)) {
                rx = -ry - rz;
            } else if (yDiff > zDiff) {
                ry = -rx - rz;
            } else {
                rz = -rx - ry;
            }

            return new HexCoord (rx, ry, rz);
        }
    }
}
