public class MenuIcon : MenuMain
{
	public static MenuIcon me;

	private MyVector list;

	private int xCenter;

	private int yCenter;

	private int wCell;

	private int hCell;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int hDis;

	public int selected;

	private PositionTran[] tranPos;

	private Image[] imgFocus;

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

	public static MenuIcon gI()
	{
		return (me != null) ? me : (me = new MenuIcon());
	}

	public void setInfo(MyVector list, int xCen, int yCen)
	{
		if (imgFocus == null)
		{
			imgFocus = new Image[2];
			for (int i = 0; i < 2; i++)
			{
				imgFocus[i] = Image.createImagePNG(T.getPath() + "/iconMenu/focusAction" + i);
			}
		}
		this.list = list;
		tranPos = new PositionTran[list.size()];
		hCell = 45 * AvMain.hd;
		xCenter = xCen;
		yCenter = yCen;
		for (int j = 0; j < list.size(); j++)
		{
			tranPos[j] = new PositionTran(xCenter, yCenter);
			tranPos[j].xTo = tranPos[j].x;
			tranPos[j].yTo = 25 * AvMain.hd + Canvas.countTab + j * hCell;
		}
		wCell = 30 * AvMain.hd;
		hDis = Canvas.hCan;
		cmyLim = list.size() * hCell - (hDis - Canvas.countTab * 2);
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
		for (int i = 0; i < list.size(); i++)
		{
			tranPos[i].update();
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

	public void click1()
	{
		trans = false;
		Command command = (Command)list.elementAt(selected);
		command.perform();
		Canvas.menuMain = null;
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.isPointerClick)
		{
			isClick = true;
			Canvas.isPointerClick = false;
			if (Canvas.isPoint(xCenter - wCell - wCell / 2, Canvas.countTab, wCell * 3, hDis))
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
				if (count % 2 == 0)
				{
					dyTran = Canvas.py;
					timePoint = count;
				}
				vY = 0;
				if (Math.abs(num) < 10 * AvMain.hd)
				{
					int countTab = Canvas.countTab;
					int num3 = (cmtoY + Canvas.py - countTab) / hCell;
					if (num3 >= 0 && num3 < list.size())
					{
						selected = num3;
					}
				}
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num2 > 3 && num2 < 8)
				{
					int countTab2 = Canvas.countTab;
					int num4 = (cmtoY + Canvas.py - countTab2) / hCell;
					if (num4 >= 0 && num4 < list.size() && !isG)
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
				int num5 = 0;
				isG = false;
				int num6 = (int)(count - timePoint);
				int num7 = dyTran - Canvas.py;
				if (CRes.abs(num7) > 40 && num6 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num7 / num6 * 10;
				}
				if (CRes.abs(Canvas.dy()) > 10 * AvMain.hd)
				{
					num5 = 1;
				}
				timePoint = -1L;
				if (Math.abs(num) < 10 * AvMain.hd)
				{
					if (num2 <= 4)
					{
						isHide = false;
						timeOpen1 = 5;
						num5 = 1;
					}
					else if (!isHide)
					{
						click1();
						num5 = 1;
					}
				}
				trans = false;
				isClick = false;
				Canvas.isPointerRelease = false;
				if (num5 == 0)
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
		base.updateKey();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		g.translate(0f, -cmy);
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			if (!isHide && i == selected)
			{
				g.drawImage(imgFocus[1], tranPos[i].x, tranPos[i].y, 3);
			}
			else
			{
				g.drawImage(imgFocus[0], tranPos[i].x, tranPos[i].y, 3);
			}
			Canvas.smallFontYellow.drawString(g, command.caption, tranPos[i].x, tranPos[i].y - 20 * AvMain.hd - AvMain.hSmall / 2, 2);
			command.paint(g, tranPos[i].x, tranPos[i].y);
		}
		base.paint(g);
	}
}
