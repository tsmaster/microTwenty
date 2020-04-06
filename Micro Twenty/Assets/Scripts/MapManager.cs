using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        SPRITE_TILE_CURSOR = 14,
        SPRITE_TILE_SHIP = 15,
        SPRITE_COMBAT_GUY = 16,
        SPRITE_COMBAT_RAT_MAN = 17,
        SPRITE_COMBAT_GUY_1 = 18,
        SPRITE_COMBAT_GUY_2 = 19,
        SPRITE_COMBAT_GUY_3 = 20,
        SPRITE_COMBAT_GUY_4 = 21,
        SPRITE_COMBAT_GUY_5 = 22,
        SPRITE_COMBAT_GUY_6 = 23,
        SPRITE_COMBAT_SNAKE = 24,
        SPRITE_COMBAT_DOG = 25,
        SPRITE_COMBAT_CAT = 26,
        SPRITE_COMBAT_CRAB = 27,
        SPRITE_COMBAT_GHOST = 28,
        SPRITE_COMBAT_DJINN = 29,
    };

    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private Texture2D hexTileSheet;

        [SerializeField]
        private Texture2D fontBitmap;

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
                new CombatMap(_gameMgr)
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
        }

        private void AddLevelHacks ()
        {
            int i = GetMapByName ("ep_1");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (5, -8, 3), "ep_2", new HexCoord (-6, 3, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (0, -4, 4), "ep1c_rycroft", new HexCoord (0, 0, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, -8, 7), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, 2, -3), "ep1d_rathole", new HexCoord (0, -2, 2)));
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (-2, 3, -1), "combat", new HexCoord(0, 0, 0)));

            i = GetMapByName ("ep_2");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-6, 3, 3), "ep_1", new HexCoord (5, -8, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (5, -8, 3), "ep_3", new HexCoord (-6, 3, 3)));

            i = GetMapByName ("ep_3");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-6, 3, 3), "ep_2", new HexCoord (5, -8, 3)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, -1, 0), "ep_4", new HexCoord (1, 1, -2)));

            i = GetMapByName ("ep_4");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (1, 1, -2), "ep_3", new HexCoord (1, -1, 0)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 1, 8), "ep_5", new HexCoord (2, -10, 8)));

            i = GetMapByName ("ep_5");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (2, -10, 8), "ep_4", new HexCoord (-9, 1, 8)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-9, 1, 8), "ep_6", new HexCoord (2, -10, 8)));

            i = GetMapByName ("ep_6");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (2, -10, 8), "ep_5", new HexCoord (-9, 1, 8)));

            i = GetMapByName ("ep1c_rycroft");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (4, 4, -8), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-8, 3, 5), "ep_1", new HexCoord (0, -4, 4)));
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (-1, -10, 11), "ep_1", new HexCoord (1, -7, 6)));

            i = GetMapByName ("ep1d_rathole");
            maps [i].dynamicObjects.Add (new TeleportTrigger (_gameMgr, new HexCoord (0, -2, 2), "ep_1", new HexCoord (1, 2, -3)));
            maps [i].dynamicObjects.Add (new CombatTrigger (_gameMgr, new HexCoord (-1, 6, -5), "combat", new HexCoord(0, 0, 0)));

            i = GetMapByName ("combat");
            // dynamic objects handled by combatMgr
        }

        public List<DynamicObject> GetDynamicObjectsAt (HexCoord hc)
        {
            var outList = new List<DynamicObject> ();
            foreach (var dynObj in dynamicObjects) {
                if (dynObj.hexCoord.SamePos (hc)) {
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
            _gameMgr.Update (deltaSeconds);

            if ((_combatMgr != null) &&
                (_combatMgr.InCombat)) {
                _combatMgr.Update (deltaSeconds);
                drawn = false;
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

        private void DrawStringAt (string message, int x, int y, Color color)
        {
            for (int i = 0; i < message.Length; ++i) {
                var pos_x = i * 6 + x;
                var pos_y = y;

                char c = message [i];
                int coff = (int)c - 32;

                var c_col = coff % 8;
                var c_row = 11 - coff / 8;

                DrawTintedPartialSprite (targetTexture, fontBitmap, pos_x, pos_y, c_col * 8, c_row * 8, 8, 8, color);
            }
        }


        private void DrawText ()
        {
            var msg = GetHexMap ().Name();

            TextureDrawing.DrawRect (targetTexture, 4, targetTextureHeight - 14, (msg.Length + 1) * 6, 12, Color.black, Color.green, true, true);

            DrawStringAt (msg, 6, targetTextureHeight - 12, Color.white);

            targetTexture.Apply ();
        }

        public HexMap GetHexMap () {
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
                if (dynObj.hexCoord.SamePos (newPos)) {
                    if (dynObj.blocksMovement) {
                        return;
                    }
                }
            }

            SetPlayerPos (newPos);
            Debug.LogFormat ("Moved to {0}", playerPos.ToString());

            foreach (var dynObj in GetHexMap ().dynamicObjects) {
                if (dynObj.hexCoord.SamePos (newPos)) {
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
            const int sheet_columns = 6;
            const int sheet_rows = 6;
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

            foreach (var t in maps[selectedMap].tiles) {
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

                DrawTintedPartialSprite (targetTexture, hexTileSheet, px, py, source_x, source_y, tile_width, tile_height, tile_tint);
            }

            foreach (var dynobj in dynamicObjects) {
                var combObj = dynobj as CombatantSprite;
                if (combObj != null) {
                    int team = combObj.GetTeam ();
                    Color c = team == 0 ? Color.cyan : Color.red;
                    DrawTintedSpriteAtLocation (combObj.GetSpriteId (), dynobj.hexCoord, c);
                } else {
                    DrawSpriteAtLoc (dynobj.objectType, dynobj.hexCoord, dynobj);
                }
            }

            foreach (var dynobj in maps[selectedMap].dynamicObjects) {
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

            DrawPartialSprite (targetTexture, hexTileSheet, screenPosX, screenPosY, sprite_x, sprite_y, 16, 16);
        }

        public void DrawTintedSpriteAtPos (SpriteId spriteId, int screenPosX, int screenPosY, Color c)
        {
            GetSpriteCoords (spriteId, out int sprite_x, out int sprite_y);

            DrawTintedPartialSprite (targetTexture, hexTileSheet, screenPosX, screenPosY, sprite_x, sprite_y, 16, 16, c);
        }

        public void DrawTintedSpriteAtLocation (SpriteId spriteId, HexCoord hexCoord, Color c)
        {
            HexCoordToScreenCoords (hexCoord, out int px, out int py);
            GetSpriteCoords (spriteId, out int source_x, out int source_y);
            DrawTintedPartialSprite (targetTexture, hexTileSheet, px, py, source_x, source_y, 16, 16, c);
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
            default:
                Debug.LogFormat ("drawing unknown dynamic object: {0}", objType);
                has_sprite = false;
                break;
            }

            if (!has_sprite) {
                return;
            }

            DrawPartialSprite (targetTexture, hexTileSheet, px, py, source_x, source_y, 16, 16);
        }

        private void DrawPartialSprite (Texture2D targetTexture, Texture2D sourceTexture, int tx, int ty, int sx, int sy, int width, int height)
        {
            var targpixels = targetTexture.GetPixels (tx, ty, width, height);
            var sourcePixels = sourceTexture.GetPixels (sx, sy, width, height);

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    Color c = sourcePixels [x + y * width];
                    if (c.a < 1.0f / 256) {
                        continue;
                    }
                    targpixels [x + y * width] = c;
                }
            }
            targetTexture.SetPixels (tx, ty, width, height, targpixels);
        }

        private void DrawTintedPartialSprite (Texture2D targetTexture, Texture2D sourceTexture, int tx, int ty, int sx, int sy, int width, int height, Color tint)
        {
            var targpixels = targetTexture.GetPixels (tx, ty, width, height);
            var sourcePixels = sourceTexture.GetPixels (sx, sy, width, height);

            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    Color c = sourcePixels [x + y * width];
                    if (c.a < 1.0f / 256) {
                        continue;
                    }
                    targpixels [x + y * width] = c * tint;
                }
            }
            targetTexture.SetPixels (tx, ty, width, height, targpixels);
        }

        internal void TeleportPlayer (string destMapName, HexCoord destMapCoord)
        {
            int foundMapIndex = -1;
            for (int i = 0; i < maps.Count; ++i) {
                if (maps [i].Name() == destMapName) {
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
            SetPlayerPos(destMapCoord);
        }

        internal void EnterCombat ()
        {
            if (_combatMgr == null) {
                _combatMgr = new CombatMgr (this, _gameMgr);
            }
            _combatMgr.InCombat = true;
        }
    }
}
