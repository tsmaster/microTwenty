using System;
namespace MicroTwenty
{
    public class CombatantSprite:DynamicObject
    {
        private SpriteId spriteId;
        private int teamNumber;

        public CombatantSprite (GameMgr gameMgr, HexCoord hexCoord, SpriteId spriteId, int teamNumber) : base(gameMgr, hexCoord, DynamicObjectType.COMBATANT, true)
        {
            this.spriteId = spriteId;
            this.teamNumber = teamNumber;
        }

        public SpriteId GetSpriteId ()
        {
            return spriteId;
        }

        internal int GetTeam ()
        {
            return teamNumber;
        }
    }
}
