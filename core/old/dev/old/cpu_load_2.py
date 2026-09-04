from __future__ import print_function
import psutil
import time
import led_config as LC

LD = LC.init()

print('__________________________________________')

prev_color = [0,0,0]
tr = 0
m = 0
cpu_load = 0
fcolor = False
console_color = ''
future = 0

while True:
    now = time.time()
    if now > future:
        cpu_load = psutil.cpu_percent()
        future = now + 0.5
        m = 0
    if cpu_load > 20 and cpu_load < 70:
        color = [0,255,0]
        console_color = LD.Style.BRIGHT+LD.Fore.GREEN
    else:
        color = [255,0,0]
        console_color = LD.Style.BRIGHT+LD.Fore.RED
    if cpu_load < 20:
        color = [0,0,255]
        console_color = LD.Style.BRIGHT+LD.Fore.BLUE

    if color != prev_color:
        tr = LD.transition(prev_color, color, tr)
    prev_color = color

    fcolor = LD.getTransitionColor(tr)
    if fcolor == False:
        fcolor = color

    print('CPU Load: '+console_color+str(cpu_load)+'%   '+LD.Style.BRIGHT+LD.Fore.WHITE, end='\r')
    LD.animate(fcolor, 1, LD.count(), 1) #animate from left to right
    LD.updateLED()

    time.sleep(0.02)
