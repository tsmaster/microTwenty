using System;
namespace MicroTwenty
{
    public class PassOrder : CombatOrder
    {
        private readonly CombatUnit _combatUnit;
        private bool _isDone;

        /// <summary>
        /// Not just pass, but if you're injured, doing nothing restores 1 HP
        /// </summary>
        /// <param name="combatUnit">Combat unit.</param>


        public PassOrder (CombatUnit combatUnit)
        {
            _combatUnit = combatUnit;
            _isDone = false;
        }
        public void Draw ()
        {
            // do nothing
        }

        public CombatUnit GetCombatUnit ()
        {
            return _combatUnit;
        }

        public bool IsDone ()
        {
            return _isDone;
        }

        public void Update (float deltaSeconds)
        {
            if (_combatUnit.IsAlive ()) {
                if (_combatUnit.currentHP < _combatUnit.maxHP) {
                    _combatUnit.currentHP++;
                    UnityEngine.Debug.LogFormat ("{0} heals to {1}/{2} HP", _combatUnit.unitName, _combatUnit.currentHP, _combatUnit.maxHP);
                }

                if (_combatUnit.currentMP < _combatUnit.maxMP) {
                    _combatUnit.currentMP++;
                    UnityEngine.Debug.LogFormat ("{0} restores to {1}/{2} MP", _combatUnit.unitName, _combatUnit.currentMP, _combatUnit.maxMP);
                }
            }

            _isDone = true;
        }
    }
}
