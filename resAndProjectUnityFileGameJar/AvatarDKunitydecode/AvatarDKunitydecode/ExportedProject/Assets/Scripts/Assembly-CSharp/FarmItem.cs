public class FarmItem
{
	public short ID;

	public short IDImg;

	public bool isItem;

	public const sbyte T_TREE = 0;

	public const sbyte T_POULTRY = 1;

	public const sbyte T_CATTLE = 2;

	public const sbyte T_DOG = 3;

	public const sbyte T_FISH = 4;

	public const sbyte T_NOT_FISH = 100;

	public const sbyte T_PUBLIC = 101;

	public sbyte type;

	public const sbyte A_FEEDING = 5;

	public const sbyte A_INJECTION = 4;

	public const sbyte A_TONIC = 6;

	public const sbyte A_BON_PHAN = 2;

	public const sbyte A_DIET_CO = 3;

	public const sbyte A_TRU_SAU = 7;

	public sbyte action;

	public string des;

	public int priceXu;

	public int priceLuong;

	public void paint(MyGraphics g, int x, int y, int dir, int anthor)
	{
		ImageIcon imgIcon = FarmData.getImgIcon(IDImg);
		if (imgIcon.count != -1)
		{
			g.drawRegion(imgIcon.img, 0f, 0f, imgIcon.w, imgIcon.h, dir, x, y, anthor);
		}
	}
}
