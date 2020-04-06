using System;
namespace MicroTwenty
{
    public class PassOrder : CombatOrder
    {
        private CombatUnit _combatUnit;

        public PassOrder (CombatUnit combatUnit)
        {
            _combatUnit = combatUnit;
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
            return true;
        }

        public void Update (float deltaSeconds)
        {
            // do nothing
        }
    }
}
