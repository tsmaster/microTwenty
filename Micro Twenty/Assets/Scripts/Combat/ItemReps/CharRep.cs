using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class CharRep
    {
        public string Name { get; private set; }
        public HexCoord Position { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public int CurrentMana { get; private set; }
        public int MaxMana { get; private set; }

        public int Initiative { get; private set; }
        public int MoveSpeed { get; private set; }
        public int LastTurnMoved { get; private set; }

        public int TeamIndex { get; private set; }

        public WeaponRep CurrentWeapon { get; private set; }
        public ArmorRep CurrentArmor { get; private set; }

        public List<SpellRep> SpellList { get; private set; }

        public CharRep (string name, HexCoord pos, 
            int curHealth, int maxHealth, 
            int curMana, int maxMana,
            int initiative, int moveSpeed,
            int lastTurnMoved, int teamIndex,
            WeaponRep weapon, ArmorRep armor,
            List<SpellRep> spellList)
        {
            Name = name;
            Position = pos;
            CurrentHealth = curHealth;
            MaxHealth = maxHealth;
            CurrentMana = curMana;
            MaxMana = maxMana;

            Initiative = initiative;
            MoveSpeed = moveSpeed;
            LastTurnMoved = lastTurnMoved;
            TeamIndex = teamIndex;

            CurrentWeapon = weapon;
            CurrentArmor = armor;

            SpellList = spellList;
        }

        public float GetExpectedDamage ()
        {
            return CurrentWeapon.GetExpectedDamage ();
        }

        public CharRep MakeCopy ()
        {
            return new CharRep (Name, Position, 
                CurrentHealth, MaxHealth, 
                CurrentMana, MaxMana, 
                Initiative, MoveSpeed, 
                LastTurnMoved, TeamIndex,
                CurrentWeapon, CurrentArmor, SpellList);
        }

        public CharRep SetLastTurn (int currentTurn)
        {
            var c = MakeCopy ();
            c.LastTurnMoved = currentTurn;
            return c;
        }

        public CharRep MoveTo (HexCoord newPos)
        {
            var c = MakeCopy ();
            c.Position = newPos;
            return c;
        }

        public CharRep SetHealth (int newHealth)
        {
            var c = MakeCopy ();
            c.CurrentHealth = newHealth;
            return c;
        }

        public CharRep IncrementHealth (int healthDelta)
        {
            int newHealth = BdgMath.ClampInt (CurrentHealth + healthDelta, 0, MaxHealth);

            if (newHealth == CurrentHealth) {
                return this;
            }

            return SetHealth (newHealth);
        }

        public CharRep SetMana (int newMana)
        {
            var c = MakeCopy ();
            c.CurrentMana = newMana;
            return c;
        }

        public CharRep IncrementMana (int manaDelta)
        {
            int newMana = BdgMath.ClampInt (CurrentMana + manaDelta, 0, MaxMana);

            if (newMana == CurrentMana) {
                return this;
            }

            return SetMana (newMana);
        }

        public CharRep SetWeapon (WeaponRep newWeapon)
        {
            var c = MakeCopy ();
            c.CurrentWeapon = newWeapon;
            return c;
        }

        public CharRep SetArmor (ArmorRep newArmor)
        {
            Debug.Assert (newArmor != null);
            var c = MakeCopy ();
            c.CurrentArmor = newArmor;
            Debug.Assert (c.CurrentArmor != null);
            return c;
        }

        public CharRep AddSpell (SpellRep newSpell)
        {
            var c = MakeCopy ();
            c.SpellList = new List<SpellRep> (c.SpellList) {
                newSpell
            };
            return c;
        }

        public override bool Equals (object otherObject)
        {
            var otherChar = otherObject as CharRep;
            if (otherChar == null) {
                return false;
            }

            return ((Name == otherChar.Name) &&
                (Position == otherChar.Position) &&
                (CurrentHealth == otherChar.CurrentHealth) &&
                (MaxHealth == otherChar.MaxHealth) &&
                (CurrentMana == otherChar.CurrentMana) &&
                (MaxMana == otherChar.MaxMana) &&
                (Initiative == otherChar.Initiative) &&
                (MoveSpeed == otherChar.MoveSpeed) &&
                (TeamIndex == otherChar.TeamIndex) &&
                (LastTurnMoved == otherChar.LastTurnMoved) &&
                (CurrentWeapon.Equals (otherChar.CurrentWeapon)) &&
                (CurrentArmor.Equals (otherChar.CurrentArmor)));
        }

        public override int GetHashCode ()
        {
            return Name.GetHashCode () +
                3 * Position.GetHashCode () +
                5 * CurrentHealth +
                7 * MaxHealth +
                11 * CurrentMana +
                13 * MaxMana +
                17 * Initiative +
                19 * MoveSpeed +
                23 * TeamIndex +
                29 * LastTurnMoved +
                31 * CurrentWeapon.GetHashCode () +
                37 * CurrentArmor.GetHashCode () +
                41 * SpellList.GetHashCode ();
        }

        public bool IsAlive ()
        {
            return CurrentHealth > 0;
        }

        public CharRep WithInitiative (int newInitiative)
        {
            var newChar = MakeCopy ();
            newChar.Initiative = newInitiative;
            return newChar;
        }
    }
}
