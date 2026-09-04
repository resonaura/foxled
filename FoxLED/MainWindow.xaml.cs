using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FoxLED;

public partial class MainWindow : Window, IComponentConnector
{
	public static LEDConnect LED = new LEDConnect();

	public static LEDCache LCACHE = new LEDCache();

	public static Analyzer AN = new Analyzer();

	public static LEDAnimation LEDAnim = new LEDAnimation();

	public static LEDRemote LR = new LEDRemote();

	public static LinearGradientBrush gr = new LinearGradientBrush();

	private Timer remoteModifyTimer = new Timer();

	private string hotkey_anim_id = "0";

	public static Screen[] Screens = Screen.AllScreens;

	public static Rectangle psL = new Rectangle();

	private string last_upd_an = "-1";

	public static Color accent_color = Color.FromRgb((byte)61, (byte)129, (byte)209);

	public static Color single_color = Color.FromRgb((byte)104, (byte)49, (byte)180);

	private bool remote_active;

	private bool slider_move;

	private string draggedSlider = "";

	private double hue;

	private double saturation;

	private double brightness;

	private string cp_active_slider = "";

	private bool cp_dragging;

	internal MainWindow MainAppWindow;

	internal RowDefinition PSL_ROW;

	internal Grid header;

	internal Grid AppLogo;

	internal Label AppTitle;

	internal Button minim;

	internal Canvas minim_icon;

	internal Path minim_path;

	internal Button close;

	internal Canvas close_icon;

	internal Path close_path;

	internal Button hp;

	internal Canvas hp_icon;

	internal Path hp_accent_path;

	internal Path hp_single_path;

	internal Button st;

	internal Canvas st_icon;

	internal Path st_accent_path;

	internal Path st_single_path;

	internal Button custom;

	internal Canvas custom_icon;

	internal Path custom_accent_path;

	internal Path custom_single_path;

	internal Button inf;

	internal Canvas inf_icon;

	internal Path inf_accent_path;

	internal Path inf_single_path;

	internal TabControl content;

	internal TabItem hpTab;

	internal StackPanel modes;

	internal StackPanel def_fx_buttons;

	internal StackPanel def_fx_buttons_1;

	internal Button static_foxled;

	internal Button static_black;

	internal StackPanel static_fx_buttons;

	internal StackPanel static_fx_buttons_1;

	internal Button static_red;

	internal Button static_green;

	internal Button static_blue;

	internal Button static_cyan;

	internal StackPanel static_fx_buttons_2;

	internal Button static_coral;

	internal Button static_yellow;

	internal Button static_light_green;

	internal Button static_white;

	internal StackPanel dynamic_fx_buttons;

	internal StackPanel dynamic_fx_buttons_1;

	internal Button dynamic_rainbow;

	internal Button dynamic_colormusic;

	internal Button dynamic_wallcolor;

	internal Button dynamic_cpu_load;

	internal StackPanel dynamic_fx_buttons_2;

	internal Button dynamic_cpu_temp;

	internal Button dynamic_screen_capture;

	internal Button dynamic_syscolor;

	internal Button dynamic_colors;

	internal TabItem stTab;

	internal Slider speed_slider;

	internal Slider brightness_slider;

	internal ComboBox screens_list;

	internal ComboBox leds_list;

	internal Button remote_button;

	internal StackPanel TokenPanel;

	internal TextBox Token_Textbox;

	internal StackPanel RestartPanel;

	internal TabItem custTab;

	internal Grid Colorpicker;

	internal Rectangle hue_box;

	internal Viewbox cp_arrow;

	internal Slider Hue_slider;

	internal Rectangle result_color;

	internal TabItem infTab;

	internal Canvas big_logo;

	internal Rectangle PseudoLED;

	private bool _contentLoaded;

	public void hotkeysLogic(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected I4, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		Button val = new Button();
		if ((int)e.Key == 74 || (int)e.Key == 75 || (int)e.Key == 76 || (int)e.Key == 77 || (int)e.Key == 78 || (int)e.Key == 79 || (int)e.Key == 80 || (int)e.Key == 81 || (int)e.Key == 82 || (int)e.Key == 83)
		{
			ClearModeButtons();
			LEDAnim.stop();
		}
		Key key = e.Key;
		switch (key - 74)
		{
		case 0:
			val = static_foxled;
			hotkey_anim_id = "0";
			break;
		case 1:
			val = static_black;
			hotkey_anim_id = "1";
			break;
		case 2:
			val = static_red;
			hotkey_anim_id = "2";
			break;
		case 3:
			val = static_green;
			hotkey_anim_id = "3";
			break;
		case 4:
			val = static_blue;
			hotkey_anim_id = "4";
			break;
		case 5:
			val = static_cyan;
			hotkey_anim_id = "5";
			break;
		case 6:
			val = static_coral;
			hotkey_anim_id = "6";
			break;
		case 7:
			val = static_yellow;
			hotkey_anim_id = "7";
			break;
		case 8:
			val = static_light_green;
			hotkey_anim_id = "8";
			break;
		case 9:
			val = static_white;
			hotkey_anim_id = "9";
			break;
		}
		LEDAnim.start(hotkey_anim_id);
		((UIElement)val).IsEnabled = false;
		((Control)val).Background = (Brush)(object)gr;
		((Control)val).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
	}

	public MainWindow()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Expected O, but got Unknown
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Expected O, but got Unknown
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		InitializeComponent();
		((Window)MainAppWindow).Activated += RestoreApp;
		RoutedEventHandler handler = null;
		handler = (RoutedEventHandler)delegate
		{
			((FrameworkElement)this).Loaded -= handler;
			((Window)(object)this).EnableBlur();
		};
		((FrameworkElement)this).Loaded += handler;
		LR.window = (Window)(object)this;
		LED.AutoConnect();
		hue = LCACHE.custom_color_hsb[0];
		saturation = LCACHE.custom_color_hsb[1];
		brightness = LCACHE.custom_color_hsb[2];
		if (!LED.conn)
		{
			psL = PseudoLED;
			PSL_ROW.Height = new GridLength(20.0);
			((FrameworkElement)this).Height = ((FrameworkElement)this).Height + 20.0;
		}
		((RangeBase)Hue_slider).Value = hue / 360.0 * 10.0;
		double num = 1.0 - brightness / (1.0 - saturation / 2.0);
		((FrameworkElement)cp_arrow).Margin = new Thickness(saturation * ((FrameworkElement)Colorpicker).Width - ((FrameworkElement)cp_arrow).Width / 2.0, num * ((FrameworkElement)Colorpicker).Height - ((FrameworkElement)cp_arrow).Height / 2.0, 0.0, 0.0);
		LED.consoleLog(hue);
		LCACHE = LED.LoadCache(LCACHE);
		Screen[] screens = Screens;
		foreach (Screen val in screens)
		{
			((ItemsControl)screens_list).Items.Add((object)val.DeviceName);
		}
		((Selector)screens_list).SelectedIndex = LCACHE.monitor_index;
		((Selector)screens_list).SelectionChanged += new SelectionChangedEventHandler(Screens_list_SelectionChanged);
		for (int j = 10; j <= 300; j++)
		{
			((ItemsControl)leds_list).Items.Add((object)j);
		}
		if (LCACHE.led_num < 10 || LCACHE.led_num > 300)
		{
			LCACHE.led_num = 30;
			LED.StoreCache(LCACHE);
		}
		((Selector)leds_list).SelectedIndex = LCACHE.led_num - 10;
		((Selector)leds_list).SelectionChanged += new SelectionChangedEventHandler(Leds_list_SelectionChanged);
		remoteModifyTimer.Interval = 1.0;
		remoteModifyTimer.Elapsed += remoteModifyTimerElapsed;
		remoteModifyTimer.Start();
		((UIElement)this).KeyDown += new KeyEventHandler(hotkeysLogic);
		gr.StartPoint = new Point(0.0, 0.0);
		gr.EndPoint = new Point(1.0, 1.0);
		GradientStop val2 = new GradientStop();
		val2.Color = accent_color;
		val2.Offset = 0.0;
		((GradientBrush)gr).GradientStops.Add(val2);
		GradientStop val3 = new GradientStop();
		val3.Color = single_color;
		val3.Offset = 1.0;
		((GradientBrush)gr).GradientStops.Add(val3);
		LR.StartDaemon();
		if (LR.active_state == "enabled")
		{
			remote_active = true;
			((ContentControl)remote_button).Content = "Выключить";
			((Control)remote_button).Background = (Brush)(object)gr;
			((Control)remote_button).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		}
		((RangeBase)speed_slider).Value = (double)(40 - LCACHE.speed) / 0.4;
		((RangeBase)brightness_slider).Value = LCACHE.brightness * 100f;
		LED.SetBright(LCACHE.brightness);
		updateModeButtons(LCACHE.last_anim, LCACHE);
		LEDAnim.start(LCACHE.last_anim);
		((UIElement)hp).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)hp).MouseLeave += new MouseEventHandler(NavHoverLeave);
		((ButtonBase)hp).Click += new RoutedEventHandler(NavClick);
		((UIElement)st).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)st).MouseLeave += new MouseEventHandler(NavHoverLeave);
		((ButtonBase)st).Click += new RoutedEventHandler(NavClick);
		((UIElement)inf).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)inf).MouseLeave += new MouseEventHandler(NavHoverLeave);
		((ButtonBase)inf).Click += new RoutedEventHandler(NavClick);
		((UIElement)custom).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)custom).MouseLeave += new MouseEventHandler(NavHoverLeave);
		((ButtonBase)custom).Click += new RoutedEventHandler(NavClick);
		((Control)hp).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)20, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)hp_accent_path).Fill = (Brush)(object)gr;
		((UIElement)hp_accent_path).Opacity = 1.0;
		((UIElement)hp_single_path).Opacity = 1.0;
		((Shape)hp_single_path).Fill = (Brush)(object)gr;
		((UIElement)hp).IsEnabled = false;
		((UIElement)header).MouseDown += new MouseButtonEventHandler(MoveWindow);
		((UIElement)AppLogo).MouseDown += new MouseButtonEventHandler(MoveWindow);
		((UIElement)AppTitle).MouseDown += new MouseButtonEventHandler(MoveWindow);
		((ButtonBase)close).Click += new RoutedEventHandler(CloseApp);
		((ButtonBase)minim).Click += new RoutedEventHandler(MinApp);
		((UIElement)close).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)close).MouseLeave += new MouseEventHandler(NavHoverLeave);
		((UIElement)minim).MouseEnter += new MouseEventHandler(NavHoverEnter);
		((UIElement)minim).MouseLeave += new MouseEventHandler(NavHoverLeave);
	}

	public static void PSfillIt(int[][] colors)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		double num = colors.Length;
		double num2 = 1.0 / num;
		LinearGradientBrush val = new LinearGradientBrush();
		val.StartPoint = new Point(0.0, 0.0);
		val.EndPoint = new Point(1.0, 0.0);
		for (int i = 0; i < colors.Length; i++)
		{
			double num3 = (double)i * num2;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			if (num3 > 1.0)
			{
				num3 = 1.0;
			}
			if (colors[i] != null)
			{
				((GradientBrush)val).GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, (byte)colors[i][0], (byte)colors[i][1], (byte)colors[i][2]), num3));
			}
		}
		((Shape)psL).Fill = (Brush)(object)val;
	}

	public static void fillPseudo(int[][] colors)
	{
		((DispatcherObject)Application.Current).Dispatcher.Invoke((Action)delegate
		{
			PSfillIt(colors);
		});
	}

	private void Leds_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		LCACHE.led_num = ((Selector)leds_list).SelectedIndex + 10;
		((UIElement)RestartPanel).Visibility = (Visibility)0;
		LED.StoreCache(LCACHE);
	}

	private void Screens_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		LCACHE.monitor_index = ((Selector)screens_list).SelectedIndex;
		LED.StoreCache(LCACHE);
	}

	private void updateModeButtons(string an, LEDCache LC)
	{
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		if (an != last_upd_an)
		{
			ClearModeButtons();
			last_upd_an = an;
		}
		Button val = static_foxled;
		switch (an)
		{
		case "0":
		case "black":
			val = static_black;
			break;
		case "1":
		case "foxled":
			val = static_foxled;
			break;
		case "2":
		case "red":
			val = static_red;
			break;
		case "3":
		case "green":
			val = static_green;
			break;
		case "4":
		case "blue":
			val = static_blue;
			break;
		case "5":
		case "cyan":
			val = static_cyan;
			break;
		case "6":
		case "coral":
			val = static_coral;
			break;
		case "8":
		case "light_green":
			val = static_light_green;
			break;
		case "7":
		case "yellow":
			val = static_yellow;
			break;
		case "9":
		case "white":
			val = static_white;
			break;
		case "10":
		case "rainbow":
			val = dynamic_rainbow;
			break;
		case "17":
		case "colors":
			val = dynamic_colors;
			break;
		case "16":
		case "syscolor":
			val = dynamic_syscolor;
			break;
		case "15":
		case "screen_capture":
			val = dynamic_screen_capture;
			break;
		case "11":
		case "colormusic":
			val = dynamic_colormusic;
			break;
		case "12":
		case "wall_color":
			val = dynamic_wallcolor;
			break;
		case "13":
		case "cpu_load":
			val = dynamic_cpu_load;
			break;
		case "14":
		case "cpu_temp":
			val = dynamic_cpu_temp;
			break;
		case "666":
		case "custom_color":
			val = new Button();
			break;
		}
		((UIElement)val).IsEnabled = false;
		((Control)val).Background = (Brush)(object)gr;
		((Control)val).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
	}

	private void updateBrightAndSpeed()
	{
		if (LCACHE.user_id != 0)
		{
			if ((double)(40 - LCACHE.speed) / 0.4 != ((RangeBase)speed_slider).Value)
			{
				((RangeBase)speed_slider).Value = (double)(40 - LCACHE.speed) / 0.4;
			}
			if ((double)(LCACHE.brightness * 100f) != ((RangeBase)brightness_slider).Value)
			{
				((RangeBase)brightness_slider).Value = LCACHE.brightness * 100f;
			}
		}
	}

	private void updateScreensList()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		Screen[] allScreens = Screen.AllScreens;
		if (allScreens != Screens)
		{
			Screens = allScreens;
			((ItemsControl)screens_list).Items.Clear();
			Screen[] screens = Screens;
			foreach (Screen val in screens)
			{
				((ItemsControl)screens_list).Items.Add((object)val.DeviceName);
			}
			if (LCACHE.monitor_index < Screens.Length)
			{
				((Selector)screens_list).SelectedIndex = LCACHE.monitor_index;
			}
			else
			{
				((Selector)screens_list).SelectedIndex = Screens.Length - 1;
			}
			if (((Selector)screens_list).SelectedIndex == -1)
			{
				((Selector)screens_list).SelectedIndex = 0;
			}
			((Selector)screens_list).SelectionChanged += new SelectionChangedEventHandler(Screens_list_SelectionChanged);
		}
	}

	private void remoteModifyTimerElapsed(object sender, ElapsedEventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		((DispatcherObject)this).Dispatcher.Invoke((Action)delegate
		{
			updateScreensList();
		});
		if (LR.active_state == "enabled")
		{
			((DispatcherObject)this).Dispatcher.Invoke<Visibility>((Func<Visibility>)delegate
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				StackPanel tokenPanel = TokenPanel;
				Visibility result = (Visibility)1;
				((UIElement)tokenPanel).Visibility = (Visibility)1;
				return result;
			});
			((DispatcherObject)this).Dispatcher.Invoke((Action)delegate
			{
				updateModeButtons(LCACHE.last_anim, LCACHE);
			});
			((DispatcherObject)this).Dispatcher.Invoke((Action)delegate
			{
				updateBrightAndSpeed();
			});
		}
		_ = LR.active_state == "disabled";
	}

	private void MoveWindow(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.LeftButton == 1)
			{
				((Window)this).DragMove();
			}
		}
		catch
		{
			LED.consoleLog("Move window error.");
		}
	}

	private void CloseApp(object sender, EventArgs e)
	{
		if (AN != null)
		{
			AN.Stop();
			AN.CaptureInstance.Dispose();
		}
		LR.StopDaemon();
		LR.onAppClose();
		((Window)this).Close();
		LED.ClearAnimations();
		LED.ClearTransitions();
		LEDAnim.stop();
		for (int i = 0; i < LEDConstants.LED_COUNT; i++)
		{
			LED.SetLED(i + 1, new int[3]);
			LED.UpdateLED();
		}
		LED.SetAllLED(new int[3]);
		LED.UpdateLED();
		Environment.Exit(0);
	}

	private void MinApp(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		DoubleAnimation val = new DoubleAnimation(0.0, Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0)));
		((Timeline)val).Completed += delegate
		{
			SystemCommands.MinimizeWindow((Window)(object)this);
			((UIElement)MainAppWindow).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)null);
			((UIElement)MainAppWindow).Opacity = 1.0;
		};
		((UIElement)MainAppWindow).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)(object)val);
	}

	private void RestoreApp(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		((UIElement)MainAppWindow).Opacity = 0.0;
		DoubleAnimation val = new DoubleAnimation(1.0, Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0)));
		((UIElement)MainAppWindow).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)(object)val);
		((Timeline)val).Completed += delegate
		{
			((UIElement)MainAppWindow).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)null);
			((UIElement)MainAppWindow).Opacity = 1.0;
		};
	}

	private void NavHoverEnter(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Button s = (Button)((sender is Button) ? sender : null);
		if (((UIElement)s).IsEnabled)
		{
			BrushAnimation brushAnimation = new BrushAnimation();
			brushAnimation.From = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			brushAnimation.To = (Brush)new SolidColorBrush(Color.FromArgb((byte)10, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			((Timeline)brushAnimation).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0));
			((Timeline)brushAnimation).Completed += delegate
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Expected O, but got Unknown
				((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)null);
				((Control)s).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)10, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			};
			((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)(object)brushAnimation);
		}
	}

	private void NavHoverLeave(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Button s = (Button)((sender is Button) ? sender : null);
		if (((UIElement)s).IsEnabled)
		{
			BrushAnimation brushAnimation = new BrushAnimation();
			brushAnimation.From = (Brush)new SolidColorBrush(Color.FromArgb((byte)10, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			brushAnimation.To = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			((Timeline)brushAnimation).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0));
			((Timeline)brushAnimation).Completed += delegate
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Expected O, but got Unknown
				((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)null);
				((Control)s).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			};
			((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)(object)brushAnimation);
		}
	}

	private void NavClick(object sender, EventArgs e)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		Button s = (Button)((sender is Button) ? sender : null);
		string Name = ((FrameworkElement)s).Name;
		((UIElement)hp).IsEnabled = true;
		((UIElement)st).IsEnabled = true;
		((UIElement)inf).IsEnabled = true;
		((UIElement)custom).IsEnabled = true;
		((UIElement)s).IsEnabled = false;
		((Shape)hp_single_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)hp_accent_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)st_single_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)st_accent_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)inf_single_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)inf_accent_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)custom_single_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Shape)custom_accent_path).Fill = (Brush)new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((UIElement)hp_accent_path).Opacity = 0.2;
		((UIElement)hp_single_path).Opacity = 0.1;
		((UIElement)st_accent_path).Opacity = 0.2;
		((UIElement)st_single_path).Opacity = 0.1;
		((UIElement)inf_accent_path).Opacity = 0.2;
		((UIElement)inf_single_path).Opacity = 0.1;
		((UIElement)custom_accent_path).Opacity = 0.2;
		((UIElement)custom_single_path).Opacity = 0.1;
		DoubleAnimation val = new DoubleAnimation();
		val.From = 1.0;
		val.To = 0.0;
		((Timeline)val).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0));
		((Timeline)val).Completed += delegate
		{
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Expected O, but got Unknown
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Expected O, but got Unknown
			if (Name != null)
			{
				switch (Name)
				{
				case "hp":
					((Selector)content).SelectedValue = hpTab;
					((Shape)hp_accent_path).Fill = (Brush)(object)gr;
					((UIElement)hp_accent_path).Opacity = 1.0;
					((UIElement)hp_single_path).Opacity = 1.0;
					((Shape)hp_single_path).Fill = (Brush)(object)gr;
					break;
				case "st":
					((Selector)content).SelectedValue = stTab;
					((Shape)st_accent_path).Fill = (Brush)(object)gr;
					((UIElement)st_accent_path).Opacity = 1.0;
					((UIElement)st_single_path).Opacity = 1.0;
					((Shape)st_single_path).Fill = (Brush)(object)gr;
					break;
				case "custom":
					((Selector)content).SelectedValue = custTab;
					((Shape)custom_accent_path).Fill = (Brush)(object)gr;
					((UIElement)custom_accent_path).Opacity = 1.0;
					((UIElement)custom_single_path).Opacity = 1.0;
					((Shape)custom_single_path).Fill = (Brush)(object)gr;
					break;
				case "inf":
					((Selector)content).SelectedValue = infTab;
					((Shape)inf_accent_path).Fill = (Brush)(object)gr;
					((UIElement)inf_accent_path).Opacity = 1.0;
					((UIElement)inf_single_path).Opacity = 1.0;
					((Shape)inf_single_path).Fill = (Brush)(object)gr;
					break;
				}
			}
			DoubleAnimation val2 = new DoubleAnimation(1.0, Duration.op_Implicit(TimeSpan.FromMilliseconds(500.0)));
			BackEase val3 = new BackEase();
			((EasingFunctionBase)val3).EasingMode = (EasingMode)0;
			val2.EasingFunction = (IEasingFunction)(object)val3;
			((Timeline)val2).Completed += delegate
			{
				((UIElement)content).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)null);
				((UIElement)content).Opacity = 1.0;
			};
			((UIElement)content).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)(object)val2);
		};
		((UIElement)content).BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)(object)val);
		((Control)hp).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Control)st).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Control)inf).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Control)custom).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		BrushAnimation brushAnimation = new BrushAnimation();
		brushAnimation.From = (Brush)new SolidColorBrush(Color.FromArgb((byte)10, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		brushAnimation.To = (Brush)new SolidColorBrush(Color.FromArgb((byte)20, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		((Timeline)brushAnimation).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(100.0));
		((Timeline)brushAnimation).Completed += delegate
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)null);
			((Control)s).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)20, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		};
		((UIElement)s).BeginAnimation(Control.BackgroundProperty, (AnimationTimeline)(object)brushAnimation);
	}

	private void ModeHoverEnter(object sender, MouseEventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		Button val = (Button)((sender is Button) ? sender : null);
		if (((UIElement)val).IsEnabled)
		{
			if (((FrameworkElement)val).Name != "remote_button")
			{
				((Control)val).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)4, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
			else if (!remote_active)
			{
				((Control)val).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)4, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
		}
	}

	private void ModeHoverLeave(object sender, MouseEventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		Button val = (Button)((sender is Button) ? sender : null);
		if (((UIElement)val).IsEnabled)
		{
			if (((FrameworkElement)val).Name != "remote_button")
			{
				((Control)val).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
			else if (!remote_active)
			{
				((Control)val).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
		}
	}

	private void clearModeButton(UIElement ui)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		if (ui is Button)
		{
			((Control)((ui is Button) ? ui : null)).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			((Control)((ui is Button) ? ui : null)).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)153, (byte)153, (byte)153));
			((ui is Button) ? ui : null).IsEnabled = true;
		}
	}

	private void ClearModeButtons()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		foreach (UIElement child in ((Panel)def_fx_buttons_1).Children)
		{
			UIElement ui = child;
			clearModeButton(ui);
		}
		foreach (UIElement child2 in ((Panel)static_fx_buttons_1).Children)
		{
			UIElement ui2 = child2;
			clearModeButton(ui2);
		}
		foreach (UIElement child3 in ((Panel)static_fx_buttons_2).Children)
		{
			UIElement ui3 = child3;
			clearModeButton(ui3);
		}
		foreach (UIElement child4 in ((Panel)dynamic_fx_buttons_1).Children)
		{
			UIElement ui4 = child4;
			clearModeButton(ui4);
		}
		foreach (UIElement child5 in ((Panel)dynamic_fx_buttons_2).Children)
		{
			UIElement ui5 = child5;
			clearModeButton(ui5);
		}
	}

	private void ModeClick(object sender, RoutedEventArgs e)
	{
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
		Button val = (Button)((sender is Button) ? sender : null);
		switch (((FrameworkElement)val).Name)
		{
		case "static_black":
			LEDAnim.stop();
			LEDAnim.start("black");
			break;
		case "static_foxled":
			LEDAnim.stop();
			LEDAnim.start("foxled");
			break;
		case "static_red":
			LEDAnim.stop();
			LEDAnim.start("red");
			break;
		case "static_green":
			LEDAnim.stop();
			LEDAnim.start("green");
			break;
		case "static_blue":
			LEDAnim.stop();
			LEDAnim.start("blue");
			break;
		case "static_cyan":
			LEDAnim.stop();
			LEDAnim.start("cyan");
			break;
		case "static_white":
			LEDAnim.stop();
			LEDAnim.start("white");
			break;
		case "static_coral":
			LEDAnim.stop();
			LEDAnim.start("coral");
			break;
		case "static_yellow":
			LEDAnim.stop();
			LEDAnim.start("yellow");
			break;
		case "static_light_green":
			LEDAnim.stop();
			LEDAnim.start("light_green");
			break;
		case "dynamic_rainbow":
			LEDAnim.stop();
			LEDAnim.start("rainbow");
			break;
		case "dynamic_colors":
			LEDAnim.stop();
			LEDAnim.start("colors");
			break;
		case "dynamic_syscolor":
			LEDAnim.stop();
			LEDAnim.start("syscolor");
			break;
		case "dynamic_screen_capture":
			LEDAnim.stop();
			LEDAnim.start("screen_capture");
			break;
		case "dynamic_colormusic":
			LEDAnim.stop();
			LEDAnim.start("colormusic");
			break;
		case "dynamic_wallcolor":
			LEDAnim.stop();
			LEDAnim.start("wall_color");
			break;
		case "dynamic_cpu_load":
			LEDAnim.stop();
			LEDAnim.start("cpu_load");
			break;
		case "dynamic_cpu_temp":
			LEDAnim.stop();
			LEDAnim.start("cpu_temp");
			break;
		}
		ClearModeButtons();
		((UIElement)val).IsEnabled = false;
		((Control)val).Background = (Brush)(object)gr;
		((Control)val).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
	}

	private void RemoteClick(object sender, RoutedEventArgs e)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		Button val = (Button)((sender is Button) ? sender : null);
		if (!remote_active)
		{
			((UIElement)TokenPanel).Visibility = (Visibility)0;
			string text = LR.generateToken();
			Token_Textbox.Text = "/auth " + text;
			((ContentControl)val).Content = "Выключить";
			((Control)val).Background = (Brush)(object)gr;
			((Control)val).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		}
		else
		{
			((UIElement)TokenPanel).Visibility = (Visibility)1;
			LR.logout();
			((ContentControl)val).Content = "Включить";
			((Control)val).Background = (Brush)new SolidColorBrush(Color.FromArgb((byte)0, (byte)0, (byte)0, (byte)0));
			((Control)val).Foreground = (Brush)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)153, (byte)153, (byte)153));
		}
		remote_active = !remote_active;
	}

	private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		int num = (int)Math.Round(((RangeBase)((sender is Slider) ? sender : null)).Value);
		LCACHE.speed = 40 - num * 40 / 100;
		if (LCACHE.speed <= 0)
		{
			LCACHE.speed = 1;
		}
		LED.StoreCache(LCACHE);
		if (!slider_move)
		{
			LEDAnim.start(LCACHE.last_anim);
		}
	}

	private void brightness_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		int num = (int)Math.Round(((RangeBase)((sender is Slider) ? sender : null)).Value);
		LED.SetBright((float)num / 100f);
		LCACHE.brightness = (float)num / 100f;
		LED.StoreCache(LCACHE);
	}

	private void slider_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Slider val = (Slider)((sender is Slider) ? sender : null);
		if ((int)e.LeftButton == 1)
		{
			if (draggedSlider == "")
			{
				draggedSlider = ((FrameworkElement)val).Name;
			}
			if (draggedSlider == ((FrameworkElement)val).Name)
			{
				slider_move = true;
				Point val2 = ((Visual)val).TransformToVisual((Visual)((ContentControl)this).Content).Transform(new Point(0.0, 0.0));
				double x = ((Point)(ref val2)).X;
				int num = (int)((float)((double)(int)((double)Cursor.Position.X - ((Window)this).Left - x) / ((FrameworkElement)val).Width) * 100f);
				((RangeBase)val).Value = num;
			}
		}
		else if (slider_move)
		{
			slider_move = false;
			((RangeBase)val).Value = ((RangeBase)val).Value + 1.0;
			draggedSlider = "";
		}
	}

	private void Hue_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		float num = (float)((RangeBase)((sender is Slider) ? sender : null)).Value;
		hue = num * 360f / 10f;
		((Shape)hue_box).Fill = (Brush)new SolidColorBrush(LED.ColorFromAhsb(255, (float)hue, 1f, 0.5f));
		((Shape)result_color).Fill = (Brush)new SolidColorBrush(LED.ColorFromAhsb(255, (float)hue, (float)saturation, (float)brightness));
		LCACHE.custom_color_hsb[0] = hue;
		LED.StoreCache(LCACHE);
	}

	private void Colorpicker_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		Grid val = (Grid)((sender is Grid) ? sender : null);
		if (!cp_dragging)
		{
			cp_dragging = true;
			cp_active_slider = ((FrameworkElement)val).Name;
		}
		if ((int)e.LeftButton == 1 && cp_active_slider == ((FrameworkElement)val).Name)
		{
			if (LCACHE.last_anim != "custom_color")
			{
				LEDAnim.stop();
				LEDAnim.start("custom_color");
				LCACHE.last_anim = "custom_color";
			}
			Point val2 = ((Visual)val).TransformToVisual((Visual)((ContentControl)this).Content).Transform(new Point(0.0, 0.0));
			double x = ((Point)(ref val2)).X;
			double y = ((Point)(ref val2)).Y;
			int num = (int)((double)Cursor.Position.X - ((Window)this).Left - x);
			int num2 = (int)((double)Cursor.Position.Y - ((Window)this).Top - y);
			double num3 = (double)num / ((FrameworkElement)val).Width;
			double num4 = (double)num2 / ((FrameworkElement)val).Height;
			((FrameworkElement)cp_arrow).Margin = new Thickness(num3 * ((FrameworkElement)val).Width - ((FrameworkElement)cp_arrow).Width / 2.0, num4 * ((FrameworkElement)val).Height - ((FrameworkElement)cp_arrow).Height / 2.0, 0.0, 0.0);
			hue = ((RangeBase)Hue_slider).Value * 360.0 / 10.0;
			saturation = num3;
			brightness = (1.0 - num3 / 2.0) * (1.0 - num4);
			if (brightness < 0.0)
			{
				brightness = 0.0;
			}
			if (brightness > 1.0)
			{
				brightness = 1.0;
			}
			((Shape)result_color).Fill = (Brush)new SolidColorBrush(LED.ColorFromAhsb(255, (float)hue, (float)saturation, (float)brightness));
			LCACHE.custom_color_hsb[0] = hue;
			LCACHE.custom_color_hsb[1] = saturation;
			LCACHE.custom_color_hsb[2] = brightness;
			LED.StoreCache(LCACHE);
		}
		else if (cp_active_slider == ((FrameworkElement)val).Name)
		{
			cp_dragging = false;
		}
	}

	private void Hue_slider_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		if (!cp_dragging)
		{
			cp_dragging = true;
			cp_active_slider = ((FrameworkElement)Hue_slider).Name;
		}
		if ((int)e.LeftButton == 1 && cp_active_slider == ((FrameworkElement)Hue_slider).Name)
		{
			if (LCACHE.last_anim != "custom_color")
			{
				LEDAnim.stop();
				LEDAnim.start("custom_color");
				LCACHE.last_anim = "custom_color";
			}
			LCACHE.custom_color_hsb[0] = hue;
			LED.StoreCache(LCACHE);
			slider_move = true;
			Point val = ((Visual)Hue_slider).TransformToVisual((Visual)((ContentControl)this).Content).Transform(new Point(0.0, 0.0));
			double y = ((Point)(ref val)).Y;
			int num = (int)((double)Cursor.Position.Y - ((Window)this).Top - y);
			float num2 = 10f - (float)((double)num / ((FrameworkElement)Hue_slider).Height) * 10f;
			((RangeBase)Hue_slider).Value = num2;
		}
		else if (cp_active_slider == ((FrameworkElement)Hue_slider).Name)
		{
			cp_dragging = false;
		}
	}

	private void Colorpicker_MouseUp(object sender, MouseEventArgs e)
	{
		cp_dragging = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/FoxLED;component/mainwindow.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected O, but got Unknown
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Expected O, but got Unknown
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Expected O, but got Unknown
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Expected O, but got Unknown
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Expected O, but got Unknown
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Expected O, but got Unknown
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Expected O, but got Unknown
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Expected O, but got Unknown
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Expected O, but got Unknown
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Expected O, but got Unknown
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Expected O, but got Unknown
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Expected O, but got Unknown
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Expected O, but got Unknown
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Expected O, but got Unknown
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Expected O, but got Unknown
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad7: Expected O, but got Unknown
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			MainAppWindow = (MainWindow)target;
			break;
		case 2:
			PSL_ROW = (RowDefinition)target;
			break;
		case 3:
			header = (Grid)target;
			break;
		case 4:
			AppLogo = (Grid)target;
			break;
		case 5:
			AppTitle = (Label)target;
			break;
		case 6:
			minim = (Button)target;
			break;
		case 7:
			minim_icon = (Canvas)target;
			break;
		case 8:
			minim_path = (Path)target;
			break;
		case 9:
			close = (Button)target;
			break;
		case 10:
			close_icon = (Canvas)target;
			break;
		case 11:
			close_path = (Path)target;
			break;
		case 12:
			hp = (Button)target;
			break;
		case 13:
			hp_icon = (Canvas)target;
			break;
		case 14:
			hp_accent_path = (Path)target;
			break;
		case 15:
			hp_single_path = (Path)target;
			break;
		case 16:
			st = (Button)target;
			break;
		case 17:
			st_icon = (Canvas)target;
			break;
		case 18:
			st_accent_path = (Path)target;
			break;
		case 19:
			st_single_path = (Path)target;
			break;
		case 20:
			custom = (Button)target;
			break;
		case 21:
			custom_icon = (Canvas)target;
			break;
		case 22:
			custom_accent_path = (Path)target;
			break;
		case 23:
			custom_single_path = (Path)target;
			break;
		case 24:
			inf = (Button)target;
			break;
		case 25:
			inf_icon = (Canvas)target;
			break;
		case 26:
			inf_accent_path = (Path)target;
			break;
		case 27:
			inf_single_path = (Path)target;
			break;
		case 28:
			content = (TabControl)target;
			break;
		case 29:
			hpTab = (TabItem)target;
			break;
		case 30:
			modes = (StackPanel)target;
			break;
		case 31:
			def_fx_buttons = (StackPanel)target;
			break;
		case 32:
			def_fx_buttons_1 = (StackPanel)target;
			break;
		case 33:
			static_foxled = (Button)target;
			((UIElement)static_foxled).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_foxled).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_foxled).Click += new RoutedEventHandler(ModeClick);
			break;
		case 34:
			static_black = (Button)target;
			((UIElement)static_black).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_black).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_black).Click += new RoutedEventHandler(ModeClick);
			break;
		case 35:
			static_fx_buttons = (StackPanel)target;
			break;
		case 36:
			static_fx_buttons_1 = (StackPanel)target;
			break;
		case 37:
			static_red = (Button)target;
			((UIElement)static_red).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_red).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_red).Click += new RoutedEventHandler(ModeClick);
			break;
		case 38:
			static_green = (Button)target;
			((UIElement)static_green).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_green).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_green).Click += new RoutedEventHandler(ModeClick);
			break;
		case 39:
			static_blue = (Button)target;
			((UIElement)static_blue).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_blue).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_blue).Click += new RoutedEventHandler(ModeClick);
			break;
		case 40:
			static_cyan = (Button)target;
			((UIElement)static_cyan).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_cyan).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_cyan).Click += new RoutedEventHandler(ModeClick);
			break;
		case 41:
			static_fx_buttons_2 = (StackPanel)target;
			break;
		case 42:
			static_coral = (Button)target;
			((UIElement)static_coral).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_coral).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_coral).Click += new RoutedEventHandler(ModeClick);
			break;
		case 43:
			static_yellow = (Button)target;
			((UIElement)static_yellow).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_yellow).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_yellow).Click += new RoutedEventHandler(ModeClick);
			break;
		case 44:
			static_light_green = (Button)target;
			((UIElement)static_light_green).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_light_green).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_light_green).Click += new RoutedEventHandler(ModeClick);
			break;
		case 45:
			static_white = (Button)target;
			((UIElement)static_white).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)static_white).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)static_white).Click += new RoutedEventHandler(ModeClick);
			break;
		case 46:
			dynamic_fx_buttons = (StackPanel)target;
			break;
		case 47:
			dynamic_fx_buttons_1 = (StackPanel)target;
			break;
		case 48:
			dynamic_rainbow = (Button)target;
			((UIElement)dynamic_rainbow).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_rainbow).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_rainbow).Click += new RoutedEventHandler(ModeClick);
			break;
		case 49:
			dynamic_colormusic = (Button)target;
			((UIElement)dynamic_colormusic).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_colormusic).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_colormusic).Click += new RoutedEventHandler(ModeClick);
			break;
		case 50:
			dynamic_wallcolor = (Button)target;
			((UIElement)dynamic_wallcolor).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_wallcolor).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_wallcolor).Click += new RoutedEventHandler(ModeClick);
			break;
		case 51:
			dynamic_cpu_load = (Button)target;
			((UIElement)dynamic_cpu_load).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_cpu_load).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_cpu_load).Click += new RoutedEventHandler(ModeClick);
			break;
		case 52:
			dynamic_fx_buttons_2 = (StackPanel)target;
			break;
		case 53:
			dynamic_cpu_temp = (Button)target;
			((UIElement)dynamic_cpu_temp).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_cpu_temp).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_cpu_temp).Click += new RoutedEventHandler(ModeClick);
			break;
		case 54:
			dynamic_screen_capture = (Button)target;
			((UIElement)dynamic_screen_capture).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_screen_capture).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_screen_capture).Click += new RoutedEventHandler(ModeClick);
			break;
		case 55:
			dynamic_syscolor = (Button)target;
			((UIElement)dynamic_syscolor).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_syscolor).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_syscolor).Click += new RoutedEventHandler(ModeClick);
			break;
		case 56:
			dynamic_colors = (Button)target;
			((UIElement)dynamic_colors).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)dynamic_colors).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)dynamic_colors).Click += new RoutedEventHandler(ModeClick);
			break;
		case 57:
			stTab = (TabItem)target;
			break;
		case 58:
			speed_slider = (Slider)target;
			((UIElement)speed_slider).MouseMove += new MouseEventHandler(slider_MouseMove);
			((RangeBase)speed_slider).ValueChanged += Slider_ValueChanged;
			break;
		case 59:
			brightness_slider = (Slider)target;
			((UIElement)brightness_slider).MouseMove += new MouseEventHandler(slider_MouseMove);
			((RangeBase)brightness_slider).ValueChanged += brightness_slider_ValueChanged;
			break;
		case 60:
			screens_list = (ComboBox)target;
			break;
		case 61:
			leds_list = (ComboBox)target;
			break;
		case 62:
			remote_button = (Button)target;
			((UIElement)remote_button).MouseEnter += new MouseEventHandler(ModeHoverEnter);
			((UIElement)remote_button).MouseLeave += new MouseEventHandler(ModeHoverLeave);
			((ButtonBase)remote_button).Click += new RoutedEventHandler(RemoteClick);
			break;
		case 63:
			TokenPanel = (StackPanel)target;
			break;
		case 64:
			Token_Textbox = (TextBox)target;
			break;
		case 65:
			RestartPanel = (StackPanel)target;
			break;
		case 66:
			custTab = (TabItem)target;
			break;
		case 67:
			Colorpicker = (Grid)target;
			((UIElement)Colorpicker).MouseMove += new MouseEventHandler(Colorpicker_MouseMove);
			((UIElement)Colorpicker).MouseUp += new MouseButtonEventHandler(Colorpicker_MouseUp);
			break;
		case 68:
			hue_box = (Rectangle)target;
			break;
		case 69:
			cp_arrow = (Viewbox)target;
			break;
		case 70:
			Hue_slider = (Slider)target;
			((UIElement)Hue_slider).MouseUp += new MouseButtonEventHandler(Colorpicker_MouseUp);
			((UIElement)Hue_slider).MouseMove += new MouseEventHandler(Hue_slider_MouseMove);
			((RangeBase)Hue_slider).ValueChanged += Hue_slider_ValueChanged;
			break;
		case 71:
			result_color = (Rectangle)target;
			break;
		case 72:
			infTab = (TabItem)target;
			break;
		case 73:
			big_logo = (Canvas)target;
			break;
		case 74:
			PseudoLED = (Rectangle)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
