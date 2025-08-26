public abstract class MyObject
{
	public int x;

	public int y;

	public static int hd = AvMain.hd;

	public const sbyte AVATAR = 0;

	public const sbyte TREE = 1;

	public const sbyte ANIMAL = 2;

	public const sbyte PET = 4;

	public const sbyte DROP = 5;

	public const sbyte EFFECT = 6;

	public const sbyte FISH = 7;

	public const sbyte STAR_FRUIT = 8;

	public const sbyte POPUPNAME = 9;

	public const sbyte PETRACE = 10;

	public sbyte catagory;

	public short height;

	public short w;

	public short h;

	public short index = -1;

	public virtual void update()
	{
	}

	public virtual void paint(MyGraphics g)
	{
	}
}
