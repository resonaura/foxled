using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace FoxLED;

public class LEDConnect
{
	private SerialPort _serialPort;

	private static byte[] _adaHeader;

	public int[][] LEDS = new int[LEDConstants.LED_COUNT][];

	private float brightness = 1f;

	public bool conn;

	private byte[] newMAP = new byte[LEDConstants.LED_COUNT * 3];

	private List<int[][]> ANIM = new List<int[][]>();

	private List<int[][]> TRANS = new List<int[][]>();

	public static string PortName { get; set; }

	public bool ConnectTo(string port)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		PortName = port;
		byte[] obj = new byte[6] { 65, 100, 97, 0, 0, 0 };
		obj[3] = (byte)(LEDConstants.LED_COUNT - 1 >> 8);
		obj[4] = (byte)((uint)(LEDConstants.LED_COUNT - 1) & 0xFFu);
		_adaHeader = obj;
		_adaHeader[5] = (byte)((uint)(_adaHeader[3] ^ _adaHeader[4]) ^ 0x55u);
		Stop();
		for (int i = 0; i < LEDConstants.LED_COUNT; i++)
		{
			LEDS[i] = new int[3];
		}
		try
		{
			SerialPort val = new SerialPort(PortName, 115200);
			val.Open();
			_serialPort = val;
			conn = true;
			return true;
		}
		catch (Exception)
		{
			conn = false;
			return false;
		}
	}

	public bool AutoConnect()
	{
		bool flag = false;
		string[] portNames = SerialPort.GetPortNames();
		foreach (string port in portNames)
		{
			if (!flag && ConnectTo(port))
			{
				flag = true;
			}
		}
		return flag;
	}

	public void Stop()
	{
		try
		{
			if (_serialPort != null)
			{
				_serialPort.Close();
				((Component)(object)_serialPort).Dispose();
				_serialPort = null;
			}
		}
		catch (Exception ex)
		{
			MainWindow.LED.consoleLog("Serial port error: " + ex.Message);
		}
	}

	public void Display(byte[] ledArray)
	{
		if (_serialPort == null)
		{
			return;
		}
		try
		{
			_serialPort.Write(_adaHeader, 0, 6);
			_serialPort.Write(ledArray, 0, (int)LEDConstants.LED_ARRAY_SIZE);
		}
		catch (Exception ex)
		{
			Stop();
			MainWindow.LED.consoleLog("Serialport write error: " + ex.Message);
		}
	}

	public Color ColorFromAhsb(int alpha, float hue, float saturation, float brightness)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		if (0 > alpha || 255 < alpha)
		{
			throw new ArgumentOutOfRangeException("alpha", alpha, "Value must be within a range of 0 - 255.");
		}
		if (0f > hue || 360f < hue)
		{
			throw new ArgumentOutOfRangeException("hue", hue, "Value must be within a range of 0 - 360.");
		}
		if (0f > saturation || 1f < saturation)
		{
			throw new ArgumentOutOfRangeException("saturation", saturation, "Value must be within a range of 0 - 1.");
		}
		if (0f > brightness || 1f < brightness)
		{
			throw new ArgumentOutOfRangeException("brightness", brightness, "Value must be within a range of 0 - 1.");
		}
		if (0f == saturation)
		{
			return Color.FromArgb((byte)alpha, (byte)Convert.ToInt32(brightness * 255f), (byte)Convert.ToInt32(brightness * 255f), (byte)Convert.ToInt32(brightness * 255f));
		}
		float num;
		float num2;
		if (0.5 < (double)brightness)
		{
			num = brightness - brightness * saturation + saturation;
			num2 = brightness + brightness * saturation - saturation;
		}
		else
		{
			num = brightness + brightness * saturation;
			num2 = brightness - brightness * saturation;
		}
		int num3 = (int)Math.Floor(hue / 60f);
		if (300f <= hue)
		{
			hue -= 360f;
		}
		hue /= 60f;
		hue -= 2f * (float)Math.Floor(((float)num3 + 1f) % 6f / 2f);
		float num4 = ((num3 % 2 != 0) ? (num2 - hue * (num - num2)) : (hue * (num - num2) + num2));
		int num5 = Convert.ToInt32(num * 255f);
		int num6 = Convert.ToInt32(num4 * 255f);
		int num7 = Convert.ToInt32(num2 * 255f);
		return (Color)(num3 switch
		{
			1 => Color.FromArgb((byte)alpha, (byte)num6, (byte)num5, (byte)num7), 
			2 => Color.FromArgb((byte)alpha, (byte)num7, (byte)num5, (byte)num6), 
			3 => Color.FromArgb((byte)alpha, (byte)num7, (byte)num6, (byte)num5), 
			4 => Color.FromArgb((byte)alpha, (byte)num6, (byte)num7, (byte)num5), 
			5 => Color.FromArgb((byte)alpha, (byte)num5, (byte)num7, (byte)num6), 
			_ => Color.FromArgb((byte)alpha, (byte)num5, (byte)num6, (byte)num7), 
		});
	}

	public void SetAllLED(int[] color)
	{
		color[0] = (int)((float)color[0] * brightness);
		color[1] = (int)((float)color[1] * brightness);
		color[2] = (int)((float)color[2] * brightness);
		for (int i = 0; i < LEDS.Length; i++)
		{
			LEDS[i] = color;
		}
	}

	public void SetLED(int n, int[] color)
	{
		color[0] = (int)((float)color[0] * brightness);
		color[1] = (int)((float)color[1] * brightness);
		color[2] = (int)((float)color[2] * brightness);
		if (n <= LEDS.Length)
		{
			LEDS[n - 1] = color;
		}
	}

	private byte[] addByteToArray(byte[] bArray, byte newByte)
	{
		byte[] array = new byte[bArray.Length + 1];
		bArray.CopyTo(array, 1);
		array[0] = newByte;
		return array;
	}

	public int Transition(int[] start_rgb, int[] end_rgb, int step = 1)
	{
		int[] array = new int[3] { step, step, step };
		int num = start_rgb[0];
		int num2 = start_rgb[1];
		int num3 = start_rgb[2];
		int num4 = end_rgb[0];
		int num5 = end_rgb[1];
		int num6 = end_rgb[2];
		if (num4 != num)
		{
			if (num4 < num)
			{
				array[0] = -step;
			}
		}
		else
		{
			array[0] = 0;
		}
		if (num5 != num2)
		{
			if (num5 < num2)
			{
				array[1] = -step;
			}
		}
		else
		{
			array[1] = 0;
		}
		if (num6 != num3)
		{
			if (num6 < num3)
			{
				array[2] = -step;
			}
		}
		else
		{
			array[2] = 0;
		}
		TRANS.Add(new int[3][] { start_rgb, end_rgb, array });
		return TRANS.Count - 1;
	}

	public int[] GetTransitionColor(int i)
	{
		if (i < TRANS.Count())
		{
			if (i < 0)
			{
				return new int[3] { -1, -1, -1 };
			}
			return TRANS[i][0];
		}
		return new int[3] { -1, -1, -1 };
	}

	public void Animate(int[] color, int start, int end, int step)
	{
		if (color != null)
		{
			color[0] = (int)((float)color[0] * brightness);
			color[1] = (int)((float)color[1] * brightness);
			color[2] = (int)((float)color[2] * brightness);
			ANIM.Add(new int[4][]
			{
				color,
				new int[1] { start },
				new int[1] { end },
				new int[1] { step }
			});
		}
	}

	public void ClearAnimations()
	{
		ANIM.Clear();
	}

	public void ClearTransitions()
	{
		TRANS.Clear();
	}

	public void UpdateLED()
	{
		try
		{
			if (TRANS.Count > 0)
			{
				int[][][] array = TRANS.ToArray();
				foreach (int[][] array2 in array)
				{
					int[] array3 = array2[0];
					int[] obj = array2[1];
					int[] obj2 = array2[2];
					int num = obj2[0];
					int num2 = obj2[1];
					int num3 = obj2[2];
					int num4 = array3[0];
					int num5 = array3[1];
					int num6 = array3[2];
					int num7 = obj[0];
					int num8 = obj[1];
					int num9 = obj[2];
					if (num4 != num7)
					{
						num4 += num;
					}
					if (num5 != num8)
					{
						num5 += num2;
					}
					if (num6 != num9)
					{
						num6 += num3;
					}
					array3 = new int[3] { num4, num5, num6 };
					array2[0] = array3;
					if (num4 == num7 && num5 == num8 && num6 == num9)
					{
						TRANS.Remove(array2);
					}
				}
			}
			if (!conn)
			{
				MainWindow.fillPseudo(LEDS);
			}
		}
		catch (Exception)
		{
		}
		if (ANIM.Count > 0)
		{
			for (int j = 0; j < ANIM.Count(); j++)
			{
				if (j >= ANIM.Count())
				{
					continue;
				}
				int[][] array4 = ANIM[j];
				int[] array5 = array4[0];
				int num10 = array4[1][0];
				_ = array4[1][0];
				int num11 = array4[2][0];
				int num12 = array4[3][0];
				if (num12 == 1)
				{
					if (num10 <= num11)
					{
						LEDS[num10 - 1] = array5;
					}
					else
					{
						ANIM.RemoveAt(j);
						LEDS[LEDConstants.LED_COUNT - 1] = LEDS[LEDConstants.LED_COUNT - 2];
					}
				}
				else if (num10 >= num11)
				{
					LEDS[num10 - 1] = array5;
				}
				else
				{
					ANIM.RemoveAt(j);
					LEDS[0] = LEDS[1];
				}
				num10 += num12;
				if (j < ANIM.Count)
				{
					ANIM[j] = new int[4][]
					{
						array5,
						new int[1] { num10 },
						new int[1] { num11 },
						new int[1] { num12 }
					};
				}
			}
			for (int k = 0; k < ANIM.Count(); k++)
			{
				int num13 = ANIM[k][1][0];
				for (int l = 0; l < ANIM.Count(); l++)
				{
					int[][] array6 = ANIM[l];
					if (array6 != null && array6[1][0] == num13 && l != k)
					{
						ANIM.Remove(array6);
					}
				}
			}
		}
		newMAP = new byte[LEDConstants.LED_COUNT * 3];
		int num14 = 0;
		foreach (int[] item in LEDS.Reverse())
		{
			if (item == null)
			{
				continue;
			}
			foreach (int item2 in item.Reverse())
			{
				newMAP = addByteToArray(newMAP, (byte)item2);
				num14++;
			}
		}
		Display(newMAP);
	}

	public void StoreCache(LEDCache LC)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LEDCache));
		string text = "Cache";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		try
		{
			FileStream fileStream = File.Create(text + "//UserSets.xml");
			xmlSerializer.Serialize(fileStream, LC);
			fileStream.Close();
		}
		catch
		{
			MainWindow.LED.consoleLog("Cache write error.");
		}
	}

	public LEDCache LoadCache(LEDCache LC)
	{
		string path = "Cache//UserSets.xml";
		if (File.Exists(path))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LEDCache));
			try
			{
				string text;
				using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
				{
					text = streamReader.ReadToEnd();
				}
				using Stream stream = new FileStream(path, FileMode.Open);
				if (stream != null && text.Contains("<LEDCache xmlns:xsi="))
				{
					LC = (LEDCache)xmlSerializer.Deserialize(stream);
				}
			}
			catch
			{
				MainWindow.LED.consoleLog("Cache read error.");
			}
			return LC;
		}
		return LC;
	}

	public void consoleLog(object o)
	{
	}

	public void SetBright(float br)
	{
		brightness = br;
	}

	public int[][] GenerateGradientMap(int[][] points, int lenght)
	{
		int[][] array = new int[lenght][];
		if (lenght >= array.Length)
		{
			int num = 0;
			for (int i = 0; i < points.Length; i++)
			{
				int[] obj = points[i];
				int num2 = obj[0];
				int num3 = obj[1];
				int num4 = obj[2];
				if (i + 1 < points.Length)
				{
					int[] obj2 = points[i + 1];
					int num5 = obj2[0];
					int num6 = obj2[1];
					int num7 = obj2[2];
					int num8 = (num5 - num2) / (lenght / (points.Length - 1));
					int num9 = (num6 - num3) / (lenght / (points.Length - 1));
					int num10 = (num7 - num4) / (lenght / (points.Length - 1));
					_ = new int[3] { num8, num9, num10 };
					for (int j = 1; j <= lenght / (points.Length - 1); j++)
					{
						array[num] = new int[3] { num2, num3, num4 };
						num2 += num8;
						num3 += num9;
						num4 += num10;
						num++;
					}
				}
			}
		}
		return array;
	}
}
