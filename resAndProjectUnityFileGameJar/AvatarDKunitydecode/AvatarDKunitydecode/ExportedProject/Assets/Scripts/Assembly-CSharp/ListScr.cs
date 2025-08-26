using System.IO;

public class ListScr : MyScreen
{
	private class IActionURemoveFr : IAction
	{
		private int sel;

		public IActionURemoveFr(int sel)
		{
			this.sel = sel;
		}

		public void perform()
		{
			instance.isRemove = false;
			Avatar avatar = (Avatar)tempList.elementAt(sel);
			GlobalService.gI().doRemoveFriend(avatar.IDDB);
			tempList.removeElement(avatar);
			instance.init();
		}
	}

	private class IActionReadList : IAction
	{
		private readonly int idList;

		private readonly sbyte page;

		public IActionReadList(int idList, sbyte page)
		{
			this.idList = idList;
			this.page = page;
		}

		public void perform()
		{
			GlobalService.gI().doListCustom(idList, page, instance.selected, -1);
		}
	}

	private class IActionListMenu : IAction
	{
		private readonly string[] tex;

		private readonly string idType;

		private readonly sbyte[] idMe;

		private readonly int idList;

		private readonly sbyte page;

		private bool isFr;

		public IActionListMenu(string[] tex, string idType, sbyte[] idMe, int idList, sbyte page, bool isFr)
		{
			this.tex = tex;
			this.idType = idType;
			this.idMe = idMe;
			this.idList = idList;
			this.page = page;
			this.isFr = isFr;
		}

		public void perform()
		{
			MyVector myVector = new MyVector();
			if (instance.isJoinH)
			{
				instance.cmdSelected.perform();
				return;
			}
			if (isFr)
			{
				myVector.addElement(instance.cmdSelected);
			}
			for (int i = 0; i < tex.Length; i++)
			{
				int ii = i;
				myVector.addElement(new Command(tex[i], new IActionListMenu2(idList, page, idMe, ii)));
			}
			if (!instance.isAction && idType.Equals(idFriendList))
			{
				myVector.addElement(new Command(T.updateList, 3, instance));
			}
			int num = (int)((float)(instance.selected * instance.wSmall) - AvCamera.gI().yCam + (float)(instance.wSmall / 2));
			if (num + MenuSub.gI().h > Canvas.h)
			{
				num = Canvas.h - MenuSub.gI().h;
			}
			MenuSub.gI().startAt(myVector, Canvas.w / 2 - MenuSub.gI().w / 2, num);
		}
	}

	private class IActionListMenu2 : IAction
	{
		private readonly int idList;

		private readonly sbyte page;

		private readonly sbyte[] idMe;

		private readonly int ii;

		public IActionListMenu2(int idList, sbyte page, sbyte[] idMe, int ii)
		{
			this.idList = idList;
			this.page = page;
			this.idMe = idMe;
			this.ii = ii;
		}

		public void perform()
		{
			GlobalService.gI().doListCustom(idList, page, instance.selected, idMe[ii]);
		}
	}

	public static ListScr instance;

	public MyScreen backMyScreen;

	public int focus;

	public int type;

	public static MyVector tempList = new MyVector();

	public Command cmdSelected;

	public Command cmdClose;

	public static MyVector friendL;

	private int wSmall;

	public static sbyte typeListFriend = 0;

	public static sbyte countClose;

	public static bool isGetTypeHouse = false;

	public new int selected;

	public static string idFriendList = "friendlist";

	public static MyHashTable hList = new MyHashTable();

	public bool isAction;

	public bool isRemove;

	public bool isJoinH;

	private string name;

	public static Image[] imgCloseTabFull;

	public static Image[] imgCloseTab;

	private new bool isHide;

	private int xCus = -20;

	private bool transY;

	private bool isTranClose;

	public ListScr()
	{
		focus = 0;
		wSmall = 60 * AvMain.hd;
		cmdClose = new Command(T.close, 1);
	}

	public static ListScr gI()
	{
		if (instance == null)
		{
			instance = new ListScr();
		}
		return instance;
	}

	public override void switchToMe()
	{
		selected = 0;
		if (Canvas.currentMyScreen != gI())
		{
			backMyScreen = Canvas.currentMyScreen;
		}
		reSize();
		base.switchToMe();
		isHide = true;
		if (onMainMenu.isOngame)
		{
			right = cmdClose;
		}
		isJoinH = false;
	}

	public void init()
	{
		Scroll.gI().init(PaintPopup.gI().h - 5 - (PaintPopup.hTab + 2 * AvMain.hDuBox), tempList.size() * wSmall, (int)CameraList.cmy);
		if (onMainMenu.isOngame)
		{
			Canvas.cameraList.setInfo(0, 50 * AvMain.hd, Canvas.w, wSmall, Canvas.w, tempList.size() * wSmall, Canvas.w, Canvas.h - 50 * AvMain.hd - 4, tempList.size());
		}
		else
		{
			Canvas.cameraList.setInfo(4 * AvMain.hd, PaintPopup.gI().y + 40 * AvMain.hd, Canvas.w - 8 * AvMain.hd, wSmall, Canvas.w - 8 * AvMain.hd, tempList.size() * wSmall, Canvas.w - 8 * AvMain.hd, PaintPopup.gI().h - 40 * AvMain.hd - 15, tempList.size());
		}
	}

	public override void initTabTrans()
	{
		if (!onMainMenu.isOngame)
		{
			reSize();
		}
	}

	public void reSize()
	{
		if (name != null)
		{
			PaintPopup.gI().setInfo(name, Canvas.w, Canvas.hCan, 1, 0, null, null);
			PaintPopup.gI().y = 0;
			PaintPopup.gI().isFull = true;
			PaintPopup.gI().countCloseTab = -1;
			if (tempList != null)
			{
				init();
			}
		}
	}

	public override void setSelected(int se, bool isAc)
	{
		if (isAc && se == selected)
		{
			if (left != null)
			{
				left.perform();
			}
			else
			{
				cmdSelected.perform();
			}
		}
		xCus = -20;
		if (se >= 0 && se < tempList.size())
		{
			selected = se;
		}
	}

	public override void setHidePointer(bool iss)
	{
		isHide = iss;
	}

	public override void closeTabAll()
	{
		cmdSelected = null;
		right = null;
		left = null;
		tempList = null;
		if (onMainMenu.isOngame)
		{
			onMainMenu.gI().switchToMe();
			return;
		}
		Canvas.cameraList.close();
		MapScr.gI().switchToMe();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		if (!onMainMenu.isOngame)
		{
			Canvas.loadMap.paintM(g);
			Canvas.resetTrans(g);
			Canvas.paint.paintTransBack(g);
			PaintPopup.gI().paint(g);
			g.setClip(5 * AvMain.hd, 0f, Canvas.w - 10 * AvMain.hd, Canvas.h);
			g.translate(0f, Canvas.cameraList.y);
		}
		else
		{
			Canvas.resetTrans(g);
			Canvas.paint.paintDefaultBg(g);
			g.translate(0f, 0f - CameraList.cmy);
			onMainMenu.paintTitle(g, name, Canvas.w / 2, 30 * AvMain.hd);
			g.translate(0f, CameraList.cmy);
			g.translate(0f, Canvas.cameraList.y);
		}
		int num = (int)CameraList.cmy / wSmall;
		if (onMainMenu.isOngame)
		{
			num--;
		}
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num + (Canvas.h - 40) / wSmall + 2;
		if (onMainMenu.isOngame)
		{
			num2++;
		}
		if (num2 > tempList.size())
		{
			num2 = tempList.size();
		}
		if (focus == 5)
		{
			paintCustom(g, num, num2);
		}
		else if (focus == 6 || focus == 0)
		{
			paintCustomAvatar(g, num, num2);
		}
		if (onMainMenu.isOngame)
		{
			Canvas.resetTransNotZoom(g);
			Canvas.paint.paintTabSoft(g);
			Canvas.paint.paintCmdBar(g, left, center, right);
		}
		Canvas.resetTrans(g);
		if (!onMainMenu.isOngame)
		{
			g.drawImage(imgCloseTabFull[countClose / 3], Canvas.w - 25 * AvMain.hd, 35 * AvMain.hd, 3);
		}
		Canvas.paintPlus(g);
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 1:
			closeTabAll();
			break;
		case 2:
			doSendMessage();
			break;
		}
	}

	public override void doMenuTab()
	{
		if (left != null)
		{
			left.perform();
		}
	}

	private void paintCustomAvatar(MyGraphics g, int x0, int y0)
	{
		int num = 0;
		int num2 = 0;
		num += wSmall * x0;
		for (int i = x0; i < y0; i++)
		{
			if (!onMainMenu.isOngame)
			{
				g.setClip(5 * AvMain.hd, 6 * AvMain.hd, Canvas.w - 10 * AvMain.hd, PaintPopup.gI().h);
			}
			g.translate(0f, 0f - CameraList.cmy);
			Avatar avatar = (Avatar)tempList.elementAt(i);
			int num3 = 0;
			if (!isHide && i == selected)
			{
				if (onMainMenu.isOngame)
				{
					Canvas.paint.paintSelect(g, 4 * AvMain.hd, num + 2, Canvas.w - 8 * AvMain.hd, wSmall - 4);
				}
				else
				{
					g.setColor(16777215);
					g.fillRect(4 * AvMain.hd, num + 2, Canvas.w - 8 * AvMain.hd, wSmall);
				}
				int width = Canvas.fontChatB.getWidth(avatar.text2);
				if (width > PaintPopup.gI().w - (57 + (AvMain.hd - 1) * 30))
				{
					xCus += 2;
					if (xCus > width - (PaintPopup.gI().w - (57 + (AvMain.hd - 1) * 30)))
					{
						xCus = -20;
					}
				}
				num3 = xCus;
				if (xCus < 0)
				{
					num3 = 0;
				}
			}
			avatar.paintIcon(g, 45 + (AvMain.hd - 1) * 20, num + wSmall / 2 + 35 * AvMain.hd / 2, false);
			int num4 = 0;
			int num5 = num + wSmall / 2 - wSmall / 5 - AvMain.hNormal / 2;
			if (avatar.text2 != null && avatar.text2.Equals(string.Empty))
			{
				num5 = num + wSmall / 2 - AvMain.hNormal / 2;
			}
			if (avatar.idImg != -1)
			{
				num4 = 6 * AvMain.hd;
				AvatarData.paintImg(g, avatar.idImg, 60 + (AvMain.hd - 1) * 30 + num4, num5 + AvMain.hNormal / 2, 3);
			}
			if (!onMainMenu.isOngame)
			{
				g.setClip(5 * AvMain.hd, (int)CameraList.cmy + 6 * AvMain.hd, Canvas.w - 10 * AvMain.hd, PaintPopup.gI().h);
			}
			if (onMainMenu.isOngame)
			{
				Canvas.normalWhiteFont.drawString(g, avatar.name, 60 + 10 * AvMain.hd + num4 * 2 + (AvMain.hd - 1) * 30, num5 + 5 * AvMain.hd, 0);
				Canvas.fontChatB.drawString(g, avatar.text2, 60 + 10 * AvMain.hd - num3 + (AvMain.hd - 1) * 30, num + wSmall / 2 + wSmall / 5 - Canvas.normalWhiteFont.getHeight() / 2 - 2 * AvMain.hd, 0);
			}
			else
			{
				if (avatar.idWedding != -1)
				{
					AvatarData.paintImg(g, avatar.idWedding, 60 + 6 * AvMain.hd + num4 * 2 + (AvMain.hd - 1) * 30 + Canvas.normalFont.getWidth(avatar.name), num + wSmall / 2 - 12 * AvMain.hd, 3);
				}
				if (avatar.idStatus != -1)
				{
					num2 = 12 * AvMain.hd;
					AvatarData.paintImg(g, avatar.idStatus, 60 - num3 + (AvMain.hd - 1) * 30 + 6 * AvMain.hd, num + wSmall / 2 + 3 * AvMain.hd + AvMain.hBlack / 2, 3);
				}
				Canvas.normalFont.drawString(g, avatar.name, 60 + num4 * 2 + (AvMain.hd - 1) * 30, num5, 0);
				Canvas.fontChat.drawString(g, avatar.text2, 62 - num3 + (AvMain.hd - 1) * 30 + num2, num + wSmall / 2 + wSmall / 5 - Canvas.fontChat.getHeight() / 2, 0);
			}
			num += wSmall;
			g.translate(0f, CameraList.cmy);
		}
	}

	private void paintCustom(MyGraphics g, int x0, int y0)
	{
		int num = 0;
		num += wSmall * x0;
		for (int i = x0; i < y0; i++)
		{
			if (!onMainMenu.isOngame)
			{
				g.setClip(5 * AvMain.hd, 5 * AvMain.hd, Canvas.w - 10 * AvMain.hd, PaintPopup.gI().h);
			}
			g.translate(0f, 0f - CameraList.cmy);
			int num2 = 0;
			if (!isHide && i == selected)
			{
				if (onMainMenu.isOngame)
				{
					Canvas.paint.paintSelect(g, 4 * AvMain.hd, num + 2, Canvas.w - 8 * AvMain.hd, wSmall - 4);
				}
				else
				{
					g.setColor(16777215);
					g.fillRect(4 * AvMain.hd, num + 2, Canvas.w - 8 * AvMain.hd, wSmall);
				}
				num2 = xCus;
				if (xCus < 0)
				{
					num2 = 0;
				}
			}
			StringObj stringObj = (StringObj)tempList.elementAt(i);
			int num3 = AvatarData.getImgIcon((short)stringObj.dis).h + 4;
			AvatarData.paintImg(g, stringObj.dis, 30 + num3 / 2, num + wSmall / 2 - 12 * AvMain.hd + Canvas.normalWhiteFont.getHeight() / 2, 3);
			if (onMainMenu.isOngame)
			{
				Canvas.normalWhiteFont.drawString(g, stringObj.str, 30 + num3, num + wSmall / 2 - 5 * AvMain.hd, 0);
				onMainMenu.smallGrey.drawString(g, stringObj.str2, 30 - num2, num + wSmall / 2, 0);
			}
			else
			{
				Canvas.normalFont.drawString(g, stringObj.str, 30 + num3, num + wSmall / 2 - 12 * AvMain.hd, 0);
				Canvas.fontChat.drawString(g, stringObj.str2, 30 - num2, num + wSmall / 2 + 3 * AvMain.hd, 0);
			}
			num += wSmall;
			g.translate(0f, CameraList.cmy);
		}
	}

	public override void updateKey()
	{
		if (onMainMenu.isOngame)
		{
			Canvas.paint.updateKeyOn(left, center, right);
		}
		else
		{
			base.updateKey();
		}
		if (Canvas.isPointerClick && !onMainMenu.isOngame && Canvas.isPoint(Canvas.w - 45 * AvMain.hd, 15 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
		{
			isTranClose = true;
			countClose = 5;
			Canvas.isPointerClick = false;
		}
		if (isTranClose)
		{
			if (Canvas.isPointerDown && !Canvas.isPoint(Canvas.w - 45 * AvMain.hd, 15 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			if (Canvas.isPointerRelease)
			{
				isTranClose = false;
				Canvas.isPointerRelease = false;
				if (countClose != 0)
				{
					countClose = 0;
					closeTabAll();
				}
			}
		}
		PaintPopup.gI().setupdateTab();
		if (!isRemove)
		{
			return;
		}
		if (Canvas.isPointerClick && Canvas.isPoint(Canvas.cameraList.x + Canvas.cameraList.disX - 70, Canvas.cameraList.y, 70, Canvas.cameraList.disY))
		{
			Canvas.isPointerClick = false;
			transY = true;
		}
		if (transY && Canvas.isPointerRelease && Canvas.isPoint(Canvas.cameraList.x + Canvas.cameraList.disX - 70, Canvas.cameraList.y, 70, Canvas.cameraList.disY) && Math.abs(Canvas.dy()) < 5 * AvMain.hd)
		{
			int num = (int)((CameraList.cmtoY + (float)Canvas.py - (float)Canvas.cameraList.y) / (float)wSmall);
			if (num >= 0 && num < tempList.size() && Canvas.isPointer(Canvas.cameraList.x + Canvas.cameraList.disX - 70, Canvas.cameraList.y, 70, Canvas.cameraList.disY))
			{
				Canvas.msgdlg.setInfoLR(T.uRemoveFriend, new Command(T.yes, new IActionURemoveFr(num)), new Command(T.no, 0, this));
			}
			Canvas.isPointerRelease = false;
		}
	}

	public override void update()
	{
		PaintPopup.gI().update();
		backMyScreen.update();
		if (focus == 5 && selected >= 0 && selected < tempList.size())
		{
			StringObj stringObj = (StringObj)tempList.elementAt(selected);
			if (!isHide && stringObj.w > PaintPopup.gI().w - 10 * AvMain.hd)
			{
				xCus += 2;
				if (xCus > PaintPopup.gI().w)
				{
					xCus = -20;
				}
			}
		}
		Scroll.gI().updateScroll((int)CameraList.cmy, (int)CameraList.cmtoY, (int)Canvas.cameraList.vY);
	}

	public void onList(int type, MyVector list, MyScreen backMyScreen)
	{
		if (Canvas.currentMyScreen != gI())
		{
			this.backMyScreen = backMyScreen;
		}
		switch (focus)
		{
		case 0:
			isGetTypeHouse = true;
			friendL = list;
			if (typeListFriend == 1)
			{
				MapScr.gI().doRequestAddFriend(MapScr.focusP);
			}
			else if (typeListFriend == 2)
			{
				isGetTypeHouse = false;
				Canvas.startWaitDlg();
				AvatarService.gI().getTypeHouse(1);
			}
			else if (Canvas.currentMyScreen != this)
			{
				switchToMe();
			}
			typeListFriend = 0;
			break;
		}
		tempList = null;
		tempList = list;
		if (focus != 5)
		{
			for (int i = 0; i < tempList.size(); i++)
			{
				Avatar avatar = (Avatar)tempList.elementAt(i);
				avatar.initPet();
				avatar.orderSeriesPath();
			}
		}
		this.type = type;
		selected = 0;
		setCam();
	}

	public void setCam()
	{
		init();
	}

	public void setFriendList(bool isJoinFarm)
	{
		focus = 0;
		if (friendL == null)
		{
			Canvas.startWaitDlg();
			CasinoService.gI().requestFriendList();
		}
		else
		{
			backMyScreen = Canvas.currentMyScreen;
			setList(idFriendList);
			switchToMe();
		}
		if (isJoinFarm)
		{
			isAction = true;
			Out.println("setFriendList");
			cmdSelected = new Command(T.selectt, 1, this);
		}
	}

	public static Avatar getAvatar(int id)
	{
		int num = friendL.size();
		for (int i = 0; i < num; i++)
		{
			Avatar avatar = (Avatar)friendL.elementAt(i);
			if (avatar.IDDB == id)
			{
				return avatar;
			}
		}
		return null;
	}

	public bool setList(string idType)
	{
		sbyte[] array = (sbyte[])hList.get(idType);
		Canvas.endDlg();
		if (array == null)
		{
			return false;
		}
		readList(array, idType);
		return true;
	}

	public void readList(sbyte[] data, string idType)
	{
		string[] array = null;
		sbyte[] array2 = null;
		DataInputStream dataInputStream = new DataInputStream(data);
		try
		{
			string text = dataInputStream.readUTF();
			int idList = dataInputStream.readInt();
			sbyte b = dataInputStream.readByte();
			sbyte page = dataInputStream.readByte();
			short num = dataInputStream.readShort();
			MyVector myVector = new MyVector();
			if (b == 0)
			{
				focus = 5;
				for (int i = 0; i < num; i++)
				{
					StringObj stringObj = new StringObj();
					stringObj.dis = dataInputStream.readShort();
					stringObj.str = dataInputStream.readUTF();
					stringObj.str2 = dataInputStream.readUTF();
					stringObj.w = (short)Canvas.fontChatB.getWidth(stringObj.str2);
					if (stringObj.w > Canvas.w)
					{
						stringObj.w = (short)Canvas.w;
					}
					myVector.addElement(stringObj);
				}
			}
			else
			{
				focus = 6;
				for (int j = 0; j < num; j++)
				{
					Avatar avatar = new Avatar();
					avatar.direct = 0;
					sbyte b2 = dataInputStream.readByte();
					avatar.seriPart = new MyVector();
					for (int k = 0; k < b2; k++)
					{
						avatar.addSeri(new SeriPart(dataInputStream.readShort()));
					}
					avatar.IDDB = dataInputStream.readInt();
					avatar.idImg = dataInputStream.readShort();
					if (idType.Equals(idFriendList))
					{
						avatar.idWedding = dataInputStream.readShort();
						avatar.idStatus = dataInputStream.readShort();
					}
					avatar.name = dataInputStream.readUTF();
					avatar.text2 = dataInputStream.readUTF();
					myVector.addElement(avatar);
				}
			}
			int num2 = dataInputStream.readByte();
			if (num2 > 0)
			{
				array = new string[num2];
				array2 = new sbyte[num2];
				for (int l = 0; l < num2; l++)
				{
					array2[l] = dataInputStream.readByte();
					array[l] = dataInputStream.readUTF();
				}
			}
			if (idType.Equals(idFriendList))
			{
				focus = 0;
			}
			gI().onList(focus, myVector, Canvas.currentMyScreen);
			name = text;
			switchToMe();
			string[] tex = array;
			sbyte[] idMe = array2;
			left = null;
			if (num2 > 0)
			{
				if (isAction)
				{
					left = cmdSelected;
				}
				else
				{
					left = new Command(T.menu, new IActionListMenu(tex, idType, idMe, idList, page, idType.Equals(idFriendList)));
				}
			}
			if (!isAction)
			{
				if (idType.Equals(idFriendList))
				{
					cmdSelected = new Command(T.sendMessage, 2);
				}
				else if (!isAction)
				{
					cmdSelected = new Command(T.selectt, new IActionReadList(idList, page));
				}
			}
			isAction = false;
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			isRemove = false;
			break;
		case 1:
			Canvas.startWaitDlg();
			FarmScr.gI().doJoinFarm(((Avatar)friendL.elementAt(instance.selected)).IDDB, true);
			break;
		case 2:
			isRemove = true;
			break;
		case 3:
			CasinoService.gI().requestFriendList();
			break;
		}
	}

	protected void doSendMessage()
	{
		if (selected >= 0 && selected < tempList.size())
		{
			Avatar p = (Avatar)tempList.elementAt(selected);
			MessageScr.gI().startChat(p);
		}
	}

	public void removeList()
	{
		hList.remove(idFriendList);
		friendL = null;
	}
}
