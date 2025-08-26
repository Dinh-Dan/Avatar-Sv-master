public abstract class FontX
{
	public const sbyte LEFT = 0;

	public const sbyte RIGHT = 1;

	public const sbyte CENTER = 2;

	public abstract void drawString(MyGraphics g, string st, int x, int y, int align);

	public abstract int getWidth(string st);

	public abstract string[] splitFontBStrInLine(string src, int lineWidth);

	public abstract MyVector splitFontBStrInLineV(string src, int lineWidth);

	public abstract string replace(string _text, string _searchStr, string _replacementStr);

	public abstract int getHeight();

	public abstract int getWidthNotExact(string s);
}
