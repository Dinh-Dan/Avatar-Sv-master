using System.Collections;
using UnityEngine;

public class MyGraphics
{
	public static int HCENTER = 1;

	public static int VCENTER = 2;

	public static int LEFT = 4;

	public static int RIGHT = 8;

	public static int TOP = 16;

	public static int BOTTOM = 32;

	private float r;

	private float g;

	private float b;

	public float clipX;

	public float clipY;

	public float clipW;

	public float clipH;

	private bool isClip;

	private bool isTranslate = true;

	private float translateX;

	private float translateY;

	private float areaTexture;

	public static Hashtable cachedTextures = new Hashtable();

	private float clipTX;

	private float clipTY;

	private int currentBGColor;

	private void cache(string key, Texture value)
	{
		if (areaTexture > 100f)
		{
			cachedTextures.Clear();
			areaTexture = 0f;
		}
		areaTexture += value.width * value.height;
		cachedTextures.Add(key, value);
	}

	public static void clearCache()
	{
		cachedTextures.Clear();
	}

	public void translate(float tx, float ty)
	{
		translateX += tx;
		translateY += ty;
		isTranslate = true;
		if (translateX == 0f && translateY == 0f)
		{
			isTranslate = false;
		}
	}

	public float getTranslateX()
	{
		return translateX;
	}

	public float getTranslateY()
	{
		return translateY;
	}

	public void setClip(float x, float y, float w, float h)
	{
		clipTX = translateX;
		clipTY = translateY;
		clipX = x;
		clipY = y;
		clipW = w;
		clipH = h;
		isClip = true;
	}

	public void drawLine(float x1, float y1, float x2, float y2)
	{
		if (y1 == y2)
		{
			if (x1 > x2)
			{
				float num = x2;
				x2 = x1;
				x1 = num;
			}
			fillRect(x1, y1, x2 - x1, 1f);
			return;
		}
		if (x1 == x2)
		{
			if (y1 > y2)
			{
				float num2 = y2;
				y2 = y1;
				y1 = num2;
			}
			fillRect(x1, y1, 1f, y2 - y1);
			return;
		}
		if (isTranslate)
		{
			x1 += translateX;
			y1 += translateY;
			x2 += translateX;
			y2 += translateY;
		}
		string key = "dl" + r + g + b;
		Texture2D texture2D = (Texture2D)cachedTextures[key];
		if (texture2D == null)
		{
			texture2D = new Texture2D(1, 1);
			Color color = new Color(r, g, b);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			cache(key, texture2D);
		}
		Vector2 vector = new Vector2(x1, y1);
		Vector2 vector2 = new Vector2(x2, y2);
		Vector2 vector3 = vector2 - vector;
		float num3 = 57.29578f * Mathf.Atan(vector3.y / vector3.x);
		if (vector3.x < 0f)
		{
			num3 += 180f;
		}
		float num4 = Mathf.Ceil(0f);
		GUIUtility.RotateAroundPivot(num3, vector);
		float num5 = 0f;
		float num6 = 0f;
		float width = 0f;
		float height = 0f;
		if (isClip)
		{
			num5 = clipX;
			num6 = clipY;
			width = clipW;
			height = clipH;
			if (isTranslate)
			{
				num5 += clipTX;
				num6 += clipTY;
			}
		}
		if (isClip)
		{
			GUI.BeginGroup(new Rect(num5, num6, width, height));
		}
		Graphics.DrawTexture(new Rect(vector.x - num5, vector.y - num4 - num6, vector3.magnitude, 1f), texture2D);
		if (isClip)
		{
			GUI.EndGroup();
		}
		GUIUtility.RotateAroundPivot(0f - num3, vector);
	}

	public void drawRect(float x, float y, float w, float h)
	{
		fillRect(x, y, w, 1f);
		fillRect(x, y, 1f, h);
		fillRect(x + w, y, 1f, h);
		fillRect(x, y + h, w, 1f);
	}

	public void fillRect(float x, float y, float w, float h)
	{
		if (w < 0f || h < 0f)
		{
			return;
		}
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		int num = 1;
		int num2 = 1;
		string key = "fr" + num + num2 + r + g + b;
		Texture2D texture2D = (Texture2D)cachedTextures[key];
		if (texture2D == null)
		{
			texture2D = new Texture2D(num, num2);
			Color color = new Color(r, g, b);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			cache(key, texture2D);
		}
		float num3 = 0f;
		float num4 = 0f;
		float width = 0f;
		float height = 0f;
		if (isClip)
		{
			num3 = clipX;
			num4 = clipY;
			width = clipW;
			height = clipH;
			if (isTranslate)
			{
				num3 += clipTX;
				num4 += clipTY;
			}
		}
		if (isClip)
		{
			GUI.BeginGroup(new Rect(num3, num4, width, height));
		}
		GUI.DrawTexture(new Rect(x - num3, y - num4, w, h), texture2D);
		if (isClip)
		{
			GUI.EndGroup();
		}
	}

	public void setColor(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		b = (float)num / 256f;
		g = (float)num2 / 256f;
		r = (float)num3 / 256f;
	}

	public void setColor(Color color)
	{
		b = color.b;
		g = color.g;
		r = color.r;
	}

	public void setBgColor(int rgb)
	{
		if (rgb != currentBGColor)
		{
			currentBGColor = rgb;
			int num = rgb & 0xFF;
			int num2 = (rgb >> 8) & 0xFF;
			int num3 = (rgb >> 16) & 0xFF;
			b = (float)num / 256f;
			g = (float)num2 / 256f;
			r = (float)num3 / 256f;
			Main.main.GetComponent<Camera>().backgroundColor = new Color(r, g, b);
		}
	}

	public void drawString(string s, float x, float y, GUIStyle style)
	{
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = 0f;
		float num2 = 0f;
		float width = 0f;
		float height = 0f;
		if (isClip)
		{
			num = clipX;
			num2 = clipY;
			width = clipW;
			height = clipH;
			if (isTranslate)
			{
				num += clipTX;
				num2 += clipTY;
			}
		}
		if (isClip)
		{
			GUI.BeginGroup(new Rect(num, num2, width, height));
		}
		GUI.Label(new Rect(x - num, y - num2, ScaleGUI.WIDTH, 100f), s, style);
		if (isClip)
		{
			GUI.EndGroup();
		}
	}

	public void drawString(string s, float x, float y, GUIStyle style, int w)
	{
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = 0f;
		float num2 = 0f;
		float width = 0f;
		float height = 0f;
		if (isClip)
		{
			num = clipX;
			num2 = clipY;
			width = clipW;
			height = clipH;
			if (isTranslate)
			{
				num += clipTX;
				num2 += clipTY;
			}
		}
		if (isClip)
		{
			GUI.BeginGroup(new Rect(num, num2, width, height));
		}
		GUI.Label(new Rect(x - num, y - num2, w, 100f), s, style);
		if (isClip)
		{
			GUI.EndGroup();
		}
	}

	public void drawRegion(Image image, float x0, float y0, int w, int h, int transform, float x, float y, int anchor)
	{
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = w;
		float num2 = h;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 1f;
		float num8 = 0f;
		if ((anchor & HCENTER) == HCENTER)
		{
			num5 -= num / 2f;
		}
		if ((anchor & VCENTER) == VCENTER)
		{
			num6 -= num2 / 2f;
		}
		if ((anchor & RIGHT) == RIGHT)
		{
			num5 -= num;
		}
		if ((anchor & BOTTOM) == BOTTOM)
		{
			num6 -= num2;
		}
		x += num5;
		y += num6;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		if (isClip)
		{
			num9 = clipX;
			num10 = clipY;
			num11 = clipW;
			num12 = clipH;
			if (isTranslate)
			{
				num9 += clipTX;
				num10 += clipTY;
			}
			Rect r = new Rect(x, y, w, h);
			Rect rect = intersectRect(r2: new Rect(num9, num10, num11, num12), r1: r);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			num = rect.width;
			num2 = rect.height;
			num3 = rect.x - r.x;
			num4 = rect.y - r.y;
		}
		float num13 = 0f;
		if (transform == 2)
		{
			num13 += num;
			num7 = -1f;
			if (isClip)
			{
				if (num9 > x)
				{
					num8 = 0f - num3;
				}
				else if (num9 + num11 < x + (float)w)
				{
					num8 = 0f - (num9 + num11 - x - (float)w);
				}
			}
		}
		Graphics.DrawTexture(new Rect(x + num3 + num13, y + num4, num * num7, num2), image.texture, new Rect((x0 + num3 + num8) / (float)image.texture.width, ((float)image.texture.height - num2 - (y0 + num4)) / (float)image.texture.height, num / (float)image.texture.width, num2 / (float)image.texture.height), 0, 0, 0, 0);
	}

	public void drawRegion(Image image, float x0, float y0, int w, int h, int transform, float x, float y, float wScale, float hScale, int anchor)
	{
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = w;
		float num2 = h;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 1f;
		float num8 = 0f;
		if ((anchor & HCENTER) == HCENTER)
		{
			num5 -= num / 2f;
		}
		if ((anchor & VCENTER) == VCENTER)
		{
			num6 -= num2 / 2f;
		}
		if ((anchor & RIGHT) == RIGHT)
		{
			num5 -= num;
		}
		if ((anchor & BOTTOM) == BOTTOM)
		{
			num6 -= num2;
		}
		x += num5;
		y += num6;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		if (isClip)
		{
			num9 = clipX;
			num10 = clipY;
			num11 = clipW;
			num12 = clipH;
			if (isTranslate)
			{
				num9 += clipTX;
				num10 += clipTY;
			}
			Rect r = new Rect(x, y, w, h);
			Rect rect = intersectRect(r2: new Rect(num9, num10, num11, num12), r1: r);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			num = rect.width;
			num2 = rect.height;
			num3 = rect.x - r.x;
			num4 = rect.y - r.y;
		}
		float num13 = 0f;
		if (transform == 2)
		{
			num13 += num;
			num7 = -1f;
			if (isClip)
			{
				if (num9 > x)
				{
					num8 = 0f - num3;
				}
				else if (num9 + num11 < x + (float)w)
				{
					num8 = 0f - (num9 + num11 - x - (float)w);
				}
			}
		}
		Graphics.DrawTexture(new Rect(x + num3 + num13, y + num4, wScale, hScale), image.texture, new Rect((x0 + num3 + num8) / (float)image.texture.width, ((float)image.texture.height - num2 - (y0 + num4)) / (float)image.texture.height, num / (float)image.texture.width, num2 / (float)image.texture.height), 0, 0, 0, 0);
	}

	public void drawImage(Image image, float x, float y, int anchor)
	{
		if (image == null)
		{
			return;
		}
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = image.w;
		float num2 = image.h;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		if ((anchor & HCENTER) == HCENTER)
		{
			num5 -= num / 2f;
		}
		if ((anchor & VCENTER) == VCENTER)
		{
			num6 -= num2 / 2f;
		}
		if ((anchor & RIGHT) == RIGHT)
		{
			num5 -= num;
		}
		if ((anchor & BOTTOM) == BOTTOM)
		{
			num6 -= num2;
		}
		x += num5;
		y += num6;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		if (isClip)
		{
			num7 = clipX;
			num8 = clipY;
			num9 = clipW;
			num10 = clipH;
			if (isTranslate)
			{
				num7 += clipTX;
				num8 += clipTY;
			}
			Rect r = new Rect(x, y, image.w, image.h);
			Rect rect = intersectRect(r2: new Rect(num7, num8, num9, num10), r1: r);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			num = rect.width;
			num2 = rect.height;
			num3 = rect.x - r.x;
			num4 = rect.y - r.y;
		}
		Graphics.DrawTexture(new Rect(x + num3, y + num4, num, num2), image.texture, new Rect(num3 / (float)image.texture.width, ((float)image.texture.height - num2 - num4) / (float)image.texture.height, num / (float)image.texture.width, num2 / (float)image.texture.height), 0, 0, 0, 0);
	}

	public void reset()
	{
		isClip = false;
		isTranslate = false;
		translateX = 0f;
		translateY = 0f;
	}

	public Rect intersectRect(Rect r1, Rect r2)
	{
		float num = r1.x;
		float num2 = r1.y;
		float x = r2.x;
		float y = r2.y;
		float num3 = num;
		num3 += r1.width;
		float num4 = num2;
		num4 += r1.height;
		float num5 = x;
		num5 += r2.width;
		float num6 = y;
		num6 += r2.height;
		if (num < x)
		{
			num = x;
		}
		if (num2 < y)
		{
			num2 = y;
		}
		if (num3 > num5)
		{
			num3 = num5;
		}
		if (num4 > num6)
		{
			num4 = num6;
		}
		num3 -= num;
		num4 -= num2;
		if (num3 < -30000f)
		{
			num3 = -30000f;
		}
		if (num4 < -30000f)
		{
			num4 = -30000f;
		}
		return new Rect(num, num2, (int)num3, (int)num4);
	}

	public void drawImageScaleClip(Image image, float x, float y, float w, float h, int tranform)
	{
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		float num = w;
		float num2 = h;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		if (isClip)
		{
			num7 = clipX;
			num8 = clipY;
			num9 = clipW;
			num10 = clipH;
			if (isTranslate)
			{
				num7 += clipTX;
				num8 += clipTY;
			}
			Rect r = new Rect(x, y, w, h);
			Rect rect = intersectRect(r2: new Rect(num7, num8, num9, num10), r1: r);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			num = rect.width;
			num2 = rect.height;
			num3 = rect.x - r.x;
			num4 = rect.y - r.y;
		}
		Graphics.DrawTexture(new Rect(x + num3, y + num4, num, num2), image.texture, new Rect(num3 / (float)image.texture.width, ((float)image.texture.height - num2 - num4) / (float)image.texture.height, num / (float)image.texture.width, num2 / (float)image.texture.height), 0, 0, 0, 0);
	}

	public void drawImageScale(Image image, int x, int y, int w, int h, int tranform)
	{
		Graphics.DrawTexture(new Rect((float)x + translateX, (float)y + translateY, (tranform != 0) ? (-w) : w, h), image.texture);
	}

	public void drawImageSimple(Image image, int x, int y)
	{
		Graphics.DrawTexture(new Rect(x, y, image.w, image.h), image.texture);
	}
}
