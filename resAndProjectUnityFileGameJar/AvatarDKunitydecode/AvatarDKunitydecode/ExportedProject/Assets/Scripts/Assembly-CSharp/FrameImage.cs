public class FrameImage
{
	public int frameWidth;

	public int frameHeight;

	public int nFrame;

	public Image imgFrame;

	public FrameImage(Image img, int width, int height)
	{
		imgFrame = img;
		frameWidth = width;
		frameHeight = height;
		nFrame = img.getHeight() / height;
	}

	public static FrameImage init(string path, int w, int h)
	{
		return new FrameImage(FilePack.getImg(path), w, h);
	}

	public void drawFrame(int idx, int x, int y, int trans, int orthor, MyGraphics g)
	{
		if (idx >= 0 && idx < nFrame)
		{
			g.drawRegion(imgFrame, 0f, idx * frameHeight, frameWidth, frameHeight, trans, x, y, orthor);
		}
	}

	public void drawFrame(int idx, int x, int y, int trans, MyGraphics g)
	{
		g.drawRegion(imgFrame, 0f, idx * frameHeight, frameWidth, frameHeight, trans, x, y, 0);
	}

	public void drawFrameXY(int idx, int idy, int x, int y, int anthor, MyGraphics g)
	{
		if (idx >= 0 && idx < nFrame)
		{
			g.drawRegion(imgFrame, idx * frameWidth, idy * frameHeight, frameWidth, frameHeight, 0, x, y, anthor);
		}
	}

	public void drawFrameXY(int idx, int x, int y, int anthor, MyGraphics g)
	{
		if (idx >= 0)
		{
			g.drawRegion(imgFrame, idx / nFrame * frameWidth, idx % nFrame * frameHeight, frameWidth, frameHeight, 0, x, y, anthor);
		}
	}
}
