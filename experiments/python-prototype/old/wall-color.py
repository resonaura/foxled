import _winreg
import time
from colorthief import ColorThief
import os
import led_config as LC

LD = LC.init()

lastFilesize = 0
color = [0,0,0]
lastColor = [0,0,0]
prevColor = [0,0,0]
tr = 1

print('__________________________________________')

def getWindowsWall():
    key = _winreg.OpenKey(_winreg.HKEY_CURRENT_USER, r"Control Panel\Desktop", 0, _winreg.KEY_READ)
    return _winreg.QueryValueEx(key, "Wallpaper")[0]
def getOptimalColorFromPalette(palette):
    optimal = False
    lastDifference = -255

    for i in LD.erange(0, len(palette) - 1, 1):
        r = palette[i][0]
        g = palette[i][1]
        b = palette[i][2]

        sort_colors = [r, g, b]
        sort_colors.sort()

        difference = sort_colors[2] - sort_colors[1] - sort_colors[0]


        if difference > lastDifference:
            optimal = palette[i]
            lastDifference = difference
    if optimal == False:
        optimal = palette[0]
    return optimal
def getImageColor(img):
    global color
    try:
        color_thief = ColorThief(img)
        palette = color_thief.get_palette(color_count=20, quality=30)
        dominant_color = getOptimalColorFromPalette(palette)
        return [dominant_color[0], dominant_color[1], dominant_color[2]]
    except Exception:
        return False
def getFileSize(file):
    try:
        return os.path.getsize(file)
    except Exception:
        return 0

def getStableWallColor(wall):
    global recursion_count
    c = getImageColor(wall)
    if c == False:
        try:
            return getStableWallColor(wall)
        except Exception:
            return False
    else:
        return c
while True:
    wall = getWindowsWall()
    filesize = getFileSize(wall)

    if filesize != lastFilesize and filesize > 0:
        newcolor = getStableWallColor(wall)
        if newcolor != False:
            if newcolor != prevColor:
                prevColor = color
                color = newcolor
                print 'Color updated to '+str(color)
            lastFilesize = filesize

    currentColor = LD.getTransitionColor(tr)

    if currentColor == False:
        tr = LD.transition(prevColor, color, tr, 1)
        prevColor = color
        currentColor = LD.getTransitionColor(tr)

    LD.setAllLED(currentColor)
    LD.updateLED()
    time.sleep(0.02)
