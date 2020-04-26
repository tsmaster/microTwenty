using System;
namespace MicroTwenty
{
    public class CombatMoveOrder : CombatOrder
    {
        float elapsedTime;
        private MapManager _mapMgr;
        private HexMap _map;
        private HexCoord _startCoord;
        private HexCoord _destination;
        private CombatUnit _combatant;

        private int _screenPosX;
        private int _screenPosY;

        const float JUMP_HEIGHT = 12.0f;
        const float MOVE_DURATION = 0.8f;

        private const float _blinkDuration = 0.314f;

        public CombatMoveOrder (MapManager mapManager, HexMap map, HexCoord destination, CombatUnit combatant)
        {
            _mapMgr = mapManager;
            _map = map;
            _startCoord = combatant.GetHexCoord();
            _destination = destination;
            _combatant = combatant;
            elapsedTime = 0;
            _combatant.SetHexCoord (_destination);

            _mapMgr.HexCoordToScreenCoords (_startCoord, out _screenPosX, out _screenPosY);
        }

        public bool IsDone ()
        {
            var md = MOVE_DURATION;

            // HACK for debugging
            md = 0.01f;

            if (UnityEngine.Input.GetKey (UnityEngine.KeyCode.LeftShift)) {
                md = 0.05f;
            }

            return elapsedTime >= md;
        }

        public void Update (float deltaSeconds)
        {
            elapsedTime += deltaSeconds;

            EvalJumpPos (_startCoord, _destination, MOVE_DURATION, out _screenPosX, out _screenPosY);
        }

        public void Draw ()
        {
            //_mapMgr.DrawSpriteAtPos (_combatant.GetSpriteId(), _screenPosX, _screenPosY);
            UnityEngine.Color teamColor = TeamColorUtil.GetColorForTeam (_combatant.GetTeamID ());
            var blinkColor = ModulateBlink (teamColor, UnityEngine.Color.white, elapsedTime, _blinkDuration);

            _mapMgr.DrawTintedSpriteAtPos(_combatant.GetSpriteId (), _screenPosX, _screenPosY, blinkColor);
        }

        private UnityEngine.Color ModulateBlink (UnityEngine.Color c1, UnityEngine.Color c2, float time, float blinkPeriod)
        {
            float normTime = time / blinkPeriod;
            float twoPiTime = (float) (normTime * 2 * Math.PI);

            float sVal = (float)Math.Sin (twoPiTime);

            var mRed = BdgMath.Map (sVal, -1, 1, c1.r, c2.r);
            var mGreen = BdgMath.Map (sVal, -1, 1, c1.g, c2.g);
            var mBlue = BdgMath.Map (sVal, -1, 1, c1.b, c2.b);

            return new UnityEngine.Color (mRed, mGreen, mBlue);
        }


        private float EvalJumpHeight (float jumpFrac)
        {
            return 4.0f * jumpFrac * (1.0f - jumpFrac);
        }

        private void EvalJumpPos (HexCoord startCoord, HexCoord endCoord, float jumpDuration, out int px, out int py)
        {
            _mapMgr.HexCoordToScreenCoords (startCoord, out int spx, out int spy);
            _mapMgr.HexCoordToScreenCoords (endCoord, out int epx, out int epy);
            float jumpFrac = elapsedTime / jumpDuration;

            px = (int)BdgMath.Map (jumpFrac, 0.0f, 1.0f, spx, epx);
            py = (int)(BdgMath.Map (jumpFrac, 0.0f, 1.0f, spy, epy) + EvalJumpHeight (jumpFrac) * JUMP_HEIGHT);
        }

        CombatUnit CombatOrder.GetCombatUnit()
        {
            return _combatant;
        }
    }
}
