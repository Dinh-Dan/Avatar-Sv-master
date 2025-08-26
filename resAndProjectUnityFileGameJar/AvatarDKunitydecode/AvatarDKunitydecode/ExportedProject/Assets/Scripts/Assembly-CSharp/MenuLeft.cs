public class MenuLeft : MenuMain
{
	public static MenuLeft me;

	public MyVector list;

	private Image imgBack;

	private Image[][] imgIcon;

	private int x;

	private int y;

	private int w;

	private int h;

	private int hCell;

	private int selected = -1;

	private bool isTranKey;

	private bool isClick;

	private int count;

	private int countCur;

	private int timeOpen;

	public static MenuLeft gI()
	{
		return (me != null) ? me : new MenuLeft();
	}

	public void startAt(MyVector list)
	{
		if (imgBack == null)
		{
			imgBack = Image.createImagePNG(T.getPath() + "/iconMenu/backMenu");
			x = 32 * AvMain.hd - imgBack.w / 2;
			y = 28 * AvMain.hd;
			w = imgBack.w;
			h = imgBack.h - 11 * AvMain.hd;
		}
		hCell = h / list.size();
		imgIcon = new Image[list.size()][];
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			imgIcon[i] = new Image[2];
			imgIcon[i][0] = Image.createImagePNG(T.getPath() + "/iconMenu/" + command.indexImage + "0");
			imgIcon[i][1] = Image.createImagePNG(T.getPath() + "/iconMenu/" + command.indexImage + "1");
		}
		this.list = list;
		selected = -1;
		count = 0;
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
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.isPointerClick)
		{
			isClick = true;
			Canvas.isPointerClick = false;
			if (Canvas.isPoint(x, y + Canvas.countTab, w, h))
			{
				isTranKey = true;
			}
			countCur = count;
		}
		if (isTranKey)
		{
			if (Canvas.isPointerDown)
			{
				if (CRes.abs(Canvas.dx()) < 10 * AvMain.hd && CRes.abs(Canvas.dy()) < 10 * AvMain.hd)
				{
					if (count - countCur > 3)
					{
						int num = (Canvas.py - (y + Canvas.countTab)) / hCell;
						if (num >= 0 && num < list.size())
						{
							selected = num;
						}
					}
				}
				else if (selected != -1)
				{
					int num2 = (Canvas.py - (y + Canvas.countTab)) / hCell;
					if (num2 != selected)
					{
						selected = -1;
					}
				}
			}
			if (Canvas.isPointerRelease)
			{
				isTranKey = false;
				if (CRes.abs(Canvas.dx()) < 10 * AvMain.hd && CRes.abs(Canvas.dy()) < 10 * AvMain.hd)
				{
					int num3 = (Canvas.py - y) / hCell;
					if (num3 >= 0 && num3 < list.size())
					{
						selected = num3;
					}
				}
				if (selected != -1)
				{
					click();
				}
				Canvas.isPointerRelease = false;
			}
		}
		if (isClick && Canvas.isPointerRelease)
		{
			isClick = false;
			Canvas.isPointerRelease = false;
			Canvas.menuMain = null;
		}
	}

	private void click()
	{
		close();
		Command command = (Command)list.elementAt(selected);
		command.perform();
		selected = -1;
	}

	public void close()
	{
		Canvas.menuMain = null;
		imgIcon = null;
		imgBack = null;
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.drawImage(imgBack, x, 19 * AvMain.hd + Canvas.countTab, 0);
		g.translate(x, y + Canvas.countTab);
		for (int i = 0; i < list.size(); i++)
		{
			Command command = (Command)list.elementAt(i);
			g.drawImage(imgIcon[command.indexImage][(selected == i) ? 1 : 0], w / 2, hCell / 2 + i * hCell, 3);
		}
	}
}
