using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    internal class CombatActionGenerator : IActionGenerator
    {
        public List<ICombatAction> GenerateMoves (WorldRep gameState)
        {
            List<ICombatAction> actions = new List<ICombatAction> ();
            actions.AddRange (GenerateMeleeAttackWeakestMoves (gameState));
            actions.AddRange (GenerateRangedAttackWeakestMoves (gameState));
            //actions.AddRange (GenerateMoveToWeakestMoves (gameState, false));
            actions.AddRange (GenerateMoveToClosestActions (gameState, false));
            if (actions.Count == 0) {
                actions.Add (new PassAction (gameState.GetIndexOfMovingUnit ()));
            }

            actions = DedupActions (actions);

            if (actions.Count == 0) {
                // hack hack hack

                var evaluator = new WorldRepEvaluatorImpl ();
                if (evaluator.IsGameOver (gameState)) {
                    //Debug.LogWarning ("but game is over, so that makes sense");
                } else {
                    Debug.LogError ("No moves and Game is NOT over!!!");

                    var mui = gameState.GetIndexOfMovingUnit ();
                    var mu = gameState.Chars [mui];

                    Debug.LogErrorFormat ("moving unit {0} {1}", mui, gameState.Chars [mui].Name);
                    if (mu.IsAlive ()) {
                        Debug.Log ("Is alive, good");
                    } else {
                        Debug.LogError ("Is not alive, wtf?");
                    }

                    for (int i = 0; i < gameState.Chars.Count; ++i) {
                        var c = gameState.Chars [i];
                        Debug.LogErrorFormat ("unit {0} {1} {2}/{3} {4} {5}", 
                            i, c.Name, c.CurrentHealth, c.MaxHealth, c.Position, c.TeamIndex);
                    }

                    bool foundAnyAdjacent = false;
                    foreach (var amui in gameState.GetEnemyIndices (mui)) {
                        var amu = gameState.Chars [amui];
                        var dist = amu.Position.DistanceTo (mu.Position);
                        Debug.LogFormat ("unit {0} {1} is distance {2} away", amui, amu.Name, dist);
                        if (dist == 1) {
                            foundAnyAdjacent = true;
                            Debug.Log ("That's adjacent!");
                        }
                    }

                    if (!foundAnyAdjacent) {
                        Debug.LogError ("Something's wrong with the move code.");

                        GenerateMoveToWeakestMoves (gameState, true);

                        Debug.Break ();
                    }
                }
            }

            return actions;
        }

        private List<ICombatAction> DedupActions (List<ICombatAction> actions)
        {
            HashSet<ICombatAction> actionSet = new HashSet<ICombatAction> (actions);

            List<ICombatAction> outActions = new List<ICombatAction> ();
            foreach(var action in actionSet) {
                outActions.Add (action);
            }
            return outActions;
        }

        public List<ICombatAction> GenerateMeleeAttackWeakestMoves (WorldRep gameState)
        {
            var outList = new List<ICombatAction> ();
            var movingUnitIndex = gameState.GetIndexOfMovingUnit ();

            if (movingUnitIndex == -1) {
                return outList;
            }

            var movingUnit = gameState.Chars [movingUnitIndex];

            var weapon = movingUnit.CurrentWeapon;
            if (weapon == null) {
                return outList;
            }

            if (weapon.IsRanged) {
                return outList;
            }

            var adjacentEnemyIndices = gameState.GetAdjacentEnemyIndices (movingUnitIndex);

            // if we don't have enemy(s) adjacent, exit

            if ((adjacentEnemyIndices == null) ||
                (adjacentEnemyIndices.Count == 0)) {
                return outList;
            }

            int lowestHealth = -1;
            bool foundAny = false;

            // find the lowest health value
            foreach (var enemyIndex in adjacentEnemyIndices) {
                var adjChar = gameState.Chars [enemyIndex];
                if ((!foundAny) ||
                    (adjChar.CurrentHealth < lowestHealth)) {
                    foundAny = true;
                    lowestHealth = adjChar.CurrentHealth;
                }
            }

            Debug.Assert (foundAny);
            Debug.Assert (lowestHealth > 0);

            // for each enemy tied for weakest, return an order to attack that enemy
            foreach (var i in adjacentEnemyIndices) {
                var c = gameState.Chars [i];
                if (c.CurrentHealth != lowestHealth) {
                    continue;
                }
                outList.Add (new AttackActionImpl (movingUnitIndex, i));
            }

            return outList;
        }

        public List<ICombatAction> GenerateRangedAttackWeakestMoves (WorldRep gameState)
        {
            var outList = new List<ICombatAction> ();
            var movingUnitIndex = gameState.GetIndexOfMovingUnit ();

            if (movingUnitIndex == -1) {
                return outList;
            }

            var movingUnit = gameState.Chars [movingUnitIndex];

            var weapon = movingUnit.CurrentWeapon;
            if (weapon == null) {
                return outList;
            }

            if (!weapon.IsRanged) {
                return outList;
            }

            var rangedEnemyIndices = gameState.GetEnemyIndicesInRange (movingUnitIndex, weapon.MinRange, weapon.MaxRange);

            // if we don't have enemy(s) adjacent, exit

            if ((rangedEnemyIndices == null) ||
                (rangedEnemyIndices.Count == 0)) {
                return outList;
            }

            int lowestHealth = -1;
            bool foundAny = false;

            // find the lowest health value
            foreach (var enemyIndex in rangedEnemyIndices) {
                var adjChar = gameState.Chars [enemyIndex];
                if ((!foundAny) ||
                    (adjChar.CurrentHealth < lowestHealth)) {
                    foundAny = true;
                    lowestHealth = adjChar.CurrentHealth;
                }
            }

            Debug.Assert (foundAny);
            Debug.Assert (lowestHealth > 0);

            // for each enemy tied for weakest, return an order to attack that enemy
            foreach (var i in rangedEnemyIndices) {
                var c = gameState.Chars [i];
                if (c.CurrentHealth != lowestHealth) {
                    continue;
                }
                outList.Add (new AttackActionImpl (movingUnitIndex, i));
            }

            return outList;
        }


        public List<ICombatAction> GenerateMoveToWeakestMoves (WorldRep gameState, bool verbose)
        {
            if (verbose) {
                Debug.LogError ("top of GMTWM");
            }
            var outList = new List<ICombatAction> ();
            var movingUnitIndex = gameState.GetIndexOfMovingUnit ();

            if (movingUnitIndex == -1) {
                if (verbose) {
                    Debug.LogError ("MUI is -1");
                }

                return outList;
            }

            var movingUnit = gameState.Chars [movingUnitIndex];
            var startPos = movingUnit.Position;

            var adjacentEnemyIndices = gameState.GetAdjacentEnemyIndices (movingUnitIndex);

            // if we have enemy(s) adjacent, exit

            if ((adjacentEnemyIndices != null) &&
                (adjacentEnemyIndices.Count > 0)) {

                if (verbose) {
                    Debug.LogError ("No moves, because found adj enemies");
                }

                return outList;
            }

            var enemyIndices = gameState.GetEnemyIndices (movingUnitIndex);
            if (enemyIndices.Count == 0) {

                if (verbose) {
                    Debug.LogError ("No moves, because found no enemies");
                }

                return outList;
            }

            var highestDistance = -1;
            foreach (var ei in enemyIndices) {
                var enemy = gameState.Chars [ei];
                var dist = enemy.Position.DistanceTo (movingUnit.Position);
                highestDistance = Math.Max (highestDistance, dist);
            }

            // make a floodfill map to make pathfinding easier

            if (verbose) {
                Debug.LogErrorFormat ("highest distance: {0}", highestDistance);
            }

            var floodFillMap = gameState.MakeFloodFillMap (movingUnitIndex, highestDistance + 2);

            // for each enemy, 
            //   for each open space adjacent
            //     try to find a move to it
            //     if the enemy is lower (or tied) for lowest HP
            //     if moveLength (in turns) is shorter (or tied) for lowest number of turns
            //     replace list (or add to list for ties) with this turn's portion of the move

            int lowestHealth = -1;
            bool foundAny = false;
            int lowestMoveLength = -1;

            int movingUnitSpeed = movingUnit.MoveSpeed;

            foreach (int enemyIndex in enemyIndices) {
                var enemyChar = gameState.Chars [enemyIndex];
                var enemyCharHealth = enemyChar.CurrentHealth;

                if (verbose) {
                    Debug.LogErrorFormat ("pathing to: {0} at {1}", enemyChar.Name, enemyChar.Position);
                }

                foreach (var destCoord in gameState.GetWalkableAdjacentCoords (enemyChar.Position)) {
                    if (!floodFillMap.ContainsKey (destCoord)) {
                        if (verbose) {
                            Debug.LogErrorFormat ("pathing to loc that can't get to: {0}", destCoord);
                        }

                        continue;
                    }
                    if (verbose) {
                        Debug.LogErrorFormat ("pathing to loc that can get to: {0}", destCoord);
                    }

                    var movesToEnemy = floodFillMap [destCoord] / movingUnitSpeed;

                    if ((!foundAny) ||
                        ((enemyCharHealth < lowestHealth) && (movesToEnemy < lowestMoveLength)) ||
                        ((enemyCharHealth < lowestHealth) && (movesToEnemy == lowestMoveLength)) ||
                        ((enemyCharHealth == lowestHealth) && (movesToEnemy < lowestMoveLength))) {
                        outList.Clear ();

                        if (verbose) {
                            Debug.LogError ("resetting outlist");
                        }

                        var movement = MakeMoveFromStartToDest (floodFillMap, 
                            startPos, destCoord, movingUnitSpeed, movingUnitIndex);
                        outList.Add (movement);
                        lowestHealth = enemyCharHealth;
                        lowestMoveLength = movesToEnemy;

                        if (verbose) {
                            Debug.LogErrorFormat ("new lowest health {0} movelen {1}", lowestHealth, lowestMoveLength);
                        }

                    } else if ((enemyCharHealth == lowestHealth) && (movesToEnemy == lowestMoveLength)) {
                        var movement = MakeMoveFromStartToDest (floodFillMap, 
                            startPos, destCoord, movingUnitSpeed, movingUnitIndex);
                        outList.Add (movement);
                        if (verbose) {
                            Debug.LogErrorFormat ("adding move to {0}", destCoord);
                        }
                    }
                }
            }

            return outList;
        }

        public List<ICombatAction> GenerateMoveToClosestActions (WorldRep gameState, bool verbose)
        {
            if (verbose) {
                Debug.LogError ("top of GMTCA");
            }
            var outList = new List<ICombatAction> ();
            var movingUnitIndex = gameState.GetIndexOfMovingUnit ();

            if (movingUnitIndex == -1) {
                if (verbose) {
                    Debug.LogError ("MUI is -1");
                }

                return outList;
            }

            var movingUnit = gameState.Chars [movingUnitIndex];
            var startPos = movingUnit.Position;

            var adjacentEnemyIndices = gameState.GetAdjacentEnemyIndices (movingUnitIndex);

            // if we have enemy(s) adjacent, exit

            if ((adjacentEnemyIndices != null) &&
                (adjacentEnemyIndices.Count > 0)) {

                if (verbose) {
                    Debug.LogError ("No moves, because found adj enemies");
                }

                return outList;
            }

            var enemyIndices = gameState.GetEnemyIndices (movingUnitIndex);
            if (enemyIndices.Count == 0) {

                if (verbose) {
                    Debug.LogError ("No moves, because found no enemies");
                }

                return outList;
            }

            enemyIndices.Sort((int unitIndex1, int unitIndex2) => {
                var unit1 = gameState.Chars [unitIndex1];
                var unit2 = gameState.Chars [unitIndex2];

                var dist1 = unit1.Position.DistanceTo (movingUnit.Position);
                var dist2 = unit2.Position.DistanceTo (movingUnit.Position);

                return dist1.CompareTo (dist2);
            });

            var highestDistanceEnemyIndex = enemyIndices [enemyIndices.Count - 1];
            var highestDistanceEnemy = gameState.Chars [highestDistanceEnemyIndex];
            var highestDistance = highestDistanceEnemy.Position.DistanceTo (movingUnit.Position); 

            // make a floodfill map to make pathfinding easier

            if (verbose) {
                Debug.LogErrorFormat ("highest distance: {0}", highestDistance);
            }

            var floodFillMap = gameState.MakeFloodFillMap (movingUnitIndex, highestDistance + 2);

            // for each enemy, 
            //   for each open space adjacent
            //     try to find a move to it
            //     if the move is lower distance or equal distance and enemy is of higher initiative, choose that

            bool foundAny = false;
            int lowestMoveLength = -1;
            int highestEnemyInitiative = -1;
            HexCoord bestDest = null;

            foreach (int enemyIndex in enemyIndices) {
                var enemyChar = gameState.Chars [enemyIndex];

                if (foundAny && (enemyChar.Position.DistanceTo (movingUnit.Position) - 1 > lowestMoveLength)) {
                    // unit cannot be reached
                    continue;
                }

                if (verbose) {
                    Debug.LogErrorFormat ("pathing to: {0} at {1}", enemyChar.Name, enemyChar.Position);
                }

                foreach (var destCoord in gameState.GetWalkableAdjacentCoords (enemyChar.Position)) {
                    if (!floodFillMap.ContainsKey (destCoord)) {
                        if (verbose) {
                            Debug.LogErrorFormat ("pathing to loc that can't get to: {0}", destCoord);
                        }

                        continue;
                    }
                    if (verbose) {
                        Debug.LogErrorFormat ("pathing to loc that can get to: {0}", destCoord);
                    }

                    var moveLength = floodFillMap [destCoord];

                    if ((!foundAny) ||
                        (moveLength < lowestMoveLength) ||
                        ((moveLength == lowestMoveLength) && (enemyChar.Initiative < highestEnemyInitiative))) {
                        foundAny = true;
                        bestDest = destCoord;
                        lowestMoveLength = moveLength;
                        highestEnemyInitiative = enemyChar.Initiative;
                    }
                }

                if (foundAny) {
                    var movingUnitSpeed = movingUnit.MoveSpeed;

                    var move = MakeMoveFromStartToDest (floodFillMap,
                            startPos, bestDest, movingUnitSpeed, movingUnitIndex);
                    outList.Add (move);
                }
            }

            return outList;
        }


        ICombatAction MakeMoveFromStartToDest (Dictionary<HexCoord, int> floodFillMap, 
            HexCoord start, HexCoord dest, 
            int moveLength, int movingCharIndex) {

            Debug.Assert (floodFillMap.ContainsKey (start));
            Debug.Assert (floodFillMap.ContainsKey (dest));

            HexCoord cursor = dest;

            while (!cursor.Equals (start)) {
                var curDist = floodFillMap [cursor];
                if (curDist <= moveLength) {
                    return new MovementAction (movingCharIndex, cursor);
                }

                bool foundAny = false;

                foreach (var nextCursor in HexCoord.GetAtRangeFromLoc (1, cursor)) {
                    if ((floodFillMap.ContainsKey (nextCursor)) &&
                        (floodFillMap [nextCursor] == curDist - 1)) {
                        cursor = nextCursor;
                        foundAny = true;
                        break;
                    }
                }

                if (!foundAny) {
                    Debug.LogErrorFormat ("failed to find any successors to cursor at {0}", cursor);
                }
            }
            return new PassAction (movingCharIndex);
        }
    }
}