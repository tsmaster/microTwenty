using System;
using UnityEngine;

namespace MicroTwenty
{
    public class AttackOrder : CombatOrder
    {
        private CombatUnit _attacker;
        private CombatUnit _target;
        private readonly MapManager _mapMgr;
        private float _elapsedTime;
        private const float DISPLAY_TIME = 0.314f;

        public AttackOrder (MapManager mapMgr, CombatUnit attacker, CombatUnit target)
        {
            _attacker = attacker;
            _target = target;
            _mapMgr = mapMgr;

            _elapsedTime = 0.0f;

            mapMgr.GetCombatMgr ().AddLogLineFormat ("{0} attacks {1} with {2}",
                _attacker.unitName, _target.unitName, attacker.weapon.Name);

            var successRange = 1.0f;
            // use armor to hit reduction
            successRange -= _target.armor.ToHitReduction;
            // determine if hit

            bool hit = UnityEngine.Random.Range (0, 1.0f) <= successRange;

            if (hit) {
                var roll = _attacker.weapon.RollDamage ();
                mapMgr.GetCombatMgr ().AddLogLineFormat("{0} rolls {1} damage",
                    _attacker.unitName, roll);

                // use armor damage reduction
                var passThrough = Math.Max(0, roll - _target.armor.DamageReduction);

                if (passThrough != roll) {
                    mapMgr.GetCombatMgr ().AddLogLineFormat ("reduced to {0}", passThrough);
                }

                if (passThrough > 0) {
                    _target.currentHP -= passThrough;
                    mapMgr.GetCombatMgr ().AddLogLineFormat ("{0} new HP {1}/{2}",
                        _target.unitName, _target.currentHP, _target.maxHP);
                }
            } else {
                mapMgr.GetCombatMgr ().AddLogLineFormat ("bounces off {0}'s {1}", _target.unitName, _target.armor.Name);
            }

        }

        public void Draw ()
        {
            _mapMgr.DrawTintedSpriteAtLocation (_attacker.GetSpriteId (), _attacker.GetHexCoord(), TeamColorUtil.GetColorForTeam (_attacker.GetTeamID ()));
            _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_CIRCLE, _attacker.GetHexCoord (), UnityEngine.Color.blue);
            _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_X, _target.GetHexCoord (), UnityEngine.Color.red);
        }

        public CombatUnit GetCombatUnit ()
        {
            return _attacker;
        }

        public bool IsDone ()
        {
            var dt = DISPLAY_TIME;
            if (UnityEngine.Input.GetKey (UnityEngine.KeyCode.LeftShift)) {
                dt = 0.09f;
            }

            // HACK HACK HACK
            dt = 0.15f;

            return _elapsedTime >= dt;
        }

        public void Update (float deltaSeconds)
        {
            _elapsedTime += deltaSeconds;
        }
    }
}
