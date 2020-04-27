using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class CombatMgr
    {
        private MapManager _mapManager;
        private GameMgr _gameMgr;
        List<CombatUnit> units;

        CombatOrder _currentOrder;

        private bool _inCombat = false;
        private bool _isDone;
        private MiniMax _minimax;
        private int _currentTurnNumber;

        private List<string> _logLines;

        private MenuObject _combatMenu;

        public bool InCombat {
            get { return _inCombat; }
            set {
                _inCombat = value;
                if (_inCombat) {
                    Initialize ();
                }
            }
        }

        private enum CombatPhase
        {
            OUT_OF_COMBAT,
            PRE_COMBAT,
            IN_COMBAT,
            POST_COMBAT,
            COMBAT_DONE
        };

        private enum OrderUiPhase
        {
            INVALID,
            SHOWING_MENU,
            CURSOR,
            EXECUTING
        };

        private enum OrderMenuItemId
        {
            INVALID,
            MOVE,
            MELEE,
            RANGE,
            MAGIC,
            ITEM,
            DEFEND
        };

        private CombatPhase _combatPhase;

        Dictionary<int, int> _winTally;

        private string _preCombatString;
        private string _postCombatString;
        private float _preBattleClock;
        private float _postBattleClock;
        private OrderUiPhase _orderUiPhase;
        private bool _showingCombatMenu;
        private MenuManager _menuMgr;
        private Texture2D _targetTexture;
        private SelectionMode _selector;

        public Dictionary<int, int> WinTally { 
            get { return _winTally; }
            private set {
                _winTally = value;
            } 
        }

        private const float PRE_BATTLE_DURATION = 0.4f;
        private const float POST_BATTLE_DURATION = 0.4f;

        public const bool MINIMAX_AI = true;
        private const int LOG_LINE_MAX_LENGTH = 32;
        private const int LOG_LINE_MAX_LINES = 6;

        public CombatMgr (MapManager mapManager, GameMgr gameMgr)
        {
            _mapManager = mapManager;
            _gameMgr = gameMgr;
            units = new List<CombatUnit> ();
            _currentOrder = null;
            _currentTurnNumber = 0;

            _winTally = new Dictionary<int, int> {
                [0] = 0,
                [1] = 0
            };

            _logLines = new List<string> ();


            var menuBitmap = mapManager.GetMenuBitmap ();
            var fontBitmap = mapManager.GetFontBitmap ();
            _targetTexture = mapManager.GetTargetTexture ();

            _menuMgr = new MenuManager (menuBitmap, fontBitmap);
            _combatMenu = new MenuObject ("combat", menuBitmap, fontBitmap);
            _combatMenu.SetWindow (1, 4);
            _combatMenu.AddItem ("Move").SetItemId((int)OrderMenuItemId.MOVE);
            _combatMenu.AddItem ("Melee Attack").SetItemId ((int)OrderMenuItemId.MELEE);
            _combatMenu.AddItem ("Ranged Attack").SetEnabled(false).SetItemId ((int)OrderMenuItemId.RANGE);
            _combatMenu.AddItem ("Cast Magic").SetEnabled(false).SetItemId ((int)OrderMenuItemId.MAGIC);
            _combatMenu.AddItem ("Use Item").SetEnabled (false).SetItemId ((int)OrderMenuItemId.ITEM);
            _combatMenu.AddItem ("Defend").SetItemId ((int)OrderMenuItemId.DEFEND);

            _combatMenu.Build ();

            _orderUiPhase = OrderUiPhase.INVALID;

            _selector = null;
        }

        internal void AddLogLineFormat (string templ, params object [] parList)
        {
            string msg = string.Format (templ, parList);
            AddLogLine (msg);
        }

        public void AddLogLine (string msg)
        {
            Debug.Log (msg);
            while (msg.Length > 0) {
                var clippedMsg = "";
                if (msg.Length <= LOG_LINE_MAX_LENGTH) {
                    clippedMsg = msg;
                    msg = "";
                } else {
                    clippedMsg = msg.Substring (0, LOG_LINE_MAX_LENGTH);
                    msg = "  " + msg.Substring (LOG_LINE_MAX_LENGTH);
                }
                _logLines.Add (clippedMsg);
            }

            if (_logLines.Count > LOG_LINE_MAX_LINES) {
                var numToRemove = _logLines.Count - LOG_LINE_MAX_LINES;
                _logLines.RemoveRange (0, numToRemove);
            }
        }

        private CombatUnit AddUnit (string name, HexCoord hexCoord, SpriteId spriteId, int teamId, int maxMove) {
            var unit = new CombatUnit (name, hexCoord, teamId, new CombatantSprite(_gameMgr, hexCoord, spriteId, teamId), maxMove);
            units.Add (unit);
            return unit;
        }

        public void Initialize ()
        {
            units.Clear ();
            _currentOrder = null;

            var possStartLocs = HexCoord.GetAtRangeFromLoc (4, new HexCoord (0, 0, 0)).FindAll(IsLocationWalkable);

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

            var t0Starts = HexCoord.GetWithinRangeFromLoc (2, sl0).FindAll (IsLocationWalkable);
            BdgRandom.ShuffleList<HexCoord> (t0Starts);

            AddUnit ("Stan", t0Starts[0], SpriteId.SPRITE_COMBAT_GUY_1, 0, 3).AddWeapon(WeaponRep.MakeSword()).AddArmor(ArmorRep.MakeLeatherArmor());
            AddUnit ("Kim", t0Starts [1], SpriteId.SPRITE_COMBAT_GUY_2, 0, 3).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakeLeatherArmor ());
            AddUnit ("Flexo", t0Starts [2], SpriteId.SPRITE_COMBAT_GUY_3, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakeLeatherArmor ());
            AddUnit ("Mags", t0Starts [3], SpriteId.SPRITE_COMBAT_GUY_4, 0, 3).AddWeapon (WeaponRep.MakeStaff ()).AddArmor (ArmorRep.MakeClothArmor ());
            AddUnit ("Torso", t0Starts [4], SpriteId.SPRITE_COMBAT_GUY_5, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakePlateArmor ());
            AddUnit ("Belto", t0Starts [5], SpriteId.SPRITE_COMBAT_GUY_6, 0, 2).AddWeapon (WeaponRep.MakeSword ()).AddArmor (ArmorRep.MakeChainArmor ());

            var t1Starts = HexCoord.GetWithinRangeFromLoc (2, sl1).FindAll (IsLocationWalkable);
            BdgRandom.ShuffleList<HexCoord> (t1Starts);

            AddUnit ("Snake", t1Starts [0], SpriteId.SPRITE_COMBAT_SNAKE, 1, 3).AddWeapon(WeaponRep.MakeBiteWeapon ());
            AddUnit ("Dog",   t1Starts [1], SpriteId.SPRITE_COMBAT_DOG, 1, 3).AddWeapon (WeaponRep.MakeBiteWeapon ());
            AddUnit ("Ratman", t1Starts [2], SpriteId.SPRITE_COMBAT_RAT_MAN, 1, 3).AddWeapon (WeaponRep.MakeSword ());
            AddUnit ("Crab",  t1Starts [3], SpriteId.SPRITE_COMBAT_CRAB, 1, 2).AddWeapon (WeaponRep.MakeBiteWeapon ()); 
            AddUnit ("Ghost", t1Starts [4], SpriteId.SPRITE_COMBAT_GHOST, 1, 2).AddWeapon (WeaponRep.MakePsiWeapon ()); 
            AddUnit ("Djinn", t1Starts [5], SpriteId.SPRITE_COMBAT_DJINN, 1, 2).AddWeapon (WeaponRep.MakePsiWeapon ());
            AddUnit ("Cat",   t1Starts [6], SpriteId.SPRITE_COMBAT_CAT, 1, 3).AddWeapon (WeaponRep.MakeBiteWeapon ());
            AddUnit ("Skeleton", t1Starts [7], SpriteId.SPRITE_COMBAT_SKELETON, 1, 3).AddWeapon (WeaponRep.MakeSword ()); 
            AddUnit ("Staff", t1Starts [8], SpriteId.SPRITE_COMBAT_STAFF, 1, 3).AddWeapon (WeaponRep.MakeStaff ());
            AddUnit ("Bug", t1Starts [9], SpriteId.SPRITE_COMBAT_BUG, 1, 3).AddWeapon (WeaponRep.MakeBiteWeapon ());

            //AddUnit ("Snake", new HexCoord (-3, 0, 3), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (-3, 3, 0), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (0, -3, 3), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (3, -3, 0), SpriteId.SPRITE_COMBAT_SNAKE, 1);
            //AddUnit ("Snake", new HexCoord (0, 3, -3), SpriteId.SPRITE_COMBAT_SNAKE, 1);

            BdgRandom.ShuffleList (units);

            // HACK HACK HACK for soak test
            var count = UnityEngine.Random.Range (2, units.Count);
            units = units.GetRange (0, count);


            int monsterCount = 0;

            for (int i = 0; i < units.Count; ++i) {
                units [i].initiative = 10 - i;
                if (units [i].GetTeamID () == 1) {
                    ++monsterCount;
                }
            }

            _preCombatString = string.Format ("You face {0} monsters.", monsterCount);
            _logLines.Clear ();
            AddLogLine ("Combat Starts");

            foreach (var unit in units) {
                var hp = UnityEngine.Random.Range (5, 10);
                unit.currentHP = hp;
                unit.maxHP = hp;
            }

            _isDone = false;
            _combatPhase = CombatPhase.PRE_COMBAT;
            _preBattleClock = 0.0f;
            _postBattleClock = 0.0f;
        }

        public void Update (float deltaSeconds) {
            switch (_combatPhase) {
            case CombatPhase.PRE_COMBAT:
                UpdatePreBattle (deltaSeconds);
                return;
            case CombatPhase.POST_COMBAT:
                UpdatePostBattle (deltaSeconds);
                return;
            }

            List<int> aliveTeams = GetAliveTeams ();
            if (aliveTeams.Count == 0) {
                UnityEngine.Debug.Log ("Everyone Dead");
                _postCombatString = "Everyone has died, Hamlet.";
                _combatPhase = CombatPhase.POST_COMBAT;
                return;
            } else if (aliveTeams.Count == 1) {
                UnityEngine.Debug.LogFormat ("Team {0} wins", aliveTeams [0]);
                if (aliveTeams [0] == 0) {
                    _postCombatString = "You Win!";
                } else {
                    _postCombatString = "You have been defeated";
                }
                _combatPhase = CombatPhase.POST_COMBAT;
                _winTally [aliveTeams [0]]++;
                return;
            }

            int nextUnitIndex = GetNextUnitIndex ();

            int team = units [nextUnitIndex].GetTeamID ();

            if (IsTeamAi (team)) {
                if (_currentOrder == null) {
                    GenerateOrder (deltaSeconds);
                }

                if (_currentOrder == null) {
                    // waiting for order
                    return;
                }
            } else {
                switch (_orderUiPhase) {
                case OrderUiPhase.INVALID:
                    _orderUiPhase = OrderUiPhase.SHOWING_MENU;
                    _showingCombatMenu = true;
                    _menuMgr.OpenMenu (_combatMenu);
                    break;
                case OrderUiPhase.SHOWING_MENU:
                    UpdateCombatMenu ();
                    // selections from the order menu (e.g. "move" "attack") go
                    // through a callback that sets the phase and tears down the menu.
                    break;
                case OrderUiPhase.CURSOR:
                    UpdateCursor ();
                    // selections from the cursor menu (e.g. move here, cancel) go
                    // through a callback that generates the _currentOrder
                    break;
                case OrderUiPhase.EXECUTING:
                    // do nothing
                    break;
                }
            }

            if (_currentOrder != null) {
                _currentOrder.GetCombatUnit ().SetLastTurnMoved (_currentTurnNumber);

                _currentOrder.Update (deltaSeconds);
                if (_currentOrder.IsDone ()) {
                    // order is done
                    _orderUiPhase = OrderUiPhase.INVALID;
                    MaybeAdvanceCurrentTurn ();
                    _currentOrder = null;
                }
            }
        }

        private bool IsTeamAi (int teamIndex)
        {
            return (teamIndex == 1);
        }

        private void UpdateCursor ()
        {
            // move cursor
            if (Input.GetKeyDown (KeyCode.W)) {
                // NW
                _selector.CursorPos = _selector.CursorPos.Add(new HexCoord (0, 1, -1));
            }
            if (Input.GetKeyDown (KeyCode.E)) {
                // NE
                _selector.CursorPos = _selector.CursorPos.Add (new HexCoord (1, 0, -1));
            }
            if (Input.GetKeyDown (KeyCode.A)) {
                // W
                _selector.CursorPos = _selector.CursorPos.Add (new HexCoord (-1, 1, 0));
            }
            if (Input.GetKeyDown (KeyCode.D)) {
                // E
                _selector.CursorPos = _selector.CursorPos.Add (new HexCoord (1, -1, 0));
            }
            if (Input.GetKeyDown (KeyCode.Z)) {
                // SW
                _selector.CursorPos = _selector.CursorPos.Add (new HexCoord (-1, 0, 1));
            }
            if (Input.GetKeyDown (KeyCode.X)) {
                // SE
                _selector.CursorPos = _selector.CursorPos.Add (new HexCoord (0, -1, 1));
            }

            // if selected a valid unit, callback
            if ((Input.GetKeyDown (KeyCode.Return)) ||
                (Input.GetKeyDown(KeyCode.Space))) {
                if (_selector.IsLocationSelectable (_selector.CursorPos)) {
                    _selector.OnLocationSelected (_selector.CursorPos);
                }
            }

            // if cancel, have to unwind
            if (Input.GetKeyDown (KeyCode.Escape)) {
                _selector = null;
                _orderUiPhase = OrderUiPhase.SHOWING_MENU;
                _menuMgr.OpenMenu (_combatMenu);
            }
        }

        private void UpdateCombatMenu ()
        {
            // TODO this doesn't seem like a thing we want to rewrite each place we use a menu
            if (Input.GetKeyDown (KeyCode.DownArrow)) {
                _menuMgr.OnDown ();
            }
            if (Input.GetKeyDown (KeyCode.UpArrow)) {
                _menuMgr.OnUp ();
            }
            if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                _menuMgr.OnLeft ();
            }
            if (Input.GetKeyDown (KeyCode.RightArrow)) {
                _menuMgr.OnRight ();
            }
            if (Input.GetKeyDown (KeyCode.Return)) {
                var res = _menuMgr.OnActivate ();

                switch (res.GetItemId ()) {
                case (int)OrderMenuItemId.MOVE:
                    // move selected

                    var nextCharIndex = GetNextUnitIndex ();
                    var nextChar = units [nextCharIndex];

                    var myLoc = nextChar.GetHexCoord ();
                    var walkableDict = MakeWalkableDict ();
                    walkableDict [myLoc] = true;

                    var sources = new List<HexCoord> {
                        myLoc };

                    Dictionary<HexCoord, int> ff = MakeFloodFill (sources, walkableDict);
                    _selector = new MovementSelectionMode (nextCharIndex, _mapManager, this, ff);
                    _selector.OnLocationSelected += OnSelectedMoveDestination;
                    _orderUiPhase = OrderUiPhase.CURSOR;
                    break;
                case (int)OrderMenuItemId.RANGE:
                    // ranged attack selected
                    // TODO select ranged target
                    break;
                case (int)OrderMenuItemId.MELEE:
                    // melee attack selected
                    _selector = new MeleeTargetSelectionMode (GetNextUnitIndex(), _mapManager, this);
                    _selector.OnLocationSelected += OnSelectedMeleeTarget;
                    _orderUiPhase = OrderUiPhase.CURSOR;
                    break;
                case (int)OrderMenuItemId.MAGIC:
                    // magic selected
                    break;
                case (int)OrderMenuItemId.ITEM:
                    // use item selected
                    break;
                case (int)OrderMenuItemId.DEFEND:
                    // defend selected
                    _currentOrder = new PassOrder (units [GetNextUnitIndex ()]);
                    break;
                default:
                    // unknown action selected
                    break;
                }
            }
            if (Input.GetKeyDown (KeyCode.Escape)) {
                // maybe don't allow this?
                _menuMgr.OnBack ();
            }
        }

        private void OnSelectedMeleeTarget (HexCoord hc)
        {
            var nextUnitIndex = GetNextUnitIndex ();
            var attacker = units [nextUnitIndex];
            var target = GetCombatUnitByLocation (hc);
            _currentOrder = new AttackOrder (_mapManager, attacker, target);
        }

        private CombatUnit GetCombatUnitByLocation (HexCoord hc)
        {
            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                if (unit.GetHexCoord ().Equals (hc)) {
                    return unit;
                }
            }
            return null;
        }

        private void OnSelectedMoveDestination (HexCoord hc)
        {
            var nextUnitIndex = GetNextUnitIndex ();
            var unit = units [nextUnitIndex];
            _currentOrder = new CombatMoveOrder (_mapManager, _mapManager.GetHexMap (), hc, unit);
        }

        private void MaybeAdvanceCurrentTurn ()
        {
            foreach (var unit in units) {
                if ((unit.IsAlive()) &&
                    (unit.GetLastTurnMoved () < _currentTurnNumber)) {
                    return;
                }
            }
            ++_currentTurnNumber;
            var turnMsg = string.Format("Combat Turn Number {0}", _currentTurnNumber);
            Debug.Log (turnMsg);
            AddLogLine (turnMsg);
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

            foreach (var unit in units) {
                if (!unit.IsAlive ()) {
                    continue;
                }
                if ((_currentOrder != null) &&
                    (_currentOrder.GetCombatUnit() == unit)) {
                    continue;
                }
                UnityEngine.Color teamColor = TeamColorUtil.GetColorForTeam (unit.GetTeamID ());
                var unitCoord = unit.GetHexCoord ();
                _mapManager.DrawTintedSpriteAtLocation (unit.GetSpriteId(), unitCoord, teamColor);
                _mapManager.HexCoordToScreenCoords (unitCoord, out int px, out int py);

                var offX = 3;
                var offY = 14;
                TextureDrawing.DrawRect (_mapManager.GetTargetTexture (), px+offX, py+offY, 10, 3, UnityEngine.Color.black, UnityEngine.Color.black, true, false);
                var hpBar = Math.Max((8 * unit.currentHP) / unit.maxHP, 1);
                TextureDrawing.DrawRect (_mapManager.GetTargetTexture (), px+offX+1, py+offY+1, hpBar, 1, UnityEngine.Color.green, UnityEngine.Color.black, true, false);
            }

            if (_currentOrder != null) {
                _currentOrder.Draw ();
            }

            if (_combatPhase == CombatPhase.PRE_COMBAT) {
                DrawPreBattle ();
            } else if (_combatPhase == CombatPhase.POST_COMBAT) {
                DrawPostBattle ();
            } else {
                DrawLog ();

                var movingUnitIndex = GetNextUnitIndex ();
                var movingUnit = units [movingUnitIndex];
                var movingUnitLoc = movingUnit.GetHexCoord ();

                _mapManager.DrawTintedSpriteAtLocation (SpriteId.SPRITE_TILE_CURSOR_CIRCLE, movingUnitLoc, Color.white);

                switch (_orderUiPhase) {
                case OrderUiPhase.SHOWING_MENU:
                    _menuMgr.Draw (_targetTexture, 10, 185);
                    break;
                case OrderUiPhase.CURSOR:
                    DrawSelector ();
                    break;
                }
                _targetTexture.Apply ();
            }
        }

        private void DrawSelector ()
        {
            var cursorLoc = _selector.CursorPos;
            var sprite = _selector.GetCursorSpriteIdForLoc (cursorLoc, out Color spriteColor);

            _mapManager.DrawTintedSpriteAtLocation (sprite, cursorLoc, spriteColor);
        }

        private void DrawPreBattle ()
        {
            DrawCenteredStringInBox (_preCombatString);
        }

        private void DrawCenteredStringInBox (string msg)
        {
            var targetTexture = _mapManager.GetTargetTexture ();

            var targetTextureHeight = targetTexture.height;
            var targetTextureWidth = targetTexture.width;

            var msgWidth = (msg.Length + 1) * 6;
            var msgHeight = 10;

            var msgPosX = targetTextureHeight / 2 - msgWidth / 2;
            var msgPosY = targetTextureWidth / 2 - msgHeight / 2;

            var fontBitmap = _mapManager.GetFontBitmap ();

            TextureDrawing.DrawRect (targetTexture, msgPosX - 2, msgPosY - 1, msgWidth, msgHeight, Color.black, Color.green, true, true);
            TextureDrawing.DrawStringAt (targetTexture, fontBitmap, msg, msgPosX, msgPosY, Color.white);
        }

        private void DrawLog ()
        {
            var targetTexture = _mapManager.GetTargetTexture ();

            var targetTextureHeight = targetTexture.height;
            var targetTextureWidth = targetTexture.width;

            var msgWidth = LOG_LINE_MAX_LENGTH * 6 + 4;
            var msgHeight = LOG_LINE_MAX_LINES * 8 + 4;

            var msgPosX = targetTextureHeight - msgWidth;
            var msgPosY = 0;

            var fontBitmap = _mapManager.GetFontBitmap ();

            var bgColor = new Color (0, 0, 0, 0.5f);

            TextureDrawing.DrawRect (targetTexture, msgPosX - 2, msgPosY - 1, msgWidth, msgHeight, bgColor, Color.yellow, true, true);

            // draw lines of the log here
            for (int logIndex = _logLines.Count - 1; logIndex >= 0; --logIndex) {
                var logMsg = _logLines [logIndex];

                var linesUp = _logLines.Count - 1 - logIndex;

                TextureDrawing.DrawStringAt (targetTexture, fontBitmap, logMsg, msgPosX + 2, msgPosY + linesUp * 8 + 2, Color.white);
            }
        }

        private void DrawPostBattle ()
        {
            DrawCenteredStringInBox (_postCombatString);
        }

        private void UpdatePreBattle (float deltaSeconds)
        {
            _preBattleClock += deltaSeconds;
            if (_preBattleClock >= PRE_BATTLE_DURATION) {
                _combatPhase = CombatPhase.IN_COMBAT;
            }
        }

        private void UpdatePostBattle (float deltaSeconds)
        {
            _postBattleClock += deltaSeconds;
            if (_postBattleClock >= POST_BATTLE_DURATION) {
                _combatPhase = CombatPhase.COMBAT_DONE;
                _isDone = true;
            }
        }

        private bool IsUnitHealthy (CombatUnit unit)
        {
            return unit.currentHP >= (unit.maxHP / 2);
        }

        private int GetNextUnitIndex ()
        {
            int nextUnitIndex = -1;
            int highestInitiative = -1;

            for (int i = 0; i < units.Count; ++i) {
                var unit = units [i];
                if (!unit.IsAlive ()) {
                    continue;
                }

                if (unit.GetLastTurnMoved () >= _currentTurnNumber) {
                    continue;
                }

                if ((nextUnitIndex == -1) ||
                    (unit.GetInitiative () > highestInitiative)) {
                    nextUnitIndex = i;
                    highestInitiative = unit.initiative;
                }
            }

            if (nextUnitIndex > -1) {
                return nextUnitIndex;
            }

            for (int i = 0; i < units.Count; ++i) {
                var unit = units [i];
                if (!unit.IsAlive ()) {
                    continue;
                }

                if ((nextUnitIndex == -1) ||
                    (unit.GetInitiative () > highestInitiative)) {
                    nextUnitIndex = i;
                    highestInitiative = unit.initiative;
                }
            }

            return nextUnitIndex;

        }

        private void GenerateOrder (float deltaSeconds)
        {
            if (MINIMAX_AI) {
                if (_minimax == null) {
                    var wr = MakeWorldRep (_mapManager);
                    var movingUnitIndex = wr.GetIndexOfMovingUnit ();
                    Debug.LogFormat ("Generating action for {0} {1}", 
                        movingUnitIndex,
                        wr.Chars[movingUnitIndex].Name);
                    _minimax = new MiniMax (wr,
                        new WorldRepEvaluatorImpl (),
                        new Dictionary<int, IActionGenerator> {
                            {0, new CombatActionGenerator() },
                            {1, new CombatActionGenerator() }
                            },
                        new ActionManagerImpl (),
                        2, // depth limit
                        0.5f // time limit
                        );
                }

                _minimax.Evaluate (Mathf.Min(deltaSeconds, 0.1f), out bool done, out float value);

                if (_minimax.IsComplete ()) {
                    var action = _minimax.GetSelectedAction ();
                    if (action != null) {
                        _currentOrder = action.GetCombatOrder (_mapManager, _mapManager.GetHexMap ());
                    } else {
                        Debug.LogWarning ("null order returned from minimax");
                        AddLogLine ("(?!) Null Order from minimax");
                        var wr = MakeWorldRep (_mapManager);
                        var movingUnitIndex = wr.GetIndexOfMovingUnit ();
                        var movingUnit = units [movingUnitIndex];
                        _currentOrder = new PassOrder (units [movingUnitIndex]);
                        Debug.LogWarningFormat ("issuing pass for {0}",movingUnit.unitName);
                    }
                    _minimax = null;
                    return;
                }
            } 
        }

        private WorldRep MakeWorldRep (MapManager mapManager)
        {
            List<CharRep> charList = new List<CharRep> ();

            for (int i = 0; i < units.Count; ++i) {
                var unit = units [i];
                var charRep = MakeCharRep (unit.unitName, unit.GetTeamID (),
                    unit.GetHexCoord (), 
                    unit.currentHP, unit.maxHP,
                    unit.GetInitiative (), unit.GetLastTurnMoved (),
                    unit.maxMove);
                charRep = charRep.SetWeapon (unit.weapon).SetArmor (unit.armor);
                charList.Add (charRep);
            }

            Dictionary<HexCoord, TileRep> tileDict = new Dictionary<HexCoord, TileRep> ();

            foreach (HexTile hexTile in mapManager.GetTiles ()) {
                var hc = hexTile.HexCoord;
                TileRep tileRep = new TileRep (hc, hexTile.CanMove);
                tileDict [hc] = tileRep;
            }

            return new WorldRep (charList, tileDict, _currentTurnNumber);
        }

        internal int GetUnitCount ()
        {
            return units.Count;
        }

        private CharRep MakeCharRep (string name, 
            int teamIndex, HexCoord position, 
            int currentHealth, int maxHealth,
            int initiative, int lastTurnMoved, 
            int moveSpeed)
        {
            var weapon = new WeaponFistRep ();
            var armor = new ArmorClothRep ();

            return new CharRep (name, position, 
                currentHealth, maxHealth, 
                0, 0, 
                initiative, moveSpeed, 
                lastTurnMoved, teamIndex, 
                weapon, armor);
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

        public List<int> GetEnemyIndicesAdjacentTo (HexCoord startCoord, int myTeamId)
        {
            var outList = new List<int> ();

            for (int i = 0; i < units.Count; ++i) {
                var unit = units [i];

                if (!unit.IsAlive ()) {
                    continue;
                }
                if (unit.GetTeamID () == myTeamId) {
                    continue;
                }
                if (unit.GetHexCoord ().DistanceTo (startCoord) == 1) {
                    outList.Add (i);
                }
            }
            return outList;
        }

        private void GenerateRetreatOrder (CombatUnit combatant)
        {
            var startCoord = combatant.GetHexCoord ();

            var locs = HexCoord.GetAtRangeFromLoc (1, startCoord).FindAll (IsLocationWalkable);
            BdgRandom.ShuffleList<HexCoord> (locs);

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
            var tiles = _mapManager.GetTilesAt (hc);

            if (tiles.Count == 0) {
                return false;
            }

            foreach (var hexTile in tiles) {
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
                        openList.Add (neighbor);
                    }
                }
            }
            return distances;
        }

        public CombatUnit GetCombatUnitByName (string name)
        {
            foreach (var combatUnit in units) {
                if (combatUnit.unitName == name) {
                    return combatUnit;
                }
            }
            return null;
        }

        public CombatUnit GetCombatUnitByIndex (int movingCharIndex)
        {
            return (units [movingCharIndex]);
        }
    }
}
