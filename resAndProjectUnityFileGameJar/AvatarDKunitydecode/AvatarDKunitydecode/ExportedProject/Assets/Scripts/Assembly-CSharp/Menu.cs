public class Menu : MenuMain
{
	public static Menu me;

	public bool showMenu;

	public MyVector list;

	public int selected;

	public int chan;

	public int menuX;

	public int menuY;

	public int menuW;

	public int menuH;

	public int menuTemY;

	public int w;

	public int hItem;

	public int wItem;

	public static FrameImage imgSellect;

	public static FrameImage imgBackIcon;

	public short[] radius;

	public int pos;

	public bool showMenuFarm;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int xTranMenu;

	public int xTranTo;

	private int xL;

	private int size;

	public static Command cmdClose;

	public IAction iNo;

	private MyVector listText = new MyVector();

	private int vY;

	private int disY;

	private int pa;

	private int dyTran;

	private int timeOpen;

	private int pyLast;

	private bool trans;

	private bool isClick;

	private bool isClose;

	private bool isFire;

	private bool isG;

	private long timeDelay;

	private long count;

	private long timePoint;

	private int dir = 1;

	private int xText;

	public Menu()
	{
		initCmd();
	}

	static Menu()
	{
	}

	public static Menu gI()
	{
		return (me != null) ? me : new Menu();
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			doFire();
			break;
		case 1:
			showMenu = (showMenuFarm = false);
			Canvas.menuMain = null;
			if (iNo != null)
			{
				iNo.perform();
			}
			break;
		}
	}

	public void initCmd()
	{
		if (Canvas.stypeInt == 0)
		{
			left = new Command(T.selectt, 0);
		}
	}

	public void startMenuFarm(MyVector menuItem, int x, int y, int w, int h)
	{
		if (menuItem.size() == 0)
		{
			return;
		}
		isHide = true;
		size = menuItem.size();
		xL = Canvas.h;
		showMenuFarm = true;
		showMenu = true;
		menuW = size * w + AvMain.hDuBox * 2 + 2;
		if (menuW > Canvas.w)
		{
			menuW = Canvas.w;
		}
		menuX = x - menuW / 2;
		menuH = h + AvMain.hDuBox / 2;
		if (menuX < 0)
		{
			menuX = 0;
		}
		if (Canvas.currentMyScreen != FarmScr.instance)
		{
			MainMenu.gI().avaPaint = null;
		}
		if (MainMenu.gI().avaPaint != null)
		{
			menuY = MainMenu.gI().avaPaint.y + AvMain.hNormal + AvMain.hDuBox;
			if (menuY + menuH > Canvas.h)
			{
				menuY = MainMenu.gI().avaPaint.y - Canvas.hTab - menuH - AvMain.hDuBox * 2;
			}
		}
		else
		{
			menuY = (int)((float)(GameMidlet.avatar.y * AvMain.hd) * ((Canvas.currentMyScreen != FarmScr.instance) ? 1f : AvMain.zoom) - AvCamera.gI().yCam - (float)menuH - (float)(AvMain.hDuBox * 2));
			if (AvMain.zoom == 2f)
			{
				menuY -= 20;
			}
			if (menuY < 10 + AvMain.hDuBox + AvMain.hBlack)
			{
				menuY = 10 + AvMain.hDuBox + AvMain.hBlack;
			}
		}
		if (Canvas.currentMyScreen == HouseScr.me)
		{
			menuY = Canvas.hCan - menuH - 30 * AvMain.hd;
		}
		menuTemY = menuY;
		hItem = h;
		wItem = w;
		list = menuItem;
		setSelected();
		cmyLim = size * wItem - (menuW - AvMain.hDuBox * 2 - 4);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		disY = menuW;
		cmdClose = null;
		pos = 0;
		iNo = null;
		xTranTo = 0;
		Canvas.xTran = 0;
		isClose = false;
		show();
	}

	private void setSelected()
	{
		if (selected < 0)
		{
			selected = 0;
		}
		if (selected >= size)
		{
			selected = 0;
		}
	}

	public void setPos(int x, int y)
	{
		menuX = x;
		menuY = y;
		if (menuX < 0)
		{
			menuX = 0;
		}
		if (menuY < 0)
		{
			menuY = 0;
		}
	}

	private void doFire()
	{
		Canvas.xTran = 0;
		xTranMenu = (xTranTo = 0);
		if (cmdClose != null)
		{
			if (cmdClose.action != null)
			{
				cmdClose.action.perform();
			}
			else if (cmdClose.pointer != null)
			{
				cmdClose.pointer.commandActionPointer(cmdClose.indexMenu, cmdClose.subIndex);
			}
			else
			{
				commandTab(cmdClose.indexMenu);
			}
		}
		Command command = (Command)list.elementAt(selected);
		if (command.action != null)
		{
			command.action.perform();
		}
		else if (command.pointer != null)
		{
			command.pointer.commandActionPointer(command.indexMenu, command.subIndex);
		}
		else
		{
			Canvas.currentMyScreen.commandAction(command.indexMenu);
		}
	}

	public override void updateKey()
	{
		updateMenuKeyMain();
	}

	private void click()
	{
		int num = hItem;
		if (showMenuFarm)
		{
			num = wItem;
		}
		int num2 = menuTemY + Canvas.transTab;
		int num3 = (cmtoY + Canvas.py - num2) / num;
		if (showMenuFarm)
		{
			num2 = menuX;
			num3 = (cmtoY + Canvas.px - num2) / num;
		}
		isHide = true;
		if (num3 >= 0 && num3 < size)
		{
			selected = num3;
			if (!showMenuFarm)
			{
				xTranTo = -menuW;
				isClose = true;
				isFire = true;
			}
			else
			{
				isFire = false;
				showMenu = (showMenuFarm = false);
				Canvas.menuMain = null;
				doFire();
			}
		}
	}

	private void updateMenuKeyMain()
	{
		count++;
		if (chan != 0)
		{
			return;
		}
		if (!showMenuFarm && CRes.abs(CRes.abs(xTranMenu) - CRes.abs(xTranTo)) < 5)
		{
			if (Canvas.isPointerClick)
			{
				pyLast = Canvas.pyLast;
				dir = 1;
				xText = 0;
				isClick = true;
				isG = false;
				if (Canvas.isPoint(menuX + 4 * AvMain.hd, menuY + 4 * AvMain.hd, menuW - 8 * AvMain.hd, menuH - 8 * AvMain.hd))
				{
					if (vY != 0)
					{
						isG = true;
					}
					Canvas.isPointerClick = false;
					pa = cmtoY;
					timeDelay = count;
					trans = true;
				}
			}
			if (trans)
			{
				int num = pyLast - Canvas.py;
				pyLast = Canvas.py;
				long num2 = count - timeDelay;
				if (Canvas.isPointerDown)
				{
					if (count % 2 == 0)
					{
						dyTran = Canvas.py;
						timePoint = count;
					}
					vY = 0;
					if (Math.abs(num) < 10 * AvMain.hd)
					{
						int num3 = menuY + 8 * AvMain.hd;
						int num4 = hItem;
						int num5 = (cmtoY + Canvas.py - num3) / num4;
						if (num5 >= 0 && num5 < size)
						{
							selected = num5;
						}
					}
					if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
					{
						isHide = true;
					}
					else if (num2 > 3 && num2 < 8)
					{
						int num6 = menuY + 8 * AvMain.hd;
						int num7 = hItem;
						int num8 = (cmtoY + Canvas.py - num6) / num7;
						if (num8 >= 0 && num8 < size && !isG)
						{
							isHide = false;
						}
					}
					if (cmtoY < 0 || cmtoY > cmyLim)
					{
						cmtoY = pa + num / 2;
						pa = cmtoY;
					}
					else
					{
						cmtoY = pa + num / 2;
						pa = cmtoY;
					}
					cmy = cmtoY;
				}
				if (Canvas.isPointerRelease && Canvas.isPoint(menuX + 4 * AvMain.hd, menuY + 4 * AvMain.hd, menuW - 8 * AvMain.hd + 12 * AvMain.hd, menuH - 8 * AvMain.hd))
				{
					isG = false;
					int num9 = (int)(count - timePoint);
					int num10 = dyTran - Canvas.py;
					if (showMenuFarm)
					{
						num10 = dyTran - Canvas.px;
					}
					if (CRes.abs(num10) > 40 && num9 < 10 && cmtoY > 0 && cmtoY < cmyLim)
					{
						vY = num10 / num9 * 10;
					}
					timePoint = -1L;
					if (Math.abs(num) < 10 * AvMain.hd)
					{
						if (num2 <= 4)
						{
							isHide = false;
							timeOpen = 5;
						}
						else if (!isHide)
						{
							click();
						}
					}
					trans = false;
					Canvas.isPointerRelease = false;
				}
			}
			if (trans || Canvas.isPoint(menuX + 4 * AvMain.hd, menuY + 4 * AvMain.hd, menuW - 8 * AvMain.hd, menuH - 8 * AvMain.hd) || !Canvas.isPointerRelease || !isClick)
			{
				return;
			}
			isG = false;
			isClick = false;
			int num11 = dyTran - Canvas.py;
			if (!trans)
			{
				if (!isClose)
				{
					xTranTo = -menuW;
				}
				isClose = true;
			}
			trans = false;
			Canvas.isPointerRelease = false;
			return;
		}
		if (Canvas.isPointerClick)
		{
			pyLast = Canvas.pyLast;
			if (showMenuFarm)
			{
				pyLast = Canvas.pxLast;
			}
			dir = 1;
			xText = 0;
			isClick = true;
			if (Canvas.isPoint(menuX - 2, menuTemY - 7 + Canvas.transTab, menuW + 4, menuH + 15))
			{
				Canvas.isPointerClick = false;
				pa = cmy;
				timeDelay = count;
				trans = true;
			}
		}
		if (trans)
		{
			int num12 = pyLast - Canvas.py;
			if (showMenuFarm)
			{
				num12 = pyLast - Canvas.px;
				pyLast = Canvas.px;
			}
			else
			{
				pyLast = Canvas.py;
			}
			long num13 = count - timeDelay;
			if (Canvas.isPointerDown)
			{
				dyTran = Canvas.py;
				if (showMenuFarm)
				{
					dyTran = Canvas.px;
				}
				timePoint = count;
				vY = 0;
				if (Math.abs(num12) < 10 * AvMain.hd)
				{
					int num14 = menuTemY;
					int num15 = hItem;
					if (showMenuFarm)
					{
						num15 = wItem;
					}
					int num16 = (cmtoY + Canvas.py - num14) / num15;
					if (showMenuFarm)
					{
						num14 = menuX + AvMain.hDuBox / 4;
						num16 = (cmtoY + Canvas.px - num14) / num15;
					}
					if (num16 >= 0 && num16 < size)
					{
						selected = num16;
					}
				}
				if (CRes.abs(Canvas.dx()) >= 10 * AvMain.hd || CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num13 > 3 && num13 < 8)
				{
					isHide = false;
				}
				if (cmy < 0 || cmy >= cmyLim)
				{
					cmtoY = pa + num12 / 2;
					pa = cmtoY;
				}
				else
				{
					cmtoY = pa + num12;
					pa = cmtoY;
				}
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease && Canvas.isPoint(menuX - 2, menuTemY - 7 + Canvas.transTab, menuW + 4, menuH + 15))
			{
				int num17 = (int)(count - timePoint);
				int num18 = dyTran - Canvas.py;
				if (showMenuFarm)
				{
					num18 = dyTran - Canvas.px;
				}
				if (CRes.abs(num18) > 40 && num17 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num18 / num17 * 10;
				}
				timePoint = -1L;
				if (Math.abs(num12) < 10 * AvMain.hd)
				{
					if (num13 <= 4)
					{
						isHide = false;
						timeOpen = 5;
					}
					else if (!isHide)
					{
						click();
					}
				}
				trans = false;
				Canvas.isPointerRelease = false;
			}
		}
		if (!Canvas.isPoint(menuX - 2, menuTemY - 7 + Canvas.transTab, menuW + 4, menuH + 15) && isClick && Canvas.isPointerRelease)
		{
			close();
			trans = false;
			Canvas.isPointerRelease = false;
		}
	}

	public void close()
	{
		isClick = false;
		if (!trans)
		{
			showMenu = (showMenuFarm = false);
			Canvas.menuMain = null;
			if (iNo != null)
			{
				iNo.perform();
			}
			xTranTo = (xTranMenu = (Canvas.xTran = 0));
		}
	}

	public override void paint(MyGraphics g)
	{
		g.translate(0f, xL);
		paintMenuFarm(g);
		base.paint(g);
	}

	private void paintMenuFarm(MyGraphics g)
	{
		Canvas.xTran = 0;
		Canvas.resetTrans(g);
		if (Canvas.currentMyScreen != MainMenu.me)
		{
			Canvas.paint.paintTransBack(g);
		}
		if (LoadMap.focusObj != null && MainMenu.gI().avaPaint != null)
		{
			((Animal)LoadMap.focusObj).paintIcon(g, MainMenu.gI().avaPaint.x, MainMenu.gI().avaPaint.y, false);
		}
		g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
		g.setClip(0f, 0f, Canvas.w, Canvas.hCan);
		Canvas.paint.paintPopupBack(g, menuX, menuY, menuW, menuH + 3 * AvMain.hd, -1, false);
		g.translate(menuX + AvMain.hDuBox + 1, menuY);
		g.setClip(0f, 0f, menuW - AvMain.hDuBox * 2 - 2, menuH + 10);
		g.translate(-cmy, 0f);
		int num = cmy / wItem;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num + menuW / wItem + 2;
		if (num2 > size)
		{
			num2 = size;
		}
		if (!isHide)
		{
			g.setColor(16777215);
			g.fillRect(selected * wItem + 4 * AvMain.hd, menuH / 2 - hItem / 2 + 4 * AvMain.hd, wItem - 8 * AvMain.hd, hItem + 4 * AvMain.hd - 8 * AvMain.hd);
		}
		for (int i = num; i < num2; i++)
		{
			Command command = (Command)list.elementAt(i);
			command.paint(g, i * wItem + wItem / 2, hItem / 2 + 4 * AvMain.hd);
		}
		if (selected >= 0 && selected < list.size())
		{
			Command command2 = (Command)list.elementAt(selected);
			g.setClip(cmy - 50 * AvMain.hd, -100f, cmy + Canvas.w + 100 * AvMain.hd, menuH + 200);
			int num3 = selected * wItem + wItem / 2;
			if (size * wItem + AvMain.hDuBox * 2 + 10 > Canvas.w)
			{
				int num4 = Canvas.borderFont.getWidth(command2.caption) / 2;
				if (num3 - num4 < cmy)
				{
					num3 = cmy + num4;
				}
				else if (num3 + num4 > Canvas.w + cmy - 15)
				{
					num3 = Canvas.w + cmy - num4 - 15;
				}
			}
			Canvas.smallFontYellow.drawString(g, command2.caption, num3, -AvMain.hSmall - 3 * AvMain.hd, 2);
		}
		Canvas.resetTrans(g);
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
		if (xL != 0)
		{
			xL += -xL >> 1;
		}
		if (xL == -1)
		{
			xL = 0;
		}
		if (CRes.abs(CRes.abs(xTranMenu) - CRes.abs(xTranTo)) > 0 && !showMenuFarm)
		{
			xTranMenu += (xTranTo - xTranMenu) / 3;
			Canvas.xTran = menuW + xTranMenu;
			if (isClose && (xTranTo - xTranMenu) / 3 == 0)
			{
				trans = false;
				isClose = false;
				isClick = false;
				showMenu = false;
				if (isFire)
				{
					isFire = false;
					doFire();
				}
				Canvas.xTran = 0;
			}
		}
		else
		{
			xTranMenu = xTranTo;
		}
		if (xTranMenu == -1)
		{
			xTranMenu = 0;
		}
		moveCamera();
		updateMain();
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
				if (cmy < -disY / 2)
				{
					cmy = -disY / 2;
					cmtoY = 0;
					vY = 0;
				}
			}
			else if (cmy > cmyLim)
			{
				if (cmy < cmyLim + disY / 2)
				{
					cmy = cmyLim + disY / 2;
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
		else if (cmy < 0)
		{
			cmtoY = 0;
		}
		else if (cmy > cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (cmy != cmtoY)
		{
			cmvy = cmtoY - cmy << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
	}

	private void updateMain()
	{
		if (menuTemY > menuY)
		{
			int num = menuTemY - menuY >> 2;
			if (num < 1)
			{
				num = 1;
			}
			menuTemY -= num;
		}
		menuTemY = menuY;
	}
}
