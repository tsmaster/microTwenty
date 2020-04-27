using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class MeleeTargetSelectionMode : SelectionMode
    {
        private int _movingUnitIndex;
        private MapManager _mapManager;
        private CombatMgr _combatMgr;
        private CombatUnit _movingUnit;
        private int _moveCount;
        private List<HexCoord> _enemyLocs;

        public MeleeTargetSelectionMode (int movingUnitIndex, MapManager mapManager, CombatMgr combatMgr)
        {
            _movingUnitIndex = movingUnitIndex;
            _mapManager = mapManager;
            _combatMgr = combatMgr;

            _movingUnit = _combatMgr.GetCombatUnitByIndex (movingUnitIndex);
            _moveCount = _movingUnit.maxMove;

            var adjacentEnemyIndices = combatMgr.GetEnemyIndicesAdjacentTo (_movingUnit.GetHexCoord(), _movingUnit.GetTeamID());

            _enemyLocs = new List<HexCoord> ();
            foreach (var enemyIndex in adjacentEnemyIndices) {
                var enemy = combatMgr.GetCombatUnitByIndex (enemyIndex);
                _enemyLocs.Add (enemy.GetHexCoord ());
            }

            if (_enemyLocs.Count > 0) {
                CursorPos = _enemyLocs[0];
            } else {
                CursorPos = combatMgr.GetCombatUnitByIndex (movingUnitIndex).GetHexCoord ();
            }
        }

        public override SpriteId GetCursorSpriteIdForLoc (HexCoord hc, out Color color)
        {
            if (!IsLocationSelectable (hc)) {
                color = Color.red;
                return SpriteId.SPRITE_TILE_CURSOR_X;
            } else {
                color = Color.yellow;
                return SpriteId.SPRITE_TILE_CURSOR_CIRCLE;
            }
        }

        public override bool IsLocationSelectable (HexCoord hc)
        {
            return (_enemyLocs.Contains (hc));
        }
    }
}
