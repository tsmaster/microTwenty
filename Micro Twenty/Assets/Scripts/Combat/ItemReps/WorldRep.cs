using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class WorldRep
    {
        // TODO - do these need guarding to be explicitly immutable?
        public List<CharRep> Chars { get; private set; }
        public Dictionary<HexCoord, TileRep> Tiles { get; private set; }

        public int CurrentTurn { get; private set; }

        public WorldRep (List<CharRep> chars, Dictionary<HexCoord, TileRep> tileDict, int currentTurn)
        {
            Chars = chars;
            Tiles = tileDict;
            CurrentTurn = currentTurn;
        }

        public WorldRep MoveCharTo (int movingCharIndex, HexCoord destPos)
        {
            var newWorld = MakeCopy ();
            var oldChar = newWorld.Chars [movingCharIndex];
            var newChar = oldChar.MoveTo (destPos);
            List<CharRep> newCharList = new List<CharRep> (newWorld.Chars) {
                [movingCharIndex] = newChar
            };
            newWorld.Chars = newCharList;
            return newWorld;
        }

        /// <summary>
        /// Gets a list of indices of units whose distance to the moving unit is less than or equal to range
        /// </summary>
        /// <returns>The unit indices in range of.</returns>
        /// <param name="movingUnitIndex">Moving unit index.</param>
        /// <param name="range">Range.</param>
        public List<int> GetUnitIndicesInRangeOf (int movingUnitIndex, int range)
        {
            var adjacentIndices = new List<int> ();
            var center = Chars [movingUnitIndex].Position;

            for (int i = 0; i < Chars.Count; ++i) {
                var c = Chars [i];
                if (!c.IsAlive ()) {
                    continue;
                }
                var cp = c.Position;
                if (cp.DistanceTo (center) <= range) {
                    adjacentIndices.Add (i);
                }
            }
            return adjacentIndices;
        }

        public List<int> GetAdjacentUnitIndices (int movingUnitIndex)
        {
            return GetUnitIndicesInRangeOf (movingUnitIndex, 1);
        }

        public WorldRep AdvanceUnit (int movingUnitIndex)
        {
            var newWorld = MakeCopy ();
            var newChar = newWorld.Chars [movingUnitIndex].SetLastTurn (newWorld.CurrentTurn);

            List<CharRep> newCharList = new List<CharRep> (newWorld.Chars) {
                [movingUnitIndex] = newChar
            };

            newWorld.Chars = newCharList;
            return newWorld;
        }

        internal List<int> GetAdjacentEnemyIndices (int movingUnitIndex)
        {
            var movingTeam = Chars [movingUnitIndex].TeamIndex;

            return GetAdjacentUnitIndices (movingUnitIndex).FindAll((idx) => {
                return Chars [idx].TeamIndex != movingTeam;
            });
        }

        public WorldRep DamageTargetFromAttacker (int targetCharIndex, int attackingCharIndex)
        {
            var attackingChar = Chars [attackingCharIndex];
            var targetChar = Chars [targetCharIndex];

            float expectedDamage = attackingChar.GetExpectedDamage ();
            ArmorRep defenderArmor = targetChar.CurrentArmor;

            float TEMP_TO_HIT = 0.95f;
            Debug.Assert (defenderArmor != null);
            expectedDamage = defenderArmor.ExpectedPassThrough (TEMP_TO_HIT, expectedDamage);

            var intDamage = (int)expectedDamage;

            if (intDamage <= 0) {
                return this;
            }

            var newWorld = MakeCopy ();
            var oldChar = newWorld.Chars [targetCharIndex];
            var newChar = oldChar.IncrementHealth (-intDamage);
            List<CharRep> newCharList = new List<CharRep> (newWorld.Chars) {
                [targetCharIndex] = newChar
            };
            newWorld.Chars = newCharList;
            return newWorld;
        }

        public WorldRep IncrementTurn ()
        {
            var c = MakeCopy ();
            c.CurrentTurn += 1;
            return c;
        }

        public int GetIndexOfMovingUnit ()
        {
            var highestInitiative = -1;
            var highestInitiativeUnitIndex = -1;

            for (int i = 0; i < Chars.Count; ++i) {
                var c = Chars [i];
                if ((!c.IsAlive ()) || (c.LastTurnMoved >= CurrentTurn)) {
                    continue;
                }
                if ((highestInitiativeUnitIndex == -1) ||
                    (c.Initiative > highestInitiative)) {
                    highestInitiative = c.Initiative;
                    highestInitiativeUnitIndex = i;
                }
            }

            if (highestInitiativeUnitIndex >= 0) {
                Debug.Assert (Chars [highestInitiativeUnitIndex].IsAlive ());
            }
            return highestInitiativeUnitIndex;
        }

        public List<int> GetEnemyIndices (int movingUnitIndex)
        {
            var outList = new List<int> ();
            var movingTeam = Chars [movingUnitIndex].TeamIndex;

            for (int i = 0; i < Chars.Count; ++i) {
                if ((Chars [i].TeamIndex != movingTeam) && 
                    (Chars[i].IsAlive())) {
                    outList.Add (i);
                }
            }
            return outList;
        }

        internal Dictionary<HexCoord, int> MakeFloodFillMap (int movingUnitIndex, int maxDistance)
        {
            var startPos = Chars [movingUnitIndex].Position;
            var outDict = new Dictionary<HexCoord, int> {
                [startPos] = 0
            };

            List<HexCoord> openList = new List<HexCoord> {
                startPos
            };

            while (openList.Count > 0) {
                var pos = openList [0]; 
                openList.RemoveAt (0);
                var curDist = outDict [pos];
                var newDist = curDist + 1;
                // HACK HACK HACK
                //if (newDist > maxDistance) {
                    //continue;
                //}

                foreach (var adjPos in GetWalkableAdjacentCoords (pos)) {
                    if ((outDict.ContainsKey (adjPos)) &&
                        (outDict [adjPos] <= newDist)) {
                        continue;
                    } else {
                        outDict [adjPos] = newDist;
                        openList.Add (adjPos);
                    }
                }
            }

            return outDict;
        }

        public List<HexCoord> GetWalkableAdjacentCoords (HexCoord pos)
        {
            var adjCoords = HexCoord.GetAtRangeFromLoc (1, pos);
            return adjCoords.FindAll (IsDynamicallyWalkable);
        }

        public bool IsStaticallyWalkable (HexCoord hc)
        {
            if (!Tiles.ContainsKey (hc)) {
                return false;
            }

            var tile = Tiles [hc];
            return tile.IsWalkable;
        }

        public bool IsDynamicallyWalkable (HexCoord hc)
        {
            if (!IsStaticallyWalkable (hc)) {
                return false;
            }

            foreach (var c in Chars) {
                if (c.IsAlive() && c.Position.Equals (hc)) {
                    return false;
                }
            }
            return true;
        }

        public WorldRep MakeCopy ()
        {
            return new WorldRep (Chars, Tiles, CurrentTurn);
        }
    }
}
