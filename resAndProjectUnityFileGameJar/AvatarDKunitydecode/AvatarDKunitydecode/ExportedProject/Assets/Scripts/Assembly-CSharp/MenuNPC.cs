public class MenuNPC : MenuMain
{
	public static MenuNPC me;

	private int idUser;

	private int x;

	private int y;

	private int w;

	private int h;

	private int xList;

	private int yList;

	private int wList;

	private int hList;

	private int hItem;

	private int focus;

	public static FrameImage imgDc;

	private MyVector list = new MyVector();

	private string nameNPC;

	private string[] textChat;

	private bool[] isMenu;

	private Image imgBackPopup;

	private Image imgBackChat;

	private int pa;

	private int dyTran;

	private int timeOpen;

	private int pyLast;

	private bool trans;

	private bool isG;

	private long timeDelay;

	private long count;

	private long timePoint;

	private int vY;

	public int cmtoY;

	public int cmy;

	public int cmdy;

	public int cmvy;

	public int cmyLim;

	static MenuNPC()
	{
		imgDc = new FrameImage(Image.createImagePNG(T.getPath() + "/race/popup/tile0"), 20 * AvMain.hd, 20 * AvMain.hd);
	}

	public MenuNPC()
	{
		w = 250 * AvMain.hd;
		h = 187 * AvMain.hd;
		x = (Canvas.w - w) / 2;
		y = (Canvas.h - h) / 2;
		yList = 75 * AvMain.hd;
		wList = 160 * AvMain.hd;
		xList = w - wList - 13 * AvMain.hd;
		hItem = 30 * AvMain.hd;
		hList = 100 * AvMain.hd;
	}

	public static MenuNPC gI()
	{
		return (me != null) ? me : (me = new MenuNPC());
	}

	public void setInfo(MyVector list, int idUser, string nameNPC, string textChat, bool[] isMenu)
	{
		if (imgBackPopup == null)
		{
			imgBackPopup = Image.createImagePNG(T.getPath() + "/effect/backMenuNPC");
			imgBackChat = Image.createImagePNG(T.getPath() + "/effect/popupChat");
		}
		this.list = list;
		this.isMenu = isMenu;
		this.idUser = idUser;
		cmyLim = list.size() * hItem - (hList - 20 * AvMain.hd);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		this.nameNPC = nameNPC;
		this.textChat = Canvas.fontChatB.splitFontBStrInLine(textChat, w - 70 * AvMain.hd);
		show();
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
		moveCamera();
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.isPointerClick)
		{
			pyLast = Canvas.pyLast;
			isG = false;
			if (Canvas.isPoint(x + xList, y + yList, wList, hList))
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
					int num3 = y + yList + 10 * AvMain.hd;
					int num4 = hItem;
					int num5 = (cmtoY + Canvas.py - num3) / num4;
					if (num5 >= 0 && num5 < list.size())
					{
						focus = num5;
					}
				}
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num2 > 3 && num2 < 8)
				{
					int num6 = y + yList + 10 * AvMain.hd;
					int num7 = hItem;
					int num8 = (cmtoY + Canvas.py - num6) / num7;
					if (num8 >= 0 && num8 < list.size() && !isG)
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
			if (!Canvas.isPointerRelease || !Canvas.isPoint(x, y, w, h))
			{
				return;
			}
			isG = false;
			int num9 = (int)(count - timePoint);
			int num10 = dyTran - Canvas.py;
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
		else if (Canvas.isPointerRelease && !Canvas.isPoint(x, y, w, h))
		{
			Canvas.isPointerRelease = false;
			Canvas.menuMain = null;
		}
	}

	private void click()
	{
		if (!isMenu[focus])
		{
			Canvas.menuMain = null;
		}
		else
		{
			Canvas.startWaitDlg();
		}
		Command command = (Command)list.elementAt(focus);
		command.perform();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, x, y, w, h, -1, false);
		g.translate(x, y);
		g.drawImage(imgBackChat, 12 * AvMain.hd, 12 * AvMain.hd, 0);
		for (int i = 0; i < textChat.Length; i++)
		{
			Canvas.fontChatB.drawString(g, textChat[i], 20 * AvMain.hd, 12 * AvMain.hd + 25 * AvMain.hd - textChat.Length * Canvas.fontChatB.getHeight() / 2 + i * AvMain.hBlack + ((AvMain.hd == 1) ? 10 : 0), 0);
		}
		Avatar avatar = LoadMap.getAvatar(idUser);
		Canvas.normalFont.drawString(g, nameNPC, xList / 2, yList + hList / 2 - AvMain.hNormal - 20 * AvMain.hd, 2);
		avatar.paintIcon(g, xList / 2, yList + hList / 2 + avatar.height, false);
		g.translate(xList, yList);
		g.drawImage(imgBackPopup, 0f, 0f, 0);
		g.setClip(0f, 0f, w, hList);
		g.translate(0f, -cmy);
		for (int j = 0; j < list.size(); j++)
		{
			Command command = (Command)list.elementAt(j);
			if (j == focus && !isHide)
			{
				g.setColor(16777215);
				g.fillRect(4 * AvMain.hd, 10 * AvMain.hd + j * hItem, wList - 6 * AvMain.hd, hItem);
			}
			Canvas.normalFont.drawString(g, command.caption, 10 * AvMain.hd, 10 * AvMain.hd + j * hItem + hItem / 2 - AvMain.hNormal / 2, 0);
		}
		base.paint(g);
	}

	public static void paintPopupTilte(MyGraphics g, int x, int y, int w, int h, FrameImage img, int color)
	{
		img.drawFrame(0, x, y, 0, g);
		img.drawFrame(2, x + w - img.frameWidth, y, 0, g);
		img.drawFrame(5, x, y + h - img.frameHeight, 0, g);
		img.drawFrame(7, x + w - img.frameWidth, y + h - img.frameHeight, 0, g);
		for (int i = 0; i < (w - img.frameWidth * 2) / img.frameWidth; i++)
		{
			img.drawFrame(1, x + (i + 1) * img.frameWidth, y, 0, g);
			img.drawFrame(6, x + (i + 1) * img.frameWidth, y + h - img.frameHeight, 0, g);
		}
		img.drawFrame(1, x + w - img.frameWidth * 2, y, 0, g);
		img.drawFrame(6, x + w - img.frameWidth * 2, y + h - img.frameHeight, 0, g);
		for (int j = 0; j < (h - img.frameHeight * 2) / img.frameHeight; j++)
		{
			img.drawFrame(3, x, y + (j + 1) * img.frameHeight, 0, g);
			img.drawFrame(4, x + w - img.frameWidth, y + (j + 1) * img.frameHeight, 0, g);
		}
		img.drawFrame(3, x, y + h - img.frameHeight * 2, 0, g);
		img.drawFrame(4, x + w - img.frameWidth, y + h - img.frameHeight * 2, 0, g);
		if (color != -1)
		{
			g.setColor(color);
			g.fillRect(x + img.frameWidth, y + img.frameHeight, w - img.frameWidth * 2, h - img.frameHeight * 2);
		}
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
				if (cmy < -hList / 2)
				{
					cmy = -hList / 2;
					cmtoY = 0;
					vY = 0;
				}
			}
			else if (cmy > cmyLim)
			{
				if (cmy < cmyLim + hList / 2)
				{
					cmy = cmyLim + hList / 2;
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
}
