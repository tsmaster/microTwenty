using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class RangedTargetSelectionMode : SelectionMode
    {
        private int _movingUnitIndex;
        private MapManager _mapManager;
        private CombatMgr _combatMgr;
        private CombatUnit _movingUnit;
        private int _moveCount;
        private List<HexCoord> _targetLocs;

        public RangedTargetSelectionMode (int movingUnitIndex, MapManager mapManager, CombatMgr combatMgr, int minRange, int maxRange, bool allowFriendly, bool allowHostile)
        {
            _movingUnitIndex = movingUnitIndex;
            _mapManager = mapManager;
            _combatMgr = combatMgr;

            _movingUnit = _combatMgr.GetCombatUnitByIndex (movingUnitIndex);
            _moveCount = _movingUnit.maxMove;

            var rangedUnitIndices = combatMgr.GetIndicesInRange (_movingUnit.GetHexCoord(), minRange, maxRange);

            _targetLocs = new List<HexCoord> ();
            foreach (var targetIndex in rangedUnitIndices) {
                var target = combatMgr.GetCombatUnitByIndex (targetIndex);
                var targetLoc = target.GetHexCoord ();

                bool targetIsFriendly = target.GetTeamID () == _movingUnit.GetTeamID ();
                if ((targetIsFriendly && (!allowFriendly)) ||
                    (!targetIsFriendly) && (!allowHostile)) {
                    continue;
                }

                if (!IsLOSBlocked (_movingUnit.GetHexCoord (), targetLoc)) {
                    _targetLocs.Add (targetLoc);
                }
            }

            if (_targetLocs.Count > 0) {
                CursorPos = _targetLocs[0];
            } else {
                CursorPos = combatMgr.GetCombatUnitByIndex (movingUnitIndex).GetHexCoord ();
            }
        }

        // TODO probably move this to map manager?
        private bool IsLOSBlocked (HexCoord start, HexCoord end)
        {
            var coords = HexCoord.DrawLine (start, end);

            coords = coords.GetRange (1, coords.Count - 2);
            foreach (var c in coords) {
                var tiles = _mapManager.GetTilesAt (c);
                if (tiles.Count == 0) {
                    return true;
                }
                foreach (var t in tiles) {
                    if (!t.CanMove) {
                        return true;
                    }
                }
            }

            return false;
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
            return (_targetLocs.Contains (hc));
        }
    }
}
