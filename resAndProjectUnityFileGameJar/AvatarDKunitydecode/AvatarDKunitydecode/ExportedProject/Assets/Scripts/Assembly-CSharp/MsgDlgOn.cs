using System;

public class MsgDlgOn : Dialog
{
	public string[] info;

	public bool isBusy;

	private int h;

	private int firstMills;

	private int currMills;

	private bool isPaint;

	public static FrameImage imgBusy;

	public override void show()
	{
		Canvas.currentDialog = this;
	}

	public void setInfo(string info, Command left, Command center, Command right)
	{
		this.info = Canvas.normalWhiteFont.splitFontBStrInLine(info, Canvas.w - 200 * ScaleGUI.numScale);
		base.left = left;
		base.center = center;
		base.right = right;
		if (info.Equals(T.pleaseWait))
		{
			isPaint = false;
			right = center;
			center = null;
		}
		else
		{
			isPaint = true;
		}
		firstMills = Environment.TickCount;
		h = 90 * ScaleGUI.numScale;
		if (this.info.Length >= 5)
		{
			h = this.info.Length * Canvas.normalWhiteFont.getHeight() + 20 * ScaleGUI.numScale;
		}
	}

	public override void updateKey()
	{
		Canvas.paint.updateKeyOn(left, center, right);
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		if (!isPaint)
		{
			currMills = Environment.TickCount;
			if (currMills - firstMills > 500)
			{
				isPaint = true;
			}
			return;
		}
		int num = Canvas.h - h - 30 + 5 * AvMain.hd;
		Canvas.paint.paintDefaultPopup(g, 80 * ScaleGUI.numScale, num, Canvas.w - 160 * ScaleGUI.numScale, h);
		int num2 = num + (h - info.Length * Canvas.normalWhiteFont.getHeight()) / 2;
		if (isBusy)
		{
			imgBusy.drawFrame(Canvas.gameTick % 24 / 2, Canvas.hw - 90 * ScaleGUI.numScale, num2 + 10 * ScaleGUI.numScale, 0, 3, g);
		}
		int num3 = 0;
		int num4 = num2;
		while (num3 < info.Length)
		{
			Canvas.normalWhiteFont.drawString(g, info[num3], Canvas.hw, num4, 2);
			num3++;
			num4 += Canvas.normalWhiteFont.getHeight();
		}
		Canvas.resetTrans(g);
		Canvas.paint.paintTabSoft(g);
		if (isPaint)
		{
			Canvas.paint.paintCmdBar(g, left, center, right);
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
			Canvas.currentDialog = null;
			break;
		default:
			Canvas.currentMyScreen.commandTab(index);
			break;
		}
	}
}
