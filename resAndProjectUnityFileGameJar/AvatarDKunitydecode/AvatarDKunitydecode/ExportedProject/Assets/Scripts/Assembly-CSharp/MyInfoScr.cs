public class MyInfoScr : MyScreen
{
	private class IActionWedding : IAction
	{
		private short idActionWedding;

		public IActionWedding(short idActionWedding)
		{
			this.idActionWedding = idActionWedding;
		}

		public void perform()
		{
			GlobalService.gI().doRequestCmdRotate(idActionWedding, -1);
			me.close();
			Canvas.startWaitDlg();
		}
	}

	public static MyInfoScr me;

	public int x;

	public int y;

	public int w;

	public int h;

	public int yLine;

	public int hIndex;

	public int yBackG;

	public int xLeft;

	public int xRight;

	public sbyte countClose;

	private MyScreen lastScr;

	public Image[] imgIcon;

	public Image[] imgThanh;

	private Image imgBack;

	private Image imgTraiTim;

	public Image imgShadow;

	private Avatar friend;

	private Avatar avatar;

	private sbyte level;

	private sbyte perLevel;

	private string tenQuanHe = string.Empty;

	private int focusTab;

	private bool isTranKey;

	private sbyte indexTab = -1;

	public MyInfoScr()
	{
		if (AvMain.hd == 2)
		{
			w = 580;
		}
		else
		{
			w = 300;
		}
		imgIcon = new Image[5];
		for (int i = 0; i < 5; i++)
		{
			imgIcon[i] = Image.createImagePNG(T.getPath() + "/myinfo/icon" + i);
		}
		imgShadow = Image.createImagePNG(T.getPath() + "/myinfo/shadow");
	}

	public static MyInfoScr gI()
	{
		return (me != null) ? me : (me = new MyInfoScr());
	}

	public override void switchToMe()
	{
		lastScr = Canvas.currentMyScreen;
		base.switchToMe();
	}

	public new void close()
	{
		lastScr.switchToMe();
		center = null;
		friend = null;
		avatar = null;
		imgBack = null;
		imgTraiTim = null;
	}

	public void setInfo(Avatar ava, Avatar friend, string sologan, short idImage, sbyte lv, sbyte perLv, string tenQuanHe, short idActionWedding, string nameAction)
	{
		this.friend = friend;
		avatar = ava;
		level = lv;
		perLevel = perLv;
		this.tenQuanHe = tenQuanHe;
		loadImage();
		avatar.direct = Base.RIGHT;
		if (friend != null)
		{
			friend.direct = Base.LEFT;
		}
		hIndex = imgThanh[2].h + 10 * AvMain.hd;
		x = (Canvas.w - w) / 2;
		h = 120 * AvMain.hd + hIndex * 3 + 40 * AvMain.hd;
		yBackG = 25 * AvMain.hd;
		if (friend != null)
		{
			h += 54 * AvMain.hd;
		}
		if (AvMain.hd == 1)
		{
			h += 22;
			yLine = h - (hIndex * 3 + 48);
		}
		else
		{
			yLine = h - (hIndex * 3 + 84);
		}
		y = (Canvas.hCan - h) / 2 + PaintPopup.hTab / 2;
		center = null;
		if (nameAction != string.Empty)
		{
			center = new Command(nameAction, new IActionWedding(idActionWedding));
			center.x = x + 20 * AvMain.hd + PaintPopup.wButtonSmall / 2;
			center.y = y + yLine + PaintPopup.hTab - PaintPopup.hButtonSmall - 3 * AvMain.hd;
		}
		for (int i = 0; i < 3; i++)
		{
			string empty = string.Empty;
			empty = ((i != 0 || avatar.lvFarm == -1) ? (T.myIndex[i] + ((i != 0) ? string.Empty : (string.Empty + avatar.lvMain))) : ("Lv NT " + avatar.lvFarm));
			int width = Canvas.fontBlu.getWidth(empty);
			if (width > xLeft)
			{
				xLeft = width;
			}
			width = Canvas.fontBlu.getWidth(avatar.indexP[2 + i] + string.Empty);
			if (width > xRight)
			{
				xRight = width;
			}
		}
		xLeft -= 4 * AvMain.hd;
		switchToMe();
		Canvas.endDlg();
	}

	private void loadImage()
	{
		if (imgBack == null)
		{
			imgBack = Image.createImagePNG(T.getPath() + "/myinfo/back");
			imgTraiTim = Image.createImagePNG(T.getPath() + "/myinfo/traitim");
			imgThanh = new Image[3];
			for (int i = 0; i < 3; i++)
			{
				imgThanh[i] = Image.createImagePNG(T.getPath() + "/myinfo/thanh" + i);
			}
		}
	}

	public override void update()
	{
		lastScr.update();
	}

	public override void updateKey()
	{
		base.updateKey();
		int num = MyScreen.wTab + 20;
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(x + w + 5 - 5 * AvMain.hd - 20 * AvMain.hd, y + MyScreen.hTab - 6 + 3 * AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				isTranKey = true;
				Canvas.isPointerClick = false;
				countClose = 5;
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					if (Canvas.isPoint(x + 12 * AvMain.hd + i * num, y - PaintPopup.hTab, num, PaintPopup.hTab))
					{
						isTranKey = true;
						Canvas.isPointerClick = false;
						indexTab = (sbyte)i;
						break;
					}
				}
			}
		}
		if (!isTranKey)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			if (countClose == 5 && !Canvas.isPoint(x + w + 5 - 5 * AvMain.hd - 20 * AvMain.hd, y + MyScreen.hTab - 6 + 3 * AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			else if (indexTab != -1 && !Canvas.isPoint(x + 12 * AvMain.hd + indexTab * num, y - PaintPopup.hTab, num, PaintPopup.hTab))
			{
				indexTab = -1;
			}
		}
		if (Canvas.isPointerRelease)
		{
			Canvas.isPointerRelease = false;
			isTranKey = false;
			if (indexTab != -1)
			{
				focusTab = indexTab;
				indexTab = -1;
			}
			else if (countClose == 5)
			{
				close();
				countClose = 0;
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		Canvas.resetTrans(g);
		Canvas.paint.paintBoxTab(g, x, y - PaintPopup.hTab, h + PaintPopup.hTab, w, (sbyte)focusTab, PaintPopup.gI().wSub, MyScreen.wTab, PaintPopup.hTab, 2, 2, PaintPopup.gI().count, PaintPopup.gI().colorTab, T.nameTab[focusTab], (sbyte)(countClose / 3), -1, false, false, T.nameTab, 0f, null);
		g.translate(x, y);
		int num = 23 * AvMain.hd;
		int num2 = 20 * AvMain.hd;
		for (int i = 0; i < 4; i++)
		{
			g.drawImage(imgIcon[i], 16 * AvMain.hd + num / 2, 20 * AvMain.hd + num / 2 + i * num, 3);
		}
		Canvas.normalFont.drawString(g, avatar.money[0] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 - Canvas.normalFont.getHeight() / 2, 0);
		Canvas.normalFont.drawString(g, avatar.money[2] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num - Canvas.normalFont.getHeight() / 2, 0);
		Canvas.normalFont.drawString(g, avatar.money[3] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num * 2 - Canvas.normalFont.getHeight() / 2, 0);
		Canvas.normalFont.drawString(g, avatar.lvMain + "+" + avatar.perLvMain + "%", 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num * 3 - Canvas.normalFont.getHeight() / 2, 0);
		int num3 = 0;
		g.drawImage(imgBack, w - imgBack.w - 20 * AvMain.hd, yBackG, 0);
		avatar.paintIcon(g, w - imgBack.w - 20 * AvMain.hd + 40 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd, true);
		if (friend == null)
		{
			g.drawImage(imgShadow, w - 20 * AvMain.hd - imgShadow.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd - imgShadow.h / 2, 3);
		}
		else
		{
			friend.paintIcon(g, w - 20 * AvMain.hd - imgShadow.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd, true);
			Canvas.fontBlu.drawString(g, tenQuanHe, w - imgBack.w / 2 - 20 * AvMain.hd, yBackG + AvMain.hd + imgBack.h, 2);
			g.drawImage(imgTraiTim, w - imgBack.w / 2 - 20 * AvMain.hd, yBackG + 2 * AvMain.hd + imgBack.h + Canvas.fontBlu.getHeight() + imgTraiTim.h / 2, 3);
			Canvas.fontBlu.drawString(g, level + "+" + perLevel + "%", w - imgBack.w / 2 - 20 * AvMain.hd, yBackG - AvMain.hd + imgBack.h + Canvas.fontBlu.getHeight() + imgTraiTim.h, 2);
			g.drawImage(imgThanh[2], w - imgBack.w / 2 - 20 * AvMain.hd, yLine - imgThanh[2].h, 3);
			num3 = perLevel * imgThanh[1].w / 100;
			g.setClip(w - imgBack.w / 2 - 20 * AvMain.hd - imgThanh[2].w / 2, yLine - imgThanh[2].h - imgThanh[2].h / 2, num3, imgThanh[2].h);
			g.drawImage(imgThanh[1], w - imgBack.w / 2 - 20 * AvMain.hd, yLine - imgThanh[2].h, 3);
			g.setClip(0f, yLine, w, h - PaintPopup.hTab - yLine);
			g.drawImage(imgThanh[0], w - imgBack.w / 2 - 20 * AvMain.hd, yLine - imgThanh[2].h - 2 * AvMain.hd, 3);
		}
		g.translate(0f, yLine);
		g.setColor(29068);
		g.fillRect(20 * AvMain.hd, 0f, w - 40 * AvMain.hd, 1f);
		g.setColor(12255224);
		g.fillRect(20 * AvMain.hd, 1f, w - 40 * AvMain.hd, 1f);
		int num4 = (h - yLine - PaintPopup.hTab - hIndex * 3) / 2 - 2;
		for (int j = 0; j < 3; j++)
		{
			g.drawImage(imgThanh[2], num2 + xLeft + imgThanh[2].w / 2, num4 + hIndex / 2 + hIndex * j, 3);
			g.drawImage(imgThanh[2], w - num2 - xRight - imgThanh[2].w / 2, num4 + hIndex / 2 + hIndex * j, 3);
			if (j == 0)
			{
				if (avatar.lvFarm != -1)
				{
					Canvas.fontBlu.drawString(g, "Lv NT " + avatar.lvFarm, num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
				}
				else
				{
					Canvas.fontBlu.drawString(g, T.myIndex[j] + ((j != 0) ? string.Empty : (string.Empty + avatar.lvMain)), num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
				}
			}
			else
			{
				Canvas.fontBlu.drawString(g, T.myIndex[j] + ((j != 0) ? string.Empty : (string.Empty + avatar.lvMain)), num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
			}
			Canvas.fontBlu.drawString(g, T.myIndex[3 + j], w - num2 - xRight - imgThanh[2].w - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
		}
		if (avatar.lvFarm != -1)
		{
			Canvas.fontBlu.drawString(g, avatar.perLvFarm + "%", num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 - Canvas.fontBlu.getHeight() / 2 - 3 * AvMain.hd, 0);
		}
		else
		{
			Canvas.fontBlu.drawString(g, avatar.perLvMain + "%", num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 - Canvas.fontBlu.getHeight() / 2 - 3 * AvMain.hd, 0);
		}
		for (int k = 0; k < 3; k++)
		{
			if (k > 0)
			{
				Canvas.fontBlu.drawString(g, avatar.indexP[k - 1] + string.Empty, num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * k - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 0);
			}
			Canvas.fontBlu.drawString(g, avatar.indexP[2 + k] + string.Empty, w - num2 - xRight + 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * k - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 0);
		}
		int num5 = 0;
		if (AvMain.hd == 2)
		{
			num5 = 1;
		}
		for (int l = 0; l < 3; l++)
		{
			g.setClip(w: (l != 0) ? (avatar.indexP[l - 1] * imgThanh[1].w / 100) : ((avatar.lvFarm == -1) ? (avatar.perLvMain * imgThanh[1].w / 100) : (avatar.perLvFarm * imgThanh[1].w / 100)), x: num2 + xLeft + 1, y: num4 + hIndex * l, h: hIndex);
			g.drawImage(imgThanh[1], num2 + xLeft + imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * l - num5, 3);
			num3 = avatar.indexP[2 + l] * imgThanh[1].w / 100;
			g.setClip(w - num2 - xRight - imgThanh[2].w + 2, num4 + hIndex * l, num3, hIndex);
			g.drawImage(imgThanh[1], w - num2 - xRight - imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * l - num5, 3);
		}
		g.setClip(0f, 0f, w, h - PaintPopup.hTab - yLine);
		for (int m = 0; m < 3; m++)
		{
			g.drawImage(imgThanh[0], num2 + xLeft + imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * m - 2 * AvMain.hd, 3);
			g.drawImage(imgThanh[0], w - num2 - xRight - imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * m - 2 * AvMain.hd, 3);
		}
		base.paint(g);
		g.setColor(0);
		g.drawRect(x + w + 5 - 5 * AvMain.hd - 20 * AvMain.hd, y + MyScreen.hTab - 6 + 3 * AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd);
	}
}
