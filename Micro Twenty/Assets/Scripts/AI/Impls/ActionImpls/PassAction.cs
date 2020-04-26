using System;
namespace MicroTwenty
{
    public class PassAction : ICombatAction
    {
        int _movingUnitIndex;

        public PassAction (int movingUnitIndex)
        {
            _movingUnitIndex = movingUnitIndex;
        }

        public WorldRep Apply (WorldRep before)
        {
            // Do nothing
            return before;
        }

        public CombatOrder GetCombatOrder (MapManager mapManager, HexMap hexMap)
        {
            return new PassOrder (mapManager.GetCombatUnitByIndex (_movingUnitIndex));
        }
    }
}
