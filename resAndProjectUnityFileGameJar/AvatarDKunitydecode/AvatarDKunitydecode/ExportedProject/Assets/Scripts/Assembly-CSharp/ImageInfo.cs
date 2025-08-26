public class ImageInfo
{
	public short ID;

	public short bigID;

	public short x0;

	public short y0;

	public short w;

	public short h;

	public void paintFarm(MyGraphics g, int x, int y, int arthor)
	{
		g.drawRegion(FarmData.imgBig[bigID], x0 * AvMain.hd, y0 * AvMain.hd, w * AvMain.hd, h * AvMain.hd, 0, x, y, arthor);
	}

	public void paintPart(MyGraphics g, int x, int y, int arthor)
	{
		g.drawRegion(AvatarData.getBigImgInfo(bigID).img, x0 * AvMain.hd, y0 * AvMain.hd, w * AvMain.hd, h * AvMain.hd, 0, x, y, arthor);
	}

	public void paintPart(MyGraphics g, int x, int y, int dir, int arthor)
	{
		g.drawRegion(AvatarData.getBigImgInfo(bigID).img, x0 * AvMain.hd, y0 * AvMain.hd, w * AvMain.hd, h * AvMain.hd, dir, x, y, arthor);
	}
}
