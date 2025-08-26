public class Kiss
{
	private int xCur;

	private int yCur;

	private int[] x;

	private int[] y;

	private sbyte[] index;

	private sbyte[] dir;

	private sbyte[] dis;

	public Kiss(int xC, int yC)
	{
		xCur = xC;
		yCur = yC;
		x = new int[3];
		y = new int[3];
		index = new sbyte[3];
		dir = new sbyte[3];
		dis = new sbyte[3];
		for (int i = 0; i < 3; i++)
		{
			index[i] = (sbyte)CRes.rnd(8);
			y[i] = -i * 20;
			dir[i] = (sbyte)((CRes.rnd(2) == 0) ? 1 : (-1));
			dis[i] = 6;
		}
	}

	public void update()
	{
		for (int i = 0; i < 3; i++)
		{
			y[i]--;
			if (y[i] < -60)
			{
				y[i] = 0;
				dis[i] = 6;
			}
			ref sbyte reference = ref index[i];
			reference++;
			if (index[i] == 6)
			{
				index[i] = 0;
			}
			x[i] += dir[i] * 2;
			if (dir[i] == 1)
			{
				if (x[i] > 10 - CRes.abs(y[i] / 10))
				{
					dir[i] = -1;
					if (dis[i] > 0)
					{
						ref sbyte reference2 = ref dis[i];
						reference2--;
					}
				}
			}
			else
			{
				if (x[i] < -(10 - CRes.abs(y[i] / 10)))
				{
					dir[i] = 1;
				}
				if (dis[i] > 0)
				{
					ref sbyte reference3 = ref dis[i];
					reference3--;
				}
			}
		}
	}

	public void paint(MyGraphics g)
	{
		for (int i = 0; i < 3; i++)
		{
			Avatar.imgKiss.drawFrame(index[i] / 3, (xCur + x[i]) * AvMain.hd, (yCur + y[i]) * AvMain.hd, 0, 3, g);
		}
	}
}
