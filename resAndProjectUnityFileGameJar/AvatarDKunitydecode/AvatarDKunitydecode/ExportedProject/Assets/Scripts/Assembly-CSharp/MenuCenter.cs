public class MenuCenter : MenuMain
{
	public static MenuCenter me;

	private int x;

	private int y;

	private int w;

	private int h;

	private int hCell;

	private MyVector list;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int selected;

	public int hDis;

	private new bool isHide;

	private int timeOpen1;

	private bool trans;

	private bool isG;

	private bool isClick;

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

	public MenuCenter()
	{
		w = 175 * AvMain.hd;
		h = 200 * AvMain.hd;
		hCell = 35 * AvMain.hd;
		hDis = h - 15 * AvMain.hd;
	}

	public static MenuCenter gI()
	{
		return (me != null) ? me : (me = new MenuCenter());
	}

	public void startAt(MyVector list)
	{
		x = (Canvas.w - w) / 2;
		y = (Canvas.hCan - h) / 2;
		this.list = list;
		cmyLim = list.size() * hCell - hDis;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = 0);
		isHide = true;
		show();
	}

	public override void update()
	{
		if (timeOpen1 > 0)
		{
			timeOpen1--;
			if (timeOpen1 == 0)
			{
				click1();
			}
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
				if (cmy < -h / 2)
				{
					cmy = -h / 2;
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

	public void click1()
	{
		Out.println("click1: " + selected);
		trans = false;
		Canvas.menuMain = null;
		if (selected < list.size())
		{
			Command command = (Command)list.elementAt(selected);
			command.perform();
		}
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.isPointerClick)
		{
			isClick = true;
			Canvas.isPointerClick = false;
			if (Canvas.isPoint(x, y, w, h))
			{
				trans = true;
				pyLast = Canvas.pyLast;
				isG = false;
				if (vY != 0)
				{
					isG = true;
				}
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
				dyTran = Canvas.py;
				timePoint = count;
				vY = 0;
				if (Math.abs(num) < 10 * AvMain.hd)
				{
					int num3 = y + 10;
					int num4 = (cmtoY + Canvas.py - num3) / hCell;
					if (num4 >= 0 && num4 < list.size())
					{
						selected = num4;
					}
				}
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num2 > 3 && num2 < 8)
				{
					int num5 = y + 10;
					int num6 = (cmtoY + Canvas.py - num5) / hCell;
					if (num6 >= 0 && num6 < list.size() && !isG)
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
			if (Canvas.isPointerRelease)
			{
				trans = false;
				Canvas.isPointerClick = false;
				int num7 = 0;
				isG = false;
				int num8 = (int)(count - timePoint);
				int num9 = dyTran - Canvas.py;
				if (CRes.abs(num9) > 40 && num8 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num9 / num8 * 10;
				}
				if (CRes.abs(Canvas.dy()) > 10 * AvMain.hd)
				{
					num7 = 1;
				}
				timePoint = -1L;
				if (Math.abs(num) < 10 * AvMain.hd)
				{
					if (num2 <= 4)
					{
						isHide = false;
						timeOpen1 = 5;
						num7 = 1;
					}
					else if (!isHide)
					{
						click1();
						num7 = 1;
					}
				}
				trans = false;
				isClick = false;
				Canvas.isPointerRelease = false;
				if (num7 == 0)
				{
					Canvas.menuMain = null;
					return;
				}
			}
		}
		if (isClick && Canvas.isPointerRelease)
		{
			isClick = false;
			Canvas.isPointerRelease = false;
			Canvas.menuMain = null;
		}
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, x, y, w, h, -1, false);
		g.translate(x, y + 5 * AvMain.hd);
		g.setClip(0f, 0f, w, hDis);
		g.translate(0f, -cmy);
		for (int i = 0; i < list.size(); i++)
		{
			if (i == selected && !isHide)
			{
				g.setColor(16777215);
				g.fillRect(10 * AvMain.hd, i * hCell, w - 20 * AvMain.hd, hCell);
			}
			Command command = (Command)list.elementAt(i);
			Canvas.normalFont.drawString(g, command.caption, w / 2, hCell / 2 + i * hCell - Canvas.normalFont.getHeight() / 2, 2);
		}
	}
}
