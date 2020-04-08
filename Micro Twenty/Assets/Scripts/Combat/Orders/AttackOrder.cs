using System;

namespace MicroTwenty
{
    public class AttackOrder : CombatOrder
    {
        private CombatUnit _combatant;
        private CombatUnit _target;
        private readonly MapManager _mapMgr;
        private float _elapsedTime;
        private const float DISPLAY_TIME = 0.314f;

        public AttackOrder (MapManager mapMgr, CombatUnit combatant, CombatUnit target)
        {
            _combatant = combatant;
            _target = target;
            _mapMgr = mapMgr;

            _elapsedTime = 0.0f;

            _target.currentHP -= 1;
            UnityEngine.Debug.LogFormat ("{0} hits {1}, new HP {2}/{3}", _combatant.unitName, _target.unitName, _target.currentHP, _target.maxHP);
        }

        public void Draw ()
        {
            _mapMgr.DrawTintedSpriteAtLocation (_combatant.GetSpriteId (), _combatant.GetHexCoord(), TeamColorUtil.GetColorForTeam (_combatant.GetTeamID ()));
            _mapMgr.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR, _target.GetHexCoord (), UnityEngine.Color.yellow);
        }

        public CombatUnit GetCombatUnit ()
        {
            return _combatant;
        }

        public bool IsDone ()
        {
            var dt = DISPLAY_TIME;
            if (UnityEngine.Input.GetKey (UnityEngine.KeyCode.LeftShift)) {
                dt = 0.09f;
            }

            return _elapsedTime >= dt;
        }

        public void Update (float deltaSeconds)
        {
            _elapsedTime += deltaSeconds;
        }
    }
}
