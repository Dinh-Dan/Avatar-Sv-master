using UnityEngine;

public class DiamondScr : BoardScr
{
	public new static DiamondScr me;

	private Point[][] array = new Point[8][];

	private int x;

	private int y;

	private int wCell;

	private int iSelected;

	private int clearCount = -1;

	private int xPlayer1;

	private int xPlayer2;

	private sbyte countHit = -1;

	private MyVector listFireWork = new MyVector();

	private bool isPath;

	private bool isTrans;

	private Command cmdSelected;

	private Command cmdSkip;

	private Command cmdCenter;

	private FrameImage imgFireWork;

	private sbyte wImg;

	public new int selected;

	public int idWin = -1;

	private bool isEnd;

	public bool isNo;

	private int yStart;

	private int[][] xCheck = new int[16][];

	private int[][] yCheck = new int[16][];

	private int[][] xSetSelected = new int[6][];

	private int[][] ySetSelected = new int[6][];

	public bool isInit;

	public bool isHd;

	private sbyte[][] arr;

	private bool isJoin;

	private ImageIcon imgSeleced;

	private ImageIcon imgDiamond;

	private bool isTranCam;

	private int hhFill;

	private int countSelected;

	private MyVector listSmall = new MyVector();

	private bool isMove;

	private bool ableMove;

	public DiamondScr()
	{
		xSetSelected[0] = new int[2] { -1, -2 };
		xSetSelected[1] = new int[2];
		xSetSelected[2] = new int[2] { 1, 2 };
		xSetSelected[3] = new int[2];
		xSetSelected[4] = new int[2] { -1, 1 };
		xSetSelected[5] = new int[2];
		ySetSelected[0] = new int[2];
		ySetSelected[1] = new int[2] { -1, -2 };
		ySetSelected[2] = new int[2];
		ySetSelected[3] = new int[2] { 1, 2 };
		ySetSelected[4] = new int[2];
		ySetSelected[5] = new int[2] { -1, 1 };
		yCheck[0] = new int[2];
		yCheck[1] = new int[2] { 0, -1 };
		yCheck[2] = new int[2] { 0, 1 };
		yCheck[3] = new int[2];
		yCheck[4] = new int[2] { 0, -1 };
		yCheck[5] = new int[2] { 0, 1 };
		yCheck[6] = new int[2] { 1, 3 };
		yCheck[7] = new int[2] { 1, 2 };
		yCheck[8] = new int[2] { 1, 2 };
		yCheck[9] = new int[2] { 1, -2 };
		yCheck[10] = new int[2] { 1, -1 };
		yCheck[11] = new int[2] { 1, -1 };
		yCheck[12] = new int[2] { -1, -1 };
		yCheck[13] = new int[2] { 1, 1 };
		yCheck[14] = new int[2] { -1, 1 };
		yCheck[15] = new int[2] { -1, 1 };
		xCheck[0] = new int[2] { 1, -2 };
		xCheck[1] = new int[2] { 1, -1 };
		xCheck[2] = new int[2] { 1, -1 };
		xCheck[3] = new int[2] { 1, 3 };
		xCheck[4] = new int[2] { 1, 2 };
		xCheck[5] = new int[2] { 1, 2 };
		xCheck[6] = new int[2];
		xCheck[7] = new int[2] { 0, -1 };
		xCheck[8] = new int[2] { 0, 1 };
		xCheck[9] = new int[2];
		xCheck[10] = new int[2] { 0, -1 };
		xCheck[11] = new int[2] { 0, 1 };
		xCheck[12] = new int[2] { -1, 1 };
		xCheck[13] = new int[2] { -1, 1 };
		xCheck[14] = new int[2] { -1, -1 };
		xCheck[15] = new int[2] { 1, 1 };
		for (int i = 0; i < 8; i++)
		{
			array[i] = new Point[8];
		}
		cmdSelected = new Command(T.selectt, 20);
		cmdSkip = new Command(T.skip, 21);
		imgFireWork = new FrameImage(Image.createImage(T.getPath() + "/dialLucky/st"), 11 * AvMain.hd, 11 * AvMain.hd);
	}

	public static DiamondScr gI()
	{
		return (me != null) ? me : (me = new DiamondScr());
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 20:
			doSelected();
			break;
		case 21:
			doSkip();
			break;
		default:
			base.commandTab(index);
			break;
		}
	}

	public void doSkip()
	{
		CasinoService.gI().doSkipDaimond();
		turn = -1;
		cmdCenter = BoardScr.cmdWaiting;
		right = null;
	}

	protected void doSelected()
	{
		if (!isPath)
		{
			if (iSelected == -1 && cmdCenter == cmdSelected && turn == GameMidlet.avatar.IDDB && !isTrans)
			{
				iSelected = selected;
			}
			else
			{
				iSelected = -1;
			}
		}
	}

	public override void init()
	{
		base.init();
		if (!isJoin)
		{
			if (Canvas.hCan > 250)
			{
				wCell = 24 * AvMain.hd;
				wImg = (sbyte)(24 * AvMain.hd);
			}
			else
			{
				wCell = 16;
				wImg = 16;
			}
			if (AvMain.hd == 2 && Screen.height > 480)
			{
				wCell = (wImg = 72);
			}
		}
		hhFill = 40 * AvMain.hd;
		y = (Canvas.hCan - PaintPopup.hButtonSmall - wCell * 8) / 2;
		if (countHit == -1 || !BoardScr.isStartGame)
		{
			if (y < 0)
			{
				x = Canvas.w - wCell * 8 - wCell / 2;
			}
			else
			{
				x = Canvas.w - wCell * 8 - y;
			}
		}
	}

	public void start(int whoMoveFirst, int interval2, sbyte[][] arr)
	{
		repaint();
		base.start(whoMoveFirst, interval2);
		isEnd = false;
		this.arr = arr;
		turn = whoMoveFirst;
		BoardScr.interval = interval2;
		cmdCenter = (center = null);
		right = null;
		idWin = -1;
		BoardScr.dieTime = Canvas.getTick() + BoardScr.interval * 1000;
		if (GameMidlet.avatar.IDDB == turn)
		{
			isInit = true;
		}
		init();
		BoardScr.isStartGame = true;
		setPosPlaying();
		iSelected = -1;
		setArray(arr);
		Canvas.endDlg();
	}

	public override void setPosPlaying()
	{
		AvCamera.gI().setPos(0, 0);
		if (BoardScr.isStartGame)
		{
			int num = wCell / 2;
			int num2 = 0;
			if (AvMain.hd == 1)
			{
				num2 = 30;
			}
			int num3 = Canvas.w - x - wCell - wCell;
			for (int i = 0; i < BoardScr.numPlayer; i++)
			{
				Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
				if (avatar.IDDB != -1)
				{
					if (avatar.IDDB != GameMidlet.avatar.IDDB)
					{
						LoadMap.addPlayer(avatar);
					}
					avatar.yCur = (avatar.y = Canvas.hCan - Canvas.hTab - AvMain.hFillTab - AvMain.hSmall / 2 - 50);
					if (avatar.IDDB == GameMidlet.avatar.IDDB)
					{
						xPlayer1 = num + 16 * AvMain.hd + hhFill + num2;
						avatar.xCur = (avatar.x = xPlayer1 - hhFill / 2);
						avatar.direct = (avatar.dirFirst = Base.RIGHT);
					}
					else
					{
						xPlayer2 = x - wCell - 16 * AvMain.hd - hhFill - num2;
						avatar.xCur = (avatar.x = xPlayer2 + hhFill / 2);
						avatar.direct = (avatar.dirFirst = Base.LEFT);
					}
					avatar.ySat = 0;
					avatar.setAction(0);
					avatar.setFrame(avatar.action);
				}
			}
			return;
		}
		for (int j = 0; j < BoardScr.avatarInfos.size(); j++)
		{
			Avatar avatar2 = (Avatar)BoardScr.avatarInfos.elementAt(j);
			avatar2.x = (avatar2.xCur = Canvas.hw);
			if (avatar2.IDDB == GameMidlet.avatar.IDDB)
			{
				avatar2.y = (avatar2.yCur = Canvas.h - 5 * AvMain.hd);
			}
			else
			{
				avatar2.y = (avatar2.yCur = 55 * AvMain.hd);
			}
		}
	}

	private void setArray(sbyte[][] arr)
	{
		int num = 4;
		isTrans = true;
		for (int num2 = 7; num2 >= 0; num2--)
		{
			num = 20;
			for (int num3 = 7; num3 >= 0; num3--)
			{
				array[num2][num3] = new Point(num3 * wCell, num2 * wCell, arr[num2][num3]);
				array[num2][num3].color = array[num2][num3].y;
				array[num2][num3].h = -num;
				num--;
				array[num2][num3].isFire = true;
				array[num2][num3].y = -(num3 * wCell + 24);
			}
		}
	}

	private void clear()
	{
		for (int i = 4 - clearCount / 10; i < 4 + clearCount / 10; i++)
		{
			for (int j = 4 - clearCount / 10; j < 4 + clearCount / 10; j++)
			{
				addFire(array[i][j].x + 12, array[i][j].y + 12, array[i][j].itemID);
				array[i][j].itemID = -1;
			}
		}
	}

	public override void update()
	{
		base.update();
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			if (AvMain.hd == 2 && Screen.height > 480)
			{
				wCell = (sbyte)((ScaleGUI.HEIGHT - (float)PaintPopup.hButtonSmall - 60f) / 8f);
				ImageIcon imgIcon = AvatarData.getImgIcon(1028);
				ImageIcon imgIcon2 = AvatarData.getImgIcon(1027);
				if (imgIcon.count != -1 && imgIcon2.count != -1 && imgIcon.img != null && imgIcon2.img != null && !isJoin)
				{
					isJoin = true;
					int num = imgIcon.h / imgIcon.w;
					int num2 = imgIcon2.h / imgIcon2.w;
					wImg = (sbyte)wCell;
					hhFill = 40 * AvMain.hd;
					imgIcon.img.texture = CRes.ScaleTexture(imgIcon.img.texture, wCell, num * wCell);
					imgIcon2.img.texture = CRes.ScaleTexture(imgIcon2.img.texture, wCell, num2 * wCell);
					imgIcon.img.w = wCell;
					imgIcon.img.h = wCell;
					imgIcon.w = (imgIcon.h = (short)wCell);
					imgIcon2.w = (imgIcon2.h = (short)wCell);
					y = ((int)ScaleGUI.HEIGHT - PaintPopup.hButtonSmall - wCell * 8) / 2;
					if (countHit == -1 || !BoardScr.isStartGame)
					{
						if (y < 0)
						{
							x = Canvas.w - wCell * 8;
						}
						else
						{
							x = Canvas.w - wCell * 8 - y;
						}
					}
					imgSeleced = imgIcon;
					imgDiamond = imgIcon2;
					setArray(arr);
					setPosPlaying();
				}
			}
			if (BoardScr.dieTime != 0)
			{
				BoardScr.currentTime = Canvas.getTick();
				if (BoardScr.currentTime > BoardScr.dieTime)
				{
					BoardScr.dieTime = 0L;
					if (turn == GameMidlet.avatar.IDDB && cmdCenter == cmdSelected)
					{
						doSkip();
					}
				}
			}
			if (turn == GameMidlet.avatar.IDDB)
			{
				countSelected++;
				if (countSelected >= 20)
				{
					countSelected = 0;
				}
			}
			else
			{
				countSelected = 0;
			}
			int num3 = 0;
			int num4 = 0;
			for (int num5 = 63; num5 >= 0; num5--)
			{
				if (array[num5 / 8][num5 % 8] != null && array[num5 / 8][num5 % 8].catagory == 1)
				{
					if (array[num5 / 8][num5 % 8].translate() == -1)
					{
						array[num5 / 8][num5 % 8].catagory = 0;
						num4 = 1;
					}
					else
					{
						num3 = 1;
					}
				}
			}
			if (num4 == 1 && isPath)
			{
				Out.println("selected: " + selected + "     " + iSelected);
				if (!setSelected(selected) && !setSelected(iSelected))
				{
					int num6 = selected;
					selected = iSelected;
					iSelected = num6;
					change();
					cmdCenter = cmdSelected;
					right = cmdSkip;
				}
				else if (turn == GameMidlet.avatar.IDDB)
				{
					CasinoService.gI().doMoveDiamond(iSelected, selected);
				}
				isPath = false;
				iSelected = -1;
			}
			if (num3 == 0)
			{
				int num7 = 0;
				for (int num8 = 63; num8 >= 0; num8--)
				{
					if (array[num8 / 8][num8 % 8] != null && array[num8 / 8][num8 % 8].isFire)
					{
						array[num8 / 8][num8 % 8].x += array[num8 / 8][num8 % 8].g;
						if (array[num8 / 8][num8 % 8].g > 1 || array[num8 / 8][num8 % 8].g < -1)
						{
							array[num8 / 8][num8 % 8].g -= array[num8 / 8][num8 % 8].g / CRes.abs(array[num8 / 8][num8 % 8].g);
						}
						array[num8 / 8][num8 % 8].y += array[num8 / 8][num8 % 8].h;
						array[num8 / 8][num8 % 8].h += 2;
						if (array[num8 / 8][num8 % 8].y >= array[num8 / 8][num8 % 8].color)
						{
							array[num8 / 8][num8 % 8].y = array[num8 / 8][num8 % 8].color;
							array[num8 / 8][num8 % 8].isFire = false;
						}
						else
						{
							num7 = 1;
						}
					}
				}
				if (num7 == 0 && isTrans)
				{
					if (turn == GameMidlet.avatar.IDDB)
					{
						if (!isInit)
						{
							if (ableMove)
							{
								setPath();
							}
						}
						else if (setOutPath())
						{
							cmdCenter = cmdSelected;
							right = cmdSkip;
						}
						else
						{
							CasinoService.gI().doOutPath();
						}
						isInit = false;
					}
					isTrans = false;
				}
			}
			if (clearCount != -1)
			{
				if (clearCount % 10 == 0)
				{
					clear();
				}
				clearCount += 2;
				if (clearCount >= 50)
				{
					createPoint();
					clearCount = -1;
				}
			}
			for (int i = 0; i < listFireWork.size(); i++)
			{
				Point point = (Point)listFireWork.elementAt(i);
				if (point.limitY >= 1)
				{
					point.limitY++;
					if (point.limitY == 3)
					{
						listFireWork.removeElement(point);
						continue;
					}
				}
				if (point.isFire)
				{
					point.x += point.g;
					if (point.g > 1 || point.g < -1)
					{
						point.g -= point.g / CRes.abs(point.g);
					}
					point.y += point.h;
					point.h++;
					if (point.catagory == 1 && point.color < 19)
					{
						point.color++;
					}
					if (point.y + y > Canvas.h)
					{
						listFireWork.removeElement(point);
					}
					continue;
				}
				int num9 = CRes.angle(point.xTo - point.x, -(point.yTo - point.y));
				if (CRes.abs(num9 - point.h) > 10)
				{
					point.h -= point.height * point.catagory;
					point.h = CRes.fixangle(point.h);
				}
				else
				{
					point.h = num9;
					point.dis += 2;
				}
				if (point.color >= 4)
				{
					point.color = 0;
				}
				point.color++;
				int num10 = point.dis * CRes.cos(point.h) >> 10;
				int num11 = -(point.dis * CRes.sin(point.h)) >> 10;
				if (CRes.distance(point.x, point.y, point.xTo, point.yTo) >= point.dis)
				{
					point.x += num10;
					point.y += num11;
				}
				else
				{
					listFireWork.removeElement(point);
				}
			}
			for (int j = 0; j < 2; j++)
			{
				Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(j);
				if (avatar.task == -1 && CRes.abs(avatar.xCur - avatar.x) < 10)
				{
					if (countHit == -2)
					{
						countHit = -1;
						avatar.task = 0;
						if (avatar.IDDB == idWin)
						{
							avatar.doAction(10);
							avatar.setFeel(10);
						}
						else
						{
							avatar.action = 0;
							if (idWin != -1)
							{
								avatar.setFeel(9);
							}
						}
						isNo = false;
						if (avatar.IDDB == GameMidlet.avatar.IDDB)
						{
							avatar.direct = Base.RIGHT;
						}
					}
					else if (avatar.task == -1)
					{
						if (avatar.isNo && Canvas.gameTick % 6 == 3)
						{
							addFireNo(avatar.x, avatar.y - avatar.height, 0);
						}
						if (countHit == -1)
						{
							for (int k = 0; k < 2; k++)
							{
								Avatar avatar2 = (Avatar)BoardScr.avatarInfos.elementAt(k);
								if (avatar2.IDDB != avatar.IDDB)
								{
									avatar2.setFeel(20);
									avatar2.action = 4;
									avatar2.ableShow = true;
									avatar.ableShow = true;
								}
							}
							countHit = 20;
							if (isNo)
							{
								countHit = 30;
							}
						}
						else if (countHit >= 0)
						{
							countHit--;
							if (countHit == -1)
							{
								countHit = -2;
								if (avatar.IDDB == GameMidlet.avatar.IDDB)
								{
									avatar.xCur = xPlayer1 - hhFill / 2;
								}
								else
								{
									avatar.xCur = xPlayer2 + hhFill / 2;
								}
							}
						}
					}
				}
				if (avatar.plusHP > 0)
				{
					int num12 = avatar.maxHP / 100 + 1;
					if (avatar.plusHP - num12 < 0)
					{
						num12 = avatar.plusHP;
					}
					avatar.plusHP = (short)(avatar.plusHP - num12);
					avatar.hp = (short)(avatar.hp + num12);
				}
				else if (avatar.plusHP < 0)
				{
					int num13 = avatar.maxHP / 100 + 1;
					if (avatar.plusHP + num13 > 0)
					{
						num13 = -avatar.plusHP;
					}
					avatar.hp = (short)(avatar.hp - num13);
					avatar.plusHP = (short)(avatar.plusHP + num13);
				}
				if (avatar.plusMP > 0)
				{
					int num14 = avatar.maxHP / 100 + 1;
					if (avatar.plusMP - num14 < 0)
					{
						num14 = avatar.plusMP;
					}
					avatar.plusMP = (short)(avatar.plusMP - num14);
					avatar.mp = (short)(avatar.mp + num14);
				}
				else if (avatar.plusMP < 0)
				{
					int num15 = avatar.maxHP / 100 + 1;
					if (avatar.plusMP + num15 > 0)
					{
						num15 = -avatar.plusMP;
					}
					avatar.mp = (short)(avatar.mp - num15);
					avatar.plusMP = (short)(avatar.plusMP + num15);
				}
			}
			for (int l = 0; l < listSmall.size(); l++)
			{
				Point point2 = (Point)listSmall.elementAt(l);
				point2.limitY--;
				if (point2.limitY <= 0)
				{
					listSmall.removeElement(point2);
				}
			}
		}
		else
		{
			right = null;
			updateReady();
		}
	}

	private bool setOutPath()
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < xCheck.Length; k++)
				{
					if (i + yCheck[k][0] >= 0 && i + yCheck[k][0] < 8 && i + yCheck[k][1] >= 0 && i + yCheck[k][1] < 8 && j + xCheck[k][0] >= 0 && j + xCheck[k][0] < 8 && j + xCheck[k][1] >= 0 && j + xCheck[k][1] < 8 && array[i][j].itemID == array[i + yCheck[k][0]][j + xCheck[k][0]].itemID && array[i][j].itemID == array[i + yCheck[k][1]][j + xCheck[k][1]].itemID)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void addFire(int x, int y, int index)
	{
		if (index == -1)
		{
			return;
		}
		Avatar avatarByID = BoardScr.getAvatarByID(turn);
		if (avatarByID == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		switch (index)
		{
		case 0:
			addFireNo(x + this.x, y + this.y, 0);
			return;
		case 1:
			num = avatarByID.x;
			num2 = avatarByID.y - avatarByID.height / 2;
			if (avatarByID.defence > 0)
			{
				num = ((avatarByID.IDDB != GameMidlet.avatar.IDDB) ? (xPlayer2 + 7 + 20) : (xPlayer1 - 20 - 7));
				num2 = avatarByID.y - 22;
			}
			break;
		case 2:
			num = ((avatarByID.IDDB != GameMidlet.avatar.IDDB) ? (xPlayer2 + (hhFill - avatarByID.hp * hhFill / avatarByID.maxHP) + 20 - avatarByID.hp * hhFill / avatarByID.maxHP) : (xPlayer1 - 20 - hhFill + avatarByID.hp * hhFill / avatarByID.maxHP));
			num2 = avatarByID.y - 2 - 10 * AvMain.hd;
			break;
		case 3:
			num = ((avatarByID.IDDB != GameMidlet.avatar.IDDB) ? (xPlayer2 + (hhFill - avatarByID.mp * hhFill / avatarByID.maxMP) + 20 - avatarByID.hp * hhFill / avatarByID.maxHP) : (xPlayer1 - 20 - hhFill + avatarByID.mp * hhFill / avatarByID.maxMP));
			num2 = avatarByID.y - 5 * AvMain.hd;
			break;
		case 4:
			addFireNo(x + this.x, y + this.y, 4);
			return;
		case 5:
			return;
		}
		Point point = new Point(x + this.x, y + this.y);
		point.limitY = 1;
		listFireWork.addElement(point);
		for (int i = 0; i < ((index == 1) ? 1 : 3); i++)
		{
			Point point2 = new Point(x + this.x, y + this.y);
			point2.distant = (short)index;
			point2.color = CRes.rnd(3);
			int g = CRes.angle(num - x, -(num2 - y));
			point2.g = g;
			point2.catagory = (sbyte)CRes.rnd(-1, 1);
			point2.h = CRes.fixangle(point2.g + point2.catagory * 90);
			int num3 = 10 * CRes.cos(point2.h) >> 10;
			int num4 = -(10 * CRes.sin(point2.h)) >> 10;
			point2.xTo = (short)num;
			point2.yTo = (short)num2;
			point2.x += num3;
			point2.y += num4;
			point2.color = 0;
			point2.dis = (sbyte)(CRes.rnd(4) + 4);
			point2.height = (short)(10 + CRes.rnd(5));
			listFireWork.addElement(point2);
		}
	}

	private void addFireNo(int x, int y, int index)
	{
		if (index != -1)
		{
			Point point = new Point(x, y);
			point.limitY = 1;
			listFireWork.addElement(point);
			for (int i = 0; i < 3; i++)
			{
				int num = CRes.rnd(-1, 1);
				Point point2 = new Point(x, y);
				point2.isFire = true;
				point2.color = CRes.rnd(3);
				point2.g = num * (CRes.rnd(100) / 10);
				point2.h = -CRes.rnd(100) / 10;
				point2.dis = (sbyte)index;
				point2.catagory = 1;
				point2.limitY = 0;
				listFireWork.addElement(point2);
			}
		}
	}

	private bool setSelected(int index)
	{
		Out.println("setSelected: " + index + "    " + iSelected + "     " + isTrans);
		if (iSelected == -1 || isTrans)
		{
			return false;
		}
		for (int i = 0; i < xSetSelected.Length; i++)
		{
			if (index / 8 + ySetSelected[i][0] >= 0 && index / 8 + ySetSelected[i][0] < 8 && index / 8 + ySetSelected[i][1] >= 0 && index / 8 + ySetSelected[i][1] < 8 && index % 8 + xSetSelected[i][0] >= 0 && index % 8 + xSetSelected[i][0] < 8 && index % 8 + xSetSelected[i][1] >= 0 && index % 8 + xSetSelected[i][1] < 8)
			{
				Out.println("-----------------------------");
				Out.println("ppp: " + index / 8 + "    " + index % 8);
				Out.println("aaa: " + i + "    " + array[index / 8][index % 8].itemID);
				Out.println("bbb: " + (index / 8 + ySetSelected[i][0]) + "     " + (index % 8 + xSetSelected[i][0]));
				Out.println("ccc: " + (index / 8 + ySetSelected[i][1]) + "      " + (index % 8 + xSetSelected[i][1]));
				Out.println("ddd: " + array[index / 8 + ySetSelected[i][0]][index % 8 + xSetSelected[i][0]].itemID + "    " + array[index / 8 + ySetSelected[i][1]][index % 8 + xSetSelected[i][1]].itemID);
				if (array[index / 8][index % 8].itemID == array[index / 8 + ySetSelected[i][0]][index % 8 + xSetSelected[i][0]].itemID && array[index / 8][index % 8].itemID == array[index / 8 + ySetSelected[i][1]][index % 8 + xSetSelected[i][1]].itemID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void updateKey()
	{
		base.updateKey();
		if (Canvas.isPointerClick && Canvas.isPointer(x, y + Canvas.transTab, wCell * 8, wCell * 8) && iSelected == -1)
		{
			Canvas.isPointerClick = false;
			isTranCam = true;
			int num = (Canvas.px - x) / wCell;
			int num2 = (Canvas.py - y - Canvas.transTab) / wCell;
			selected = num2 * 8 + num;
		}
		if (Canvas.getTick() <= BoardScr.dieTime - (BoardScr.interval - 1) * 1000 || isPath || isTrans || cmdCenter != cmdSelected || cmdCenter == BoardScr.cmdWaiting || !isTranCam)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			int num3 = Canvas.dx();
			int num4 = Canvas.dy();
			if (num3 < -wCell / 2)
			{
				if (selected % 8 < 7)
				{
					iSelected = selected;
					selected++;
					isTranCam = false;
					change();
				}
			}
			else if (num3 > wCell / 2)
			{
				if (selected % 8 > 0)
				{
					iSelected = selected;
					selected--;
					isTranCam = false;
					change();
				}
			}
			else if (num4 < -wCell / 2)
			{
				if (selected / 8 < 7)
				{
					iSelected = selected;
					selected += 8;
					isTranCam = false;
					change();
				}
			}
			else if (num4 > wCell / 2 && selected >= 8)
			{
				iSelected = selected;
				selected -= 8;
				isTranCam = false;
				change();
			}
		}
		if (Canvas.isPointerRelease)
		{
			Canvas.isPointerRelease = false;
			isTranCam = false;
		}
	}

	private void change()
	{
		if (iSelected != -1 && !isTrans)
		{
			cmdCenter = BoardScr.cmdWaiting;
			right = null;
			isPath = true;
			isTranCam = false;
			Point point = array[selected / 8][selected % 8];
			Point point2 = array[iSelected / 8][iSelected % 8];
			int num = point.x;
			int num2 = point.y;
			short itemID = point.itemID;
			point.x = point2.x;
			point.y = point2.y;
			point.itemID = point2.itemID;
			point2.x = num;
			point2.y = num2;
			point2.itemID = itemID;
			point2.catagory = 1;
			point.catagory = 1;
		}
	}

	private void setPath()
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < 64; i++)
		{
			if (array[i / 8][i % 8].itemID == -2)
			{
				continue;
			}
			num = 0;
			for (int j = i + 1; j % 8 < 8 && j < 64 && j / 8 == i / 8 && array[i / 8][i % 8].itemID == array[j / 8][j % 8].itemID; j++)
			{
				num++;
			}
			if (num > 1)
			{
				for (int k = i; k < i + num + 1; k++)
				{
					array[k / 8][k % 8].isRemove = true;
					flag = true;
				}
			}
			num = 0;
			for (int l = i + 8; l < 64 && l % 8 == i % 8 && array[i / 8][i % 8].itemID == array[l / 8][l % 8].itemID; l += 8)
			{
				num++;
			}
			if (num > 1)
			{
				for (int m = i; m < i + (num + 1) * 8; m += 8)
				{
					array[m / 8][m % 8].isRemove = true;
					flag = true;
				}
			}
		}
		if (flag)
		{
			CasinoService.gI().createCell(array);
		}
		else if (isMove)
		{
			isMove = false;
			doSkip();
		}
	}

	private void createPoint()
	{
		int num = 4;
		for (int i = 0; i < 8; i++)
		{
			num = 4;
			for (int num2 = 7; num2 >= 0; num2--)
			{
				if (array[(num2 * 8 + i) / 8][(num2 * 8 + i) % 8].itemID == -1)
				{
					setChange(num2 * 8 + i, num, -2);
					num2++;
				}
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		base.paint(g);
	}

	public override void paintMain(MyGraphics g)
	{
		base.paintMain(g);
		if (BoardScr.isStartGame)
		{
			Canvas.resetTransNotZoom(g);
			if ((AvMain.hd == 2 && Screen.height > 480 && !isJoin) || isJoin || AvMain.hd == 1 || Screen.height < 480)
			{
				paintGame(g);
			}
			Canvas.resetTransNotZoom(g);
			paintNamePlayers(g);
			paintPlayer(g);
			Canvas.resetTransNotZoom(g);
			string text = string.Empty;
			if (BoardScr.dieTime != 0)
			{
				long num = (BoardScr.currentTime - BoardScr.dieTime) / 1000;
				text += -num;
			}
			Canvas.numberFont.drawString(g, text + string.Empty, x - wCell / 2 - 5, y + wCell * 8 / 2 - Canvas.numberFont.getHeight() / 2, 1);
			paintFireWork(g);
		}
		else
		{
			paintPlayerCo(g);
		}
	}

	public void paintCaro(MyGraphics g)
	{
		if ((AvMain.hd != 2 || Screen.height <= 480 || isJoin) && !isJoin && AvMain.hd != 1 && Screen.height >= 480)
		{
			return;
		}
		Canvas.resetTransNotZoom(g);
		g.setClip(x - wCell / 2, y - wCell / 2, wCell * 8 + wCell + 1, wCell * 8 + wCell + 1);
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				if (j % 2 == i % 2)
				{
					g.setColor(5197647);
				}
				else
				{
					g.setColor(2697513);
				}
				g.fillRect(x - wCell + i * wCell, y + j * wCell - wCell, wCell, wCell);
			}
		}
		g.setColor(0);
		g.drawRect(x - wCell / 2, y - wCell / 2, wCell * 8 + wCell, wCell * 8 + wCell);
		g.drawRect(x - wCell / 2 + 1, y - wCell / 2 + 1, wCell * 8 + wCell - 2, wCell * 8 + wCell - 2);
	}

	private void paintPlayer(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		for (int i = 0; i < 2; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			int num10 = avatar.y + 50;
			int num11 = avatar.x;
			if (avatar.IDDB == GameMidlet.avatar.IDDB)
			{
				num11 += hhFill / 2;
			}
			else
			{
				num11 -= hhFill / 2;
			}
			if (countHit != -1 && avatar.task == -1 && avatar.action == 0)
			{
				ImageIcon imgIcon = AvatarData.getImgIcon((short)((!isNo) ? 881 : 882));
				if (imgIcon.count != -1)
				{
					g.drawRegion(imgIcon.img, 0f, 48 * AvMain.hd * ((Canvas.gameTick % 6 >= 3) ? 1 : 0), 48 * AvMain.hd, 48 * AvMain.hd, 0, avatar.x, avatar.y - avatar.height / 2, 3);
				}
			}
			if (avatar.IDDB == GameMidlet.avatar.IDDB)
			{
				num = xPlayer1 + hhFill / 2;
				num -= 10 + 10 * AvMain.hd + hhFill;
				num2 = 0;
				num3 = (num9 = (num8 = 0));
				num5 = -2;
				num4 = 1;
				num6 = hhFill - 7;
				num7 = hhFill - 16 * AvMain.hd;
				Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), num + hhFill, num10, 1);
			}
			else
			{
				num = xPlayer2 - hhFill / 2;
				num += 10 + 10 * AvMain.hd;
				num2 += hhFill - avatar.hp * hhFill / avatar.maxHP;
				num3 += hhFill - avatar.mp * hhFill / avatar.maxMP;
				num9 = hhFill - (avatar.hp + avatar.plusHP) * hhFill / avatar.maxHP;
				num8 = hhFill - (avatar.mp + avatar.plusMP) * hhFill / avatar.maxMP;
				num5 = hhFill + 2;
				num6 = 8;
				num4 = 0;
				num7 = 16 * AvMain.hd;
				Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), num, num10, 0);
			}
			Canvas.smallFontYellow.drawString(g, avatar.hp + string.Empty, num + num5, num10 - AvMain.hSmall * 2 + 3 * AvMain.hd - AvMain.hSmall / 2, num4);
			Canvas.smallFontYellow.drawString(g, avatar.mp + string.Empty, num + num5, num10 - AvMain.hSmall + 3 * AvMain.hd - AvMain.hSmall / 2, num4);
			if ((avatar.defence > 0 && avatar.countDefent <= 0) || (avatar.countDefent > 0 && Canvas.gameTick % 6 < 3))
			{
				AvatarData.paintImg(g, 880, num + num6, num10 - AvMain.hSmall * 3, 3);
				Canvas.smallFontYellow.drawString(g, avatar.defence + string.Empty, num + num7, num10 - AvMain.hSmall * 3 - AvMain.hSmall / 2, num4);
				if (avatar.countDefent > 0)
				{
					avatar.countDefent--;
				}
			}
			if (avatar.plusHP != 0 && Canvas.gameTick % 6 >= 3)
			{
				g.setColor(1908254);
			}
			else
			{
				g.setColor(0);
			}
			g.fillRect(num, num10 - AvMain.hSmall * 2, hhFill, 6 * AvMain.hd);
			g.fillRect(num, num10 - AvMain.hSmall, hhFill, 6 * AvMain.hd);
			if (avatar.plusHP > 0)
			{
				g.setColor(16583178);
				g.fillRect(num + num9, num10 - 4 - 10 * AvMain.hd, (avatar.hp + avatar.plusHP) * hhFill / avatar.maxHP, 6 * AvMain.hd);
			}
			if (avatar.plusHP != 0 && Canvas.gameTick % 6 >= 3)
			{
				g.setColor(16734553);
			}
			else
			{
				g.setColor(16711680);
			}
			g.fillRect(num + num2, num10 - AvMain.hSmall * 2, avatar.hp * hhFill / avatar.maxHP, 6 * AvMain.hd);
			g.setColor(14137273);
			g.drawRect(num, num10 - AvMain.hSmall * 2, hhFill, 6 * AvMain.hd);
			g.drawRect(num, num10 - AvMain.hSmall, hhFill, 6 * AvMain.hd);
			if (avatar.plusMP > 0)
			{
				g.setColor(3771903);
				g.fillRect(num + num8, num10 - AvMain.hSmall + 1, (avatar.mp + avatar.plusMP) * hhFill / avatar.maxMP, 6 * AvMain.hd - 1);
			}
			if ((avatar.plusMP != 0 || avatar.isNo) && Canvas.gameTick % 6 >= 3)
			{
				g.setColor(6799871);
			}
			else
			{
				g.setColor(299247);
			}
			g.fillRect(num + num3, num10 - AvMain.hSmall + 1, avatar.mp * hhFill / avatar.maxMP, 6 * AvMain.hd - 1);
		}
	}

	private void paintGame(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		g.translate(x, y);
		if (AvatarData.getImgIcon(876).count != -1)
		{
			int num = 0;
			for (int i = 0; i < listSmall.size(); i++)
			{
				Point point = (Point)listSmall.elementAt(i);
				num = i * 17 - wCell / 2 + 8;
				if (point.color != GameMidlet.avatar.IDDB)
				{
					num = wCell * 8 - i * 17 + wCell / 2 - 8;
				}
				g.drawRegion(AvatarData.getImgIcon(876).img, 0f, point.itemID * 16, 16, 16, 0, num, wCell * 8 + wCell, 3);
				Canvas.smallFontYellow.drawString(g, point.dis + string.Empty, num, wCell * 8 + wCell - AvMain.hSmall / 2, 2);
			}
		}
		g.setClip(-wCell / 2, -wCell / 2, wCell * 8 + wCell, wCell * 8 + wCell);
		if (selected >= 0 && array[selected / 8][selected % 8] != null)
		{
			ImageIcon imageIcon = null;
			imageIcon = ((AvMain.hd != 2 || Screen.height <= 480) ? AvatarData.getImgIcon((short)((Canvas.hCan <= 250) ? 879 : 878)) : imgSeleced);
			if (imageIcon != null && imageIcon.count != -1)
			{
				g.drawRegion(imageIcon.img, 0f, countSelected / 10 * wCell, wCell, wCell, 0, array[selected / 8][selected % 8].x, array[selected / 8][selected % 8].y, 0);
			}
		}
		ImageIcon imageIcon2 = null;
		imageIcon2 = ((AvMain.hd != 2 || Screen.height <= 480) ? AvatarData.getImgIcon((short)((Canvas.hCan <= 250) ? 876 : 875)) : imgDiamond);
		if (imageIcon2 == null || imageIcon2.count == -1)
		{
			return;
		}
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				if (array[j][k] != null && array[j][k].itemID >= 0)
				{
					g.drawRegion(imageIcon2.img, 0f, array[j][k].itemID * wImg, wImg, wImg, 0, array[j][k].x, array[j][k].y, 0);
				}
			}
		}
	}

	private void paintFireWork(MyGraphics g)
	{
		for (int i = 0; i < listFireWork.size(); i++)
		{
			Point point = (Point)listFireWork.elementAt(i);
			if (point.limitY > 0)
			{
				AvatarData.paintImg(g, 877, point.x, point.y, 3);
			}
			else if (point.isFire)
			{
				imgFireWork.drawFrame(point.color / 5, point.x, point.y, 0, 3, g);
			}
			else if (point.dis >= 0)
			{
				imgFireWork.drawFrame(point.color / 2 + 1, point.x, point.y, 0, 3, g);
			}
		}
	}

	public void onCreateCell(sbyte[] arrClear, AvPosition[] listCreate, sbyte countCombo, MyVector listStr)
	{
		int num = 0;
		for (int i = 0; i < arrClear.Length; i++)
		{
			array[arrClear[i] / 8][arrClear[i] % 8].isRemove = true;
			if (Canvas.h <= 300)
			{
				continue;
			}
			num = 0;
			for (int j = 0; j < listSmall.size(); j++)
			{
				Point point = (Point)listSmall.elementAt(j);
				if (point.itemID == array[arrClear[i] / 8][arrClear[i] % 8].itemID)
				{
					point.limitY += 20;
					num = 1;
					point.dis++;
					break;
				}
			}
			if (num == 0)
			{
				Point point2 = new Point();
				point2.itemID = array[arrClear[i] / 8][arrClear[i] % 8].itemID;
				point2.limitY = 40;
				point2.dis = 1;
				point2.color = turn;
				listSmall.addElement(point2);
			}
		}
		removePoint();
		for (int k = 0; k < listCreate.Length; k++)
		{
			int anchor = listCreate[k].anchor;
			array[anchor / 8][anchor % 8].itemID = listCreate[k].depth;
		}
		if (countCombo > 1)
		{
			Canvas.addFlyTextSmall("Combo x" + countCombo, Canvas.hw, Canvas.hh, -1, 1, 20);
		}
		if (listStr.size() > 0)
		{
			for (int l = 0; l < listStr.size(); l++)
			{
				Canvas.addFlyTextSmall((string)listStr.elementAt(l), Canvas.hw, Canvas.hh + 40, -1, 1, l * 30);
			}
		}
		for (int m = 0; m < 2; m++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(m);
			avatar.setFeel(4);
			if (avatar.IDDB != turn && avatar.fight > 0)
			{
				Avatar avatarByID = BoardScr.getAvatarByID(turn);
				if (avatarByID.task != -1)
				{
					avatarByID.setPosTo(avatar.x, avatar.y);
				}
				avatarByID.task = -1;
				if (avatar.defence > 0)
				{
					avatar.countDefent = 20;
				}
			}
		}
		Canvas.endDlg();
	}

	private void removePoint()
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (array[i][j].isRemove)
				{
					array[i][j].isRemove = false;
					addFire(array[i][j].x + 12, array[i][j].y + 12, array[i][j].itemID);
					array[i][j].itemID = -1;
				}
			}
		}
		createPoint();
	}

	private int setChange(int a, int du, short ItemID)
	{
		isTrans = true;
		int num = a;
		while (num / 8 > 0)
		{
			array[num / 8][num % 8].itemID = array[(num - 8) / 8][(num - 8) % 8].itemID;
			array[num / 8][num % 8].color = num / 8 * wCell;
			if (!array[num / 8][num % 8].isFire)
			{
				array[num / 8][num % 8].h = -du;
				du++;
				array[num / 8][num % 8].isFire = true;
			}
			array[num / 8][num % 8].y = array[(num - 8) / 8][(num - 8) % 8].y;
			num -= 8;
		}
		array[0][a % 8].itemID = ItemID;
		array[0][a % 8].color = 0;
		if (!array[0][a % 8].isFire)
		{
			array[0][a % 8].h = -du;
			du++;
			array[0][a % 8].isFire = true;
			array[0][a % 8].y = 0;
		}
		array[0][a % 8].y -= 24;
		return du;
	}

	public void move(int whoMove, int iSelected, int selected)
	{
		if (isEnd)
		{
			return;
		}
		Avatar avatarByID = BoardScr.getAvatarByID(whoMove);
		if (avatarByID != null && avatarByID.action == 4)
		{
			avatarByID.action = 0;
		}
		if (whoMove == GameMidlet.avatar.IDDB)
		{
			isMove = true;
			setPath();
			ableMove = true;
			return;
		}
		cmdCenter = BoardScr.cmdWaiting;
		right = null;
		this.iSelected = iSelected;
		this.selected = selected;
		change();
		if (whoMove == -1)
		{
			isPath = false;
			this.iSelected = -1;
		}
	}

	public void onSkip(int whoMove1)
	{
		if (isEnd)
		{
			return;
		}
		iSelected = -1;
		BoardScr.dieTime = Canvas.getTick() + BoardScr.interval * 1000;
		turn = whoMove1;
		ableMove = false;
		if (whoMove1 == GameMidlet.avatar.IDDB)
		{
			if (setOutPath())
			{
				cmdCenter = cmdSelected;
				right = cmdSkip;
			}
			else
			{
				CasinoService.gI().doOutPath();
			}
		}
		else
		{
			isMove = false;
			cmdCenter = null;
			right = null;
		}
	}

	public void onOutPath(int whoMove, sbyte[][] arr)
	{
		turn = whoMove;
		if (whoMove == GameMidlet.avatar.IDDB)
		{
			isInit = true;
		}
		setArray(arr);
	}

	public void setContinue()
	{
		right = BoardScr.cmdBack;
		turn = -1;
		resetReady();
	}

	public override void doContinue()
	{
		base.doContinue();
		BoardScr.isStartGame = false;
		setPosPlaying();
		isEnd = false;
		idWin = -1;
		for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			avatar.resetAction();
			avatar.setFeel(4);
		}
	}

	public void onFinish(MyVector list)
	{
		setContinue();
		left = null;
		isEnd = true;
	}

	public void onData(sbyte[][] arr)
	{
		Out.println("on Data");
		for (int num = 7; num >= 0; num--)
		{
			for (int num2 = 7; num2 >= 0; num2--)
			{
				array[num][num2].itemID = arr[num][num2];
			}
		}
	}
}
