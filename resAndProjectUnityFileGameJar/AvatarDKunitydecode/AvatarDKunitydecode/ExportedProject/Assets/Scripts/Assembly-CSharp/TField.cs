using System;
using System.Threading;
using UnityEngine;

public class TField
{
	private class IActionChat : IKbAction
	{
		private TField me;

		public IActionChat(TField me)
		{
			this.me = me;
		}

		public void perform(string text)
		{
			if (!text.Equals(string.Empty))
			{
				me.setText(text);
				me.action.perform();
			}
		}
	}

	public bool UpperCaseEnable = true;

	public string name;

	public static TField currentTField;

	public const sbyte KEY_LEFT = 14;

	public const sbyte KEY_RIGHT = 15;

	public const sbyte KEY_CLEAR = 19;

	public int x;

	public int y;

	public int width;

	public int height;

	private bool isFocus;

	public bool lockArrow;

	public bool paintFocus = true;

	public bool isChangeFocus = true;

	public static int typeXpeed = 1;

	public static int[] MAX_TIME_TO_CONFIRM_KEY = new int[7] { 18, 14, 11, 9, 6, 4, 2 };

	public static int CARET_HEIGHT = 0;

	public const int CARET_WIDTH = 1;

	public const int CARET_SHOWING_TIME = 5;

	public static int TEXT_GAP_X = 5;

	public const int MAX_SHOW_CARET_COUNER = 10;

	public const int INPUT_TYPE_ANY = 0;

	public const int INPUT_TYPE_NUMERIC = 1;

	public const int INPUT_TYPE_PASSWORD = 2;

	public const int INPUT_ALPHA_NUMBER_ONLY = 3;

	public bool isUser;

	public static string[] modeNotify = new string[4] { "abc", "Abc", "ABC", "123" };

	public IAction action;

	private static string[] print = new string[12]
	{
		" 0", ".,@?!_1\"/$-():*+<=>;%&~#%^&*{}[];'/1", "abc2âă", "def3đê", "ghi4", "jkl5", "mno6ôơ", "pqrs7", "tuv8ư", "wxyz9",
		"*", "#"
	};

	private static string[] printA = new string[12]
	{
		"0", "1", "abc2", "def3", "ghi4", "jkl5", "mno6", "pqrs7", "tuv8", "wxyz9",
		"0", "0"
	};

	private static string[] printBB = new string[17]
	{
		" 0", "er1", "ty2", "ui3", "df4", "gh5", "jk6", "cv7", "bn8", "m9",
		"0", "0", "qw!", "as?", "zx", "op.", "l,"
	};

	private string text = string.Empty;

	private string passwordText = string.Empty;

	public string paintedText = string.Empty;

	public int caretPos;

	public int counter;

	private int maxTextLenght = 40;

	public int offsetX;

	private static int lastKey = -1984;

	public int keyInActiveState;

	private int indexOfActiveChar;

	public int showCaretCounter = 10;

	public int inputType = ipKeyboard.TEXT;

	public static bool isQwerty = true;

	public static int typingModeAreaWidth;

	public const sbyte abc = 0;

	public const sbyte Abc = 1;

	public const sbyte ABC = 2;

	public const sbyte number123 = 3;

	public static int mode = 0;

	public static int timeChangeMode;

	public static FrameImage frame;

	public static FrameImage tfframe;

	private sbyte indexEraser;

	public static int changeModeKey = 11;

	public static int changeDau;

	public string sDefaust = string.Empty;

	public static int xDu;

	public static int yDu;

	public AvMain parent;

	public static IAction acClear;

	public Command cmdClear;

	public static bool isOpenTextBox = false;

	private int indexDau = -1;

	private int indexTemplate;

	private int indexCong;

	private long timeDau;

	private static string printDau = "aáàảãạâấầẩẫậăắằẳẵặeéèẻẽẹêếềểễệiíìỉĩịoóòỏõọôốồổỗộơớờởỡợuúùủũụưứừửữựyýỳỷỹỵ";

	private string tempScr = string.Empty;

	private bool openVirtual;

	public bool autoScaleScreen;

	public bool showSubTextField = true;

	private bool isTransTF;

	public TField(string name, MyScreen parent, IAction ac)
	{
		action = ac;
		this.name = name;
		this.parent = parent;
		text = string.Empty;
		init();
		setFocus(false);
		height = tfframe.frameHeight;
	}

	public static void setVendorTypeMode(int mode)
	{
	}

	public void setFocus(bool isFocus)
	{
		if (this.isFocus != isFocus)
		{
			mode = 0;
		}
		lastKey = -1984;
		timeChangeMode = (int)(DateTime.Now.Ticks / 1000);
		this.isFocus = false;
		if (!isFocus)
		{
		}
	}

	public void setFocusWithKb(bool isFocus)
	{
		Out.println(string.Concat("setFocusWithKb: ", this, "    ", text));
		if (this.isFocus != isFocus)
		{
			mode = 0;
		}
		lastKey = -1984;
		timeChangeMode = (int)(DateTime.Now.Ticks / 1000);
		this.isFocus = false;
		if (isFocus)
		{
			currentTField = this;
		}
		else if (currentTField == this)
		{
			currentTField = null;
		}
		if (Thread.CurrentThread.Name == Main.mainThreadName && currentTField != null && Canvas.currentMyScreen == currentTField.parent)
		{
			Debug.LogWarning("SHOW KEYBOARD FOR " + currentTField.name);
			if (Screen.orientation != ScreenOrientation.Portrait || ipKeyboard.tk == null)
			{
				ipKeyboard.openKeyBoard(T.nameAcc, inputType, text, new IActionChat(this), true);
				ipKeyboard.tk.text = text;
			}
			else
			{
				ipKeyboard.tk.text = text;
				ipKeyboard.act = new IActionChat(this);
			}
		}
	}

	private void init()
	{
		CARET_HEIGHT = Canvas.blackF.getHeight() + 1;
	}

	public static void close()
	{
		if (TouchScreenKeyboard.visible)
		{
			Canvas.isPointerRelease = false;
			Canvas.isKeyBoard = false;
			ipKeyboard.tk = null;
		}
	}

	public void clear()
	{
		if (caretPos > 0 && text.Length > 0)
		{
			text = text.Substring(0, caretPos - 1);
			caretPos--;
			setOffset(0);
			setPasswordTest();
			if (ipKeyboard.tk != null)
			{
				ipKeyboard.tk.text = string.Empty;
			}
		}
	}

	public void setOffset(int index)
	{
		if (inputType == ipKeyboard.PASS)
		{
			paintedText = passwordText;
		}
		else
		{
			paintedText = text;
		}
		int num = Canvas.fontChatB.getWidth(paintedText.Substring(0, caretPos));
		switch (index)
		{
		case -1:
			if (num + offsetX < 15 && caretPos > 0 && caretPos < paintedText.Length)
			{
				offsetX += Canvas.fontChatB.getWidth(paintedText.Substring(caretPos, 1));
			}
			break;
		case 1:
			if (num + offsetX > width - 25 && caretPos < paintedText.Length && caretPos > 0)
			{
				offsetX -= Canvas.fontChatB.getWidth(paintedText.Substring(caretPos - 1, 1));
			}
			break;
		default:
			offsetX = -(num - (width - 12 - 20 * AvMain.hd));
			break;
		}
		if (offsetX > 0)
		{
			offsetX = 0;
		}
		else if (offsetX < 0)
		{
			int num2 = Canvas.fontChatB.getWidth(paintedText) - (width - 12 - 20 * AvMain.hd);
			if (offsetX < -num2)
			{
				offsetX = -num2;
			}
		}
	}

	private void keyPressedAny(int keyCode)
	{
		string[] array = null;
		array = ((inputType != 2 && inputType != 3) ? print : printA);
		if (keyCode == lastKey)
		{
			indexOfActiveChar = (indexOfActiveChar + 1) % array[keyCode - 48].Length;
			char c = array[keyCode - 48][indexOfActiveChar];
			string text = string.Concat(arg1: (mode == 0) ? char.ToLower(c) : ((mode == 1) ? char.ToUpper(c) : ((mode != 2) ? array[keyCode - 48][array[keyCode - 48].Length - 1] : char.ToUpper(c))), arg0: this.text.Substring(0, caretPos - 1));
			if (caretPos < this.text.Length)
			{
				text += this.text.Substring(caretPos, this.text.Length - caretPos);
			}
			this.text = text;
			keyInActiveState = MAX_TIME_TO_CONFIRM_KEY[typeXpeed];
			setPasswordTest();
		}
		else if (this.text.Length < maxTextLenght)
		{
			if (mode == 1 && lastKey != -1984)
			{
				mode = 0;
			}
			indexOfActiveChar = 0;
			char c2 = array[keyCode - 48][indexOfActiveChar];
			string text2 = string.Concat(arg1: (mode == 0) ? char.ToLower(c2) : ((mode == 1) ? char.ToUpper(c2) : ((mode != 2) ? array[keyCode - 48][array[keyCode - 48].Length - 1] : char.ToUpper(c2))), arg0: this.text.Substring(0, caretPos));
			if (caretPos < this.text.Length)
			{
				text2 += this.text.Substring(caretPos, this.text.Length - caretPos);
			}
			this.text = text2;
			keyInActiveState = MAX_TIME_TO_CONFIRM_KEY[typeXpeed];
			caretPos++;
			setPasswordTest();
			setOffset(0);
		}
		lastKey = keyCode;
	}

	private void keyPressedAscii(int keyCode)
	{
		if (((inputType != 2 && inputType != 3) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122)) && this.text.Length < maxTextLenght)
		{
			string text = this.text.Substring(0, caretPos) + (char)keyCode;
			if (caretPos < this.text.Length)
			{
				text += this.text.Substring(caretPos, this.text.Length - caretPos);
			}
			this.text = text;
			caretPos++;
			setPasswordTest();
			setOffset(0);
		}
	}

	public static void setMode()
	{
		mode++;
		if (mode > 3)
		{
			mode = 0;
		}
		lastKey = changeModeKey;
		timeChangeMode = Environment.TickCount / 1000;
	}

	public bool keyPressed(int keyCode)
	{
		if (keyCode == 8 || keyCode == -8 || keyCode == 204)
		{
			clear();
			return true;
		}
		if (isQwerty && keyCode >= 32)
		{
			keyPressedAscii(keyCode);
			return false;
		}
		if (keyCode == changeDau && inputType == 0)
		{
			setDau();
			return false;
		}
		if (keyCode == 42)
		{
			keyCode = 58;
		}
		if (keyCode == 35)
		{
			keyCode = 59;
		}
		if (keyCode >= 48 && keyCode <= 59)
		{
			if (inputType == 0 || inputType == 2 || inputType == 3)
			{
				keyPressedAny(keyCode);
			}
			else if (inputType == 1)
			{
				keyPressedAscii(keyCode);
				keyInActiveState = 1;
			}
		}
		else
		{
			indexOfActiveChar = 0;
			lastKey = -1984;
			if (keyCode == 14 && !lockArrow)
			{
				if (caretPos > 0)
				{
					caretPos--;
					setOffset(0);
					showCaretCounter = 10;
					return false;
				}
			}
			else if (keyCode == 15 && !lockArrow)
			{
				if (caretPos < text.Length)
				{
					caretPos++;
					setOffset(0);
					showCaretCounter = 10;
					return false;
				}
			}
			else
			{
				if (keyCode == 19)
				{
					clear();
					return false;
				}
				lastKey = keyCode;
			}
		}
		return true;
	}

	private void setDau()
	{
		timeDau = Environment.TickCount / 100;
		if (indexDau == -1)
		{
			for (int num = caretPos; num > 0; num--)
			{
				char c = this.text[num - 1];
				for (int i = 0; i < printDau.Length; i++)
				{
					char c2 = printDau[i];
					if (c == c2)
					{
						indexTemplate = i;
						indexCong = 0;
						indexDau = num - 1;
						return;
					}
				}
			}
			indexDau = -1;
		}
		else
		{
			indexCong++;
			if (indexCong >= 6)
			{
				indexCong = 0;
			}
			string text = this.text.Substring(0, indexDau);
			string text2 = this.text.Substring(indexDau + 1);
			string text3 = printDau.Substring(indexTemplate + indexCong, 1);
			this.text = text + text3 + text2;
		}
	}

	public void paint(MyGraphics g)
	{
		bool flag = isFocused();
		if (inputType == ipKeyboard.PASS)
		{
			paintedText = passwordText;
		}
		else
		{
			if (!UpperCaseEnable)
			{
				text = text.ToLower();
			}
			paintedText = text;
		}
		g.setClip(0f, 0f, Canvas.w + 20, Canvas.hCan);
		Canvas.paint.paintTextBox(g, x, y, width, height, this, flag, indexEraser);
	}

	public bool isFocused()
	{
		if (currentTField == this)
		{
			return true;
		}
		return false;
	}

	private void setPasswordTest()
	{
		if (inputType == ipKeyboard.PASS)
		{
			passwordText = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				passwordText += "*";
			}
			if (keyInActiveState > 0 && caretPos > 0)
			{
				passwordText = passwordText.Substring(0, caretPos - 1) + text[caretPos - 1] + passwordText.Substring(caretPos, passwordText.Length - caretPos);
			}
		}
	}

	public void update()
	{
		if (ipKeyboard.tk != null && ipKeyboard.tk.active && currentTField == this && ipKeyboard.tk.text != text)
		{
			tempScr = ipKeyboard.tk.text;
			if (tempScr.Length > text.Length)
			{
				int num = tempScr.Substring(tempScr.Length - 1).ToCharArray()[0];
				if (isUser && (num < 48 || num > 57) && (num < 65 || num > 90) && (num < 97 || num > 122))
				{
					ipKeyboard.tk.text = text;
				}
				else if (tempScr.Length > maxTextLenght)
				{
					ipKeyboard.tk.text = text;
				}
				else
				{
					setText(ipKeyboard.tk.text);
				}
			}
			else
			{
				setText(ipKeyboard.tk.text);
			}
		}
		counter++;
		if (showCaretCounter > 0)
		{
			showCaretCounter--;
		}
		if (Canvas.currentDialog == null && Canvas.menuMain == null)
		{
			setTextBox();
		}
		else if (Canvas.currentDialog == Canvas.inputDlg && this == Canvas.inputDlg.tfInput)
		{
			setTextBox();
		}
		if (indexDau != -1 && Environment.TickCount / 100 - timeDau > 5)
		{
			indexDau = -1;
		}
	}

	public void setTextBox()
	{
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(x, y + 1, width, height - 2))
			{
				isTransTF = true;
				Canvas.isPointerClick = false;
				Out.println("name: " + name);
			}
			if (currentTField == this && Canvas.isPoint(x + width - 22 * AvMain.hd, y + 1, 24 * AvMain.hd, height - 2))
			{
				indexEraser = 1;
				isTransTF = true;
				Canvas.isPointerClick = false;
			}
		}
		if (!isTransTF)
		{
			return;
		}
		if (Canvas.isPointerDown && !Canvas.isPoint(x + width - 22 * AvMain.hd, y + 1, 24 * AvMain.hd, height - 2))
		{
			indexEraser = 0;
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		isTransTF = false;
		if (indexEraser == 1)
		{
			Canvas.isPointerRelease = false;
			indexEraser = 0;
			setText(string.Empty);
			if (currentTField == this && ipKeyboard.tk != null)
			{
				ipKeyboard.tk.text = string.Empty;
			}
		}
		else if (Canvas.isPoint(x, y + 1, width, height - 2))
		{
			Canvas.isPointerRelease = false;
			if (currentTField != this || ipKeyboard.tk == null)
			{
				setFocusWithKb(true);
			}
		}
	}

	public string getText()
	{
		return text;
	}

	public void setText(string text)
	{
		if (text != null)
		{
			lastKey = -1984;
			keyInActiveState = 0;
			indexOfActiveChar = 0;
			this.text = text;
			if (Thread.CurrentThread.Name == Main.mainThreadName && ipKeyboard.tk != null)
			{
				ipKeyboard.tk.text = text;
			}
			paintedText = text;
			setPasswordTest();
			caretPos = text.Length;
			setOffset(0);
		}
	}

	public void setMaxTextLenght(int maxTextLenght)
	{
		this.maxTextLenght = maxTextLenght;
	}

	public void setIputType(int iputType)
	{
		inputType = iputType;
	}

	public void setFocus2(bool b)
	{
		isFocus = false;
	}
}
