using System;

public class Avatar : Base
{
	public sbyte cFrame;

	public int[] money = new int[4] { 0, -1, 0, 0 };

	public int luongKhoa;

	public string strMoney = string.Empty;

	public sbyte gender;

	public sbyte maxJump;

	public MyVector seriPart;

	public MyVector emotionList;

	public MyVector moveList = new MyVector();

	public sbyte ySat;

	public int friendShip;

	public int idFrom;

	public int idTo;

	public int idGift;

	public string showName;

	public string text2;

	public int exp;

	public bool isReady;

	public bool isLeave;

	public sbyte typeHome = -1;

	public sbyte perLvMain;

	public sbyte perLvFarm;

	public sbyte dirFirst;

	public short lvFarm = -1;

	public short lvMain;

	public bool isLoad;

	public bool isSetAction;

	public const sbyte HEART = -6;

	public const sbyte POINTER = -5;

	public const sbyte BUS_WAIT = -4;

	public const sbyte GIFT = -3;

	public const sbyte HIT = -2;

	public const sbyte SAT_DOWN_STAND = 5;

	public const sbyte NAM_NGHI = 4;

	public const sbyte ANEMONES = 6;

	public const sbyte HANDSHAKE_LEFT = 10;

	public const sbyte SURRENDER = 7;

	public const sbyte GIFT_GIVING = 9;

	public const sbyte RECEIVING_GIFT = 8;

	public const sbyte QUY_CHO = 11;

	public const sbyte QUY_NHAN = 12;

	public const sbyte NGOI_NHAN = 13;

	public int task;

	public int isJumps = -1;

	private int numSleep;

	public const sbyte NOT_FEEL = 4;

	public const sbyte SAD = 5;

	public const sbyte FUNNY = 6;

	public const sbyte DA_LONG_NHEO = 7;

	public const sbyte DAN = 8;

	public const sbyte CRY = 9;

	public const sbyte KISS = 11;

	public const sbyte OTHER = 12;

	public short feel = 4;

	public short numFeel;

	public short firFeel;

	public short wName;

	public short nFrame;

	public static int iHit;

	public short idPet = -1;

	public short hungerPet;

	public short idImg = -1;

	public short timeTask;

	public short idWedding = -1;

	public short idStatus = -1;

	private int angle;

	public sbyte blogNews = -1;

	public bool isHit;

	public bool isNo;

	public static FrameImage imgBlog;

	public sbyte fight;

	public sbyte countDefent = -1;

	public short hp = 1000;

	public short mp = 300;

	public short plusHP;

	public short plusMP;

	public short maxHP = 1000;

	public short maxMP = 1000;

	public short defence;

	public Avatar focus;

	public static FrameImage imgHit;

	public static FrameImage imgKiss;

	public sbyte timeHit;

	public Kiss kiss;

	public string[] textChat;

	public int countChat;

	public sbyte[] indexP;

	public static sbyte I_FRIENDLY;

	public static sbyte I_CRAZY;

	public static sbyte I_STYLISH;

	public static sbyte I_HAPPY;

	public static sbyte I_HUNGER;

	public static sbyte[][] FRAME;

	private static sbyte[] duX;

	public short timeEmotion;

	private int indexChat = -1;

	private int pa;

	private int pb;

	public Avatar()
	{
		catagory = 0;
		height = 42;
		cFrame = (sbyte)CRes.rnd(9);
		maxJump = (sbyte)(CRes.rnd(30) + 10);
	}

	static Avatar()
	{
		iHit = 0;
		I_FRIENDLY = 0;
		I_CRAZY = 1;
		I_STYLISH = 2;
		I_HAPPY = 3;
		I_HUNGER = 4;
		FRAME = new sbyte[15][];
		duX = new sbyte[3] { -3, 0, 1 };
		FRAME[0] = new sbyte[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
		FRAME[1] = new sbyte[10] { 0, 0, 0, 0, 0, 2, 2, 2, 2, 2 };
		FRAME[2] = new sbyte[10] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
		FRAME[3] = new sbyte[10] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
		FRAME[4] = new sbyte[10] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
		FRAME[5] = new sbyte[10] { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 };
		FRAME[6] = new sbyte[10] { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 };
		FRAME[7] = new sbyte[10] { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
		FRAME[8] = new sbyte[10] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
		FRAME[9] = new sbyte[10] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
		FRAME[10] = new sbyte[10] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 };
		FRAME[11] = new sbyte[10] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
		FRAME[12] = new sbyte[10] { 13, 13, 13, 13, 13, 13, 13, 13, 13, 13 };
		FRAME[13] = new sbyte[10] { 14, 14, 14, 14, 14, 14, 14, 14, 14, 14 };
		FRAME[14] = new sbyte[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
	}

	public int getMoney()
	{
		return money[0];
	}

	public int getMoneyNew()
	{
		if (MapScr.isNewVersion)
		{
			return money[3];
		}
		return money[0];
	}

	public void setMoneyNew(int mo)
	{
		if (MapScr.isNewVersion)
		{
			money[3] = mo;
		}
		else
		{
			money[0] = mo;
		}
	}

	public void setMoney(int mo)
	{
		money[0] = mo;
		strMoney = Canvas.getMoneys(money[0]) + T.dola;
	}

	public void setGold(int gold)
	{
		money[2] = gold;
	}

	public override void paint(MyGraphics g)
	{
		if ((float)((x + 15) * MyObject.hd) < AvCamera.gI().xCam || (float)((x - 15) * MyObject.hd) > AvCamera.gI().xCam + (float)Canvas.w || ableShow || Canvas.currentMyScreen == MainMenu.gI() || Canvas.currentMyScreen == ParkListSrc.gI() || Canvas.currentMyScreen == MessageScr.me || Canvas.currentMyScreen == ListScr.instance)
		{
			return;
		}
		if (action != 14)
		{
			g.drawImage(LoadMap.imgShadow, (x + ((direct != Base.LEFT) ? (-2) : 2)) * MyObject.hd, (y - 1) * MyObject.hd, 3);
		}
		int num = seriPart.size();
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			Part part = AvatarData.getPart(((SeriPart)seriPart.elementAt(i)).idPart);
			if (part == null || (action == 14 && part.zOrder != 30 && part.zOrder != 40 && part.zOrder != 50))
			{
				continue;
			}
			if (part.zOrder == 40)
			{
				if (feel != 4)
				{
					part = AvatarData.getPart(feel);
				}
				else if ((feel == 4 || feel == 6) && nFrame < 1 + gender)
				{
					flag = true;
				}
			}
			if ((LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53) && !AvatarData.isZOrderMain(part.zOrder) && part.zOrder != 52)
			{
				continue;
			}
			part.paintAvatar(g, frame, x * MyObject.hd, (y + vh + ySat) * MyObject.hd, direct, 0);
			if (flag)
			{
				flag = false;
				Part part2 = AvatarData.getPart(606);
				if ((LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53) || AvatarData.isZOrderMain(part2.zOrder) || part2.zOrder == 52)
				{
					part2.paintAvatar(g, frame, x * MyObject.hd, (y + vh + ySat) * MyObject.hd, direct, 0);
				}
			}
		}
		if ((OptionScr.gI().mapFocus[0] == 0 || this == LoadMap.focusObj) && LoadMap.TYPEMAP != 24)
		{
			paintName(g, x * MyObject.hd, y * MyObject.hd - AvMain.hSmall);
		}
		if (kiss != null)
		{
			kiss.paint(g);
		}
		if (timeHit > 0 && task == -2)
		{
			imgHit.drawFrame((Canvas.gameTick % 6 < 3) ? 1 : 0, x * MyObject.hd, y * MyObject.hd - height / 2, 0, 3, g);
		}
		if (Canvas.currentMyScreen != MainMenu.gI())
		{
			base.paint(g);
		}
	}

	public override void paintIcon(MyGraphics g, int x, int y, bool isName)
	{
		g.drawImage(LoadMap.imgShadow, x + ((direct != Base.LEFT) ? (-2) : 2), y - 1, 3);
		if (this.seriPart != null)
		{
			int num = this.seriPart.size();
			for (int i = 0; i < num; i++)
			{
				SeriPart seriPart = (SeriPart)this.seriPart.elementAt(i);
				Part part = AvatarData.getPart(seriPart.idPart);
				if (part != null)
				{
					if (part.zOrder == 40 && feel != 4)
					{
						part = AvatarData.getPart(feel);
					}
					part.paintAvatar(g, frame, x, y, direct, 0);
				}
			}
		}
		if (isName)
		{
			paintName(g, x, y - AvMain.hSmall);
		}
		base.paint(g);
	}

	public void paintName(MyGraphics g, int x, int y)
	{
		int num = 0;
		int num2 = y - height * MyObject.hd + (vh + ySat) * MyObject.hd;
		if (idImg != -1)
		{
			num = 7;
			AvatarData.paintImg(g, idImg, x + duX[direct] * MyObject.hd - wName / 2, num2 + Canvas.smallFontRed.getHeight() / 2, 3);
		}
		int num3 = x + (duX[direct] + num) * MyObject.hd;
		if (idWedding != -1)
		{
			AvatarData.paintImg(g, idWedding, num3 + wName / 2 + 7 * MyObject.hd, num2 + AvMain.hSmall / 2, 3);
		}
		if (blogNews != -1)
		{
			imgBlog.drawFrame(blogNews, num3 + wName / 2 + 7 * MyObject.hd, num2 + 3, 0, 3, g);
		}
		if (IDDB == GameMidlet.avatar.IDDB)
		{
			Canvas.smallFontRed.drawString(g, name, num3, num2, 2);
		}
		else
		{
			Canvas.smallFontYellow.drawString(g, name, num3, num2, 2);
		}
	}

	public void setExp(int exp)
	{
		this.exp = exp;
		int num = 1;
		int num2 = exp;
		while (true)
		{
			int num3 = num * 100;
			num2 = exp;
			int num4 = exp - num3;
			if (num4 >= 0)
			{
				num++;
				exp = num4;
				continue;
			}
			break;
		}
		lvMain = (sbyte)num;
		perLvMain = (sbyte)(num2 * 100 / (num * 100));
	}

	public void addSeri(SeriPart seri)
	{
		if (seriPart == null)
		{
			seriPart = new MyVector();
		}
		seriPart.addElement(seri);
	}

	public void setName(string name)
	{
		base.name = name;
		if (name.Length > 7)
		{
			showName = name.Substring(0, 6) + "..";
		}
		else
		{
			showName = name;
		}
		wName = (short)Canvas.smallFontYellow.getWidth(name);
	}

	public void setFeel(int f)
	{
		feel = (short)f;
	}

	public override void update()
	{
		if (kiss != null)
		{
			kiss.update();
		}
		if (isLoad && Canvas.gameTick % 20 == 10)
		{
			orderSeriesPath();
		}
		if (isLeave)
		{
			updateLeave();
		}
		updateAvatar();
		if (emotionList == null)
		{
			return;
		}
		for (int i = 0; i < emotionList.size(); i++)
		{
			Emotion emotion = (Emotion)emotionList.elementAt(i);
			if (emotion.time == timeEmotion)
			{
				timeEmotion = 0;
				feel = emotion.id;
				emotionList.removeElement(emotion);
				break;
			}
		}
		timeEmotion++;
	}

	private void updateLeave()
	{
		if ((!MapScr.isWedding || (IDDB != MapScr.idUserWedding_1 && IDDB != MapScr.idUserWedding_2)) && moveList.size() == 0 && x == xCur && y == yCur)
		{
			LoadMap.removePlayer(this);
			if (MapScr.focusP != null && MapScr.focusP.IDDB == IDDB)
			{
				MapScr.focusP = null;
				LoadMap.focusObj = null;
			}
		}
	}

	public void setFrame(int action)
	{
		if (action < 0)
		{
			frame = FRAME[0][cFrame];
		}
		else
		{
			frame = FRAME[action][cFrame];
		}
	}

	public void updateFrame()
	{
		if (nFrame < 1)
		{
			nFrame = (short)(10 + CRes.rnd(70) / (gender + 1));
		}
		nFrame--;
		cFrame++;
		if (cFrame >= 10)
		{
			cFrame = 0;
		}
		if (action < 0)
		{
			frame = FRAME[0][cFrame];
		}
		else if (action < FRAME.Length)
		{
			frame = FRAME[action][cFrame];
		}
	}

	public void updateAvatar()
	{
		updateFrame();
		if (numFeel != 0 || feel == 11 || feel == 7 || feel == 9)
		{
			if (numFeel == 0)
			{
				firFeel = feel;
			}
			numFeel++;
			if (numFeel % 10 > 5)
			{
				if (numFeel > 45)
				{
					numFeel = 0;
				}
				setFeel(4);
			}
			else
			{
				setFeel(firFeel);
			}
		}
		move();
		x += vx;
		y += vy;
		vh += vhy;
		if (action == 10)
		{
			vhy++;
		}
		if (Math.abs(vhy) >= g || Math.abs(vh) > 28)
		{
			Out.println("11111111111");
			action = 0;
			vhy = 0;
			vh = 0;
		}
		if (isJumps != -1 && action == 0)
		{
			isJumps++;
			if (isJumps > maxJump)
			{
				isJumps = -1;
			}
			else if (isJumps % 6 == 0)
			{
				doJumps();
			}
		}
		if (action == 0)
		{
			ySat = 0;
		}
		if (action == 1 && vx == 0 && vy == 0)
		{
			action = 0;
		}
		vx = 0;
		vy = 0;
		if (timeHit > 0)
		{
			timeHit--;
			if (timeHit == 0)
			{
				if (task == -2)
				{
					focus.action = 4;
					focus.feel = 20;
					action = 4;
					feel = 20;
				}
				else if (task == 11)
				{
					feel = 12;
					focus.feel = 12;
					kiss = null;
				}
				task = 0;
				focus.task = 0;
				focus = null;
			}
		}
		if (textChat != null)
		{
			if (chat == null)
			{
				if (countChat != -1 && Canvas.getTick() / 1000 - countChat > 1)
				{
					indexChat++;
					if (indexChat >= textChat.Length)
					{
						indexChat = 0;
					}
					countChat = -1;
					chat = new ChatPopup(100, textChat[indexChat], (sbyte)((idFrom >= 2000000000) ? 1 : 0));
					chat.setPos(x, y - 45);
				}
			}
			else
			{
				countChat = (int)(Canvas.getTick() / 1000);
			}
		}
		base.update();
	}

	public void updateKey()
	{
		if (action != -1 && !ableShow && task == 0)
		{
			vx = 0;
			vy = 0;
			numSleep = 0;
			if (vx == 0 && vy == 0 && action == 1)
			{
				action = 0;
				Out.println("33333333333333333333");
			}
		}
	}

	public void doAction(sbyte a)
	{
		if (action == 10)
		{
			return;
		}
		if (a == 2 || a == 13 || a == 4)
		{
			int num = LoadMap.type[(y - 15) / LoadMap.w * LoadMap.wMap + x / LoadMap.w];
			switch (num)
			{
			case 54:
			case 79:
			case 81:
				ySat = -6;
				if (num == 81)
				{
					if (a != 4)
					{
						ySat = (sbyte)(-6 * MyObject.hd);
					}
					else
					{
						ySat = 0;
					}
				}
				break;
			case 67:
			case 92:
				ySat = -10;
				break;
			}
			action = a;
		}
		else if (action != 14 && a != 14)
		{
			action = 0;
		}
		if (a == 10)
		{
			doJumps();
		}
		else if (IDDB != GameMidlet.avatar.IDDB)
		{
			moveList.addElement(new AvPosition(-1, -1, a));
		}
		else
		{
			action = a;
		}
	}

	private void updateTask()
	{
		if (task == 0 || task == -5)
		{
			return;
		}
		if (IDDB == idTo && LoadMap.getAvatar(idFrom) == null)
		{
			task = 0;
			idFrom = -1;
		}
		else
		{
			if (idFrom == -1 || idTo == -1)
			{
				return;
			}
			Avatar avatar = LoadMap.getAvatar(idFrom);
			Avatar avatar2 = LoadMap.getAvatar(idTo);
			if (avatar2 == null || avatar == null)
			{
				if (avatar != null)
				{
					avatar.task = 0;
					avatar.ableShow = false;
				}
				if (avatar2 != null)
				{
					avatar2.task = 0;
					avatar2.ableShow = false;
				}
				return;
			}
			if (avatar2.x > avatar.x)
			{
				avatar2.direct = (avatar2.dirFirst = Base.LEFT);
				avatar.direct = (avatar.dirFirst = Base.RIGHT);
			}
			else
			{
				avatar2.direct = (avatar2.dirFirst = Base.RIGHT);
				avatar.direct = (avatar.dirFirst = Base.LEFT);
			}
			if (IDDB != idFrom)
			{
				return;
			}
			if (timeTask > 0)
			{
				timeTask--;
				return;
			}
			switch (task)
			{
			case 9:
				if (this == GameMidlet.avatar)
				{
					MapScr.doAction(9);
				}
				else if (GameMidlet.avatar.task == 8 && IDDB == GameMidlet.avatar.idFrom)
				{
					MapScr.doAction(8);
					GameMidlet.avatar.task = 0;
				}
				MapScr.gI().setGifts(avatar2);
				task = 0;
				avatar2.task = 0;
				break;
			case -3:
				if (LoadMap.weather == -1)
				{
					AnimateEffect animateEffect = new AnimateEffect(2, true, 0);
					animateEffect.show();
				}
				task = 0;
				avatar2.task = 0;
				break;
			case 12:
				avatar2.setTask(0);
				setTask(0);
				break;
			}
			idGift = -1;
			idFrom = -1;
			idTo = -1;
		}
	}

	public void setPosTo(int x0, int y0)
	{
		xCur = x0;
		yCur = y0;
		setDir(x0);
	}

	public void setDir(int x0)
	{
		if (x0 > x)
		{
			direct = Base.RIGHT;
		}
		else if (x0 < x)
		{
			direct = Base.LEFT;
		}
	}

	public void move()
	{
		if ((this == GameMidlet.avatar && task == 0 && Canvas.currentMyScreen != BoardScr.me) || action == 10)
		{
			return;
		}
		if (CRes.distance(x, y, xCur, yCur) <= v)
		{
			if (focus != null && timeHit == 0)
			{
				if (task == -2)
				{
					timeHit = 20;
				}
				else if (task == 11)
				{
					timeHit = 30;
					feel = 107;
					focus.feel = 107;
					kiss = new Kiss(x, y);
				}
			}
			if (task == -5)
			{
				dirFirst = direct;
				x = xCur;
				y = yCur;
				if (LoadMap.nPath <= 0)
				{
					task = 0;
				}
				if (Canvas.currentDialog == null)
				{
					MapScr.gI().doMove(x, y, direct);
				}
			}
			else
			{
				if (IDDB != GameMidlet.avatar.IDDB)
				{
					xCur = x;
					yCur = y;
				}
				if (moveList.size() == 0)
				{
					if (action == 1)
					{
						action = 0;
					}
					direct = dirFirst;
				}
				else
				{
					AvPosition avPosition = (AvPosition)moveList.elementAt(0);
					setPosTo(avPosition.x, avPosition.y);
					if (xCur == -1 && yCur == -1)
					{
						xCur = x;
						yCur = y;
						if (action == 14)
						{
							LoadMap.type[y / LoadMap.w * LoadMap.wMap + x / LoadMap.w] = 112;
						}
						action = (sbyte)avPosition.anchor;
						if (action == 14)
						{
							LoadMap.type[y / LoadMap.w * LoadMap.wMap + x / LoadMap.w] = 90;
						}
						setLay();
					}
					else
					{
						dirFirst = (sbyte)avPosition.anchor;
						direct = dirFirst;
					}
					moveList.removeElementAt(0);
				}
			}
			updateTask();
			return;
		}
		angle = CRes.angle(xCur - x, -(yCur - y));
		int num = v * CRes.cos(angle) >> 10;
		int num2 = -(v * CRes.sin(angle)) >> 10;
		if (isSetAction && task == -5 && GameMidlet.avatar.setLayPLayer(x + num, y + num2))
		{
			LoadMap.resetPath();
			vx = (vy = 0);
			return;
		}
		vx = num;
		vy = num2;
		vhy = 0;
		vh = 0;
		ySat = 0;
		setDir(x + num);
		if (x != xCur)
		{
			resetTypeChair();
		}
		if (y != yCur)
		{
			resetTypeChair();
		}
		action = 1;
	}

	private void setLay()
	{
		if (action != 2 && action != 13 && action != 4)
		{
			return;
		}
		int num = (y - LoadMap.w) / LoadMap.w * LoadMap.wMap + x / LoadMap.w;
		if (num < 0 || num >= LoadMap.type.Length)
		{
			return;
		}
		if (action == 4)
		{
			int num2 = LoadMap.type[num];
			if (num2 == 67)
			{
				ySat = -10;
			}
		}
		int num3 = LoadMap.getposMap(x, y - 10);
		yCur = y;
		if (num3 != -1)
		{
			int num4 = LoadMap.type[num3];
			if (num4 == 92)
			{
				ySat = -10;
			}
			if (num4 == 79 || num4 == 92 || num4 == 90 || num4 == 54)
			{
				LoadMap.type[num3] = 90;
			}
		}
	}

	public void resetTypeChair()
	{
		if (action != 2 && action != 13)
		{
			return;
		}
		int num = LoadMap.getposMap(x, y - 18);
		if (num == -1)
		{
			return;
		}
		action = 0;
		ySat = 0;
		int num2 = LoadMap.type[num];
		if (num2 == 90)
		{
			if (LoadMap.map[num] == 80)
			{
				LoadMap.type[num] = 92;
			}
			else if (LoadMap.map[num] == 97)
			{
				LoadMap.type[num] = 54;
			}
			else
			{
				LoadMap.type[num] = 79;
			}
		}
	}

	public void resetNam_nghi(int vx, int vy)
	{
		if (action != 4)
		{
			return;
		}
		int num = LoadMap.getposMap(x, y - 18);
		if (num != -1)
		{
			int typeMap = LoadMap.getTypeMap(x + vx * 12, y + vy * 12 - 10);
			if (typeMap == 80)
			{
				action = 0;
				ySat = 0;
			}
		}
	}

	public bool doJoin(int vX, int vY)
	{
		if (action == -1 || Canvas.currentDialog != null)
		{
			return false;
		}
		bool flag = Canvas.loadMap.doJoin(x + vX, y + vY);
		if (flag && action == 1)
		{
			action = 0;
		}
		return flag;
	}

	public bool detectCollisionMap(int vX, int vY)
	{
		bool flag = detectCollision(vX, vY);
		if (flag)
		{
			setWay(vX, vY);
		}
		return flag;
	}

	public bool setLayPLayer(int vX, int vY)
	{
		if ((action != 0 && action != 1) || (task != 0 && task != -5))
		{
			return false;
		}
		switch (LoadMap.type[vY / LoadMap.w * LoadMap.wMap + vX / LoadMap.w])
		{
		case 54:
		case 79:
		case 81:
			action = 2;
			ySat = -6;
			x = vX / LoadMap.w * LoadMap.w + LoadMap.w / 2;
			y = vY / LoadMap.w * LoadMap.w + LoadMap.w - 1;
			MapScr.gI().doMove(x, y, direct);
			MapScr.doAction(action);
			return true;
		case 92:
			action = 2;
			ySat = -10;
			x = vX / LoadMap.w * LoadMap.w + LoadMap.w / 2;
			y = vY / LoadMap.w * LoadMap.w + LoadMap.w - 1;
			MapScr.gI().doMove(x, vY, direct);
			MapScr.doAction(2);
			return true;
		case 67:
			action = 4;
			ySat = -10;
			x = vX / LoadMap.w * LoadMap.w + LoadMap.w / 2;
			MapScr.gI().doMove(x, vY, direct);
			MapScr.doAction(4);
			return true;
		default:
			return false;
		}
	}

	public void doJumps()
	{
		if (action == 0 || action == 1)
		{
			action = 10;
			if (isJumps == -1)
			{
				isJumps = 0;
			}
			vhy = (sbyte)(-g);
		}
	}

	public void orderSeriesPath()
	{
		isLoad = false;
		try
		{
			for (int i = 0; i < this.seriPart.size() - 1; i++)
			{
				SeriPart seriPart = (SeriPart)this.seriPart.elementAt(i);
				if (AvatarData.getPart(seriPart.idPart) == null)
				{
					continue;
				}
				for (int j = i + 1; j < this.seriPart.size(); j++)
				{
					SeriPart seriPart2 = (SeriPart)this.seriPart.elementAt(j);
					if (AvatarData.getPart(seriPart2.idPart).IDPart == -1)
					{
						isLoad = true;
					}
					if (AvatarData.getPart(seriPart2.idPart) != null && AvatarData.getPart(seriPart.idPart).zOrder > AvatarData.getPart(seriPart2.idPart).zOrder)
					{
						this.seriPart.setElementAt(seriPart, j);
						this.seriPart.setElementAt(seriPart2, i);
						seriPart = seriPart2;
					}
				}
			}
		}
		catch (Exception)
		{
			isLoad = true;
		}
	}

	public void setAction(sbyte ac)
	{
		action = ac;
	}

	public void setTask(int task)
	{
		this.task = task;
	}

	public void addSeriPart(SeriPart seri)
	{
		Part part = AvatarData.getPart(seri.idPart);
		if (part != null && part.follow != -2)
		{
			SeriPart seriByZ = AvatarData.getSeriByZ(part.zOrder, seriPart);
			if (seriByZ != null)
			{
				seriPart.removeElement(seriByZ);
			}
			seriPart.addElement(seri);
		}
	}

	public void initPet()
	{
		try
		{
			for (int i = 0; i < this.seriPart.size(); i++)
			{
				SeriPart seriPart = (SeriPart)this.seriPart.elementAt(i);
				if (AvatarData.getPart(seriPart.idPart).zOrder == -1)
				{
					this.seriPart.removeElement(seriPart);
					idPet = seriPart.idPart;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void setPet()
	{
		initPet();
		LoadMap.setPet(this);
	}

	public void changePet(short idPart)
	{
		idPet = idPart;
		Pet pet = LoadMap.getPet(IDDB);
		if (pet != null)
		{
			LoadMap.removePlayer(pet);
			idPet = idPart;
		}
		setPet();
	}

	public void resetAction()
	{
		vhy = 0;
		vh = 0;
		action = 0;
	}

	public void addPart(int idPart, int zOrder)
	{
		for (int i = 0; i < this.seriPart.size() - 1; i++)
		{
			SeriPart seriPart = (SeriPart)this.seriPart.elementAt(i);
			Part part = AvatarData.getPart(seriPart.idPart);
			if (zOrder == part.zOrder)
			{
				this.seriPart.removeElement(seriPart);
				break;
			}
		}
		addSeri(new SeriPart((short)idPart));
	}

	public void updateMoney(int xu, int luong, int luongK)
	{
		if (money[0] != xu)
		{
			Canvas.addFlyTextSmall(xu - money[0] + "xu", x, y, -1, 0, -1);
			money[0] = xu;
		}
		if (money[2] != luong)
		{
			Canvas.addFlyTextSmall(luong - money[2] + "luong", x, y, -1, 0, -1);
			money[2] = luong;
		}
		if (luongKhoa != luongK)
		{
			Canvas.addFlyTextSmall(luongK - luongKhoa + "luong", x, y, -1, 0, -1);
			luongKhoa = luongK;
		}
	}
}
