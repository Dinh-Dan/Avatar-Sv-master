public class PimgBC
{
	public int type;

	public int moneyPut;

	public int x;

	public int y;

	public void paint(MyGraphics g)
	{
		if (AvatarData.getImgIcon(872).count != -1)
		{
			g.drawRegion(AvatarData.getImgIcon(872).img, 0f, type * BCBoardScr.hH, BCBoardScr.rW, BCBoardScr.hH, 0, x, y, 0);
		}
	}
}
