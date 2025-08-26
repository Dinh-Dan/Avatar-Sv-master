using System;

public class PopupShop : MyScreen
{
	public static PopupShop me;

	private static string[] name;

	public static int numTap;

	public static int x;

	public static int y;

	public static int w;

	public static int h;

	public static int wCell;

	public static int num = 6;

	public static int hAllCell;

	public static int numH = 5;

	public static int hT;

	public static int focusTap;

	public MyVector[] listCell;

	private MyVector listCmd;

	private Command[] listCmdL;

	private Command[] listCmdR;

	public static int focus = 0;

	public static int numberPrice = 0;

	public static int duCam = 0;

	public static MyVector strDes = new MyVector();

	public static MyScreen lastScr;

	private int xL;

	private int fliped;

	public bool isFull;

	public static bool isTransFocus = false;

	public static bool isHorizontal = false;

	public static bool isSelectedTab = false;

	public static bool isQuaTrang;

	public static int wPrice = 0;

	public static int hPrice;

	public static int hDuHori;

	public static Image imgShadow;

	public static FrameImage imgScroll;

	private int indexScroll;

	public bool[] isDuCell;

	public string[] textTop;

	public static Image[] imgCell;

	public static Image[] imgTimeUse;

	public static FrameImage imgTimeUsePer;

	public new bool isHide;

	private int xMoney;

	private int wStr = 120 * AvMain.hd;

	public PopupShop()
	{
		hT = AvMain.hBlack + 2;
		wCell = 45 * AvMain.hd;
		init();
		initCmd();
		hPrice = 25 * (2 - AvMain.hd) + 40 * (Canvas.stypeInt + 1) + 10 * (AvMain.hd - 1) + MsgDlg.hCell + (AvMain.hd - 1) * 40;
		imgScroll = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/scroll"), 12 * AvMain.hd, 12 * AvMain.hd);
		imgShadow = Image.createImagePNG(T.getPath() + "/temp/shadowbox");
		imgTimeUse = new Image[2];
		for (int i = 0; i < 2; i++)
		{
			imgTimeUse[i] = Image.createImagePNG(T.getPath() + "/iconMenu/perUse" + i);
		}
		imgTimeUsePer = new FrameImage(Image.createImagePNG(T.getPath() + "/iconMenu/perUse2"), 3, (AvMain.hd != 2) ? 4 : 9);
		imgCell = new Image[2];
		for (int j = 0; j < 2; j++)
		{
			imgCell[j] = Image.createImagePNG(T.getPath() + "/iconMenu/focusCell" + j);
		}
	}

	public static PopupShop gI()
	{
		if (me == null)
		{
			me = new PopupShop();
		}
		return me;
	}

	public override void switchToMe()
	{
		lastScr = Canvas.currentMyScreen;
		xL = Canvas.h + 50;
		fliped = Canvas.getSecond();
		isTransFocus = true;
		wPrice = 86 + 60 * Canvas.stypeInt;
		isHorizontal = false;
		isQuaTrang = false;
		base.switchToMe();
	}

	public override void commandTab(int index)
	{
		if (index == 1)
		{
			doSelect();
		}
	}

	public override void commandAction(int index)
	{
		lastScr.commandAction(index);
	}

	public override void close()
	{
		Canvas.cameraList.close();
		isFull = false;
		lastScr.switchToMe();
		MapScr.avatarShop = null;
		isHorizontal = false;
		center = null;
		if (!Canvas.isInitChar)
		{
			return;
		}
		if (LoadMap.TYPEMAP == 25 && Welcome.indexFarmPath != 0)
		{
			Canvas.welcome = new Welcome();
			if (Welcome.indexFarmPath == 2)
			{
				Welcome.indexFarmPath = 3;
			}
			Canvas.welcome.initFarmPath(MapScr.instance);
			GameMidlet.avatar.direct = Base.LEFT;
		}
		else if (LoadMap.TYPEMAP == 57)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initShop(MapScr.instance);
		}
	}

	public void initCmd()
	{
		center = new Command(T.selectt, 1);
		setPosCenter();
	}

	public void setPosCenter()
	{
		if (center != null && (center.caption == null || center.caption.Equals(string.Empty)))
		{
			center = null;
		}
		if (left != null && (left.caption == null || left.caption.Equals(string.Empty)))
		{
			left = null;
		}
		int num = 0;
		if (left != null)
		{
			num++;
		}
		if (center != null)
		{
			num++;
		}
		if (right != null)
		{
			num++;
		}
		int num2 = Canvas.cameraList.y + wCell * 2 + 2 * AvMain.hd;
		int num3 = PaintPopup.gI().h - (Canvas.cameraList.y - y) - wCell * 2;
		int num4 = PaintPopup.hButtonSmall - 5 * AvMain.hd;
		if (num == 2)
		{
			num4 += 10 * AvMain.hd;
		}
		int num5 = num2 + num3 / 2 - num4 * num / 2;
		num2 = num5;
		num3 /= 2;
		num2 = Canvas.cameraList.y + wCell * 2;
		if (center != null)
		{
			center.x = x + w - PaintPopup.wButtonSmall / 2 - 10 * AvMain.hd;
			center.y = num5;
			num5 += num4;
		}
		if (left != null)
		{
			left.x = x + w - PaintPopup.wButtonSmall / 2 - 10 * AvMain.hd;
			left.y = num5;
			num5 += num4;
		}
		if (right != null)
		{
			right.x = x + w - PaintPopup.wButtonSmall / 2 - 10 * AvMain.hd;
			right.y = num5;
			num5 += num4;
		}
	}

	public static void init()
	{
		w = wCell * 6 + 11 + AvMain.hDuBox + 2;
		h = wCell * 5 + 40 + AvMain.hDuBox * AvMain.hd;
		x = Canvas.hw - wCell * 6 / 2;
	}

	public static void addStr(string str)
	{
		if (str != null)
		{
			strDes.addElement(new StringObj(str, Canvas.tempFont.getWidth(str)));
		}
	}

	public void addElement(string[] name1, MyVector[] listCell1, MyVector cmd, sbyte[] idIcon)
	{
		focusTap = 0;
		listCell = listCell1;
		left = (center = (right = null));
		listCmdL = new Command[listCell1.Length];
		listCmdR = new Command[listCell1.Length];
		isDuCell = new bool[listCell1.Length];
		textTop = new string[listCell1.Length];
		listCmd = cmd;
		name = name1;
		numTap = listCell.Length;
		PaintPopup.gI().setInfo(name[focusTap], w, h, numTap, 0, name, idIcon);
		x = PaintPopup.gI().x;
		y = PaintPopup.gI().y;
		setCmyLim();
	}

	public void setHorizontal(bool isHori)
	{
		isHorizontal = isHori;
	}

	public override void closeTabAll()
	{
		close();
	}

	public void setCmyLim()
	{
		try
		{
			focus = 0;
			hDuHori = 0;
			if (isHorizontal || isDuCell[focusTap])
			{
				hDuHori = 25 * AvMain.hd;
			}
			if (listCell[focusTap] != null)
			{
				hAllCell = listCell[focusTap].size() / 5;
				numH = 2;
				if (listCell[focusTap].size() % 5 != 0)
				{
					hAllCell++;
				}
				if (hAllCell < numH || isHorizontal)
				{
					hAllCell = numH;
				}
			}
			int num = 1;
			if (listCell[focusTap] == null)
			{
				PopupShop.num = 1;
			}
			else
			{
				num = listCell[focusTap].size();
				PopupShop.num = 6;
				if (isHorizontal)
				{
					PopupShop.num = num / 2 + 1;
					if (PopupShop.num < 6)
					{
						PopupShop.num = 6;
					}
				}
			}
			if (numH > 2 || isDuCell[focusTap])
			{
				duCam = 0;
			}
			Canvas.cameraList.setInfo(PaintPopup.gI().x + (w - 6 * wCell) / 2, PaintPopup.gI().y + PaintPopup.hTab + ((isHorizontal || isDuCell[focusTap]) ? hDuHori : 0) + AvMain.hDuBox, wCell, wCell, wCell * PopupShop.num, wCell * hAllCell, wCell * 6, (numH != 5) ? (numH * wCell - ((!isHorizontal) ? duCam : 0)) : (h - PaintPopup.hTab - AvMain.hDuBox * 2), num);
			Canvas.cameraList.isQuaTrang = isQuaTrang;
			setCaption();
			PaintPopup.gI().setNameAndFocus(name[focusTap], focusTap);
			setSelected(focus, false);
			setPosCenter();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void setDuCell(bool isa, int index, string text)
	{
		isDuCell[index] = isa;
		textTop[index] = text;
		setCmyLim();
	}

	private void doSelect()
	{
		if (focus < listCell[focusTap].size())
		{
			((Command)listCell[focusTap].elementAt(focus)).action.perform();
			setCaption();
		}
	}

	public override void update()
	{
		PaintPopup.gI().update();
		lastScr.update();
		if (xL != 0)
		{
			xL += -xL >> 1;
		}
		if (xL == -1)
		{
			xL = 0;
		}
		if (listCell[focusTap] != null)
		{
			int num = listCell[focusTap].size();
			for (int i = 0; i < num; i++)
			{
				if (isTransFocus)
				{
					((Command)listCell[focusTap].elementAt(i)).update();
				}
			}
		}
		if (listCell[focusTap] == null)
		{
			((Command)listCmd.elementAt(focusTap)).update();
		}
		if (isHorizontal)
		{
			float num2 = (PopupShop.num * wCell - Canvas.cameraList.disX) / 3;
			indexScroll = (int)(CameraList.cmtoX / num2);
			if (indexScroll > 2)
			{
				indexScroll = 2;
			}
		}
	}

	public void setCmdLeft(Command cmd, int index)
	{
		listCmdL[index] = cmd;
	}

	public void setCmdRight(Command cmd, int index)
	{
		listCmdR[index] = cmd;
	}

	public override void updateKey()
	{
		updateKeyMain();
		if (!isHide)
		{
			base.updateKey();
		}
	}

	public void setNumberPrice(int dir)
	{
		numberPrice += dir;
		if (numberPrice < 0)
		{
			numberPrice = 99;
		}
		if (numberPrice > 99)
		{
			numberPrice = 0;
		}
		setCaption();
	}

	public override void setSelected(int se, bool isAc)
	{
		if (listCell[focusTap] == null)
		{
			return;
		}
		if (focus == se && center != null && isAc && center == null)
		{
			center.perform();
		}
		if (center != null && listCmdL[focusTap] != null)
		{
			left = listCmdL[focusTap];
			right = listCmdR[focusTap];
			if (listCell[focusTap] != null && focus < listCell[focusTap].size())
			{
				center = (Command)listCell[focusTap].elementAt(focus);
			}
			setPosCenter();
		}
		else
		{
			left = null;
		}
		focus = se;
		setCaption();
	}

	public void updateKeyMain()
	{
		int num = PaintPopup.gI().setupdateTab();
		if (num != 0)
		{
			setTap(num);
			Canvas.isPointerClick = false;
		}
	}

	public void setTap(int dir)
	{
		focusTap += dir;
		if (focusTap == numTap)
		{
			focusTap = 0;
		}
		if (focusTap < 0)
		{
			focusTap = numTap - 1;
		}
		left = (center = (right = null));
		setCmyLim();
		if (listCmdL != null && listCmdL[focusTap] != null)
		{
			left = listCmdL[focusTap];
			if (listCmdR != null)
			{
				right = listCmdR[focusTap];
			}
			else
			{
				right = null;
			}
			left.x = x + w - MyScreen.wTab / 2 - 15 * AvMain.hd;
			left.y = y + h - PaintPopup.hButtonSmall - 20 * AvMain.hd;
			setPosCenter();
		}
	}

	public void setCaption()
	{
		if (listCell[focusTap] != null && focus < listCell[focusTap].size())
		{
			center = (Command)listCell[focusTap].elementAt(focus);
			setPosCenter();
		}
		else if (listCmd != null && focusTap < listCmd.size())
		{
			Command command = (Command)listCmd.elementAt(focusTap);
			if (command != null)
			{
				center = command;
				setPosCenter();
			}
		}
		else
		{
			center = null;
		}
		isTransFocus = true;
		fliped = Canvas.getSecond();
	}

	public override void setHidePointer(bool isa)
	{
		isHide = isa;
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		Canvas.resetTrans(g);
		PaintPopup.gI().paint(g);
		g.setColor(0);
		g.translate(Canvas.cameraList.x, Canvas.cameraList.y);
		if (isHorizontal && listCell[focusTap] != null)
		{
			int num = xMoney;
			if (CRes.abs(num) > 200)
			{
				num = 0;
			}
			g.setClip(-5 * AvMain.hd, -2 - hDuHori + 13 * AvMain.hd - 20 * AvMain.hd, w - 5 * AvMain.hd, 100f);
			g.drawImage(MyInfoScr.gI().imgIcon[0], num + 15 * AvMain.hd, -2 - hDuHori + 13 * AvMain.hd, 3);
			g.drawImage(MyInfoScr.gI().imgIcon[1], num + 115 * AvMain.hd, -2 - hDuHori + 13 * AvMain.hd, 3);
			g.drawImage(MyInfoScr.gI().imgIcon[4], num + 200 * AvMain.hd, -2 - hDuHori + 13 * AvMain.hd, 3);
			g.drawImage(MyInfoScr.gI().imgIcon[2], num + 280 * AvMain.hd, -2 - hDuHori + 13 * AvMain.hd, 3);
			Canvas.tempFont.drawString(g, Canvas.getMoneys(GameMidlet.avatar.money[0]) + string.Empty, num + 15 * AvMain.hd + MyInfoScr.gI().imgIcon[0].getWidth() / 2 + 5 * AvMain.hd, -2 - hDuHori + 10 * AvMain.hd - Canvas.tempFont.getHeight() / 2, 0);
			Canvas.tempFont.drawString(g, Canvas.getMoneys(GameMidlet.avatar.money[2]), num + 115 * AvMain.hd + MyInfoScr.gI().imgIcon[1].getWidth() / 2 + 5 * AvMain.hd, -2 - hDuHori + 10 * AvMain.hd - Canvas.tempFont.getHeight() / 2, 0);
			Canvas.tempFont.drawString(g, Canvas.getMoneys(GameMidlet.avatar.luongKhoa) + string.Empty, num + 200 * AvMain.hd + MyInfoScr.gI().imgIcon[1].getWidth() / 2 + 5 * AvMain.hd, -2 - hDuHori + 10 * AvMain.hd - Canvas.tempFont.getHeight() / 2, 0);
			Canvas.tempFont.drawString(g, Canvas.getMoneys(GameMidlet.avatar.money[3]) + string.Empty, num + 280 * AvMain.hd + MyInfoScr.gI().imgIcon[2].getWidth() / 2 + 5 * AvMain.hd, -2 - hDuHori + 10 * AvMain.hd - Canvas.tempFont.getHeight() / 2, 0);
			if (CRes.abs(xMoney) > 250)
			{
				xMoney = 0;
			}
			xMoney--;
		}
		if (listCell[focusTap] != null)
		{
			if (isDuCell[focusTap])
			{
				Canvas.normalFont.drawString(g, textTop[focusTap], 0, -Canvas.blackF.getHeight() - 10, 0);
			}
			g.setClip(0f, 0f, 6 * wCell, Canvas.cameraList.disY);
			if (!isHorizontal)
			{
				g.translate(0f, 0f - CameraList.cmy);
			}
			else
			{
				g.translate(0f - CameraList.cmx, 0f);
			}
			for (int i = 0; i < hAllCell * PopupShop.num; i++)
			{
				if (i == focus && !isHide)
				{
					g.drawImage(imgCell[1], wCell * (i % PopupShop.num) + 2 + wCell / 2, wCell * (i / PopupShop.num) + wCell / 2, 3);
				}
				else
				{
					g.drawImage(imgCell[0], wCell * (i % PopupShop.num) + 2 + wCell / 2, wCell * (i / PopupShop.num) + wCell / 2, 3);
				}
			}
			int num2 = listCell[focusTap].size();
			int num3 = (int)CameraList.cmy / wCell * PopupShop.num;
			if (num3 < 0)
			{
				num3 = 0;
			}
			int num4 = (int)CameraList.cmy / wCell * PopupShop.num + (numH + 1) * PopupShop.num;
			if (num4 > listCell[focusTap].size())
			{
				num4 = listCell[focusTap].size();
			}
			for (int j = num3; j < num4 && j < num2; j++)
			{
				g.setClip(CameraList.cmx, 0f, 6 * wCell, Canvas.cameraList.disY);
				((Command)listCell[focusTap].elementAt(j)).paint(g, wCell * (j % PopupShop.num), wCell * (j / PopupShop.num));
			}
			if (!isHorizontal)
			{
				g.translate(0f, CameraList.cmy - (float)duCam);
			}
			else
			{
				g.translate(CameraList.cmx, 0f);
			}
			g.setClip(0f, 0f, w - 9, h);
			if (numH == 2)
			{
				if (isHorizontal && strDes != null && focus < listCell[focusTap].size() && !isHide && MapScr.avatarShop != null)
				{
					MapScr.avatarShop.paintIcon(g, 25 * AvMain.hd - ((AvMain.hd == 1) ? 10 : 0), numH * wCell + hDuHori + PaintPopup.hTab, false);
					g.translate(50 * AvMain.hd - ((AvMain.hd == 1) ? 20 : 0), 0f);
				}
				if (!isHide)
				{
					paintStringSmall(g);
				}
			}
			else
			{
				paintStringBig(g);
			}
		}
		else
		{
			g.setClip(-5f, 0f, w - 10 * AvMain.hd, h);
			((Command)listCmd.elementAt(focusTap)).paint(g, 0, 0);
		}
		if (!isHide && (Canvas.welcome == null || Welcome.isOut || !Welcome.isPaintArrow))
		{
			base.paint(g);
		}
		Canvas.resetTrans(g);
		Canvas.paintPlus(g);
	}

	public void paintStringBig(MyGraphics g)
	{
		if (Canvas.getSecond() - fliped < 1 || strDes == null || focus >= listCell[focusTap].size())
		{
			return;
		}
		int num = focus % PopupShop.num * wCell - wStr / 2 + wCell / 2;
		int num2 = (int)((float)((focus / PopupShop.num + 1) * wCell) - CameraList.cmy + 5f);
		int num3 = strDes.size() * AvMain.hBlack2 + AvMain.hDuBox * 2 + AvMain.hBlack2 * 2;
		if (num2 + num3 + y + 12 > Canvas.h)
		{
			num2 -= num3 + wCell + 10;
		}
		if (num2 + y < 0)
		{
			num2 = -y;
		}
		if (num + x + 5 + wStr > Canvas.w)
		{
			num = Canvas.w - (x + 5 + wStr);
		}
		else if (num + x < 0)
		{
			num = -x;
		}
		g.setClip(num, num2, wStr, num3 * AvMain.hd);
		Canvas.paint.paintPopupBack(g, num, num2, wStr, num3, -1, false);
		num += AvMain.hDuBox;
		num2 += AvMain.hDuBox;
		for (int i = 0; i < strDes.size(); i++)
		{
			StringObj stringObj = (StringObj)strDes.elementAt(i);
			int num4 = 0;
			if (stringObj.w2 > wStr + 5)
			{
				stringObj.transTextLimit(wStr - 30);
				if (stringObj.dis >= 0)
				{
					num4 = stringObj.dis;
				}
			}
			Canvas.tempFont.drawString(g, stringObj.str, num - num4, num2 + 5 + i * hT, 0);
		}
	}

	public void paintStringSmall(MyGraphics g)
	{
		int num = w;
		if (center != null || left != null)
		{
			num = w / 2;
		}
		g.setClip(0f, numH * wCell, w - 95 * AvMain.hd, 100 * AvMain.hd);
		int num2 = PaintPopup.hTab + hDuHori + AvMain.hDuBox + numH * wCell + (h - (PaintPopup.hTab + hDuHori + AvMain.hDuBox * 2 + numH * wCell)) / 2;
		if (strDes == null || focus >= listCell[focusTap].size())
		{
			return;
		}
		int num3 = wCell * 2;
		int num4 = PaintPopup.gI().h - (Canvas.cameraList.y - y) - wCell * 2;
		int num5 = PaintPopup.hButtonSmall - 5 * AvMain.hd;
		int num6 = num3 + num4 / 2 - strDes.size() * Canvas.tempFont.getHeight() / 2 - 5 * AvMain.hd;
		for (int i = 0; i < strDes.size(); i++)
		{
			StringObj stringObj = (StringObj)strDes.elementAt(i);
			int num7 = 0;
			if (stringObj.w2 > num - 25 * AvMain.hd + 5)
			{
				stringObj.transTextLimit(num - 25 * AvMain.hd - 10 * AvMain.hd);
				if (stringObj.dis >= 0)
				{
					num7 = stringObj.dis;
				}
			}
			if (isHorizontal)
			{
				Canvas.tempFont.drawString(g, stringObj.str, 2 - num7, num6 + i * Canvas.tempFont.getHeight(), 0);
			}
			else
			{
				Canvas.tempFont.drawString(g, stringObj.str, 2 - num7, num6 + i * Canvas.tempFont.getHeight(), 0);
			}
		}
	}

	public static void resetIsTrans()
	{
		isTransFocus = false;
		strDes.removeAllElements();
	}
}
