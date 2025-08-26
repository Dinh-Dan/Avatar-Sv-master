public class onMainMenu : OnScreen
{
	public static onMainMenu me;

	public Command cmdSellect;

	private int index;

	private int x;

	private int y;

	private int wBai;

	private int hBai;

	private int numW;

	private int wImg;

	private static FrameImage[] imgBaiLon;

	public static int cmtoX;

	public static int cmx;

	public static int cmdx;

	public static int cmvx;

	public static int cmxLim;

	public static bool isOngame;

	public new static bool isHide;

	public static Image imgSelected;

	public static FontX smallGrey;

	public static Image[] imgTitle;

	public static int iChangeGame;

	public float pb;

	public float vX;

	public float dxTran;

	public float timeOpen;

	public float xCamLast;

	public float count;

	public float timeDelay;

	public float timePointX;

	public bool transX;

	private int pxLast;

	private int a;

	private int b;

	private int v = 2;

	private int g;

	private int ylogo = -40;

	private int dir = 1;

	public onMainMenu()
	{
		cmdSellect = new Command(T.selectt, 0);
		right = new Command("Top", 2);
		addCmd(1, 1);
	}

	public static void initImg()
	{
		if (imgBaiLon == null)
		{
			imgBaiLon = new FrameImage[5];
			imgTitle = new Image[3];
			for (int i = 0; i < 5; i++)
			{
				imgBaiLon[i] = new FrameImage(Image.createImagePNG(T.mode[AvMain.hd - 1] + "/hd/on/icon" + i), 71 * AvMain.hd, 76 * AvMain.hd);
			}
			for (int j = 0; j < 3; j++)
			{
				imgTitle[j] = Image.createImagePNG(T.mode[AvMain.hd - 1] + "/hd/on/imgTitle" + j);
			}
			smallGrey = new HDFont(13, "arialSmall", 12907498, 12907498);
		}
	}

	public static void resetImg()
	{
		smallGrey = null;
		imgBaiLon = null;
		imgSelected = null;
		smallGrey = null;
		Menu.me = null;
		OnSplashScr.imgLogomainMenu = null;
	}

	public static void resetIcon()
	{
		imgBaiLon = null;
		imgSelected = null;
		OnSplashScr.imgLogomainMenu = null;
	}

	public static onMainMenu gI()
	{
		if (me == null)
		{
			me = new onMainMenu();
		}
		return me;
	}

	public override void switchToMe()
	{
		MyScreen.colorBar = MyScreen.colorMiniMap;
		selected = 2;
		Canvas.menuMain = null;
		Canvas.endDlg();
		initImg();
		base.switchToMe();
		init();
		if (Canvas.load == 0)
		{
			Canvas.load = 1;
		}
		isHide = true;
		isOngame = true;
		ChatTextField.gI().init(Canvas.hCan);
	}

	public void init()
	{
		numW = 4;
		wImg = 70 * AvMain.hd;
		wBai = Canvas.w / numW;
		if (wBai > 100 * AvMain.hd)
		{
			wBai = 100 * AvMain.hd;
		}
		hBai = imgBaiLon[0].frameHeight + 5 * AvMain.hd;
		y = Canvas.h - hBai * 2 + hBai / 2;
		x = (Canvas.w - numW * wBai) / 2 + wBai / 2;
		cmxLim = numW * wBai - Canvas.w;
		if (cmxLim < 0)
		{
			cmxLim = 0;
		}
		initSize();
	}

	public static void initSize()
	{
		Canvas.hTab = MyScreen.hText;
		Canvas.h = (int)ScaleGUI.HEIGHT - Canvas.hTab;
		for (int i = 0; i < 3; i++)
		{
			Canvas.posCmd[i].y = Canvas.hCan - Canvas.hTab;
		}
	}

	public static void resetSize()
	{
		Canvas.hCan = (Canvas.h = (int)ScaleGUI.HEIGHT);
		Canvas.paint.initPos();
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			switch (this.index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				doGetHandlerCasino(this.index);
				break;
			case 4:
				TransMoneyDlg.gI().show();
				break;
			}
			break;
		case 1:
			iChangeGame = 1;
			Canvas.cameraList.close();
			GlobalService.gI().joinAvatar();
			Canvas.startWaitDlg();
			break;
		case 2:
			GlobalService.gI().doCommunicate(1);
			Canvas.startWaitDlg();
			break;
		}
	}

	public override void close()
	{
		commandTab(1);
	}

	public void doGetHandlerCasino(int i)
	{
		GlobalService.gI().getHandler(3);
		MapScr.typeCasino = (sbyte)i;
		Canvas.load = 0;
	}

	public void click()
	{
		isHide = true;
		cmdSellect.perform();
	}

	public override void update()
	{
		if (timeOpen > 0f)
		{
			timeOpen -= 1f;
			if (timeOpen == 0f && Canvas.currentMyScreen != PopupShop.me)
			{
				click();
			}
		}
		if (vX != 0f)
		{
			if (cmx < 0 || cmx > cmxLim)
			{
				if (vX > 500f)
				{
					vX = 500f;
				}
				else if (vX < -500f)
				{
					vX = -500f;
				}
				vX -= vX / 5f;
				if (CRes.abs((int)(vX / 10f)) <= 10)
				{
					vX = 0f;
				}
			}
			cmx += (int)(vX / 15f);
			cmtoX = cmx;
			vX -= vX / 20f;
		}
		else if (cmx < 0)
		{
			cmtoX = 0;
		}
		else if (cmx > cmxLim)
		{
			cmtoX = cmxLim;
		}
		if (cmx != cmtoX)
		{
			cmvx = cmtoX - cmx << 2;
			cmdx += cmvx;
			cmx += cmdx >> 4;
			cmdx &= 15;
		}
		if (g >= 0)
		{
			ylogo += dir * g;
			g += dir * v;
			if (g <= 0)
			{
				dir *= -1;
			}
			if (ylogo > 0)
			{
				dir *= -1;
				g -= 2 * v;
			}
		}
	}

	public override void updateKey()
	{
		count += 1f;
		base.updateKey();
		if (Canvas.isPointerClick)
		{
			for (int i = 0; i < T.nameMenuOn.Length; i++)
			{
				if (Canvas.isPointer(x + i % numW * wBai - wImg / 2, y + i / numW * hBai - wImg / 2, wImg, wImg + Canvas.normalWhiteFont.getHeight() + 10))
				{
					pxLast = Canvas.pxLast;
					timeDelay = count;
					pb = cmx;
					vX = 0f;
					Canvas.isPointerClick = false;
					transX = true;
					break;
				}
			}
		}
		if (!transX)
		{
			return;
		}
		float num = count - timeDelay;
		int num2 = pxLast - Canvas.px;
		pxLast = Canvas.px;
		if (Canvas.isPointerDown)
		{
			if (count % 2f == 0f)
			{
				dxTran = Canvas.px;
				timePointX = count;
			}
			vX = 0f;
			if (cmtoX <= 0 || cmtoX >= cmxLim)
			{
				cmtoX = (int)(pb + (float)(Canvas.dx() / 2));
			}
			else
			{
				cmtoX = (int)(pb + (float)num2);
				pb = cmtoX;
			}
			cmx = cmtoX;
			if (num < 20f)
			{
				int num3 = (cmtoX + Canvas.px - (x - wBai / 2)) / wBai;
				int num4 = (Canvas.py - (y - wBai / 2)) / hBai;
				index = num4 * numW + num3;
				if (index < 0)
				{
					index = 0;
				}
				if (index >= T.nameMenuOn.Length)
				{
					index = T.nameMenuOn.Length - 1;
				}
			}
			if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd || CRes.abs(Canvas.dx()) >= 10 * AvMain.hd)
			{
				isHide = true;
			}
			else if (num > 3f && num < 8f)
			{
				isHide = false;
			}
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		float num5 = dxTran - (float)Canvas.px;
		int num6 = (int)(count - timePointX);
		if (CRes.abs((int)num5) > 40 && num6 < 20 && cmtoX > 0 && cmtoX < cmxLim)
		{
			vX = num5 / (float)num6 * 10f;
		}
		timePointX = -1f;
		if (CRes.abs(Canvas.dy()) < 10 * AvMain.hd && CRes.abs(Canvas.dx()) < 10 * AvMain.hd)
		{
			if (num <= 4f)
			{
				timeOpen = 5f;
				isHide = false;
			}
			else if (!isHide)
			{
				click();
			}
			Canvas.paint.clickSound();
		}
		transX = false;
	}

	public override void paintMain(MyGraphics g)
	{
		Canvas.paint.paintDefaultBg(g);
		if (imgBaiLon == null || Canvas.load != -1)
		{
			return;
		}
		if (Canvas.iOpenOngame != 2)
		{
			paintTitle(g, T.listNameGame, Canvas.w / 2, (y - hBai / 2) / 2);
		}
		g.translate(x, y);
		g.translate(-cmx, 0f);
		if (Canvas.load == -1)
		{
			for (int i = 0; i < T.nameMenuOn.Length; i++)
			{
				imgBaiLon[i].drawFrame((index == i && !isHide) ? 1 : 0, i % numW * wBai, i / numW * hBai, 0, 3, g);
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		paintMain(g);
		base.paint(g);
		Canvas.paintPlus2(g);
	}

	public static void paintTitle(MyGraphics g, string text, int x, int y)
	{
		int num = Canvas.numberFont.getWidth(text) + 20;
		int width = imgTitle[1].getWidth();
		int num2 = num / width;
		for (int i = 0; i < num2; i++)
		{
			g.drawImage(imgTitle[1], x - num2 * width / 2 + width * i, y - imgTitle[1].getHeight() / 2, 0);
		}
		g.drawImage(imgTitle[0], x - num2 * width / 2 - imgTitle[0].getWidth(), y - imgTitle[1].getHeight() / 2, 0);
		g.drawImage(imgTitle[2], x - num2 * width / 2 + num2 * width, y - imgTitle[1].getHeight() / 2, 0);
		Canvas.numberFont.drawString(g, text, x, y - Canvas.numberFont.getHeight(), 2);
	}
}
