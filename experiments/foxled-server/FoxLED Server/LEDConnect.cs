using SocketLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FoxLED_Server
{
    public class LEDConnect
    {
        private SerialPort _serialPort;
        private static byte[] _adaHeader;
        public static int LED_COUNT = 30;
        private float brightness = 1;

        public static string PortName { get; set; }

        public bool conn = false;

        public bool ConnectTo(string port)
        {
            PortName = port;

            _adaHeader = new byte[]
            {
                (byte)'A',
                (byte)'d',
                (byte)'a',
                (byte)((LED_COUNT - 1) >> 8),
                (byte)((LED_COUNT - 1) & 0xff),
                0
            };
            _adaHeader[5] = (byte)(_adaHeader[3] ^ _adaHeader[4] ^ 0x55);


            Stop();

            try
            {
                var serialPort = new SerialPort(PortName, 115200);

                serialPort.Open();







                _serialPort = serialPort;

                conn = true;
                return true;
            }
            catch (Exception ex)
            {
                conn = false;
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }



        }
        public bool AutoConnect()
        {
            bool Connected = false;
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                System.Diagnostics.Debug.Write(s);
                if (Connected == false)
                {
                    bool result = ConnectTo(s);
                    if (result == true)
                    {
                        Connected = true;
                        System.Diagnostics.Debug.Write("Yeah!");
                    }
                }
            }

            return Connected;
        }
        public void Stop()
        {
            try
            {
                if (_serialPort != null)
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public void Display(byte[] ledArray)
        {
            if (_serialPort == null) return;

            try
            {
                _serialPort.Write(_adaHeader, 0, 6);
                _serialPort.Write(ledArray, 0, (byte)(LED_COUNT * 3));
            }
            catch (Exception ex)
            {
                Stop();
                Debug.WriteLine("Serialport write error: " + ex.Message);
            }
        }
        public System.Windows.Media.Color ColorFromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            if (0 > alpha
                    || 255 < alpha)
            {
                throw new ArgumentOutOfRangeException(
                    "alpha",
                    alpha,
                    "Value must be within a range of 0 - 255.");
            }

            if (0f > hue
                || 360f < hue)
            {
                throw new ArgumentOutOfRangeException(
                    "hue",
                    hue,
                    "Value must be within a range of 0 - 360.");
            }

            if (0f > saturation
                || 1f < saturation)
            {
                throw new ArgumentOutOfRangeException(
                    "saturation",
                    saturation,
                    "Value must be within a range of 0 - 1.");
            }

            if (0f > brightness
                || 1f < brightness)
            {
                throw new ArgumentOutOfRangeException(
                    "brightness",
                    brightness,
                    "Value must be within a range of 0 - 1.");
            }

            if (0 == saturation)
            {
                return System.Windows.Media.Color.FromArgb(
                                    (byte)alpha,
                                    (byte)Convert.ToInt32(brightness * 255),
                                    (byte)Convert.ToInt32(brightness * 255),
                                    (byte)Convert.ToInt32(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMid, (byte)iMax, (byte)iMin);
                case 2:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMin, (byte)iMax, (byte)iMid);
                case 3:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMin, (byte)iMid, (byte)iMax);
                case 4:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMid, (byte)iMin, (byte)iMax);
                case 5:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMax, (byte)iMin, (byte)iMid);
                default:
                    return System.Windows.Media.Color.FromArgb((byte)alpha, (byte)iMax, (byte)iMid, (byte)iMin);
            }
        }
        
        
        private byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
        private byte[] newMAP = new byte[LED_COUNT * 3];
    }
}
