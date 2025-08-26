public class Card
{
	public sbyte phom;

	public sbyte cardID;

	public int x;

	public int y;

	public int distant;

	public bool isSelected;

	public bool isShow;

	public bool isUp;

	public int cmdy;

	public int cmvy;

	public int cmdx;

	public int cmvx;

	public int[] cardMapping;

	public int cardType;

	public int cardValue;

	public int cardColor;

	public int yTo;

	public int xTo;

	public Card(sbyte ID, bool isPhom)
		: this(ID)
	{
		if (isPhom)
		{
			cardMapping = new int[13]
			{
				11, 12, 0, 1, 2, 3, 4, 5, 6, 7,
				8, 9, 10
			};
		}
	}

	public Card(sbyte ID)
	{
		cardID = ID;
		phom = 0;
		cardType = cardID % 4;
		cardValue = cardID / 4;
		cardColor = ((cardType >= 2) ? 1 : 0);
		cardMapping = new int[13]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12
		};
	}

	public static void copyData(Card a, Card b)
	{
		a.phom = b.phom;
		a.cardID = b.cardID;
		a.isSelected = b.isSelected;
		a.isShow = b.isShow;
		a.isUp = b.isUp;
		for (int i = 0; i < b.cardMapping.Length; i++)
		{
			a.cardMapping[i] = b.cardMapping[i];
		}
		a.cardType = b.cardType;
		a.cardValue = b.cardValue;
		a.cardColor = b.cardColor;
	}

	public void paintHalf(MyGraphics g)
	{
		Canvas.paint.paintHalf(g, this);
	}

	public void paintHalfBackFull(MyGraphics g)
	{
		Canvas.paint.paintHalfBackFull(g, this);
	}

	public void paintFull(MyGraphics g)
	{
		Canvas.paint.paintFull(g, this);
	}

	public void paintSmall(MyGraphics g, bool isCh)
	{
		Canvas.paint.paintSmall(g, this, isCh);
	}

	public void setPosTo(int xT, int yT)
	{
		xTo = xT;
		yTo = yT;
		distant = CRes.distance(x, y, xTo, yTo);
	}

	public int translate()
	{
		if (x == xTo && y == yTo)
		{
			return -1;
		}
		if (Math.abs((xTo - x) / 2) <= 1 && Math.abs((yTo - y) / 2) <= 1)
		{
			x = xTo;
			y = yTo;
			return 0;
		}
		if (y != yTo)
		{
			cmvy = yTo - y << 2;
			cmdy += cmvy;
			y += cmdy >> 4;
			cmdy &= 15;
		}
		if (x != xTo)
		{
			cmvx = xTo - x << 2;
			cmdx += cmvx;
			x += cmdx >> 4;
			cmdx &= 15;
		}
		if (CRes.distance(x, y, xTo, yTo) >= distant - distant / 4)
		{
			return 3;
		}
		if (CRes.distance(x, y, xTo, yTo) <= distant / 5)
		{
			return 2;
		}
		return 1;
	}

	public bool setCollision()
	{
		if (Canvas.px >= x - BoardScr.wCard / 2 && Canvas.px <= x + BoardScr.wCard / 2 && Canvas.py >= y - BoardScr.hcard / 2 && Canvas.py <= y + BoardScr.hcard / 2)
		{
			return true;
		}
		return false;
	}
}
