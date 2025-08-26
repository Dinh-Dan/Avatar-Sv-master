using System;
using UnityEngine;

public class ChatTextField : AvMain
{
	private class IActionChat2 : IAction
	{
		public ChatTextField me;

		public IActionChat2(ChatTextField me)
		{
			this.me = me;
		}

		public void perform()
		{
			if (me != null && me.parentMyScreen != null)
			{
				string text = me.tfChat.getText();
				me.parentMyScreen.onChatFromMe(text);
				me.tfChat.setText(string.Empty);
				if (ipKeyboard.tk != null)
				{
					ipKeyboard.tk.text = string.Empty;
				}
				if (Screen.orientation == ScreenOrientation.Portrait)
				{
					isShow = false;
				}
				if (text.ToLower().Equals("cauca") && TouchScreenKeyboard.visible)
				{
					ipKeyboard.close();
					ipKeyboard.tk = null;
				}
			}
		}
	}

	public static ChatTextField instance;

	public TField tfChat;

	public static bool isShow;

	public IChatable parentMyScreen;

	private long lastTimeChat;

	private int chatButtonLight;

	protected ChatTextField()
	{
		center = new Command(T.chat, new IActionChat2(this));
		tfChat = new TField("chat", MapScr.instance, new IActionChat2(this));
		tfChat.setFocus(true);
		tfChat.showSubTextField = false;
		tfChat.autoScaleScreen = true;
		tfChat.setIputType(ipKeyboard.TEXT);
		init(Canvas.hCan);
		tfChat.x = 5 * AvMain.hd;
		tfChat.setMaxTextLenght(40);
		tfChat.action = new IActionChat2(this);
	}

	public void init(int hc)
	{
		tfChat.y = hc - tfChat.height - 7 * AvMain.hd;
		center = new Command(T.chat, new IActionChat2(this));
		if (onMainMenu.isOngame)
		{
			center.y = -200;
			tfChat.y = hc - tfChat.height - 7 * AvMain.hd - PaintPopup.hButtonSmall;
			tfChat.width = Canvas.w - 10 * AvMain.hd;
		}
		else
		{
			center.x = Canvas.w - MyScreen.wTab / 2 - 2 * AvMain.hd;
			center.y = tfChat.y + tfChat.height / 2 - PaintPopup.hButtonSmall / 2;
			tfChat.width = center.x - MyScreen.wTab / 2 - 10 * AvMain.hd;
		}
	}

	public void keyPressed(int keyCode)
	{
		if (isShow)
		{
			tfChat.keyPressed(keyCode);
		}
		if (tfChat.getText().Equals(string.Empty))
		{
			isShow = false;
		}
	}

	public static ChatTextField gI()
	{
		return (instance != null) ? instance : (instance = new ChatTextField());
	}

	public void startChat(int firstCharacter, IChatable parentMyScreen)
	{
		if (Canvas.currentFace == null && !ipKeyboard.tk.text.Equals(string.Empty))
		{
			tfChat.keyPressed(firstCharacter);
			this.parentMyScreen = parentMyScreen;
			isShow = true;
			tfChat.setFocusWithKb(true);
		}
	}

	public override void updateKey()
	{
		TField.currentTField = tfChat;
		if (onMainMenu.isOngame)
		{
			Canvas.paint.updateKeyOn(left, center, right);
		}
		else
		{
			base.updateKey();
		}
		if (chatButtonLight > 0)
		{
			chatButtonLight--;
		}
		tfChat.update();
	}

	public override void paint(MyGraphics g)
	{
		if (onMainMenu.isOngame)
		{
			Canvas.resetTrans(g);
			Canvas.paint.paintCmdBar(g, left, center, right);
		}
		else
		{
			base.paint(g);
		}
		tfChat.paint(g);
		g.setClip(0f, 0f, Canvas.w, Canvas.h);
		Canvas.resetTrans(g);
	}

	public void showTF()
	{
		if (!isShow)
		{
			tfChat.setFocus(true);
			try
			{
				parentMyScreen = (IChatable)Canvas.currentMyScreen;
				isShow = true;
				tfChat.parent = Canvas.currentMyScreen;
				tfChat.setFocusWithKb(true);
				Canvas.isPointerClick = false;
				Canvas.isPointerRelease = false;
			}
			catch (Exception)
			{
				try
				{
					parentMyScreen = MapScr.gI();
					isShow = true;
					tfChat.parent = Canvas.currentMyScreen;
					tfChat.setFocusWithKb(true);
					Canvas.isPointerClick = false;
					Canvas.isPointerRelease = false;
				}
				catch (Exception)
				{
				}
			}
		}
		Out.println("showTF: " + isShow);
	}

	public void showTF(string text)
	{
		if (!isShow)
		{
			tfChat.setFocus(true);
			try
			{
				parentMyScreen = (IChatable)Canvas.currentMyScreen;
				isShow = true;
				tfChat.parent = Canvas.currentMyScreen;
				tfChat.setText(text);
				tfChat.setFocusWithKb(true);
				Canvas.isPointerClick = false;
				Canvas.isPointerRelease = false;
			}
			catch (Exception)
			{
				try
				{
					parentMyScreen = MapScr.gI();
					isShow = true;
					tfChat.parent = Canvas.currentMyScreen;
					tfChat.setText(text);
					tfChat.setFocusWithKb(true);
					Canvas.isPointerClick = false;
					Canvas.isPointerRelease = false;
				}
				catch (Exception)
				{
				}
			}
		}
		Out.println("showTF: " + isShow);
	}
}
