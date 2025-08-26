public class ReportDlg : Dialog
{
	private class IActionFinishReport : IAction
	{
		public void perform()
		{
			Canvas.endDlg();
			instance = null;
		}
	}

	public MyVector list;

	private static ReportDlg instance;

	private int y;

	private int h;

	public ReportDlg()
	{
		center = new Command(T.OK, new IActionFinishReport());
	}

	public static ReportDlg gI()
	{
		if (instance == null)
		{
			instance = new ReportDlg();
		}
		return instance;
	}

	public override void show()
	{
		Canvas.currentDialog = this;
	}

	public void update()
	{
	}

	public override void updateKey()
	{
		Canvas.paint.updateKeyOn(left, center, right);
	}

	public override void paint(MyGraphics g)
	{
		y = Canvas.h - ((AvMain.hFillTab == 0) ? Canvas.hTab : AvMain.hFillTab) - h - 10;
		Canvas.paint.paintPopupBack(g, 8, y, Canvas.w - 16, h, -1, false);
		y += 5 + AvMain.hDuBox - Canvas.arialFont.getHeight() / 2;
		for (int i = 0; i < list.size(); i++)
		{
			string st = (string)list.elementAt(i);
			Canvas.arialFont.drawString(g, st, 40, y + 3, 0);
			y += Canvas.arialFont.getHeight();
		}
		Canvas.resetTrans(g);
		Canvas.paint.paintTabSoft(g);
		Canvas.paint.paintCmdBar(g, left, center, right);
	}

	public void setInfo(MyVector matchResult)
	{
		list = matchResult;
		h = list.size() * Canvas.normalFont.getHeight() + AvMain.hDuBox * 2 + 10;
	}
}
