public class BoardListOnScr : OnScreen
{
	private class IActionBL1 : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			CasinoService.gI().joinAnyBoard();
		}
	}

	private class IActionBL3 : IAction
	{
		private BoardListOnScr p;

		public IActionBL3(BoardListOnScr p)
		{
			this.p = p;
		}

		public void perform()
		{
			p.doAskForBoardToGo();
		}
	}

	private class IActionBL4 : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			GlobalService.gI().requestInfoOf(GameMidlet.avatar.IDDB);
		}
	}

	private class IActionToGo : IKbAction
	{
		private BoardListOnScr bscr;

		public IActionToGo(BoardListOnScr b)
		{
			bscr = b;
		}

		public void perform(string text)
		{
			me.boardIDToGo = int.Parse(text);
			me.doAskForPass();
		}
	}

	private class IActionPass : IKbAction
	{
		public void perform(string text)
		{
			CasinoService.gI().joinBoard(me.roomID, (sbyte)me.boardIDToGo, text);
			Canvas.endDlg();
		}
	}

	private class IActionXeng : IAction
	{
		public void perform()
		{
			TransMoneyDlg.gI().show();
		}
	}

	private class IActionJoinBoard : IKbAction
	{
		public void perform(string text)
		{
			BoardInfo boardInfo = (BoardInfo)me.boardList.elementAt(me.selected);
			CasinoService.gI().joinBoard(me.roomID, boardInfo.boardID, text);
			Canvas.load = 0;
		}
	}

	public static BoardListOnScr me;

	public static sbyte STYLE_2PLAYER = 0;

	public static sbyte STYLE_4PLAYER = 1;

	public static sbyte STYLE_5PLAYER = 2;

	public static sbyte type = STYLE_4PLAYER;

	public static FrameImage imgBoard;

	public static Image imgSoBan;

	public static Image imgLock;

	public Image imgTitleBoard;

	public Image imgNumPlayer;

	public Image imgPlaying;

	private int nBoardPerLine;

	private int defX;

	public MyVector boardList = new MyVector();

	public int xStart;

	public int yStart;

	public int countPaint;

	public sbyte roomID;

	private short wSmall;

	private Command cmdSellect;

	private Command cmdMenu;

	private Command cmdClose;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public static Image imgSelectBoard;

	private int boardIDToGo;

	private int dis;

	private long timeDelay;

	private int pa;

	private int vY;

	private int dyTran;

	private int timePoint;

	private int count;

	private bool transY;

	private bool isGO;

	private int timeOpen;

	private int pyLast;

	private bool trans;

	private bool isG;

	public BoardListOnScr()
	{
		defX = 0;
		cmdSellect = new Command(T.selectt, 1);
		center = new Command(T.playNow, 5);
		right = new Command("Đến bàn", 4);
		addCmd(2, 1);
		wSmall = (short)(110 * AvMain.hd);
		nBoardPerLine = Canvas.w / wSmall + 1;
		if (nBoardPerLine * wSmall > Canvas.w - wSmall / 2)
		{
			nBoardPerLine--;
		}
		xStart = wSmall / 2;
		yStart = wSmall / 2 + 20 * AvMain.hd;
		yStart += 10;
		if (Canvas.w > nBoardPerLine * wSmall)
		{
			xStart = (Canvas.w - nBoardPerLine * wSmall) / 2 + wSmall / 2;
		}
	}

	public static BoardListOnScr gI()
	{
		return (me != null) ? me : (me = new BoardListOnScr());
	}

	public override void switchToMe()
	{
		repaint();
		selected = 0;
		loadImgBoard();
		isHide = true;
		RoomListOnScr.gI().roomList.removeAllElements();
		countPaint = 0;
		base.switchToMe();
	}

	public void loadImgBoard()
	{
		Canvas.paint.initImgBoard(type);
	}

	public new void close()
	{
		Canvas.startWaitDlg();
		doExitBoardList();
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 1:
			doJoinBoard();
			break;
		case 2:
			doExitBoardList();
			break;
		case 3:
			commandAction(1);
			break;
		case 4:
			doAskForBoardToGo();
			break;
		case 5:
			Canvas.startWaitDlg();
			CasinoService.gI().joinAnyBoard();
			break;
		}
	}

	public override void commandAction(int index)
	{
		switch (index)
		{
		case 1:
			Canvas.startWaitDlg(T.pleaseWait);
			CasinoService.gI().requestBoardList(roomID);
			break;
		case 3:
			Canvas.startWaitDlg();
			GlobalService.gI().requestInfoOf(GameMidlet.avatar.IDDB);
			break;
		case 4:
			doExitBoardList();
			break;
		}
	}

	private void doAskForBoardToGo()
	{
		ipKeyboard.openKeyBoard(T.goToBoard, ipKeyboard.NUMBERIC, string.Empty, new IActionToGo(this), false);
	}

	protected void doAskForPass()
	{
		ipKeyboard.openKeyBoard(T.ifPassword, ipKeyboard.PASS, string.Empty, new IActionPass(), false);
	}

	public void setXeng()
	{
		Canvas.startOKDlg("Hiện tại bạn không đủ Xèng để tham gia màn chơi, bạn có muốn nạp thêm Xèng không?", new IActionXeng());
	}

	protected void doJoinBoard()
	{
		BoardInfo boardInfo = (BoardInfo)boardList.elementAt(selected);
		if (MapScr.isNewVersion && boardInfo.money > GameMidlet.avatar.money[3])
		{
			gI().setXeng();
			return;
		}
		if (boardInfo.isPass)
		{
			ipKeyboard.openKeyBoard(T.ifPassword, ipKeyboard.NUMBERIC, string.Empty, new IActionJoinBoard(), false);
			return;
		}
		CasinoService.gI().joinBoard(roomID, boardInfo.boardID, string.Empty);
		Canvas.load = 0;
	}

	private void doExitBoardList()
	{
		Canvas.cameraList.close();
		CasinoService.gI().requestRoomList();
		Canvas.load = 0;
		RoomListOnScr.gI().switchToMe();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintDefaultBg(g);
		onMainMenu.paintTitle(g, T.room + RoomListOnScr.title + " " + roomID, Canvas.w / 2, 30 * AvMain.hd - cmy);
		if (boardList.size() > 0 && Canvas.load == -1)
		{
			paintBoardList(g);
		}
		base.paint(g);
		Canvas.paintPlus2(g);
	}

	public void paintBoardList(MyGraphics g)
	{
		g.translate(xStart, yStart);
		g.translate(0f, -cmy);
		int num = cmy / wSmall * nBoardPerLine - nBoardPerLine;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num + Canvas.h / wSmall * nBoardPerLine + nBoardPerLine * 2 + nBoardPerLine;
		if (num2 > boardList.size())
		{
			num2 = boardList.size();
		}
		int width = imgTitleBoard.getWidth();
		int height = imgTitleBoard.getHeight();
		for (int i = num; i < num2 && i < boardList.size() && i < countPaint; i++)
		{
			int num3 = i % nBoardPerLine * wSmall;
			int num4 = i / nBoardPerLine * wSmall;
			BoardInfo boardInfo = (BoardInfo)boardList.elementAt(i);
			if (i == selected && !isHide)
			{
				g.drawImage(imgSelectBoard, num3, num4 + 10 * AvMain.hd, 3);
			}
			imgBoard.drawFrame(boardInfo.nPlayer, num3, num4 + 10 * AvMain.hd, 0, 3, g);
			g.drawImage(imgTitleBoard, num3, num4 - imgBoard.frameHeight / 2 - 5 * AvMain.hd, 3);
			Canvas.menuFont.drawString(g, string.Empty + boardInfo.boardID, num3 - width / 2 + 10 * AvMain.hd, num4 - imgBoard.frameHeight / 2 - 5 * AvMain.hd - Canvas.menuFont.getHeight() / 2, 2);
			Canvas.numberFont.drawString(g, boardInfo.strMoney, num3 + 8 * AvMain.hd, num4 - imgBoard.frameHeight / 2 - 5 * AvMain.hd - Canvas.numberFont.getHeight() / 2, 2);
			if ((type == STYLE_4PLAYER && boardInfo.maxPlayer < 4) || (type == STYLE_5PLAYER && boardInfo.maxPlayer < 5))
			{
				g.drawImage(imgNumPlayer, num3 + width / 2, num4 - imgBoard.frameHeight / 2 - 5 * AvMain.hd, 3);
				Canvas.menuFont.drawString(g, string.Empty + boardInfo.maxPlayer, num3 + width / 2, num4 - imgBoard.frameHeight / 2 - 5 * AvMain.hd - Canvas.menuFont.getHeight() / 2 + 1, 2);
			}
			if (boardInfo.isPlaying)
			{
				g.drawImage(imgPlaying, num3, num4 + 10 * AvMain.hd, 3);
			}
			if (boardInfo.isPass)
			{
				g.drawImage(imgLock, num3 + wSmall / 2 - 10 * AvMain.hd, num4 + wSmall / 2 - 14 * AvMain.hd, 3);
			}
		}
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
		int i = pyLast - Canvas.py;
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
			if (Math.abs(num) < 5 && Math.abs(i) < 5)
			{
				int num3 = (Canvas.py - (yStart - wSmall / 2) + cmtoY) / wSmall;
				int num4 = (Canvas.px - (xStart - wSmall / 2)) / wSmall;
				if (num3 >= 0 && num3 < boardList.size() && Canvas.isPoint(xStart - wSmall / 2, 0, Canvas.w - (xStart - wSmall / 2) * 2, dis))
				{
					selected = num3 * nBoardPerLine + num4;
				}
			}
			if (CRes.abs(Canvas.dy()) >= 5 && CRes.abs(Canvas.dx()) >= 5)
			{
				isHide = true;
			}
			else if (num2 > 3 && num2 < 8)
			{
				int num5 = (Canvas.py - (yStart - wSmall / 2) + cmtoY) / wSmall;
				if (num5 >= 0 && num5 < boardList.size() && !isG)
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
		int num6 = count - timePoint;
		int num7 = dyTran - Canvas.py;
		if (CRes.abs(num7) > 40 && num6 < 10 && cmtoY > 0 && cmtoY < cmyLim)
		{
			vY = num7 / num6 * 10;
		}
		timePoint = -1;
		if (Math.abs(num) < 5 && Math.abs(i) < 5)
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
		cmdSellect.perform();
	}

	public void setBoardList(MyVector boardList)
	{
		this.boardList = boardList;
		dis = Canvas.hCan - PaintPopup.hButtonSmall;
		int num = boardList.size() / nBoardPerLine;
		if (boardList.size() % nBoardPerLine > 0)
		{
			num++;
		}
		cmyLim = num * wSmall - dis;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
	}

	public void setCam()
	{
		int num = boardList.size() / nBoardPerLine;
		if (boardList.size() % nBoardPerLine != 0)
		{
			num++;
		}
		yStart = 110 * AvMain.hd;
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
		if (countPaint < boardList.size())
		{
			countPaint++;
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
