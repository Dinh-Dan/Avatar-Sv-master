using System;
using UnityEngine;

public class HouseScr : MyScreen, IChatable
{
	private class IActionKick : IAction
	{
		private Base ba;

		public IActionKick(Base b)
		{
			ba = b;
		}

		public void perform()
		{
			AvatarService.gI().doKickOutHome(ba.IDDB);
		}
	}

	private class CommandMap : Command
	{
		private int ii;

		public CommandMap(string name, IActionMap action, int i)
			: base(name, action)
		{
			ii = i;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			LoadMap.imgMap.drawFrame(ii, x + 1, y + 1, 0, 3, g);
		}
	}

	private class IActionMap : IAction
	{
		public readonly int ii;

		private HouseScr me;

		public IActionMap(HouseScr me, int i)
		{
			this.me = me;
			ii = i;
		}

		public void perform()
		{
			isBuyTileMap = false;
			me.indexTileMapBuy = -1;
			me.setStatusBuyItem();
			if (me.temp == null)
			{
				me.temp = new short[LoadMap.map.Length];
				for (int i = 0; i < LoadMap.map.Length; i++)
				{
					me.temp[i] = LoadMap.map[i];
				}
			}
			me.right = me.cmdFinish;
			me.left = new Command(T.selectt, 5);
			me.center = null;
			if (me.selected < numW)
			{
				me.indexName = 1;
			}
			else
			{
				me.indexName = 0;
			}
			me.indexTileMapBuy = ii;
			isBuyTileMap = true;
		}
	}

	private class IActionObject : IAction
	{
		private readonly string text;

		private readonly HouseScr me;

		public IActionObject(HouseScr me, string str)
		{
			text = str;
			this.me = me;
		}

		public void perform()
		{
			me.doSelectedItem(text);
		}
	}

	private class IActionNo : IAction
	{
		private readonly HouseScr house;

		public IActionNo(HouseScr me)
		{
			house = me;
		}

		public void perform()
		{
			house.doSelectObject();
		}
	}

	private class CommandItem : Command
	{
		public readonly MapItemType map;

		public readonly string na;

		public readonly string money;

		private int hh;

		public CommandItem(string s, IActionItem item, MapItemType map, string money, string na, int hh)
			: base(s, item)
		{
			this.map = map;
			this.money = money;
			this.na = na;
			this.hh = hh;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, map.imgID, x, y + hh / 2 - AvMain.hBlack - AvMain.hNormal - 5, 33);
			Canvas.blackF.drawString(g, money, x, y + hh / 2 - AvMain.hBlack, 2);
			Canvas.normalFont.drawString(g, na, x, y + hh / 2 - AvMain.hBlack - AvMain.hNormal, 2);
		}
	}

	private class IActionItem : IAction
	{
		public readonly int ii;

		private string na;

		private readonly HouseScr me;

		public IActionItem(HouseScr me, int i, string na)
		{
			this.na = na;
			this.me = me;
			ii = i;
		}

		public void perform()
		{
			me.setStatusBuyItem();
			me.xItemTranBuy = GameMidlet.avatar.x;
			me.yItemTranBuy = GameMidlet.avatar.y;
			isTranItemBuy = true;
			me.indexItemTranBuy = ii;
		}
	}

	private class IActionCenterSet : IAction
	{
		private readonly int ii;

		private readonly string name;

		private readonly HouseScr me;

		public IActionCenterSet(HouseScr me, int i, string na)
		{
			ii = i;
			name = na;
			this.me = me;
		}

		public void perform()
		{
			xTemp = me.x;
			yTemp = me.y;
			me.doBuyItemHouse(ii, name);
		}
	}

	private class IActionBuyItemClose : IAction
	{
		private HouseScr me;

		public IActionBuyItemClose(HouseScr me)
		{
			this.me = me;
		}

		public void perform()
		{
			me.reset();
			Canvas.endDlg();
			me.indexItemTranBuy = -1;
			isTranItemBuy = false;
		}
	}

	private class IActionBuyItem : IAction
	{
		private readonly MapItemType map;

		private readonly int type;

		private HouseScr me;

		private string name;

		public IActionBuyItem(HouseScr me, MapItemType map, int type, string na)
		{
			name = na;
			this.me = me;
			this.map = map;
			this.type = type;
		}

		public void perform()
		{
			MapItem mapItem = new MapItem(type, me.xItemTranBuy, me.yItemTranBuy, 1, map.idType);
			AvatarService.gI().doBuyItemHouse(mapItem);
			me.reset();
			Canvas.endDlg();
			me.indexItemTranBuy = -1;
			isTranItemBuy = false;
			me.doSelectedItem(name);
		}
	}

	private class updateCoffer : IAction
	{
		public void perform()
		{
			GlobalService.gI().doUpdateChest(0);
			Canvas.startWaitDlg();
		}
	}

	private class IActionPass : IKbAction
	{
		public void perform(string text)
		{
			AvatarService.gI().doSetPassMyHouse(text, 0, 0);
			Canvas.endDlg();
		}
	}

	private class IActionFinish : IAction
	{
		private readonly TField[] tf_P;

		public IActionFinish(TField[] t)
		{
			tf_P = t;
		}

		public void perform()
		{
			if (MapScr.setEnterPass(tf_P))
			{
				GlobalService.gI().doChangeChestPass(tf_P[0].getText(), tf_P[1].getText());
				Canvas.startWaitDlg();
				InputFace.gI().close();
			}
		}
	}

	private class IActionCenterOk : IAction
	{
		private readonly MapItem map2;

		private HouseScr me;

		public IActionCenterOk(HouseScr me, MapItem map)
		{
			this.me = me;
			map2 = map;
		}

		public void perform()
		{
			if (!me.isDisable(AvatarData.getMapItemTypeByID(map2.typeID), me.x, me.y))
			{
				AvatarService.gI().doSortItem(me.posSort.anchor, me.posSort.x, me.posSort.y, me.x / 24, me.y / 24, map2.dir);
				isSelectObj = false;
				me.isSelectedItem = -1;
				me.selected = -1;
				isChange = false;
				me.setType(map2);
				if (me.isSetTuong(map2))
				{
					map2.y++;
				}
				LoadMap.orderVector(LoadMap.treeLists);
				me.doOption();
			}
		}
	}

	private class IActionSellItem : IAction
	{
		private MapItem map2;

		public IActionSellItem(MapItem map)
		{
			map2 = map;
		}

		public void perform()
		{
			HomeMsgHandler.onHandler();
			AvatarService.gI().dodelItem(map2);
			Canvas.startWaitDlg();
		}
	}

	private class IActionEnterPass : IKbAction
	{
		public void perform(string text)
		{
			GlobalService.gI().doEnterPass(text, 0);
			Canvas.endDlg();
		}
	}

	private class CommandShop1 : Command
	{
		private int i;

		private string nameItem;

		private string timeLimit;

		private string des;

		private short idPart;

		private short idItem;

		private short idPartGirl;

		private int price;

		public CommandShop1(string caption, IAction action, int i, string nameItem, short idPart, short idItem, string timeLimit, int price, string des, short idPartGirl)
			: base(caption, action)
		{
			this.i = i;
			this.nameItem = nameItem;
			this.idPart = idPart;
			this.idItem = idItem;
			this.timeLimit = timeLimit;
			this.price = price;
			this.des = des;
			this.idPartGirl = idPartGirl;
		}

		public override void update()
		{
			if (!PopupShop.isTransFocus || i != PopupShop.focus)
			{
				return;
			}
			PopupShop.resetIsTrans();
			Part part = null;
			part = ((GameMidlet.avatar.gender != 1) ? AvatarData.getPart(idPartGirl) : AvatarData.getPart(idPart));
			if (part.IDPart != -1)
			{
				if (GameMidlet.avatar.gender == 1)
				{
					MapScr.setAvatarShop(part);
				}
				else
				{
					MapScr.setAvatarShop(part);
				}
			}
			PopupShop.addStr(nameItem);
			if (timeLimit != null)
			{
				PopupShop.addStr(timeLimit);
			}
			if (price >= 0)
			{
				PopupShop.addStr(T.priceStr + Canvas.getMoneys(price) + " Tim");
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			Part part = AvatarData.getPart(idPart);
			part.paintIcon(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 0, 3);
		}
	}

	private class IActionShop1 : IAction
	{
		private short idItem;

		private string des;

		private sbyte typeShop;

		public IActionShop1(sbyte typeShop, short idItem, string des)
		{
			this.typeShop = typeShop;
			this.idItem = idItem;
			this.des = des;
		}

		public void perform()
		{
			Canvas.startOKDlg(des, new IActionDes(typeShop, idItem));
		}
	}

	private class IActionDes : IAction
	{
		private sbyte typeShop;

		private short idItem;

		public IActionDes(sbyte typeShop, short idItem)
		{
			this.typeShop = typeShop;
			this.idItem = idItem;
		}

		public void perform()
		{
			GlobalService.gI().doSendOpenShopHouse(typeShop, idItem);
		}
	}

	public static HouseScr me;

	private int x;

	private int y;

	private new int selected = -1;

	private Command cmdBrick;

	private Command cmdFinish;

	private Command cmdMenu;

	private Command cmdRotate;

	private MyScreen lastScr;

	private static short numW;

	public static bool isSelectObj;

	private MyVector listItem;

	private sbyte typeHome = -1;

	public int indexName = -1;

	public int isSelectedItem = -1;

	public int idHouse;

	public static bool isChange;

	public static bool isDuyChuyen;

	public static bool isTranItemBuy;

	public static bool isBuyTileMap;

	private Tile[] listTile;

	private AvPosition posSort;

	private AvPosition posJoin;

	public BigImgInfo imgTileMap;

	private FrameImage imgBuyItem;

	private static int xTemp = -1;

	private static int yTemp = -1;

	private new int[] color = new int[2] { 1688583, 14744065 };

	public short IDHoa = 69;

	public short IDHo = 68;

	private short[] temp;

	private int indexTileMapBuy = -1;

	private string nameSelectedItem = string.Empty;

	private int indexItemTranBuy = -1;

	private int xItemTranBuy;

	private int yItemTranBuy;

	private bool isTranItem;

	private bool isMoveItem;

	private int indexChangeItem = -1;

	private int xItemOld;

	private int yItemOld;

	private int xTempItem = -1;

	private int yTempItem;

	private int indexFireItem;

	private int indexCloseItem;

	private int indexRotateItem;

	private bool isTrans;

	private bool isMove;

	private int xDu;

	private int yDu;

	private int indexChans;

	private int x0;

	public int xHo;

	public int yHo;

	private MyVector listP_Chest;

	private MyVector listP_Con;

	private int moneyOnChest;

	private sbyte levelChest;

	public HouseScr()
	{
		cmdBrick = new Command(T.sett, 0);
		cmdFinish = new Command(T.finish, 1);
		cmdMenu = new Command(T.menu, 2);
		imgBuyItem = new FrameImage(Image.createImagePNG(T.getPath() + "/temp/buyItem"), 25 * AvMain.hd, 25 * AvMain.hd);
		cmdRotate = new Command(T.rota, 16, this);
	}

	public static HouseScr gI()
	{
		if (me == null)
		{
			me = new HouseScr();
		}
		return me;
	}

	public void switchToMe(MyScreen sre)
	{
		lastScr = sre;
		base.switchToMe();
	}

	public override void initZoom()
	{
		AvCamera.gI().init(70 + typeHome);
	}

	public override void switchToMe()
	{
		base.switchToMe();
		SoundManager.playSoundBG(83);
	}

	private void addPlayer()
	{
		LoadMap.addPlayer(GameMidlet.avatar);
		GameMidlet.avatar.x = posJoin.x;
		GameMidlet.avatar.y = posJoin.y;
		GameMidlet.avatar.action = 0;
		AvCamera.gI().setToPos(posJoin.x * AvMain.hd, posJoin.y * AvMain.hd);
	}

	public override void doMenu()
	{
		doMenuHouse();
	}

	protected void doMenuHouse()
	{
		MyVector myVector = new MyVector();
		if (idHouse == GameMidlet.avatar.IDDB)
		{
			myVector.addElement(new Command(T.container, 6, this));
			myVector.addElement(new Command(T.homeRepait, 7, this));
			int num = 0;
			for (int i = 0; i < LoadMap.playerLists.size(); i++)
			{
				MyObject myObject = (MyObject)LoadMap.playerLists.elementAt(i);
				if (myObject.catagory == 0)
				{
					num++;
				}
			}
			if (num > 1)
			{
				myVector.addElement(new Command(T.kick, 8, this));
			}
			myVector.addElement(new Command(T.setPass, 9, this));
		}
		myVector.addElement(new Command(T.exit, 13, this));
		MenuCenter.gI().startAt(myVector);
	}

	public override void close()
	{
		MapScr.gI().doExit();
	}

	private void doOption()
	{
		right = new Command(T.finish, 9);
		left = new Command(T.sell, 17, this);
		isChange = true;
		x = GameMidlet.avatar.x;
		y = GameMidlet.avatar.y;
		LoadMap.removePlayer(GameMidlet.avatar);
	}

	protected void doBuyItem()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command(T.buyItem, 10, this));
		myVector.addElement(new Command(T.latGach, 11, this));
		if (listItem.size() > 0)
		{
			myVector.addElement(new Command(T.sellItem, 12, this));
		}
		MenuCenter.gI().startAt(myVector);
	}

	private void setStatusBuyItem()
	{
		HomeMsgHandler.onHandler();
		x = GameMidlet.avatar.x;
		y = GameMidlet.avatar.y;
		LoadMap.removePlayer(GameMidlet.avatar);
	}

	protected void doDiChuyen()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command(T.move, 11));
		myVector.addElement(new Command(T.rota, 12));
		myVector.addElement(new Command(T.sell, 13));
		MenuCenter.gI().startAt(myVector);
	}

	protected void doKick()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < LoadMap.playerLists.size(); i++)
		{
			Base @base = (Base)LoadMap.playerLists.elementAt(i);
			if (@base.catagory == 0 && @base.IDDB != GameMidlet.avatar.IDDB)
			{
				myVector.addElement(new Command(@base.name, new IActionKick(@base)));
			}
		}
		MenuCenter.gI().startAt(myVector);
	}

	protected void doCreateMap()
	{
		if (listTile == null)
		{
			HomeMsgHandler.onHandler();
			AvatarService.gI().doGetTileInfo();
			Canvas.startWaitDlg();
		}
	}

	private void doSelectMap()
	{
		isBuyTileMap = false;
		me.indexTileMapBuy = -1;
		MyVector myVector = new MyVector();
		for (int i = 0; i < listTile.Length; i++)
		{
			int i2 = i;
			if (listTile[i].priceXu != -1 || listTile[i].priceLuong != -1)
			{
				myVector.addElement(new CommandMap(listTile[i].name + "(" + Canvas.getPriceMoney(listTile[i].priceXu, listTile[i].priceLuong, true) + ")", new IActionMap(this, i2), i2));
			}
		}
		if (myVector.size() > 0)
		{
			Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - 70 * AvMain.hd - 10, 70 * AvMain.hd, 70 * AvMain.hd);
		}
	}

	private void reset()
	{
		isSelectedItem = -1;
		selected = -1;
		isChange = false;
		isSelectObj = false;
		left = null;
		center = null;
		right = null;
		if (LoadMap.getAvatar(GameMidlet.avatar.IDDB) == null)
		{
			addPlayer();
		}
	}

	private void doSelectObject()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < AvatarData.listMapItemType.size(); i++)
		{
			MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(i);
			if (mapItemType.buy == 0 || ((typeHome == 4 || (mapItemType.buy != 1 && mapItemType.buy != 2)) && typeHome != 4))
			{
				continue;
			}
			int num = mapItemType.name.IndexOf(":");
			if (num == -1)
			{
				continue;
			}
			int num2 = 0;
			string text = mapItemType.name.Substring(0, num);
			for (int j = 0; j < myVector.size(); j++)
			{
				if (((Command)myVector.elementAt(j)).caption.Equals(text))
				{
					num2 = 1;
				}
			}
			if (num2 == 0 || myVector.size() == 0)
			{
				myVector.addElement(new Command(text, new IActionObject(me, text)));
			}
		}
		MenuCenter.gI().startAt(myVector);
	}

	private void doSelectedItem(string name)
	{
		reset();
		nameSelectedItem = name;
		MyVector myVector = new MyVector();
		int num = 90 * AvMain.hd;
		int num2 = 90 * AvMain.hd;
		for (int i = 0; i < AvatarData.listMapItemType.size(); i++)
		{
			MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(i);
			int num3 = mapItemType.name.IndexOf(name);
			if (mapItemType.buy == 0 || num3 == -1 || ((typeHome == 4 || (mapItemType.buy != 1 && mapItemType.buy != 2)) && typeHome != 4))
			{
				continue;
			}
			string text = mapItemType.name.Substring(mapItemType.name.IndexOf(":") + 1);
			string text2 = string.Empty;
			if (mapItemType.priceXu > 0)
			{
				text2 = text2 + Canvas.getMoneys(mapItemType.priceXu) + T.xu;
			}
			if (mapItemType.priceLuong > 0)
			{
				if (mapItemType.priceXu > 0)
				{
					text2 += " - ";
				}
				text2 = text2 + Canvas.getMoneys(mapItemType.priceLuong) + "l";
			}
			int i2 = i;
			int hh = num2;
			myVector.addElement(new CommandItem(string.Empty, new IActionItem(this, i2, text), mapItemType, text, text2, hh));
		}
		if (myVector.size() > 0)
		{
			Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - num2 - 10, 120 * AvMain.hd, num2);
			Menu.gI().iNo = new IActionNo(this);
		}
	}

	private bool isDisable(MapItemType map, int x0, int y0)
	{
		x0 += 12;
		y0 += 12;
		if (map.buy != 2 && map.buy != 4)
		{
			if (LoadMap.type[y0 / 24 * LoadMap.wMap + x0 / 24] != 80)
			{
				Canvas.startOKDlg(T.noPlaceItemHere);
				return true;
			}
			for (int i = 0; i < map.listNotTrans.size(); i++)
			{
				AvPosition avPosition = (AvPosition)map.listNotTrans.elementAt(i);
				if (LoadMap.type[((y0 + 12) / 24 + avPosition.y) * LoadMap.wMap + ((x0 + 12) / 24 + avPosition.x)] != 80)
				{
					Canvas.startOKDlg(T.noPlaceItemHere);
					return true;
				}
			}
		}
		else
		{
			string text = string.Empty;
			for (int j = 0; j < listItem.size(); j++)
			{
				MapItem mapItem = (MapItem)listItem.elementAt(j);
				if (mapItem.typeID == map.idType && j != indexChangeItem && x0 / 24 == mapItem.x / 24 && y0 / 24 == mapItem.y / 24)
				{
					text = T.haveItem;
					break;
				}
			}
			if (!text.Equals(string.Empty))
			{
				Canvas.startOKDlg(text);
				return true;
			}
			if ((map.buy == 2 || map.buy == 4) && (LoadMap.type[y0 / 24 * LoadMap.wMap + x0 / 24] != 80 || LoadMap.type[(y0 / 24 - 1) * LoadMap.wMap + x0 / 24] != 88))
			{
				Canvas.startOKDlg(T.setTuong);
				return true;
			}
		}
		return false;
	}

	private void doBuyItemHouse(int ii, string name)
	{
		MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(ii);
		if (!isDisable(mapItemType, xItemTranBuy, yItemTranBuy))
		{
			Canvas.getTypeMoney(mapItemType.priceXu, mapItemType.priceLuong, new IActionBuyItem(this, mapItemType, 1, name), new IActionBuyItem(this, mapItemType, 2, name), new IActionBuyItemClose(this));
		}
	}

	public void onBuyItemHouse(MapItem map3)
	{
		if (isSetTuong(map3))
		{
			map3.y++;
		}
		listItem.addElement(map3);
		LoadMap.treeLists.addElement(map3);
		setType(map3);
		LoadMap.orderVector(LoadMap.treeLists);
	}

	protected void doBrick()
	{
		if (selected != -1)
		{
			int num = (y + 12) / 24 * LoadMap.wMap + (x + 12) / 24;
			if (listTile[LoadMap.map[num]].priceLuong == -1 && listTile[LoadMap.map[num]].priceXu == -1)
			{
				Canvas.startOKDlg(T.noPlaceItemHere);
				return;
			}
			if ((indexTileMapBuy < numW && LoadMap.map[num] >= numW) || (indexTileMapBuy >= numW && LoadMap.map[num] < numW))
			{
				Canvas.startOKDlg(T.noPlaceItemHere);
				return;
			}
			xTemp = x;
			yTemp = y;
			LoadMap.map[(y + 12) / 24 * LoadMap.wMap + (x + 12) / 24] = (short)indexTileMapBuy;
		}
	}

	public override void updateKey()
	{
		base.updateKey();
		if (!isDuyChuyen && !isTranItemBuy && !isBuyTileMap)
		{
			if (!ChatTextField.isShow && PopupShop.gI() != Canvas.currentMyScreen && Input.touchCount <= 1 && !Canvas.isZoom)
			{
				Canvas.loadMap.updatePointer();
			}
			GameMidlet.avatar.updateKey();
		}
		else if (isDuyChuyen)
		{
			updateKeyMoveItem();
		}
		else if (isTranItemBuy)
		{
			updateKeyBuyItem();
		}
		else if (isBuyTileMap)
		{
			updateKeyBuyTile();
		}
	}

	private void updateKeyBuyTile()
	{
		if (Canvas.isPointerClick)
		{
			Canvas.isPointerClick = false;
			isTranItem = true;
		}
		if (isTranItem)
		{
			if (Canvas.isPointerDown && (CRes.abs(Canvas.dx()) > 10 || CRes.abs(Canvas.dy()) > 10))
			{
				isTranItem = false;
				Canvas.isPointerClick = true;
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranItem = false;
				if (CRes.abs(Canvas.dx()) < 10 && CRes.abs(Canvas.dy()) < 10)
				{
					int num = (int)((float)Canvas.px / AvMain.zoom + AvCamera.gI().xCam) / AvMain.hd / 24;
					int num2 = (int)((float)Canvas.py / AvMain.zoom + AvCamera.gI().yCam) / AvMain.hd / 24;
					int num3 = num2 * LoadMap.wMap + num;
					if (((indexTileMapBuy < numW && LoadMap.map[num3] < numW) || (indexTileMapBuy >= numW && LoadMap.map[num3] >= numW)) && (listTile[LoadMap.map[num3]].priceLuong != -1 || listTile[LoadMap.map[num3]].priceXu != -1))
					{
						LoadMap.map[num2 * LoadMap.wMap + num] = (short)indexTileMapBuy;
					}
				}
			}
		}
		Canvas.loadMap.updateKey();
	}

	private void updateKeyBuyItem()
	{
		if (Canvas.isPointerClick)
		{
			int num = (int)((float)Canvas.px / AvMain.zoom + AvCamera.gI().xCam) / AvMain.hd;
			int num2 = (int)((float)Canvas.py / AvMain.zoom + AvCamera.gI().yCam) / AvMain.hd;
			MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(indexItemTranBuy);
			Image img = AvatarData.getImgIcon(mapItemType.imgID).img;
			if (num > xItemTranBuy + mapItemType.dx && num < xItemTranBuy + mapItemType.dx + img.w / AvMain.hd && num2 > yItemTranBuy + mapItemType.dy && num2 < yItemTranBuy + mapItemType.dy + img.h / AvMain.hd)
			{
				isTranItem = true;
				xTempItem = xItemTranBuy;
				yTempItem = yItemTranBuy;
				Canvas.isPointerClick = false;
			}
			else if (Canvas.isPoint((int)((float)((xItemTranBuy + mapItemType.dx) * AvMain.hd - imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam) * AvMain.zoom), (int)((float)((yItemTranBuy + mapItemType.dy) * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom)))
			{
				Canvas.isPointerClick = false;
				isTranItem = true;
				indexFireItem = 1;
			}
			else if (Canvas.isPoint((int)((float)((xItemTranBuy + mapItemType.dx) * AvMain.hd + img.w + imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam) * AvMain.zoom), (int)((float)((yItemTranBuy + mapItemType.dy) * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom)))
			{
				Canvas.isPointerClick = false;
				isTranItem = true;
				indexCloseItem = 1;
			}
		}
		if (isTranItem)
		{
			if (Canvas.isPointerDown && (CRes.abs(Canvas.dx()) > 10 || CRes.abs(Canvas.dy()) > 10))
			{
				MapItemType mapItemType2 = (MapItemType)AvatarData.listMapItemType.elementAt(indexItemTranBuy);
				Image img2 = AvatarData.getImgIcon(mapItemType2.imgID).img;
				if (indexFireItem == 1 && !Canvas.isPoint((xItemTranBuy + mapItemType2.dx) * AvMain.hd - imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam, (yItemTranBuy + mapItemType2.dy) * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam, 32 * AvMain.hd, 32 * AvMain.hd))
				{
					indexFireItem = 0;
				}
				else if (indexCloseItem == 1 && !Canvas.isPoint((xItemTranBuy + mapItemType2.dx) * AvMain.hd + img2.w + imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam, (yItemTranBuy + mapItemType2.dy) * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam, 32 * AvMain.hd, 32 * AvMain.hd))
				{
					indexCloseItem = 0;
				}
				else
				{
					xItemTranBuy = (int)((float)xTempItem - (float)Canvas.dx() / ((float)AvMain.hd * AvMain.zoom));
					yItemTranBuy = (int)((float)yTempItem - (float)Canvas.dy() / ((float)AvMain.hd * AvMain.zoom));
				}
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranItem = false;
				if (indexFireItem == 1)
				{
					MapItemType mapItemType3 = (MapItemType)AvatarData.listMapItemType.elementAt(indexItemTranBuy);
					string name = mapItemType3.name.Substring(mapItemType3.name.IndexOf(":") + 1);
					doBuyItemHouse(indexItemTranBuy, name);
				}
				else if (indexCloseItem == 1)
				{
					me.reset();
					Canvas.endDlg();
					indexItemTranBuy = -1;
					isTranItemBuy = false;
					doSelectedItem(nameSelectedItem);
				}
				indexCloseItem = (indexFireItem = 0);
			}
		}
		Canvas.loadMap.updateKey();
	}

	private void updateKeyMoveItem()
	{
		if (Canvas.isPointerClick)
		{
			int num = (int)((float)Canvas.px / AvMain.zoom + AvCamera.gI().xCam) / AvMain.hd;
			int num2 = (int)((float)Canvas.py / AvMain.zoom + AvCamera.gI().yCam) / AvMain.hd;
			if (!isMoveItem)
			{
				for (int i = 0; i < listItem.size(); i++)
				{
					MapItem mapItem = (MapItem)listItem.elementAt(i);
					MapItemType mapItemType = null;
					mapItemType = ((!mapItem.isGetImg) ? AvatarData.getMapItemTypeByID(mapItem.typeID) : LoadMap.getMapItemTypeByID(mapItem.typeID));
					ImageIcon imgIcon = AvatarData.getImgIcon(mapItemType.imgID);
					if (imgIcon.count != -1 && Canvas.isPoint((int)(((float)((mapItem.x + mapItemType.dx) * AvMain.hd) - AvCamera.gI().xCam) * AvMain.zoom), (int)(((float)((mapItem.y + mapItemType.dy) * AvMain.hd) - AvCamera.gI().yCam) * AvMain.zoom), (int)((float)imgIcon.img.w * AvMain.zoom), (int)((float)imgIcon.img.h * AvMain.zoom)))
					{
						Canvas.isPointerClick = false;
						isTranItem = true;
						indexChangeItem = i;
						xItemOld = (xTempItem = mapItem.x + 12);
						yItemOld = (yTempItem = mapItem.y + 12);
						removeTrans(mapItem);
						break;
					}
				}
			}
			else
			{
				MapItem mapItem2 = (MapItem)listItem.elementAt(indexChangeItem);
				MapItemType mapItemType2 = null;
				mapItemType2 = ((!mapItem2.isGetImg) ? AvatarData.getMapItemTypeByID(mapItem2.typeID) : LoadMap.getMapItemTypeByID(mapItem2.typeID));
				ImageIcon imgIcon2 = AvatarData.getImgIcon(mapItemType2.imgID);
				if (imgIcon2.count == -1)
				{
					return;
				}
				if (Canvas.isPoint((int)(((float)((mapItem2.x + mapItemType2.dx) * AvMain.hd) - AvCamera.gI().xCam) * AvMain.zoom), (int)(((float)((mapItem2.y + mapItemType2.dy) * AvMain.hd) - AvCamera.gI().yCam) * AvMain.zoom), (int)((float)imgIcon2.img.w * AvMain.zoom), (int)((float)imgIcon2.img.h * AvMain.zoom)))
				{
					xTempItem = mapItem2.x;
					yTempItem = mapItem2.y;
					isTranItem = true;
					Canvas.isPointerClick = false;
				}
				else if (Canvas.isPoint((int)((float)(mapItem2.X() * AvMain.hd - imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam) * AvMain.zoom), (int)((float)(mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom)))
				{
					indexFireItem = 1;
					isTranItem = true;
					Canvas.isPointerClick = false;
				}
				else if (Canvas.isPoint((int)((float)(mapItem2.X() * AvMain.hd + mapItem2.w + imgBuyItem.frameWidth - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam) * AvMain.zoom), (int)((float)(mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom)))
				{
					indexCloseItem = 1;
					isTranItem = true;
					Canvas.isPointerClick = false;
				}
				else if (Canvas.isPoint((int)((float)(mapItem2.X() * AvMain.hd + mapItem2.w / 2 - imgBuyItem.frameWidth / 2 - 3 * AvMain.hd - (int)AvCamera.gI().xCam) * AvMain.zoom), (int)((float)(mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight * 2 - imgBuyItem.frameHeight / 2 - 3 * AvMain.hd - (int)AvCamera.gI().yCam) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom), (int)((float)(32 * AvMain.hd) * AvMain.zoom)))
				{
					indexRotateItem = 1;
					isTranItem = true;
					Canvas.isPointerClick = false;
				}
			}
		}
		if (isTranItem)
		{
			if (Canvas.isPointerDown && indexChangeItem != -1 && isMoveItem && xTempItem != -1 && (CRes.abs(Canvas.dx()) > 10 || CRes.abs(Canvas.dy()) > 10))
			{
				MapItem mapItem3 = (MapItem)listItem.elementAt(indexChangeItem);
				mapItem3.x = (mapItem3.xTo = (int)((float)xTempItem - (float)Canvas.dx() / ((float)AvMain.hd * AvMain.zoom)));
				mapItem3.y = (mapItem3.yTo = (int)((float)yTempItem - (float)Canvas.dy() / ((float)AvMain.hd * AvMain.zoom)));
			}
			if (Canvas.isPointerRelease)
			{
				Canvas.isPointerRelease = false;
				isTranItem = false;
				xTempItem = (yTempItem = -1);
				if (indexChangeItem != -1)
				{
					if (isMoveItem && CRes.abs(Canvas.dx()) <= 10 && CRes.abs(Canvas.dy()) <= 10)
					{
						setItemChange();
					}
					else
					{
						isMoveItem = true;
					}
				}
			}
		}
		Canvas.loadMap.updateKey();
	}

	private void setItemChange()
	{
		MapItem mapItem = (MapItem)listItem.elementAt(indexChangeItem);
		center = null;
		if (indexFireItem == 1)
		{
			indexFireItem = 0;
			if (!isDisable(AvatarData.getMapItemTypeByID(mapItem.typeID), mapItem.x, mapItem.y))
			{
				AvatarService.gI().doSortItem(mapItem.typeID, xItemOld / 24, yItemOld / 24, (mapItem.x + 12) / 24, (mapItem.y + 12) / 24, mapItem.dir);
				isSelectedItem = -1;
				selected = -1;
				mapItem.x = (mapItem.xTo = (mapItem.x + 12) / 24 * 24);
				mapItem.y = (mapItem.yTo = (mapItem.y + 12) / 24 * 24);
				int num = yItemOld / 24 * LoadMap.wMap + xItemOld / 24;
				LoadMap.type[num] = 80;
				setType(mapItem);
				LoadMap.orderVector(LoadMap.treeLists);
				doOption();
				isMoveItem = false;
				indexChangeItem = -1;
			}
		}
		else if (indexCloseItem == 1)
		{
			mapItem.xTo = xItemOld / 24 * 24;
			mapItem.yTo = yItemOld / 24 * 24;
			indexCloseItem = 0;
			indexChangeItem = -1;
			isMoveItem = false;
			setType(mapItem);
		}
		else if (indexRotateItem == 1)
		{
			indexRotateItem = 0;
			cmdRotate.perform();
		}
	}

	private bool setCollision(int x, int y)
	{
		if (LoadMap.map[y * LoadMap.wMap + x] != LoadMap.imgMap.nFrame - 2 && LoadMap.map[y * LoadMap.wMap + x] != -1)
		{
			return false;
		}
		return true;
	}

	public void moveCamera()
	{
		if (Canvas.menuMain != null || Canvas.currentDialog != null)
		{
			return;
		}
		if (AvCamera.gI().vY != 0f)
		{
			if (AvCamera.gI().yCam + AvCamera.gI().vY / 15f < (float)(-(Canvas.hCan / 3 - 100 * AvMain.hd)))
			{
				if (AvCamera.gI().yCam + AvCamera.gI().vY / 15f < (float)(-(Canvas.hCan / 3)))
				{
					AvCamera.gI().yCam = (AvCamera.gI().yTo = -(Canvas.hCan / 3));
					AvCamera.gI().vY /= 6f;
					AvCamera.gI().vY *= -1f;
				}
				else
				{
					AvCamera.gI().vY -= AvCamera.gI().vY / 20f;
				}
			}
			if (AvCamera.gI().yCam + AvCamera.gI().vY / 15f > AvCamera.gI().yLimit + (float)(LoadMap.w * AvMain.hd) - (float)(100 * AvMain.hd))
			{
				if (AvCamera.gI().yCam + AvCamera.gI().vY / 15f >= AvCamera.gI().yLimit + (float)(LoadMap.w * AvMain.hd))
				{
					AvCamera.gI().yCam = (AvCamera.gI().yTo = AvCamera.gI().yLimit + (float)(LoadMap.w * AvMain.hd));
					AvCamera.gI().vY /= 6f;
					AvCamera.gI().vY *= -1f;
				}
				else
				{
					AvCamera.gI().vY -= AvCamera.gI().vY / 20f;
				}
			}
			AvCamera.gI().yCam += AvCamera.gI().vY / 15f;
			AvCamera.gI().yTo = AvCamera.gI().yCam;
			AvCamera.gI().vY -= AvCamera.gI().vY / 20f;
		}
		if (AvCamera.gI().vX == 0f)
		{
			return;
		}
		if (AvCamera.gI().xCam + AvCamera.gI().vX / 15f < (float)(-LoadMap.w * AvMain.hd + 100 * AvMain.hd))
		{
			if (AvCamera.gI().xCam + AvCamera.gI().vX / 15f < (float)(-LoadMap.w * AvMain.hd))
			{
				AvCamera.gI().xCam = (AvCamera.gI().xTo = -LoadMap.w * AvMain.hd);
				AvCamera.gI().vX /= 6f;
				AvCamera.gI().vX *= -1f;
			}
			else
			{
				AvCamera.gI().vX -= AvCamera.gI().vX / 20f;
			}
		}
		if (AvCamera.gI().xCam + AvCamera.gI().vX / 15f > AvCamera.gI().xLimit + (float)(LoadMap.w * AvMain.hd) - (float)(100 * AvMain.hd))
		{
			if (AvCamera.gI().xCam + AvCamera.gI().vX / 15f >= AvCamera.gI().xLimit + (float)(LoadMap.w * AvMain.hd))
			{
				AvCamera.gI().xCam = (AvCamera.gI().xTo = AvCamera.gI().xLimit + (float)(LoadMap.w * AvMain.hd));
				AvCamera.gI().vX /= 6f;
				AvCamera.gI().vX *= -1f;
			}
			else
			{
				AvCamera.gI().vX -= AvCamera.gI().vX / 20f;
			}
		}
		AvCamera.gI().xCam += AvCamera.gI().vX / 15f;
		AvCamera.gI().xTo = AvCamera.gI().xCam;
		AvCamera.gI().vX -= AvCamera.gI().vX / 20f;
	}

	public override void update()
	{
		moveCamera();
		MapScr.gI().update();
		if (isSelectedItem == -1)
		{
			for (int i = 0; i < listItem.size(); i++)
			{
				MapItem mapItem = (MapItem)listItem.elementAt(i);
				if (CRes.abs(mapItem.x - mapItem.xTo) > 1 || CRes.abs(mapItem.y - mapItem.yTo) > 1)
				{
					mapItem.x += (mapItem.xTo - mapItem.x) / 2;
					mapItem.y += (mapItem.yTo - mapItem.y) / 2;
				}
				else
				{
					mapItem.x = mapItem.xTo;
					mapItem.y = mapItem.yTo;
				}
			}
		}
		if (!isChange && !isSelectObj && right == null && MapScr.gI().right != null)
		{
			right = LoadMap.cmdNext;
		}
		if (isChange)
		{
			x0++;
			if (x0 > 5)
			{
				x0 = 0;
			}
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		base.paint(g);
		Canvas.paintPlus(g);
	}

	private void paintIndexAble(MyGraphics g, MapItemType map)
	{
		float num = (AvCamera.gI().xCam + (float)Canvas.w) / (float)LoadMap.w + 1f;
		if (num > (float)LoadMap.wMap)
		{
			num = LoadMap.wMap;
		}
		float num2 = (AvCamera.gI().yCam + (float)Canvas.hCan) / (float)LoadMap.w + 1f;
		if (num2 > (float)LoadMap.Hmap)
		{
			num2 = LoadMap.Hmap;
		}
		int num3 = (int)(AvCamera.gI().xCam / (float)(LoadMap.w * AvMain.hd));
		if (num3 < 0)
		{
			num3 = 0;
		}
		int num4 = (int)(AvCamera.gI().yCam / (float)(LoadMap.w * AvMain.hd));
		if (num4 < 0)
		{
			num4 = 0;
		}
		int num5;
		if (map.buy == 2 || map.buy == 4)
		{
			num5 = 0;
			for (int i = num4; (float)i < num2; i++)
			{
				for (int j = num3; (float)j < num; j++)
				{
					num5 = LoadMap.map[i * LoadMap.wMap + j];
					if (num5 != -1 && LoadMap.type[i * LoadMap.wMap + j] == 80 && LoadMap.map[i * LoadMap.wMap + j] < numW && LoadMap.map[i * LoadMap.wMap + j - LoadMap.wMap] >= numW)
					{
						paintIndex(g, 2 + j * 24, 2 + i * 24, 0, 20);
					}
				}
			}
			return;
		}
		num5 = 0;
		for (int k = num4; (float)k < num2; k++)
		{
			for (int l = num3; (float)l < num; l++)
			{
				num5 = LoadMap.map[k * LoadMap.wMap + l];
				if (num5 != -1 && LoadMap.type[k * LoadMap.wMap + l] == 80)
				{
					paintIndex(g, 2 + l * 24, 2 + k * 24, 0, 20);
				}
			}
		}
	}

	public override void paintMain(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		Canvas.loadMap.paint(g);
		if (isBuyTileMap && indexTileMapBuy != -1)
		{
			float num = (AvCamera.gI().xCam + (float)Canvas.w) / (float)(LoadMap.w * AvMain.hd) + 1f;
			if (num > (float)LoadMap.wMap)
			{
				num = LoadMap.wMap;
			}
			int num2 = (int)(AvCamera.gI().yCam / (float)(LoadMap.w * AvMain.hd));
			float num3 = (AvCamera.gI().yCam + (float)Canvas.hCan) / (float)(LoadMap.w * AvMain.hd) + 1f;
			if (num3 > (float)LoadMap.Hmap)
			{
				num3 = LoadMap.Hmap;
			}
			int num4 = (int)(AvCamera.gI().xCam / (float)(LoadMap.w * AvMain.hd));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			for (int i = num2; (float)i < num3; i++)
			{
				for (int j = num4; (float)j < num; j++)
				{
					int num5 = LoadMap.map[i * LoadMap.wMap + j];
					if (num5 != -1 && ((indexTileMapBuy < numW && LoadMap.map[i * LoadMap.wMap + j] < numW) || (indexTileMapBuy >= numW && LoadMap.map[i * LoadMap.wMap + j] >= numW)) && (listTile[LoadMap.map[i * LoadMap.wMap + j]].priceLuong != -1 || listTile[LoadMap.map[i * LoadMap.wMap + j]].priceXu != -1))
					{
						paintIndex(g, 2 + j * 24, 2 + i * 24, 0, 20);
					}
				}
			}
		}
		if (indexChangeItem != -1 && isMoveItem)
		{
			MapItem mapItem = (MapItem)listItem.elementAt(indexChangeItem);
			paintIndexAble(g, AvatarData.getMapItemTypeByID(mapItem.typeID));
		}
		else if (isTranItemBuy && indexItemTranBuy != -1)
		{
			MapItemType map = (MapItemType)AvatarData.listMapItemType.elementAt(indexItemTranBuy);
			paintIndexAble(g, map);
		}
		Canvas.loadMap.paintObject(g);
		if (indexChangeItem != -1 && isMoveItem)
		{
			MapItem mapItem2 = (MapItem)listItem.elementAt(indexChangeItem);
			imgBuyItem.drawFrame(0, mapItem2.X() * AvMain.hd - imgBuyItem.frameWidth, mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight, 0, 3, g);
			imgBuyItem.drawFrame(1, mapItem2.X() * AvMain.hd + mapItem2.w + imgBuyItem.frameWidth, mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight, 0, 3, g);
			imgBuyItem.drawFrame(2, mapItem2.X() * AvMain.hd + mapItem2.w / 2, mapItem2.Y() * AvMain.hd - imgBuyItem.frameHeight * 2, 0, 3, g);
			if (Canvas.gameTick % 20 >= 10)
			{
				g.setColor(458496);
			}
			else
			{
				g.setColor(2662437);
			}
			g.drawRect((mapItem2.x + 12) * AvMain.hd - 2, (mapItem2.y + 12) * AvMain.hd - 2, 5f, 5f);
			if (Canvas.gameTick % 20 >= 10)
			{
				g.setColor(9239945);
			}
			else
			{
				g.setColor(6804068);
			}
			g.drawRect((mapItem2.x + 12) * AvMain.hd - 2 + 1, (mapItem2.y + 12) * AvMain.hd - 2 + 1, 3f, 3f);
		}
		else if (isTranItemBuy && indexItemTranBuy != -1)
		{
			MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(indexItemTranBuy);
			Image img = AvatarData.getImgIcon(mapItemType.imgID).img;
			g.drawImage(img, (xItemTranBuy + mapItemType.dx) * AvMain.hd, (yItemTranBuy + mapItemType.dy) * AvMain.hd, 0);
			imgBuyItem.drawFrame(0, (xItemTranBuy + mapItemType.dx) * AvMain.hd - imgBuyItem.frameWidth, (yItemTranBuy + mapItemType.dy) * AvMain.hd - imgBuyItem.frameHeight, 0, 3, g);
			imgBuyItem.drawFrame(1, (xItemTranBuy + mapItemType.dx) * AvMain.hd + img.w + imgBuyItem.frameWidth, (yItemTranBuy + mapItemType.dy) * AvMain.hd - imgBuyItem.frameHeight, 0, 3, g);
			if (Canvas.gameTick % 20 >= 10)
			{
				g.setColor(458496);
			}
			else
			{
				g.setColor(2662437);
			}
			g.drawRect((xItemTranBuy + 12) * AvMain.hd - 2, (yItemTranBuy + 12) * AvMain.hd - 2, 5f, 5f);
			if (Canvas.gameTick % 20 >= 10)
			{
				g.setColor(9239945);
			}
			else
			{
				g.setColor(6804068);
			}
			g.drawRect((xItemTranBuy + 12) * AvMain.hd + 1 - 2, (yItemTranBuy + 12) * AvMain.hd + 1 - 2, 3f, 3f);
		}
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
	}

	private void paintIndex(MyGraphics g, int x, int y, int i, int w)
	{
		g.setColor(color[i]);
		g.drawRect(x * AvMain.hd, y * AvMain.hd, (w - 1) * AvMain.hd, (w - 1) * AvMain.hd);
	}

	public void paintSelected(MyGraphics g)
	{
		if (isSelectObj || isSelectedItem != -1)
		{
			if (selected != -1)
			{
				MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(selected);
				if (mapItemType.buy == 2 || mapItemType.buy == 4)
				{
					for (int i = 0; i < LoadMap.map.Length; i++)
					{
						if (i > 0 && LoadMap.map[i] < numW && LoadMap.map[i - LoadMap.wMap] >= numW)
						{
							paintIndex(g, 2 + i % LoadMap.wMap * 24, 2 + i / LoadMap.wMap * 24, 0, 20);
						}
					}
				}
				else
				{
					for (int j = 0; j < LoadMap.type.Length; j++)
					{
						if (LoadMap.type[j] == 80 && (j % LoadMap.wMap != x || j / LoadMap.wMap != y))
						{
							paintIndex(g, 2 + j % LoadMap.wMap * 24, 2 + j / LoadMap.wMap * 24, 0, 20);
						}
					}
				}
			}
		}
		else if (selected != -1)
		{
			for (int k = 0; k < LoadMap.type.Length; k++)
			{
				if ((indexName == 0 && LoadMap.map[k] >= numW && LoadMap.map[k] < listTile.Length && (listTile[LoadMap.map[k]].priceLuong != -1 || listTile[LoadMap.map[k]].priceXu != -1)) || (indexName == 1 && LoadMap.map[k] < numW))
				{
					paintIndex(g, 2 + k % LoadMap.wMap * 24, 2 + k / LoadMap.wMap * 24, 0, 20);
				}
			}
			LoadMap.imgMap.drawFrame(selected, (x - xDu) * AvMain.hd, (y - yDu) * AvMain.hd, 0, 0, g);
		}
		if (indexName != -1)
		{
			paintIndex(g, x - xDu, y - yDu, 1, 24);
		}
	}

	public void onJoin(sbyte houseType, int idUser, short[] type, sbyte w, MyVector listObjM, MyVector listPlayer)
	{
		typeHome = houseType;
		idHouse = idUser;
		listItem = listObjM;
		LoadMap.wMap = w;
		LoadMap.Hmap = (short)(type.Length / w);
		LoadMap.map = type;
		if (typeHome == 4)
		{
			Canvas.loadMap.load(111, false);
		}
		else
		{
			Canvas.loadMap.load(68 + typeHome, false);
		}
		LoadMap.rememMap = -1;
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < LoadMap.Hmap; j++)
			{
				if (LoadMap.map[j * w + i] < numW)
				{
					LoadMap.type[j * w + i] = 80;
				}
				else
				{
					LoadMap.type[j * w + i] = 88;
				}
			}
			if (LoadMap.map[(LoadMap.Hmap - 1) * w + i] == imgTileMap.img.getHeight() / (24 * AvMain.hd) - 1)
			{
				LoadMap.map[(LoadMap.Hmap - 1) * w + i] = LoadMap.map[(LoadMap.Hmap - 2) * w + i];
				LoadMap.type[(LoadMap.Hmap - 1) * w + i] = 21;
				num2++;
				if (num == -1)
				{
					num = i * 24;
				}
			}
		}
		posJoin = new AvPosition(num + num2 * 24 / 2, LoadMap.Hmap * 24 - 30);
		GameMidlet.avatar.x = posJoin.x;
		GameMidlet.avatar.y = posJoin.y;
		Pet pet = LoadMap.getPet(GameMidlet.avatar.IDDB);
		if (pet != null)
		{
			pet.setPos(GameMidlet.avatar.x, GameMidlet.avatar.y);
			pet.reset();
		}
		AvCamera.gI().init(70 + typeHome);
		LoadMap.imgMap = new FrameImage(imgTileMap.img, 24 * AvMain.hd, 24 * AvMain.hd);
		for (int k = 0; k < listPlayer.size(); k++)
		{
			Avatar avatar = (Avatar)listPlayer.elementAt(k);
			avatar.xCur = avatar.x;
			avatar.yCur = avatar.y;
			if (avatar.IDDB != GameMidlet.avatar.IDDB)
			{
				LoadMap.addPlayer(avatar);
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int l = 0; l < listItem.size(); l++)
		{
			MapItem mapItem = (MapItem)listItem.elementAt(l);
			if (mapItem.x == 0 && mapItem.y == 0)
			{
				int num5 = 0;
				for (int m = 0; m < LoadMap.map.Length; m++)
				{
					if (LoadMap.type[m] == 80)
					{
						mapItem.x = (mapItem.yTo = m % LoadMap.wMap * 24);
						mapItem.y = (mapItem.yTo = m / LoadMap.wMap * 24);
						num3 = mapItem.x;
						num4 = mapItem.y;
						num5 = 1;
						setType(mapItem);
						AvatarService.gI().doSortItem(mapItem.typeID, 0, 0, mapItem.x / 24, mapItem.y / 24, mapItem.dir);
						break;
					}
				}
				if (num5 == 0)
				{
					mapItem.x = num3;
					mapItem.y = num4;
					AvatarService.gI().doSortItem(mapItem.typeID, 0, 0, mapItem.x / 24, mapItem.y / 24, mapItem.dir);
				}
			}
			if (isSetTuong(mapItem))
			{
				mapItem.y++;
			}
		}
		MapScr.gI().move();
		addItem(listItem);
		switchToMe();
		Canvas.endDlg();
		if ((float)Canvas.w >= (float)(LoadMap.wMap * LoadMap.w * AvMain.hd) * AvMain.zoom)
		{
			AvCamera.gI().xTo = (AvCamera.gI().xCam = (0f - ((float)Canvas.w - (float)(LoadMap.wMap * LoadMap.w * AvMain.hd) * AvMain.zoom)) / 2f);
		}
	}

	private bool isSetTuong(MapItem m)
	{
		if (AvatarData.getMapItemTypeByID(m.typeID).buy != 2 && AvatarData.getMapItemTypeByID(m.typeID).buy != 4)
		{
			int num = (m.y / 24 - 1) * LoadMap.wMap + m.x / 24;
			if (LoadMap.map[num] >= numW && LoadMap.map[m.y / 24 * LoadMap.wMap + m.x / 24] < numW)
			{
				return true;
			}
		}
		return false;
	}

	private bool getTileMap(short verTile)
	{
		if (imgTileMap == null)
		{
			loadTileMap();
			if (imgTileMap == null || verTile != imgTileMap.ver)
			{
				if (imgTileMap == null)
				{
					imgTileMap = new BigImgInfo();
					imgTileMap.ver = verTile;
				}
				AvatarService.gI().doGetTileMap();
				return false;
			}
			return true;
		}
		return true;
	}

	private BigImgInfo loadTileMap()
	{
		DataInputStream dataInputStream = AvatarData.initLoad("avatarTileMap");
		if (dataInputStream == null)
		{
			return null;
		}
		imgTileMap = new BigImgInfo();
		try
		{
			imgTileMap.ver = dataInputStream.readShort();
			numW = dataInputStream.readShort();
			sbyte[] data = new sbyte[dataInputStream.available()];
			dataInputStream.read(ref data);
			imgTileMap.img = CRes.createImgByByteArray(ArrayCast.cast(data));
			dataInputStream.close();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		return imgTileMap;
	}

	public void saveTileMap(sbyte[] array, int wNum)
	{
		numW = (short)wNum;
		imgTileMap.img = CRes.createImgByByteArray(ArrayCast.cast(array));
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeShort(imgTileMap.ver);
			dataOutputStream.writeShort((short)wNum);
			dataOutputStream.write(array);
			RMS.saveRMS("avatarTileMap", dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		if (MapScr.idHouse != -1)
		{
			AvatarService.gI().doJoinHouse(MapScr.idHouse);
			MapScr.idHouse = -1;
		}
		else
		{
			Canvas.endDlg();
		}
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			doBrick();
			break;
		case 1:
		{
			indexName = -1;
			int num = 0;
			for (int i = 0; i < temp.Length; i++)
			{
				if (temp[i] != LoadMap.map[i])
				{
					num = 1;
					break;
				}
			}
			if (num == 1)
			{
				AvatarService.gI().doCreateHome(LoadMap.map, 0);
				Canvas.startWaitDlg();
			}
			addPlayer();
			isBuyTileMap = false;
			indexTileMapBuy = -1;
			isChange = false;
			center = null;
			left = null;
			right = null;
			break;
		}
		case 3:
			doDiChuyen();
			break;
		case 4:
			reset();
			isDuyChuyen = false;
			AvCamera.isFollow = false;
			break;
		case 5:
			doSelectMap();
			break;
		case 8:
			InputFace.gI().close();
			break;
		case 9:
			reset();
			indexCloseItem = 0;
			if (indexChangeItem != -1)
			{
				MapItem mapItem = (MapItem)listItem.elementAt(indexChangeItem);
				mapItem.xTo = xItemOld;
				mapItem.yTo = yItemOld;
			}
			indexChangeItem = -1;
			isMoveItem = false;
			isDuyChuyen = false;
			break;
		case 50:
			AvatarService.gI().doCreateHome(LoadMap.map, 1);
			Canvas.startWaitDlg();
			break;
		case 51:
			LoadMap.map = temp;
			temp = null;
			ParkMsgHandler.onHandler();
			break;
		case 53:
			GlobalService.gI().doUpdateChest(0);
			Canvas.startWaitDlg();
			break;
		case 100:
			AvatarService.gI().doSetPassMyHouse(Canvas.inputDlg.getText(), 0, 0);
			Canvas.endDlg();
			break;
		case 101:
			GlobalService.gI().doEnterPass(Canvas.inputDlg.getText(), 0);
			break;
		}
	}

	public void onCreateHome(short type1, string str)
	{
		Canvas.endDlg();
		if (type1 == 0)
		{
			Canvas.msgdlg.setInfoLR(str, new Command(T.yes, 50), new Command(T.no, 51));
			return;
		}
		Canvas.startOKDlg(str);
		if (type1 == 2)
		{
			LoadMap.map = temp;
		}
		temp = null;
		ParkMsgHandler.onHandler();
		GameMidlet.avatar.x = posJoin.x;
		GameMidlet.avatar.y = posJoin.y;
		AvCamera.gI().init(70 + typeHome);
	}

	public void onGetTileInfo(Tile[] listTile)
	{
		this.listTile = listTile;
		doSelectMap();
		Canvas.endDlg();
	}

	private void removeTrans(MapItem map)
	{
		int num = 0;
		for (int i = 0; i < listItem.size(); i++)
		{
			MapItem mapItem = (MapItem)listItem.elementAt(i);
			if (mapItem.x / 24 == map.x / 24 && mapItem.y / 24 == map.y / 24)
			{
				num++;
			}
		}
		if (num == 1)
		{
			MapItemType mapItemTypeByID = AvatarData.getMapItemTypeByID(map.typeID);
			for (int j = 0; j < mapItemTypeByID.listNotTrans.size(); j++)
			{
				AvPosition avPosition = (AvPosition)mapItemTypeByID.listNotTrans.elementAt(j);
				LoadMap.type[(map.y / 24 + avPosition.y) * LoadMap.wMap + (map.x / 24 + avPosition.x)] = 80;
			}
		}
	}

	private MapItem getMapItem(MapItem p)
	{
		for (int i = 0; i < listItem.size(); i++)
		{
			MapItem mapItem = (MapItem)listItem.elementAt(i);
			if (mapItem.x / 24 == p.x && mapItem.y / 24 == p.y && mapItem.typeID == p.typeID)
			{
				return mapItem;
			}
		}
		return null;
	}

	public void onRemoveItem(MapItem p)
	{
		MapItem mapItem = getMapItem(p);
		LoadMap.treeLists.removeElement(mapItem);
		listItem.removeElement(mapItem);
		removeTrans(mapItem);
		ParkMsgHandler.onHandler();
		Canvas.endDlg();
	}

	protected void addItem(MyVector listItem2)
	{
		for (int i = 0; i < listItem2.size(); i++)
		{
			MapItem mapItem = (MapItem)listItem2.elementAt(i);
			LoadMap.treeLists.addElement(mapItem);
			setType(mapItem);
		}
		LoadMap.orderVector(LoadMap.treeLists);
	}

	public void setType(MapItem pos)
	{
		MapItemType mapItemTypeByID = AvatarData.getMapItemTypeByID(pos.typeID);
		sbyte b = 88;
		if (mapItemTypeByID.idType == IDHo)
		{
			b = 112;
		}
		else if (mapItemTypeByID.idType == IDHoa)
		{
			b = 111;
		}
		else if (mapItemTypeByID.iconID == 1)
		{
			b = 79;
		}
		else if (mapItemTypeByID.iconID == 2)
		{
			b = 67;
		}
		for (int i = 0; i < mapItemTypeByID.listNotTrans.size(); i++)
		{
			AvPosition avPosition = (AvPosition)mapItemTypeByID.listNotTrans.elementAt(i);
			LoadMap.type[(pos.yTo / 24 + avPosition.y) * LoadMap.wMap + (pos.xTo / 24 + avPosition.x)] = b;
		}
	}

	public void onGetTypeHouse(int type, int houseType, short verTile, MyVector list)
	{
		if (type == 0)
		{
			GameMidlet.avatar.typeHome = (sbyte)houseType;
			MapScr.gI().switchToMe();
			if (getTileMap(verTile))
			{
				if (MapScr.idHouse != -1)
				{
					AvatarService.gI().doJoinHouse(MapScr.idHouse);
					MapScr.idHouse = -1;
				}
				else
				{
					Canvas.load = 1;
					Canvas.endDlg();
				}
			}
			else
			{
				Canvas.load = 1;
			}
			return;
		}
		for (int i = 0; i < list.size(); i++)
		{
			Avatar avatar = (Avatar)list.elementAt(i);
			Avatar avatar2 = ListScr.getAvatar(avatar.IDDB);
			if (avatar != null && avatar2 != null)
			{
				avatar2.typeHome = avatar.typeHome;
			}
		}
		Canvas.endDlg();
		onRoadFriend();
	}

	public override void keyPress(int keyCode)
	{
		ChatTextField.gI().startChat(keyCode, this);
		base.keyPress(keyCode);
	}

	public void onChatFromMe(string text)
	{
		MapScr.gI().onChatFromMe(text);
	}

	public void onRoadFriend()
	{
		if (ListScr.friendL == null)
		{
			Canvas.startWaitDlg();
			CasinoService.gI().requestFriendList();
			ListScr.typeListFriend = 2;
			return;
		}
		if (ListScr.isGetTypeHouse)
		{
			ListScr.isGetTypeHouse = false;
			Canvas.startWaitDlg();
			AvatarService.gI().getTypeHouse(1);
			return;
		}
		MyVector myVector = new MyVector();
		for (int i = 0; i < ListScr.friendL.size(); i++)
		{
			Avatar avatar = (Avatar)ListScr.friendL.elementAt(i);
			if (avatar.typeHome == typeHome)
			{
				myVector.addElement(avatar);
			}
		}
		if (myVector.size() == 0)
		{
			if (Canvas.currentMyScreen == ListScr.gI())
			{
				ListScr.gI().backMyScreen.switchToMe();
			}
			Canvas.startOKDlg(T.noFriend);
		}
		else
		{
			ListScr.gI().switchToMe();
			ListScr.gI().isJoinH = true;
			ListScr.tempList = myVector;
			ListScr.gI().init();
			ListScr.gI().cmdSelected = new Command(T.selectt, 2, this);
		}
	}

	public void doJoinFriendHome(int houseType)
	{
		typeHome = (sbyte)houseType;
		if (GameMidlet.avatar.typeHome == houseType || GameMidlet.avatar.typeHome == -1)
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(T.goHome, 0, this));
			myVector.addElement(new Command(T.joinFrHome, 1, this));
			MenuCenter.gI().startAt(myVector);
		}
		else
		{
			onRoadFriend();
		}
	}

	public override void commandActionPointer(int indexMenu, int subIndex)
	{
		switch (indexMenu)
		{
		case 0:
			AvatarService.gI().doJoinHouse(GameMidlet.avatar.IDDB);
			Canvas.startWaitDlg();
			break;
		case 1:
			onRoadFriend();
			break;
		case 2:
		{
			Avatar avatar = (Avatar)ListScr.tempList.elementAt(ListScr.gI().selected);
			AvatarService.gI().doJoinHouse(avatar.IDDB);
			Canvas.startWaitDlg();
			break;
		}
		case 4:
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(T.upgradeChest, 14, this));
			myVector.addElement(new Command(T.setPass, 15, this));
			MenuCenter.gI().startAt(myVector);
			break;
		}
		case 5:
			break;
		case 6:
			GlobalService.gI().doRequestChest();
			break;
		case 7:
			doBuyItem();
			break;
		case 8:
			doKick();
			break;
		case 9:
			ipKeyboard.openKeyBoard(T.pass + ":", ipKeyboard.PASS, string.Empty, new IActionPass(), false);
			break;
		case 10:
			doSelectObject();
			break;
		case 11:
			if (listTile == null)
			{
				doCreateMap();
			}
			else
			{
				doSelectMap();
			}
			break;
		case 12:
			isDuyChuyen = true;
			doOption();
			break;
		case 13:
			MapScr.gI().doExit();
			break;
		case 14:
			PopupShop.gI().close();
			Canvas.startOKDlg(T.doYouWantUpgradeCoffer, new updateCoffer());
			break;
		case 15:
		{
			TField[] array = new TField[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = new TField(string.Empty, this, new IActionFinish(array));
				array[i].setIputType(ipKeyboard.PASS);
			}
			array[0].setFocus(true);
			Command cmd = new Command(T.finish, new IActionFinish(array));
			PopupShop.gI().close();
			InputFace.gI().setInfo(array, T.changePass, T.nameChangePass, cmd, Canvas.hCan);
			InputFace.gI().show();
			InputFace.gI().left = new Command(T.close, 8);
			break;
		}
		case 16:
			if (indexChangeItem != -1)
			{
				MapItem mapItem2 = (MapItem)listItem.elementAt(indexChangeItem);
				if (mapItem2.dir == 0)
				{
					mapItem2.dir = 2;
				}
				else
				{
					mapItem2.dir = 0;
				}
				AvatarService.gI().doSortItem(mapItem2.typeID, x / 24, y / 24, x / 24, y / 24, mapItem2.dir);
			}
			break;
		case 17:
		{
			MapItem mapItem = (MapItem)listItem.elementAt(indexChangeItem);
			if (mapItem.typeID != IDHoa)
			{
				Canvas.startOKDlg(T.doWantSellItem, new IActionSellItem(mapItem));
				indexCloseItem = 0;
				indexChangeItem = -1;
				isMoveItem = false;
			}
			break;
		}
		case 3:
			break;
		}
	}

	public override void commandAction(int indexMenu)
	{
		int num = -1;
		for (int i = 0; i < listItem.size(); i++)
		{
			MapItem mapItem = (MapItem)listItem.elementAt(i);
			if (mapItem.x / 24 == x / 24 && mapItem.y / 24 == y / 24)
			{
				num = i;
				break;
			}
		}
		int num2 = num;
		MapItem mapItem2 = null;
		if (num != -1)
		{
			mapItem2 = (MapItem)listItem.elementAt(num);
		}
		MapItem mapItem3 = mapItem2;
		switch (indexMenu)
		{
		case 5:
			MapScr.gI().doExit();
			break;
		case 11:
		{
			if (num == -1)
			{
				Canvas.startOKDlg(T.noItem);
				break;
			}
			isSelectedItem = num2;
			for (int j = 0; j < AvatarData.listMapItemType.size(); j++)
			{
				MapItemType mapItemType = (MapItemType)AvatarData.listMapItemType.elementAt(j);
				if (mapItemType.idType == mapItem3.typeID)
				{
					selected = j;
					break;
				}
			}
			left = null;
			right = null;
			center = null;
			removeTrans(mapItem3);
			posSort = new AvPosition(x / 24, y / 24, mapItem3.typeID);
			break;
		}
		case 12:
			if (num == -1)
			{
				Canvas.startOKDlg(T.noItem);
				break;
			}
			if (mapItem3.dir == 0)
			{
				mapItem3.dir = 2;
			}
			else
			{
				mapItem3.dir = 0;
			}
			AvatarService.gI().doSortItem(mapItem3.typeID, x / 24, y / 24, x / 24, y / 24, mapItem3.dir);
			break;
		case 13:
			if (num == -1)
			{
				Canvas.startOKDlg(T.noItem);
			}
			else
			{
				Canvas.startOKDlg(T.doWantSellItem, new IActionSellItem(mapItem3));
			}
			break;
		}
	}

	public void doOut()
	{
		listP_Chest = null;
		listP_Con = null;
		ParkService.gI().doJoinPark(21, 0);
		LoadMap.rememMap = -1;
	}

	public void onCustomChest(MyVector listPartCon, MyVector listPartChest, int moneyOnChest, sbyte levelChest)
	{
		listP_Con = listPartCon;
		listP_Chest = listPartChest;
		this.moneyOnChest = moneyOnChest;
		this.levelChest = levelChest;
		MyVector listCmdDoUsing = MapScr.gI().getListCmdDoUsing(listPartCon, GameMidlet.avatar.IDDB, 3, T.trans, true);
		MyVector listCmdDoUsing2 = MapScr.gI().getListCmdDoUsing(listPartChest, GameMidlet.avatar.IDDB, 2, T.trans, true);
		if (Canvas.currentMyScreen != MainMenu.me)
		{
			PopupShop.isHorizontal = true;
			PopupShop.gI().addElement(new string[2]
			{
				T.basket,
				T.container
			}, new MyVector[2] { listCmdDoUsing, listCmdDoUsing2 }, null, null);
			Command cmd = MapScr.gI().cmdDellPart(listPartCon, 1, 1, false);
			Command cmd2 = new Command(T.menu, 4, this);
			PopupShop.gI().setCmdLeft(cmd, 0);
			PopupShop.gI().setCmdLeft(cmd2, 1);
			if (Canvas.currentMyScreen != PopupShop.gI())
			{
				PopupShop.gI().switchToMe();
				PopupShop.isHorizontal = true;
			}
		}
	}

	public void onEnterPass()
	{
		Canvas.inputDlg.setInfoIkb(T.pass, new IActionEnterPass(), 0, this);
	}

	public void onTransChestPart(bool readBoolean, string readUTF)
	{
		if (!readBoolean)
		{
			Canvas.startOKDlg(readUTF);
			return;
		}
		int focusTap = PopupShop.focusTap;
		int focus = PopupShop.focus;
		if (focusTap == 0)
		{
			SeriPart o = (SeriPart)listP_Con.elementAt(focus);
			listP_Chest.addElement(o);
			listP_Con.removeElement(o);
		}
		else
		{
			SeriPart o2 = (SeriPart)listP_Chest.elementAt(focus);
			listP_Con.addElement(o2);
			listP_Chest.removeElement(o2);
		}
		restartPopup();
		Canvas.endDlg();
	}

	public void restartPopup()
	{
		int focusTap = PopupShop.focusTap;
		int num = PopupShop.focus;
		PopupShop.gI().close();
		onCustomChest(listP_Con, listP_Chest, moneyOnChest, levelChest);
		PopupShop.focusTap = focusTap;
		PopupShop.gI().setCmyLim();
		if (num >= PopupShop.gI().listCell[focusTap].size())
		{
			num = 0;
		}
		PopupShop.focus = num;
		PopupShop.gI().setCaption();
		Canvas.cameraList.setSelect(PopupShop.focus);
	}

	public void onOpenShop(sbyte typeShop, string nameShop, string[] nameItem, short[] idPart, short[] idItem, string[] timeLimit, string[] des, int[] price, short[] idPartGirl)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < nameItem.Length; i++)
		{
			myVector.addElement(new CommandShop1(T.selectt, new IActionShop1(typeShop, idItem[i], des[i]), i, nameItem[i], idPart[i], idItem[i], timeLimit[i], (price != null) ? price[i] : (-1), des[i], idPartGirl[i]));
		}
		if (myVector.size() > 0)
		{
			PopupShop.gI().switchToMe();
			PopupShop.isHorizontal = true;
			PopupShop.gI().addElement(new string[1] { nameShop }, new MyVector[1] { myVector }, null, null);
		}
	}
}
