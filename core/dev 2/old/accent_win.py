import _winreg
import time
import led_config as LC

LD = LC.init()

print('__________________________________________')

last_rgb = [0,0,0]
last_trans_color = [0,0,0]
tr = 0
m = 256
hx = '000000'

def hex_to_rgb(h):
    h = h.lstrip('#')
    return tuple(int(h[i:i+2], 16) for i in (0, 2 ,4))

while True:
    m += 1
    if m > 255:
        key = _winreg.OpenKey(_winreg.HKEY_CURRENT_USER, r"Software\Microsoft\Windows\DWM", 0, _winreg.KEY_READ)
        hx = hex(_winreg.QueryValueEx(key, "ColorizationColor")[0])
        m = 0
    if len(hx) <= 9:
        dm = 2
    else:
        dm = 4
        hx = "#" + hx[dm:][:6]
        rgb = hex_to_rgb(hx)
    if rgb != last_rgb:
        tr = LD.transition(last_rgb, rgb, tr)
        print 'Color updated to ' + str(rgb)
        last_rgb = rgb

    trans_color = LD.getTransitionColor(tr)
    if trans_color != False:
        LD.setAllLED(trans_color)
        last_trans_color = trans_color
    else:
        LD.setAllLED(last_trans_color)

    LD.updateLED()
    time.sleep(0.01)
