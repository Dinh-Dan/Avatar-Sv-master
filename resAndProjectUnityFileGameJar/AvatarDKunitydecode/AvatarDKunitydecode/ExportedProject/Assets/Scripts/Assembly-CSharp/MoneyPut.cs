public class MoneyPut
{
	public int x;

	public int y;

	public int valuea;

	public int typePaint;

	public static int fl;

	public MoneyPut(int x, int y, int valuea, int typePaint)
	{
		this.x = x;
		this.y = y;
		this.valuea = valuea;
		this.typePaint = typePaint;
	}

	public void paint(MyGraphics g)
	{
		ImageIcon imgIcon = AvatarData.getImgIcon((short)((Canvas.w <= 200) ? 871 : 870));
		if (imgIcon.count != -1)
		{
			g.drawRegion(imgIcon.img, 0f, typePaint * BCBoardScr.hHT, BCBoardScr.rWT, BCBoardScr.hHT, 0, x, y, 3);
			Canvas.smallFontYellow.drawString(g, valuea + string.Empty, x, y - AvMain.hSmall / 2, 2);
		}
	}
}
