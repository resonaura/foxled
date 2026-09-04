#import main libs
from __future__ import print_function
import sys
import os
import telnetlib
import time
import serial
import serial.tools.list_ports
import struct
import win32api
import win32con

s = ''
comLED = False

from colorama import init
init()
from colorama import Fore, Back, Style

def getByte(source):
    source = int(source)
    if source > 255:
        source = 255
    if source < 0:
        source = 0
    return struct.pack('B', source)

_adaheader = ''
UsedCOM = ''

def adaInit():
    global s, UsedCOM
    comlist = serial.tools.list_ports.comports()

    for element in comlist:
        s = serial.Serial(element.device, 115200, timeout=2)
        UsedCOM = element.device
        m = s.readline()

        if m != bytes('Ada\n', 'utf-8'):
            s.close()


    s.write(_adaheader)

#Arrays to store LEDs and Animation
LED = []
ANIM = []
PSEUDO_ANIM = []
PSEUDO_LED = []
lastNLED = []
lastNColor = [0,0,0]
TRANS_A = {}

updates = 0
isConnected = False

#Color Maps
RAINBOW_GRADIENT = [
    [255, 44, 0],
    [252, 118, 0],
    [252, 198, 0],
    [255, 244, 0],
    [247, 252, 0],
    [206, 254, 0],
    [135, 251, 0],
    [60, 252, 0],
    [6, 251, 0],
    [0, 254, 3],
    [0, 251, 46],
    [0, 252, 120],
    [0, 252, 195],
    [0, 252, 247],
    [0, 246, 252],
    [0, 207, 255],
    [0, 132, 255],
    [0, 54, 255],
    [0, 7, 255],
    [5, 0, 252],
    [46, 0, 252],
    [115, 0, 255],
    [195, 0, 250],
    [247, 0, 251],
    [255, 0, 250],
    [252, 0, 203],
    [252, 0, 132],
    [255, 0, 58],
    [255, 0, 5],
    [255, 3, 3],
    [255, 44, 0],
    [252, 118, 0],
    [252, 198, 0],
    [255, 244, 0],
    [247, 252, 0],
    [206, 254, 0],
    [135, 251, 0],
    [60, 252, 0],
    [6, 251, 0],
    [0, 254, 3],
    [0, 251, 46],
    [0, 252, 120],
    [0, 252, 195],
    [0, 252, 247],
    [0, 246, 252],
    [0, 207, 255],
    [0, 132, 255],
    [0, 54, 255],
    [0, 7, 255],
    [5, 0, 252],
    [46, 0, 252],
    [115, 0, 255],
    [195, 0, 250],
    [247, 0, 251],
    [255, 0, 250],
    [252, 0, 203],
    [252, 0, 132],
    [255, 0, 58],
    [255, 0, 5],
    [255, 3, 3]
]
RAINBOW = [
    [255,0,0],
    [255,255,0],
    [0,255,0],
    [0,255,255],
    [0,0,255],
    [255,0,255],
    [255,0,0]
]

#important variables
tn = ''
speed = 5 #fadeOutSpeed
log = False

#Range functions
def erange(start, end, step):
    while start <= end:
        yield start
        start += step
def erange_d(start, end, step):
    while start >= end:
        yield start
        start -= step

CURSOR_UP_ONE = '\x1b[1A'
ERASE_LINE = '\x1b[2K'

error_showed = False
block_errors = False
enable_led = True

#Function to init led
def initLED(host, port, count, logging = False):
    global enable_led, isConnected, error_showed, log, comLED, tn, PSEUDO_LED, LED, ANIM, PSEUDO_ANIM, lastNLED, lastNColor, TRANS_A, comLED, log #Get global variable for telnet
    error_showed = False
    LED = []
    ANIM = []
    PSEUDO_ANIM = []
    PSEUDO_LED = []
    lastNLED = []
    lastNColor = [0,0,0]
    TRANS_A = {}
    comLED = False
    log = logging
    if log != False:
        os.system('color 0f')
        os.system('title EasyLED')
        print('Connecting AmbiBox...      ', end='\r')

    if enable_led != False:
        tn = telnetlib.Telnet(host,port) #connect telnet
        tn.read_until(bytes('version: 1.3 (enter "help" for more info)', 'utf-8')) #Wait for successfuly connection
        tn.write(bytes('lock\n', 'utf-8')) #lock led
        tn.read_until(bytes('lock:success', 'utf-8')) #wait for ok
        tn.write(bytes('setsmooth:100;\n', 'utf-8')) #smooth
    if log != False:
        print("Connecting AmbiBox... "+Style.BRIGHT+Fore.GREEN+"success!"+Fore.WHITE)
        print('LED map initialization...', end='\r')
    for a in erange(1, count, 1): #build LED MAP
        LED.append([0,0,0]) #add new led to array
        lastNLED.append([0,0,0]) #add new led to array
        PSEUDO_LED.append([0,0,0]) #add new led to array
    if log != False:
        print("LED map initialization... "+Style.BRIGHT+Fore.GREEN+"done! "+Fore.WHITE)
    isConnected = True
    return True

def initLocalLED(count, logging = False):
    global enable_led, isConnected, error_showed, log, comLED, tn, PSEUDO_LED, LED, ANIM, PSEUDO_ANIM, lastNLED, lastNColor, TRANS_A, comLED, log, _adaheader #Get global variable for telnet
    error_showed = False
    LED = []
    ANIM = []
    PSEUDO_ANIM = []
    PSEUDO_LED = []
    lastNLED = []
    lastNColor = [0,0,0]
    TRANS_A = {}
    comLED = True
    log = logging
    if log != False:
        os.system('color 0f')
        os.system('title EasyLED')
        print('Connecting COM...', end='\r')


    _adaheader = bytes('Ada', 'utf-8') + getByte(0) + getByte(count - 1)
    if enable_led != False:
        adaInit()

    if log != False:
        print("Connecting COM... "+Style.BRIGHT+Fore.GREEN+"success!"+Fore.WHITE)
        print('LED map initialization...', end='\r')
    for a in erange(1, count, 1): #build LED MAP
        LED.append([0,0,0]) #add new led to array
        lastNLED.append([0,0,0]) #add new led to array
        PSEUDO_LED.append([0,0,0]) #add new led to array
    if log != False:
        print("LED map initialization... "+Style.BRIGHT+Fore.GREEN+"done!"+Fore.WHITE)
    isConnected = True
    return True

def rebuildMap(count):
    global error_showed, log, comLED, tn, PSEUDO_LED, LED, ANIM, PSEUDO_ANIM, lastNLED, lastNColor, TRANS_A, comLED, log, _adaheader #Get global variable for telnet
    error_showed = False
    LED = []
    ANIM = []
    PSEUDO_ANIM = []
    PSEUDO_LED = []
    lastNLED = []
    lastNColor = [0,0,0]
    TRANS_A = {}

    _adaheader = bytes('Ada', 'utf-8') + getByte(0) + getByte(count - 1)
    for a in erange(1, count, 1): #build LED MAP
        LED.append([0,0,0]) #add new led to array
        lastNLED.append([0,0,0]) #add new led to array
        PSEUDO_LED.append([0,0,0]) #add new led to array
def setLED(num, color): #function to set led color
    global LED #get LED MAP
    LED[int(num)-1] = color #set to led by num
def setAllLED(color): #function to set all led's color
    global LED #get LED MAP
    for led in erange(1, len(LED), 1): #start loop
        LED[led-1] = color #set color to current led in loop
def transition(start_rgb, end_rgb, i = len(TRANS_A), step = 1):
    try:
        del TRANS_A[i]
    except Exception:
        pass
    step_points = [step, step, step]

    start_r = start_rgb[0]
    start_g = start_rgb[1]
    start_b = start_rgb[2]

    end_r = end_rgb[0]
    end_g = end_rgb[1]
    end_b = end_rgb[2]

    if end_r != start_r:
        if end_r < start_r:
            step_points[0] = -step
    else:
        step_points[0] = 0

    if end_g != start_g:
        if end_g < start_g:
            step_points[1] = -step
    else:
        step_points[1] = 0

    if end_b != start_b:
        if end_b < start_b:
            step_points[2] = -step
    else:
        step_points[2] = 0
    TRANS_A[i] = [start_rgb, end_rgb, step_points]
    return i
def generateGradientMap(points, lenght):
    GRADIENT = []

    if lenght >= len(points):
        for i in erange(0, len(points) - 1, 1):
            point = points[i]
            point_r = point[0]
            point_g = point[1]
            point_b = point[2]

            if (i+1) < len(points):
                next_point = points[i+1]
                next_point_r = next_point[0]
                next_point_g = next_point[1]
                next_point_b = next_point[2]

                step_r = (next_point_r - point_r) / (lenght / (len(points) - 1))
                step_g = (next_point_g - point_g) / (lenght / (len(points) - 1))
                step_b = (next_point_b - point_b) / (lenght / (len(points) - 1))

                step_points = [step_r, step_g, step_b]

                for s in erange(1, lenght / (len(points) - 1), 1):
                    GRADIENT.append([point_r, point_g, point_b])
                    point_r += step_r
                    point_g += step_g
                    point_b += step_b
    return GRADIENT
def getTransitionColor(i):
    try:
        return TRANS_A[i][0]
    except Exception:
        return False
def updateLED(): #function to update led
    global enable_led, isConnected, LED, ANIM, updates, log, _adaheader, s, error_showed, block_errors, comLED, TRANS_A #get LED and ANIMation MAP

    if len(TRANS_A) > 0:
        for i in list(TRANS_A):
            try:
                cur = TRANS_A[i]
                cur_rgb = cur[0]
                cur_end_rgb = cur[1]
                cur_step_points = cur[2]

                cur_step_r = cur_step_points[0]
                cur_step_g = cur_step_points[1]
                cur_step_b = cur_step_points[2]

                cur_r = cur_rgb[0]
                cur_g = cur_rgb[1]
                cur_b = cur_rgb[2]

                cur_end_r = cur_end_rgb[0]
                cur_end_g = cur_end_rgb[1]
                cur_end_b = cur_end_rgb[2]

                if cur_r != cur_end_r:
                    cur_r += cur_step_r
                if cur_g != cur_end_g:
                    cur_g += cur_step_g
                if cur_b != cur_end_b:
                    cur_b += cur_step_b

                cur_rgb = [cur_r, cur_g, cur_b]
                TRANS_A[i][0] = cur_rgb

                if cur_rgb == cur_end_rgb:
                    del TRANS_A[i]
            except Exception:
                pass
    if len(ANIM) < 1: #if no animations
        animate([0,0,0], 1, 1, 1) #create not visible animation
    for a in erange(1, len(ANIM), 1): #proccessing all animations
        if (a - 1) < len(ANIM): #if index not out of range
            curA = ANIM[a - 1] #get current animation
            curAcolor = curA[0] #current animation color
            curApos = curA[1] #current animation position
            curAend = curA[2] #current animation end
            curAstep = curA[3] #current animation step

            curApos += curAstep #move

            if curAstep == 1: #if step is 1
                if curApos <= curAend: #if not a end
                    LED[int(curApos) - 1] = curAcolor #append to led
                else:
                    try:
                        del ANIM[a-1] #delete animation
                    except Exception:
                        pass
            else: #if step is -1
                if curApos > curAend: #if not a end
                    LED[int(curApos) - 1] = curAcolor #append to led
                else:
                    del ANIM[a-1] #delete animation
            curA[1] = curApos #set new position

    REQ_ADDT = '' #variable for store led's colors to query
    if enable_led != False:
        if comLED == False:
            for led in erange(1, len(LED), 1): #all led's loop
                REQ_ADDT += str(led)+'-'+str(LED[led-1][0])+','+str(LED[led-1][1])+','+str(LED[led-1][2])+';'
                if led < (len(LED)-1):
                    REQ_ADDT += ' ' #if not last add space
            try:
                tn.write(bytes('setcolor: '+str(REQ_ADDT)+'\n', 'utf-8')) #run query
            except Exception:
                isConnected = False
                if error_showed == False:
                    select = win32api.MessageBox(0, 'Unfortunately, the connection failed. Retry?', 'EasyLED: Connection error', win32con.MB_RETRYCANCEL)
                    error_showed = True
                    if select == 4:
                        return 'RETRY'
                        quit()
                    else:
                        return 'EXIT'
                        quit()
                else:
                    quit()
        else:
            BYTE_LED = ''
            for i in erange(0, len(LED) - 1, 1):
                for m in erange(0, len(LED[i]) - 1, 1):
                    if BYTE_LED == '':
                        BYTE_LED = getByte(LED[i][m])
                    else:
                        BYTE_LED += getByte(LED[i][m])
            try:
                s.write(_adaheader + bytes('H', 'utf-8') + BYTE_LED)
            except Exception:
                if error_showed == False:
                    select = win32api.MessageBox(0, 'Unfortunately, the connection failed. Retry?', 'EasyLED: Connection error', win32con.MB_RETRYCANCEL)
                    error_showed = True
                    if select == 4:
                        return 'RETRY'
                        quit()
                    else:
                        return 'EXIT'
                        quit()
                else:
                    quit()
    if log != False:
        updates += 1
def count(): #function to count led's
    return len(LED)
def autoFade(color): #function to auto fadeOut
    global lastNColor, speed
    r = color[0]
    g = color[1]
    b = color[2]
    if r == 0 and g == 0 and b == 0: #if fill color is black
        lastNColor_r = lastNColor[0] #rgb r from last normal color
        lastNColor_g = lastNColor[1] #rgb g from last normal color
        lastNColor_b = lastNColor[2] #rgb b from last normal color

        if lastNColor_r != 0 or lastNColor_g != 0 or lastNColor_b != 0: #if last normal color not black
            lastNColor_r -= speed #reduce brightness
            lastNColor_g -= speed #reduce brightness
            lastNColor_b -= speed #reduce brightness

            #if r or g or b less than zero
            if lastNColor_r < 0:
                lastNColor_r = 0
            if lastNColor_g < 0:
                lastNColor_g = 0
            if lastNColor_b < 0:
                lastNColor_b = 0

            #update last normal color
            lastNColor[0] = lastNColor_r #rgb red
            lastNColor[1] = lastNColor_g #rgb green
            lastNColor[2] = lastNColor_b #rgb blue

            color = lastNColor #update current color using last normal color
    else: #if not black
        lastNColor = color #update last normal color using current
    return color
def autoFadeOne(led, color, speed = speed): #function to auto fadeOut
    global lastNLED
    r = color[0]
    g = color[1]
    b = color[2]

    if r == 0 and g == 0 and b == 0: #if fill color is black
        cur_r = lastNLED[led-1][0] #rgb r from last normal color
        cur_g = lastNLED[led-1][1] #rgb g from last normal color
        cur_b = lastNLED[led-1][2] #rgb b from last normal color

        if cur_r != 0 or cur_g != 0 or cur_b != 0: #if last normal color not black
            cur_r -= speed #reduce brightness
            cur_g -= speed #reduce brightness
            cur_b -= speed #reduce brightness

            #if r or g or b less than zero
            if cur_r < 0:
                cur_r = 0
            if cur_g< 0:
                cur_g = 0
            if cur_b < 0:
                cur_b = 0

            #update last normal color
            lastNLED[led-1][0] = cur_r #rgb red
            lastNLED[led-1][1] = cur_g #rgb green
            lastNLED[led-1][2] = cur_b #rgb blue

            color = lastNLED[led-1] #update current color using last normal color
    else: #if not black
        lastNLED[led-1] = color #update last normal color using current
    return color
def allLED(): #function for loop from all led's
    return erange(1, count(), 1)
def animate(color, start, end, step): #function to start animation
    ANIM.append([color, start, end, step])
def pseudo_animate(color, start, end, step): #function to start animation
    PSEUDO_ANIM.append([color, start, end, step])
def get_pseudo_pixel(pixel):
    if pixel <= len(PSEUDO_LED):
        return PSEUDO_LED[pixel - 1]
    else:
        return [0,0,0]
def updatePseudoLED():
    if len(PSEUDO_ANIM) < 1: #if no animations
        pseudo_animate([0,0,0], 1, 1, 1) #create not visible animation
    for a in erange(1, len(PSEUDO_ANIM), 1): #proccessing all animations
        if (a - 1) < len(PSEUDO_ANIM): #if index not out of range
            pseudo_curA = PSEUDO_ANIM[a - 1] #get current animation
            pseudo_curAcolor = pseudo_curA[0] #current animation color
            pseudo_curApos = pseudo_curA[1] #current animation position
            pseudo_curAend = pseudo_curA[2] #current animation end
            pseudo_curAstep = pseudo_curA[3] #current animation step

            pseudo_curApos += pseudo_curAstep #move

            if pseudo_curAstep == 1: #if step is 1
                if pseudo_curApos <= pseudo_curAend: #if not a end
                    PSEUDO_LED[pseudo_curApos - 1] = pseudo_curAcolor #append to led
                else:
                    del PSEUDO_ANIM[a-1] #delete animation
            else: #if step is -1
                if pseudo_curApos > pseudo_curAend: #if not a end
                    PSEUDO_LED[pseudo_curApos - 1] = pseudo_curAcolor #append to led
                else:
                    del PSEUDO_ANIM[a-1] #delete animation
            pseudo_curA[1] = pseudo_curApos #set new position
def clearLED(): #function to clear all colors
    for i in allLED():
        LED[i - 1] = [0,0,0]
def array_maximum(items, default=None):
    iterator = iter(items)
    m = next(iterator)
    for item in iterator:
        if item > m:
            m = item
    return m
def is_fake():
    return False
