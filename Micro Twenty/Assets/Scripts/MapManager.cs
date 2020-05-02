using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MicroTwenty
{
    public enum SpriteId
    {
        SPRITE_TILE_EMPTY = 0,
        SPRITE_TILE_MOUNTAIN = 1,
        SPRITE_TILE_WATER = 2,
        SPRITE_TILE_CITY = 3,
        SPRITE_TILE_DUNGEON = 4,
        SPRITE_TILE_ROCK_WALL = 5,
        SPRITE_TILE_WOOD_WALL = 6,
        SPRITE_TILE_BUILDING = 7,
        SPRITE_TILE_BUILDING_ENTRANCE = 8,
        SPRITE_TILE_DOCK_NW_SE = 9,
        SPRITE_TILE_GUY = 10,
        SPRITE_TILE_RAT_MAN = 11,
        SPRITE_TILE_SLIME = 12,
        SPRITE_TILE_TREE = 13,
        SPRITE_TILE_CURSOR_CIRCLE = 14,
        SPRITE_TILE_CURSOR_DOT = 15,
        SPRITE_TILE_CURSOR_PLUS = 16,
        SPRITE_TILE_CURSOR_X = 17,
        SPRITE_TILE_DOT = 18,
        SPRITE_TILE_SHIP = 19,
        SPRITE_COMBAT_GUY = 20,
        SPRITE_COMBAT_RAT_MAN = 21,
        SPRITE_COMBAT_GUY_1 = 22, // stan
        SPRITE_COMBAT_GUY_2 = 23, // kim
        SPRITE_COMBAT_GUY_3 = 24, // flexo
        SPRITE_COMBAT_GUY_4 = 25, // mags
        SPRITE_COMBAT_GUY_5 = 26, // torso
        SPRITE_COMBAT_GUY_6 = 27, // belto
        SPRITE_COMBAT_SNAKE = 28,
        SPRITE_COMBAT_DOG = 29,
        SPRITE_COMBAT_CAT = 30,
        SPRITE_COMBAT_CRAB = 31,
        SPRITE_COMBAT_GHOST = 32,
        SPRITE_COMBAT_DJINN = 33,
        SPRITE_COMBAT_SKELETON = 34,
        SPRITE_COMBAT_SKELETON_ARCHER = 35,
        SPRITE_COMBAT_STAFF = 36,
        SPRITE_COMBAT_BUG = 37,
        SPRITE_SIGN = 38,
        SPRITE_LAMPPOST = 39,
        SPRITE_CHEST = 40,
        SPRITE_POTION = 41,
        SPRITE_SCROLL = 42,
        SPRITE_CHICKEN_LEG = 43, // no, but seriously - what is this?
        SPRITE_ARROW_EW = 44,
        SPRITE_ARROW_SWNE = 45,
        SPRITE_ARROW_NWSE = 46,
        SPRITE_BALL = 47,
    };

    public enum ScreenId
    {
        NoIntroGameScreen,
        BigDiceGamesScreen,
        TitleScreen,
        CreditsScreen,
        MenuScreen,
    };

    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private Texture2D hexTileSheet;

        [SerializeField]
        private Texture2D fontBitmap;

        [SerializeField]
        private Texture2D menuBitmap;

        [SerializeField]
        private Texture2D bigDiceGamesTexture;

        [SerializeField]
        private Texture2D microTwentyTexture;

        [SerializeField]
        private Texture2D signIconTexture;

        [SerializeField]
        private Texture2D ratKingTexture;

        [SerializeField]
        private CanvasRenderer targetCanvasRenderer;

        [SerializeField]
        private int targetTextureWidth;

        [SerializeField]
        private int targetTextureHeight;

        private Texture2D targetTexture;

        private bool drawn;

        List<HexMap> maps;
        private int selectedMap;

        HexCoord playerPos;

        GameMgr _gameMgr;

        List<DynamicObject> dynamicObjects;
        private CombatMgr _combatMgr;

        public MenuManager _menuMgr;
        public MenuObject _mainMenu;

        public IIntroScreen _introScreen;

        // Start is called before the first frame update
        void Start ()
        {
            drawn = false;
            dynamicObjects = new List<DynamicObject> ();

            _gameMgr = new GameMgr (this);

            maps = new List<HexMap> {
                new Episode1Map(_gameMgr),
                new Episode2Map(_gameMgr),
                new Episode3Map(_gameMgr),
                new Episode4Map(_gameMgr),
                new Episode5Map(_gameMgr),
                new Episode6Map(_gameMgr),
                new Ep1CityRycroftMap(_gameMgr),
                new Ep1DungeonRatHoleMap(_gameMgr),
                new CombatMap(_gameMgr),
                new BigCombatMap(_gameMgr),
                new CombatFoyerMap(_gameMgr),
                new CombatHallMap(_gameMgr),
                new RatIsland(_gameMgr),
                new CavernsMap(_gameMgr),
                new DocksMap(_gameMgr),
                new LabyrinthMap(_gameMgr),
                new RiverCrossingMap(_gameMgr),
                new TownMap(_gameMgr),
            };

            AddLevelHacks ();

            selectedMap = 0;

            targetTexture = new Texture2D (targetTextureWidth, targetTextureHeight);
            targetTexture.wrapMode = TextureWrapMode.Clamp;
            targetTexture.filterMode = FilterMode.Point;

            var im = targetCanvasRenderer.GetComponent<Image> ();

            im.material.SetTexture ("_MainTex", targetTexture);

            playerPos = new HexCoord (0, 0, 0);
            var playerObject = new DynamicObject (_gameMgr, playerPos, DynamicObject.DynamicObjectType.PLAYER, false);
            dynamicObjects.Add (playerObject);

            _menuMgr = new MenuManager (menuBitmap, fontBitmap);
            _mainMenu = new MenuObject ("main menu", menuBitmap, fontBitmap);
            _mainMenu.SetWindow (1, 4);
            _mainMenu.AddItem ("Party");
            _mainMenu.AddItem ("Combat").SetWindow (1, 4);
            _mainMenu ["Combat"].AddItem ("Attack");
            _mainMenu ["Combat"].AddItem ("Defend");
            _mainMenu ["Combat"].AddItem ("Item").SetWindow (1, 5);
            _mainMenu ["Combat"] ["Item"].AddItem ("Map");
            _mainMenu ["Combat"] ["Item"].AddItem ("Compass");
            _mainMenu ["Combat"] ["Item"].AddItem ("Heal");
            _mainMenu ["Combat"] ["Item"].AddItem ("Restore");
            _mainMenu ["Combat"] ["Item"].AddItem ("Revive");
            _mainMenu ["Combat"] ["Item"].AddItem ("Mana");
            _mainMenu ["Combat"] ["Item"].AddItem ("Teleport").SetEnabled (false);
            _mainMenu ["Combat"].AddItem ("Magic").SetWindow (2, 6);
            _mainMenu ["Combat"] ["Magic"].AddItem ("Minor Heal");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Heal");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Major Heal");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Light");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Reveal");
            _mainMenu ["Combat"] ["Magic"].AddItem ("M. Mouth");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Fireball");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Exp. FBall");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Lightning");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Chn Ltning");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sum. Rat");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sum. Ratman");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sum. Skeleton");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sum. Zombie");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sum. Dragon");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Queasiness");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Sickness");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Illness");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Plague");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Minor Death").SetEnabled (false);
            _mainMenu ["Combat"] ["Magic"].AddItem ("Death");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Major Death");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Red Death");
            _mainMenu ["Combat"] ["Magic"].AddItem ("Raise Dead");
            _mainMenu ["Combat"].AddItem ("Wear").SetWindow (2, 5);
            _mainMenu ["Combat"] ["Wear"].AddItem ("* None *");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Cloth");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Leather");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Scale");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Banded");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Studded");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Plate");
            _mainMenu ["Combat"] ["Wear"].AddItem ("+1 Plate");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Sm. Shld");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Md. Shld");
            _mainMenu ["Combat"] ["Wear"].AddItem ("Lg. Shld");
            _mainMenu ["Combat"].AddItem ("Wield").SetWindow (2, 6);
            _mainMenu ["Combat"] ["Wield"].AddItem ("* None *");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Dagger");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Club");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Mace");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Flail");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Sword");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Falchion");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Halberd");
            _mainMenu ["Combat"] ["Wield"].AddItem ("Flm Sword");
            _mainMenu.AddItem ("Lore");
            _mainMenu.AddItem ("Help");
            _mainMenu.AddItem ("Cheat");
            _mainMenu ["Cheat"] ["Teleport"].SetWindow (2, 8);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_1"].SetItemId (1000);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_2"].SetItemId (1001);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_3"].SetItemId (1002);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_4"].SetItemId (1003);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_5"].SetItemId (1004);
            _mainMenu ["Cheat"] ["Teleport"] ["ep_6"].SetItemId (1005);
            _mainMenu ["Cheat"] ["Teleport"] ["combat"].SetItemId (1010);
            _mainMenu ["Cheat"] ["Teleport"] ["bigcombat"].SetItemId (1011);
            _mainMenu ["Cheat"] ["Teleport"] ["caverns"].SetItemId (2001);
            _mainMenu ["Cheat"] ["Teleport"] ["docks"].SetItemId (2002);
            _mainMenu ["Cheat"] ["Teleport"] ["labyrinth"].SetItemId (2003);
            _mainMenu ["Cheat"] ["Teleport"] ["rvr_cross"].SetItemId (2004);
            _mainMenu ["Cheat"] ["Teleport"] ["town"].SetItemId (2005);
            _mainMenu ["Cheat"] ["Teleport"] ["c_ryc"].SetItemId (3001);
            _mainMenu ["Cheat"] ["Teleport"] ["d_hole"].SetItemId (3002);
            _mainMenu ["Cheat"] ["Teleport"] ["rat isl"].SetItemId (3003);
            _mainMenu.AddItem ("Debug");
            _mainMenu.AddItem ("Quit");
            _mainMenu.Build ();

            ShowScreen (ScreenId.BigDiceGamesScreen);
        }

        internal int GetUnitCount () => _combatMgr.GetUnitCount ();

        private void AddLevelHacks ()
        {
            int i = GetMapByName ("ep_1");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (5, -8, 3), "ep_2", new HexCoord (-7, 4, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (0, -4, 4), "ep1c_rycroft", new HexCoord (0, 0, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, -8, 7), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, 2, -3), "ep1d_rathole", new HexCoord (0, -2, 2)));
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (-2, 3, -1), "combat", new HexCoord (0, 0, 0)));
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (2, 3, -5), "bigcombat", new HexCoord (0, 0, 0)));
            maps [i].dynamicObjects.Add (new Signpost (_gameMgr, new HexCoord (2, -1, -1), "Signpost", new List<string> {
                "A sign says",
                "'Hello, Adventurer'." }));

            maps [i].dynamicObjects.Add (new NarrativeMsg (_gameMgr, new HexCoord (4, 1, -5), "Message", new List<string> {
                "It is quiet and still.",
                "Still what?",
                "Still quiet." }));

            i = GetMapByName ("ep_2");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-7, 4, 3), "ep_1", new HexCoord (5, -8, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (5, -8, 3), "ep_3", new HexCoord (-7, 4, 3)));

            i = GetMapByName ("ep_3");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-7, 4, 3), "ep_2", new HexCoord (5, -8, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, -1, 0), "ep_4", new HexCoord (1, 0, -1)));

            i = GetMapByName ("ep_4");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, 0, -1), "ep_3", new HexCoord (1, -1, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 1, 8), "ep_5", new HexCoord (2, -10, 8)));

            i = GetMapByName ("ep_5");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (2, -10, 8), "ep_4", new HexCoord (-9, 1, 8)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 1, 8), "ep_6", new HexCoord (2, -10, 8)));

            i = GetMapByName ("ep_6");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (2, -10, 8), "ep_5", new HexCoord (-9, 1, 8)));

            i = GetMapByName ("ep1c_rycroft");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (4, 4, -8), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-8, 3, 5), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-1, -10, 11), "ratisland", new HexCoord (-9, 9, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-6, -3, 9), "caverns", new HexCoord (-9, 1, 8)));

            i = GetMapByName ("ep1d_rathole");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (0, -2, 2), "ep_1", new HexCoord (1, 2, -3)));
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (-1, 6, -5), "combat", new HexCoord (0, 0, 0)));

            i = GetMapByName ("combat");
            // dynamic objects handled by combatMgr

            i = GetMapByName ("ratisland");
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (3, 0, -3), "bigcombat", new HexCoord (0, 0, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 9, 0), "ep1c_rycroft", new HexCoord (-1, -10, 11)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (3, -6, 3), "ep_5", new HexCoord (1, 1, -2)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (9, -10, 1), "ep_2", new HexCoord (-6, 3, 3)));

            i = GetMapByName ("bigcombat");
            // dynamic objects handled by combatMgr

            i = GetMapByName ("caverns");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 1, 8), "ep1c_rycroft", new HexCoord (-6, -3, 9)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-1, 8, -7), "docks", new HexCoord (-10, 10, 0)));

            i = GetMapByName ("docks");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-10, 10, 0), "caverns", new HexCoord (-1, 8, -7)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (10, -10, 0), "labyrinth", new HexCoord (2, -10, 8)));

            i = GetMapByName ("labyrinth");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (2, -10, 8), "docks", new HexCoord (10, -10, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-2, 10, -8), "river_crossing", new HexCoord (-9, 9, 0)));

            i = GetMapByName ("river_crossing");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 9, 0), "labyrinth", new HexCoord (-2, 10, -8)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (9, -9, 0), "town", new HexCoord (-14, 5, 9)));

            i = GetMapByName ("town");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-14, 5, 9), "river_crossing", new HexCoord (9, -9, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (14, -5, -9), "ep_1", new HexCoord (0, 0, 0)));
        }

        internal Texture2D GetFontBitmap ()
        {
            return fontBitmap;
        }

        internal Texture2D GetMenuBitmap ()
        {
            return menuBitmap;
        }

        internal Texture2D GetTargetTexture ()
        {
            return targetTexture;
        }

        internal List<HexTile> GetTiles ()
        {
            return GetHexMap ().GetTiles ();
        }

        public List<DynamicObject> GetDynamicObjectsAt (HexCoord hc)
        {
            var outList = new List<DynamicObject> ();
            foreach (var dynObj in dynamicObjects) {
                if (dynObj.hexCoord.Equals (hc)) {
                    outList.Add (dynObj);
                }
            }
            return outList;
        }

        public List<HexTile> GetTilesAt (HexCoord hc)
        {
            return GetHexMap ().GetTilesAt (hc);
        }

        private int GetMapByName (string destMapName)
        {
            int foundMapIndex = -1;
            for (int i = 0; i < maps.Count; ++i) {
                if (maps [i].Name () == destMapName) {
                    foundMapIndex = i;
                    break;
                }
            }
            if (foundMapIndex == -1) {
                Debug.LogWarningFormat ("Could not find dest map name {0}", destMapName);
            }
            return foundMapIndex;
        }

        // Update is called once per frame
        void Update ()
        {
            var deltaSeconds = Time.deltaTime;

            if (_introScreen != null) {
                _introScreen.Draw ();
                _introScreen.UpdateScreen (deltaSeconds);
                return;
            }

            //_gameMgr.Update (deltaSeconds);
            drawn = false;

            if ((_combatMgr != null) &&
                (_combatMgr.InCombat)) {
                _combatMgr.Update (deltaSeconds);
                drawn = false;

                if (_combatMgr.GetIsDone ()) {
                    UnityEngine.Debug.Log ("Combat is done, incombat = false");
                    _combatMgr.InCombat = false;

                    // TODO return player to where they were beforehand
                    TeleportPlayer ("ep_1", new HexCoord (0, 0, 0));
                }
            }

            if (StaticSettings.IS_SOAKING_COMBAT) {
                if ((_combatMgr == null) ||
                (!_combatMgr.InCombat)) {
                    var combatMapIndex = Random.Range (0, 4);

                    string [] combatMaps = new string [] {
                    "combat",
                    "bigcombat",
                    "combat_foyer",
                    "combat_hall"
                };

                    var combatMapName = combatMaps [combatMapIndex];
                    _gameMgr.AddCommand (new CombatCommand ("soak combat", _gameMgr, combatMapName, new HexCoord (0, 0, 0)));
                }
            }

            if (!drawn) {
                ClearTex ();
                DrawMap ();

                DrawText ();

                if ((_combatMgr != null) &&
                    (_combatMgr.InCombat)) {
                    _combatMgr.Draw ();
                    targetTexture.Apply ();
                } else {
                    drawn = true;
                }
            }

            // Movement

            if ((_combatMgr == null) ||
                (!_combatMgr.InCombat)) {

                if (Input.GetKeyDown (KeyCode.D)) {
                    // go east
                    tryMove (new HexCoord (1, -1, 0));
                }
                if (Input.GetKeyDown (KeyCode.E)) {
                    // go northeast
                    tryMove (new HexCoord (1, 0, -1));
                }
                if (Input.GetKeyDown (KeyCode.W)) {
                    // go northwest
                    tryMove (new HexCoord (0, 1, -1));
                }
                if (Input.GetKeyDown (KeyCode.A)) {
                    // go west
                    tryMove (new HexCoord (-1, 1, 0));
                }
                if (Input.GetKeyDown (KeyCode.Z)) {
                    // go southwest
                    tryMove (new HexCoord (-1, 0, 1));
                }
                if (Input.GetKeyDown (KeyCode.X)) {
                    // go southeast
                    tryMove (new HexCoord (0, -1, 1));
                }
            }

            // Menu
            if (Input.GetKeyDown (KeyCode.M)) {
                // show menu
                ShowMenu ();
            }

            if (_menuMgr.IsOpen ()) {
                if (Input.GetKeyDown (KeyCode.UpArrow)) {
                    // show menu
                    _menuMgr.OnUp ();
                }
                if (Input.GetKeyDown (KeyCode.DownArrow)) {
                    // show menu
                    _menuMgr.OnDown ();
                }
                if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                    // show menu
                    _menuMgr.OnLeft ();
                }
                if (Input.GetKeyDown (KeyCode.RightArrow)) {
                    // show menu
                    _menuMgr.OnRight ();
                }
                if ((Input.GetKeyDown (KeyCode.Space)) ||
                    (Input.GetKeyDown (KeyCode.Return)) ||
                    (Input.GetButtonDown ("Submit"))) {

                    var result = _menuMgr.OnActivate ();
                    if (result != null) {
                        var resId = result.GetItemId ();
                        switch (resId) {
                        case 1000:
                            // teleport to EP_1
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_1", new HexCoord (0, 0, 0)));
                            break;
                        case 1001:
                            // teleport to EP_2
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_2", new HexCoord (1, 2, -3)));
                            break;
                        case 1002:
                            // teleport to EP_3
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_3", new HexCoord (0, 0, 0)));
                            break;
                        case 1003:
                            // teleport to EP_4
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_4", new HexCoord (0, 0, 0)));
                            break;
                        case 1004:
                            // teleport to EP_5
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_5", new HexCoord (0, 0, 0)));
                            break;
                        case 1005:
                            // teleport to EP_6
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep_6", new HexCoord (0, 0, 0)));
                            break;
                        case 1010:
                            // teleport to combat
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "combat", new HexCoord (0, 0, 0)));
                            break;
                        case 1011:
                            // teleport to bigcombat
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "bigcombat", new HexCoord (0, 0, 0)));
                            break;
                        case 2001:
                            // teleport to caverns
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "caverns", new HexCoord (0, 0, 0)));
                            break;
                        case 2002:
                            // teleport to docks
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "docks", new HexCoord (-4, 4, 0)));
                            break;
                        case 2003:
                            // teleport to labyrinth
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "labyrinth", new HexCoord (0, 0, 0)));
                            break;
                        case 2004:
                            // teleport to river crossing
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "river_crossing", new HexCoord (-1, 1, 0)));
                            break;
                        case 2005:
                            // teleport to town
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "town", new HexCoord (-1, 1, 0)));
                            break;
                        case 3001:
                            // teleport to Rycroft
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep1c_rycroft", new HexCoord (-1, 1, 0)));
                            break;
                        case 3002:
                            // teleport to rat hole
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ep1d_rathole", new HexCoord (-1, 1, 0)));
                            break;
                        case 3003:
                            // teleport to rat island
                            _gameMgr.AddCommand (new TeleportCommand ("teleport", _gameMgr, "ratisland", new HexCoord (0, 0, 0)));
                            break;
                        default:
                            Debug.Log ("Got unknown ID: " + resId);
                            break;
                        }
                    }
                }
                if ((Input.GetKeyDown (KeyCode.Escape)) ||
                    (Input.GetButtonDown ("Cancel"))) {
                    // show menu
                    _menuMgr.OnBack ();
                }
            }

            if (_menuMgr.IsOpen ()) {
                _menuMgr.Draw (targetTexture, 10, targetTexture.height - 10);
                drawn = false;
            }

            _gameMgr.Update (deltaSeconds);
        }

        private void ShowMenu ()
        {
            if (!_menuMgr.IsOpen ()) {
                _menuMgr.OpenMenu (_mainMenu);
            }
        }

        private void DrawText ()
        {
            var fillColor = new Color (0, 0, 0, 0.5f);
            var startPosY = targetTextureHeight - 14;
            int msgHeight = 12;

            var msg = GetHexMap ().Name ();

            TextureDrawing.DrawRect (targetTexture, 4, startPosY, (msg.Length + 1) * 6, msgHeight, fillColor, Color.green, true, true);

            TextureDrawing.DrawStringAt (targetTexture, fontBitmap, msg, 6, startPosY + 2, Color.white);

            if (_combatMgr != null) {
                var zeroScoreString = _combatMgr.WinTally [0].ToString ();
                var oneScoreString = _combatMgr.WinTally [1].ToString ();

                var msgWidth = (oneScoreString.Length * 6) + 4;
                var startPosX = targetTextureWidth - (oneScoreString.Length + 1) * 6;

                TextureDrawing.DrawRect (targetTexture, startPosX, startPosY, msgWidth, msgHeight, fillColor, Color.red, true, true);
                TextureDrawing.DrawStringAt (targetTexture, fontBitmap, oneScoreString, startPosX + 2, startPosY + 2, Color.red);

                msgWidth = (zeroScoreString.Length * 6) + 4;
                startPosX -= (6 + 6 * zeroScoreString.Length + 8);

                TextureDrawing.DrawRect (targetTexture, startPosX, startPosY, msgWidth, msgHeight, fillColor, Color.cyan, true, true);
                TextureDrawing.DrawStringAt (targetTexture, fontBitmap, zeroScoreString, startPosX + 2, startPosY + 2, Color.cyan);

            }
            targetTexture.Apply ();
        }


        public HexMap GetHexMap ()
        {
            return maps [selectedMap];
        }

        private void tryMove (HexCoord delta)
        {
            var newPos = playerPos.Add (delta);

            int tileIndex = -1;

            HexMap map = GetHexMap ();

            for (int i = 0; i < map.tiles.Count; ++i) {
                var t = map.tiles [i];

                if ((t.x == newPos.x) &&
                    (t.y == newPos.y) &&
                    (t.z == newPos.z)) {
                    tileIndex = i;
                    break;
                }
            }

            if (tileIndex == -1) {
                // no neighbor tile found, return
                return;
            }

            if (!map.tiles [tileIndex].CanMove) {
                return;
            }

            foreach (var dynObj in GetHexMap ().dynamicObjects) {
                if (dynObj.hexCoord.Equals (newPos)) {
                    if (dynObj.blocksMovement) {
                        return;
                    }
                }
            }

            SetPlayerPos (newPos);
            Debug.LogFormat ("Moved to {0}", playerPos.ToString ());

            foreach (var dynObj in GetHexMap ().dynamicObjects) {
                if (dynObj.hexCoord.Equals (newPos)) {
                    dynObj.OnMoveOver ();
                }
            }
        }

        private void SetPlayerPos (HexCoord newPos)
        {
            playerPos = newPos;
            foreach (var obj in dynamicObjects) {
                if (obj.objectType == DynamicObject.DynamicObjectType.PLAYER) {
                    obj.hexCoord = newPos;
                }
            }
            drawn = false;
        }

        private void ClearTex ()
        {
            Color c = new Color (0.5f, 1.0f, 0.5f);
            for (int x = 0; x < targetTexture.width; ++x) {
                for (int y = 0; y < targetTexture.height; ++y) {
                    targetTexture.SetPixel (x, y, c);
                }
            }
            targetTexture.Apply ();
        }

        public void HexCoordToScreenCoords (HexCoord hc, out int px, out int py)
        {
            var center_x = targetTextureWidth / 2;
            var center_y = targetTextureHeight / 2;

            var step_x = 8;
            var step_y = 12;

            var rx = hc.x - playerPos.x;
            var ry = hc.y - playerPos.y;
            var rz = hc.z - playerPos.z;

            px = center_x + step_x * (rx - ry);
            py = center_y + step_y * (rx + ry);
        }

        private void GetSpriteCoords (SpriteId id, out int sx, out int sy)
        {
            const int sheet_columns = 7;
            const int sheet_rows = 7;
            const int tile_size = 16;

            var id_as_int = (int)id;
            var column = id_as_int % sheet_columns;
            var row = id_as_int / sheet_columns;
            // except that the pixels are bottom up
            row = sheet_rows - row - 1;

            sx = column * tile_size;
            sy = row * tile_size;
        }

        private void DrawMap ()
        {
            //Debug.LogFormat ("Drawing map");

            foreach (var t in maps [selectedMap].tiles) {
                var tile_width = 16;
                var tile_height = 16;

                //Debug.LogFormat ("drawing tile {0}", t);
                var tx = t.x;
                var ty = t.y;
                var tz = t.z;

                var hc = new HexCoord (tx, ty, tz);

                HexCoordToScreenCoords (hc, out int px, out int py);

                var source_x = 0;
                var source_y = 32;

                Color tile_tint = Color.white;

                switch (t.TileType) {
                case HexMap.TileType.PLAIN:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_EMPTY, out source_x, out source_y);
                    tile_tint = Color.HSVToRGB (0.3f, 1, 1); // green
                    break;
                case HexMap.TileType.MOUNTAIN:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_MOUNTAIN, out source_x, out source_y);
                    tile_tint = Color.HSVToRGB (0.6f, .5f, 1);
                    break;
                case HexMap.TileType.WATER:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_WATER, out source_x, out source_y);
                    tile_tint = Color.blue;
                    break;
                case HexMap.TileType.WATER_MOUNT:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_MOUNTAIN, out source_x, out source_y);
                    tile_tint = Color.blue;
                    break;
                case HexMap.TileType.PORTAL:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_DUNGEON, out source_x, out source_y);
                    tile_tint = Color.gray;
                    break;
                case HexMap.TileType.CITY:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_CITY, out source_x, out source_y);
                    tile_tint = Color.gray;
                    break;
                case HexMap.TileType.ROCKWALL:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_ROCK_WALL, out source_x, out source_y);
                    tile_tint = Color.gray;
                    break;
                case HexMap.TileType.WOODWALL:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_WOOD_WALL, out source_x, out source_y);
                    tile_tint = Color.magenta;
                    break;
                case HexMap.TileType.BUILDING:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_BUILDING, out source_x, out source_y);
                    tile_tint = Color.cyan;
                    break;
                case HexMap.TileType.BUILDING_ENTRANCE:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_BUILDING_ENTRANCE, out source_x, out source_y);
                    tile_tint = Color.red;
                    break;
                case HexMap.TileType.DOCK:
                    GetSpriteCoords (SpriteId.SPRITE_TILE_DOCK_NW_SE, out source_x, out source_y);
                    tile_tint = Color.red;
                    break;
                }

                if (px < 0) {
                    tile_width += px;
                    source_x -= px;
                    px = 0;
                }

                if (py < 0) {
                    tile_height += py;
                    source_y -= py;
                    py = 0;
                }

                if (px + tile_width > targetTextureWidth) {
                    tile_width = targetTextureWidth - px;
                }

                if (py + tile_height > targetTextureHeight) {
                    tile_height = targetTextureHeight - py;
                }

                if ((tile_width <= 0) || (tile_height <= 0)) {
                    continue;
                }

                TextureDrawing.DrawTintedPartialSprite (targetTexture, hexTileSheet,
                    px, py, source_x, source_y, tile_width, tile_height, tile_tint);
            }

            foreach (var dynobj in dynamicObjects) {
                var combObj = dynobj as CombatantSprite;
                if (combObj != null) {
                    int team = combObj.GetTeam ();
                    Color c = team == 0 ? Color.cyan : Color.red;
                    DrawTintedSpriteAtLocation (combObj.GetSpriteId (), dynobj.hexCoord, c);
                } else {
                    // TODO clean this up - draw a player party sprite except in combat
                    if ((_combatMgr == null) ||
                        (!_combatMgr.InCombat)) {
                        DrawSpriteAtLoc (dynobj.objectType, dynobj.hexCoord, dynobj);
                    }
                }
            }

            foreach (var dynobj in maps [selectedMap].dynamicObjects) {
                var combObj = dynobj as CombatantSprite;
                if (combObj != null) {
                    int team = combObj.GetTeam ();
                    Color c = team == 0 ? Color.cyan : Color.red;
                    DrawTintedSpriteAtLocation (combObj.GetSpriteId (), dynobj.hexCoord, c);
                } else {
                    DrawSpriteAtLoc (dynobj.objectType, dynobj.hexCoord, dynobj);
                }
            }

            targetTexture.Apply ();
        }

        public void DrawSpriteAtPos (SpriteId spriteId, int screenPosX, int screenPosY)
        {
            GetSpriteCoords (spriteId, out int sprite_x, out int sprite_y);

            TextureDrawing.DrawPartialSprite (targetTexture, hexTileSheet, screenPosX, screenPosY, sprite_x, sprite_y, 16, 16);
        }

        public void DrawTintedSpriteAtPos (SpriteId spriteId, int screenPosX, int screenPosY, Color c)
        {
            GetSpriteCoords (spriteId, out int sprite_x, out int sprite_y);

            TextureDrawing.DrawTintedPartialSprite (targetTexture, hexTileSheet,
                screenPosX, screenPosY, sprite_x, sprite_y, 16, 16, c);
        }

        public void DrawTintedSpriteAtLocation (SpriteId spriteId, HexCoord hexCoord, Color c)
        {
            HexCoordToScreenCoords (hexCoord, out int px, out int py);
            GetSpriteCoords (spriteId, out int source_x, out int source_y);
            TextureDrawing.DrawTintedPartialSprite (targetTexture, hexTileSheet,
                px, py, source_x, source_y, 16, 16, c);
        }

        public void DrawSpriteAtLoc (DynamicObject.DynamicObjectType objType, HexCoord hexCoord, DynamicObject obj)
        {
            HexCoordToScreenCoords (hexCoord, out int px, out int py);

            var source_x = 0;
            var source_y = 0;
            bool has_sprite = false;

            switch (objType) {
            case DynamicObject.DynamicObjectType.PLAYER:
                GetSpriteCoords (SpriteId.SPRITE_TILE_GUY, out source_x, out source_y);
                has_sprite = true;
                break;
            case DynamicObject.DynamicObjectType.TREE:
                GetSpriteCoords (SpriteId.SPRITE_TILE_TREE, out source_x, out source_y);
                has_sprite = true;
                break;
            case DynamicObject.DynamicObjectType.SHIP:
                GetSpriteCoords (SpriteId.SPRITE_TILE_SHIP, out source_x, out source_y);
                has_sprite = true;
                break;
            case DynamicObject.DynamicObjectType.TRIGGER:
                has_sprite = false;
                break;
            case DynamicObject.DynamicObjectType.RATMAN:
                GetSpriteCoords (SpriteId.SPRITE_TILE_RAT_MAN, out source_x, out source_y);
                has_sprite = true;
                break;
            case DynamicObject.DynamicObjectType.COMBATANT:
                // TODO would prefer to draw this tinted
                var combatantObj = obj as CombatantSprite;
                if (combatantObj == null) {
                    Debug.LogWarning ("can't draw combatant object?");
                    has_sprite = false;
                } else {
                    var spriteId = combatantObj.GetSpriteId ();
                    GetSpriteCoords (spriteId, out source_x, out source_y);
                    has_sprite = true;
                }
                break;
            case DynamicObject.DynamicObjectType.SIGN:
                GetSpriteCoords (SpriteId.SPRITE_SIGN, out source_x, out source_y);
                has_sprite = true;
                break;

            case DynamicObject.DynamicObjectType.NARRATIVEMSG:
                has_sprite = false;
                break;

            default:
                Debug.LogFormat ("drawing unknown dynamic object: {0}", objType);
                has_sprite = false;
                break;
            }

            if (!has_sprite) {
                return;
            }

            TextureDrawing.DrawPartialSprite (targetTexture, hexTileSheet, px, py, source_x, source_y, 16, 16);
        }

        internal void TeleportPlayer (string destMapName, HexCoord destMapCoord)
        {
            int foundMapIndex = -1;
            for (int i = 0; i < maps.Count; ++i) {
                if (maps [i].Name () == destMapName) {
                    foundMapIndex = i;
                    break;
                }
            }
            if (foundMapIndex == -1) {
                Debug.LogErrorFormat ("Could not find dest map name {0}", destMapName);
                return;
            }
            selectedMap = foundMapIndex;
            drawn = false;
            SetPlayerPos (destMapCoord);
        }

        internal void EnterCombat (string destMapName)
        {
            if (_combatMgr == null) {
                _combatMgr = new CombatMgr (this, _gameMgr, destMapName);
            }
            _combatMgr.InCombat = true;
        }

        public CombatUnit GetCombatUnitByName (string name)
        {
            return _combatMgr.GetCombatUnitByName (name);
        }

        internal CombatUnit GetCombatUnitByIndex (int movingCharIndex)
        {
            return _combatMgr.GetCombatUnitByIndex (movingCharIndex);
        }

        internal CombatMgr GetCombatMgr ()
        {
            return _combatMgr;
        }

        internal void ShowScreen (ScreenId screenId)
        {
            Debug.LogFormat ("Showing screen {0}", screenId);
            switch (screenId) {
            case ScreenId.BigDiceGamesScreen:
                _introScreen = new BigDiceGamesScreen (this, bigDiceGamesTexture);
                break;

            case ScreenId.TitleScreen:
                _introScreen = new MicroTwentyTitleScreen (this, microTwentyTexture);
                break;

            case ScreenId.MenuScreen:
                _introScreen = new MainMenuScreen (this, microTwentyTexture);
                break;

            case ScreenId.CreditsScreen:
                _introScreen = new CreditsScreen (this);
                break;

            case ScreenId.NoIntroGameScreen:
                _introScreen = null;
                break;
            }
        }

        internal Texture2D GetSignBitmap ()
        {
            return signIconTexture;
        }

        internal Texture2D GetRatKingBitmap ()
        {
            return ratKingTexture;
        }
    }
}
