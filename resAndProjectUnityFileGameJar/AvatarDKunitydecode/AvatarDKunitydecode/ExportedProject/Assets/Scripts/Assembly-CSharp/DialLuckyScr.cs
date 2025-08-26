using System;

public class DialLuckyScr : MyScreen
{
	private static DialLuckyScr me;

	public static Image imgCau;

	public static Image imgCau_back;

	public static Image imgDo;

	public static Image imgDauHoi;

	public static Image imgDot;

	public static FrameImage imgFireWork;

	private int radius;

	private int degree;

	private int part;

	private int g;

	private int degreeKim;

	private int num;

	private int selectedNumber;

	private AvPosition posCenter;

	private bool isTurn;

	private bool isPaint;

	private bool isable;

	private MyScreen lastScr;

	private short idPart;

	private Command cmdDial;

	private Command cmdWait;

	private Command cmdClose;

	private MyVector listGift = new MyVector();

	private long timePaint;

	private bool[] isFireWork;

	private MyVector listFireWork;

	public DialLuckyScr()
	{
		radius = 90 * AvMain.hd;
		posCenter = new AvPosition(Canvas.w, Canvas.hh);
		part = 30;
		num = 360 / part;
		cmdDial = new Command(T.quay, 0, this);
		cmdWait = new Command(T.pleaseWait, 1, this);
		cmdClose = new Command(T.close, 2, this);
		center = cmdDial;
		degreeKim = 90;
		isFireWork = new bool[3];
		listFireWork = new MyVector();
	}

	public static DialLuckyScr gI()
	{
		return (me != null) ? me : (me = new DialLuckyScr());
	}

	public void switchToMe(MyScreen currentMyScreen, short idPart)
	{
		lastScr = currentMyScreen;
		this.idPart = idPart;
		Canvas.keyHold[5] = false;
		base.switchToMe();
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			isable = true;
			if (degreeKim > 90)
			{
				start();
			}
			break;
		case 2:
			lastScr.switchToMe();
			doContinue();
			break;
		}
	}

	protected void doContinue()
	{
		isPaint = false;
		center = cmdDial;
		for (int i = 0; i < 3; i++)
		{
			isFireWork[i] = false;
		}
		listFireWork.removeAllElements();
		setItemBay(listGift, GameMidlet.avatar, 0);
	}

	private void setItemBay(MyVector list, Avatar ava, int delay)
	{
		int num = delay;
		for (int i = 0; i < list.size(); i++)
		{
			Gift gift = (Gift)list.elementAt(i);
			string text = string.Empty;
			switch (gift.type)
			{
			case 1:
			{
				Part part = AvatarData.getPart(gift.idPart);
				ImageInfo imageInfo = AvatarData.listImgInfo[part.idIcon];
				Canvas.addFlyText(0, ava.x, ava.y - 50, -1, CRes.createImgByImg(imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd, AvatarData.getBigImgInfo(imageInfo.bigID).img), num);
				break;
			}
			case 2:
				text = "+" + gift.xu + T.xu;
				ava.setMoney(ava.money[0] + gift.xu);
				num += 20;
				break;
			case 3:
				text = "+" + gift.xp + " xp";
				ava.setExp(ava.exp + gift.xp);
				num += 20;
				break;
			case 4:
				text = "+" + gift.luong + T.gold;
				ava.money[2] += gift.luong;
				num += 20;
				break;
			}
			if (!text.Equals(string.Empty))
			{
				Canvas.addFlyTextSmall(text, ava.x, ava.y - 50, -1, 1, num);
			}
		}
	}

	public override void update()
	{
		lastScr.update();
		if (g > 0)
		{
			degree -= g;
			if (degree < 0)
			{
				degree = 7200 + degree;
			}
			if (g < 10)
			{
				if (degree / 20 % 30 == 0)
				{
					g = 0;
				}
			}
			else
			{
				g--;
			}
			if (Canvas.gameTick % 8 == 4)
			{
				int num = CRes.rnd(this.num);
				int num2 = degree / 20 + num * part;
				if (num2 > 360)
				{
					num2 -= 360;
				}
				int num3 = radius * CRes.cos(CRes.fixangle(num2)) >> 10;
				int num4 = -(radius * CRes.sin(CRes.fixangle(num2))) >> 10;
				addFire(posCenter.x + num3, posCenter.y + num4);
			}
		}
		else if (isTurn)
		{
			stop();
		}
		if (center == cmdWait)
		{
			int num5 = 0;
			for (int i = 0; i < isFireWork.Length; i++)
			{
				if (isFireWork[i])
				{
					num5++;
				}
			}
			if (num5 == 3)
			{
				center = cmdClose;
			}
		}
		for (int j = 0; j < listFireWork.size(); j++)
		{
			Point point = (Point)listFireWork.elementAt(j);
			point.x += point.g;
			if (point.g > 1 || point.g < -1)
			{
				point.g -= point.g / CRes.abs(point.g);
			}
			point.y += point.h;
			point.h++;
			point.color++;
			if (point.color > 20)
			{
				listFireWork.removeElement(point);
			}
		}
		if (!isPaint)
		{
			return;
		}
		for (int k = 0; k < listGift.size(); k++)
		{
			if (!isFireWork[k] && Environment.TickCount / 100 - timePaint > (k + 1) * 5)
			{
				isFireWork[k] = true;
				Gift gift = (Gift)listGift.elementAt(k);
				addFire(gift.x, gift.y);
			}
		}
	}

	private void addFire(int x, int y)
	{
		for (int i = 0; i < 10; i++)
		{
			int num = 1;
			if (i % 2 == 0)
			{
				num = -1;
			}
			Point point = new Point(x, y);
			point.color = 0;
			point.g = num * (CRes.rnd(80) / 10);
			point.h = -CRes.rnd(70) / 10;
			listFireWork.addElement(point);
		}
	}

	private void stop()
	{
		isTurn = false;
		isPaint = true;
		isable = false;
		timePaint = Environment.TickCount / 100;
		for (int i = 0; i < listGift.size(); i++)
		{
			Gift gift = (Gift)listGift.elementAt(i);
			int num = 0;
			switch (i)
			{
			case 0:
				num = 150;
				break;
			case 1:
				num = 180;
				break;
			default:
				num = 210;
				break;
			}
			int num2 = radius * CRes.cos(CRes.fixangle(num)) >> 10;
			int num3 = -(radius * CRes.sin(CRes.fixangle(num))) >> 10;
			gift.x = posCenter.x + num2;
			gift.y = posCenter.y + num3;
		}
	}

	public override void updateKey()
	{
		if (!isPaint)
		{
			if (AvMain.indexCenter == 4)
			{
				if (degreeKim < 270)
				{
					degreeKim += 3;
				}
			}
			else if (degreeKim > 90)
			{
				degreeKim -= 3;
			}
		}
		base.updateKey();
	}

	private void start()
	{
		if (!isTurn && isable)
		{
			selectedNumber = degreeKim;
			GlobalService.gI().doDialLucky(idPart, selectedNumber - 90);
			Canvas.startWaitDlg();
		}
	}

	public void onStart(int idUser, int degree, MyVector listGift1)
	{
		if (idUser != GameMidlet.avatar.IDDB)
		{
			Avatar avatar = LoadMap.getAvatar(idUser);
			if (avatar != null)
			{
				setItemBay(listGift1, avatar, 100 + degree + 20);
			}
		}
		else
		{
			center = cmdWait;
			listGift = listGift1;
			g = 100 + (selectedNumber - 90);
			isTurn = true;
			Canvas.endDlg();
		}
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		Canvas.resetTrans(g);
		int num = degree / 20;
		for (int i = 0; i < this.num; i++)
		{
			int num2 = num + i * part;
			if (num2 > 360)
			{
				num2 -= 360;
			}
			if (num2 >= 82 && num2 <= 278)
			{
				int num3 = radius * CRes.cos(CRes.fixangle(num2)) >> 10;
				int num4 = -(radius * CRes.sin(CRes.fixangle(num2))) >> 10;
				g.drawImage(imgCau_back, posCenter.x + num3, posCenter.y + num4, 3);
			}
		}
		if (isPaint)
		{
			paintGift(g);
		}
		int num5 = 0;
		for (int j = 0; j < this.num; j++)
		{
			int num6 = num + j * part;
			if (num6 > 360)
			{
				num6 -= 360;
			}
			if (num6 >= 82 && num6 <= 278)
			{
				int num7 = radius * CRes.cos(CRes.fixangle(num6)) >> 10;
				int num8 = -(radius * CRes.sin(CRes.fixangle(num6))) >> 10;
				long num9 = Environment.TickCount / 100 - timePaint;
				if (!isPaint || num6 < 150 || num6 > 210 || (num9 <= (num5 + 1) * 5 && num9 > (num5 + 1) * 5 - 5))
				{
					g.drawImage(imgDauHoi, posCenter.x + num7, posCenter.y + num8, 3);
				}
				else
				{
					num5++;
				}
				g.drawImage(imgCau, posCenter.x + num7, posCenter.y + num8, 3);
			}
		}
		g.drawImage(imgDo, posCenter.x - imgDo.w / 2, posCenter.y, 3);
		paintKim(g);
		if (isPaint || this.g > 0)
		{
			paintFireWork(g);
		}
		base.paint(g);
	}

	private void paintFireWork(MyGraphics g)
	{
		for (int i = 0; i < listFireWork.size(); i++)
		{
			Point point = (Point)listFireWork.elementAt(i);
			imgFireWork.drawFrame(point.color / 5, point.x, point.y, 0, 3, g);
		}
	}

	private void paintKim(MyGraphics g)
	{
		int num = radius / 2 * CRes.cos(CRes.fixangle(degreeKim)) >> 10;
		int num2 = -(radius / 2 * CRes.sin(CRes.fixangle(degreeKim))) >> 10;
		g.drawImage(imgDot, posCenter.x + num, posCenter.y + num2, 3);
	}

	private void paintGift(MyGraphics g)
	{
		for (int i = 0; i < listGift.size(); i++)
		{
			if (Environment.TickCount / 100 - timePaint > (i + 1) * 5)
			{
				Gift gift = (Gift)listGift.elementAt(i);
				switch (gift.type)
				{
				case 1:
				{
					Part part = AvatarData.getPart(gift.idPart);
					part.paint(g, gift.x, gift.y, 3);
					Canvas.borderFont.drawString(g, gift.expire, gift.x - 17, gift.y - 7, 1);
					break;
				}
				case 2:
					Canvas.borderFont.drawString(g, T.xu, gift.x, gift.y - Canvas.borderFont.getHeight() / 2, 2);
					Canvas.borderFont.drawString(g, gift.xu + string.Empty, gift.x - 17, gift.y - 8, 1);
					break;
				case 3:
					Canvas.borderFont.drawString(g, "xp", gift.x, gift.y - Canvas.borderFont.getHeight() / 2, 2);
					Canvas.borderFont.drawString(g, gift.xp + string.Empty, gift.x - 17, gift.y - 8, 1);
					break;
				case 4:
					Canvas.borderFont.drawString(g, T.gold, gift.x, gift.y - Canvas.borderFont.getHeight() / 2, 2);
					Canvas.borderFont.drawString(g, gift.luong + string.Empty, gift.x - 17, gift.y - 8, 1);
					break;
				}
			}
		}
	}
}
