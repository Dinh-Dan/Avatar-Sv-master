public class EffectData
{
	public sbyte[] arrFrame;

	public ImageInfo[] imgImfo;

	public Image img;

	public Frame[] frame;

	public short ID;

	public ImageInfo getImgInfo(sbyte id)
	{
		for (int i = 0; i < imgImfo.Length; i++)
		{
			if (imgImfo[i].ID == id)
			{
				return imgImfo[i];
			}
		}
		return null;
	}

	public void paint(MyGraphics g, int x, int y, int index)
	{
		Frame frame = this.frame[arrFrame[index]];
		for (int i = 0; i < frame.dx.Length; i++)
		{
			ImageInfo imgInfo = getImgInfo(frame.idImg[i]);
			g.drawRegion(img, imgInfo.x0 * AvMain.hd, imgInfo.y0 * AvMain.hd, imgInfo.w * AvMain.hd, imgInfo.h * AvMain.hd, 0, x * AvMain.hd + frame.dx[i] * AvMain.hd, y * AvMain.hd + frame.dy[i] * AvMain.hd, 0);
		}
	}
}
