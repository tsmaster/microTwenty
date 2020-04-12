class Hex3Coord:
    
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z


    def __add__(self, other):
        nx = self.x + other.x
        ny = self.y + other.y
        nz = self.z + other.z

        return Hex3Coord(nx, ny, nz)

    def __str__(self):
        return "[{0} {1} {2}]".format(self.x, self.y, self.z)

NW = Hex3Coord(0, 1, -1)
NE = Hex3Coord(1, 0, -1)
E = Hex3Coord(1, -1, 0)
SE = Hex3Coord(0, -1, 1)
SW = Hex3Coord(-1, 0, 1)
W = Hex3Coord(-1, 1, 0)

def makeTile(coord, outFile, tileType):
    outFile.write("      {0}({1}, {2}, {3});\n".format(tileType, coord.x, coord.y, coord.z))

def makeObject(coord, outFile, objType):
    outFile.write("      {0}({1}, {2}, {3});\n".format(objType, coord.x, coord.y, coord.z))

def makeMountain(coord, outFile):
    makeTile(coord, outFile, 'Mountain')

def makePlain(coord, outFile):
    makeTile(coord, outFile, 'Plain')

def makeCity(coord, outFile):
    makeTile(coord, outFile, 'City')

def makeWater(coord, outFile):
    makeTile(coord, outFile, 'Water')

def makeWaterMount(coord, outFile):
    makeTile(coord, outFile, 'WaterMount')

def makePortal(coord, outFile):
    makeTile(coord, outFile, 'Portal')

def makeTree(coord, outFile):
    makeTile(coord, outFile, 'Plain')
    makeObject(coord, outFile, 'Tree')

def makeRockWall(coord, outFile):
    makeTile(coord, outFile, 'RockWall')

def makeWoodWall(coord, outFile):
    makeTile(coord, outFile, 'WoodWall')
    
def makeBuilding(coord, outFile):
    makeTile(coord, outFile, 'Building')

def makeBuildingEntrance(coord, outFile):
    makeTile(coord, outFile, 'BuildingEntrance')

def makeDock(coord, outFile):
    makeTile(coord, outFile, 'Dock')

def makeShip(coord, outFile):
    makeTile(coord, outFile, 'Water')
    makeObject(coord, outFile, 'Ship')
    
tileMap = {
    'A': makeMountain,
    '.': makePlain,
    '*': makeCity,
    '~': makeWater,
    '^': makeWaterMount,
    'O': makePortal,
    't': makeTree,
    '#': makeRockWall,
    '=': makeWoodWall,
    'B': makeBuilding,
    'E': makeBuildingEntrance,
    'D': makeDock,
    's': makeShip,
    }

def writeHeader(className, episodeName, f):
    f.write("// This file was auto-generated\n\n")

    f.write("using MicroTwenty;\n\n")

    f.write("namespace MicroTwenty {\n")
    f.write("  class %s : HexMap {\n" % className)
    f.write("    public %s (GameMgr gameMgr) : base(gameMgr) {\n" % className)
    f.write("      LayoutMap();\n")
    f.write("    }\n\n")
    f.write("    public override string Name () {\n")
    f.write('      return "{0}";\n'.format(episodeName))
    f.write("    }\n\n")
    f.write("    public override void LayoutMap() {\n");

def writeFooter(f):
    f.write("    }\n")
    f.write("  }\n")
    f.write("}\n\n")
    f.write("// This file was auto-generated\n\n")
    
def compileMap(inFileName, outFileName, outName, className):
    print("Episode", inFileName)
    pageStart = Hex3Coord(-1, 9, -8)
    with open(inFileName) as inFile:
        with open(outFileName, "wt") as outFile:
            writeHeader(className, outName, outFile)
            lineStart = pageStart
            for lineNum, line in enumerate(inFile.readlines()):
                if "|" in line:
                    barIndex = line.index("|")
                    line = line[:barIndex]
                
                line = line.strip()
                if not line:
                    continue

                if line[0] == ':':
                    if line.startswith(":NW="):
                        hc = line[5:]
                        closeParenIndex = hc.index(")")
                        hc = hc[:closeParenIndex]
                        coords = hc.split(",")
                        xc = int(coords[0])
                        yc = int(coords[1])
                        zc = int(coords[2])
                        pageStart = Hex3Coord(xc, yc, zc)
                        lineStart = pageStart
                        continue

                #print ("line #", lineNum)
                curPos = lineStart
                for cn, c in enumerate(line):
                    if c == ' ':
                        continue
                    colNum = cn // 2
                    #print (lineNum, colNum, curPos, c)
                    if c in tileMap:
                        tileMap[c](curPos, outFile)
                    else:
                        print ("bad char:", c)
                    curPos = curPos + E

                if lineNum % 2 == 0:
                    lineStart = lineStart + SW
                else:
                    lineStart = lineStart + SE
                outFile.write("\n");
            writeFooter(outFile)
    
filedescs = [("ep_1.txt", "ep_1.cs", "ep_1", "Episode1Map"),
             ("ep_2.txt", "ep_2.cs", "ep_2", "Episode2Map"),
             ("ep_3.txt", "ep_3.cs", "ep_3", "Episode3Map"),
             ("ep_4.txt", "ep_4.cs", "ep_4", "Episode4Map"),
             ("ep_5.txt", "ep_5.cs", "ep_5", "Episode5Map"),
             ("ep_6.txt", "ep_6.cs", "ep_6", "Episode6Map"),
             ("ep1c_rycroft.txt", "ep1c_rycroft.cs", "ep1c_rycroft", "Ep1CityRycroftMap"),
             ("ep1d_rathole.txt", "ep1d_rathole.cs", "ep1d_rathole", "Ep1DungeonRatHoleMap"),
             ("combat.txt", "combatmap.cs", "combat", "CombatMap"),
             ("bigcombat.txt", "bigcombatmap.cs", "bigcombat", "BigCombatMap"),
             ("ratIsland.txt", "ratisland.cs", "ratisland", "RatIsland"),
             
             ("caverns.txt", "cavernsmap.cs", "caverns", "CavernsMap"),
             ("docks.txt", "docksmap.cs", "docks", "DocksMap"),
             ("labyrinth.txt", "labyrinthmap.cs", "labyrinth", "LabyrinthMap"),
             ("river_crossing.txt", "rivercrossingmap.cs", "river_crossing", "RiverCrossingMap"),
             ("town.txt", "townmap.cs", "town", "TownMap"),
]

for d in filedescs:
    infn, outfn, outname, classname = d
    compileMap(infn, outfn, outname, classname)

             
