public class Bus
{
	public static Image imgBus;

	private int x;

	private int y;

	private int v;

	private int g;

	private int count;

	public static sbyte damToc;

	public static sbyte direct;

	public static AvPosition posBusStop;

	public static bool isRun;

	public static bool isExit;

	public void setBus(sbyte dir)
	{
		if (!isRun && GameMidlet.avatar.action != -1)
		{
			direct = dir;
			y = LoadMap.Hmap * LoadMap.w - imgBus.getHeight() / AvMain.hd + 10 + 20 * AvMain.hd;
			x = posBusStop.x + 300;
			if (direct == 1)
			{
				AvCamera.gI().setToPos(posBusStop.x, AvCamera.gI().yCam + (float)(Canvas.hCan / 2));
				AvCamera.gI().xCam = AvCamera.gI().xTo;
				GameMidlet.avatar.y = (GameMidlet.avatar.yCur -= LoadMap.w);
			}
			v = (g = 15);
			count = 0;
			damToc = 1;
			isRun = true;
			GameMidlet.avatar.setAction(-1);
			AvCamera.disable = true;
			isExit = false;
			if (direct == 1)
			{
				GameMidlet.avatar.ableShow = true;
			}
		}
	}

	public void update()
	{
		if (((damToc == 1 && direct == 1) || (damToc == -1 && direct == -1)) && direct == -1 && !isExit)
		{
			GlobalService.gI().getHandler(8);
			isExit = true;
			GameMidlet.avatar.ableShow = true;
		}
		x -= v;
		count += CRes.abs(g - v / 2);
		if (count >= 20)
		{
			count = 0;
			v -= damToc;
			if (v == 0)
			{
				damToc = -1;
				g = 8;
				GameMidlet.avatar.setAction(0);
				AvCamera.disable = false;
				GameMidlet.avatar.ableShow = false;
				AvCamera.gI().setPosFollowPlayer();
				if (Canvas.isInitChar && Session_ME.gI().isConnected())
				{
					if (LoadMap.TYPEMAP == 9)
					{
						Canvas.welcome = new Welcome();
						Canvas.welcome.initMapScr();
					}
					else if (direct == 1 && LoadMap.TYPEMAP == 25)
					{
						Canvas.welcome = new Welcome();
						Canvas.welcome.initFarmPath(MapScr.instance);
					}
					else if (LoadMap.TYPEMAP == 13 && Welcome.indexFish < 8)
					{
						Canvas.welcome = new Welcome();
						Canvas.welcome.initFish();
					}
					else if (direct == 1 && LoadMap.TYPEMAP == 23)
					{
						Canvas.welcome = new Welcome();
						Canvas.welcome.initKhuMuaSam();
					}
				}
			}
			else if (direct == 1 && damToc == 1)
			{
				AvCamera.gI().update();
			}
		}
		if ((float)((x + 58) * AvMain.hd) < AvCamera.gI().xCam)
		{
			isRun = false;
			if (direct == -1)
			{
				Canvas.startWaitDlg();
			}
		}
	}

	public void paint(MyGraphics g)
	{
		int num = 0;
		if (v > 1)
		{
			num = ((Canvas.gameTick % 6 < 3) ? 1 : 0);
		}
		g.drawImage(imgBus, x * AvMain.hd, (y + num) * AvMain.hd, MyGraphics.TOP | MyGraphics.HCENTER);
	}
}
