public class ParkListSrc : MyScreen
{
	public static ParkListSrc instance;

	public int[] listBoard;

	private MyScreen lastScr;

	private int maxW = 4;

	private int w;

	private int maxH = 4;

	private int x;

	private int y;

	private int wAll;

	private int h;

	private sbyte countClose;

	private FrameImage imgIcon;

	private Image imgFocus;

	private bool isTranKey;

	public ParkListSrc()
	{
		w = Canvas.stypeInt * 50;
		h = w * maxH;
		imgIcon = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/parkIcon"), 34 * AvMain.hd, 16 * AvMain.hd);
		imgFocus = Image.createImagePNG(T.getPath() + "/temp/statfocus");
	}

	public static ParkListSrc gI()
	{
		if (instance == null)
		{
			instance = new ParkListSrc();
		}
		return instance;
	}

	public void switchToMe(MyScreen lastScr)
	{
		base.switchToMe();
		this.lastScr = lastScr;
		selected = 0;
		isHide = true;
	}

	protected void doSelect()
	{
		Canvas.cameraList.close();
		lastScr.switchToMe();
		ParkService.gI().doJoinPark(MapScr.roomID, selected);
	}

	public override void setSelected(int se, bool isAc)
	{
		if (isAc && selected == se)
		{
			doSelect();
		}
		base.setSelected(se, isAc);
	}

	public void setList(int[] list)
	{
		listBoard = list;
		int num = listBoard.Length / maxW;
		if (listBoard.Length % maxW != 0)
		{
			num++;
		}
		x = Canvas.hw - (w * maxW + 10) / 2;
		y = Canvas.hCan / 2 - w * maxH / 2 + 40 * AvMain.hd / 2;
		wAll = w * maxW + 10;
		int num2 = listBoard.Length / maxW;
		if (listBoard.Length % maxW != 0)
		{
			num2++;
		}
		Canvas.cameraList.setInfo(x, y, w, w, maxW * w, num2 * w, w * maxW, h - AvMain.hDuBox, list.Length);
	}

	public override void updateKey()
	{
		if (Canvas.isPointerClick && Canvas.isPointer(x - 10 * AvMain.hd + (wAll + 20 * AvMain.hd) - 3 - 20 * AvMain.hd, y - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
		{
			isTranKey = true;
			countClose = 5;
			Canvas.isPointerClick = false;
		}
		if (isTranKey)
		{
			if (Canvas.isPointerDown && countClose == 5 && !Canvas.isPointer(x - 10 * AvMain.hd + (wAll + 20 * AvMain.hd) - 3 - 20 * AvMain.hd, y - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranKey = false;
				if (countClose == 5)
				{
					Canvas.cameraList.close();
					lastScr.switchToMe();
				}
			}
		}
		base.updateKey();
	}

	public override void update()
	{
		if (!isTranKey && countClose > 0)
		{
			countClose--;
			if (countClose == 0)
			{
				return;
			}
		}
		lastScr.update();
	}

	public override void paint(MyGraphics g)
	{
		g.translate(0f, 0f);
		g.setClip(0f, 0f, Canvas.w, Canvas.h);
		lastScr.paint(g);
		Canvas.paint.paintPopupBack(g, x - 10 * AvMain.hd, y, wAll + 20 * AvMain.hd, h, countClose / 3, false);
		paintList(g);
		base.paint(g);
	}

	private void paintList(MyGraphics g)
	{
		g.translate(Canvas.hw - (w * maxW + 10) / 2 + 4, Canvas.cameraList.y);
		g.setClip(0f, 4 * AvMain.hd, w * maxW + 2, Canvas.cameraList.disY);
		g.translate(1f, 0f - CameraList.cmy);
		if (!isHide)
		{
			g.drawImage(imgFocus, selected % maxW * w + w / 2, selected / maxW * w + w / 2, 3);
		}
		int num = (int)(CameraList.cmy / (float)w * (float)maxW - (float)maxW);
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num + maxH * maxW + maxW * 2;
		if (num2 > listBoard.Length)
		{
			num2 = listBoard.Length;
		}
		for (int i = num; i < num2; i++)
		{
			imgIcon.drawFrame(listBoard[i], i % maxW * w + w / 2, i / maxW * w + w / 2, 0, 3, g);
		}
		for (int j = num; j < num2; j++)
		{
			Canvas.smallFontYellow.drawString(g, j + string.Empty, j % maxW * w + w / 2, j / maxW * w + w / 2 - AvMain.hSmall / 2 - ((AvMain.hd == 1) ? 2 : 0), 2);
		}
	}
}
