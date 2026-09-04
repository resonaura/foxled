from __future__ import print_function
import led_config as LC

LD = LC.init()

print('__________________________________________')

mode = 0
x = 0
m = 0
sw = 0

color = [
    [65,138,179],
    [166,183,39],
    [246,146,0],
    [223,83,39]
]
transition = [
    0,
    1,
    2,
    3
]
lastNColor = [
    [0,0,0],
    [0,0,0],
    [0,0,0],
    [0,0,0]
]

transition_current = [
    False,
    False,
    False,
    False
]

future = 0

def anim_1(speed):
    global sw, transition, color

    if sw == 1:
        transition[0] = LD.transition(color[0], [0,0,0], transition[0], speed)
        transition[1] = LD.transition([0,0,0], color[1], transition[1], speed)
        transition[2] = LD.transition(color[2], [0,0,0], transition[2], speed)
        transition[3] = LD.transition([0,0,0], color[3], transition[3], speed)
    if sw == 0:
        transition[0] = LD.transition([0,0,0], color[0], transition[0], speed)
        transition[1] = LD.transition(color[1], [0,0,0], transition[1], speed)
        transition[2] = LD.transition([0,0,0], color[2], transition[2], speed)
        transition[3] = LD.transition(color[3], [0,0,0], transition[3], speed)
def anim_2(speed):
    global sw, transition, color

    if sw == 1:
        transition[0] = LD.transition(color[0], [0,0,0], transition[0], speed)
        transition[1] = LD.transition([0,0,0], [0,0,0], transition[1], speed)
        transition[2] = LD.transition(color[2], [0,0,0], transition[2], speed)
        transition[3] = LD.transition([0,0,0], [0,0,0], transition[3], speed)
    if sw == 0:
        transition[0] = LD.transition([0,0,0], [0,0,0], transition[0], speed)
        transition[1] = LD.transition(color[1], [0,0,0], transition[1], speed)
        transition[2] = LD.transition([0,0,0], [0,0,0], transition[2], speed)
        transition[3] = LD.transition(color[3], [0,0,0], transition[3], speed)
def anim_3(speed):
    global sw, transition, color

    if sw == 1:
        transition[0] = LD.transition([0,0,0], color[0], transition[0], speed)
        transition[1] = LD.transition([0,0,0], color[1], transition[1], speed)
        transition[2] = LD.transition([0,0,0], color[2], transition[2], speed)
        transition[3] = LD.transition([0,0,0], color[3], transition[3], speed)
    if sw == 0:
        transition[0] = LD.transition(color[0], [0,0,0], transition[0], speed)
        transition[1] = LD.transition(color[1], [0,0,0], transition[1], speed)
        transition[2] = LD.transition(color[2], [0,0,0], transition[2], speed)
        transition[3] = LD.transition(color[3], [0,0,0], transition[3], speed)


while True: #Infinite loop
    now = LD.time.time()
    for i in LD.erange(0, len(transition_current) - 1, 1):
        transition_current[i] = LD.getTransitionColor(transition[i])


    if transition_current[0] == False and transition_current[1] == False and transition_current[2] == False and transition_current[3] == False:
        if sw == 0:
            sw = 1
        else:
            sw = 0

        x += 1
        if now > future:
            mode += 1
            future = now + 20
        if mode > 7:
            mode = 1
        print('Mode: '+str(mode)+'   ', end='\r')

        if mode == 1:
            anim_1(1)
        if mode == 2:
            anim_1(0.50)
        if mode == 3:
            anim_1(1)
        if mode == 4:
            anim_2(0.25)
        if mode == 5:
            anim_2(0.50)
        if mode == 6:
            anim_2(1)
        if mode == 7:
            anim_3(0.03125)

    for i in LD.erange(0, len(transition_current) - 1, 1):
        if transition_current[i] == False:
            transition_current[i] = lastNColor[i]
        else:
            lastNColor[i] = transition_current[i]

    m = 0
    for i in LD.erange(1, LD.count(), 1):
        m += 1
        if m > 4:
            m = 1
        if m == 1:
            LD.setLED(i, transition_current[0])
        if m == 2:
            LD.setLED(i, transition_current[1])
        if m == 3:
            LD.setLED(i, transition_current[2])
        if m == 4:
            LD.setLED(i, transition_current[3])

    LD.time.sleep(0.002)
    LD.updateLED()
