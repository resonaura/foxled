import led_config as LC

LD = LC.init()

GRADIENT = []
COLOR = [0,255,0]
SPEED = 10
PERCENT = 100

while True: #Infinite loop
    for x in LD.allLED(): #first all led's loop
        PERCENT -= SPEED

        if PERCENT <= 0:
            PERCENT = 0
            SPEED = -SPEED
        if PERCENT >= 100:
            PERCENT = 100
            SPEED = -SPEED

        r = (COLOR[0] * PERCENT) / 100
        g = (COLOR[1] * PERCENT) / 100
        b = (COLOR[2] * PERCENT) / 100

        LD.animate([r, g, b], 0, LD.count(), 1)

        LD.time.sleep(0.02) #sleep 20ms

        LD.updateLED() #updateLED to see results
