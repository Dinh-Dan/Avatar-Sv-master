using UnityEngine;

public class OnSplashScr : MyScreen
{
	public static OnSplashScr me;

	private static int runningPercent;

	public int splashScrStat;

	public static Image imgLogomainMenu;

	public static Image imgBg;

	public static bool isOpen;

	public static OnSplashScr gI()
	{
		return (me != null) ? me : (me = new OnSplashScr());
	}

	public override void switchToMe()
	{
		if (Screen.orientation == ScreenOrientation.Portrait)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Canvas.isRotateTop = 2;
		}
		Canvas.listInfoSV.removeAllElements();
		Canvas.transTab = 0;
		Canvas.instance.setSize((int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
		onMainMenu.isOngame = true;
		imgLogomainMenu = Image.createImagePNG(T.mode[AvMain.hd - 1] + "/hd/on/logo");
		if (imgBg == null)
		{
			imgBg = Image.createImage("backgroundOn");
		}
		base.switchToMe();
		Canvas.paint.clearImgAvatar();
	}

	public override void update()
	{
		if (!isOpen)
		{
			if (splashScrStat > 10)
			{
				onMainMenu.gI().switchToMe();
			}
			else if (splashScrStat == 1)
			{
				onMainMenu.initSize();
				Canvas.paint.initOngame();
			}
			splashScrStat++;
		}
	}

	public override void paint(MyGraphics g)
	{
		Canvas.paint.paintDefaultBg(g);
		g.drawImage(imgLogomainMenu, Canvas.hw, Canvas.hh, 3);
		Canvas.paintPlus(g);
	}
}
