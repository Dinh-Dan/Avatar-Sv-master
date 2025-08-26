using System;
using UnityEngine;

public class Canvas
{
	private class IActionOk : IAction
	{
		public void perform()
		{
			endDlg();
		}
	}

	public static Canvas instance;

	public static bool bRun;

	public static bool[] keyPressed = new bool[14];

	public static bool[] keyReleased = new bool[14];

	public static bool[] keyHold = new bool[14];

	public static bool isPointerDown;

	public static bool isPointerRelease;

	public static bool isPointerClick;

	public static int px;

	public static int py;

	public static int pxLast;

	public static int pyLast;

	public static int gameTick;

	public static int w = 0;

	public static int h;

	public static int hw;

	public static int hh;

	public static int rw;

	public static int rh;

	public static int hCan;

	public static MyScreen currentMyScreen;

	public static MsgDlg msgdlg;

	public static MenuMain menuMain;

	public static InputDlg inputDlg;

	public static Dialog currentDialog;

	public static MyVector currentPopup;

	public static int count0;

	public static AvatarData avataData;

	public static LoadMap loadMap;

	public static CameraList cameraList;

	public static Face currentFace;

	public static MyVector currentEffect = new MyVector();

	private static long[] timeBB;

	public static MyVector listInfoSV = new MyVector();

	public static bool isVirHorizontal;

	public static bool isInitChar;

	public static bool isKeyBoard = false;

	public static bool isDoubleImage = true;

	public static int load = -1;

	public static FontX normalFont;

	public static FontX normalWhiteFont;

	public static FontX borderFont;

	public static FontX arialFont;

	public static FontX blackF;

	public static FontX numberFont;

	public static FontX smallFontRed;

	public static FontX smallFontYellow;

	public static FontX menuFont;

	public static FontX tempFont;

	public static FontX smallWhite;

	public static FontX fontChat;

	public static FontX fontChatB;

	public static FontX fontBlu;

	public static FontX fontWhiteBold;

	public static IPaint paint;

	public static int hTab = 0;

	public static int transTab = 0;

	public static int iOpenOngame;

	public static int xTran;

	public static int hKeyBoard;

	public static int tran18;

	public static Welcome welcome;

	public static MyVector listAc = new MyVector();

	public static string pass;

	public static string user;

	public static T t;

	public static int timeNameSV = -1;

	public static string nameSV = string.Empty;

	public static Image imagePlug;

	public static bool isPaint18 = false;

	public static int stypeInt = 1;

	public static string text = string.Empty;

	private static bool isStart = false;

	public static float disStart = 0f;

	public static float disStartZoom;

	public static float temp;

	public static float xZoom;

	public static float yZoom;

	public static bool isZoom = false;

	public static sbyte dirZoom = 1;

	public static int isRotateTop = 0;

	public static int iOpenBoard = -1;

	public static bool aTran = false;

	public static Image imgTabInfo;

	public static int countTab = 30;

	private long lastTimePress;

	public static MyVector flyTexts = new MyVector();

	private float xTouch;

	private float yTouch;

	public static string test = string.Empty;

	public static string test1 = string.Empty;

	public static string test2 = string.Empty;

	private int countDown;

	private int num;

	public static MyVector listPoint;

	public static Command cmdEndDlg;

	public static AvPosition[] posCmd = new AvPosition[3];

	public static AvPosition posByteCOunt;

	public Canvas()
	{
		w = (int)ScaleGUI.WIDTH;
		h = (int)ScaleGUI.HEIGHT;
		hCan = h;
		initFont();
		initResource();
		setSize(w, h);
		hw = w / 2;
		hh = h / 2;
		instance = this;
		CRes.init();
		currentPopup = new MyVector();
		msgdlg = new MsgDlg();
		avataData = new AvatarData();
		inputDlg = new InputDlg();
		loadMap = new LoadMap();
		cameraList = new CameraList();
		if (currentMyScreen != null && currentMyScreen == OptionScr.instance)
		{
			OptionScr.gI().initSize();
		}
		paint.initPos();
		listPoint = new MyVector();
		MiniMap.gI();
		paint.initString(0);
	}

	public static void setPopupTime(string text)
	{
		nameSV = text;
		timeNameSV = 100;
	}

	public void initFont()
	{
		t = new T();
		if (Main.hdtype == 2)
		{
			normalFont = new HDFont(0, "vo", 24, 2720192, 2720192);
			stypeInt = 2;
			borderFont = new HDFont(1);
			arialFont = new HDFont(2);
			blackF = new HDFont(3);
			numberFont = new HDFont(4, "Colossalis", 15131223, 0);
			smallFontRed = new HDFont(5, "vo", 16, 16746118, 0);
			smallFontYellow = new HDFont(6, "vo", 16, 15848992, 8344576);
			menuFont = new HDFont(9);
			tempFont = new HDFont(10, "temp");
			normalWhiteFont = new HDFont(11, "normalW", 16777215, 5921884);
			fontChat = new HDFont(15, "UTM Swiss Condensed_1", 2720192, 2720192);
			fontChatB = new HDFont(16, "UTM Swiss Condensed_1", 0, 0);
			fontBlu = new HDFont(17, "UTM Swiss Condensed_1", 22, 29068, 29068);
			fontWhiteBold = new HDFont(18, "temp");
			paint = new HDPaint();
		}
		if (Main.hdtype == 1)
		{
			stypeInt = 1;
			normalFont = new HDFont(0, "vo", 14, 2720192, 2720192);
			borderFont = new HDFont(1);
			arialFont = new HDFont(2);
			blackF = new HDFont(3);
			numberFont = new HDFont(4);
			smallFontRed = new HDFont(5);
			smallFontRed = new HDFont(5, "vo", 12, 16746118, 0);
			smallFontYellow = new HDFont(6, "vo", 12, 15848992, 8344576);
			menuFont = new HDFont(9);
			tempFont = new HDFont(10, "temp");
			normalWhiteFont = new HDFont(11, "normalW", 16777215, 5921884);
			fontChat = new HDFont(15, "UTM Swiss Condensed_1", 2720192, 2720192);
			fontChatB = new HDFont(16, "UTM Swiss Condensed_1", 0, 0);
			fontBlu = new HDFont(17, "UTM Swiss Condensed_1", 14, 29068, 29068);
			fontWhiteBold = new HDFont(18, "temp");
			paint = new MediumPaint();
			Debug.Log("MEDIUM");
		}
		MyScreen.ITEM_HEIGHT = normalFont.getHeight() + 6;
		AvMain.hBlack = (sbyte)blackF.getHeight();
		AvMain.hBorder = (sbyte)borderFont.getHeight();
		AvMain.hNormal = (sbyte)normalFont.getHeight();
		AvMain.hSmall = (sbyte)smallFontRed.getHeight();
	}

	public void sizeChanged(int w, int h)
	{
		if (!TField.isOpenTextBox)
		{
			setSize(w, h);
		}
	}

	public static void paintPlus(MyGraphics g)
	{
	}

	public static void paintPlus2(MyGraphics g)
	{
	}

	public void setSize(int wd, int hd)
	{
		Out.println("setSize: " + wd + "   " + hd);
		rw = (w = wd);
		rh = (h = hd - transTab);
		rh = hd;
		hCan = hd;
		AvMain.duPopup = 20;
		isVirHorizontal = false;
		hw = w / 2;
		hh = h / 2;
		paint.initPos();
		paint.init();
		menuMain = null;
		if (currentMyScreen != null && currentMyScreen == LoginScr.me)
		{
			LoginScr.gI().init(hd);
		}
		AvCamera.gI().init(LoadMap.TYPEMAP);
		if (PopupShop.me != null)
		{
			PopupShop.init();
		}
		if (PaintPopup.me != null)
		{
			PaintPopup.gI().init();
		}
		if (BoardScr.me != null)
		{
			BoardScr.me.init();
		}
		if (msgdlg != null)
		{
			msgdlg.init();
		}
		if (RoomListOnScr.instance != null && RoomListOnScr.instance == currentMyScreen)
		{
			RoomListOnScr.gI().init();
		}
		if (inputDlg != null)
		{
			inputDlg.init(hCan);
		}
		if (currentMyScreen != null && currentMyScreen == MoneyScr.instance)
		{
			MoneyScr.gI().initCanvas();
		}
		if (ChatTextField.instance != null)
		{
			ChatTextField.gI().init(hCan);
		}
		if (CustomTab.me != null && CustomTab.me == currentFace)
		{
			CustomTab.gI().init();
		}
		if (currentMyScreen != null)
		{
			if (currentMyScreen == RaceScr.me)
			{
				RaceScr.gI().initPos();
			}
			if (RegisterScr.instance == currentMyScreen)
			{
				RegisterScr.gI().init();
			}
			if (BoardListOnScr.me == currentMyScreen)
			{
				BoardListOnScr.gI().setCam();
			}
			if (currentMyScreen == OptionScr.instance)
			{
				OptionScr.gI().initSize();
			}
			if (currentMyScreen == ServerListScr.me)
			{
				ServerListScr.gI().init();
			}
			if (currentMyScreen == ListScr.gI())
			{
				ListScr.gI().reSize();
			}
			if (currentMyScreen == MoneyScr.gI())
			{
				MoneyScr.gI().init();
			}
			if (currentFace != null)
			{
				currentFace.init(hCan);
			}
		}
		cmdEndDlg = new Command(T.no, -1);
	}

	public void setInfoSV(string info)
	{
		Out.println("setInfoSV: " + info);
		if (!onMainMenu.isOngame && !info.Equals(string.Empty))
		{
			StringObj stringObj = new StringObj(info, -arialFont.getWidth(info));
			stringObj.x = w + 10;
			listInfoSV.addElement(stringObj);
			if (countTab == 0)
			{
				countTab = 1;
			}
			if (isPaint18)
			{
				countTab = 15 * AvMain.hd;
			}
			transTab = 0;
		}
	}

	public static void connect()
	{
		if (Session_ME.gI().isConnected())
		{
			return;
		}
		int num = ServerListScr.selected - 1;
		if (num < 0)
		{
			num = 0;
		}
		string text;
		int num2;
		if (GameMidlet.isEnglish)
		{
			text = GameMidlet.IPEng;
			num2 = GameMidlet.PORTEng;
			GameMidlet.gameID = "14";
		}
		else
		{
			text = GameMidlet.IP[OptionScr.gI().mapFocus[4]][ServerListScr.indexSV][ServerListScr.index];
			num2 = GameMidlet.PORT[OptionScr.gI().mapFocus[4]][ServerListScr.indexSV][ServerListScr.index];
			MiniMap.nameSV = GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][ServerListScr.indexSV][ServerListScr.index + 1];
			if (ServerListScr.indexSV > 0)
			{
				GameMidlet.gameID = "13";
			}
			else
			{
				GameMidlet.gameID = "12";
			}
		}
		Session_ME.gI().setHandler(GlobalMessageHandler.gI());
		Out.println("connect: " + text + ":" + num2 + "     " + ServerListScr.indexSV + "    " + ServerListScr.index);
		Session_ME.gI().connect(text, num2);
		GlobalService.gI().setProviderAndClientType();
	}

	public void start()
	{
		isStart = true;
		Session_ME.gI().close();
	}

	public void setLimitMap()
	{
		currentMyScreen.initZoom();
		AvCamera.gI().timeDelay = 0L;
		AvCamera.gI().vY = 0f;
		AvCamera.gI().vX = 0f;
		AvCamera.gI().xTo = xZoom - (float)(w / 2) / AvMain.zoom;
		AvCamera.gI().yTo = yZoom - (float)(hCan / 2) / AvMain.zoom;
		AvCamera.gI().update();
		if (AvCamera.gI().yTo > AvCamera.gI().yLimit)
		{
			AvCamera.gI().yTo = (AvCamera.gI().yCam = AvCamera.gI().yLimit);
		}
		if (w < LoadMap.wMap * AvCamera.w)
		{
			if (AvCamera.gI().xTo < 0f)
			{
				AvCamera.gI().xTo = 0f;
			}
			else if (AvCamera.gI().xTo > AvCamera.gI().xLimit)
			{
				AvCamera.gI().xTo = AvCamera.gI().xLimit;
			}
		}
		AvCamera.gI().xCam = AvCamera.gI().xTo;
		AvCamera.gI().yCam = AvCamera.gI().yTo;
	}

	public void initKeyBoard(int ih, bool isRe)
	{
		Out.println(string.Concat("initKeyBoard: ", ih, "   ", currentMyScreen, "     ", MessageScr.me));
		if (ChatTextField.isShow || isRe || Screen.orientation == ScreenOrientation.Portrait)
		{
			setSize(w, ih);
		}
		else
		{
			if (currentMyScreen == LoginScr.me)
			{
				LoginScr.gI().init(ih);
			}
			if (currentFace != null)
			{
				currentFace.init(ih);
			}
			if (currentDialog != null)
			{
				currentDialog.init(ih);
			}
		}
		if (currentMyScreen == MessageScr.me)
		{
			MessageScr.gI().init(ih);
		}
	}

	public void update()
	{
		try
		{
			if (isRotateTop == 1 && Screen.orientation == ScreenOrientation.Portrait)
			{
				isRotateTop = 0;
				TouchScreenKeyboard.hideInput = true;
				Screen.orientation = ScreenOrientation.Portrait;
				ScaleGUI.initScaleGUI();
				setSize((int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
				Out.println("isRotateTop: " + 1 + "    :" + ipKeyboard.tk);
				ChatTextField.isShow = false;
			}
			else if (isRotateTop == 2 && Screen.orientation != ScreenOrientation.Portrait)
			{
				Out.println("isRotateTop: " + 2);
				isRotateTop = 0;
				Screen.orientation = ScreenOrientation.LandscapeLeft;
				ChatTextField.isShow = false;
				ipKeyboard.tk.active = false;
				ipKeyboard.isReset = true;
				ipKeyboard.tk = null;
				ScaleGUI.initScaleGUI();
				setSize((int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
			}
			if (Session_ME.isStart && (getTick() - Session_ME.timeStart) / 1000 > 20 && Session_ME.messageHandler != null)
			{
				startOK(T.canNotConnect, new GlobalLogicHandler.IActionDisconnect());
				Session_ME.gI().close();
			}
			if (listAc.size() > 0 && (currentDialog == null || msgdlg.isWaiting))
			{
				IAction action = (IAction)listAc.elementAt(0);
				listAc.removeElement(action);
				action.perform();
			}
			if (currentMyScreen != MiniMap.me)
			{
				if (Input.touchCount == 0)
				{
					if (isZoom)
					{
						isPointerRelease = false;
						if ((double)AvMain.zoom > 2.0 || (AvMain.zoom > 1f && (double)AvMain.zoom < 1.25) || ((double)AvMain.zoom > 1.5 && AvMain.zoom < 2f))
						{
							dirZoom = -1;
						}
						else
						{
							dirZoom = 1;
						}
					}
					float num = AvMain.zoom * 1000f;
					int num2 = (int)num;
					if (num2 % 500 != 0)
					{
						float num3 = num2 % 500;
						if (dirZoom == 1)
						{
							num3 = 500f - num3;
						}
						float num4 = num3 / 1000f;
						if (num4 < 0.05f)
						{
							int num5 = num2 % 500;
							float num6 = num2 - num5;
							if (dirZoom == 1)
							{
								num6 += 500f;
							}
							float zoom = num6 / 1000f;
							AvMain.zoom = zoom;
							isZoom = false;
						}
						else
						{
							AvMain.zoom += num4 / 5f * (float)dirZoom;
						}
						setLimitMap();
					}
				}
				if (Input.touchCount == 1)
				{
					isZoom = false;
				}
				if (Input.touchCount > 1)
				{
					if (currentDialog == null && menuMain == null && currentMyScreen != ListScr.instance && currentMyScreen != PopupShop.me)
					{
						Touch touch = Input.GetTouch(0);
						Touch touch2 = Input.GetTouch(1);
						if (!isZoom)
						{
							xZoom = AvCamera.gI().xCam + (float)(w / 2) / AvMain.zoom;
							yZoom = AvCamera.gI().yCam + (float)(hCan / 2) / AvMain.zoom;
							AvCamera.gI().vY = 0f;
							AvCamera.gI().vX = 0f;
						}
						if (touch.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
						{
							float num7 = Vector2.Distance(touch.position, touch2.position);
							float num8 = num7 - disStart;
							if (isZoom && disStart != 0f)
							{
								temp = num7 - disStartZoom;
								AvMain.zoom += num8 / 800f;
								if (AvMain.zoom < 1f)
								{
									AvMain.zoom = 1f;
								}
								else if (AvMain.zoom > 2.2f)
								{
									AvMain.zoom = 2.2f;
								}
								setLimitMap();
							}
							else
							{
								disStartZoom = Vector2.Distance(touch.position, touch2.position);
							}
							disStart = Vector2.Distance(touch.position, touch2.position);
						}
						isZoom = true;
					}
				}
				else
				{
					disStart = 0f;
				}
			}
			int num9 = 0;
			if (Screen.orientation != ScreenOrientation.Portrait)
			{
				if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeLeft)
				{
					Screen.orientation = ScreenOrientation.LandscapeLeft;
				}
				if (Input.deviceOrientation == DeviceOrientation.LandscapeRight && Screen.orientation != ScreenOrientation.LandscapeRight)
				{
					Screen.orientation = ScreenOrientation.LandscapeRight;
				}
			}
			if (ipKeyboard.isReset)
			{
				if (TouchScreenKeyboard.visible)
				{
					if (TField.currentTField.autoScaleScreen)
					{
						int num10 = (int)ScaleGUI.WIDTH;
						num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height);
						if (Screen.orientation != ScreenOrientation.Portrait)
						{
							if (ScaleGUI.WIDTH >= 960f)
							{
								num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height - 162f);
								if (ScaleGUI.isAndroid && ScaleGUI.scaleAndroid == 2)
								{
									num9 -= 115;
								}
							}
							if (ScaleGUI.WIDTH == 1024f)
							{
								num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height);
							}
							num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height);
						}
						else
						{
							if (ScaleGUI.HEIGHT >= 960f)
							{
								num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height - 220f);
								if (ScaleGUI.isAndroid && ScaleGUI.scaleAndroid == 2)
								{
									num9 -= 115;
								}
							}
							if (ScaleGUI.HEIGHT == 1024f)
							{
								num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height);
							}
							num9 = (int)(ScaleGUI.HEIGHT - TouchScreenKeyboard.area.height);
						}
						hKeyBoard = num9;
						if (num9 != rh)
						{
							isKeyBoard = true;
							aTran = true;
							if (aTran)
							{
								aTran = false;
								initKeyBoard(num9, false);
							}
							else
							{
								aTran = true;
							}
							rh = num9;
						}
					}
				}
				else if ((float)rw != ScaleGUI.WIDTH || (float)rh != ScaleGUI.HEIGHT || (float)hCan != ScaleGUI.HEIGHT)
				{
					if (isKeyBoard)
					{
						isKeyBoard = false;
						initKeyBoard((int)ScaleGUI.HEIGHT, true);
					}
					if (isKeyBoard && rh == hKeyBoard)
					{
						isKeyBoard = false;
						hKeyBoard = 0;
					}
					if (ChatTextField.isShow)
					{
						ChatTextField.isShow = false;
					}
				}
			}
			gameTick++;
			if (gameTick > 10000)
			{
				gameTick = 0;
			}
			if (timeNameSV >= 0)
			{
				timeNameSV--;
			}
			if (load != -1)
			{
				updateOpenScr();
			}
			if (load == 0 && !onMainMenu.isOngame)
			{
				return;
			}
			if (load != 0 && welcome != null && currentDialog == null)
			{
				welcome.updateKey();
			}
			if (currentEffect.size() > 0)
			{
				for (int i = 0; i < currentEffect.size(); i++)
				{
					Effect effect = (Effect)currentEffect.elementAt(i);
					effect.update();
				}
			}
			if (currentMyScreen != null)
			{
				if (load != 0 && currentDialog != null)
				{
					currentDialog.updateKey();
				}
				else if (load != 0 && ChatTextField.isShow)
				{
					ChatTextField.gI().updateKey();
				}
				updateInfoSV();
				currentMyScreen.update();
				if (cameraList.isShow)
				{
					cameraList.moveCamera();
				}
				if (load != 0 && currentFace != null)
				{
					currentFace.updateKey();
				}
				if (load != 0 && currentDialog != null)
				{
					currentDialog.updateKey();
				}
				else if (menuMain != null)
				{
					menuMain.updateKey();
					if (menuMain != null)
					{
						menuMain.update();
					}
				}
				else
				{
					if (load != 0 && currentFace == null && !isZoom && !ChatTextField.isShow)
					{
						currentMyScreen.updateKey();
					}
					if (cameraList.isShow && currentFace == null)
					{
						cameraList.updateKey();
					}
				}
				if (gameTick % 20 == 10)
				{
					AvatarData.setLimitImage();
				}
			}
			isPointerClick = false;
			isPointerRelease = false;
			updateFlyTexts();
			if (SoundManager.isStop)
			{
				Main.main.GetComponent<AudioSource>().volume -= 0.02f;
				if (Main.main.GetComponent<AudioSource>().volume == 0f)
				{
					SoundManager.isStop = false;
					SoundManager.stopALLBGSound();
				}
			}
			if (SoundManager.isOpen)
			{
				Main.main.GetComponent<AudioSource>().volume += 0.02f;
				if (Main.main.GetComponent<AudioSource>().volume * 100f >= (float)OptionScr.gI().volume)
				{
					SoundManager.isOpen = false;
				}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void updateInfoSV()
	{
		if (listInfoSV.size() <= 0)
		{
			if (countTab > (isPaint18 ? ((AvMain.hd != 2) ? 20 : 30) : 0))
			{
				countTab--;
				currentMyScreen.initTabTrans();
			}
			return;
		}
		int num = menuFont.getHeight() + 2;
		if (isPaint18)
		{
			num = 35;
		}
		if (countTab < num)
		{
			countTab++;
			currentMyScreen.initTabTrans();
		}
		StringObj stringObj = (StringObj)listInfoSV.elementAt(0);
		stringObj.x -= 2;
		if (stringObj.x < stringObj.w2)
		{
			listInfoSV.removeElementAt(0);
		}
	}

	public void paintInfoSV(MyGraphics g)
	{
		resetTransNotZoom(g);
		if (isPaint18)
		{
			g.drawImageScale(imgTabInfo, 0, countTab - 40, w, 40 + ((AvMain.hd == 2 && listInfoSV.size() > 0) ? 20 : 0), 0);
		}
		else
		{
			g.drawImageScale(imgTabInfo, 0, countTab - 30, w, 30, 0);
		}
		if (listInfoSV.size() > 0)
		{
			int num = countTab / 2 - AvMain.hBlack / 2 + ((AvMain.hd == 2) ? 1 : (-2));
			StringObj stringObj = (StringObj)listInfoSV.elementAt(0);
			menuFont.drawString(g, stringObj.str, stringObj.x, num + ((!isPaint18) ? (-12) : 8) + ((AvMain.hd == 2 && listInfoSV.size() > 0) ? 14 : 0), 0);
			resetTrans(g);
		}
	}

	private void updateFlyTexts()
	{
		for (int i = 0; i < flyTexts.size(); i++)
		{
			FlyTextInfo flyTextInfo = (FlyTextInfo)flyTexts.elementAt(i);
			flyTextInfo.update();
		}
	}

	private void paintFlyTexts(MyGraphics g)
	{
		resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		for (int i = 0; i < flyTexts.size(); i++)
		{
			FlyTextInfo flyTextInfo = (FlyTextInfo)flyTexts.elementAt(i);
			flyTextInfo.paint(g);
		}
		resetTrans(g);
	}

	public static bool isKeyPressed(int index)
	{
		if (keyPressed[index])
		{
			keyPressed[index] = false;
			return true;
		}
		return false;
	}

	public void pointerDragged(int x, int y)
	{
		px = x;
		py = y;
	}

	public void pointerPressed(int x, int y)
	{
		isPointerClick = true;
		isPointerDown = true;
		pxLast = x;
		pyLast = y;
		px = x;
		py = y;
	}

	public void pointerReleased(int x, int y)
	{
		isPointerDown = false;
		isPointerRelease = true;
		px = x;
		py = y;
	}

	public static void clearKeyPressed()
	{
		isPointerRelease = false;
		for (int i = 0; i < 14; i++)
		{
			keyPressed[i] = false;
		}
	}

	public static void clearKeyHold()
	{
		isPointerRelease = false;
		isPointerDown = false;
		for (int i = 0; i < 14; i++)
		{
			keyHold[i] = false;
		}
	}

	public static void clearKeyReleased()
	{
		isPointerDown = false;
		for (int i = 0; i < 14; i++)
		{
			keyReleased[i] = false;
		}
	}

	public static void addFlyText(int text, int x, int y, int dir, int delay, int imgIDFarm, int imgIDAvatar)
	{
		flyTexts.addElement(new FlyTextInfo(x, y, text, dir, null, delay, imgIDFarm, imgIDAvatar));
	}

	public static void addFlyText(int text, int x, int y, int dir, int delay)
	{
		flyTexts.addElement(new FlyTextInfo(x, y, text, dir, null, delay, -1, -1));
	}

	public static void addFlyText(int text, int x, int y, int dir, Image img, int delay)
	{
		flyTexts.addElement(new FlyTextInfo(x, y, text, dir, img, delay, -1, -1));
	}

	public static void addFlyTextSmall(string text, int x, int y, int dir, int type, int delay)
	{
		flyTexts.addElement(new FlyTextInfo(x, y, text, dir, type, delay));
	}

	public void onPaint(MyGraphics g)
	{
		if (ScaleGUI.scaleAndroid == 2)
		{
			GUIUtility.ScaleAroundPivot(new Vector2(2f, 2f), Vector2.zero);
		}
		try
		{
			long tick = getTick();
			resetTrans(g);
			g.translate(0f, 0f);
			if (load != 0 || onMainMenu.isOngame)
			{
				if (currentMyScreen != null)
				{
					currentMyScreen.paint(g);
				}
				if (currentEffect.size() > 0)
				{
					for (int i = 0; i < currentEffect.size(); i++)
					{
						Effect effect = (Effect)currentEffect.elementAt(i);
						effect.paint(g);
					}
				}
				if (ChatTextField.isShow)
				{
					ChatTextField.gI().paint(g);
				}
				if (currentFace != null)
				{
					currentFace.paint(g);
				}
				if (currentDialog != null)
				{
					currentDialog.paint(g);
				}
				else if (menuMain != null)
				{
					menuMain.paint(g);
				}
				if (currentMyScreen != MessageScr.me)
				{
					paint.paintMSG(g);
				}
				if (welcome != null)
				{
					welcome.paint(g);
				}
				paintFlyTexts(g);
			}
			paintInfoSV(g);
			if (load != -1)
			{
				paintOpenScr(g);
			}
			if (timeNameSV > 0)
			{
				paintPopupTime(g);
			}
			resetTransNotZoom(g);
			if (isPaint18)
			{
				g.drawImage(imagePlug, 3 * AvMain.hd, (AvMain.hd != 2) ? 10 : 14, 2);
				menuFont.drawString(g, "Chơi quá 180 phút một ngày sẽ ảnh hưởng xấu đến sức khỏe.", 4 * AvMain.hd + imagePlug.getWidth(), 1, 0);
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		if (ScaleGUI.scaleAndroid == 2)
		{
			GUIUtility.ScaleAroundPivot(new Vector2(0f, 0f), Vector2.zero);
		}
	}

	public void paintPopupTime(MyGraphics g)
	{
		MiniMap.imgPopupName.drawFrame(0, hw - 85 * AvMain.hd / 2, 15 * AvMain.hd + 30 * AvMain.hd / 2, 0, 3, g);
		MiniMap.imgPopupName.drawFrame(0, hw + 85 * AvMain.hd / 2 + ((AvMain.hd == 1) ? 1 : 0), 15 * AvMain.hd + 30 * AvMain.hd / 2, 2, 3, g);
		arialFont.drawString(g, nameSV, hw, 15 * AvMain.hd + 30 * AvMain.hd / 2 - arialFont.getHeight() / 2, 2);
	}

	private void paintEffect(MyGraphics g)
	{
		if (listPoint.size() <= 0)
		{
			return;
		}
		g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
		g.setColor(16627970);
		for (int i = 0; i < listPoint.size(); i++)
		{
			AvPosition avPosition = (AvPosition)listPoint.elementAt(i);
			avPosition.anchor++;
			g.fillRect(avPosition.x - 2, avPosition.y - 2, 4f, 4f);
			if (avPosition.anchor > 16)
			{
				listPoint.removeElement(avPosition);
				i--;
			}
		}
	}

	public static void resetTrans(MyGraphics g)
	{
		g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
		int num = (int)((float)xTran / AvMain.zoom);
		g.translate(num, transTab);
		g.setClip(0f, 0f, w, hCan + hTab);
	}

	public static void resetTransNotZoom(MyGraphics g)
	{
		g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
		g.translate(xTran, transTab);
		g.setClip(0f, 0f, w, hCan + hTab);
	}

	public void updateOpenScr()
	{
		if (load == 1)
		{
			countDown += 15;
		}
		else
		{
			num++;
			if (num >= MsgDlg.imgLoad.nFrame * ((!onMainMenu.isOngame) ? 1 : 2))
			{
				num = 0;
			}
		}
		if (countDown >= hh)
		{
			countDown = 0;
			load = -1;
		}
	}

	public void paintOpenScr(MyGraphics g)
	{
		resetTransNotZoom(g);
		if (!onMainMenu.isOngame)
		{
			g.setClip(0f, 0f, (int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT + 50);
			g.setColor(0);
			g.fillRect(0f, 0f, w, hh - countDown);
			g.fillRect(0f, hh + countDown, w, hh - countDown);
		}
		if (load != 1)
		{
			MsgDlg.imgLoad.drawFrame(num / 2, (int)ScaleGUI.WIDTH / 2, (int)ScaleGUI.HEIGHT / 2, 0, 3, g);
		}
	}

	public static void endDlg()
	{
		msgdlg.setIsWaiting(false);
		currentDialog = null;
	}

	public static void startOKDlg(string info)
	{
		msgdlg.setInfoC(info, new Command(T.OK, new IActionOk()));
	}

	public static void startOKDlg(string info, IAction yes)
	{
		msgdlg.setInfoLR(info, new Command(T.yes, yes), cmdEndDlg);
	}

	public static void startOK(string info, IAction ok)
	{
		msgdlg.setInfoC(info, new Command(T.OK, ok));
	}

	public static void startOKDlg(string info, int index)
	{
		msgdlg.setInfoLR(info, new Command(T.yes, index), cmdEndDlg);
	}

	public static void startOKDlg(string info, int index, AvMain pointer)
	{
		msgdlg.setInfoLR(info, new Command(T.yes, index, pointer), cmdEndDlg);
	}

	public static void startOK(string info, int index)
	{
		msgdlg.setInfoC(info, new Command(T.OK, index));
	}

	public static void startWaitDlg(string info)
	{
		msgdlg.setInfoC(info, null);
		msgdlg.setIsWaiting(true);
	}

	public static void startWaitCancelDlg(string info)
	{
		msgdlg.setInfoC(info, new Command(T.cancel, -1));
	}

	public static void startWaitDlg()
	{
		startWaitDlg(T.pleaseWait);
	}

	public static string getPriceMoney(int xu, int gold, bool iss)
	{
		string text = string.Empty;
		if (xu > 0)
		{
			text = text + getMoneys(xu) + T.xu;
		}
		if (gold > 0)
		{
			if (xu > 0)
			{
				text += " - ";
			}
			text = text + getMoneys(gold) + T.gold;
		}
		return text;
	}

	public static string getMoneys(int m)
	{
		string text = string.Empty;
		int num = m / 1000 + 1;
		for (int i = 0; i < num; i++)
		{
			if (m >= 1000)
			{
				int num2 = m % 1000;
				text = ((num2 != 0) ? ((num2 >= 10) ? ((num2 >= 100) ? ("." + num2 + text) : (".0" + num2 + text)) : (".00" + num2 + text)) : (".000" + text));
				m /= 1000;
				continue;
			}
			text = m + text;
			break;
		}
		return text;
	}

	public static bool isPointer(int x, int y, int w, int h)
	{
		if (!isPointerDown && !isPointerRelease)
		{
			return false;
		}
		return isPoint(x, y, w, h);
	}

	public static bool isPoint(int x, int y, int w, int h)
	{
		if (px >= x && px <= x + w && py >= y && py <= y + h)
		{
			return true;
		}
		return false;
	}

	public static void getTypeMoney(int xu, int luong, IAction iXu, IAction iLuong, IAction iaEnd)
	{
		string text = string.Empty;
		MyVector myVector = new MyVector();
		if (xu > 0)
		{
			myVector.addElement(new Command((luong > 0) ? T.xu : T.yes, iXu));
			text = " " + xu + T.xu;
		}
		if (luong > 0)
		{
			myVector.addElement(new Command((xu > 0) ? T.gold : T.yes, iLuong));
			text = " " + luong + T.gold;
		}
		text = ((myVector.size() != 1) ? (T.selectMoney + " \n" + xu + T.xu + " - " + luong + " " + T.gold) : (T.doYouWanBuyPrice + text + " " + T.no + " ?"));
		if (iaEnd == null)
		{
			myVector.addElement(cmdEndDlg);
		}
		else
		{
			myVector.addElement(new Command(T.no, iaEnd));
		}
		switch (myVector.size())
		{
		case 1:
			msgdlg.setInfoC(text, (Command)myVector.elementAt(0));
			break;
		case 2:
			msgdlg.setInfoLR(text, (Command)myVector.elementAt(0), (Command)myVector.elementAt(1));
			break;
		case 3:
			msgdlg.setInfoLCR(text, (Command)myVector.elementAt(0), (Command)myVector.elementAt(1), (Command)myVector.elementAt(2));
			break;
		}
	}

	public void onKeyPressed(int keyCode)
	{
		lastTimePress = DateTime.Now.Ticks;
		mapKeyPress(keyCode);
	}

	public static void mapKeyPress(int keyCode)
	{
		if (currentFace != null)
		{
			currentFace.keyPress(keyCode);
		}
		else if (currentDialog != null)
		{
			currentDialog.keyPress(keyCode);
		}
		else if (menuMain == null)
		{
			if (ChatTextField.isShow)
			{
				ChatTextField.gI().keyPressed(keyCode);
			}
			else
			{
				currentMyScreen.keyPress(keyCode);
			}
		}
		switch (keyCode)
		{
		case 42:
			keyHold[10] = true;
			keyPressed[10] = true;
			break;
		case 35:
			keyHold[11] = true;
			keyPressed[11] = true;
			break;
		case -21:
		case -6:
			keyHold[12] = true;
			keyPressed[12] = true;
			break;
		case -22:
		case -7:
			keyHold[13] = true;
			keyPressed[13] = true;
			break;
		case -5:
		case 10:
			keyHold[5] = true;
			keyPressed[5] = true;
			break;
		case -38:
		case -1:
			keyHold[2] = true;
			keyPressed[2] = true;
			break;
		case -39:
		case -2:
			keyHold[8] = true;
			keyPressed[8] = true;
			break;
		case -3:
			keyHold[4] = true;
			keyPressed[4] = true;
			break;
		case -4:
			keyHold[6] = true;
			keyPressed[6] = true;
			break;
		}
	}

	public void onKeyReleased(int keyCode)
	{
		mapKeyRelease(keyCode);
	}

	public void mapKeyRelease(int keyCode)
	{
		switch (keyCode)
		{
		case 42:
			keyHold[10] = false;
			keyReleased[10] = true;
			break;
		case 35:
			keyHold[11] = false;
			keyReleased[11] = true;
			break;
		case -21:
		case -6:
			keyHold[12] = false;
			keyReleased[12] = true;
			break;
		case -22:
		case -7:
			keyHold[13] = false;
			keyReleased[13] = true;
			break;
		case -5:
		case 10:
			keyHold[5] = false;
			keyReleased[5] = true;
			break;
		case -38:
		case -1:
			keyHold[2] = false;
			keyReleased[2] = true;
			break;
		case -39:
		case -2:
			keyHold[8] = false;
			keyReleased[8] = true;
			break;
		case -3:
			keyHold[4] = false;
			keyReleased[4] = true;
			break;
		case -4:
			keyHold[6] = false;
			keyReleased[6] = true;
			break;
		}
	}

	public static void initResource()
	{
		Bus.imgBus = Image.createImagePNG(T.getPath() + "/home/839");
		ChatPopup.wPop = (short)(13 * AvMain.hd);
		ChatPopup.imgPopup[0] = new FrameImage(Image.createImagePNG(T.getPath() + "/main/c"), ChatPopup.wPop, ChatPopup.wPop);
		ChatPopup.imgPopup[1] = new FrameImage(Image.createImagePNG(T.getPath() + "/main/cB"), ChatPopup.wPop, ChatPopup.wPop);
		ChatPopup.imgPopup[2] = new FrameImage(Image.createImagePNG(T.getPath() + "/main/c0"), ChatPopup.wPop, ChatPopup.wPop);
		ChatPopup.imgArrow[0] = Image.createImagePNG(T.getPath() + "/main/aryou0");
		ChatPopup.imgArrow[1] = Image.createImagePNG(T.getPath() + "/main/aryou1");
		MiniMap.imgSmallIcon = Image.createImage(T.getPath() + "/effect/sIc");
		DialLuckyScr.imgCau = Image.createImage(T.getPath() + "/dialLucky/c");
		DialLuckyScr.imgDo = Image.createImage(T.getPath() + "/dialLucky/sq");
		DialLuckyScr.imgDauHoi = Image.createImage(T.getPath() + "/dialLucky/q");
		DialLuckyScr.imgFireWork = new FrameImage(Image.createImage(T.getPath() + "/dialLucky/st"), 11 * AvMain.hd, 11 * AvMain.hd);
		DialLuckyScr.imgDot = Image.createImage(T.getPath() + "/dialLucky/dot");
		DialLuckyScr.imgCau_back = Image.createImage(T.getPath() + "/dialLucky/cb");
		FishingScr.imgPhao = Image.createImagePNG(T.getPath() + "/effect/cucphao");
		FishingScr.imgCa = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/ca"), 14 * AvMain.hd, 14 * AvMain.hd);
		LoadMap.imgDen = Image.createImagePNG(T.getPath() + "/effect/den");
		LoadMap.imgShadow = Image.createImage(T.getPath() + "/effect/s0");
	}

	public static bool setShowMsg()
	{
		if (currentMyScreen != LoginScr.me && currentMyScreen != MoneyScr.instance && currentMyScreen != PopupShop.gI() && currentMyScreen != ListScr.instance && currentMyScreen != RoomListOnScr.instance && currentMyScreen != OptionScr.instance && currentFace == null && currentMyScreen != ParkListSrc.instance && currentMyScreen != MyInfoScr.me && currentDialog == null && !onMainMenu.isOngame && currentMyScreen != SplashScr.me && !HouseScr.isDuyChuyen && !HouseScr.isTranItemBuy && !HouseScr.isBuyTileMap && menuMain != Menu.gI())
		{
			return true;
		}
		return false;
	}

	public static bool setShowIconMenu()
	{
		if (menuMain == Menu.gI())
		{
			return false;
		}
		if (menuMain != Menu.gI() && !HouseScr.isDuyChuyen && !HouseScr.isTranItemBuy && !HouseScr.isBuyTileMap && currentMyScreen != SplashScr.me && currentFace == null && !LoginScr.gI().isReg && currentDialog == null && currentMyScreen != ListScr.instance && (currentMyScreen == MapScr.instance || currentMyScreen == FarmScr.instance || (currentMyScreen == HouseScr.me && !HouseScr.isSelectObj && !HouseScr.isChange) || currentMyScreen == MiniMap.me || currentMyScreen == LoginScr.me || currentMyScreen == ServerListScr.me || currentMyScreen == RegisterScr.instance || currentMyScreen == RaceScr.me || !onMainMenu.isOngame))
		{
			return true;
		}
		return false;
	}

	public static bool isPaintIconVir()
	{
		if (currentDialog == null && currentMyScreen != ServerListScr.me && currentMyScreen != LoginScr.me && currentMyScreen != PopupShop.gI() && currentMyScreen != ListScr.instance && currentMyScreen != MainMenu.me && currentDialog == null && currentMyScreen != onMainMenu.me && currentMyScreen != MoneyScr.instance && currentMyScreen != MiniMap.me && currentMyScreen != OptionScr.instance && currentMyScreen != FarmScr.instance && currentFace == null && !HouseScr.isSelectObj && !HouseScr.isChange && GameMidlet.CLIENT_TYPE != 8 && currentMyScreen != ParkListSrc.instance && currentMyScreen != MyInfoScr.me && !onMainMenu.isOngame && currentMyScreen != SplashScr.me && !HouseScr.isDuyChuyen && !HouseScr.isTranItemBuy && !HouseScr.isBuyTileMap && menuMain != Menu.gI())
		{
			return true;
		}
		return false;
	}

	public static int dx()
	{
		return pxLast - px;
	}

	public static int dy()
	{
		return pyLast - py;
	}

	public static int getSecond()
	{
		return Environment.TickCount / 1000;
	}

	public static long getTick()
	{
		return Environment.TickCount;
	}

	public static string trim(string str)
	{
		string text = str.Substring(0, 1);
		if (text.Equals("\n") || text.Equals(" "))
		{
			str = str.Substring(1);
		}
		string text2 = str.Substring(str.Length - 1);
		if (text2.Equals(" ") || text.Equals("\n"))
		{
			str = str.Substring(0, str.Length - 1);
		}
		return str;
	}
}
