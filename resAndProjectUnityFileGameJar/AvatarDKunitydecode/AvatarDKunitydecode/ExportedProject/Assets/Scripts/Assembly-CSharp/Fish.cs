public class Fish
{
	public static int CAN = 322;

	public static int CUOC = 0;

	public static int LUOI = 1;

	public static int MOI = 2;

	public static int dis = 10;

	public int direct = 1;

	public sbyte size = 9;

	public Avatar ava;

	public AvPosition[] posDay;

	public AvPosition[] posTemp;

	private AvPosition[] posGoc;

	private int indexQuan;

	public int distant = dis;

	private static int force = 0;

	private static int v = 10;

	public int g = -8;

	public int isQuan;

	public int countQuan = -1;

	private int radius = 25;

	private int rnd;

	public int idFish = -1;

	private int yTemp = -1;

	private int iRnd;

	public bool isCanCau;

	public bool isSuccess;

	public bool isWait;

	private bool isLac;

	private AvPosition[] waves;

	private AvPosition posMoi;

	private AvPosition posMoitran;

	private AvPosition posMoiTemp;

	public static int[] color = new int[2] { 12577266, 10341591 };

	private int aa;

	private int rLac;

	private int count;

	public Fish()
	{
		size = (sbyte)(7 + CRes.rnd(4));
		waves = new AvPosition[2];
		for (int i = 0; i < 2; i++)
		{
			waves[i] = new AvPosition(-10, 0, i * 15);
		}
		posGoc = new AvPosition[2];
		posGoc[0] = new AvPosition();
		posGoc[1] = new AvPosition();
		posDay = new AvPosition[size];
		posTemp = new AvPosition[size];
		for (int j = 0; j < size; j++)
		{
			posDay[j] = new AvPosition();
			posTemp[j] = new AvPosition();
		}
		posMoi = new AvPosition(0, 0, -1);
		posMoitran = new AvPosition(0, 0, -1);
		posMoiTemp = new AvPosition();
	}

	public void doSetDayCau()
	{
		indexQuan = 0;
		isQuan = 0;
		g = -(10 + CRes.rnd(4));
		countQuan = -1;
		isCanCau = false;
		isSuccess = false;
		yTemp = -1;
		isLac = false;
	}

	public void doQuanCau(Avatar ava)
	{
		this.ava = ava;
		if (ava.direct == Base.RIGHT)
		{
			direct = 1;
		}
		else
		{
			direct = -1;
		}
		doSetDayCau();
		doSetPos(ava);
		if (ava.IDDB == GameMidlet.avatar.IDDB)
		{
			MapScr.doAction(13);
		}
	}

	public void doSetPos(Avatar ava)
	{
		countQuan = 0;
		idFish = 0;
		Part part = AvatarData.getPartByZ(ava.seriPart, 70);
		if (part.follow >= 0)
		{
			part = AvatarData.getPart(part.follow);
		}
		APartInfo aPartInfo = (APartInfo)part;
		ImageInfo imageInfo = AvatarData.listImgInfo[aPartInfo.imgID[3]];
		ImageInfo imageInfo2 = AvatarData.listImgInfo[aPartInfo.imgID[14]];
		int x = ava.x;
		int num = ava.y + ava.ySat;
		posGoc[0].x = x + aPartInfo.dx[3] * AvMain.hd + imageInfo.w * AvMain.hd;
		posGoc[0].y = num + aPartInfo.dy[3] * AvMain.hd - 5 * (AvMain.hd - 1);
		posGoc[1].x = x + aPartInfo.dx[14] * AvMain.hd + imageInfo2.w * AvMain.hd;
		posGoc[1].y = num + aPartInfo.dy[14] * AvMain.hd - 5 * (AvMain.hd - 1);
		posMoi.anchor = -1;
	}

	public void doQuanDay()
	{
		indexQuan++;
		distant = dis;
		for (int i = 0; i < size; i++)
		{
			posDay[i].x = posGoc[1].x;
			posDay[i].y = posGoc[1].y;
		}
	}

	public void setPosDay(int index)
	{
		posDay[0].x = posGoc[index].x;
		posDay[0].y = posGoc[index].y;
		if (index == 1)
		{
			ava.action = 13;
		}
		else
		{
			ava.action = 2;
		}
	}

	public void update()
	{
		if (ava == null)
		{
			return;
		}
		count++;
		if (count >= 6)
		{
			count = 0;
		}
		updateDayCau();
		updateQuanCau();
		updateCanCau();
		if (isWait)
		{
			updateWait();
		}
		if (isQuan != 0)
		{
			updateWave();
		}
		if (!isSuccess)
		{
			updatePosMoi();
		}
		if (ava.direct == Base.RIGHT)
		{
			direct = 1;
		}
		else
		{
			direct = -1;
		}
		for (int i = 0; i < size; i++)
		{
			int num = posDay[i].x - ava.x;
			if (i != size - 2 || CRes.abs(posTemp[i].x - (ava.x + direct * num)) > 1)
			{
				posTemp[i].x = ava.x * AvMain.hd + direct * num;
			}
			posTemp[i].y = posDay[i].y;
		}
	}

	private void updatePosMoi()
	{
		if (isQuan != 1)
		{
			return;
		}
		if (posMoi.anchor == -1)
		{
			posMoi.x = (posMoitran.x = (posMoiTemp.x = posDay[size - 1].x));
			posMoi.y = (posMoitran.y = (posMoiTemp.y = posDay[size - 1].y));
			posMoi.anchor = 0;
			iRnd = -1;
		}
		int num = posMoiTemp.x - posMoitran.x;
		int num2 = posMoiTemp.y - posMoitran.y;
		if (iRnd > 0)
		{
			iRnd--;
		}
		if ((iRnd <= 0 || isCanCau) && Canvas.gameTick % 2 == 1)
		{
			if (CRes.abs(num) > 0)
			{
				if (num > 0)
				{
					posMoiTemp.x--;
				}
				else
				{
					posMoiTemp.x++;
				}
				posDay[size - 1].x = posMoiTemp.x;
			}
			if (CRes.abs(num2) > 0)
			{
				if (num2 > 0)
				{
					posMoiTemp.y--;
				}
				else
				{
					posMoiTemp.y++;
				}
				posDay[size - 1].y = posMoiTemp.y;
			}
		}
		if (CRes.abs(num) <= 0 && CRes.abs(num2) <= 0)
		{
			iRnd = 50 + CRes.rnd(100);
			posMoitran.x = posMoi.x + 10 - CRes.rnd(20);
			posMoitran.y = posMoi.y + CRes.rnd(6);
		}
	}

	private void updateWave()
	{
		for (int i = 0; i < 2; i++)
		{
			if (waves[i].anchor == 0 || waves[i].x == -10)
			{
				waves[i].x = posTemp[size - 2].x;
				waves[i].y = posTemp[size - 2].y;
			}
			if (isCanCau)
			{
				waves[i].anchor += 2;
			}
			else
			{
				waves[i].anchor++;
			}
			if (waves[i].anchor > radius + (isCanCau ? 10 : 0))
			{
				waves[i].anchor = 0;
			}
		}
	}

	private void updateWait()
	{
		if (isAble())
		{
			doQuanCau(ava);
			isWait = false;
		}
	}

	private void updateCanCau()
	{
		if (!isCanCau)
		{
			return;
		}
		if (distant > 4 && Canvas.gameTick % 6 == 3)
		{
			distant--;
		}
		if (!isSuccess && Canvas.gameTick % 6 == 3 && ava != GameMidlet.avatar)
		{
			if (ava.action == 2)
			{
				setPosDay(1);
			}
			else
			{
				setPosDay(0);
			}
		}
		if (!isSuccess || distant > 4)
		{
			return;
		}
		distant = 2;
		int num = 0;
		if (!isLac)
		{
			for (int i = 0; i < size - 1; i++)
			{
				if (!posDay[i].setDetectX(posDay[i + 1].x, 1))
				{
					num++;
				}
			}
		}
		if (num == 0 && !isLac)
		{
			posMoi.anchor = -2;
			isLac = true;
		}
	}

	private void updateQuanCau()
	{
		if (countQuan == -1)
		{
			return;
		}
		countQuan++;
		if (Canvas.gameTick % 4 != 2)
		{
			return;
		}
		if (ava.action == 2)
		{
			ava.action = 13;
			if (countQuan > 16)
			{
				doQuanDay();
				countQuan = -1;
			}
		}
		else
		{
			ava.action = 2;
		}
	}

	private void setLacMoi()
	{
		if (isLac && idFish > 0)
		{
			aa++;
			if (aa < 2)
			{
				for (int i = 1; i < size; i++)
				{
					posDay[i].x -= 6;
				}
			}
			else if (aa > 4 && aa < 8)
			{
				for (int j = 1; j < size; j++)
				{
					posDay[j].x += 6;
				}
			}
			else if (aa > 14)
			{
				rLac--;
				if (rLac < 0)
				{
					aa = 0;
					rLac = CRes.rnd(20);
				}
			}
		}
		else
		{
			rLac = CRes.rnd(20);
		}
	}

	private void updateDayCau()
	{
		if (indexQuan == 0)
		{
			return;
		}
		if (isQuan == 1)
		{
			for (int i = 1; i < size - 2; i++)
			{
				posDay[i].y += 6;
			}
			setLacMoi();
			if (distant == dis)
			{
				distant = 7;
			}
		}
		bool flag = false;
		int num = size - 1;
		int num2 = 1;
		if (isSuccess)
		{
			num2 = 0;
		}
		for (int j = 1; j < size - isQuan * num2; j++)
		{
			int num3 = CRes.distance(posDay[j].x, posDay[j].y, posDay[j - 1].x, posDay[j - 1].y);
			if (num3 > distant + 1)
			{
				flag = true;
				int num4 = num3 - distant;
				int num5 = 0;
				num5 = CRes.angle(posDay[j - 1].x - posDay[j].x, -(posDay[j - 1].y - force - posDay[j].y));
				int num6 = num4 * CRes.cos(CRes.fixangle(num5)) >> 10;
				int num7 = -(num4 * CRes.sin(CRes.fixangle(num5))) >> 10;
				posDay[j].x += num6;
				posDay[j].y += num7;
			}
		}
		if (posDay[num].y < ava.y + ava.ySat + 5)
		{
			posDay[num].x += v;
			posDay[num].y += g;
			g++;
		}
		if (!isSuccess)
		{
			for (int num8 = num - 1; num8 > 0; num8--)
			{
				int num9 = CRes.distance(posDay[num8].x, posDay[num8].y, posDay[num8 + 1].x, posDay[num8 + 1].y);
				if (num9 > distant + 1)
				{
					flag = true;
					int angle = CRes.angle(posDay[num8 + 1].x - posDay[num8].x, -(posDay[num8 + 1].y - posDay[num8].y));
					int num10 = num9 - distant;
					int num11 = num10 * CRes.cos(CRes.fixangle(angle)) >> 10;
					int num12 = -(num10 * CRes.sin(CRes.fixangle(angle))) >> 10;
					posDay[num8].x += num11;
					posDay[num8].y += num12;
				}
			}
		}
		if (!flag)
		{
			isQuan = 1;
		}
	}

	public void paint(MyGraphics g)
	{
		if (isWait || countQuan != -1)
		{
			return;
		}
		if (AvMain.hd > 1)
		{
			g.translate(0f, ava.y);
		}
		if (isQuan != 0 && !isSuccess && (float)waves[0].x > AvCamera.gI().xCam && (float)waves[0].x < AvCamera.gI().xCam + (float)Canvas.w)
		{
			g.setColor(color[LoadMap.status]);
		}
		g.setColor(8685448);
		if (((float)posTemp[0].x > AvCamera.gI().xCam && (float)posTemp[0].x < AvCamera.gI().xCam + (float)Canvas.w) || ((float)posTemp[size - 1].x > AvCamera.gI().xCam && (float)posTemp[size - 1].x < AvCamera.gI().xCam + (float)Canvas.w))
		{
			for (int i = 0; i < size - 1 - isQuan; i++)
			{
				if (posTemp[i + 1].y < ava.y + ava.ySat + 20)
				{
					g.drawLine(posTemp[i].x, posTemp[i].y, posTemp[i + 1].x, posTemp[i + 1].y);
				}
			}
			if (isQuan == 0 && posTemp[size - 1].y < ava.y + ava.ySat + 10)
			{
				PaintPopup.fill(posTemp[size - 1].x, posTemp[size - 1].y, 2, 2, 0, g);
			}
			g.drawImage(FishingScr.imgPhao, posTemp[size - 2].x, posTemp[size - 2].y, 3);
			if (isSuccess && idFish > 0)
			{
				FishingScr.imgCa.drawFrame(count / 3, posTemp[size - 2].x + 2, posTemp[size - 2].y + 4, 0, MyGraphics.RIGHT | MyGraphics.TOP, g);
				if (Canvas.gameTick % 10 > 5)
				{
					PartSmall partSmall = (PartSmall)AvatarData.getPart((short)idFish);
					if (partSmall != null)
					{
						partSmall.paint(g, ava.x * AvMain.hd, ava.y - 55 * AvMain.hd, 3);
					}
				}
			}
		}
		if (AvMain.hd > 1)
		{
			g.translate(0f, -ava.y);
		}
	}

	public bool isAble()
	{
		if (ava.action == 2 || ava.action == 13)
		{
			return true;
		}
		return false;
	}
}
