public class PartFollow : Part
{
	public short color;

	public override void paintIcon(MyGraphics g, int x, int y, int direct, int arthor)
	{
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart(follow);
		if (idIcon == aPartInfo.imgID[0])
		{
			ImageInfo imageInfo = AvatarData.listImgInfo[aPartInfo.imgID[0]];
			g.drawRegion(AvatarData.getBigImgInfo(color).img, imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd, direct, x, y, arthor);
		}
		else
		{
			aPartInfo.paint(g, x, y, arthor);
		}
	}

	public override void paintAvatar(MyGraphics g, int index, int x, int y, int direct, int arthor)
	{
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart(follow);
		ImageInfo imageInfo = AvatarData.listImgInfo[aPartInfo.imgID[index]];
		AvatarData.paintPart(g, color, imageInfo.x0, imageInfo.y0, imageInfo.w, imageInfo.h, x + aPartInfo.dx[index] * AvMain.hd - ((direct == Base.LEFT) ? (aPartInfo.dx[index] * AvMain.hd * 2 + imageInfo.w * AvMain.hd) : 0), y + aPartInfo.dy[index] * AvMain.hd, direct, 0);
	}
}
