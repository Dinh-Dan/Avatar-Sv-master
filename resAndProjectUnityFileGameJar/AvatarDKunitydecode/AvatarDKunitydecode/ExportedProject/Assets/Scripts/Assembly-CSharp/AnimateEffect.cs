public class AnimateEffect : Effect
{
	private class Layer1 : Layer
	{
		private AnimateEffect parent;

		private readonly Point po;

		private readonly int x0;

		private readonly int y0;

		public Layer1(AnimateEffect parent, Point po, int x0, int y0)
		{
			this.parent = parent;
			this.po = po;
			this.x0 = x0;
			this.y0 = y0;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			img.drawFrame(po.g, x0, y0, 0, 3, g);
		}

		public override void update()
		{
			po.limitY--;
			if (po.limitY <= 0)
			{
				LoadMap.dynamicLists.removeElement(po);
			}
		}
	}

	public static FrameImage img;

	public static FrameImage imgFlower;

	public static FrameImage imgSnow;

	public static FrameImage imgTemp;

	private const sbyte RAIN = 0;

	private const sbyte FALLING_LEAVES = 1;

	private const sbyte FALLING_FLOWER = 2;

	private const sbyte SNOW = 3;

	private sbyte type;

	public int number;

	public int timeStop;

	public int timeCur;

	private static int wind = 5;

	private static int countWind;

	private static int dirWind = CRes.rnd(1, -1);

	private MyVector list = new MyVector();

	public AnimateEffect(sbyte type, bool isStart, int num)
	{
		this.type = type;
		number = num * 10;
		if (AvMain.hd == 1)
		{
			number = num * 5;
		}
		timeCur = (int)(Canvas.getTick() / 1000);
		switch (type)
		{
		case 0:
			number = Canvas.w * Canvas.h / 1000 + 50;
			break;
		case 1:
			number = 30;
			if (img == null)
			{
				img = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/cobay"), 16 * AvMain.hd, 10 * AvMain.hd);
			}
			break;
		case 3:
			number = Canvas.w * Canvas.h / 1000;
			img = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/tuyet"), 5 * AvMain.hd, 5 * AvMain.hd);
			imgTemp = img;
			break;
		}
		isStart = false;
		for (int i = 0; i < number; i++)
		{
			Point point = null;
			point = new Point((int)((AvCamera.gI().xCam - (float)Canvas.hw + (float)CRes.rnd(Canvas.w * 2)) * 10f), (int)((AvCamera.gI().yCam - (float)(Canvas.h * 2) + (float)CRes.rnd(Canvas.h * 2)) * 10f))
			{
				x = (-Canvas.w / 2 + CRes.rnd(LoadMap.wMap * LoadMap.w + Canvas.w)) * 10
			};
			if (type == 3 || this.type == 2)
			{
				point.h = CRes.rnd(3);
			}
			else
			{
				point.h = CRes.rnd(4);
			}
			point.limitY = 16 + CRes.rnd(3) * 4;
			point.v = CRes.rnd(-1, 1);
			point.color = CRes.rnd(point.limitY);
			point.dis = (sbyte)CRes.rnd(20);
			list.addElement(point);
		}
		if (type != 2)
		{
			return;
		}
		for (int j = 0; j < list.size() - 1; j++)
		{
			Point point2 = (Point)list.elementAt(j);
			for (int k = j + 1; k < list.size(); k++)
			{
				Point point3 = (Point)list.elementAt(k);
				if (point2.h > point3.h)
				{
					list.setElementAt(point2, k);
					list.setElementAt(point3, j);
					point2 = point3;
				}
			}
		}
	}

	public override void close()
	{
		base.close();
	}

	public void stop()
	{
		isStop = true;
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		switch (type)
		{
		case 0:
			paintRain(g);
			break;
		case 1:
			paintFallingLeaves(g);
			break;
		case 2:
		{
			int num = 0;
			if (IDAction == -1)
			{
				break;
			}
			EffectData effect = AvatarData.getEffect(IDAction);
			for (int j = 0; j < number; j++)
			{
				Point point2 = (Point)list.elementAt(j);
				point2.countFr++;
				if (!((float)(point2.x * AvMain.hd / 10) > AvCamera.gI().xCam) || !((float)(point2.x * AvMain.hd / 10) < AvCamera.gI().xCam + (float)Canvas.w) || !((float)(point2.y * AvMain.hd / 10) > AvCamera.gI().yCam) || !((float)(point2.y * AvMain.hd / 10) < AvCamera.gI().yCam + (float)Canvas.h))
				{
					continue;
				}
				if (effect != null)
				{
					if (point2.countFr >= effect.arrFrame.Length)
					{
						point2.countFr = 0;
					}
					effect.paint(g, point2.x / 10, point2.y / 10, point2.countFr);
				}
				point2.dis++;
				if (point2.dis >= 20)
				{
					point2.dis = 0;
				}
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i < number; i++)
			{
				Point point = (Point)list.elementAt(i);
				if ((float)(point.x * AvMain.hd / 10) > AvCamera.gI().xCam && (float)(point.x * AvMain.hd / 10) < AvCamera.gI().xCam + (float)Canvas.w && (float)(point.y * AvMain.hd / 10) > AvCamera.gI().yCam && (float)(point.y * AvMain.hd / 10) < AvCamera.gI().yCam + (float)Canvas.h)
				{
					imgTemp.drawFrame(2 - point.h, point.x * AvMain.hd / 10, point.y * AvMain.hd / 10, 0, g);
				}
			}
			break;
		}
		}
	}

	public override void paintBack(MyGraphics g)
	{
	}

	private void paintFallingLeaves(MyGraphics g)
	{
		for (int i = 0; i < number; i++)
		{
			Point point = (Point)list.elementAt(i);
			if ((float)(point.x * AvMain.hd / 10) > AvCamera.gI().xCam && (float)(point.x * AvMain.hd / 10) < AvCamera.gI().xCam + (float)Canvas.w && (float)(point.y * AvMain.hd / 10) > AvCamera.gI().yCam && (float)(point.y * AvMain.hd / 10) < AvCamera.gI().yCam + (float)Canvas.h)
			{
				img.drawFrame(point.color / (point.limitY / 4), point.x * AvMain.hd / 10, point.y * AvMain.hd / 10, 0, 3, g);
			}
		}
	}

	private void paintRain(MyGraphics g)
	{
		g.setColor(14540253);
		for (int i = 0; i < number; i++)
		{
			Point point = (Point)list.elementAt(i);
			if ((float)(point.x * AvMain.hd / 10) > AvCamera.gI().xCam && (float)(point.x * AvMain.hd / 10) < AvCamera.gI().xCam + (float)Canvas.w && (float)(point.y * AvMain.hd / 10) > AvCamera.gI().yCam && (float)(point.y * AvMain.hd / 10) < AvCamera.gI().yCam + (float)Canvas.h)
			{
				g.fillRect(point.x * AvMain.hd / 10, point.y * AvMain.hd / 10, 1f, point.h + 1);
			}
		}
	}

	public override void update()
	{
		updateWind();
		switch (type)
		{
		case 0:
			updateRain();
			break;
		case 1:
			updateFallingLeaves();
			break;
		case 2:
			updateFlower();
			break;
		case 3:
			updateSnow();
			break;
		}
	}

	public static void updateWind()
	{
		int num = 1;
		if (Canvas.gameTick % 6 == 3)
		{
			num = CRes.rnd(15);
		}
		if (num == 0 && wind == 5)
		{
			wind = 5 + CRes.rnd(20);
			countWind = 50 + CRes.rnd(100);
		}
		if (countWind > 0)
		{
			countWind--;
		}
		if (countWind == 0 && wind > 5 && Canvas.gameTick % 4 == 2)
		{
			wind--;
		}
	}

	private void updateRain()
	{
		for (int i = 0; i < number; i++)
		{
			Point point = (Point)list.elementAt(i);
			point.y += (point.h + 1) * 15 + (3 - point.h) * 3;
			point.g++;
			point.x += (point.h + 1) * 4;
			if ((float)(point.y / 10) > AvCamera.gI().yCam + (float)Canvas.h - (float)((4 - point.h) * 50))
			{
				rndPos(point);
			}
			setLimitX(point);
		}
	}

	private void setLimitX(Point pos)
	{
		int num = (int)(AvCamera.gI().xCam * (float)((2 - pos.h) * 20) / 120f);
		if ((float)(pos.x / 10 + num) < AvCamera.gI().xCam - 10f)
		{
			pos.x += (Canvas.w + 20) * 10;
		}
		if ((float)(pos.x / 10 + num) > AvCamera.gI().xCam + (float)Canvas.w + 10f)
		{
			pos.x -= (Canvas.w + 20) * 10;
		}
	}

	private void updateFlower()
	{
		if (Canvas.getTick() / 1000 - timeCur > timeStop)
		{
			timeStop++;
			for (int i = 0; i < 5; i++)
			{
				list.removeElementAt(0);
				number = list.size();
				if (number == 0)
				{
					close();
					return;
				}
			}
		}
		for (int j = 0; j < number; j++)
		{
			Point point = (Point)list.elementAt(j);
			point.y += (point.h + 2) * 5;
			point.x += (point.h + 1) * 2 + wind * dirWind;
			if (point.y / 10 > LoadMap.Hmap * LoadMap.w - (4 - point.h) * 20)
			{
				rndPos(point);
			}
		}
	}

	private void updateSnow()
	{
		for (int i = 0; i < number; i++)
		{
			Point point = (Point)list.elementAt(i);
			point.y += (point.h + 4) * 3;
			point.x += (point.h + 1) * 2 + wind * dirWind;
			if (point.y / 10 > LoadMap.Hmap * LoadMap.w - (4 - point.h) * 20)
			{
				rndPos(point);
			}
		}
	}

	private void updateFallingLeaves()
	{
		for (int i = 0; i < number; i++)
		{
			Point point = (Point)list.elementAt(i);
			point.y += 10;
			point.x += point.v * 10 + wind * dirWind;
			point.color++;
			if (point.color >= point.limitY)
			{
				point.color = 0;
			}
			if (point.y / 10 > LoadMap.Hmap * LoadMap.w - (4 - point.h) * 20)
			{
				rndPos(point);
			}
		}
	}

	private void setLeaveFall(int x0, int y0)
	{
		Point point = new Point(x0, y0);
		point.limitY = 200;
		point.g = CRes.rnd(4);
		point.layer = new Layer1(this, point, x0, y0);
		LoadMap.dynamicLists.addElement(point);
	}

	private void rndPos(Point pos)
	{
		if (isStop)
		{
			list.removeElement(pos);
			number = list.size();
			if (list.size() == 0)
			{
				close();
			}
		}
		else
		{
			pos.y = (int)(AvCamera.gI().yCam - (float)Canvas.hh + (float)CRes.rnd(Canvas.h * 2)) * 10;
			pos.x = (-Canvas.w / 2 + CRes.rnd(LoadMap.wMap * LoadMap.w + Canvas.w)) * 10;
		}
	}
}
