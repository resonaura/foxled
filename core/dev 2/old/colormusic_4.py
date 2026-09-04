import easyaudio as AUD #Audio Visual lib
import led_config as LC

LD = LC.init()
AUD.connect(True) #Connect Audio

print('__________________________________________')

x = 0 #Color offset
lastPitch = 0

while True: #Infinite loop
    data = AUD.getData() #Get sound data
    pitch = AUD.getPitch(data) #Get pitch

    percent = AUD.getPercent(data) #Get percent

    if pitch != lastPitch:
        if(x < len(LD.RAINBOW_GRADIENT) / 2):
            x += 1 #move offset
        else:
            x = 0 #set to zero

    lastPitch = pitch #keep last pitch
    colorID = int(pitch / 300) + x #Color id from pitch
    if colorID > (len(LD.RAINBOW_GRADIENT) - 1) or colorID < 0: #If out of range
        colorID = (len(LD.RAINBOW_GRADIENT) - 1) #Set last item id

    r = LD.RAINBOW_GRADIENT[colorID][0] * (percent / 100) #rgb red
    g = LD.RAINBOW_GRADIENT[colorID][1] * (percent / 100) #rgb green
    b = LD.RAINBOW_GRADIENT[colorID][2] * (percent / 100) #rgb blue
    color = [r, g, b] #rgb
    color = LD.autoFade(color) #auto fadeOut

    LD.setLED(LD.count() / 2, color) #set middle led color to current
    LD.animate(color, LD.count() / 2, 1, -1) #animate from middle to left
    LD.animate(color, LD.count() / 2, LD.count(), 1) #animate from middle to right

    LD.updateLED() #updateLED to see results
