using UnityEngine;

public class PaintPopup
{
	public static FrameImage frame;

	public static PaintPopup me;

	public static int[] color;

	public int h;

	public int w;

	public int x;

	public int y;

	public int numTab;

	public int wTab;

	public int wSub = 10;

	public int focusTab;

	public int maxTab;

	public int[] colorTab;

	public int[] count;

	public string name;

	public string[] nameList;

	public static sbyte hTab;

	public sbyte countCloseTab;

	public sbyte countName;

	private int wName;

	public static Image[][] imgMuiIOS;

	public bool isMenu;

	public bool isFull;

	public static float cmtoX;

	public static float cmx;

	public static float cmdx;

	public static float cmvx;

	public static float cmxLim;

	public static MyScreen parent;

	public static int wButtonSmall;

	public static int hButtonSmall;

	public static int xTab;

	public static int wTabDu;

	private Image[][] imgIcon;

	private int countTempName;

	private int pxLast;

	private int vY;

	private int timePoint;

	private int countTouch;

	private int timeDelay;

	private int dxTran;

	private int timeOpen;

	private int indexFocus = -1;

	private float pa;

	private float vX;

	private bool isG;

	private bool trans;

	private bool isHide;

	public PaintPopup()
	{
		wSub = 30;
		hTab = (sbyte)(40 * AvMain.hd);
	}

	public static PaintPopup gI()
	{
		if (me == null)
		{
			me = new PaintPopup();
		}
		return me;
	}

	public void setInfo(string na, int wp, int hp, int num, sbyte countCloseAll, string[] nameList, sbyte[] idIcon)
	{
		focusTab = 0;
		parent = null;
		isFull = false;
		isMenu = false;
		imgIcon = null;
		if (idIcon != null)
		{
			imgIcon = new Image[idIcon.Length][];
			for (int i = 0; i < idIcon.Length; i++)
			{
				imgIcon[i] = new Image[2];
				imgIcon[i][0] = Image.createImagePNG(T.getPath() + "/iconShop/shop" + idIcon[i] + "0");
				imgIcon[i][1] = Image.createImagePNG(T.getPath() + "/iconShop/shop" + idIcon[i] + "1");
			}
		}
		wSub = 70 * AvMain.hd;
		w = wp;
		h = hp;
		numTab = num;
		this.nameList = null;
		wTab = 0;
		if (nameList != null)
		{
			this.nameList = new string[nameList.Length];
			for (int j = 0; j < nameList.Length; j++)
			{
				if (nameList[j].Length > 10)
				{
					this.nameList[j] = nameList[j].Substring(0, 10).Trim() + "..";
				}
				else
				{
					this.nameList[j] = nameList[j];
				}
				int num2 = Canvas.normalFont.getWidth(this.nameList[j]) + 12 * AvMain.hd;
				if (num2 > wTab)
				{
					wTab = num2;
				}
				if (wTab < 65 * AvMain.hd)
				{
					wTab = 65 * AvMain.hd;
				}
				if (wTab > 100 * AvMain.hd)
				{
					wTab = 100 * AvMain.hd;
				}
			}
			if (nameList.Length > 3)
			{
				xTab = 8 * AvMain.hd;
				wTabDu = wTab + 4 * AvMain.hd;
			}
			setName(nameList[focusTab]);
		}
		else
		{
			setName(na);
			wTab = Canvas.normalFont.getWidth(na) + 12 * AvMain.hd;
			if (wTab < 65 * AvMain.hd)
			{
				wTab = 65 * AvMain.hd;
			}
			if (wTab > 100 * AvMain.hd)
			{
				wTab = 100 * AvMain.hd;
			}
		}
		if (idIcon != null)
		{
			wTab = 55 * AvMain.hd;
		}
		xTab = 12 * AvMain.hd;
		wTabDu = wTab + 10 * AvMain.hd;
		init();
		colorTab = new int[numTab];
		count = new int[numTab];
		maxTab = (w - wTab) / wSub;
		countCloseTab = countCloseAll;
		cmxLim = 0f;
		if (nameList != null)
		{
			cmxLim = nameList.Length * wSub - (w - 30 * AvMain.hd) + wSub;
			if (cmxLim < 0f)
			{
				cmxLim = 0f;
			}
			if (cmx > cmxLim)
			{
				cmx = (cmtoX = cmxLim);
			}
		}
		else
		{
			cmx = (cmtoX = 0f);
		}
	}

	public void setIcon(sbyte[] idIcon)
	{
	}

	public void setFocus(int focus)
	{
		int num = 0;
		if (isMenu)
		{
			num = 29 * AvMain.hd;
		}
		focusTab = focus;
		if ((float)x - cmx + (float)num + (float)(focusTab * wSub) <= 0f)
		{
			cmtoX = x + num + focusTab * wSub;
		}
		else if ((float)x - cmtoX + (float)num + (float)(focusTab * wSub) >= (float)(w - 30 * AvMain.hd - wTab))
		{
			cmtoX = x + num + focusTab * wSub - (w - 30 * AvMain.hd - wTab);
		}
	}

	public void setName(string na)
	{
		name = na;
		wName = Canvas.fontWhiteBold.getWidth(name);
	}

	public void init()
	{
		x = Canvas.hw - w / 2;
		y = (Canvas.hCan - (TouchScreenKeyboard.visible ? 10 : 0)) / 2 - h / 2;
	}

	public void setColor(int col, int index)
	{
		if (index != focusTab)
		{
			colorTab[index] = col;
			count[index] = CRes.rnd(20);
		}
	}

	public void setNameAndFocus(string na, int focus)
	{
		if (colorTab != null && focus < colorTab.Length)
		{
			colorTab[focus] = 0;
		}
		name = na;
		wName = Canvas.fontWhiteBold.getWidth(name);
		focusTab = focus;
	}

	public void setNumTab(int num)
	{
		colorTab = new int[num];
		count = new int[num];
		numTab = num;
	}

	public void update()
	{
		if (wName <= wTab - 15 * AvMain.hd)
		{
			return;
		}
		if (countName + wName / 2 <= (wTab - 15 * AvMain.hd) / 2)
		{
			if (countTempName == 20)
			{
				countName *= -1;
			}
			countTempName--;
		}
		else if (countTempName == 0 || countTempName == 40)
		{
			countTempName = 40;
			countName--;
		}
		else
		{
			countTempName--;
		}
	}

	public int setupdateTab()
	{
		countTouch++;
		if (Canvas.isPointerClick)
		{
			if (countCloseTab != -1 && Canvas.isPoint(x + w + 5 - 5 * AvMain.hd - 20 * AvMain.hd, y + hTab - 6 + 3 * AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				trans = true;
				Canvas.isPointerClick = false;
				countCloseTab = 5;
			}
			else
			{
				for (int i = 0; i < numTab; i++)
				{
					if (Canvas.isPoint(xTab + x + i * wTabDu, y, wTabDu, hTab))
					{
						Canvas.isPointerClick = false;
						pxLast = Canvas.pyLast;
						isG = false;
						if (vY != 0)
						{
							isG = true;
						}
						pa = cmtoX;
						timeDelay = countTouch;
						trans = true;
						indexFocus = i;
						break;
					}
				}
			}
		}
		if (trans)
		{
			int num = pxLast - Canvas.px;
			pxLast = Canvas.px;
			long num2 = countTouch - timeDelay;
			int num3 = 25;
			int num4 = wTab + 20;
			if (Canvas.isPointerDown)
			{
				if (countCloseTab != 0 && !Canvas.isPoint(x + w + 5 - 5 * AvMain.hd - 20 * AvMain.hd, y + hTab - 6 + 3 * AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					countCloseTab = 0;
				}
				else if (indexFocus != -1)
				{
					if (countTouch % 2 == 0)
					{
						dxTran = Canvas.px;
						timePoint = countTouch;
					}
					vY = 0;
					if (Math.abs(num) < 10 * AvMain.hd)
					{
						int num5 = x;
						int num6 = (int)((cmtoX + (float)Canvas.px - (float)num5) / (float)wTab);
						if (num6 >= 0 && num6 >= numTab)
						{
						}
					}
					if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
					{
						isHide = true;
					}
					else if (num2 > 3 && num2 < 8)
					{
						int countTab = Canvas.countTab;
						int num7 = (int)((cmtoX + (float)Canvas.px - (float)countTab) / (float)wTab);
						if (num7 >= 0 && num7 < numTab && !isG)
						{
							isHide = false;
						}
					}
					if (cmtoX < 0f || cmtoX > cmxLim)
					{
						cmtoX = pa + (float)(num / 2);
						pa = cmtoX;
					}
					else
					{
						cmtoX = pa + (float)(num / 2);
						pa = cmtoX;
					}
					cmx = cmtoX;
				}
			}
			if (Canvas.isPointerRelease)
			{
				trans = false;
				isG = false;
				if (countCloseTab == 5)
				{
					Canvas.currentMyScreen.closeTabAll();
					countCloseTab = 0;
				}
				else if (indexFocus != -1)
				{
					int num8 = countTouch - timePoint;
					int num9 = dxTran - Canvas.px;
					if (CRes.abs(num9) > 40 && num8 < 10 && cmtoX > 0f && cmtoX < cmxLim)
					{
						vX = num9 / num8 * 10;
					}
					timePoint = -1;
					if (Math.abs(Canvas.dx()) < 10 * AvMain.hd)
					{
						int result = indexFocus - focusTab;
						if (indexFocus > focusTab)
						{
							focusTab = indexFocus;
							return result;
						}
						if (indexFocus < focusTab)
						{
							focusTab = indexFocus;
							return result;
						}
					}
					indexFocus = -1;
				}
				Canvas.isPointerRelease = false;
			}
		}
		return 0;
	}

	public void paint(MyGraphics g)
	{
		Canvas.paint.paintBoxTab(g, x, y, h, w, focusTab, wSub, wTab, hTab, numTab, maxTab, count, colorTab, name, (sbyte)((countCloseTab == -1) ? countCloseTab : (countCloseTab / 3)), countName, isMenu, isFull, nameList, cmx, imgIcon);
		Canvas.resetTrans(g);
	}

	public static void paintCell(MyGraphics g, int x, int y, int w, int h)
	{
		fill(x, y, w, h, color[0], g);
		g.setColor(color[2]);
		g.drawRect(x, y, w, h);
		g.setColor(12450472);
		g.drawRect(x + 1, y + 1, w - 2, h - 2);
		g.setColor(5738823);
		g.drawRect(x + 2, y + 2, w - 4, h - 4);
	}

	public static void fill(int x, int y, int w, int h, int color, MyGraphics g)
	{
		g.setColor(color);
		g.fillRect(x, y, w, h);
	}
}
