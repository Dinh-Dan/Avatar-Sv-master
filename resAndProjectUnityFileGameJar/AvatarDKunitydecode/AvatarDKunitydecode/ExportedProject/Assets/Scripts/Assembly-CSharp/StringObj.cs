public class StringObj : SubObject
{
	public string str;

	public string str2;

	public int w2;

	public int dir = 1;

	public int dis;

	public int anthor;

	public StringObj()
	{
	}

	public StringObj(string st, int ww)
	{
		str = st;
		w2 = ww;
	}

	public StringObj(int x, int y, string str)
	{
		base.x = x;
		base.y = y;
		this.str = str;
	}

	public void reset()
	{
		dir = 1;
		dis = 0;
	}

	public void transTextLimit(int limit)
	{
		dis += dir;
		if (dis > w2 - limit)
		{
			dis = -20;
		}
	}
}
