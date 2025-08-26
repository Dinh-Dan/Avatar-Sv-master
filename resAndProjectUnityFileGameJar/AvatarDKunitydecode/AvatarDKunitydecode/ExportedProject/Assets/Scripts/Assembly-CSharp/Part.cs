public abstract class Part
{
	public short follow;

	public short IDPart;

	public short idIcon;

	public int[] price = new int[2];

	public sbyte zOrder;

	public sbyte sell;

	public string name;

	public virtual void paint(MyGraphics g, int x, int y, int arthor)
	{
		if (IDPart != -1)
		{
			if (IDPart >= 2000)
			{
				paintDynamic(g, idIcon, x, y, 0, arthor);
			}
			else
			{
				AvatarData.listImgInfo[idIcon].paintPart(g, x, y, arthor);
			}
		}
	}

	public virtual void paintIcon(MyGraphics g, int x, int y, int direct, int arthor)
	{
		paint(g, x, y, arthor);
	}

	public virtual void paintAvatar(MyGraphics g, int index, int x, int y, int direct, int arthor)
	{
	}

	public void paintDynamic(MyGraphics g, short idImg, int x, int y, int direct, int arthor)
	{
		ImageIcon imagePart = AvatarData.getImagePart(idImg);
		if (imagePart.count != -1 || IDPart == -1)
		{
			g.drawRegion(imagePart.img, 0f, 0f, imagePart.w, imagePart.h, direct, x, y, arthor);
		}
	}
}
