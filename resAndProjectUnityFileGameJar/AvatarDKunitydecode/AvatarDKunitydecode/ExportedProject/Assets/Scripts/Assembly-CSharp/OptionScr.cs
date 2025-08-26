using System;
using System.IO;

public class OptionScr : MyScreen
{
	public static OptionScr instance;

	private int point;

	private int focus;

	private int max = 5;

	public int[] mapFocus;

	public int volume = 10;

	private int xL;

	private new int hText;

	public const int SHOW_NAME = 0;

	public const int SHOW_DIRECTION = 1;

	public const int SOUND = 2;

	private MyScreen lastScr;

	public static bool isVirTualKey;

	private bool[] isPaint;

	public int x;

	public int y;

	public int w;

	public int h;

	public int hCell;

	private bool isTranKey;

	private new int indexLeft = -1;

	private new int indexRight = -1;

	public OptionScr()
	{
		isPaint = new bool[max];
	}

	public static OptionScr gI()
	{
		if (instance == null)
		{
			instance = new OptionScr();
		}
		return instance;
	}

	public override void switchToMe()
	{
		initSize();
		lastScr = Canvas.currentMyScreen;
		base.switchToMe();
		load();
		mapFocus[2] = 1;
		if (!Main.isCompactDevice)
		{
			volume = 0;
			mapFocus[0] = 0;
			mapFocus[1] = 0;
			mapFocus[2] = 0;
		}
		else if (volume == 0)
		{
			mapFocus[2] = 0;
		}
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		if (index == 0)
		{
			doClose();
		}
	}

	public void initSize()
	{
		center = new Command(T.complete, 0, this);
		hText = MyScreen.hText;
		xL = Canvas.h;
		int num = PaintPopup.hTab + AvMain.hDuBox * 2 - 10;
		if (isPaint != null)
		{
			w = 200 * AvMain.hd;
			h = 200 * AvMain.hd;
			x = (Canvas.w - w) / 2;
			y = (Canvas.hCan - h) / 2;
			hCell = h / 4;
			for (int i = 0; i < 3; i++)
			{
				isPaint[i] = true;
			}
			isPaint[3] = true;
			mapFocus = new int[max];
		}
	}

	protected void doClose()
	{
		save(volume);
		lastScr.switchToMe();
	}

	public void save(int volume)
	{
		this.volume = volume;
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeByte((sbyte)volume);
			for (int i = 0; i < max; i++)
			{
				dataOutputStream.writeByte((sbyte)mapFocus[i]);
			}
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		try
		{
			RMS.saveRMS("avatarShowName", dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception e2)
		{
			Out.logError(e2);
		}
		init();
	}

	public void load()
	{
		initSize();
		DataInputStream dataInputStream = AvatarData.initLoad("avatarShowName");
		isVirTualKey = false;
		if (dataInputStream == null)
		{
			return;
		}
		try
		{
			volume = dataInputStream.readByte();
			mapFocus = new int[max];
			for (int i = 0; i < max; i++)
			{
				mapFocus[i] = dataInputStream.readByte();
				if (mapFocus[i] > 1)
				{
					mapFocus[i] = 0;
				}
			}
			dataInputStream.close();
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		init();
	}

	private void init()
	{
		Canvas.paint.setLanguage();
	}

	public override void updateKey()
	{
		base.updateKey();
		if (!Main.isCompactDevice)
		{
			return;
		}
		if (Canvas.isPointerClick)
		{
			int num = hCell;
			for (int i = 0; i < 3; i++)
			{
				if (Canvas.isPoint(x + 90 * AvMain.hd - 20 * AvMain.hd, y + (i + 1) * hCell - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					isTranKey = true;
					indexLeft = i;
					Canvas.isPointerClick = false;
				}
				else if (Canvas.isPoint(x + w - 30 * AvMain.hd - 20 * AvMain.hd, y + (i + 1) * hCell - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					isTranKey = true;
					indexRight = i;
					Canvas.isPointerClick = false;
				}
				num += hCell;
			}
		}
		if (!isTranKey)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			if (indexLeft != -1 && !Canvas.isPoint(x + 90 * AvMain.hd - 20 * AvMain.hd, y + (indexLeft + 1) * hCell - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				indexLeft = -1;
			}
			else if (indexRight != -1 && !Canvas.isPoint(x + w - 30 * AvMain.hd - 20 * AvMain.hd, y + (indexRight + 1) * hCell - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				indexRight = -1;
			}
		}
		if (Canvas.isPointerRelease)
		{
			Canvas.isPointerRelease = false;
			isTranKey = false;
			if (indexLeft != -1)
			{
				setMapFocus(-1, indexLeft);
				indexLeft = -1;
			}
			else if (indexRight != -1)
			{
				setMapFocus(1, indexRight);
				indexRight = -1;
			}
		}
	}

	private void setMapFocus(int dir, int i)
	{
		if (i == 2)
		{
			volume += dir * 10;
			if (volume < 0)
			{
				volume = 0;
			}
			if (volume > 100)
			{
				volume = 100;
			}
			SoundManager.setVolume(volume / 100);
		}
		else if (mapFocus[i] == 0)
		{
			mapFocus[i] = 1;
		}
		else
		{
			mapFocus[i] = 0;
		}
	}

	public override void update()
	{
		lastScr.update();
		if (xL != 0)
		{
			xL += -xL >> 1;
			if (xL < 0)
			{
				xL = 0;
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		lastScr.paintMain(g);
		paintMain(g);
		base.paint(g);
		g.setColor(0);
	}

	public override void paintMain(MyGraphics g)
	{
		g.translate(0f - g.getTranslateX(), 0f - g.getTranslateY());
		g.translate(0f, xL);
		Canvas.paint.paintPopupBack(g, x, y + 10 * AvMain.hd, w, h - 20 * AvMain.hd, -1, false);
		g.translate(x, y);
		if (point >= 4)
		{
			point = 0;
		}
		int num = hCell;
		for (int i = 0; i < 3; i++)
		{
			if (isPaint[i])
			{
				g.drawImage(PaintPopup.imgMuiIOS[(indexLeft == i) ? 1 : 0][2], 90 * AvMain.hd, num, 3);
				g.drawImage(PaintPopup.imgMuiIOS[(indexRight == i) ? 1 : 0][3], w - 30 * AvMain.hd, num, 3);
				Canvas.normalFont.drawString(g, T.name[i][2], 10 * AvMain.hd, num - Canvas.normalFont.getHeight() / 2, 0);
				Canvas.normalFont.drawString(g, T.name[i][mapFocus[i]], 90 * AvMain.hd + (w - 30 * AvMain.hd - 90 * AvMain.hd) / 2, num - Canvas.normalFont.getHeight() / 2, 2);
				num += hCell;
			}
		}
		Canvas.normalFont.drawString(g, volume + string.Empty, 90 * AvMain.hd + (w - 30 * AvMain.hd - 90 * AvMain.hd) / 2, num - Canvas.normalFont.getHeight() / 2 - hCell, 2);
		point++;
	}
}
