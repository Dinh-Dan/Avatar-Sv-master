public class MiniMap : MyScreen
{
	private class IActionRequestOK : IAction
	{
		private string des;

		public IActionRequestOK(string des)
		{
			this.des = des;
		}

		public void perform()
		{
			Canvas.startOK(des, new IActionYesRef());
		}
	}

	private class IActionRequestReg : IAction
	{
		private string des;

		public IActionRequestReg(string des)
		{
			this.des = des;
		}

		public void perform()
		{
			Canvas.startOKDlg(des, new IActionYesRef());
		}
	}

	private class IActionYesRef : IAction
	{
		public void perform()
		{
			TField[] array = new TField[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = new TField(string.Empty, Canvas.currentMyScreen, new IActionOkReg(array));
				array[i].autoScaleScreen = true;
				array[i].showSubTextField = false;
			}
			array[0].setFocus(true);
			array[0].setIputType(0);
			array[1].setIputType(2);
			array[2].setIputType(2);
			array[3].setIputType(0);
			string[][] subName = new string[4][]
			{
				new string[2]
				{
					"Tên:",
					string.Empty
				},
				new string[2]
				{
					"Mật khẩu:",
					string.Empty
				},
				new string[2] { "Nhập lại", "mật khẩu:" },
				new string[2] { "Số di động", "hoặc email:" }
			};
			InputFace.gI().setInfo(array, "Đăng Ký", subName, new Command(T.finish, new IActionOkReg(array)), Canvas.hCan);
			InputFace.gI().show();
		}
	}

	private class IActionOkReg : IAction
	{
		private TField[] tf;

		public IActionOkReg(TField[] tf)
		{
			this.tf = tf;
		}

		public void perform()
		{
			if (tf[0].getText().Equals(string.Empty))
			{
				Canvas.startOKDlg("Bạn chưa nhập tên");
				return;
			}
			if (tf[1].getText().Equals(string.Empty) || tf[2].getText().Equals(string.Empty))
			{
				Canvas.startOKDlg("Bạn chưa nhập mật khẩu");
				return;
			}
			if (!tf[1].getText().Equals(tf[2].getText()))
			{
				Canvas.startOKDlg("Hai mật khẩu không giống nhau");
				return;
			}
			Canvas.currentFace = null;
			GlobalService.gI().doRegisterByEmail(tf[0].getText(), tf[1].getText(), tf[3].getText());
		}
	}

	public static MiniMap me;

	public static FrameImage imgMap;

	public static FrameImage imgArrow;

	public sbyte[] map;

	public MyVector listPos;

	public sbyte wMini;

	public sbyte hMini;

	public sbyte wSmall = 16;

	public int x;

	public int y;

	public static Image imgSmallIcon;

	public static Image imgBackIcon;

	public new int selected;

	public static float cmtoX;

	public static float cmx;

	public static float cmdx;

	public static float cmvx;

	public static float cmxLim;

	public static float cmtoY;

	public static float cmy;

	public static float cmdy;

	public static float cmvy;

	public static float cmyLim;

	public MyScreen lastScr;

	public IAction cmdUpdateKey;

	private bool trans;

	private new bool isHide;

	public static bool isCityMap = false;

	public static bool isChange = true;

	private Image[] imgClound;

	private static MyVector listClound = new MyVector();

	public static FrameImage imgPopup;

	public static FrameImage imgPopupName;

	public static string nameSV = string.Empty;

	private Command cmdCenter;

	public static Image imgCreateMap;

	private int vY;

	private int vX;

	private float pa;

	private float pb;

	public bool ableTrans;

	private int dyTran;

	private int dxTran;

	private long timePointY;

	private long count;

	public static IAction actionReg;

	public static byte iRequestReg;

	public MiniMap()
	{
		imgArrow = new FrameImage(Image.createImagePNG(T.getPath() + "/main/up"), 13 * AvMain.hd, 11 * AvMain.hd);
		imgSmallIcon = Image.createImage(T.getPath() + "/effect/sIc");
		imgBackIcon = Image.createImagePNG(T.getPath() + "/effect/b_p");
		imgPopupName = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/minimapbanner"), 85 * AvMain.hd, 35 * AvMain.hd);
		imgClound = new Image[2];
		for (int i = 0; i < 2; i++)
		{
			imgClound[i] = Image.createImagePNG(T.getPath() + "/effect/clMini" + i);
		}
	}

	public static MiniMap gI()
	{
		return (me != null) ? me : (me = new MiniMap());
	}

	public override void switchToMe()
	{
		base.switchToMe();
		isHide = true;
	}

	public void switchToMe(MyScreen last)
	{
		lastScr = Canvas.currentMyScreen;
		base.switchToMe();
		if (!GlobalLogicHandler.isNewVersion)
		{
			Canvas.endDlg();
		}
		if (Session_ME.gI().isConnected() && Canvas.isInitChar)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initMiniMap();
		}
		if (Canvas.load == 0)
		{
			Canvas.load = 1;
		}
		initZoom();
		LoadMap.TYPEMAP = -1;
		Canvas.currentEffect.removeAllElements();
		FarmScr.cell = null;
		SoundManager.playSoundBG(85);
		Canvas.setPopupTime(nameSV);
		MapScr.idMapOld = -1;
	}

	public override void initZoom()
	{
		init();
		tran();
	}

	public override void commandTab(int index)
	{
		if (index == 1)
		{
			MapScr.gI().switchToMe();
			imgPopup = null;
		}
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			GlobalService.gI().requestService(6, string.Empty);
			break;
		case 1:
			MapScr.gI().doChangePass();
			break;
		case 3:
			GlobalService.gI().requestService(3, null);
			break;
		case 4:
			OptionScr.gI().switchToMe();
			break;
		case 5:
			MapScr.gI().exitGame();
			ServerListScr.gI().switchToMe();
			break;
		case 2:
			break;
		}
	}

	public override void doMenu()
	{
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Đăng ký", actionReg));
			Command o = new Command(T.info, 0, this);
			Command o2 = new Command(T.changePass, 1, this);
			myVector.addElement(o);
			myVector.addElement(o2);
			Command o3 = new Command(T.otherGame, 3, this);
			Command o4 = new Command(T.option, 4, this);
			myVector.addElement(o3);
			myVector.addElement(o4);
			myVector.addElement(new Command(T.chooseAnotherCity, 5, this));
			if (Canvas.currentMyScreen != PopupShop.gI())
			{
				MenuCenter.gI().startAt(myVector);
			}
		}
	}

	public override void close()
	{
		if (!isCityMap && Canvas.currentMyScreen != ServerListScr.me)
		{
			MapScr.gI().switchToMe();
			imgPopup = null;
		}
		else
		{
			MapScr.gI().doExitGame();
		}
	}

	public void setInfo(FrameImage img, sbyte[] map, MyVector pos, sbyte wMn, int wsmall, Command cmdCenter)
	{
		AvatarData.getImgIcon(839);
		GameMidlet.avatar.ableShow = false;
		wSmall = (sbyte)wsmall;
		this.map = map;
		listPos = pos;
		wMini = wMn;
		this.cmdCenter = cmdCenter;
		if (Canvas.stypeInt == 0)
		{
			center = cmdCenter;
		}
		hMini = (sbyte)(map.Length / wMini);
		right = null;
		init();
		cmdUpdateKey = null;
		listClound.removeAllElements();
		for (int i = 0; i < 7; i++)
		{
			listClound.addElement(new AvPosition(i * (wMini * wSmall) / 10 + 50, CRes.rnd(10) * (hMini * wSmall / 10) + 20, CRes.rnd(2)));
		}
		cmtoY = (cmy = (cmx = (cmtoX = (selected = 0))));
		tran();
		if (isCityMap)
		{
			imgPopup = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/k"), 40 * AvMain.hd, 40 * AvMain.hd);
		}
		imgMap = img;
	}

	public void init()
	{
		x = (Canvas.w - wMini * wSmall) / 2;
		y = (Canvas.hCan - ((AvMain.hFillTab == 0) ? Canvas.hTab : AvMain.hFillTab) - hMini * wSmall) / 2;
		if (x < 0)
		{
			x = 0;
		}
		if (y < 0)
		{
			y = 0;
		}
		float num = wSmall;
		cmxLim = wMini * wSmall - Canvas.w;
		cmyLim = hMini * wSmall - Canvas.hCan;
		if (cmxLim < 0f)
		{
			cmxLim = (cmx = 0f);
		}
		if (cmyLim < 0f)
		{
			cmyLim = (cmy = 0f);
		}
	}

	public override void update()
	{
		for (int i = 0; i < listPos.size(); i++)
		{
			PositionMap positionMap = (PositionMap)listPos.elementAt(i);
			if (i != selected)
			{
				positionMap.count++;
				if (positionMap.count >= 18)
				{
					positionMap.count = 0;
				}
			}
		}
		if (vY != 0)
		{
			if (cmy < 0f || cmy > cmyLim)
			{
				vY -= vY / 4;
				cmy += vY / 20;
				if (vY / 10 <= 1)
				{
					vY = 0;
				}
			}
			cmy += vY / 20;
			cmtoY = cmy;
			vY -= vY / 20;
			if (vY / 10 == 0)
			{
				vY = 0;
			}
		}
		if (cmy < 0f)
		{
			cmtoY = 0f;
			vY = 0;
		}
		else if (cmy > cmyLim)
		{
			cmtoY = cmyLim;
			vY = 0;
		}
		if (vX != 0)
		{
			if (cmx < 0f || cmx > cmxLim)
			{
				vX -= vX / 4;
				cmx += vX / 20;
				if (vX / 10 <= 1)
				{
					vX = 0;
				}
			}
			cmx += vX / 20;
			vX -= vX / 20;
			cmtoX = cmx;
			if (vX / 10 == 0)
			{
				vX = 0;
			}
		}
		if (cmx < 0f)
		{
			cmtoX = 0f;
			vX = 0;
		}
		else if (cmx > cmxLim)
		{
			cmtoX = cmxLim;
			vX = 0;
		}
		if (!Canvas.isZoom)
		{
			if (cmy != cmtoY)
			{
				cmvy = (int)(cmtoY - cmy) << 2;
				cmdy += cmvy;
				cmy += (int)cmdy >> 4;
				cmdy = (int)cmdy & 0xF;
			}
			if (cmx != cmtoX)
			{
				cmvx = (int)(cmtoX - cmx) << 2;
				cmdx += cmvx;
				cmx += (int)cmdx >> 4;
				cmdx = (int)cmdx & 0xF;
			}
		}
		if (cmtoY < 0f || cmy < 0f)
		{
			cmtoY = (cmy = 0f);
		}
		if (cmtoY > cmyLim || cmy > cmyLim)
		{
			cmtoY = (cmy = cmyLim);
		}
		if (cmtoX < 0f || cmx < 0f)
		{
			cmtoX = (cmx = 0f);
		}
		if (cmtoX > cmxLim || cmx > cmxLim)
		{
			cmtoX = (cmx = cmxLim);
		}
		for (int j = 0; j < listClound.size(); j++)
		{
			AvPosition avPosition = (AvPosition)listClound.elementAt(j);
			avPosition.x -= avPosition.anchor + ((Canvas.gameTick % 5 == 1) ? 1 : 0);
			if (avPosition.x < -x - 50)
			{
				avPosition.x = x + CRes.rnd(4) * 50 + wMini * wSmall;
				avPosition.y = CRes.rnd(10) * (hMini * wSmall / 10) + 10;
				avPosition.anchor = CRes.rnd(2);
			}
		}
	}

	public override void updateKey()
	{
		count++;
		base.updateKey();
		if (Canvas.isPointer(0, 0, Canvas.w, Canvas.h))
		{
			int num = Canvas.dx();
			int num2 = Canvas.dy();
			if (Canvas.isPointerClick)
			{
				ableTrans = true;
				Canvas.isPointerClick = false;
				for (int i = 0; i < listPos.size(); i++)
				{
					PositionMap positionMap = (PositionMap)listPos.elementAt(i);
					if (Canvas.isPointer((int)((float)(x + positionMap.x * wSmall + wSmall / 2) - cmx - (float)(48 * AvMain.hd / 2)), (int)((float)(y + positionMap.y * wSmall + wSmall / 2) - cmy - (float)(56 * AvMain.hd)), 48 * AvMain.hd, 56 * AvMain.hd))
					{
						selected = i;
						isHide = false;
						return;
					}
				}
			}
			if (ableTrans && Canvas.isPointerDown)
			{
				if (CRes.abs(num) >= 20 && CRes.abs(num2) >= 20)
				{
					isHide = true;
				}
				if (Canvas.gameTick % 3 == 0)
				{
					dyTran = Canvas.py;
					dxTran = Canvas.px;
					timePointY = count;
				}
				vY = 0;
				vX = 0;
				if (!trans)
				{
					trans = true;
					pa = cmx;
					pb = cmy;
				}
				cmtoY = pb + (float)num2;
				cmtoX = pa + (float)num;
				setLimit();
				cmy = cmtoY;
				cmx = cmtoX;
			}
			if (ableTrans && Canvas.isPointerRelease)
			{
				int num3 = (int)(count - timePointY);
				int num4 = dyTran - Canvas.py;
				if (num3 < 10)
				{
					if (cmtoY >= 0f && cmtoY < cmyLim)
					{
						vY = num4 / num3 * 10;
					}
					int num5 = dxTran - Canvas.px;
					if (cmtoX >= 0f && cmtoX < cmxLim)
					{
						vX = num5 / num3 * 10;
					}
				}
				timePointY = -1L;
				trans = false;
				ableTrans = false;
				if (CRes.abs(num) < 20 && CRes.abs(num2) < 20)
				{
					PositionMap positionMap2 = (PositionMap)listPos.elementAt(selected);
					if (Canvas.isPointer((int)((float)(x + positionMap2.x * wSmall + wSmall / 2) - cmx - (float)(48 * AvMain.hd / 2)), (int)((float)(y + positionMap2.y * wSmall + wSmall / 2) - cmy - (float)(56 * AvMain.hd)), 48 * AvMain.hd, 56 * AvMain.hd))
					{
						cmdCenter.perform();
						return;
					}
				}
			}
		}
		if (cmdUpdateKey != null)
		{
			cmdUpdateKey.perform();
		}
	}

	private void tran()
	{
		PositionMap positionMap = (PositionMap)listPos.elementAt(selected);
		cmtoX = positionMap.x * wSmall - Canvas.w / 2;
		cmtoY = positionMap.y * wSmall - Canvas.hCan / 2;
		setLimit();
		if (Canvas.isZoom)
		{
			cmx = cmtoX;
			cmy = cmtoY;
		}
	}

	private void setLimit()
	{
		if (cmtoY < 0f)
		{
			cmtoY = 0f;
		}
		if (cmtoY > cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (cmtoX < 0f)
		{
			cmtoX = 0f;
		}
		if (cmtoX > cmxLim)
		{
			cmtoX = cmxLim;
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			base.paint(g);
		}
		Canvas.paintPlus(g);
	}

	public override void paintMain(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.translate(x, y);
		g.translate(0f - cmx, 0f - cmy);
		if (imgCreateMap != null)
		{
			g.drawImage(imgCreateMap, 0f, 0f, 0);
		}
		else
		{
			for (int i = 0; i < map.Length; i++)
			{
				imgMap.drawFrameXY(map[i], i % wMini * wSmall, i / wMini * wSmall, 0, g);
			}
		}
		for (int j = 0; j < listPos.size(); j++)
		{
			PositionMap positionMap = (PositionMap)listPos.elementAt(j);
			if (j == selected && !isHide)
			{
				g.drawImage(imgBackIcon, positionMap.x * wSmall + wSmall / 2, positionMap.y * wSmall, 33);
				if (isCityMap)
				{
					imgPopup.drawFrame(j, positionMap.x * wSmall + wSmall / 2, positionMap.y * wSmall - 12 * AvMain.hd, 0, 33, g);
				}
				else
				{
					AvatarData.paintImg(g, positionMap.idImg, positionMap.x * wSmall + wSmall / 2, positionMap.y * wSmall - 12 * AvMain.hd, 33);
				}
			}
			else
			{
				g.drawImage(imgSmallIcon, positionMap.x * wSmall + wSmall / 2, positionMap.y * wSmall - positionMap.count / 3, 33);
			}
		}
		paintName(g);
		paintClound(g);
	}

	private void paintName(MyGraphics g)
	{
		for (int i = 0; i < listPos.size(); i++)
		{
			PositionMap positionMap = (PositionMap)listPos.elementAt(i);
			float num = positionMap.x * wSmall;
			float num2 = positionMap.y * wSmall;
			if (num2 < cmy + 50f)
			{
				num2 = cmy + 50f;
			}
			if (num2 > cmy + (float)Canvas.hCan - 20f)
			{
				num2 = cmy + (float)Canvas.hCan - 20f;
			}
			if (num < cmx + 20f)
			{
				num = cmx + 20f;
			}
			if (num > cmx + (float)Canvas.w - 47f)
			{
				num = cmx + (float)Canvas.w - 47f;
			}
			Canvas.borderFont.drawString(g, positionMap.text, (int)num + 10, (int)num2 - ((i != selected || isHide) ? (35 * AvMain.hd) : (70 * AvMain.hd)) - positionMap.count / 6, 2);
		}
	}

	private void paintClound(MyGraphics g)
	{
		int num = listClound.size();
		for (int i = 0; i < num; i++)
		{
			AvPosition avPosition = (AvPosition)listClound.elementAt(i);
			if ((float)avPosition.x > cmx - 30f && (float)avPosition.x < cmx + 30f + (float)Canvas.w && (float)avPosition.y > cmy - 20f && (float)avPosition.y < cmy + 20f + (float)Canvas.h)
			{
				g.drawImage(imgClound[avPosition.anchor], avPosition.x, avPosition.y, 3);
			}
		}
	}

	public void onRegisterByEmail(sbyte step, string des, string name, string pass)
	{
		Out.println("onRegisterByEmail: " + name + "   " + pass);
		if (step == 0)
		{
			actionReg = new IActionRequestReg(des);
		}
		else if (step == 1)
		{
			actionReg = new IActionRequestOK(des);
		}
		else if (step == 2)
		{
			LoginScr.gI().tfUser.setText(name);
			LoginScr.gI().tfPass.setText(pass);
			LoginScr.gI().saveLogin();
			Canvas.startOKDlg("Đăng ký thành công.");
			actionReg = null;
		}
	}
}
