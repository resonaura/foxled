import easyled as LD #Main LED lib

win32api = LD.win32api
win32con = LD.win32con
import pickle
import sys
import wx
import keyboard


local = False
access_error_showed = False

def loadLEDNum():
    try:
        cache = pickle.load(open( "cache.p", "rb" ) )

        if isinstance(cache, dict):
            if isinstance(cache['num'], int):
                return cache['num']
            else:
                return False
        else:
            return False
    except Exception:
        return False
def loadLEDLastAnim():
    try:
        cache = pickle.load(open( "cache.p", "rb" ) )

        if isinstance(cache, dict):
            if isinstance(cache['anim'], int):
                return cache['anim']
            else:
                return False
        else:
            return False
    except Exception:
        return False

def storeLEDParams(num, anim):
    global access_error_showed
    try:
        cache = { "num": num, "anim": anim }
        pickle.dump(cache, open( "cache.p", "wb" ) )
        access_error_showed = False
    except Exception:
        if access_error_showed == False:
            win32api.MessageBox(0, 'The app could not save the cache', 'Cache update error', win32con.MB_ICONERROR)
            access_error_showed = True

led_num = loadLEDNum()

if led_num == False:
    led_num = 30

logging = True
host = 'localhost'
port = 3636
frame = False

def killApp():
    if frame != False:
        frame.Close()
def init():
    try:
        try:
            ill = LD.initLocalLED(led_num, logging)
        except Exception:
            ill = False
        if ill:
            return LD
        else:
            LD.initLED(host, port, led_num, logging) #Connect LED
            return LD
    except Exception:
        if frame != False:
            frame.kblock = True
            for btn in frame.btns:
                frame.btns[btn].Disable()
                frame.btns[btn].SetBackgroundColour(wx.Colour(0,0,0))
                frame.btns[btn].SetForegroundColour(wx.Colour(0,0,0))
                frame.connect.SetLabel('Connection failed! Please restart the app')
                for i in range(0, 10, 1):
                    keyboard.add_hotkey(str(i), killApp)
        win32api.MessageBox(0, 'Unfortunately, the connection failed. Please check the connection!', 'Connection error', win32con.MB_ICONERROR)
        if frame != False:
            frame.Close()
        sys.exit(0)
