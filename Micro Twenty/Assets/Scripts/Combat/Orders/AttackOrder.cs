using System;

namespace MicroTwenty
{
    public class AttackOrder : CombatOrder
    {
        private CombatUnit _combatant;
        private CombatUnit _target;
        private bool _isDone;

        public AttackOrder (CombatUnit combatant, CombatUnit target)
        {
            _combatant = combatant;
            _target = target;
            _isDone = false;
        }

        public void Draw ()
        {
            // do nothing
        }

        public CombatUnit GetCombatUnit ()
        {
            return _combatant;
        }

        public bool IsDone ()
        {
            return _isDone;
        }

        public void Update (float deltaSeconds)
        {
            _target.currentHP -= 1;
            UnityEngine.Debug.LogFormat ("{0} hits {1}, new HP {2}/{3}", _combatant.unitName, _target.unitName, _target.currentHP, _target.maxHP);
            _isDone = true;
        }
    }
}
