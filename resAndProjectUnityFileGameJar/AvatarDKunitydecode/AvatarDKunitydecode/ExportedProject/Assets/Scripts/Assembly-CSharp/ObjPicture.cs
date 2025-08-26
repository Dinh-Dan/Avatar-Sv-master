public class ObjPicture : SubObject
{
	private Image img;

	public ObjPicture(int x, int y, Image im)
	{
		base.x = x;
		base.y = y;
		img = im;
		w = (short)im.getWidth();
	}

	public override void paint(MyGraphics g)
	{
		if (!((float)(x * MyObject.hd + w / 2) < AvCamera.gI().xCam) && !((float)(x * MyObject.hd - w / 2) > AvCamera.gI().xCam + (float)Canvas.w))
		{
			g.drawImage(img, x * MyObject.hd, y * MyObject.hd, MyGraphics.BOTTOM | MyGraphics.HCENTER);
		}
	}
}
