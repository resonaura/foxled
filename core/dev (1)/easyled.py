#import main libs
import sys
import telnetlib
import time

#Arrays to store LEDs and Animation
LED = []
ANIM = []
tn = ''

#Range functions
def erange(start, end, step):
    while start <= end:
        yield start
        start += step
def erange_d(start, end, step):
    while start >= end:
        yield start
        start -= step

#Function to init led
def initLED(host, port, count):
    global tn #Get global variable for telnet
    tn = telnetlib.Telnet(host,port) #connect telnet
    tn.read_until('version: 1.3 (enter "help" for more info)') #Wait for successfuly connection
    tn.write('lock\n') #lock led
    tn.read_until('lock:success') #wait for ok
    tn.write('setsmooth:100;\n') #smooth

    for a in erange(1, count, 1): #build LED MAP
        LED.append([0,0,0]) #add new led to array
def setLED(num, color): #function to set led color
    global LED #get LED MAP
    LED[num-1] = color #set to led by num
def setAllLED(color): #function to set all led's color
    global LED #get LED MAP
    for led in erange(1, len(LED), 1): #start loop
        LED[led-1] = color #set color to current led in loop
def updateLED(): #function to update led
    global LED, ANIM #get LED and ANIMation MAP
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
                    LED[curApos - 1] = curAcolor #append to led
                else:
                    del ANIM[a-1] #delete animation
            else: #if step is -1
                if curApos > curAend: #if not a end
                    LED[curApos - 1] = curAcolor #append to led
                else:
                    del ANIM[a-1] #delete animation
            curA[1] = curApos #set new position


	REQ_ADDT = ''; #variable for store led's colors to query
	for led in erange(1, len(LED), 1): #all led's loop
		REQ_ADDT += str(led)+'-'+str(LED[led-1][0])+','+str(LED[led-1][1])+','+str(LED[led-1][2])+';' #append current led color to query
		if led < (len(LED)-1): REQ_ADDT += ' ' #if not last add space
	tn.write('setcolor: '+REQ_ADDT+'\n') #run query
def count(): #function to count led's
    return len(LED)
def allLED(): #function for loop from all led's
    return erange(1, count(), 1)
def animate(color, start, end, step): #function to start animation
    ANIM.append([color, start, end, step])
