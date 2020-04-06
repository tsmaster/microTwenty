from reportlab.pdfgen import canvas
from reportlab.lib.units import inch
from reportlab.lib.pagesizes import letter, landscape
import math

# Primarily using "IJ" coordinates, which are basically axial
# coordinates from https://www.redblobgames.com/grids/hexagons/ where
# I is 60 degrees CW from North (2 o'clock), and J is North (12
# o'clock).

def IJToXY(i, j):
    y = j + i / 2.0
    x = math.sqrt(3) * i / 2.0
    return (x,y)

# Convert to "cubic" coordinates, where x+y+z = 0. Since we're
# starting with axial coordinates, the conversion is relatively
# easy. The gotcha is that we can't just take I => x, J => -z and be
# done; the vector sum of those is constructed so that there's no y
# coordinate (y = 0), so the sum invariant is typically violated. But
# it's easy enough to find out the amount of error and distribute it
# evenly to all coordinates.

def IJToXYZ(i, j):
    x = i
    z = -j
    y = 0

    s = x + y + z

    f = s // 3
    x -= f
    y -= f
    z -= f

    return (x,y,z)

# Draw an actual line specified in IJ space on the canvas.

def drawIJLine(canvas, ij1, ij2, hexRad):
    #print ij1, ij2
    x1, y1 = IJToXY(*ij1)
    x2, y2 = IJToXY(*ij2)

    canvas.line(x1 * hexRad, y1 * hexRad,
                x2 * hexRad, y2 * hexRad)


# generate line-tuples and add them to the lineList if the tuple isn't
# already in the list.
    
def drawHex(point, halfLongDiagonalSize, lineList):
    ic, jc = point
    
    verts = [(1,0), (0,1),
             (-1, 1), (-1, 0),
             (0, -1), (1, -1)]

    hRad = halfLongDiagonalSize
    
    for i in range(6):
        i1, j1 = verts[i-1]
        i2, j2 = verts[i]

        i1 += ic
        j1 += jc
        i2 += ic
        j2 += jc

        if i2 < i1:
            i1, j1, i2, j2 = i2, j2, i1, j1

        if ((i1, j1, i2, j2) not in lineList):
            lineList.append((i1, j1, i2, j2))

# (sigh) doesn't actually draw a line. Instead, iterates over a
# horizontal stripe of tiles, and adds the boundary lines into
# lineList, if they're not already there.
            
def drawLine(si, sj, hexRad, lineList):
    for mult in range(-20, 20):
        drawHex((si + mult * 2, sj + mult * -1), hexRad, lineList)

# Similar to drawLine, except that it DOES draw the labels for all the
# tiles that drawLine prepares.
        
def drawLabels(canvas, si, sj, hexRad):
    canvas.setFont("Helvetica", 4)
    
    for mult in range(-20, 20):
        mi = si + mult * 2
        mj = sj + mult * -1
        
        xyz = IJToXYZ(mi, mj)

        xy = IJToXY(mi, mj)

        cx, cy = xy

        hx, hy, hz = xyz

        LABEL_FIVES = True
        LABEL_ORIGIN = True

        labelTile = False
        
        if LABEL_FIVES:
            if (((hx % 5) == 0) and
                ((hy % 5) == 0) and
                ((hz % 5) == 0)):
                labelTile = True

                
        if LABEL_ORIGIN:
            od = 2
            
            if ((abs(hx) <= od) and
                (abs(hy) <= od) and
                (abs(hz) <= od)):
                labelTile = True

        if (labelTile):
            LABEL_IJ = False

            if LABEL_IJ:
                label = "I:{0} J:{1}".format(mi, mj)
            else:
                label = "({0} {1} {2})".format(hx, hy, hz)

            canvas.drawCentredString(cx * hexRad, cy * hexRad, label)

        
    
# draws the whole grid, one line at a time, trying not to overdraw
# line segments.

def drawGrid(canvas, size, lineWidth, color, hexRad):
    canvas.setStrokeColorRGB(*color)
    canvas.setLineWidth(lineWidth)
    width, height = size

    yc = height / 2.0
    xc = width / 2.0

    canvas.saveState()
    canvas.translate(xc, yc)

    lineList = []

    for v in range(-20, 20):
        drawLine(0, 3 * v, hexRad, lineList)
        drawLine(1, 1 + 3 * v, hexRad, lineList)

        drawLabels(canvas, 0, 3 * v, hexRad)
        drawLabels(canvas, 1, 1 + 3 * v, hexRad)

    for i1, j1, i2, j2 in lineList:
        drawIJLine(canvas, (i1, j1), (i2, j2), hexRad)

    iVec = IJToXY(hexRad * 3, 0)
    #canvas.line(0, 0, iVec[0], iVec[1])
    jVec = IJToXY(0, hexRad * 3)
    #canvas.line(0, 0, jVec[0], jVec[1])
    
    canvas.restoreState()
        
    

c = canvas.Canvas("hex_solid_050inch_landscape.pdf", pagesize=landscape(letter))

drawGrid(c, landscape(letter), 0.1, (0,0,0), 0.50 * inch / 2.0)
c.showPage()
c.save()

