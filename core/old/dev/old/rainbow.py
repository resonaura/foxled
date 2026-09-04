import led_config as LC

LD = LC.init()

x = 0 #offset
GRADIENT = LD.generateGradientMap(LD.RAINBOW, LD.count())

while True: #Infinite loop
    x += 1
    if x > (len(GRADIENT) - 1):
        x = 0

    LD.animate(GRADIENT[x], 0, LD.count(), 1)
    LD.time.sleep(0.03) #sleep 30ms

    LD.updateLED() #updateLED to see results
