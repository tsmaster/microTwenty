using System;
namespace MicroTwenty
{
    public class MovementAction : ICombatAction
    {
        internal int _movingCharIndex;
        internal HexCoord _destPos;

        public MovementAction (int movingCharIndex, HexCoord destPos)
        {
            _movingCharIndex = movingCharIndex;
            _destPos = destPos;
        }

        public WorldRep Apply (WorldRep before)
        {
            return before.MoveCharTo (_movingCharIndex, _destPos);
        }

        public CombatOrder GetCombatOrder (MapManager mapManager, HexMap hexMap)
        {
            CombatUnit combatUnit = mapManager.GetCombatUnitByIndex (_movingCharIndex);
            return new CombatMoveOrder (mapManager, hexMap, _destPos, combatUnit);
        }

        public override bool Equals (object otherObj)
        {
            MovementAction otherAction = otherObj as MovementAction;
            if (otherAction == null) {
                return false;
            }
            return _movingCharIndex == otherAction._movingCharIndex && _destPos == otherAction._destPos;
        }
    }
}
