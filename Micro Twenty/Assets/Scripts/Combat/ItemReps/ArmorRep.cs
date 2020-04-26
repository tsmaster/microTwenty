using System;
namespace MicroTwenty
{
    public class ArmorRep
    {
        public readonly string Name;
        public readonly int DamageReduction;
        /// <summary>
        /// To hit reduction as a fraction from 0 to 1
        /// </summary>
        public readonly float ToHitReduction;

        public ArmorRep (string name, int dr, float thr)
        {
            Name = name;
            DamageReduction = dr;
            ToHitReduction = thr;
        }

        public float ExpectedPassThrough (float baseToHit, float startDamage)
        {
            float adjustedToHit = baseToHit - ToHitReduction;
            return (startDamage - DamageReduction) * adjustedToHit;
        }

        static public ArmorRep MakeClothArmor ()
        {
            return new ArmorClothRep ();
        }

        static public ArmorRep MakeLeatherArmor ()
        {
            return new ArmorRep ("Leather Armor", 1, 0.1f);
        }

        static public ArmorRep MakeChainArmor ()
        {
            return new ArmorRep ("Chainmail", 2, 0.2f);
        }

        static public ArmorRep MakePlateArmor ()
        {
            return new ArmorRep ("Platemail", 3, 0.3f);
        }
    }
}
