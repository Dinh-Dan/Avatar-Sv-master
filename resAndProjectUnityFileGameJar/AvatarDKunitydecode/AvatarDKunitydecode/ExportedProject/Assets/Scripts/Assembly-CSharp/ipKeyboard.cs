using UnityEngine;

public class ipKeyboard
{
	public static TouchScreenKeyboard tk;

	public static int TEXT;

	public static int NUMBERIC = 1;

	public static int PASS = 2;

	public static IKbAction act;

	public static int typeInput;

	public static bool isReset;

	public static bool isInput;

	public static void openKeyBoard(string caption, int type, string text, IKbAction action, bool hideInput)
	{
		Out.println("openKeyBoard: " + caption);
		act = action;
		typeInput = type;
		isInput = hideInput;
		TouchScreenKeyboardType keyboardType = ((type == 0 || type == 2) ? TouchScreenKeyboardType.ASCIICapable : TouchScreenKeyboardType.NumberPad);
		if (action == null)
		{
			TouchScreenKeyboard.hideInput = true;
		}
		else
		{
			TouchScreenKeyboard.hideInput = hideInput;
		}
		Out.println("openKeyBoard: " + text);
		tk = TouchScreenKeyboard.Open(text, keyboardType, false, false, type == 2, false, caption);
		isReset = true;
	}

	public static void update()
	{
		if (tk == null)
		{
			return;
		}
		if (Canvas.currentMyScreen != MessageScr.me && !ChatTextField.isShow && Canvas.isPaintIconVir() && !tk.text.Equals(string.Empty))
		{
			ChatTextField.gI().showTF(tk.text);
		}
		if (tk.done)
		{
			if (act != null)
			{
				act.perform(tk.text);
				act = null;
				if (ChatTextField.isShow)
				{
					ChatTextField.isShow = false;
				}
			}
			tk.text = string.Empty;
			if (Screen.orientation == ScreenOrientation.Portrait)
			{
				isReset = true;
				tk.active = true;
				TouchScreenKeyboard.hideInput = isInput;
			}
			else
			{
				tk = null;
			}
		}
		if (tk != null && !tk.active && Screen.orientation == ScreenOrientation.Portrait)
		{
			isReset = false;
			tk.active = true;
			TouchScreenKeyboard.hideInput = true;
			TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.ASCIICapable;
			tk = TouchScreenKeyboard.Open(string.Empty, keyboardType, false, false, false, false, string.Empty);
		}
	}

	public static void close()
	{
		tk.active = false;
		Canvas.aTran = false;
	}
}
