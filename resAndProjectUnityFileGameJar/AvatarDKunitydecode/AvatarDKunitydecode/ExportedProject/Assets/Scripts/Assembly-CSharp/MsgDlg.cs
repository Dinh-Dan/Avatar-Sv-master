public class MsgDlg : Dialog
{
	protected MyVector info;

	private string str = string.Empty;

	public MyVector list = new MyVector();

	public int index;

	public int w;

	public int h;

	public int x;

	public int y;

	public int yWait;

	public bool isWaiting;

	private int num;

	private int size;

	private int hText;

	private int hDu;

	private new int indexLeft;

	private new int indexRight;

	public static int hCell;

	public static FrameImage imgLoad;

	public static Image imgLoadOn;

	private long timeDelay = -1L;

	private long limitTime;

	private long timeEnd;

	public static bool isBack = true;

	public long timeOpen;

	public static sbyte[] fr = new sbyte[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };

	public int xCow;

	private int wStr;

	private int frame;

	private int direct;

	public MsgDlg()
	{
		hText = AvMain.hBlack;
	}

	public override void show()
	{
	}

	public void setInfoLR(string info, Command lef, Command righ)
	{
		isWaiting = false;
		Canvas.currentDialog = null;
		w = Canvas.w - 100 * AvMain.hd;
		if (w > 300 * AvMain.hd)
		{
			w = 300 * AvMain.hd;
		}
		str = info;
		index = 0;
		timeDelay = -1L;
		left = lef;
		right = righ;
		center = null;
		init();
		Canvas.currentDialog = this;
	}

	public void setInfoLCR(string info, Command lef, Command cente, Command righ)
	{
		Canvas.currentDialog = null;
		isWaiting = false;
		w = Canvas.w - 60 * AvMain.hd;
		if (w > 350 * AvMain.hd)
		{
			w = 350 * AvMain.hd;
		}
		str = info;
		index = 0;
		timeDelay = -1L;
		left = lef;
		right = righ;
		center = cente;
		init();
		Canvas.currentDialog = this;
	}

	public void setInfoC(string info, Command cente)
	{
		isWaiting = false;
		Canvas.currentDialog = null;
		w = Canvas.w - 100 * AvMain.hd;
		if (w > 300 * AvMain.hd)
		{
			w = 300 * AvMain.hd;
		}
		if (cente == null && w > 500)
		{
			w = 500;
		}
		str = info;
		index = 0;
		timeDelay = -1L;
		left = null;
		right = null;
		center = null;
		center = cente;
		init();
		Canvas.currentDialog = this;
	}

	public void setDelay(int limit)
	{
		limitTime = limit;
		timeDelay = Canvas.getTick() / 100;
	}

	public void init()
	{
		if (Canvas.currentDialog == null && left == null && center == null && right == null)
		{
			timeOpen = Canvas.getTick() / 100;
			list.removeAllElements();
		}
		if (Canvas.load == 0 && onMainMenu.isOngame)
		{
			Canvas.load = -1;
		}
		isBack = true;
		if (str.Equals(T.pleaseWait))
		{
			w = Canvas.hw;
		}
		info = Canvas.tempFont.splitFontBStrInLineV(str, w - w / 3);
		h = AvMain.hCmd / 2 + 10 + info.size() * Canvas.tempFont.getHeight() + 30 * AvMain.hd;
		hDu = 0;
		x = Canvas.hw - w / 2;
		y = Canvas.hCan / 2 - h / 2;
		if (onMainMenu.isOngame)
		{
			if (center != null)
			{
				center.x = -1;
			}
			if (left != null)
			{
				left.x = -1;
			}
			if (right != null)
			{
				right.x = -1;
			}
		}
		else
		{
			if (center == null && left != null && right != null)
			{
				left.x = x + w / 2 - MyScreen.wTab / 2 - 10 * AvMain.hd;
				left.y = (right.y = y + h - AvMain.hCmd / 2);
				right.x = x + w / 2 + MyScreen.wTab / 2 + 10 * AvMain.hd;
			}
			if (left != null && center != null && right != null)
			{
				left.x = x + w / 2 - MyScreen.wTab - 20;
				left.y = y + h - AvMain.hCmd / 2;
				right.x = x + w / 2 + MyScreen.wTab + 20;
				right.y = y + h - AvMain.hCmd / 2;
				center.x = Canvas.hw;
				center.y = y + h - AvMain.hCmd / 2;
			}
			if (left == null && right == null && center != null)
			{
				center.y = y + h - AvMain.hCmd / 2;
				center.x = Canvas.hw;
			}
		}
		if (onMainMenu.isOngame)
		{
			y = Canvas.hCan - 36 * AvMain.hd - h - 10;
		}
	}

	public void setIsWaiting(bool isa)
	{
		isWaiting = isa;
		timeEnd = Canvas.getSecond();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		if (isWaiting && Canvas.getTick() / 100 - timeOpen < 4)
		{
			return;
		}
		if (isBack && !onMainMenu.isOngame)
		{
			Canvas.paint.paintTransBack(g);
		}
		if (onMainMenu.isOngame)
		{
			g.drawImageScale(imgLoadOn, x, y, w, h, 0);
		}
		else
		{
			Canvas.paint.paintPopupBack(g, x, y, w, h, -1, false);
		}
		int num = y + 4 * AvMain.hd + (h - 4 * AvMain.hd) / 2 - Canvas.tempFont.getHeight() * info.size() / 2 - 2 * AvMain.hd;
		if (left != null || center != null || right != null)
		{
			num = y + 4 * AvMain.hd + (h - 4 * AvMain.hd - AvMain.hCmd / 2) / 2 - Canvas.tempFont.getHeight() * info.size() / 2 - 2 * AvMain.hd;
		}
		if (isWaiting)
		{
			imgLoad.drawFrame(this.num / 2, x + w / 2 - Canvas.tempFont.getWidth((string)info.elementAt(0)) / 2 - 30 * AvMain.hd, y + h / 2, 0, 3, g);
		}
		for (int i = 0; i < info.size(); i++)
		{
			if (onMainMenu.isOngame)
			{
				Canvas.menuFont.drawString(g, (string)info.elementAt(i), Canvas.hw, y + h / 2 - Canvas.menuFont.getHeight() * info.size() / 2 + i * Canvas.tempFont.getHeight(), 2);
			}
			else
			{
				Canvas.tempFont.drawString(g, (string)info.elementAt(i), Canvas.hw, num + i * Canvas.tempFont.getHeight() - 3, 2);
			}
		}
		if (onMainMenu.isOngame)
		{
			Canvas.paint.paintTabSoft(g);
			Canvas.paint.paintCmdBar(g, left, center, right);
		}
		else
		{
			base.paint(g);
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case -2:
			MapScr.gI().doExitGame();
			break;
		case -1:
			isWaiting = false;
			Canvas.currentDialog = null;
			break;
		default:
			Canvas.currentMyScreen.commandTab(index);
			break;
		}
	}

	private void setIndex(int ina)
	{
		if (size > 0)
		{
			index += ina;
			if (index < 0)
			{
				index = size - 1;
			}
			if (index >= size)
			{
				index = 0;
			}
			Command command = (Command)list.elementAt(index);
			center.action = command.action;
			center.indexMenu = command.indexMenu;
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
		if (isWaiting)
		{
			num++;
			if (num >= 16)
			{
				num = 0;
			}
			if (Canvas.getSecond() - timeEnd > 30)
			{
				isWaiting = false;
				string text = string.Empty;
				for (int i = 0; i < info.size(); i++)
				{
					text = text + (string)info.elementAt(i) + " ";
				}
				Canvas.startOKDlg(T.doYouWantExit2, new GlobalLogicHandler.IActionDisconnect());
			}
		}
		if (timeDelay == -1 || Canvas.getTick() / 100 - timeDelay > limitTime)
		{
		}
		if (indexLeft > 0)
		{
			indexLeft--;
		}
		if (indexRight > 0)
		{
			indexRight--;
		}
		if (Canvas.isPointerClick)
		{
			if (!isWaiting && size > 0)
			{
				if (Canvas.isPointer(Canvas.hw - wStr / 2 - 11 - hCell, ((AvMain.hd != 2) ? (Canvas.normalFont.getHeight() / 2) : 0) + y + h - (hCell + 15 * AvMain.hd - 4) + hCell / 2 + 1 + ((AvMain.hd == 1) ? (-7) : 0) + ((AvMain.hd == 0) ? (-3) : 0) - hCell / 2, hCell * 2, hCell))
				{
					setIndex(-1);
					indexLeft = 5;
					Canvas.isPointerRelease = false;
				}
				else if (Canvas.isPointer(Canvas.hw - wStr / 2 - 11 - hCell + wStr + 20, ((AvMain.hd != 2) ? (Canvas.normalFont.getHeight() / 2) : 0) + y + h - (hCell + 15 * AvMain.hd - 4) + hCell / 2 + 1 + ((AvMain.hd == 1) ? (-7) : 0) + ((AvMain.hd == 0) ? (-3) : 0) - hCell / 2, hCell * 2, hCell))
				{
					setIndex(1);
					indexRight = 5;
					Canvas.isPointerRelease = false;
				}
			}
			if (!isWaiting && Canvas.isPointer(Canvas.hw - hCell, ((AvMain.hd != 2) ? (Canvas.normalFont.getHeight() / 2) : 0) + y + h - (hCell + 15 * AvMain.hd - 4) + hCell / 2 + 1 + ((AvMain.hd == 1) ? (-7) : 0) + ((AvMain.hd == 0) ? (-3) : 0) - hCell / 2, hCell * 2, hCell))
			{
				Canvas.endDlg();
				perform(center);
				Canvas.isPointerRelease = false;
				Canvas.paint.clickSound();
			}
		}
		base.updateKey();
	}
}
