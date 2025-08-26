using UnityEngine;

public class NormalFont : FontX
{
	public new const sbyte LEFT = 0;

	public new const sbyte RIGHT = 1;

	public new const sbyte CENTER = 2;

	public Image imgFont;

	public string charList;

	public sbyte[] charWidth;

	private static string[] NAME = new string[7] { "arial", "arial", "arial", "arial", "copper", "arial", "arial" };

	private static int[] STYLE = new int[7] { 0, 2, 0, 0, 1, 2, 2 };

	private static Color[] COLOR1 = new Color[7]
	{
		new Color(0f, 0.294f, 0.27f),
		Color.white,
		Color.white,
		Color.black,
		Color.yellow,
		Color.red,
		Color.yellow
	};

	private static Color[] COLOR2 = new Color[7]
	{
		Color.black,
		Color.black,
		Color.black,
		Color.black,
		Color.black,
		Color.black,
		Color.black
	};

	private static int[] SIZE = new int[7] { 11, 11, 11, 11, 17, 10, 10 };

	private FontB fontB;

	public NormalFont(int type)
	{
		fontB = new FontB(NAME[type], SIZE[type], STYLE[type], (sbyte)type, COLOR1[type], COLOR2[type]);
	}

	public override void drawString(MyGraphics g, string st, int x, int y, int align)
	{
		fontB.drawString(g, st, x, y, align);
	}

	public override int getWidth(string st)
	{
		return fontB.getWidthOf(st);
	}

	public override string[] splitFontBStrInLine(string src, int lineWidth)
	{
		return fontB.splitStrInLine(src, lineWidth);
	}

	public override MyVector splitFontBStrInLineV(string src, int lineWidth)
	{
		return fontB.splitStrInLineV(src, lineWidth);
	}

	public override string replace(string _text, string _searchStr, string _replacementStr)
	{
		if (_text.Equals(string.Empty) || _searchStr.Equals(string.Empty))
		{
			return _text;
		}
		string text = string.Empty;
		int num = _text.IndexOf(_searchStr);
		int num2 = 0;
		int length = _searchStr.Length;
		while (num != -1)
		{
			text = text + _text.Substring(num2, num - num2) + _replacementStr;
			num2 = num + length;
			num = _text.IndexOf(_searchStr, num2);
		}
		return text + _text.Substring(num2, _text.Length - num2);
	}

	public override int getHeight()
	{
		return fontB.getHeight();
	}

	public override int getWidthNotExact(string s)
	{
		return fontB.getWidthNotExactOf(s);
	}
}
