public class Math
{
	public static int abs(int i)
	{
		return (i <= 0) ? (-i) : i;
	}

	public static int min(int x, int y)
	{
		return (x >= y) ? y : x;
	}

	public static int max(int x, int y)
	{
		return (x <= y) ? y : x;
	}
}
