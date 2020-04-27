using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class MovementSelectionMode : SelectionMode
    {
        private int _movingUnitIndex;
        private MapManager _mapManager;
        private CombatMgr _combatMgr;
        private Dictionary<HexCoord, int> _floodFill;
        private CombatUnit _movingUnit;
        private int _moveCount;

        public MovementSelectionMode (int movingUnitIndex, MapManager mapManager, CombatMgr combatMgr, Dictionary<HexCoord, int> floodFill)
        {
            _movingUnitIndex = movingUnitIndex;
            _mapManager = mapManager;
            _combatMgr = combatMgr;
            _floodFill = floodFill;

            _movingUnit = _combatMgr.GetCombatUnitByIndex (movingUnitIndex);
            _moveCount = _movingUnit.maxMove;

            CursorPos = combatMgr.GetCombatUnitByIndex (movingUnitIndex).GetHexCoord ();
        }

        public override SpriteId GetCursorSpriteIdForLoc (HexCoord hc, out Color color)
        {
            if (!IsLocationSelectable (hc)) {
                color = Color.red;
                return SpriteId.SPRITE_TILE_CURSOR_X;
            } else {
                color = Color.blue;
                return SpriteId.SPRITE_TILE_CURSOR_DOT;
            }
        }

        public override bool IsLocationSelectable (HexCoord hc)
        {
            if (!_floodFill.ContainsKey (hc)) {
                return false;
            }

            return (_floodFill.ContainsKey (hc) &&
                _floodFill [hc] <= _moveCount);

        }
    }
}
