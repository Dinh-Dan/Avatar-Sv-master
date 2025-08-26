public class MoneySV
{
	public int x;

	public int y;

	public int valuea;

	public int typePaint;

	public int addFrom;

	public int addTo;

	public int xto;

	public int yto;

	public int xbg;

	public bool move;

	public bool isMoveOK;

	public MoneySV(int x, int y, int xto, int yto, int valuea, int typePaint, int addFrom, int addTo, bool isMoveOK)
	{
		this.x = x;
		this.y = y;
		this.valuea = valuea;
		this.typePaint = typePaint;
		this.addFrom = addFrom;
		this.addTo = addTo;
		this.xto = xto;
		this.yto = yto;
		this.isMoveOK = isMoveOK;
		xbg = BCBoardScr.rW - BCBoardScr.rWT;
	}

	public void update()
	{
		if (x != xto)
		{
			if (xto - x >> 1 == 0)
			{
				x = xto;
			}
			else
			{
				x += xto - x >> 1;
			}
		}
		if (y != yto)
		{
			if (yto - y >> 1 == 0)
			{
				y = yto;
			}
			else
			{
				y += yto - y >> 1;
			}
		}
		if (isMoveOK && x == xto && y == yto)
		{
			move = true;
		}
	}

	public void paint(MyGraphics g)
	{
		if (!move)
		{
			int num = x + BCBoardScr.rW / 4 + typePaint % 2 * BCBoardScr.rW / 2;
			int num2 = y + BCBoardScr.hH / 4 + typePaint / 2 * BCBoardScr.hH / 2;
			if (AvatarData.getImgIcon(870).count != -1)
			{
				g.drawRegion(AvatarData.getImgIcon(870).img, 0f, typePaint * BCBoardScr.hHT, BCBoardScr.rWT, BCBoardScr.hHT, 0, num, num2, 3);
				Canvas.smallFontYellow.drawString(g, valuea + string.Empty, num, num2 - AvMain.hSmall / 2, 2);
			}
		}
	}
}
