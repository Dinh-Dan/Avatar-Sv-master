using System;
using UnityEngine;

public class MainMenu : MyScreen
{
	private class IActionUpdateContainerYesNo : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.doYouWantUpgradeCoffer, new IActionUpdateContainer());
		}
	}

	private class IActionUpdateContainer : IAction
	{
		public void perform()
		{
			GlobalService.gI().doUpdateContainer(0);
			Canvas.startWaitDlg();
		}
	}

	private class IActionOkWedding : IAction
	{
		public void perform()
		{
			ParkService.gI().doRequestWedding(MapScr.roomID, MapScr.boardID);
			Canvas.startWaitDlg();
		}
	}

	private class IActionExchange : IAction
	{
		private StringObj str;

		public IActionExchange(StringObj strObj)
		{
			str = strObj;
		}

		public void perform()
		{
			GlobalService.gI().doRequestCmdRotate(str.anthor, (MapScr.focusP == null) ? (-1) : MapScr.focusP.IDDB);
		}
	}

	private class CommandExchange : Command
	{
		private StringObj str;

		private sbyte count;

		public CommandExchange(string name, IActionExchange ac, StringObj strObj)
			: base(name, ac)
		{
			str = strObj;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			if (AvatarData.getImgIcon((short)str.dis).count != -1)
			{
				AvatarData.paintImg(g, str.dis, x, y, 3);
			}
			else
			{
				imgLoading.drawFrame(count / 3, x, y, 0, 3, g);
			}
		}

		public override void update()
		{
			count++;
			if (count >= 9)
			{
				count = 0;
			}
		}
	}

	private class CommandMap : Command
	{
		private StringObj str;

		private sbyte count;

		public CommandMap(string name, IActionMap ac, StringObj str)
			: base(name, ac)
		{
			this.str = str;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			if (AvatarData.getImgIcon((short)str.dis).count != -1)
			{
				AvatarData.paintImg(g, str.dis, x, y, 3);
			}
			else
			{
				imgLoading.drawFrame(count / 3, x, y, 0, 3, g);
			}
		}

		public override void update()
		{
			count++;
			if (count >= 9)
			{
				count = 0;
			}
		}
	}

	private class IActionMap : IAction
	{
		private int ii;

		private int type;

		private readonly StringObj str;

		public IActionMap(int i, int type, StringObj str)
		{
			ii = i;
			this.str = str;
			this.type = type;
		}

		public void perform()
		{
			if (type == 0)
			{
				GlobalService.gI().doCommunicate(ii);
			}
			else
			{
				GlobalService.gI().doRequestCmdRotate(str.anthor, (MapScr.focusP == null) ? (-1) : MapScr.focusP.IDDB);
			}
		}
	}

	private class CommandMenu : Command
	{
		private int index;

		public CommandMenu(string name, int type, int index)
			: base(name, type)
		{
			this.index = index;
		}

		public CommandMenu(string name, IAction ac, int index)
			: base(name, ac)
		{
			this.index = index;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			int idx = 0;
			int idy = index;
			if (index >= Menu.imgSellect.nFrame)
			{
				idx = 1;
				idy = index % Menu.imgSellect.nFrame;
			}
			Menu.imgSellect.drawFrameXY(idx, idy, x, y, 3, g);
		}
	}

	private class IActionMenu : IAction
	{
		private IAction ac;

		public IActionMenu(IAction ac)
		{
			this.ac = ac;
		}

		public void perform()
		{
			ac.perform();
		}
	}

	public static MainMenu me;

	public new int selected;

	public int x;

	public int y;

	public int wSmall;

	public int wGo;

	public int disSmall;

	public int numW;

	public int dis;

	public int angleNormal;

	public int disTran;

	public bool isGO;

	public bool isName;

	public bool isAble;

	private MyVector list;

	public static FrameImage imgIconFlower;

	public static FrameImage imgGO;

	private MyScreen lastScr;

	private Command cmdCenter;

	public bool isCircle;

	public new bool isHide;

	private int angleCircle;

	private int wCircle;

	private int maxW = 50 * AvMain.hd;

	private MyVector listObj;

	public AvPosition avaPaint;

	public static PopupName popFocus;

	public int cmtoX;

	public int cmx;

	public int cmdx;

	public int cmvx;

	public int cmxLim;

	public int disX;

	public bool isWearing;

	private static FrameImage imgLoading;

	private int angle = 45;

	private int xCenter;

	private int yCenter;

	private int maxRadius;

	private float distant;

	private float v = 5f;

	public int g;

	private bool trans;

	private bool isClick;

	private int dxTran;

	private int timeOpen;

	private long timeDelay;

	private long count;

	private long timePoint;

	private int vX;

	private int indexCircle = -1;

	private int indexTemp = -1;

	public MainMenu()
	{
		cmdCenter = new Command(T.selectt, 1);
		wGo = 23;
		if (Canvas.stypeInt > 0)
		{
			wGo = 60 * Canvas.stypeInt;
		}
		imgLoading = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/loading"), 24 * AvMain.hd, 24 * AvMain.hd);
	}

	public static MainMenu gI()
	{
		return (me != null) ? me : (me = new MainMenu());
	}

	public override void switchToMe()
	{
		if (Canvas.currentMyScreen != this)
		{
			lastScr = Canvas.currentMyScreen;
		}
		base.switchToMe();
		isHide = true;
	}

	public void initCmd()
	{
		isAble = true;
		cmdCenter = new Command(T.selectt, 1);
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			closeGo();
			break;
		case 1:
		{
			closeGo();
			Command command = (Command)list.elementAt(selected);
			if (command.action != null)
			{
				command.action.perform();
			}
			else
			{
				commandAction(command.indexMenu);
			}
			break;
		}
		}
	}

	public override void commandAction(int index)
	{
		switch (index)
		{
		case 3:
			doExchange();
			break;
		case 6:
			isWearing = false;
			GlobalService.gI().doRequestContainer(GameMidlet.avatar.IDDB);
			break;
		case 7:
			MapScr.gI().doRequestAddFriend(MapScr.focusP);
			break;
		case 8:
			GlobalService.gI().requestShop(26);
			Canvas.startWaitDlg();
			break;
		case 9:
			MapScr.gI().doHit();
			break;
		case 10:
			MapScr.gI().doKiss();
			break;
		case 11:
			MapScr.gI().doRequestYourInfo();
			break;
		case 12:
			if (MapScr.focusP != null)
			{
				MessageScr.gI().startChat(MapScr.focusP);
			}
			break;
		case 13:
			MapScr.gI().doInviteToMyHome();
			break;
		case 16:
		{
			MyVector myVector = new MyVector();
			if (MapScr.listCmdRotate.size() > 0)
			{
				for (int i = 0; i < MapScr.listCmdRotate.size(); i++)
				{
					StringObj stringObj = (StringObj)MapScr.listCmdRotate.elementAt(i);
					if (stringObj.type == 1)
					{
						myVector.addElement(new CommandExchange(stringObj.str, new IActionExchange(stringObj), stringObj));
					}
				}
			}
			setInfo(myVector);
			break;
		}
		case 17:
			MapScr.isOpenInfo = true;
			MapScr.gI().doRequestYourInfo();
			break;
		case 4:
		case 5:
		case 14:
		case 15:
			break;
		}
	}

	public void doWearing()
	{
		if (Canvas.currentMyScreen != me)
		{
			PopupShop.gI().isFull = true;
			PopupShop.gI().addElement(new string[2]
			{
				T.wearing,
				T.container
			}, new MyVector[2]
			{
				MapScr.gI().getListYourPart(GameMidlet.avatar, 0, true),
				MapScr.gI().getListCmdDoUsing(GameMidlet.listContainer, GameMidlet.avatar.IDDB, 1, T.use, true)
			}, null, null);
			PopupShop.gI().setCmdLeft(MapScr.gI().cmdDellPart(GameMidlet.avatar.seriPart, 0, 0, false), 0);
			PopupShop.gI().setCmdLeft(MapScr.gI().cmdDellPart(GameMidlet.listContainer, 1, 0, true), 1);
			PopupShop.gI().setCmdRight(new Command(T.update, new IActionUpdateContainerYesNo()), 1);
			PopupShop.gI().setTap(1);
			if (Canvas.currentMyScreen != PopupShop.gI())
			{
				PopupShop.gI().switchToMe();
				PopupShop.gI().setHorizontal(true);
				PopupShop.isQuaTrang = true;
				PopupShop.gI().setCmyLim();
			}
		}
	}

	public void closeGo()
	{
		lastScr.switchToMe();
		if (isCircle)
		{
			for (int i = 0; i < listObj.size(); i++)
			{
				AvPosition avPosition = (AvPosition)listObj.elementAt(i);
				Avatar avatar = LoadMap.getAvatar(avPosition.anchor);
				if (avatar != null)
				{
					avatar.ableShow = false;
				}
			}
			listObj = null;
		}
		else if (MapScr.focusP != null)
		{
			MapScr.focusP.ableShow = false;
		}
		indexCircle = -1;
		indexTemp = -1;
		isCircle = false;
		popFocus = null;
		center = null;
		trans = false;
		isTran = false;
	}

	public void setInfo(MyVector list)
	{
		this.list = list;
		wSmall = 40 * AvMain.hd + (AvMain.hd - 1) * 30;
		if (Canvas.stypeInt == 1 && Canvas.w > 300)
		{
			wSmall += 20;
		}
		disSmall = wSmall + 2 * AvMain.hd;
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			int num = Canvas.smallFontRed.getWidth(command.caption) + 10;
			if (num > disSmall)
			{
				disSmall = num;
			}
		}
		x = 0;
		numW = Canvas.w / disSmall;
		if (list.size() * disSmall < Canvas.w)
		{
			x = (Canvas.w - list.size() * disSmall) / 2;
		}
		else
		{
			x = 5;
		}
		if (selected >= list.size())
		{
			selected = 0;
		}
		isCircle = false;
		if (MapScr.focusP != null)
		{
			MapScr.focusP.ableShow = true;
		}
		y = Canvas.hCan - 30 * AvMain.hd - disSmall / 2;
		if (avaPaint != null && LoadMap.focusObj != null)
		{
			xCenter = (int)((float)(LoadMap.focusObj.x * AvMain.hd) - AvCamera.gI().xCam);
			yCenter = (int)((float)(LoadMap.focusObj.y * AvMain.hd) - AvCamera.gI().yCam);
		}
		else
		{
			xCenter = (int)((float)(GameMidlet.avatar.x * AvMain.hd) - AvCamera.gI().xCam);
			yCenter = (int)((float)(GameMidlet.avatar.y * AvMain.hd) - AvCamera.gI().yCam);
		}
		maxRadius = 60 * AvMain.hd;
		if (AvMain.zoom > 1f)
		{
			maxRadius += (int)(30f * AvMain.zoom);
		}
		if ((float)yCenter * AvMain.zoom > (float)(Canvas.hCan - maxRadius - 25 * AvMain.hd))
		{
			yCenter = (int)((float)(Canvas.hCan - maxRadius - 25 * AvMain.hd) / AvMain.zoom);
		}
		if ((float)xCenter * AvMain.zoom < (float)(maxRadius + 25 * AvMain.hd))
		{
			xCenter = (int)((float)(maxRadius + 25 * AvMain.hd) / AvMain.zoom);
		}
		else if ((float)xCenter * AvMain.zoom > (float)(Canvas.w - maxRadius - 25 * AvMain.hd))
		{
			xCenter = (int)((float)(Canvas.w - maxRadius - 25 * AvMain.hd) / AvMain.zoom);
		}
		distant = 20 * AvMain.hd;
		v = 2f;
		Canvas.isPointerClick = false;
		Canvas.isPointerRelease = false;
		Canvas.isPointerDown = false;
		switchToMe();
		if (Canvas.stypeInt == 0)
		{
			center = cmdCenter;
		}
		isName = false;
		disX = Canvas.w - x * 2;
		cmxLim = list.size() * disSmall - disX;
	}

	public override void update()
	{
		try
		{
			lastScr.update();
			if (timeOpen > 0)
			{
				timeOpen--;
				if (timeOpen == 0)
				{
					click();
				}
			}
			if (!isGO)
			{
				if (isCircle)
				{
					for (int i = 0; i < listObj.size(); i++)
					{
						AvPosition avPosition = (AvPosition)listObj.elementAt(i);
						int num = maxW * CRes.cos(CRes.fixangle(angleCircle * i)) >> 10;
						int num2 = -(maxW * CRes.sin(CRes.fixangle(angleCircle * i))) >> 10;
						int num3 = x + num;
						int num4 = y + num2;
						if (CRes.distance(avPosition.x, avPosition.y, num3, num4) > 10 * AvMain.hd)
						{
							int num5 = CRes.angle(num3 - avPosition.x, -(num4 - avPosition.y));
							int num6 = 10 * AvMain.hd * CRes.cos(CRes.fixangle(num5)) >> 10;
							int num7 = -(10 * AvMain.hd * CRes.sin(CRes.fixangle(num5))) >> 10;
							avPosition.x += num6;
							avPosition.y += num7;
						}
						else
						{
							avPosition.x = num3;
							avPosition.y = num4;
						}
					}
					if (wCircle < maxW)
					{
						wCircle += 10 * AvMain.hd;
					}
				}
				else
				{
					if (distant < (float)maxRadius)
					{
						v += v / 2f;
						distant += v;
					}
					for (int j = 0; j < list.size(); j++)
					{
						Command command = (Command)list.elementAt(j);
						command.update();
					}
				}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		moveCamera();
	}

	public void moveCamera()
	{
		if (vX != 0)
		{
			if (cmx < 0 || cmx > cmxLim)
			{
				vX -= vX / 4;
				cmx += vX / 20;
				if (vX / 10 <= 1)
				{
					vX = 0;
				}
			}
			if (cmx < 0)
			{
				if (cmx < -disX / 2)
				{
					cmx = -disX / 2;
					cmtoX = 0;
					vX = 0;
				}
			}
			else if (cmx > cmxLim)
			{
				if (cmx < cmxLim + disX / 2)
				{
					cmx = cmxLim + disX / 2;
					cmtoX = cmxLim;
					vX = 0;
				}
			}
			else
			{
				cmx += vX / 10;
			}
			cmtoX = cmx;
			vX -= vX / 10;
			if (vX / 10 == 0)
			{
				vX = 0;
			}
		}
		else if (cmx < 0)
		{
			cmtoX = 0;
		}
		else if (cmx > cmxLim)
		{
			cmtoX = cmxLim;
		}
		if (cmx != cmtoX)
		{
			cmvx = cmtoX - cmx << 2;
			cmdx += cmvx;
			cmx += cmdx >> 4;
			cmdx &= 15;
		}
	}

	public override void updateKey()
	{
		count++;
		if (isCircle)
		{
			if (Canvas.isPointerClick)
			{
				Canvas.isPointerClick = false;
				isClick = true;
				for (int i = 0; i < list.size(); i++)
				{
					int num = wCircle * CRes.cos(CRes.fixangle(angleCircle * i)) >> 10;
					int num2 = -(wCircle * CRes.sin(CRes.fixangle(angleCircle * i))) >> 10;
					if (Canvas.isPoint((int)((float)(x + num) * AvMain.zoom - (float)(20 * AvMain.hd)), (int)((float)(y + num2) * AvMain.zoom - (float)(30 * AvMain.hd) + (float)Canvas.transTab), 40 * AvMain.hd, 50 * AvMain.hd))
					{
						indexCircle = i;
						trans = true;
						break;
					}
				}
			}
			if (trans)
			{
				if (indexCircle != -1 && Canvas.isPointerDown)
				{
					int num3 = wCircle * CRes.cos(CRes.fixangle(angleCircle * indexCircle)) >> 10;
					int num4 = -(wCircle * CRes.sin(CRes.fixangle(angleCircle * indexCircle))) >> 10;
					if (!Canvas.isPoint((int)((float)(x + num3) * AvMain.zoom - (float)(20 * AvMain.hd)), (int)((float)(y + num4) * AvMain.zoom - (float)(30 * AvMain.hd) + (float)Canvas.transTab), 40 * AvMain.hd, 50 * AvMain.hd))
					{
						indexCircle = -1;
					}
				}
				if (Canvas.isPointerRelease)
				{
					Canvas.isPointerRelease = false;
					trans = false;
					isClick = false;
					if (indexCircle != -1)
					{
						Command command = (Command)list.elementAt(indexCircle);
						closeGo();
						command.action.perform();
					}
				}
			}
			if (isClick && Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isClick = false;
				closeGo();
			}
		}
		else
		{
			if (Canvas.isPointerClick)
			{
				isClick = true;
				Canvas.isPointerClick = false;
				for (int j = 0; j < list.size(); j++)
				{
					int num5 = (int)(distant * (float)CRes.cos(CRes.fixangle(angle * j + angle / 2))) >> 10;
					int num6 = -(int)(distant * (float)CRes.sin(CRes.fixangle(angle * j + angle / 2))) >> 10;
					if (Canvas.isPointer((int)((float)xCenter * AvMain.zoom + (float)num5) - 15 * AvMain.hd, (int)((float)yCenter * AvMain.zoom + (float)num6) - 15 * AvMain.hd, 30 * AvMain.hd, 30 * AvMain.hd))
					{
						isTran = true;
						selected = j;
						indexTemp = j;
						timeDelay = count;
						break;
					}
				}
			}
			if (isTran)
			{
				long num7 = count - timeDelay;
				int a = Canvas.dx();
				int a2 = Canvas.dy();
				if (Canvas.isPointerDown)
				{
					if (indexTemp != -1)
					{
						int num8 = (int)(distant * (float)CRes.cos(CRes.fixangle(angle * indexTemp + angle / 2))) >> 10;
						int num9 = -(int)(distant * (float)CRes.sin(CRes.fixangle(angle * indexTemp + angle / 2))) >> 10;
						if (!Canvas.isPointer((int)((float)xCenter * AvMain.zoom + (float)num8) - 15 * AvMain.hd, (int)((float)yCenter * AvMain.zoom + (float)num9) - 15 * AvMain.hd, 30 * AvMain.hd, 30 * AvMain.hd))
						{
							indexTemp = -1;
						}
					}
					if (CRes.abs(a) >= 10 * AvMain.hd || CRes.abs(a2) > 10 * AvMain.hd)
					{
						isHide = true;
					}
					else if (num7 > 3 && num7 < 8)
					{
						isHide = false;
					}
				}
				if (Canvas.isPointerRelease)
				{
					isClick = false;
					isTran = false;
					if (indexTemp != -1)
					{
						if (num7 <= 4)
						{
							isHide = false;
							timeOpen = 5;
						}
						else if (!isHide)
						{
							click();
						}
					}
					Canvas.isPointerRelease = false;
				}
			}
			if (isClick && Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isClick = false;
				closeGo();
			}
		}
		base.updateKey();
	}

	private void click()
	{
		if (indexCircle != -1)
		{
			Command command = (Command)list.elementAt(indexCircle);
			command.perform();
		}
		else if (indexTemp != -1)
		{
			cmdCenter.perform();
		}
		indexTemp = (indexCircle = -1);
		trans = false;
		isClick = false;
		isHide = true;
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		Canvas.resetTransNotZoom(g);
		Canvas.paint.paintTransBack(g);
		if (isCircle)
		{
			paintCircle(g);
		}
		else
		{
			paintNormal(g);
		}
		base.paint(g);
	}

	private void paintCircle(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			if (i < listObj.size())
			{
				AvPosition avPosition = (AvPosition)listObj.elementAt(i);
				num = avPosition.x;
				num2 = avPosition.y;
			}
			else
			{
				int num3 = wCircle * CRes.cos(CRes.fixangle(angleCircle * i)) >> 10;
				int num4 = -(wCircle * CRes.sin(CRes.fixangle(angleCircle * i))) >> 10;
				num = x + num3;
				num2 = y + num4;
			}
			command.paint(g, num, num2);
			Canvas.smallFontYellow.drawString(g, command.caption, num, num2 - 25 * AvMain.hd - Canvas.arialFont.getHeight(), 2);
		}
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
	}

	private void paintNormal(MyGraphics g)
	{
		if (lastScr != MiniMap.me)
		{
			GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
			if (MapScr.focusP != null && avaPaint != null)
			{
				MapScr.focusP.paintIcon(g, xCenter, yCenter + (int)((float)(35 * AvMain.hd / 2) + 5f * AvMain.zoom), false);
				Canvas.smallFontRed.drawString(g, MapScr.focusP.name, xCenter, yCenter - 35 * AvMain.hd / 2 - AvMain.hSmall, 2);
			}
			if (avaPaint == null)
			{
				GameMidlet.avatar.paintIcon(g, xCenter, yCenter + 35 * AvMain.hd / 2, false);
				Canvas.smallFontRed.drawString(g, GameMidlet.avatar.name, xCenter, yCenter - 35 * AvMain.hd / 2 - AvMain.hSmall, 2);
			}
			GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
		}
		Canvas.resetTransNotZoom(g);
		for (int i = 0; i < list.size(); i++)
		{
			int num = (int)(distant * (float)CRes.cos(CRes.fixangle(angle * i + angle / 2))) >> 10;
			int num2 = -(int)(distant * (float)CRes.sin(CRes.fixangle(angle * i + angle / 2))) >> 10;
			Command command = (Command)list.elementAt(i);
			if (!isHide && i == selected)
			{
				Menu.imgBackIcon.drawFrame(1, (int)((float)xCenter * AvMain.zoom + (float)num), (int)((float)yCenter * AvMain.zoom + (float)num2), 0, 3, g);
			}
			else
			{
				Menu.imgBackIcon.drawFrame(0, (int)((float)xCenter * AvMain.zoom + (float)num), (int)((float)yCenter * AvMain.zoom + (float)num2), 0, 3, g);
			}
			Canvas.smallFontYellow.drawString(g, command.caption, (int)((float)xCenter * AvMain.zoom + (float)num), (int)((float)yCenter * AvMain.zoom + (float)num2 - (float)(20 * AvMain.hd) - (float)(AvMain.hSmall / 2)), 2);
			g.setColor(0);
			command.paint(g, (int)((float)xCenter * AvMain.zoom + (float)num), (int)((float)yCenter * AvMain.zoom + (float)num2));
		}
	}

	public void showCircle(MyVector list2, MyVector listObj)
	{
		this.listObj = listObj;
		list = list2;
		isCircle = true;
		angleCircle = 360 / list2.size();
		wCircle = 5;
		x = (int)((float)LoadMap.posFocus.x - AvCamera.gI().xCam);
		y = (int)((float)LoadMap.posFocus.y - AvCamera.gI().yCam);
		if (x < maxW + 35 * AvMain.hd / 2)
		{
			x = maxW + 35 * AvMain.hd / 2;
		}
		else if ((float)x * AvMain.zoom > (float)Canvas.w - (float)maxW * AvMain.zoom - (float)(35 * AvMain.hd / 2) * AvMain.zoom)
		{
			x = (int)(((float)Canvas.w - (float)maxW * AvMain.zoom - (float)(35 * AvMain.hd / 2) * AvMain.zoom) / AvMain.zoom);
		}
		if ((float)y * AvMain.zoom < (float)maxW * AvMain.zoom + (float)(35 * AvMain.hd / 2) * AvMain.zoom)
		{
			y = maxW + 35 * AvMain.hd / 2;
		}
		if ((float)y * AvMain.zoom > (float)Canvas.hCan - (float)maxW * AvMain.zoom - (float)(35 * AvMain.hd / 2) * AvMain.zoom)
		{
			y = (int)(((float)Canvas.hCan - (float)maxW * AvMain.zoom - (float)(35 * AvMain.hd / 2) * AvMain.zoom) / AvMain.zoom);
		}
		switchToMe();
	}

	public void doExchange()
	{
		if (MapScr.focusP != null)
		{
			if (LoadMap.focusObj != null && LoadMap.focusObj.catagory == 0 && ((Avatar)LoadMap.focusObj).IDDB == -100)
			{
				Canvas.startOKDlg("Bạn có muốn bất đầu lể cưới không ?", new IActionOkWedding());
				return;
			}
			isCircle = false;
			MyVector myVector = new MyVector();
			myVector.addElement(setCommandMenu(T.hit, 9, 13));
			myVector.addElement(setCommandMenu(T.privateMsg, 12, 2));
			myVector.addElement(setCommandMenu(T.addFriend, 7, 11));
			myVector.addElement(setCommandMenu(T.giveGift, 8, 12));
			myVector.addElement(setCommandMenu(T.kiss, 10, 21));
			myVector.addElement(setCommandMenu(T.inviteMyHouse, 13, 22));
			myVector.addElement(setCommandMenu(T.info, 17, 19));
			myVector.addElement(setCommandMenu(T.other, 16, 6));
			setInfo(myVector);
			isName = true;
		}
	}

	public void doMenuMiniMap()
	{
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			MyVector myVector = new MyVector();
			Command o = setCommandMenu(T.info, 18, 3);
			Command o2 = setCommandMenu(T.changePass, 19, 4);
			myVector.addElement(o);
			myVector.addElement(o2);
			Command o3 = setCommandMenu(T.otherGame, 20, 6);
			Command o4 = setCommandMenu(T.option, 21, 23);
			myVector.addElement(o3);
			myVector.addElement(o4);
			if (Canvas.currentMyScreen != PopupShop.gI())
			{
				setInfo(myVector);
				avaPaint = null;
			}
		}
	}

	public Command setCommandMenu(string text, int type, int index)
	{
		return new CommandMenu(text, type, index);
	}

	public Command setCommandMenu(string text, IAction action, int index)
	{
		return new CommandMenu(text, action, index);
	}
}
