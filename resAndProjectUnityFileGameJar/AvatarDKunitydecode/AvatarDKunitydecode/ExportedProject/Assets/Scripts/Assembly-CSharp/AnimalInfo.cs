public class AnimalInfo
{
	public sbyte species;

	public sbyte frame;

	public sbyte area;

	public int harvestTime;

	public int[] price = new int[2];

	public short priceProduct;

	public short iconID;

	public short iconProduct = -1;

	public short iconO = -1;

	public short[] idImg = new short[3];

	public sbyte[][] arrFrame;

	public string name;

	public string des;

	public AnimalInfo()
	{
		arrFrame = new sbyte[3][];
		for (int i = 0; i < 3; i++)
		{
			arrFrame[i] = new sbyte[12];
		}
	}
}
