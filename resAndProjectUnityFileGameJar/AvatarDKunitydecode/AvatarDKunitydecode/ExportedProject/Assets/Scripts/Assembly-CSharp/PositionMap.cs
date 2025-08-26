public class PositionMap
{
	public int x;

	public int y;

	public sbyte id;

	public string text;

	public short idImg;

	public short price;

	public sbyte typeMoney;

	public int count;

	public PositionMap()
	{
		count = CRes.rnd(9);
	}
}
