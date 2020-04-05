from PIL import Image, ImageFont, ImageDraw



fontcolor = (255, 255, 255)

step_x = 8
step_y = 8

start_char = 32
last_char = 127

columns = 8
rows = (last_char + 1 - start_char) // columns

im = Image.new('RGBA', (columns * step_x, rows * step_y))

font = ImageFont.truetype("PrintChar21.ttf", 8)
draw = ImageDraw.Draw(im)


#draw.text((10, 10), "HELLO SHRDLU", font=font, fill=fontcolor)
for i in range(start_char, last_char + 1):
    nc = i - start_char
    xcol = nc % columns
    ycol = nc // columns
    xpx = xcol * step_x
    ypx = ycol * step_y

    draw.text((xpx, ypx), chr(i), font=font, fill=fontcolor)

im.save("a2font.png")
