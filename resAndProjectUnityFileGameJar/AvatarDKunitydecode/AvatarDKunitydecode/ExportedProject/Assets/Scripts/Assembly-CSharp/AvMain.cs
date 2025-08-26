public class AvMain
{
	public static int hd;

	public static int hDuBox;

	public static int duPopup;

	public static int hFillTab;

	public static int hCmd;

	public static sbyte hBlack;

	public static sbyte hBorder;

	public static sbyte hNormal;

	public static sbyte hSmall;

	public static sbyte hBlack2;

	public static float zoom;

	public static byte indexLeft;

	public static byte indexCenter;

	public static byte indexRight;

	public Command left;

	public Command center;

	public Command right;

	public bool isHide;

	public bool isTran;

	public static bool isQwerty;

	public static int lsplash;

	public static int csplash;

	public static int rsplash;

	public static string lsplashs;

	public static string csplashs;

	public static string rsplashs;

	static AvMain()
	{
		zoom = 1f;
		if (Main.hdtype == 0)
		{
			hd = 1;
		}
		if (Main.hdtype == 1)
		{
			hd = 1;
		}
		if (Main.hdtype == 2)
		{
			hd = 2;
		}
	}

	public virtual void keyPress(int keyCode)
	{
	}

	public virtual void paint(MyGraphics g)
	{
		Canvas.resetTransNotZoom(g);
		Canvas.paint.paintCmd(g, left, center, right);
	}

	public virtual void commandAction(int index)
	{
	}

	public virtual void commandActionPointer(int index, int subIndex)
	{
	}

	public virtual void commandTab(int index)
	{
	}

	public virtual void closeTabAll()
	{
	}

	public virtual void initTabTrans()
	{
	}

	private void click(Command cmd)
	{
		if (cmd != null)
		{
			Canvas.isPointerClick = false;
			Canvas.isPointerRelease = false;
			Canvas.endDlg();
			perform(cmd);
			SoundManager.playSound(7);
			indexRight = (indexCenter = (indexLeft = 0));
		}
	}

	public virtual void updateKey()
	{
		if (lsplash > 0)
		{
			lsplash--;
		}
		if (csplash > 0)
		{
			csplash--;
		}
		if (rsplash > 0)
		{
			rsplash--;
		}
		if (Canvas.currentDialog != null || Canvas.welcome == null || !Welcome.isPaintArrow || Welcome.lastScr != Canvas.currentMyScreen)
		{
			Canvas.paint.setDrawPointer(left, center, right);
			if (Canvas.currentMyScreen != MessageScr.me)
			{
				Canvas.paint.setBack();
			}
		}
		else if (Canvas.currentMyScreen != MessageScr.me && (Canvas.welcome == null || !Welcome.isPaintArrow || Welcome.lastScr != Canvas.currentMyScreen))
		{
			Canvas.paint.setBack();
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		switch (Canvas.paint.collisionCmdBar(left, center, right))
		{
		case 0:
			if (indexLeft == 4 || Canvas.stypeInt == 0)
			{
				click(left);
			}
			break;
		case 1:
			if (indexCenter == 4 || Canvas.stypeInt == 0)
			{
				click(center);
			}
			break;
		case 2:
			if (indexRight == 4 || Canvas.stypeInt == 0)
			{
				click(right);
			}
			break;
		}
		indexRight = (indexCenter = (indexLeft = 0));
	}

	public virtual void perform(Command cmd)
	{
		if (cmd != null)
		{
			if (cmd.action != null || cmd.ipaction != null)
			{
				cmd.perform();
			}
			else if (cmd.pointer != null)
			{
				cmd.pointer.commandActionPointer(cmd.indexMenu, cmd.subIndex);
			}
			else
			{
				commandTab(cmd.indexMenu);
			}
		}
	}
}
