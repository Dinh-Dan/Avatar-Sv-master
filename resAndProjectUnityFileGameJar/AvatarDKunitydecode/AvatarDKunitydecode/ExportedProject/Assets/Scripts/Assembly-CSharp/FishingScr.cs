using System;

public class FishingScr : MyScreen
{
	private class IActionNothing : IAction
	{
		public void perform()
		{
		}
	}

	private class CommandInfo : Command
	{
		private readonly Avatar ava1;

		private readonly sbyte perLv;

		private readonly short idPart;

		private readonly int numFish;

		private readonly sbyte lv;

		public CommandInfo(string s, IActionNothing nothing, Avatar ava1, sbyte perLv, short idPart, int numFish, sbyte lv)
			: base(s, nothing)
		{
			this.ava1 = ava1;
			this.perLv = perLv;
			this.idPart = idPart;
			this.numFish = numFish;
			this.lv = lv;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			Canvas.resetTrans(g);
			int num = PaintPopup.hTab + AvMain.hDuBox * 2 + 10 * AvMain.hd + 30 * (AvMain.hd - 1) + PopupShop.y;
			int height = Canvas.normalFont.getHeight();
			ava1.paintIcon(g, Canvas.w / 2, num, false);
			Canvas.normalFont.drawString(g, T.nameStr + ava1.name, Canvas.w / 2, num + height, 2);
			Canvas.normalFont.drawString(g, T.level[3] + lv + " (" + perLv + "%)", Canvas.w / 2, num + height * 2, 2);
			Canvas.normalFont.drawString(g, T.numberFish + numFish, Canvas.w / 2, num + height * 3, 2);
			Canvas.normalFont.drawString(g, T.achieve + ": ", Canvas.w / 2, num + height * 4, 2);
			if (idPart != -1)
			{
				PartSmall partSmall = (PartSmall)AvatarData.getPart(idPart);
				partSmall.paint(g, Canvas.w / 2, num + height * 6, 3);
			}
		}
	}

	public static FishingScr me;

	private Command cmdQuanCau;

	private Command cmdClose;

	private Command cmdXong;

	public static Image imgPhao;

	public static FrameImage imgCa;

	public Fish fish = new Fish();

	public int finish;

	public int xTime;

	public int yTime;

	public int xTemp;

	private int wTime;

	public bool isSuccess;

	public bool isAble;

	private Image[] imgArrow;

	private int index;

	private sbyte[] arrIndex;

	private long cTime;

	private short timeDelay;

	private int iCancau;

	private int xKeyArr;

	private int yKeyArr;

	public FishingScr()
	{
		cmdQuanCau = new Command(T.toss, 0, this);
		cmdXong = new Command(T.finish, 1, this);
		cmdClose = new Command(T.close, 2, this);
		center = cmdQuanCau;
		wTime = 530;
	}

	public static FishingScr gI()
	{
		return (me != null) ? me : (me = new FishingScr());
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			if (GameMidlet.avatar.action != 2 && GameMidlet.avatar.action != 13)
			{
				MapScr.gI().switchToMe();
			}
			ParkService.gI().doQuanCau();
			Canvas.startWaitDlg();
			center = null;
			break;
		case 1:
			ParkService.gI().doCauCaXong();
			Canvas.startWaitDlg();
			break;
		case 2:
			doClose();
			ParkService.gI().doCauCaXong();
			break;
		case 4:
			doClose();
			break;
		case 3:
			break;
		}
	}

	protected void doClose()
	{
		GameMidlet.avatar.resetTypeChair();
		if (GameMidlet.avatar.direct == Base.RIGHT)
		{
			GameMidlet.avatar.x -= 18;
		}
		else
		{
			GameMidlet.avatar.x += 18;
		}
		GameMidlet.avatar.y -= 10;
		AvCamera.setDistance(Canvas.w / 10);
		MapScr.listFish.removeElement(fish);
		MapScr.gI().switchToMe();
	}

	protected void doQuanCau(Avatar ava)
	{
		Fish fish = getFish(ava.IDDB);
		if (fish != null)
		{
			MapScr.listFish.removeElement(fish);
		}
		Fish fish2 = new Fish();
		if (ava.IDDB == GameMidlet.avatar.IDDB)
		{
			Canvas.endDlg();
			this.fish = fish2;
			finish = wTime / 2;
		}
		else
		{
			fish2 = new Fish();
		}
		MapScr.listFish.addElement(fish2);
		if (ava.action != 2)
		{
			if (ava.IDDB != GameMidlet.avatar.IDDB)
			{
				fish2.ava = ava;
				fish2.isWait = true;
			}
		}
		else
		{
			fish2.doQuanCau(ava);
		}
	}

	public bool doSat(int x, int y)
	{
		yKeyArr = Canvas.h / 2;
		if (yKeyArr > Canvas.h - 70 * AvMain.hd)
		{
			yKeyArr = Canvas.h - 70 * AvMain.hd;
		}
		xKeyArr = 60;
		if (xKeyArr < (Canvas.w - LoadMap.wMap * 24) / 2 + 50 * AvMain.hd)
		{
			xKeyArr = (Canvas.w - LoadMap.wMap * 24) / 2 + 50 * AvMain.hd;
		}
		index = 0;
		int num = LoadMap.getposMap(x, y);
		if (LoadMap.map[num + 1] == 100 || LoadMap.map[num + 1] == 16 || LoadMap.map[num + 1] == 13)
		{
			GameMidlet.avatar.direct = Base.RIGHT;
			xKeyArr = Canvas.w - xKeyArr;
		}
		else
		{
			GameMidlet.avatar.direct = Base.LEFT;
		}
		GameMidlet.avatar.setLayPLayer(x, y);
		isAble = false;
		ParkService.gI().doStartFishing();
		Canvas.startWaitDlg();
		right = cmdClose;
		Canvas.clearKeyHold();
		return true;
	}

	public override void update()
	{
		MapScr.gI().update();
		if (!fish.isCanCau || fish.isSuccess)
		{
			return;
		}
		if (index < arrIndex.Length && Environment.TickCount - cTime > timeDelay)
		{
			setIndex(0);
		}
		if (GameMidlet.avatar.action == 2)
		{
			iCancau--;
			if (iCancau < 0)
			{
				iCancau = 0;
				fish.setPosDay(1);
			}
		}
	}

	public override void keyPress(int keyCode)
	{
		if (fish.isCanCau && !fish.isSuccess)
		{
			switch (keyCode)
			{
			case 50:
			case 52:
			case 54:
			case 56:
				Canvas.keyPressed[keyCode - 48] = true;
				break;
			case 51:
			case 53:
			case 55:
				break;
			}
		}
		else
		{
			MapScr.gI().keyPress(keyCode);
		}
	}

	public override void updateKey()
	{
		if (!fish.isCanCau || !fish.isSuccess)
		{
		}
		int num = Canvas.paint.updateKeyArr(xKeyArr, yKeyArr);
		if (num != -1 && fish.isCanCau && !fish.isSuccess)
		{
			Canvas.isPointerClick = false;
			setIndex(num);
		}
		base.updateKey();
	}

	private void setIndex(int key)
	{
		cTime = Environment.TickCount;
		if (index < arrIndex.Length)
		{
			Canvas.test2 = Canvas.test2 + key + ", ";
			arrIndex[index] = (sbyte)key;
		}
		index++;
		if (GameMidlet.avatar.action != 2)
		{
			fish.setPosDay(0);
			iCancau = 2;
		}
		if (index >= arrIndex.Length)
		{
			fish.setPosDay(0);
			fish.isSuccess = true;
			ParkService.gI().doFinishFishing(true, arrIndex);
			Canvas.startWaitDlg();
		}
	}

	public override void paint(MyGraphics g)
	{
		MapScr.gI().paintMain(g);
		if (fish.isCanCau && !fish.isSuccess)
		{
			paintTime(g);
		}
		Canvas.paint.paintKeyArrow(g, xKeyArr, yKeyArr);
		base.paint(g);
	}

	private void paintTime(MyGraphics g)
	{
		Canvas.resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		g.setColor(8575990);
		if (imgArrow != null && index < imgArrow.Length)
		{
			if (Environment.TickCount - cTime > 50)
			{
				g.setColor(1423411);
			}
			else
			{
				g.setColor(15612731);
			}
			g.drawImage(imgArrow[index], xTime, yTime * AvMain.hd, 0);
		}
	}

	public void onQuanCau(int idUser)
	{
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar != null)
		{
			doQuanCau(avatar);
		}
	}

	public void onCaCanCau(int idUser3, int idFish, short timeDelay, sbyte[][] arrImg)
	{
		Fish fish = getFish(idUser3);
		if (fish == null || fish.isQuan == 0 || (fish.ava.action != 13 && fish.ava.action != 2) || fish.isCanCau)
		{
			return;
		}
		fish.isCanCau = true;
		fish.setPosDay(0);
		fish.ava.action = 2;
		fish.idFish = idFish;
		Canvas.addFlyTextSmall(T.bite, fish.ava.x, fish.ava.y - 60, -1, 1, -1);
		SoundManager.playSound(0);
		if (idUser3 == GameMidlet.avatar.IDDB)
		{
			cTime = Environment.TickCount;
			index = 0;
			iCancau = 2;
			imgArrow = new Image[arrImg.Length];
			arrIndex = new sbyte[arrImg.Length];
			for (int i = 0; i < imgArrow.Length; i++)
			{
				imgArrow[i] = Image.createImage(ArrayCast.cast(arrImg[i]));
			}
			this.timeDelay = timeDelay;
			xTemp = this.fish.posDay[this.fish.size - 2].x + wTime / 20;
			xTime = this.fish.posTemp[this.fish.size - 2].x;
			yTime = this.fish.posTemp[this.fish.size - 2].y - 30;
		}
	}

	public void onFinish(int idUser4, int idFish)
	{
		Fish fish = getFish(idUser4);
		if (fish == null)
		{
			return;
		}
		if (fish.ava.action != 2 && fish.ava.action != 13)
		{
			MapScr.listFish.removeElement(fish);
			return;
		}
		if (idFish < 0)
		{
			Canvas.addFlyTextSmall(T.miss, fish.ava.x, fish.ava.y - 60, -1, 1, -1);
		}
		else
		{
			SoundManager.playSound(1);
		}
		fish.idFish = idFish;
		fish.isSuccess = true;
		fish.setPosDay(0);
		if (fish.ava.IDDB == GameMidlet.avatar.IDDB)
		{
			right = cmdXong;
			Canvas.endDlg();
		}
	}

	public static Fish getFish(int id)
	{
		for (int i = 0; i < MapScr.listFish.size(); i++)
		{
			Fish fish = (Fish)MapScr.listFish.elementAt(i);
			if (fish.ava.IDDB == id)
			{
				return fish;
			}
		}
		return null;
	}

	public void onCauCaXong(int idUser5)
	{
		Fish fish = getFish(idUser5);
		if (idUser5 == GameMidlet.avatar.IDDB)
		{
			finish = wTime / 2;
			center = cmdQuanCau;
			right = cmdClose;
			Canvas.endDlg();
		}
		if (fish == null)
		{
			return;
		}
		if (fish.idFish > 0)
		{
			PartSmall partSmall = (PartSmall)AvatarData.getPart((short)fish.idFish);
			if (partSmall != null)
			{
				ImageInfo imageInfo = AvatarData.listImgInfo[partSmall.idIcon];
				Image img = Image.createImage(AvatarData.getBigImgInfo(imageInfo.bigID).img, imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd, 0);
				Canvas.addFlyText(1, fish.ava.x, fish.ava.y + fish.ava.ySat - 50, -1, img, -1);
				img = null;
			}
		}
		MapScr.listFish.removeElement(fish);
	}

	public void onStartFishing(bool iss, string error)
	{
		if (iss)
		{
			fish.doSetDayCau();
			center = cmdQuanCau;
			switchToMe();
			AvCamera.setDistance(Canvas.w / 3);
			Canvas.endDlg();
		}
		else
		{
			Canvas.msgdlg.setInfoC(error, new Command(T.OK, 4, this));
		}
	}

	public void onStatus(int idUser6, int status)
	{
		Avatar avatar = LoadMap.getAvatar(idUser6);
		if (avatar != null && (avatar.action == 2 || avatar.action == 13))
		{
			Fish fish = new Fish();
			MapScr.listFish.addElement(fish);
			fish.doQuanCau(avatar);
			fish.doQuanDay();
			fish.posDay[fish.size - 1].x = avatar.x + 70 + (AvMain.hd - 1) * 35 + CRes.rnd(25);
			fish.posDay[fish.size - 1].y = avatar.y;
			fish.isQuan = 1;
			fish.countQuan = -1;
			fish.setPosDay(1);
			switch (status)
			{
			case 2:
				fish.isCanCau = true;
				break;
			case 3:
				fish.isCanCau = true;
				fish.isSuccess = true;
				fish.distant = 2;
				break;
			}
		}
	}

	public void onInfo(int idUser7, sbyte lv, sbyte perLv, int numFish, short idPart)
	{
		Avatar avatar = LoadMap.getAvatar(idUser7);
		if (avatar == null && ListScr.tempList != null)
		{
			for (int i = 0; i < ListScr.tempList.size(); i++)
			{
				Avatar avatar2 = (Avatar)ListScr.tempList.elementAt(i);
				if (avatar2.IDDB == idUser7)
				{
					avatar = avatar2;
				}
			}
		}
		Avatar avatar3 = avatar;
		if (avatar3 != null)
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new CommandInfo(string.Empty, new IActionNothing(), avatar3, perLv, idPart, numFish, lv));
			PopupShop.gI().addElement(new string[1] { T.info }, new MyVector[1], myVector, null);
			PopupShop.gI().switchToMe();
		}
		Canvas.endDlg();
	}
}
