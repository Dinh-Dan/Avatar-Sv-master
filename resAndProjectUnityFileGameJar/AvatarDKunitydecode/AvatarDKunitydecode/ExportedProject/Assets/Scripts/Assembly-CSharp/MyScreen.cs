using UnityEngine;

public abstract class MyScreen : AvMain
{
	public static int ITEM_HEIGHT = 20;

	public static Image imgLogo;

	public static Image[] imgChat;

	public int selected;

	public static int nMsg = 0;

	public static int hTab = 20;

	public static int wTab;

	public static int hText;

	public static int colorBar = 0;

	public static Color color;

	public static int colorMiniMap = 0;

	public static int colorPark = 8705740;

	public static int[] colorCity = new int[2] { 4802889, 3092271 };

	public static int[] colorFarmPath = new int[2] { 14400144, 12689526 };

	public virtual void setSelected(int se, bool iss)
	{
		selected = se;
	}

	public void action(int sel)
	{
	}

	public virtual void doMenu()
	{
	}

	public virtual void doSetting()
	{
	}

	public virtual void doMenuTab()
	{
	}

	public virtual void close()
	{
	}

	public static int getHTF()
	{
		return Canvas.h - hTab - 17 * AvMain.hd;
	}

	public virtual void setHidePointer(bool iss)
	{
		isHide = iss;
	}

	public virtual void switchToMe()
	{
		Canvas.currentMyScreen = this;
	}

	public virtual void initZoom()
	{
	}

	public override void paint(MyGraphics g)
	{
		if (Canvas.menuMain == null)
		{
			if (Canvas.currentDialog == null && Canvas.currentFace == null && !ChatTextField.isShow)
			{
				base.paint(g);
			}
			else
			{
				Canvas.resetTransNotZoom(g);
			}
			if (!Session_ME.gI().isConnected())
			{
				Canvas.arialFont.drawString(g, "2.5.8", Canvas.posByteCOunt.x, Canvas.posByteCOunt.y, Canvas.posByteCOunt.anchor);
			}
			else if (Canvas.currentMyScreen == ServerListScr.me)
			{
				Canvas.arialFont.drawString(g, Session_ME.strRecvByteCount + string.Empty, Canvas.posByteCOunt.x, Canvas.posByteCOunt.y, Canvas.posByteCOunt.anchor);
			}
		}
	}

	public virtual void paintMain(MyGraphics g)
	{
	}

	public abstract void update();

	public override void keyPress(int keyCode)
	{
	}

	public override void commandTab(int index)
	{
	}

	public override void commandAction(int index)
	{
	}

	public virtual void doChat(string text)
	{
	}

	public void repaint()
	{
	}
}
