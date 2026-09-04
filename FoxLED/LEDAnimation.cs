using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Timers;
using System.Windows.Media;
using Microsoft.Win32;

namespace FoxLED;

public class LEDAnimation
{
	private Timer aTimer = new Timer();

	private static int foxled_x = 0;

	private static int rainbow_x = 0;

	private static int colors_x = 0;

	private static int colors_tr = 6;

	private static int[] colors_last_color = new int[3];

	private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

	private static int cpl_m = 0;

	private static int[] cpl_prev_color = new int[3];

	private static int[] cpl_prev_tr_color = new int[3];

	private static int cpl_tr = 0;

	private static int cpuLoad = 0;

	private static int cpt_m = 0;

	private static int[] cpt_prev_color = new int[3];

	private static int[] cpt_prev_tr_color = new int[3];

	private static int cpt_tr = 0;

	private static int cpuTemp = 0;

	private static int[] syscolor_last_color = new int[3];

	private static int[] syscolor_last_tr_color = new int[3] { -1, -1, -1 };

	private static int syscolor_tr = 1;

	private static string last_wall = "";

	private static int[] last_wall_color = new int[3];

	private static int[] last_wall_tr_color = new int[3];

	private static int wall_tr = 0;

	private static void black(LEDConnect LD)
	{
		LD.SetAllLED(new int[3]);
		LD.UpdateLED();
	}

	private static void custom_color(LEDConnect LD)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		double[] custom_color_hsb = MainWindow.LCACHE.custom_color_hsb;
		Color val = LD.ColorFromAhsb(255, (float)custom_color_hsb[0], (float)custom_color_hsb[1], (float)custom_color_hsb[2]);
		int r = ((Color)(ref val)).R;
		int g = ((Color)(ref val)).G;
		int b = ((Color)(ref val)).B;
		LD.SetAllLED(new int[3] { r, g, b });
		LD.UpdateLED();
	}

	private static void foxled(LEDConnect LD)
	{
		int[][] points = new int[4][]
		{
			new int[3] { 61, 129, 209 },
			new int[3] { 125, 20, 189 },
			new int[3] { 125, 20, 189 },
			new int[3] { 61, 129, 209 }
		};
		int[][] array = LD.GenerateGradientMap(points, LEDConstants.LED_COUNT * 2);
		foxled_x++;
		if (foxled_x > array.Length - 1)
		{
			foxled_x = 0;
		}
		LD.Animate(array[foxled_x], 1, LEDConstants.LED_COUNT, 1);
		LD.UpdateLED();
	}

	private static void red(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 255, 0, 0 });
		LD.UpdateLED();
	}

	private static void green(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 0, 255, 0 });
		LD.UpdateLED();
	}

	private static void blue(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 0, 0, 255 });
		LD.UpdateLED();
	}

	private static void cyan(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 0, 255, 255 });
		LD.UpdateLED();
	}

	private static void rainbow(LEDConnect LD)
	{
		int[][] points = new int[7][]
		{
			new int[3] { 255, 0, 0 },
			new int[3] { 255, 255, 0 },
			new int[3] { 0, 255, 0 },
			new int[3] { 0, 255, 255 },
			new int[3] { 0, 0, 255 },
			new int[3] { 255, 0, 255 },
			new int[3] { 255, 0, 0 }
		};
		int[][] array = LD.GenerateGradientMap(points, LEDConstants.LED_COUNT * 3);
		rainbow_x++;
		if (rainbow_x > array.Length - 1)
		{
			rainbow_x = 0;
		}
		LD.Animate(array[rainbow_x], 1, LEDConstants.LED_COUNT, 1);
		LD.UpdateLED();
	}

	private static void colors(LEDConnect LD)
	{
		int[][] array = new int[7][]
		{
			new int[3] { 255, 0, 0 },
			new int[3] { 255, 255, 0 },
			new int[3] { 0, 255, 0 },
			new int[3] { 0, 255, 255 },
			new int[3] { 0, 0, 255 },
			new int[3] { 255, 0, 255 },
			new int[3] { 255, 0, 0 }
		};
		int[] transitionColor = LD.GetTransitionColor(colors_tr);
		if (transitionColor[0] == -1)
		{
			int[] end_rgb = new int[3]
			{
				array[colors_x][0],
				array[colors_x][1],
				array[colors_x][2]
			};
			colors_x++;
			if (colors_x > array.Length - 1)
			{
				colors_x = 0;
			}
			colors_tr = LD.Transition(colors_last_color, end_rgb);
			colors_last_color = end_rgb;
			transitionColor = LD.GetTransitionColor(colors_tr);
		}
		else
		{
			colors_last_color = transitionColor;
		}
		LD.SetAllLED(new int[3]
		{
			transitionColor[0],
			transitionColor[1],
			transitionColor[2]
		});
		LD.UpdateLED();
	}

	private static void coral(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 100, 50, 255 });
		LD.UpdateLED();
	}

	private static void light_green(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 50, 255, 0 });
		LD.UpdateLED();
	}

	private static void yellow(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 255, 255, 0 });
		LD.UpdateLED();
	}

	private static void white(LEDConnect LD)
	{
		LD.SetAllLED(new int[3] { 255, 255, 255 });
		LD.UpdateLED();
	}

	private static void cpu_load(LEDConnect LD)
	{
		cpl_m++;
		if (cpl_m > 100)
		{
			cpuLoad = (int)cpuCounter.NextValue();
			cpl_m = 0;
		}
		int[] array = LD.GetTransitionColor(cpl_tr);
		if (array[0] == -1)
		{
			int[] array2 = new int[3];
			array2 = ((cpuLoad <= 20 || cpuLoad >= 70) ? new int[3] { 255, 0, 0 } : new int[3] { 0, 255, 0 });
			if (cpuLoad < 20)
			{
				array2 = new int[3] { 0, 0, 255 };
			}
			if (array2 == cpl_prev_color)
			{
				array = ((cpl_prev_tr_color[0] == -1) ? cpl_prev_color : cpl_prev_tr_color);
			}
			else
			{
				cpl_tr = LD.Transition(cpl_prev_color, array2);
				cpl_prev_color = array2;
				array = LD.GetTransitionColor(cpl_tr);
			}
		}
		else
		{
			cpl_prev_tr_color = array;
		}
		LD.SetAllLED(new int[3]
		{
			array[0],
			array[1],
			array[2]
		});
		LD.UpdateLED();
	}

	private static void cpu_temp(LEDConnect LD)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		cpt_m++;
		if (cpt_m > 100)
		{
			try
			{
				List<int> list = new List<int>();
				ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature").Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num = (int)Convert.ToDouble(((ManagementBaseObject)(ManagementObject)enumerator.Current)["CurrentTemperature"].ToString());
						num = (int)((double)(num - 2732) / 10.0);
						list.Add(num);
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
				MainWindow.LED.consoleLog(list.ToString());
				cpuTemp = 0;
			}
			catch (Exception ex)
			{
				MainWindow.LED.consoleLog("CPU Temp get error: " + ex.Message);
			}
			MainWindow.LED.consoleLog(cpuTemp);
			cpt_m = 0;
		}
		int[] array = LD.GetTransitionColor(cpt_tr);
		if (array[0] == -1)
		{
			int[] array2 = new int[3];
			array2 = ((cpuTemp <= 20 || cpuTemp >= 50) ? new int[3] { 255, 0, 0 } : new int[3] { 0, 255, 0 });
			if (cpuTemp < 20)
			{
				array2 = new int[3] { 0, 0, 255 };
			}
			if (array2 == cpt_prev_color)
			{
				array = ((cpt_prev_tr_color[0] == -1) ? cpt_prev_color : cpt_prev_tr_color);
			}
			else
			{
				cpt_tr = LD.Transition(cpt_prev_color, array2);
				cpt_prev_color = array2;
				array = LD.GetTransitionColor(cpt_tr);
			}
		}
		else
		{
			cpt_prev_tr_color = array;
		}
		LD.SetAllLED(new int[3]
		{
			array[0],
			array[1],
			array[2]
		});
		LD.UpdateLED();
	}

	public static double[] RGBtoYUV(int red, int green, int blue)
	{
		double[] array = new double[3];
		double num = (double)red / 255.0;
		double num2 = (double)green / 255.0;
		double num3 = (double)blue / 255.0;
		array[0] = 0.299 * num + 0.587 * num2 + 0.114 * num3;
		array[1] = -0.14713769751693 * num - 0.28886230248307 * num2 + 0.436 * num3;
		array[2] = 0.615 * num - 0.5149857346647646 * num2 - 0.10001426533523537 * num3;
		return array;
	}

	public static int[] YUVtoRGB(double y, double u, double v)
	{
		return new int[3]
		{
			Convert.ToInt32((y + 1.1398373983739838 * v) * 255.0),
			Convert.ToInt32((y - 0.39465170435897035 * u - 0.5805986066674976 * v) * 255.0),
			Convert.ToInt32((y + 2.032110091743119 * u) * 255.0)
		};
	}

	private static void screen_capture(LEDConnect LD)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		int num = MainWindow.LCACHE.monitor_index;
		if (num > MainWindow.Screens.Length - 1)
		{
			num = MainWindow.Screens.Length - 1;
		}
		if (num < 0)
		{
			num = 0;
		}
		Bitmap val = new Bitmap(MainWindow.Screens[num].Bounds.Width, MainWindow.Screens[num].Bounds.Height);
		Bitmap val2 = new Bitmap((int)LEDConstants.LED_COUNT, (int)LEDConstants.LED_COUNT);
		Graphics obj = Graphics.FromImage((Image)(object)val);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		obj.CopyFromScreen(MainWindow.Screens[num].Bounds.X, MainWindow.Screens[num].Bounds.Y, 0, 0, ((Image)val).Size, (CopyPixelOperation)13369376);
		val3.DrawImage((Image)(object)val, 0, 0, (int)LEDConstants.LED_COUNT, (int)LEDConstants.LED_COUNT);
		for (int i = 0; i < LEDConstants.LED_COUNT; i++)
		{
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			for (int j = 0; j < LEDConstants.LED_COUNT; j++)
			{
				Color pixel = val2.GetPixel(i, j);
				double[] array = RGBtoYUV(pixel.R, pixel.G, pixel.B);
				num2 += array[0];
				num3 += array[1];
				num4 += array[2];
			}
			double y = num2 / (double)(int)LEDConstants.LED_COUNT;
			double u = num3 / (double)(int)LEDConstants.LED_COUNT;
			double v = num4 / (double)(int)LEDConstants.LED_COUNT;
			int[] array2 = YUVtoRGB(y, u, v);
			LD.SetLED(i + 1, new int[3]
			{
				array2[0],
				array2[1],
				array2[2]
			});
		}
		LD.UpdateLED();
	}

	public static int[] IntToRgb(int value)
	{
		int num = value & 0xFF;
		int num2 = (value >> 8) & 0xFF;
		int num3 = (value >> 16) & 0xFF;
		return new int[3] { num3, num2, num };
	}

	private static void syscolor(LEDConnect LD)
	{
		int num = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\DWM", "ColorizationColor", 0);
		if (num != 0)
		{
			int[] array = IntToRgb(num);
			int[] array2 = LD.GetTransitionColor(syscolor_tr);
			if (array2[0] == -1)
			{
				if (array[0] != syscolor_last_color[0] || array[1] != syscolor_last_color[1] || array[2] != syscolor_last_color[2])
				{
					syscolor_tr = LD.Transition(syscolor_last_color, array);
					syscolor_last_color = array;
					array2 = LD.GetTransitionColor(syscolor_tr);
				}
				else
				{
					array2 = ((syscolor_last_tr_color[0] == -1) ? syscolor_last_color : syscolor_last_tr_color);
				}
			}
			else
			{
				syscolor_last_tr_color = array2;
			}
			LD.SetAllLED(new int[3]
			{
				array2[0],
				array2[1],
				array2[2]
			});
		}
		else
		{
			LD.SetAllLED(new int[3]);
		}
		LD.UpdateLED();
	}

	public static Color getDominantColor(Bitmap bmp)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < ((Image)bmp).Width; i++)
		{
			for (int j = 0; j < ((Image)bmp).Height; j++)
			{
				Color pixel = bmp.GetPixel(i, j);
				num += pixel.R;
				num2 += pixel.G;
				num3 += pixel.B;
				num4++;
			}
		}
		num /= num4;
		num2 /= num4;
		num3 /= num4;
		return Color.FromArgb(num, num2, num3);
	}

	private static void wall_color(LEDConnect LD)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", writable: false);
		string text = registryKey.GetValue("WallPaper").ToString();
		registryKey.Close();
		int[] array = LD.GetTransitionColor(wall_tr);
		if (array[0] == -1)
		{
			if (!(text != last_wall) || text == null)
			{
				array = ((syscolor_last_tr_color[0] == -1) ? last_wall_color : last_wall_tr_color);
			}
			else
			{
				Bitmap val = new Bitmap((Image)new Bitmap(text), 300, 200);
				last_wall = text;
				Color dominantColor = getDominantColor(val);
				int[] array2 = new int[3] { dominantColor.R, dominantColor.G, dominantColor.B };
				if (array2[0] != last_wall_color[0] && array2[1] != last_wall_color[1] && array2[2] != last_wall_color[2])
				{
					wall_tr = LD.Transition(last_wall_color, array2);
					last_wall_color = new int[3]
					{
						array2[0],
						array2[1],
						array2[2]
					};
				}
				array = LD.GetTransitionColor(wall_tr);
			}
		}
		else
		{
			last_wall_tr_color = new int[3]
			{
				array[0],
				array[1],
				array[2]
			};
		}
		LD.SetAllLED(new int[3]
		{
			array[0],
			array[1],
			array[2]
		});
		LD.UpdateLED();
	}

	public void start(string a)
	{
		MainWindow.LED.ClearAnimations();
		MainWindow.LED.ClearTransitions();
		stop();
		aTimer = new Timer();
		aTimer.AutoReset = true;
		aTimer.Enabled = true;
		MainWindow.LCACHE.last_anim = a;
		MainWindow.LED.StoreCache(MainWindow.LCACHE);
		if (MainWindow.AN != null)
		{
			MainWindow.AN.Stop();
		}
		if (a == null)
		{
			return;
		}
		switch (a)
		{
		case "0":
		case "black":
			aTimer.Elapsed += delegate
			{
				black(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			black(MainWindow.LED);
			break;
		case "1":
		case "foxled":
			aTimer.Elapsed += delegate
			{
				foxled(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			foxled(MainWindow.LED);
			break;
		case "2":
		case "red":
			aTimer.Elapsed += delegate
			{
				red(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			red(MainWindow.LED);
			break;
		case "3":
		case "green":
			aTimer.Elapsed += delegate
			{
				green(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			green(MainWindow.LED);
			break;
		case "4":
		case "blue":
			aTimer.Elapsed += delegate
			{
				blue(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			blue(MainWindow.LED);
			break;
		case "5":
		case "cyan":
			aTimer.Elapsed += delegate
			{
				cyan(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			cyan(MainWindow.LED);
			break;
		case "6":
		case "coral":
			aTimer.Elapsed += delegate
			{
				coral(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			coral(MainWindow.LED);
			break;
		case "7":
		case "yellow":
			aTimer.Elapsed += delegate
			{
				yellow(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			yellow(MainWindow.LED);
			break;
		case "8":
		case "light_green":
			aTimer.Elapsed += delegate
			{
				light_green(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			light_green(MainWindow.LED);
			break;
		case "9":
		case "white":
			aTimer.Elapsed += delegate
			{
				white(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			white(MainWindow.LED);
			break;
		case "10":
		case "rainbow":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				rainbow(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			rainbow(MainWindow.LED);
			break;
		case "15":
		case "screen_capture":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				screen_capture(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			screen_capture(MainWindow.LED);
			break;
		case "16":
		case "syscolor":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				syscolor(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			syscolor(MainWindow.LED);
			break;
		case "17":
		case "colors":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				colors(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			colors(MainWindow.LED);
			break;
		case "11":
		case "colormusic":
			MainWindow.AN.Start();
			break;
		case "12":
		case "wall_color":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				wall_color(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			wall_color(MainWindow.LED);
			break;
		case "13":
		case "cpu_load":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				cpu_load(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			cpu_load(MainWindow.LED);
			break;
		case "14":
		case "cpu_temp":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				cpu_temp(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			cpu_temp(MainWindow.LED);
			break;
		case "666":
		case "custom_color":
			black(MainWindow.LED);
			aTimer.Elapsed += delegate
			{
				custom_color(MainWindow.LED);
			};
			aTimer.Interval = MainWindow.LCACHE.speed;
			custom_color(MainWindow.LED);
			break;
		}
	}

	public void stop()
	{
		aTimer.Stop();
		aTimer.Enabled = false;
	}
}
