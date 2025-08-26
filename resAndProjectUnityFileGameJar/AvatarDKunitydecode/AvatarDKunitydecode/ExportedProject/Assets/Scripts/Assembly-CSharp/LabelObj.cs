public class LabelObj
{
	public int x;

	public int y;

	public string[] str;

	public LabelObj(string str, int w, int x, int y)
	{
		this.str = Canvas.normalFont.splitFontBStrInLine(str, w);
		this.x = x;
		this.y = y;
	}

	public void paint(MyGraphics g)
	{
		for (int i = 0; i < str.Length; i++)
		{
			Canvas.normalFont.drawString(g, str[i], x, y + 14 * i, 0);
		}
	}
}
