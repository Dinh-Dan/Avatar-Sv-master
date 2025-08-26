using UnityEngine;

public class HDFont : FontX
{
	public Image imgFont;

	public string charList;

	public sbyte[] charWidth;

	private static int[] STYLE = new int[19]
	{
		0, 2, 0, 0, 2, 2, 2, 1, 0, 0,
		0, 3, 0, 0, 0, 0, 0, 0, 0
	};

	private static Color[] COLOR1 = new Color[19]
	{
		new Color(0f, 29f / 64f, 35f / 64f),
		new Color(82f / 85f, 1f, 0.38039216f),
		Color.white,
		Color.black,
		Color.yellow,
		new Color(1f, 0.5254902f, 0.5254902f),
		new Color(1f, 74f / 85f, 0.56078434f),
		Color.white,
		Color.black,
		Color.white,
		new Color(0.16f, 0.504f, 0.75f),
		Color.white,
		Color.black,
		Color.white,
		new Color(0f, 29f / 64f, 0.5507813f),
		Color.white,
		Color.white,
		Color.white,
		Color.white
	};

	private static Color[] COLOR2 = new Color[19]
	{
		Color.gray,
		Color.black,
		Color.gray,
		Color.gray,
		Color.black,
		Color.black,
		Color.black,
		new Color(0.105f, 0.415f, 0.368f),
		Color.black,
		Color.white,
		new Color(0f, 29f / 64f, 0.5507813f),
		Color.white,
		Color.black,
		Color.white,
		new Color(0f, 29f / 64f, 0.5507813f),
		Color.white,
		Color.white,
		Color.white,
		Color.white
	};

	private static int[] SIZE = new int[19]
	{
		13, 14, 13, 10, 17, 10, 10, 14, 14, 12,
		12, 12, 12, 10, 10, 10, 10, 5, 12
	};

	private FontB fontB;

	public HDFont(int type)
	{
		getColor(12907498);
		string name = ((Main.hdtype != 2) ? "arial" : "vo");
		if (type == 9)
		{
			name = "fontmenu" + AvMain.hd;
		}
		int size = SIZE[type] * Main.hdtype;
		if (type == 5 || type == 6)
		{
			name = "vo2";
			size = SIZE[type] * 3 / 2;
		}
		if (type == 3 && Main.hdtype == 2)
		{
			name = "vo";
		}
		fontB = new FontB(name, size, STYLE[type], (sbyte)type, COLOR1[type], COLOR2[type]);
	}

	public HDFont(int type, string path)
	{
		int size = SIZE[type] * Main.hdtype;
		if ((type == 5 || type == 6) && Main.hdtype == 2)
		{
			size = SIZE[type] * 3 / 2;
		}
		fontB = new FontB(T.getPath() + "/font/" + path, size, STYLE[type], (sbyte)type, COLOR1[type], COLOR2[type]);
	}

	public HDFont(int type, string path, int color, int color2)
	{
		int size = SIZE[type] * Main.hdtype;
		fontB = new FontB(T.getPath() + "/font/" + path, size, STYLE[type], (sbyte)type, getColor(color), getColor(color2));
	}

	public HDFont(int type, string path, int size, int color, int color2)
	{
		fontB = new FontB("font/" + path, size, STYLE[type], (sbyte)type, getColor(color), getColor(color2));
	}

	public static Color getColor(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		float b = (float)num / 256f;
		float g = (float)num2 / 256f;
		float r = (float)num3 / 256f;
		return new Color(r, g, b);
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
