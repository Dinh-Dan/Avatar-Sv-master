public class MenuSub : MenuMain
{
	public static MenuSub me;

	public MyVector list;

	public int x;

	public int y;

	public int w;

	public int h;

	public int hItem;

	public int index;

	public static int cmtoY;

	public static int cmy;

	public static int cmdy;

	public static int cmvy;

	public static int cmyLim;

	private int vY;

	private long count;

	private long timePoint;

	private int dyTran;

	private int timeOpen;

	private int pyLast;

	private bool isFire;

	private bool isG;

	private bool transY;

	private int pa;

	private long timeDelay;

	public MenuSub()
	{
		w = 200 * AvMain.hd;
		hItem = 35 * AvMain.hd;
		h = hItem * 4;
	}

	public static MenuSub gI()
	{
		return (me != null) ? me : (me = new MenuSub());
	}

	public void startAt(MyVector menuList, int x, int y)
	{
		list = menuList;
		this.x = x;
		this.y = y;
		cmyLim = list.size() * hItem - h / 2 - 90;
		cmy = (cmtoY = 0);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		isHide = true;
		show();
	}

	private void click()
	{
		Command command = (Command)list.elementAt(index);
		command.perform();
		isHide = true;
		list.removeAllElements();
		Canvas.menuMain = null;
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
		if (vY != 0)
		{
			if (cmy < 0 || cmy > cmyLim)
			{
				if (vY > 500)
				{
					vY = 500;
				}
				else if (vY < -500)
				{
					vY = -500;
				}
				vY -= vY / 5;
				if (CRes.abs(vY / 10) <= 10)
				{
					vY = 0;
				}
			}
			cmy += vY / 15;
			cmtoY = cmy;
			vY -= vY / 20;
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
		Canvas.loadMap.update();
	}

	public override void updateKey()
	{
		count++;
		PaintPopup.gI().setupdateTab();
		if (Canvas.isPointerClick && Canvas.isPoint(x, y, w, h))
		{
			isTran = true;
			isG = false;
			if (vY != 0)
			{
				isG = true;
			}
			pyLast = Canvas.pyLast;
			Canvas.isPointerClick = false;
			pa = cmy;
			transY = true;
			timeDelay = count;
			isFire = true;
		}
		if (transY)
		{
			long num = count - timeDelay;
			int num2 = pyLast - Canvas.py;
			pyLast = Canvas.py;
			if (Canvas.isPointerDown)
			{
				if (count % 2 == 0)
				{
					dyTran = Canvas.py;
					timePoint = count;
				}
				vY = 0;
				int num3 = (cmtoY + Canvas.py - y) / hItem;
				index = num3;
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num > 3 && num < 8 && isFire && !isG)
				{
					isHide = false;
				}
				cmtoY = pa + num2;
				if (cmtoY < 0 || cmtoY > cmyLim)
				{
					cmtoY = pa + num2 / 2;
				}
				pa = cmtoY;
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease && Canvas.isPoint(x, y, w, h))
			{
				isG = false;
				int num4 = (int)(count - timePoint);
				int num5 = dyTran - Canvas.py;
				if (CRes.abs(num5) > 40 && num4 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num5 / num4 * 10;
				}
				timePoint = -1L;
				if (Math.abs(Canvas.dy()) < 10 * AvMain.hd && isFire)
				{
					if (num <= 4)
					{
						if (isFire)
						{
							isHide = false;
						}
						timeOpen = 5;
					}
					else if (!isHide)
					{
						click();
					}
					isFire = false;
				}
			}
		}
		if (Canvas.isPointerRelease && !transY && !Canvas.isPoint(x, y, w, h))
		{
			transY = false;
			list.removeAllElements();
			Canvas.menuMain = null;
		}
		if (Canvas.isPointerRelease)
		{
			transY = false;
		}
		base.updateKey();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, x, y - 8 * AvMain.hd, w, h + 16 * AvMain.hd, -1, false);
		g.translate(x, y);
		g.setClip(0f, 0f, w, h);
		g.translate(0f, -cmy);
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			if (i == index && !isHide)
			{
				g.setColor(16777215);
				g.fillRect(10 * AvMain.hd, i * hItem, w - 20 * AvMain.hd, hItem);
			}
			Canvas.normalFont.drawString(g, command.caption, 20, i * hItem + hItem / 2 - Canvas.normalFont.getHeight() / 2, 0);
		}
	}
}
