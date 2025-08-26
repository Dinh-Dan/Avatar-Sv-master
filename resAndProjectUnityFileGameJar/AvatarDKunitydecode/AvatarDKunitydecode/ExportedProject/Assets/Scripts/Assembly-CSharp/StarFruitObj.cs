public class StarFruitObj : SubObject
{
	public short lv;

	public short productID;

	public short fruitID;

	public short numberFruit;

	public short anTrom;

	public static int imgID;

	public int timeFinish;

	public int w0;

	public int h0;

	public long time;

	public sbyte[] xFruit;

	public sbyte[] yFruit;

	public StarFruitObj()
	{
		catagory = 8;
	}

	public void setFruit()
	{
		if (numberFruit > 0)
		{
			int num = CRes.rnd(3) + 3;
			xFruit = new sbyte[num];
			yFruit = new sbyte[num];
			for (int i = 0; i < num; i++)
			{
				xFruit[i] = (sbyte)(CRes.rnd(w0 - 10) - (w0 - 10) / 2);
				yFruit[i] = (sbyte)(CRes.rnd(h0 - 10) - (h0 - 10) / 2);
			}
		}
	}

	public override void update()
	{
		type = imgID;
		if (Canvas.getTick() - time < 1000)
		{
			return;
		}
		if (timeFinish > 0)
		{
			timeFinish--;
			if (timeFinish == 0)
			{
				FarmService.gI().doFinishStarFruit();
			}
		}
		time = Canvas.getTick();
		ImageIcon imgIcon = FarmData.getImgIcon((short)imgID);
		if (imgIcon.w > 0)
		{
			if (w == 0)
			{
				w = imgIcon.w;
				h = imgIcon.h;
			}
			if (w0 == 0)
			{
				w0 = imgIcon.w / 3 * 2;
				h0 = imgIcon.h / 2;
				setFruit();
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		if (type < 0 && ((float)(x * MyObject.hd + w / 2) < AvCamera.gI().xCam || (float)(x * MyObject.hd - w / 2) > AvCamera.gI().xCam + (float)Canvas.w))
		{
			return;
		}
		FarmData.paintImg(g, imgID, x * MyObject.hd, y * MyObject.hd, MyGraphics.HCENTER | MyGraphics.BOTTOM);
		if (numberFruit > 0 && xFruit != null)
		{
			for (int i = 0; i < xFruit.Length; i++)
			{
				FarmData.paintImg(g, fruitID, x * MyObject.hd + xFruit[i], y * MyObject.hd - (FarmData.getImgIcon((short)imgID).h / 2 + 5) + yFruit[i], 3);
			}
		}
		int num = 0;
		num = FarmData.getImgIcon((short)imgID).h + AvMain.hBorder;
		if (timeFinish > 0)
		{
			num += AvMain.hSmall;
		}
		FarmData.paintImg(g, fruitID, (x - 8) * MyObject.hd, y * MyObject.hd - num, 3);
		Canvas.borderFont.drawString(g, "Lv" + lv, x * MyObject.hd, y * MyObject.hd - num - AvMain.hBorder / 2, 0);
		if (timeFinish > 0)
		{
			int num2 = timeFinish / 3600;
			int num3 = (timeFinish - num2 * 3600) / 60;
			int num4 = timeFinish - num2 * 3600 - num3 * 60;
			Canvas.smallFontYellow.drawString(g, num2 + ":" + num3 + ":" + num4, (x + 3) * MyObject.hd, y * MyObject.hd - num + Canvas.borderFont.getHeight() / 2 + 2 * MyObject.hd, 2);
		}
	}
}
