import easyled as LD #Main LED lib
import easyaudio as AUD #Audio Visual lib

LD.initLED('localhost', 3636, 30) #Connect LED
AUD.connect() #Connect Audio

#Color Map
COLORS_GRADIENT = [
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

lastNColor = [0,0,0] #last normal color
speed = 10 #fadeOutSpeed

while True: #Infinite loop
    data = AUD.getData() #Get sound data
    pitch = AUD.getPitch(data) #Get pitch

    percent = AUD.getPercent(data) #Get percent

    colorID = 30 - int(pitch / 300) #Color id from pitch
    if colorID > (len(COLORS_GRADIENT) - 1) or colorID < 0: #If out of range
        colorID = (len(COLORS_GRADIENT) - 1) #Set last item id

    r = COLORS_GRADIENT[colorID][0] * (percent / 100) #rgb red
    g = COLORS_GRADIENT[colorID][1] * (percent / 100) #rgb green
    b = COLORS_GRADIENT[colorID][2] * (percent / 100) #rgb blue
    color = [r, g, b] #rgb

    #fadeOut
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

    LD.setAllLED(color) #set color to all

    LD.updateLED() #updateLED to see results
