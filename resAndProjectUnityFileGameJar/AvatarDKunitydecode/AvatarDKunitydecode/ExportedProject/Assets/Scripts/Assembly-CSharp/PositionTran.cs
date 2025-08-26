public class PositionTran
{
	public int x;

	public int y;

	public int xTo;

	public int yTo;

	public int cmdx;

	public int cmvx;

	public int cmdy;

	public int cmvy;

	public PositionTran(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void update()
	{
		if (x != xTo)
		{
			cmvx = xTo - x << 2;
			cmdx += cmvx;
			x += cmdx >> 4;
			cmdx &= 15;
		}
		if (y != yTo)
		{
			cmvy = yTo - y << 2;
			cmdy += cmvy;
			y += cmdy >> 4;
			cmdy &= 15;
		}
	}
}
