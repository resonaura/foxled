#import main libs
from __future__ import print_function
import pyaudio
import numpy as np
import math
import time
import audioop
import struct
import matplotlib
from colorama import init
init()
from colorama import Fore, Back, Style
from scipy.fftpack import fft
matplotlib.use('Agg')
from matplotlib import pylab
canplaysound = False

B2 = 987.75
ASH2 = 932.32
A2 = 880.00
GSH2 = 830.60
G2 = 784.00
FSH2 = 739.98
F2 = 698.46
E2 = 659.26
DSH2 = 622.26
D2 = 587.32
CSH2 = 554.36
C2 = 523.25
B1 = 493.88
ASH1 = 466.16
A1 = 440.00
GSH1 = 415.30
G1 = 392.00
FSH1 = 369.99
F1 = 349.23
E1 = 329.63
DSH1 = 311.13
D1 = 293.66
CSH1 = 277.18
C1 = 261.63


try:
    import winsound
    canplaysound = True
except Exception:
    canplaysound = False

def playSound(file):
    if canplaysound == True:
        try:
            winsound.PlaySound(file, winsound.SND_FILENAME)
        except Exception:
            return False
        return False

def beep(frequency, duration):
    if canplaysound == True:
        try:
            winsound.Beep(frequency, duration)
        except Exception:
            return False
        return False
#config
chunk = 1024
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 44100
RECORD_SECONDS = 20

#variables for stream
p = ''
stream = ''

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

#Deprecated function from matplotlib.mlab
def mfind(array):
    result = []
    array = np.ravel(array)
    for i in erange(0, (len(array) - 1), 1):
        if array[i] != 0:
            result.append(i)
    return result
def connect(logging = False): #audio connect function
    global p, stream, log #get global variables for stream
    log = logging
    if log != False:
        print('Connecting audio...', end='\r')
    p = pyaudio.PyAudio() #init pyAudio

    #init stream
    stream = p.open(format = FORMAT,
    channels = CHANNELS,
    rate = RATE,
    input = True,
    output = True,
    frames_per_buffer = chunk)
    if log != False:
        print("Connecting audio... "+Style.BRIGHT+Fore.GREEN+"success!"+Fore.WHITE)
def getData(): #function to get current audio data
    global stream, chunk #get global variables - stream and chunk
    return stream.read(chunk) #return data

def getRMS(signal): #function to get RMS
    return audioop.rms(signal, 2);
def getPeak(signal): #function to get Peak
    RMS = audioop.rms(signal, 2); #get RMS
    peak = RMS / 3

    if peak > 255: #if out of range
        peak = 255 #set max
    return peak
def getPercent(signal): #function to get percent
    global log
    peak = getPeak(signal) #get peak
    percent = peak * 10 / 255 * 10

    peak_map = ''

    for i in erange(1, percent / 10, 1):
        peak_map += Style.BRIGHT+Back.WHITE+'  '+Back.RESET
    for i in erange(percent / 10, 10, 1):
        peak_map += '  '
    peak_map = peak_map[:-2]
    db = percent / 10
    visual_db = ''
    if db < 10:
        visual_db += '0'
    visual_db += str(int(db))
    print('Audio Peak: '+visual_db+' db. '+peak_map+'   ', end='\r')
    return percent #return percent
def getPitch(signal): #function to get pitch
    #some magic
    signal = np.fromstring(signal, 'Int16');
    crossing = [math.copysign(1.0, s) for s in signal]
    index = mfind(np.diff(crossing));
    f0 = round(len(index) *RATE /(2*np.prod(len(signal))))
    #ta da! :)
    return f0;
def piff(val):
   return int(2*1024*val/44100)
def getSpectre(signal, bars):
    spectre = []

    data = struct.unpack("%dh"%(len(signal)/2),signal)
    data = np.array(data, dtype='h')

    # Apply FFT - real data
    fourier=np.fft.rfft(data)
    # Remove last element in array to make it the same size as chunk
    fourier=np.delete(fourier,len(fourier)-1)
    # Find average 'amplitude' for specific frequency ranges in Hz
    power = np.abs(fourier)
    percent = getPercent(signal)
    one_bar = 20000 / bars
    for i in range(1, bars + 1, 1):
        pw = int(np.mean(power[piff((one_bar * i) / 2)    :piff((one_bar * i)):1])) / 1000
        spectre.append(pw)
    return spectre
