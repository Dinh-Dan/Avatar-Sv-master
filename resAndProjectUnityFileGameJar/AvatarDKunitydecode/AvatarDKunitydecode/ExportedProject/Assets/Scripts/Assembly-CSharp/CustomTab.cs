using System;
using System.Globalization;

public class CustomTab : Face
{
	private class IActionCT : IAction
	{
		private readonly sbyte idAction;

		public IActionCT(sbyte idAction)
		{
			this.idAction = idAction;
		}

		public void perform()
		{
			GlobalService.gI().doCustomTab(idAction);
		}
	}

	public static CustomTab me;

	public int x;

	public int xChange;

	public int y;

	public int w;

	public int h;

	public MyVector listLabel = new MyVector();

	public MyVector listPic = new MyVector();

	public MyHashTable listImg;

	public string title = string.Empty;

	public string strTemp = string.Empty;

	public int selected;

	public int x0 = 5;

	public int y0 = 5;

	private int wTab = 30;

	public sbyte idAction;

	public sbyte countClose;

	private long time;

	public static int[] timeSub;

	public static int cmtoY;

	public static int cmy;

	public static int cmdy;

	public static int cmvy;

	public static int cmyLim;

	public static int yL;

	public static int wStr = 14;

	private int ww;

	private bool trans;

	private bool tranKey;

	private bool changeFocus;

	public int pa;

	public int pb;

	public int vY;

	public int dyTran;

	public bool transY;

	private long timePointY;

	private long count;

	public CustomTab()
	{
		setSize();
		wStr = Canvas.blackF.getHeight() + 5 * AvMain.hd;
	}

	public static CustomTab gI()
	{
		return (me != null) ? me : (me = new CustomTab());
	}

	private void setSize()
	{
		w = Canvas.w - 20;
		h = Canvas.hCan - 20;
	}

	public void init()
	{
		setSize();
		setInfo(listImg, title, strTemp, idAction);
	}

	private void setFont(string str)
	{
		int num = str.IndexOf("Ę");
		if (num != -1)
		{
			string str2 = str.Substring(0, num);
			setCanhle(str2, string.Empty);
			str = str.Substring(num + 1);
			int num2 = str.IndexOf("\n");
			if (num2 != -1)
			{
				string str3 = str.Substring(0, num2);
				setCanhle(str3, "Ę");
				str = str.Substring(num2 + 1);
				setFont(str);
			}
			else
			{
				setCanhle(str, "Ę");
			}
		}
		else
		{
			setCanhle(str, string.Empty);
		}
	}

	private void setCanhle(string str, string tem)
	{
		if (str.Equals(string.Empty))
		{
			return;
		}
		int num = str.IndexOf("ę");
		if (num != -1)
		{
			string text = str.Substring(0, num);
			if (!text.Equals(string.Empty))
			{
				addd(text, tem, 0);
			}
			int anthor = int.Parse(str.Substring(num + 1, 1));
			str = str.Substring(num + 2, str.Length - (num + 2));
			int num2 = str.IndexOf("\n");
			if (num2 != -1)
			{
				addd(str.Substring(0, num2), tem, anthor);
				setCanhle(str.Substring(num2 + 1), tem);
			}
			else
			{
				addd(str, tem, anthor);
			}
		}
		else
		{
			addd(str, tem, 0);
		}
	}

	private void addd(string str, string tem, int anthor)
	{
		int num = 0;
		string[] array = null;
		if (str.IndexOf("tem") != -1)
		{
			num = 1;
			array = Canvas.blackF.splitFontBStrInLine(str, w - 60 * AvMain.hd);
		}
		else
		{
			array = Canvas.tempFont.splitFontBStrInLine(str, w - 60 * AvMain.hd);
		}
		for (int i = 0; i < array.Length; i++)
		{
			StringObj stringObj = new StringObj(x0, y0 += wStr, tem + array[i]);
			stringObj.anthor = anthor;
			listLabel.addElement(stringObj);
		}
	}

	public void setInfo(MyHashTable list, string title, string str, sbyte idAction)
	{
		count = 0L;
		ww = 0;
		center = null;
		if (idAction != -1)
		{
			center = new Command(T.selectt, new IActionCT(idAction));
		}
		this.idAction = idAction;
		ww = 0;
		wTab = Canvas.tempFont.getWidth(title) + 80 * AvMain.hd;
		if (wTab < 80 * AvMain.hd)
		{
			wTab = 80 * AvMain.hd;
		}
		listImg = list;
		this.title = title;
		strTemp = str;
		listLabel.removeAllElements();
		listPic.removeAllElements();
		bool flag = false;
		x0 = 0;
		y0 = -10;
		while (true)
		{
			int num = str.IndexOf("µ");
			if (num == -1)
			{
				break;
			}
			string text = str.Substring(0, num);
			str = str.Substring(num + 1, str.Length - (num + 1));
			if (flag)
			{
				int num2 = text.IndexOf(",");
				string text2 = text.Substring(0, num2);
				text = text.Substring(num2 + 1, text.Length - (num2 + 1));
				int num3 = text.IndexOf(",");
				int num4 = int.Parse(text.Substring(0, num3));
				text = text.Substring(num3 + 1, text.Length - (num3 + 1));
				bool flag2 = int.Parse(text) != 0;
				Image image = (Image)listImg.get(text2 + string.Empty);
				int num5 = 0;
				switch (num4)
				{
				case 17:
					num5 = 1;
					break;
				case 24:
					num5 = 2;
					break;
				}
				PictureObj pictureObj = new PictureObj(int.Parse(text2), num5, y0 + wStr + 5, num4, flag2);
				pictureObj.w = image.getWidth();
				pictureObj.h = image.getHeight();
				if (flag2)
				{
					PictureObj pictureObj2 = (PictureObj)listPic.elementAt(listPic.size() - 1);
					int height = ((Image)listImg.get(pictureObj2.ID + string.Empty)).getHeight();
					if (image.getHeight() > height)
					{
						pictureObj2.y += image.getHeight() - height;
					}
					pictureObj.y = pictureObj2.y + height - image.getHeight();
				}
				y0 = pictureObj.y + image.getHeight() - 10;
				image = null;
				listPic.addElement(pictureObj);
				text = string.Empty;
			}
			flag = !flag;
			while (true)
			{
				int num6 = text.IndexOf("¶");
				if (num6 != -1)
				{
					string text3 = text.Substring(0, num6);
					text = text.Substring(num6 + 1, text.Length - (num6 + 1));
					try
					{
						setFont("¶" + text3);
						y0 -= wStr / 2;
					}
					catch (Exception e)
					{
						Out.logError(e);
						setFont(text3);
					}
				}
				else
				{
					if (!text.Equals(string.Empty))
					{
						setFont(text.Substring(0, text.Length - 1));
					}
					if (str.IndexOf("µ") != -1 || str.IndexOf("¶") == -1)
					{
						break;
					}
					text = str;
					str = string.Empty;
				}
			}
		}
		setFont(str);
		xChange = 9 * AvMain.hd;
		int num7 = 0;
		for (int i = 0; i < listLabel.size(); i++)
		{
			StringObj stringObj = (StringObj)listLabel.elementAt(i);
			FontX fontX = null;
			fontX = ((stringObj.str.IndexOf("tem") == -1) ? Canvas.tempFont : Canvas.blackF);
			int width = fontX.getWidth(stringObj.str);
			if (width + 100 * AvMain.hd > num7)
			{
				num7 = width + 100 * AvMain.hd;
			}
		}
		if (num7 < w)
		{
			w = num7;
			xChange = 10 * AvMain.hd;
		}
		if (y0 + 10 + wStr * 2 < h - 30)
		{
			h = y0 + 10 + wStr * 2 + 20;
		}
		if (w > Canvas.w - 100 * AvMain.hd)
		{
			w = Canvas.w;
			h = Canvas.hCan;
		}
		if (w < wTab * 2)
		{
			w = wTab * 2;
		}
		if (w > Canvas.w)
		{
			w = Canvas.w;
		}
		cmyLim = y0 - (h - PaintPopup.hTab - 2 * AvMain.hDuBox - wStr * 2);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmy = (cmtoY = 0);
		x = (Canvas.w - w) / 2;
		y = (Canvas.hCan - h) / 2;
		time = Environment.TickCount;
	}

	public override void updateKey()
	{
		if (Environment.TickCount - time >= 1000)
		{
			if (timeSub != null)
			{
				for (int i = 0; i < timeSub.Length; i++)
				{
					if (timeSub[i] > 0)
					{
						timeSub[i]--;
						if (timeSub[i] == 0)
						{
							FarmService.gI().doStealInfo();
						}
					}
				}
			}
			time = Environment.TickCount;
		}
		count++;
		bool flag = false;
		if (yL != 0)
		{
			yL += -yL >> 1;
		}
		if (yL == -1)
		{
			yL = 0;
		}
		int num = Canvas.w - 45 * AvMain.hd;
		int num2 = 15 * AvMain.hd;
		if (w != Canvas.w)
		{
			num = x + w - 20 * AvMain.hd;
			num2 = y + 20 * AvMain.hd;
		}
		if (Canvas.isPointerClick && Canvas.isPointer(num, num2, 40 * AvMain.hd, 40 * AvMain.hd))
		{
			countClose = 5;
			tranKey = true;
			Canvas.isPointerClick = false;
		}
		if (tranKey)
		{
			if (Canvas.isPointerDown && !Canvas.isPointer(num, num2, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				tranKey = false;
				if (countClose == 5)
				{
					countClose = 0;
					listLabel.removeAllElements();
					listPic.removeAllElements();
					listImg.clear();
					close();
					Canvas.endDlg();
					me = null;
				}
			}
		}
		if (Canvas.isPointerClick && Canvas.isPointer(x, y, w, h) && !trans)
		{
			pa = cmy;
			trans = true;
			vY = 0;
		}
		if (trans)
		{
			if (Canvas.isPointerDown)
			{
				int num3 = Canvas.dy();
				if (Canvas.gameTick % 3 == 0)
				{
					dyTran = Canvas.py;
					timePointY = count;
				}
				vY = 0;
				cmtoY = pa + num3;
				if (cmtoY < 0 || cmtoY > cmyLim)
				{
					cmtoY = pa + num3 / 2;
				}
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease)
			{
				int num4 = (int)(count - timePointY);
				int num5 = dyTran - Canvas.py;
				if (CRes.abs(num5) > 40 && num4 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num5 / num4 * 10;
				}
				timePointY = -1L;
				trans = false;
			}
		}
		if (Canvas.keyHold[2])
		{
			cmtoY -= 14;
			flag = true;
		}
		else if (Canvas.keyHold[8])
		{
			flag = true;
			cmtoY += 14;
		}
		if (flag)
		{
			if (cmtoY < 0)
			{
				cmtoY = 0;
			}
			if (cmtoY > cmyLim)
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
		if (Math.abs(cmtoY - cmy) < 15 && cmy < 0)
		{
			cmtoY = 0;
		}
		if (Math.abs(cmtoY - cmy) < 10 && cmy > cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (vY != 0)
		{
			int num6 = h - PaintPopup.hTab - 8 * AvMain.hd;
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
				if (cmy < -num6 / 2)
				{
					cmy = -num6 / 2;
					cmtoY = 0;
					vY = 0;
				}
			}
			else if (cmy > cmyLim)
			{
				if (cmy < cmyLim + num6 / 2)
				{
					cmy = cmyLim + num6 / 2;
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
		base.updateKey();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		Canvas.paint.paintTransBack(g);
		Canvas.paint.paintBoxTab(g, x, y, h, w, 0, PaintPopup.gI().wSub, wTab, 40 * AvMain.hd, 1, 1, PaintPopup.gI().count, PaintPopup.gI().colorTab, title, -1, -1, false, (w == Canvas.w) ? true : false, null, 0f, null);
		g.setClip(x + 4, y + 45 * AvMain.hd, w - 8, h - 40 * AvMain.hd - 10);
		g.translate(x + xChange, y + 35 * AvMain.hd);
		g.translate(0f, -cmy);
		g.setColor(0);
		int num = 0;
		for (int i = 0; i < listLabel.size(); i++)
		{
			StringObj stringObj = (StringObj)listLabel.elementAt(i);
			if (stringObj.y <= cmy - 10 || stringObj.y >= cmy + h)
			{
				continue;
			}
			if (stringObj.str.Length > 2 && stringObj.str.Substring(0, 1).Equals("¶"))
			{
				int color = int.Parse(stringObj.str.Substring(1, stringObj.str.Length - 1), NumberStyles.HexNumber);
				PaintPopup.fill(stringObj.x, stringObj.y, Canvas.w - stringObj.x * 2, 1, color, g);
				continue;
			}
			int num2 = stringObj.x;
			if (stringObj.anthor == 2)
			{
				num2 += (w - 30) / 2 + 4;
			}
			else if (stringObj.anthor == 1)
			{
				num2 += w - 30 + 10;
			}
			if (stringObj.str.Length > 2 && stringObj.str.Substring(0, 1).Equals("Ę"))
			{
				Canvas.blackF.drawString(g, stringObj.str.Substring(1, stringObj.str.Length - 1), num2, stringObj.y, stringObj.anthor);
			}
			else if (stringObj.str.Length > 1 && stringObj.str.Substring(0, 1).Equals("0"))
			{
				Canvas.smallFontYellow.drawString(g, stringObj.str.Substring(1) + ((timeSub == null || timeSub[num] < 0) ? string.Empty : (" " + timeSub[num] / 60 + ":" + (timeSub[num] - timeSub[num] / 60 * 60))), num2, stringObj.y + AvMain.hNormal / 2 - AvMain.hSmall / 2 + 4, stringObj.anthor);
				num++;
			}
			else
			{
				Canvas.tempFont.drawString(g, stringObj.str, num2, stringObj.y, stringObj.anthor);
			}
		}
		for (int j = 0; j < listPic.size(); j++)
		{
			PictureObj pictureObj = (PictureObj)listPic.elementAt(j);
			if (pictureObj.y + pictureObj.h > cmy && pictureObj.y < cmy + h)
			{
				g.drawImage((Image)listImg.get(pictureObj.ID + string.Empty), pictureObj.x * ((w - xChange * 2) / 2), pictureObj.y, pictureObj.orthor);
			}
		}
		base.paint(g);
		if ((w == Canvas.w) ? true : false)
		{
			g.drawImage(ListScr.imgCloseTabFull[countClose / 3], Canvas.w - 25 * AvMain.hd, 35 * AvMain.hd, 3);
		}
		else
		{
			g.drawImage(ListScr.imgCloseTab[countClose / 3], x + w - 2 * AvMain.hd, y + 40 * AvMain.hd, 3);
		}
	}
}
