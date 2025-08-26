public class AvPosition
{
	public int x;

	public int y;

	public int anchor;

	public int xTo;

	public int yTo;

	public sbyte depth;

	public short index = -1;

	public AvPosition()
	{
		x = 0;
		y = 0;
	}

	public AvPosition(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public AvPosition(int x, int y, int anchor)
	{
		this.x = x;
		this.y = y;
		this.anchor = anchor;
	}

	public bool setDetect(AvPosition pos, int num)
	{
		if (CRes.abs(pos.x - x) <= num && CRes.abs(pos.y - y) <= num)
		{
			return true;
		}
		return false;
	}

	public bool setDetectX(int xx, int num)
	{
		if (CRes.abs(xx - x) <= num)
		{
			return true;
		}
		return false;
	}

	public bool setDetectY(int yy, int num)
	{
		if (CRes.abs(yy - y) <= num)
		{
			return true;
		}
		return false;
	}
}
