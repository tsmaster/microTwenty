using System;
namespace MicroTwenty
{
    public class DynamicObject
    {
        public enum DynamicObjectType
        {
            PLAYER,
            TRIGGER,
            SIGN,
            RATMAN,
            SLIME,
            CURSOR,
            TREE,
            SHIP,
            COMBATANT,
        }

        public HexCoord hexCoord;
        public DynamicObjectType objectType;
        public bool blocksMovement;
        protected GameMgr gameMgr;

        public DynamicObject (GameMgr gameMgr, HexCoord hexCoord, DynamicObjectType objectType, bool blocksMovement)
        {
            this.hexCoord = hexCoord;
            this.objectType = objectType;
            this.blocksMovement = blocksMovement;
            this.gameMgr = gameMgr;
        }

        public virtual void OnMoveOver ()
        {
            // does nothing by default
        }
    }
}
