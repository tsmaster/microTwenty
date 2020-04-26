using System;
using System.Collections.Generic;
using UnityEngine;

/// See https://en.wikipedia.org/wiki/Minimax

namespace MicroTwenty
{
    public class MiniMax
    {
        private WorldRep _worldRep;
        private IWorldRepEvaluator _evaluator;
        private Dictionary<int, IActionGenerator> _moveGenerators;
        private IActionManager _moveManager;
        private int _movingPlayer;
        private bool _isMaximizing;
        private List<ICombatAction> _moves;
        private int _workingOnChildIndex;
        private int _bestChildIndex;
        private float _bestChildValue;
        private MiniMax _childMiniMax;
        private int _depth;
        private float _timeLimitSeconds;
        private float _elapsedTimeSeconds;
        private int _movingUnit;

        public MiniMax (WorldRep worldRep, 
            IWorldRepEvaluator evaluator, 
            Dictionary<int, IActionGenerator> moveGenerators,
            IActionManager moveManager,
            int depth,
            float timeLimitSeconds)
        {
            _evaluator = evaluator;
            _moveGenerators = moveGenerators;
            _moveManager = moveManager;
            while (true) {
                _movingUnit = worldRep.GetIndexOfMovingUnit ();
                if (_movingUnit == -1) {
                    worldRep = worldRep.IncrementTurn ();
                } else {
                    break;
                }
            }

            _movingPlayer = moveManager.GetMovingPlayer (worldRep);
            _worldRep = worldRep.AdvanceUnit(_movingUnit);

            _isMaximizing = moveManager.IsMaximising (_movingPlayer);
            _moves = moveGenerators [_movingPlayer].GenerateMoves (worldRep);

            _workingOnChildIndex = 0;
            _bestChildIndex = -1;

            if (_isMaximizing) {
                _bestChildValue = float.MinValue;
            } else {
                _bestChildValue = float.MaxValue;
            }

            _childMiniMax = null;
            _depth = depth;
            _timeLimitSeconds = timeLimitSeconds;
            _elapsedTimeSeconds = 0.0f;
        }

        public void Evaluate (float timeToEvaluate, out bool done, out float value)
        {
            if ((_moves.Count == 0) || (_evaluator.IsGameOver(_worldRep)))
             {
                value = _evaluator.Evaluate (_worldRep);
                done = true;
                return;
            }

            float startTime = Time.time;

            timeToEvaluate = Mathf.Min (timeToEvaluate, _timeLimitSeconds - _elapsedTimeSeconds);

            while (Time.time < startTime + timeToEvaluate) {
                if (_workingOnChildIndex != 0) {
                    Debug.Assert (_bestChildIndex != -1);
                }

                if (_workingOnChildIndex >= _moves.Count) {
                    done = true;
                    value = _bestChildValue;
                    _elapsedTimeSeconds += Time.time - startTime;
                    return;
                }

                if (_depth == 0) {
                    var childValue = _evaluator.Evaluate (_worldRep);
                    UpdateBestValue (childValue);
                    _childMiniMax = null;
                } else {
                    if (_childMiniMax == null) {

                        _childMiniMax = new MiniMax (
                            _moves [_workingOnChildIndex].Apply (_worldRep),
                            _evaluator,
                            _moveGenerators,
                            _moveManager,
                            _depth - 1,
                            _timeLimitSeconds - _elapsedTimeSeconds);
                    }

                    _childMiniMax.Evaluate (timeToEvaluate, out bool childIsDone, out float childValue);

                    if (childIsDone) {
                        UpdateBestValue (childValue);
                        _childMiniMax = null;
                    }
                }
            }

            done = false;
            value = -9999.9999f;
        }

        private void UpdateBestValue (float childValue)
        {
            if (_isMaximizing) {
                if ((_bestChildIndex < 0) ||
                    (childValue > _bestChildValue)) {
                    _bestChildIndex = _workingOnChildIndex;
                    _bestChildValue = childValue;
                }
            } else {
                if ((_bestChildIndex < 0) ||
                    (childValue < _bestChildValue)) {
                    _bestChildIndex = _workingOnChildIndex;
                    _bestChildValue = childValue;
                }
            }
            _workingOnChildIndex++;
            Debug.Assert (_bestChildIndex >= 0);
        }

        public bool IsComplete ()
        {
            bool isDone = (_workingOnChildIndex >= _moves.Count);

            if (isDone && (_bestChildIndex < 0)) {
                Debug.LogWarningFormat ("I'm done, with an invalid child index, and with {0} children", _moves.Count);
            }

            return isDone;
        }

        public ICombatAction GetSelectedAction ()
        {
            if (!IsComplete ()) {
                Debug.LogWarningFormat ("Um, not done yet, still working on {0}", _workingOnChildIndex);
            }

            if (_bestChildIndex < 0) {
                return null;
            }
            return _moves [_bestChildIndex];
        }
    }
}
