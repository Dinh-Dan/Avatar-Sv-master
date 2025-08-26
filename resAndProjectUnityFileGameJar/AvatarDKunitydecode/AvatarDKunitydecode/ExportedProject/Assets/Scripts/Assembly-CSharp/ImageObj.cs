public class ImageObj : SubObject
{
	public Image img;

	public ImageObj(int type, int x, int y, int w, int h)
		: base(type, x, y, w, h)
	{
		if (type >= 0)
		{
			img = Image.createImagePNG(T.getPath() + "/home/" + type);
		}
		if (img != null)
		{
			base.w = (short)img.w;
			base.h = (short)img.h;
		}
		else
		{
			base.w = (base.h = 0);
		}
	}

	public override void update()
	{
	}

	public override void paint(MyGraphics g)
	{
		if (img == null)
		{
			if (w == 0)
			{
				w = AvatarData.getImgIcon((short)type).w;
				h = AvatarData.getImgIcon((short)type).h;
			}
			AvatarData.paintImg(g, type, x * MyObject.hd, y * MyObject.hd, 33);
		}
		else
		{
			g.drawImage(img, x * MyObject.hd, y * MyObject.hd, 33);
		}
		if (type == 846)
		{
			Canvas.blackF.drawString(g, MapScr.boardID + string.Empty, x * MyObject.hd, y * MyObject.hd - 30 * MyObject.hd, 2);
		}
		else if (type == 1029 && FarmScr.foodID != 0)
		{
			Food foodByID = FarmData.getFoodByID(FarmScr.foodID);
			FarmItem farmItem = FarmScr.getFarmItem(foodByID.productID);
			string text = string.Empty;
			int num = FarmScr.remainTime / 3600;
			if (num > 0)
			{
				text = num + ":";
			}
			int num2 = (FarmScr.remainTime - num * 3600) / 60;
			if (num2 > 0 || num > 0)
			{
				text = text + num2 + ":";
			}
			int num3 = FarmScr.remainTime - num * 3600 - num2 * 60;
			text = text + num3 + string.Empty;
			if (FarmScr.remainTime == 0)
			{
				text = "Hoan thanh";
			}
			FarmScr.xPosCook = x - Canvas.smallFontYellow.getWidth(text) / 2 / MyObject.hd;
			int num4 = 60 * MyObject.hd;
			FarmScr.yPosCook = y - num4 / MyObject.hd - 10;
			FarmData.paintImg(g, farmItem.IDImg, x * MyObject.hd - Canvas.smallFontYellow.getWidth(text) / 2, y * MyObject.hd - num4 - 10 * MyObject.hd, 3);
			Canvas.smallFontYellow.drawString(g, text, x * MyObject.hd - Canvas.smallFontYellow.getWidth(text) / 2 + 10 * MyObject.hd, y * MyObject.hd - num4 - 10 * MyObject.hd - AvMain.hSmall / 2 + 2, 0);
		}
	}
}
