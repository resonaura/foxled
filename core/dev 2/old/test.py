import psutil
import time
import led_config as LC

LD = LC.init()

GRADIENT = LD.generateGradientMap(LD.RAINBOW, LD.count())

while True:
    for i in LD.erange(0, len(GRADIENT) - 1, 1):
        LD.setLED(i + 1, GRADIENT[i])

    LD.updateLED()
    time.sleep(0.005)
