import led_config as LC
import pyscreenshot as ImageGrab
import mss
import time
from PIL import Image
from win32api import GetSystemMetrics
from threading import Thread

LD = LC.init()

screen_width = GetSystemMetrics(0)
screen_height = GetSystemMetrics(1)

sector_width = int(screen_width / LC.led_num)
sector_height = int(screen_height / LC.led_num)

def pil_frombytes(im):
    return Image.frombytes('RGB', im.size, im.bgra, 'raw', 'BGRX')

MAP = {}
TRANS = []

for i in range(0, LC.led_num, 1):
    TRANS.append(i)
    MAP[i] = [0,0,0]

def screen_thread():
    global TRANS

    while True:
        with mss.mss() as sct:

            im = sct.grab(sct.monitors[1])
            im = pil_frombytes(im)
            for w in range(0, LC.led_num, 1):
                sector_hor_start = w * sector_width
                sector_hor_end = (w+1) * sector_width

                summ_r = 0
                summ_g = 0
                summ_b = 0
                for h in range(0, LC.led_num, 1):
                    sector_ver_start = h * sector_height
                    sector_ver_end = (h+1) * sector_height

                    rgb = im.getpixel((sector_hor_start, sector_ver_start))
                    summ_r += rgb[0]
                    summ_g += rgb[1]
                    summ_b += rgb[2]

                mid_r = summ_r / LC.led_num
                mid_g = summ_g / LC.led_num
                mid_b = summ_b / LC.led_num

                mid_rgb = [mid_r, mid_g, mid_b]

                try:
                    TRANS[w] = LD.transition(MAP[w], mid_rgb, TRANS[w], 0.5)
                except Exception:
                    s = ''

                MAP[w] = mid_rgb

def transition_thread():
    global TRANS, MAP

    while True:
        for i in range(0, LC.led_num, 1):
            currentColor = LD.getTransitionColor(TRANS[i])

            if currentColor == False:
                currentColor = MAP[i]
            LD.setLED(i+1, currentColor)
        LD.updateLED()

t1 = Thread(target=screen_thread)
t1.start()

t2 = Thread(target=transition_thread)
t2.start()
