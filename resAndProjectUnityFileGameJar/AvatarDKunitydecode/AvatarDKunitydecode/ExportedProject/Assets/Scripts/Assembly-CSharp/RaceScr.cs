using System;

public class RaceScr : MyScreen, IChatable
{
	private class IActionOkChat : IAction
	{
		public void perform()
		{
		}
	}

	private class IActionOkListTF : IAction
	{
		public void perform()
		{
			gI().commandActionPointer(1, -1);
		}
	}

	private class IACctionOut : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			GlobalService.gI().getHandler(9);
		}
	}

	public static RaceScr me;

	public Command cmdChat;

	public Command cmdChangeFocus;

	public Command cmdExit;

	private int focus;

	private MyVector listChat = new MyVector();

	public RaceMsgHandler.PetRace[] listPet;

	private short timeRemain;

	private short wPetInfo;

	private short hPetInfo;

	public bool isRace;

	public bool isStart;

	public bool isEnd;

	private long curTime;

	public sbyte countStart;

	public sbyte nWin = 1;

	public sbyte indexFocus;

	public static Image imgWater;

	public static Image imgFire;

	public static Image[] imgBui;

	public static Image[] imgTe;

	private ChatPopup myChat;

	public RaceMsgHandler.dialogWin diaWin;

	public static sbyte[][] FRAME;

	public static string test = string.Empty;

	public int wPopup;

	public int hPopup;

	public int xPopup;

	public int yPopup;

	public int xInfo;

	public int yInfo;

	public int wInfo;

	public int Hinfo;

	public int xDC;

	public int yDC;

	public int wDC;

	public int hDC;

	public int xSelectDC;

	public int ySelectDC;

	public int wSelectDC;

	public int hSelectDC;

	public FrameImage imgInfo;

	public FrameImage imgBackpet;

	public FrameImage imgBackMoney;

	public short timeStart;

	public long curTimeStart;

	private FrameImage imgTime;

	private bool isDC;

	public sbyte countCloseDC;

	public MyVector listPlayer = new MyVector();

	private int idPet;

	private int countChangePetInfo;

	private int indexPet = -1;

	private int indexMoney = -1;

	private int indexDC = -1;

	private sbyte timeOpen;

	private new bool isTran;

	private long count;

	private long timeDelay;

	private int[] iMoney = new int[9] { 100, 500, 1000, 2000, 5000, 10000, 20000, 30000, 50000 };

	private string[] phongDo = new string[3] { "Thấp", "Thường", "Cao" };

	private string[] sucKhoe = new string[3] { "Thấp", "Thường", "Cao" };

	private bool isPetInfo;

	private short idImgPeInfo;

	private short numWin;

	private string namePetInfo;

	private sbyte ratePetInfo;

	private sbyte phongDoPetInfo;

	private sbyte sucKhoePetInfo;

	public RaceScr()
	{
		FRAME = new sbyte[3][];
		FRAME[0] = new sbyte[12]
		{
			0, 0, 0, 1, 1, 1, 0, 0, 0, 1,
			1, 1
		};
		FRAME[1] = new sbyte[12]
		{
			2, 2, 2, 3, 3, 3, 2, 2, 2, 3,
			3, 3
		};
		FRAME[2] = new sbyte[12]
		{
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4
		};
		cmdChangeFocus = new Command(T.next, 3, this);
		cmdExit = new Command(T.exit, 2, this);
		wPopup = 280 * AvMain.hd;
		hPopup = 240 * AvMain.hd;
		xInfo = 9 * AvMain.hd;
		yInfo = (yDC = 23 * AvMain.hd);
		wInfo = 138 * AvMain.hd;
		Hinfo = (hDC = 211 * AvMain.hd);
		wDC = 120 * AvMain.hd;
		xDC = 150 * AvMain.hd;
		wSelectDC = 210 * AvMain.hd + 10 * AvMain.hd + 14 * AvMain.hd;
		hSelectDC = 130 * AvMain.hd;
		xSelectDC = (Canvas.w - wSelectDC) / 2;
		ySelectDC = (Canvas.h - hSelectDC) / 2;
		initPos();
	}

	public static RaceScr gI()
	{
		return (me != null) ? me : (me = new RaceScr());
	}

	public void initPos()
	{
		xPopup = (Canvas.w - wPopup) / 2;
		yPopup = (Canvas.hCan - hPopup) / 2;
	}

	public override void switchToMe()
	{
		base.switchToMe();
	}

	public void doOpenRace(RaceMsgHandler.PetRace[] pet, short timeRemain, bool isSta, bool isRace)
	{
		isEnd = false;
		nWin = 1;
		idPet = -1;
		Canvas.currentDialog = null;
		Canvas.currentFace = null;
		if (imgWater == null)
		{
			try
			{
				imgInfo = new FrameImage(Image.createImagePNG(T.getPath() + "/race/popup/tile1"), 20 * AvMain.hd, 20 * AvMain.hd);
				imgBackpet = new FrameImage(Image.createImagePNG(T.getPath() + "/race/popup/bt1"), 31 * AvMain.hd, 31 * AvMain.hd);
				imgBackMoney = new FrameImage(Image.createImagePNG(T.getPath() + "/race/popup/bt0"), 70 * AvMain.hd, 25 * AvMain.hd);
				imgTime = new FrameImage(Image.createImagePNG(T.getPath() + "/race/popup/time"), 14 * AvMain.hd, 14 * AvMain.hd);
				imgWater = Image.createImage(T.getPath() + "/race/28");
				imgFire = Image.createImage(T.getPath() + "/race/29");
				imgBui = new Image[5];
				for (int i = 0; i < 5; i++)
				{
					imgBui[i] = Image.createImage(T.getPath() + "/race/bui/d0" + i + string.Empty);
				}
				imgTe = new Image[3];
				for (int j = 0; j < 3; j++)
				{
					imgTe[j] = Image.createImage(T.getPath() + "/race/bui/w" + j + string.Empty);
				}
			}
			catch (Exception e)
			{
				Out.logError(e);
			}
		}
		if (!isSta)
		{
			if (isRace)
			{
				for (int k = 0; k < LoadMap.playerLists.size(); k++)
				{
					MyObject myObject = (MyObject)LoadMap.playerLists.elementAt(k);
					if (myObject.catagory == 10)
					{
						LoadMap.removePlayer(myObject);
					}
				}
			}
			if (me != Canvas.currentMyScreen)
			{
				LoadMap.orderVector(LoadMap.playerLists);
				gI().switchToMe();
				LoadMap.rememMap = -1;
				randomPlayer(1);
				randomPlayer(2);
				Canvas.loadMap.load(108, true);
				LoadMap.removePlayer(GameMidlet.avatar);
				gI().init();
				AvCamera.isFollow = false;
			}
			listPet = null;
			listPet = pet;
			if (pet != null)
			{
				for (int l = 0; l < 6; l++)
				{
					listPet[l].x = 20;
					listPet[l].y = 80 + l * 12;
					LoadMap.playerLists.addElement(listPet[l]);
				}
				AvCamera.gI().followPlayer = listPet[2];
				indexFocus = 3;
			}
			GameMidlet.avatar.x = (GameMidlet.avatar.xCur = 0);
		}
		GameMidlet.avatar.y = (GameMidlet.avatar.yCur = 96 * AvMain.hd);
		isStart = isSta;
		this.isRace = isRace;
		this.timeRemain = timeRemain;
		if (pet == null)
		{
			if (!isStart)
			{
			}
			center = null;
		}
		else if (isSta || !isRace)
		{
			center = null;
		}
		curTime = Environment.TickCount;
		if (isSta)
		{
			countStart = 48;
			center = null;
			right = cmdChangeFocus;
		}
		else
		{
			right = null;
			if (!isRace)
			{
				right = cmdChangeFocus;
				for (int m = 0; m < 6; m++)
				{
					int num = 0;
					for (int n = 0; n < listPet[m].numTick.Length; n++)
					{
						num += listPet[m].numTick[n];
						listPet[m].x += listPet[m].vTick[n] * listPet[m].numTick[n];
						RaceMsgHandler.PetRace obj = listPet[m];
						obj.count++;
						if (num >= (timeRemain - 4) * 20)
						{
							break;
						}
					}
				}
			}
			else
			{
				GlobalService.gI().doPetInfo(listPet[0].IDDB);
			}
		}
		wPetInfo = (short)(200 * AvMain.hd);
		hPetInfo = (short)(AvMain.hNormal * 7 + AvMain.hBorder * 3 + 20);
		myChat = new ChatPopup();
	}

	private void randomPlayer(int gender)
	{
		MyVector myVector = new MyVector();
		MyVector myVector2 = new MyVector();
		MyVector myVector3 = new MyVector();
		MyVector myVector4 = new MyVector();
		MyVector myVector5 = new MyVector();
		for (int i = 0; i < AvatarData.listPart.Length; i++)
		{
			Part part = AvatarData.listPart[i];
			if (part.follow != -1 || part.IDPart >= 2000 || part.sell <= 0)
			{
				continue;
			}
			APartInfo aPartInfo = (APartInfo)part;
			if (aPartInfo.gender == gender || aPartInfo.gender == 0)
			{
				if (aPartInfo.zOrder == 10)
				{
					myVector.addElement(aPartInfo);
				}
				else if (part.zOrder == 20)
				{
					myVector2.addElement(aPartInfo);
				}
				else if (part.zOrder == 30)
				{
					myVector3.addElement(aPartInfo);
				}
				else if (part.zOrder == 40)
				{
					myVector4.addElement(aPartInfo);
				}
				else if (part.zOrder == 50)
				{
					myVector5.addElement(aPartInfo);
				}
			}
		}
		for (int j = 0; j < 10; j++)
		{
			Avatar avatar = new Avatar();
			avatar.gender = (sbyte)gender;
			SeriPart seriPart = new SeriPart();
			seriPart.idPart = ((Part)myVector.elementAt(CRes.rnd(myVector.size()))).IDPart;
			avatar.addSeri(seriPart);
			SeriPart seriPart2 = new SeriPart();
			seriPart2.idPart = ((Part)myVector2.elementAt(CRes.rnd(myVector2.size()))).IDPart;
			avatar.addSeri(seriPart2);
			SeriPart seriPart3 = new SeriPart();
			seriPart3.idPart = ((Part)myVector3.elementAt(CRes.rnd(myVector3.size()))).IDPart;
			avatar.addSeri(seriPart3);
			SeriPart seriPart4 = new SeriPart();
			seriPart4.idPart = ((Part)myVector4.elementAt(CRes.rnd(myVector4.size()))).IDPart;
			avatar.addSeri(seriPart4);
			SeriPart seriPart5 = new SeriPart();
			seriPart5.idPart = ((Part)myVector5.elementAt(CRes.rnd(myVector5.size()))).IDPart;
			avatar.addSeri(seriPart5);
			avatar.orderSeriesPath();
			listPlayer.addElement(avatar);
		}
	}

	public void init()
	{
		AvCamera.gI().init(LoadMap.TYPEMAP);
	}

	public override void doMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Lịch sử", 0, this));
		myVector.addElement(new Command(T.exit, 2, this));
		MenuCenter.gI().startAt(myVector);
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			GlobalService.gI().doHistoryRace();
			Canvas.startWaitDlg();
			break;
		case 1:
			break;
		case 2:
		{
			IAction yes = new IACctionOut();
			if (isStart)
			{
				Canvas.startOKDlg("Bạn có muốn thoát không ?", yes);
			}
			else
			{
				Canvas.startOKDlg("Bạn có muốn thoát không (nếu có đặt cược bạn sẽ mất tiền cược) ?", yes);
			}
			break;
		}
		case 3:
		{
			AvCamera avCamera = AvCamera.gI();
			RaceMsgHandler.PetRace[] array = listPet;
			sbyte b;
			indexFocus = (sbyte)((b = indexFocus) + 1);
			avCamera.followPlayer = array[b];
			if (indexFocus >= 6)
			{
				indexFocus = 0;
			}
			break;
		}
		case 5:
			if (isStart || !isRace)
			{
				right = cmdChangeFocus;
			}
			if (!isStart && isRace)
			{
				focus = 0;
			}
			break;
		case 4:
			break;
		}
	}

	public override void update()
	{
		if (timeOpen >= 0)
		{
			timeOpen--;
			if (timeOpen == 0)
			{
				click();
			}
		}
		if ((isStart || !isRace) && Environment.TickCount - curTimeStart >= 1000)
		{
			curTimeStart = Environment.TickCount;
			timeStart--;
			if (timeStart < 0)
			{
				timeStart = 0;
			}
		}
		GameMidlet.avatar.setPos((int)(AvCamera.gI().xCam + (float)Canvas.hw), (int)(AvCamera.gI().yCam + (float)Canvas.h - (float)(40 * AvMain.hd)));
		if (Environment.TickCount - curTime >= 1000)
		{
			curTime = Environment.TickCount;
			timeRemain--;
			if (timeRemain < 0)
			{
				timeRemain = 0;
			}
			else
			{
				countChangePetInfo++;
			}
		}
		if (listPet != null)
		{
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				if ((isStart || !isRace) && listPet[i].count >= listPet[i].vTick.Length)
				{
					num++;
				}
			}
			if (!isEnd && num == 6)
			{
				isEnd = true;
				for (int j = 0; j < 6; j++)
				{
					LoadMap.removePlayer(listPet[j]);
				}
			}
			if (isEnd && diaWin != null)
			{
				isEnd = false;
				Canvas.currentFace = diaWin;
				GameMidlet.avatar.money[0] += diaWin.tienNhanDuoc;
				Canvas.addFlyText(diaWin.tienNhanDuoc, Canvas.hw, Canvas.h - 30 * AvMain.hd, -1, -1);
				diaWin = null;
			}
		}
		Canvas.loadMap.update();
		if (isStart && countStart > 0)
		{
			countStart--;
		}
		if (myChat != null && myChat.setOut())
		{
			myChat.chats = null;
		}
		if (!isStart && isRace)
		{
			return;
		}
		for (int k = 0; k < LoadMap.playerLists.size(); k++)
		{
			Base @base = (Base)LoadMap.playerLists.elementAt(k);
			if (@base.catagory != 11)
			{
				continue;
			}
			Avatar avatar = (Avatar)@base;
			if (Environment.TickCount / 1000 - avatar.exp > avatar.defence)
			{
				avatar.exp = Environment.TickCount / 1000;
				avatar.defence = (short)(CRes.rnd(10) + 6);
				switch (CRes.rnd(6))
				{
				case 1:
					avatar.setAction(0);
					break;
				case 3:
					avatar.setAction(0);
					avatar.doJumps();
					break;
				case 2:
					avatar.setAction(7);
					break;
				default:
					avatar.setAction(2);
					break;
				}
			}
		}
	}

	public override void keyPress(int keyCode)
	{
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			base.updateKey();
		}
		if (Canvas.isKeyPressed(2))
		{
			focus--;
			if (focus < 0)
			{
				focus = 0;
			}
		}
		else if (Canvas.isKeyPressed(8))
		{
			focus++;
			if (focus > 6)
			{
				focus = 6;
			}
		}
		if (Canvas.isPointerClick && listPet != null && !isStart && isRace)
		{
			if (isDC)
			{
				if (Canvas.isPoint(xSelectDC + wSelectDC - 23 * AvMain.hd, ySelectDC - 18 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					Canvas.isPointerClick = false;
					countCloseDC = 5;
					isTran = true;
					timeDelay = count;
				}
				else
				{
					for (int i = 0; i < 9; i++)
					{
						if (Canvas.isPoint(xSelectDC + 7 * AvMain.hd + i % 3 * (5 * AvMain.hd + imgBackMoney.frameWidth), ySelectDC + (hSelectDC - 29 * AvMain.hd * 3) + i / 3 * 29 * AvMain.hd - AvMain.hd, 70 * AvMain.hd, 26 * AvMain.hd))
						{
							indexDC = i;
							Canvas.isPointerClick = false;
							isTran = true;
							timeDelay = count;
							break;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < 6; j++)
				{
					if (Canvas.isPoint(xPopup + xDC + 6 * AvMain.hd, yPopup + yDC + 3 * AvMain.hd + 35 * AvMain.hd * j, 31 * AvMain.hd, 31 * AvMain.hd))
					{
						indexPet = j;
						isTran = true;
						Canvas.isPointerClick = false;
						timeDelay = count;
						break;
					}
					if (Canvas.isPoint(xPopup + xDC + wDC - 6 * AvMain.hd - imgBackMoney.frameWidth, yPopup + yDC + 3 * AvMain.hd + 35 * AvMain.hd * j, 70 * AvMain.hd, 31 * AvMain.hd))
					{
						indexMoney = j;
						isTran = true;
						Canvas.isPointerClick = false;
						timeDelay = count;
						break;
					}
				}
			}
		}
		if (isTran)
		{
			if (Canvas.isPointerDown)
			{
				if (indexDC != -1)
				{
					if (!Canvas.isPoint(xSelectDC + 7 * AvMain.hd + indexDC % 3 * (5 * AvMain.hd + imgBackMoney.frameWidth), ySelectDC + (hSelectDC - 29 * AvMain.hd * 3) + indexDC / 3 * 29 * AvMain.hd - AvMain.hd, 70 * AvMain.hd, 26 * AvMain.hd))
					{
						indexDC = -1;
					}
				}
				else if (countCloseDC != 0)
				{
					if (!Canvas.isPoint(xSelectDC + wSelectDC - 23 * AvMain.hd, ySelectDC - 18 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
					{
						countCloseDC = 0;
					}
				}
				else if (indexPet != -1)
				{
					if (!Canvas.isPoint(xPopup + xDC + 6 * AvMain.hd, yPopup + yDC + 3 * AvMain.hd + 35 * AvMain.hd * indexPet, 31 * AvMain.hd, 31 * AvMain.hd))
					{
						indexPet = -1;
					}
				}
				else if (indexMoney != -1 && !isDC && !Canvas.isPoint(xPopup + xDC + wDC - 6 * AvMain.hd - imgBackMoney.frameWidth, yPopup + yDC + 3 * AvMain.hd + 35 * AvMain.hd * indexMoney, 70 * AvMain.hd, 31 * AvMain.hd))
				{
					indexMoney = -1;
				}
			}
			if (Canvas.isPointerRelease)
			{
				long num = count - timeDelay;
				if (num <= 4)
				{
					timeOpen = 5;
				}
				else
				{
					click();
				}
				isTran = false;
				Canvas.isPointerRelease = false;
			}
		}
		Canvas.loadMap.updateKey();
	}

	private void click()
	{
		if (indexDC != -1)
		{
			GlobalService.gI().doDatCuoc(listPet[indexMoney].IDDB, iMoney[indexDC]);
			indexDC = -1;
			indexMoney = -1;
			isDC = false;
		}
		else if (countCloseDC == 5)
		{
			countCloseDC = 0;
			isDC = false;
			indexMoney = -1;
		}
		else if (indexPet != -1)
		{
			focus = indexPet;
			GlobalService.gI().doPetInfo(listPet[indexPet].IDDB);
			indexPet = -1;
		}
		else if (indexMoney != -1)
		{
			isDC = true;
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		if (isRace)
		{
			if (isDC)
			{
				paintDC(g);
			}
			else if (Canvas.currentDialog == null)
			{
				Canvas.paint.paintPopupBack(g, xPopup, yPopup, wPopup, hPopup, -1, false);
				g.translate(xPopup, yPopup);
				Canvas.normalFont.drawString(g, T.datCuoc, wPopup / 2, 6 * AvMain.hd, 2);
				MenuNPC.paintPopupTilte(g, xInfo, yInfo, wInfo, Hinfo, imgInfo, -1);
				MenuNPC.paintPopupTilte(g, xDC, yDC, wDC, hDC, MenuNPC.imgDc, 10409727);
				for (int i = 0; i < 6; i++)
				{
					imgBackpet.drawFrame((indexPet == i) ? 1 : 0, xDC + 6 * AvMain.hd, yDC + 3 * AvMain.hd + 35 * AvMain.hd * i, 0, g);
					Canvas.smallFontYellow.drawString(g, "X" + listPet[i].rate, xDC + 6 * AvMain.hd + 31 * AvMain.hd, yDC + 3 * AvMain.hd + 35 * AvMain.hd * i + imgBackpet.frameHeight - AvMain.hSmall, 1);
					AvatarData.paintImg(g, listPet[i].idIcon, xDC + 6 * AvMain.hd + 31 * AvMain.hd / 2, yDC + 3 * AvMain.hd + 35 * AvMain.hd * i + 31 * AvMain.hd / 2, 3);
					imgBackMoney.drawFrame((indexMoney == i && !isDC) ? 1 : 0, xDC + wDC - 6 * AvMain.hd - imgBackMoney.frameWidth, yDC + 7 * AvMain.hd + 35 * AvMain.hd * i, 0, g);
					if (listPet[i].money > 0)
					{
						Canvas.normalFont.drawString(g, string.Empty + listPet[i].money, xDC + wDC - 6 * AvMain.hd - imgBackMoney.frameWidth / 2, yDC + 7 * AvMain.hd + 24 * AvMain.hd / 2 + 35 * AvMain.hd * i - AvMain.hNormal / 2 - 2 * AvMain.hd - ((AvMain.hd == 1) ? 2 : 0), 2);
					}
					else
					{
						Canvas.normalFont.drawString(g, T.datCuoc, xDC + wDC - 6 * AvMain.hd - imgBackMoney.frameWidth / 2, yDC + 7 * AvMain.hd + 24 * AvMain.hd / 2 + 35 * AvMain.hd * i - AvMain.hNormal / 2 - 2 * AvMain.hd - ((AvMain.hd == 1) ? 2 : 0), 2);
					}
				}
				if (isPetInfo && listPet != null)
				{
					paintPetInfo(g);
				}
			}
		}
		else if (isStart && countStart > 0)
		{
			ImageIcon imgIcon = AvatarData.getImgIcon(1065);
			if (imgIcon.count != -1)
			{
				int num = imgIcon.h / 4;
				g.drawRegion(imgIcon.img, 0f, (3 - countStart / 12) * num, imgIcon.w, num, 0, Canvas.w / 2, Canvas.h / 2, 3);
			}
		}
		Canvas.resetTrans(g);
		if (myChat != null && myChat.chats != null)
		{
			myChat.paintAnimal(g);
		}
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			base.paint(g);
		}
		if ((isStart || !isRace) && Canvas.currentDialog == null && isEnd)
		{
			Canvas.borderFont.drawString(g, timeStart + string.Empty, Canvas.hw, 5, 2);
		}
		Canvas.paintPlus(g);
	}

	private void paintDC(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, xSelectDC, ySelectDC, wSelectDC, hSelectDC, countCloseDC / 3, false);
		g.translate(xSelectDC, ySelectDC);
		Canvas.normalFont.drawString(g, "Bản đặt cược", wSelectDC / 2, 10 * AvMain.hd, 2);
		for (int i = 0; i < 9; i++)
		{
			imgBackMoney.drawFrame((indexDC == i) ? 1 : 0, 7 * AvMain.hd + i % 3 * (5 * AvMain.hd + imgBackMoney.frameWidth), hSelectDC - 29 * AvMain.hd * 3 + i / 3 * 29 * AvMain.hd, 0, g);
			Canvas.normalFont.drawString(g, iMoney[i] + string.Empty, 7 * AvMain.hd + i % 3 * (5 * AvMain.hd + imgBackMoney.frameWidth) + imgBackMoney.frameWidth / 2, hSelectDC - 29 * AvMain.hd * 3 + i / 3 * 29 * AvMain.hd + imgBackMoney.frameHeight / 2 - AvMain.hNormal / 2 - 2 * AvMain.hd, 2);
		}
		Canvas.resetTrans(g);
	}

	private void paintPetInfo(MyGraphics g)
	{
		Canvas.normalFont.drawString(g, namePetInfo, xInfo + wInfo / 2, yInfo + 6 * AvMain.hd, 2);
		AvatarData.paintImg(g, idImgPeInfo, xInfo + wInfo / 2, yInfo + 40 * AvMain.hd, 3);
		int num = yInfo + 70 * AvMain.hd;
		Canvas.normalFont.drawString(g, "Thắng", xInfo + 8 * AvMain.hd, num, 0);
		Canvas.normalFont.drawString(g, string.Empty + numWin + "%", xInfo + wInfo - 8 * AvMain.hd, num + AvMain.hNormal / 2 - AvMain.hNormal / 2, 1);
		num += AvMain.hNormal + 2 * AvMain.hd;
		Canvas.normalFont.drawString(g, "Tỉ lệ", xInfo + 8 * AvMain.hd, num, 0);
		Canvas.normalFont.drawString(g, "X" + ratePetInfo, xInfo + wInfo - 8 * AvMain.hd, num + AvMain.hNormal / 2 - AvMain.hNormal / 2, 1);
		num += AvMain.hNormal + 2 * AvMain.hd;
		Canvas.normalFont.drawString(g, "Phong độ", xInfo + 8 * AvMain.hd, num, 0);
		Canvas.normalFont.drawString(g, string.Empty + phongDo[phongDoPetInfo], xInfo + wInfo - 8 * AvMain.hd, num + AvMain.hNormal / 2 - AvMain.hNormal / 2, 1);
		num += AvMain.hNormal + 2 * AvMain.hd;
		Canvas.normalFont.drawString(g, "Sức khỏe", xInfo + 8 * AvMain.hd, num, 0);
		Canvas.normalFont.drawString(g, string.Empty + sucKhoe[sucKhoePetInfo], xInfo + wInfo - 8 * AvMain.hd, num + AvMain.hNormal / 2 - AvMain.hNormal / 2, 1);
		imgTime.drawFrame(0, xInfo + imgTime.frameWidth / 2 + 8 * AvMain.hd, yInfo + Hinfo - AvMain.hBorder - imgTime.frameHeight - 8 * AvMain.hd, 0, 3, g);
		Canvas.normalFont.drawString(g, timeRemain + string.Empty, xInfo + 8 * AvMain.hd + imgTime.frameWidth + 2 * AvMain.hd, yInfo + Hinfo - AvMain.hBorder - imgTime.frameHeight - 8 * AvMain.hd - Canvas.normalFont.getHeight() / 2 - ((AvMain.hd == 1) ? 1 : 0), 0);
		imgTime.drawFrame(1, xInfo + imgTime.frameWidth / 2 + 8 * AvMain.hd, yInfo + Hinfo - AvMain.hBorder - AvMain.hd, 0, 3, g);
		Canvas.normalFont.drawString(g, GameMidlet.avatar.money[0] + string.Empty, xInfo + 8 * AvMain.hd + imgTime.frameWidth + 2 * AvMain.hd, yInfo + Hinfo - AvMain.hBorder - AvMain.hd - AvMain.hNormal / 2 - ((AvMain.hd == 1) ? 1 : 0), 0);
	}

	private void paintTextTfChat(MyGraphics g, TField tf)
	{
		g.setClip(tf.x, tf.y, tf.width + 1, tf.height + 1);
		g.setColor(16579834);
		g.fillRect(tf.x, tf.y, tf.width, tf.height);
		g.setColor(2598571);
		g.drawRect(tf.x, tf.y, tf.width, tf.height);
		g.setClip(tf.x + 3, tf.y + 1, tf.width - 8, tf.height - 2);
		g.setColor(0);
		if (tf.paintedText.Equals(string.Empty))
		{
			Canvas.normalFont.drawString(g, tf.sDefaust, TField.TEXT_GAP_X + tf.offsetX + tf.x, tf.y + (tf.height - AvMain.hBlack) / 2, 0);
		}
		else
		{
			Canvas.blackF.drawString(g, tf.paintedText, TField.TEXT_GAP_X + tf.offsetX + tf.x, tf.y + (tf.height - AvMain.hBlack) / 2, 0);
		}
		if (tf.isFocused() && tf.keyInActiveState == 0 && (tf.showCaretCounter > 0 || tf.counter / 5 % 2 == 0))
		{
			g.setColor(0);
			g.fillRect(TField.TEXT_GAP_X + tf.offsetX + tf.x + Canvas.arialFont.getWidth(tf.paintedText.Substring(0, tf.caretPos)) - 1 + 1, tf.y + 2, 1f, tf.height - 4);
		}
	}

	public override void paintMain(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.loadMap.paint(g);
		Canvas.loadMap.paintObject(g);
		Canvas.resetTrans(g);
	}

	public void onChatFromMe(string text)
	{
		if (!text.Equals(string.Empty))
		{
			myChat = new ChatPopup(50, text, 0);
			myChat.xc = Canvas.hw / AvMain.hd;
			myChat.yc = (Canvas.h - myChat.h - MyScreen.hTab - ChatTextField.gI().tfChat.height) / AvMain.hd;
			GlobalService.gI().chatToBoard(text);
		}
	}

	public void onPetInfo(short idImg, string namePet, short numWin, sbyte tile, sbyte phongDo, sbyte sucKhoe)
	{
		isPetInfo = true;
		idImgPeInfo = idImg;
		namePetInfo = namePet;
		this.numWin = numWin;
		ratePetInfo = tile;
		phongDoPetInfo = phongDo;
		sucKhoePetInfo = sucKhoe;
	}

	public void onChat(string text)
	{
		MyVector myVector = new MyVector();
		int num = (int)AvCamera.gI().xTo;
		if (isStart || !isRace)
		{
			num += Canvas.w / 3;
		}
		for (int i = 0; i < LoadMap.playerLists.size(); i++)
		{
			Base @base = (Base)LoadMap.playerLists.elementAt(i);
			if (@base.catagory == 11 && @base.x * AvMain.hd > num && @base.x * AvMain.hd < num + Canvas.w)
			{
				myVector.addElement(@base);
			}
		}
		if (myVector.size() > 0)
		{
			int i2 = CRes.rnd(myVector.size());
			Avatar avatar = (Avatar)myVector.elementAt(i2);
			avatar.chat = new ChatPopup(50, text, 0);
		}
	}
}
