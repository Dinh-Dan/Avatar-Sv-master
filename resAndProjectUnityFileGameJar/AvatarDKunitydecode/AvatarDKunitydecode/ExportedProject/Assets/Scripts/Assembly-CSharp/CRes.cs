using System;
using UnityEngine;

public class CRes
{
	public static MyRandom r = new MyRandom();

	private static short[] sinn = new short[91]
	{
		0, 18, 36, 54, 71, 89, 107, 125, 143, 160,
		178, 195, 213, 230, 248, 265, 282, 299, 316, 333,
		350, 367, 384, 400, 416, 433, 449, 465, 481, 496,
		512, 527, 543, 558, 573, 587, 602, 616, 630, 644,
		658, 672, 685, 698, 711, 724, 737, 749, 761, 773,
		784, 796, 807, 818, 828, 839, 849, 859, 868, 878,
		887, 896, 904, 912, 920, 928, 935, 943, 949, 956,
		962, 968, 974, 979, 984, 989, 994, 998, 1002, 1005,
		1008, 1011, 1014, 1016, 1018, 1020, 1022, 1023, 1023, 1024,
		1024
	};

	private static short[] coss;

	private static int[] tann;

	public static void init()
	{
		coss = new short[91];
		tann = new int[91];
		for (int i = 0; i <= 90; i++)
		{
			coss[i] = sinn[90 - i];
			if (coss[i] == 0)
			{
				tann[i] = 32767;
			}
			else
			{
				tann[i] = (sinn[i] << 10) / coss[i];
			}
		}
	}

	public static int sin(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return sinn[a];
		}
		if (a >= 90 && a < 180)
		{
			return sinn[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return -sinn[a - 180];
		}
		return -sinn[360 - a];
	}

	public static int cos(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return coss[a];
		}
		if (a >= 90 && a < 180)
		{
			return -coss[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return -coss[a - 180];
		}
		return coss[360 - a];
	}

	public static int tan(int a)
	{
		a = fixangle(a);
		if (a >= 0 && a < 90)
		{
			return tann[a];
		}
		if (a >= 90 && a < 180)
		{
			return -tann[180 - a];
		}
		if (a >= 180 && a < 270)
		{
			return tann[a - 180];
		}
		return -tann[360 - a];
	}

	public static int atan(int a)
	{
		for (int i = 0; i <= 90; i++)
		{
			if (tann[i] >= a)
			{
				return i;
			}
		}
		return 0;
	}

	public static int angle(int dx, int dy)
	{
		int num;
		if (dx != 0)
		{
			int a = Math.abs((dy << 10) / dx);
			num = atan(a);
			if (dy >= 0 && dx < 0)
			{
				num = 180 - num;
			}
			if (dy < 0 && dx < 0)
			{
				num = 180 + num;
			}
			if (dy < 0 && dx >= 0)
			{
				num = 360 - num;
			}
		}
		else
		{
			num = ((dy <= 0) ? 270 : 90);
		}
		return num;
	}

	public static int myAngle(int dx, int dy)
	{
		int num = angle(dx, dy);
		if (num >= 315)
		{
			num = 360 - num;
		}
		return num;
	}

	public static int fixangle(int angle)
	{
		if (angle >= 360)
		{
			angle -= 360;
		}
		if (angle < 0)
		{
			angle += 360;
		}
		return angle;
	}

	public static int subangle(int a1, int a2)
	{
		int num = a2 - a1;
		if (num < -180)
		{
			return num + 360;
		}
		if (num > 180)
		{
			return num - 360;
		}
		return num;
	}

	public static int random(int a)
	{
		return r.nextInt() % a;
	}

	public static int rnd(int a)
	{
		return r.nextInt(a);
	}

	public static int rnd(int a, int b)
	{
		if (r.nextInt(2) == 0)
		{
			return a;
		}
		return b;
	}

	public static int abs(int a)
	{
		return (a < 0) ? (-a) : a;
	}

	public static bool isHit(int x, int y, int w, int h, int tX, int tY, int tW, int tH)
	{
		return x + w >= tX && x <= tX + tW && y + h >= tY && y <= tY + tH;
	}

	public static int sqrt(int a)
	{
		if (a <= 0)
		{
			return 0;
		}
		int num = (a + 1) / 2;
		int num2;
		do
		{
			num2 = num;
			num = num / 2 + a / (2 * num);
		}
		while (Math.abs(num2 - num) > 1);
		return num;
	}

	public static int distance(int x1, int y1, int x2, int y2)
	{
		return sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
	}

	public static int byte2int(sbyte b)
	{
		return b & 0xFF;
	}

	public static int getShort(int off, sbyte[] data)
	{
		return (byte2int(data[off]) << 8) | byte2int(data[off + 1]);
	}

	public static void saveRMSInt(string file, int x)
	{
		try
		{
			RMS.saveRMS(file, new sbyte[1] { (sbyte)x });
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public static short byte2short(sbyte[] data)
	{
		short num = data[1];
		return (short)((num << 8) | (byte)data[0]);
	}

	public static sbyte[] encoding(sbyte[] array)
	{
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (sbyte)(~array[i]);
			}
		}
		return array;
	}

	public static int loadRMSInt(string file)
	{
		sbyte[] array = RMS.loadRMS(file);
		return (array != null) ? array[0] : (-1);
	}

	public static Image createImgByByteArray(byte[] array)
	{
		return Image.createImage(array);
	}

	public static void rndaaa()
	{
		LoginScr.aa = 1;
		for (int i = 0; i < FarmScr.gI().listLeftMenu.size(); i++)
		{
			Command command = (Command)FarmScr.gI().listLeftMenu.elementAt(i);
			LoginScr.aa += command.caption.GetHashCode();
		}
	}

	public static Image createImgByHeader(sbyte[] header, sbyte[] data)
	{
		sbyte[] array = new sbyte[header.Length + data.Length];
		Array.Copy(header, 0, array, 0, header.Length);
		Array.Copy(data, 0, array, header.Length, data.Length);
		return createImgByByteArray(ArrayCast.cast(array));
	}

	public static Image createImgByImg(int x, int y, int w, int h, Image img)
	{
		return Image.createImage(img, x, y, w, h, 0);
	}

	public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)source.width * ((float)source.width / (float)targetWidth);
		float num2 = 1f / (float)source.height * ((float)source.height / (float)targetHeight);
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * ((float)i % (float)targetWidth), num2 * Mathf.Floor(i / targetWidth));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}
}
