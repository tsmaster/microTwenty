using System;
namespace MicroTwenty
{
    public class SpellActionImpl : ICombatAction
    {
        private int _attackingCharIndex;
        private int _targetCharIndex;
        private SpellRep _spell;

        public SpellActionImpl (int attackingCharIndex, int targetCharIndex, SpellRep spell)
        {
            _attackingCharIndex = attackingCharIndex;
            _targetCharIndex = targetCharIndex;
            _spell = spell;
        }

        public WorldRep Apply (WorldRep before)
        {
            return _spell.ApplyFunc (_attackingCharIndex, _targetCharIndex, before);
        }

        public CombatOrder GetCombatOrder (MapManager mapManager, HexMap unused)
        {
            var attacker = mapManager.GetCombatUnitByIndex (_attackingCharIndex);
            var target = mapManager.GetCombatUnitByIndex (_targetCharIndex);

            return new ParticleOrder (mapManager, attacker, target, _spell.Sprite, _spell.SpriteColor, _spell.HitAction);
        }
    }
}
