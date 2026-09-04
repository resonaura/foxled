from __future__ import print_function
import time
from easyaudio as AUD #Audio Visual lib
import threading

#LD.initLED('localhost', 3636, 30, True) #Connect LED

print('__________________________________________')

lastColor = [0,0,0]
transition = False
lastStableColor = [0,0,0]

def playIntro():
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.D2), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.E2), 900)
    AUD.beep(int(AUD.G2), 600)
    AUD.beep(int(AUD.FSH2), 900)

    time.sleep(0.6)

    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.D2), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.G2), 900)
    AUD.beep(int(AUD.FSH2), 600)
    AUD.beep(int(AUD.D2), 900)

    time.sleep(0.6)

def playCUP1():
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.D2), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.E2), 600)


    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.C2), 300)
    AUD.beep(int(AUD.B1), 600)

    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.A1), 300)
    AUD.beep(int(AUD.G1), 900)

    time.sleep(0.6)

    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.A1), 300)
    AUD.beep(int(AUD.G1), 600)
    AUD.beep(int(AUD.A1), 600)

    time.sleep(0.3)

    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.B1), 300)
    AUD.beep(int(AUD.A1), 300)
    AUD.beep(int(AUD.G1), 600)
    AUD.beep(int(AUD.A1), 600)

def lead():

    playIntro()

    playCUP1()

    print('Fin!')

l = threading.Thread(target=lead, args=())
l.daemon = True
l.start()

while True:
    dosome = 1
