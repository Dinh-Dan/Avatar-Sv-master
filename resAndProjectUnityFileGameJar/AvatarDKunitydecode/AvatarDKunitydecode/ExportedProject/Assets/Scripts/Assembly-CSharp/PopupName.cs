public class PopupName : SubObject
{
	private string name;

	public sbyte num;

	public sbyte iPrivate;

	public PopupName(string name, int x, int y)
	{
		base.x = x;
		base.y = y;
		this.name = name;
		num = (sbyte)CRes.rnd(8);
	}

	public override void update()
	{
		num++;
		if (num >= 8)
		{
			num = 0;
		}
	}

	public override void paint(MyGraphics g)
	{
		if (OptionScr.gI().mapFocus[1] != 1 && (!MainMenu.gI().isGO || iPrivate != 0) && !((float)(x * MyObject.hd) < AvCamera.gI().xCam) && !((float)(x * MyObject.hd) > AvCamera.gI().xCam + (float)Canvas.w) && !((float)(y * MyObject.hd) < AvCamera.gI().yCam) && !((float)(y * MyObject.hd) > AvCamera.gI().yCam + (float)Canvas.hCan + 10f))
		{
			g.drawImage(LoadMap.imgShadow, x * MyObject.hd, y * MyObject.hd, 3);
			if (MiniMap.imgArrow != null)
			{
				MiniMap.imgArrow.drawFrame(0, x * MyObject.hd, (y - 10 + num / 2) * MyObject.hd, 0, MyGraphics.BOTTOM | MyGraphics.HCENTER, g);
			}
			Canvas.smallFontYellow.drawString(g, name, x * MyObject.hd, (y - 32 + num / 2) * MyObject.hd, 2);
		}
	}
}
