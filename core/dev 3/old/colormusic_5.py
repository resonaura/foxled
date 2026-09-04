import easyaudio as AUD #Audio Visual lib
import led_config as LC

LD = LC.init()
AUD.connect(True) #Connect Audio

print('__________________________________________')

x = 0 #Color offset

lastPitch = 0
GRADIENT = LD.generateGradientMap([
    [255,0,0],
    [0,0,255],
    [0,0,255],
    [255,0,0],
    [255,0,0],
    [0,0,255],
    [0,0,255],
    [255,0,0],
    [255,0,255],
    [0,0,255],
    [0,0,255],
    [255,0,255],
    [255,0,255],
    [0,0,255],
    [0,0,255],
    [255,0,0],
    [255,255,0],
    [255,255,0],
    [0,255,0],
    [255,255,0],
    [255,255,0],
    [0,255,0],
    [255,0,0],
    [255,0,0]
], 512)

future = 0

while True: #Infinite loop
    data = AUD.getData() #Get sound data

    spectre = AUD.getSpectre(data, LD.count() * 2)
    for i in range(0, LD.count(), 1):
        percent = int(spectre[i] * 100 / 255)

        percent = percent * 10
        if percent > 100:
            percent = 100

        now = LD.time.time()
        if now > future:
            x += 1
            if x > len(GRADIENT) - 1:
                x = 0
            LD.pseudo_animate(GRADIENT[x], 1, LD.count(), 1)
            LD.updatePseudoLED()
            y = 0
            future = now + 0.02


        CUR_C = LD.get_pseudo_pixel(i)
        r = CUR_C[0] * percent / 100
        g = CUR_C[1] * percent / 100
        b = CUR_C[2] * percent / 100


        pw = LD.autoFadeOne(i + 1, [r, g, b], 1)

        if i < LD.count():
            LD.setLED(i + 1, pw)

    LD.updateLED() #updateLED to see results
