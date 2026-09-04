import serial.tools.list_ports
import serial
comlist = serial.tools.list_ports.comports()
connected = []
for element in comlist:
    s = serial.Serial(element.device, 115200, timeout=3)

    m = s.readline()

    if m == bytes('Ada\n', 'utf-8'):
        print(1)
    else:
        s.close()
    connected.append(element.device)
print("Connected COM ports: " + str(connected))

while True:
    s.write(1)
