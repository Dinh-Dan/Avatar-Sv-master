public class Xingau
{
	public int x;

	public int index;

	public int idFrame;

	public int y;

	public int type;

	public int typeStop;

	public static int[][] array = new int[3][];

	public bool stopHere;

	public static sbyte wImg;

	public static sbyte hImg;

	public Xingau(int x, int y, int type, int typeStop, bool stopHere)
	{
		this.x = x;
		this.y = y;
		this.type = type;
		this.typeStop = typeStop;
		this.stopHere = stopHere;
		hImg = 50;
		wImg = 54;
		if (AvMain.hd == 2)
		{
			wImg = (hImg = 108);
		}
	}

	public void paint(MyGraphics g)
	{
		if (AvatarData.getImgIcon(874).count != -1)
		{
			g.drawRegion(AvatarData.getImgIcon(874).img, 0f, idFrame * hImg, wImg, hImg, 0, x, y, MyGraphics.TOP | MyGraphics.HCENTER);
		}
	}

	public void update()
	{
		if (!stopHere)
		{
			if (Canvas.gameTick % 2 == 0)
			{
				index++;
				if (index > array[type].Length - 1)
				{
					index = 0;
				}
			}
			idFrame = array[type][index];
		}
		else
		{
			idFrame = typeStop;
		}
	}
}
