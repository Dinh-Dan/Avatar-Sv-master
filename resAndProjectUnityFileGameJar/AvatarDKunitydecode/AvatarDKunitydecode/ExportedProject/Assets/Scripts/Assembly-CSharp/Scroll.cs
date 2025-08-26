using System;

public class Scroll
{
	private static Scroll instance;

	public int limit;

	public int temp;

	public int dis;

	public int yScroll;

	public int hScroll = 100;

	public int h;

	public static bool Disvisible;

	public static bool isAble;

	private FrameImage img;

	public Scroll()
	{
		if (AvMain.hd == 2)
		{
			img = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/scrollList"), 7, 20);
		}
		else
		{
			img = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/scrollList"), 4, 10);
		}
	}

	public static Scroll gI()
	{
		if (instance == null)
		{
			instance = new Scroll();
		}
		return instance;
	}

	public void init(int dis, int size, int cmy)
	{
		if (size == 0)
		{
			size = 1;
		}
		if (limit == 0)
		{
			limit = 1;
		}
		try
		{
			this.dis = dis;
			hScroll = 100;
			limit = size - dis;
			if (size > dis)
			{
				hScroll = dis * 100 / size;
				if (hScroll < 2)
				{
					hScroll = 2;
				}
				h = cmy * 100 / limit;
				int num = dis - dis * hScroll / 100;
				yScroll = h * num / 100;
			}
			Disvisible = limit + dis < dis;
			isAble = true;
		}
		catch (Exception)
		{
		}
	}

	public void updateScroll(int cmy, int cmtoY, int v)
	{
		if (!Disvisible && limit != 0)
		{
			if (cmtoY < 0)
			{
				cmtoY = 0;
			}
			h = cmy * 100 / limit;
			int num = dis * hScroll / 100;
			if (num < 22 * AvMain.hd)
			{
				num = 22 * AvMain.hd;
			}
			int num2 = dis - num;
			yScroll = h * num2 / 100;
			if (yScroll > dis - 3)
			{
				yScroll = dis - 3;
			}
			if (yScroll < 0)
			{
				yScroll = 0;
			}
			if (yScroll + num > dis)
			{
				yScroll = dis - num;
			}
			if (cmy != cmtoY || Canvas.isPointerDown || v != 0)
			{
				isAble = true;
			}
			else
			{
				isAble = false;
			}
		}
	}

	public void paintScroll(MyGraphics g, int x, int y)
	{
		if (!Disvisible && isAble)
		{
			int num = dis * hScroll / 100;
			if (num < 22 * AvMain.hd)
			{
				num = 22 * AvMain.hd;
			}
			g.setClip(x, y + yScroll, 7f, num);
			img.drawFrame(0, x, y + 1 + yScroll, 0, g);
			img.drawFrame(2, x, y + 1 + yScroll + num - 2 - 10 * AvMain.hd, 0, g);
			int num2 = (num - 20 * AvMain.hd) / (10 * AvMain.hd);
			for (int i = 0; i < num2; i++)
			{
				img.drawFrame(1, x, y + 1 + yScroll + 10 * AvMain.hd + 10 * AvMain.hd * i, 0, g);
			}
			int num3 = num - 2 - 10 * AvMain.hd - (10 * AvMain.hd + num2 * 10 * AvMain.hd);
			g.drawRegion(img.imgFrame, 0f, 10 * AvMain.hd, 7, num3, 0, x, y + 1 + yScroll + 10 * AvMain.hd + num2 * 10 * AvMain.hd, 0);
		}
	}
}
