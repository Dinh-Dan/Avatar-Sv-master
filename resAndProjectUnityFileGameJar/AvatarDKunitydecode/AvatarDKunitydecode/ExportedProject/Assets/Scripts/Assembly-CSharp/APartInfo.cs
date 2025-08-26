public class APartInfo : Part
{
	public sbyte level;

	public sbyte gender;

	public short[] imgID;

	public sbyte[] dx;

	public sbyte[] dy;

	public override void paintAvatar(MyGraphics g, int index, int x, int y, int direct, int arthor)
	{
		if (IDPart == -1)
		{
			return;
		}
		if (IDPart >= 2000)
		{
			ImageIcon imagePart = AvatarData.getImagePart(imgID[index]);
			if (imagePart.count != -1)
			{
				g.drawRegion(imagePart.img, 0f, 0f, imagePart.w, imagePart.h, direct, x + dx[index] * AvMain.hd - ((direct == Base.LEFT) ? (dx[index] * AvMain.hd * 2 + imagePart.w) : 0), y + dy[index] * AvMain.hd, 0);
			}
		}
		else
		{
			ImageInfo imageInfo = AvatarData.listImgInfo[imgID[index]];
			AvatarData.paintPart(g, imageInfo.bigID, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, x + dx[index] * AvMain.hd - ((direct == Base.LEFT) ? (dx[index] * AvMain.hd * 2 + imageInfo.w * AvMain.hd) : 0), y + dy[index] * AvMain.hd, direct, 0);
		}
	}
}
