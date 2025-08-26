using System;
using UnityEngine;

public class LoadMap
{
	private class IActionNextFocus : IAction
	{
		public void perform()
		{
			NextFocus();
		}
	}

	private class IActionExitToCity : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			if (TYPEMAP == 108)
			{
				ParkService.gI().doJoinPark(9, -1);
				return;
			}
			Canvas.startWaitDlg();
			GlobalService.gI().getHandler(9);
		}
	}

	private class IActionExitCity2 : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			if (TYPEMAP == 108)
			{
				ParkService.gI().doJoinPark(9, -1);
			}
			else
			{
				MapScr.gI().doExit();
			}
		}
	}

	private class IActionExitPark2 : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			MapScr.gI().doExit();
		}
	}

	private class IActionCloseAd : IAction
	{
		public void perform()
		{
		}
	}

	private class IActionAd : IAction
	{
		private ObjAd obj;

		public IActionAd(ObjAd ob)
		{
			obj = ob;
		}

		public void perform()
		{
			if (obj.typeShop != -1)
			{
				GlobalService.gI().requestShop(obj.typeShop);
				Canvas.startWaitDlg();
			}
			else if (obj.url != null && !obj.url.Equals(string.Empty))
			{
				GameMidlet.flatForm(obj.url);
			}
			else
			{
				GameMidlet.sendSMS(obj.sms, obj.to, null, null);
			}
		}
	}

	public const sbyte T_PARK = 0;

	public const sbyte T_PARK_1 = 1;

	public const sbyte T_PARK_2 = 2;

	public const sbyte T_PARK_3 = 3;

	public const sbyte T_PARK_4 = 4;

	public const sbyte T_PARK_5 = 5;

	public const sbyte T_PARK_6 = 6;

	public const sbyte T_PARK_7 = 7;

	public const sbyte T_PARK_8 = 8;

	public const sbyte T_CITY = 9;

	public const sbyte T_CONG_SANH_CUOI = 10;

	public const sbyte T_PARK_PATH = 11;

	public const sbyte T_PARK_ADVANCED = 12;

	public const sbyte T_FISHING_ADVANCED = 13;

	public const sbyte T_FISING_1 = 14;

	public const sbyte T_FISING_2 = 15;

	public const sbyte T_FISING_3 = 16;

	public const sbyte T_SLUM = 17;

	public const sbyte T_PRISON = 18;

	public const sbyte T_LE_CUOI = 19;

	public const sbyte T_SAN_BAY = 20;

	public const sbyte T_ROAD_HOUSE = 21;

	public const sbyte T_KHU_MUA_SAM = 23;

	public const sbyte T_FARM = 24;

	public const sbyte T_FARMWAY = 25;

	public const sbyte T_SHOP = 57;

	public const sbyte T_BEAUTYSALON = 58;

	public const sbyte T_GIFT = 59;

	public const sbyte T_BOARD_WAIT_2 = 60;

	public const sbyte T_BOARD_WAIT_4 = 61;

	public const sbyte T_SHOP_2 = 62;

	public const sbyte T_BEAUTYSALON_2 = 63;

	public const sbyte T_GIFT_2 = 64;

	public const sbyte T_POPUP = 27;

	public const sbyte T_STORE = 28;

	public const sbyte T_CHECK_POINT = 29;

	public const sbyte T_JOIN_ANY = -125;

	public const sbyte T_PLANT = 51;

	public const sbyte T_CUAHANG = 52;

	public const sbyte T_FARM_FRIEND = 53;

	public const sbyte T_FISHING_CHAIR = 54;

	public const sbyte T_BANK = 55;

	public const sbyte T_JOIN = 56;

	public const sbyte T_BOARD_WAIT_5 = 65;

	public const sbyte T_NAM_NGHI = 67;

	public const sbyte T_HOUSE_1 = 68;

	public const sbyte T_HOUSE_2 = 69;

	public const sbyte T_HOUSE_3 = 70;

	public const sbyte T_VE_BAY = 71;

	public const sbyte T_TL = 72;

	public const sbyte T_P = 73;

	public const sbyte T_CT = 74;

	public const sbyte T_CR = 75;

	public const sbyte T_DM = 76;

	public const sbyte T_BC = 77;

	public const sbyte T_FOOD_PET = 78;

	public const sbyte T_CHAIR = 79;

	public const sbyte T_EMPTY = 80;

	public const sbyte T_BUS_STOP = 81;

	public const sbyte T_OBJECT = 82;

	public const sbyte T_AD = 83;

	public const sbyte T_PIG_TROUGH = 84;

	public const sbyte T_DOG_TROUGH = 85;

	public const sbyte T_MILK_BUCKET = 86;

	public const sbyte T_NEST = 87;

	public const sbyte T_ROCK = 88;

	public const sbyte T_TOP_FARM = 89;

	public const sbyte T_NOTENTER = 90;

	public const sbyte T_NOT_FISHING = 91;

	public const sbyte T_CHAIR_RED = 92;

	public const sbyte T_ICE_CREAM = 93;

	public const sbyte T_FISH_SHOP = 94;

	public const sbyte T_UPDATE_CATTLE = 95;

	public const sbyte T_UPDATE_FISH = 96;

	public const sbyte T_STAR_FRUIT = 97;

	public const sbyte T_COOKING = 98;

	public const sbyte T_QUAY_SO = 99;

	public const sbyte T_TASK = 100;

	public const sbyte T_DAU_GIA = 106;

	public const sbyte T_UPGRADE = 102;

	public const sbyte T_PRIMEUM_SHOP = 103;

	public const sbyte T_PET_SHOP = 104;

	public const sbyte T_THO_KHOA = 105;

	public const sbyte T_MR_DOOM = 101;

	public const sbyte T_RACE = 107;

	public const sbyte T_CASINO = 108;

	public const sbyte T_CASINO_2 = 109;

	public const sbyte T_HOUSE_4 = 110;

	public const sbyte T_FLOWER_LOVE = 111;

	public const sbyte T_HO_HOUSE = 112;

	public const short INSTANCE = 127;

	public const sbyte MAPMAIN = -1;

	public static int TYPEMAP = -1;

	public static Image imgDen;

	public static Image imgBG;

	public static FrameImage imgMap;

	public static short[] map;

	public static short wMap;

	public static short Hmap;

	public static sbyte[] type;

	public static sbyte[] bg = new sbyte[26]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
		1, 1, 3, 3, 3, 3, 3, 2, -1, -1,
		-1, 0, 0, 0, 2, 2
	};

	public static int w = 24;

	public static sbyte status = 0;

	public static sbyte weather = -1;

	public static MyVector treeLists = new MyVector();

	public static MyVector playerLists = new MyVector();

	public static MyVector dynamicLists = new MyVector();

	public static MyVector listImgAD;

	public static int fWint = 0;

	public static int star = 0;

	public AvPosition[] clound;

	public static MyVector listStar = new MyVector();

	public static int[] colorStar = new int[4] { 15853390, 15006199, 8183509, 12254198 };

	public static MyObject focusObj;

	public static Command cmdNext;

	public static Image imgShadow;

	public static FrameImage imgFocus;

	private static int[] colorBg = new int[2] { 6143735, 21 };

	public static Color colorBackGr;

	public static int rememMap = -1;

	public static int rememBg = -1;

	public static AvPosition posFocus;

	public static MyVector effBgList;

	public static MyVector effCameraList;

	public static MyVector effManager;

	public static int idTileImg = -1;

	public static Bus bus = new Bus();

	private Image[] imgClound = new Image[2];

	private int hBG;

	private Image imgTreeBg;

	private static int x0_imgTreeBg = 0;

	private static int x0_imgBG = 0;

	private int countRndSound;

	private long timeCurSound;

	public static MyVector listDeltaPosition = new MyVector();

	public static float zoom = 0f;

	public static float disTouch;

	public static bool trans;

	public static bool isGo;

	public static int dirFocus = -1;

	public static string test = string.Empty;

	private int pxLast;

	private int pyLast;

	public float pa;

	public float pb;

	public float dyTran;

	public float dxTran;

	public bool transY;

	public bool transX;

	private long count;

	private long timeDelay;

	private long timePointY;

	private long timePointX;

	private sbyte iTop;

	private sbyte iLeft;

	private int xFirFocus;

	private int yFirFocus;

	private int xfirDu;

	private int yfirDu;

	private int xLastDu;

	private int yLastDu;

	private bool[][] used;

	private short[] to;

	private short[] from;

	private short[] mPath;

	public static int nPath = 0;

	private static int wFocus = 3;

	public static int xJoinCasino;

	public static int yJoinCasino;

	private MyObject player;

	private MyObject obj;

	private MyObject dynamic;

	private Base temp1 = new Base();

	private SubObject temp2 = new SubObject(10000, 10000);

	private int p;

	private int o;

	private int d;

	private static int numF = 0;

	private Image imgDayDien0;

	private Image imgDayDien1;

	private Image imgDayDien2;

	public static Image[] imgCreateMap;

	public static int typeAny = 0;

	public static int typeTemp = -1;

	public static bool isCasino = false;

	public static MyVector mapItemType;

	public static MyVector mapItem;

	public static int xDichChuyen = -1;

	public static int yDichChuyen = -1;

	public LoadMap()
	{
		cmdNext = new Command(T.next, new IActionNextFocus());
		star = CRes.rnd(3);
		w = 24;
		imgDen = Image.createImagePNG(T.getPath() + "/effect/den");
		imgShadow = Image.createImage(T.getPath() + "/effect/s0");
		imgFocus = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/focus"), 32 * AvMain.hd, 11 * AvMain.hd);
		posFocus = new AvPosition();
	}

	public void loadBG(int i)
	{
		if (i != 107 && (i < 0 || i >= bg.Length || bg[i] == -1))
		{
			rememBg = -1;
			imgBG = null;
			imgTreeBg = null;
			imgClound = null;
			return;
		}
		if (imgBG == null)
		{
			rememBg = -1;
		}
		if (i != 107 && rememBg == bg[i] && rememMap == status)
		{
			return;
		}
		int num = 0;
		num = (rememBg = ((i != 107) ? bg[i] : 0));
		imgTreeBg = Image.createImagePNG(T.getPath() + "/bgHD/may" + num + string.Empty + status);
		imgBG = Image.createImagePNG(T.getPath() + "/bgHD/" + num + string.Empty + status);
		imgClound = new Image[2];
		for (int j = 0; j < 2; j++)
		{
			imgClound[j] = Image.createImagePNG(T.getPath() + "/effect/cl" + j + status);
		}
		x0_imgBG = (x0_imgTreeBg = 0);
		if (rememBg == 1)
		{
			x0_imgTreeBg = 25 * AvMain.hd;
			x0_imgBG += 20 * AvMain.hd;
		}
		else if (rememBg == 0)
		{
			x0_imgTreeBg = 20 * AvMain.hd;
			x0_imgBG = 2 * AvMain.hd;
		}
		else if (rememBg == 2)
		{
			x0_imgTreeBg = 10 * AvMain.hd;
			x0_imgBG = 15 * AvMain.hd;
		}
		else if (rememBg == 3)
		{
			x0_imgTreeBg = 33 * AvMain.hd;
			if (status == 1)
			{
				x0_imgBG = 12 * AvMain.hd;
				x0_imgTreeBg -= 12 * AvMain.hd;
			}
		}
	}

	public void resetImg()
	{
		imgTreeBg = null;
		rememBg = -1;
		rememMap = -1;
		if (idTileImg == -1)
		{
			imgMap = null;
		}
		imgCreateMap = null;
		AvatarData.listImgIcon.clear();
		AvatarData.listImgPart.clear();
		Resources.UnloadUnusedAssets();
	}

	public void updateKey()
	{
		if (PopupShop.gI() != Canvas.currentMyScreen && Input.touchCount <= 1 && !Canvas.isZoom)
		{
			updatePointer();
		}
	}

	private void updateSound()
	{
		if (imgBG == null)
		{
			return;
		}
		if (countRndSound == 0)
		{
			countRndSound = 5 + CRes.rnd(5);
			timeCurSound = Canvas.getTick();
			switch (CRes.rnd(2))
			{
			}
		}
		else if (Canvas.getTick() - timeCurSound >= 1000)
		{
			timeCurSound = Canvas.getTick();
			countRndSound--;
		}
	}

	public void update()
	{
		if (!Canvas.isZoom && Canvas.currentMyScreen != HouseScr.me)
		{
			AvCamera.gI().update();
		}
		updateSound();
		if (Canvas.menuMain == null && GameMidlet.avatar.task == -5 && Input.touchCount <= 1 && !Canvas.isZoom)
		{
			updatePathAvatar();
		}
		if ((Canvas.stypeInt == 0 || Canvas.currentMyScreen != MainMenu.gI()) && playerLists.size() > 0)
		{
			for (int i = 0; i < playerLists.size(); i++)
			{
				MyObject myObject = (MyObject)playerLists.elementAt(i);
				myObject.update();
			}
			orderVector(playerLists);
		}
		if (dynamicLists.size() > 0)
		{
			orderVector(dynamicLists);
			for (int j = 0; j < dynamicLists.size(); j++)
			{
				((MyObject)dynamicLists.elementAt(j)).update();
			}
		}
		if (treeLists.size() > 0)
		{
			for (int k = 0; k < treeLists.size(); k++)
			{
				MyObject myObject2 = (MyObject)treeLists.elementAt(k);
				myObject2.update();
			}
		}
		updateClound();
		if (Canvas.gameTick % 4 == 2 && !FarmScr.isSelected && !FarmScr.isAutoVatNuoi && Canvas.currentMyScreen != RaceScr.me)
		{
			setFocus();
		}
		if (Bus.isRun)
		{
			bus.update();
		}
		if (effManager != null)
		{
			for (int l = 0; l < effManager.size(); l++)
			{
				EffectManager effectManager = (EffectManager)effManager.elementAt(l);
				effectManager.update();
			}
		}
		if (effBgList != null)
		{
			for (int m = 0; m < effBgList.size(); m++)
			{
				EffectObj effectObj = (EffectObj)effBgList.elementAt(m);
				effectObj.update();
			}
		}
		if (effCameraList != null)
		{
			for (int n = 0; n < effCameraList.size(); n++)
			{
				EffectObj effectObj2 = (EffectObj)effCameraList.elementAt(n);
				effectObj2.update();
			}
		}
		if (imgFocus != null && dirFocus != -1 && nPath > 0)
		{
			posFocus.anchor++;
			if (posFocus.anchor >= 10)
			{
				posFocus.anchor = 0;
			}
		}
		numF++;
		if (numF >= 6)
		{
			numF = 0;
		}
	}

	public void updatePointer()
	{
		if (GameMidlet.avatar.ableShow || Canvas.isZoom || Canvas.currentDialog != null || Canvas.currentFace != null || Canvas.menuMain != null || ((Canvas.welcome != null) & Welcome.isPaintArrow))
		{
			return;
		}
		float num = AvMain.zoom;
		count++;
		if (Canvas.isPointerClick && Canvas.isPointer(0, 0, Canvas.w, Canvas.hCan))
		{
			pyLast = Canvas.pyLast;
			pxLast = Canvas.pxLast;
			Canvas.isPointerClick = false;
			timeDelay = count;
			pa = AvCamera.gI().yCam;
			pb = AvCamera.gI().xCam;
			transY = true;
			AvCamera.gI().vY = 0f;
			AvCamera.gI().vX = 0f;
		}
		if (!transY)
		{
			return;
		}
		long num2 = count - timeDelay;
		int num3 = (int)((float)(pyLast - Canvas.py) / AvMain.zoom);
		pyLast = Canvas.py;
		int num4 = (int)((float)(pxLast - Canvas.px) / AvMain.zoom);
		pxLast = Canvas.px;
		if (Canvas.isPointerDown)
		{
			if (count % 2 == 0)
			{
				dyTran = Canvas.py;
				dxTran = Canvas.px;
				timePointY = count;
				timePointX = count;
			}
			AvCamera.gI().vY = 0f;
			AvCamera.gI().vX = 0f;
			if (Canvas.currentMyScreen != HouseScr.me && (Canvas.w < wMap * w * AvMain.hd + w * 2 * AvMain.hd || Canvas.hCan < Hmap * w * 2 * AvMain.hd + w * 2 * AvMain.hd))
			{
				if ((float)Canvas.w < (float)(wMap * w * AvMain.hd) * AvMain.zoom)
				{
					AvCamera.gI().xTo = (int)(pb + (float)num4);
					if (AvCamera.gI().xTo <= 0f)
					{
						AvCamera.gI().xTo = 0f;
					}
					else if (AvCamera.gI().xTo > AvCamera.gI().xLimit)
					{
						AvCamera.gI().xTo = AvCamera.gI().xLimit;
					}
				}
				else
				{
					AvCamera.gI().xTo = (0f - ((float)Canvas.w - (float)(wMap * w * AvMain.hd) * AvMain.zoom)) / 2f;
				}
			}
			if (imgBG != null || TYPEMAP == 68 || TYPEMAP == 69 || TYPEMAP == 70 || TYPEMAP == 110 || idTileImg != -1 || Canvas.w < wMap * w * AvMain.hd + w * 2 * AvMain.hd || Canvas.hCan < Hmap * w * 2 * AvMain.hd + w * 2 * AvMain.hd)
			{
				AvCamera.gI().yTo = (int)(pa + (float)num3);
				if (Canvas.currentMyScreen == HouseScr.me)
				{
					if (AvCamera.gI().yTo < (float)(-(Canvas.hCan / 3)))
					{
						AvCamera.gI().yTo = -(Canvas.hCan / 3);
					}
					if (AvCamera.gI().yTo > AvCamera.gI().yLimit + (float)(w * AvMain.hd))
					{
						AvCamera.gI().yTo = AvCamera.gI().yLimit + (float)(w * AvMain.hd);
					}
					if ((float)Canvas.w < (float)(wMap * w * AvMain.hd) * AvMain.zoom)
					{
						AvCamera.gI().xTo = (int)(pb + (float)num4);
						if (AvCamera.gI().xTo <= (float)(-w * AvMain.hd))
						{
							AvCamera.gI().xTo = -w * AvMain.hd;
						}
						else if (AvCamera.gI().xTo > AvCamera.gI().xLimit + (float)(w * AvMain.hd))
						{
							AvCamera.gI().xTo = AvCamera.gI().xLimit + (float)(w * AvMain.hd);
						}
					}
					else
					{
						AvCamera.gI().xTo = (0f - ((float)Canvas.w - (float)(wMap * w * AvMain.hd) * AvMain.zoom)) / 2f;
					}
				}
				else if (AvCamera.gI().yTo < (float)(-Canvas.hCan))
				{
					AvCamera.gI().yTo = -Canvas.hCan;
				}
				else if (imgBG != null && AvCamera.gI().yTo > AvCamera.gI().yLimit)
				{
					AvCamera.gI().yTo = AvCamera.gI().yLimit;
				}
				pa = AvCamera.gI().yTo;
				AvCamera.gI().yCam = AvCamera.gI().yTo;
			}
			pb = AvCamera.gI().xTo;
			AvCamera.gI().xCam = AvCamera.gI().xTo;
			if (CRes.abs(Canvas.dx()) > 20 * AvMain.hd || CRes.abs(Canvas.dy()) > 20 * AvMain.hd)
			{
				AvCamera.gI().timeDelay = Environment.TickCount / 100;
			}
		}
		else
		{
			transY = false;
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		transY = false;
		int num5 = (int)(count - timePointY);
		float num6 = dyTran - (float)Canvas.py;
		float num7 = dxTran - (float)Canvas.px;
		if (!MapScr.isWedding)
		{
			if (CRes.abs(Canvas.dx()) > 20 * AvMain.hd || CRes.abs(Canvas.dy()) > 20 * AvMain.hd)
			{
				AvCamera.gI().timeDelay = Environment.TickCount / 100;
				if (CRes.abs((int)num6) > 20 * AvMain.hd && num5 < 20)
				{
					AvCamera.gI().vY = num6 / (float)num5 * 5f;
					AvCamera.isMove = true;
				}
				if (!((float)(Hmap * w * AvMain.hd) > (float)(Canvas.hCan / 3 * 2) / AvMain.zoom))
				{
					AvCamera.gI().vY = 0f;
				}
				int num8 = (int)(count - timePointY);
				if (CRes.abs((int)num7) > 20 * AvMain.hd && num8 < 20 && AvCamera.gI().xTo > 0f && AvCamera.gI().xTo < AvCamera.gI().xLimit)
				{
					AvCamera.gI().vX = num7 / (float)num8 * 5f;
					AvCamera.isMove = true;
				}
			}
			else
			{
				int num9 = (int)((float)Canvas.px + AvCamera.gI().xCam);
				int num10 = (int)((float)Canvas.py + AvCamera.gI().yCam);
				int xF = (int)((float)Canvas.px / num + AvCamera.gI().xCam);
				int yF = (int)((float)Canvas.py / num + AvCamera.gI().yCam - (float)Canvas.transTab);
				bool flag = false;
				for (int i = 0; i < treeLists.size(); i++)
				{
					MyObject myObject = (MyObject)treeLists.elementAt(i);
					if (myObject.index != -1 && num9 > myObject.x * AvMain.hd - myObject.w / 2 && num9 < myObject.x * AvMain.hd - myObject.w / 2 + myObject.w && num10 > myObject.y * AvMain.hd - myObject.h && num10 < myObject.y * AvMain.hd && setJoin(myObject.index))
					{
						Canvas.isPointerRelease = false;
						posFocus.x = myObject.x * AvMain.hd;
						posFocus.y = (myObject.y - 4) * AvMain.hd;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					posFocus.x = xF;
					posFocus.y = yF;
					SoundManager.playSound(7);
				}
				int num11 = 88;
				int num12 = w * AvMain.hd;
				if (posFocus.y < 0)
				{
					posFocus.y = num12 + num12 / 2;
				}
				if (posFocus.y / num12 * wMap + posFocus.x / num12 > 0 && posFocus.y / num12 * wMap + posFocus.x / num12 < type.Length)
				{
					num11 = type[posFocus.y / num12 * wMap + posFocus.x / num12];
				}
				posFocus.anchor = 0;
				if (GameMidlet.avatar.task == 0 || GameMidlet.avatar.task == -5)
				{
					GameMidlet.avatar.task = -5;
					GameMidlet.avatar.isJumps = -1;
					GameMidlet.avatar.xCur = GameMidlet.avatar.x;
					GameMidlet.avatar.yCur = GameMidlet.avatar.y;
					xfirDu = GameMidlet.avatar.x % w;
					yfirDu = GameMidlet.avatar.y % w;
					xLastDu = posFocus.x % num12 / 2;
					yLastDu = posFocus.y % num12 / 2 + 3;
					xFirFocus = GameMidlet.avatar.x;
					yFirFocus = GameMidlet.avatar.y;
					if (GameMidlet.avatar.y > posFocus.y / AvMain.hd)
					{
						iTop = -1;
					}
					else
					{
						iTop = 1;
					}
					if (GameMidlet.avatar.x > posFocus.x / AvMain.hd)
					{
						iLeft = 1;
					}
					else
					{
						iLeft = -1;
					}
					if (GameMidlet.avatar.x > posFocus.x / AvMain.hd)
					{
						dirFocus = Base.LEFT;
					}
					else
					{
						dirFocus = Base.RIGHT;
					}
					if (!Canvas.paint.selectedPointer(xF, yF) && (num11 == 80 || TYPEMAP == 24 || setTypeJoint(num11) || setTypeFind(num11)))
					{
						change();
					}
				}
			}
		}
		timePointY = -1L;
		timePointX = -1L;
		transX = false;
	}

	public bool setJoin(int type)
	{
		switch (type)
		{
		case 18:
		case 24:
		case 25:
		case 29:
		case 52:
		case 55:
		case 58:
		case 59:
		case 63:
		case 64:
		case 68:
		case 69:
		case 70:
		case 89:
		case 93:
		case 94:
		case 97:
		case 98:
		case 100:
		case 103:
		case 104:
		case 107:
		case 108:
		case 109:
		case 110:
		case 111:
		case 112:
			return true;
		default:
			return false;
		}
	}

	public void change()
	{
		if (GameMidlet.avatar.action == 14)
		{
			Out.println("pos: " + posFocus.x / AvMain.hd + "    " + GameMidlet.avatar.x + "    " + GameMidlet.avatar.direct);
			if (posFocus.x / AvMain.hd < GameMidlet.avatar.x && GameMidlet.avatar.direct == Base.RIGHT)
			{
				GameMidlet.avatar.direct = Base.LEFT;
				return;
			}
			if (posFocus.x / AvMain.hd > GameMidlet.avatar.x && GameMidlet.avatar.direct == Base.LEFT)
			{
				GameMidlet.avatar.direct = Base.RIGHT;
				return;
			}
			GameMidlet.avatar.action = 0;
			GameMidlet.avatar.setPos(HouseScr.gI().xHo, HouseScr.gI().yHo);
			AvatarService.gI().doFeel(0);
			MapScr.gI().doMove(GameMidlet.avatar.x, GameMidlet.avatar.y, GameMidlet.avatar.direct);
		}
		else
		{
			int num = w * AvMain.hd;
			if (GameMidlet.avatar.x / w == posFocus.x / num && GameMidlet.avatar.y / w == posFocus.y / num)
			{
				GameMidlet.avatar.setPosTo(posFocus.x / num * w + posFocus.x % num / 2, posFocus.y / num * w + posFocus.y % num / 2);
			}
			else if (!findPath(GameMidlet.avatar.x / w, GameMidlet.avatar.y / w, posFocus.x / num, posFocus.y / num))
			{
				nPath = 0;
				dirFocus = -1;
			}
		}
	}

	public void updatePathAvatar()
	{
		if (GameMidlet.avatar.xCur != GameMidlet.avatar.x || GameMidlet.avatar.yCur != GameMidlet.avatar.y)
		{
			return;
		}
		nPath--;
		if (nPath < 0)
		{
			GameMidlet.avatar.task = 0;
			dirFocus = -1;
			GameMidlet.avatar.isSetAction = false;
			return;
		}
		int num = mPath[nPath] & 0xFF;
		int num2 = mPath[nPath] >> 8;
		int num3 = w / 2;
		int num4 = w / 2;
		if (iTop == -1)
		{
			if (num2 == yFirFocus / w && num3 > yfirDu)
			{
				num3 = yfirDu;
			}
			else if (num2 == posFocus.y / AvMain.hd / w && num3 < yLastDu)
			{
				num3 = yLastDu;
			}
		}
		else if (num3 > yLastDu && num2 == posFocus.y / AvMain.hd / w)
		{
			num3 = yLastDu;
		}
		else if (num2 == GameMidlet.avatar.y / w && num3 < yfirDu)
		{
			num3 = yfirDu;
		}
		if (iLeft == 1)
		{
			if (num == xFirFocus / w && num4 > xfirDu)
			{
				num4 = xfirDu;
			}
			else if (num == posFocus.x / AvMain.hd / w && num4 < xLastDu)
			{
				num4 = xLastDu;
			}
		}
		else if (num == xFirFocus / w && num4 < xfirDu)
		{
			num4 = xfirDu;
		}
		else if (num == posFocus.x / AvMain.hd / w && num4 > xLastDu)
		{
			num4 = xLastDu;
		}
		num = num * w + num4;
		num2 = num2 * w + num3;
		if (nPath == 0)
		{
			GameMidlet.avatar.isSetAction = true;
			num = posFocus.x / AvMain.hd;
			num2 = posFocus.y / AvMain.hd;
			nPath = 0;
		}
		if (!GameMidlet.avatar.doJoin(num - GameMidlet.avatar.x, num2 - GameMidlet.avatar.y) && !GameMidlet.avatar.detectCollisionMap(num - GameMidlet.avatar.x, num2 - GameMidlet.avatar.y))
		{
			if (!setTypeFindEnd(getTypeMap(num, num2)))
			{
				GameMidlet.avatar.setPosTo(num, num2);
				GameMidlet.avatar.action = 1;
			}
		}
		else
		{
			nPath = 0;
			GameMidlet.avatar.isSetAction = false;
		}
	}

	public void initFindPath()
	{
		int num = wMap;
		int hmap = Hmap;
		used = new bool[num][];
		for (int i = 0; i < num; i++)
		{
			used[i] = new bool[hmap];
		}
		to = new short[num * hmap];
		from = new short[num * hmap];
		mPath = new short[num * hmap];
	}

	public static void resetPath()
	{
		nPath = 0;
		GameMidlet.avatar.isSetAction = false;
		GameMidlet.avatar.task = 0;
		Out.println("resetPath");
		dirFocus = -1;
	}

	public bool findPath(int from_x, int from_y, int to_x, int to_y)
	{
		int num = 1;
		int num2 = 0;
		int[] array = new int[4] { 0, -1, 1, 0 };
		int[] array2 = new int[4] { -1, 0, 0, 1 };
		bool flag = false;
		for (int i = 0; i < used.Length * used[0].Length; i++)
		{
			to[i] = 0;
			from[i] = 0;
			mPath[i] = 0;
			if (type[i] != 80 && type[i] != 51 && !setTypeJoint(type[i]))
			{
				used[i % wMap][i / wMap] = true;
			}
			else
			{
				used[i % wMap][i / wMap] = false;
			}
		}
		int typeMap = getTypeMap(to_x * w, to_y * w);
		if (setTypeFind(typeMap))
		{
			used[to_x][to_y] = false;
		}
		to[num2] = (short)((from_y << 8) + from_x);
		while (!flag && num2 < num)
		{
			int num3 = to[num2] & 0xFF;
			int num4 = to[num2] >> 8;
			for (int j = 0; j < 4; j++)
			{
				if (flag)
				{
					break;
				}
				int num5 = num3 + array[j];
				int num6 = num4 + array2[j];
				if (num5 >= 0 && num5 < used.Length && num6 >= 0 && num6 < used[0].Length && !used[num5][num6])
				{
					from[num] = to[num2];
					to[num++] = (short)((num6 << 8) + num5);
					used[num5][num6] = true;
					if (to_x == num5 && to_y == num6)
					{
						flag = true;
					}
				}
				if (num >= used.Length * used[0].Length)
				{
					flag = true;
					break;
				}
			}
			num2++;
		}
		nPath = 0;
		if (flag)
		{
			GameMidlet.avatar.resetAction();
			int j = num - 1;
			mPath[nPath++] = to[j];
			while (j > 0)
			{
				for (int k = 0; k < num; k++)
				{
					if (to[k] == from[j])
					{
						j = k;
						mPath[nPath++] = to[j];
						break;
					}
				}
			}
		}
		nPath--;
		return flag;
	}

	private void updateClound()
	{
		if (clound == null || imgBG == null || imgClound == null)
		{
			return;
		}
		for (int i = 0; i < clound.Length; i++)
		{
			clound[i].x -= clound[i].index;
			if (clound[i].x / 100 < -imgClound[clound[i].anchor].w)
			{
				int num = 0;
				if (rememBg == 0)
				{
					num = -10 * AvMain.hd;
				}
				else if (rememBg == 2)
				{
					num = 5 * AvMain.hd;
				}
				else if (rememBg == 3)
				{
					num = 25 * AvMain.hd;
				}
				clound[i].anchor = CRes.rnd(2);
				clound[i].y = -(imgBG.h / 2 + imgClound[clound[i].anchor].h + num + CRes.rnd(imgBG.h / 2));
				if (clound[i].anchor == 1)
				{
					clound[i].index = (short)(10 + CRes.rnd(30));
				}
				else
				{
					clound[i].index = (short)(30 + CRes.rnd(30));
				}
				clound[i].x = (wMap * w + imgClound[clound[i].anchor].w + CRes.rnd(imgBG.w)) * 100;
			}
		}
	}

	public static void setFocus()
	{
		if (TYPEMAP == -1 || isGo || Canvas.currentMyScreen == MainMenu.me || Canvas.menuMain != null)
		{
			return;
		}
		if (focusObj == null)
		{
			for (int i = 0; i < playerLists.size() && !setFocus(i); i++)
			{
			}
		}
		else if (CRes.abs(focusObj.x - GameMidlet.avatar.x) / w >= wFocus || CRes.abs(focusObj.y - GameMidlet.avatar.y) / w >= wFocus)
		{
			focusObj = null;
			MapScr.focusP = null;
		}
	}

	public static void NextFocus()
	{
		if (focusObj == null)
		{
			return;
		}
		isGo = false;
		int num = 0;
		int num2 = playerLists.size();
		for (int i = 0; i < num2; i++)
		{
			MyObject myObject = (MyObject)playerLists.elementAt(i);
			if (myObject.catagory != 4 && myObject == focusObj)
			{
				num = i;
				break;
			}
		}
		focusObj = null;
		for (int j = num + 1; j < num2 && !setFocus(j); j++)
		{
		}
		if (focusObj == null)
		{
			for (int k = 0; k <= num && !setFocus(k); k++)
			{
			}
		}
	}

	private static bool setFocus(int i)
	{
		MyObject myObject = (MyObject)playerLists.elementAt(i);
		if (myObject.catagory != 4 && myObject != GameMidlet.avatar && myObject.catagory != 6 && Math.abs(myObject.x - GameMidlet.avatar.x) / w < wFocus && Math.abs(myObject.y - GameMidlet.avatar.y) / w < wFocus)
		{
			if (myObject.catagory != 0 || !((Avatar)myObject).ableShow)
			{
				focusObj = myObject;
			}
			if (myObject.catagory == 0 && !((Avatar)myObject).ableShow)
			{
				MapScr.focusP = (Avatar)playerLists.elementAt(i);
			}
			return true;
		}
		return false;
	}

	private bool setTypeJoint(int type)
	{
		if (type >= -125 && type < 0)
		{
			return true;
		}
		if (type == -1 || type == 0 || type == 1 || type == 2 || type == 3 || type == 4 || type == 5 || type == 6 || type == 7 || type == 8 || type == 12 || type == 11 || type == 14 || type == 15 || type == 16 || type == 13 || type == 25 || type == 24 || type == 52 || type == 53 || type == 9 || type == 56 || type == 72 || type == 73 || type == 75 || type == 74 || type == 76 || type == 77 || type == 21 || type == 68 || type == 110 || type == 69 || type == 70 || type == 17 || type == 18 || type == 51 || type == 71 || type == 95 || type == 96 || type == 111 || type == 112)
		{
			return true;
		}
		return false;
	}

	public bool setTypeFind(int type)
	{
		if (type >= -125 && type < 0)
		{
			return true;
		}
		if (type == 57 || type == 62 || type == 58 || type == 63 || type == 59 || type == 64 || type == 99 || type == 106 || type == 108 || type == 109 || type == 55 || type == 93 || type == 78 || type == 89 || type == 27 || type == 28 || type == 29 || type == 84 || type == 85 || type == 86 || type == 83 || type == 87 || type == 54 || type == 67 || type == 81 || type == 71 || type == 79 || type == 92 || type == 52 || type == 94 || type == 95 || type == 96 || type == 97 || type == 98 || type == 100 || type == 103 || type == 101 || type == 104 || type == 23 || type == 107 || type == 19 || type == 10)
		{
			return true;
		}
		return false;
	}

	public bool setTypeFindEnd(int type)
	{
		if (type >= -125 && type < 0)
		{
			return true;
		}
		if (type == 57 || type == 62 || type == 58 || type == 63 || type == 59 || type == 64 || type == 99 || type == 106 || type == 108 || type == 109 || type == 55 || type == 93 || type == 78 || type == 89 || type == 27 || type == 28 || type == 29 || type == 84 || type == 85 || type == 86 || type == 83 || type == 87 || type == 54 || type == 71 || type == 52 || type == 94 || type == 95 || type == 96 || type == 97 || type == 98 || type == 100 || type == 103 || type == 101 || type == 104 || type == 23 || type == 107 || type == 19 || type == 10)
		{
			return true;
		}
		return false;
	}

	public bool doJoin(int x, int y)
	{
		isGo = false;
		int typeMap = getTypeMap(x, y);
		if (typeMap == -2)
		{
			return false;
		}
		switch (typeMap)
		{
		case -1:
			MapScr.gI().move();
			if (TYPEMAP == 25)
			{
				FarmScr.gI().doExitBus();
			}
			if (imgBG != null)
			{
				bus.setBus(-1);
			}
			else
			{
				MapScr.gI().doExit();
			}
			break;
		case 55:
			Canvas.startWaitDlg();
			GlobalService.gI().requestChargeMoneyInfo();
			break;
		case 108:
		case 109:
			Canvas.startWaitDlg();
			MapScr.idCityMap = 1;
			MapScr.idSelectedMini = 0;
			xJoinCasino = GameMidlet.avatar.x;
			yJoinCasino = GameMidlet.avatar.y;
			GlobalService.gI().requestJoinAny(4);
			break;
		case 57:
			MapScr.gI().move();
			MapScr.gI().doJoinShop(1);
			break;
		case 62:
			MapScr.gI().move();
			MapScr.gI().doJoinShop(6);
			break;
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 11:
		case 13:
		case 14:
		case 15:
		case 16:
		case 18:
			MapScr.gI().move();
			ParkService.gI().doJoinPark(typeMap, -1);
			break;
		case 17:
			Canvas.startOKDlg(T.doYouWantExit2, new IActionExitToCity());
			break;
		case 12:
			Canvas.startOKDlg(T.doYouWantExit2, new IActionExitToCity());
			break;
		case 25:
			FarmScr.gI().doGoFarmWay();
			break;
		case 24:
			if (FarmScr.cell == null || FarmScr.idFarm != GameMidlet.avatar.IDDB)
			{
				Canvas.startWaitDlg();
				FarmScr.gI().doJoinFarm(GameMidlet.avatar.IDDB, true);
			}
			else
			{
				FarmScr.gI().onJoin(FarmScr.idFarm, FarmScr.cell, FarmScr.animalLists, FarmScr.numBarn, FarmScr.numPond, FarmScr.foodID, FarmScr.remainTime);
			}
			break;
		case 52:
			FarmScr.gI().doOpenCuaHang();
			break;
		case 53:
			FarmScr.gI().doMenuFarmFriend();
			break;
		case 9:
			Canvas.startOKDlg(T.doYouWantExit2, new IActionExitToCity());
			break;
		case 58:
			MapScr.gI().doJoinShop(2);
			break;
		case 63:
			MapScr.gI().doJoinShop(7);
			break;
		case 59:
			MapScr.gI().doJoinShop(3);
			break;
		case 64:
			MapScr.gI().doJoinShop(8);
			break;
		case 27:
		case 56:
			if (TYPEMAP != 18 && TYPEMAP != 109 && TYPEMAP != 108 && GameMidlet.CLIENT_TYPE == 8)
			{
				MapScr.gI().doOpenShopOffline(GameMidlet.avatar, 0);
			}
			break;
		case 28:
			FarmScr.gI().doOpenKhoHang();
			break;
		case 29:
			Canvas.startWaitDlg();
			ParkService.gI().doRequestBoardList(MapScr.roomID);
			break;
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
			MapScr.indexMap = TYPEMAP;
			xJoinCasino = GameMidlet.avatar.x;
			yJoinCasino = GameMidlet.avatar.y;
			if (typeMap - 72 == 2 && Canvas.iOpenOngame == 1)
			{
				Canvas.startWaitDlg();
				MapScr.idCityMap = 1;
				MapScr.idSelectedMini = 0;
				GlobalService.gI().requestJoinAny(4);
			}
			else
			{
				MapScr.gI().doGetHandlerCasino(typeMap - 72);
			}
			break;
		case 93:
			MapScr.gI().doOpenIceDream(T.food, 4);
			break;
		case 78:
			MapScr.gI().doOpenIceDream(T.food, 5);
			break;
		case 83:
			Canvas.loadMap.getAd(x / w, y / w);
			break;
		case 84:
			FarmScr.gI().doCattleFeeding(2, 5);
			break;
		case 85:
			FarmScr.gI().doCattleFeeding(3, 5);
			break;
		case 86:
		{
			int num4 = getposMap(x, y);
			int num5 = getposMap(Cattle.posBucket.x, Cattle.posBucket.y);
			FarmScr.gI().doHarvestAnimal(2, num4 - num5, FarmScr.listBucket);
			break;
		}
		case 87:
		{
			int num2 = getposMap(x, y);
			int num3 = getposMap(Chicken.posNest.x, Chicken.posNest.y);
			FarmScr.gI().doHarvestAnimal(1, num2 - num3, FarmScr.listNest);
			break;
		}
		case 89:
		{
			int num = 0;
			num = ((TYPEMAP == 108 || TYPEMAP == 109) ? 1 : ((TYPEMAP != 13) ? 3 : 2));
			GlobalService.gI().doCommunicate(num);
			Canvas.startWaitDlg();
			break;
		}
		case 54:
			return FishingScr.gI().doSat(x, y);
		case 21:
			HouseScr.gI().doOut();
			break;
		case 68:
		case 69:
		case 70:
			HouseScr.gI().doJoinFriendHome(typeMap - 67);
			break;
		case 110:
			Canvas.startWaitDlg();
			AvatarService.gI().doJoinHouse4(GameMidlet.avatar.IDDB);
			break;
		case 20:
			GlobalService.gI().requestJoinAny(0);
			Canvas.startWaitDlg();
			break;
		case 71:
			Canvas.startWaitDlg();
			GlobalService.gI().requestCityMap(-1);
			break;
		case 94:
			GlobalService.gI().doCommunicate(4);
			Canvas.startWaitDlg();
			break;
		case 95:
			Canvas.startWaitDlg();
			FarmScr.xRemember = GameMidlet.avatar.x;
			FarmScr.yRemember = GameMidlet.avatar.y;
			FarmService.gI().doUpdateFarm(0, 0);
			break;
		case 96:
			Canvas.startWaitDlg();
			FarmScr.xRemember = GameMidlet.avatar.x;
			FarmScr.yRemember = GameMidlet.avatar.y;
			FarmService.gI().doUpdateFish(0, 0);
			break;
		case 103:
			MapScr.gI().doJoinMapOffline(3);
			break;
		case 23:
			GlobalService.gI().getHandler(9);
			Canvas.startWaitDlg();
			break;
		case 104:
			MapScr.gI().doJoinMapOffline(4);
			break;
		case 100:
			MapScr.gI().doJoinMapOffline(5);
			break;
		case 101:
			MapScr.gI().doJoinMapOffline(6);
			break;
		case 97:
			FarmScr.gI().doMenuStarFruit();
			break;
		case 98:
			FarmScr.gI().doOpenCooking();
			break;
		case 107:
			Canvas.startWaitDlg();
			MapScr.indexMap = TYPEMAP;
			xJoinCasino = GameMidlet.avatar.x;
			yJoinCasino = GameMidlet.avatar.y;
			GlobalService.gI().getHandler(12);
			break;
		case 19:
			Canvas.startWaitDlg();
			MapScr.gI().move();
			rememMap = -1;
			ParkService.gI().doJoinPark(19, -1);
			break;
		case 10:
			Canvas.startWaitDlg();
			MapScr.gI().move();
			rememMap = -1;
			ParkService.gI().doJoinPark(10, -1);
			break;
		case 111:
			Canvas.startWaitDlg();
			GlobalService.gI().doFlowerLove();
			break;
		default:
			if (typeMap == 112)
			{
				if (GameMidlet.avatar.action != 14)
				{
					HouseScr.gI().xHo = GameMidlet.avatar.x;
					HouseScr.gI().yHo = GameMidlet.avatar.y;
					GameMidlet.avatar.setPos(x / w * w + w / 2 + 2, y / w * w + 5);
					MapScr.gI().doMove(GameMidlet.avatar.x, GameMidlet.avatar.y, GameMidlet.avatar.direct);
					GameMidlet.avatar.doAction(14);
					AvatarService.gI().doFeel(14);
				}
			}
			else
			{
				if (typeMap < -125 || typeMap >= 0)
				{
					return false;
				}
				Canvas.startWaitDlg();
				typeTemp = typeMap;
				GlobalService.gI().requestJoinAny((short)(typeMap - -125));
			}
			return true;
		}
		return true;
	}

	public void paintEffectCamera(MyGraphics g)
	{
		if (effCameraList != null)
		{
			for (int i = 0; i < effCameraList.size(); i++)
			{
				EffectObj effectObj = (EffectObj)effCameraList.elementAt(i);
				effectObj.paint(g);
			}
		}
	}

	public void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintBGCMD(g, 0, Canvas.h, Canvas.w, Canvas.hTab);
		Canvas.resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		paintM(g);
	}

	public void paintCreateMap(MyGraphics g, int x)
	{
		if (imgCreateMap == null)
		{
			return;
		}
		int num = 0;
		int num2 = 1;
		if (imgCreateMap.Length == 1)
		{
			num2 = 0;
		}
		for (int i = 0; i < imgCreateMap.Length; i++)
		{
			if (AvCamera.gI().xCam + (float)Canvas.w > (float)(x - num2 + num) && AvCamera.gI().xCam < (float)(x - num2 + num + (imgCreateMap[i].w - num2 * 2)))
			{
				g.drawImage(imgCreateMap[i], x - num2 + num, Hmap * w * AvMain.hd - imgCreateMap[i].h, 0);
			}
			num += imgCreateMap[i].w - num2 * 2;
		}
	}

	public void paintM(MyGraphics g)
	{
		paintBackGround(g);
		if (imgCreateMap != null)
		{
			if ((float)Canvas.w < (float)(wMap * w * AvMain.hd) * AvMain.zoom)
			{
				g.setColor(0);
			}
			paintCreateMap(g, 0);
			if (imgDayDien0 != null)
			{
				g.drawImage(imgDayDien0, 0f, w + ((AvMain.hd == 2) ? w : 0) + w / 2 - imgDayDien0.getHeight(), 0);
			}
			if (imgDayDien1 != null)
			{
				g.drawImage(imgDayDien1, imgDayDien0.getWidth(), w + ((AvMain.hd == 2) ? w : 0) + w / 2 - imgDayDien0.getHeight(), 0);
			}
			if (imgDayDien2 != null)
			{
				g.drawImage(imgDayDien2, imgDayDien0.getWidth() + imgDayDien1.getWidth(), w + ((AvMain.hd == 2) ? w : 0) + w / 2 - imgDayDien0.getHeight(), 0);
			}
		}
		else
		{
			paintMap(g);
		}
		paintTouchMap(g);
		if ((float)Canvas.w > (float)(wMap * w) * zoom)
		{
			g.setColor(0);
			g.fillRect((int)AvCamera.gI().xCam, (int)AvCamera.gI().yCam, -(int)AvCamera.gI().xCam, Canvas.hCan);
			g.fillRect((float)(wMap * w * AvMain.hd) * AvMain.zoom, (int)AvCamera.gI().yCam, -(int)AvCamera.gI().xCam, Canvas.hCan);
		}
	}

	public void paintTouchMap(MyGraphics g)
	{
		if (imgFocus != null && dirFocus != -1 && nPath > 0)
		{
			imgFocus.drawFrame(posFocus.anchor / 2, posFocus.x, posFocus.y, dirFocus, 3, g);
		}
	}

	public void paintMap(MyGraphics g)
	{
		float num = (AvCamera.gI().xCam + (float)Canvas.w) / (float)w + 1f;
		if (num > (float)wMap)
		{
			num = wMap;
		}
		float num2 = (AvCamera.gI().yCam + (float)Canvas.h) / (float)w + 1f;
		if (num2 > (float)Hmap)
		{
			num2 = Hmap;
		}
		int num3 = (int)(AvCamera.gI().xCam / (float)(w * AvMain.hd));
		if (num3 < 0)
		{
			num3 = 0;
		}
		for (int i = 0; (float)i < num2; i++)
		{
			for (int j = num3; (float)j < num; j++)
			{
				int num4 = map[i * wMap + j];
				if (num4 != -1)
				{
					int idx = num4 / imgMap.nFrame;
					imgMap.drawFrameXY(idx, num4 % imgMap.nFrame, j * (w * AvMain.hd), i * w * AvMain.hd, 0, g);
				}
			}
		}
	}

	public void paintObject(MyGraphics g)
	{
		try
		{
			p = 0;
			o = 0;
			d = 0;
			temp1.x = 10000;
			temp1.y = 10000;
			while (p < playerLists.size() || o < treeLists.size() || d < dynamicLists.size())
			{
				player = temp1;
				obj = temp2;
				dynamic = temp1;
				if (p < playerLists.size())
				{
					player = (MyObject)playerLists.elementAt(p);
				}
				if (o < treeLists.size())
				{
					obj = (MyObject)treeLists.elementAt(o);
				}
				if (d < dynamicLists.size())
				{
					dynamic = (Point)dynamicLists.elementAt(d);
				}
				if (player.y < obj.y && player.y < dynamic.y)
				{
					player.paint(g);
					p++;
				}
				else if (obj.y < dynamic.y)
				{
					obj.paint(g);
					o++;
				}
				else
				{
					dynamic.paint(g);
					d++;
				}
			}
			paintFocusPlayer(g);
			if (Bus.isRun)
			{
				bus.paint(g);
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	private void paintFocusPlayer(MyGraphics g)
	{
		if (Canvas.stypeInt == 0 && focusObj != null)
		{
			int num = ((focusObj.catagory != 7) ? focusObj.height : 10);
			g.drawImage(MapScr.imgFocusP, focusObj.x * AvMain.hd, (focusObj.y - num) * AvMain.hd - numF / 2, 3);
		}
	}

	public void paintBackGround(MyGraphics g)
	{
		if (imgBG == null)
		{
			g.setColor(1);
			g.fillRect((int)AvCamera.gI().xCam, (int)AvCamera.gI().yCam, Canvas.w, Canvas.hCan);
			return;
		}
		if (idTileImg != -1)
		{
			g.setColor(colorBackGr);
		}
		else
		{
			g.setColor(colorBg[status]);
		}
		g.fillRect((int)AvCamera.gI().xCam, (int)AvCamera.gI().yCam, Canvas.w, Canvas.h);
		int num = (int)(AvCamera.gI().xCam * 30f / 30f);
		int num2 = (int)((AvCamera.gI().xCam - (float)num) / (float)imgBG.w);
		int num3 = -imgBG.h * AvMain.hd;
		if (Canvas.currentMyScreen == RaceScr.me)
		{
			num3 += 2 * w * AvMain.hd;
		}
		if (idTileImg != -1)
		{
			num3 += AvMain.hd;
		}
		if (imgTreeBg != null)
		{
			for (int i = num2; i <= num2 + Canvas.w / imgTreeBg.w + 1; i++)
			{
				g.drawImage(imgTreeBg, num + i * (imgTreeBg.w - 2) - 1, -imgTreeBg.h - x0_imgTreeBg, 0);
			}
		}
		int num4 = listStar.size();
		if (num4 > 0)
		{
			for (int j = 0; j < num4; j++)
			{
				AvPosition avPosition = (AvPosition)listStar.elementAt(j);
				if ((float)(avPosition.x + num) > AvCamera.gI().xCam && (float)(avPosition.x + num) < AvCamera.gI().yCam + (float)Canvas.w)
				{
					g.setColor(colorStar[avPosition.anchor]);
					g.fillRect(avPosition.x + num, avPosition.y, 1f, 1f);
				}
			}
		}
		if (clound != null && imgBG != null)
		{
			int num5 = (int)(AvCamera.gI().xCam * 30f / 35f);
			for (int k = 0; k < clound.Length; k++)
			{
				if (clound[k].anchor == 1 && (float)(num5 + clound[k].x / 100 + imgClound[clound[k].anchor].w) > AvCamera.gI().xCam && (float)(num5 + clound[k].x / 100) < AvCamera.gI().xCam + (float)Canvas.w)
				{
					g.drawImage(imgClound[clound[k].anchor], num5 + clound[k].x / 100, clound[k].y, 0);
				}
			}
			int num6 = (int)(AvCamera.gI().xCam * 30f / 40f);
			for (int l = 0; l < clound.Length; l++)
			{
				if (clound[l].anchor == 0 && (float)(num6 + clound[l].x / 100 + imgClound[clound[l].anchor].w) > AvCamera.gI().xCam && (float)(num6 + clound[l].x / 100) < AvCamera.gI().xCam + (float)Canvas.w)
				{
					g.drawImage(imgClound[clound[l].anchor], num6 + clound[l].x / 100, clound[l].y, 0);
				}
			}
		}
		int num7 = (int)(AvCamera.gI().xCam * 30f / 50f);
		int num8 = (int)((AvCamera.gI().xCam - (float)num7) / (float)imgBG.w);
		if (imgBG != null)
		{
			for (int m = num8; m <= num8 + Canvas.w / imgBG.w + 1; m++)
			{
				g.drawImage(imgBG, num7 + m * (imgBG.w - 2) - 1, -imgBG.h + x0_imgBG, 0);
			}
		}
		if (Canvas.currentEffect.size() > 0)
		{
			for (int n = 0; n < Canvas.currentEffect.size(); n++)
			{
				Effect effect = (Effect)Canvas.currentEffect.elementAt(n);
				effect.paintBack(g);
			}
		}
		if (effBgList != null)
		{
			for (int num9 = 0; num9 < effBgList.size(); num9++)
			{
				EffectObj effectObj = (EffectObj)effBgList.elementAt(num9);
				effectObj.paint(g);
			}
		}
	}

	public static void loadMapImage(int index)
	{
		Out.println("loadMapImage: " + index);
		if (rememMap == status && imgMap != null)
		{
			return;
		}
		rememMap = status;
		switch (index)
		{
		case 20:
			rememMap = -1;
			imgMap = new FrameImage(Image.createImage(T.getPath() + "/wedding"), w * AvMain.hd, w * AvMain.hd);
			return;
		case 108:
			try
			{
				w = 12;
				rememMap = -1;
				if (index - 1 == 107)
				{
					x0_imgBG = 30 * AvMain.hd;
					x0_imgTreeBg = -30 * AvMain.hd;
				}
				return;
			}
			catch (Exception e)
			{
				Out.logError(e);
				return;
			}
		}
		DataInputStream resourceAsStream = DataInputStream.getResourceAsStream(T.getPath() + "/data/h" + status);
		DataInputStream resourceAsStream2 = DataInputStream.getResourceAsStream(T.getPath() + "/data/data");
		try
		{
			sbyte[] data = new sbyte[resourceAsStream.available()];
			resourceAsStream.read(ref data);
			sbyte[] data2 = new sbyte[resourceAsStream2.available()];
			resourceAsStream2.read(ref data2);
			imgMap = new FrameImage(CRes.createImgByHeader(data, data2), w * AvMain.hd, w * AvMain.hd);
		}
		catch (Exception e2)
		{
			Out.logError(e2);
		}
	}

	public static void setStar()
	{
		listStar.removeAllElements();
		if (status == 0 || star == 0 || weather != -1)
		{
			return;
		}
		if (TYPEMAP == 9 || TYPEMAP == 12)
		{
			int num = CRes.rnd(Canvas.w / 10);
			for (int i = 0; i < num; i++)
			{
				listStar.addElement(new AvPosition(CRes.rnd(wMap * w), -(98 + CRes.rnd(Canvas.hh)), CRes.rnd(4)));
			}
		}
		else
		{
			int num2 = CRes.rnd(Canvas.w / 10);
			for (int j = 0; j < num2; j++)
			{
				listStar.addElement(new AvPosition(CRes.rnd(wMap * w), -(38 + CRes.rnd(Canvas.hh)), CRes.rnd(4)));
			}
		}
	}

	public static DataInputStream loadDataMap(int index)
	{
		try
		{
			return DataInputStream.getResourceAsStream("map/" + index);
		}
		catch (Exception)
		{
			Out.println("ERROR LOAD DATA MAP");
		}
		return null;
	}

	private void loadImageMapFull(int index)
	{
		imgCreateMap = null;
		Image[] array = new Image[10];
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			array[i] = Image.createImagePNG(T.getPath() + "/imageMap/" + index + "/" + i);
			if (array[i] == null)
			{
				num = i;
				break;
			}
		}
		if (num > 0)
		{
			imgCreateMap = new Image[num];
			for (int j = 0; j < num; j++)
			{
				imgCreateMap[j] = array[j];
			}
		}
		array = null;
		imgDayDien0 = Image.createImagePNG(T.getPath() + "/imageMap/" + index + "/daydien0");
		imgDayDien1 = Image.createImagePNG(T.getPath() + "/imageMap/" + index + "/daydien1");
		imgDayDien2 = Image.createImagePNG(T.getPath() + "/imageMap/" + index + "/daydien2");
	}

	public void load(int index, bool isCreate)
	{
		if (index - 1 == 14 || index - 1 == 15 || index - 1 == 16)
		{
			AvMain.zoom = 1f;
		}
		if (Session_ME.gI().isConnected())
		{
			Canvas.load = 0;
			Canvas.endDlg();
		}
		ipKeyboard.isReset = true;
		Canvas.rh = 0;
		hBG = 214 * AvMain.hd;
		nPath = 0;
		idTileImg = -1;
		isCasino = false;
		cmdNext.caption = T.next;
		Canvas.currentEffect.removeAllElements();
		GameMidlet.avatar.ableShow = false;
		Bus.isRun = false;
		AvCamera.disable = false;
		GameMidlet.avatar.setAction(0);
		resetObject();
		MapScr.listFish.removeAllElements();
		focusObj = null;
		MapScr.focusP = null;
		int hour = DateTime.Now.Hour;
		if (hour >= 18 || hour < 6)
		{
			status = 1;
		}
		else
		{
			status = 0;
		}
		loadBG(index - 1);
		loadImageMapFull(index);
		if (imgCreateMap == null || index - 1 == 107)
		{
			loadMapImage(index);
		}
		else
		{
			imgMap = null;
			isCreate = false;
		}
		DataInputStream dataInputStream = loadDataMap(index);
		MyScreen.colorBar = MyScreen.colorMiniMap;
		if (dataInputStream != null)
		{
			Hmap = 8;
			switch (index - 1)
			{
			case 25:
				MyScreen.colorBar = MyScreen.colorFarmPath[status];
				Hmap = 7;
				break;
			case 21:
				MyScreen.colorBar = MyScreen.colorCity[status];
				Hmap = 7;
				break;
			case 9:
				MyScreen.colorBar = MyScreen.colorCity[status];
				Hmap = 8;
				break;
			case 20:
			case 57:
			case 58:
			case 59:
			case 62:
			case 63:
			case 64:
			case 100:
			case 101:
			case 103:
			case 104:
			case 108:
			case 109:
				Hmap = 11;
				break;
			case 60:
			case 61:
			case 65:
				Hmap = 5;
				break;
			case 18:
				Hmap = 10;
				break;
			case 17:
				MyScreen.colorBar = MyScreen.colorFarmPath[status];
				Hmap = 6;
				break;
			case 11:
			case 13:
				MyScreen.colorBar = MyScreen.colorCity[status];
				break;
			case 107:
				Hmap = 16;
				break;
			case 19:
				Hmap = 13;
				break;
			case 10:
				Hmap = 9;
				break;
			}
		}
		setMap(dataInputStream, index, true);
		TYPEMAP = index - 1;
		if (weather != -1 && TYPEMAP < bg.Length && bg[TYPEMAP] != -1)
		{
			AnimateEffect animateEffect = new AnimateEffect(weather, false, 0);
			animateEffect.show();
		}
		setClound();
		if (Session_ME.gI().isConnected() && GameMidlet.avatar.seriPart != null)
		{
			addPlayer(GameMidlet.avatar);
		}
		if (Canvas.load == 0)
		{
			Canvas.load = 1;
		}
	}

	public void setMap(DataInputStream ip, int index, bool newType)
	{
		sbyte b = 0;
		sbyte b2 = 0;
		sbyte b3 = 0;
		sbyte b4 = 0;
		sbyte b5 = 0;
		sbyte b6 = 0;
		sbyte b7 = 0;
		sbyte b8 = 0;
		sbyte b9 = 0;
		sbyte b10 = 0;
		sbyte b11 = 0;
		sbyte b12 = 0;
		sbyte b13 = 0;
		sbyte b14 = 0;
		sbyte b15 = 0;
		sbyte b16 = 0;
		sbyte b17 = 0;
		sbyte b18 = 0;
		sbyte b19 = 0;
		sbyte b20 = 0;
		sbyte b21 = 0;
		sbyte b22 = 0;
		sbyte b23 = 0;
		sbyte b24 = 0;
		sbyte b25 = 0;
		sbyte b26 = 0;
		sbyte b27 = 0;
		sbyte b28 = 0;
		sbyte b29 = 0;
		sbyte b30 = 0;
		sbyte b31 = 0;
		sbyte b32 = 0;
		sbyte b33 = 0;
		sbyte b34 = 0;
		sbyte b35 = 0;
		sbyte b36 = 0;
		sbyte b37 = 0;
		sbyte b38 = 0;
		sbyte b39 = 0;
		sbyte b40 = 0;
		sbyte b41 = 0;
		sbyte b42 = 0;
		int num = 0;
		sbyte[] array = new sbyte[13];
		try
		{
			if (ip != null)
			{
				wMap = (short)(ip.available() / Hmap);
				map = new short[Hmap * wMap];
			}
			if (newType)
			{
				type = new sbyte[Hmap * wMap];
			}
			for (int i = 0; i < Hmap * wMap; i++)
			{
				if (ip != null)
				{
					map[i] = (byte)ip.readByte();
					if (map[i] == 255)
					{
						map[i] = -1;
					}
				}
			}
			switch (index)
			{
			case 20:
			{
				MapScr.listChair = new MyVector();
				for (int k = 0; k < map.Length; k++)
				{
					if (map[k] < 32)
					{
						type[k] = 80;
					}
					else
					{
						type[k] = 88;
					}
					if (map[k] == 65)
					{
						type[k] = 10;
						map[k] = 1;
						if (b42 == 1)
						{
							map[k] = 16;
							GameMidlet.avatar.x = (GameMidlet.avatar.xCur = x(k) + w);
							GameMidlet.avatar.y = (GameMidlet.avatar.yCur = y(k) + 12);
							addPopup(T.joinA, x(k) + w / 2, y(k) + 12);
						}
						b42++;
					}
					else if (map[k] == 27)
					{
						AvPosition avPosition2 = new AvPosition();
						avPosition2.x = x(k);
						avPosition2.y = y(k);
						avPosition2.index = (short)((5 - MapScr.listChair.size() % 6) * 2 + MapScr.listChair.size() / 6);
						MapScr.listChair.addElement(avPosition2);
					}
				}
				Avatar avatar = new Avatar();
				avatar.x = (avatar.xCur = 26 * w + w / 2);
				avatar.y = (avatar.yCur = 8 * w + w / 2);
				avatar.name = "chu hon";
				avatar.IDDB = -100;
				avatar.addSeri(new SeriPart(2480));
				playerLists.addElement(avatar);
				break;
			}
			case 108:
			{
				for (int l = 0; l < Hmap * wMap; l++)
				{
					if (map[l] == 61 && CRes.rnd(2) == 1)
					{
						Avatar avatar2 = new Avatar();
						Avatar avatar3 = (Avatar)RaceScr.gI().listPlayer.elementAt(CRes.rnd(RaceScr.gI().listPlayer.size()));
						avatar2.seriPart = avatar3.seriPart;
						avatar2.x = (avatar2.xCur = x(l) + 12);
						avatar2.y = (avatar2.yCur = y(l) + 12);
						avatar2.action = 2;
						avatar2.catagory = 11;
						playerLists.addElement(avatar2);
					}
					if (map[l] == 59 || map[l] == 60)
					{
					}
				}
				break;
			}
			default:
			{
				for (int j = 0; j < Hmap * wMap; j++)
				{
					if (map[j] == -4)
					{
						type[j] = 80;
					}
					else if (map[j] == -5)
					{
						type[j] = 88;
					}
					else if (map[j] != -3 && map[j] != -6)
					{
						if (map[j] >= 120 && map[j] <= 123)
						{
							type[j] = 80;
						}
						else if (map[j] >= 114 && map[j] <= 119)
						{
							type[j] = 80;
						}
						else if (map[j] == 67 || map[j] == 85)
						{
							type[j] = 92;
						}
						else if (map[j] >= 20 && map[j] <= 23)
						{
							type[j] = 79;
						}
						else if (map[j] < 7)
						{
							type[j] = 80;
						}
						else
						{
							type[j] = 88;
						}
						if (map[j] >= 44 && map[j] <= 55)
						{
							type[j] = 80;
						}
						if (index - 1 != 103 && index - 1 != 100 && index - 1 != 101 && index - 1 != 104 && map[j] == 62 && index - 1 != 62)
						{
							type[j] = 56;
						}
						if (map[j] == 111 || map[j] == 112)
						{
							type[j] = 80;
						}
					}
					if (ip != null || GameMidlet.CLIENT_TYPE != 11)
					{
						switch (map[j])
						{
						case -1:
							type[j] = 88;
							break;
						case 24:
						case 25:
						case 26:
							if (newType && (index - 1 != 9 || (index - 1 == 9 && j / wMap > wMap / 2)))
							{
								addObjTree(845, x(j) + w / 2, y(j) + w, -1);
							}
							break;
						case 27:
							if (newType)
							{
								addObjTree(844, x(j) + 11, y(j) + 1, -1);
							}
							break;
						case 28:
							if (newType && !Session_ME.gI().isConnected())
							{
								map[j] = 4;
							}
							break;
						case 127:
							if (b34 == 0)
							{
								if (index - 1 != 9)
								{
									addObjTree(830, x(j) + 36, y(j) + w - 2, 108);
								}
								int i17 = j;
								sbyte num19 = b34;
								b34 = (sbyte)(num19 + 1);
								setPopup(i17, num19, 2);
							}
							setTypeMap(j, 108, 96);
							break;
						case 128:
							if (index - 1 != 25)
							{
								int i19 = j;
								sbyte num21 = b;
								b = (sbyte)(num21 + 1);
								setPopup(i19, num21, 2);
								setTypeMap(j, 55, 20);
								map[j] = map[j + wMap];
							}
							break;
						case 129:
						case 160:
							if (b2 == 0)
							{
								switch (index)
								{
								case 18:
									addObjTree(836, j, (map[j] != 129) ? 62 : 57);
									break;
								default:
									addObjTree(829, j, (map[j] != 129) ? 62 : 57);
									break;
								case 24:
									break;
								}
								int i11 = j;
								sbyte num13 = b2;
								b2 = (sbyte)(num13 + 1);
								setPopup(i11, num13, 2);
							}
							setTypeMap(j, (sbyte)((map[j] != 129) ? 62 : 57), 96);
							break;
						case 130:
						case 131:
						case 132:
						case 133:
						case 134:
						case 135:
						case 136:
						case 137:
						case 138:
						{
							int num16 = map[j] - 130;
							int i14 = j;
							ref sbyte reference4 = ref array[num16];
							sbyte b43;
							reference4 = (sbyte)((b43 = reference4) + 1);
							setPopup(i14, b43, 0);
							setTypePark(j, (sbyte)num16);
							break;
						}
						case 153:
							if (b32 == 0)
							{
								int i4 = j;
								sbyte num6 = b32;
								b32 = (sbyte)(num6 + 1);
								setPopup(i4, num6, 0);
							}
							setTypePark(j, 11);
							break;
						case 139:
						{
							type[j] = -1;
							int i13 = j;
							sbyte num15 = b29;
							b29 = (sbyte)(num15 + 1);
							setPopup(i13, num15, 0);
							if (TYPEMAP == -1 && index != 21 && imgBG != null)
							{
								Bus.posBusStop = new AvPosition(x(j) + w / 2, y(j) - w / 2);
								bus.setBus(1);
							}
							setMapPaint(j, map);
							break;
						}
						case 140:
						{
							int i12 = j;
							sbyte num14 = b27;
							b27 = (sbyte)(num14 + 1);
							setPopup(i12, num14, 0);
							setTypeMap(j, 25, 55);
							b27++;
							break;
						}
						case 141:
							if (b4 == 0)
							{
								int i18 = j;
								sbyte num20 = b4;
								b4 = (sbyte)(num20 + 1);
								setPopup(i18, num20, 0);
							}
							setTypeMap(j, 24, 5);
							map[j] = map[j + wMap];
							break;
						case 142:
							setTypeMap(j, 80, 4);
							FarmScr.gI().posTree[b14] = new AvPosition(x(j) / w, y(j) / w, 0);
							b14++;
							break;
						case 143:
							if (b3 == 0)
							{
								int i9 = j;
								sbyte num11 = b3;
								b3 = (sbyte)(num11 + 1);
								setPopup(i9, num11, 2);
							}
							setTypeMap(j, 52, 51);
							map[j] = map[j + wMap];
							break;
						case 144:
							if (b5 == 0)
							{
								int i3 = j;
								sbyte num5 = b5;
								b5 = (sbyte)(num5 + 1);
								setPopup(i3, num5, 2);
							}
							setTypeMap(j, 53, 5);
							break;
						case 145:
						{
							int i10 = j;
							sbyte num12 = b28;
							b28 = (sbyte)(num12 + 1);
							setPopup(i10, num12, 0);
							if (index - 1 == 109 || (index - 1 == 57 && TYPEMAP == 17))
							{
								setTypeMap(j, 17, -1);
							}
							else if (TYPEMAP == 23)
							{
								setTypeMap(j, 23, -1);
							}
							else
							{
								setTypeMap(j, 9, -1);
								if (index - 1 == 100)
								{
									map[j] = 47;
								}
							}
							if (index - 1 == 100)
							{
								map[j] = 47;
							}
							break;
						}
						case 147:
						case 161:
							if (b6 == 0)
							{
								if (index - 1 != 23)
								{
									addObjTree(832, j, (map[j] != 147) ? 63 : 58);
								}
								int i20 = j;
								sbyte num22 = b6;
								b6 = (sbyte)(num22 + 1);
								setPopup(i20, num22, 2);
							}
							setTypeMap(j, (sbyte)((map[j] != 147) ? 63 : 58), 96);
							break;
						case 148:
						case 162:
							if (b7 == 0)
							{
								if (index - 1 != 23)
								{
									addObjTree(833, x(j) + 48, y(j) + w - 2, (map[j] != 148) ? 64 : 59);
								}
								int i6 = j;
								sbyte num8 = b7;
								b7 = (sbyte)(num8 + 1);
								setPopup(i6, num8, 2);
							}
							map[j] = 0;
							setTypeMap(j, (sbyte)((map[j] != 148) ? 64 : 59), 96);
							break;
						case 149:
							if (b8 == 0)
							{
								if (GameMidlet.avatar.IDDB == FarmScr.idFarm)
								{
									setPopup(j, b8, 2);
								}
								b8++;
							}
							setTypeMap(j, 28, 4);
							break;
						case 150:
							if (b33 == 0)
							{
								addObjTree(842, j, 93);
							}
							if (index == 26)
							{
								setTypeMap(j, 93, 4);
							}
							else
							{
								setTypeMap(j, 93, 0);
							}
							b33++;
							break;
						case 151:
							if (b35 == 0)
							{
								addObjTree(843, j, 78);
							}
							setTypeMap(j, 78, 0);
							b35++;
							break;
						case 152:
							if (b9 == 0)
							{
								addObjTree(835, j, 81);
							}
							setTypeMap(j, 81, (index - 1 == 25) ? 55 : 0);
							b9++;
							break;
						case 155:
							setTypeMap(j, 80, 55);
							if (Cattle.numPig > 0)
							{
								setTypeMap(j, 84, 112);
								addObjTree(-5, x(j) + w / 2, y(j) + w / 2, 84);
								Cattle.posPigTr = new AvPosition(x(j) + w / 2, y(j) + w / 2);
							}
							break;
						case 156:
							setTypeMap(j, 80, 5);
							if (Dog.numBer > 0)
							{
								setTypeMap(j, 85, 5);
								addObjTree(-6, x(j) + w / 2, y(j) + w / 2, 85);
								Dog.posDosTr = new AvPosition(x(j) + w / 2, y(j) + w / 2);
							}
							break;
						case 157:
							setTypeMap(j, 80, 111);
							Cattle.posBucket = new AvPosition(x(j) + w / 2, y(j) + w / 2);
							break;
						case 158:
							setTypeMap(j, 80, 5);
							if (Chicken.numChicken > 0)
							{
								Chicken.posNest = new AvPosition(x(j) + w / 2, y(j) + w / 2);
							}
							break;
						case 159:
						{
							int m = 4;
							switch (index)
							{
							case 26:
								m = 4;
								break;
							case 109:
							case 110:
								m = 47;
								break;
							case 14:
								m = 0;
								break;
							}
							setTypeMap(j, 89, m);
							if (b11 == 0)
							{
								addObjTree(848, x(j) + 12, y(j) + 20, 89);
							}
							b11++;
							break;
						}
						case 163:
						{
							int i2 = j;
							sbyte num2 = b28;
							b28 = (sbyte)(num2 + 1);
							setPopup(i2, num2, 0);
							setTypeMap(j, 12, -1);
							break;
						}
						case 164:
						{
							setPopup(j, array[9], 0);
							ref sbyte reference3 = ref array[9];
							reference3++;
							setTypeMap(j, 13, 6);
							break;
						}
						case 165:
						{
							setPopup(j, array[10], 0);
							setTypeMap(j, 14, 0);
							ref sbyte reference = ref array[10];
							reference++;
							break;
						}
						case 166:
						{
							setPopup(j, array[11], 0);
							setTypeMap(j, 15, 0);
							ref sbyte reference2 = ref array[11];
							reference2++;
							break;
						}
						case 167:
						{
							setPopup(j, array[12], 0);
							ref sbyte reference5 = ref array[12];
							reference5++;
							setTypeMap(j, 16, 43);
							break;
						}
						case 172:
							setTypeMap(j, 88, 96);
							break;
						case 173:
							setTypeMap(j, 88, 96);
							break;
						case 174:
							setTypeMap(j, 88, 96);
							break;
						case 175:
						{
							int i24 = j;
							sbyte num26 = b15;
							b15 = (sbyte)(num26 + 1);
							setPopup(i24, num26, 0);
							setTypeMap(j, 68, 96);
							break;
						}
						case 176:
						{
							int i22 = j;
							sbyte num24 = b16;
							b16 = (sbyte)(num24 + 1);
							setPopup(i22, num24, 0);
							setTypeMap(j, 69, 96);
							break;
						}
						case 177:
						{
							int i23 = j;
							sbyte num25 = b17;
							b17 = (sbyte)(num25 + 1);
							setPopup(i23, num25, 0);
							setTypeMap(j, 70, 96);
							break;
						}
						case 178:
							if (b34 == 0)
							{
								addObjTree(830, x(j) + w, y(j) + w - 2, 109);
								int i21 = j;
								sbyte num23 = b34;
								b34 = (sbyte)(num23 + 1);
								setPopup(i21, num23, 2);
							}
							setTypeMap(j, 109, 96);
							break;
						case 179:
							if (b17 == 0)
							{
								int i16 = j;
								sbyte num18 = b17;
								b17 = (sbyte)(num18 + 1);
								setPopup(i16, num18, 2);
								addObjTree(837, j, 18);
							}
							setTypeMap(j, 18, 96);
							break;
						case 180:
						{
							int i15 = j;
							sbyte num17 = b17;
							b17 = (sbyte)(num17 + 1);
							setPopup(i15, num17, 0);
							setTypeMap(j, 17, 77);
							if (index - 1 == 101)
							{
								map[j] = 0;
							}
							break;
						}
						case 181:
							if (index - 1 != 103 && index - 1 != 100 && index - 1 != 101 && index - 1 != 104)
							{
								if (b36 == 0)
								{
									addPopup(T.joinA, x(j) + w / 2, y(j) + w / 2);
								}
								b36++;
								setTypeMap(j, 56, 46);
							}
							break;
						case 182:
							FarmScr.posBarn = new AvPosition(x(j), y(j));
							setTypeMap(j, 80, 39);
							break;
						case 183:
							FarmScr.posPond = new AvPosition(x(j) + 24, y(j) + 24);
							setTypeMap(j, 88, 13);
							break;
						case 184:
							type[j] = (sbyte)(72 + b30);
							break;
						case 185:
							if (b37 == 1 && index == 18)
							{
								addObjTree(975, x(j) + 24, y(j) + 24, 71);
							}
							if (index == 18)
							{
								setTypeMap(j, 71, 43);
								if (b37 == 2)
								{
									addPopup(T.joinA, x(j), y(j) + 25);
								}
							}
							else
							{
								int i7 = j;
								sbyte num9 = b37;
								b37 = (sbyte)(num9 + 1);
								setPopup(i7, num9, 0);
								setTypeMap(j, 71, 47);
							}
							b37++;
							break;
						case 186:
							b12++;
							if (b12 == 3)
							{
								addPopup(T.joinA, x(j), y(j) + 24);
							}
							setTypeMap(j, 94, 17);
							break;
						case 187:
							if (b39 == 0 && FarmScr.idFarm == GameMidlet.avatar.IDDB)
							{
								treeLists.addElement(new SubObject(-10, x(j) + 20, y(j) + 20, FarmScr.imgBuyLant.getWidth(), FarmScr.imgBuyLant.getHeight()));
							}
							b39++;
							setTypeMap(j, (sbyte)((FarmScr.idFarm != GameMidlet.avatar.IDDB) ? 80 : 95), 4);
							break;
						case 188:
							if (FarmScr.idFarm == GameMidlet.avatar.IDDB)
							{
								treeLists.addElement(new SubObject(-10, x(j) + 20, y(j) + 20, FarmScr.imgBuyLant.getWidth(), FarmScr.imgBuyLant.getWidth()));
							}
							setTypeMap(j, (sbyte)((FarmScr.idFarm != GameMidlet.avatar.IDDB) ? 80 : 96), 4);
							break;
						case 189:
							FarmScr.starFruil.x = x(j) + 12;
							FarmScr.starFruil.y = y(j) + 12;
							type[j] = 97;
							if (GameMidlet.avatar.IDDB == FarmScr.idFarm)
							{
								FarmScr.starFruil.index = 97;
							}
							map[j] = 4;
							treeLists.addElement(FarmScr.starFruil);
							if (GameMidlet.avatar.IDDB != FarmScr.idFarm && FarmScr.isSteal && FarmScr.starFruil.numberFruit > 0)
							{
								addPopup("trom", x(j) + 12, y(j) + 24, -2);
							}
							break;
						case 190:
							type[j] = 98;
							map[j] = 4;
							if (b40 == 0)
							{
								addObjTree(1029, x(j) + 36, y(j) + 20, 98);
								FarmScr.xPosCook = x(j) + 26;
								FarmScr.yPosCook = y(j) + 10;
								if (FarmScr.idFarm == GameMidlet.avatar.IDDB)
								{
									addPopup(T.joinA, x(j) + 36, y(j) + 24);
								}
							}
							if (FarmScr.idFarm != GameMidlet.avatar.IDDB)
							{
								type[j] = 88;
							}
							b40++;
							break;
						case 191:
							type[j] = 23;
							if (index - 1 == 104)
							{
								map[j] = 0;
								if (b18 == 1)
								{
									addPopup(T.joinA, x(j) + 12, y(j) + 12);
								}
							}
							else
							{
								if (b18 % 2 == 0)
								{
									map[j] = 46;
								}
								else
								{
									map[j] = 44;
								}
								if (b18 == 1)
								{
									addPopup(T.joinA, x(j) + 24, y(j) + 12);
								}
							}
							b18++;
							break;
						case 192:
							type[j] = 99;
							map[j] = 4;
							b22++;
							break;
						case 193:
							type[j] = 100;
							map[j] = 4;
							if (b21 == 1)
							{
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b21++;
							break;
						case 194:
							type[j] = 106;
							map[j] = 4;
							break;
						case 195:
							type[j] = 102;
							map[j] = 4;
							break;
						case 196:
							type[j] = 103;
							map[j] = 4;
							if (b19 == 1)
							{
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b19++;
							break;
						case 197:
							type[j] = 104;
							map[j] = 4;
							if (b20 == 1)
							{
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b20++;
							break;
						case 198:
							type[j] = 105;
							map[j] = 4;
							addObjTree(1036, x(j) + 12, y(j) + 20, 105);
							break;
						case 199:
							type[j] = 101;
							map[j] = 4;
							if (b10 == 1)
							{
								addObjTree(1031, x(j) + 24, y(j) + 24, 101);
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b10++;
							break;
						case 200:
							type[j] = 107;
							if (b25 == 1)
							{
								addObjTree(-1, x(j) + 24, y(j) + 24, 107);
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b25++;
							map[j] = 5;
							break;
						case 201:
							type[j] = 19;
							map[j] = 5;
							if (b41 == 1)
							{
								addPopup(T.joinA, x(j) + 24, y(j) + 30);
							}
							b41++;
							break;
						case 202:
							setTypeMap(j, 88, 96);
							if (b13 % 4 == 0)
							{
								addObjTree(4, x(j) + w * 2, y(j) + w, 88);
							}
							b13++;
							break;
						case 203:
						{
							int i8 = j;
							sbyte num10 = b36;
							b36 = (sbyte)(num10 + 1);
							setPopup(i8, num10, 0);
							setTypeMap(j, 110, 96);
							break;
						}
						case 204:
							map[j] = 43;
							type[j] = 10;
							if (b41 == 1)
							{
								addPopup(T.joinA, x(j), y(j) + 30);
							}
							b41++;
							break;
						case 63:
						case 65:
							if (index - 1 != 103 && index - 1 != 100 && index - 1 != 101 && index - 1 != 104)
							{
								type[j] = 27;
								int i5 = j;
								sbyte num7 = b26;
								b26 = (sbyte)(num7 + 1);
								setPopup(i5, num7, 0);
								switch (index)
								{
								case 58:
								case 63:
									addPopup(T.joinA, x(j) - 12, y(j) + 12);
									break;
								case 59:
								case 64:
									addPopup(T.joinA, x(j) + 12, y(j) + 36);
									break;
								default:
									addPopup(T.joinA, x(j) - 12, y(j) + 12);
									break;
								}
							}
							break;
						case 97:
							type[j] = 54;
							break;
						case 98:
							type[j] = 29;
							addObjTree(846, j, 29);
							if (index - 1 == 108 || index - 1 == 109)
							{
								map[j] = 56;
							}
							break;
						case 102:
						{
							type[j] = 92;
							BoardScr.listPosAvatar.addElement(new AvPosition(x(j) + w / 2, y(j) + w));
							int num3 = 0;
							int num4 = wMap * w;
							if (wMap * w < Canvas.w)
							{
								num3 = -(Canvas.w - wMap * w) / 2;
								num4 = wMap * w - num3;
							}
							AvPosition avPosition = new AvPosition(num3, y(j) + w);
							switch (index)
							{
							case 66:
								if (b31 == 2 || b31 == 4)
								{
									avPosition.x = num4;
								}
								break;
							case 62:
								if (b31 == 1 || b31 == 3)
								{
									avPosition.x = num4;
								}
								break;
							default:
								if (b31 == 1)
								{
									avPosition.x = num4;
								}
								break;
							}
							BoardScr.listPosCasino.addElement(avPosition);
							b31++;
							break;
						}
						case 110:
							FarmScr.posName = new AvPosition(x(j) - w + 8, y(j) - 2);
							addObjTree(847, x(j) + 11, y(j), -1);
							break;
						case 69:
							if (index - 1 == 108 || index - 1 == 109 || isCasino)
							{
								type[j] = (sbyte)(72 + b30);
								b30++;
							}
							break;
						case 68:
							if (index - 1 == 108 || index - 1 == 109 || isCasino)
							{
								type[j] = (sbyte)(72 + b38);
								b38++;
							}
							break;
						default:
							b37 = 0;
							b29 = 0;
							break;
						}
					}
					if (type[j] == 80)
					{
						num++;
					}
				}
				break;
			}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		AvCamera.gI().followPlayer = GameMidlet.avatar;
		setMapItem(index);
		orderVector(treeLists);
		if (TYPEMAP == 24 && FarmScr.idFarm != GameMidlet.avatar.IDDB)
		{
			TYPEMAP = 53;
		}
		int tYPEMAP = TYPEMAP;
		if (tYPEMAP != -1 && idTileImg != -1)
		{
			tYPEMAP = typeAny;
		}
		for (int n = 0; n < type.Length; n++)
		{
			if (isType(n % wMap, n / wMap, (short)tYPEMAP))
			{
				AvPosition avPosition3 = setPosPlayer(n);
				if (avPosition3 != null)
				{
					GameMidlet.avatar.x = avPosition3.x;
					GameMidlet.avatar.y = avPosition3.y;
				}
				break;
			}
		}
		if (typeTemp != -1)
		{
			typeAny = typeTemp;
		}
		AvCamera.gI().init(index);
		initFindPath();
	}

	public AvPosition setPosPlayer(int i)
	{
		if (i + 1 < type.Length && type[i] == type[i + 1])
		{
			for (int j = i; j < type.Length; j++)
			{
				if (type[j] != type[j + 1])
				{
					int num = w;
					if (i / wMap == Hmap - 1 || i / wMap == Hmap - 2)
					{
						num = -w;
					}
					return new AvPosition(x(i) + (j - i + 1) * w / 2, y(i) + w / 2 + num);
				}
			}
		}
		else if (i + wMap < type.Length && type[i] == type[i + wMap])
		{
			for (int k = i; k < type.Length; k += wMap)
			{
				if (type[k] != type[k + wMap])
				{
					int num2 = -w;
					if (i % wMap == 0)
					{
						num2 = w;
					}
					return new AvPosition(x(i) + w / 2 + num2, y(i) + ((k - i) / wMap + 1) * w / 2);
				}
			}
		}
		return null;
	}

	public static void addObjTree(int type, int x, int y, int index)
	{
		if (idTileImg == -1)
		{
			SubObject subObject = null;
			subObject = ((type < 0) ? new SubObject(type, x, y, 0, 0) : new ImageObj(type, x, y, 0, 0));
			subObject.index = (short)index;
			treeLists.addElement(subObject);
		}
	}

	public static void addObjTree(int type, int i, int index)
	{
		if (idTileImg == -1)
		{
			ImageObj imageObj = new ImageObj(type, x(i) + getWTileImg(i, map), y(i) + w - 4, 0, 0);
			imageObj.index = (short)index;
			treeLists.addElement(imageObj);
		}
	}

	public static int x(int i)
	{
		return i % wMap * w;
	}

	public static int y(int i)
	{
		return i / wMap * w;
	}

	private void setTypeMap(int i, sbyte t, int m)
	{
		type[i] = t;
		map[i] = (short)m;
	}

	private void setTypePark(int i, sbyte t)
	{
		type[i] = t;
		if (i / wMap == 0)
		{
			map[i] = 43;
		}
		else
		{
			map[i] = 6;
		}
	}

	public static void setType(int x, int y, sbyte t)
	{
		type[y * wMap + x] = t;
	}

	public static bool isType(int x, int y, short t)
	{
		return type[y * wMap + x] == t;
	}

	public static void addPopup(string info, int x, int y)
	{
		if (Session_ME.connected)
		{
			treeLists.addElement(new PopupName(info, x, y));
		}
	}

	public static void addPopup(string info, int x, int y, int type)
	{
		if (Session_ME.connected)
		{
			PopupName popupName = new PopupName(info, x, y);
			popupName.type = type;
			treeLists.addElement(popupName);
		}
	}

	public static MapItemType getMapItemTypeByID(int idType)
	{
		int num = LoadMap.mapItemType.size();
		for (int i = 0; i < num; i++)
		{
			MapItemType mapItemType = (MapItemType)LoadMap.mapItemType.elementAt(i);
			if (mapItemType.idType == idType)
			{
				return mapItemType;
			}
		}
		return null;
	}

	public void setMapItemType()
	{
		if (LoadMap.mapItem != null && mapItemType != null)
		{
			for (int i = 0; i < LoadMap.mapItem.size(); i++)
			{
				MapItem mapItem = (MapItem)LoadMap.mapItem.elementAt(i);
				MapItemType mapItemTypeByID = getMapItemTypeByID(mapItem.typeID);
				setTypeSeat(mapItem, mapItemTypeByID);
				MapItem mapItem2 = new MapItem(mapItem.type, mapItem.x * w, mapItem.y * w, mapItem.ID, mapItem.typeID);
				mapItem2.isGetImg = mapItem.isGetImg;
				treeLists.addElement(mapItem2);
			}
			orderVector(treeLists);
		}
	}

	private void setMapItem(int typ)
	{
		for (int i = 0; i < AvatarData.listMapItem.size(); i++)
		{
			MapItem mapItem = (MapItem)AvatarData.listMapItem.elementAt(i);
			if (mapItem.type == typ)
			{
				MapItemType mapItemTypeByID = AvatarData.getMapItemTypeByID(mapItem.typeID);
				setTypeSeat(mapItem, mapItemTypeByID);
				treeLists.addElement(new MapItem(mapItem.type, mapItem.x * w, mapItem.y * w, mapItem.ID, mapItem.typeID));
			}
		}
		if (AvatarData.listAd == null)
		{
			return;
		}
		for (int j = 0; j < AvatarData.listAd.size(); j++)
		{
			ObjAd objAd = (ObjAd)AvatarData.listAd.elementAt(j);
			for (int k = 0; k < objAd.listPoint.size(); k++)
			{
				AvPosition avPosition = (AvPosition)objAd.listPoint.elementAt(k);
				if (avPosition.anchor == typ)
				{
					if (avPosition.y * wMap + avPosition.x >= 0 && avPosition.y * wMap + avPosition.x < type.Length)
					{
						type[avPosition.y * wMap + avPosition.x] = 83;
					}
					addPopup(objAd.title, avPosition.x * w + w / 2, avPosition.y * w + w / 2);
				}
			}
		}
	}

	public void getAd(int tx, int ty)
	{
		if (AvatarData.listAd == null)
		{
			return;
		}
		for (int i = 0; i < AvatarData.listAd.size(); i++)
		{
			ObjAd objAd = (ObjAd)AvatarData.listAd.elementAt(i);
			for (int j = 0; j < objAd.listPoint.size(); j++)
			{
				AvPosition avPosition = (AvPosition)objAd.listPoint.elementAt(j);
				if (avPosition.x == tx && avPosition.y == ty && TYPEMAP + 1 == avPosition.anchor)
				{
					Canvas.msgdlg.setInfoLR(objAd.text, new Command(T.OK, new IActionAd(objAd)), new Command(T.close, new IActionCloseAd()));
					return;
				}
			}
		}
	}

	public void setClound()
	{
		clound = null;
		if (imgTreeBg == null || imgBG == null)
		{
			return;
		}
		clound = new AvPosition[6];
		int num = 0;
		if (rememBg == 0)
		{
			num = -10 * AvMain.hd;
		}
		else if (rememBg == 2)
		{
			num = 5 * AvMain.hd;
		}
		else if (rememBg == 3)
		{
			num = 25 * AvMain.hd;
		}
		else if (rememBg != 1)
		{
		}
		sbyte b = 0;
		for (int i = 0; i < clound.Length; i++)
		{
			b = (sbyte)CRes.rnd(2);
			int num2 = CRes.rnd(wMap * w + imgBG.w) * 100;
			int num3 = -(imgBG.h / 2 + imgClound[b].h + num + CRes.rnd(imgBG.h / 2));
			clound[i] = new AvPosition(num2, num3);
			clound[i].anchor = b;
			if (clound[i].anchor == 1)
			{
				clound[i].index = (short)(10 + CRes.rnd(30));
			}
			else
			{
				clound[i].index = (short)(30 + CRes.rnd(30));
			}
		}
		CRes.rndaaa();
	}

	public static MyVector orderVector(MyVector obj)
	{
		try
		{
			int num = obj.size();
			for (int i = 0; i < num - 1; i++)
			{
				MyObject myObject = (MyObject)obj.elementAt(i);
				for (int j = i + 1; j < num; j++)
				{
					MyObject myObject2 = (MyObject)obj.elementAt(j);
					if (myObject.y > myObject2.y)
					{
						obj.setElementAt(myObject, j);
						obj.setElementAt(myObject2, i);
						myObject = myObject2;
					}
				}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		return obj;
	}

	public static void resetObject()
	{
		Canvas.currentEffect.removeAllElements();
		treeLists.removeAllElements();
		playerLists.removeAllElements();
		dynamicLists.removeAllElements();
		effBgList = null;
		effCameraList = null;
		effManager = null;
	}

	public static bool isTrans(int x, int y)
	{
		int typeMap = getTypeMap(x, y);
		if (typeMap == 80 || typeMap == 51)
		{
			return true;
		}
		if (GameMidlet.avatar.task == -5 && (typeMap == 79 || typeMap == 92 || typeMap == 81 || typeMap == 67))
		{
			return true;
		}
		return false;
	}

	public static int getTypeMap(int vX, int vY)
	{
		if (vX < 0 || vX > wMap * w || vY / w * wMap + vX / w < 0 || vY / w * wMap + vX / w >= type.Length)
		{
			return -2;
		}
		return type[vY / w * wMap + vX / w];
	}

	public static int getposMap(int vX, int vY)
	{
		if (vX < 0 || vX > wMap * w || vY / w * wMap + vX / w >= type.Length)
		{
			return -1;
		}
		return vY / w * wMap + vX / w;
	}

	public static Avatar getAvatar(int id)
	{
		for (int i = 0; i < playerLists.size(); i++)
		{
			MyObject myObject = (MyObject)playerLists.elementAt(i);
			if (myObject.catagory == 0 && ((Base)myObject).IDDB == id)
			{
				return (Avatar)myObject;
			}
		}
		return null;
	}

	public static void onWeather(sbyte weather2)
	{
		for (int i = 0; i < Canvas.currentEffect.size(); i++)
		{
			Effect effect = (Effect)Canvas.currentEffect.elementAt(i);
			effect.isStop = true;
		}
		if (weather2 != -1)
		{
			AnimateEffect animateEffect = new AnimateEffect(weather2, true, 0);
			animateEffect.show();
		}
		weather = weather2;
	}

	public static void setPet(Avatar ava)
	{
		if (ava.idPet != -1)
		{
			Pet pet = new Pet(ava);
			playerLists.addElement(pet);
		}
	}

	public static Pet getPet(int id)
	{
		for (int i = 0; i < playerLists.size(); i++)
		{
			MyObject myObject = (MyObject)playerLists.elementAt(i);
			if (myObject.catagory == 4 && ((Pet)myObject).follow.IDDB == id)
			{
				return (Pet)myObject;
			}
		}
		return null;
	}

	public static void addPlayer(Avatar ava)
	{
		playerLists.addElement(ava);
		ava.setPet();
	}

	public static void removePlayer(Avatar ava)
	{
		playerLists.removeElement(ava);
		Pet pet = getPet(ava.IDDB);
		if (pet != null)
		{
			playerLists.removeElement(pet);
		}
	}

	public static void removePlayer(MyObject obj)
	{
		if (focusObj == obj)
		{
			focusObj = null;
		}
		playerLists.removeElement(obj);
	}

	public void onTileImg(sbyte idTileMap, sbyte[] arr)
	{
		idTileImg = idTileMap;
		imgMap = new FrameImage(CRes.createImgByByteArray(ArrayCast.cast(arr)), w * AvMain.hd, AvMain.hd * w);
		setMapAny();
		Canvas.load = 0;
	}

	private void setMapPaint(int i, short[] m)
	{
		if (i % wMap == 0)
		{
			m[i] = m[i + 1];
		}
		else
		{
			m[i] = m[i - 1];
		}
	}

	public void setMapAny()
	{
		Bus.isRun = false;
		resetObject();
		addPlayer(GameMidlet.avatar);
		short[] array = new short[map.Length];
		type = new sbyte[map.Length];
		sbyte[] array2 = new sbyte[125];
		sbyte b = 0;
		sbyte b2 = 0;
		for (int i = 0; i < map.Length; i++)
		{
			array[i] = map[i];
		}
		isCasino = false;
		for (int j = 0; j < map.Length; j++)
		{
			if (map[j] < imgMap.nFrame)
			{
				map[j] = -4;
				continue;
			}
			if (map[j] < imgMap.nFrame * 2)
			{
				map[j] = -5;
				continue;
			}
			int num = map[j] - imgMap.nFrame * 2;
			switch (num)
			{
			case 0:
			{
				map[j] = 98;
				ImageObj imageObj = new ImageObj(846, x(j) + w / 2, y(j) + w / 2, 0, 0);
				treeLists.addElement(imageObj);
				break;
			}
			case 2:
				map[j] = 139;
				break;
			case 3:
				map[j] = 152;
				break;
			case 12:
				map[j] = 150;
				break;
			case 13:
				map[j] = 151;
				break;
			case 14:
				isCasino = true;
				setPopup(j, b, 0);
				b++;
				map[j] = 184;
				array[j] = 33;
				break;
			case 15:
				b2++;
				array[j] = 0;
				map[j] = 185;
				break;
			default:
				setPopup(j, array2[num], 0);
				type[j] = (sbyte)(-125 + num);
				map[j] = -3;
				break;
			}
			if (num > 0 && array2[num] == 0 && num - 1 < MapScr.idImg.Length && MapScr.idImg[num - 1] != -1)
			{
				ImageObj imageObj2 = new ImageObj(MapScr.idImg[num - 1], x(j) + getWTileImg(j, array), y(j) + w - 4, 0, 0);
				treeLists.addElement(imageObj2);
			}
			if (num != 14)
			{
				setMapPaint(j, array);
			}
			ref sbyte reference = ref array2[num];
			reference++;
		}
		AvCamera.disable = false;
		GameMidlet.avatar.action = 0;
		imgTreeBg = null;
		imgCreateMap = null;
		setMap(null, MapScr.roomID + 1, false);
		x0_imgBG = 26;
		TYPEMAP = MapScr.roomID;
		map = array;
		AvCamera.gI().init(MapScr.roomID + 1);
		Canvas.endDlg();
		rememBg = -1;
		rememMap = -1;
		setMapItemType();
		ParkService.gI().doJoinPark(MapScr.roomID, -1);
		Canvas.paint.setColorBar();
	}

	public static int getWTileImg(int i, short[] m)
	{
		for (int j = i; j < m.Length; j++)
		{
			if (m[j] != m[j + 1])
			{
				return (j - i + 1) * w / 2;
			}
		}
		return 0;
	}

	public void setPopup(int i, sbyte count, int type)
	{
		if (count != 0)
		{
			return;
		}
		if (i + 1 < map.Length && map[i] == map[i + 1])
		{
			for (int j = i; j < map.Length; j++)
			{
				if (map[j] != map[j + 1])
				{
					addPopup((type == 1) ? T.exit : T.joinA, x(i) + (j - i + 1) * w / 2, y(i) + ((idTileImg != -1) ? w : (w / 2)) + ((type == 2) ? (w / 2) : 0));
					break;
				}
			}
		}
		else
		{
			if (i + wMap >= map.Length || map[i] != map[i + wMap])
			{
				return;
			}
			for (int k = i; k < map.Length; k += wMap)
			{
				if (map[k] != map[k + wMap])
				{
					addPopup((type == 1) ? T.exit : T.joinA, x(i) + 3, y(i) + ((k - i) / wMap + 1) * w / 2);
					break;
				}
			}
		}
	}

	public static void setTypeSeat(MapItem pos, MapItemType map)
	{
		sbyte b = 88;
		if (map.iconID == 1)
		{
			b = 79;
		}
		else if (map.iconID == 2)
		{
			b = 67;
		}
		for (int i = 0; i < map.listNotTrans.size(); i++)
		{
			AvPosition avPosition = (AvPosition)map.listNotTrans.elementAt(i);
			type[(pos.y + avPosition.y) * wMap + (pos.x + avPosition.x)] = b;
		}
	}

	public void onDichChuyen(sbyte roomID, sbyte boardID, int xTe, int yTe)
	{
		xDichChuyen = xTe;
		yDichChuyen = yTe;
		idTileImg = -1;
		Canvas.startWaitDlg();
		if (GameMidlet.CLIENT_TYPE != 9)
		{
			GlobalService.gI().getHandler(9);
		}
		ParkService.gI().doJoinPark(roomID, boardID);
	}
}
