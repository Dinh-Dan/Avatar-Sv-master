public class Point : MyObject
{
	public Layer layer;

	public int g;

	public int v;

	public new int w;

	public new int h;

	public int color;

	public int limitY;

	public int countFr;

	public sbyte dis;

	public short itemID;

	public bool isFire;

	public bool isRemove;

	public short yTo;

	public short xTo;

	public short distant;

	public Point()
	{
	}

	public Point(int x, int y)
	{
		base.x = x;
		base.y = y;
	}

	public Point(int x, int y, int id)
	{
		base.x = x;
		base.y = y;
		xTo = (short)x;
		yTo = (short)y;
		itemID = (short)id;
	}

	public void setPosTo(int xT, int yT)
	{
		xTo = (short)xT;
		yTo = (short)yT;
		distant = (short)CRes.distance(x, y, xTo, yTo);
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
		if (x != xTo)
		{
			x += (xTo - x) / 2;
		}
		if (y != yTo)
		{
			y += (yTo - y) / 2;
		}
		if (CRes.distance(x, y, xTo, yTo) <= distant / 5)
		{
			return 2;
		}
		return 1;
	}

	public override void update()
	{
		layer.update();
	}

	public override void paint(MyGraphics g)
	{
		layer.paint(g, x, y);
	}
}
