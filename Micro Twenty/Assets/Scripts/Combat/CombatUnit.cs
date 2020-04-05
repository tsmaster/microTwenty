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

        public CombatantSprite _sprite;

        public int maxMove;

        public CombatUnit (string unitName, HexCoord hexCoord, int teamIndex, CombatantSprite sprite)
        {
            this.unitName = unitName;
            this.hexCoord = hexCoord;
            this.teamIndex = teamIndex;
            _sprite = sprite;
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
    }
}