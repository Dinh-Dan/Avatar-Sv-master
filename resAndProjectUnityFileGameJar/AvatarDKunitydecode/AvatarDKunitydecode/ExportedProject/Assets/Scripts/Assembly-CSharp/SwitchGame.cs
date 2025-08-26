using System.IO;

public class SwitchGame : MyScreen
{
	public static SwitchGame me;

	private int type;

	private Image[] imageElements = new Image[2];

	private string[] stringElements;

	private int aa;

	private MyScreen last;

	public static SwitchGame gI()
	{
		return (me != null) ? me : (me = new SwitchGame());
	}

	public void setInfo(int type)
	{
		this.type = type;
		last = Canvas.currentMyScreen;
		try
		{
			if (type == 0)
			{
				stringElements = new string[2] { "Ongame", "Avatar" };
				imageElements[0] = Image.createImage(T.getPath() + "/on/10");
				imageElements[1] = Image.createImage(T.getPath() + "/on/icon57");
				aa = imageElements[1].getHeight() * 2;
			}
			else
			{
				stringElements = new string[2] { "Avatar", "Ongame" };
				imageElements[0] = Image.createImage(T.getPath() + "/on/icon57");
				imageElements[1] = Image.createImage(T.getPath() + "/on/10");
				aa = imageElements[0].getHeight() * 2;
			}
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		aa++;
	}

	public override void updateKey()
	{
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		Canvas.isPointerRelease = false;
		for (int i = 0; i < 2; i++)
		{
			if (!Canvas.isPoint(10, 10 + i * aa, aa + Canvas.blackF.getWidth(stringElements[i]) + 10, aa))
			{
				continue;
			}
			if (i == 0)
			{
				if (type == 0)
				{
					last.switchToMe();
				}
				else
				{
					onMainMenu.iChangeGame = 1;
					Canvas.cameraList.close();
					GlobalService.gI().joinAvatar();
					SplashScr.gI().switchToMe();
					onMainMenu.resetSize();
				}
			}
			else if (OnSplashScr.isOpen)
			{
				onMainMenu.isOngame = false;
				MapScr.gI().switchToMe();
				AvCamera.gI().init(LoadMap.TYPEMAP);
			}
			else
			{
				onMainMenu.isOngame = true;
				onMainMenu.gI().switchToMe();
			}
			OnSplashScr.isOpen = false;
			SplashScr.isOpen = false;
		}
	}

	public override void update()
	{
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.setColor(16777215);
		g.fillRect(0f, 0f, Canvas.w, (int)ScaleGUI.HEIGHT);
		for (int i = 0; i < 2; i++)
		{
			g.drawImage(imageElements[i], 10 + aa / 2, aa / 2 + 10 + aa * i, 3);
			Canvas.blackF.drawString(g, stringElements[i], 20 + aa, aa / 2 + 10 + aa * i - AvMain.hBlack / 2, 0);
		}
	}
}
