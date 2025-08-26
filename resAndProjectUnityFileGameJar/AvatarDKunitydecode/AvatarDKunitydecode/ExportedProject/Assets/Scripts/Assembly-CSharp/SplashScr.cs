using UnityEngine;

public class SplashScr : MyScreen
{
	public static SplashScr me;

	public static int iDraw = -1;

	public static long splashScrStat;

	public new static Image imgLogo;

	public static bool isOpen;

	public static bool isSelected;

	private bool isDraw;

	public static SplashScr gI()
	{
		return (me != null) ? me : (me = new SplashScr());
	}

	public static void init()
	{
	}

	public override void switchToMe()
	{
		if (me != Canvas.currentMyScreen)
		{
			splashScrStat = Canvas.getTick();
			imgLogo = Image.createImagePNG("sp/" + Screen.width + "x" + Screen.height);
			if (imgLogo == null)
			{
				imgLogo = Image.createImagePNG("sp/" + Screen.height + "x" + Screen.width);
			}
			if (imgLogo == null)
			{
				imgLogo = Image.createImagePNG("sp/2048x1536");
			}
			base.switchToMe();
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 50:
			selectedLanguage(0);
			break;
		case 51:
			selectedLanguage(1);
			break;
		}
	}

	public override void updateKey()
	{
	}

	public void switchLogin()
	{
		LoginScr.gI().loadLogin();
		OptionScr.gI().load();
		isSelected = true;
		LoginScr.gI().switchToMe();
		AvatarData.loadIP();
	}

	public override void update()
	{
		if ((Canvas.getTick() - splashScrStat) / 1000 > 3)
		{
			if (onMainMenu.iChangeGame != 0)
			{
				if (onMainMenu.iChangeGame == 2)
				{
					MapScr.gI().switchToMe();
					imgLogo = null;
					onMainMenu.iChangeGame = 0;
					Canvas.paint.resetOngame();
					onMainMenu.resetImg();
					onMainMenu.resetSize();
				}
			}
			else
			{
				switchLogin();
			}
		}
		else if (onMainMenu.iChangeGame != 0 && splashScrStat == 0)
		{
			isOpen = true;
			splashScrStat++;
			return;
		}
		splashScrStat++;
	}

	private void selectedLanguage(int type)
	{
		LoginScr.isSelectedLanguage = true;
		isSelected = false;
		Canvas.isPointerClick = false;
		Canvas.paint.initString(type);
		OptionScr.gI().mapFocus[4] = type;
		OptionScr.gI().save(0);
		LoginScr.gI().switchToMe();
		LoginScr.gI().load();
		imgLogo = null;
		LoginScr.gI().tfUser.setFocus(true);
	}

	public override void paint(MyGraphics g)
	{
		g.setColor(16777215);
		g.fillRect(0f, 0f, (int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
		g.drawImageScale(imgLogo, 0, 0, (int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT, 0);
		Canvas.paintPlus(g);
	}
}
