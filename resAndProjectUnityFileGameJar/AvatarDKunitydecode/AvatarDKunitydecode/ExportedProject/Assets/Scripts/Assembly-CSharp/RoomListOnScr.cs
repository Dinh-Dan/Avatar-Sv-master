public class RoomListOnScr : OnScreen
{
	public static RoomListOnScr instance;

	public Image imgTitleRoom;

	public static FrameImage imgRoomStat;

	public MyVector roomList = new MyVector();

	public Image imgBG;

	public static string title;

	public int dis;

	public new int selected;

	public new int hSmall;

	private Command cmdSellect;

	private Command cmdMenu;

	private Command cmdClose;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int numW;

	public int numH;

	public int y;

	public int x;

	public Image imgTab;

	public byte countClose;

	public static int index;

	public static int indexTemp;

	private int count;

	private long timeDelay;

	private int pa;

	private int vY;

	private int dyTran;

	private int timePoint;

	private bool transY;

	private bool isGO;

	private int timeOpen;

	private int pyLast;

	private bool trans;

	private bool isG;

	public RoomListOnScr()
	{
		init();
		initCmd();
		x = 20 * AvMain.hd;
		right = new Command(T.menu, 0);
		center = new Command(T.playNow, 1);
		addCmd(2, 1);
	}

	public static RoomListOnScr gI()
	{
		if (instance == null)
		{
			instance = new RoomListOnScr();
		}
		return instance;
	}

	public override void switchToMe()
	{
		base.switchToMe();
		selected = 1;
		BoardListOnScr.gI().boardList.removeAllElements();
		if (AvMain.hd > 0)
		{
			isHide = true;
		}
		init();
		Canvas.paint.clearImgAvatar();
		Canvas.loadMap.resetImg();
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 1:
			Canvas.startWaitDlg();
			CasinoService.gI().joinAnyBoard();
			break;
		case 2:
			Canvas.startWaitDlg();
			CasinoService.gI().requestRoomList();
			break;
		case 3:
			Canvas.startWaitDlg();
			GlobalService.gI().requestInfoOf(GameMidlet.avatar.IDDB);
			break;
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(T.playNow, 1, this));
			myVector.addElement(new Command(T.updateList, 2, this));
			if (AvMain.hd == 0)
			{
				myVector.addElement(MapScr.gI().cmdEvent);
			}
			myVector.addElement(new Command(T.viewMyInfo, 3, this));
			MenuCenter.gI().startAt(myVector);
			break;
		}
		case 1:
			doSelectRoom();
			break;
		case 2:
			GlobalService.gI().getHandler(9);
			Canvas.startWaitDlg();
			break;
		}
	}

	public void initCmd()
	{
		cmdMenu = new Command(T.menu, 0);
		cmdSellect = new Command(T.selectt, 1);
		cmdClose = new Command(T.close, 2);
		cmdClose.indexImage = 1;
	}

	public static void setName(int index, BoardScr board)
	{
		if (!onMainMenu.isOngame)
		{
			title = T.nameCasino[index];
		}
		else
		{
			title = T.nameMenuOn[index];
		}
		CasinoMsgHandler.curScr = board;
	}

	public override void closeTabAll()
	{
		commandTab(2);
	}

	public override void doMenuTab()
	{
		if (left != null)
		{
			left.perform();
		}
	}

	public void init()
	{
		hSmall = MyScreen.hText;
		dis = Canvas.hCan - Canvas.hTab / 3 * 2;
		if (roomList != null && hSmall != 0)
		{
			int num = roomList.size();
			cmyLim = (num + 50 * AvMain.hd / hSmall + 1) * hSmall - dis;
			if (cmyLim < 0)
			{
				cmyLim = 0;
			}
			y = 50 * AvMain.hd;
		}
	}

	public override void initTabTrans()
	{
		init();
	}

	protected void doSelectRoom()
	{
		sbyte id = ((RoomInfo)roomList.elementAt(selected)).id;
		if (id != -1)
		{
			CasinoService.gI().requestBoardList(id);
			Canvas.load = 0;
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		base.paint(g);
		Canvas.paintPlus2(g);
	}

	public override void paintMain(MyGraphics g)
	{
		Canvas.paint.paintDefaultBg(g);
		onMainMenu.paintTitle(g, T.room + title, Canvas.w / 2, 30 * AvMain.hd - cmy);
		paintRoomList(g);
	}

	private void paintRoomList(MyGraphics g)
	{
		Canvas.resetTrans(g);
		if (roomList == null || roomList.size() == 0 || Canvas.load != -1)
		{
			return;
		}
		g.translate(0f, y);
		g.translate(0f, -cmy);
		int num = 4;
		int num2 = (hSmall - AvMain.hBorder) / 2;
		int num3 = cmy / hSmall - 1;
		if (num3 < 0)
		{
			num3 = 0;
		}
		int num4 = num3 + Canvas.hCan / hSmall + 2;
		if (num4 > roomList.size())
		{
			num4 = roomList.size();
		}
		num += num3 * hSmall;
		for (int i = num3; i < num4 && i < roomList.size(); i++)
		{
			RoomInfo roomInfo = (RoomInfo)roomList.elementAt(i);
			if (!isHide && i == selected && roomInfo.id != -1)
			{
				Canvas.paint.paintSelect(g, x + 50 * AvMain.hd + 1, num + 1, Canvas.w - x * 2 - 50 * AvMain.hd, hSmall - 2);
			}
			if (roomInfo.id == -1)
			{
				g.drawImage(imgTitleRoom, x, num + hSmall / 2 - 12 * AvMain.hd, 0);
				Canvas.menuFont.drawString(g, T.roomLevelText[roomInfo.lv], x + 30 * AvMain.hd, num + hSmall / 2 - Canvas.menuFont.getHeight() / 2, 0);
				num += hSmall;
				continue;
			}
			g.setColor(15196756);
			g.drawRect(x, num, Canvas.w - x * 2, hSmall);
			g.fillRect(x + 50 * AvMain.hd + 1, num, 1f, hSmall);
			g.setColor(7607603);
			g.fillRect(x + 1, num + 1, 50 * AvMain.hd, hSmall - 1);
			Canvas.normalWhiteFont.drawString(g, roomInfo.id + string.Empty, x + 25 * AvMain.hd, num + hSmall / 2 - Canvas.normalWhiteFont.getHeight() / 2, 2);
			imgRoomStat.drawFrame(roomInfo.roomFree, x + (Canvas.w - x * 2 - imgRoomStat.frameWidth / 2) - (hSmall - imgRoomStat.frameHeight) / 2, num + hSmall / 2, 0, 3, g);
			num += hSmall;
		}
	}

	public void setRoomList(MyVector roomList)
	{
		for (int i = 0; i < roomList.size(); i++)
		{
			RoomInfo roomInfo = (RoomInfo)roomList.elementAt(i);
			for (int j = i; j < roomList.size(); j++)
			{
				RoomInfo roomInfo2 = (RoomInfo)roomList.elementAt(j);
				if (roomInfo2.lv < roomInfo.lv)
				{
					roomList.setElementAt(roomInfo, j);
					roomList.setElementAt(roomInfo2, i);
					roomInfo = roomInfo2;
				}
			}
		}
		this.roomList = new MyVector();
		int num = -1;
		for (int k = 0; k < roomList.size(); k++)
		{
			RoomInfo roomInfo3 = (RoomInfo)roomList.elementAt(k);
			if (num == -1 || roomInfo3.lv != num)
			{
				this.roomList.addElement(new RoomInfo(-1, 0, 0, roomInfo3.lv));
			}
			this.roomList.addElement(roomInfo3);
			num = roomInfo3.lv;
		}
		selected = 1;
		init();
	}

	public override void updateKey()
	{
		base.updateKey();
		count++;
		if (Canvas.isPointerClick)
		{
			pyLast = Canvas.pyLast;
			isG = false;
			if (Canvas.isPoint(0, 0, Canvas.w, dis))
			{
				if (vY != 0)
				{
					isG = true;
				}
				pa = cmtoY;
				timeDelay = count;
				trans = true;
			}
			Canvas.isPointerClick = false;
		}
		if (!trans)
		{
			return;
		}
		int num = pyLast - Canvas.py;
		pyLast = Canvas.py;
		long num2 = count - timeDelay;
		if (Canvas.isPointerDown)
		{
			if (count % 2 == 0)
			{
				dyTran = Canvas.py;
				timePoint = count;
			}
			vY = 0;
			if (Math.abs(num) < 10 * AvMain.hd)
			{
				int num3 = (Canvas.py - 50 * AvMain.hd + cmtoY) / hSmall;
				if (num3 >= 0 && num3 < roomList.size())
				{
					selected = num3;
				}
			}
			if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
			{
				isHide = true;
			}
			else if (num2 > 3 && num2 < 8)
			{
				int num4 = (cmtoY + Canvas.py - 50 * AvMain.hd) / hSmall;
				if (num4 >= 0 && num4 < roomList.size() && !isG)
				{
					isHide = false;
				}
			}
			if (cmtoY < 0 || cmtoY > cmyLim)
			{
				cmtoY = pa + num / 2;
				pa = cmtoY;
			}
			else
			{
				cmtoY = pa + num / 2;
				pa = cmtoY;
			}
			cmy = cmtoY;
		}
		if (!Canvas.isPointerRelease || !Canvas.isPoint(0, 0, Canvas.w, dis))
		{
			return;
		}
		isG = false;
		int num5 = count - timePoint;
		int num6 = dyTran - Canvas.py;
		if (CRes.abs(num6) > 40 && num5 < 10 && cmtoY > 0 && cmtoY < cmyLim)
		{
			vY = num6 / num5 * 10;
		}
		timePoint = -1;
		if (Math.abs(num) < 10 * AvMain.hd)
		{
			if (num2 <= 4)
			{
				isHide = false;
				timeOpen = 5;
			}
			else if (!isHide)
			{
				click();
			}
		}
		trans = false;
		Canvas.isPointerRelease = false;
	}

	private void click()
	{
		doSelectRoom();
	}

	public override void update()
	{
		if (timeOpen > 0)
		{
			timeOpen--;
			if (timeOpen == 0)
			{
				click();
			}
		}
		moveCamera();
	}

	public void moveCamera()
	{
		if (vY != 0)
		{
			if (cmy < 0 || cmy > cmyLim)
			{
				vY -= vY / 4;
				cmy += vY / 20;
				if (vY / 10 <= 1)
				{
					vY = 0;
				}
			}
			if (cmy < 0)
			{
				if (cmy < -dis / 2)
				{
					cmy = -dis / 2;
					cmtoY = 0;
					vY = 0;
				}
			}
			else if (cmy > cmyLim)
			{
				if (cmy < cmyLim + dis / 2)
				{
					cmy = cmyLim + dis / 2;
					cmtoY = cmyLim;
					vY = 0;
				}
			}
			else
			{
				cmy += vY / 10;
			}
			cmtoY = cmy;
			vY -= vY / 10;
			if (vY / 10 == 0)
			{
				vY = 0;
			}
		}
		else if (cmy < 0)
		{
			cmtoY = 0;
		}
		else if (cmy > cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (cmy != cmtoY)
		{
			cmvy = cmtoY - cmy << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
	}
}
