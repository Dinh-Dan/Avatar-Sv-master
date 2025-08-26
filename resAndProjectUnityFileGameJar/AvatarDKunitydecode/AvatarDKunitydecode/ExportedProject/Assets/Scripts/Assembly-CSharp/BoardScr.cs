using System;

public abstract class BoardScr : OnScreen, IChatable
{
	private class IActionClose : IAction
	{
		public void perform()
		{
			me.doContinue();
			me.doCloseBoard();
			isStartGame = false;
			me.turn = -1;
			interval = 0;
		}
	}

	private class IActionExit : IAction
	{
		private BoardScr me;

		public IActionExit(BoardScr me)
		{
			this.me = me;
		}

		public void perform()
		{
			Canvas.load = 0;
			me.resetCard();
			CasinoService.gI().leaveBoard(roomID, boardID);
			CasinoService.gI().requestBoardList(roomID);
			me.setPosCam();
			Canvas.endDlg();
			Canvas.load = 0;
		}
	}

	private class IActionSettingMoney : IKbAction
	{
		public void perform(string text)
		{
			try
			{
				int num = int.Parse(text);
				if (num >= 0)
				{
					if (MapScr.isNewVersion && num > GameMidlet.avatar.money[3])
					{
						BoardListOnScr.gI().setXeng();
						return;
					}
					Canvas.endDlg();
					GlobalService.gI().setMoney(roomID, boardID, num);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private class IActionMaxPlayer : IAction
	{
		private int ii;

		public IActionMaxPlayer(int i)
		{
			ii = i;
		}

		public void perform()
		{
			CasinoService.gI().setMaxPlayer(ii);
		}
	}

	private class IActionPass : IKbAction
	{
		public void perform(string text)
		{
			GlobalService.gI().setPassword(roomID, boardID, text);
			Canvas.startOKDlg(T.setPassed);
		}
	}

	private class IActionKick : IAction
	{
		private int IDDB;

		public IActionKick(int id)
		{
			IDDB = id;
		}

		public void perform()
		{
			CasinoService.gI().kick(roomID, boardID, IDDB);
		}
	}

	private class IActionAddFriend : IAction
	{
		private Avatar p;

		public IActionAddFriend(Avatar p)
		{
			this.p = p;
		}

		public void perform()
		{
			MapScr.gI().doRequestAddFriend(p);
		}
	}

	public static BoardScr me;

	public static bool isStartGame;

	public static bool disableReady;

	public static bool isGameEnd;

	public static MyVector avatarInfos;

	public int currentPlayer;

	public int selectedCard;

	public static sbyte roomID;

	public static sbyte boardID;

	public static int ownerID;

	public static int money;

	public static sbyte indexOfMe;

	public static long dieTime;

	public static long currentTime;

	public static int interval;

	public static int notReadyDelay;

	public static int[] indexPlayer = new int[4];

	public int disCard = 10;

	public static int wCard;

	public static int hcard;

	public static Image imgBoard;

	public static int xBoard;

	public static int yBoard;

	public static int wBoard;

	public static int hBoard;

	public int turn = -1;

	public static Command cmdCloseBoard;

	public static Command cmdStart;

	public static Command cmdBack;

	public static Command cmdFire;

	public static Command cmdReady;

	public static Command cmdWaiting;

	public static Command cmdMenu;

	public static Image[] imgReady;

	public static AvPosition[] posAvatar;

	public static MyVector chatHistory = new MyVector();

	public static Image imgBan;

	public static int numPlayer = 4;

	public static MyVector listPosAvatar = new MyVector();

	public static MyVector listPosCasino = new MyVector();

	private static ChatPopup chatPublic;

	public BoardScr()
	{
		init();
		cmdMenu = new Command(T.menu, 0);
		cmdCloseBoard = new Command(T.OK, 1);
		cmdStart = new Command(T.start, 2);
		cmdBack = new Command(T.continuee, 3);
		cmdFire = new Command(T.fire, 4);
		cmdReady = new Command(T.ready, 5);
		cmdWaiting = new Command(T.pleaseWait, 6);
		addCmd(10, 1);
		addCmd(0, 5);
		addCmd(11, 2);
		addCmd(12, 4);
	}

	public void initImg()
	{
		if (imgReady == null)
		{
		}
	}

	public override void close()
	{
		doExit();
	}

	public override void switchToMe()
	{
		Canvas.clearKeyPressed();
		base.switchToMe();
		me = this;
	}

	public virtual void init()
	{
		if (AvMain.hd == 2)
		{
			wCard = 144;
			hcard = 194;
		}
		else
		{
			wCard = 72;
			hcard = 97;
		}
		posAvatar = new AvPosition[4]
		{
			new AvPosition(Canvas.hw, 55 * AvMain.hd, 2),
			new AvPosition(13 * AvMain.hd, Canvas.hh - 20, 0),
			new AvPosition(Canvas.hw, Canvas.h - 5 * AvMain.hd, 2),
			new AvPosition(Canvas.w - 13 * AvMain.hd, Canvas.hh - 20, 1)
		};
		setPosCam();
		if (isStartGame || disableReady)
		{
			setPosPlaying();
		}
		if (me != null)
		{
			repaint();
		}
	}

	public void doCloseBoard()
	{
		chatHistory.removeAllElements();
		setPosCam();
		Canvas.endDlg();
	}

	public void closeBoard(string info)
	{
		left = null;
		center = null;
		Canvas.startOK(info, new IActionClose());
	}

	public virtual void doReady()
	{
		Avatar avatarByID = getAvatarByID(GameMidlet.avatar.IDDB);
		if (avatarByID.action == 1)
		{
			return;
		}
		if (MapScr.isNewVersion && money > GameMidlet.avatar.money[3])
		{
			BoardListOnScr.gI().setXeng();
			return;
		}
		bool flag = !((Avatar)avatarInfos.elementAt(indexOfMe)).isReady;
		if (flag)
		{
			notReadyDelay = 100;
		}
		setCmdWaiting();
		Canvas.startWaitDlg();
		CasinoService.gI().ready(roomID, boardID, flag);
	}

	public void setCmdWaiting()
	{
		me.center = cmdWaiting;
		me.right = null;
	}

	public virtual void doFire()
	{
	}

	protected void doStartGame()
	{
		if (isStartGame)
		{
			return;
		}
		if (MapScr.isNewVersion && money > GameMidlet.avatar.money[3])
		{
			BoardListOnScr.gI().setXeng();
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != GameMidlet.avatar.IDDB && avatar.IDDB != -1)
			{
				if (avatar.isReady)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (num == 0 || num2 > 0)
		{
			Canvas.startOKDlg(T.opponentAreNotReady);
			return;
		}
		if (me == PBoardScr.instance)
		{
			me.center = cmdWaiting;
			me.right = null;
		}
		else
		{
			Canvas.startWaitDlg();
		}
		repaint();
		GlobalService.gI().startGame(roomID, boardID);
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 1:
			doOption();
			break;
		case 2:
			doKick();
			break;
		case 3:
			doAddFriend();
			break;
		case 4:
			doViewMessage();
			break;
		case 5:
			doExit();
			break;
		case 6:
			Canvas.startWaitDlg();
			GlobalService.gI().requestInfoOf(GameMidlet.avatar.IDDB);
			break;
		case 10:
			doSettingMoney();
			break;
		case 11:
			doSetMaxPlayer();
			break;
		case 12:
			doSettingPassword();
			break;
		case 7:
		case 8:
		case 9:
			break;
		}
	}

	public override void commandAction(int index)
	{
		switch (index)
		{
		case 1:
			doOption();
			break;
		case 2:
			doKick();
			break;
		case 3:
			doAddFriend();
			break;
		case 4:
			doViewMessage();
			break;
		case 5:
			doExit();
			break;
		case 6:
			Canvas.startWaitDlg();
			GlobalService.gI().requestInfoOf(GameMidlet.avatar.IDDB);
			break;
		case 10:
			doSettingMoney();
			break;
		case 11:
			doSetMaxPlayer();
			break;
		case 12:
			doSettingPassword();
			break;
		case 7:
		case 8:
		case 9:
			break;
		}
	}

	private void doOption()
	{
		MyVector myVector = new MyVector();
		Command o = new Command(T.setMoney, 10, this);
		Command o2 = new Command(T.setNumPlayers, 11, this);
		Command o3 = new Command(T.setPass, 12, this);
		myVector.addElement(o);
		if (BoardListOnScr.type != 0)
		{
			myVector.addElement(o2);
		}
		myVector.addElement(o3);
		MenuCenter.gI().startAt(myVector);
	}

	public override void doMenu()
	{
		Command o = new Command(T.option, 1, this);
		Command o2 = new Command(T.kick, 2, this);
		int num = 0;
		for (int i = 0; i < numPlayer; i++)
		{
			if (((Avatar)avatarInfos.elementAt(i)).IDDB != -1)
			{
				num++;
			}
		}
		MyVector myVector = new MyVector();
		if (ownerID == GameMidlet.avatar.IDDB && !isStartGame)
		{
			myVector.addElement(o);
			if (num > 1)
			{
				myVector.addElement(o2);
			}
		}
		if (num > 1)
		{
			myVector.addElement(new Command(T.addFriend, 3, this));
		}
		myVector.addElement(new Command(T.viewMyInfo, 6, this));
		myVector.addElement(new Command(T.viewMessage, 4, this));
		myVector.addElement(new Command(T.exitBoard, 5, this));
		if (myVector.size() == 1 && ownerID == GameMidlet.avatar.IDDB)
		{
			commandAction(1);
		}
		else
		{
			MenuCenter.gI().startAt(myVector);
		}
	}

	public static void startMenu(MyVector menu, int pos)
	{
		MenuLeft.gI().startAt(menu);
	}

	public virtual void resetCard()
	{
		currentTime = 0L;
		dieTime = 0L;
		isStartGame = false;
		disableReady = false;
		isGameEnd = false;
	}

	public void setPosCam()
	{
	}

	public void loadMap(int type)
	{
		listPosAvatar.removeAllElements();
		listPosCasino.removeAllElements();
		setPosPlaying();
	}

	public void setAt(int seat, Avatar ava)
	{
		avatarInfos.setElementAt(ava, seat);
		setPosBoard();
		setPosPlaying();
	}

	public void setPosTrans(int id)
	{
		Avatar avatarByID = getAvatarByID(id);
		if (avatarByID != null)
		{
			int indexByID = getIndexByID(id);
			AvPosition avPosition = (AvPosition)listPosCasino.elementAt(indexByID);
			avatarByID.x = avPosition.x;
			avatarByID.y = avPosition.y;
			avatarByID.action = 1;
		}
	}

	public virtual void doContinue()
	{
		setPosCam();
		repaint();
	}

	public void resetPos()
	{
		for (int i = 0; i < avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			AvPosition avPosition = (AvPosition)listPosAvatar.elementAt(i);
			avatar.setPos(avPosition.x, avPosition.y);
			if ((avatar.IDDB == ownerID || avatar.isReady) && avatar.action == 0)
			{
				avatar.ySat = -8;
				avatar.setAction(2);
			}
			if ((BoardListOnScr.type == BoardListOnScr.STYLE_5PLAYER && i % 2 == 1) || (BoardListOnScr.type != BoardListOnScr.STYLE_5PLAYER && i % 2 == 0))
			{
				avatar.direct = (avatar.dirFirst = Base.RIGHT);
			}
			else
			{
				avatar.direct = (avatar.dirFirst = Base.LEFT);
			}
		}
	}

	public override void updateKey()
	{
		base.updateKey();
	}

	public override void update()
	{
		base.update();
		if (notReadyDelay > 0)
		{
			notReadyDelay--;
		}
		if (!isStartGame)
		{
		}
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != -1)
			{
				avatar.updateAvatar();
			}
		}
		if (chatPublic != null && chatPublic.setOut())
		{
			chatPublic = null;
		}
	}

	public void updateCo()
	{
		if (!isStartGame)
		{
			updateReady();
			return;
		}
		if (turn == GameMidlet.avatar.IDDB)
		{
			center = cmdFire;
		}
		else
		{
			center = null;
		}
		right = null;
		if (onMainMenu.isOngame)
		{
			left = cmdMenu;
		}
		else
		{
			left = null;
		}
		if (dieTime != 0)
		{
			currentTime = Canvas.getTick();
			if (currentTime > dieTime)
			{
				dieTime = 0L;
			}
		}
	}

	public virtual void updateReady()
	{
		if (ownerID == GameMidlet.avatar.IDDB)
		{
			if (center != cmdWaiting)
			{
				center = cmdStart;
				cmdStart.caption = T.start;
			}
			bool flag = true;
			for (int i = 0; i < numPlayer; i++)
			{
				Avatar avatar = (Avatar)avatarInfos.elementAt(i);
				if (avatar.IDDB == -1)
				{
					flag = false;
				}
				else if (avatar.IDDB != GameMidlet.avatar.IDDB && !avatar.isReady)
				{
					flag = false;
				}
			}
			if (flag && Canvas.gameTick % 10 > 7)
			{
				cmdStart.caption = string.Empty;
			}
		}
		else
		{
			if (disableReady)
			{
				return;
			}
			center = cmdReady;
			cmdReady.caption = T.ready;
			for (int j = 0; j < numPlayer; j++)
			{
				Avatar avatar2 = (Avatar)avatarInfos.elementAt(j);
				if (avatar2.IDDB != GameMidlet.avatar.IDDB)
				{
					continue;
				}
				if (!avatar2.isReady)
				{
					if (Canvas.gameTick % 10 > 7)
					{
						cmdReady.caption = " ";
					}
					continue;
				}
				cmdReady.caption = T.noReady;
				if (notReadyDelay == 0)
				{
					center = cmdReady;
				}
				else
				{
					center = null;
				}
			}
		}
	}

	public override void keyPress(int keyCode)
	{
		ChatTextField.gI().startChat(keyCode, this);
		base.keyPress(keyCode);
	}

	public override void paint(MyGraphics g)
	{
		if (chatPublic != null)
		{
			chatPublic.paintAnimal(g);
		}
		base.paint(g);
		Canvas.loadMap.paintEffectCamera(g);
		Canvas.paintPlus2(g);
	}

	public virtual void paintNamePlayers(MyGraphics g)
	{
		int num = AvMain.hd;
		if (isStartGame || disableReady || onMainMenu.isOngame)
		{
			num = 1;
		}
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != -1)
			{
				avatar.paintIcon(g, avatar.x * num, avatar.y * num, false);
				avatar.paintName(g, avatar.x * num, avatar.y * num);
				paintReady(g, avatar.x * num, (avatar.y - 50 * ((num != 1) ? 1 : AvMain.hd)) * num - 10 * (num - 1), MyGraphics.HCENTER | MyGraphics.VCENTER, avatar);
			}
		}
	}

	public void paintChat(MyGraphics g)
	{
		for (int i = 0; i < avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != -1 && avatar.chat != null)
			{
				avatar.chat.paintAnimal(g);
			}
		}
	}

	public override void paintMain(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		g.setClip(0f, 0f, Canvas.w, Canvas.hCan);
		if (isStartGame || disableReady)
		{
			paintBgOngame(g);
			return;
		}
		Canvas.resetTrans(g);
		if (onMainMenu.isOngame)
		{
			paintBgOngame(g);
		}
	}

	public virtual void paintBgOngame(MyGraphics g)
	{
		Canvas.paint.paintDefaultBg(g);
		if (Canvas.load == -1)
		{
			if (Canvas.currentMyScreen != DiamondScr.me)
			{
				paintBoard(g);
			}
			if (!isStartGame)
			{
				Canvas.normalWhiteFont.drawString(g, "P: " + roomID + " - B: " + boardID, Canvas.hw, Canvas.h / 2 - 10 * AvMain.hd, 2);
				Canvas.smallFontYellow.drawString(g, money + T.dola, Canvas.hw, Canvas.h / 2 + 10 * AvMain.hd, 2);
			}
			else if (Canvas.currentMyScreen == DiamondScr.me)
			{
				DiamondScr.me.paintCaro(g);
			}
		}
	}

	private void paintBoard(MyGraphics g)
	{
		if (imgBoard != null)
		{
			g.drawImageScale(imgBoard, xBoard - wBoard / 2, yBoard - hBoard / 2, wBoard, hBoard, 0);
		}
	}

	public virtual void paintBgCo(MyGraphics g)
	{
		Canvas.paint.paintDefaultBg(g);
		Canvas.normalWhiteFont.drawString(g, "P: " + roomID + " - B: " + boardID, Canvas.hw, Canvas.h / 2 - 10 * AvMain.hd, 2);
		Canvas.smallFontYellow.drawString(g, money + T.dola, Canvas.hw, Canvas.h / 2 + 10 * AvMain.hd, 2);
	}

	public void paintNameRoom(MyGraphics g)
	{
		Canvas.smallFontYellow.drawString(g, RoomListOnScr.title, (int)(AvCamera.gI().xCam + (float)Canvas.hw), (int)(AvCamera.gI().yCam + (float)Canvas.hh - (float)AvMain.hSmall - (float)(AvMain.hSmall / 2) - 5f - (float)(10 * AvMain.hd) - 10f), 2);
		Canvas.smallFontYellow.drawString(g, "P: " + roomID + " - B: " + boardID, (int)(AvCamera.gI().xCam + (float)Canvas.hw), (int)(AvCamera.gI().yCam + (float)Canvas.hh - (float)(AvMain.hSmall / 2) - 5f - (float)(10 * AvMain.hd) - 10f), 2);
		Canvas.smallFontYellow.drawString(g, money + T.dola, (int)(AvCamera.gI().xCam + (float)Canvas.hw), (int)(AvCamera.gI().yCam + (float)Canvas.hh - 5f + (float)(AvMain.hSmall / 2) - (float)(10 * AvMain.hd) - 10f), 2);
		paintChat(g);
	}

	public virtual void paintPlayerCo(MyGraphics g)
	{
		for (int i = 0; i < avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (!avatar.name.Equals(string.Empty))
			{
				avatar.paintIcon(g, avatar.x, avatar.y, false);
				avatar.paintName(g, avatar.x, avatar.y);
				paintReady(g, avatar.x, avatar.y - 50 * AvMain.hd, MyGraphics.HCENTER | MyGraphics.VCENTER, avatar);
			}
		}
	}

	public void paintReady(MyGraphics g, int x, int y, int author, Avatar ava)
	{
		if (!isStartGame)
		{
			if (ava.IDDB == ownerID)
			{
				g.drawImage(imgReady[1], x, y, author);
			}
			else if (ava.isReady)
			{
				g.drawImage(imgReady[0], x, y, author);
			}
		}
	}

	public void paintName(MyGraphics g)
	{
		int num = 10;
		if (Canvas.stypeInt > 0)
		{
			num = -5;
		}
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB == -1)
			{
				continue;
			}
			string text = string.Empty;
			if (avatar.IDDB == turn && dieTime != 0)
			{
				long num2 = (currentTime - dieTime) / 1000;
				text += -num2;
			}
			int width = Canvas.arialFont.getWidth(avatar.name);
			if (GameMidlet.avatar.IDDB == avatar.IDDB)
			{
				Canvas.numberFont.drawString(g, text, Canvas.hw - width - 3, Canvas.hCan - Canvas.hTab - Canvas.numberFont.getHeight() + num, 1);
				int num3 = 0;
				int y = Canvas.hCan - Canvas.hTab - AvMain.hBlack + num;
				if (avatar.idImg != -1)
				{
					num3 = 6 * AvMain.hd;
					AvatarData.paintImg(g, avatar.idImg, Canvas.hw - Canvas.arialFont.getWidth(avatar.showName) / 2 - num3, y, 0);
				}
				Canvas.arialFont.drawString(g, avatar.showName, Canvas.hw + num3, y, 2);
				Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), Canvas.hw + width + 3, Canvas.hCan - Canvas.hTab - AvMain.hSmall + num, 0);
				if (avatar.chat != null)
				{
					avatar.chat.setPos(Canvas.hw, Canvas.h - 35 * AvMain.hd);
					avatar.chat.paintAnimal(g);
				}
			}
			else
			{
				Canvas.numberFont.drawString(g, text, Canvas.hw - width - 3, 2, 1);
				int num4 = 0;
				if (avatar.idImg != -1)
				{
					num4 = 6 * AvMain.hd;
					AvatarData.paintImg(g, avatar.idImg, Canvas.hw - Canvas.arialFont.getWidth(avatar.showName) / 2 - num4, 3, 0);
				}
				Canvas.arialFont.drawString(g, avatar.showName, Canvas.hw + num4, 2, 2);
				Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), Canvas.hw + Canvas.arialFont.getWidth(avatar.name) + 3, 6, 0);
				if (avatar.chat != null)
				{
					avatar.chat.setPos(Canvas.hw, 4 + avatar.chat.h);
					avatar.chat.paintAnimal(g);
				}
			}
		}
	}

	public void doViewMessage()
	{
		MessageScr.gI().switchToMe();
	}

	protected void doExit()
	{
		IAction action = new IActionExit(me);
		if (isStartGame && !disableReady)
		{
			Canvas.startOKDlg(T.doYouWantExit, action);
		}
		else
		{
			action.perform();
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			doMenu();
			break;
		case 1:
			me.doCloseBoard();
			break;
		case 2:
			me.doStartGame();
			break;
		case 3:
			me.doContinue();
			break;
		case 4:
			me.doFire();
			break;
		case 5:
			me.doReady();
			break;
		case 10:
			doExit();
			break;
		case 11:
			ChatTextField.gI().showTF();
			break;
		case 12:
			MapScr.gI().doEvent();
			break;
		case 100:
			try
			{
				int num = int.Parse(Canvas.inputDlg.getText());
				if (num >= 0)
				{
					Canvas.endDlg();
					GlobalService.gI().setMoney(roomID, boardID, num);
				}
				break;
			}
			catch (Exception)
			{
				break;
			}
		case 101:
			GlobalService.gI().setPassword(roomID, boardID, Canvas.inputDlg.getText());
			Canvas.startOKDlg(T.setPassed);
			break;
		}
	}

	protected void doSettingMoney()
	{
		ipKeyboard.openKeyBoard(T.numTienCuoc, ipKeyboard.NUMBERIC, string.Empty, new IActionSettingMoney(), false);
	}

	protected void doSetMaxPlayer()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < 3; i++)
		{
			myVector.addElement(new Command(T.numPlayer[i], new IActionMaxPlayer(2 + i)));
		}
		startMenu(myVector, 0);
	}

	public void doSettingPassword()
	{
		ipKeyboard.openKeyBoard(T.setPass, ipKeyboard.PASS, string.Empty, new IActionPass(), false);
	}

	protected void doKick()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != GameMidlet.avatar.IDDB && avatar.IDDB != -1)
			{
				myVector.addElement(new Command(avatar.showName, new IActionKick(avatar.IDDB)));
			}
		}
		startMenu(myVector, 0);
	}

	protected static void doAddFriend()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != GameMidlet.avatar.IDDB && avatar.IDDB != -1)
			{
				myVector.addElement(new Command(avatar.name, new IActionAddFriend(avatar)));
			}
		}
		startMenu(myVector, 0);
	}

	public void playerLeave(int leaveID)
	{
		Avatar avatarByID = getAvatarByID(leaveID);
		if (avatarByID != null)
		{
			addInfo(avatarByID.name + T.exited, 30, avatarByID.IDDB);
			avatarByID.IDDB = -1;
			avatarByID.setName(string.Empty);
			avatarByID.setExp(0);
			avatarByID.isReady = false;
		}
		setPosBoard();
		if (isStartGame || disableReady)
		{
			setPosPlaying();
		}
	}

	public static void setOwner(int newOwner)
	{
		ownerID = newOwner;
		Avatar avatarByID = getAvatarByID(ownerID);
		if (avatarByID != null)
		{
			avatarByID.isReady = true;
		}
	}

	public virtual void setPlayers(sbyte roomID1, sbyte boardID1, int ownerID, int money1, MyVector playerInfos)
	{
		initImg();
		roomID = roomID1;
		boardID = boardID1;
		money = money1;
		if (avatarInfos != null)
		{
			avatarInfos.removeAllElements();
		}
		avatarInfos = playerInfos;
		setOwner(ownerID);
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			avatar.direct = Base.RIGHT;
			avatar.setAction(2);
			avatar.setFrame(avatar.action);
			if (avatar.IDDB == GameMidlet.avatar.IDDB)
			{
				indexOfMe = (sbyte)i;
				break;
			}
		}
		setPosBoard();
		if (BoardListOnScr.type == BoardListOnScr.STYLE_4PLAYER)
		{
			Canvas.paint.initImgCard();
		}
		else
		{
			Canvas.paint.resetCasino();
		}
		Canvas.load = -1;
	}

	public void resetReady()
	{
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			avatar.isReady = false;
		}
	}

	public void setMoney(int money1)
	{
		money = money1;
		resetReady();
	}

	public void setReady(int id, bool isReady)
	{
		Avatar avatarByID = getAvatarByID(id);
		if (avatarByID == null)
		{
			return;
		}
		avatarByID.isReady = isReady;
		if (!onMainMenu.isOngame)
		{
			if (isReady)
			{
				avatarByID.ySat = -8;
				avatarByID.setAction(2);
			}
			else
			{
				avatarByID.ySat = 0;
				avatarByID.setAction(0);
			}
		}
	}

	public virtual void onChatFromMe(string text)
	{
		if (!text.Trim().Equals(string.Empty))
		{
			CasinoService.gI().chatToBoard(text);
			showChat(GameMidlet.avatar.IDDB, text);
		}
	}

	public static void showChat(int fromID, string text)
	{
		Avatar avatarByID = getAvatarByID(fromID);
		Avatar avatar = new Avatar();
		if (avatarByID == null)
		{
			return;
		}
		avatar.x = avatarByID.x;
		avatar.y = avatarByID.y;
		avatar.IDDB = avatarByID.IDDB;
		if (avatar == null || avatar.IDDB == -1)
		{
			return;
		}
		if ((isStartGame || onMainMenu.isOngame) && BoardListOnScr.type == BoardListOnScr.STYLE_2PLAYER)
		{
			avatar.x = Canvas.hw;
			if (avatar.IDDB != GameMidlet.avatar.IDDB)
			{
				avatar.y = 30;
			}
			else
			{
				avatar.y = Canvas.h - 40 * AvMain.hd;
			}
		}
		addInfo(text, 50, avatar.IDDB);
	}

	public static void showFlyText(int fromID, int money)
	{
		if (money != 0)
		{
			if (!isStartGame)
			{
				int indexByID = getIndexByID(fromID);
				Canvas.addFlyText(money, posAvatar[indexPlayer[indexByID]].x, posAvatar[indexPlayer[indexByID]].y, -1, -1);
			}
			else
			{
				Avatar avatarByID = getAvatarByID(fromID);
				Canvas.addFlyText(money, avatarByID.x, avatarByID.y, -1, -1);
			}
		}
	}

	public static bool setR_B(sbyte roomID1, sbyte boardID1)
	{
		if (roomID == roomID1 && boardID == boardID1)
		{
			return true;
		}
		return false;
	}

	public virtual void start(int whoMoveFirst, int interval2)
	{
		setPosBoard();
	}

	public virtual void move(int whoMove, sbyte x, sbyte y, int nextMove)
	{
	}

	public static void addInfo(string info, int time, int id)
	{
		if (id == -1)
		{
			if (chatPublic == null)
			{
				chatPublic = new ChatPopup(time, info, 0);
				chatPublic.setPos(Canvas.hw, Canvas.hh - 20);
			}
			else
			{
				chatPublic.prepareData(time, info);
			}
			return;
		}
		for (int i = 0; i < avatarInfos.size(); i++)
		{
			Base @base = (Base)avatarInfos.elementAt(i);
			if (@base.IDDB == id)
			{
				if (@base.chat == null)
				{
					@base.chat = new ChatPopup(time, info, 0);
					@base.chat.setPos(@base.x, @base.y - 65 * AvMain.hd);
				}
				else
				{
					@base.chat.prepareData(time, info);
				}
			}
		}
	}

	public void setPosBoard()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			avatar.setAction(0);
			if (avatar.IDDB != -1)
			{
				num++;
				if (avatar.IDDB != GameMidlet.avatar.IDDB)
				{
					num2 = i;
				}
			}
		}
		int[] array = new int[numPlayer];
		int num3 = 2;
		if (num == 2)
		{
			array[indexOfMe] = 2;
			array[num2] = 0;
		}
		else
		{
			for (int j = indexOfMe; j < indexOfMe + numPlayer; j++)
			{
				int num4 = j;
				if (num4 > numPlayer - 1)
				{
					num4 -= numPlayer;
				}
				array[num4] = num3;
				num3++;
				if (num3 >= numPlayer)
				{
					num3 = 0;
				}
			}
		}
		indexPlayer = array;
	}

	public static Avatar getAvatarByID(int id)
	{
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB == id)
			{
				return avatar;
			}
		}
		return null;
	}

	public static int getIndexByID(int id)
	{
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB == id)
			{
				return i;
			}
		}
		return -1;
	}

	public virtual void setPosPlaying()
	{
		for (int i = 0; i < numPlayer; i++)
		{
			Avatar avatar = (Avatar)avatarInfos.elementAt(i);
			if (avatar.IDDB != -1)
			{
				avatar.ySat = 0;
				avatar.setAction(0);
				avatar.setFrame(avatar.action);
				avatar.xCur = (avatar.x = posAvatar[indexPlayer[i]].x);
				avatar.yCur = (avatar.y = posAvatar[indexPlayer[i]].y);
				if (indexPlayer[i] == 2 || indexPlayer[i] == 3)
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
