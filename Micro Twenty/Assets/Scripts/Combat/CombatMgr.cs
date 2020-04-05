using System;
using System.Collections.Generic;

namespace MicroTwenty
{
    public class CombatMgr
    {
        private MapManager _mapManager;
        private GameMgr _gameMgr;
        List<CombatUnit> units;

        CombatOrder _currentOrder;

        private bool _inCombat = false;
        private int _movingUnit;

        public bool InCombat {
            get { return _inCombat; }
            set {
                _inCombat = value;
                Initialize ();
            }
        }

        public CombatMgr (MapManager mapManager, GameMgr gameMgr)
        {
            _mapManager = mapManager;
            _gameMgr = gameMgr;
            units = new List<CombatUnit> ();
            _currentOrder = null;
        }

        private void AddUnit (string name, HexCoord hexCoord, SpriteId spriteId, int teamId) {
            var unit = new CombatUnit (name, hexCoord, teamId, new CombatantSprite(_gameMgr, hexCoord, spriteId, teamId));
            units.Add (unit);
        }

        public void Initialize ()
        {
            units.Clear ();
            AddUnit ("Stan", new HexCoord (0, 0, 0), SpriteId.SPRITE_COMBAT_GUY_1, 0);
            AddUnit ("Kim", new HexCoord (1, 0, -1), SpriteId.SPRITE_COMBAT_GUY_2, 0);
            AddUnit ("Flexo", new HexCoord (-1, 1, 0), SpriteId.SPRITE_COMBAT_GUY_3, 0);
            AddUnit ("Mags", new HexCoord (0, -1, 1), SpriteId.SPRITE_COMBAT_GUY_4, 0);
            AddUnit ("Torso", new HexCoord (1, -1, 0), SpriteId.SPRITE_COMBAT_GUY_5, 0);
            AddUnit ("Belto", new HexCoord (0, 1, -1), SpriteId.SPRITE_COMBAT_GUY_6, 0);

            AddUnit ("Snake", new HexCoord (3, 0, -3), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            AddUnit ("Dog", new HexCoord (4, 0, -4), SpriteId.SPRITE_COMBAT_DOG, 1);
            AddUnit ("Ratman", new HexCoord (2, 1, -3), SpriteId.SPRITE_COMBAT_RAT_MAN, 1);
            AddUnit ("Crab", new HexCoord (3, -1, -2), SpriteId.SPRITE_COMBAT_CRAB, 1);
            AddUnit ("Ghost", new HexCoord (4, -1, -3), SpriteId.SPRITE_COMBAT_GHOST, 1);
            AddUnit ("Djinn", new HexCoord (3, 1, -4), SpriteId.SPRITE_COMBAT_DJINN, 1);
            AddUnit ("Cat", new HexCoord (2, 0, -2), SpriteId.SPRITE_COMBAT_CAT, 1);

            BdgRandom.ShuffleList (units);
            _movingUnit = 0;
        }

        public void Update (float deltaSeconds)
        {
            if (_currentOrder == null) {
                UnityEngine.Debug.Log ("Generating Order");
                GenerateOrder (_movingUnit);
            }

            _currentOrder.Update (deltaSeconds);
            if (_currentOrder.IsDone ()) {
                _movingUnit = (_movingUnit + 1) % units.Count;
                _currentOrder = null;
            }
        }

        public void Draw ()
        {
            var center = new HexCoord (0, 0, 0);
            foreach (var hc in HexCoord.GetWithinRange (2)) {
                // draw this hex somehow
            }

            foreach (var unit in units) {
                if ((_currentOrder != null) &&
                    (_currentOrder.GetCombatUnit() == unit)) {
                    continue;
                }
                //_mapManager.DrawSpriteAtLoc (unit.GetDynamicObjectType (), unit.GetHexCoord (), unit.GetDynamicObject ());
                UnityEngine.Color teamColor = TeamColorUtil.GetColorForTeam (unit.GetTeamID ());
                _mapManager.DrawTintedSpriteAtLocation (unit.GetSpriteId(), unit.GetHexCoord (), teamColor);
            }

            if (_currentOrder != null) {
                _currentOrder.Draw ();
            }
        }

        private void GenerateOrder (int unitIndex)
        {
            var combatant = units [unitIndex];
            UnityEngine.Debug.LogFormat ("combatant {0}", combatant.unitName);

            var startCoord = combatant.GetHexCoord ();
            //UnityEngine.Debug.LogFormat ("start coord {0} {1} {2}", startCoord.x, startCoord.y, startCoord.z);

            var locs = HexCoord.GetAtRange (1);
            var locIndex = UnityEngine.Random.Range (0, locs.Count);
            var destCoord = locs[locIndex].Add(startCoord);
            //UnityEngine.Debug.LogFormat ("end coord {0} {1} {2}", destCoord.x, destCoord.y, destCoord.z);

            _currentOrder = new CombatMoveOrder (_mapManager, _mapManager.GetHexMap(), destCoord, combatant);
        }
    }
}
