using UnityEngine;

public class FlyTextInfo
{
	private string text = string.Empty;

	private int x;

	private int y;

	private int state;

	private int delay;

	private bool isSmall;

	private Image img;

	private int imgID = -1;

	private int imgID_2;

	private sbyte dir;

	private sbyte normal = -1;

	public FlyTextInfo(int x0, int y0, int text1, int dir1, Image img1, int delay1, int imgID, int imgID_2)
	{
		delay = delay1;
		dir = (sbyte)dir1;
		x = x0;
		y = y0;
		if (text1 > 0)
		{
			text = "+";
		}
		text += text1;
		if (text1 == 0)
		{
			text = string.Empty;
		}
		img = img1;
		isSmall = false;
		normal = -1;
		this.imgID = imgID;
		this.imgID_2 = imgID_2;
	}

	public FlyTextInfo(int x0, int y0, string text1, int dir1, int type, int delay1)
	{
		delay = delay1;
		dir = (sbyte)dir1;
		x = x0;
		y = y0;
		text = text1;
		state = 0;
		isSmall = true;
		normal = (sbyte)type;
		imgID = -1;
		imgID_2 = -1;
	}

	public void update()
	{
		if (delay > 0)
		{
			delay--;
			return;
		}
		state++;
		if (state > 40)
		{
			img = null;
			Canvas.flyTexts.removeElement(this);
		}
		if (state < 3)
		{
			y += -2 * dir;
		}
		else
		{
			y += dir;
		}
	}

	public void paint(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(Vector2.one * AvMain.zoom, Vector2.zero);
		if (Canvas.currentMyScreen == RaceScr.me)
		{
			Canvas.resetTrans(g);
		}
		if (delay > 0)
		{
			GUIUtility.ScaleAroundPivot(Vector2.one / AvMain.zoom, Vector2.zero);
			return;
		}
		int num = AvMain.hd;
		if ((Canvas.currentMyScreen == BoardScr.me && (BoardScr.isStartGame || BoardScr.disableReady)) || Canvas.currentMyScreen == RaceScr.me)
		{
			num = 1;
		}
		if (isSmall)
		{
			if (normal == 0)
			{
				Canvas.smallFontRed.drawString(g, text, x * num, y * num, 2);
			}
			else
			{
				Canvas.borderFont.drawString(g, text, x * num, y * num, 2);
			}
		}
		else
		{
			Canvas.numberFont.drawString(g, text, x * num, y * num, 2);
			if (img == null)
			{
				if (imgID != -1)
				{
					FarmData.paintImg(g, imgID, x * num, (y - 5) * num, MyGraphics.HCENTER | MyGraphics.BOTTOM);
				}
				else if (imgID_2 != -1)
				{
					AvatarData.paintImg(g, imgID_2, x * num, (y - 5) * num, MyGraphics.HCENTER | MyGraphics.BOTTOM);
				}
			}
			else if (img != null)
			{
				g.drawImage(img, x * num, y * num, MyGraphics.BOTTOM | MyGraphics.HCENTER);
			}
		}
		GUIUtility.ScaleAroundPivot(Vector2.one / AvMain.zoom, Vector2.zero);
	}
}
