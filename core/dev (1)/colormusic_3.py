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

    LD.setLED(1, color) #set left led color to current
    LD.setLED(LD.count(), color) #set right led color to current
    LD.animate(color, 1, LD.count() / 2, 1) #animate from left to middle
    LD.animate(color, LD.count(), LD.count() / 2, -1) #animate from right to middle

    LD.updateLED() #updateLED to see results
