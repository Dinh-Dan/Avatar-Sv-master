using System;
using UnityEngine;

public class GlobalLogicHandler
{
	private class IActionConnectFailYes : IAction
	{
		public void perform()
		{
			ServerListScr.gI().doUpdateServer();
			if (Canvas.currentMyScreen == MiniMap.me)
			{
				Canvas.listAc.addElement(new IAciton22());
			}
		}
	}

	private class IActionConnectFailNo : IAction
	{
		public void perform()
		{
			if (Canvas.currentMyScreen == MiniMap.me)
			{
				Canvas.listAc.addElement(new IAciton22());
			}
		}
	}

	private class IActionDis : IAction
	{
		public void perform()
		{
			AvCamera.gI().xCam = (AvCamera.gI().xTo = 100f);
			MapScr.gI().exitGame();
			LoginScr.gI().switchToMe();
			if (Screen.orientation != ScreenOrientation.Portrait)
			{
				LoginScr.gI().init(Screen.height);
			}
		}
	}

	private class IAciton22 : IAction
	{
		public void perform()
		{
			Session_ME.gI().close();
			LoginScr.gI().switchToMe();
		}
	}

	public class IActionDisconnect : IAction
	{
		public void perform()
		{
			if (Canvas.currentMyScreen != RegisterScr.gI())
			{
				AvCamera.gI().xCam = (AvCamera.gI().xTo = 100f);
				MapScr.gI().exitGame();
				LoginScr.gI().switchToMe();
			}
		}
	}

	private class IAciton33 : IAction
	{
		public void perform()
		{
			LoginScr.gI().switchToMe();
		}
	}

	private class IActionRegisterOK : IAction
	{
		private readonly string smsPrefix;

		private readonly string username;

		private readonly string smsTo;

		public IActionRegisterOK(string smsPrefix, string username, string smsTo)
		{
			this.smsPrefix = smsPrefix;
			this.username = username;
			this.smsTo = smsTo;
		}

		public void perform()
		{
			Canvas.startOKDlg(T.registerSuccess);
			GlobalService.gI().sendSMSSuccess(smsPrefix + username + " " + LoginScr.gI().passRemem, smsTo);
			LoginScr.gI().passRemem = string.Empty;
		}
	}

	private class IActionRegisterFail : IAction
	{
		private string info;

		private string to;

		public IActionRegisterFail(string i, string t)
		{
			info = i;
			to = t;
		}

		public void perform()
		{
			Canvas.startOKDlg(T.cannotRegister + " Soạn tin nhắn: " + info + " gửi đến " + to + " để đăng ký!");
		}
	}

	private class IActionVersionDown : IAction
	{
		private readonly string url;

		public IActionVersionDown(string url)
		{
			this.url = url;
		}

		public void perform()
		{
			GameMidlet.flatForm(url);
		}
	}

	private class IActionUpdateContainer : IAction
	{
		public void perform()
		{
			GlobalService.gI().doUpdateContainer(1);
			Canvas.startWaitDlg();
		}
	}

	private class IActionOnSMS : IAction
	{
		private readonly string syst;

		private readonly string number1;

		public IActionOnSMS(string syst, string number1)
		{
			this.syst = syst;
			this.number1 = number1;
		}

		public void perform()
		{
			GameMidlet.sendSMS(syst, string.Empty + number1, new IActionOnSMS1(), new IActionOnSMS2());
		}
	}

	private class IActionOnSMS1 : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.sentMsg);
		}
	}

	private class IActionOnSMS2 : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.canNotSendMsg);
		}
	}

	private class IActionMenuOption : IAction
	{
		private int userID;

		private sbyte IDMenu;

		private int ii;

		public IActionMenuOption(int ii, int userID, sbyte IDMenu)
		{
			this.ii = ii;
			this.userID = userID;
			this.IDMenu = IDMenu;
		}

		public void perform()
		{
			if (!ListScr.gI().setList(userID + "-" + IDMenu + "-" + ii))
			{
				GlobalService.gI().doMenuOption(userID, IDMenu, ii);
			}
		}
	}

	private class IActionTextBox : IAction
	{
		private readonly int userID;

		private readonly sbyte idMenu;

		public IActionTextBox(int userID, sbyte idMenu)
		{
			this.userID = userID;
			this.idMenu = idMenu;
		}

		public void perform()
		{
			GlobalService.gI().doTextBox(userID, idMenu, Canvas.inputDlg.getText());
			Canvas.endDlg();
		}
	}

	private class IActionChest : IAction
	{
		public void perform()
		{
			GlobalService.gI().doUpdateChest(1);
			Canvas.startWaitDlg();
		}
	}

	private bool isCon;

	public static bool isAutoLogin;

	public static bool isNewVersion;

	public void onConnectFail()
	{
		Out.println("onConnectFail");
		if (Canvas.currentMyScreen != LoginScr.me)
		{
			if (!isCon)
			{
				Canvas.msgdlg.setInfoLR(T.connectFail, new Command(T.yes, new IActionConnectFailYes()), new Command(T.no, new IActionDisconnect()));
			}
			else
			{
				Canvas.startOK(T.cityIsOffLine, new IActionDisconnect());
			}
		}
		isCon = true;
	}

	public void onConnectOK()
	{
	}

	public static void onDisconnect()
	{
		Out.println("onDisconnect");
		try
		{
			GameMidlet.CLIENT_TYPE = 8;
			Canvas.menuMain = null;
			HouseScr.me = null;
			MessageScr.me = null;
			Canvas.currentPopup.removeAllElements();
			Canvas.currentFace = null;
			LoadMap.w = 24;
			LoadMap.rememMap = -1;
			ChatTextField.isShow = false;
			Canvas.endDlg();
			FarmData.init();
			if (ipKeyboard.tk != null)
			{
				ipKeyboard.tk.active = false;
				ipKeyboard.tk = null;
			}
			if (Canvas.menuMain != null)
			{
				Canvas.menuMain = null;
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		if (Canvas.currentMyScreen != LoginScr.me && Canvas.currentMyScreen != ServerListScr.me)
		{
			Canvas.setPopupTime(T.disConnect);
			if (Canvas.currentMyScreen != RegisterScr.gI())
			{
				Canvas.listAc.addElement(new IActionDis());
			}
		}
		isAutoLogin = false;
	}

	public void onLoginFail(string reason)
	{
		Canvas.startOKDlg(reason);
		Canvas.startOK(reason, new IAciton33());
	}

	public void onLoginSuccess()
	{
		isAutoLogin = false;
		AvatarData.listImgIcon = new MyHashTable();
		AvatarMsgHandler.onHandler();
		Out.println("onLoginSuccess: " + AvatarData.playing + "   " + Canvas.currentMyScreen);
		if (AvatarData.playing == -1)
		{
			AvatarService.gI().getBigData();
		}
		else
		{
			MapScr.gI().joinCitymap();
		}
		AvatarService.gI().doRequestExpicePet(GameMidlet.avatar.IDDB);
	}

	public void onRegisterInfo(string username, bool available, string smsPrefix, string smsTo)
	{
		Canvas.endDlg();
		if (available)
		{
			string text = smsPrefix + username + " " + LoginScr.gI().passRemem;
			GameMidlet.sendSMS(text, string.Empty + smsTo, new IActionRegisterOK(smsPrefix, username, smsTo), new IActionRegisterFail(text, smsTo));
		}
	}

	public void onChatFrom(int id, string name, string info)
	{
		MessageScr.gI().addPlayer(id, name, info, false, null);
	}

	public void onServerInfo(string info)
	{
		Canvas.instance.setInfoSV(info);
	}

	public void onServerMessage(string msg)
	{
		Canvas.startOKDlg(msg);
	}

	public void onVersion(string info, string url)
	{
		IAction action = new IActionVersionDown(url);
		Canvas.msgdlg.setIsWaiting(false);
		Canvas.msgdlg.setInfoLR(info, new Command(T.OK, action), new Command(T.close, -1, MapScr.instance));
		isNewVersion = true;
	}

	public void onAdminCommandResponse(string responseText)
	{
	}

	public void onSetMoneyError(string error, bool boo)
	{
		Out.println("onSetMoneyError: " + boo);
		if (boo)
		{
			Canvas.startOK(error, new IActionDisconnect());
		}
		else
		{
			Canvas.startOKDlg(error);
		}
	}

	public void onTransferMoney(int newMoney, string info)
	{
		GameMidlet.avatar.setMoney(newMoney);
		if (Canvas.currentDialog == null)
		{
			Canvas.startOKDlg(info);
		}
	}

	public void onMoneyInfo(MyVector mni)
	{
		MoneyScr.gI().setAvatarList(mni);
		MoneyScr.gI().switchToMe(Canvas.currentMyScreen);
		Canvas.endDlg();
	}

	public void doGetHandler(sbyte index)
	{
		if (GameMidlet.CLIENT_TYPE == 9)
		{
			isNewVersion = false;
		}
		if (GlobalMessageHandler.gI().miniGameMessageHandler != null)
		{
			switch (index)
			{
			case 3:
				CasinoMsgHandler.onHandler();
				MapScr.gI().onJoinCasino();
				break;
			case 8:
				AvatarMsgHandler.onHandler();
				if (MapScr.idMapOffline != -1)
				{
					GlobalService.gI().doJoinOfflineMap(MapScr.idMapOffline);
					MapScr.idMapOffline = -1;
				}
				else if (MapScr.typeJoin != -1)
				{
					Canvas.loadMap.load(57 + MapScr.typeJoin, true);
					if (Canvas.isInitChar && LoadMap.TYPEMAP == 57)
					{
						Canvas.welcome = new Welcome();
						Canvas.welcome.initShop(MapScr.instance);
					}
					GameMidlet.avatar.setFeel(4);
					Canvas.endDlg();
				}
				else
				{
					MapScr.gI().joinCitymap();
					Canvas.endDlg();
				}
				break;
			case 9:
				ParkMsgHandler.onHandler();
				if (LoadMap.xDichChuyen == -1)
				{
					if (!onMainMenu.isOngame)
					{
						if (GameMidlet.CLIENT_TYPE == 12)
						{
							LoadMap.w = 24;
							Canvas.load = 0;
							LoadMap.rememMap = -1;
							ParkService.gI().doJoinPark(MapScr.indexMap, -1);
						}
						else if (GameMidlet.CLIENT_TYPE == 3)
						{
							ParkService.gI().doJoinPark(MapScr.roomID, -1);
						}
						else if (MapScr.typeJoin != -1)
						{
							MapScr.gI().doSetHandlerSuccess();
						}
						else if (MapScr.idMapOld != -1)
						{
							Canvas.startWaitDlg();
							ParkService.gI().doJoinPark(MapScr.idMapOld, -1);
							MapScr.idMapOld = -1;
						}
						else
						{
							MapScr.gI().doJoin();
						}
					}
					else
					{
						onMainMenu.gI().switchToMe();
						Canvas.endDlg();
					}
				}
				else
				{
					LoadMap.idTileImg = -1;
				}
				break;
			case 10:
				FarmMsgHandler.onHandler();
				if (FarmData.playing == -1)
				{
					FarmService.gI().setBigData();
					break;
				}
				if (FarmScr.itemProduct == null)
				{
					FarmService.gI().getInventory();
					break;
				}
				ParkService.gI().doJoinPark(25, 0);
				FarmScr.init();
				FarmScr.gI().doJoinFarm(GameMidlet.avatar.IDDB, false);
				break;
			case 11:
				HomeMsgHandler.onHandler();
				LoadMap.TYPEMAP = -1;
				ParkService.gI().doJoinPark(21, 0);
				if (MapScr.idHouse != -1)
				{
					Canvas.startWaitDlg();
					AvatarService.gI().getTypeHouse(0);
				}
				break;
			case 12:
				RaceMsgHandler.onHandler();
				GlobalService.gI().doJoinRoomRace();
				break;
			}
		}
		GameMidlet.CLIENT_TYPE = index;
	}

	public void updateMoney(int moneyUpdate, int typeMoney)
	{
		if (moneyUpdate != 0)
		{
			Canvas.addFlyText(moneyUpdate, GameMidlet.avatar.x, GameMidlet.avatar.y, -1, -1);
		}
		switch (typeMoney)
		{
		case 1:
			if (onMainMenu.isOngame)
			{
				GameMidlet.avatar.setMoneyNew(GameMidlet.avatar.getMoneyNew() + moneyUpdate);
			}
			else
			{
				GameMidlet.avatar.setMoney(GameMidlet.avatar.getMoney() + moneyUpdate);
			}
			break;
		case 2:
			GameMidlet.avatar.money[2] += moneyUpdate;
			break;
		}
	}

	public void onUpdateContainer(sbyte index, string str)
	{
		if (index == 0)
		{
			Canvas.startOKDlg(str, new IActionUpdateContainer());
		}
		else
		{
			Canvas.startOKDlg(str);
		}
	}

	public void onSms(string info1, string syst, string number1)
	{
		Canvas.startOKDlg(info1, new IActionOnSMS(syst, number1));
	}

	public void onMenuOption(int userID, sbyte IDMenu, string[] listStr, short[] idImg, string nameNPC, string textChat, bool[] isMenu)
	{
		if (Canvas.menuMain != null)
		{
			Canvas.menuMain = null;
		}
		Canvas.endDlg();
		MyVector myVector = new MyVector();
		for (int i = 0; i < listStr.Length; i++)
		{
			int ii = i;
			myVector.addElement(new Command(listStr[i], new IActionMenuOption(ii, userID, IDMenu)));
		}
		if (nameNPC != null)
		{
			MenuNPC.gI().setInfo(myVector, userID, nameNPC, textChat, isMenu);
		}
		else
		{
			MenuCenter.gI().startAt(myVector);
		}
	}

	public void onTextBox(int userID, sbyte idMenu, string nameText, int typeInput)
	{
		Canvas.inputDlg.setInfoIA(nameText, new IActionTextBox(userID, idMenu), typeInput, Canvas.currentMyScreen);
	}

	public void onUpdateCHest(sbyte type, sbyte index2, string str2)
	{
		if (index2 == 0)
		{
			Canvas.startOKDlg(str2, new IActionChest());
		}
		else
		{
			Canvas.startOKDlg(str2);
		}
	}
}
