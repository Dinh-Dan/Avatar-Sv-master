public class MenuOn : MenuMain
{
	public static MenuOn me;

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

	public int pos;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	private int xL;

	private int size;

	public static Command cmdClose;

	public static Image imgTab;

	public static Image imgSelect;

	private int vY;

	private int disY;

	private int pa;

	private int dyTran;

	private int timeOpen;

	private bool trans;

	private bool isClick;

	private long timeDelay;

	private long count;

	private long timePoint;

	public MenuOn()
	{
		initCmd();
	}

	public static MenuOn gI()
	{
		return (me != null) ? me : (me = new MenuOn());
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			doFire();
			break;
		case 1:
			showMenu = false;
			break;
		}
	}

	public void initCmd()
	{
		left = new Command(T.close, 1);
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
		showMenu = false;
		if (cmdClose != null)
		{
			if (cmdClose.action != null)
			{
				cmdClose.action.perform();
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
		else
		{
			Canvas.currentMyScreen.commandAction(command.indexMenu);
		}
	}

	public override void updateKey()
	{
		updateMenuKeyMain();
		Canvas.paint.updateKeyOn(left, right, center);
		if (timeOpen > 0)
		{
			timeOpen--;
			if (timeOpen == 0)
			{
				click();
			}
		}
	}

	private void click()
	{
		int num = hItem;
		int num2 = menuTemY + Canvas.transTab;
		int num3 = (cmtoY + Canvas.py - num2) / num;
		if (num3 >= 0 && num3 < size)
		{
			selected = num3;
			doFire();
		}
		isHide = true;
	}

	private void updateMenuKeyMain()
	{
		count++;
		if (chan != 0)
		{
			return;
		}
		bool flag = false;
		if (Canvas.isPointerClick)
		{
			isClick = true;
			if (Canvas.isPoint(menuX - 2, menuTemY - 7 + Canvas.transTab, menuW + 4, menuH + 15))
			{
				isTran = true;
				Canvas.isPointerClick = false;
				pa = cmy;
				timeDelay = count;
				trans = true;
			}
		}
		if (trans)
		{
			int num = Canvas.dy();
			long num2 = count - timeDelay;
			if (Canvas.isPointerDown)
			{
				if (Canvas.gameTick % 3 == 0)
				{
					dyTran = Canvas.py;
					timePoint = count;
				}
				vY = 0;
				if (Math.abs(num) < 20 * AvMain.hd)
				{
					int num3 = menuTemY + 1 + Canvas.transTab;
					int num4 = hItem;
					int num5 = (cmtoY + Canvas.py - num3) / num4;
					if (num5 >= 0 && num5 < size)
					{
						selected = num5;
					}
				}
				if (CRes.abs(num) >= 20 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num2 > 3 && num2 < 8)
				{
					isHide = false;
				}
				cmtoY = pa + num;
				if (cmtoY < 0 || cmtoY > cmyLim)
				{
					cmtoY = pa + num / 3;
				}
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease && Canvas.isPoint(menuX - 2, menuTemY - 7 + Canvas.transTab, menuW + 4, menuH + 15))
			{
				int num6 = (int)(count - timePoint);
				int num7 = dyTran - Canvas.py;
				if (CRes.abs(num7) > 40 && num6 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num7 / num6 * 10;
				}
				timePoint = -1L;
				if (Math.abs(num) < 20 * AvMain.hd)
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
				Canvas.isPointerRelease = false;
			}
		}
		if (isClick && Canvas.isPointerRelease)
		{
			isClick = false;
			if (!trans)
			{
				showMenu = false;
			}
			trans = false;
			Canvas.isPointerRelease = false;
		}
		if (flag)
		{
			int num8 = hItem;
			cmtoY = selected * num8 - menuW / 2 + num8 / 2;
			if (cmtoY > cmyLim)
			{
				cmtoY = cmyLim;
			}
			else if (cmtoY < 0)
			{
				cmtoY = 0;
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		if (Canvas.currentDialog == null)
		{
			Canvas.paint.paintTabSoft(g);
			Canvas.paint.paintCmdBar(g, left, center, right);
		}
	}

	public void paintMenuNormal(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.drawImage(imgTab, menuX, menuY - 5 * AvMain.hd, 0);
		g.setClip(menuX, menuY, menuW, menuH - 10 * AvMain.hd);
		g.translate(menuX + 3, menuTemY + 1);
		g.translate(0f, -cmy);
		int num = (hItem - AvMain.hNormal) / 2;
		int num2 = 0;
		for (int i = 0; i < size; i++)
		{
			g.setColor(0);
			if (!isHide && i == selected)
			{
				g.drawImageScale(imgSelect, 0, i * hItem, menuW - 6, hItem, 0);
			}
			num2 = 0;
			Canvas.normalWhiteFont.drawString(g, ((Command)list.elementAt(i)).caption, 5 + num2, i * hItem + num, 0);
		}
	}

	public void paintMain(MyGraphics g)
	{
		if (size != 0)
		{
			g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
			if (chan == 0)
			{
				paintMenuNormal(g);
			}
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
		if (xL != 0)
		{
			xL += -xL >> 1;
		}
		if (xL == -1)
		{
			xL = 0;
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
