from __future__ import print_function
import psutil
import time
import led_config as LC

LD = LC.init()

print('__________________________________________')

x = 0
frame = 0
tr = 0

COLORS = [
    [37,0,106],
    [19,0,53]
]

currentColor = COLORS[0]
lastColor = [0,0,0]

while True:
    frame += 1
    currentColor = LD.getTransitionColor(tr)
    if currentColor == False:
        tr = LD.transition(lastColor, COLORS[x], tr, 0.5)
        currentColor = LD.getTransitionColor(tr)
        x += 1
        if x > len(COLORS) - 1:
            x = 0
            frame = 1

    lastColor = currentColor

    print('Frame: '+str(frame)+'   ', end='\r')
    LD.setAllLED(currentColor)
    LD.updateLED()


    time.sleep(0.01)
