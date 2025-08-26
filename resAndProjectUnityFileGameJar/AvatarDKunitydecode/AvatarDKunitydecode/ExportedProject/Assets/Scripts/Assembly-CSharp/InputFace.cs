using UnityEngine;

public class InputFace : Face
{
	public static InputFace me;

	public TField[] list;

	private string title;

	private int x;

	private int y;

	private int w;

	private int h;

	private int focus;

	private sbyte countClose;

	public IAction iAcClose;

	private int wTab;

	private string[][] nameChangePass;

	private bool isTranKey;

	public InputFace()
	{
		w = 190 * AvMain.hd;
		x = (Canvas.w - w) / 2;
	}

	public static InputFace gI()
	{
		return (me != null) ? me : (me = new InputFace());
	}

	public override void commandTab(int index)
	{
		if (index == 0)
		{
			if (iAcClose != null)
			{
				iAcClose.perform();
			}
			close();
		}
		else
		{
			Canvas.currentMyScreen.commandTab(index);
		}
	}

	public override void init(int h)
	{
		h = Canvas.h;
		setInfo(list, title, nameChangePass, center, h);
	}

	public void setInfo(TField[] list, string title, string[][] subName, Command cmd, int hh)
	{
		center = cmd;
		this.title = title;
		this.list = list;
		nameChangePass = subName;
		h = list[0].height * list.Length + 20 * AvMain.hd * (list.Length + 1) + 6 * AvMain.hd;
		if (!TouchScreenKeyboard.visible)
		{
			y = (hh - Canvas.transTab - ((AvMain.hFillTab == 0) ? Canvas.hTab : AvMain.hFillTab) - h) / 2;
		}
		else
		{
			y = (hh - ((AvMain.hFillTab == 0) ? Canvas.hTab : AvMain.hFillTab) - h) / 2;
		}
		int num = y + 3 * AvMain.hd;
		int num2 = 0;
		for (int i = 0; i < list.Length; i++)
		{
			for (int j = 0; j < subName[i].Length; j++)
			{
				if (num2 < Canvas.normalFont.getWidth(subName[i][j]))
				{
					num2 = Canvas.normalFont.getWidth(subName[i][j]);
				}
			}
		}
		for (int k = 0; k < list.Length; k++)
		{
			list[k].autoScaleScreen = true;
			list[k].showSubTextField = false;
			list[k].width = w - 30 * AvMain.hd - num2;
			list[k].x = x + w - list[k].width - 10 * AvMain.hd;
			num += 20 * AvMain.hd;
			list[k].y = num;
			num += list[k].height;
		}
		wTab = Canvas.normalFont.getWidth(title) + 50 * AvMain.hd;
		if (wTab < 80 * AvMain.hd)
		{
			wTab = 80 * AvMain.hd;
		}
		setFocus();
	}

	public override void updateKey()
	{
		base.updateKey();
		if (Canvas.isPointerClick && Canvas.isPointer(x + w - 24 * AvMain.hd, y - 19 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
		{
			countClose = 5;
			Canvas.isPointerClick = false;
			isTranKey = true;
		}
		if (isTranKey)
		{
			if (Canvas.isPointerDown && countClose == 5 && !Canvas.isPoint(x + w - 24 * AvMain.hd, y - 19 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranKey = false;
				if (countClose == 5)
				{
					countClose = 0;
					if (iAcClose != null)
					{
						iAcClose.perform();
					}
					close();
				}
			}
		}
		for (int i = 0; i < list.Length; i++)
		{
			list[i].update();
		}
		if (Canvas.isPointerClick && ipKeyboard.tk != null)
		{
			ipKeyboard.close();
			Canvas.isPointerClick = false;
		}
	}

	private void setFocus()
	{
		for (int i = 0; i < list.Length; i++)
		{
			list[i].setFocus(false);
		}
		list[focus].setFocus(true);
		right = null;
	}

	public override void keyPress(int keyCode)
	{
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i].isFocused())
			{
				list[i].keyPressed(keyCode);
			}
		}
		base.keyPress(keyCode);
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, x, y, w, h, countClose / 3, false);
		for (int i = 0; i < list.Length; i++)
		{
			g.setClip(x + 4 * AvMain.hd, y, w - 8 * AvMain.hd, h);
			int num = list[i].x - Canvas.normalFont.getWidth(nameChangePass[i][0]) - 5;
			if (num > x + 10 * AvMain.hd)
			{
				num = x + 10 * AvMain.hd;
			}
			int num2 = 2;
			if (nameChangePass[i][1].Equals(string.Empty))
			{
				num2 = 1;
			}
			for (int j = 0; j < num2; j++)
			{
				Canvas.normalFont.drawString(g, nameChangePass[i][j], num, list[i].y + list[i].height / 2 - AvMain.hNormal * num2 / 2 + AvMain.hNormal * j, 0);
			}
			list[i].paint(g);
		}
		base.paint(g);
	}
}
