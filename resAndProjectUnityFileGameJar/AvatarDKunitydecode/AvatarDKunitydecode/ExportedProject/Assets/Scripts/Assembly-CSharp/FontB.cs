using System;
using System.Collections;
using UnityEngine;

public class FontB
{
	public Font myfont;

	public int size = 15;

	private int height;

	private int wO;

	public Color color1 = Color.white;

	public Color color2 = Color.gray;

	private int fstyle;

	public static string st1;

	public static string st2;

	public sbyte[][] wStr;

	public sbyte type;

	public static string stSetWDown;

	public static string stSetWUp;

	public sbyte[] wChar;

	public static char[] cCharDown;

	public static char[] cCharUp;

	static FontB()
	{
		st1 = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ";
		st2 = "\u00b8µ¶·¹\u00a8¾»¼½Æ©ÊÇÈÉËÐÌÎÏÑªÕÒÓÔÖÝ×ØÜÞãßáâä«èåæçé¬íêëìîóïñòô\u00adøõö÷ùýúûüþ®\u00b8µ¶·¹¡¾»¼½Æ¢ÊÇÈÉËÐÌÎÏÑ£ÕÒÓÔÖÝ×ØÜÞãßáâä¤èåæçé¥íêëìîóïñòô¦øõö÷ùýúûüþ§";
		stSetWDown = " +-%:,.0123456789aáàảãạăắằẳẵặâấầẩẫậbcdđeéèẻẽẹêếềểễệfghiíìỉĩịklmnoóòỏõọôốồổỗộơớờởỡợpquúùủũụưứừửữựyýỳỷỹỵrvjxtszw*/!AÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQUÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴRVJXTSZW";
		stSetWUp = "*";
		cCharDown = stSetWDown.ToCharArray();
		cCharUp = stSetWUp.ToCharArray();
	}

	public FontB(string name, int size, int style, sbyte type, Color color1, Color color2)
	{
		this.type = type;
		myfont = (Font)Resources.Load(name);
		this.size = size;
		this.color1 = color1;
		this.color2 = color2;
		fstyle = style;
		wO = getWidthExactOf("O");
		setW();
	}

	public void setW()
	{
		int num = 0;
		int num2 = 1000;
		wChar = new sbyte[cCharDown.Length + cCharUp.Length];
		for (int i = 0; i < cCharDown.Length; i++)
		{
			num = cCharDown[i];
			if (cCharDown[i] == '/' || cCharDown[i] == '!')
			{
				wChar[i] = (sbyte)(3 * AvMain.hd);
				num2 = i;
			}
			else if (cCharDown[i] == '*' || cCharDown[i] == '/')
			{
				wChar[i] = (sbyte)getWidthExactOf("*");
			}
			else if (cCharDown[i] == ' ')
			{
				wChar[i] = (sbyte)(getWidthExactOf("a") - 4);
			}
			else if (num >= 65 && num <= 90)
			{
				wChar[i] = (sbyte)getWidthExactOf(cCharDown[i] + string.Empty);
			}
			else if (i > 16 && (num < 97 || num > 122))
			{
				wChar[i] = (sbyte)getWidthExactOf("a");
			}
			else
			{
				wChar[i] = (sbyte)getWidthExactOf(cCharDown[i] + string.Empty);
			}
		}
		for (int j = 0; j < cCharUp.Length; j++)
		{
			num = cCharUp[j];
			if (cCharDown[j] != ' ' && (num < 65 || num > 90))
			{
				wChar[cCharDown.Length + j] = (sbyte)getWidthExactOf("A");
			}
			else
			{
				wChar[cCharDown.Length + j] = (sbyte)getWidthExactOf(cCharUp[j] + string.Empty);
			}
		}
	}

	public int getWidth(string str)
	{
		char[] array = str.ToCharArray();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < cCharDown.Length; j++)
			{
				if (array[i] == cCharDown[j])
				{
					num += wChar[j];
					num2 = 1;
					break;
				}
			}
			if (num2 != 0)
			{
				continue;
			}
			for (int k = 0; k < cCharUp.Length; k++)
			{
				if (array[i] == cCharUp[k])
				{
					num += wChar[k + cCharDown.Length];
					break;
				}
			}
		}
		return num;
	}

	public int getWidthOf(string str)
	{
		return getWidthExactOf(str);
	}

	public int getWidthExactOf(string s)
	{
		try
		{
			GUIStyle gUIStyle = new GUIStyle();
			gUIStyle.font = myfont;
			gUIStyle.fontSize = size;
			return (int)gUIStyle.CalcSize(new GUIContent(s)).x;
		}
		catch (Exception ex)
		{
			Debug.LogError("GET WIDTH OF " + s + " FAIL.\n" + ex.Message + "\n" + ex.StackTrace);
			return getWidthNotExactOf(s);
		}
	}

	public int getHeight()
	{
		if (height > 0)
		{
			return height;
		}
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.font = myfont;
		gUIStyle.fontSize = size;
		try
		{
			height = (int)gUIStyle.CalcSize(new GUIContent("Adg")).y + 2;
		}
		catch (Exception ex)
		{
			Debug.LogError("FAIL GET HEIGHT " + ex.StackTrace);
			height = 20;
		}
		return height;
	}

	public void drawString(MyGraphics g, string st, int x0, int y0, int align)
	{
		if (type != 14 && type != 10 && type != 18 && type != 15 && type != 16 && type != 17 && (type != 4 || AvMain.hd == 1))
		{
			char[] array = st.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				for (int j = 0; j < st1.Length; j++)
				{
					if (st1[j] == c)
					{
						array[i] = st2[j];
						break;
					}
				}
			}
			st = new string(array);
		}
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.font = myfont;
		gUIStyle.fontSize = size;
		float num = 0f;
		float num2 = 0f;
		int num3 = getWidthNotExactOf(st) + 10;
		if (num3 < Canvas.w)
		{
			num3 = Canvas.w;
		}
		switch (align)
		{
		case 0:
			num = x0;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperLeft;
			break;
		case 1:
			num = x0 - num3;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperRight;
			break;
		case 2:
			num = x0 - num3 / 2;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperCenter;
			break;
		}
		if (fstyle == 2)
		{
			gUIStyle.normal.textColor = color2;
			if (type != 4)
			{
				g.drawString(st, (int)num - 1, (int)num2 - 1, gUIStyle, num3);
				g.drawString(st, (int)num + 1, (int)num2 + 1, gUIStyle, num3);
				g.drawString(st, (int)num + 1, (int)num2 - 1, gUIStyle, num3);
			}
			g.drawString(st, (int)num - 1, (int)num2 + 1, gUIStyle, num3);
			if (Main.isCompactDevice)
			{
				if (type != 4)
				{
					g.drawString(st, (int)num - 1, (int)num2, gUIStyle, num3);
					g.drawString(st, (int)num + 1, (int)num2, gUIStyle, num3);
					g.drawString(st, (int)num, (int)num2 - 1, gUIStyle, num3);
				}
				g.drawString(st, (int)num, (int)num2 + 1, gUIStyle, num3);
			}
			gUIStyle.normal.textColor = color1;
			g.drawString(st, (int)num, (int)num2, gUIStyle, num3);
		}
		else if (fstyle == 1)
		{
			gUIStyle.normal.textColor = color2;
			g.drawString(st, (int)num - 1, (int)num2 - 1, gUIStyle, num3);
			gUIStyle.normal.textColor = color1;
			g.drawString(st, (int)num, (int)num2, gUIStyle, num3);
		}
		else if (fstyle == 0)
		{
			gUIStyle.normal.textColor = color1;
			g.drawString(st, (int)num, (int)num2, gUIStyle, num3);
		}
		else if (fstyle == 3)
		{
			gUIStyle.normal.textColor = color2;
			g.drawString(st, (int)num, (int)num2 - 1, gUIStyle, num3);
			gUIStyle.normal.textColor = color1;
			g.drawString(st, (int)num, (int)num2, gUIStyle, num3);
		}
	}

	public string[] splitStrInLine(string src, int lineWidth)
	{
		ArrayList arrayList = splitStrInLineA(src, lineWidth);
		string[] array = new string[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			array[i] = (string)arrayList[i];
		}
		return array;
	}

	public MyVector splitStrInLineV(string src, int lineWidth)
	{
		return new MyVector(splitStrInLineA(src, lineWidth));
	}

	public ArrayList splitStrInLineA(string src, int lineWidth)
	{
		ArrayList arrayList = new ArrayList();
		int i = 0;
		int num = 0;
		int length = src.Length;
		if (length < 5)
		{
			arrayList.Add(src);
			return arrayList;
		}
		string text = string.Empty;
		try
		{
			while (true)
			{
				if (getWidth(text) < lineWidth)
				{
					text += src[num];
					num++;
					if (src[num] != '\n')
					{
						if (num < length - 1)
						{
							continue;
						}
						num = length - 1;
					}
				}
				if (num != length - 1 && src[num + 1] != ' ')
				{
					int num2 = num;
					while (src[num + 1] != '\n' && (src[num + 1] != ' ' || src[num] == ' ') && num != i)
					{
						num--;
					}
					if (num == i)
					{
						num = num2;
					}
				}
				string text2 = src.Substring(i, num + 1 - i);
				if (text2[0] == '\n')
				{
					text2 = text2.Substring(1, text2.Length - 1);
				}
				if (text2[text2.Length - 1] == '\n')
				{
					text2 = text2.Substring(0, text2.Length - 1);
				}
				arrayList.Add(text2);
				if (num == length - 1)
				{
					break;
				}
				for (i = num + 1; i != length - 1 && src[i] == ' '; i++)
				{
				}
				if (i == length - 1)
				{
					break;
				}
				num = i;
				text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("EXCEPTION WHEN REAL SPLIT " + src + "\nend=" + num + "\n" + ex.Message + "\n" + ex.StackTrace);
			arrayList.Add(src);
		}
		return arrayList;
	}

	public int getWidthNotExactOf(string s)
	{
		return s.Length * wO;
	}
}
