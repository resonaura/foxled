namespace FoxLED;

public class LEDConstants
{
	public static byte LED_COUNT = (byte)new LEDConnect().LoadCache(new LEDCache()).led_num;

	public static byte BYTES_PER_LED = 3;

	public static byte LED_ARRAY_SIZE = (byte)(LED_COUNT * BYTES_PER_LED);

	public const int TICK_EVERY_MILISEC = 10;
}
