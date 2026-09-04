import easyaudio as AUD #Audio Visual lib
import led_config as LC

LD = LC.init()

print('__________________________________________')

x = 0 #Color offset
anim_future = 0
movement_future = 0

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
    [255,0,0],
    [0,0,255],
    [0,0,255]
], 512)

while True: #Infinite loop
    if x > len(GRADIENT) - 1:
        x = 0
    now = LD.time.time()
    if now > anim_future:
        LD.animate(GRADIENT[x], 1, LD.count(), 1)
        LD.updateLED()
        anim_future = now + 0.02
    if now > movement_future:
        x += 1
        movement_future = now + 0.1
    LD.time.sleep(0.02)
