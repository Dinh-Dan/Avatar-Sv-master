public class Welcome : AvMain
{
	private class IActionLeft : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.usureStop, new IActionLeft1());
		}
	}

	private class IActionLeft1 : IAction
	{
		public void perform()
		{
			Canvas.isInitChar = false;
			Canvas.welcome = null;
			AvCamera.isFollow = false;
			MapScr.gI().initCmd();
			Canvas.endDlg();
		}
	}

	private class IActionClick : IAction
	{
		public void perform()
		{
			Canvas.welcome.click();
		}
	}

	private int x;

	private int y;

	private int wPopup;

	private int next;

	private string[][] chats;

	public static int index = 0;

	public byte num;

	public static MyScreen lastScr;

	public static int indexFish = 0;

	private string[][] textFish;

	public static int indexShop = 0;

	private string[][] textShop;

	public static bool isPaintArrow = false;

	private static int indexMiniMap = 0;

	private string[][] textMiniMap;

	public static int indexMapScr = 0;

	private static short[] posArrayPopupX;

	private static short[] posArrayPopupY;

	private string[][] textMapScr;

	public static int indexFarmPath = 0;

	private string[][] textFarmPath;

	public static sbyte[] joinOrder;

	public static int indexFarm = 0;

	public static int wText;

	private string[][] textFarm;

	public static bool isOut = false;

	public static byte[] indexWelcomeMiniMap = new byte[5] { 3, 7, 4, 1, 5 };

	private string[][] textKhuMuaSam;

	private static int indexKhuMuaSam = 0;

	private SubObject subPath;

	private string[][] textTask;

	public static int indexTask = 0;

	public static int indexCasino = 0;

	private string[][] textCasino;

	public AvPosition posPopup;

	public Welcome()
	{
		isOut = false;
		isPaintArrow = true;
		x = 10;
		next = 0;
		center = new Command("Tiếp", new IActionClick());
		left = new Command(T.close, new IActionLeft());
		wText = (int)(ScaleGUI.WIDTH - (float)(x * 2) - (float)(100 * AvMain.hd));
	}

	public void update()
	{
	}

	public override void updateKey()
	{
		if (isPaintArrow)
		{
			Canvas.paint.setDrawPointer(left, center, right);
			Canvas.paint.setBack();
			base.updateKey();
		}
		if (isPaintArrow && lastScr == Canvas.currentMyScreen && Canvas.menuMain == null && Canvas.currentDialog == null && chats != null)
		{
			bool[] keyHold = Canvas.keyHold;
			bool[] keyHold2 = Canvas.keyHold;
			bool flag;
			Canvas.keyHold[6] = (flag = (Canvas.keyHold[8] = false));
			keyHold2[4] = (flag = flag);
			keyHold[2] = flag;
		}
	}

	private void click()
	{
		if (next < chats.Length - 1)
		{
			next++;
			isPaintArrow = true;
			setNext();
			if (LoadMap.TYPEMAP == 23)
			{
				if (indexKhuMuaSam == 1 && next == chats.Length - 1)
				{
					AvCamera.gI().setToPos(posArrayPopupX[0] * AvMain.hd, 20 * AvMain.hd);
					AvCamera.isFollow = true;
					SubObject o = new SubObject(-9, posArrayPopupX[indexKhuMuaSam - 1], 50, 20);
					LoadMap.treeLists.addElement(o);
					LoadMap.orderVector(LoadMap.treeLists);
				}
			}
			else if (LoadMap.TYPEMAP == 9 && indexMapScr == 1 && next == chats.Length - 1)
			{
				initMapScr();
			}
		}
		else
		{
			if (next != chats.Length - 1)
			{
				return;
			}
			AvCamera.isFollow = false;
			if (LoadMap.TYPEMAP == 100)
			{
				Canvas.welcome = null;
				return;
			}
			if (Canvas.currentMyScreen == MiniMap.me && textMiniMap != null && indexMiniMap == textMiniMap.Length)
			{
				initMiniMap();
				return;
			}
			if (LoadMap.TYPEMAP == 24)
			{
				if (indexFarm == 3 || indexFarm == 4 || indexFarm == 5 || indexFarm == 6)
				{
					removeArrow();
					Canvas.welcome = new Welcome();
					Canvas.welcome.initFarm();
					isPaintArrow = true;
					return;
				}
				if (indexFarm == 7 && isPaintArrow && !isOut)
				{
					close(FarmScr.posName.x + 27 + 50, 178);
					return;
				}
			}
			else if (LoadMap.TYPEMAP == 25)
			{
				if (indexFarmPath == textFarmPath.Length - 1)
				{
					Canvas.welcome = null;
				}
			}
			else if (LoadMap.TYPEMAP == 13)
			{
				next = 0;
				if (!isOut)
				{
					initFish();
					return;
				}
			}
			y = Canvas.transTab + 5;
			isPaintArrow = false;
		}
	}

	private void setNext()
	{
		wPopup = chats[next].Length * AvMain.hBlack + AvMain.hDuBox * 2;
		if (wPopup < AvMain.hBlack * 2 + AvMain.hDuBox * 2)
		{
			wPopup = AvMain.hBlack * 2 + AvMain.hDuBox * 2;
		}
		y = 5;
	}

	public override void paint(MyGraphics g)
	{
		if (lastScr != Canvas.currentMyScreen || Canvas.currentDialog != null || Canvas.menuMain != null)
		{
			return;
		}
		Canvas.resetTrans(g);
		g.translate(0f, Canvas.countTab);
		if (isPaintArrow || Canvas.gameTick % 20 > 2)
		{
			ChatPopup.paintRoundRect(g, x, y, Canvas.w - x * 2, wPopup, 16777215, 13940870, 0);
			if (chats != null && chats[next] != null)
			{
				int num = 0;
				if (chats[next].Length == 1)
				{
					num = 2;
				}
				for (int i = 0; i < chats[next].Length; i++)
				{
					Canvas.blackF.drawString(g, chats[next][i], x + (Canvas.w - x * 2) / 2, y + wPopup / 2 - chats[next].Length * AvMain.hBlack / 2 + i * AvMain.hBlack - num, 2);
				}
				this.num++;
				if (this.num >= 8)
				{
					this.num = 0;
				}
				if (Canvas.currentMyScreen == MiniMap.me)
				{
					g.translate((int)(0f - MiniMap.cmx + (float)MiniMap.gI().x), (int)(0f - MiniMap.cmy + (float)MiniMap.gI().y));
				}
				else
				{
					g.translate(-(int)AvCamera.gI().xCam, -(int)AvCamera.gI().yCam);
				}
			}
		}
		if (isPaintArrow)
		{
			base.paint(g);
		}
	}

	public void initMiniMap()
	{
		if (indexMiniMap == indexWelcomeMiniMap.Length + 1)
		{
			Canvas.welcome = null;
			Canvas.isInitChar = false;
			return;
		}
		if (textMiniMap == null)
		{
			textMiniMap = Canvas.t.getTextMiniMap();
		}
		lastScr = MiniMap.me;
		isPaintArrow = true;
		if (indexMiniMap < indexWelcomeMiniMap.Length)
		{
			MiniMap.gI().selected = indexWelcomeMiniMap[indexMiniMap];
		}
		Canvas.welcome.setText(textMiniMap[indexMiniMap]);
		indexMiniMap++;
	}

	public void setText(string[] text)
	{
		chats = new string[text.Length][];
		for (int i = 0; i < chats.Length; i++)
		{
			chats[i] = Canvas.blackF.splitFontBStrInLine(text[i], wText);
		}
		setNext();
		isPaintArrow = true;
	}

	public void initMapScr()
	{
		if (textMapScr == null)
		{
			textMapScr = Canvas.t.getTextMapScr();
		}
		lastScr = MapScr.instance;
		posArrayPopupX = new short[3];
		posArrayPopupX[0] = 180;
		posArrayPopupX[1] = 312;
		posArrayPopupX[2] = 720;
		joinOrder = new sbyte[3] { 10, 100, 107 };
		if (indexMapScr != 0)
		{
			if (indexMapScr == posArrayPopupX.Length)
			{
				close(288, 150);
				return;
			}
			AvCamera.gI().setToPos(posArrayPopupX[indexMapScr] * AvMain.hd, 20 * AvMain.hd);
			AvCamera.isFollow = true;
		}
		if (indexMapScr != 0)
		{
			SubObject o = new SubObject(-9, posArrayPopupX[indexMapScr], 50, 20);
			LoadMap.treeLists.addElement(o);
			LoadMap.orderVector(LoadMap.treeLists);
		}
		Canvas.welcome.setText(textMapScr[indexMapScr]);
		indexMapScr++;
	}

	public void initKhuMuaSam()
	{
		if (textKhuMuaSam == null)
		{
			textKhuMuaSam = Canvas.t.getTextMuaSam();
		}
		lastScr = MapScr.instance;
		posArrayPopupX = new short[3];
		posArrayPopupX[0] = 865;
		posArrayPopupX[1] = 445;
		posArrayPopupX[2] = 95;
		joinOrder = new sbyte[5] { 57, 104, 58, 100, 107 };
		if (indexKhuMuaSam != 0)
		{
			if (indexKhuMuaSam == posArrayPopupX.Length)
			{
				close(640, 150);
				return;
			}
			AvCamera.gI().setToPos(posArrayPopupX[indexKhuMuaSam] * AvMain.hd, 20 * AvMain.hd);
			AvCamera.isFollow = true;
			SubObject o = new SubObject(-9, posArrayPopupX[indexKhuMuaSam], 50, 20);
			LoadMap.treeLists.addElement(o);
			LoadMap.orderVector(LoadMap.treeLists);
		}
		Canvas.welcome.setText(textKhuMuaSam[indexKhuMuaSam]);
		indexKhuMuaSam++;
	}

	public bool isJoinMapScr(int pos)
	{
		if (isOut)
		{
			return true;
		}
		switch (LoadMap.TYPEMAP)
		{
		case 23:
			if (indexKhuMuaSam - 1 < joinOrder.Length && pos == joinOrder[indexKhuMuaSam - 1])
			{
				return true;
			}
			break;
		case 9:
			if (indexMapScr - 1 < joinOrder.Length && pos == joinOrder[indexMapScr - 1])
			{
				return true;
			}
			break;
		case 25:
			if (indexFarmPath <= joinOrder.Length && pos == joinOrder[indexFarmPath - 1])
			{
				return true;
			}
			break;
		case 57:
			if (indexShop <= joinOrder.Length && pos == joinOrder[indexShop - 1])
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void initFarmPath(MyScreen last)
	{
		if (textFarmPath == null)
		{
			textFarmPath = Canvas.t.getTextFarmPath();
		}
		lastScr = last;
		if (indexFarmPath == 0)
		{
			posArrayPopupX = new short[4] { 372, -1, -1, 220 };
			posArrayPopupY = new short[4] { 25, -1, -1, 25 };
			joinOrder = new sbyte[4] { 52, -1, -1, 24 };
		}
		else if (indexFarmPath == textFarmPath.Length)
		{
			close(170, 150);
			return;
		}
		if (indexFarmPath == 1)
		{
			removeArrow();
		}
		SubObject o = new SubObject(-9, posArrayPopupX[indexFarmPath], posArrayPopupY[indexFarmPath], 20);
		LoadMap.treeLists.addElement(o);
		LoadMap.orderVector(LoadMap.treeLists);
		AvCamera.gI().setToPos(posArrayPopupX[indexFarmPath] * AvMain.hd, 20 * AvMain.hd);
		AvCamera.isFollow = true;
		Canvas.welcome.setText(textFarmPath[indexFarmPath]);
		indexFarmPath++;
	}

	public void initTash()
	{
		if (textTask == null)
		{
			textTask = Canvas.t.getTextToaThiChinh();
		}
		if (indexTask == 0)
		{
		}
		Canvas.welcome.setText(textTask[indexTask]);
		indexTask++;
	}

	public void initFarm()
	{
		if (textFarm == null)
		{
			textFarm = Canvas.t.getTextFarm();
		}
		lastScr = FarmScr.instance;
		if (indexFarm == 0)
		{
			posArrayPopupX = new short[5]
			{
				(short)(FarmScr.gI().posTree[0].x * LoadMap.w + 12),
				(short)(FarmScr.posBarn.x + 12),
				(short)FarmScr.xPosCook,
				(short)FarmScr.starFruil.x,
				(short)(FarmScr.posPond.x + 12)
			};
			short[] obj = new short[5] { 36, 36, 0, 36, 36 };
			obj[2] = (short)(FarmScr.yPosCook + 15);
			posArrayPopupY = obj;
		}
		int num = indexFarm;
		if (num < 3)
		{
			num = 0;
		}
		else
		{
			switch (num)
			{
			case 3:
				num = 1;
				break;
			case 4:
				num = 2;
				break;
			case 5:
				num = 3;
				break;
			case 6:
				num = 4;
				break;
			}
		}
		if (indexFarm < 3 || indexFarm == 4 || indexFarm == 5)
		{
			SubObject subObject = null;
			subObject = new SubObject(-9, posArrayPopupX[num], posArrayPopupY[num], 20);
			LoadMap.treeLists.addElement(subObject);
			LoadMap.orderVector(LoadMap.treeLists);
		}
		AvCamera.gI().setToPos(posArrayPopupX[num] * AvMain.hd, 36 * AvMain.hd);
		AvCamera.isFollow = true;
		Canvas.welcome.setText(textFarm[indexFarm]);
		indexFarm++;
		FarmScr.gI().left = null;
	}

	public void initShop(MyScreen last)
	{
		if (textShop == null)
		{
			textShop = Canvas.t.getTextShop();
		}
		lastScr = last;
		if (indexShop == 0)
		{
			posArrayPopupX = new short[1] { 192 };
			joinOrder = new sbyte[1] { 56 };
			SubObject o = new SubObject(-9, posArrayPopupX[indexShop] + 12, 135, 20);
			LoadMap.treeLists.addElement(o);
			LoadMap.orderVector(LoadMap.treeLists);
			AvCamera.gI().setToPos(posArrayPopupX[indexShop] + 12, 130 * AvMain.hd);
		}
		else
		{
			if (indexShop == textShop.Length)
			{
				close(180, 240);
				return;
			}
			AvCamera.isFollow = true;
		}
		Canvas.welcome.setText(textShop[indexShop]);
		indexShop++;
	}

	public void initFish()
	{
		if (textFish == null)
		{
			textFish = Canvas.t.getTextFish();
		}
		lastScr = MapScr.instance;
		if (indexFish == 0)
		{
			joinOrder = new sbyte[1] { 56 };
		}
		else
		{
			if (indexFish == textFish.Length)
			{
				close(170, 170);
				return;
			}
			if (indexFish < 4)
			{
				posArrayPopupX = new short[3] { 12, 480, 230 };
				posArrayPopupY = new short[3] { 110, 110, 12 };
				AvCamera.gI().setToPos(posArrayPopupX[indexFish - 1] * AvMain.hd, posArrayPopupY[indexFish - 1] * AvMain.hd);
				AvCamera.isFollow = true;
				SubObject o = new SubObject(-9, posArrayPopupX[indexFish - 1], posArrayPopupY[indexFish - 1], 20);
				LoadMap.treeLists.addElement(o);
				LoadMap.orderVector(LoadMap.treeLists);
			}
			else
			{
				AvCamera.isFollow = false;
			}
		}
		Canvas.welcome.setText(textFish[indexFish]);
		indexFish++;
	}

	private void removeArrow()
	{
		for (int i = 0; i < LoadMap.treeLists.size(); i++)
		{
			MyObject myObject = (MyObject)LoadMap.treeLists.elementAt(i);
			if (myObject.catagory == 1 && ((SubObject)myObject).type == -9)
			{
				LoadMap.treeLists.removeElement(myObject);
				i--;
			}
		}
	}

	public void close(int x, int y)
	{
		next = 0;
		isOut = true;
		string[] array = null;
		removeArrow();
		SubObject o = new SubObject(-9, x, y, 20);
		LoadMap.treeLists.addElement(o);
		LoadMap.orderVector(LoadMap.treeLists);
		AvCamera.gI().setToPos(x * AvMain.hd, y * AvMain.hd);
		AvCamera.isFollow = true;
		array = Canvas.t.getTextOut();
		Canvas.welcome.setText(array);
	}

	public static void goFarm()
	{
		int num = indexFarm;
		if (num < 3)
		{
			num = 0;
		}
		else
		{
			switch (num)
			{
			case 3:
				num = 1;
				break;
			case 4:
				num = 2;
				break;
			}
		}
		if (num < posArrayPopupX.Length)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initFarm();
		}
	}

	public static void restart()
	{
		Canvas.isInitChar = true;
		indexFarm = 0;
		indexFarmPath = 0;
		indexFish = 0;
		indexMapScr = 0;
		indexMiniMap = 0;
		indexShop = 0;
		isOut = false;
		isPaintArrow = false;
	}

	public void initCasino()
	{
		if (textCasino == null)
		{
			textCasino = new string[1][];
			textCasino[0] = new string[2] { "Chào mừng bạn vào hội đánh cờ.", "Bạn hảy di chuyển đến chổ mũi tên để vào trò chơi." };
		}
		lastScr = MapScr.instance;
		posPopup = null;
		if (indexCasino == 0)
		{
			posArrayPopupX = new short[1] { 230 };
			joinOrder = new sbyte[1] { 72 };
			SubObject o = new SubObject(-9, posArrayPopupX[indexShop], 120, 20);
			LoadMap.treeLists.addElement(o);
			AvCamera.isFollow = true;
			LoadMap.orderVector(LoadMap.treeLists);
			AvCamera.gI().setToPos(posArrayPopupX[indexCasino] * AvMain.hd + 12, 100 * AvMain.hd);
		}
		else
		{
			if (indexCasino == textCasino.Length)
			{
				close(345, 250);
				return;
			}
			AvCamera.isFollow = true;
		}
		Canvas.welcome.setText(textCasino[indexCasino]);
		indexCasino++;
	}
}
