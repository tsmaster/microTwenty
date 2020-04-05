from reportlab.pdfgen import canvas
from reportlab.lib.units import inch
from reportlab.lib.pagesizes import letter

import math

SLOPE = math.sqrt(3.0)

def IJToXY(i, j):
    y = j + i / 2.0
    x = math.sqrt(3) * i / 2.0
    return (x,y)

def drawIJLine(canvas, ij1, ij2):
    #print ij1, ij2
    x1, y1 = IJToXY(*ij1)
    x2, y2 = IJToXY(*ij2)

    canvas.line(x1, y1, x2, y2)


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
        if ((i1,j1,i2,j2) not in lineList):
            lineList.append((i1 * hRad, j1 * hRad, i2 * hRad, j2 * hRad))

def drawLine(si, sj, hexRad, lineList):
    for mult in range(-20, 20):
        drawHex((si + mult * 2, sj + mult * -1), hexRad, lineList)


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


    for i1, j1, i2, j2 in lineList:
        drawIJLine(canvas, (i1, j1), (i2, j2))

    canvas.restoreState()
        
    

c = canvas.Canvas("hex_solid_075inch.pdf", pagesize=letter)

drawGrid(c, letter, 0.1, (0,0,0), 0.75 * inch / 2.0)
c.showPage()
c.save()

