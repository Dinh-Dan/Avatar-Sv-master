public class TreeInfo
{
	public string name;

	public short ID;

	public short[] idImg;

	public sbyte[] Phase;

	public short harvestTime;

	public short dieTime = -1;

	public short[] priceSeed = new short[2];

	public short priceProduct;

	public short numProduct;

	public short productID;

	public string name1;

	public short idIcon;

	public bool isDynamic;

	public sbyte lv = 1;

	public void paint(MyGraphics g, int id, int x, int y, int arthor)
	{
		if (isDynamic)
		{
			FarmData.paintImg(g, idImg[id], x, y, arthor);
		}
		else
		{
			FarmData.listImgInfo[idImg[id]].paintFarm(g, x, y, arthor);
		}
	}
}
