namespace FoxLED;

public class LEDCache
{
	public int speed = 20;

	public string last_anim = "foxled";

	public float brightness = 1f;

	public int updateID;

	public int user_id;

	public int monitor_index;

	public int led_num = 30;

	public double[] custom_color_hsb = new double[3] { 0.0, 1.0, 0.5 };
}
