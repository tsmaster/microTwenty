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
        private bool _isDone;

        public bool InCombat {
            get { return _inCombat; }
            set {
                _inCombat = value;
                if (_inCombat) {
                    Initialize ();
                }
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
            _currentOrder = null;

            var possStartLocs = HexCoord.GetAtRangeFromLoc (4, new HexCoord (0, 0, 0)).FindAll((hc)=>IsLocationWalkable(hc));

            BdgRandom.ShuffleList<HexCoord> (possStartLocs);
            var sl0 = possStartLocs [UnityEngine.Random.Range(0,possStartLocs.Count)];

            var bestDist = -1;
            HexCoord bestLoc = null;
            foreach (var hc in possStartLocs) {
                var d = sl0.DistanceTo (hc);
                if (d > bestDist) {
                    bestDist = d;
                    bestLoc = hc;
                }
            }
            var sl1 = bestLoc;

            var t0Starts = HexCoord.GetWithinRangeFromLoc (2, sl0).FindAll ((hc) => IsLocationWalkable (hc));
            BdgRandom.ShuffleList<HexCoord> (t0Starts);

            AddUnit ("Stan", t0Starts[0], SpriteId.SPRITE_COMBAT_GUY_1, 0);
            AddUnit ("Kim", t0Starts [1], SpriteId.SPRITE_COMBAT_GUY_2, 0);
            AddUnit ("Flexo", t0Starts [2], SpriteId.SPRITE_COMBAT_GUY_3, 0);
            AddUnit ("Mags", t0Starts [3], SpriteId.SPRITE_COMBAT_GUY_4, 0);
            AddUnit ("Torso", t0Starts [4], SpriteId.SPRITE_COMBAT_GUY_5, 0);
            AddUnit ("Belto", t0Starts [5], SpriteId.SPRITE_COMBAT_GUY_6, 0);

            var t1Starts = HexCoord.GetWithinRangeFromLoc (2, sl1).FindAll ((hc) => IsLocationWalkable (hc));
            BdgRandom.ShuffleList<HexCoord> (t1Starts);

            AddUnit ("Snake", t1Starts [0], SpriteId.SPRITE_COMBAT_SNAKE, 1);
            AddUnit ("Dog",   t1Starts [1], SpriteId.SPRITE_COMBAT_DOG, 1);
            AddUnit ("Ratman", t1Starts [2], SpriteId.SPRITE_COMBAT_RAT_MAN, 1);
            AddUnit ("Crab",  t1Starts [3], SpriteId.SPRITE_COMBAT_CRAB, 1);
            AddUnit ("Ghost", t1Starts [4], SpriteId.SPRITE_COMBAT_GHOST, 1);
            AddUnit ("Djinn", t1Starts [5], SpriteId.SPRITE_COMBAT_DJINN, 1);
            AddUnit ("Cat",   t1Starts [6], SpriteId.SPRITE_COMBAT_CAT, 1);

            //AddUnit ("Snake", new HexCoord (-3, 0, 3), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (-3, 3, 0), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (0, -3, 3), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (3, -3, 0), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (0, 3, -3), SpriteId.SPRITE_COMBAT_SNAKE, 1);

            BdgRandom.ShuffleList (units);
            _movingUnit = 0;

            foreach (var unit in units) {
                var hp = UnityEngine.Random.Range (5, 10);
                unit.currentHP = hp;
                unit.maxHP = hp;
            }

            _isDone = false;
        }

        public void Update (float deltaSeconds)
        {
            if (_currentOrder == null) {
                //UnityEngine.Debug.Log ("Generating Order");
                GenerateOrder (_movingUnit);
            }

            _currentOrder.Update (deltaSeconds);
            if (_currentOrder.IsDone ()) {
                _movingUnit = (_movingUnit + 1) % units.Count;
                _currentOrder = null;
            }

            List<int> aliveTeams = GetAliveTeams ();
            if (aliveTeams.Count == 0) {
                UnityEngine.Debug.Log ("Everyone Dead");
                _isDone = true;
            } else if (aliveTeams.Count == 1) {
                UnityEngine.Debug.LogFormat ("Team {0} wins", aliveTeams[0]);
                _isDone = true;
            }
        }

        public bool GetIsDone ()
        {
            return _isDone;
        }

        private List<int> GetAliveTeams ()
        {
            var outList = new List<int> ();
            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                var unitTeam = unit.GetTeamID ();
                if (!outList.Contains (unitTeam)) {
                    outList.Add (unitTeam);
                }
            }
            return outList;
        }

        public void Draw ()
        {
            var center = new HexCoord (0, 0, 0);
            foreach (var hc in HexCoord.GetWithinRange (2)) {
                // draw this hex somehow
            }

            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                if ((_currentOrder != null) &&
                    (_currentOrder.GetCombatUnit() == unit)) {
                    continue;
                }
                //_mapManager.DrawSpriteAtLoc (unit.GetDynamicObjectType (), unit.GetHexCoord (), unit.GetDynamicObject ());
                UnityEngine.Color teamColor = TeamColorUtil.GetColorForTeam (unit.GetTeamID ());
                var unitCoord = unit.GetHexCoord ();
                _mapManager.DrawTintedSpriteAtLocation (unit.GetSpriteId(), unitCoord, teamColor);
                _mapManager.HexCoordToScreenCoords (unitCoord, out int px, out int py);

                var offX = 3;
                var offY = 14;
                TextureDrawing.DrawRect (_mapManager.GetTargetTexture (), px+offX, py+offY, 10, 3, UnityEngine.Color.black, UnityEngine.Color.black, true, false);
                var hpBar = Math.Max((8 * unit.currentHP) / unit.maxHP, 1);
                //UnityEngine.Debug.LogFormat ("drawing hp {0}/{1} = {2}", unit.currentHP, unit.maxHP, hpBar);
                TextureDrawing.DrawRect (_mapManager.GetTargetTexture (), px+offX+1, py+offY+1, hpBar, 1, UnityEngine.Color.green, UnityEngine.Color.black, true, false);
            }

            if (_currentOrder != null) {
                _currentOrder.Draw ();
            }
        }

        private bool IsUnitHealthy (CombatUnit unit)
        {
            return unit.currentHP >= (unit.maxHP / 2);
        }

        private void GenerateOrder (int unitIndex)
        {
            var combatant = units [unitIndex];
            //UnityEngine.Debug.LogFormat ("combatant {0}", combatant.unitName);

            if (!combatant.IsAlive ()) {
                _currentOrder = new PassOrder (combatant);
                return;
            }

            if (IsUnitHealthy (combatant)) {
                GenerateAggressiveOrder (combatant);
            } else {
                GenerateRetreatOrder (combatant);
            }
        }

        private void GenerateAggressiveOrder(CombatUnit combatant) 
        {
            var startCoord = combatant.GetHexCoord ();
            //UnityEngine.Debug.LogFormat ("start coord {0} {1} {2}", startCoord.x, startCoord.y, startCoord.z);

            List<CombatUnit> adjEnemies = GetEnemiesAdjacentTo (startCoord, combatant.GetTeamID());
            if (adjEnemies.Count != 0) {
                var enemyIndex = UnityEngine.Random.Range (0, adjEnemies.Count);
                _currentOrder = new AttackOrder (_mapManager, combatant, adjEnemies [enemyIndex]);
                return;
            }

            var locs = HexCoord.GetAtRangeFromLoc (1, startCoord).FindAll (IsLocationWalkable);
            BdgRandom.ShuffleList<HexCoord> (locs);

            //UnityEngine.Debug.LogFormat ("found {0} locations", locs.Count);

            if (locs.Count == 0) {
                _currentOrder = new PassOrder (combatant);
                return;
            } else {
                var myLoc = combatant.GetHexCoord ();
                var sources = MakeEnemyLocationList (combatant.GetTeamID ());

                if (sources.Count == 0) {
                    // we win?!
                    _currentOrder = new PassOrder (combatant);
                    return;
                }

                var walkableDict = MakeWalkableDict ();
                walkableDict [myLoc] = true;
                var distDict = MakeFloodFill (sources, walkableDict);

                var minDist = -1;
                var bestLoc = locs [0];
                foreach (var testLoc in locs) {
                    if (!distDict.ContainsKey (testLoc)) {
                        continue;
                    } 
                    var testDist = distDict [testLoc];
                    if ((minDist == -1) || (testDist < minDist)) {
                        minDist = testDist;
                        bestLoc = testLoc;
                    }
                }

                if (minDist == -1) {
                    UnityEngine.Debug.Log ("no moves found");
                    _currentOrder = new PassOrder (combatant);
                    return;
                }

                if ((distDict.ContainsKey(myLoc)) &&
                    (minDist >= distDict [myLoc])) {
                    UnityEngine.Debug.Log ("no improvement");
                    _currentOrder = new PassOrder (combatant);
                    return;
                }
                //var locIndex = UnityEngine.Random.Range (0, locs.Count);
                //var destCoord = locs [locIndex];
                var destCoord = bestLoc;
                _currentOrder = new CombatMoveOrder (_mapManager, _mapManager.GetHexMap (), destCoord, combatant);
            }
        }

        private List<CombatUnit> GetEnemiesAdjacentTo (HexCoord startCoord, int myTeamId)
        {
            var outList = new List<CombatUnit> ();

            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                if (unit.GetTeamID () == myTeamId) {
                    continue;
                }
                if (unit.GetHexCoord ().DistanceTo (startCoord) == 1) {
                    outList.Add (unit);
                }
            }
            return outList;
        }

        private void GenerateRetreatOrder (CombatUnit combatant)
        {
            var startCoord = combatant.GetHexCoord ();
            //UnityEngine.Debug.LogFormat ("start coord {0} {1} {2}", startCoord.x, startCoord.y, startCoord.z);

            var locs = HexCoord.GetAtRangeFromLoc (1, startCoord).FindAll (IsLocationWalkable);
            BdgRandom.ShuffleList<HexCoord> (locs);

            //UnityEngine.Debug.LogFormat ("found {0} locations", locs.Count);

            if (locs.Count == 0) {
                _currentOrder = new PassOrder (combatant);
                return;
            } else {
                var myLoc = combatant.GetHexCoord ();
                var sources = MakeEnemyLocationList (combatant.GetTeamID ());
                var walkableDict = MakeWalkableDict ();
                walkableDict [myLoc] = true;
                var distDict = MakeFloodFill (sources, walkableDict);

                var amountToHeal = (combatant.maxHP - combatant.currentHP) * 3 / 4;

                if ((!distDict.ContainsKey(myLoc)) ||
                    (distDict [myLoc] >= amountToHeal)) {
                    // we're probably safe?
                    _currentOrder = new PassOrder (combatant);
                    return;
                }

                var maxDist = distDict [locs [0]];
                var bestLoc = locs [0];
                foreach (var testLoc in locs) {
                    var testDist = distDict [testLoc];
                    if (testDist > maxDist) {
                        maxDist = testDist;
                        bestLoc = testLoc;
                    }
                }

                if (maxDist < distDict [myLoc]) {
                    UnityEngine.Debug.Log ("no improvement");
                    _currentOrder = new PassOrder (combatant);
                    return;
                }
                //var locIndex = UnityEngine.Random.Range (0, locs.Count);
                //var destCoord = locs [locIndex];
                var destCoord = bestLoc;
                _currentOrder = new CombatMoveOrder (_mapManager, _mapManager.GetHexMap (), destCoord, combatant);
            }
        }

        private List<HexCoord> MakeEnemyLocationList (int myTeam)
        {
            List<HexCoord> outList = new List<HexCoord> ();
            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                if (unit.GetTeamID () != myTeam) {
                    outList.Add (unit.GetHexCoord ());
                }
            }
            return outList;
        }

        private Dictionary<HexCoord, bool> MakeWalkableDict ()
        {
            var outDict = new Dictionary<HexCoord, bool> ();

            foreach (var hexTile in _mapManager.GetTiles()) {
                var hc = hexTile.HexCoord;
                outDict [hc] = IsLocationWalkable (hc);
            }

            return outDict;
        }

        private bool IsLocationWalkable (HexCoord hc)
        {
            foreach (var hexTile in _mapManager.GetTilesAt (hc)) {
                if (!hexTile.CanMove) {
                    return false;
                }
            }

            foreach (var dynObj in _mapManager.GetDynamicObjectsAt (hc)) {
                if (dynObj.blocksMovement) {
                    return false;
                }
            }

            foreach (var combatUnit in units) {
                if (!combatUnit.IsAlive ()) {
                    continue;
                }

                if (combatUnit.GetHexCoord ().Equals(hc)) {
                    // TODO make use of dynamic objects features instead?
                    return false;
                }
            }

            return true;
        }

        private Dictionary<HexCoord, int> MakeFloodFill (List<HexCoord> sources, Dictionary<HexCoord, bool> walkableDict)
        {
            Dictionary<HexCoord, int> distances = new Dictionary<HexCoord, int> ();
            foreach (var sourceCoord in sources) {
                distances [sourceCoord] = 0;
            }

            List<HexCoord> openList = new List<HexCoord> (sources);
            while (openList.Count > 0) {
                var node = openList [0];
                openList.RemoveAt (0);
                var curDist = distances [node];

                var newDist = curDist + 1;
                foreach (var neighbor in HexCoord.GetAtRangeFromLoc (1, node)) {
                    if ((!walkableDict.ContainsKey (neighbor)) ||
                        (!walkableDict [neighbor])) {
                        continue;
                    }
                    if ((!distances.ContainsKey (neighbor)) ||
                        (distances [neighbor] > newDist)) {
                        distances [neighbor] = newDist;
                        //UnityEngine.Debug.LogFormat ("Flood filling loc {0} to dist {1}", neighbor, newDist);
                        openList.Add (neighbor);
                    }
                }
            }
            return distances;
        }
    }
}
