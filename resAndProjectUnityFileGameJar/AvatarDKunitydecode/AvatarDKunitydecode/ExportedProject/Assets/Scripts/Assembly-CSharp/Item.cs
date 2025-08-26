public class Item
{
	public short ID;

	public short idIcon;

	public sbyte shopType;

	public int[] price = new int[2];

	public int number;

	public string name = string.Empty;

	public string des;

	public static Item getItemByList(MyVector list, int id)
	{
		int num = list.size();
		for (int i = 0; i < num; i++)
		{
			Item item = (Item)list.elementAt(i);
			if (item.ID == id)
			{
				return item;
			}
		}
		return null;
	}
}
