try:
    import pyglet
    canplaysound = True
except Exception:
    canplaysound = False

def playSound(file):
    if canplaysound == True:
        try:
            sound1 = pyglet.resource.media(file, streaming=False)
            sound1.play()
        except Exception:
            return False
        return False

playSound('metronome_first.wav')
