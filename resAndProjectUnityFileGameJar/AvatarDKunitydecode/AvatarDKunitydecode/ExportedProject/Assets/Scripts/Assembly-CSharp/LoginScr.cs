using System;
using UnityEngine;

public class LoginScr : MyScreen
{
	private class IActionOkUser : IAction
	{
		public void perform()
		{
		}
	}

	private class actDoSettingPassword : IKbAction
	{
		private LoginScr bscr;

		public actDoSettingPassword(LoginScr b)
		{
			bscr = b;
		}

		public void perform(string text)
		{
			string text2 = Canvas.inputDlg.getText();
			if (!text.Equals(string.Empty))
			{
				bscr.doForgetPass(text);
			}
		}
	}

	private class IActionForgetPass : IAction
	{
		private string user;

		public IActionForgetPass(string user)
		{
			this.user = user;
		}

		public void perform()
		{
			if (!Session_ME.connected)
			{
				Canvas.startWaitDlg(T.connecting);
				Canvas.connect();
			}
			else
			{
				Canvas.startWaitDlg();
			}
			GlobalService.gI().requestService(4, user);
		}
	}

	private class IActionEferral : IKbAction
	{
		public void perform(string text)
		{
			gI().referral = text;
			if (GameMidlet.IP == null)
			{
				isLoadIP = true;
				ServerListScr.gI().doUpdateServer();
			}
			else
			{
				gI().regRequest();
			}
		}
	}

	private class IAcionNewGameOk : IAction
	{
		public void perform()
		{
			ServerListScr.gI().switchToMe();
		}
	}

	private class IActionChangeAcc : IAction
	{
		public void perform()
		{
			isNewGame = false;
			gI().right = new Command(T.back, 105, gI().xLogin + gI().wLogin - MyScreen.wTab / 2 - 30 * AvMain.hd, gI().yLogin + gI().hLogin - PaintPopup.hButtonSmall / 2);
			me.center = me.cmdLogin;
		}
	}

	public static LoginScr me;

	public TField tfUser;

	public TField tfPass;

	public TField tfReg;

	public TField tfEmail;

	private int focus;

	private int yL;

	private int defYL;

	private Command cmdRemem;

	private Command cmdLogin;

	private Command cmdReg;

	private Command cmdRegister;

	private Command cmdBack;

	private Command cmdSelected;

	private bool isCheckBox = true;

	private Command cmdMenu;

	public bool isReg;

	public string numSupport = "19006610";

	public string passRemem = string.Empty;

	public short[] listIDPart;

	public int xLogin;

	public int yLogin;

	public int wLogin;

	public int hLogin;

	public int xCheck;

	public int yCheck;

	public int wC;

	public int xC;

	public long timeOut;

	public static int aa;

	public static bool isSelectedLanguage;

	public static bool isNewGame;

	public static bool isAccVir;

	private string[] listStrNew = new string[3] { "Chơi mới", "Chơi tiêp", "Đổi tài khoản" };

	public int hCellNew;

	public int yNew;

	private sbyte indexNewGame;

	private string nameVir = string.Empty;

	private string passVir = string.Empty;

	private int h0;

	public static bool isLoadIP;

	public string referral;

	public string email;

	private int indexLQ;

	private bool isCheck;

	private bool isClickNew;

	public LoginScr()
	{
		tfUser = new TField(string.Empty, this, new IActionOkUser());
		tfUser.setFocus(true);
		tfUser.showSubTextField = false;
		tfUser.autoScaleScreen = true;
		tfUser.setIputType(ipKeyboard.TEXT);
		tfUser.setMaxTextLenght(20);
		tfPass = new TField(string.Empty, this, new IActionOkUser());
		tfReg = new TField(string.Empty, this, new IActionOkUser());
		tfEmail = new TField(string.Empty, this, new IActionOkUser());
		tfEmail.sDefaust = "Tùy chọn";
		tfEmail.setMaxTextLenght(20);
		tfPass.isUser = true;
		tfReg.isUser = true;
		tfUser.isUser = true;
		tfEmail.isUser = true;
		tfPass.showSubTextField = false;
		tfReg.showSubTextField = false;
		tfEmail.showSubTextField = false;
		tfPass.autoScaleScreen = true;
		tfUser.autoScaleScreen = true;
		tfReg.autoScaleScreen = true;
		tfEmail.autoScaleScreen = true;
		init(Canvas.hCan);
		tfPass.setIputType(ipKeyboard.PASS);
		tfReg.setIputType(ipKeyboard.PASS);
		tfEmail.setIputType(ipKeyboard.TEXT);
		focus = 0;
	}

	public static LoginScr gI()
	{
		if (me == null)
		{
			me = new LoginScr();
		}
		return me;
	}

	public override void close()
	{
		Canvas.startOKDlg(T.doYouWantExit2, 54);
	}

	public override void switchToMe()
	{
		gI().load();
		initCmd();
		Canvas.paint.loadImgAvatar();
		isLoadIP = false;
		GameMidlet.avatar = new Avatar();
		init(Canvas.hCan);
		base.switchToMe();
		indexNewGame = -1;
		isNewGame = true;
		if (nameVir.Equals(string.Empty) && tfUser.getText().Equals(string.Empty))
		{
			listStrNew = new string[2] { "Chơi mới", "Đổi tài khoản" };
		}
		else
		{
			listStrNew = new string[3]
			{
				"Chơi tiêp" + (tfUser.getText().Equals(string.Empty) ? string.Empty : (", " + tfUser.getText())),
				"Chơi mới",
				"Đổi tài khoản"
			};
		}
	}

	public void setTF()
	{
		tfPass.setFocus(false);
		tfReg.setFocus(false);
		tfUser.setFocus(true);
		if (isSelectedLanguage)
		{
		}
		tfPass.setFocus(false);
		tfReg.setFocus(false);
		tfUser.setFocus(true);
	}

	public void load()
	{
		onMainMenu.iChangeGame = 0;
		onMainMenu.isOngame = false;
		LoadMap.idTileImg = -1;
		timeOut = Canvas.getTick();
		resetLogo();
		loadLogin();
		if (LoadMap.TYPEMAP != 25)
		{
			Canvas.loadMap.load(26, true);
		}
		GameMidlet.avatar.x = (GameMidlet.avatar.xCur = LoadMap.wMap * 24 / 2 + 30);
		AvCamera.gI().xCam = (AvCamera.gI().xTo = 100f);
		focus = 0;
	}

	public void initImg()
	{
		if (GameMidlet.PROVIDER == 6)
		{
			MyScreen.imgLogo = Image.createImagePNG(T.getPath() + "/lgyeah");
		}
		else
		{
			MyScreen.imgLogo = Image.createImagePNG(T.getPath() + "/l");
		}
	}

	public void initCmd()
	{
		cmdMenu = new Command(T.menu, 0);
		cmdRegister = new Command(T.register, 4, xLogin + wLogin - MyScreen.wTab / 2 - 30 * AvMain.hd, yLogin + hLogin - AvMain.hCmd / 2);
		cmdReg = new Command(T.register, 3, xLogin + MyScreen.wTab / 2 + 30 * AvMain.hd, yLogin + hLogin - AvMain.hCmd / 2);
		cmdLogin = new Command(T.selectt, 1, xLogin + MyScreen.wTab / 2 + 30 * AvMain.hd, yLogin + hLogin - AvMain.hCmd / 2);
		cmdRemem = new Command(T.remem, 2);
		cmdBack = new Command(T.back, 5, xLogin + wLogin - MyScreen.wTab / 2 - 30 * AvMain.hd, yLogin + hLogin - AvMain.hCmd / 2);
		cmdSelected = new Command(T.selectt, 104);
	}

	public void reset()
	{
	}

	public void init(int bh)
	{
		h0 = bh;
		defYL = bh / 2 - 80;
		resetLogo();
		wC = Canvas.w - 30;
		if (wC < 70)
		{
			wC = 70;
		}
		if (wC > 99)
		{
			wC = 99;
		}
		xC = (Canvas.w - wC >> 1) + 29;
		if (Canvas.w <= 128)
		{
			wC = 80;
			xC = (Canvas.w - wC >> 1) + 20;
		}
		xC -= (AvMain.hd - 1) * 40;
		Canvas.paint.loadImgAvatar();
		Canvas.paint.initPosLogin(this, bh);
		initCmd();
		defYL = yLogin / 2;
		yL = defYL;
		AvCamera.gI().followPlayer = GameMidlet.avatar;
		AvCamera.gI().update();
		if (TouchScreenKeyboard.visible)
		{
			defYL = yLogin - 100;
		}
	}

	public override void commandActionPointer(int index, int subIndex)
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
			ipKeyboard.openKeyBoard(T.nameAcc, ipKeyboard.TEXT, string.Empty, new actDoSettingPassword(this), false);
			break;
		case 5:
			OptionScr.gI().switchToMe();
			break;
		case 6:
			GameMidlet.flatForm("http://wap.teamobi.com/faqs.php?provider=" + GameMidlet.PROVIDER);
			break;
		case 7:
			GameMidlet.flatForm("http://wap.teamobi.com?info=checkupdate&game=8&version=2.5.8&provider=" + GameMidlet.PROVIDER + "&agent=" + GameMidlet.AGENT);
			break;
		case 9:
			Canvas.startOKDlg(T.alreadyDelRMS + T.delRMS);
			AvatarData.delRMS();
			break;
		}
	}

	public override void doSetting()
	{
		if (TouchScreenKeyboard.visible)
		{
			Canvas.isPointerRelease = false;
			ipKeyboard.tk = null;
		}
		OptionScr.gI().switchToMe();
	}

	public override void doMenu()
	{
		if (TouchScreenKeyboard.visible)
		{
			Canvas.isPointerRelease = false;
			Canvas.isKeyBoard = false;
			ipKeyboard.tk.active = false;
		}
		MyVector myVector = new MyVector();
		Command command = new Command(T.exit, 2, this);
		myVector.addElement(new Command(T.support, 8, this));
		myVector.addElement(new Command(T.fogetPass, 4, this));
		myVector.addElement(new Command(T.option, 5, this));
		if (OptionScr.gI().mapFocus[4] == 0)
		{
			myVector.addElement(new Command(T.FAQs, 6, this));
		}
		myVector.addElement(new Command(T.updateGame, 7, this));
		if (OptionScr.gI().mapFocus[4] == 0)
		{
			myVector.addElement(new Command(T.delRMS, 9, this));
		}
		MenuCenter.gI().startAt(myVector);
	}

	public void doForgetPass(string user)
	{
		IAction action = new IActionForgetPass(user);
		action.perform();
	}

	protected void doRememberPass()
	{
		if (!isCheckBox)
		{
			isCheckBox = true;
			cmdRemem.caption = T.removee;
		}
		else
		{
			isCheckBox = false;
			cmdRemem.caption = T.remem;
		}
		Out.println("remember: " + isCheckBox);
	}

	protected void doRegister()
	{
		isReg = true;
		Canvas.paint.initPosLogin(this, h0);
	}

	protected void doReg()
	{
		if (tfUser.getText().Equals(string.Empty))
		{
			Canvas.startOKDlg(T.nameReg[0]);
			tfUser.setFocus(true);
			return;
		}
		if (tfPass.getText().Equals(string.Empty))
		{
			Canvas.startOKDlg(T.nameReg[1]);
			tfPass.setFocus(true);
			return;
		}
		if (tfReg.getText().Equals(string.Empty))
		{
			Canvas.startOKDlg(T.nameReg[2]);
			tfReg.setFocus(true);
			return;
		}
		if (!tfPass.getText().Equals(tfReg.getText()))
		{
			Canvas.startOKDlg(T.nameReg[3]);
			return;
		}
		Canvas.endDlg();
		timeOut = Canvas.getTick();
		if (!tfEmail.getText().Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn nên điền chính xác số di động hoặc email. Khi quên mật khẩu, bạn sẽ dùng nó để lấy lại. Bạn có chắc chắn đã điền số di động / email đúng chưa?", 102);
		}
		else
		{
			doSendRegisterInfo();
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 1:
			isNewGame = true;
			center = (right = null);
			indexNewGame = 0;
			listStrNew = new string[3]
			{
				"Chơi tiêp" + (tfUser.getText().Equals(string.Empty) ? string.Empty : (", " + tfUser.getText())),
				"Chơi mới",
				"Đổi tài khoản"
			};
			break;
		case 2:
			doRememberPass();
			break;
		case 3:
			doReg();
			break;
		case 4:
			doRegister();
			break;
		case 5:
			isReg = false;
			center = cmdLogin;
			Canvas.paint.initPosLogin(this, h0);
			break;
		case 50:
			Canvas.startOKDlg(T.youUseNumRegGetpass);
			break;
		case 51:
			regRequest();
			break;
		case 53:
			GameMidlet.flatForm("http://teamobi.com/dieukhoan.htm");
			break;
		case 54:
			saveLogin();
			break;
		case 55:
			isSelectedLanguage = false;
			saveLogin();
			AvatarData.delErrorRms("avatarSV");
			Application.Quit();
			break;
		case 100:
		{
			string text = Canvas.inputDlg.getText();
			if (!text.Equals(string.Empty))
			{
				doForgetPass(text);
			}
			break;
		}
		case 102:
			doSendRegisterInfo();
			break;
		case 104:
			clickNewGame();
			break;
		case 105:
			center = (right = null);
			isNewGame = true;
			break;
		}
	}

	private void doSelectLQ()
	{
		isSelectedLanguage = true;
		Canvas.paint.initString(indexLQ);
		OptionScr.gI().mapFocus[4] = indexLQ;
		OptionScr.gI().save(0);
		initImg();
		resetLogo();
		initCmd();
		SplashScr.imgLogo = null;
	}

	public void regRequest()
	{
		Canvas.startWaitDlg();
		Canvas.connect();
		GlobalService.gI().doRegisterByEmail(tfUser.getText().ToLower(), tfPass.getText().ToLower(), tfEmail.getText());
		passRemem = tfPass.getText();
		isReg = false;
		center = cmdLogin;
		Canvas.paint.initPosLogin(this, h0);
	}

	protected void doSendRegisterInfo()
	{
		Canvas.msgdlg.setInfoLCR(T.areYouAgreeRule, new Command(T.agree, 51), new Command(T.no, 52), new Command(T.viewRule, 53));
	}

	protected void doLogin()
	{
		string text = tfUser.getText().ToLower().Trim();
		string text2 = tfPass.getText();
		if (text.ToLower().Equals("showimei"))
		{
			Canvas.startOKDlg(SystemInfo.deviceUniqueIdentifier + string.Empty);
		}
		if (!text.Equals(string.Empty) && !text2.Equals(string.Empty))
		{
			if (GameMidlet.IP == null)
			{
				ServerListScr.gI().doUpdateServer();
			}
			else
			{
				ServerListScr.gI().switchToMe();
			}
		}
	}

	public override void update()
	{
		if (!isNewGame && this == Canvas.currentMyScreen && Canvas.menuMain == null)
		{
			tfUser.update();
			tfPass.update();
			if (isReg)
			{
				tfReg.update();
				tfEmail.update();
			}
		}
		updateLogo();
		if (isReg)
		{
		}
		Canvas.loadMap.update();
	}

	public void updateLogo()
	{
		if (defYL != yL)
		{
			yL += defYL - yL >> 1;
		}
	}

	public void resetLogo()
	{
		yL = -50;
	}

	public override void keyPress(int keyCode)
	{
		if (tfUser.isFocused())
		{
			tfUser.keyPressed(keyCode);
		}
		else if (tfPass.isFocused())
		{
			tfPass.keyPressed(keyCode);
		}
		else if (tfReg.isFocused())
		{
			tfReg.keyPressed(keyCode);
		}
		else if (tfEmail.isFocused())
		{
			tfEmail.keyPressed(keyCode);
		}
		base.keyPress(keyCode);
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		Canvas.resetTrans(g);
		if (isNewGame)
		{
			paintNewGame(g);
		}
		else if (this == Canvas.currentMyScreen && Canvas.currentDialog == null && Canvas.menuMain == null)
		{
			paintLogin(g);
		}
		Canvas.resetTrans(g);
		paintLogo(g);
		base.paint(g);
		Canvas.paintPlus(g);
	}

	private void paintNewGame(MyGraphics g)
	{
		Canvas.paint.paintPopupBack(g, xLogin, yLogin, wLogin, hLogin, -1, false);
		g.translate(xLogin, yLogin + yNew);
		if (indexNewGame != -1)
		{
			g.setColor(16777215);
			g.fillRect(5 * AvMain.hd, indexNewGame * hCellNew, wLogin - 10 * AvMain.hd, hCellNew);
		}
		for (int i = 0; i < listStrNew.Length; i++)
		{
			Canvas.normalFont.drawString(g, listStrNew[i], wLogin / 2, i * hCellNew + hCellNew / 2 - Canvas.normalFont.getHeight() / 2, 2);
		}
	}

	public override void paintMain(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		Canvas.loadMap.paint(g);
		Canvas.loadMap.paintObject(g);
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
	}

	private void paintLogin(MyGraphics g)
	{
		Canvas.paint.paintPopupBack(g, xLogin, yLogin, wLogin, hLogin, -1, false);
		g.setClip(xLogin + 4, yLogin + 4, wLogin - 8, hLogin - 8);
		tfUser.paint(g);
		g.setClip(xLogin + 4, yLogin + 4, wLogin - 8, hLogin - 8);
		int width = Canvas.tempFont.getWidth(T.acc + ":");
		width = ((width >= tfUser.x - xLogin) ? (tfUser.x - width - 5) : ((tfUser.x - xLogin - width) / 2));
		Canvas.tempFont.drawString(g, T.acc, xLogin + width, tfUser.y + tfUser.height / 2 - Canvas.tempFont.getHeight() / 2 - 2 * AvMain.hd, 0);
		Canvas.tempFont.drawString(g, T.pass, xLogin + width, tfPass.y + tfUser.height / 2 - Canvas.tempFont.getHeight() / 2 - 2 * AvMain.hd, 0);
		if (!isReg)
		{
			Canvas.paint.paintCheckBox(g, xCheck, yCheck, focus, isCheckBox);
		}
		else
		{
			Canvas.tempFont.drawString(g, T.enterAgain, xLogin + width, tfReg.y + tfUser.height / 2 - Canvas.tempFont.getHeight() - 2 * AvMain.hd, 0);
			Canvas.tempFont.drawString(g, T.pass, xLogin + width, tfReg.y + tfUser.height / 2 - 2 * AvMain.hd, 0);
			Canvas.tempFont.drawString(g, "Số di động", xLogin + width, tfEmail.y + tfUser.height / 2 - Canvas.tempFont.getHeight() - 2 * AvMain.hd, 0);
			Canvas.tempFont.drawString(g, "hoặc email:", xLogin + width, tfEmail.y + tfUser.height / 2 - 2 * AvMain.hd, 0);
			tfReg.paint(g);
			tfEmail.paint(g);
		}
		tfPass.paint(g);
	}

	public void paintLogo(MyGraphics g)
	{
		if (!TouchScreenKeyboard.visible)
		{
			g.drawImage(MyScreen.imgLogo, Canvas.hw, yL, 3);
		}
	}

	public override void updateKey()
	{
		if (isNewGame)
		{
			updateKeyNewGame();
			return;
		}
		if (Canvas.isPointerClick && Canvas.isPointer(xCheck - 10, yCheck, Canvas.tempFont.getWidth(T.rememPass) + 70, 35 * AvMain.hd))
		{
			isCheck = true;
			Canvas.isPointerClick = false;
		}
		if (isCheck && Canvas.isPointerRelease && Canvas.isPoint(xCheck - 10, yCheck, Canvas.tempFont.getWidth(T.rememPass) + 70, 35 * AvMain.hd))
		{
			Canvas.isPointerRelease = false;
			isCheck = false;
			doRememberPass();
		}
		if (Canvas.isPointerClick && Screen.orientation != ScreenOrientation.Portrait && ipKeyboard.tk != null)
		{
			ipKeyboard.close();
		}
		base.updateKey();
	}

	private void updateKeyNewGame()
	{
		if (Canvas.isKeyPressed(2))
		{
			indexNewGame--;
			if (indexNewGame < 0)
			{
				indexNewGame = (sbyte)(listStrNew.Length - 1);
			}
		}
		else if (Canvas.isKeyPressed(8))
		{
			indexNewGame++;
			if (indexNewGame >= listStrNew.Length)
			{
				indexNewGame = 0;
			}
		}
		if (Canvas.isPointerClick)
		{
			for (int i = 0; i < listStrNew.Length; i++)
			{
				if (Canvas.isPoint(xLogin, yLogin + yNew + i * hCellNew, wLogin, hCellNew))
				{
					indexNewGame = (sbyte)i;
					Canvas.isPointerClick = false;
					isClickNew = true;
					Out.println("aaaaaaaaaaaaaaaa");
					break;
				}
			}
		}
		if (isClickNew)
		{
			if (Canvas.isPointerDown && !Canvas.isPoint(xLogin, yLogin + yNew + indexNewGame * hCellNew, wLogin, hCellNew))
			{
				indexNewGame = -1;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isClickNew = false;
				if (indexNewGame != -1)
				{
					clickNewGame();
				}
			}
		}
		base.updateKey();
	}

	private void clickNewGame()
	{
		Out.println("clickNewGame: " + isAccVir + "    " + indexNewGame);
		switch (indexNewGame)
		{
		case 0:
			if (listStrNew.Length == 2)
			{
				IAction action2 = new IAcionNewGameOk();
				action2.perform();
			}
			else if (isAccVir)
			{
				ServerListScr.gI().switchToMe();
			}
			else
			{
				doLogin();
			}
			break;
		case 1:
		{
			if (listStrNew.Length == 2)
			{
				changeAcc();
				break;
			}
			IAction action = new IAcionNewGameOk();
			if (!nameVir.Equals(string.Empty) && tfUser.getText().Equals(string.Empty))
			{
				Canvas.startOKDlg("Tài khoản của bạn chưa được đăng kí liên kết với một tài khoản Team. Bạn sẽ mất tài khoản đang chơi nếu tiếp tục. Bạn có muốn tiếp tục ?", action);
			}
			else
			{
				action.perform();
			}
			break;
		}
		case 2:
			changeAcc();
			break;
		}
	}

	private void changeAcc()
	{
		IAction action = new IActionChangeAcc();
		if (!nameVir.Equals(string.Empty) && tfUser.getText().Equals(string.Empty))
		{
			Canvas.startOKDlg("Tài khoản của bạn chưa được đăng kí liên kết với một tài khoản Team. Bạn sẽ mất tài khoản đang chơi nếu tiếp tục. Bạn có muốn tiếp tục ?", action);
		}
		else
		{
			action.perform();
		}
	}

	public void saveLogin()
	{
		Out.println("SAVE LOGINaaaaaaaaaaaaaaaaaaaaa");
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeUTF("2.5.8");
			dataOutputStream.writeByte((sbyte)selected);
			dataOutputStream.writeUTF(numSupport);
			dataOutputStream.writeBoolean(isCheckBox);
			dataOutputStream.writeUTF(nameVir);
			dataOutputStream.writeUTF(passVir);
			if (isCheckBox)
			{
				dataOutputStream.writeUTF(gI().tfUser.getText());
				dataOutputStream.writeUTF(gI().tfPass.getText());
			}
			dataOutputStream.writeInt(aa);
			dataOutputStream.writeBoolean(isSelectedLanguage);
			dataOutputStream.writeByte(ServerListScr.selected);
			dataOutputStream.writeBoolean(isAccVir);
			RMS.saveRMS("avlogin", dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void loadLogin()
	{
		DataInputStream dataInputStream = AvatarData.initLoad("avlogin");
		if (dataInputStream == null)
		{
			isSelectedLanguage = true;
			return;
		}
		Out.println("loadLogin");
		string value = string.Empty;
		try
		{
			value = dataInputStream.readUTF();
			selected = dataInputStream.readByte();
			numSupport = dataInputStream.readUTF();
			isCheckBox = dataInputStream.readBoolean();
			nameVir = dataInputStream.readUTF();
			passVir = dataInputStream.readUTF();
			if (isCheckBox)
			{
				tfUser.setText(dataInputStream.readUTF());
				tfPass.setText(dataInputStream.readUTF());
			}
			aa = dataInputStream.readInt();
			isSelectedLanguage = dataInputStream.readBoolean();
			ServerListScr.selected = dataInputStream.readByte();
			isAccVir = dataInputStream.readBoolean();
			dataInputStream.close();
		}
		catch (Exception)
		{
			AvatarData.delErrorRms("avlogin");
		}
		if (!isSelectedLanguage)
		{
			AvatarData.delErrorRms("avatarSV");
		}
		if (!"2.5.8".Equals(value))
		{
			AvatarData.delRMS();
		}
	}

	public void onNumSupport(string numSup)
	{
		numSupport = numSup;
		saveLogin();
	}

	public void login()
	{
		Canvas.connect();
		GlobalService.gI().doRequestNumSupport(gI().numSupport.GetHashCode());
		Out.println("login: " + isNewGame + "    " + indexNewGame);
		if (isNewGame && ((indexNewGame == 0 && listStrNew.Length == 2) || (indexNewGame == 1 && listStrNew.Length == 3)))
		{
			GlobalService.gI().doLoginNewGame();
		}
		else if (tfUser.getText().Equals(string.Empty))
		{
			GlobalService.gI().login(nameVir, passVir, "2.5.8");
		}
		else
		{
			GlobalService.gI().login(tfUser.getText().ToLower(), tfPass.getText(), "2.5.8");
		}
	}

	public void onRegisterByEmail(string name, string pass)
	{
		tfUser.setText(name);
		tfPass.setText(pass);
		Canvas.startOKDlg("Đăng ký thành công.");
	}

	public void onLoginNewGame(string nameNewGame, string passNewGame)
	{
		Out.println("onLoginNewGame: " + nameNewGame + "   " + passNewGame);
		nameVir = nameNewGame;
		passVir = passNewGame;
		tfUser.setText(string.Empty);
		tfPass.setText(string.Empty);
		isAccVir = true;
		isNewGame = false;
		login();
	}
}
