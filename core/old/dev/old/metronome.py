from __future__ import print_function
import psutil
import time
import easyaudio as AUD #Audio Visual lib
import threading
import led_config as LC

LD = LC.init()

print('__________________________________________')

lastColor = [0,0,0]

transition = False

lastStableColor = [0,0,0]

bpm = 1

def tempo_select():
    global bpm

    LD.updateLED()

    bpm = raw_input('Please enter tempo in BPM: ')
    try:
        bpm = float(bpm)
    except Exception:
            try:
                bpm = int(bpm)
            except Exception:
                print(LD.Style.BRIGHT+LD.Fore.RED+'Error! The variable is not int or float.'+LD.Fore.WHITE)
                tempo_select()
tempo_select()

bps = bpm / float(60)
decay = float(1) / bps
future = float(0)
beat = 0
beat_per_tact = 4
beat_colors = [
    [255,255,255],
    [0,0,0],
    [100,100,100],
    [0,0,0]
]


def metronome_sound(decay, type):
    if type == 1:
        AUD.beep(2500, 8)
    else:
        AUD.beep(2000, 8)

def ledLogic(LD, AUD, beat_per_tact, decay, beat_colors, bpm):
    future = 0
    beat = 0
    lastColor = [0,0,0]
    transition = False

    while True:
        now = time.time()

        if now > future:
            beat += 1
            if beat > beat_per_tact:
                beat = 1
            if beat == 1:
                type = 1
            else:
                type = 2
            future = now + decay
            color = beat_colors[beat - 1]
            LD.setAllLED(color)
            transition = LD.transition(lastColor, color, transition, decay / 2)

            t = threading.Thread(target=metronome_sound, args=(decay,type,))
            t.daemon = True
            t.start()

            lastColor = color
        currentColor = LD.getTransitionColor(transition)
        if currentColor != False:
            lastStableColor = currentColor
        else:
            currentColor = lastStableColor
        LD.setAllLED(currentColor)
        LD.updateLED()

def displayLogic(LD, AUD, beat_per_tact, decay, beat_colors, bpm):
    beat = 0

    while True:
        beat += 1
        if beat > beat_per_tact:
            beat = 1
        print('Metronome: '+str(bpm)+' BPM  1/'+str(beat_per_tact)+'  '+str(beat)+' from '+str(beat_per_tact), end='\r')
        time.sleep(decay)
t1 = threading.Thread(target=ledLogic, args=(LD, AUD, beat_per_tact, decay, beat_colors, bpm,))
t1.daemon = True
t1.start()

t2 = threading.Thread(target=displayLogic, args=(LD, AUD, beat_per_tact, decay, beat_colors, bpm,))
t2.daemon = True
t2.start()

while True:
    dosome = 1
