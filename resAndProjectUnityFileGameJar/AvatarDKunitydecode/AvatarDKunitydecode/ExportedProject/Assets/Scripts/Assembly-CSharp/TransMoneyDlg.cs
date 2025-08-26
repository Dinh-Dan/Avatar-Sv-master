using System;

public class TransMoneyDlg : Dialog
{
	private class IActionTransXeng : IAction
	{
		public void perform()
		{
			GlobalService.gI().transXeng(gI().money[gI().focus]);
			Canvas.startWaitDlg();
		}
	}

	private FrameImage imgButton;

	public static TransMoneyDlg me;

	private int x;

	private int y;

	private int w;

	private int h;

	private int hItem;

	private int wItem;

	private int focus;

	private int[] money;

	public static TransMoneyDlg gI()
	{
		return (me != null) ? me : (me = new TransMoneyDlg());
	}

	public override void show()
	{
		init();
		Canvas.currentDialog = this;
	}

	private void init()
	{
		if (imgButton == null)
		{
			try
			{
				imgButton = new FrameImage(Image.createImagePNG(T.mode[AvMain.hd - 1] + "/hd/on/button"), 65 * AvMain.hd, (AvMain.hd != 2) ? 18 : 37);
			}
			catch (Exception)
			{
			}
			w = imgButton.frameWidth * 3 + 30 * AvMain.hd;
			h = imgButton.frameHeight * 3 + 60 * AvMain.hd;
			x = (Canvas.w - w) / 2;
			y = (Canvas.h - h) / 2;
			hItem = h / 3;
			wItem = w / 3;
			money = new int[9] { 100, 1000, 10000, 50000, 100000, 500000, 1000000, 5000000, 10000000 };
			center = new Command(T.selectt, 0, this);
			right = new Command(T.close, 1, this);
		}
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			Canvas.startOKDlg(T.doYouWantTransMoney, new IActionTransXeng());
			break;
		case 1:
			Canvas.currentDialog = null;
			break;
		}
	}

	public void update()
	{
	}

	public override void updateKey()
	{
		Canvas.paint.updateKeyOn(left, center, right);
		if (Canvas.isKeyPressed(2))
		{
			if (focus / 3 > 0)
			{
				focus -= 3;
			}
		}
		else if (Canvas.isKeyPressed(4))
		{
			if (focus % 3 > 0)
			{
				focus--;
			}
		}
		else if (Canvas.isKeyPressed(6))
		{
			if (focus % 3 < 2)
			{
				focus++;
			}
		}
		else if (Canvas.isKeyPressed(8) && focus / 3 < 2)
		{
			focus += 3;
		}
		if (!Canvas.isPointerClick)
		{
			return;
		}
		for (int i = 0; i < money.Length; i++)
		{
			if (Canvas.isPoint(x + i % 3 * wItem, y + i / 3 * hItem, wItem, hItem))
			{
				Canvas.isPointerClick = false;
				focus = i;
				break;
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		onMainMenu.gI().paintMain(g);
		Canvas.resetTrans(g);
		Canvas.paint.paintTransMoney(g, x, y, w, h);
		g.translate(x, y);
		for (int i = 0; i < money.Length; i++)
		{
			imgButton.drawFrame((focus == i) ? 1 : 0, wItem / 2 + i % 3 * wItem, hItem / 2 + i / 3 * hItem, 0, 3, g);
			Canvas.smallFontYellow.drawString(g, money[i] + string.Empty, wItem / 2 + i % 3 * wItem, hItem / 2 + i / 3 * hItem - AvMain.hSmall / 2, 2);
		}
		Canvas.resetTrans(g);
		OnScreen.paintBar(g, left, center, right, null);
	}
}
