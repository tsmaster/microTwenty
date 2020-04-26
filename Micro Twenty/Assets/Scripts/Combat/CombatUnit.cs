using System;

namespace MicroTwenty
{
    public class CombatUnit
    {
        public string unitName;
        private HexCoord hexCoord;
        private int teamIndex;
        public int currentHP;
        public int maxHP;
        public int currentMP;
        public int maxMP;
        public int initiative;
        public int lastTurnMoved;
        public WeaponRep weapon;
        public ArmorRep armor;

        public CombatantSprite _sprite;

        public int maxMove;


        public CombatUnit (string unitName, HexCoord hexCoord, int teamIndex, CombatantSprite sprite)
        {
            this.unitName = unitName;
            this.hexCoord = hexCoord;
            this.teamIndex = teamIndex;
            _sprite = sprite;
            initiative = 1;
            lastTurnMoved = -1;
            this.weapon = new WeaponFistRep ();
            this.armor = new ArmorClothRep ();
        }

        public HexCoord GetHexCoord ()
        {
            return hexCoord;
        }

        public SpriteId GetSpriteId ()
        {
            return _sprite.GetSpriteId ();
        }

        public DynamicObject.DynamicObjectType GetDynamicObjectType ()
        {
            return DynamicObject.DynamicObjectType.COMBATANT;
        }

        public DynamicObject GetDynamicObject ()
        {
            return _sprite;
        }

        internal void SetHexCoord (HexCoord destination)
        {
            this.hexCoord = destination;
            _sprite.hexCoord = destination;
        }

        public int GetTeamID ()
        {
            return teamIndex;
        }

        public bool IsAlive ()
        {
            return currentHP > 0;
        }

        public int GetInitiative ()
        {
            return initiative;
        }

        public int GetLastTurnMoved ()
        {
            return lastTurnMoved;
        }

        internal void SetLastTurnMoved (int currentTurnNumber)
        {
            lastTurnMoved = currentTurnNumber;
        }

        public CombatUnit AddWeapon (WeaponRep wr)
        {
            weapon = wr;
            return this;
        }

        public CombatUnit AddArmor (ArmorRep ar)
        {
            armor = ar;
            return this;
        }
    }
}