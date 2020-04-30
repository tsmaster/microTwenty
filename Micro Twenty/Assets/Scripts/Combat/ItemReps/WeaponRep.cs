using System;
namespace MicroTwenty
{
    public class WeaponRep
    {
        public readonly string Name;
        public readonly int NumDice;
        public readonly int DiceSides;
        public readonly int Modifier;
        public readonly int MinRange;
        public readonly int MaxRange;
        public readonly bool IsRanged;

        public WeaponRep (string name, int numDice, int diceSides, int modifier)
        {
            Name = name;
            NumDice = numDice;
            DiceSides = diceSides;
            Modifier = modifier;
            IsRanged = false;
        }

        public WeaponRep (string name, int numDice, int diceSides, int modifier, int minRange, int maxRange)
        {
            Name = name;
            NumDice = numDice;
            DiceSides = diceSides;
            Modifier = modifier;
            IsRanged = true;
            MinRange = minRange;
            MaxRange = maxRange;
        }

        public float GetExpectedDamage ()
        {
            return ((DiceSides / 2.0f) + 0.5f) * NumDice + Modifier;
        }

        public int RollDamage ()
        {
            int total = Modifier;
            for (int die = 0; die < NumDice; ++die) {
                total += UnityEngine.Random.Range (1, DiceSides + 1);
            }
            return total;
        }

        public bool InRange (int testRange)
        {
            return ((testRange >= MinRange) &&
                (testRange <= MaxRange));
        }

        public static WeaponRep MakeSword ()
        {
            return new WeaponRep ("Sword", 1, 8, 0);
        }

        public static WeaponRep MakeBow ()
        {
            return new WeaponRep ("Bow", 1, 6, 0, 2, 5);
        }

        public static WeaponRep MakeStaff ()
        {
            return new WeaponRep ("Staff", 1, 4, 0);
        }

        public static WeaponRep MakeBiteWeapon ()
        {
            return new WeaponRep ("Bite", 1, 3, 0);
        }

        public static WeaponRep MakePsiWeapon ()
        {
            return new WeaponRep ("Psi", 1, 4, 0);
        }
    }
}
