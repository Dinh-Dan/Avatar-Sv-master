using System;
using UnityEngine;

public class ServerListScr : MyScreen, HTTPHandler
{
	private class actDoSettingPassword : IKbAction
	{
		public void perform(string text)
		{
			string text2 = Canvas.inputDlg.getText();
			if (!text.Equals(string.Empty))
			{
				LoginScr.gI().doForgetPass(text);
			}
		}
	}

	public static ServerListScr me;

	public static int indexSV;

	public static int index;

	public new static int selected;

	public Image imgF;

	public Image imgArr;

	private bool isSelected;

	public static int cmtoY;

	public static int cmy;

	public static int cmdy;

	public static int cmvy;

	public static int cmyLim;

	public static int w;

	public static int h;

	public static int hDis;

	public static int x;

	public static int y;

	private sbyte countClose;

	private int indexUSer;

	private long timeDelay;

	private int vY;

	private bool transY;

	private int pa;

	private string test = string.Empty;

	private long count;

	private long timePoint;

	private int dyTran;

	private int timeOpen;

	private int pyLast;

	private bool isFire;

	private bool isG;

	private bool isTranKey;

	public ServerListScr()
	{
		imgArr = Image.createImagePNG(T.getPath() + "/ios/i1");
		imgF = Image.createImage(T.getPath() + "/effect/tp");
		initCmd();
		w = 200 * AvMain.hd;
		h = 250 * AvMain.hd;
		hDis = h - 20;
	}

	public static ServerListScr gI()
	{
		return (me != null) ? me : (me = new ServerListScr());
	}

	public override void switchToMe()
	{
		base.switchToMe();
		init();
		indexUSer = 0;
		if (center == null)
		{
			initCmd();
		}
		chans();
	}

	public override void doMenu()
	{
		LoginScr.gI().doMenu();
	}

	public override void commandAction(int index)
	{
		switch (index)
		{
		case 2:
			Canvas.startOKDlg(T.doYouWantExit2, 54);
			break;
		case 3:
			Canvas.startOK(T.uNeedExitGame, 55);
			break;
		case 4:
			ipKeyboard.openKeyBoard(T.nameAcc, ipKeyboard.TEXT, string.Empty, new actDoSettingPassword(), false);
			break;
		case 5:
			OptionScr.gI().switchToMe();
			break;
		case 6:
			GameMidlet.flatForm("http://wap.teamobi.com/faqs.php?provider=" + GameMidlet.PROVIDER);
			break;
		case 7:
			GameMidlet.flatForm("http://wap.teamobi.com?info=checkupdate&game=8&version=2.5.8&provider=" + GameMidlet.PROVIDER);
			break;
		case 9:
			Canvas.startOKDlg(T.alreadyDelRMS + T.delRMS);
			AvatarData.delRMS();
			break;
		case 10:
			closeTabAll();
			break;
		}
	}

	public void login()
	{
		GameMidlet.avatar = new Avatar();
		Out.println("login");
		LoginScr.gI().timeOut = Canvas.getTick();
		int num = 0;
		for (int i = 0; i < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length; i++)
		{
			num += GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length;
			if (num >= selected)
			{
				indexSV = i;
				int num2 = selected - (num - GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length) - 1;
				if (num2 >= 0 && num2 < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length - 1)
				{
					index = num2;
				}
				break;
			}
		}
		Canvas.startWaitCancelDlg(T.logging);
		Session_ME.gI().close();
		LoginScr.gI().login();
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			login();
			break;
		case 1:
			doUpdateServer();
			break;
		}
	}

	public void initCmd()
	{
		if (T.selectt != null)
		{
		}
	}

	public void doUpdateServer()
	{
		Canvas.startWaitDlg();
		MsgDlg.isBack = false;
		if (indexUSer >= GameMidlet.linkGetHost.Length)
		{
			Canvas.startOKDlg(T.canNotConnect);
			indexUSer = 0;
		}
		else
		{
			Net.connectHTTP(GameMidlet.linkGetHost[OptionScr.gI().mapFocus[4]][indexUSer], this);
		}
	}

	public void init()
	{
		x = (Canvas.w - w) / 2;
		y = (Canvas.hCan - h) / 2;
		isHide = true;
		int num = 0;
		for (int i = 0; i < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length; i++)
		{
			num += GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length;
		}
		cmyLim = num * MyScreen.hText - hDis;
		cmy = (cmtoY = 0);
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
	}

	private void click()
	{
		int num = (cmtoY + Canvas.py - (y + 10)) / MyScreen.hText;
		if (index < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][indexSV].Length)
		{
			commandTab(0);
		}
		isHide = true;
	}

	public override void update()
	{
		if (timeOpen > 0)
		{
			timeOpen--;
			if (timeOpen == 0)
			{
				click();
			}
		}
		if (vY != 0)
		{
			if (cmy < 0 || cmy > cmyLim)
			{
				if (vY > 500)
				{
					vY = 500;
				}
				else if (vY < -500)
				{
					vY = -500;
				}
				vY -= vY / 5;
				if (CRes.abs(vY / 10) <= 10)
				{
					vY = 0;
				}
			}
			cmy += vY / 15;
			cmtoY = cmy;
			vY -= vY / 20;
		}
		else if (cmy < 0)
		{
			cmtoY = 0;
		}
		else if (cmy > cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (cmy != cmtoY)
		{
			cmvy = cmtoY - cmy << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
		Canvas.loadMap.update();
	}

	private void setIndex(int index)
	{
		indexSV = index;
		if (indexSV >= GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length)
		{
			indexSV = 0;
		}
		if (indexSV < 0)
		{
			indexSV = GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length - 1;
		}
	}

	public override void setSelected(int se, bool isAction)
	{
		selected = se;
		if (selected >= GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][indexSV].Length || selected <= 0)
		{
			selected = 0;
			if (isAction)
			{
				isSelected = false;
				init();
			}
		}
	}

	public override void closeTabAll()
	{
		isSelected = false;
		indexSV = 0;
		selected = 0;
		LoginScr.gI().switchToMe();
	}

	public override void updateKey()
	{
		count++;
		if (Canvas.isPointerClick)
		{
			if (Canvas.isPoint(x + w - 3 * AvMain.hd - 20 * AvMain.hd, y + AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				isTranKey = true;
				Canvas.isPointerClick = false;
				countClose = 5;
			}
			else if (Canvas.isPoint(x, y, w, h))
			{
				isTran = true;
				isG = false;
				if (vY != 0)
				{
					isG = true;
				}
				pyLast = Canvas.pyLast;
				Canvas.isPointerClick = false;
				pa = cmy;
				transY = true;
				timeDelay = count;
				isFire = false;
				int num = (cmtoY + Canvas.py - (y + 10)) / MyScreen.hText;
				selected = num;
				int num2 = 0;
				for (int i = 0; i < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length; i++)
				{
					num2 += GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length;
					if (num2 >= selected)
					{
						indexSV = i;
						int num3 = selected - (num2 - GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length) - 1;
						if (num3 >= 0 && num3 < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length - 1)
						{
							index = num3;
							isFire = true;
						}
						break;
					}
				}
			}
		}
		if (isTranKey)
		{
			if (Canvas.isPointerDown && !Canvas.isPoint(x + w - 3 * AvMain.hd - 20 * AvMain.hd, y + AvMain.hd - 20 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				countClose = 0;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranKey = false;
				if (countClose > 0)
				{
					LoginScr.gI().switchToMe();
				}
				countClose = 0;
			}
		}
		if (transY)
		{
			long num4 = count - timeDelay;
			int num5 = pyLast - Canvas.py;
			pyLast = Canvas.py;
			if (Canvas.isPointerDown)
			{
				if (count % 2 == 0)
				{
					dyTran = Canvas.py;
					timePoint = count;
				}
				vY = 0;
				if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd)
				{
					isHide = true;
				}
				else if (num4 > 3 && num4 < 8 && isFire && !isG)
				{
					isHide = false;
				}
				test = pa + "    " + num5;
				cmtoY = pa + num5;
				if (cmtoY < 0 || cmtoY > cmyLim)
				{
					cmtoY = pa + num5 / 2;
				}
				pa = cmtoY;
				cmy = cmtoY;
			}
			if (Canvas.isPointerRelease && Canvas.isPoint(x, y, w, h))
			{
				isG = false;
				int num6 = (int)(count - timePoint);
				int num7 = dyTran - Canvas.py;
				if (CRes.abs(num7) > 40 && num6 < 10 && cmtoY > 0 && cmtoY < cmyLim)
				{
					vY = num7 / num6 * 10;
				}
				timePoint = -1L;
				if (Math.abs(Canvas.dy()) < 10 * AvMain.hd && isFire)
				{
					if (num4 <= 4)
					{
						if (isFire)
						{
							isHide = false;
						}
						timeOpen = 5;
					}
					else if (!isHide)
					{
						click();
					}
					isFire = false;
				}
			}
		}
		if (Canvas.isPointerRelease)
		{
			transY = false;
		}
		base.updateKey();
	}

	private void chans()
	{
		cmtoY = 0;
		if (cmtoY < 0)
		{
			cmtoY = 0;
		}
		if (cmtoY > cmyLim)
		{
			cmtoY = cmyLim;
		}
	}

	public override void paintMain(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		Canvas.loadMap.paint(g);
		Canvas.loadMap.paintObject(g);
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		if (Canvas.currentDialog == null && Canvas.menuMain == null)
		{
			Canvas.paint.paintPopupBack(g, x, y, w, h, countClose / 3, false);
			g.translate(x, y + 10);
			g.setClip(0f, 0f, w, hDis);
			g.translate(0f, -cmy);
			if (!isHide)
			{
				g.setColor(16777215);
				g.fillRect(12 * AvMain.hd, selected * MyScreen.hText, w - 24 * AvMain.hd, MyScreen.hText);
			}
			int num = (MyScreen.hText - AvMain.hNormal) / 2;
			for (int i = 0; i < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]].Length; i++)
			{
				Canvas.normalFont.drawString(g, GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i][0], 24 * AvMain.hd, num, 0);
				g.drawImage(imgArr, 14 * AvMain.hd, num + AvMain.hNormal / 2, 3);
				num += MyScreen.hText;
				for (int j = 1; j < GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i].Length; j++)
				{
					Canvas.normalFont.drawString(g, GameMidlet.nameSV[OptionScr.gI().mapFocus[4]][i][j], 36 * AvMain.hd, num, 0);
					g.drawImage(imgF, 24 * AvMain.hd, num + AvMain.hNormal / 2, 3);
					num += MyScreen.hText;
				}
			}
			base.paint(g);
		}
		else
		{
			LoginScr.gI().paintLogo(g);
		}
		Canvas.paintPlus(g);
	}

	public void onGetText(string s)
	{
		if (s.Equals(string.Empty))
		{
			indexUSer++;
			doUpdateServer();
			return;
		}
		bool flag = false;
		if (s == null || s == string.Empty)
		{
			flag = true;
		}
		string[][][] array = null;
		int[][][] array2 = null;
		string[][][] array3 = null;
		try
		{
			string[] array4 = s.Split('*');
			if (array4.Length == 0 || array4.Length == 1)
			{
				flag = true;
			}
			array2 = new int[2][][];
			array = new string[2][][];
			array3 = new string[2][][];
			array2[OptionScr.gI().mapFocus[4]] = new int[array4.Length - 1][];
			array[OptionScr.gI().mapFocus[4]] = new string[array4.Length - 1][];
			array3[OptionScr.gI().mapFocus[4]] = new string[array4.Length - 1][];
			for (int i = 1; i < array4.Length; i++)
			{
				string[] array5 = array4[i].Split('\n');
				array3[OptionScr.gI().mapFocus[4]][i - 1] = new string[array5.Length - 1];
				array[OptionScr.gI().mapFocus[4]][i - 1] = new string[array5.Length - 2];
				array2[OptionScr.gI().mapFocus[4]][i - 1] = new int[array5.Length - 2];
				array3[OptionScr.gI().mapFocus[4]][i - 1][0] = array5[0];
				for (int j = 1; j < array5.Length - 1; j++)
				{
					string[] array6 = array5[j].Split(':');
					array3[OptionScr.gI().mapFocus[4]][i - 1][j] = array6[0];
					array[OptionScr.gI().mapFocus[4]][i - 1][j - 1] = array6[1];
					array6[2] = array6[2].Substring(0, array6[2].Length - 1);
					array2[OptionScr.gI().mapFocus[4]][i - 1][j - 1] = int.Parse(array6[2]);
				}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
			flag = true;
		}
		if (flag)
		{
			indexUSer++;
			doUpdateServer();
			return;
		}
		GameMidlet.IP = array;
		GameMidlet.PORT = array2;
		GameMidlet.nameSV = array3;
		AvatarData.saveIP();
		Canvas.endDlg();
		if (LoginScr.isLoadIP)
		{
			LoginScr.isLoadIP = false;
			LoginScr.gI().regRequest();
		}
		else
		{
			init();
			switchToMe();
		}
	}
}
