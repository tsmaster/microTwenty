using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class ParticleOrder : CombatOrder
    {
        private CombatUnit _caster;
        private CombatUnit _target;
        private readonly MapManager _mapMgr;
        private float _elapsedTime;
        private int _range;

        private SpriteId _particleSprite;
        private const float PARTICLE_SPEED = 4.0f;

        private float _flightTime;
        private Color _particleColor;
        private Action<CombatUnit, CombatUnit> _hitAction; // caster, target
        private bool _hasDoneAction;

        public ParticleOrder (MapManager mapMgr, 
            CombatUnit caster, CombatUnit target, 
            SpriteId particleSprite, Color particleColor,
            Action<CombatUnit, CombatUnit> hitAction)
        {
            _caster = caster;
            _target = target;
            _mapMgr = mapMgr;

            _elapsedTime = 0.0f;

            _range = caster.GetHexCoord ().DistanceTo (target.GetHexCoord ());

            mapMgr.GetCombatMgr ().AddLogLineFormat ("{0} attacks {1} with {2}",
                _caster.unitName, _target.unitName, caster.weapon.Name);

            _flightTime = _range / PARTICLE_SPEED;
            Debug.LogFormat ("Flight time: {0} seconds", _flightTime);

            _particleSprite = particleSprite;
            _particleColor = particleColor;

            _hitAction = hitAction;
            _hasDoneAction = false;
        }


        public void Draw ()
        {
            _mapMgr.DrawTintedSpriteAtLocation (_caster.GetSpriteId (), _caster.GetHexCoord(), TeamColorUtil.GetColorForTeam (_caster.GetTeamID ()));
            _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_CIRCLE, _caster.GetHexCoord (), UnityEngine.Color.blue);
            var fracTime = _elapsedTime / _flightTime;

            var attackerPos = _caster.GetHexCoord ();
            var targetPos = _target.GetHexCoord ();

            _mapMgr.HexCoordToScreenCoords (attackerPos, out int attScrX, out int attScrY);
            _mapMgr.HexCoordToScreenCoords (targetPos, out int tarScrX, out int tarScrY);

            int px = (int)BdgMath.Map (fracTime, 0.0f, 1.0f, attScrX, tarScrX);
            int py = (int)BdgMath.Map (fracTime, 0.0f, 1.0f, attScrY, tarScrY);

            _mapMgr.DrawTintedSpriteAtPos (_particleSprite, px, py, _particleColor);

            if (fracTime >= 0.95f) {
                _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_X, _target.GetHexCoord (), UnityEngine.Color.red);
            }
        }

        public CombatUnit GetCombatUnit ()
        {
            return _caster;
        }

        public bool IsDone ()
        {
            return _elapsedTime >= _flightTime;
        }

        public void Update (float deltaSeconds)
        {
            _elapsedTime += deltaSeconds;

            var fracTime = _elapsedTime / (float)deltaSeconds;
            if (fracTime >= 0.95f) {
                if (!_hasDoneAction) {
                    _hitAction (_caster, _target);
                    _hasDoneAction = true;
                }
            }
        }
    }
}
