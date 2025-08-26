using UnityEngine;

public class MessageScr : MyScreen
{
	private class IActionChat : IAction
	{
		public void perform()
		{
			if (me.focusTab == 0)
			{
				MapScr.gI().onChatFromMe(me.tfChat.getText());
				me.tfChat.setText(string.Empty);
			}
			else if (me.focusTab == 2)
			{
				me.doSendMsg();
			}
		}
	}

	public static MessageScr me;

	private int x;

	private int y;

	private int w;

	private int h;

	private new int wTab;

	private int focusTab;

	private new int hTab;

	private int hDis;

	private new int hText;

	private new int selected;

	private int size;

	private sbyte countClose;

	private sbyte numTab = 2;

	private string[] nameTab = new string[3]
	{
		"Chung",
		"Tin den",
		string.Empty
	};

	public sbyte sizeTab = 2;

	public sbyte hString;

	private int cmtoY;

	private int cmy;

	private int cmdy;

	private int cmvy;

	private int cmyLim;

	private MyVector listTextTab_1 = new MyVector();

	private MyVector listPlayer = new MyVector();

	private ElementPlayer chatPlayer;

	private MyScreen lastScr;

	private TField tfChat;

	private Command cmdChat;

	public FrameImage imgTick;

	private FrameImage imgDel;

	public bool isNewMsg;

	public static Image[] imgArrow;

	private Image imgBound;

	private int indexDel = -1;

	private bool isTranKey;

	private bool isClickDel;

	private sbyte indexTab = -1;

	private bool trans;

	private bool isG;

	private bool isDel;

	private int pa;

	private int dxTran;

	private int timeOpen;

	private int pyLast;

	private int dyTran;

	private long delay;

	private long timeDelay;

	private long count;

	private long timePoint;

	private int vX;

	private int vY;

	public MessageScr()
	{
		x = 0;
		y = 0;
		h = Canvas.hCan;
		hTab = 35 * AvMain.hd;
		hText = 30;
		tfChat = new TField("chat", this, new IActionChat());
		tfChat.setFocus(true);
		tfChat.showSubTextField = false;
		tfChat.autoScaleScreen = true;
		tfChat.setIputType(ipKeyboard.TEXT);
		init(Canvas.hCan);
		tfChat.x = 5 * AvMain.hd;
		tfChat.setMaxTextLenght(40);
		tfChat.action = new IActionChat();
		imgArrow = new Image[2];
		imgArrow[0] = Image.createImagePNG(T.getPath() + "/main/ar");
		imgArrow[1] = Image.createImagePNG(T.getPath() + "/main/ar0");
		imgBound = Image.createImagePNG(T.getPath() + "/iconMenu/nummsg");
		imgDel = new FrameImage(Image.createImagePNG(T.getPath() + "/iconMenu/btDelMes"), 37 * AvMain.hd, 23 * AvMain.hd);
		init(h);
	}

	public static MessageScr gI()
	{
		return (me != null) ? me : (me = new MessageScr());
	}

	public void init(int hc)
	{
		h = hc;
		w = Canvas.w;
		wTab = 100 * AvMain.hd;
		if (Screen.orientation == ScreenOrientation.Portrait)
		{
			wTab = 80 * AvMain.hd;
		}
		hDis = h - hTab;
		tfChat.y = hc - tfChat.height - 3 * AvMain.hd;
		cmdChat = new Command(T.chat, new IActionChat());
		cmdChat.x = Canvas.w - PaintPopup.wButtonSmall / 2 - 2 * AvMain.hd;
		cmdChat.y = tfChat.y + tfChat.height / 2 - PaintPopup.hButtonSmall / 2;
		tfChat.width = cmdChat.x - PaintPopup.wButtonSmall / 2 - 10 * AvMain.hd;
		changeFocusTab(focusTab);
	}

	public void doSendMsg()
	{
		if (tfChat.getText().Equals(string.Empty))
		{
			return;
		}
		string text = tfChat.getText();
		if (text.IndexOf("hack") != -1)
		{
			text += " ";
			for (int i = 0; i < chatPlayer.text.size(); i++)
			{
				TextMsg textMsg = (TextMsg)chatPlayer.text.elementAt(i);
				for (int j = 0; j < textMsg.text.Length; j++)
				{
					text += textMsg.text[j];
				}
			}
			GlobalService.gI().doServerKick(chatPlayer.IDPlayer, text);
			tfChat.setText(string.Empty);
		}
		else
		{
			GlobalService.gI().chatTo(chatPlayer.IDPlayer, text);
			tfChat.setText(string.Empty);
			addPlayer(chatPlayer.IDPlayer, chatPlayer.name, text, true, null);
		}
	}

	public override void switchToMe()
	{
		if (imgTick == null)
		{
			imgTick = new FrameImage(Image.createImagePNG(T.getPath() + "/iconMenu/tickMSg"), 6 * AvMain.hd, 6 * AvMain.hd);
		}
		lastScr = Canvas.currentMyScreen;
		init(Canvas.hCan);
		base.switchToMe();
		focusTab = 1;
		changeFocusTab(0);
		isHide = true;
	}

	public void addText(string name, string text)
	{
		listTextTab_1.addElement(new ElementPlayer(name, text));
		if (listTextTab_1.size() > 100)
		{
			listTextTab_1.removeElementAt(0);
		}
		if (focusTab == 0)
		{
			size = listTextTab_1.size();
			setLimit();
		}
	}

	public void startChat(Avatar p)
	{
		gI().addPlayer(p.IDDB, p.name, string.Empty, false, null);
		chatPlayer = (ElementPlayer)listPlayer.elementAt(listPlayer.size() - 1);
		switchToMe();
		changeFocusTab(2);
	}

	private ElementPlayer getPlayer(int id)
	{
		for (int i = 0; i < listPlayer.size(); i++)
		{
			ElementPlayer elementPlayer = (ElementPlayer)listPlayer.elementAt(i);
			if (elementPlayer.IDPlayer == id)
			{
				return elementPlayer;
			}
		}
		return null;
	}

	public void addPlayer(int ID, string name, string text, bool isOwner, IAction act)
	{
		ElementPlayer player = getPlayer(ID);
		if (player == null)
		{
			player = new ElementPlayer(ID, name, text);
			player.action = act;
			listPlayer.addElement(player);
		}
		else
		{
			player.addText(text, isOwner);
			if (focusTab == 2 && player == chatPlayer)
			{
				int num = 0;
				for (int i = 0; i < chatPlayer.text.size(); i++)
				{
					TextMsg textMsg = (TextMsg)chatPlayer.text.elementAt(i);
					for (int j = 0; j < textMsg.text.Length; j++)
					{
						num++;
					}
					num += 2;
				}
				size = num;
				setLimit();
			}
		}
		if (focusTab == 1)
		{
			size = listPlayer.size();
			setLimit();
		}
		if (focusTab != 1 || Canvas.currentMyScreen != me)
		{
			isNewMsg = true;
		}
	}

	private void setLimit()
	{
		int num = size * hText - (hDis - 14 * AvMain.hd);
		if (cmtoY == cmyLim)
		{
			cmtoY = num;
		}
		cmyLim = num;
		if (cmyLim < 0)
		{
			cmyLim = 0;
			cmy = (cmtoY = 0);
		}
	}

	public override void keyPress(int keyCode)
	{
		if (focusTab != 1 && tfChat.isFocused())
		{
			tfChat.keyPressed(keyCode);
		}
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
		if (focusTab != 1 && (chatPlayer == null || focusTab != 2 || !chatPlayer.name.Equals("admin")))
		{
			tfChat.update();
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
				if (cmy < -hDis / 2)
				{
					cmy = -hDis / 2;
					cmtoY = 0;
					vY = 0;
				}
			}
			else if (cmy > cmyLim)
			{
				if (cmy < cmyLim + hDis / 2)
				{
					cmy = cmyLim + hDis / 2;
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
		else if (!trans)
		{
			if (cmy < 0)
			{
				cmtoY = 0;
			}
			else if (cmy > cmyLim)
			{
				cmtoY = cmyLim;
			}
		}
		if (cmy != cmtoY)
		{
			cmvy = cmtoY - cmy << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
	}

	public override void updateKey()
	{
		base.updateKey();
		int num = wTab + 20;
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(Canvas.w - 25 * AvMain.hd - 20 * AvMain.hd, y + hTab - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				isTranKey = true;
				Canvas.isPointerClick = false;
				countClose = 5;
			}
			else
			{
				for (int i = 0; i < numTab; i++)
				{
					if (Canvas.isPoint(x + 12 * AvMain.hd + i * num, y, num, PaintPopup.hTab))
					{
						isTranKey = true;
						Canvas.isPointerClick = false;
						indexTab = (sbyte)i;
						break;
					}
				}
			}
			if (indexDel != -1 && Canvas.isPoint(w - imgDel.frameWidth - 10 * AvMain.hd - 10 * AvMain.hd, y + PaintPopup.hTab + 10 * AvMain.hd + indexDel * hText, imgDel.frameWidth + 20 * AvMain.hd, hText))
			{
				Canvas.isPointerClick = false;
				isTranKey = true;
				isClickDel = true;
			}
		}
		if (isTranKey)
		{
			if (Canvas.isPointerDown)
			{
				if (countClose == 5 && !Canvas.isPoint(Canvas.w - 25 * AvMain.hd - 20 * AvMain.hd, y + hTab - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					countClose = 0;
				}
				else if (indexTab != -1 && !Canvas.isPoint(x + 12 * AvMain.hd + indexTab * num, y, num, PaintPopup.hTab))
				{
					indexTab = -1;
				}
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranKey = false;
				if (isClickDel)
				{
					isClickDel = false;
					listPlayer.removeElementAt(indexDel);
					changeFocusTab(1);
					indexDel = -1;
				}
				else if (indexTab != -1)
				{
					changeFocusTab(indexTab);
					indexTab = -1;
				}
				else if (countClose == 5)
				{
					lastScr.switchToMe();
					if (Screen.orientation != ScreenOrientation.Portrait && ipKeyboard.tk != null)
					{
						ipKeyboard.tk.active = false;
						ipKeyboard.tk = null;
					}
					countClose = 0;
				}
			}
		}
		updateKeyText();
	}

	private void changeFocusTab(int foc)
	{
		switch (foc)
		{
		case 0:
			if (AvMain.hd == 1)
			{
				hText = 40;
			}
			else
			{
				hText = 60;
			}
			hDis = h - PaintPopup.hTab - tfChat.height - 10 * AvMain.hd;
			size = listTextTab_1.size();
			setLimit();
			center = cmdChat;
			break;
		case 1:
			if (AvMain.hd == 1)
			{
				hText = 50;
			}
			else
			{
				hText = 70;
			}
			hDis = h - PaintPopup.hTab - 12 * AvMain.hd;
			cmyLim = listPlayer.size() * hText - (hDis - 10 * AvMain.hd);
			size = listPlayer.size();
			center = null;
			if (Screen.orientation != ScreenOrientation.Portrait && ipKeyboard.tk != null)
			{
				ipKeyboard.tk.active = false;
				ipKeyboard.tk = null;
			}
			isNewMsg = false;
			break;
		case 2:
		{
			numTab = 3;
			hText = Canvas.fontChat.getHeight();
			hDis = h - PaintPopup.hTab - 5 * AvMain.hd;
			center = null;
			if (foc != 1 && (chatPlayer == null || foc != 2 || !chatPlayer.name.Equals("admin")))
			{
				hDis = h - PaintPopup.hTab - tfChat.height;
				center = cmdChat;
			}
			int num = 0;
			for (int i = 0; i < chatPlayer.text.size(); i++)
			{
				TextMsg textMsg = (TextMsg)chatPlayer.text.elementAt(i);
				for (int j = 0; j < textMsg.text.Length; j++)
				{
					num++;
				}
				num += 2;
			}
			cmyLim = num * hText - hDis;
			size = num;
			nameTab[foc] = chatPlayer.name;
			if (nameTab[foc].Length > 10)
			{
				nameTab[foc] = nameTab[foc].Substring(0, 10);
			}
			hString = (sbyte)Canvas.fontChat.getHeight();
			break;
		}
		}
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		cmtoY = (cmy = cmyLim);
		focusTab = foc;
	}

	private void updateKeyText()
	{
		count++;
		if (Canvas.isPointerClick && Canvas.isPoint(x, y + hTab, w, hDis))
		{
			Canvas.isPointerClick = false;
			pyLast = Canvas.pyLast;
			isG = false;
			if (vY != 0)
			{
				isG = true;
			}
			pa = cmtoY;
			timeDelay = count;
			trans = true;
			int num = y + hTab + 10 * AvMain.hd;
			int num2 = (cmtoY + Canvas.py - num) / hText;
			if (Canvas.isPoint(0, y + PaintPopup.hTab + 10 * AvMain.hd + num2 * hText, w, hText))
			{
				isDel = true;
			}
		}
		if (trans)
		{
			int num3 = pyLast - Canvas.py;
			pyLast = Canvas.py;
			long num4 = count - timeDelay;
			if (Canvas.isPointerDown)
			{
				if (count % 2 == 0)
				{
					dyTran = Canvas.py;
					timePoint = count;
				}
				vY = 0;
				if (Math.abs(num3) < 10 * AvMain.hd)
				{
					int num5 = y + hTab + 10 * AvMain.hd;
					int num6 = (cmtoY + Canvas.py - num5) / hText;
					if (num6 >= 0 && num6 < size)
					{
						selected = num6;
					}
				}
				if (isDel && Canvas.dx() >= 10 * AvMain.hd)
				{
					int num7 = y + hTab + 10 * AvMain.hd;
					int num8 = (cmtoY + Canvas.py - num7) / hText;
					indexDel = num8;
				}
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num4 > 3 && num4 < 8)
				{
					int num9 = y + hTab + 10 * AvMain.hd;
					int num10 = (cmtoY + Canvas.py - num9) / hText;
					if (num10 >= 0 && num10 < size && !isG)
					{
						isHide = false;
					}
				}
				if (cmtoY < 0 || cmtoY > cmyLim)
				{
					cmtoY = pa + num3 / 2;
					pa = cmtoY;
				}
				else
				{
					cmtoY = pa + num3 / 2;
					pa = cmtoY;
				}
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease)
			{
				trans = false;
				isG = false;
				if (isDel && Canvas.dx() < -10 * AvMain.hd)
				{
					indexDel = -1;
				}
				isDel = false;
				int num11 = (int)(count - timePoint);
				int num12 = dyTran - Canvas.py;
				if (CRes.abs(num12) > 40 && num11 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num12 / num11 * 10;
				}
				timePoint = -1L;
				if (Math.abs(num3) < 10 * AvMain.hd && Math.abs(Canvas.dx()) < 10 * AvMain.hd)
				{
					if (num4 <= 4)
					{
						isHide = false;
						timeOpen = 5;
					}
					else
					{
						click();
					}
				}
				else if (Math.abs(Canvas.dx()) <= 10 * AvMain.hd)
				{
				}
				trans = false;
				Canvas.isPointerRelease = false;
			}
		}
		base.updateKey();
	}

	private void click()
	{
		if (selected > size - 1)
		{
			selected = size - 1;
		}
		else if (focusTab == 1 && listPlayer.size() > 0)
		{
			ElementPlayer elementPlayer = (ElementPlayer)listPlayer.elementAt(selected);
			if (elementPlayer.action != null)
			{
				elementPlayer.action.perform();
				listPlayer.removeElement(elementPlayer);
				size = listPlayer.size();
				setLimit();
			}
			else
			{
				chatPlayer = (ElementPlayer)listPlayer.elementAt(selected);
				chatPlayer.numSMS = 0;
				changeFocusTab(2);
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		Canvas.resetTrans(g);
		Canvas.paint.paintTransBack(g);
		Canvas.paint.paintBoxTab(g, x, y, h, w, focusTab, PaintPopup.gI().wSub, wTab, PaintPopup.hTab, numTab, 3, PaintPopup.gI().count, PaintPopup.gI().colorTab, nameTab[focusTab], -1, -1, false, true, nameTab, 0f, null);
		Canvas.resetTrans(g);
		int num = wTab + 10 * AvMain.hd;
		if (isNewMsg)
		{
			imgTick.drawFrame((Canvas.gameTick % 20 <= 9) ? 1 : 0, x + 12 * AvMain.hd + num + num / 2 + wTab / 2 - 8 * AvMain.hd, y + hTab - 17 * AvMain.hd, 0, 3, g);
		}
		g.setClip(x, y + PaintPopup.hTab + 4 * AvMain.hd, w, hDis);
		g.translate(x, y + PaintPopup.hTab + 10 * AvMain.hd);
		int num2 = cmy / hText - 1;
		if (num2 < 0)
		{
			num2 = 0;
		}
		int num3 = num2 + h / hText;
		if (num3 > size)
		{
			num3 = size;
		}
		switch (focusTab)
		{
		case 0:
			paintPublicTab(g, num2, num3);
			break;
		case 1:
			paintListPlayerTab(g, num2, num3);
			break;
		case 2:
			paintChatTab(g, num2, num3);
			break;
		}
		Canvas.resetTrans(g);
		if (focusTab != 1 && (chatPlayer == null || focusTab != 2 || !chatPlayer.name.Equals("admin")))
		{
			tfChat.paint(g);
		}
		Canvas.resetTrans(g);
		base.paint(g);
		g.drawImage(ListScr.imgCloseTabFull[countClose / 3], Canvas.w - 25 * AvMain.hd, y + hTab, 3);
	}

	private void paintChatTab(MyGraphics g, int x0, int y0)
	{
		g.translate(0f, -cmy);
		int num = 0;
		g.setColor(0);
		for (int i = 0; i < chatPlayer.text.size(); i++)
		{
			TextMsg textMsg = (TextMsg)chatPlayer.text.elementAt(i);
			if (!textMsg.isOwner)
			{
				ChatPopup.paintRoundRect(g, 5 * AvMain.hd, num - 2 * AvMain.hd, textMsg.wPopup, textMsg.text.Length * hString + 10 * AvMain.hd, 12320735, 9493435, 2);
				g.drawImage(imgArrow[1], 23 * AvMain.hd, num - 3 * AvMain.hd + textMsg.text.Length * hString + 10 * AvMain.hd, MyGraphics.TOP | MyGraphics.HCENTER);
				for (int j = 0; j < textMsg.text.Length; j++)
				{
					Canvas.fontChat.drawString(g, textMsg.text[j], 10 * AvMain.hd, num, 0);
					num += hString;
				}
			}
			else
			{
				ChatPopup.paintRoundRect(g, Canvas.w - 5 * AvMain.hd - textMsg.wPopup, num - 2 * AvMain.hd, textMsg.wPopup, textMsg.text.Length * hString + 10 * AvMain.hd, 16777215, 14145495, 0);
				g.drawImage(imgArrow[0], Canvas.w - 23 * AvMain.hd, num - 3 * AvMain.hd + textMsg.text.Length * hString + 10 * AvMain.hd, MyGraphics.TOP | MyGraphics.HCENTER);
				for (int k = 0; k < textMsg.text.Length; k++)
				{
					Canvas.fontChat.drawString(g, textMsg.text[k], Canvas.w - 10 * AvMain.hd, num, 1);
					num += hString;
				}
			}
			num += hText * 2;
		}
	}

	private void paintListPlayerTab(MyGraphics g, int x0, int y0)
	{
		g.translate(0f, -cmy);
		if (listPlayer.size() > 0)
		{
			for (int i = x0; i < y0; i++)
			{
				g.setColor(16777215);
				g.fillRect(5 * AvMain.hd, (i + 1) * hText + 1, w - 10 * AvMain.hd, 1f);
				if (i == selected && !isHide)
				{
					g.fillRect(5 * AvMain.hd, i * hText, w - 10 * AvMain.hd, hText);
				}
				ElementPlayer elementPlayer = (ElementPlayer)listPlayer.elementAt(i);
				Canvas.tempFont.drawString(g, elementPlayer.name, 8 * AvMain.hd, i * hText + hText / 2 - Canvas.tempFont.getHeight() + 2 * AvMain.hd, 0);
				Canvas.fontChat.drawString(g, elementPlayer.subText, 8 * AvMain.hd, i * hText + hText / 2 - 2 * AvMain.hd, 0);
				if (elementPlayer.numSMS > 0 && indexDel == -1)
				{
					g.drawImage(imgBound, w - 15 * AvMain.hd, i * hText + hText / 2, 3);
					Canvas.menuFont.drawString(g, elementPlayer.numSMS + string.Empty, w - 15 * AvMain.hd, i * hText + hText / 2 - Canvas.menuFont.getHeight() / 2, 2);
				}
				if (i == indexDel)
				{
					imgDel.drawFrame(0, w - imgDel.frameWidth - 10 * AvMain.hd, i * hText + hText / 2 - imgDel.frameHeight / 2, 0, g);
				}
			}
		}
		g.fillRect(5 * AvMain.hd, y0 * hText, w - 10 * AvMain.hd, 1f);
	}

	private void paintPublicTab(MyGraphics g, int x0, int y0)
	{
		g.translate(0f, -cmy);
		for (int i = x0; i < y0; i++)
		{
			ElementPlayer elementPlayer = (ElementPlayer)listTextTab_1.elementAt(i);
			Canvas.tempFont.drawString(g, elementPlayer.name + ":", 5 * AvMain.hd, i * hText + hText / 2 - Canvas.tempFont.getHeight() + 4 * AvMain.hd, 0);
			Canvas.fontChat.drawString(g, elementPlayer.subText, 5 * AvMain.hd, i * hText + hText / 2, 0);
		}
	}
}
