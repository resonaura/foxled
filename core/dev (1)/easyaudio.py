#import main libs
import pyaudio
import numpy as np
import math
import time
import audioop

#config
chunk = 1024
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 44100
RECORD_SECONDS = 20

#variables for stream
p = ''
stream = ''

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
def connect(): #audio connect function
    global p, stream #get global variables for stream
    p = pyaudio.PyAudio() #init pyAudio

    #init stream
    stream = p.open(format = FORMAT,
    channels = CHANNELS,
    rate = RATE,
    input = True,
    output = True,
    frames_per_buffer = chunk)
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
    peak = getPeak(signal) #get peak
    return peak * 10 / 255 * 10 #return percent
def getPitch(signal): #function to get pitch
    #some magic
    signal = np.fromstring(signal, 'Int16');
    crossing = [math.copysign(1.0, s) for s in signal]
    index = mfind(np.diff(crossing));
    f0 = round(len(index) *RATE /(2*np.prod(len(signal))))
    #ta da! :)
    return f0;
