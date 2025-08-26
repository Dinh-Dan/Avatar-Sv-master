public class MapItem : SubObject
{
	public short ID;

	public short typeID;

	public new short w;

	public new short h;

	public sbyte dir;

	public bool isGetImg;

	public int xTo;

	public int yTo;

	public MapItem()
	{
	}

	public MapItem(int type, int x, int y, int id, short typeID)
	{
		base.type = (sbyte)type;
		base.x = x;
		base.y = y;
		ID = (short)id;
		this.typeID = typeID;
	}

	public void setPos(int x0, int y0)
	{
		x = (xTo = x0);
		y = (yTo = y0);
	}

	public int X()
	{
		MapItemType mapItemType = null;
		mapItemType = ((!isGetImg) ? AvatarData.getMapItemTypeByID(typeID) : LoadMap.getMapItemTypeByID(typeID));
		return x + mapItemType.dx;
	}

	public int Y()
	{
		MapItemType mapItemType = null;
		mapItemType = ((!isGetImg) ? AvatarData.getMapItemTypeByID(typeID) : LoadMap.getMapItemTypeByID(typeID));
		return y + mapItemType.dy;
	}

	public override void paint(MyGraphics g)
	{
		MapItemType mapItemType = null;
		mapItemType = ((!isGetImg) ? AvatarData.getMapItemTypeByID(typeID) : LoadMap.getMapItemTypeByID(typeID));
		if (!isGetImg && LoadMap.TYPEMAP != 68 && LoadMap.TYPEMAP != 69 && LoadMap.TYPEMAP != 70 && LoadMap.TYPEMAP != 110)
		{
			ImageInfo imageInfo = AvatarData.listImgInfo[mapItemType.imgID];
			if (w == 0)
			{
				w = imageInfo.w;
				h = imageInfo.h;
			}
			if (!((float)((x + mapItemType.dx + imageInfo.w) * MyObject.hd) < AvCamera.gI().xCam) && !((float)((x + mapItemType.dx - imageInfo.w) * MyObject.hd) > AvCamera.gI().xCam + (float)Canvas.w) && !((float)((y + imageInfo.h) * MyObject.hd) < AvCamera.gI().yCam) && !((float)((y + mapItemType.dy - imageInfo.h) * MyObject.hd) > AvCamera.gI().yCam + (float)Canvas.h))
			{
				imageInfo.paintPart(g, (x + mapItemType.dx) * MyObject.hd, (y + mapItemType.dy) * MyObject.hd, dir, 0);
			}
		}
		else
		{
			paintPartImage(g, mapItemType.imgID, (x + mapItemType.dx) * MyObject.hd, (y + mapItemType.dy) * MyObject.hd, 0);
		}
	}

	public void paintPartImage(MyGraphics g, short imgID, int x, int y, int arthor)
	{
		ImageIcon imgIcon = AvatarData.getImgIcon(imgID);
		if (!((float)(x + imgIcon.w) < AvCamera.gI().xCam) && !((float)x > AvCamera.gI().xCam + (float)Canvas.w) && !((float)(y + imgIcon.h) < AvCamera.gI().yCam) && !((float)y > AvCamera.gI().yCam + (float)Canvas.h) && imgIcon.count != -1)
		{
			if (w == 0)
			{
				w = imgIcon.w;
				h = imgIcon.h;
			}
			g.drawRegion(imgIcon.img, 0f, 0f, imgIcon.w, imgIcon.h, dir, x, y, arthor);
		}
	}

	public override void update()
	{
	}
}
