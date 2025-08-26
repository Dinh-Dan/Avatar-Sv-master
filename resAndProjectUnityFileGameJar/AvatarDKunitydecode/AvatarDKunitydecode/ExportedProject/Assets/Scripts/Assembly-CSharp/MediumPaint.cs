using System;
using UnityEngine;

public class MediumPaint : IPaint
{
	private class CommandPointerGo : Command
	{
		public CommandPointerGo(string name, IActionPointerGO a)
			: base(name, a)
		{
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			MainMenu.imgGO.drawFrame(1, x, y, 0, 3, g);
		}
	}

	private class IActionPointerGO : IAction
	{
		public void perform()
		{
			int num = LoadMap.posFocus.x / AvMain.hd;
			int num2 = LoadMap.posFocus.y / AvMain.hd;
			if (!Canvas.loadMap.doJoin(num, num2) && !Canvas.loadMap.doJoin(num + 24, num2) && !Canvas.loadMap.doJoin(num - 24, num2) && !Canvas.loadMap.doJoin(num, num2 + 24) && !Canvas.loadMap.doJoin(num, num2 - 24))
			{
			}
		}
	}

	private class CommandPointer : Command
	{
		private Base b;

		public CommandPointer(string name, IActionPointer a, Base b)
			: base(name, a)
		{
			this.b = b;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			b.paintIcon(g, x, y + b.height / 2, false);
		}
	}

	private class IActionPointer : IAction
	{
		private Base b;

		public IActionPointer(Base b)
		{
			this.b = b;
		}

		public void perform()
		{
			LoadMap.focusObj = b;
			if (b.catagory == 0)
			{
				MapScr.focusP = (Avatar)b;
			}
			if (LoadMap.focusObj != null)
			{
				MainMenu.gI().avaPaint = new AvPosition((int)((float)(LoadMap.focusObj.x * AvMain.hd) - AvCamera.gI().xCam), (int)((float)(LoadMap.focusObj.y * AvMain.hd) - AvCamera.gI().yCam));
			}
			if (LoadMap.TYPEMAP == 24)
			{
				FarmScr.gI().commandTab(2);
			}
			else if (MapScr.focusP.IDDB > 2000000000)
			{
				Canvas.startWaitDlg();
				GlobalService.gI().doCommunicate(MapScr.focusP.IDDB);
			}
			else
			{
				MainMenu.gI().doExchange();
			}
		}
	}

	public static FrameImage imgPopupBack;

	public static FrameImage imgEffectBack;

	public static FrameImage imgNotFocusTab;

	public static FrameImage imgFocusTab;

	public static Image imgCardBg;

	public static Image imgCardBg1;

	public static Image imgCardBg2;

	public static Image imgMenuTab;

	public static Image imgBar;

	public static Image imgBarMoney;

	public static Image imgCloseSmall;

	public static Image[] imgCardIcon;

	public static Image[] imgPopup;

	public static Image[] imgPopup2;

	public static Image[] imgPopupBackNum;

	public static FrameImage[] imgCardNumber;

	public static FrameImage imgCheck;

	public static FrameImage imgButtonOn;

	public static FrameImage imgEraser;

	public static Image[] iconMenu;

	public static Image[] iconAction;

	public static Image[] iconFeel;

	public static Image[] imgMSG;

	public static Image[] iconMenu_2;

	public static Image[] imgButton;

	public static Image[] iconRota;

	private static Image imgNotFocusTab_1;

	private static Image imgFocusTab_1;

	private static Image imgNewMsg;

	public static sbyte[][] cardIconInfo;

	public static int colorSelect;

	public static int colorBold;

	public static int colorNormal;

	public static int colorLight;

	public static int colorInfoPopup;

	private static Image imgTrans;

	private int wwCard = 36;

	private int hhCard = 49;

	private int aa = -1;

	private MyVector listAnimalSound = new MyVector();

	private Player soundClick;

	public int ind0;

	public int ind1;

	public int ind2;

	public int ind3;

	private bool isTranFish;

	public static string bank;

	public static string casino;

	public static string shop;

	public static string park;

	public static string caro;

	public static string caloc;

	public static string camap;

	public static string cauca;

	public static string prison;

	public static string slum;

	public static string farmroad;

	public static string farm;

	public static string farmFriend;

	public static string entertaiment;

	public static string salon;

	public static string store;

	public static string food;

	public static string petFood;

	public static string eatPig;

	public static string eatDog;

	public static string getMilk;

	public static string getEgg;

	public static string topFarm;

	public static string fishing;

	public static string houseRoad;

	public static string gotoHouse;

	public static string quayVe;

	private int indexAction;

	private int indexFeel;

	private int indexMenu;

	private int indexMSG;

	private int indexChat;

	private int indexRota;

	private bool isTranIcon;

	private FrameImage imgRegGender;

	private sbyte indexLeft;

	private sbyte indexCenter;

	private sbyte indexRight;

	static MediumPaint()
	{
		cardIconInfo = new sbyte[13][];
		colorSelect = 35217;
		colorBold = 16709;
		colorNormal = 23135;
		colorLight = 10276804;
		colorInfoPopup = 10461344;
		cardIconInfo[0] = new sbyte[9] { 4, 6, 17, 0, 27, 14, 0, 27, 36 };
		cardIconInfo[1] = new sbyte[9] { 4, 6, 17, 0, 17, 13, 0, 37, 13 };
		cardIconInfo[2] = new sbyte[12]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			27, 36
		};
		cardIconInfo[3] = new sbyte[15]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			17, 36, 0, 37, 36
		};
		cardIconInfo[4] = new sbyte[18]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			17, 36, 0, 37, 36, 0, 27, 30
		};
		cardIconInfo[5] = new sbyte[15]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			17, 28, 0, 37, 28
		};
		cardIconInfo[6] = new sbyte[18]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			17, 28, 0, 37, 28, 0, 27, 36
		};
		cardIconInfo[7] = new sbyte[18]
		{
			4, 6, 17, 0, 17, 13, 0, 37, 13, 0,
			17, 28, 0, 37, 28, 0, 27, 20
		};
		cardIconInfo[8] = new sbyte[6] { 4, 6, 17, 8, 27, 36 };
		cardIconInfo[9] = new sbyte[6] { 4, 6, 17, 9, 27, 36 };
		cardIconInfo[10] = new sbyte[6] { 4, 6, 17, 10, 27, 36 };
		cardIconInfo[11] = new sbyte[6] { 4, 6, 17, 0, 27, 36 };
		cardIconInfo[12] = new sbyte[6] { 4, 6, 17, 0, 27, 14 };
		TField.xDu = 8;
		TField.yDu = 8;
		PaintPopup.color = new int[6] { 3521446, 2378578, 8052436, 2716523, 13621272, 7042560 };
		Canvas.imagePlug = Image.createImagePNG(T.getPath() + "/12Plus");
		Avatar.imgHit = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/5"), 50, 48);
		Avatar.imgKiss = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/2"), 11, 10);
		Canvas.imgTabInfo = Image.createImage(T.getPath() + "/effect/transtab");
		MapScr.imgBar = Image.createImagePNG(T.getPath() + "/effect/bar");
		Pet.imgShadow[0] = Image.createImage(T.getPath() + "/effect/s1");
		Pet.imgShadow[1] = Image.createImage(T.getPath() + "/effect/s2");
		Menu.imgSellect = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/cmd"), 24, 24);
		MainMenu.imgGO = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/go"), 24, 24);
		MapScr.imgFocusP = Image.createImagePNG(T.getPath() + "/effect/arF");
		Avatar.imgBlog = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/dauhoathi"), 9, 9);
		imgEraser = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/eraser"), 13, 13);
		imgCheck = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/check"), 22, 22);
		TField.tfframe = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/tb"), 25, 28);
		MyScreen.imgChat = new Image[2];
		imgMSG = new Image[2];
		ListScr.imgCloseTab = new Image[2];
		ListScr.imgCloseTabFull = new Image[2];
		for (int i = 0; i < 2; i++)
		{
			MyScreen.imgChat[i] = Image.createImagePNG(T.getPath() + "/iconMenu/chat" + i);
			imgMSG[i] = Image.createImagePNG(T.getPath() + "/iconMenu/msg" + i);
			ListScr.imgCloseTabFull[i] = Image.createImagePNG(T.getPath() + "/iconMenu/close" + i);
			ListScr.imgCloseTab[i] = Image.createImagePNG(T.getPath() + "/iconMenu/closenot" + i);
		}
		imgTrans = Image.createImage(T.getPath() + "/effect/trans");
		MyScreen.imgChat = new Image[2];
		for (int j = 0; j < 2; j++)
		{
			MyScreen.imgChat[j] = Image.createImagePNG(T.getPath() + "/iconMenu/chat" + j);
		}
		PaintPopup.imgMuiIOS = new Image[2][];
		for (int k = 0; k < 2; k++)
		{
			PaintPopup.imgMuiIOS[k] = new Image[4];
			for (int l = 0; l < 4; l++)
			{
				PaintPopup.imgMuiIOS[k][l] = Image.createImagePNG(T.getPath() + "/ios/a" + k + string.Empty + l);
			}
		}
		MsgDlg.hCell = PaintPopup.imgMuiIOS[0][2].getHeight() + 4;
		AvMain.hCmd = 29;
		imgPopupBack = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/popupBack"), 20, 20);
		imgPopupBackNum = new Image[4];
		for (int m = 0; m < 4; m++)
		{
			imgPopupBackNum[m] = Image.createImagePNG(T.getPath() + "/effect/popupBack" + m);
		}
		imgEffectBack = new FrameImage(Image.createImagePNG(T.getPath() + "/effect/effectPopupBack"), 100, 50);
		imgFocusTab = new FrameImage(Image.createImagePNG(T.getPath() + "/iconMenu/tabnotfocus"), 12, 42);
		imgNotFocusTab = new FrameImage(Image.createImagePNG(T.getPath() + "/iconMenu/tabfocus"), 12, 35);
		imgFocusTab_1 = Image.createImagePNG(T.getPath() + "/iconMenu/tabnotfocus1");
		imgNotFocusTab_1 = Image.createImagePNG(T.getPath() + "/iconMenu/tabfocus1");
		imgNewMsg = Image.createImagePNG(T.getPath() + "/iconMenu/imgMessIn");
	}

	public void loadImgAvatar()
	{
		iconMenu = new Image[2];
		iconFeel = new Image[2];
		iconAction = new Image[2];
		iconMenu_2 = new Image[2];
		iconRota = new Image[2];
		for (int i = 0; i < 2; i++)
		{
			iconMenu[i] = Image.createImagePNG(T.getPath() + "/iconMenu/menuAction" + i);
			iconAction[i] = Image.createImagePNG(T.getPath() + "/iconMenu/action" + i);
			iconFeel[i] = Image.createImagePNG(T.getPath() + "/iconMenu/feel" + i);
			iconMenu_2[i] = Image.createImagePNG(T.getPath() + "/iconMenu/menu" + i);
			iconRota[i] = Image.createImagePNG(T.getPath() + "/iconMenu/rota" + i);
		}
		MsgDlg.imgLoad = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/busy"), 16, 16);
		Menu.imgBackIcon = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/backcmd"), 30, 30);
		imgMenuTab = Image.createImagePNG(T.getPath() + "/temp/menuTab");
		imgButton = new Image[2];
		for (int j = 0; j < 2; j++)
		{
			imgButton[j] = Image.createImagePNG(T.getPath() + "/effect/button" + j);
		}
		PaintPopup.wButtonSmall = imgButton[1].getWidth();
		PaintPopup.hButtonSmall = imgButton[1].getHeight();
	}

	public void clearImgAvatar()
	{
		iconMenu = null;
		Menu.imgBackIcon = null;
		imgMenuTab = null;
		imgButton = null;
		iconAction = (iconFeel = null);
	}

	public void paintTextBox(MyGraphics g, int x, int y, int width, int height, TField tf, bool isFocus, sbyte indexEraser)
	{
		Canvas.resetTrans(g);
		TField.tfframe.drawFrame(0, x, y, 0, g);
		TField.tfframe.drawFrame(1, x + width - TField.tfframe.frameWidth, y, 0, g);
		g.setColor(16777215);
		g.fillRect(x + TField.tfframe.frameWidth, y + 1, width - TField.tfframe.frameWidth * 2, height - 2);
		g.setColor(2720192);
		g.fillRect(x + TField.tfframe.frameWidth, y, width - TField.tfframe.frameWidth * 2, 1f);
		g.fillRect(x + TField.tfframe.frameWidth, y + height - 1, width - TField.tfframe.frameWidth * 2, 1f);
		if (tf.isFocused() && !tf.paintedText.Equals(string.Empty))
		{
			imgEraser.drawFrame(indexEraser, x + width - 10, y + height / 2, 0, 3, g);
		}
		g.setClip(x + 2, y + 1, width - 4, height - 2);
		g.setColor(0);
		if (tf.paintedText.Equals(string.Empty))
		{
			Canvas.fontChatB.drawString(g, tf.sDefaust, TField.TEXT_GAP_X + tf.offsetX + x + 3, y + (height - Canvas.fontChatB.getHeight()) / 2 - 2, 0);
		}
		else
		{
			Canvas.fontChatB.drawString(g, tf.paintedText, TField.TEXT_GAP_X + tf.offsetX + x, y + (height - Canvas.fontChatB.getHeight()) / 2 - ((tf.inputType != ipKeyboard.PASS) ? 4 : 0), 0);
		}
		if (tf.isFocused() && tf.keyInActiveState == 0 && (tf.showCaretCounter > 0 || tf.counter / 5 % 2 == 0))
		{
			g.setColor(0);
			g.fillRect(TField.TEXT_GAP_X + tf.offsetX + x + Canvas.fontChatB.getWidth(tf.paintedText.Substring(0, tf.caretPos) + "a") - Canvas.fontChatB.getWidth("a") - 1 + 1, y + 6, 1f, height - 12);
		}
	}

	public void paintBoxTab(MyGraphics g, int x, int y, int h, int w, int focusTab, int wSub, int wTab, int hTab, int numTab, int maxTab, int[] count, int[] colorTab, string name, sbyte countCloseAll, sbyte countText, bool isMenu, bool isFull, string[] subName, float cmx, Image[][] imgIcon)
	{
		Canvas.resetTrans(g);
		int xTab = PaintPopup.xTab;
		int wTabDu = PaintPopup.wTabDu;
		xTab = 12;
		wTabDu = wTab + 10;
		g.setColor(0);
		for (int i = 0; i < numTab; i++)
		{
			if (i != focusTab)
			{
				imgNotFocusTab.drawFrame(0, xTab + x + i * wTabDu + wTabDu / 2 - wTab / 2, y + hTab - imgNotFocusTab.frameHeight, 0, g);
				imgNotFocusTab.drawFrame(1, xTab + x + i * wTabDu + wTabDu / 2 + wTab / 2 - imgNotFocusTab.frameWidth, y + hTab - imgNotFocusTab.frameHeight, 0, g);
				g.drawImageScale(imgNotFocusTab_1, xTab + x + i * wTabDu + wTabDu / 2 - wTab / 2 + imgNotFocusTab.frameWidth, y + hTab - imgNotFocusTab.frameHeight, wTab - imgNotFocusTab.frameWidth * 2, imgNotFocusTab.frameHeight, 0);
				if (imgIcon != null)
				{
					g.drawImage(imgIcon[i][1], xTab + x + i * wTabDu + wTabDu / 2, y + hTab - imgNotFocusTab.frameHeight / 2 + 4, 3);
				}
				else
				{
					Canvas.fontWhiteBold.drawString(g, subName[i], xTab + x + i * wTabDu + wTabDu / 2, y + hTab - imgNotFocusTab.frameHeight / 2 - Canvas.fontWhiteBold.getHeight() / 2, 2);
				}
			}
		}
		paintPopupBack(g, x, y + hTab, w, h - hTab, -1, isFull);
		imgFocusTab.drawFrame(0, xTab + x + focusTab * wTabDu + wTabDu / 2 - wTab / 2, y + hTab - imgFocusTab.frameHeight + 2, 0, g);
		imgFocusTab.drawFrame(1, xTab + x + focusTab * wTabDu + wTabDu / 2 + wTab / 2 - imgFocusTab.frameWidth, y + hTab - imgFocusTab.frameHeight + 2, 0, g);
		g.drawImageScale(imgFocusTab_1, xTab + x + focusTab * wTabDu + wTabDu / 2 - wTab / 2 + imgFocusTab.frameWidth, y + hTab - imgFocusTab.frameHeight + 2, wTab - imgFocusTab.frameWidth * 2, imgFocusTab_1.getHeight(), 0);
		if (numTab > 1)
		{
			if (imgIcon != null)
			{
				g.drawImage(imgIcon[focusTab][0], xTab + x + focusTab * wTabDu + wTabDu / 2, y + hTab - imgFocusTab.frameHeight / 2 + 2, 3);
			}
			else
			{
				Canvas.fontWhiteBold.drawString(g, subName[focusTab], xTab + x + focusTab * wTabDu + wTabDu / 2, y + hTab - imgFocusTab.frameHeight / 2 + 2 - Canvas.fontWhiteBold.getHeight() / 2, 2);
			}
		}
		else if (imgIcon != null)
		{
			g.drawImage(imgIcon[focusTab][0], xTab + x + focusTab * wTabDu + wTabDu / 2, y + hTab - imgNotFocusTab.frameHeight / 2, 3);
		}
		else
		{
			g.setClip(xTab + x + focusTab * wTabDu + wTabDu / 2 - wTab / 2 + 2, y + hTab - imgFocusTab.frameHeight + 6, wTab - 4, hTab);
			Canvas.fontWhiteBold.drawString(g, name, xTab + x + focusTab * wTabDu + wTabDu / 2 + countText, y + hTab - imgNotFocusTab.frameHeight / 2 - Canvas.fontWhiteBold.getHeight() / 2 - 4, 2);
			Canvas.resetTrans(g);
		}
		Canvas.resetTrans(g);
		g.setClip(x, 0f, w + ListScr.imgCloseTab[0].w, h);
		if (countCloseAll != -1)
		{
			if (!isFull)
			{
				g.drawImage(ListScr.imgCloseTab[countCloseAll], x + w, y + hTab - 3, 3);
			}
			else
			{
				g.drawImage(ListScr.imgCloseTabFull[countCloseAll], x + w - 5 - (isFull ? 25 : 0), y + hTab + 5 - (isFull ? 23 : 0), 3);
			}
		}
	}

	public void paintBGCMD(MyGraphics g, int x, int y, int w, int h)
	{
	}

	public void paintButton(MyGraphics g, int x, int y, int index, string text)
	{
		g.drawImage(imgButton[index], x - PaintPopup.wButtonSmall / 2, y, 0);
		Canvas.normalFont.drawString(g, text, x, y + AvMain.hCmd / 2 - AvMain.hNormal / 2 + 1, 2);
	}

	public void paintCmd(MyGraphics g, Command left, Command center, Command right)
	{
		int wTab = MyScreen.wTab;
		if (left != null && left.caption != null)
		{
			if (left.x != -1)
			{
				paintButton(g, left.x, left.y, AvMain.indexLeft / 3, left.caption);
			}
			else
			{
				paintButton(g, Canvas.posCmd[0].x + wTab / 2, Canvas.hCan - PaintPopup.hButtonSmall - 3, AvMain.indexLeft / 3, left.caption);
			}
		}
		if (center != null && center.caption != null)
		{
			if (center.x != -1)
			{
				paintButton(g, center.x, center.y, AvMain.indexCenter / 3, center.caption);
			}
			else
			{
				paintButton(g, Canvas.posCmd[1].x + wTab / 2, Canvas.hCan - PaintPopup.hButtonSmall - 3, AvMain.indexCenter / 3, center.caption);
			}
		}
		if (right != null && right.caption != null)
		{
			if (right.x != -1)
			{
				paintButton(g, right.x, right.y, AvMain.indexRight / 3, right.caption);
			}
			else
			{
				paintButton(g, Canvas.posCmd[2].x + wTab / 2, Canvas.hCan - PaintPopup.hButtonSmall - 3, AvMain.indexRight / 3, right.caption);
			}
		}
	}

	public void paintHalf(MyGraphics g, Card c)
	{
		if (c.cardID == -1)
		{
			g.drawImage(imgCardBg, c.x - wwCard, c.y - hhCard, 0);
		}
		else
		{
			g.drawImage(imgCardIcon[c.cardMapping[c.cardID / 4] * 4 + c.cardID % 4], c.x - wwCard, c.y - hhCard, 0);
		}
	}

	public void paintHalfBackFull(MyGraphics g, Card c)
	{
		if (c.cardID == -1)
		{
			g.drawImage(imgCardBg, c.x - wwCard, c.y - hhCard, 0);
		}
		else
		{
			g.drawImage(imgCardIcon[c.cardMapping[c.cardID / 4] * 4 + c.cardID % 4], c.x - wwCard, c.y - hhCard, 0);
		}
	}

	public void paintFull(MyGraphics g, Card c)
	{
		if (c.cardID == -1)
		{
			g.drawImage(imgCardBg, c.x - wwCard, c.y - hhCard, 0);
		}
		else
		{
			g.drawImage(imgCardIcon[c.cardMapping[c.cardID / 4] * 4 + c.cardID % 4], c.x - wwCard, c.y - hhCard, 0);
		}
	}

	public void paintSmall(MyGraphics g, Card c, bool isCh)
	{
		if (c.cardID == -1)
		{
			g.drawImage(imgCardBg, c.x - wwCard, c.y - hhCard, 0);
		}
		else
		{
			g.drawImage(imgCardIcon[c.cardMapping[c.cardID / 4] * 4 + c.cardID % 4], c.x - wwCard, c.y - hhCard, 0);
		}
	}

	public void init()
	{
		AvMain.hDuBox = 5;
	}

	public void initPos()
	{
		MyScreen.hText = Canvas.h / 12;
		if (MyScreen.hText < 50)
		{
			MyScreen.hText = 50;
		}
		if (MyScreen.hText > 70)
		{
			MyScreen.hText = 70;
		}
		AvMain.hFillTab = 0;
		MyScreen.hTab = 40;
		Canvas.hTab = MyScreen.hText;
		if (Canvas.instance != null)
		{
			AvMain.hFillTab = Canvas.hTab / 6;
			Canvas.h -= AvMain.hFillTab * 5;
		}
		MyScreen.wTab = 86;
		int h = Canvas.h;
		Canvas.posCmd[0] = new AvPosition(2, Canvas.hCan - 36, 2);
		Canvas.posCmd[1] = new AvPosition(Canvas.hw - MyScreen.wTab / 2, Canvas.hCan - 36, 2);
		Canvas.posCmd[2] = new AvPosition(Canvas.w - MyScreen.wTab - 2, Canvas.hCan - 36, 2);
		Canvas.posByteCOunt = new AvPosition(Canvas.w - 2, 1, 1);
		if (Canvas.instance != null)
		{
			MyScreen.hTab = 0;
		}
	}

	public int collisionCmdBar(Command left, Command center, Command right)
	{
		if (imgButton == null)
		{
			return -1;
		}
		if (left != null)
		{
			if (left.x != -1)
			{
				if (Canvas.isPoint(left.x - MyScreen.wTab / 2, left.y, MyScreen.wTab, AvMain.hCmd))
				{
					return 0;
				}
			}
			else if (Canvas.isPoint(Canvas.posCmd[0].x, Canvas.hCan - PaintPopup.hButtonSmall - 3, MyScreen.wTab, AvMain.hCmd))
			{
				return 0;
			}
		}
		if (center != null)
		{
			if (center.x != -1)
			{
				if (Canvas.isPoint(center.x - MyScreen.wTab / 2, center.y, MyScreen.wTab, AvMain.hCmd))
				{
					return 1;
				}
			}
			else if (Canvas.isPoint(Canvas.posCmd[1].x, Canvas.hCan - PaintPopup.hButtonSmall - 3, MyScreen.wTab, AvMain.hCmd))
			{
				return 1;
			}
		}
		if (right != null)
		{
			if (right.x != -1)
			{
				if (Canvas.isPoint(right.x - MyScreen.wTab / 2, right.y, MyScreen.wTab, AvMain.hCmd))
				{
					return 2;
				}
			}
			else if (Canvas.isPoint(Canvas.posCmd[2].x, Canvas.hCan - PaintPopup.hButtonSmall - 3, MyScreen.wTab, AvMain.hCmd))
			{
				return 2;
			}
		}
		return -1;
	}

	public void getSound(string path, int loop)
	{
	}

	public void setSoundAnimalFarm()
	{
		if (listAnimalSound.size() <= 0)
		{
			return;
		}
		if (aa == -1)
		{
			aa = 50 + CRes.rnd(200);
			return;
		}
		aa--;
		if (aa == -1)
		{
			getSound((string)listAnimalSound.elementAt(CRes.rnd(listAnimalSound.size())), 1);
		}
	}

	public void setAnimalSound(MyVector animalLists)
	{
	}

	public void clickSound()
	{
	}

	public void paintCheckBox(MyGraphics g, int x, int y, int focus, bool isCheck)
	{
		int idx = 0;
		imgCheck.drawFrame(idx, x, y + Canvas.tempFont.getHeight() / 2, 0, g);
		if (isCheck)
		{
			imgCheck.drawFrame(1, x, y + Canvas.tempFont.getHeight() / 2, 0, g);
		}
		Canvas.tempFont.drawString(g, T.rememPass, x + 25, y + imgCheck.frameHeight / 2 - 2, 0);
	}

	public void paintSelected(MyGraphics g, int x, int y, int w, int h)
	{
		g.setColor(7138780);
		g.fillRect(x - 3, y, w + 6, h);
	}

	public void paintArrow(MyGraphics g, int index, int x, int y, int w, int indLeft, int indRight)
	{
		g.drawImage(PaintPopup.imgMuiIOS[indLeft][2], x, y, 3);
		g.drawImage(PaintPopup.imgMuiIOS[indRight][3], x + w + 5, y, 3);
	}

	public int getWNormalFont(string str)
	{
		return Canvas.arialFont.getWidth(str);
	}

	public void paintSelected_2(MyGraphics g, int x, int y, int w, int h)
	{
		g.setColor(5299141);
		g.fillRect(x, y, w, h);
	}

	public void paintTransBack(MyGraphics g)
	{
		g.drawImageScale(imgTrans, 0, 0, Canvas.w, Canvas.hCan, 0);
	}

	public void initPosLogin(LoginScr lg, int h0)
	{
		lg.wLogin = 255;
		int num = 15;
		if (lg.isReg)
		{
			num = 5;
			lg.hLogin = lg.tfUser.height * 4 + num * 5 + PaintPopup.hButtonSmall / 2;
		}
		else
		{
			lg.hLogin = 155;
		}
		lg.hCellNew = (lg.hLogin - 20) / 3;
		lg.yNew = 10;
		lg.yLogin = h0 / 2 - lg.hLogin / 2;
		lg.xLogin = Canvas.hw - lg.wLogin / 2;
		int num2 = lg.yLogin + num + 4;
		lg.tfUser.width = (lg.tfPass.width = (lg.tfReg.width = (lg.tfEmail.width = 115)));
		lg.tfUser.y = num2;
		lg.tfUser.x = (lg.tfEmail.x = (lg.tfPass.x = (lg.tfReg.x = lg.xLogin + lg.wLogin - 115 - 30)));
		num2 += lg.tfUser.height + num;
		lg.tfPass.y = num2;
		num2 += lg.tfUser.height + num;
		lg.tfReg.y = num2;
		lg.yCheck = num2 - 10;
		lg.xCheck = lg.xLogin + 65;
		num2 += lg.tfUser.height + num;
		lg.tfEmail.y = num2;
	}

	public void paintKeyArrow(MyGraphics g, int x, int y)
	{
		Canvas.resetTrans(g);
		g.drawImage(PaintPopup.imgMuiIOS[ind1 / 5][2], x - 40, y, 3);
		g.drawImage(PaintPopup.imgMuiIOS[ind0 / 5][3], x + 40, y, 3);
		g.drawImage(PaintPopup.imgMuiIOS[ind3 / 5][0], x, y - 40, 3);
		g.drawImage(PaintPopup.imgMuiIOS[ind2 / 5][1], x, y + 40, 3);
	}

	public int updateKeyArr(int x, int y)
	{
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(x + 40 - 20, y - 20, 40, 40))
			{
				isTranFish = true;
				Canvas.isPointerClick = false;
				ind0 = 1;
			}
			else if (Canvas.isPoint(x - 40 - 20, y - 20, 40, 40))
			{
				isTranFish = true;
				Canvas.isPointerClick = false;
				ind1 = 1;
			}
			else if (Canvas.isPoint(x - 20, y + 40 - 20, 40, 40))
			{
				isTranFish = true;
				Canvas.isPointerClick = false;
				ind2 = 1;
			}
			else if (Canvas.isPoint(x - 20, y - 40 - 20, 40, 40))
			{
				isTranFish = true;
				Canvas.isPointerClick = false;
				ind3 = 1;
			}
		}
		if (isTranFish)
		{
			if (Canvas.isPointerDown)
			{
				if (!Canvas.isPoint(x + 40 - 20, y - 20, 40, 40))
				{
					ind0 = 0;
				}
				if (!Canvas.isPoint(x - 40 - 20, y - 20, 40, 40))
				{
					ind1 = 0;
				}
				if (!Canvas.isPoint(x - 20, y + 40 - 20, 40, 40))
				{
					ind2 = 0;
				}
				if (!Canvas.isPoint(x - 20, y - 40 - 20, 40, 40))
				{
					ind3 = 0;
				}
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranFish = false;
				if (ind0 == 1)
				{
					ind0 = 0;
					return 3;
				}
				if (ind1 == 1)
				{
					ind1 = 0;
					return 1;
				}
				if (ind2 == 1)
				{
					ind2 = 0;
					return 4;
				}
				if (ind3 == 1)
				{
					ind3 = 0;
					return 2;
				}
			}
		}
		return -1;
	}

	public void setVirtualKeyFish(int index)
	{
	}

	public void initPosPhom()
	{
		int h = Canvas.h;
		PBoardScr.posName = new AvPosition[4]
		{
			new AvPosition(Canvas.hw + 5, 5, 0),
			new AvPosition(5, h / 2, 0),
			new AvPosition(Canvas.hw + 5, h - 50, 0),
			new AvPosition(Canvas.w - 5, h / 2, 1)
		};
		PBoardScr.posFinish = new AvPosition[4]
		{
			new AvPosition(Canvas.hw, 2, 3),
			new AvPosition(10, h / 2, MyGraphics.TOP | MyGraphics.LEFT),
			new AvPosition(Canvas.hw - 10, h - 75 - MyScreen.hTab, 3),
			new AvPosition(Canvas.w - 60, h / 2, 3)
		};
		int num = Canvas.h - ((AvMain.hFillTab == 0) ? 24 : AvMain.hFillTab);
		int num2 = h - AvMain.hFillTab;
		PBoardScr.posCardShow = new AvPosition[4]
		{
			new AvPosition(Canvas.hw, BoardScr.hcard / 2, 0),
			new AvPosition(BoardScr.wCard / 2, num / 2, 0),
			new AvPosition(Canvas.hw, num2 - BoardScr.hcard, 0),
			new AvPosition(Canvas.w - BoardScr.wCard / 2, num / 2, 0)
		};
		PBoardScr.posCardEat = new AvPosition[4]
		{
			new AvPosition(Canvas.hw, 0, 0),
			new AvPosition(BoardScr.wCard / 4 * 3, num / 2, 0),
			new AvPosition(Canvas.hw, num2 - BoardScr.hcard / 2, 0),
			new AvPosition(Canvas.w - BoardScr.wCard / 4, num / 2, 0)
		};
		PBoardScr.posNamePlaying = new AvPosition[4]
		{
			new AvPosition(Canvas.hw, BoardScr.hcard + 2, 2),
			new AvPosition(BoardScr.wCard / 4 * 3 + BoardScr.wCard / 2 + 5, num / 2 - 10, 0),
			new AvPosition(Canvas.hw, num2 - BoardScr.hcard - BoardScr.hcard / 2 - Canvas.smallFontYellow.getHeight() - 1, 2),
			new AvPosition(Canvas.w - BoardScr.wCard - 5, num / 2 - 10, 1)
		};
	}

	public int initShop()
	{
		return 60;
	}

	public void initString(int type)
	{
		Out.println("initString: " + type);
		if (type == 0)
		{
			bank = "Ngân hàng";
			casino = "Hội đánh cờ";
			shop = "Cửa hàng";
			park = "Công viên";
			caro = "khu câu cá rô";
			caloc = "khu câu cá lóc";
			camap = "khu câu cá mập";
			cauca = "khu câu cá";
			prison = "Nhà giam";
			slum = "Khu ngoại ô";
			farmroad = "Đường vào nông trại";
			farm = "Nông trại";
			farmFriend = "Nông trại bạn bè";
			entertaiment = "Khu giải trí";
			salon = "Thẩm mỹ viện";
			store = "Nhà kho";
			food = "Thức ăn";
			petFood = "Thức ăn thú nuôi";
			eatPig = "Cho heo ăn";
			eatDog = "Cho bò ăn";
			getMilk = "Lấy sữa";
			getEgg = "Lấy trứng";
			topFarm = "TOP nông trại";
			fishing = "Câu cá";
			houseRoad = "Đường vào nhà";
			gotoHouse = "Vào nhà";
			quayVe = "Quày vé";
		}
		else
		{
			bank = "Bank";
			casino = "Casino";
			shop = "Shop";
			park = "Park";
			caro = " Area anabas";
			caloc = "Area Snakehead  fish";
			camap = " Area shark";
			cauca = " Area fishing";
			slum = "Area suburban";
			prison = "Prison";
			farmroad = "Farm road";
			farm = "Farm";
			farmFriend = "Farm Friend";
			entertaiment = "Entertaiment";
			salon = "Thẩm mỹ viện";
			store = "Warehouse";
			food = "Food";
			petFood = "Pet Food";
			eatPig = "Feed Pig";
			eatDog = "Feed Dog";
			getMilk = "Get Milk";
			getEgg = "Get Egg";
			topFarm = "TOP Farm";
			fishing = "Fishing";
			houseRoad = "House Road";
			gotoHouse = "Go to House";
			quayVe = "ticket agent";
		}
	}

	public string doJoinGo(int x, int y)
	{
		int typeMap = LoadMap.getTypeMap(x, y);
		switch (typeMap)
		{
		default:
			switch (typeMap)
			{
			case -1:
				return T.QuytA;
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
				return park;
			case 14:
				return caro;
			case 15:
				return caloc;
			case 16:
				return camap;
			case 13:
				return cauca;
			case 18:
				return prison;
			case 17:
				return slum;
			case 12:
				return park;
			case 25:
				return farmroad;
			case 24:
				return farm;
			case 9:
				return entertaiment;
			case 27:
				break;
			case 28:
				return store;
			case 29:
				return T.joinA;
			case 21:
				return houseRoad;
			default:
				if (typeMap >= -125 && typeMap < 0)
				{
					return "Vào";
				}
				return null;
			}
			goto case 56;
		case 55:
			return bank;
		case 108:
		case 109:
			return casino;
		case 57:
		case 62:
			return shop;
		case 52:
			return shop;
		case 53:
			return farmFriend;
		case 58:
		case 63:
			return salon;
		case 59:
		case 64:
			return shop;
		case 56:
			return T.joinA;
		case 72:
			return T.nameCasino[0];
		case 73:
			return T.nameCasino[1];
		case 74:
			return T.nameCasino[2];
		case 75:
			return T.nameCasino[3];
		case 76:
			return T.nameCasino[4];
		case 77:
			return T.nameCasino[5];
		case 93:
			return food;
		case 78:
			return petFood;
		case 83:
			return T.joinA;
		case 84:
			return eatPig;
		case 85:
			return eatDog;
		case 86:
			return getMilk;
		case 87:
			return getEgg;
		case 89:
			return topFarm;
		case 54:
			return fishing;
		case 68:
		case 69:
		case 70:
			return gotoHouse;
		case 71:
			return quayVe;
		}
	}

	public bool selectedPointer(int xF, int yF)
	{
		if (xF > 0 && yF > 0 && CRes.distance(xF / AvMain.hd, yF / AvMain.hd, GameMidlet.avatar.x, GameMidlet.avatar.y) <= ((LoadMap.TYPEMAP != 24) ? 35 : 300))
		{
			bool[] array = new bool[3] { true, false, false };
			bool flag = false;
			MyVector myVector = new MyVector();
			MyVector myVector2 = new MyVector();
			for (int i = 0; i < LoadMap.playerLists.size(); i++)
			{
				Base @base = (Base)LoadMap.playerLists.elementAt(i);
				if (@base.IDDB != GameMidlet.avatar.IDDB && @base.catagory != 4 && ((@base.IDDB == GameMidlet.avatar.IDDB && LoadMap.TYPEMAP != 24) || @base.IDDB != GameMidlet.avatar.IDDB) && CRes.abs(@base.x * AvMain.hd - xF) <= 20 && @base.y * AvMain.hd - yF < 30 && @base.y * AvMain.hd - yF > 0)
				{
					LoadMap.focusObj = @base;
					if (@base.catagory == 0)
					{
						MapScr.focusP = (Avatar)@base;
					}
					array[1] = true;
					flag = true;
					string text = @base.name;
					if (@base.catagory == 2)
					{
						text = string.Empty;
					}
					if (text.Length > 8)
					{
						text = text.Substring(0, 8) + "..";
					}
					if (myVector2.size() >= 6)
					{
						break;
					}
					@base.ableShow = true;
					AvPosition o = new AvPosition((int)((float)(@base.x * AvMain.hd) - AvCamera.gI().xCam - (float)Canvas.transTab), (int)((float)(@base.y * AvMain.hd) - AvCamera.gI().yCam - (float)Canvas.transTab), @base.IDDB);
					myVector.addElement(o);
					myVector2.addElement(new CommandPointer(text, new IActionPointer(@base), @base));
				}
			}
			int num = LoadMap.posFocus.x / AvMain.hd;
			int num2 = LoadMap.posFocus.y / AvMain.hd;
			string text2 = doJoinGo(num, num2);
			if (text2 == null)
			{
				text2 = doJoinGo(num - 24, num2);
			}
			if (text2 == null)
			{
				text2 = doJoinGo(num - 24, num2);
			}
			if (text2 == null)
			{
				doJoinGo(num, num2 - 24);
			}
			if (text2 == null)
			{
				text2 = doJoinGo(num, num2 + 24);
			}
			if (text2 != null)
			{
				array[2] = true;
				flag = true;
				myVector2.addElement(new CommandPointerGo(text2, new IActionPointerGO()));
				if (myVector2.size() == 1)
				{
					return false;
				}
			}
			else if (myVector2.size() == 1 && myVector.size() > 0)
			{
				((Command)myVector2.elementAt(0)).action.perform();
				AvPosition avPosition = (AvPosition)myVector.elementAt(0);
				Avatar avatar = LoadMap.getAvatar(avPosition.anchor);
				if (avatar != null)
				{
					avatar.ableShow = false;
				}
				return true;
			}
			if (myVector2.size() == 0)
			{
				GameMidlet.avatar.task = -5;
				LoadMap.isGo = false;
				Canvas.loadMap.change();
				return true;
			}
			if (flag)
			{
				MainMenu.popFocus = new PopupName(string.Empty, (int)((float)LoadMap.posFocus.x - AvCamera.gI().xCam), (int)((float)LoadMap.posFocus.y - AvCamera.gI().yCam));
				MainMenu.popFocus.iPrivate = 1;
				LoadMap.treeLists = LoadMap.orderVector(LoadMap.treeLists);
				LoadMap.isGo = true;
				LoadMap.nPath = 0;
				LoadMap.dirFocus = -1;
				MainMenu.gI().showCircle(myVector2, myVector);
				return true;
			}
			LoadMap.isGo = false;
		}
		return false;
	}

	public void paintNormalFont(MyGraphics g, string str, int x, int y, int anthor, bool isSelect)
	{
		if (!isSelect)
		{
			Canvas.arialFont.drawString(g, str, x, y, anthor);
		}
		else
		{
			Canvas.normalFont.drawString(g, str, x, y, anthor);
		}
	}

	public void paintMSG(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.setColor(0);
		int num = 0;
		if (Canvas.setShowIconMenu())
		{
			if (Canvas.currentMyScreen == MapScr.instance)
			{
				num = 70;
				g.drawImage(iconMenu[indexMenu], 33f, 18 + Canvas.countTab, 3);
			}
			else
			{
				num = 50;
				g.drawImage(iconMenu_2[indexMenu], 23f, 18 + Canvas.countTab, 3);
			}
		}
		if (Canvas.setShowMsg())
		{
			g.drawImage(imgMSG[indexMSG], num + imgMSG[0].w / 2, 18 + Canvas.countTab, 3);
			if (MessageScr.gI().isNewMsg)
			{
				g.drawImage(imgNewMsg, num + imgMSG[0].w / 2 + imgMSG[indexMSG].w / 2 - 3, 18 + Canvas.countTab + imgMSG[indexMSG].h / 2 - 3, 3);
			}
		}
		if (!Canvas.isPaintIconVir())
		{
			return;
		}
		g.drawImage(MyScreen.imgChat[indexChat], num + imgMSG[0].w + 23, 18 + Canvas.countTab, 3);
		if (!ScaleGUI.isAndroid && Canvas.currentMyScreen != RaceScr.me && !Bus.isRun)
		{
			g.drawImage(iconRota[indexRota], Canvas.w - 20 - 50, 20 + Canvas.countTab, 3);
		}
		if (!onMainMenu.isOngame && Canvas.currentMyScreen != RaceScr.me && (Canvas.menuMain == null || Canvas.menuMain != MenuIcon.me))
		{
			if (GameMidlet.avatar.action != 14)
			{
				g.drawImage(iconAction[indexAction], Canvas.w - 20, 20 + Canvas.countTab, 3);
			}
			g.drawImage(iconFeel[indexFeel], Canvas.w - 20, 60 + Canvas.countTab, 3);
		}
	}

	public void setBack()
	{
		int num = 0;
		if (Canvas.setShowIconMenu())
		{
			num = ((Canvas.currentMyScreen != MapScr.instance) ? 50 : 70);
		}
		if (Canvas.isPointerClick)
		{
			isTranIcon = true;
			Canvas.isPointerClick = false;
			if ((Canvas.currentMyScreen == MapScr.instance && Canvas.isPoint(0, 0, 65, 35 + Canvas.countTab)) || (Canvas.currentMyScreen != MapScr.instance && Canvas.isPoint(0, 0, 45, 35 + Canvas.countTab)))
			{
				indexMenu = 1;
			}
			else if (Canvas.setShowMsg() && Canvas.isPoint(num - 5, 0, 40, 35 + Canvas.countTab))
			{
				indexMSG = 1;
			}
			else if (Canvas.isPaintIconVir() && Canvas.isPoint(num + imgMSG[0].w + 3, 0, 39, 35 + Canvas.countTab))
			{
				indexChat = 1;
			}
			else if (Canvas.isPaintIconVir() && GameMidlet.avatar.action != 14 && !onMainMenu.isOngame && Canvas.currentMyScreen != RaceScr.me && (Canvas.menuMain == null || Canvas.menuMain != MenuIcon.me) && Canvas.isPoint(Canvas.w - 40, Canvas.countTab, 40, 40))
			{
				indexAction = 1;
			}
			else if (Canvas.isPaintIconVir() && !onMainMenu.isOngame && Canvas.currentMyScreen != RaceScr.me && (Canvas.menuMain == null || Canvas.menuMain != MenuIcon.me) && Canvas.isPoint(Canvas.w - 40, 40 + Canvas.countTab, 40, 40))
			{
				indexFeel = 1;
			}
			else if (Canvas.currentMyScreen != RaceScr.me && !Bus.isRun && Canvas.isPaintIconVir() && Canvas.isPoint(Canvas.w - 20 - 75, 20 + Canvas.countTab - 25, 50, 50))
			{
				indexRota = 1;
			}
			else
			{
				isTranIcon = false;
				Canvas.isPointerClick = true;
			}
		}
		if (!isTranIcon)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			if (indexMenu == 1 && Canvas.currentMyScreen != MapScr.instance && !Canvas.isPoint(0, 0, 45, 35 + Canvas.countTab))
			{
				indexMenu = 0;
			}
			if (indexMenu == 1 && Canvas.currentMyScreen == MapScr.instance && !Canvas.isPoint(0, 0, 65, 35 + Canvas.countTab))
			{
				indexMenu = 0;
			}
			else if (indexMSG == 1 && !Canvas.isPoint(num - 5, 0, 40, 35 + Canvas.countTab))
			{
				indexMSG = 0;
			}
			else if (indexChat == 1 && !Canvas.isPoint(num + imgMSG[0].w + 3, 0, 39, 35 + Canvas.countTab))
			{
				indexChat = 0;
			}
			else if (indexAction == 1 && !Canvas.isPoint(Canvas.w - 40, Canvas.countTab, 40, 40))
			{
				indexAction = 0;
			}
			else if (indexFeel == 1 && !Canvas.isPoint(Canvas.w - 40, 40 + Canvas.countTab, 40, 40))
			{
				indexFeel = 0;
			}
			else if (indexRota == 1 && !Canvas.isPoint(Canvas.w - 20 - 75, 20 + Canvas.countTab - 25, 50, 50))
			{
				indexRota = 0;
			}
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		Canvas.isPointerRelease = false;
		isTranIcon = false;
		if (indexMenu == 1)
		{
			indexMenu = 0;
			Canvas.currentMyScreen.doMenu();
		}
		else if (indexMSG == 1)
		{
			indexMSG = 0;
			MessageScr.gI().switchToMe();
		}
		else if (indexChat == 1)
		{
			indexChat = 0;
			ChatTextField.gI().showTF();
		}
		else if (indexAction == 1)
		{
			indexAction = 0;
			MapScr.gI().doSellectAction(Canvas.w - 20, 20 + Canvas.countTab);
		}
		else if (indexFeel == 1)
		{
			indexFeel = 0;
			MapScr.gI().doFeel(Canvas.w - 20, 60 + Canvas.countTab);
		}
		else
		{
			if (indexRota != 1)
			{
				return;
			}
			indexRota = 0;
			if (Canvas.isRotateTop == 0)
			{
				Out.println("rotation: " + Screen.orientation);
				if (Screen.orientation == ScreenOrientation.Portrait)
				{
					Screen.orientation = ScreenOrientation.LandscapeLeft;
					Canvas.isRotateTop = 2;
				}
				else
				{
					ChatTextField.gI().showTF();
					Screen.orientation = ScreenOrientation.Portrait;
					Canvas.isRotateTop = 1;
				}
				Out.println(string.Concat("111: ", Screen.orientation, "    ", Canvas.isRotateTop));
			}
		}
	}

	public void setDrawPointer(Command left, Command center, Command right)
	{
		if (Canvas.isPointerDown)
		{
			switch (Canvas.paint.collisionCmdBar(left, center, right))
			{
			case 0:
				AvMain.indexCenter = (AvMain.indexRight = 0);
				break;
			case 1:
				AvMain.indexLeft = (AvMain.indexRight = 0);
				break;
			case 2:
				AvMain.indexCenter = (AvMain.indexLeft = 0);
				break;
			}
		}
		if (!Canvas.isPointerClick)
		{
			return;
		}
		switch (Canvas.paint.collisionCmdBar(left, center, right))
		{
		case 0:
			if (left != null)
			{
				Canvas.isPointerClick = false;
				AvMain.indexLeft = 4;
			}
			break;
		case 1:
			if (center != null)
			{
				Canvas.isPointerClick = false;
				AvMain.indexCenter = 4;
			}
			break;
		case 2:
			if (right != null)
			{
				Canvas.isPointerClick = false;
				AvMain.indexRight = 4;
			}
			break;
		}
	}

	public void paintList(MyGraphics g, int w, int maxW, int maxH, bool isHide, int selected, int[] listBoard)
	{
	}

	public void setLanguage()
	{
		if (OptionScr.gI().mapFocus[4] == 1)
		{
			new TE();
			Canvas.paint.initString(1);
		}
		else
		{
			new T();
			Canvas.paint.initString(0);
		}
	}

	public void paintDefaultBg(MyGraphics g)
	{
		g.drawImageScale(OnSplashScr.imgBg, 0, 0, Canvas.w, Canvas.hCan, 0);
	}

	public void paintLogo(MyGraphics g, int x, int y)
	{
		g.drawImage(OnSplashScr.imgLogomainMenu, x, y, 3);
	}

	public void paintDefaultScrList(MyGraphics g, string title, string subTitle, string check)
	{
		g.setClip(0f, 0f, Canvas.w, Canvas.h);
		Canvas.paint.paintDefaultBg(g);
		Canvas.borderFont.drawString(g, title, Canvas.w / 2, 2, 2);
		g.setColor(6192786);
		g.fillRect(0f, 25f, Canvas.w, MyScreen.ITEM_HEIGHT);
		Canvas.arialFont.drawString(g, subTitle, 10, 28, 0);
		Canvas.arialFont.drawString(g, check, Canvas.w - 10, 28, 1);
	}

	public void initImgBoard(int type)
	{
		if (BoardListOnScr.imgBoard == null)
		{
			string empty = string.Empty;
			switch (type)
			{
			case 0:
				empty = "imgBan2";
				break;
			case 1:
				empty = "imgBan4";
				break;
			default:
				empty = "imgBan5";
				break;
			}
			BoardListOnScr.imgBoard = new FrameImage(Image.createImagePNG("medium/hd/on/" + empty), 81, 64);
			BoardListOnScr.gI().imgNumPlayer = Image.createImagePNG("medium/hd/on/imgNumPlayer");
			BoardListOnScr.gI().imgPlaying = Image.createImagePNG("medium/hd/on/imgPlay");
			BoardListOnScr.imgLock = Image.createImagePNG("medium/hd/on/imgLock");
			BoardListOnScr.imgSelectBoard = Image.createImagePNG("medium/hd/on/imgSelectban");
			BoardScr.imgBoard = Image.createImagePNG("medium/hd/on/imgTable");
			BoardScr.xBoard = Canvas.w / 2;
			BoardScr.yBoard = (Canvas.hCan - PaintPopup.hButtonSmall) / 2 + 15;
			BoardScr.wBoard = Canvas.w - 100;
			if (BoardScr.wBoard > 600)
			{
				BoardScr.wBoard = 600;
			}
			BoardScr.hBoard = Canvas.hCan - PaintPopup.hButtonSmall - 50;
			if (BoardScr.hBoard > 400)
			{
				BoardScr.hBoard = 400;
			}
			BoardScr.imgReady = new Image[2];
			BoardScr.imgReady[0] = Image.createImagePNG("medium/hd/on/ready");
			BoardScr.imgReady[1] = Image.createImagePNG("medium/hd/on/owner");
			BoardScr.imgBan = Image.createImagePNG("medium/hd/on/star");
		}
	}

	public void setColorBar()
	{
		int num = LoadMap.map[(LoadMap.Hmap - 1) * LoadMap.wMap + 1];
		if (num != -1)
		{
			int num2 = num / LoadMap.imgMap.nFrame;
			Image image = CRes.createImgByImg(num2 * 24, num % LoadMap.imgMap.nFrame * 24, 24, 24, LoadMap.imgMap.imgFrame);
			MyScreen.color = image.texture.GetPixel(0, 0);
			MyScreen.colorBar = -1;
		}
	}

	public void initOngame()
	{
		try
		{
			MenuOn.imgTab = Image.createImagePNG("medium/hd/on/imgTabMenu");
			MsgDlg.imgLoadOn = Image.createImagePNG("medium/hd/on/loadingbg");
			MsgDlg.imgLoad = new FrameImage(Image.createImagePNG("medium/hd/on/busy"), 27, 18);
			RoomListOnScr.imgRoomStat = new FrameImage(Image.createImagePNG("medium/hd/on/imgStatus"), 27, 18);
			BoardListOnScr.gI().imgTitleBoard = Image.createImagePNG("medium/hd/on/imgkhungsoban");
			OnScreen.imgButtomSmall = new FrameImage(Image.createImagePNG("medium/hd/on/buttonSmall"), 30, 30);
			OnScreen.imgIconButton = new FrameImage(Image.createImagePNG("medium/hd/on/iconButton"), 30, 30);
			imgButtonOn = new FrameImage(Image.createImagePNG("medium/hd/on/imgButton"), 84, 28);
			RoomListOnScr.gI().imgTitleRoom = Image.createImagePNG("medium/hd/on/imgRoomtitle");
			BCBoardScr.pointer = Image.createImagePNG("hd/hd/on/p");
			PaintPopup.wButtonSmall = imgButtonOn.frameWidth;
			imgBarMoney = Image.createImagePNG("medium/hd/on/barMoney");
			imgBar = Image.createImagePNG("medium/hd/on/imgBar");
			PaintPopup.hButtonSmall = 40;
			imgPopup = new Image[7];
			for (int i = 0; i < 7; i++)
			{
				imgPopup[i] = Image.createImage("medium/hd/on/imgPopupn" + i);
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		onMainMenu.initImg();
	}

	public void resetOngame()
	{
		OnSplashScr.imgBg = null;
		imgButtonOn = null;
		imgPopup = null;
		imgPopup2 = null;
		Menu.me = null;
		imgBarMoney = null;
	}

	public void paintRoomList(MyGraphics g, MyVector roomList, int hSmall, int cmy)
	{
	}

	public void setName(int index, BoardScr board)
	{
		RoomListOnScr.index = index;
		if (!onMainMenu.isOngame)
		{
			RoomListOnScr.title = T.nameCasino[index];
		}
		else
		{
			RoomListOnScr.title = T.nameCasinoOngame[index];
		}
		CasinoMsgHandler.curScr = board;
	}

	public void paintPlayer(MyGraphics g, int index, int male, int countLeft, int countRight)
	{
		g.setColor(5299141);
		g.fillRect(4f, PaintPopup.hTab + 20 + AvMain.hBlack / 2 + 15, PaintPopup.gI().w - 8, 50f);
		imgRegGender.drawFrame((male == 2) ? 1 : 0, PaintPopup.gI().w / 2 - 21, PaintPopup.hTab + 20, 0, 3, g);
		imgRegGender.drawFrame((male != 2) ? 1 : 0, PaintPopup.gI().w / 2 + 21, PaintPopup.hTab + 20, 0, 3, g);
		Canvas.blackF.drawString(g, T.gender[0], PaintPopup.gI().w / 2 - 22, PaintPopup.hTab + 20 - AvMain.hBlack / 2, 2);
		Canvas.blackF.drawString(g, T.gender[1], PaintPopup.gI().w / 2 + 21, PaintPopup.hTab + 20 - AvMain.hBlack / 2, 2);
		g.drawImage(PaintPopup.imgMuiIOS[countLeft / 3][2], PaintPopup.gI().w / 2 - 45 - countLeft / 2, PaintPopup.hTab + AvMain.hBlack / 2 + 60, 3);
		g.drawImage(PaintPopup.imgMuiIOS[countRight / 3][3], PaintPopup.gI().w / 2 + 45 + countRight / 2, PaintPopup.hTab + AvMain.hBlack / 2 + 60, 3);
		GameMidlet.avatar.paintIcon(g, PaintPopup.gI().w / 2 + 1, PaintPopup.hTab + 87, false);
		Canvas.normalFont.drawString(g, T.nameStr + GameMidlet.avatar.name, PaintPopup.gI().w / 2, PaintPopup.hTab + 100, 2);
		Canvas.normalFont.drawString(g, T.moneyStr + GameMidlet.avatar.strMoney, PaintPopup.gI().w / 2, PaintPopup.hTab + 115, 2);
		Canvas.resetTrans(g);
		g.setColor(0);
	}

	public void updateKeyRegister()
	{
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(PaintPopup.gI().x + PaintPopup.gI().w / 2 - 42, PaintPopup.gI().y + PaintPopup.hTab + 5, 40, 30))
			{
				RegisterScr.gI().male = 1;
				RegisterScr.gI().getAvatarPart();
				Canvas.isPointerClick = false;
			}
			else if (Canvas.isPoint(PaintPopup.gI().x + PaintPopup.gI().w / 2, PaintPopup.gI().y + PaintPopup.hTab + 5, 40, 30))
			{
				RegisterScr.gI().male = 2;
				RegisterScr.gI().getAvatarPart();
				Canvas.isPointerClick = false;
			}
			else if (Canvas.isPoint(PaintPopup.gI().x + PaintPopup.gI().w / 2 - 20 - 45, PaintPopup.gI().y + PaintPopup.hTab + 20 + AvMain.hBlack / 2 + 15, 40, 50))
			{
				RegisterScr.gI().index = 1;
				RegisterScr.gI().setKeyLeftRight(-1);
				RegisterScr.gI().countLeft = 5;
				Canvas.isPointerClick = false;
			}
			else if (Canvas.isPoint(PaintPopup.gI().x + PaintPopup.gI().w / 2 - 20 + 45, PaintPopup.gI().y + PaintPopup.hTab + 20 + AvMain.hBlack / 2 + 15, 40, 50))
			{
				RegisterScr.gI().index = 1;
				RegisterScr.gI().setKeyLeftRight(1);
				RegisterScr.gI().countRight = 5;
				Canvas.isPointerClick = false;
			}
		}
	}

	public void initReg()
	{
		try
		{
			imgRegGender = new FrameImage(Image.createImage(T.getPath() + "/gender"), 32, 20);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void paintPopupBack(MyGraphics g, int x, int y, int w, int h, int countClose, bool isFull)
	{
		g.drawImageScale(imgEffectBack.imgFrame, x, y + h - 20 - imgEffectBack.frameHeight, w + 6, 100, 0);
		if (!isFull)
		{
			imgPopupBack.drawFrame(0, x, y, 0, g);
			imgPopupBack.drawFrame(1, x + w - imgPopupBack.frameWidth, y, 0, g);
			g.drawImageScale(imgPopupBackNum[0], x + imgPopupBack.frameWidth, y, w - imgPopupBack.frameWidth * 2, imgPopupBack.frameHeight, 0);
			g.drawImageScale(imgPopupBackNum[1], x + imgPopupBack.frameWidth, y + h - imgPopupBack.frameHeight, w - imgPopupBack.frameWidth * 2, imgPopupBack.frameHeight, 0);
			g.drawImageScale(imgPopupBackNum[2], x, y + imgPopupBack.frameHeight, imgPopupBack.frameWidth, h - imgPopupBack.frameHeight * 2, 0);
			g.drawImageScale(imgPopupBackNum[3], x + w - imgPopupBack.frameWidth, y + imgPopupBack.frameHeight, imgPopupBack.frameWidth, h - imgPopupBack.frameHeight * 2, 0);
			g.setColor(13495295);
			g.fillRect(x + imgPopupBack.frameWidth, y + imgPopupBack.frameHeight, w - imgPopupBack.frameWidth * 2, h - imgPopupBack.frameHeight * 2);
		}
		else
		{
			g.drawImageScale(imgPopupBackNum[0], x, y, w, imgPopupBack.frameHeight, 0);
			g.setColor(13495295);
			g.fillRect(x, y + imgPopupBack.frameHeight, w, h - imgPopupBack.frameHeight);
		}
		if (!isFull)
		{
			imgPopupBack.drawFrame(3, x, y + h - imgPopupBack.frameHeight, 0, g);
			imgPopupBack.drawFrame(2, x + w - imgPopupBack.frameWidth, y + h - imgPopupBack.frameHeight, 0, g);
		}
		if (countClose != -1)
		{
			if (!isFull)
			{
				g.drawImage(ListScr.imgCloseTab[countClose], x + w, y - 3, 3);
			}
			else
			{
				g.drawImage(ListScr.imgCloseTabFull[countClose], x + w - 10, y + 10, 3);
			}
		}
	}

	public void initImgCard()
	{
		if (imgCardBg != null)
		{
			return;
		}
		try
		{
			imgCardIcon = new Image[52];
			for (int i = 0; i < 52; i++)
			{
				imgCardIcon[i] = Image.createImagePNG(T.getPath() + "/card/" + i);
			}
			imgCardBg = Image.createImagePNG(T.getPath() + "/card/down");
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.StackTrace);
		}
	}

	public void resetCasino()
	{
		imgCardIcon = null;
		imgCardBg = null;
	}

	public void paintMoney(MyGraphics g, int x, int y)
	{
		int width = Canvas.tempFont.getWidth(GameMidlet.avatar.money[0] + string.Empty);
		int width2 = Canvas.tempFont.getWidth(GameMidlet.avatar.money[2] + string.Empty);
		Canvas.tempFont.drawString(g, GameMidlet.avatar.money[0] + string.Empty, x, y, 0);
		g.drawImage(MyInfoScr.gI().imgIcon[0], x + width + 19, y + Canvas.tempFont.getHeight() / 2, 3);
		g.drawImage(MyInfoScr.gI().imgIcon[1], x + width2 + 70 + width, y + Canvas.tempFont.getHeight() / 2, 3);
		Canvas.tempFont.drawString(g, GameMidlet.avatar.money[2] + string.Empty, x + width + 50, y, 0);
	}

	public void paintTabSoft(MyGraphics g)
	{
		int num = 40;
		g.drawImageScale(imgBar, 0, Canvas.hCan - num, Canvas.w, Canvas.hCan - num, 0);
	}

	public void paintCmdBar(MyGraphics g, Command left, Command center, Command right)
	{
		int num = Canvas.hCan - PaintPopup.hButtonSmall / 2 + 2;
		if (left != null && left.caption != string.Empty)
		{
			if (left.x != -1)
			{
				imgButtonOn.drawFrame(indexLeft, left.x + PaintPopup.wButtonSmall / 2, left.y + PaintPopup.hButtonSmall / 2, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, left.caption, left.x + PaintPopup.wButtonSmall / 2, left.y + PaintPopup.hButtonSmall / 2 - Canvas.normalWhiteFont.getHeight() / 2, 2);
			}
			else
			{
				imgButtonOn.drawFrame(indexLeft, 4 + PaintPopup.wButtonSmall / 2, num, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, left.caption, 4 + PaintPopup.wButtonSmall / 2, num - Canvas.normalWhiteFont.getHeight() / 2 - 1, 2);
			}
		}
		if (center != null && center.caption != string.Empty)
		{
			if (center.x != -1)
			{
				imgButtonOn.drawFrame(indexCenter, center.x + PaintPopup.wButtonSmall / 2, center.y + PaintPopup.hButtonSmall / 2, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, center.caption, center.x + PaintPopup.wButtonSmall / 2, center.y + PaintPopup.hButtonSmall / 2 - Canvas.normalWhiteFont.getHeight() / 2, 2);
			}
			else
			{
				imgButtonOn.drawFrame(indexCenter, Canvas.w / 2, num, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, center.caption, Canvas.w / 2, num - Canvas.normalWhiteFont.getHeight() / 2 - 1, 2);
			}
		}
		if (right != null && right.caption != string.Empty)
		{
			if (right.x != -1)
			{
				imgButtonOn.drawFrame(indexRight, right.x + PaintPopup.wButtonSmall / 2, right.y + PaintPopup.hButtonSmall / 2, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, right.caption, right.x + PaintPopup.wButtonSmall / 2, right.y + PaintPopup.hButtonSmall / 2 - Canvas.normalWhiteFont.getHeight() / 2, 2);
			}
			else
			{
				imgButtonOn.drawFrame(indexRight, Canvas.w - PaintPopup.wButtonSmall / 2 - 4, num, 0, 3, g);
				Canvas.normalWhiteFont.drawString(g, right.caption, Canvas.w - PaintPopup.wButtonSmall / 2 - 4, num - Canvas.normalWhiteFont.getHeight() / 2 - 1, 2);
			}
		}
	}

	public void updateKeyOn(Command left, Command center, Command right)
	{
		if (Canvas.isPointerClick)
		{
			switch (pointCmdBar(left, center, right))
			{
			case 1:
				indexLeft = 1;
				Canvas.isPointerClick = false;
				break;
			case 2:
				indexCenter = 1;
				Canvas.isPointerClick = false;
				break;
			case 3:
				indexRight = 1;
				Canvas.isPointerClick = false;
				break;
			}
		}
		if (Canvas.isPointerDown)
		{
			switch (pointCmdBar(left, center, right))
			{
			case 1:
				indexCenter = (indexRight = 0);
				break;
			case 2:
				indexLeft = (indexRight = 0);
				break;
			case 3:
				indexCenter = (indexLeft = 0);
				break;
			}
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		switch (pointCmdBar(left, center, right))
		{
		case 1:
			if (indexLeft == 1)
			{
				left.perform();
				Canvas.isPointerRelease = false;
				indexLeft = 0;
			}
			break;
		case 2:
			if (indexCenter == 1)
			{
				indexCenter = 0;
				center.perform();
				Canvas.isPointerRelease = false;
			}
			break;
		case 3:
			if (indexRight == 1)
			{
				indexRight = 0;
				right.perform();
				Canvas.isPointerRelease = false;
			}
			break;
		}
	}

	public static int pointCmdBar(Command left, Command center, Command right)
	{
		if (left != null && !left.caption.Equals(string.Empty))
		{
			if (left.x != -1)
			{
				if (Canvas.isPoint(left.x, left.y, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
				{
					return 1;
				}
			}
			else if (Canvas.isPoint(4, Canvas.hCan - PaintPopup.hButtonSmall, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
			{
				return 1;
			}
		}
		if (center != null && !center.caption.Equals(string.Empty))
		{
			if (center.x != -1)
			{
				if (Canvas.isPoint(center.x, center.y, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
				{
					return 2;
				}
			}
			else if (Canvas.isPoint(Canvas.w / 2 - PaintPopup.wButtonSmall / 2, Canvas.hCan - PaintPopup.hButtonSmall, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
			{
				return 2;
			}
		}
		if (right != null && !right.caption.Equals(string.Empty))
		{
			if (right.x != -1)
			{
				if (Canvas.isPoint(right.x, right.y, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
				{
					return 3;
				}
			}
			else if (Canvas.isPoint(Canvas.w - 4 - PaintPopup.wButtonSmall, Canvas.hCan - PaintPopup.hButtonSmall, PaintPopup.wButtonSmall, PaintPopup.hButtonSmall))
			{
				return 3;
			}
		}
		return -1;
	}

	public void paintDefaultPopup(MyGraphics g, int x, int y, int w, int h)
	{
		paintBorder(g, x, y, w, h, true);
	}

	public static void paintBorder(MyGraphics g, int x, int y, int w, int h, bool paintTop)
	{
		int width = imgPopup[0].getWidth();
		int height = imgPopup[0].getHeight();
		if (paintTop)
		{
			g.drawImage(imgPopup[0], x, y, 0);
			for (int i = 1; i < w / width - 1; i++)
			{
				g.drawImage(imgPopup[1], x + width * i, y, 0);
			}
			g.drawImage(imgPopup[1], x + w - width * 2, y, 0);
			g.drawImage(imgPopup[2], x + w - width, y, 0);
		}
		if (h / height > 2)
		{
			for (int j = 1; j < h / height; j++)
			{
				g.drawImage(imgPopup[3], x, y + height * j, 0);
				g.drawImage(imgPopup[4], x + w - width, y + height * j, 0);
			}
			g.drawImage(imgPopup[3], x, y + h - height * 2, 0);
			g.drawImage(imgPopup[4], x + w - width, y + h - height * 2, 0);
		}
		if (h > height * 2 - 10 * ScaleGUI.numScale && h <= height * 3)
		{
			g.drawImage(imgPopup[3], x, y + h / 2 - height / 2, 0);
			g.drawImage(imgPopup[4], x + w - width, y + h / 2 - height / 2, 0);
		}
		g.drawImage(imgPopup[5], x, y + h - height, 0);
		for (int k = 1; k < w / width - 1; k++)
		{
			g.drawImage(imgPopup[6], x + width * k, y + h - height, 0);
		}
		g.drawImage(imgPopup[6], x + w - width * 2, y + h - height, 0);
		g.drawImage(imgPopup[7], x + w - width, y + h - height, 0);
		g.setColor(colorNormal);
		g.fillRect(x + 20, y + 20, w - 40, h - 40);
	}

	public void paintLineRoom(MyGraphics g, int x, int y, int xTo, int yTo)
	{
		if (ScaleGUI.numScale == 1)
		{
			g.setColor(colorBold);
			g.fillRect(x, y + 1, xTo - x, yTo - y + 1);
		}
		else
		{
			g.setColor(colorBold);
			g.fillRect(x, y + 1, xTo - x, yTo - y + 2);
		}
	}

	public void paintSelect(MyGraphics g, int x, int y, int w, int h)
	{
		g.setColor(colorSelect);
		g.fillRect(x, y, w, h);
	}

	public void paintBorderTitle(MyGraphics g, int x, int y, int w, int h)
	{
		paintBorder(g, x, y, w, h, false);
		int width = imgPopup2[0].getWidth();
		g.drawImage(imgPopup2[0], x, y, 0);
		for (int i = 1; i < w / width - 1; i++)
		{
			g.drawImage(imgPopup2[1], x + width * i, y, 0);
		}
		g.drawImage(imgPopup2[1], x + w - width * 2, y, 0);
		g.drawImage(imgPopup2[2], x + w - width, y, 0);
	}

	public void paintTransMoney(MyGraphics g, int x, int y, int w, int h)
	{
		g.drawImage(imgBarMoney, x + w / 2, y + h / 2, 3);
	}
}
