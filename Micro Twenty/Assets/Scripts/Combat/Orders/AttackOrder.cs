using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class AttackOrder : CombatOrder
    {
        private CombatUnit _attacker;
        private CombatUnit _target;
        private readonly MapManager _mapMgr;
        private float _elapsedTime;
        private int _range;
        private const float DISPLAY_TIME = 0.314f;

        private SpriteId _ammoSprite;
        private const float AMMO_SPEED = 8.0f;

        private bool _isRanged = false;
        private float _flightTime;

        public AttackOrder (MapManager mapMgr, CombatUnit attacker, CombatUnit target, SpriteId ammoSprite)
        {
            _attacker = attacker;
            _target = target;
            _mapMgr = mapMgr;

            _elapsedTime = 0.0f;

            _range = attacker.GetHexCoord ().DistanceTo (target.GetHexCoord ());
            // TODO what are the rules for ranged to hit?

            Debug.LogFormat ("Is Ranged? {0} {1}", _range, _range > 1);

            mapMgr.GetCombatMgr ().AddLogLineFormat ("{0} attacks {1} with {2}",
                _attacker.unitName, _target.unitName, attacker.weapon.Name);

            var successRange = 1.0f;
            // use armor to hit reduction
            successRange -= _target.armor.ToHitReduction;
            // determine if hit

            if (_range > 1) {
                // fake a to hit falloff
                var toHitReductionByRange = 0.05f;
                var extraRange = _range - attacker.weapon.MinRange;
                successRange -= toHitReductionByRange * extraRange;

                _ammoSprite = CalcAmmoForVector (target.GetHexCoord ().Sub(attacker.GetHexCoord ()), ammoSprite);
                _flightTime = _range / AMMO_SPEED;
                Debug.LogFormat ("Flight time: {0} seconds", _flightTime);
                _isRanged = true;
            } else {
                _ammoSprite = SpriteId.SPRITE_TILE_EMPTY;
                _isRanged = false;
            }

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

        private SpriteId CalcAmmoForVector (HexCoord vectorToTarget, SpriteId baseAmmoSprite)
        {
            int hextor = CalcHextorForVector (vectorToTarget);

            switch (hextor) {
            case 0:
            case 3:
                return SpriteId.SPRITE_ARROW_EW;
            case 1:
            case 4:
                return SpriteId.SPRITE_ARROW_SWNE;
            case 2:
            case 5:
                return SpriteId.SPRITE_ARROW_NWSE;
            default:
                // error
                return SpriteId.SPRITE_TILE_SLIME;
            }
        }

        private int CalcHextorForVector (HexCoord vec)
        {
            // from https://www.redblobgames.com/grids/hexagons/directions.html
            // Thanks, Amit!

            var xmy = vec.x - vec.y;
            var ymz = vec.y - vec.z;
            var zmx = vec.z - vec.x;

            var axmy = Math.Abs (xmy);
            var aymz = Math.Abs (ymz);
            var azmx = Math.Abs (zmx);

            if ((axmy > aymz) && (axmy > azmx)) {
                // E or W
                return (xmy > 0) ? 0 : 3;
            } else if (azmx > aymz) {
                // SW or NE
                return (zmx > 0) ? 4 : 1;
            } else {
                // NW or SE
                return (ymz > 0) ? 2 : 5;
            }
        }

        private int CalcHextorForVectorSlow (HexCoord vec)
        {
            List<HexCoord> bases = new List<HexCoord> {
                new HexCoord(1, -1, 0), // East
                new HexCoord(1, 0, -1), // NE
                new HexCoord(0, 1, -1), // NW
                new HexCoord(-1, 1, 0), // West
                new HexCoord(-1, 0, 1), // SW
                new HexCoord(0, -1, 1)  // SE
            };

            int maxDot = -1;
            int bestHextor = -1;
            for (int i = 0; i < 6; ++i) {
                var b = bases [i];
                int dot = vec.x * b.x + vec.y * b.y + vec.z * b.z;

                if ((bestHextor == -1) ||
                    (dot > maxDot)) {
                    maxDot = dot;
                    bestHextor = i;
                }
            }
            return bestHextor;
        }

        public void Draw ()
        {
            _mapMgr.DrawTintedSpriteAtLocation (_attacker.GetSpriteId (), _attacker.GetHexCoord(), TeamColorUtil.GetColorForTeam (_attacker.GetTeamID ()));
            _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_CIRCLE, _attacker.GetHexCoord (), UnityEngine.Color.blue);
            if (_isRanged) {
                var fracTime = _elapsedTime / _flightTime;

                var attackerPos = _attacker.GetHexCoord ();
                var targetPos = _target.GetHexCoord ();

                _mapMgr.HexCoordToScreenCoords (attackerPos, out int attScrX, out int attScrY);
                _mapMgr.HexCoordToScreenCoords (targetPos, out int tarScrX, out int tarScrY);

                int px = (int)BdgMath.Map (fracTime, 0.0f, 1.0f, attScrX, tarScrX);
                int py = (int)BdgMath.Map (fracTime, 0.0f, 1.0f, attScrY, tarScrY);

                _mapMgr.DrawSpriteAtPos (_ammoSprite, px, py);

                if (fracTime >= 0.95f) {
                    _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_X, _target.GetHexCoord (), UnityEngine.Color.red);
                }
            } else {
                _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_X, _target.GetHexCoord (), UnityEngine.Color.red);
            }
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

            if (StaticSettings.IS_SOAKING_COMBAT) {
                dt = 0.15f;
            }

            if (_isRanged) {
                return _elapsedTime >= Mathf.Max (dt, _flightTime);
            } else {
                return _elapsedTime >= dt;
            }
        }

        public void Update (float deltaSeconds)
        {
            _elapsedTime += deltaSeconds;
        }
    }
}
