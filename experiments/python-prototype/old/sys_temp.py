# -*- coding: cp866 -*-
from __future__ import print_function
import wmi
import time
import led_config as LC

LD = LC.init()

print('__________________________________________')

prev_color = [0,0,0]
tr = 0
m = 0
temperature = 0
fcolor = False
console_color = ''

while True:
    m += 1
    if m > 255:
        w = wmi.WMI(namespace="root\wmi")
        temperature_info = w.MSAcpi_ThermalZoneTemperature()[0]
        temperature = int(str(temperature_info.CurrentTemperature)[2:])
        m = 0
    if temperature > 20 and temperature < 50:
        color = [0,255,0]
        console_color = LD.Style.BRIGHT+LD.Fore.GREEN
    else:
        color = [255,0,0]
        console_color = LD.Style.BRIGHT+LD.Fore.RED
    if temperature < 20:
        color = [0,0,255]
        console_color = LD.Style.BRIGHT+LD.Fore.BLUE

    if color != prev_color:
        tr = LD.transition(prev_color, color, tr)
    prev_color = color

    fcolor = LD.getTransitionColor(tr)
    if fcolor == False:
        fcolor = color

    print('System temperature: '+console_color+str(temperature)+'�C   '+LD.Style.BRIGHT+LD.Fore.WHITE, end='\r')
    LD.setAllLED(fcolor)
    LD.updateLED()

    time.sleep(0.005)
