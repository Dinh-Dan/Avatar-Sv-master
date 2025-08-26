using System;
using UnityEngine;

public class MapScr : MyScreen, IChatable
{
	private class IActionExchange : IAction
	{
		private StringObj str;

		public IActionExchange(StringObj strObj)
		{
			str = strObj;
		}

		public void perform()
		{
			GlobalService.gI().doRequestCmdRotate(str.anthor, (focusP == null) ? (-1) : focusP.IDDB);
		}
	}

	private class IActionMap : IAction
	{
		private int ii;

		private int type;

		private readonly StringObj str;

		public IActionMap(int i, int type, StringObj str)
		{
			ii = i;
			this.str = str;
			this.type = type;
		}

		public void perform()
		{
			if (type == 0)
			{
				GlobalService.gI().doCommunicate(ii);
			}
			else
			{
				GlobalService.gI().doRequestCmdRotate(str.anthor, (focusP == null) ? (-1) : focusP.IDDB);
			}
		}
	}

	private class IActionInviteHouse : IAction
	{
		private int idUser2;

		public IActionInviteHouse(int id)
		{
			idUser2 = id;
		}

		public void perform()
		{
			ParkService.gI().doInviteToMyHome(1, idUser2);
			Canvas.startWaitDlg();
		}
	}

	private class CommandGift : Command
	{
		private Part part;

		private int ii;

		public CommandGift(string name, IActionGift ac, Part part, int i)
			: base(name, ac)
		{
			this.part = part;
			ii = i;
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				string text = string.Empty;
				if (part.zOrder == 20)
				{
					text = T.cloth;
				}
				else if (part.zOrder == 10)
				{
					text = T.pant;
				}
				text += part.name;
				PopupShop.addStr(text);
				PopupShop.addStr(Canvas.getPriceMoney(part.price[0], part.price[1], false));
				PopupShop.addStr(T.level[0] + ((APartInfo)part).level);
				PopupShop.addStr(T.moneyStr + GameMidlet.avatar.strMoney);
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			part.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}
	}

	private class IActionGift : IAction
	{
		private short idPart;

		public IActionGift(short id)
		{
			idPart = id;
		}

		public void perform()
		{
			Canvas.endDlg();
			gI().doGiving(idPart);
			PopupShop.gI().close();
		}
	}

	private class CommandGiftDef : Command
	{
		private int ii;

		private int count;

		private string price;

		private ItemEffectInfo item;

		public CommandGiftDef(string name, IActionGiftDef ac, int i, ItemEffectInfo item, int count)
			: base(name, ac)
		{
			ii = i;
			this.item = item;
			this.count = count;
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && PopupShop.focus - count == ii)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(T.nameStr + item.name);
				PopupShop.addStr(T.priceStr + item.money + ((item.typeMoney != 0) ? T.gold : T.xu));
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, item.IDIcon, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}
	}

	private class IActionGiftDef : IAction
	{
		private int ii;

		private short id;

		public IActionGiftDef(int i, short id)
		{
			ii = i;
			this.id = id;
		}

		public void perform()
		{
			if (ii != 0 || LoadMap.weather == -1)
			{
				doGivingDefferent(id);
			}
			PopupShop.gI().close();
		}
	}

	private class CommandAction : Command
	{
		public int index;

		public CommandAction(string name, IActionSelectAction action, int index)
			: base(name, action)
		{
			this.index = index;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			int idx = 0;
			int idy = index;
			if (index >= Menu.imgSellect.nFrame)
			{
				idx = 1;
				idy = index % Menu.imgSellect.nFrame;
			}
			Menu.imgSellect.drawFrameXY(idx, idy, x, y, 3, g);
		}
	}

	private class IActionSelectAction : IAction
	{
		private sbyte id;

		public IActionSelectAction(sbyte id)
		{
			this.id = id;
		}

		public void perform()
		{
			if (GameMidlet.avatar.action != 2)
			{
				doAction(id);
			}
		}
	}

	private class CommandFeel : Command
	{
		private sbyte id;

		public CommandFeel(string name, IActionFeel a, sbyte id)
			: base(name, a)
		{
			this.id = id;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			APartInfo aPartInfo = (APartInfo)AvatarData.getPart(0);
			aPartInfo.paint(g, x + 2 + aPartInfo.dx[0] * AvMain.hd, y + 21 + 20 * (AvMain.hd - 1) + aPartInfo.dy[0] * AvMain.hd, 0);
			APartInfo aPartInfo2 = (APartInfo)AvatarData.getPart(id);
			aPartInfo2.paint(g, x + 2 + aPartInfo2.dx[0] * AvMain.hd, y + 21 + 20 * (AvMain.hd - 1) + aPartInfo2.dy[0] * AvMain.hd, 0);
		}
	}

	private class IActionFeel : IAction
	{
		private int ii;

		private sbyte id;

		public IActionFeel(int ii, sbyte id)
		{
			this.id = id;
			this.ii = ii;
		}

		public void perform()
		{
			if (ii == 0)
			{
				doSellectFeel(4);
			}
			else
			{
				doSellectFeel(id);
			}
		}
	}

	private class IActionGiving : IAction
	{
		private APartInfo ava;

		private int type;

		public IActionGiving(APartInfo a, int t)
		{
			ava = a;
			type = t;
		}

		public void perform()
		{
			if (focusP != null)
			{
				ParkService.gI().doGiftGiving(focusP.IDDB, ava.IDPart, type);
			}
		}
	}

	private class IActionAddFriend5 : IAction
	{
		private Avatar ava;

		private string text;

		public IActionAddFriend5(Avatar a, string text)
		{
			ava = a;
			this.text = text;
		}

		public void perform()
		{
			Canvas.msgdlg.setInfoLR(text, new Command(T.agree, new IActionAddFriend(ava)), new Command(T.refused, new IActionAddFriend1(ava)));
		}
	}

	private class IActionAddFriend1 : IAction
	{
		private Avatar p;

		public IActionAddFriend1(Avatar p)
		{
			this.p = p;
		}

		public void perform()
		{
			ParkService.gI().doAddFriend(p.IDDB, false);
		}
	}

	private class IActionAddFriend : IAction
	{
		private Avatar p;

		public IActionAddFriend(Avatar p)
		{
			this.p = p;
		}

		public void perform()
		{
			if (ListScr.friendL != null)
			{
				ListScr.gI().removeList();
			}
			ParkService.gI().doAddFriend(p.IDDB, true);
			Canvas.startOKDlg(T.addFriend + T.with + p.name + ".");
		}
	}

	private class IActionMenuUpdateChest : IAction
	{
		private MyVector list;

		private int type;

		private int typeScr;

		public IActionMenuUpdateChest(MyVector list, int type, int typeScr)
		{
			this.list = list;
			this.type = type;
			this.typeScr = typeScr;
		}

		public void perform()
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command(T.removee, new IActionDellPart(list, type, typeScr)));
			myVector.addElement(new Command(T.upgradeChest, new IactionUpdateChest()));
			MenuCenter.gI().startAt(myVector);
		}
	}

	private class IactionUpdateChest : IAction
	{
		public void perform()
		{
			Canvas.startOKDlg(T.doYouWantUpgradeCoffer, new IActionUpgrade());
		}
	}

	private class IActionDellPart : IAction
	{
		private MyVector list;

		private int type;

		private int typeScr;

		public IActionDellPart(MyVector list, int type, int typeScr)
		{
			this.list = list;
			this.typeScr = typeScr;
			this.type = type;
		}

		public void perform()
		{
			MyVector myVector = new MyVector();
			for (int i = 0; i < list.size(); i++)
			{
				SeriPart seriPart = (SeriPart)list.elementAt(i);
				Part part = AvatarData.getPart(seriPart.idPart);
				if (part != null && part.zOrder != 30 && part.zOrder != 40)
				{
					myVector.addElement(seriPart);
				}
			}
			if (PopupShop.focus < myVector.size())
			{
				SeriPart seriPart2 = (SeriPart)myVector.elementAt(PopupShop.focus);
				Part part2 = AvatarData.getPart(seriPart2.idPart);
				if (!AvatarData.isZOrderMain(part2.zOrder) || type == 1)
				{
					Canvas.startOKDlg(T.doYouWantDel, new IActionDel(list, type, typeScr, part2, myVector, seriPart2, instance));
				}
			}
		}
	}

	private class IActionDel : IAction
	{
		private Part part;

		private MyVector list;

		private int type;

		private int typeScr;

		private MyVector lis;

		private SeriPart se;

		private MapScr me;

		public IActionDel(MyVector list, int type, int typeScr, Part p, MyVector lis, SeriPart seri, MapScr me)
		{
			this.me = me;
			se = seri;
			part = p;
			this.lis = lis;
			this.list = list;
			this.type = type;
			this.typeScr = typeScr;
		}

		public void perform()
		{
			GlobalService.gI().doRemoveItem(part.IDPart, type);
			lis.removeElementAt(PopupShop.focus);
			list.removeElement(se);
			if (typeScr == 0)
			{
				if (MainMenu.gI().isWearing)
				{
					MainMenu.gI().doWearing();
				}
				else
				{
					me.doStore();
				}
			}
			else
			{
				HouseScr.gI().restartPopup();
			}
		}
	}

	private class IAction111 : IAction
	{
		private int type;

		private int id;

		private int ii;

		private SeriPart seri;

		public IAction111(int type, SeriPart se, int id, int ii)
		{
			this.type = type;
			seri = se;
			this.id = id;
			this.ii = ii;
		}

		public void perform()
		{
			Canvas.startOKDlg(T.trasContainter[type], new IAction222(type, seri, id, ii));
		}
	}

	private class IAction222 : IAction
	{
		private int type;

		private int id;

		private int ii;

		private SeriPart seri;

		public IAction222(int type, SeriPart se, int id, int ii)
		{
			this.type = type;
			seri = se;
			this.id = id;
			this.ii = ii;
		}

		public void perform()
		{
			Part part = AvatarData.getPart(seri.idPart);
			if (id == GameMidlet.avatar.IDDB && (!AvatarData.isZOrderMain(part.zOrder) || type != 0))
			{
				if (type == 2)
				{
					GlobalService.gI().doTransChestPart(1, ii, seri.idPart);
				}
				else if (type == 3)
				{
					GlobalService.gI().doTransChestPart(0, ii, seri.idPart);
				}
				else
				{
					GlobalService.gI().doUsingItem(seri.idPart, (sbyte)type);
				}
				Canvas.startWaitDlg();
			}
		}
	}

	private class IActionTransItem : IAction
	{
		private SeriPart se;

		private int type;

		public IActionTransItem(int ty, SeriPart seri)
		{
			type = ty;
			se = seri;
		}

		public void perform()
		{
			Canvas.startOKDlg(T.trasContainter[type], new IActionDoUsingItem(type, se));
		}
	}

	private class IActionDoUsingItem : IAction
	{
		private SeriPart se;

		private int type;

		public IActionDoUsingItem(int ty, SeriPart seri)
		{
			type = ty;
			se = seri;
		}

		public void perform()
		{
			GlobalService.gI().doUsingItem(se.idPart, (sbyte)type);
			Canvas.startWaitDlg();
		}
	}

	private class CommandUsingPart : Command
	{
		private SeriPart se;

		private int ii;

		private int type;

		public CommandUsingPart(string name, IAction ac, SeriPart seri, int i, int ty)
			: base(name, ac)
		{
			se = seri;
			ii = i;
			type = ty;
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				Part part = AvatarData.getPart(se.idPart);
				PopupShop.resetIsTrans();
				string empty = string.Empty;
				empty += AvatarData.getName(part);
				PopupShop.addStr(empty);
				PopupShop.addStr(T.doBen + (100 - se.time) + "%");
				if (se.expireString != null && !se.expireString.Equals(string.Empty))
				{
					PopupShop.addStr(se.expireString);
				}
				if (type == 0)
				{
					PopupShop.addStr(T.level[2] + ": " + AvatarData.getLevel(part));
				}
				else if (part.follow != -2)
				{
					int num = 0;
					PopupShop.addStr(string.Concat(arg2: (int)((part.follow < 0) ? ((APartInfo)part).level : ((APartInfo)AvatarData.getPart(part.follow)).level), arg0: T.level[2], arg1: ": "));
				}
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			Part part = AvatarData.getPart(se.idPart);
			part.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
			int num = (100 - se.time) * PopupShop.imgTimeUse[0].w / 100;
			g.drawImage(PopupShop.imgTimeUse[0], x + 6 * AvMain.hd, y + PopupShop.wCell - 6 * AvMain.hd - 4 * AvMain.hd, 0);
			int num2 = num;
			if (num2 >= PopupShop.imgTimeUsePer.frameWidth * 2)
			{
				PopupShop.imgTimeUsePer.drawFrame(0, x + 6 * AvMain.hd, y + PopupShop.wCell - 6 * AvMain.hd - 4 * AvMain.hd, 0, g);
				PopupShop.imgTimeUsePer.drawFrame(1, x + 6 * AvMain.hd + num2 - PopupShop.imgTimeUsePer.frameWidth, y + PopupShop.wCell - 6 * AvMain.hd - 4 * AvMain.hd, 0, g);
				g.drawImageScaleClip(PopupShop.imgTimeUse[1], x + 6 * AvMain.hd + PopupShop.imgTimeUsePer.frameWidth, y + PopupShop.wCell - 6 * AvMain.hd - 4 * AvMain.hd, num2 - PopupShop.imgTimeUsePer.frameWidth * 2, PopupShop.imgTimeUse[1].getHeight(), 0);
			}
		}
	}

	private class IActionUsingPart : IAction
	{
		private int ii;

		private int type;

		private SeriPart se;

		private int id;

		public IActionUsingPart(int ii, int type, SeriPart seri, int id)
		{
			this.ii = ii;
			this.type = type;
			se = seri;
			this.id = id;
		}

		public void perform()
		{
			Part part = AvatarData.getPart(se.idPart);
			if (id == GameMidlet.avatar.IDDB && (!AvatarData.isZOrderMain(part.zOrder) || type != 0))
			{
				Canvas.startOKDlg(T.trasContainter[type], new IActionTransContainer(ii, se, type));
			}
		}
	}

	private class IActionTransContainer : IAction
	{
		private int ii;

		private int type;

		private SeriPart se;

		public IActionTransContainer(int i, SeriPart seri, int type)
		{
			ii = i;
			se = seri;
			this.type = type;
		}

		public void perform()
		{
			if (type == 2)
			{
				GlobalService.gI().doTransChestPart(1, ii, se.idPart);
			}
			else if (type == 3)
			{
				GlobalService.gI().doTransChestPart(0, ii, se.idPart);
			}
			else
			{
				GlobalService.gI().doUsingItem(se.idPart, (sbyte)type);
			}
			Canvas.startWaitDlg();
		}
	}

	private class IActionEmpty : IAction
	{
		public void perform()
		{
		}
	}

	private class IActionUpgrade : IAction
	{
		public void perform()
		{
			GlobalService.gI().doUpdateContainer(0);
			Canvas.startWaitDlg();
		}
	}

	private class CommandIceDream : Command
	{
		private Item item;

		private int ii;

		public CommandIceDream(string name, IActionIceDream a, Item it, int i)
			: base(name, a)
		{
			item = it;
			ii = i;
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(item.name);
				PopupShop.addStr(T.priceStr + Canvas.getMoneys(item.price[0]) + T.dola);
				PopupShop.addStr(T.have + GameMidlet.avatar.strMoney);
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.listImgInfo[item.idIcon].paintPart(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}
	}

	private class IActionIceDream : IAction
	{
		private readonly Item item;

		public IActionIceDream(Item it)
		{
			item = it;
		}

		public void perform()
		{
			doBuyIceDream(item);
		}
	}

	private class IActionBuyDream : IAction
	{
		private Item item;

		public IActionBuyDream(Item it)
		{
			item = it;
		}

		public void perform()
		{
			ParkService.gI().doBuyItem(item.ID);
			Canvas.startWaitDlg();
		}
	}

	private class CommandOpenShop : Command
	{
		private readonly Part ava;

		private readonly short IDPart;

		private readonly short idShop;

		private readonly int ii;

		private readonly int idBoss1;

		public CommandOpenShop(string name, IActionOpenShop a, Part p, short idPart, int i, int idB, int idShop)
			: base(name, a)
		{
			ava = p;
			IDPart = idPart;
			ii = i;
			idBoss1 = idB;
			this.idShop = (short)idShop;
		}

		public override void update()
		{
			if (!PopupShop.isTransFocus || ii != PopupShop.focus)
			{
				return;
			}
			Part part = ava;
			if (ava.IDPart == -1)
			{
				part = AvatarData.getPart(IDPart);
			}
			if (part.IDPart != -1)
			{
				setAvatarShop(part);
				PopupShop.resetIsTrans();
				PopupShop.addStr(part.name);
				if (idBoss1 == -1)
				{
					PopupShop.addStr(Canvas.getPriceMoney(part.price[0], part.price[1], false));
				}
				if (part.follow == -1)
				{
					PopupShop.addStr(T.level[0] + ((APartInfo)part).level);
				}
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			Part part = ava;
			if (ava.IDPart == -1)
			{
				part = AvatarData.getPart(IDPart);
			}
			if (part.IDPart != -1)
			{
				part.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
			}
		}
	}

	private class CommandOpenShop2 : Command
	{
		private int ii;

		private short idPart;

		private string textDes;

		public CommandOpenShop2(string name, IActionOpenShop2 a, int i, short idP, string text)
			: base(name, a)
		{
			ii = i;
			idPart = idP;
			textDes = text;
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(textDes);
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, idPart, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}
	}

	private class IActionOpenShop2 : IAction
	{
		private readonly int idBoss1;

		private readonly int idShop;

		private readonly int ii;

		public IActionOpenShop2(int idB, int idShop, int i)
		{
			idBoss1 = idB;
			this.idShop = idShop;
			ii = i;
		}

		public void perform()
		{
			ParkService.gI().doBossShop(idBoss1, idShop, ii);
			Canvas.endDlg();
		}
	}

	private class IActionOpenShop : IAction
	{
		private readonly Part ava;

		private readonly short IDPart;

		private readonly int idBoss;

		private readonly string textDes;

		private readonly int idShop;

		private readonly int ii;

		public IActionOpenShop(Part p, short listPar, int idShop, string textDes, int idB, int i)
		{
			ava = p;
			IDPart = listPar;
			this.idShop = idShop;
			this.textDes = textDes;
			idBoss = idB;
			ii = i;
		}

		public void perform()
		{
			if (idShop == 100)
			{
				Canvas.startOKDlg(T.doYouWantDial, new IActionDial(IDPart));
				return;
			}
			if (idShop == 26)
			{
				Canvas.endDlg();
				gI().doGiving(IDPart);
				PopupShop.gI().close();
				return;
			}
			Part part = ava;
			if (ava.IDPart == -1)
			{
				part = AvatarData.getPart(IDPart);
			}
			short iDPart = part.IDPart;
			if (idBoss == -1 || idShop == 17 || idShop == 18)
			{
				doSelectMoneyBuyItem(part);
			}
			else
			{
				Canvas.startOKDlg(textDes, new IActionTextDes(idBoss, idShop, ii));
			}
		}
	}

	private class IActionTextDes : IAction
	{
		private readonly int idBoss1;

		private readonly int idShop;

		private readonly int ii;

		public IActionTextDes(int idB, int idShop, int i)
		{
			idBoss1 = idB;
			this.idShop = idShop;
			ii = i;
		}

		public void perform()
		{
			ParkService.gI().doBossShop(idBoss1, idShop, ii);
		}
	}

	private class IActionDial : IAction
	{
		private readonly short id;

		public IActionDial(short id)
		{
			this.id = id;
		}

		public void perform()
		{
			PopupShop.gI().close();
			DialLuckyScr.gI().switchToMe(Canvas.currentMyScreen, id);
		}
	}

	private class IActionPopup1 : IAction
	{
		private string text5;

		private int idBoss;

		private int idPopup;

		private int type;

		private string[] subText;

		public IActionPopup1(string text5, string[] subText, int idBoss, int idPopup, int type)
		{
			this.text5 = text5;
			this.subText = subText;
			this.idBoss = idBoss;
			this.idPopup = idPopup;
			this.type = type;
		}

		public void perform()
		{
			switch (type)
			{
			case 1:
				Canvas.msgdlg.setInfoC(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)));
				break;
			case 2:
				Canvas.msgdlg.setInfoLR(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)), new Command(subText[1], new IActionCustomPopup(idBoss, idPopup, 1)));
				break;
			case 3:
				Canvas.msgdlg.setInfoLCR(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)), new Command(subText[1], new IActionCustomPopup(idBoss, idPopup, 1)), new Command(subText[2], new IActionCustomPopup(idBoss, idPopup, 2)));
				break;
			}
		}
	}

	private class IActionCustomPopup : IAction
	{
		private readonly int idBoss;

		private readonly int idPopup;

		private readonly int ii;

		public IActionCustomPopup(int idB, int idPopup, int i)
		{
			idBoss = idB;
			this.idPopup = idPopup;
			ii = i;
		}

		public void perform()
		{
			ParkService.gI().doCustomPopup(idBoss, idPopup, ii);
		}
	}

	private class CommandMenuRotate : Command
	{
		private readonly int idIcon;

		public CommandMenuRotate(string name, IActionMenuRotate a, int id)
			: base(name, a)
		{
			idIcon = id;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, idIcon, x, y, 3);
		}
	}

	private class IActionMenuRotate : IAction
	{
		private readonly StringObj str2;

		public IActionMenuRotate(StringObj str)
		{
			str2 = str;
		}

		public void perform()
		{
			if (focusP != null)
			{
				GlobalService.gI().doRequestCmdRotate(str2.anthor, focusP.IDDB);
			}
			else
			{
				GlobalService.gI().doRequestCmdRotate(str2.anthor, -1);
			}
		}
	}

	private class IActionMiniMapKey : IAction
	{
		public void perform()
		{
		}
	}

	private class IActionChangePass : IAction
	{
		private readonly TField[] tf_P;

		public IActionChangePass(TField[] tf)
		{
			tf_P = tf;
		}

		public void perform()
		{
			if (setEnterPass(tf_P))
			{
				GlobalService.gI().doChangePass(tf_P[0].getText(), tf_P[1].getText());
				Canvas.startWaitDlg();
				InputFace.gI().close();
			}
		}
	}

	private class CommandShopOffline : Command
	{
		private Part ava;

		private int cou;

		public CommandShopOffline(string name, IActionShopOffline ac, Part ava, int count)
			: base(name, ac)
		{
			this.ava = ava;
			cou = count;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			ava.paintIcon(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 0, 3);
		}

		public override void update()
		{
			if (cou == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				setAvatarShop(ava);
				string empty = string.Empty;
				empty += AvatarData.getName(ava);
				PopupShop.addStr(empty);
				PopupShop.addStr(Canvas.getPriceMoney(ava.price[0], ava.price[1], false));
				PopupShop.addStr(T.levelRequest + AvatarData.getLevel(ava));
				PopupShop.addStr(T.level[0] + GameMidlet.myIndexP.level);
			}
		}
	}

	private class IActionShopOffline : IAction
	{
		private short idPart;

		public IActionShopOffline(short idPart)
		{
			this.idPart = idPart;
		}

		public void perform()
		{
			doBuyItem(idPart);
		}
	}

	private class IActionSelectedMoney : IAction
	{
		private short idPart;

		private int type;

		public IActionSelectedMoney(short idPart, int type)
		{
			this.idPart = idPart;
			this.type = type;
		}

		public void perform()
		{
			AvatarService.gI().doBuyItem(idPart, type);
		}
	}

	private class IActionChat : IKbAction
	{
		public void perform(string text)
		{
			if (!text.Equals(string.Empty))
			{
				Canvas.currentMyScreen.doChat(text);
			}
		}
	}

	private class IActionWedding : IAction
	{
		private short idActionWedding;

		public IActionWedding(short idActionWedding)
		{
			this.idActionWedding = idActionWedding;
		}

		public void perform()
		{
			GlobalService.gI().doRequestCmdRotate(idActionWedding, -1);
			PopupShop.gI().close();
			Canvas.startWaitDlg();
		}
	}

	private class IActionInfo : IAction
	{
		public void perform()
		{
		}
	}

	private class CommandInfo : Command
	{
		private Image[] imgIcon;

		private Image[] imgThanh;

		private Image imgShadow;

		private Image imgBack;

		private Avatar avatar;

		private Avatar friend;

		private string tenQuanHe;

		private int w;

		private int h;

		private int yLine;

		private int hIndex;

		private int xLeft;

		private int xRight;

		private int yBackG = 25 * AvMain.hd;

		private sbyte level;

		private sbyte perLevel;

		private int idImage;

		public CommandInfo(string caption, IAction action, Avatar avatar, Avatar friend, string tenQuanHe, sbyte lv, sbyte perLV, int idImage)
			: base(caption, action)
		{
			this.avatar = avatar;
			this.avatar.direct = Base.RIGHT;
			this.friend = friend;
			this.idImage = idImage;
			this.tenQuanHe = tenQuanHe;
			level = lv;
			perLevel = perLV;
			w = PopupShop.w;
			h = PopupShop.h;
			imgIcon = new Image[5];
			for (int i = 0; i < 5; i++)
			{
				imgIcon[i] = Image.createImagePNG(T.getPath() + "/myinfo/icon" + i);
			}
			imgShadow = Image.createImagePNG(T.getPath() + "/myinfo/shadow");
			imgBack = Image.createImagePNG(T.getPath() + "/myinfo/back");
			imgThanh = new Image[3];
			for (int j = 0; j < 3; j++)
			{
				imgThanh[j] = Image.createImagePNG(T.getPath() + "/myinfo/thanh" + j);
			}
			hIndex = imgThanh[2].h + 10 * AvMain.hd;
			if (AvMain.hd == 1)
			{
				yLine = h - (hIndex * 3 + 48);
			}
			else
			{
				yLine = h - (hIndex * 3 + 84);
			}
			for (int k = 0; k < 3; k++)
			{
				string empty = string.Empty;
				empty = ((k != 0 || avatar.lvFarm == -1) ? (T.myIndex[k] + ((k != 0) ? string.Empty : (string.Empty + avatar.lvMain))) : ("Lv NT " + avatar.lvFarm));
				int width = Canvas.fontBlu.getWidth(empty);
				if (width > xLeft)
				{
					xLeft = width;
				}
				width = Canvas.fontBlu.getWidth(avatar.indexP[2 + k] + string.Empty);
				if (width > xRight)
				{
					xRight = width;
				}
			}
			xLeft -= 4 * AvMain.hd;
		}

		public override void update()
		{
			if (PopupShop.gI().left != null)
			{
				PopupShop.gI().left.x = PopupShop.x + w - imgBack.w / 2 - 20 * AvMain.hd;
				PopupShop.gI().left.y = PopupShop.y + yBackG + imgBack.getHeight() + PaintPopup.hButtonSmall + 20;
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			g.translate(-8 * AvMain.hd, -40 * AvMain.hd);
			g.setClip(0f, 0f, w, h);
			int num = 23 * AvMain.hd;
			int num2 = 20 * AvMain.hd;
			for (int i = 0; i < 2; i++)
			{
				g.drawImage(imgIcon[i], 16 * AvMain.hd + num / 2, 20 * AvMain.hd + num / 2 + i * num, 3);
			}
			g.drawImage(imgIcon[4], 16 * AvMain.hd + num / 2, 20 * AvMain.hd + num / 2 + 2 * num, 3);
			g.drawImage(imgIcon[2], 16 * AvMain.hd + num / 2, 20 * AvMain.hd + num / 2 + 3 * num, 3);
			g.drawImage(imgIcon[3], 16 * AvMain.hd + num / 2, 20 * AvMain.hd + num / 2 + 4 * num, 3);
			Canvas.normalFont.drawString(g, avatar.money[0] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 - Canvas.normalFont.getHeight() / 2, 0);
			Canvas.normalFont.drawString(g, avatar.money[2] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num - Canvas.normalFont.getHeight() / 2, 0);
			Canvas.normalFont.drawString(g, avatar.luongKhoa + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num * 2 - Canvas.normalFont.getHeight() / 2, 0);
			Canvas.normalFont.drawString(g, avatar.money[3] + string.Empty, 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num * 3 - Canvas.normalFont.getHeight() / 2, 0);
			Canvas.normalFont.drawString(g, avatar.lvMain + "+" + avatar.perLvMain + "%", 16 * AvMain.hd + num, 20 * AvMain.hd + num / 2 + num * 4 - Canvas.normalFont.getHeight() / 2, 0);
			int num3 = 0;
			g.drawImage(imgBack, w - imgBack.w - 20 * AvMain.hd, yBackG + 3 * AvMain.hd, 0);
			avatar.paintIcon(g, w - imgBack.w - 30 * AvMain.hd + 40 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd, true);
			if (friend == null)
			{
				g.drawImage(imgShadow, w - 20 * AvMain.hd - imgShadow.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd - imgShadow.h / 2, 3);
			}
			else
			{
				friend.paintIcon(g, w - 10 * AvMain.hd - imgShadow.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 10 * AvMain.hd, true);
				Canvas.fontBlu.drawString(g, tenQuanHe, w - imgBack.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 5 * AvMain.hd - imgShadow.h / 2 - 11 * AvMain.hd - Canvas.fontBlu.getHeight(), 2);
				AvatarData.paintImg(g, idImage, w - imgBack.w / 2 - 20 * AvMain.hd, yBackG + imgBack.h - 5 * AvMain.hd - imgShadow.h / 2, 3);
				Canvas.fontBlu.drawString(g, level + "+" + perLevel + "%", w - imgBack.w / 2 - 20 * AvMain.hd + Canvas.fontBlu.getWidth("%") / 2, yBackG + imgBack.h - 5 * AvMain.hd - imgShadow.h / 2 + 10 * AvMain.hd, 2);
			}
			g.translate(0f, yLine + ((AvMain.hd == 1) ? 10 : 0));
			g.setColor(29068);
			g.fillRect(20 * AvMain.hd, 0f, w - 40 * AvMain.hd, 1f);
			g.setColor(12255224);
			g.fillRect(20 * AvMain.hd, 1f, w - 40 * AvMain.hd, 1f);
			int num4 = (h - yLine - PaintPopup.hTab - hIndex * 3) / 2 - 2;
			for (int j = 0; j < 3; j++)
			{
				g.drawImage(imgThanh[2], num2 + xLeft + imgThanh[2].w / 2, num4 + hIndex / 2 + hIndex * j, 3);
				g.drawImage(imgThanh[2], w - num2 - xRight - imgThanh[2].w / 2, num4 + hIndex / 2 + hIndex * j, 3);
				if (j == 0)
				{
					if (avatar.lvFarm != -1)
					{
						Canvas.fontBlu.drawString(g, "Lv NT " + avatar.lvFarm, num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
					}
					else
					{
						Canvas.fontBlu.drawString(g, T.myIndex[j] + ((j != 0) ? string.Empty : (string.Empty + avatar.lvMain)), num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
					}
				}
				else
				{
					Canvas.fontBlu.drawString(g, T.myIndex[j] + ((j != 0) ? string.Empty : (string.Empty + avatar.lvMain)), num2 + xLeft - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
				}
				Canvas.fontBlu.drawString(g, T.myIndex[3 + j], w - num2 - xRight - imgThanh[2].w - 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * j - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 1);
			}
			if (avatar.lvFarm != -1)
			{
				Canvas.fontBlu.drawString(g, avatar.perLvFarm + string.Empty, num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 - Canvas.fontBlu.getHeight() / 2 - 3 * AvMain.hd, 0);
			}
			else
			{
				Canvas.fontBlu.drawString(g, avatar.perLvMain + string.Empty, num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 - Canvas.fontBlu.getHeight() / 2 - 3 * AvMain.hd, 0);
			}
			for (int k = 0; k < 3; k++)
			{
				if (k > 0)
				{
					Canvas.fontBlu.drawString(g, avatar.indexP[k - 1] + string.Empty, num2 + xLeft + imgThanh[2].w + 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * k - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 0);
				}
				Canvas.fontBlu.drawString(g, avatar.indexP[2 + k] + string.Empty, w - num2 - xRight + 2 * AvMain.hd, num4 + hIndex / 2 + hIndex * k - Canvas.fontBlu.getHeight() / 2 - 2 * AvMain.hd, 0);
			}
			int num5 = 0;
			if (AvMain.hd == 2)
			{
				num5 = 1;
			}
			for (int l = 0; l < 3; l++)
			{
				g.setClip(w: (l != 0) ? (avatar.indexP[l - 1] * imgThanh[1].w / 100) : ((avatar.lvFarm == -1) ? (avatar.perLvMain * imgThanh[1].w / 100) : (avatar.perLvFarm * imgThanh[1].w / 100)), x: num2 + xLeft + 1, y: num4 + hIndex * l, h: hIndex);
				g.drawImage(imgThanh[1], num2 + xLeft + imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * l - num5, 3);
				num3 = avatar.indexP[2 + l] * imgThanh[1].w / 100;
				g.setClip(w - num2 - xRight - imgThanh[2].w + 2, num4 + hIndex * l, num3, hIndex);
				g.drawImage(imgThanh[1], w - num2 - xRight - imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * l - num5, 3);
			}
			g.setClip(0f, 0f, w, h - PaintPopup.hTab - yLine);
			for (int m = 0; m < 3; m++)
			{
				g.drawImage(imgThanh[0], num2 + xLeft + imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * m - 2 * AvMain.hd, 3);
				g.drawImage(imgThanh[0], w - num2 - xRight - imgThanh[2].w / 2 + 1, num4 + hIndex / 2 + hIndex * m - 2 * AvMain.hd, 3);
			}
		}
	}

	public static MapScr instance;

	public static sbyte roomID;

	public static sbyte boardID;

	public static Image imgFocusP;

	public static Image imgHeart;

	public Command cmdMenu;

	public Command cmdEvent;

	public Command cmdGoToMap;

	public Command cmdExchangeBoss;

	public static sbyte typeJoin = -1;

	public static Avatar focusP;

	public static sbyte typeCasino = -1;

	public static int indexMenu = 0;

	public static int indexAction;

	public static int indexFeel;

	public static int indexExchange;

	public static Image imgBar;

	public static MyVector listFish = new MyVector();

	public static int indexMap = -1;

	public static MyVector listCmd;

	public static MyVector listCmdRotate;

	public static MyVector listChair;

	public static MyVector listItemEffect;

	public static bool isWedding = false;

	public static bool isNewVersion = false;

	public static int idHouse = -1;

	private static sbyte[] ac = new sbyte[4] { 10, 4, 3, 5 };

	private int dir = 1;

	public static Avatar avatarShop;

	private sbyte countWeeding = -1;

	public static bool isOpenInfo = false;

	private int count;

	private MyVector chatList = new MyVector();

	private int chatDelay;

	private int MAX_CHAT_DELAY = 120;

	public bool isTour = true;

	public static sbyte idSelectedMini;

	public static sbyte idCityMap;

	public static short[] idImg;

	public static int idMapOffline = -1;

	public static int idMapOld = -1;

	private sbyte iGoChaSu;

	public static int idUserWedding_1;

	public static int idUserWedding_2;

	public MapScr()
	{
		initCmd();
	}

	public static MapScr gI()
	{
		if (instance == null)
		{
			instance = new MapScr();
		}
		return instance;
	}

	public void initCmd()
	{
		cmdMenu = new Command(T.menu, 0);
		cmdEvent = new Command(T.eventt, 1, this);
		cmdGoToMap = new Command(T.exit, 0, this);
		cmdExchangeBoss = new Command(T.exchange, 2);
	}

	public override void switchToMe()
	{
		base.switchToMe();
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case -1:
			break;
		case 0:
			doExit();
			break;
		case 1:
			doEvent();
			break;
		case 2:
			exitGame();
			break;
		case 3:
			doSelectedMiniMap();
			break;
		case 4:
		{
			MyVector myVector = new MyVector();
			if (listCmdRotate.size() <= 0)
			{
				break;
			}
			for (int i = 0; i < listCmdRotate.size(); i++)
			{
				StringObj stringObj = (StringObj)listCmdRotate.elementAt(i);
				if (stringObj.type == 0)
				{
					myVector.addElement(new Command(stringObj.str, new IActionExchange(stringObj)));
				}
			}
			MenuCenter.gI().startAt(myVector);
			break;
		}
		case 5:
			isOpenInfo = true;
			ParkService.gI().doRequestYourInfo(GameMidlet.avatar.IDDB);
			break;
		case 6:
			MainMenu.gI().isWearing = true;
			GlobalService.gI().doRequestContainer(GameMidlet.avatar.IDDB);
			break;
		case 7:
			ListScr.gI().setFriendList(false);
			break;
		}
	}

	public override void doMenu()
	{
		MyVector myVector = new MyVector();
		if (listCmd != null && listCmd.size() > 0)
		{
			for (int i = 0; i < listCmd.size(); i++)
			{
				int i2 = i;
				StringObj stringObj = (StringObj)listCmd.elementAt(i);
				Command command = new Command(stringObj.str, new IActionMap(i2, 0, stringObj));
				command.indexImage = 0;
				myVector.addElement(command);
			}
		}
		int num = 0;
		if (listCmdRotate.size() > 0)
		{
			for (int j = 0; j < listCmdRotate.size(); j++)
			{
				StringObj stringObj2 = (StringObj)listCmdRotate.elementAt(j);
				if (stringObj2.type == 0)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			Command command2 = new Command(T.game, 4, this);
			command2.indexImage = 1;
			myVector.addElement(command2);
		}
		Command command3 = new Command(T.info, 5, this);
		command3.indexImage = 2;
		myVector.addElement(command3);
		Command command4 = new Command(T.basket, 6, this);
		command4.indexImage = 3;
		myVector.addElement(command4);
		Command command5 = new Command(T.friend, 7, this);
		command5.indexImage = 4;
		myVector.addElement(command5);
		Command command6 = new Command(T.map, 0, this);
		command6.indexImage = 5;
		myVector.addElement(command6);
		MenuLeft.gI().startAt(myVector);
	}

	public override void close()
	{
		doExit();
	}

	public void doExit()
	{
		Canvas.startWaitDlg();
		typeJoin = -1;
		typeCasino = -1;
		if (GameMidlet.CLIENT_TYPE == 8)
		{
			joinCitymap();
		}
		else
		{
			GlobalService.gI().getHandler(8);
		}
	}

	public override void initZoom()
	{
		AvCamera.gI().init(roomID + 1);
	}

	public void doEvent()
	{
		MessageScr.gI().switchToMe();
	}

	public void doHit()
	{
		if (focusP != null)
		{
			doGivingDefferent(100);
		}
	}

	public void doInviteToMyHome()
	{
		if (focusP != null)
		{
			ParkService.gI().doInviteToMyHome(0, focusP.IDDB);
		}
	}

	public void onInviteToMyHome(sbyte type2, int idUser2)
	{
		Canvas.endDlg();
		Avatar avatar = LoadMap.getAvatar(idUser2);
		if (avatar != null)
		{
			if (type2 == 0)
			{
				Canvas.startOKDlg(T.youAreInvite + avatar.name + ". " + T.doYouWant, new IActionInviteHouse(idUser2));
			}
			else if (type2 == 1)
			{
				idHouse = idUser2;
				GlobalService.gI().getHandler(11);
				Canvas.startWaitDlg();
			}
		}
	}

	public static void doAction(sbyte k)
	{
		GameMidlet.avatar.doAction(k);
		AvatarService.gI().doFeel(k);
	}

	public static void doSellectFeel(int focus)
	{
		GameMidlet.avatar.setFeel(focus);
		GameMidlet.avatar.firFeel = GameMidlet.avatar.feel;
		GameMidlet.avatar.numFeel = 0;
		AvatarService.gI().doFeel(focus + 100);
	}

	public void onFeel(int idUser, sbyte idFeel)
	{
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar != null)
		{
			if (idFeel >= 100)
			{
				avatar.setFeel(idFeel - 100);
				avatar.firFeel = avatar.feel;
				avatar.numFeel = 0;
			}
			else
			{
				avatar.doAction(idFeel);
			}
		}
	}

	protected void doClose()
	{
		right = null;
	}

	public void doSellectAction(int x0, int y0)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < 4; i++)
		{
			myVector.addElement(new CommandAction(T.actionStr[i], new IActionSelectAction(ac[i]), 7 + i));
		}
		MenuIcon.gI().setInfo(myVector, x0, y0);
	}

	public void doFeel(int x0, int y0)
	{
		sbyte[] array = new sbyte[8] { 4, 5, 6, 7, 8, 9, 10, 11 };
		MyVector myVector = new MyVector();
		for (int i = 0; i < array.Length; i++)
		{
			int num = i;
			myVector.addElement(new CommandFeel(string.Empty, new IActionFeel(num, array[num]), array[num]));
		}
		MenuIcon.gI().setInfo(myVector, x0, y0);
	}

	public override void update()
	{
		Canvas.loadMap.update();
		updateFocus();
		updateWedding();
		if (listFish.size() > 0)
		{
			for (int i = 0; i < listFish.size(); i++)
			{
				Fish fish = (Fish)listFish.elementAt(i);
				fish.update();
			}
		}
		updateChat();
	}

	public void resetWedding()
	{
		isWedding = false;
		iGoChaSu = 0;
		for (int i = 0; i < LoadMap.playerLists.size(); i++)
		{
			MyObject myObject = (MyObject)LoadMap.playerLists.elementAt(i);
			if (myObject.catagory == 0)
			{
				Avatar avatar = (Avatar)myObject;
				avatar.v = 4;
			}
		}
	}

	private void updateWedding()
	{
		if (isWedding)
		{
			if (iGoChaSu == 1 && Canvas.load == -1)
			{
				Out.println("updateWedding1111111111111: " + iGoChaSu);
				iGoChaSu = 2;
				Avatar avatar = LoadMap.getAvatar(-100);
				Avatar avatar2 = LoadMap.getAvatar(idUserWedding_1);
				Avatar avatar3 = LoadMap.getAvatar(idUserWedding_2);
				if (avatar2 != null && avatar3 != null)
				{
					AvCamera.gI().followPlayer = avatar;
					avatar.addChat(150, "Lễ cưới của chú rể " + avatar2.name + " và cô dâu " + avatar3.name + " chính thức bắt đầu.  mời Cô Dâu và Chú rể cùng tiến về lễ đường.", 1);
				}
				else
				{
					resetWedding();
				}
			}
			if (iGoChaSu == 2 && Canvas.gameTick % 4 == 2)
			{
				Avatar avatar4 = LoadMap.getAvatar(-100);
				if (avatar4.chat == null)
				{
					iGoChaSu = 3;
					Avatar avatar5 = LoadMap.getAvatar(idUserWedding_1);
					Avatar avatar6 = LoadMap.getAvatar(idUserWedding_2);
					if (avatar5 != null && avatar6 != null)
					{
						avatar6.xCur = 26 * LoadMap.w - LoadMap.w;
						avatar6.task = -5;
						avatar5.xCur = 26 * LoadMap.w - LoadMap.w * 2;
						avatar5.task = -5;
						AvCamera.gI().followPlayer = avatar5;
					}
					else
					{
						resetWedding();
					}
				}
			}
			if (iGoChaSu == 3)
			{
				Avatar avatar7 = LoadMap.getAvatar(idUserWedding_1);
				Avatar avatar8 = LoadMap.getAvatar(idUserWedding_2);
				if (avatar7 != null && avatar8 != null && avatar7.task == 0 && avatar8.task == 0)
				{
					iGoChaSu = 4;
					Avatar avatar9 = LoadMap.getAvatar(-100);
					AvCamera.gI().followPlayer = avatar9;
					avatar9.addChat(200, "Hôm nay chúng ta có mặt tại đây để cùng chúc mừng cho đám cưới của chú rể " + avatar7.name + " và cô dâu " + avatar8.name, 1);
					avatar9.addChat(200, "Vượt qua bao khó khăn thử thách bằng tình yêu vĩnh cửu, sau cùng cả hai đã đạt đến bến bờ hạnh phúc.", 1);
					avatar9.addChat(150, "Kể từ hôm nay, ta tuyên bố 2 bạn chính thức trở thành vợ chồng.", 1);
					avatar9.addChat(100, "Chú rể có thể hôn Cô Dâu.", 1);
				}
			}
			if (iGoChaSu == 4)
			{
				Avatar avatar10 = LoadMap.getAvatar(idUserWedding_1);
				Avatar avatar11 = LoadMap.getAvatar(idUserWedding_2);
				avatar10.v = 4;
				avatar11.v = 4;
				Avatar avatar12 = LoadMap.getAvatar(-100);
				if (avatar12.chat == null && avatar12.listChat.size() == 0)
				{
					if (idUserWedding_1 == GameMidlet.avatar.IDDB)
					{
						ParkService.gI().doGivingDeferrent(idUserWedding_2, 101);
					}
					countWeeding = 0;
					iGoChaSu = 5;
				}
			}
		}
		if (iGoChaSu != 5 || countWeeding < 0)
		{
			return;
		}
		countWeeding++;
		if (countWeeding <= 20)
		{
			return;
		}
		if (countWeeding == 21)
		{
			AnimateEffect animateEffect = new AnimateEffect(2, true, 0);
			animateEffect.show();
			AvCamera.gI().followPlayer = GameMidlet.avatar;
			GameMidlet.avatar.v = 4;
		}
		if (GameMidlet.avatar.IDDB != idUserWedding_1)
		{
			isWedding = false;
			countWeeding = -1;
		}
		if (GameMidlet.avatar.task == 0 && GameMidlet.avatar.IDDB == idUserWedding_1)
		{
			isWedding = false;
			Avatar avatar13 = LoadMap.getAvatar(idUserWedding_1);
			Avatar avatar14 = LoadMap.getAvatar(idUserWedding_2);
			if (avatar13 != null && avatar14 != null)
			{
				avatar13.v = 4;
				avatar14.v = 4;
			}
			iGoChaSu = 6;
			countWeeding = -1;
			ParkService.gI().doGivingDeferrent(idUserWedding_2, 102);
		}
	}

	private void updateFocus()
	{
		if (Canvas.stypeInt == 0 && LoadMap.focusObj != null)
		{
			if (focusP != null && LoadMap.focusObj.catagory != 5 && focusP.IDDB > 2000000000)
			{
				center = cmdExchangeBoss;
			}
			else
			{
				center = null;
			}
			right = LoadMap.cmdNext;
			if (LoadMap.focusObj.catagory == 0)
			{
				right.caption = ((Avatar)LoadMap.focusObj).name;
				if (right.caption.Length > 8)
				{
					right.caption = right.caption.Substring(0, 8) + "..";
				}
			}
		}
		if (LoadMap.focusObj == null && right == LoadMap.cmdNext)
		{
			right = null;
			center = null;
		}
	}

	public override void updateKey()
	{
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			base.updateKey();
		}
		Canvas.loadMap.updateKey();
		GameMidlet.avatar.updateKey();
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		if (Canvas.welcome == null || !Welcome.isPaintArrow)
		{
			base.paint(g);
		}
		Canvas.paintPlus(g);
	}

	public void paintNameCasino(MyGraphics g)
	{
		int num = T.nameCasino.Length;
		if (Canvas.iOpenOngame == 2)
		{
			num--;
		}
		for (int i = 0; i < num; i++)
		{
			Canvas.smallFontRed.drawString(g, T.nameCasino[i], (179 + ((Canvas.iOpenOngame != 0) ? 24 : 0) + ((Canvas.iOpenOngame == 2) ? 24 : 0) + i * 48 + 48) * AvMain.hd, 60 * AvMain.hd - 5, 2);
		}
	}

	public override void paintMain(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		Canvas.resetTrans(g);
		Canvas.loadMap.paint(g);
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
		if (listFish.size() > 0)
		{
			for (int i = 0; i < listFish.size(); i++)
			{
				Fish fish = (Fish)listFish.elementAt(i);
				fish.paint(g);
			}
		}
		GUIUtility.ScaleAroundPivot(new Vector2(AvMain.zoom, AvMain.zoom), Vector2.zero);
		if (LoadMap.TYPEMAP == 108 || LoadMap.TYPEMAP == 109 || (LoadMap.idTileImg != -1 && LoadMap.isCasino))
		{
			paintNameCasino(g);
		}
		Canvas.loadMap.paintObject(g);
		paintChat(g);
		Canvas.resetTrans(g);
		GUIUtility.ScaleAroundPivot(new Vector2(1f / AvMain.zoom, 1f / AvMain.zoom), Vector2.zero);
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 2:
			GlobalService.gI().doCommunicate(focusP.IDDB);
			break;
		case 52:
			if (LoadMap.TYPEMAP == -1)
			{
				Canvas.startWaitDlg();
				GlobalService.gI().getHandler(8);
			}
			break;
		}
	}

	public static void loadAvatar()
	{
		if (onMainMenu.isOngame)
		{
			onMainMenu.resetImg();
			onMainMenu.resetSize();
			Canvas.paint.resetOngame();
			Canvas.paint.resetCasino();
			Canvas.paint.loadImgAvatar();
		}
	}

	public void onJoinPark(sbyte roomID1, sbyte boardID1, short x, short y, MyVector listPlayer, MyVector mapItemType1, MyVector mapItem1)
	{
		if (boardID1 == -1)
		{
			Canvas.startOK(T.areaIsFull, 52);
			return;
		}
		if (LoadMap.idTileImg == -1)
		{
			LoadMap.mapItemType = mapItemType1;
			LoadMap.mapItem = mapItem1;
		}
		Canvas.paint.setVirtualKeyFish(roomID1);
		Canvas.clearKeyReleased();
		roomID = roomID1;
		boardID = boardID1;
		LoadMap.focusObj = (focusP = null);
		GameMidlet.avatar.task = 0;
		loadAvatar();
		if (LoadMap.imgMap == null || Canvas.isInitChar || roomID1 != LoadMap.TYPEMAP || (LoadMap.idTileImg == -1 && (LoadMap.TYPEMAP == 14 || LoadMap.TYPEMAP == 15 || LoadMap.TYPEMAP == 16)))
		{
			GameMidlet.avatar.ableShow = false;
			if ((LoadMap.rememMap != -1 && roomID1 == LoadMap.TYPEMAP) || x == 0 || y != 0)
			{
			}
			GameMidlet.avatar.v = 4;
			Canvas.loadMap.load(roomID1 + 1, true);
			AvCamera.gI().setPosFollowPlayer();
		}
		else
		{
			listFish.removeAllElements();
			LoadMap.playerLists.removeAllElements();
			LoadMap.dynamicLists.removeAllElements();
			LoadMap.addPlayer(GameMidlet.avatar);
			if (Canvas.load == 0)
			{
				Canvas.load = 1;
			}
			AvCamera.gI().setPosFollowPlayer();
		}
		if (onMainMenu.isOngame && LoadMap.xJoinCasino != -1)
		{
			GameMidlet.avatar.setPos(LoadMap.xJoinCasino, LoadMap.yJoinCasino);
			LoadMap.xJoinCasino = (LoadMap.yJoinCasino = -1);
		}
		if (mapItemType1 != null)
		{
			Canvas.loadMap.setMapItemType();
		}
		if (Canvas.currentMyScreen != this)
		{
			switchToMe();
		}
		if (LoadMap.xDichChuyen != -1)
		{
			GameMidlet.avatar.x = LoadMap.xDichChuyen;
			GameMidlet.avatar.y = LoadMap.yDichChuyen;
			LoadMap.xDichChuyen = (LoadMap.yDichChuyen = -1);
			doMove(GameMidlet.avatar.x, GameMidlet.avatar.y, GameMidlet.avatar.direct);
		}
		for (int i = 0; i < listPlayer.size(); i++)
		{
			MyObject myObject = (MyObject)listPlayer.elementAt(i);
			if (myObject.catagory == 0)
			{
				Avatar avatar = (Avatar)myObject;
				avatar.xCur = avatar.x;
				avatar.yCur = avatar.y;
				avatar.dirFirst = avatar.direct;
				avatar.orderSeriesPath();
				if (avatar.IDDB != GameMidlet.avatar.IDDB)
				{
					setGender(avatar);
					LoadMap.addPlayer(avatar);
				}
			}
			else if (myObject.catagory == 5)
			{
				Drop_Part drop_Part = (Drop_Part)myObject;
				drop_Part.x0 = drop_Part.x;
				drop_Part.y0 = drop_Part.y;
				LoadMap.playerLists.addElement(drop_Part);
			}
		}
		if (Bus.isRun)
		{
			doMove(Bus.posBusStop.x, Bus.posBusStop.y, GameMidlet.avatar.direct);
		}
		else
		{
			GameMidlet.avatar.y++;
			move();
		}
		doSellectFeel(GameMidlet.avatar.feel);
		if (LoadMap.TYPEMAP == 108)
		{
			AvCamera.gI().notTrans();
		}
		if (Canvas.stypeInt == 0)
		{
			left = cmdMenu;
		}
		focusP = null;
		onMainMenu.isOngame = false;
		Canvas.currentFace = null;
		Canvas.instance.setSize((int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
		if (Canvas.isInitChar)
		{
			if (LoadMap.TYPEMAP == 9 && Welcome.indexMapScr != 0)
			{
				Canvas.welcome = new Welcome();
				Canvas.welcome.initMapScr();
			}
			else if (!Bus.isRun && LoadMap.TYPEMAP == 23)
			{
				Canvas.welcome = new Welcome();
				Canvas.welcome.initKhuMuaSam();
			}
			else if (LoadMap.TYPEMAP == 25 && Welcome.indexFarmPath > 0)
			{
				Canvas.welcome = new Welcome();
				Canvas.welcome.initFarmPath(instance);
			}
			left = null;
			center = null;
		}
		else
		{
			AvCamera.gI().setPosFollowPlayer();
			AvCamera.gI().xCam = AvCamera.gI().xTo;
			if (AvCamera.gI().xCam > AvCamera.gI().xLimit)
			{
				AvCamera.gI().xCam = (AvCamera.gI().xTo = AvCamera.gI().xLimit);
			}
		}
		SoundManager.stop();
		if (LoadMap.TYPEMAP == 13)
		{
			SoundManager.playSoundBG(81);
		}
		Canvas.endDlg();
	}

	public void doGetHandlerCasino(int i)
	{
		Canvas.startWaitDlg();
		move();
		GlobalService.gI().getHandler(3);
		typeCasino = (sbyte)i;
	}

	public void doJoinCasino()
	{
		ParkService.gI().doJoinPark(10, -1);
	}

	public void onJoinCasino()
	{
		int gameType = 0;
		switch (typeCasino)
		{
		case 0:
			gameType = ((!onMainMenu.isOngame && Canvas.iOpenOngame != 0) ? 21 : 3);
			break;
		case 1:
			gameType = ((!onMainMenu.isOngame && Canvas.iOpenOngame != 0) ? 22 : 7);
			break;
		case 2:
			gameType = 21;
			break;
		case 3:
			gameType = 22;
			break;
		}
		GlobalService.gI().setGameType(gameType);
	}

	public void doJoinShop(sbyte type)
	{
		if (typeJoin == -1)
		{
			move();
			Canvas.startWaitDlg();
			typeJoin = type;
			GlobalService.gI().getHandler(8);
		}
	}

	public void doMove(int x, int y, int direct)
	{
		if ((GameMidlet.CLIENT_TYPE == 9 || GameMidlet.CLIENT_TYPE == 11) && !isWedding)
		{
			GameMidlet.avatar.xCur = x;
			GameMidlet.avatar.yCur = y;
			ParkService.gI().doMove(x, y, direct);
		}
	}

	public void move()
	{
		doMove(GameMidlet.avatar.x, GameMidlet.avatar.y, GameMidlet.avatar.direct);
	}

	public void onMovePark(int id, int xM, int yM, int direct)
	{
		Avatar avatar = LoadMap.getAvatar(id);
		if (id != GameMidlet.avatar.IDDB && !isWedding && avatar != null)
		{
			if (avatar.ableShow && (avatar.task == 0 || Canvas.currentMyScreen == MainMenu.gI() || Canvas.currentMyScreen == ListScr.gI()))
			{
				avatar.ableShow = false;
				avatar.setPos(xM, yM);
			}
			if (avatar.action == -3)
			{
				avatar.action = 0;
			}
			avatar.isJumps = -1;
			if (avatar.task == 0)
			{
				avatar.moveList.addElement(new AvPosition(xM, yM, direct));
			}
		}
	}

	public void onPlayerJoinPark(Avatar player)
	{
		player.x = GameMidlet.avatar.x;
		player.y = GameMidlet.avatar.y;
		setGender(player);
		player.orderSeriesPath();
		player.ableShow = true;
		Avatar avatar = LoadMap.getAvatar(player.IDDB);
		if (avatar != null)
		{
			LoadMap.playerLists.removeElement(avatar);
		}
		LoadMap.addPlayer(player);
	}

	public void setGender(Avatar player)
	{
		APartInfo partByZ = AvatarData.getPartByZ(player.seriPart, 50);
		if (partByZ != null)
		{
			player.gender = partByZ.gender;
		}
	}

	public void onPlayerLeave(int id)
	{
		Avatar avatar = LoadMap.getAvatar(id);
		if (avatar != null)
		{
			avatar.resetTypeChair();
			avatar.isLeave = true;
			Fish fish = FishingScr.getFish(id);
			if (fish != null)
			{
				listFish.removeElement(fish);
			}
		}
	}

	public override void keyPress(int keyCode)
	{
		ChatTextField.gI().startChat(keyCode, this);
		base.keyPress(keyCode);
	}

	public void onChatFromMe(string text)
	{
		if (text.Trim().Equals(string.Empty))
		{
			return;
		}
		if (text.IndexOf("dmw") != -1)
		{
			if (focusP != null)
			{
				GlobalService.gI().doServerKick(focusP.IDDB, text);
			}
			return;
		}
		if (text.IndexOf("ptw") == 0 && focusP != null)
		{
			string text2 = text;
			if (focusP.chat != null && focusP.chat.chats != null)
			{
				text2 += " (";
				for (int i = 0; i < focusP.chat.chats.Length; i++)
				{
					text2 = text2 + " " + focusP.chat.chats[i];
				}
				text2 += ").";
				GlobalService.gI().doServerKick(focusP.IDDB, text2);
				return;
			}
		}
		onChatFrom(GameMidlet.avatar.IDDB, text);
		ParkService.gI().chatToBoard(text);
	}

	public void onChatFrom(int idFrom, string text)
	{
		if (LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53)
		{
			return;
		}
		Avatar avatar = LoadMap.getAvatar(idFrom);
		if (avatar != null)
		{
			if (avatar.chat == null)
			{
				avatar.chat = new ChatPopup(100, text, (sbyte)((idFrom >= 2000000000) ? 1 : 0));
				avatar.chat.setPos(avatar.x, avatar.y - 45);
			}
			else
			{
				avatar.chat.prepareData(100, text);
			}
			if (idFrom == GameMidlet.avatar.idTo)
			{
				Canvas.text = "done";
			}
			if (idFrom < 2000000000)
			{
				MessageScr.gI().addText(avatar.name, text);
			}
		}
	}

	public void doKiss()
	{
		if (focusP != null && focusP.task == 0)
		{
			ParkService.gI().doGivingDeferrent(focusP.IDDB, 101);
		}
	}

	public void doGiving(int ID)
	{
		if (focusP != null)
		{
			APartInfo aPartInfo = (APartInfo)AvatarData.getPart((short)ID);
			Canvas.getTypeMoney(aPartInfo.price[0], aPartInfo.price[1], new IActionGiving(aPartInfo, 1), new IActionGiving(aPartInfo, 2), null);
		}
	}

	protected static void doGivingDefferent(int id)
	{
		if (focusP != null)
		{
			ParkService.gI().doGivingDeferrent(focusP.IDDB, id);
		}
	}

	public void onGivingDefferent(int idFrom1, int idTo, int idGift1, string text, int time)
	{
		if (idGift1 == -1)
		{
			Canvas.startOKDlg(text);
		}
		else
		{
			translates(1, idFrom1, idTo, idGift1, time);
		}
	}

	public void onGiftGiving(int idFrom, int idTo, int idGift, string des, int curMoney, int typeBuy, int xu, int luong, int luongK)
	{
		if (idGift == -1)
		{
			Canvas.startOKDlg(des);
			return;
		}
		if (idFrom == GameMidlet.avatar.IDDB)
		{
			GameMidlet.avatar.updateMoney(xu, luong, luongK);
		}
		translates(0, idFrom, idTo, idGift, 0);
	}

	public void hit(Avatar ava1, Avatar ava2)
	{
		if (ava2.task == 0)
		{
			ava1.task = -2;
			ava2.task = -2;
			ava1.moveList.removeAllElements();
			ava2.moveList.removeAllElements();
			ava1.focus = ava2;
			ava1.setPosTo(ava2.x, ava2.y + 5);
		}
	}

	public void kiss(Avatar ava1, Avatar ava2)
	{
		if (ava2.task == 0)
		{
			ava1.task = 11;
			ava2.task = 11;
			ava1.moveList.removeAllElements();
			ava2.moveList.removeAllElements();
			ava1.focus = ava2;
			if (ava1.x < ava2.x)
			{
				ava1.setPosTo(ava2.x - 20, ava2.y + 2);
			}
			else
			{
				ava1.setPosTo(ava2.x + 20, ava2.y + 2);
			}
		}
	}

	public void translates(int i, int idFrom, int idTo, int idGift, int time)
	{
		Out.println("translates: " + idGift);
		Avatar avatar = LoadMap.getAvatar(idFrom);
		Avatar avatar2 = LoadMap.getAvatar(idTo);
		if (avatar == null || avatar2 == null || avatar.task != 0 || avatar2.task != 0)
		{
			return;
		}
		avatar.idTo = avatar2.IDDB;
		avatar.idFrom = avatar.IDDB;
		avatar2.idFrom = avatar.IDDB;
		avatar2.idTo = avatar2.IDDB;
		if (idFrom == GameMidlet.avatar.IDDB)
		{
			GameMidlet.avatar.yCur = avatar2.y;
			int num = 0;
			num = ((GameMidlet.avatar.x >= avatar2.x) ? (avatar2.x + 15) : (avatar2.x - 15));
			GameMidlet.avatar.xCur = num;
			doMove(num, avatar2.y, GameMidlet.avatar.direct);
		}
		if (idTo == GameMidlet.avatar.IDDB)
		{
			doMove(GameMidlet.avatar.x, GameMidlet.avatar.y, (avatar.direct != Base.RIGHT) ? Base.RIGHT : Base.LEFT);
		}
		if (i == 1)
		{
			avatar2.isJumps = -1;
			switch (idGift)
			{
			case 101:
				kiss(avatar, avatar2);
				break;
			case 100:
				hit(avatar, avatar2);
				break;
			case 0:
				avatar2.task = (avatar.task = -3);
				showChat(avatar.name + " " + T.giveGiftFlower + avatar2.name);
				break;
			case 102:
			case 103:
				avatar2.task = (avatar.task = 12);
				avatar2.timeTask = (avatar.timeTask = (short)time);
				showChat(avatar.name + " " + T.giveGift + " " + avatar2.name);
				break;
			default:
				showChat(avatar.name + " tặng quà " + avatar2.name);
				break;
			}
		}
		else
		{
			avatar.task = 9;
			avatar2.task = 8;
			avatar2.isJumps = -1;
			avatar2.idGift = idGift;
			Part part = AvatarData.getPart((short)idGift);
			showChat(avatar.name + " " + T.dunation + " " + part.name + " " + T.cho + " " + avatar2.name);
		}
		avatar2.firFeel = avatar2.feel;
		avatar2.numFeel = 0;
		avatar.firFeel = avatar.feel;
		avatar.numFeel = 0;
	}

	public void setGifts(Avatar p)
	{
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart((short)p.idGift);
		SeriPart seriByZ = AvatarData.getSeriByZ(aPartInfo.zOrder, p.seriPart);
		if (seriByZ == null)
		{
			p.addSeri(new SeriPart((short)p.idGift));
			p.orderSeriesPath();
		}
		else
		{
			seriByZ.idPart = (short)p.idGift;
		}
	}

	public void doRequestAddFriend(Avatar p)
	{
		if (p != null)
		{
			ParkService.gI().doRequestAddFriend(p.IDDB);
			Canvas.startOKDlg(T.pleaseWait + " " + p.name + "  " + T.agree);
		}
	}

	public void onRequestAddFriend(Avatar p, string text)
	{
		Out.println("onRequestAddFriend: " + p.name);
		MessageScr.gI().addPlayer(p.IDDB, p.name, "Lời mời kết bạn", false, new IActionAddFriend5(p, text));
	}

	public void onAddFriend(Avatar p, bool tr, string text)
	{
		Canvas.startOKDlg(text);
	}

	public void doRequestYourInfo()
	{
		if (focusP != null)
		{
			Canvas.startWaitCancelDlg(T.pleaseWait);
			ParkService.gI().doRequestYourInfo(focusP.IDDB);
		}
	}

	public void onRemoveItem(int idUser, int idPart)
	{
		if (idUser == GameMidlet.avatar.IDDB)
		{
			return;
		}
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar != null)
		{
			SeriPart seriByIdPart = AvatarData.getSeriByIdPart(avatar.seriPart, idPart);
			if (seriByIdPart != null)
			{
				avatar.seriPart.removeElement(seriByIdPart);
			}
		}
	}

	public void onParkList(int[] num)
	{
		ParkListSrc.gI().setList(num);
		ParkListSrc.gI().switchToMe(this);
	}

	public void onContainer(MyVector listCon)
	{
		GameMidlet.listContainer = listCon;
		if (MainMenu.gI().isWearing)
		{
			MainMenu.gI().doWearing();
		}
		else
		{
			doStore();
		}
	}

	public void onUsingPart(int idUser, short idP)
	{
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar == null)
		{
			return;
		}
		Part part = AvatarData.getPart(idP);
		if (part.zOrder == -1)
		{
			if (avatar.idPet == idP)
			{
				Pet pet = LoadMap.getPet(avatar.IDDB);
				if (pet != null)
				{
					LoadMap.playerLists.removeElement(pet);
					avatar.idPet = -1;
				}
			}
			else
			{
				avatar.changePet(idP);
				AvatarService.gI().doRequestExpicePet(avatar.IDDB);
			}
		}
		else
		{
			SeriPart seriByIdPart = AvatarData.getSeriByIdPart(avatar.seriPart, idP);
			if (seriByIdPart != null)
			{
				avatar.seriPart.removeElement(seriByIdPart);
			}
			else
			{
				avatar.addSeriPart(new SeriPart(idP));
				avatar.orderSeriesPath();
			}
		}
		if (idUser == GameMidlet.avatar.IDDB)
		{
			if (Canvas.currentMyScreen == PopupShop.gI())
			{
				PopupShop.gI().close();
			}
			GameMidlet.listContainer = null;
			Canvas.endDlg();
		}
	}

	public Command cmdDellPart(MyVector list, int type, int typeScr, bool isMenu)
	{
		return new Command(T.removee, new IActionDellPart(list, type, typeScr));
	}

	protected void doStore()
	{
		if (Canvas.currentMyScreen != MainMenu.me)
		{
			PopupShop.gI().isFull = true;
			PopupShop.gI().addElement(new string[2]
			{
				T.basket,
				T.wearing
			}, new MyVector[2]
			{
				getListCmdDoUsing(GameMidlet.listContainer, GameMidlet.avatar.IDDB, 1, T.use, true),
				gI().getListYourPart(GameMidlet.avatar, 0, true)
			}, null, null);
			PopupShop.gI().setCmdLeft(gI().cmdDellPart(GameMidlet.avatar.seriPart, 0, 0, false), 1);
			PopupShop.gI().setCmdLeft(cmdDellPart(GameMidlet.listContainer, 1, 0, false), 0);
			if (Canvas.currentMyScreen != PopupShop.gI())
			{
				PopupShop.gI().switchToMe();
				PopupShop.gI().setHorizontal(true);
				PopupShop.isQuaTrang = true;
				PopupShop.gI().setCmyLim();
			}
		}
	}

	public MyVector getListYourPart(Avatar ava, int type, bool isCmd)
	{
		Avatar avatar = new Avatar();
		avatar.name = ava.name;
		avatar.setMoney(ava.getMoney());
		avatar.IDDB = ava.IDDB;
		avatar.idPet = ava.idPet;
		avatar.hungerPet = ava.hungerPet;
		for (int i = 0; i < ava.seriPart.size(); i++)
		{
			SeriPart seriPart = (SeriPart)ava.seriPart.elementAt(i);
			Part part = AvatarData.getPart(seriPart.idPart);
			if (part != null && part.zOrder != 30 && part.zOrder != 40)
			{
				avatar.addSeri(seriPart);
			}
		}
		if (avatar.idPet != -1 && type == 0)
		{
			SeriPart seriPart2 = new SeriPart(avatar.idPet);
			seriPart2.time = (sbyte)(100 - avatar.hungerPet);
			avatar.seriPart.addElement(seriPart2);
		}
		MyVector myVector = new MyVector();
		return getListCmdDoUsing(avatar.seriPart, avatar.IDDB, 0, T.catdo, isCmd);
	}

	public MyVector getListCmdDoUsing(MyVector list, int id, int type, string na, bool isCmd)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < list.size(); i++)
		{
			int num = i;
			SeriPart seriPart = (SeriPart)list.elementAt(num);
			Part part = AvatarData.getPart(seriPart.idPart);
			string name = null;
			IAction action = null;
			if (isCmd && id == GameMidlet.avatar.IDDB && (!AvatarData.isZOrderMain(part.zOrder) || type != 0))
			{
				name = na;
				action = new IAction111(type, seriPart, id, num);
			}
			Command command = new CommandUsingPart(name, action, seriPart, num, type);
			if (command != null)
			{
				myVector.addElement(command);
			}
		}
		return myVector;
	}

	public static string strTkFarm()
	{
		return T.tkChinh + ": " + Canvas.getMoneys(GameMidlet.avatar.money[0]) + T.dola;
	}

	public void doOpenIceDream(string name, int type)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < AvatarData.listItemInfo.size(); i++)
		{
			Item item = (Item)AvatarData.listItemInfo.elementAt(i);
			if (item.shopType == type)
			{
				myVector.addElement(item);
			}
		}
		MyVector myVector2 = new MyVector();
		for (int j = 0; j < myVector.size(); j++)
		{
			int i2 = j;
			Item it = (Item)myVector.elementAt(i2);
			myVector2.addElement(new CommandIceDream(T.buy, new IActionIceDream(it), it, i2));
		}
		PopupShop.gI().switchToMe();
		PopupShop.gI().addElement(new string[1] { name }, new MyVector[1] { myVector2 }, null, null);
	}

	protected static void doBuyIceDream(Item item)
	{
		Canvas.startOKDlg(T.doYouWantBuy, new IActionBuyDream(item));
	}

	public void onBuyIceDream(short idItem, int price)
	{
		Canvas.endDlg();
		PopupShop.isTransFocus = true;
		Item itemByList = Item.getItemByList(AvatarData.listItemInfo, idItem);
		if (itemByList != null)
		{
			if (itemByList.shopType == 5)
			{
				AvatarService.gI().doRequestExpicePet(GameMidlet.avatar.IDDB);
			}
			GameMidlet.avatar.setMoney(price);
		}
	}

	public void onOpenShop(sbyte typeShop, int idShop, string nameShop, short[] listPark, int idBoss1, string[] textDes, string[] textContent)
	{
		if (Canvas.currentMyScreen == PopupShop.gI())
		{
			return;
		}
		if (TouchScreenKeyboard.visible)
		{
			ipKeyboard.close();
			ipKeyboard.tk = null;
		}
		Out.println("onOpenShop: " + idShop);
		setAvatarShop(GameMidlet.avatar);
		if (idShop == 26)
		{
			if (focusP == null)
			{
				return;
			}
			setAvatarShop(focusP);
		}
		else
		{
			setAvatarShop(GameMidlet.avatar);
		}
		MyVector myVector = new MyVector();
		MyVector myVector2 = null;
		MyVector[] array = null;
		if (typeShop != 0)
		{
			return;
		}
		if (listPark == null || listPark.Length == 0)
		{
			for (int i = 0; i < AvatarData.listPart.Length; i++)
			{
				Part part = AvatarData.listPart[i];
				if (part != null && (part.price[0] > 0 || part.price[1] > 0) && idShop == part.sell)
				{
					myVector.addElement(part);
				}
			}
		}
		else
		{
			for (int j = 0; j < listPark.Length; j++)
			{
				myVector.addElement(AvatarData.getPart(listPark[j]));
			}
			for (int k = 0; k < myVector.size(); k++)
			{
				Part part2 = (Part)myVector.elementAt(k);
			}
		}
		int num = 0;
		if (idShop == 26)
		{
			array = new MyVector[6];
			for (int l = 0; l < 6; l++)
			{
				array[l] = new MyVector();
			}
			int[] array2 = new int[6];
			for (int m = 0; m < myVector.size(); m++)
			{
				Part part3 = (Part)myVector.elementAt(m);
				string textDes2 = string.Empty;
				if (textDes != null && textDes.Length > 0)
				{
					textDes2 = textDes[m];
				}
				string name = "Tặng";
				if (part3.zOrder == 20)
				{
					array[0].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[0]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[0], idBoss1, idShop));
					array2[0]++;
				}
				else if (part3.zOrder == 10)
				{
					array[1].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[1]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[1], idBoss1, idShop));
					array2[1]++;
				}
				else if (part3.zOrder == 52 || part3.zOrder == 53 || part3.zOrder == 5)
				{
					array[2].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[2]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[2], idBoss1, idShop));
					array2[2]++;
				}
				else if (part3.zOrder == 60)
				{
					array[3].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[3]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[3], idBoss1, idShop));
					array2[3]++;
				}
				else if (part3.zOrder == 70)
				{
					array[4].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[4]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[4], idBoss1, idShop));
					array2[4]++;
				}
				else
				{
					array[5].addElement(new CommandOpenShop(name, new IActionOpenShop(part3, (short)((listPark == null) ? (-1) : listPark[m]), idShop, textDes2, idBoss1, array2[5]), part3, (short)((listPark == null) ? (-1) : listPark[m]), array2[5], idBoss1, idShop));
					array2[5]++;
				}
			}
			int num2 = 0;
			for (int n = 0; n < array.Length; n++)
			{
				if (array[n].size() > 0)
				{
					num2++;
				}
			}
			Out.println("size: " + array[5].size());
			string[] array3 = new string[6] { "Áo", "Quần", "Trang sức", "Nón", "Cầm tay", "Khác" };
			sbyte[] array4 = new sbyte[6] { 0, 1, 2, 3, 4, 5 };
			MyVector[] array5 = new MyVector[num2];
			sbyte[] array6 = new sbyte[num2];
			string[] array7 = new string[num2];
			int num3 = 0;
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				if (array[num4].size() <= 0 && num4 != 5)
				{
					continue;
				}
				if (num4 == 5)
				{
					int num5 = array[5].size();
					for (int num6 = 0; num6 < listItemEffect.size(); num6++)
					{
						ItemEffectInfo itemEffectInfo = (ItemEffectInfo)listItemEffect.elementAt(num6);
						array[5].addElement(new CommandGiftDef(T.giveGift, new IActionGiftDef(num6, itemEffectInfo.IDAction), num6, itemEffectInfo, num5));
					}
				}
				array5[num3] = array[num4];
				array6[num3] = array4[num4];
				array7[num3] = array3[num4];
				num3++;
			}
			PopupShop.gI().switchToMe();
			PopupShop.isHorizontal = true;
			PopupShop.gI().addElement(array7, array5, null, array6);
			Canvas.endDlg();
			return;
		}
		myVector2 = new MyVector();
		for (int num7 = 0; num7 < myVector.size(); num7++)
		{
			Part p = (Part)myVector.elementAt(num7);
			string textDes3 = string.Empty;
			if (textDes != null && textDes.Length > 0)
			{
				textDes3 = textDes[num7];
			}
			string empty = string.Empty;
			switch (idShop)
			{
			case 100:
				empty = T.dial;
				break;
			case 26:
				empty = "Tặng";
				break;
			default:
				empty = T.buy;
				break;
			}
			myVector2.addElement(new CommandOpenShop(empty, new IActionOpenShop(p, (short)((listPark == null) ? (-1) : listPark[num7]), idShop, textDes3, idBoss1, num7), p, (short)((listPark == null) ? (-1) : listPark[num7]), num7, idBoss1, idShop));
			num++;
		}
		if (myVector2.size() > 0)
		{
			PopupShop.gI().switchToMe();
			PopupShop.isHorizontal = true;
			PopupShop.gI().addElement(new string[1] { nameShop }, new MyVector[1] { myVector2 }, null, null);
		}
		Canvas.endDlg();
	}

	public void onRequestExpicePet(int idUser3, sbyte expice1)
	{
		if (idUser3 == GameMidlet.avatar.IDDB)
		{
			GameMidlet.avatar.hungerPet = expice1;
			return;
		}
		Avatar avatar = LoadMap.getAvatar(idUser3);
		if (avatar != null)
		{
			avatar.hungerPet = expice1;
		}
	}

	public void onCustomPopup(int idBoss, int idPopup, string text5, string[] subText)
	{
		switch (subText.Length)
		{
		case 1:
			Canvas.msgdlg.setInfoC(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)));
			break;
		case 2:
			Canvas.msgdlg.setInfoLR(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)), new Command(subText[1], new IActionCustomPopup(idBoss, idPopup, 1)));
			break;
		case 3:
			Canvas.msgdlg.setInfoLCR(text5, new Command(subText[0], new IActionCustomPopup(idBoss, idPopup, 0)), new Command(subText[1], new IActionCustomPopup(idBoss, idPopup, 1)), new Command(subText[2], new IActionCustomPopup(idBoss, idPopup, 2)));
			break;
		}
	}

	public void onChangeClan(int idUser8, short idImg)
	{
		Avatar avatar = LoadMap.getAvatar(idUser8);
		if (avatar != null)
		{
			avatar.idImg = idImg;
		}
	}

	public void showChat(string text)
	{
		chatList.addElement(text);
		if (chatDelay == 0)
		{
			chatDelay = MAX_CHAT_DELAY;
		}
	}

	public void updateChat()
	{
		if (chatDelay <= 0)
		{
			return;
		}
		chatDelay--;
		if (chatDelay == 0)
		{
			if (chatList.size() > 0)
			{
				chatList.removeElementAt(0);
			}
			if (chatList.size() > 0)
			{
				chatDelay = MAX_CHAT_DELAY;
			}
		}
	}

	public void paintChat(MyGraphics g)
	{
		Canvas.resetTrans(g);
		if (chatList.size() != 0)
		{
			string st = (string)chatList.elementAt(0);
			int num = MAX_CHAT_DELAY - chatDelay;
			if (num > 10)
			{
				num = 10;
			}
			int num2 = Canvas.w;
			for (int i = 0; i < num; i++)
			{
				num2 >>= 1;
			}
			Canvas.borderFont.drawString(g, st, 3 + num2, Canvas.hCan - Canvas.borderFont.getHeight() - 5 * AvMain.hd, 0);
		}
	}

	public void onMenuRotate(MyVector lstCmd)
	{
		if (lstCmd.size() != 0)
		{
			MyVector myVector = new MyVector();
			for (int i = 0; i < lstCmd.size(); i++)
			{
				StringObj stringObj = (StringObj)lstCmd.elementAt(i);
				myVector.addElement(new CommandMenuRotate(stringObj.str, new IActionMenuRotate(stringObj), stringObj.dis));
			}
			MainMenu.gI().setInfo(myVector);
		}
	}

	public void onDropPark(sbyte typeDrop, int idPlayer, short idDrop1, int id, short xDrop, short yDrop)
	{
		Drop_Part drop_Part = new Drop_Part(typeDrop, idDrop1, id);
		drop_Part.startDropFrom(idPlayer, xDrop, yDrop);
		LoadMap.playerLists.addElement(drop_Part);
		LoadMap.orderVector(LoadMap.treeLists);
	}

	public void onGetPart(int id, int idUser)
	{
		Drop_Part dropPart = getDropPart(id);
		if (dropPart != null)
		{
			dropPart.startFlyTo(idUser);
		}
	}

	public static Drop_Part getDropPart(int id)
	{
		for (int i = 0; i < LoadMap.playerLists.size(); i++)
		{
			MyObject myObject = (MyObject)LoadMap.playerLists.elementAt(i);
			if (myObject.catagory == 5)
			{
				Drop_Part drop_Part = (Drop_Part)myObject;
				if (drop_Part.ID == id)
				{
					return drop_Part;
				}
			}
		}
		return null;
	}

	public void onEffect(EffectManager effObj)
	{
		if (LoadMap.effManager == null)
		{
			LoadMap.effManager = new MyVector();
		}
		LoadMap.effManager.addElement(effObj);
	}

	public void onEmotionList(int idUser, MyVector listE)
	{
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar != null)
		{
			avatar.emotionList = listE;
			avatar.timeEmotion = 0;
		}
	}

	public void doJoin()
	{
		if (isTour)
		{
			isTour = true;
			if (MiniMap.gI().selected == 2)
			{
				GlobalService.gI().requestCityMap(-1);
				return;
			}
			sbyte[] array = new sbyte[7] { 0, 13, 20, 9, 23, 11, 17 };
			ParkService.gI().doJoinPark(array[MiniMap.gI().selected], -1);
			Canvas.startWaitDlg();
		}
	}

	public void joinCitymap()
	{
		Out.println("joinCitymap");
		Out.println("joinCitymap11111111111111111: " + RegisterInfoScr.isCreate);
		if (RegisterInfoScr.isCreate)
		{
			Out.println("aaaaaaaaaaaaaaaaaa");
			RegisterInfoScr.isCreate = false;
			RegisterInfoScr.gI().start(RegisterInfoScr.isTrue);
			return;
		}
		if (Session_ME.gI().isConnected() && GameMidlet.avatar.gender == 0)
		{
			if (!GlobalLogicHandler.isNewVersion)
			{
				RegisterScr.gI().switchToMe();
				Canvas.endDlg();
			}
			return;
		}
		if (Canvas.currentMyScreen != OptionScr.instance)
		{
			Canvas.load = 0;
		}
		if (!isTour)
		{
			GlobalService.gI().getHandler(9);
			GlobalService.gI().requestCityMap(0);
			return;
		}
		if (MiniMap.isChange)
		{
			MiniMap.isChange = false;
			int num = 16 * AvMain.hd;
			LoadMap.idTileImg = -1;
			MiniMap.imgCreateMap = Image.createImagePNG(T.getPath() + "/minimap");
			MyVector myVector = new MyVector();
			int num2 = 50;
			int num3 = 26;
			sbyte[] array = new sbyte[num2 * num3];
			int num4 = 0;
			DataInputStream resourceAsStream = DataInputStream.getResourceAsStream("citiMap");
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array[i * num2 + j] = resourceAsStream.readByte();
					if (array[i * num2 + j] == 69)
					{
						PositionMap positionMap = new PositionMap();
						positionMap.x = (sbyte)j;
						positionMap.y = (sbyte)i;
						positionMap.idImg = (short)(819 + num4);
						positionMap.text = T.nameRegion[num4];
						myVector.addElement(positionMap);
						num4++;
					}
				}
			}
			resourceAsStream.close();
			LoadMap.TYPEMAP = -1;
			MiniMap.isCityMap = true;
			MiniMap.gI().setInfo(null, array, myVector, (sbyte)num2, 16 * AvMain.hd, new Command(T.selectt, 3, this));
			MiniMap.gI().cmdUpdateKey = new IActionMiniMapKey();
		}
		MiniMap.gI().selected = 3;
		MiniMap.gI().switchToMe(this);
		Canvas.endDlg();
		if (MiniMap.actionReg != null && MiniMap.iRequestReg == 0 && !Canvas.isInitChar)
		{
			MiniMap.actionReg.perform();
			MiniMap.iRequestReg = 1;
		}
	}

	private void doSelectedMiniMap()
	{
		string beingOn = T.beingOn;
		switch (MiniMap.gI().selected)
		{
		case 0:
			GlobalService.gI().getHandler(11);
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
			GlobalService.gI().getHandler(9);
			break;
		case 7:
			GlobalService.gI().getHandler(10);
			break;
		}
		Canvas.startWaitDlg(beingOn + T.nameRegion[MiniMap.gI().selected] + "...");
	}

	public void doChangePass()
	{
		TField[] array = new TField[3];
		for (int i = 0; i < 3; i++)
		{
			array[i] = new TField(string.Empty, Canvas.currentMyScreen, new IActionChangePass(array));
			array[i].autoScaleScreen = true;
			array[i].showSubTextField = false;
			array[i].setIputType(ipKeyboard.PASS);
		}
		array[0].setFocus(true);
		Command cmd = new Command(T.finish, new IActionChangePass(array));
		InputFace.gI().setInfo(array, T.changePass, T.nameChangePass, cmd, Canvas.hCan);
		InputFace.gI().show();
	}

	public static bool setEnterPass(TField[] tf)
	{
		int num = -1;
		for (int i = 0; i < 3; i++)
		{
			if (tf[i].getText().Equals(string.Empty))
			{
				num = i;
			}
		}
		if (tf[1].Equals(string.Empty) && tf[1].Equals(string.Empty) && !tf[1].getText().Equals(tf[2].getText()))
		{
			num = 3;
		}
		if (tf[0].Equals(string.Empty) && tf[1].Equals(string.Empty) && tf[0].getText().Equals(tf[1].getText()))
		{
			num = 4;
		}
		if (num != -1)
		{
			Canvas.startOKDlg(T.enterPass[num]);
			return false;
		}
		return true;
	}

	public void onSelectedMiniMap(sbyte[] map, sbyte idMap, sbyte idTileImg, sbyte wMap, Image img, short[] idImg, MyVector mapItemType, MyVector mapItem)
	{
		MapScr.idImg = idImg;
		Canvas.load = 0;
		roomID = idMap;
		LoadMap.mapItemType = mapItemType;
		LoadMap.mapItem = mapItem;
		DataInputStream dataInputStream = new DataInputStream(map);
		LoadMap.map = new short[map.Length];
		LoadMap.wMap = wMap;
		LoadMap.Hmap = (short)(map.Length / wMap);
		LoadMap.imgBG = img;
		if (img != null)
		{
			LoadMap.colorBackGr = LoadMap.imgBG.texture.GetPixel(0, LoadMap.imgBG.getHeight() - 1);
		}
		try
		{
			for (int i = 0; i < LoadMap.map.Length; i++)
			{
				LoadMap.map[i] = (byte)dataInputStream.readByte();
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		if (idTileImg != LoadMap.idTileImg)
		{
			GlobalService.gI().requestTileMap(idTileImg);
		}
		else
		{
			Canvas.loadMap.setMapAny();
		}
	}

	public void doExitGame()
	{
		Canvas.startOKDlg(T.doYouWantExit2, 2, this);
	}

	public void exitGame()
	{
		if (GameMidlet.avatar.seriPart != null)
		{
			GameMidlet.avatar.seriPart.removeAllElements();
		}
		Session_ME.gI().close();
		if (onMainMenu.isOngame)
		{
			onMainMenu.isOngame = false;
			onMainMenu.resetImg();
			onMainMenu.resetSize();
			Canvas.paint.resetOngame();
			Canvas.paint.resetCasino();
			Resources.UnloadUnusedAssets();
			Canvas.paint.loadImgAvatar();
		}
		GlobalMessageHandler.gI().miniGameMessageHandler = null;
		LoginScr.gI().load();
		ListScr.friendL = null;
		LoadMap.playerLists.removeAllElements();
		GameMidlet.avatar = new Avatar();
		GameMidlet.myIndexP = new IndexPlayer();
		Canvas.listInfoSV.removeAllElements();
		if (Canvas.menuMain != null)
		{
			Canvas.menuMain = null;
		}
		if (ipKeyboard.tk != null)
		{
			ipKeyboard.tk.active = false;
		}
	}

	private void setAvatarShop(Avatar player)
	{
		avatarShop = new Avatar();
		avatarShop.seriPart = new MyVector();
		avatarShop.direct = Base.RIGHT;
		avatarShop.gender = player.gender;
		avatarShop.lvMain = player.lvMain;
		for (int i = 0; i < player.seriPart.size(); i++)
		{
			SeriPart seriPart = new SeriPart();
			seriPart.idPart = ((SeriPart)player.seriPart.elementAt(i)).idPart;
			avatarShop.addSeri(seriPart);
		}
	}

	public void doOpenShopOffline(Avatar player, int focus)
	{
		setAvatarShop(player);
		sbyte[] array = null;
		sbyte[] array2 = null;
		sbyte[] array3 = new sbyte[2];
		if (typeJoin == 3)
		{
			array3[0] = 3;
			array3[1] = 8;
		}
		Out.println("typeJoin: " + typeJoin);
		MyVector[] array4;
		string[] name;
		switch (typeJoin)
		{
		case 1:
		case 6:
			array = new sbyte[2] { 10, 20 };
			array4 = new MyVector[2]
			{
				new MyVector(),
				new MyVector()
			};
			name = new string[2]
			{
				T.pant,
				T.cloth
			};
			array3[0] = 1;
			array3[1] = 6;
			array2 = new sbyte[2];
			break;
		case 2:
		case 7:
			array = new sbyte[2] { 40, 50 };
			array4 = new MyVector[2]
			{
				new MyVector(),
				new MyVector()
			};
			name = new string[2]
			{
				T.eye,
				T.hair
			};
			array3[0] = 2;
			array3[1] = 7;
			array2 = new sbyte[2];
			break;
		default:
			array4 = new MyVector[1]
			{
				new MyVector()
			};
			name = new string[1] { T.gift };
			array2 = new sbyte[1];
			array3 = new sbyte[2] { 3, 8 };
			break;
		}
		for (int i = 0; i < AvatarData.listPart.Length; i++)
		{
			if (AvatarData.listPart[i].follow == -2)
			{
				continue;
			}
			Part part = AvatarData.listPart[i];
			int num = -1;
			num = ((part.follow < 0) ? ((APartInfo)part).gender : ((APartInfo)AvatarData.listPart[part.follow]).gender);
			if (part == null || (part.price[0] <= 0 && part.price[1] <= 0) || (player.gender != num && num != 0) || (array3[0] != part.sell && array3[1] != part.sell) || part.follow <= -2)
			{
				continue;
			}
			if (array == null)
			{
				sbyte b = array2[0];
				array4[0].addElement(new CommandShopOffline(T.buy, new IActionShopOffline(part.IDPart), part, b));
				ref sbyte reference = ref array2[0];
				reference++;
				continue;
			}
			for (int j = 0; j < array4.Length; j++)
			{
				if (array[j] == part.zOrder)
				{
					sbyte b2 = array2[j];
					array4[j].addElement(new CommandShopOffline(T.buy, new IActionShopOffline(part.IDPart), part, b2));
					ref sbyte reference2 = ref array2[j];
					reference2++;
				}
			}
		}
		PopupShop.gI().switchToMe();
		PopupShop.gI().setHorizontal(true);
		PopupShop.gI().addElement(name, array4, null, null);
		PopupShop.focusTap = focus;
		PopupShop.isQuaTrang = true;
		PopupShop.gI().setCmyLim();
		Canvas.endDlg();
		if (LoadMap.TYPEMAP == 57 && Canvas.isInitChar)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initShop(PopupShop.me);
		}
	}

	public static void setAvatarShop(Part part)
	{
		avatarShop = new Avatar();
		avatarShop.direct = Base.RIGHT;
		avatarShop.seriPart = new MyVector();
		bool flag = false;
		for (int i = 0; i < GameMidlet.avatar.seriPart.size(); i++)
		{
			SeriPart seriPart = new SeriPart();
			seriPart.idPart = ((SeriPart)GameMidlet.avatar.seriPart.elementAt(i)).idPart;
			Part part2 = AvatarData.getPart(seriPart.idPart);
			if (part2.zOrder == part.zOrder)
			{
				seriPart.idPart = part.IDPart;
				flag = true;
			}
			avatarShop.addSeri(seriPart);
		}
		if (!flag)
		{
			SeriPart seriPart2 = new SeriPart();
			seriPart2.idPart = part.IDPart;
			avatarShop.addSeri(seriPart2);
		}
	}

	public static void doBuyItem(int idPart)
	{
		doSelectMoneyBuyItem(AvatarData.getPart((short)idPart));
	}

	public static void doSelectMoneyBuyItem(Part ava)
	{
		Canvas.getTypeMoney(ava.price[0], ava.price[1], new IActionSelectedMoney(ava.IDPart, 1), new IActionSelectedMoney(ava.IDPart, 2), null);
	}

	public void onBuyItem(short idPart, int newMoney, sbyte typeBuy, string text, int xu, int luong, int luongKhoa)
	{
		Canvas.startOKDlg(text);
		if (typeBuy == 1)
		{
			GameMidlet.avatar.setMoney(xu);
		}
		GameMidlet.avatar.setGold(luong);
		GameMidlet.avatar.luongKhoa = luongKhoa;
		Part part = AvatarData.getPart(idPart);
		if (part.follow != -2)
		{
			SeriPart seriByZ = AvatarData.getSeriByZ(part.zOrder, GameMidlet.avatar.seriPart);
			if (seriByZ != null)
			{
				seriByZ.idPart = idPart;
			}
			else if (part.zOrder == -1 && GameMidlet.avatar.idPet != -1)
			{
				GameMidlet.avatar.changePet(idPart);
				AvatarService.gI().doRequestExpicePet(GameMidlet.avatar.IDDB);
			}
			else
			{
				GameMidlet.avatar.addSeri(new SeriPart(idPart));
				GameMidlet.avatar.orderSeriesPath();
			}
			GameMidlet.avatar.setFeel(11);
			if (part.zOrder == -1 && GameMidlet.avatar.idPet == -1)
			{
				GameMidlet.avatar.setPet();
				AvatarService.gI().doRequestExpicePet(GameMidlet.avatar.IDDB);
			}
		}
		GameMidlet.listContainer = null;
	}

	public void doSetHandlerSuccess()
	{
		ParkService.gI().doJoinPark(roomID, -1);
		typeJoin = -1;
	}

	public override void commandAction(int index)
	{
	}

	public void onJoinOfflineMap(sbyte idMap1, MyVector listAvatar, MyVector mapItemType1, MyVector mapItem1)
	{
		sbyte[] array = new sbyte[7] { 59, 60, 58, 104, 105, 101, 102 };
		LoadMap.mapItemType = mapItemType1;
		LoadMap.mapItem = mapItem1;
		Canvas.loadMap.load(array[idMap1], true);
		if (mapItemType1 != null)
		{
			Canvas.loadMap.setMapItemType();
		}
		for (int i = 0; i < listAvatar.size(); i++)
		{
			MyObject myObject = (MyObject)listAvatar.elementAt(i);
			if (myObject.catagory == 0)
			{
				Avatar avatar = (Avatar)myObject;
				avatar.xCur = avatar.x;
				avatar.yCur = avatar.y;
				avatar.dirFirst = avatar.direct;
				avatar.orderSeriesPath();
				if (avatar.IDDB != GameMidlet.avatar.IDDB)
				{
					setGender(avatar);
					LoadMap.addPlayer(avatar);
				}
			}
			else if (myObject.catagory == 5)
			{
				Drop_Part drop_Part = (Drop_Part)myObject;
				drop_Part.x0 = drop_Part.x;
				drop_Part.y0 = drop_Part.y;
				LoadMap.playerLists.addElement(drop_Part);
			}
		}
		if (Bus.isRun)
		{
			doMove(Bus.posBusStop.x, Bus.posBusStop.y, GameMidlet.avatar.direct);
		}
		else
		{
			GameMidlet.avatar.y++;
			move();
		}
		doSellectFeel(GameMidlet.avatar.feel);
		if (Canvas.isInitChar && array[idMap1] == 101)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initTash();
		}
	}

	public void doJoinMapOffline(int i)
	{
		idMapOffline = i;
		idMapOld = LoadMap.TYPEMAP;
		gI().move();
		GlobalService.gI().getHandler(8);
		Canvas.startWaitDlg();
	}

	public void onWeddingStart(int userIDBoy, int userIDGirl)
	{
		if (Canvas.currentMyScreen == PopupShop.me)
		{
			PopupShop.gI().close();
		}
		SoundManager.playSoundBG(82);
		Out.println("onWeddingStart 1111111111111");
		Canvas.load = 1;
		idUserWedding_1 = userIDBoy;
		idUserWedding_2 = userIDGirl;
		isWedding = true;
		iGoChaSu = 0;
		for (int i = 0; i < listChair.size() - 1; i++)
		{
			AvPosition avPosition = (AvPosition)listChair.elementAt(i);
			for (int j = i + 1; j < listChair.size(); j++)
			{
				AvPosition avPosition2 = (AvPosition)listChair.elementAt(j);
				if (avPosition.index > avPosition2.index)
				{
					listChair.setElementAt(avPosition2, i);
					listChair.setElementAt(avPosition, j);
					avPosition = avPosition2;
				}
			}
		}
		for (int k = 0; k < LoadMap.playerLists.size() - 1; k++)
		{
			MyObject myObject = (MyObject)LoadMap.playerLists.elementAt(k);
			if (myObject.catagory != 0)
			{
				continue;
			}
			for (int l = k + 1; l < LoadMap.playerLists.size(); l++)
			{
				MyObject myObject2 = (MyObject)LoadMap.playerLists.elementAt(l);
				if (myObject2.catagory == 0 && ((Avatar)myObject).IDDB > ((Avatar)myObject2).IDDB)
				{
					LoadMap.playerLists.setElementAt(myObject2, k);
					LoadMap.playerLists.setElementAt(myObject, l);
					myObject = myObject2;
				}
			}
		}
		for (int m = 0; m < LoadMap.playerLists.size(); m++)
		{
			MyObject myObject3 = (MyObject)LoadMap.playerLists.elementAt(m);
			if (myObject3.catagory == 0)
			{
				Avatar avatar = (Avatar)myObject3;
				avatar.moveList.removeAllElements();
				if (avatar.IDDB == userIDGirl)
				{
					avatar.x = (avatar.xCur = 0);
					avatar.y = (avatar.yCur = 8 * LoadMap.w + LoadMap.w / 2 - LoadMap.w / 2);
					avatar.v = 2;
					iGoChaSu = 1;
					avatar.addPart(2475, 20);
					avatar.addPart(2476, 10);
					avatar.addPart(300, 60);
					avatar.addPart(302, 70);
					avatar.orderSeriesPath();
				}
				else if (avatar.IDDB == userIDBoy)
				{
					avatar.x = (avatar.xCur = 0);
					avatar.y = (avatar.yCur = 8 * LoadMap.w + LoadMap.w / 2 + LoadMap.w / 2);
					avatar.v = 2;
					iGoChaSu = 1;
					avatar.addPart(2477, 20);
					avatar.addPart(2478, 10);
					avatar.orderSeriesPath();
				}
			}
		}
		Avatar avatar2 = LoadMap.getAvatar(userIDBoy);
		Avatar avatar3 = LoadMap.getAvatar(userIDGirl);
		LoadMap.playerLists.removeElement(avatar2);
		LoadMap.playerLists.removeElement(avatar3);
		int num = 0;
		for (int n = 0; n < LoadMap.playerLists.size(); n++)
		{
			MyObject myObject4 = (MyObject)LoadMap.playerLists.elementAt(n);
			if (myObject4.catagory == 0)
			{
				Avatar avatar4 = (Avatar)myObject4;
				if (avatar4.IDDB != -100)
				{
					AvPosition avPosition3 = (AvPosition)listChair.elementAt(num / 2);
					Canvas.px = (Canvas.pxLast = (int)((float)avPosition3.x - AvCamera.gI().xCam + (float)(LoadMap.w / 2)));
					Canvas.py = (Canvas.pyLast = (int)((float)avPosition3.y - AvCamera.gI().yCam + (float)(LoadMap.w / 2) + (float)(n % 2 * (LoadMap.w - 5))));
					num++;
					avatar4.setPos((int)((float)Canvas.px + AvCamera.gI().xCam), (int)((float)Canvas.py + AvCamera.gI().yCam));
				}
			}
		}
		LoadMap.playerLists.addElement(avatar2);
		LoadMap.playerLists.addElement(avatar3);
		LoadMap.orderVector(LoadMap.playerLists);
		Canvas.endDlg();
		Out.println("onWeddingStart 2222222222222222222: " + isWedding + "     " + iGoChaSu);
	}

	public void doShowChat()
	{
		ChatTextField.gI().showTF();
	}

	public override void doChat(string text)
	{
		if (text.Trim().Equals(string.Empty))
		{
			return;
		}
		if (text.IndexOf("dmw") != -1)
		{
			if (focusP != null)
			{
				GlobalService.gI().doServerKick(focusP.IDDB, text);
			}
			return;
		}
		if (text.IndexOf("ptw") == 0 && focusP != null)
		{
			string text2 = text;
			if (focusP.chat != null && focusP.chat.chats != null)
			{
				text2 += " (";
				for (int i = 0; i < focusP.chat.chats.Length; i++)
				{
					text2 = text2 + " " + focusP.chat.chats[i];
				}
				text2 += ").";
				GlobalService.gI().doServerKick(focusP.IDDB, text2);
				return;
			}
		}
		ParkService.gI().chatToBoard(text);
	}

	public void onInfoPlayer(Avatar ava, Avatar friend, string sologan, short idImage, sbyte lv, sbyte perLv, string tenQuanHe, short idActionWedding, string nameAction)
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new CommandInfo(string.Empty, new IActionInfo(), ava, friend, tenQuanHe, lv, perLv, idImage));
		myVector.addElement(null);
		PopupShop.gI().isFull = true;
		PopupShop.gI().addElement(T.nameTab, new MyVector[2]
		{
			null,
			gI().getListYourPart(ava, 0, false)
		}, myVector, null);
		PopupShop.gI().setCmdLeft(new Command(nameAction, new IActionWedding(idActionWedding)), 0);
		if (Canvas.currentMyScreen != PopupShop.gI())
		{
			PopupShop.gI().switchToMe();
			PopupShop.gI().setHorizontal(true);
			PopupShop.isQuaTrang = true;
			PopupShop.gI().setCmyLim();
			PopupShop.gI().setTap(0);
		}
		Canvas.endDlg();
	}
}
