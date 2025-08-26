public class BCBoardScr : BoardScr
{
	public new static BCBoardScr me;

	private MyVector moneyInput = new MyVector();

	private MyVector vtmoneySV = new MyVector();

	private MyVector mapSeat = new MyVector();

	private MyVector xn = new MyVector();

	private MyVector bc = new MyVector();

	private Command cmdskipBC;

	private Command cmdNextBC;

	public int xbg;

	public int xFC;

	public int ybg;

	public sbyte idffr = -1;

	public sbyte idT = -1;

	public sbyte index;

	public sbyte addfr;

	public sbyte addt;

	public sbyte seat;

	public sbyte countEnter;

	public sbyte saveTime;

	public static int rWT;

	public static int hHT;

	public static int rWSM;

	public static int hHSM;

	private bool[] isFinish = new bool[6];

	public sbyte[][] moneySV = new sbyte[5][];

	public sbyte[] result = new sbyte[3];

	private sbyte count;

	private sbyte autoLuot;

	private bool canTa;

	private bool taOK;

	private bool beginCharTa;

	private bool isStopXn;

	public bool canpointer;

	private int[] moneyP;

	public static int th = MyGraphics.TOP | MyGraphics.HCENTER;

	public static int bh = MyGraphics.BOTTOM | MyGraphics.HCENTER;

	private bool canMoveBoard;

	public static Image pointer;

	public static int rW;

	public static int hH;

	public static AvPosition[] posAvatar5;

	private MyVector listFireWork = new MyVector();

	private sbyte addY;

	private Command cmdSkip;

	public BCBoardScr()
	{
		Xingau.array[0] = new int[8] { 6, 0, 7, 1, 6, 2, 7, 3 };
		Xingau.array[1] = new int[8] { 6, 5, 7, 4, 6, 3, 7, 2 };
		Xingau.array[2] = new int[8] { 7, 4, 6, 1, 7, 3, 6, 5 };
		for (int i = 0; i < 5; i++)
		{
			moneySV[i] = new sbyte[6];
		}
		resetdata();
		moneyP = null;
		init();
		cmdskipBC = new Command(T.skip, 7);
		cmdNextBC = new Command(T.continuee, 8);
		cmdSkip = new Command(T.skip, 9);
		if (Canvas.w > 200)
		{
			rWT = (hHT = 23);
			rW = (hH = 48);
			if (AvMain.hd == 2)
			{
				rW = (hH = 96);
			}
		}
		loadIMG();
	}

	public static BoardScr gI()
	{
		return (me != null) ? me : (me = new BCBoardScr());
	}

	public override void switchToMe()
	{
		init();
		base.switchToMe();
	}

	public void resetdata()
	{
		for (int i = 0; i < result.Length; i++)
		{
			result[i] = -1;
		}
		for (int j = 0; j < isFinish.Length; j++)
		{
			isFinish[j] = false;
		}
		for (int k = 0; k < moneySV.Length; k++)
		{
			for (int l = 0; l < moneySV[k].Length; l++)
			{
				moneySV[k][l] = 0;
			}
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 7:
			onSkip();
			break;
		case 8:
			doNextBC();
			break;
		case 9:
			doSkip();
			break;
		}
		base.commandTab(index);
	}

	public override void init()
	{
		base.init();
		posAvatar5 = new AvPosition[5]
		{
			new AvPosition(20 * AvMain.hd, 50 + 30 * AvMain.hd, MyGraphics.VCENTER | MyGraphics.LEFT),
			new AvPosition(20 * AvMain.hd, Canvas.hh + 60, MyGraphics.VCENTER | MyGraphics.LEFT),
			new AvPosition(Canvas.hw, Canvas.hCan - Canvas.hTab - 10, MyGraphics.BOTTOM | MyGraphics.HCENTER),
			new AvPosition(Canvas.w - 14 * AvMain.hd, Canvas.hh + 60, MyGraphics.VCENTER | MyGraphics.RIGHT),
			new AvPosition(Canvas.w - 14 * AvMain.hd, 50 + 30 * AvMain.hd, MyGraphics.VCENTER | MyGraphics.RIGHT)
		};
	}

	private void setAnimateBC()
	{
		for (sbyte b = 0; b < 5; b++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(b);
			if (avatar.IDDB != -1)
			{
				BoardScr.showChat(avatar.IDDB, moneyP[b] + string.Empty);
				avatar.setMoneyNew(avatar.getMoneyNew() + moneyP[b]);
			}
		}
	}

	public void onSetPlayer(sbyte fromUse, sbyte toUse, int moneyValue)
	{
		showFlyText5Baucua(fromUse, toUse, moneyValue);
	}

	public void showFlyText5Baucua(sbyte fro, sbyte to, int money2)
	{
		if (money2 != 0)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(fro);
			Avatar avatar2 = (Avatar)BoardScr.avatarInfos.elementAt(to);
			Point point = new Point(avatar.x, avatar.y);
			point.distant = (short)money2;
			point.color = CRes.rnd(3);
			int g = CRes.angle(avatar2.x - avatar.x, -(avatar2.x - avatar.y));
			point.g = g;
			point.catagory = (sbyte)CRes.rnd(-1, 1);
			point.h = CRes.fixangle(point.g + point.catagory * 90);
			int num = 10 * CRes.cos(point.h) >> 10;
			int num2 = -(10 * CRes.sin(point.h)) >> 10;
			point.xTo = (short)avatar2.x;
			point.yTo = (short)avatar2.y;
			point.x += num;
			point.y += num2;
			point.color = 0;
			point.dis = (sbyte)(CRes.rnd(4) + 2);
			point.height = (short)(8 + CRes.rnd(5));
			listFireWork.addElement(point);
		}
	}

	public void checkTimeLimit()
	{
		BoardScr.dieTime = (int)(Canvas.getSecond() - BoardScr.currentTime);
		if (!BoardScr.isStartGame || BoardScr.isGameEnd || BoardScr.disableReady || BoardScr.interval - BoardScr.dieTime >= 0)
		{
			return;
		}
		canpointer = true;
		if (GameMidlet.avatar.IDDB != BoardScr.ownerID)
		{
			if (autoLuot == 0)
			{
				autoLuot = 1;
				putMoneyFN();
			}
			if (autoLuot == 2)
			{
				autoLuot = 3;
				onSkip();
			}
		}
	}

	public void resetGame()
	{
		idffr = -1;
		idT = -1;
		addfr = 0;
		addt = 0;
		seat = 0;
		canTa = false;
		taOK = false;
		beginCharTa = false;
		count = 0;
		canpointer = false;
		moneyInput.removeAllElements();
		vtmoneySV.removeAllElements();
		xn.removeAllElements();
		countEnter = 0;
		isStopXn = false;
		BoardScr.isStartGame = false;
		currentPlayer = -1;
		autoLuot = 0;
		BoardScr.disableReady = false;
		canMoveBoard = false;
		resetdata();
		for (int i = 0; i < bc.size(); i++)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(i);
			pimgBC.moneyPut = 0;
		}
	}

	private void loadIMG()
	{
		bc.removeAllElements();
		xbg = Canvas.w / 2 - rW - rW / 2 - 10;
		ybg = Canvas.h / 2 - hH - 12;
		for (int i = 0; i < 6; i++)
		{
			PimgBC pimgBC = new PimgBC();
			pimgBC.type = i;
			pimgBC.x = xbg + i % 3 * (rW + 10);
			pimgBC.y = ybg + i / 3 * (hH + 8);
			bc.addElement(pimgBC);
		}
	}

	private void loadXingau()
	{
		int num = 0;
		int num2 = 0;
		num2 = 10;
		if (xn.size() > 0)
		{
			return;
		}
		if (Canvas.w > 200)
		{
			num = Canvas.w / 2 - 64 * AvMain.hd;
			for (int i = 0; i < 3; i++)
			{
				creatXn(num + i * 64 * AvMain.hd, num2, i, i, false);
			}
		}
		else
		{
			num = Canvas.w / 2 - 49;
			for (int j = 0; j < 3; j++)
			{
				creatXn(num + j * 49, 0, j, j, false);
			}
		}
	}

	public void ta()
	{
		if (!taOK)
		{
			canpointer = true;
			CasinoService.gI().ta(BoardScr.roomID, BoardScr.boardID, idffr, idT);
			BoardScr.disableReady = true;
			currentPlayer = -1;
			canMoveBoard = true;
		}
	}

	public void onSkip()
	{
		setCmdWaiting();
		BoardScr.disableReady = true;
		CasinoService.gI().skip(BoardScr.roomID, BoardScr.boardID);
	}

	public void reset()
	{
		BoardScr.isGameEnd = false;
		BoardScr.isStartGame = false;
		BoardScr.disableReady = false;
		canMoveBoard = false;
		Canvas.startWaitDlg();
		CasinoService.gI().leaveBoard(BoardScr.roomID, BoardScr.boardID);
		CasinoService.gI().requestBoardList(BoardScr.roomID);
		resetGame();
	}

	private void setMoney()
	{
		PimgBC pimgBC = (PimgBC)bc.elementAt(index);
		pimgBC.moneyPut++;
		paintPutMoney();
	}

	public void paintMoneyTa()
	{
		if (taOK)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(addt);
			int seatATmapSeat = getSeatATmapSeat(mapSeat, seat);
			creatSVMoneyPut(pimgBC.x, pimgBC.y, pimgBC.x, pimgBC.y, moneySV[seat][addt], getIndex(seatATmapSeat), addt, addt, false);
			taOK = false;
		}
	}

	public void paintSVmoney()
	{
		for (int i = 0; i < 6; i++)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(i);
			int seatATmapSeat = getSeatATmapSeat(mapSeat, seat);
			creatSVMoneyPut(pimgBC.x, pimgBC.y, pimgBC.x, pimgBC.y, moneySV[seat][i], getIndex(seatATmapSeat), i, i, false);
		}
	}

	public void paintXingau(MyGraphics g)
	{
		if (xn.size() > 0)
		{
			for (int i = 0; i < xn.size(); i++)
			{
				Xingau xingau = (Xingau)xn.elementAt(i);
				xingau.paint(g);
			}
		}
	}

	private int getIndex(int vt)
	{
		switch (vt)
		{
		case 0:
			return 3;
		case 1:
			return 0;
		case 2:
			return 1;
		case 3:
			return 2;
		default:
			return -1;
		}
	}

	public void paintPutMoney()
	{
		for (int i = 0; i < 6; i++)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(i);
			int seatATmapSeat = getSeatATmapSeat(mapSeat, BoardScr.getIndexByID(GameMidlet.avatar.IDDB));
			creatMoneyPut(pimgBC.x + rW / 2, pimgBC.y + hH / 2, pimgBC.moneyPut, getIndex(seatATmapSeat));
		}
	}

	private void creatMoneyPut(int x, int y, int value, int typePaint)
	{
		MoneyPut o = new MoneyPut(x, y, value, typePaint);
		moneyInput.addElement(o);
	}

	private void creatXn(int x, int y, int type, int typeStop, bool stopHere)
	{
		Xingau o = new Xingau(x, y, type, typeStop, stopHere);
		xn.addElement(o);
	}

	private void creatSVMoneyPut(int x, int y, int xto, int yto, int value, int typePaint, int addFrom, int addTo, bool isMoveOK)
	{
		MoneySV o = new MoneySV(x, y, xto, yto, value, typePaint, addFrom, addTo, isMoveOK);
		vtmoneySV.addElement(o);
	}

	public void paintImgBC(MyGraphics g)
	{
		if (bc.size() <= 0)
		{
			return;
		}
		if (idffr != -1)
		{
			g.setColor(16777215);
			if (Canvas.gameTick % 20 > 10)
			{
				g.fillRect(xbg + idffr % 3 * (rW + 10), ybg + idffr / 3 * (hH + 8), rW, hH);
			}
		}
		if (idT != -1)
		{
			g.setColor(1112500);
			if (Canvas.gameTick % 20 > 10)
			{
				g.fillRect(xbg + idT % 3 * (rW + 10), ybg + idT / 3 * (hH + 8), rW, hH);
			}
		}
		for (int i = 0; i < bc.size(); i++)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(i);
			if (AvatarData.getImgIcon(872).count != -1)
			{
				g.drawRegion(AvatarData.getImgIcon(872).img, 0f, pimgBC.type * hH, rW, hH, 0, xbg + i % 3 * (rW + 10), ybg + i / 3 * (hH + 8), 0);
			}
		}
	}

	public void paintMoneyAtPlayer(MyGraphics g)
	{
		for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB == BoardScr.ownerID || avatar.IDDB != -1)
			{
				if (currentPlayer != avatar.IDDB || Canvas.gameTick % 10 >= 5)
				{
					Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), avatar.x, avatar.y + 5, 2);
				}
				int seatATmapSeat = getSeatATmapSeat(mapSeat, BoardScr.getIndexByID(avatar.IDDB));
				if (seatATmapSeat != -1 && AvatarData.getImgIcon(871).count != -1)
				{
					g.drawRegion(AvatarData.getImgIcon(871).img, 0f, getIndex(seatATmapSeat) * 12, 12, 12, 0, avatar.x, avatar.y + 5 + AvMain.hSmall, MyGraphics.TOP | MyGraphics.HCENTER);
				}
			}
		}
	}

	public void onSetTurn(sbyte seatI)
	{
		int indexByID = BoardScr.getIndexByID(GameMidlet.avatar.IDDB);
		Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(seatI);
		if (indexByID == seatI)
		{
			isFinish[indexByID] = false;
			canTa = true;
			right = null;
			autoLuot = 2;
			canpointer = false;
		}
		currentPlayer = avatar.IDDB;
		BoardScr.interval = saveTime;
		BoardScr.currentTime = Canvas.getSecond();
		if (!beginCharTa)
		{
			beginCharTa = true;
		}
		if (GameMidlet.avatar.IDDB != BoardScr.ownerID && indexByID == seatI)
		{
			setBotCmdTa();
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		paintFireWork(g);
		base.paint(g);
	}

	private void paintFireWork(MyGraphics g)
	{
		for (int i = 0; i < listFireWork.size(); i++)
		{
			Point point = (Point)listFireWork.elementAt(i);
			if (point.dis >= 0)
			{
				Canvas.numberFont.drawString(g, "+" + point.distant, point.x, point.y, 2);
			}
		}
	}

	public override void paintMain(MyGraphics g)
	{
		base.paintMain(g);
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			Canvas.resetTrans(g);
			paintImgBC(g);
		}
		paintNamePlayers(g);
		if (!BoardScr.isStartGame && !BoardScr.disableReady)
		{
			return;
		}
		Canvas.resetTrans(g);
		paintMoneyAtPlayer(g);
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			int num = (int)(BoardScr.interval - BoardScr.dieTime);
			if (num > 0 && !BoardScr.isGameEnd && xn.size() <= 0)
			{
				Canvas.numberFont.drawString(g, num + string.Empty, Canvas.hw, 10, 2);
			}
			if (beginCharTa)
			{
				if (count < 100)
				{
					count++;
				}
				else
				{
					count = 100;
				}
				if (count < 50)
				{
					Canvas.borderFont.drawString(g, T.startTa, Canvas.hw, ybg - 40, 2);
				}
			}
		}
		if (moneyInput.size() > 0)
		{
			for (int i = 0; i < moneyInput.size(); i++)
			{
				MoneyPut moneyPut = (MoneyPut)moneyInput.elementAt(i);
				if (moneyPut.valuea > 0)
				{
					moneyPut.paint(g);
				}
			}
		}
		if (vtmoneySV.size() > 0)
		{
			for (int j = 0; j < vtmoneySV.size(); j++)
			{
				MoneySV moneySV = (MoneySV)vtmoneySV.elementAt(j);
				if (moneySV.valuea > 0)
				{
					moneySV.paint(g);
				}
			}
		}
		if (GameMidlet.avatar.IDDB != BoardScr.ownerID && BoardScr.isStartGame && xn.size() == 0)
		{
			g.drawImage(pointer, xbg + rW / 2 + index % 3 * (rW + 10), ybg + hH / 2 + index / 3 * (hH + 8) + Canvas.gameTick % 4 + 5, 3);
		}
		paintXingau(g);
	}

	public void putMoneyFN()
	{
		setCmdWaiting();
		canpointer = true;
		CasinoService.gI().PutMoneyOk(bc, BoardScr.roomID, BoardScr.boardID);
		moneyInput.removeAllElements();
	}

	public override void updateKey()
	{
		base.updateKey();
		updateK();
	}

	private void updatePointer()
	{
		if (canpointer || !BoardScr.isStartGame || BoardScr.isGameEnd || bc.size() <= 0 || !Canvas.isPointerClick)
		{
			return;
		}
		Canvas.isPointerClick = false;
		for (int i = 0; i < bc.size(); i++)
		{
			PimgBC pimgBC = (PimgBC)bc.elementAt(i);
			if (Canvas.px >= pimgBC.x && Canvas.px <= pimgBC.x + rW && Canvas.py >= pimgBC.y && Canvas.py <= pimgBC.y + hH)
			{
				index = (sbyte)i;
				pointerFire();
				break;
			}
		}
	}

	private void updateK()
	{
		if (!isFinish[BoardScr.getIndexByID(GameMidlet.avatar.IDDB)] && GameMidlet.avatar.IDDB != BoardScr.ownerID)
		{
			updatePointer();
		}
	}

	private void testMoneySVupdate()
	{
		if (vtmoneySV.size() <= 0 || bc.size() <= 0)
		{
			return;
		}
		for (int i = 0; i < vtmoneySV.size(); i++)
		{
			MoneySV moneySV = (MoneySV)vtmoneySV.elementAt(i);
			moneySV.update();
			if (moneySV.move)
			{
				vtmoneySV.removeElement(moneySV);
				paintMoneyTa();
			}
		}
		PimgBC pimgBC = (PimgBC)bc.elementAt(addt);
		if (!taOK)
		{
			return;
		}
		for (int j = 0; j < vtmoneySV.size(); j++)
		{
			MoneySV moneySV2 = (MoneySV)vtmoneySV.elementAt(j);
			if (moneySV2.addFrom == addfr)
			{
				moneySV2.xto = pimgBC.x;
				moneySV2.yto = pimgBC.y;
				moneySV2.isMoveOK = true;
			}
		}
	}

	public override void doReady()
	{
		base.doReady();
		if (!BoardScr.isStartGame && !BoardScr.disableReady)
		{
			resetGame();
		}
	}

	public override void update()
	{
		base.update();
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			checkTimeLimit();
			testMoneySVupdate();
			if (xn.size() > 0)
			{
				for (int i = 0; i < xn.size(); i++)
				{
					Xingau xingau = (Xingau)xn.elementAt(i);
					xingau.update();
					if (isStopXn)
					{
						xingau.typeStop = result[i];
						xingau.stopHere = true;
					}
				}
			}
			for (int j = 0; j < listFireWork.size(); j++)
			{
				Point point = (Point)listFireWork.elementAt(j);
				int num = CRes.angle(point.xTo - point.x, -(point.yTo - point.y));
				if (CRes.abs(num - point.h) > 10)
				{
					point.h -= point.height * point.catagory;
					point.h = CRes.fixangle(point.h);
				}
				else
				{
					point.h = num;
					point.dis += 2;
				}
				if (point.color >= 4)
				{
					point.color = 0;
				}
				point.color++;
				int num2 = point.dis * CRes.cos(point.h) >> 10;
				int num3 = -(point.dis * CRes.sin(point.h)) >> 10;
				if (CRes.distance(point.x, point.y, point.xTo, point.yTo) >= point.dis)
				{
					point.x += num2;
					point.y += num3;
				}
				else
				{
					listFireWork.removeElement(point);
				}
			}
		}
		else
		{
			updateReady();
		}
	}

	public void onFinish(int[] moneyP)
	{
		this.moneyP = moneyP;
		isStopXn = true;
		canMoveBoard = false;
		BoardScr.isGameEnd = true;
		right = null;
		beginCharTa = false;
		count = 0;
		center = cmdNextBC;
		setAnimateBC();
	}

	protected void doNextBC()
	{
		doContinue();
		listFireWork.removeAllElements();
		BoardScr.isGameEnd = false;
		BoardScr.isStartGame = false;
		BoardScr.disableReady = false;
		currentPlayer = -1;
		moneyP = null;
		moneyInput.removeAllElements();
		vtmoneySV.removeAllElements();
		idffr = -1;
		idT = -1;
	}

	public void onPlaying(sbyte time)
	{
		BoardScr.isStartGame = false;
		BoardScr.disableReady = true;
		mapSeat.removeAllElements();
		setPosPlaying();
		for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB != BoardScr.ownerID)
			{
				mapSeat.addElement(i + string.Empty);
			}
		}
		paintSVmoney();
		center = BoardScr.cmdWaiting;
	}

	public void setCmdWaitingBC()
	{
		center = null;
		right = null;
	}

	public void setCmdWaitingRechoose()
	{
		right = null;
	}

	private void pointerFire()
	{
		if (!canTa)
		{
			if (!isFinish[BoardScr.getIndexByID(GameMidlet.avatar.IDDB)])
			{
				if (countEnter < 6)
				{
					setMoney();
				}
				countEnter++;
			}
		}
		else
		{
			if (idT != -1)
			{
				return;
			}
			if (idffr == -1)
			{
				MoneySV moneySV = (MoneySV)vtmoneySV.elementAt(index);
				if (moneySV.valuea > 0)
				{
					idffr = index;
					center.caption = T.ta;
					setBotCmdReChoose();
				}
			}
			else
			{
				idT = index;
				ta();
			}
		}
	}

	public override void doFire()
	{
		if (!canTa)
		{
			if (!isFinish[BoardScr.getIndexByID(GameMidlet.avatar.IDDB)])
			{
				if (countEnter < 6)
				{
					setMoney();
				}
				countEnter++;
			}
		}
		else
		{
			if (idT != -1)
			{
				return;
			}
			if (idffr == -1)
			{
				MoneySV moneySV = (MoneySV)vtmoneySV.elementAt(index);
				if (moneySV.valuea > 0)
				{
					idffr = index;
					center.caption = T.ta;
					setBotCmdReChoose();
				}
			}
			else
			{
				idT = index;
				ta();
			}
		}
	}

	public void doSkip()
	{
		if (!canTa)
		{
			if (!isFinish[BoardScr.getIndexByID(GameMidlet.avatar.IDDB)])
			{
				autoLuot = 1;
				putMoneyFN();
			}
		}
		else if (idffr != -1)
		{
			idffr = -1;
			center.caption = T.selectt;
			right = cmdskipBC;
		}
	}

	public void setBotCmd()
	{
		center = BoardScr.cmdFire;
		right = cmdSkip;
		center.caption = T.sett;
		right.caption = T.finish;
	}

	private void setBotCmdTa()
	{
		center = BoardScr.cmdFire;
		center.caption = T.selectt;
		right = cmdskipBC;
	}

	private void setBotCmdReChoose()
	{
		right = cmdSkip;
		right.caption = T.selectAgain;
	}

	public int getSeatATmapSeat(MyVector info, int ghe)
	{
		for (int i = 0; i < info.size(); i++)
		{
			string text = (string)info.elementAt(i);
			if (text.Equals(ghe + string.Empty))
			{
				return i;
			}
		}
		return -1;
	}

	public new void setPosBoard()
	{
		BoardScr.me.setPosBoard();
		mapSeat.removeAllElements();
		for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB != BoardScr.ownerID)
			{
				mapSeat.addElement(i + string.Empty);
			}
		}
	}

	public void onStartGame(sbyte boardID6, sbyte roomID6, sbyte interval2)
	{
		base.start(0, interval2);
		Canvas.endDlg();
		resetGame();
		resetReady();
		mapSeat.removeAllElements();
		setPosPlaying();
		for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB != BoardScr.ownerID)
			{
				mapSeat.addElement(i + string.Empty);
			}
		}
		if (GameMidlet.avatar.IDDB != BoardScr.ownerID)
		{
			setBotCmd();
		}
		else
		{
			center = null;
			right = null;
		}
		BoardScr.isGameEnd = false;
		BoardScr.isStartGame = true;
		BoardScr.interval = interval2;
		BoardScr.currentTime = Canvas.getSecond();
	}

	public void onMove(sbyte idseat)
	{
		seat = idseat;
		isFinish[seat] = true;
		paintSVmoney();
	}

	public void onHaphom(sbyte seatHP, sbyte fromHP, sbyte toHP)
	{
		if (fromHP != toHP)
		{
			seat = seatHP;
			addfr = fromHP;
			addt = toHP;
			taOK = true;
			autoLuot = 3;
		}
	}

	public void onResult(sbyte[] resultSV)
	{
		result = resultSV;
		MyVector myVector = new MyVector();
		for (int i = 0; i < 6; i++)
		{
			PimgBC pimgBC = new PimgBC();
			if (i == result[0])
			{
				pimgBC.moneyPut = 6;
			}
			myVector.addElement(pimgBC);
		}
		CasinoService.gI().PutMoneyOk(myVector, BoardScr.roomID, BoardScr.boardID);
		loadXingau();
	}

	public override void setPosPlaying()
	{
		for (int i = 0; i < BoardScr.numPlayer; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB != -1)
			{
				avatar.ySat = 0;
				avatar.setAction(0);
				avatar.setFrame(avatar.action);
				avatar.xCur = (avatar.x = posAvatar5[BoardScr.indexPlayer[i]].x);
				avatar.yCur = (avatar.y = posAvatar5[BoardScr.indexPlayer[i]].y);
				if (BoardScr.indexPlayer[i] == 2 || BoardScr.indexPlayer[i] == 3 || BoardScr.indexPlayer[i] == 4)
				{
					avatar.direct = (avatar.dirFirst = Base.LEFT);
				}
				else
				{
					avatar.direct = (avatar.dirFirst = Base.RIGHT);
				}
			}
		}
	}
}
