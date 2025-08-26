using System;

public class MoneyScr : MyScreen
{
	private class IActionInputMoney : IKbAction
	{
		private readonly MoneyScr p;

		public IActionInputMoney(MoneyScr p)
		{
			this.p = p;
		}

		public void perform(string text)
		{
			try
			{
				if (!text.Equals(string.Empty))
				{
					Canvas.endDlg();
					int money = int.Parse(text);
					FarmService.gI().doTransMoney(money, (p.selected == 0) ? 1 : 0);
					Canvas.startWaitDlg();
				}
			}
			catch (Exception e)
			{
				Out.logError(e);
			}
		}
	}

	private class IActionDoBuy1 : IAction
	{
		private readonly string link;

		public IActionDoBuy1(string link)
		{
			this.link = link;
		}

		public void perform()
		{
			GameMidlet.flatForm(link);
		}
	}

	private class IActionDoBuy2 : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.sendSmgFinish);
		}
	}

	private class IActionDoBuy3 : IAction
	{
		private readonly MoneyInfo mi;

		public IActionDoBuy3(MoneyInfo mi)
		{
			this.mi = mi;
		}

		public void perform()
		{
			Canvas.startOKDlg(T.notSendSmg + mi.smsContent + GameMidlet.avatar.name.ToUpper() + T.sendTo + mi.smsTo);
		}
	}

	private class IActionClose : IAction
	{
		private MoneyScr me;

		public IActionClose(MoneyScr me)
		{
			this.me = me;
		}

		public void perform()
		{
			me.init();
			InputFace.gI().close();
			TField.close();
		}
	}

	private class IActionLoad : IAction
	{
		private string link;

		private TField[] tf;

		private MoneyScr me;

		public IActionLoad(string link, TField[] tf, MoneyScr me)
		{
			this.link = link;
			this.tf = tf;
			this.me = me;
		}

		public void perform()
		{
			me.init();
			me.sendLoadCard(link, tf[0].getText(), tf[1].getText());
		}
	}

	private class IActionDoLoadCard1 : IAction
	{
		private readonly MoneyScr p;

		public IActionDoLoadCard1(MoneyScr p)
		{
			this.p = p;
		}

		public void perform()
		{
			p.doCloseLoadCard();
		}
	}

	public static MoneyScr instance;

	public string[][] itemID = new string[2][]
	{
		new string[6] { "099X", "299X", "499X", "099L", "299L", "499L" },
		new string[6] { "Buy 24000 Coins ($0.99)", "Buy 84000 Coins ($2.99)", "Buy 150000 Coins ($4.99)", "Buy 24 Gold ($0.99)", "Buy 84 Gold ($2.99)", "Buy 150 Gold ($4.99)" }
	};

	private MyVector avs;

	public int type;

	public int max;

	public int focusTap;

	private MyScreen backScr;

	private Command cmdTrans;

	private Command cmdLoad;

	private Command cmdClose;

	private bool isLoadCard;

	private TField tfSeri;

	private TField tfPassCard;

	private Image imgSell;

	private int x;

	private int y;

	private int w;

	private int h;

	private new int hSmall;

	private sbyte countClose;

	private AvPosition pTrans = new AvPosition(0, 1);

	private int xTrans;

	private int dir = -1;

	public MoneyScr()
	{
		cmdTrans = new Command(T.loadMoney, 0);
		cmdLoad = new Command(T.selectt, 0);
		cmdClose = new Command(T.close, 1);
	}

	public static MoneyScr gI()
	{
		if (instance == null)
		{
			instance = new MoneyScr();
		}
		return instance;
	}

	public void switchToMe(MyScreen scr)
	{
		init();
		focusTap = 0;
		selected = 0;
		backScr = scr;
		isLoadCard = false;
		isHide = true;
		if (onMainMenu.isOngame)
		{
			right = cmdClose;
		}
		else
		{
			left = (right = (center = null));
		}
		base.switchToMe();
	}

	public void init()
	{
		if (imgSell == null)
		{
			imgSell = Image.createImagePNG(T.getPath() + "/farm/coin");
		}
		string empty = string.Empty;
		string[] array = new string[2];
		if (LoadMap.TYPEMAP == 25)
		{
			type = 1;
			max = 2;
			empty = T.strName[1];
			array[0] = T.strName[1];
			array[1] = T.strName[2];
			FarmService.gI().doTransMoney(0, 0);
			Canvas.startWaitDlg();
		}
		else
		{
			empty = T.strName[0];
			type = 0;
			array[0] = T.strName[0];
			array[1] = T.strName[2];
		}
		PaintPopup.gI().setInfo(empty, w, h, 2, countClose, array, null);
		if (onMainMenu.isOngame)
		{
			PaintPopup.gI().y = 25 + MyScreen.ITEM_HEIGHT + 1;
		}
		y = PaintPopup.gI().y;
		initPos();
		setCamera();
		initSize();
	}

	public void initSize()
	{
		if (tfSeri != null)
		{
			tfSeri.x = Canvas.hw - 70 + 20;
			tfSeri.y = Canvas.hh - 60 + 40;
			tfPassCard.x = Canvas.hw - 70 + 20;
			tfPassCard.y = Canvas.hh - 60 + 85;
		}
	}

	public override void setSelected(int se, bool isAc)
	{
		if (isAc && selected == se && PaintPopup.gI().focusTab == 0)
		{
			doCenter();
		}
		base.setSelected(se, isAc);
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			if (type == 0)
			{
				doBuy();
			}
			else
			{
				doInputMoney();
			}
			break;
		case 1:
			Canvas.cameraList.close();
			backScr.switchToMe();
			imgSell = null;
			break;
		}
	}

	public override void closeTabAll()
	{
		Canvas.cameraList.close();
		backScr.switchToMe();
		imgSell = null;
	}

	protected void doInputMoney()
	{
		ipKeyboard.openKeyBoard(T.number, ipKeyboard.NUMBERIC, string.Empty, new IActionInputMoney(this), false);
	}

	protected void doBuy()
	{
		MoneyInfo moneyInfo = (MoneyInfo)avs.elementAt(selected);
		if (moneyInfo.smsContent.IndexOf(T.link) != -1)
		{
			string link = Canvas.normalFont.replace(moneyInfo.smsContent, T.replaceNam, GameMidlet.avatar.name);
			Canvas.startOKDlg(T.doYouWantExitIntoRegion, new IActionDoBuy1(link));
		}
		else if (moneyInfo.smsContent.IndexOf("napthe:") != -1)
		{
			string searchStr = moneyInfo.smsContent.Substring(0, moneyInfo.smsContent.IndexOf("napthe:") + "napthe:".Length);
			string link2 = Canvas.normalFont.replace(moneyInfo.smsContent, searchStr, string.Empty);
			doLoadCard(link2, moneyInfo.info);
		}
		else if (moneyInfo.smsContent.IndexOf("ServerNap:") != -1)
		{
			string searchStr2 = moneyInfo.smsContent.Substring(0, moneyInfo.smsContent.IndexOf("ServerNap:") + "ServerNap:".Length);
			string link3 = Canvas.normalFont.replace(moneyInfo.smsContent, searchStr2, string.Empty);
			AvatarService.gI().doSMSServerLoad(link3);
			Canvas.startWaitDlg();
		}
		else if (moneyInfo.smsTo == "appstore")
		{
			if (moneyInfo.smsContent == "099X")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][0], GameMidlet.avatar.name, GameMidlet.gameID);
			}
			if (moneyInfo.smsContent == "299X")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][1], GameMidlet.avatar.name, GameMidlet.gameID);
			}
			if (moneyInfo.smsContent == "499X")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][2], GameMidlet.avatar.name, GameMidlet.gameID);
			}
			if (moneyInfo.smsContent == "099L")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][3], GameMidlet.avatar.name, GameMidlet.gameID);
			}
			if (moneyInfo.smsContent == "299L")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][4], GameMidlet.avatar.name, GameMidlet.gameID);
			}
			if (moneyInfo.smsContent == "499L")
			{
				iOSPlugins.purchaseItem("com.TeaM.Avatar." + itemID[0][5], GameMidlet.avatar.name, GameMidlet.gameID);
			}
		}
		else
		{
			Canvas.startWaitDlg();
			GlobalService.gI().doRequestMoneyLoad(moneyInfo.strID);
			cmdClose.perform();
		}
	}

	private void doLoadCard(string link, string info)
	{
		TField[] array = new TField[2];
		array[0] = new TField(string.Empty, Canvas.currentMyScreen, new IActionLoad(link, array, instance));
		array[1] = new TField(string.Empty, Canvas.currentMyScreen, new IActionLoad(link, array, instance));
		array[0].setIputType(ipKeyboard.TEXT);
		array[1].setIputType(ipKeyboard.TEXT);
		InputFace.gI().setInfo(array, info, T.loadCard, new Command(T.finish, new IActionLoad(link, array, instance)), Canvas.hCan);
		InputFace.gI().iAcClose = new IActionClose(instance);
		InputFace.gI().show();
	}

	public void doCenter()
	{
		if (type == 0)
		{
			doBuy();
		}
		else
		{
			doInputMoney();
		}
	}

	public void initCanvas()
	{
		initPos();
		init();
	}

	protected void sendLoadCard(string link, string seri, string pass)
	{
		if (seri.Equals(string.Empty))
		{
			Canvas.startOKDlg(T.enterCard[0]);
			return;
		}
		if (pass.Equals(string.Empty))
		{
			Canvas.startOKDlg(T.enterCard[1]);
			return;
		}
		GlobalService.gI().doLoadCard(link, seri, pass);
		doCenter();
		Canvas.startWaitDlg();
	}

	protected void doCloseLoadCard()
	{
		isLoadCard = false;
		tfSeri = null;
		tfPassCard = null;
		init();
		right = cmdClose;
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		if (onMainMenu.isOngame)
		{
			Canvas.paint.paintDefaultBg(g);
			Canvas.paint.paintDefaultScrList(g, T.loadMoney.ToUpper(), GameMidlet.avatar.money[0] + T.xu, GameMidlet.avatar.money[2] + T.gold);
		}
		else if (backScr != null)
		{
			backScr.paintMain(g);
		}
		if (InputFace.me != null && Canvas.currentFace == InputFace.me)
		{
			return;
		}
		if (!onMainMenu.isOngame)
		{
			PaintPopup.gI().paint(g);
			g.translate(0f, y + PaintPopup.hTab + AvMain.hDuBox);
			g.setClip(x + 5, 0f, w - 10, PaintPopup.gI().h - PaintPopup.hTab - 2 * AvMain.hDuBox);
		}
		else
		{
			g.translate(0f, y);
			g.setClip(x + 5, 0f, w - 10, h);
		}
		if (focusTap == 1)
		{
			int num = (h - PaintPopup.hTab + AvMain.hDuBox * 2) / 6;
			Canvas.tempFont.drawString(g, T.nameStr + GameMidlet.avatar.name, x + w / 2, num / 2, 2);
			Canvas.paint.paintMoney(g, x + w / 2 - (60 + Canvas.tempFont.getWidth(GameMidlet.avatar.money[0] + string.Empty + 5)), num / 2 + num);
			if (GameMidlet.avatar.money[1] != -1)
			{
				Canvas.tempFont.drawString(g, MapScr.strTkFarm(), x + w / 2, num / 2 + num * 2, 2);
			}
		}
		else
		{
			g.translate(0f, 0f - CameraList.cmy);
			if (type == 0)
			{
				paintRichList(g);
			}
			else
			{
				paintTransMoney(g);
			}
		}
		base.paint(g);
		Canvas.paintPlus(g);
	}

	private void paintLoadCard(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.translate(x, y);
		Canvas.resetTrans(g);
		Canvas.normalFont.drawString(g, T.enterCard[2], tfSeri.x - 10 * AvMain.hd, tfSeri.y - Canvas.normalFont.getHeight() - 2, 0);
		Canvas.normalFont.drawString(g, T.enterCard[3], tfPassCard.x - 10 * AvMain.hd, tfPassCard.y - Canvas.normalFont.getHeight() - 2, 0);
		tfSeri.paint(g);
		tfPassCard.paint(g);
	}

	public void setAvatarList(MyVector avatarList)
	{
		initPos();
		avs = new MyVector();
		for (int i = 0; i < avatarList.size(); i++)
		{
			MoneyInfo moneyInfo = (MoneyInfo)avatarList.elementAt(i);
			if (moneyInfo.smsContent.IndexOf(T.link) != -1 || moneyInfo.smsContent.IndexOf("napthe:") != -1 || moneyInfo.smsContent.IndexOf("ServerNap:") != -1)
			{
				avs.addElement(moneyInfo);
			}
		}
		setCamera();
		xTrans = 0;
	}

	private void setCamera()
	{
		if (avs != null)
		{
			int num = avs.size();
			int num2 = avs.size() * hSmall;
			int size = avs.size();
			if (LoadMap.TYPEMAP == 25)
			{
				num2 = hSmall * 2;
				size = 2;
			}
			Canvas.cameraList.setInfo(x, y + ((!onMainMenu.isOngame) ? (PaintPopup.hTab + AvMain.hDuBox) : 0), w, hSmall, w, num2, w, h - (PaintPopup.hTab + 2 * AvMain.hDuBox) - AvMain.hDuBox, size);
			max = num;
		}
	}

	private void initPos()
	{
		if (onMainMenu.isOngame)
		{
			w = Canvas.w + 8;
			h = Canvas.h - 25 - MyScreen.ITEM_HEIGHT + AvMain.hDuBox * 2;
		}
		else
		{
			w = LoginScr.gI().wLogin;
			h = 8 * MyScreen.hText;
			if (h > Canvas.h - MyScreen.hText)
			{
				h = Canvas.h - MyScreen.hText;
			}
		}
		hSmall = MyScreen.hText;
		x = Canvas.hw - w / 2;
		setCamera();
	}

	private void paintTransMoney(MyGraphics g)
	{
		for (int i = 0; i < 2; i++)
		{
			if (!isHide && i == selected)
			{
				Canvas.paint.paintSelected_2(g, x + 3 * AvMain.hd, i * hSmall + 10, w - 6 * AvMain.hd, hSmall);
			}
			Canvas.normalFont.drawString(g, T.strTransMoney[i], x + 10 * AvMain.hd + ((selected == i) ? xTrans : 0), i * hSmall + 10 + hSmall / 2 - Canvas.normalFont.getHeight() / 2, 0);
		}
	}

	private void paintRichList(MyGraphics g)
	{
		int num = imgSell.getWidth() + 14;
		int num2 = avs.size();
		for (int i = 0; i < num2; i++)
		{
			if (i == selected && !isHide)
			{
				if (onMainMenu.isOngame)
				{
					g.setColor(14328855);
					g.fillRect(x, i * hSmall, w - 3 * AvMain.hd, hSmall);
				}
				else
				{
					g.setColor(16777215);
					g.fillRect(x + 6, i * hSmall, w - 6 * AvMain.hd, hSmall);
				}
			}
			g.drawImage(imgSell, x + num / 2 + 4 * AvMain.hd, i * hSmall + hSmall / 2, 3);
		}
		for (int j = 0; j < num2; j++)
		{
			MoneyInfo moneyInfo = (MoneyInfo)avs.elementAt(j);
			g.setClip(x + 4 * AvMain.hd + num - 3, (int)CameraList.cmy, w - num - 2 - 4 * AvMain.hd, h - ((!onMainMenu.isOngame) ? (PaintPopup.hTab + 2 * AvMain.hDuBox) : 0));
			Canvas.normalFont.drawString(g, moneyInfo.info, x + num + 4 * AvMain.hd, j * hSmall + hSmall / 2 - AvMain.hNormal / 2, 0);
		}
	}

	private void setTab(int dir)
	{
		focusTap += dir;
		string empty = string.Empty;
		if (type == 0)
		{
			if (focusTap == 0)
			{
				empty = T.strName[0];
				left = null;
			}
			else
			{
				empty = T.strName[2];
			}
		}
		else if (focusTap == 0)
		{
			empty = T.strName[1];
			left = null;
		}
		else
		{
			empty = T.strName[2];
		}
		PaintPopup.gI().setNameAndFocus(empty, focusTap);
	}

	public override void updateKey()
	{
		base.updateKey();
		if (!onMainMenu.isOngame && !isLoadCard)
		{
			int num = PaintPopup.gI().setupdateTab();
			if (num != 0)
			{
				setTab(num);
				Canvas.isPointerClick = false;
			}
		}
	}

	private void setTap()
	{
		string empty = string.Empty;
		if (focusTap == 0)
		{
			focusTap = 1;
			left = null;
			empty = T.strName[2];
		}
		else
		{
			empty = ((type != 1) ? T.strName[0] : T.strName[1]);
			focusTap = 0;
		}
		PaintPopup.gI().setNameAndFocus(empty, focusTap);
	}

	public override void keyPress(int keyCode)
	{
		if (isLoadCard)
		{
			if (tfSeri.isFocused())
			{
				tfSeri.keyPressed(keyCode);
			}
			else if (tfPassCard.isFocused())
			{
				tfPassCard.keyPressed(keyCode);
			}
		}
	}

	public override void update()
	{
		PaintPopup.gI().update();
		if (backScr != null)
		{
			backScr.update();
		}
		if (isLoadCard && PaintPopup.gI().focusTab == 0)
		{
			tfSeri.update();
			tfPassCard.update();
		}
		int num = 0;
		if (type == 0)
		{
			MoneyInfo moneyInfo = (MoneyInfo)avs.elementAt(selected);
			num = Canvas.normalFont.getWidth(moneyInfo.info);
		}
		else
		{
			num = Canvas.normalFont.getWidth(T.strTransMoney[selected]);
		}
		if (num > w - 20)
		{
			xTrans += dir;
			if (xTrans <= -(num - (w - 30)))
			{
				dir = 1;
			}
			if (xTrans > 0)
			{
				dir = -1;
			}
		}
		else
		{
			xTrans = 0;
		}
		if (isLoadCard)
		{
			return;
		}
		if (focusTap == 0)
		{
			if (LoadMap.TYPEMAP != 25)
			{
				center = null;
			}
			else
			{
				left = null;
			}
		}
		else
		{
			left = null;
			center = null;
		}
	}
}
