using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace FoxLED;

public class LEDRemote
{
	public Window window;

	private Timer timer = new Timer();

	private int user_id;

	public string active_state = "disabled";

	private string last_token = "";

	public LEDRemote()
	{
		MainWindow.LCACHE = MainWindow.LED.LoadCache(MainWindow.LCACHE);
		user_id = MainWindow.LCACHE.user_id;
		if (MainWindow.LCACHE.user_id != 0)
		{
			active_state = "enabled";
			try
			{
				new WebClient().DownloadString("https://anidark.ru/foxled/loginME?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + MainWindow.LCACHE.user_id);
			}
			catch
			{
				MainWindow.LED.consoleLog("Server login error");
			}
			MainWindow.LED.consoleLog("TG Auth: Successfull!");
			string keyboard = "{\"keyboard\": [[{\"text\": \"\ud83d\udd3b Уменьшить скорость анимации\"},{\"text\": \"\ud83d\udd3a Увеличить скорость анимации\"}], [{\"text\": \"\ud83c\udf18 Уменьшить яркость\"},{\"text\": \"\ud83c\udf16 Увеличить яркость\"}], [{\"text\": \"FoxLED\"},{\"text\": \"Чёрный\"}], [{\"text\": \"Красный\"},{\"text\": \"Зелёный\"}], [{\"text\": \"Синий\"},{\"text\": \"Голубой\"}], [{\"text\": \"Коралловый\"},{\"text\": \"Жёлтый\"}], [{\"text\": \"Салатовый\"},{\"text\": \"Белый\"}], [{\"text\": \"Радуга\"},{\"text\": \"Цветомузыка\"}], [{\"text\": \"Цвет обой\"},{\"text\": \"Загрузка процессора\"}], [{\"text\": \"Температура процессора\"},{\"text\": \"Захват экрана\"}], [{\"text\": \"Системный цвет\"},{\"text\": \"Анимация по спектру\"}] ]}";
			TGAnswer(MainWindow.LCACHE.user_id, "Приложение запущено.", keyboard);
		}
		timer.Interval = 1000.0;
		timer.Elapsed += CheckAct;
	}

	private void TGAnswer(int uid, string msg, string keyboard = "")
	{
		string text = "https://api.telegram.org/bot880918795:AAE-H5hRVwk4HFMyHOKp-iyR8yOU5fnaKeY/sendMessage?chat_id=" + uid + "&text=" + msg;
		if (keyboard != "")
		{
			text = text + "&reply_markup=" + keyboard;
		}
		WebRequest webRequest = WebRequest.Create(text);
		try
		{
			new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
		}
		catch
		{
			MainWindow.LED.consoleLog("TG Sent Failed");
		}
	}

	private string GetMd5Hash(MD5 md5Hash, string input)
	{
		byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public void onAppClose()
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				new WebClient().DownloadString("https://anidark.ru/foxled/logoutME?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + MainWindow.LCACHE.user_id);
				TGAnswer(MainWindow.LCACHE.user_id, "Приложение было закрыто.", "{\"hide_keyboard\": true}");
			}
			catch
			{
				MainWindow.LED.consoleLog("Server logout error");
			}
		});
	}

	public void logout()
	{
		StopDaemon();
		TGAnswer(MainWindow.LCACHE.user_id, "Выполнен выход", "{\"hide_keyboard\": true}");
		MainWindow.LCACHE = MainWindow.LED.LoadCache(MainWindow.LCACHE);
		MainWindow.LCACHE.user_id = 0;
		MainWindow.LED.StoreCache(MainWindow.LCACHE);
		active_state = "disabled";
		MainWindow.LED.consoleLog("TG Auth: Logout");
		StartDaemon();
		try
		{
			new WebClient().DownloadString("https://anidark.ru/foxled/logoutME?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + MainWindow.LCACHE.user_id);
		}
		catch
		{
			MainWindow.LED.consoleLog("Server error");
		}
	}

	public string generateToken()
	{
		active_state = "reg";
		MainWindow.LED.consoleLog("TG Auth: Token generation");
		string result = "";
		using (MD5 md5Hash = MD5.Create())
		{
			result = GetMd5Hash(md5Hash, Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
		}
		last_token = result;
		return result;
	}

	private void CheckAct(object sender, ElapsedEventArgs e)
	{
		if (!(active_state != "disabled"))
		{
			return;
		}
		if (active_state == "reg")
		{
			try
			{
				if (last_token != "")
				{
					MainWindow.LED.consoleLog("Token: " + last_token);
					WebClient webClient = new WebClient();
					webClient.DownloadStringCompleted += OnRegGetData;
					string responseFromServer = webClient.DownloadString("https://anidark.ru/foxled/getTask?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + last_token);
					RegProcessData(responseFromServer);
				}
				else
				{
					MainWindow.LED.consoleLog("Token is empty!");
				}
			}
			catch
			{
				MainWindow.LED.consoleLog("Server request error");
			}
		}
		if (active_state == "enabled")
		{
			try
			{
				WebClient webClient2 = new WebClient();
				webClient2.DownloadStringCompleted += OnEnGetData;
				string responseFromServer2 = webClient2.DownloadString("https://anidark.ru/foxled/getTask?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + MainWindow.LCACHE.user_id);
				EnProcessData(responseFromServer2);
			}
			catch
			{
				MainWindow.LED.consoleLog("Server request error");
			}
		}
	}

	private void EnProcessData(string responseFromServer)
	{
		if (!(responseFromServer != "") || !(responseFromServer != "[\"No data\"]"))
		{
			return;
		}
		JObject jObject = JObject.Parse(responseFromServer);
		string text = "";
		int num = MainWindow.LCACHE.speed;
		float num2 = MainWindow.LCACHE.brightness;
		int num3 = 0;
		int num4 = 0;
		int uid = MainWindow.LCACHE.user_id;
		foreach (JToken item in (IEnumerable<JToken>)jObject["tasks"])
		{
			string text2 = (string)item;
			num4 = 0;
			num3 = 0;
			bool num5 = text2.IndexOf("/") > -1;
			bool flag = false;
			if (num5)
			{
				string[] array = text2.Split(new char[1] { '/' })[1].Split(new char[1] { ' ' });
				string text3 = array[0];
				string text4 = "";
				text4 = ((array.Count() <= 1) ? "" : array[1]);
				switch (text3)
				{
				case "mode":
					text = text4;
					break;
				case "hmode":
					text = text4;
					flag = true;
					break;
				case "speed":
					if (text4 == "up")
					{
						num -= 5;
					}
					else if (text4 == "down")
					{
						num += 5;
					}
					if (num > 40)
					{
						num = 40;
						num3 = 1;
					}
					if (num < 0)
					{
						num = 0;
						num3 = 2;
					}
					break;
				case "bright":
					if (text4 == "up")
					{
						num2 += 0.2f;
					}
					else if (text4 == "down")
					{
						num2 -= 0.2f;
					}
					if (num2 > 1f)
					{
						num2 = 1f;
						num4 = 2;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
						num4 = 1;
					}
					break;
				default:
				{
					MainWindow.LED.consoleLog("TG Rejected Unknown Command");
					string keyboard = "{\"keyboard\": [[{\"text\": \"\ud83d\udd3b Уменьшить скорость анимации\"},{\"text\": \"\ud83d\udd3a Увеличить скорость анимации\"}], [{\"text\": \"\ud83c\udf18 Уменьшить яркость\"},{\"text\": \"\ud83c\udf16 Увеличить яркость\"}], [{\"text\": \"FoxLED\"},{\"text\": \"Чёрный\"}], [{\"text\": \"Красный\"},{\"text\": \"Зелёный\"}], [{\"text\": \"Синий\"},{\"text\": \"Голубой\"}], [{\"text\": \"Коралловый\"},{\"text\": \"Жёлтый\"}], [{\"text\": \"Салатовый\"},{\"text\": \"Белый\"}], [{\"text\": \"Радуга\"},{\"text\": \"Цветомузыка\"}], [{\"text\": \"Цвет обой\"},{\"text\": \"Загрузка процессора\"}], [{\"text\": \"Температура процессора\"},{\"text\": \"Захват экрана\"}], [{\"text\": \"Системный цвет\"},{\"text\": \"Анимация по спектру\"}] ]}";
					TGAnswer(uid, "Команда не найдена.", keyboard);
					break;
				}
				}
			}
			if (text != "")
			{
				MainWindow.LED.consoleLog("TG Received Command: /mode " + text);
				if (!flag)
				{
					TGAnswer(uid, "Выполняю...");
				}
				MainWindow.LEDAnim.stop();
				MainWindow.LEDAnim.start(text);
				continue;
			}
			if (num != MainWindow.LCACHE.speed)
			{
				MainWindow.LCACHE.speed = num;
				MainWindow.LED.StoreCache(MainWindow.LCACHE);
				MainWindow.LEDAnim.stop();
				MainWindow.LEDAnim.start(MainWindow.LCACHE.last_anim);
				MainWindow.LED.consoleLog("TG Received Command: speed set. New speed - " + num);
				TGAnswer(uid, "\ud83d\ude80 Текущая скорость - " + (double)(40 - MainWindow.LCACHE.speed) / 0.4 + "%");
			}
			if (num2 != MainWindow.LCACHE.brightness)
			{
				MainWindow.LCACHE.brightness = num2;
				MainWindow.LED.StoreCache(MainWindow.LCACHE);
				MainWindow.LED.consoleLog("TG Received Command: bright set. New bright - " + num2);
				TGAnswer(uid, "\ud83d\udd06 Текущая яркость - " + num2 * 100f + "%");
			}
			if (num4 == 2)
			{
				MainWindow.LED.consoleLog("TG Received Command: bright not set - >max");
				TGAnswer(uid, "\ud83d\udd06 Текущая яркость - " + num2 * 100f + "%");
				TGAnswer(uid, "Достигнута максимальная яркость.");
			}
			if (num4 == 1)
			{
				MainWindow.LED.consoleLog("TG Received Command: bright not set - <min");
				TGAnswer(uid, "\ud83d\udd06 Текущая яркость - " + num2 * 100f + "%");
				TGAnswer(uid, "Достигнута минимальная яркость.");
			}
			if (num3 == 2)
			{
				MainWindow.LED.consoleLog("TG Received Command: speed not set - >max");
				TGAnswer(uid, "\ud83d\ude80 Текущая скорость - " + (double)(40 - MainWindow.LCACHE.speed) / 0.4 + "%");
				TGAnswer(uid, "Достигнута максимальная скорость анимации.");
			}
			if (num3 == 1)
			{
				MainWindow.LED.consoleLog("TG Received Command: speed not set - <min");
				TGAnswer(uid, "\ud83d\ude80 Текущая скорость - " + (double)(40 - MainWindow.LCACHE.speed) / 0.4 + "%");
				TGAnswer(uid, "Достигнута минимальная скорость анимации.");
			}
		}
	}

	private void RegProcessData(string responseFromServer)
	{
		if (!(responseFromServer != "") || !(responseFromServer != "[\"No data\"]"))
		{
			return;
		}
		foreach (JToken item in (IEnumerable<JToken>)JObject.Parse(responseFromServer)["tasks"])
		{
			string text = (string)item;
			if (text.IndexOf("/") <= -1)
			{
				continue;
			}
			string[] array = text.Split(new char[1] { '/' })[1].Split(new char[1] { ' ' });
			string obj = array[0];
			string text2 = "";
			if (array.Count() > 1)
			{
				text2 = array[1];
			}
			if (obj == "auth" && text2 != "")
			{
				try
				{
					MainWindow.LCACHE.user_id = Convert.ToInt32(text2);
					MainWindow.LED.StoreCache(MainWindow.LCACHE);
					MainWindow.LCACHE = MainWindow.LED.LoadCache(MainWindow.LCACHE);
					MainWindow.LED.consoleLog("TG Auth: Successfull!");
					string keyboard = "{\"keyboard\": [[{\"text\": \"\ud83d\udd3b Уменьшить скорость анимации\"},{\"text\": \"\ud83d\udd3a Увеличить скорость анимации\"}], [{\"text\": \"\ud83c\udf18 Уменьшить яркость\"},{\"text\": \"\ud83c\udf16 Увеличить яркость\"}], [{\"text\": \"FoxLED\"},{\"text\": \"Чёрный\"}], [{\"text\": \"Красный\"},{\"text\": \"Зелёный\"}], [{\"text\": \"Синий\"},{\"text\": \"Голубой\"}], [{\"text\": \"Коралловый\"},{\"text\": \"Жёлтый\"}], [{\"text\": \"Салатовый\"},{\"text\": \"Белый\"}], [{\"text\": \"Радуга\"},{\"text\": \"Цветомузыка\"}], [{\"text\": \"Цвет обой\"},{\"text\": \"Загрузка процессора\"}], [{\"text\": \"Температура процессора\"},{\"text\": \"Захват экрана\"}], [{\"text\": \"Системный цвет\"},{\"text\": \"Анимация по спектру\"}] ]}";
					TGAnswer(MainWindow.LCACHE.user_id, "Авторизация успешна!", keyboard);
					active_state = "enabled";
				}
				catch
				{
					MainWindow.LED.consoleLog("TG Auth: Invalid uid");
				}
				try
				{
					new WebClient().DownloadString("https://anidark.ru/foxled/loginME?secret=3ogk35kg9035g93509bjrib04964u904u9rq&uid=" + MainWindow.LCACHE.user_id);
				}
				catch
				{
					MainWindow.LED.consoleLog("Server login error");
				}
			}
		}
	}

	private void OnEnGetData(object sender, DownloadStringCompletedEventArgs e)
	{
		_ = e.Result;
		MainWindow.LED.consoleLog(e.Result);
	}

	private void OnRegGetData(object sender, DownloadStringCompletedEventArgs e)
	{
		_ = e.Result;
		MainWindow.LED.consoleLog(e.Result);
	}

	public void StartDaemon()
	{
		timer.Start();
		timer.Enabled = true;
	}

	public void StopDaemon()
	{
		timer.Stop();
		timer.Enabled = false;
	}
}
