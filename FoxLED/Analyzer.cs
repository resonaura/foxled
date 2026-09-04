using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

namespace FoxLED;

public class Analyzer
{
	public WasapiLoopbackCapture CaptureInstance = new WasapiLoopbackCapture();

	private static System.Timers.Timer timer1 = new System.Timers.Timer();

	private Task ts;

	private static CancellationTokenSource tokenSource2 = new CancellationTokenSource();

	private CancellationToken ct = tokenSource2.Token;

	private float[] WaveBuffer;

	private static MMDeviceEnumerator mDeviceEnumerator = new MMDeviceEnumerator();

	private static MMDevice mDevice;

	private float[] FFT;

	private int[][] rc_colors = new int[26][]
	{
		new int[3] { 211, 47, 47 },
		new int[3] { 194, 24, 91 },
		new int[3] { 123, 31, 162 },
		new int[3] { 81, 45, 168 },
		new int[3] { 48, 63, 159 },
		new int[3] { 25, 118, 210 },
		new int[3] { 2, 136, 209 },
		new int[3] { 0, 151, 167 },
		new int[3] { 0, 121, 107 },
		new int[3] { 56, 142, 60 },
		new int[3] { 104, 159, 56 },
		new int[3] { 175, 180, 43 },
		new int[3] { 251, 192, 45 },
		new int[3] { 255, 160, 0 },
		new int[3] { 245, 124, 0 },
		new int[3] { 230, 74, 25 },
		new int[3] { 93, 64, 55 },
		new int[3] { 69, 90, 100 },
		new int[3] { 255, 255, 255 },
		new int[3] { 255, 0, 0 },
		new int[3] { 255, 255, 0 },
		new int[3] { 0, 255, 0 },
		new int[3] { 0, 255, 255 },
		new int[3] { 0, 0, 255 },
		new int[3] { 255, 0, 255 },
		new int[3] { 255, 0, 0 }
	};

	private int[][] rc;

	private int xstep;

	private int dec;

	public void Start()
	{
		CaptureInstance = new WasapiLoopbackCapture();
		tokenSource2 = new CancellationTokenSource();
		ct = tokenSource2.Token;
		ts = Task.Factory.StartNew(delegate
		{
			if (CaptureInstance.CaptureState == CaptureState.Stopped)
			{
				CaptureInstance.StartRecording();
				CaptureInstance.DataAvailable += onDataAviable;
			}
		}, tokenSource2.Token);
	}

	public void Stop()
	{
		CaptureInstance.StopRecording();
		tokenSource2.Cancel();
	}

	public static float[] UpdateFFTbuffer(float[] wbuffer)
	{
		Complex[] array = new Complex[wbuffer.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].X = (float)((double)wbuffer[i] * FastFourierTransform.BlackmannHarrisWindow(i, wbuffer.Length));
			array[i].Y = 0f;
		}
		FastFourierTransform.FFT(forward: true, (int)Math.Log(array.Length, 2.0), array);
		float[] array2 = new float[array.Length / 2 - 1];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = (float)Math.Sqrt(array[j].X * array[j].X) + array[j].Y * array[j].Y;
		}
		return array2;
	}

	private float[] AbsArr(float[] arr)
	{
		float[] array = new float[arr.Length];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = Math.Abs(arr[i]);
		}
		return array;
	}

	public float[] NoiseReduction(float[] src, int severity = 1)
	{
		for (int i = 1; i < src.Length; i++)
		{
			int num = ((i - severity > 0) ? (i - severity) : 0);
			int num2 = ((i + severity < src.Length) ? (i + severity) : src.Length);
			float num3 = 0f;
			for (int j = num; j < num2; j++)
			{
				num3 += src[j];
			}
			float num4 = num3 / (float)(num2 - num);
			src[i] = num4;
		}
		return src;
	}

	private void onDataAviable(object sender, WaveInEventArgs a)
	{
		byte[] buffer = a.Buffer;
		WaveBuffer = new float[buffer.Length / 4];
		for (int i = 0; i < buffer.Length / 4; i++)
		{
			WaveBuffer[i] = BitConverter.ToSingle(buffer, i * 4);
		}
		FFT = NoiseReduction(AbsArr(UpdateFFTbuffer(WaveBuffer)));
		int num = 200 / LEDConstants.LED_COUNT;
		float[] array = new float[LEDConstants.LED_COUNT];
		if (mDevice == null)
		{
			mDevice = mDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
		}
		int num2 = (int)Math.Round(mDevice.AudioMeterInformation.MasterPeakValue * 100f);
		if (num2 == 0)
		{
			dec += 2;
		}
		else
		{
			dec = 0;
		}
		for (int j = 0; j < LEDConstants.LED_COUNT; j++)
		{
			float num3 = FFT[num * j] * 48000f;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (num2 == 0)
			{
				num3 -= (float)dec;
			}
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (num3 > 255f)
			{
				num3 = 255f;
			}
			array[j] = num3;
		}
		array = NoiseReduction(array);
		if (rc == null)
		{
			rc = MainWindow.LED.GenerateGradientMap(rc_colors, 3000);
		}
		if (xstep >= rc.Length)
		{
			xstep = 0;
		}
		xstep++;
		int[] array2 = rc[xstep - 1];
		for (int k = 0; k < array.Length; k++)
		{
			int num4 = (int)((float)(int)array[k] / 255f * 100f);
			float num5 = (float)array2[0] * (float)num4 / 100f;
			float num6 = (float)array2[1] * (float)num4 / 100f;
			float num7 = (float)array2[2] * (float)num4 / 100f;
			int[] color = new int[3]
			{
				(int)num5,
				(int)num6,
				(int)num7
			};
			MainWindow.LED.SetLED(k + 1, color);
		}
		int num8 = LEDConstants.LED_COUNT / 2 - LEDConstants.LED_COUNT / 2 * num2 / 100;
		for (int l = 0; l < num8 / 2; l++)
		{
		}
		MainWindow.LED.UpdateLED();
	}
}
