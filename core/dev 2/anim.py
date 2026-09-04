########################################################################
# Import important lib's                                               #
########################################################################
import winreg
import time
from colorthief import ColorThief
import os
import psutil
import wmi
import math
########################################################################

########################################################################
# Config variables                                                     #
########################################################################
AUD_CONNECTED = False
AUD = ''
########################################################################

########################################################################
# Animation variables                                                  #
########################################################################

#### WallColor animation wallpaper variables ####
SWC_lastFilesize = 0
SWC_color = [0,0,0]
SWC_lastColor = [0,0,0]
SWC_prevColor = [0,0,0]
SWC_tr = 0
#### Rainbow animation variables ####
RAINBOW_x = 0
RAINBOW_GRADIENT = ''
#### Red animation variables ####
R_GRADIENT = []
R_COLOR = [255,0,0]
R_SPEED = 10
R_PERCENT = 100
#### Green animation variables ####
G_GRADIENT = []
G_COLOR = [0,255,0]
G_SPEED = 10
G_PERCENT = 100
#### Blue animation variables ####
B_GRADIENT = []
B_COLOR = [0,0,255]
B_SPEED = 10
B_PERCENT = 100
#### White animation variables ####
W_GRADIENT = []
W_COLOR = [255,255,255]
W_SPEED = 10
W_PERCENT = 100
#### Violet animation variables ####
V_x = 0
V_frame = 0
V_tr = 1
V_COLORS = [
    [37,0,106],
    [19,0,53]
]
V_currentColor = V_COLORS[0]
V_lastColor = [0,0,0]
#### Colors animation variables ####
C_x = 0
C_frame = 0
C_tr = 2
C_COLORS = [
    [255,0,0],
    [255,255,0],
    [0,255,0],
    [0,255,255],
    [0,0,255],
    [255,0,255],
    [255,0,0]
]
C_currentColor = C_COLORS[0]
C_lastColor = [0,0,0]
#### New Year animation variables ####
NY_mode = 0
NY_x = 0
NY_m = 0
NY_sw = 0
NY_color = [
    [65,138,179],
    [166,183,39],
    [246,146,0],
    [223,83,39]
]
NY_transition = [
    0,
    1,
    2,
    3
]
NY_lastNColor = [
    [0,0,0],
    [0,0,0],
    [0,0,0],
    [0,0,0]
]
NY_transition_current = [
    False,
    False,
    False,
    False
]
NY_future = 0
#### CPU1 smart animation variables ####
CPU1_prev_color = [0,0,0]
CPU1_tr = 3
CPU1_m = 0
CPU1_cpu_load = 0
CPU1_fcolor = False
CPU1_console_color = ''
#### CPU2 smart animation variables ####
CPU2_prev_color = [0,0,0]
CPU2_tr = 4
CPU2_m = 0
CPU2_cpu_load = 0
CPU2_fcolor = False
CPU2_console_color = ''
CPU2_future = 0
#### System temperature smart animation variables ####
SYST_prev_color = [0,0,0]
SYST_tr = 5
SYST_m = 0
SYST_temperature = 0
SYST_fcolor = False
SYST_console_color = ''
#### Colormusic smart animation variables ####
AUD_x = 0 #Color offset
AUD_lastPitch = 0
AUD_GRADIENT = []
AUD_GR_INITED = False
AUD_future = 0
#### Windows Theme Color smart animation variables ####
WINC_last_rgb = [0,0,0]
WINC_last_trans_color = [0,0,0]
WINC_tr = 6
WINC_m = 256
WINC_hx = '000000'

class easing():
    def linearTwin(self, t, b, c, d):
        return c*t/d + b
    def easeInQuad(self, t, b, c, d):
        t /= d
        return c*t*t + b
    def easeOutQuad(self, t, b, c, d):
        t /= d
        return -c * t*(t-2) + b
    def easeInOutQuad(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return c/2*t*t + b
        t -= 1
        return -c/2 * (t*(t-2) - 1) + b
    def easeInCubic(self, t, b, c, d):
        t /= d
        return c*t*t*t + b
    def easeOutCubic(self, t, b, c, d):
        t /= d;
        t -= 1;
        return c*(t*t*t + 1) + b
    def easeInOutCubic(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return c/2*t*t*t + b
        t -= 2
        return c/2*(t*t*t + 2) + b
    def easeInQuart(self, t, b, c, d):
        t /= d
        return c*t*t*t*t + b
    def easeOutQuart(self, t, b, c, d):
        t /= d
        t -= 1
        return -c * (t*t*t*t - 1) + b
    def easeInOutQuart(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return c/2*t*t*t*t + b
        t -= 2
        return -c/2 * (t*t*t*t - 2) + b
    def easeInQuint(self, t, b, c, d):
        t /= d
        return c*t*t*t*t*t + b
    def easeOutQuint(self, t, b, c, d):
        t /= d
        t -= 1
        return c*(t*t*t*t*t + 1) + b
    def easeInOutQuint(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return c/2*t*t*t*t*t + b
        t -= 2
        return c/2*(t*t*t*t*t + 2) + b
    def easeInSine(self, t, b, c, d):
        return -c * math.cos(t/d * (math.pi/2)) + c + b
    def easeOutSine(self, t, b, c, d):
        return c * math.sin(t/d * (math.pi/2)) + b
    def easeInOutSine(self, t, b, c, d):
        return -c/2 * (math.cos(math.pi*t/d) - 1) + b
    def easeInExpo(self, t, b, c, d):
        return c * math.pow( 2, 10 * (t/d - 1) ) + b
    def easeOutExpo(self, t, b, c, d):
        return c * ( -Math.pow( 2, -10 * t/d ) + 1 ) + b
    def easeInOutExpo(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return c/2 * math.pow( 2, 10 * (t - 1) ) + b
        t -= 1
        return c/2 * ( -math.pow( 2, -10 * t) + 2 ) + b
    def easeInCirc(self, t, b, c, d):
        t /= d
        return -c * (math.sqrt(1 - t*t) - 1) + b
    def easeOutCirc(self, t, b, c, d):
        t /= d
        t -= 1
        return c * math.sqrt(1 - t*t) + b
    def easeInOutCirc(self, t, b, c, d):
        t /= d/2
        if t < 1:
            return -c/2 * (math.sqrt(1 - t*t) - 1) + b
        t -= 2
        return c/2 * (math.sqrt(1 - t*t) + 1) + b
    ############
    def generateMap(self, funct, start, end, duration):
        map = []
        for t in range(0, duration + 1, 1):
            value = funct(t, start, end, duration)
            map.append(value)
        return map
ease = easing()
########################################################################
# Class with functions required for animations                         #
########################################################################

class anim_req_functions():
    def hex_to_rgb(self, h): #HEX to RGB convert
        #Some magic
        h = h.lstrip('#')
        return tuple(int(h[i:i+2], 16) for i in (0, 2 ,4))
    def NY_anim_1(self, speed, LD): #New year animation 1
        global NY_sw, NY_transition, NY_color

        if NY_sw == 1:
            NY_transition[0] = LD.transition(NY_color[0], [0,0,0], NY_transition[0], speed)
            NY_transition[1] = LD.transition([0,0,0], NY_color[1], NY_transition[1], speed)
            NY_transition[2] = LD.transition(NY_color[2], [0,0,0], NY_transition[2], speed)
            NY_transition[3] = LD.transition([0,0,0], NY_color[3], NY_transition[3], speed)
        if NY_sw == 0:
            NY_transition[0] = LD.transition([0,0,0], NY_color[0], NY_transition[0], speed)
            NY_transition[1] = LD.transition(NY_color[1], [0,0,0], NY_transition[1], speed)
            NY_transition[2] = LD.transition([0,0,0], NY_color[2], NY_transition[2], speed)
            NY_transition[3] = LD.transition(NY_color[3], [0,0,0], NY_transition[3], speed)
    def NY_anim_2(self, speed, LD): #New year animation 2
        global NY_sw, NY_transition, NY_color

        if NY_sw == 1:
            NY_transition[0] = LD.transition(NY_color[0], [0,0,0], NY_transition[0], speed)
            NY_transition[1] = LD.transition([0,0,0], [0,0,0], NY_transition[1], speed)
            NY_transition[2] = LD.transition(NY_color[2], [0,0,0], NY_transition[2], speed)
            NY_transition[3] = LD.transition([0,0,0], [0,0,0], NY_transition[3], speed)
        if NY_sw == 0:
            NY_transition[0] = LD.transition([0,0,0], [0,0,0], NY_transition[0], speed)
            NY_transition[1] = LD.transition(NY_color[1], [0,0,0], NY_transition[1], speed)
            NY_transition[2] = LD.transition([0,0,0], [0,0,0], NY_transition[2], speed)
            NY_transition[3] = LD.transition(NY_color[3], [0,0,0], NY_transition[3], speed)
    def NY_anim_3(self, speed, LD): #New year animation 3
        global NY_sw, NY_transition, NY_color

        if NY_sw == 1:
            NY_transition[0] = LD.transition([0,0,0], NY_color[0], NY_transition[0], speed)
            NY_transition[1] = LD.transition([0,0,0], NY_color[1], NY_transition[1], speed)
            NY_transition[2] = LD.transition([0,0,0], NY_color[2], NY_transition[2], speed)
            NY_transition[3] = LD.transition([0,0,0], NY_color[3], NY_transition[3], speed)
        if NY_sw == 0:
            NY_transition[0] = LD.transition(NY_color[0], [0,0,0], NY_transition[0], speed)
            NY_transition[1] = LD.transition(NY_color[1], [0,0,0], NY_transition[1], speed)
            NY_transition[2] = LD.transition(NY_color[2], [0,0,0], NY_transition[2], speed)
            NY_transition[3] = LD.transition(NY_color[3], [0,0,0], NY_transition[3], speed)
    def getWindowsWall(self): #Get windows current wallpaper
        key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, r"Control Panel\Desktop", 0, winreg.KEY_READ)
        return winreg.QueryValueEx(key, "Wallpaper")[0]
    def getOptimalColorFromPalette(self, palette): #Get optimal color from palette (not white)
        optimal = False
        lastDifference = -255

        for i in range(0, len(palette) - 1, 1):
            r = palette[i][0]
            g = palette[i][1]
            b = palette[i][2]

            sort_colors = [r, g, b]
            sort_colors.sort()

            difference = sort_colors[2] - sort_colors[1] - sort_colors[0]

            if difference > lastDifference:
                optimal = palette[i]
                lastDifference = difference
        if optimal == False:
            optimal = palette[0]
        return optimal
    def getImageColor(self, img): #Get image color
        global color
        try:
            color_thief = ColorThief(img)
            palette = color_thief.get_palette(color_count=20, quality=30)
            dominant_color = self.getOptimalColorFromPalette(palette)
            return [dominant_color[0], dominant_color[1], dominant_color[2]]
        except Exception:
            return False
    def getFileSize(self, file): #Get filesize
        try:
            return os.path.getsize(file)
        except Exception:
            return 0

    def getStableWallColor(self, wall): #Get stable wall color
        global recursion_count
        c = self.getImageColor(wall)
        if c == False:
            try:
                return self.getStableWallColor(wall)
            except Exception:
                return False
        else:
            return c

anim_req = anim_req_functions()
########################################################################
########################################################################
#                              Animations                              #
########################################################################
########################################################################

class animations():
    #### Rainbow animation ####
    def rainbow(self, LD):
        global RAINBOW_x, RAINBOW_GRADIENT
        RAINBOW_GRADIENT = LD.generateGradientMap(LD.RAINBOW, LD.count())
        RAINBOW_x += 1
        if RAINBOW_x > (len(RAINBOW_GRADIENT) - 1):
            RAINBOW_x = 0

        LD.animate(RAINBOW_GRADIENT[RAINBOW_x], 0, LD.count(), 1)
        LD.time.sleep(0.03) #sleep 30ms

        return LD.updateLED() #updateLED to see results

    #### Red animation ####
    def red(self, LD):
        global R_PERCENT, R_SPEED, R_COLOR
        for x in LD.allLED(): #first all led's loop
            R_PERCENT -= R_SPEED

            if R_PERCENT <= 0:
                R_PERCENT = 0
                R_SPEED = -R_SPEED
            if R_PERCENT >= 100:
                R_PERCENT = 100
                R_SPEED = -R_SPEED

            r = (R_COLOR[0] * R_PERCENT) / 100
            g = (R_COLOR[1] * R_PERCENT) / 100
            b = (R_COLOR[2] * R_PERCENT) / 100

            LD.animate([r, g, b], 0, LD.count(), 1)
            LD.time.sleep(0.02) #sleep 20ms
            return LD.updateLED() #updateLED to see results

    #### Green animation ####
    def green(self, LD):
        global G_PERCENT, G_SPEED, G_COLOR
        for x in LD.allLED(): #first all led's loop
            G_PERCENT -= G_SPEED

            if G_PERCENT <= 0:
                G_PERCENT = 0
                G_SPEED = -G_SPEED
            if G_PERCENT >= 100:
                G_PERCENT = 100
                G_SPEED = -G_SPEED

            r = (G_COLOR[0] * G_PERCENT) / 100
            g = (G_COLOR[1] * G_PERCENT) / 100
            b = (G_COLOR[2] * G_PERCENT) / 100

            LD.animate([r, g, b], 0, LD.count(), 1)
            LD.time.sleep(0.02) #sleep 20ms
            return LD.updateLED() #updateLED to see results
    #### Blue animation ####
    def blue(self, LD):
        global B_PERCENT, B_SPEED, B_COLOR
        for x in LD.allLED(): #first all led's loop
            B_PERCENT -= B_SPEED

            if B_PERCENT <= 0:
                B_PERCENT = 0
                B_SPEED = -B_SPEED
            if B_PERCENT >= 100:
                B_PERCENT = 100
                B_SPEED = -B_SPEED

            r = (B_COLOR[0] * B_PERCENT) / 100
            g = (B_COLOR[1] * B_PERCENT) / 100
            b = (B_COLOR[2] * B_PERCENT) / 100

            LD.animate([r, g, b], 0, LD.count(), 1)
            LD.time.sleep(0.02) #sleep 20ms
            return LD.updateLED() #updateLED to see results
    #### White animation ####
    def white(self, LD):
        global W_PERCENT, W_SPEED, W_COLOR
        for x in LD.allLED(): #first all led's loop
            W_PERCENT -= W_SPEED

            if W_PERCENT <= 0:
                W_PERCENT = 0
                W_SPEED = -W_SPEED
            if W_PERCENT >= 100:
                W_PERCENT = 100
                W_SPEED = -W_SPEED

            r = (W_COLOR[0] * W_PERCENT) / 100
            g = (W_COLOR[1] * W_PERCENT) / 100
            b = (W_COLOR[2] * W_PERCENT) / 100

            LD.animate([r, g, b], 0, LD.count(), 1)
            LD.time.sleep(0.02) #sleep 20ms
            return LD.updateLED() #updateLED to see results
    #### Violet animation ####
    def violet(self, LD):
        global V_frame, V_tr, V_currentColor, V_COLORS, V_lastColor, V_x
        V_frame += 1
        V_currentColor = LD.getTransitionColor(V_tr)
        if V_currentColor == False:
            V_tr = LD.transition(V_lastColor, V_COLORS[V_x], V_tr, 0.5)
            V_currentColor = LD.getTransitionColor(V_tr)
            V_x += 1
            if V_x > len(V_COLORS) - 1:
                V_x = 0
                V_frame = 1

        V_lastColor = V_currentColor

        LD.setAllLED(V_currentColor)
        time.sleep(0.01)
        return LD.updateLED()
    #### Colors animation ####
    def colors(self, LD):
        global C_frame, C_tr, C_currentColor, C_COLORS, C_lastColor, C_x
        C_frame += 1
        C_currentColor = LD.getTransitionColor(C_tr)

        if C_currentColor == False:
            C_tr = LD.transition(C_lastColor, C_COLORS[C_x], C_tr, 0.5)
            C_currentColor = LD.getTransitionColor(C_tr)
            C_x += 1
            if C_x > len(C_COLORS) - 1:
                C_x = 0
                C_frame = 1

        C_lastColor = C_currentColor

        LD.setAllLED(C_currentColor)
        time.sleep(0.02)

        return LD.updateLED()


    #### NewYear animation ####
    def ny(self, LD):
        global anim_req, NY_sw, NY_transition, NY_color, NY_transition_current, NY_future, NY_lastNColor, NY_x, NY_mode, NY_m
        NY_now = time.time()
        for i in LD.erange(0, len(NY_transition_current) - 1, 1):
            NY_transition_current[i] = LD.getTransitionColor(NY_transition[i])

        if NY_transition_current[0] == False and NY_transition_current[1] == False and NY_transition_current[2] == False and NY_transition_current[3] == False:
            if NY_sw == 0:
                NY_sw = 1
            else:
                NY_sw = 0

            NY_x += 1
            if NY_now > NY_future:
                NY_mode += 1
                NY_future = NY_now + 20
            if NY_mode > 7:
                NY_mode = 1

            if NY_mode == 1:
                anim_req.NY_anim_1(0.25, LD)
            if NY_mode == 2:
                anim_req.NY_anim_1(0.50, LD)
            if NY_mode == 3:
                anim_req.NY_anim_1(1, LD)
            if NY_mode == 4:
                anim_req.NY_anim_2(0.25, LD)
            if NY_mode == 5:
                anim_req.NY_anim_2(0.50, LD)
            if NY_mode == 6:
                anim_req.NY_anim_2(1, LD)
            if NY_mode == 7:
                anim_req.NY_anim_3(0.03125, LD)

        for i in LD.erange(0, len(NY_transition_current) - 1, 1):
            if NY_transition_current[i] == False:
                NY_transition_current[i] = NY_lastNColor[i]
            else:
                NY_lastNColor[i] = NY_transition_current[i]

        NY_m = 0
        for i in LD.erange(1, LD.count(), 1):
            NY_m += 1
            if NY_m > 4:
                NY_m = 1
            if NY_m == 1:
                LD.setLED(i, NY_transition_current[0])
            if NY_m == 2:
                LD.setLED(i, NY_transition_current[1])
            if NY_m == 3:
                LD.setLED(i, NY_transition_current[2])
            if NY_m == 4:
                LD.setLED(i, NY_transition_current[3])


        return LD.updateLED()
    #### Smart wall color animation ####
    def sm_wall_color(self, LD):
        global SWC_lastFilesize, SWC_tr, SWC_color, SWC_lastColor, SWC_prevColor
        SWC_wall = anim_req.getWindowsWall()
        SWC_filesize = anim_req.getFileSize(SWC_wall)

        if SWC_filesize != SWC_lastFilesize and SWC_filesize > 0:
            SWC_newcolor = anim_req.getImageColor(SWC_wall)
            if SWC_newcolor != False:
                if SWC_newcolor != SWC_prevColor:
                    SWC_prevColor = SWC_color
                    SWC_color = SWC_newcolor
                    print('Color updated to '+str(SWC_color))
                SWC_lastFilesize = SWC_filesize

        SWC_currentColor = LD.getTransitionColor(SWC_tr)

        if SWC_currentColor == False:
            SWC_tr = LD.transition(SWC_prevColor, SWC_color, SWC_tr, 1)
            SWC_prevColor = SWC_color
            SWC_currentColor = LD.getTransitionColor(SWC_tr)

        LD.setAllLED(SWC_currentColor)
        time.sleep(0.02)
        return LD.updateLED()
    #### Smart CPU1 animation ####
    def sm_cpu1(self, LD):
        global CPU1_m, CPU1_cpu_load, CPU1_prev_color, CPU1_tr, CPU1_fcolor
        CPU1_m += 1
        if CPU1_m > 255:
            CPU1_cpu_load = psutil.cpu_percent()
            CPU1_m = 0
        if CPU1_cpu_load > 20 and CPU1_cpu_load < 70:
            CPU1_color = [0,255,0]
        else:
            CPU1_color = [255,0,0]
        if CPU1_cpu_load < 20:
            CPU1_color = [0,0,255]

        if CPU1_color != CPU1_prev_color:
            CPU1_tr = LD.transition(CPU1_prev_color, CPU1_color, CPU1_tr)
        CPU1_prev_color = CPU1_color

        CPU1_fcolor = LD.getTransitionColor(CPU1_tr)
        if CPU1_fcolor == False:
            CPU1_fcolor = CPU1_color

        LD.setAllLED(CPU1_fcolor)
        time.sleep(0.005)
        return LD.updateLED()
    #### Smart CPU2 animation ####
    def sm_cpu2(self, LD):
        global CPU2_m, CPU2_cpu_load, CPU2_prev_color, CPU2_tr, CPU2_fcolor, CPU2_future
        CPU2_now = time.time()
        if CPU2_now > CPU2_future:
            CPU2_cpu_load = psutil.cpu_percent()
            CPU2_future = CPU2_now + 0.5
            CPU2_m = 0
        if CPU2_cpu_load > 20 and CPU2_cpu_load < 70:
            CPU2_color = [0,255,0]
        else:
            CPU2_color = [255,0,0]
        if CPU2_cpu_load < 20:
            CPU2_color = [0,0,255]

        if CPU2_color != CPU2_prev_color:
            CPU2_tr = LD.transition(CPU2_prev_color, CPU2_color, CPU2_tr)
        CPU2_prev_color = CPU2_color

        CPU2_fcolor = LD.getTransitionColor(CPU2_tr)
        if CPU2_fcolor == False:
            CPU2_fcolor = CPU2_color

        LD.animate(CPU2_fcolor, 0, LD.count(), 1) #animate from left to right
        time.sleep(0.02)
        return LD.updateLED()
    #### Smart system color animation ####
    def sm_syst(self, LD):
        global SYST_m, SYST_temperature, SYST_prev_color, SYST_tr, SYST_fcolor
        SYST_m += 1
        if SYST_m > 255:
            SYST_w = wmi.WMI(namespace="root\wmi")
            SYST_temperature_info = SYST_w.MSAcpi_ThermalZoneTemperature()[0]
            SYST_temperature = int(str(SYST_temperature_info.CurrentTemperature)[2:])
            SYST_m = 0
        if SYST_temperature > 20 and SYST_temperature < 50:
            SYST_color = [0,255,0]
        else:
            SYST_color = [255,0,0]
        if SYST_temperature < 20:
            SYST_color = [0,0,255]

        if SYST_color != SYST_prev_color:
            SYST_tr = LD.transition(SYST_prev_color, SYST_color, SYST_tr)
        SYST_prev_color = SYST_color

        SYST_fcolor = LD.getTransitionColor(SYST_tr)
        if SYST_fcolor == False:
            SYST_fcolor = SYST_color

        LD.setAllLED(SYST_fcolor)
        time.sleep(0.005)
        return LD.updateLED()
    #### Smart colormusic #1 animation ####
    def sm_colormusic_1(self, LD):
        global AUD, AUD_CONNECTED, AUD_x, AUD_lastPitch

        if AUD_CONNECTED == False:
            import easyaudio as AUD #Audio Visual lib
            AUD.connect(True) #Connect Audio
            AUD_CONNECTED = True

        AUD_data = AUD.getData() #Get sound data
        AUD_pitch = AUD.getPitch(AUD_data) #Get pitch

        AUD_percent = AUD.getPercent(AUD_data) #Get percent

        if AUD_pitch != AUD_lastPitch:
            if(AUD_x < len(LD.RAINBOW_GRADIENT) / 2):
                AUD_x += 1 #move offset
            else:
                AUD_x = 0 #set to zero

        AUD_lastPitch = AUD_pitch #keep last pitch
        AUD_colorID = AUD_x #Color id from pitch
        if AUD_colorID > (len(LD.RAINBOW_GRADIENT) - 1) or AUD_colorID < 0: #If out of range
            AUD_colorID = (len(LD.RAINBOW_GRADIENT) - 1) #Set last item id

        r = LD.RAINBOW_GRADIENT[AUD_colorID][0] * (AUD_percent / 100) #rgb red
        g = LD.RAINBOW_GRADIENT[AUD_colorID][1] * (AUD_percent / 100) #rgb green
        b = LD.RAINBOW_GRADIENT[AUD_colorID][2] * (AUD_percent / 100) #rgb blue
        color = [r, g, b] #rgb
        color = LD.autoFade(color) #auto fadeOut

        LD.setLED(1, color) #set color to first led
        LD.animate(color, 1, LD.count(), 1) #animate from left to right

        return LD.updateLED()
    #### Smart colormusic #2 animation ####
    def sm_colormusic_2(self, LD):
        global AUD, AUD_CONNECTED, AUD_x, AUD_lastPitch

        if AUD_CONNECTED == False:
            import easyaudio as AUD #Audio Visual lib
            AUD.connect(True) #Connect Audio
            AUD_CONNECTED = True

        AUD_data = AUD.getData() #Get sound data
        AUD_pitch = AUD.getPitch(AUD_data) #Get pitch

        AUD_percent = AUD.getPercent(AUD_data) #Get percent

        if AUD_pitch != AUD_lastPitch:
            if(AUD_x < len(LD.RAINBOW_GRADIENT) / 2):
                AUD_x += 1 #move offset
            else:
                AUD_x = 0 #set to zero

        AUD_lastPitch = AUD_pitch #keep last pitch
        AUD_colorID = AUD_x #Color id from pitch
        if AUD_colorID > (len(LD.RAINBOW_GRADIENT) - 1) or AUD_colorID < 0: #If out of range
            AUD_colorID = (len(LD.RAINBOW_GRADIENT) - 1) #Set last item id

        r = LD.RAINBOW_GRADIENT[AUD_colorID][0] * (AUD_percent / 100) #rgb red
        g = LD.RAINBOW_GRADIENT[AUD_colorID][1] * (AUD_percent / 100) #rgb green
        b = LD.RAINBOW_GRADIENT[AUD_colorID][2] * (AUD_percent / 100) #rgb blue
        color = [r, g, b] #rgb
        color = LD.autoFade(color) #auto fadeOut

        LD.setAllLED(color) #set color to first led

        return LD.updateLED()
    #### Smart colormusic #3 animation ####
    def sm_colormusic_3(self, LD):
        global AUD, AUD_CONNECTED, AUD_x, AUD_lastPitch

        if AUD_CONNECTED == False:
            import easyaudio as AUD #Audio Visual lib
            AUD.connect(True) #Connect Audio
            AUD_CONNECTED = True

        AUD_data = AUD.getData() #Get sound data
        AUD_pitch = AUD.getPitch(AUD_data) #Get pitch

        AUD_percent = AUD.getPercent(AUD_data) #Get percent

        if AUD_pitch != AUD_lastPitch:
            if(AUD_x < len(LD.RAINBOW_GRADIENT) / 2):
                AUD_x += 1 #move offset
            else:
                AUD_x = 0 #set to zero

        AUD_lastPitch = AUD_pitch #keep last pitch
        AUD_colorID = AUD_x #Color id from pitch
        if AUD_colorID > (len(LD.RAINBOW_GRADIENT) - 1) or AUD_colorID < 0: #If out of range
            AUD_colorID = (len(LD.RAINBOW_GRADIENT) - 1) #Set last item id

        r = LD.RAINBOW_GRADIENT[AUD_colorID][0] * (AUD_percent / 100) #rgb red
        g = LD.RAINBOW_GRADIENT[AUD_colorID][1] * (AUD_percent / 100) #rgb green
        b = LD.RAINBOW_GRADIENT[AUD_colorID][2] * (AUD_percent / 100) #rgb blue
        color = [r, g, b] #rgb
        color = LD.autoFade(color) #auto fadeOut

        LD.setLED(1, color) #set left led color to current
        LD.setLED(LD.count(), color) #set right led color to current
        LD.animate(color, 1, LD.count() / 2, 1) #animate from left to middle
        LD.animate(color, LD.count(), LD.count() / 2, -1) #animate from right to middle

        return LD.updateLED()
    #### Smart colormusic #4 animation ####
    def sm_colormusic_4(self, LD):
        global AUD, AUD_CONNECTED, AUD_x, AUD_lastPitch

        if AUD_CONNECTED == False:
            import easyaudio as AUD #Audio Visual lib
            AUD.connect(True) #Connect Audio
            AUD_CONNECTED = True

        AUD_data = AUD.getData() #Get sound data
        AUD_pitch = AUD.getPitch(AUD_data) #Get pitch

        AUD_percent = AUD.getPercent(AUD_data) #Get percent

        if AUD_pitch != AUD_lastPitch:
            if(AUD_x < len(LD.RAINBOW_GRADIENT) / 2):
                AUD_x += 1 #move offset
            else:
                AUD_x = 0 #set to zero

        AUD_lastPitch = AUD_pitch #keep last pitch
        AUD_colorID = AUD_x #Color id from pitch
        if AUD_colorID > (len(LD.RAINBOW_GRADIENT) - 1) or AUD_colorID < 0: #If out of range
            AUD_colorID = (len(LD.RAINBOW_GRADIENT) - 1) #Set last item id

        r = LD.RAINBOW_GRADIENT[AUD_colorID][0] * (AUD_percent / 100) #rgb red
        g = LD.RAINBOW_GRADIENT[AUD_colorID][1] * (AUD_percent / 100) #rgb green
        b = LD.RAINBOW_GRADIENT[AUD_colorID][2] * (AUD_percent / 100) #rgb blue
        color = [r, g, b] #rgb
        color = LD.autoFade(color) #auto fadeOut

        LD.setLED(LD.count() / 2, color) #set middle led color to current
        LD.animate(color, LD.count() / 2, 1, -1) #animate from middle to left
        LD.animate(color, LD.count() / 2, LD.count(), 1) #animate from middle to right

        return LD.updateLED()
    #### Smart colormusic #5 animation ####
    def sm_colormusic_5(self, LD):
        global AUD, AUD_CONNECTED, AUD_x, AUD_lastPitch, AUD_future, AUD_GRADIENT, AUD_GR_INITED

        if AUD_GR_INITED == False:
            AUD_GRADIENT = LD.generateGradientMap(LD.RAINBOW, LD.count())
            AUD_GR_INITED = True

        if AUD_CONNECTED == False:
            import easyaudio as AUD #Audio Visual lib
            AUD.connect(True) #Connect Audio
            AUD_CONNECTED = True

        AUD_data = AUD.getData() #Get sound data

        AUD_spectre = AUD.getSpectre(AUD_data, LD.count())
        for i in range(0, LD.count(), 1):
            AUD_percent = int(AUD_spectre[i] * 100 / 255)

            AUD_percent = AUD_percent * 10
            if AUD_percent > 100:
                AUD_percent = 100

            CUR_C = AUD_GRADIENT[i]
            r = CUR_C[0] * AUD_percent / 100
            g = CUR_C[1] * AUD_percent / 100
            b = CUR_C[2] * AUD_percent / 100


            pw = LD.autoFadeOne(i + 1, [r, g, b], 1)

            if i < LD.count():
                LD.setLED(i + 1, pw)

        return LD.updateLED() #updateLED to see results
    #### Static colors ####
    def static_white(self, LD):
        LD.setAllLED([255,255,255])
        time.sleep(1)
        return LD.updateLED()
    def static_red(self, LD):
        LD.setAllLED([255,0,0])
        time.sleep(1)
        return LD.updateLED()
    def static_green(self, LD):
        LD.setAllLED([0,255,0])
        time.sleep(1)
        return LD.updateLED()
    def static_blue(self, LD):
        LD.setAllLED([0,0,255])
        time.sleep(1)
        return LD.updateLED()
    def static_yellow(self, LD):
        LD.setAllLED([255,255,0])
        time.sleep(1)
        return LD.updateLED()
    def static_magenta(self, LD):
        LD.setAllLED([255,0,255])
        time.sleep(1)
        return LD.updateLED()
    def static_cyan(self, LD):
        LD.setAllLED([0,255,255])
        time.sleep(1)
        return LD.updateLED()
    def static_orange(self, LD):
        LD.setAllLED([255,149,0])
        time.sleep(1)
        return LD.updateLED()
    def static_violet(self, LD):
        LD.setAllLED([124,0,255])
        time.sleep(1)
        return LD.updateLED()
    def static_grass(self, LD):
        LD.setAllLED([147,255,0])
        time.sleep(1)
        return LD.updateLED()
    def static_peach(self, LD):
        LD.setAllLED([255,218,185])
        time.sleep(1)
        return LD.updateLED()
    def static_tomato(self, LD):
        LD.setAllLED([255,99,71])
        time.sleep(1)
        return LD.updateLED()
    def static_light_sea_green(self, LD):
        LD.setAllLED([32,178,170])
        time.sleep(1)
        return LD.updateLED()
    #### Smart windows theme color animation ####
    def sm_theme_color(self, LD):
        global WINC_last_rgb, WINC_last_trans_color, WINC_tr, WINC_m, WINC_hx

        WINC_m += 1
        if WINC_m > 255:
            WINC_key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, r"Software\Microsoft\Windows\DWM", 0, winreg.KEY_READ)
            WINC_hx = hex(winreg.QueryValueEx(WINC_key, "ColorizationColor")[0])
            WINC_m = 0
            if len(WINC_hx) <= 9:
                WINC_dm = 2
                WINC_hx = "#" + WINC_hx[WINC_dm:][:6]
            else:
                WINC_dm = 4
                WINC_hx = "#" + WINC_hx[WINC_dm:][:6]

        WINC_rgb = anim_req.hex_to_rgb(WINC_hx)
        if WINC_rgb != WINC_last_rgb:
            WINC_tr = LD.transition(WINC_last_rgb, WINC_rgb, WINC_tr)
            print('Color updated to ' + str(WINC_rgb))
            WINC_last_rgb = WINC_rgb

        WINC_trans_color = LD.getTransitionColor(WINC_tr)
        if WINC_trans_color != False:
            LD.setAllLED(WINC_trans_color)
            WINC_last_trans_color = WINC_trans_color
        else:
            LD.setAllLED(WINC_last_trans_color)

        time.sleep(0.01)
        return LD.updateLED()

animation = animations()
