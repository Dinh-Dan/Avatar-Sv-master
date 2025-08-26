public class RegisterScr : MyScreen
{
	private class IActionOkUser : IAction
	{
		public void perform()
		{
			instance.doFinish();
		}
	}

	public static RegisterScr instance;

	public static bool isCreateChar;

	public sbyte male = 1;

	public int index;

	public new int selected;

	public int countLeft;

	public int countRight;

	private MyVector listHair;

	private MyVector listClothing;

	private MyVector listQ;

	private int time;

	public static RegisterScr gI()
	{
		if (instance == null)
		{
			instance = new RegisterScr();
		}
		return instance;
	}

	public override void commandTab(int index)
	{
		if (index == 0)
		{
			doFinish();
		}
	}

	public override void switchToMe()
	{
		GameMidlet.avatar.seriPart = new MyVector();
		GameMidlet.avatar.direct = Base.RIGHT;
		getAvatarPart();
		center = new Command(T.success, 0);
		SeriPart seriPart = new SeriPart();
		int i = CRes.r.nextInt(listQ.size());
		seriPart.idPart = ((APartInfo)listQ.elementAt(i)).IDPart;
		GameMidlet.avatar.addSeri(seriPart);
		SeriPart seriPart2 = new SeriPart();
		int i2 = CRes.r.nextInt(listClothing.size());
		seriPart2.idPart = ((APartInfo)listClothing.elementAt(i2)).IDPart;
		GameMidlet.avatar.addSeri(seriPart2);
		SeriPart seriPart3 = new SeriPart();
		seriPart3.idPart = 4;
		GameMidlet.avatar.addSeri(seriPart3);
		SeriPart seriPart4 = new SeriPart();
		int i3 = CRes.r.nextInt(listHair.size());
		seriPart4.idPart = ((APartInfo)listHair.elementAt(i3)).IDPart;
		GameMidlet.avatar.addSeri(seriPart4);
		GameMidlet.avatar.addSeri(new SeriPart(0));
		GameMidlet.avatar.orderSeriesPath();
		init();
		Canvas.paint.initReg();
		base.switchToMe();
	}

	public override void doMenu()
	{
		LoginScr.gI().doMenu();
	}

	public void init()
	{
		PaintPopup.gI().setInfo(T.createChar, 150 * AvMain.hd, 170 + ((AvMain.hd == 2) ? 160 : 0), 1, -1, null, null);
	}

	public void getAvatarPart()
	{
		GameMidlet.avatar.gender = male;
		if (listHair != null)
		{
			listHair.removeAllElements();
			listClothing.removeAllElements();
			listQ.removeAllElements();
		}
		listHair = new MyVector();
		listClothing = new MyVector();
		listQ = new MyVector();
		for (int i = 0; i < AvatarData.listPart.Length; i++)
		{
			if (!(AvatarData.listPart[i] is APartInfo))
			{
				continue;
			}
			APartInfo aPartInfo = (APartInfo)AvatarData.listPart[i];
			if (aPartInfo != null && (aPartInfo.gender == male || aPartInfo.gender == 0) && aPartInfo.level == 0)
			{
				if (aPartInfo.zOrder == 50)
				{
					listHair.addElement(aPartInfo);
				}
				else if (aPartInfo.zOrder == 20)
				{
					listClothing.addElement(aPartInfo);
				}
				else if (aPartInfo.zOrder == 10)
				{
					listQ.addElement(aPartInfo);
				}
			}
		}
		selected = 0;
		getId();
		GameMidlet.avatar.orderSeriesPath();
		if (GameMidlet.avatar.action != 10)
		{
			GameMidlet.avatar.setAction(1);
		}
	}

	protected void doFinish()
	{
		Canvas.isInitChar = true;
		Canvas.startWaitDlg(T.createChar + "...");
		GlobalService.gI().doRequestCreCharacter();
	}

	public override void keyPress(int keyCode)
	{
		base.keyPress(keyCode);
	}

	public override void update()
	{
		if (countLeft > 0)
		{
			countLeft--;
		}
		if (countRight > 0)
		{
			countRight--;
		}
		time++;
		if (time > 50)
		{
			time = 0;
			int num = CRes.r.nextInt(3);
			if (GameMidlet.avatar.action != 10)
			{
				if (num == 0)
				{
					GameMidlet.avatar.setAction(1);
				}
				else
				{
					GameMidlet.avatar.setAction(0);
				}
			}
		}
		GameMidlet.avatar.updateAvatar();
	}

	public void setKeyUpDown(int ind)
	{
		index = ind;
		if (index < 0)
		{
			index = 1;
		}
		if (index > 1)
		{
			index = 0;
		}
	}

	public void setKeyLeftRight(int ind)
	{
		selected += ind;
		if (selected < 0)
		{
			selected = 1;
		}
		if (selected > 1)
		{
			selected = 0;
		}
		if (index == 0)
		{
			if (male == 1)
			{
				male = 2;
			}
			else
			{
				male = 1;
			}
			getAvatarPart();
		}
		else
		{
			getId();
		}
	}

	public override void updateKey()
	{
		Canvas.paint.updateKeyRegister();
		base.updateKey();
	}

	private void getId()
	{
		for (int i = 0; i < GameMidlet.avatar.seriPart.size(); i++)
		{
			SeriPart seriPart = (SeriPart)GameMidlet.avatar.seriPart.elementAt(i);
			APartInfo aPartInfo = (APartInfo)AvatarData.getPart(seriPart.idPart);
			if (aPartInfo.zOrder == 50 && listHair.size() != 0 && selected < listHair.size())
			{
				seriPart.idPart = ((APartInfo)listHair.elementAt(selected)).IDPart;
			}
			if (aPartInfo.zOrder == 20 && listClothing.size() != 0 && selected < listClothing.size())
			{
				seriPart.idPart = ((APartInfo)listClothing.elementAt(selected)).IDPart;
			}
			if (aPartInfo.zOrder == 10 && listQ.size() != 0 && selected < listQ.size())
			{
				seriPart.idPart = ((APartInfo)listQ.elementAt(selected)).IDPart;
			}
		}
		GameMidlet.avatar.orderSeriesPath();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.loadMap.paint(g);
		Canvas.loadMap.paintObject(g);
		Canvas.resetTrans(g);
		PaintPopup.gI().paint(g);
		Canvas.resetTrans(g);
		g.translate(PaintPopup.gI().x, PaintPopup.gI().y);
		Canvas.paint.paintPlayer(g, index, male, countLeft, countRight);
		base.paint(g);
	}

	public void onCreaCharacter(bool isCreaCha)
	{
		Canvas.endDlg();
		if (isCreaCha)
		{
			MapScr.gI().joinCitymap();
		}
		else
		{
			Canvas.startOKDlg(T.createCharFail);
		}
	}

	public void onRegister(string userName, string pass)
	{
		Canvas.user = userName;
		Canvas.pass = pass;
		AvatarData.saveMyAccount();
		GlobalMessageHandler.gI().miniGameMessageHandler = null;
		ServerListScr.gI().login();
	}
}
