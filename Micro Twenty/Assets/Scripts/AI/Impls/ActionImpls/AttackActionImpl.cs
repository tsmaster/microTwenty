using System;
namespace MicroTwenty
{
    public class AttackActionImpl : ICombatAction
    {
        private int _attackingCharIndex;
        private int _targetCharIndex;

        public AttackActionImpl (int attackingCharIndex, int targetCharIndex)
        {
            _attackingCharIndex = attackingCharIndex;
            _targetCharIndex = targetCharIndex;
        }

        public WorldRep Apply (WorldRep before)
        {
            return before.DamageTargetFromAttacker (_targetCharIndex, _attackingCharIndex);
        }

        public CombatOrder GetCombatOrder (MapManager mapManager, HexMap unused)
        {
            var attacker = mapManager.GetCombatUnitByIndex (_attackingCharIndex);
            var target = mapManager.GetCombatUnitByIndex (_targetCharIndex);
            return new AttackOrder (mapManager, attacker, target);
        }
    }
}
