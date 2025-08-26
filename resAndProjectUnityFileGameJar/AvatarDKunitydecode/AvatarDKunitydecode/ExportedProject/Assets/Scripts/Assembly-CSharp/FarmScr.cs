using System;
using System.IO;
using UnityEngine;

public class FarmScr : MyScreen
{
	private class IActionFeeding1 : IAction
	{
		private readonly FarmScr p;

		private readonly Item item;

		public IActionFeeding1(FarmScr p, Item item)
		{
			this.p = p;
			this.item = item;
		}

		public void perform()
		{
			p.doUsingVatPhamAnimal(item, 1);
		}
	}

	private class FeedingCommand : Command
	{
		private readonly FarmItem fItem;

		public FeedingCommand(string des, IActionFeeding1 feeding1, FarmItem fItem)
			: base(des, feeding1)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class CommandKhoGiong : Command
	{
		public CommandKhoGiong(string caption, int type)
			: base(caption, type)
		{
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.paintImg(g, 65, x, y, 3);
		}
	}

	private class IActionSeed : IAction
	{
		private int ii;

		private int pos;

		public IActionSeed(int i, int pos)
		{
			ii = i;
			this.pos = pos;
		}

		public void perform()
		{
			gI().doPlantSeed(ii, pos);
		}
	}

	private class CommandKhoGiong2 : Command
	{
		private Item item;

		public CommandKhoGiong2(string caption, IAction ac, Item it)
			: base(caption, ac)
		{
			item = it;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.getTreeByID(item.ID).paint(g, 7, x, y, 3);
		}
	}

	private class IActionPlantSeed : IAction
	{
		private readonly int ii;

		private readonly FarmScr p;

		public IActionPlantSeed(int ii, FarmScr p)
		{
			this.ii = ii;
			this.p = p;
		}

		public void perform()
		{
			int posTreeByFocus = instance.getPosTreeByFocus(focusCell.x, focusCell.y);
			if (posTreeByFocus < cell.size())
			{
				p.doPlantSeed(ii, posTreeByFocus);
			}
		}
	}

	private class CommandPlantSeed : Command
	{
		private readonly Item item;

		public CommandPlantSeed(string s, IActionPlantSeed seed, Item item)
			: base(s, seed)
		{
			this.item = item;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.getTreeByID(item.ID).paint(g, 7, x, y - AvMain.hSmall / 2, 3);
			Canvas.smallFontYellow.drawString(g, "(" + item.number + ")", x, y + Menu.gI().menuH / 2 - AvMain.hDuBox - AvMain.hSmall * 2 + (AvMain.hd - 1) * 10, 2);
			Canvas.smallFontYellow.drawString(g, item.name, x, y + Menu.gI().menuH / 2 - AvMain.hDuBox - AvMain.hSmall + (AvMain.hd - 1) * 10, 2);
		}
	}

	private class CommandVatPham1 : Command
	{
		public CommandVatPham1(string caption, int type)
			: base(caption, type)
		{
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.paintImg(g, 65, x, y, 3);
		}
	}

	private class IActionVatPham1 : IAction
	{
		public void perform()
		{
			gI().setAction(1, idItemUsing);
		}
	}

	private class IActionVatPham2 : IAction
	{
		private CellFarm cell;

		public IActionVatPham2(CellFarm c)
		{
			cell = c;
		}

		public void perform()
		{
			gI().doLamDat(cell);
		}
	}

	private class CommandVatPham2 : Command
	{
		private int index;

		public CommandVatPham2(string caption, IAction ac, int ind)
			: base(caption, ac)
		{
			index = ind;
		}

		public CommandVatPham2(string caption, int type, int ind)
			: base(caption, type)
		{
			index = ind;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			img.drawFrame(index, x, y, 0, 3, g);
		}
	}

	private class ActionVatPham3 : IAction
	{
		public void perform()
		{
		}
	}

	private class iCommandItemFarm : Command
	{
		private FarmItem fItem;

		public iCommandItemFarm(string caption, IAction action, FarmItem f)
			: base(caption, action)
		{
			fItem = f;
		}

		public iCommandItemFarm(string caption, int index, int subIndex, FarmItem f, AvMain pointer)
			: base(caption, index, subIndex, pointer)
		{
			fItem = f;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class IActionLamDat : IAction
	{
		private readonly FarmScr p;

		private readonly CellFarm cell;

		public IActionLamDat(FarmScr p, CellFarm cell)
		{
			this.p = p;
			this.cell = cell;
		}

		public void perform()
		{
			p.doLamDat(cell);
		}
	}

	private class CommandLamDat : Command
	{
		public CommandLamDat(string land, IActionLamDat dat)
			: base(land, dat)
		{
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			img.drawFrame(1, x, y, 0, 3, g);
		}
	}

	private class IActionVatPham : IAction
	{
		private readonly FarmScr p;

		private readonly Item item;

		public IActionVatPham(FarmScr p, Item item)
		{
			this.p = p;
			this.item = item;
		}

		public void perform()
		{
			if (item.number > 0)
			{
				p.doUsingVatPham(item.ID, item);
			}
			else
			{
				Canvas.startOKDlg(T.empty + item.name);
			}
		}
	}

	private class CommandVatPham : Command
	{
		private readonly FarmItem fr;

		public CommandVatPham(string na, IActionVatPham pham, FarmItem fr)
			: base(na, pham)
		{
			this.fr = fr;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fr.paint(g, x, y, 0, 3);
		}
	}

	private class IActionItem5 : IAction
	{
		private FarmItem fItem;

		private AnimalInfo aInfo;

		private Item item;

		public IActionItem5(FarmItem fItem, AnimalInfo aInfo, Item item)
		{
			this.fItem = fItem;
			this.aInfo = aInfo;
			this.item = item;
		}

		public void perform()
		{
			if (LoadMap.focusObj != null)
			{
				gI().doUsingVatPhamAnimal(item, (aInfo.area != 1) ? 1 : 0);
			}
		}
	}

	private class CommandItem5 : Command
	{
		private FarmItem fItem;

		public CommandItem5(string name, IAction ac, FarmItem fItem)
			: base(name, ac)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class IActionThuoc : IAction
	{
		private FarmItem fItem;

		private Item item;

		public IActionThuoc(FarmItem fItem, Item item)
		{
			this.fItem = fItem;
			this.item = item;
		}

		public void perform()
		{
			if (LoadMap.focusObj != null)
			{
				if (fItem.action == 4)
				{
					gI().setAction(4, item.ID);
					gI().aniDoing = (Animal)LoadMap.focusObj;
					gI().aniDoing.isStand = true;
					gI().aniDoing.timeStand = (int)Canvas.getTick() / 1000;
				}
				FarmService.gI().doUsingItem(idFarm, ((Base)LoadMap.focusObj).IDDB, item.ID);
			}
		}
	}

	private class CommandThuoc : Command
	{
		private FarmItem fItem;

		public CommandThuoc(string name, IActionThuoc ac, FarmItem fItem)
			: base(name, ac)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class IActionSetAnimal : IAction
	{
		private FarmItem fItem;

		private Animal ani;

		private short ID;

		public IActionSetAnimal(FarmItem f, short ID, Animal ani)
		{
			fItem = f;
			this.ID = ID;
			this.ani = ani;
		}

		public void perform()
		{
			if (fItem.action == 4)
			{
				instance.setAction(4, ID);
				instance.aniDoing = (Animal)LoadMap.focusObj;
				instance.aniDoing.isStand = true;
				instance.aniDoing.timeStand = Canvas.getSecond();
			}
			FarmService.gI().doUsingItem(idFarm, ani.IDDB, ID);
		}
	}

	private class CommandSellAnimal : Command
	{
		public CommandSellAnimal(string sell, int index, AvMain pointer)
			: base(sell, index, pointer)
		{
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			g.drawImage(imgSell, x, y, 3);
		}
	}

	private class IActionVatPhamAnimal : IAction
	{
		private readonly FarmScr p;

		private readonly AnimalInfo aInfo;

		private readonly Item item;

		public IActionVatPhamAnimal(FarmScr p, AnimalInfo aInfo, Item item)
		{
			this.p = p;
			this.aInfo = aInfo;
			this.item = item;
		}

		public void perform()
		{
			if (LoadMap.focusObj != null)
			{
				p.doUsingVatPhamAnimal(item, (aInfo.area != 1) ? 1 : 0);
			}
		}
	}

	private class CommandVatPhamAnimal : Command
	{
		private readonly FarmItem fItem;

		public CommandVatPhamAnimal(string s, IActionVatPhamAnimal animal, FarmItem fItem)
			: base(s, animal)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class IActionChichThuocAnimal : IAction
	{
		private readonly FarmScr p;

		private readonly Item item;

		private readonly FarmItem fItem;

		public IActionChichThuocAnimal(FarmScr p, Item item, FarmItem fItem)
		{
			this.p = p;
			this.item = item;
			this.fItem = fItem;
		}

		public void perform()
		{
			if (LoadMap.focusObj != null)
			{
				if (fItem.action == 4)
				{
					p.setAction(4, item.ID);
					p.aniDoing = (Animal)LoadMap.focusObj;
					p.aniDoing.isStand = true;
					p.aniDoing.timeStand = Environment.TickCount / 1000;
				}
				FarmService.gI().doUsingItem(idFarm, ((Base)LoadMap.focusObj).IDDB, item.ID);
			}
		}
	}

	private class CommandChichThuocAnimal : Command
	{
		private readonly FarmItem fItem;

		public CommandChichThuocAnimal(string s, IActionChichThuocAnimal animal, FarmItem fItem)
			: base(s, animal)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class PoLayer : Layer
	{
		private readonly Point po;

		public PoLayer(Point po)
		{
			this.po = po;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			PaintPopup.fill(po.x * AvMain.hd, po.y * AvMain.hd, po.w * AvMain.hd, po.h * AvMain.hd, 5921542, g);
		}

		public override void update()
		{
			if (po.y < po.limitY)
			{
				po.x += po.v;
				po.y += po.g;
				po.g++;
			}
			else
			{
				po.v = 0;
				po.g = 0;
			}
		}
	}

	private class IActionHound : IAction
	{
		private readonly int pos;

		public IActionHound(int pos)
		{
			this.pos = pos;
		}

		public void perform()
		{
			FarmService.gI().doHervest(idFarm, pos);
		}
	}

	private class IActionCuocDat : IAction
	{
		private readonly FarmScr p;

		public IActionCuocDat(FarmScr p)
		{
			this.p = p;
		}

		public void perform()
		{
			p.setAction(0, -1);
			Canvas.endDlg();
		}
	}

	private class IActionBonPhan : IAction
	{
		private int pos;

		private FarmItem fr;

		public IActionBonPhan(int pos, FarmItem fr)
		{
			this.pos = pos;
			this.fr = fr;
		}

		public void perform()
		{
			instance.setAction(3, fr.ID);
			FarmService.gI().doUsingItem(idFarm, pos, fr.ID);
		}
	}

	private class IActionSet3 : IAction
	{
		private AvPosition pos;

		public IActionSet3(AvPosition pos)
		{
			this.pos = pos;
		}

		public void perform()
		{
			instance.doPlantSeed(indexItem, pos.anchor);
			instance.setGieoHat();
		}
	}

	private class IActionSet2 : IAction
	{
		private CellFarm c;

		public IActionSet2(CellFarm c)
		{
			this.c = c;
		}

		public void perform()
		{
			GameMidlet.avatar.action = 0;
			focusCell.x = c.x / LoadMap.w;
			focusCell.y = c.y / LoadMap.w;
			instance.doLamDat(c);
		}
	}

	private class IActionSet1 : IAction
	{
		private CellFarm c;

		public IActionSet1(CellFarm c)
		{
			this.c = c;
		}

		public void perform()
		{
			focusCell.x = c.x / LoadMap.w;
			focusCell.y = c.y / LoadMap.w;
			instance.setAction(1, idItemUsing);
		}
	}

	private class IActionSellProduct : IAction
	{
		private readonly short idItem;

		public IActionSellProduct(short idItem)
		{
			this.idItem = idItem;
		}

		public void perform()
		{
			FarmService.gI().doSellItem(idItem);
			Canvas.startWaitDlg();
		}
	}

	private class IActionBuyAnimalXu : IAction
	{
		private readonly AnimalInfo animal;

		public IActionBuyAnimalXu(AnimalInfo animal)
		{
			this.animal = animal;
		}

		public void perform()
		{
			FarmService.gI().doBuyAnimal(animal, 1);
		}
	}

	private class IActionBuyAnimalLuong : IAction
	{
		private readonly AnimalInfo animal;

		public IActionBuyAnimalLuong(AnimalInfo animal)
		{
			this.animal = animal;
		}

		public void perform()
		{
			FarmService.gI().doBuyAnimal(animal, 2);
		}
	}

	private class IactionBuyTreeInput : IAction
	{
		private int idTree;

		public IactionBuyTreeInput(int id)
		{
			idTree = id;
		}

		public void perform()
		{
			ipKeyboard.openKeyBoard(T.number, ipKeyboard.NUMBERIC, string.Empty, new IactionBuyTree(idTree), false);
		}
	}

	private class IactionBuyTree : IKbAction
	{
		private int idTree;

		public IactionBuyTree(int id)
		{
			idTree = id;
		}

		public void perform(string text)
		{
			int num = int.Parse(text);
			int num2 = 0;
			int num3 = 0;
			TreeInfo treeInfoByID = FarmData.getTreeInfoByID(idTree);
			num2 = treeInfoByID.priceSeed[0];
			num3 = treeInfoByID.priceSeed[1];
			int pri = num2;
			int num4 = num3;
			Canvas.getTypeMoney(num2 * num, num3 * num, new IActionXuTree(idTree, num, pri, 1), new IActionXuTree(idTree, num, pri, 2), new IActionEnd());
		}
	}

	private class IActionEnd : IAction
	{
		public void perform()
		{
		}
	}

	private class IActionXuTree : IAction
	{
		private int idTree;

		private int num;

		private int price;

		private int type;

		public IActionXuTree(int id, int num, int pri, int type)
		{
			idTree = id;
			this.num = num;
			price = pri;
			this.type = type;
		}

		public void perform()
		{
			FarmService.gI().doBuyItem((short)idTree, (sbyte)num, type, price * num);
		}
	}

	private class IActionBuyItemCuaHang : IAction
	{
		private readonly FarmScr p;

		private readonly int ii;

		public IActionBuyItemCuaHang(FarmScr p, int ii)
		{
			this.p = p;
			this.ii = ii;
		}

		public void perform()
		{
			ipKeyboard.openKeyBoard(T.number, ipKeyboard.NUMBERIC, string.Empty, new IActionBuyItem(ii, 0), false);
		}
	}

	private class IActionBuyItem : IKbAction
	{
		private int type;

		private int index;

		public IActionBuyItem(int index, int type)
		{
			this.type = type;
			this.index = index;
		}

		public void perform(string text)
		{
			int num = int.Parse(text);
			int num2 = 0;
			int num3 = 0;
			if (type == 0)
			{
				TreeInfo treeInfoByID = FarmData.getTreeInfoByID(index);
				num2 = treeInfoByID.priceSeed[0];
				num3 = treeInfoByID.priceSeed[1];
			}
			else if (type == 2)
			{
				num2 = FarmData.getVPbyID(index).price[0];
				num3 = FarmData.getVPbyID(index).price[1];
			}
			else if (type == 4)
			{
				FarmItem farmItem = getFarmItem(index);
				if (farmItem != null)
				{
					num2 = farmItem.priceXu;
					num3 = farmItem.priceLuong;
				}
			}
			int a = num2;
			int b = num3;
			Canvas.getTypeMoney(num2 * num, num3 * num, new IActionDoBuyItem1(index, a, num), new IActionDoBuyItem2(index, b, num), null);
		}
	}

	private class CommandBuyItemCuaHang : Command
	{
		private readonly int ii;

		public CommandBuyItemCuaHang(string select, IAction hang, int ii)
			: base(select, hang)
		{
			this.ii = ii;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.treeInfo[ii].paint(g, 7, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(FarmData.treeInfo[ii].name1 + "(" + FarmData.treeInfo[ii].harvestTime + T.h + ")");
				PopupShop.addStr(T.priceStr + Canvas.getPriceMoney(FarmData.treeInfo[ii].priceSeed[0], FarmData.treeInfo[ii].priceSeed[1], false));
				PopupShop.addStr(T.level[2] + ": " + FarmData.treeInfo[ii].lv);
				if (FarmData.treeInfo[ii].isDynamic)
				{
					FarmItem farmItem = getFarmItem(FarmData.treeInfo[ii].productID);
					PopupShop.addStr("Sản lượng: " + Canvas.getMoneys(FarmData.treeInfo[ii].numProduct) + " " + farmItem.des);
				}
				else
				{
					PopupShop.addStr("Sản lượng: " + Canvas.getMoneys(FarmData.treeInfo[ii].numProduct));
				}
			}
		}
	}

	private class IActionBuyAnimalCuaHang : IAction
	{
		private readonly FarmScr p;

		private readonly AnimalInfo ani;

		public IActionBuyAnimalCuaHang(FarmScr p, AnimalInfo ani, int ii)
		{
			this.p = p;
			this.ani = ani;
		}

		public void perform()
		{
			p.doBuyAnimal(ani);
		}
	}

	private class CommandBuyAnimalCuaHang : Command
	{
		private readonly AnimalInfo ani;

		private readonly int ii;

		public CommandBuyAnimalCuaHang(string select, IActionBuyAnimalCuaHang hang, AnimalInfo ani, int ii)
			: base(select, hang)
		{
			this.ani = ani;
			this.ii = ii;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, ani.iconID, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus - FarmData.treeInfo.Length)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(ani.name + "(" + ani.harvestTime + T.h + ")");
				PopupShop.addStr(T.priceStr + Canvas.getPriceMoney(ani.price[0], ani.price[1], true));
				PopupShop.addStr(ani.des);
			}
		}
	}

	private class IActionGoVatPham : IAction
	{
		private readonly FarmScr p;

		private readonly FarmItem item;

		public IActionGoVatPham(FarmScr p, FarmItem item)
		{
			this.p = p;
			this.item = item;
		}

		public void perform()
		{
			ipKeyboard.openKeyBoard(T.number, ipKeyboard.NUMBERIC, string.Empty, new IActionBuyItem(item.ID, 4), false);
		}
	}

	private class CommandGoVatPham : Command
	{
		private readonly FarmItem item;

		private readonly int ii;

		public CommandGoVatPham(string s, IActionGoVatPham pham, FarmItem item, int ii)
			: base(s, pham)
		{
			this.item = item;
			this.ii = ii;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			item.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 0, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(item.des);
				PopupShop.addStr(T.priceStr + Canvas.getPriceMoney(item.priceXu, item.priceLuong, false));
			}
		}
	}

	private class IActionGoKhoHang1 : IAction
	{
		private readonly FarmScr p;

		private readonly Item item;

		public IActionGoKhoHang1(FarmScr p, Item item)
		{
			this.p = p;
			this.item = item;
		}

		public void perform()
		{
			p.doSellProduct(item.ID);
		}
	}

	private class CommandGoKhoHang1 : Command
	{
		private readonly Item item;

		private readonly int ii;

		public CommandGoKhoHang1(string sell, IActionGoKhoHang1 hang1, Item item, int ii)
			: base(sell, hang1)
		{
			this.item = item;
			this.ii = ii;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			if (item.ID < 50)
			{
				FarmData.getTreeByID(item.ID).paint(g, 7, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
				return;
			}
			AnimalInfo animalByID = FarmData.getAnimalByID(item.ID);
			AvatarData.paintImg(g, animalByID.iconProduct, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				if (item.ID < 50)
				{
					PopupShop.addStr(item.name);
				}
				else
				{
					AnimalInfo animalByID = FarmData.getAnimalByID(item.ID);
					PopupShop.addStr(animalByID.name);
				}
				PopupShop.addStr(T.number + item.number);
				PopupShop.addStr(T.inCome + Canvas.getMoneys(item.price[0] * item.number) + T.xu);
				PopupShop.addStr(MapScr.strTkFarm());
			}
		}
	}

	private class IActionGoKhoHang2 : IAction
	{
		private readonly FarmScr p;

		private readonly FarmItem fItem;

		public IActionGoKhoHang2(FarmScr p, FarmItem fItem)
		{
			this.p = p;
			this.fItem = fItem;
		}

		public void perform()
		{
			p.doSellProduct(fItem.ID);
		}
	}

	private class CommandGoKhoHang2 : Command
	{
		private readonly FarmItem fItem;

		private readonly int ii;

		private readonly Item item;

		public CommandGoKhoHang2(string s, IActionGoKhoHang2 hang2, FarmItem fItem, int ii, Item item)
			: base(s, hang2)
		{
			this.fItem = fItem;
			this.ii = ii;
			this.item = item;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 0, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus - itemProduct.size())
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(fItem.des);
				if (fItem.priceLuong > 0)
				{
					PopupShop.addStr(T.inCome + Canvas.getMoneys(item.number * fItem.priceLuong) + T.dola + "(Tài khoản chính)");
				}
				else if (fItem.priceXu > 0)
				{
					PopupShop.addStr(T.inCome + Canvas.getMoneys(item.number * fItem.priceXu) + T.dola);
				}
				PopupShop.addStr(T.number + item.number);
				PopupShop.addStr(MapScr.strTkFarm());
			}
		}
	}

	private class IActionEmpty : IAction
	{
		public void perform()
		{
		}
	}

	private class CommandOpenKhoHang1 : Command
	{
		private readonly int ii;

		private Item item;

		public CommandOpenKhoHang1(string s, IActionEmpty empty, int ii)
			: base(s, empty)
		{
			this.ii = ii;
			item = (Item)itemSeed.elementAt(ii);
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.getTreeByID(item.ID).paint(g, 7, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(item.name);
				PopupShop.addStr(T.number + item.number);
			}
		}
	}

	private class CommandOpenKhoHang2 : Command
	{
		private readonly int ii;

		private Item item;

		public CommandOpenKhoHang2(string s, IActionEmpty empty, int ii)
			: base(s, empty)
		{
			this.ii = ii;
			item = (Item)listItemFarm.elementAt(ii);
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmItem farmItem = getFarmItem(item.ID);
			farmItem.paint(g, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 0, 3);
		}

		public override void update()
		{
			if (PopupShop.isTransFocus && ii == PopupShop.focus - itemSeed.size())
			{
				PopupShop.resetIsTrans();
				FarmItem farmItem = getFarmItem(item.ID);
				PopupShop.addStr(farmItem.des);
				FarmItem farmItem2 = getFarmItem(item.ID);
				int num = item.number;
				if (farmItem2.type == 4)
				{
					num -= listFood[1].size();
				}
				else if (farmItem2.type == 1)
				{
					num -= listFood[0].size();
				}
				PopupShop.addStr(T.number + num);
			}
		}
	}

	private class IActionDoBuyItem1 : IAction
	{
		private readonly int index;

		private readonly int a;

		private readonly int n;

		public IActionDoBuyItem1(int index, int a, int n)
		{
			this.index = index;
			this.a = a;
			this.n = n;
		}

		public void perform()
		{
			FarmService.gI().doBuyItem((short)index, (sbyte)n, 1, a * n);
		}
	}

	private class IActionDoBuyItem2 : IAction
	{
		private readonly int index;

		private readonly int b;

		private readonly int n;

		public IActionDoBuyItem2(int index, int b, int n)
		{
			this.index = index;
			this.b = b;
			this.n = n;
		}

		public void perform()
		{
			FarmService.gI().doBuyItem((short)index, (sbyte)n, 2, b * n);
		}
	}

	private class CommandGieoHat1 : Command
	{
		private Item item;

		public CommandGieoHat1(string name, IActionHieoHat1 action, Item item)
			: base(name, action)
		{
			this.item = item;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.getTreeByID(item.ID).paint(g, 7, x, y, 3);
		}
	}

	private class IActionHieoHat1 : IAction
	{
		private int index;

		public IActionHieoHat1(int index)
		{
			this.index = index;
		}

		public void perform()
		{
			instance.setAuto(index);
		}
	}

	private class IActionEat : IAction
	{
		private Animal pet;

		public IActionEat(Animal pet)
		{
			this.pet = pet;
		}

		public void perform()
		{
			bool flag = false;
			AnimalInfo animalByID = FarmData.getAnimalByID(pet.species);
			for (int i = 0; i < listItemFarm.size(); i++)
			{
				Item item = (Item)listItemFarm.elementAt(i);
				FarmItem farmItem = getFarmItem(item.ID);
				if (farmItem.type == animalByID.area && farmItem.action == 5 && (animalByID.area == 4 || animalByID.area == 1))
				{
					int number = item.number;
					if (number > 0)
					{
						flag = true;
						pet.hunger = false;
						gI().doEat(farmItem.ID, pet.IDDB);
						gI().commandAction(10);
					}
				}
			}
			if (!flag)
			{
				Canvas.startOKDlg("Kho của bạn đã hết thức ăn, xin vào cửa hàng để mua.");
				gI().commandTab(8);
			}
		}
	}

	private class IActionTriBenh3 : IAction
	{
		private Animal pet;

		public IActionTriBenh3(Animal pet)
		{
			this.pet = pet;
		}

		public void perform()
		{
			bool flag = false;
			for (int i = 0; i < listItemFarm.size(); i++)
			{
				Item item = (Item)listItemFarm.elementAt(i);
				FarmItem farmItem = getFarmItem(item.ID);
				if (farmItem.action == 6)
				{
					FarmService.gI().doUsingItem(idFarm, pet.IDDB, item.ID);
					flag = true;
					gI().commandAction(10);
					break;
				}
			}
			if (!flag)
			{
				gI().commandTab(8);
				Canvas.startOKDlg("Kho của bạn đã hết thuốc bổ, xin vào cửa hàng để mua.");
			}
		}
	}

	private class IActionTriBenh2 : IAction
	{
		private Animal pet;

		public IActionTriBenh2(Animal pet)
		{
			this.pet = pet;
		}

		public void perform()
		{
			bool flag = false;
			for (int i = 0; i < listItemFarm.size(); i++)
			{
				Item item = (Item)listItemFarm.elementAt(i);
				FarmItem farmItem = getFarmItem(item.ID);
				if (farmItem.ID == 121)
				{
					gI().setActionAnimal(farmItem, item.ID, pet);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Canvas.startOKDlg("Kho của bạn đã hết thuốc cúm, xin vào cửa hàng để mua.");
				gI().commandTab(8);
			}
		}
	}

	private class IActionTriBenh1 : IAction
	{
		private Animal pet;

		public IActionTriBenh1(Animal pet)
		{
			this.pet = pet;
		}

		public void perform()
		{
			bool flag = false;
			for (int i = 0; i < listItemFarm.size(); i++)
			{
				Item item = (Item)listItemFarm.elementAt(i);
				FarmItem farmItem = getFarmItem(item.ID);
				if (farmItem.ID == 120)
				{
					gI().setActionAnimal(farmItem, item.ID, pet);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Canvas.startOKDlg("Kho của bạn đã hết thuốc tiêu chảy, xin vào cửa hàng để mua.");
				gI().commandTab(8);
			}
		}
	}

	private class IActionCattleFeeding : IAction
	{
		private readonly sbyte type;

		private readonly Item item;

		public IActionCattleFeeding(sbyte type, Item item)
		{
			this.type = type;
			this.item = item;
		}

		public void perform()
		{
			if (type == 2)
			{
				Cattle.itemID = item.ID;
			}
			else
			{
				Dog.itemID = item.ID;
			}
		}
	}

	private class CommandCattleFeeding : Command
	{
		private readonly FarmItem fItem;

		public CommandCattleFeeding(string name, IActionCattleFeeding feeding, FarmItem fItem)
			: base(name, feeding)
		{
			this.fItem = fItem;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			fItem.paint(g, x, y, 0, 3);
		}
	}

	private class IActionSendHarvestAnimal : IAction
	{
		private readonly Animal pet;

		public IActionSendHarvestAnimal(Animal pet)
		{
			this.pet = pet;
		}

		public void perform()
		{
			FarmService.gI().doHarvestAnimal(idFarm, pet.IDDB);
		}
	}

	private class IActionPriceAnimal : IAction
	{
		private readonly sbyte index;

		public IActionPriceAnimal(sbyte index)
		{
			this.index = index;
		}

		public void perform()
		{
			FarmService.gI().doSellAnimal(idFarm, index);
		}
	}

	private class CommandMenuStarFruit1 : Command
	{
		private int index;

		public CommandMenuStarFruit1(string name, int type, int index)
			: base(name, type)
		{
			this.index = index;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmData.paintImg(g, index, x, y, 3);
		}
	}

	private class CommandCooking1 : Command
	{
		private int ii;

		private Food food;

		public CommandCooking1(string caption, IActionCooking1 ac, int ii, Food food)
			: base(caption, ac)
		{
			this.food = food;
			this.ii = ii;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			FarmItem farmItem = getFarmItem(food.productID);
			FarmData.paintImg(g, farmItem.IDImg, x + PopupShop.wCell / 2, y + PopupShop.wCell / 2, 3);
			g.translate(CameraList.cmx, CameraList.cmy);
			g.setClip(0f, 0f, 6 * PopupShop.wCell, PopupShop.h);
			if (ii == PopupShop.focus && !PopupShop.gI().isHide)
			{
				for (int i = 0; i < food.material.Length; i++)
				{
					Item item = null;
					if (food.material[i] < 50)
					{
						item = getProductByID(food.material[i]);
						FarmData.getTreeByID(food.material[i]).paint(g, 7, PopupShop.w / 2 - food.material.Length * 50 * AvMain.hd / 2 + 50 * i * AvMain.hd + 25 * (AvMain.hd - 1) + ((AvMain.hd == 1) ? 15 : 0), PopupShop.wCell * 2 + 10 * AvMain.hd + AvMain.hBlack * 4 + ((AvMain.hd == 1) ? 15 : 0), 3);
					}
					else if (food.material[i] < 100)
					{
						item = getProductByID(food.material[i]);
						AnimalInfo animalByID = FarmData.getAnimalByID(food.material[i]);
						AvatarData.paintImg(g, animalByID.iconProduct, PopupShop.w / 2 - food.material.Length * 50 * AvMain.hd / 2 + 50 * i * AvMain.hd + 25 * (AvMain.hd - 1) + ((AvMain.hd == 1) ? 15 : 0), PopupShop.wCell * 2 + 10 * AvMain.hd + AvMain.hBlack * 4 + ((AvMain.hd == 1) ? 15 : 0), 3);
					}
					else
					{
						item = getItemProductByID(food.material[i]);
						FarmItem farmItem2 = getFarmItem(food.material[i]);
						FarmData.paintImg(g, farmItem2.IDImg, PopupShop.w / 2 - food.material.Length * 50 * AvMain.hd / 2 + 50 * i * AvMain.hd + 25 * (AvMain.hd - 1) + ((AvMain.hd == 1) ? 15 : 0), PopupShop.wCell * 2 + 10 * AvMain.hd + AvMain.hBlack * 4 + ((AvMain.hd == 1) ? 15 : 0), 3);
					}
					FontX fontX = Canvas.blackF;
					if (item == null || item.number < food.numberMaterial[i])
					{
						fontX = Canvas.arialFont;
					}
					fontX.drawString(g, food.numberMaterial[i] + string.Empty, PopupShop.w / 2 - food.material.Length * 50 * AvMain.hd / 2 + 50 * i * AvMain.hd - 1 + 25 * (AvMain.hd - 1) + ((AvMain.hd == 1) ? 15 : 0), PopupShop.wCell * 2 + 10 * AvMain.hd + AvMain.hBlack * 4 + 8 * AvMain.hd + ((AvMain.hd == 1) ? 15 : 0), 2);
					if (i != food.material.Length - 1)
					{
						Canvas.blackF.drawString(g, "+", PopupShop.w / 2 - food.material.Length * 50 * AvMain.hd / 2 + 50 * i * AvMain.hd + 25 * AvMain.hd + 25 * (AvMain.hd - 1) + ((AvMain.hd == 1) ? 15 : 0), PopupShop.wCell * 2 + 10 * AvMain.hd + AvMain.hBlack * 4 + ((AvMain.hd == 1) ? 15 : 0), 2);
					}
				}
			}
			g.setClip(0f, 0f, 6 * PopupShop.wCell, PopupShop.numH * PopupShop.wCell - PopupShop.duCam);
			g.translate(0f - CameraList.cmx, 0f - CameraList.cmy);
		}

		public override void update()
		{
			if (ii == PopupShop.focus)
			{
				PopupShop.resetIsTrans();
				PopupShop.addStr(food.text);
				PopupShop.addStr(T.time + food.cookTime + "phut");
				FarmItem farmItem = getFarmItem(food.productID);
				if (farmItem.priceXu > 0)
				{
					PopupShop.addStr(T.salePrice + Canvas.getMoneys(farmItem.priceXu) + T.xu);
				}
				else if (farmItem.priceLuong > 0)
				{
					PopupShop.addStr(T.salePrice + Canvas.getMoneys(farmItem.priceLuong) + T.xu);
				}
				PopupShop.addStr(T.material);
			}
		}
	}

	private class IActionCooking1 : IAction
	{
		private Food food;

		public IActionCooking1(Food f)
		{
			food = f;
		}

		public void perform()
		{
			for (int i = 0; i < food.material.Length; i++)
			{
				Item item = null;
				string text = string.Empty;
				if (food.material[i] < 100)
				{
					item = getProductByID(food.material[i]);
					if (food.material[i] < 50)
					{
						text = FarmData.getTreeByID(food.material[i]).name;
					}
					else if (FarmData.getAnimalByID(food.material[i]).area == 1)
					{
						text = T.egg + " " + FarmData.getAnimalByID(food.material[i]).name;
					}
					else if (FarmData.getAnimalByID(food.material[i]).area == 2)
					{
						text = T.milk + " " + FarmData.getAnimalByID(food.material[i]).name;
					}
				}
				else
				{
					item = getItemProductByID(food.material[i]);
					text = getFarmItem(food.material[i]).des;
				}
				if (item == null || item.number < food.numberMaterial[i])
				{
					Canvas.startOKDlg(T.notEnough + text);
					return;
				}
			}
			FarmService.gI().doCooking(food.ID);
			PopupShop.gI().close();
		}
	}

	private class CommandCooking2 : Command
	{
		public override void paint(MyGraphics g, int x, int y)
		{
			Food foodByID = FarmData.getFoodByID(foodID);
			FarmItem farmItem = getFarmItem(foodByID.productID);
			FarmData.paintImg(g, farmItem.IDImg, Canvas.cameraList.disX / 2, PopupShop.h / 2 - 30, 3);
			Canvas.blackF.drawString(g, foodByID.text, Canvas.cameraList.disX / 2, PopupShop.h / 2 - 30 + 5 + FarmData.getImgIcon(farmItem.IDImg).h / 2 + AvMain.hSmall + 2, 2);
			string text = string.Empty;
			int num = remainTime / 3600;
			FontX fontX = Canvas.smallFontYellow;
			if (num > 0)
			{
				text = num + ":";
			}
			int num2 = (remainTime - num * 3600) / 60;
			if (num2 > 0 || num > 0)
			{
				text = text + num2 + ":";
			}
			int num3 = remainTime - num * 3600 - num2 * 60;
			text = text + num3 + string.Empty;
			if (remainTime == 0)
			{
				text = T.done;
				fontX = Canvas.blackF;
			}
			fontX.drawString(g, text, Canvas.cameraList.disX / 2, PopupShop.h / 2 - 30 + 5 + FarmData.getImgIcon(farmItem.IDImg).h / 2, 2);
		}
	}

	private class CommandCooking : Command
	{
		public CommandCooking(string caption, int index, AvMain pointer)
			: base(caption, index, pointer)
		{
		}

		public override void update()
		{
			if (PopupShop.gI().center != null)
			{
				PopupShop.gI().center.x = Canvas.w / 2 + PaintPopup.wButtonSmall;
				PopupShop.gI().center.y = PopupShop.y + PopupShop.h - PaintPopup.hButtonSmall;
			}
			if (PopupShop.gI().left != null)
			{
				PopupShop.gI().left.x = Canvas.w / 2 - PaintPopup.wButtonSmall;
				PopupShop.gI().left.y = PopupShop.y + PopupShop.h - PaintPopup.hButtonSmall;
			}
			else if (PopupShop.gI().center != null)
			{
				PopupShop.gI().center.x = Canvas.w / 2;
				PopupShop.gI().center.y = PopupShop.y + PopupShop.h - PaintPopup.hButtonSmall;
			}
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			Food foodByID = FarmData.getFoodByID(foodID);
			FarmItem farmItem = getFarmItem(foodByID.productID);
			FarmData.paintImg(g, farmItem.IDImg, Canvas.cameraList.disX / 2, PopupShop.h / 3 - 25 * AvMain.hd, 3);
			Canvas.blackF.drawString(g, foodByID.text, Canvas.cameraList.disX / 2, PopupShop.h / 3 - 15 * AvMain.hd + 5 + FarmData.getImgIcon(farmItem.IDImg).h / 2 + AvMain.hSmall + 2, 2);
			string text = string.Empty;
			int num = remainTime / 3600;
			FontX fontX = Canvas.smallFontYellow;
			if (num > 0)
			{
				text = num + ":";
			}
			int num2 = (remainTime - num * 3600) / 60;
			if (num2 > 0 || num > 0)
			{
				text = text + num2 + ":";
			}
			int num3 = remainTime - num * 3600 - num2 * 60;
			text = text + num3 + string.Empty;
			if (remainTime == 0)
			{
				text = T.done;
				fontX = Canvas.blackF;
			}
			fontX.drawString(g, text, Canvas.cameraList.disX / 2, PopupShop.h / 3 - 20 * AvMain.hd + 5 + FarmData.getImgIcon(farmItem.IDImg).h / 2, 2);
		}
	}

	public static FarmScr instance;

	public static int idFarm;

	private string nameFarm;

	public static MyVector cell;

	private static MyVector itemSeed;

	public static MyVector listItemFarm;

	public static MyVector listFarmProduct;

	public static MyVector itemProduct;

	public static MyVector listNest;

	public static MyVector listBucket;

	public static MyVector animalLists;

	public static MyVector[] listFood;

	public static Image[] imgWorm_G;

	public static Image imgBuyLant;

	public static Image imgFocusCel;

	public static Image imgSell;

	public static FrameImage imgWormAndGrass;

	public static FrameImage imgTrough;

	public static FrameImage imgDogTr;

	public static FrameImage img;

	public static FrameImage imgBenh;

	public AvPosition[] posTree;

	private MyVector listHound;

	public static int numTileBarn;

	public static int numTilePond;

	public sbyte[] typeCell = new sbyte[5] { 1, 0, 1, 2, 3 };

	public byte[] typeCell1 = new byte[5] { 5, 4, 5, 6, 7 };

	public static AvPosition focusCell;

	public static AvPosition posName;

	public static AvPosition posBarn;

	public static AvPosition posPond;

	public new static sbyte action;

	public static sbyte frame;

	public AvPosition posDoing;

	public const sbyte CUT_DAT = 0;

	public const sbyte TUOI_NUOC = 1;

	public const sbyte BON_PHAN = 2;

	public const sbyte DIET_CO = 3;

	public const sbyte CHICH_THUOC = 4;

	private int t;

	public static int numO;

	public static int numW;

	public static int numH;

	public static int idItemUsing;

	public int timeLimit;

	public long curTime;

	public static int money;

	public static int numStatusAnimal;

	private static sbyte[][] FRAME;

	public static bool isAutoVatNuoi;

	public MyVector listAction = new MyVector();

	private Command cmdSelected;

	public static StarFruitObj starFruil;

	public static int priceSteal;

	public static string nameTemp;

	public static bool isSteal;

	public static bool isAbleSteal;

	public static bool isNew;

	public Command cmdNextSteal;

	public Command cmdCloseSteal;

	public Command cmdStreal;

	public static Image[] imgCell;

	public static Command cmdMenu;

	public static Command cmdLeftMenu;

	public static Command cmdFocusBet;

	public static Command cmdFeeding;

	public static Command cmdFinishAuto;

	public static Command cmdNextAuto;

	public MyVector listLeftMenu = new MyVector();

	private Animal aniDoing;

	public long timeDoing = -1L;

	private int tempTime;

	private int repeatTime = 350;

	public static bool isSelected;

	private bool isSelectedCell;

	private bool isChamSoc;

	public static int indexItem;

	public static int idSelected;

	private bool isTrans;

	private MyVector listSelectedCell = new MyVector();

	private new bool isTran;

	private int n;

	public static sbyte levelStore;

	public static int capacityStore;

	public static bool isReSize;

	public static sbyte numBarn;

	public static sbyte numPond;

	public static int xRemember;

	public static int yRemember;

	public static int remainTime;

	public static int curTimeCooking;

	public static short foodID;

	private bool isJoin = true;

	private int ii;

	private int indexAuto;

	public static int xPosCook;

	public static int yPosCook;

	static FarmScr()
	{
		itemSeed = new MyVector();
		listItemFarm = new MyVector();
		listFarmProduct = new MyVector();
		animalLists = new MyVector();
		listFood = new MyVector[2];
		action = -1;
		numO = 12;
		numW = 3;
		numH = 4;
		idItemUsing = -1;
		money = 0;
		numStatusAnimal = 0;
		FRAME = new sbyte[5][];
		isAutoVatNuoi = false;
		priceSteal = -1;
		nameTemp = string.Empty;
		isSteal = false;
		isAbleSteal = false;
		isNew = false;
		isSelected = false;
		indexItem = -1;
		idSelected = -1;
		isReSize = false;
		xRemember = -1;
		yRemember = -1;
		foodID = 0;
		FRAME[0] = new sbyte[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
		FRAME[1] = new sbyte[10] { 2, 2, 2, 2, 2, 3, 3, 3, 3, 3 };
		FRAME[2] = new sbyte[10] { 4, 4, 4, 4, 4, 5, 5, 5, 5, 5 };
		FRAME[3] = new sbyte[10] { 6, 6, 6, 6, 6, 7, 7, 7, 7, 7 };
		FRAME[4] = new sbyte[10] { 8, 8, 8, 8, 8, 9, 9, 9, 9, 9 };
	}

	public FarmScr()
	{
		listFood[0] = new MyVector();
		listFood[1] = new MyVector();
		cmdMenu = new Command(T.selectt, 0);
		cmdLeftMenu = new Command(T.menu, 1);
		cmdFocusBet = new Command(T.selectt, 2);
		cmdFeeding = new Command(T.selectt, 3);
		doLeftMenu();
		cmdFinishAuto = new Command(T.finish, 29, this);
		cmdNextAuto = new Command(T.next, 28, this);
		cmdNextSteal = new Command(T.next, 21, this);
		cmdCloseSteal = new Command(T.close, 18, this);
		cmdStreal = new Command("Trộm", 20, this);
	}

	public static FarmScr gI()
	{
		if (instance == null)
		{
			instance = new FarmScr();
		}
		return instance;
	}

	public override void switchToMe()
	{
		base.switchToMe();
	}

	public static void init()
	{
		isSteal = (isAbleSteal = false);
	}

	public override void initZoom()
	{
		AvCamera.gI().init(25);
	}

	public static void resetImg()
	{
		img = null;
		imgBuyLant = null;
		imgWorm_G = null;
		imgWormAndGrass = null;
		imgFocusCel = null;
	}

	public static void initImg()
	{
		imgBuyLant = Image.createImagePNG(T.getPath() + "/farm/buyLand");
		img = new FrameImage(Image.createImagePNG(T.getPath() + "/farm/cut"), 24 * AvMain.hd, 24 * AvMain.hd);
		imgWorm_G = new Image[2];
		imgWorm_G[0] = Image.createImagePNG(T.getPath() + "/farm/w");
		imgWorm_G[1] = Image.createImagePNG(T.getPath() + "/farm/g");
		imgWormAndGrass = new FrameImage(Image.createImagePNG(T.getPath() + "/farm/wg"), 13 * AvMain.hd, 9 * AvMain.hd);
		imgTrough = new FrameImage(Image.createImagePNG(T.getPath() + "/farm/m"), 27 * AvMain.hd, 17 * AvMain.hd);
		imgDogTr = new FrameImage(Image.createImagePNG(T.getPath() + "/farm/tc"), 13 * AvMain.hd, 13 * AvMain.hd);
		imgFocusCel = Image.createImagePNG(T.getPath() + "/temp/focusCell");
		imgBenh = new FrameImage(Image.createImagePNG(T.getPath() + "/farm/iB"), 9 * AvMain.hd, 13 * AvMain.hd);
		imgSell = Image.createImagePNG(T.getPath() + "/farm/coin");
		imgCell = new Image[8];
		for (int i = 0; i < 8; i++)
		{
			imgCell[i] = Image.createImagePNG(T.getPath() + "/farm/cell" + i);
		}
	}

	public override void close()
	{
		Canvas.startWaitDlg();
		GlobalService.gI().getHandler(8);
	}

	protected void doFeeding()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < listItemFarm.size(); i++)
		{
			Item item = (Item)listItemFarm.elementAt(i);
			FarmItem farmItem = getFarmItem(item.ID);
			if (farmItem.action == 5 && (farmItem.type == 4 || farmItem.type == 101))
			{
				myVector.addElement(new FeedingCommand(farmItem.des, new IActionFeeding1(this, item), farmItem));
			}
		}
		int num = MyScreen.hText + 10;
		Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - num - 10, num, num);
	}

	public void doLeftMenu()
	{
		listLeftMenu.addElement(new Command("Chăm sóc vật nuôi", 27, this));
		listLeftMenu.addElement(new Command("Gieo hạt", 25, this));
		listLeftMenu.addElement(new Command("Chăm sóc cây trồng", 26, this));
		listLeftMenu.addElement(MapScr.gI().cmdEvent);
		Command o = new Command(T.exit, 0, this);
		listLeftMenu.addElement(o);
	}

	protected void doExit()
	{
		Canvas.startWaitDlg();
		GlobalService.gI().getHandler(8);
	}

	protected void doSellect()
	{
		if (isSteal && !isAbleSteal)
		{
			return;
		}
		int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
		if (posTreeByFocus - cell.size() == 0)
		{
			doRequestPricePlant();
			return;
		}
		if (posTreeByFocus >= 0 && posTreeByFocus < cell.size())
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(posTreeByFocus);
			if (cellFarm.idTree == -1 && ((cellFarm.level == 1 && cellFarm.status != typeCell[1]) || (cellFarm.level == 2 && cellFarm.status != typeCell1[1])))
			{
				doVatPham(cellFarm);
				return;
			}
			if (cellFarm.idTree == -1 && ((cellFarm.level == 1 && cellFarm.status == typeCell[1]) || (cellFarm.level == 2 && cellFarm.status == typeCell1[1])))
			{
				doKhoGiong();
				return;
			}
		}
		doMenuCenter();
	}

	public override void doMenu()
	{
		commandTab(1);
	}

	private void doMenuCenter()
	{
		if (LoadMap.TYPEMAP == 25)
		{
			return;
		}
		int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
		int num = cell.size();
		if (posTreeByFocus - num == 0)
		{
			doRequestPricePlant();
		}
		if (posTreeByFocus >= 0 && posTreeByFocus < num)
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(posTreeByFocus);
			if (cellFarm.idTree == -1)
			{
				doKhoGiong();
			}
			else if (cellFarm.statusTree == 5)
			{
				doHarvest();
			}
			else
			{
				doVatPham(cellFarm);
			}
		}
	}

	public void setStatusCell(CellFarm c, int index)
	{
		if (c.level == 2)
		{
			c.status = (sbyte)typeCell1[index];
		}
		else
		{
			c.status = typeCell[index];
		}
	}

	protected void doKhoGiong()
	{
		if (itemSeed.size() == 0)
		{
			Canvas.startOKDlg(T.StoreEmtpy);
		}
		else
		{
			if (action != -1)
			{
				return;
			}
			MyVector myVector = new MyVector();
			int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
			CellFarm cellFarm = (CellFarm)cell.elementAt(posTreeByFocus);
			CellFarm cellFarm2 = null;
			if (posTreeByFocus > 0)
			{
				cellFarm2 = (CellFarm)cell.elementAt(posTreeByFocus - 1);
			}
			try
			{
				for (int i = 0; i < itemSeed.size(); i++)
				{
					Item item = (Item)itemSeed.elementAt(i);
					if (FarmData.getTreeByID(item.ID) != null)
					{
						myVector.addElement(new CommandKhoGiong2(item.name + "(" + item.number + ")", new IActionSeed(i, posTreeByFocus), item));
					}
				}
				if ((cellFarm.level == 1 && posTreeByFocus == 0) || (posTreeByFocus > 0 && cellFarm.level < cellFarm2.level))
				{
					myVector.addElement(new CommandKhoGiong(T.update, 11));
				}
			}
			catch (Exception e)
			{
				Out.logError(e);
			}
			startMenuFarm(myVector);
		}
	}

	protected void doRequestPricePlant()
	{
		Canvas.startOKDlg(T.gettingPrice);
		FarmService.gI().doRequestPricePlant(idFarm);
	}

	public void onPricePlant(string str)
	{
		Canvas.msgdlg.setInfoLCR(str, new Command(T.xu, 1, this), new Command(T.gold, 2, this), Canvas.cmdEndDlg);
	}

	public void doVatPham(CellFarm cell)
	{
		int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
		CellFarm cellFarm = (CellFarm)FarmScr.cell.elementAt(posTreeByFocus);
		CellFarm cellFarm2 = null;
		if (posTreeByFocus > 0)
		{
			cellFarm2 = (CellFarm)FarmScr.cell.elementAt(posTreeByFocus - 1);
		}
		Command command = null;
		if ((cellFarm.level == 1 && posTreeByFocus == 0) || (posTreeByFocus > 0 && cellFarm.level < cellFarm2.level))
		{
			command = new CommandVatPham1(T.update, 11);
		}
		if (cell.idTree != -1 && cell.statusTree < 6 && cell.isArid)
		{
			setAction(new IActionVatPham1());
		}
		if (cell.idTree == -1 || cell.statusTree >= 6)
		{
			IAction ac = new IActionVatPham2(cell);
			if (command != null)
			{
				MyVector myVector = new MyVector();
				myVector.addElement(new CommandVatPham2(T.land, ac, 0));
				myVector.addElement(command);
				startMenuFarm(myVector);
				return;
			}
			setAction(ac);
		}
		if (cell.idTree != -1 && cell.statusTree < 6 && posTreeByFocus < FarmScr.cell.size() && listItemFarm.size() > 0)
		{
			if (cell.isWorm)
			{
				setBonPhan(cell, posTreeByFocus, 7);
			}
			if (cell.isGrass)
			{
				setBonPhan(cell, posTreeByFocus, 3);
			}
		}
		if (action != -1)
		{
			return;
		}
		MyVector myVector2 = new MyVector();
		Command o = new CommandVatPham2(T.watering, 1, 2);
		myVector2.addElement(o);
		if (idFarm == GameMidlet.avatar.IDDB)
		{
			myVector2.addElement(new CommandVatPham2(T.land, new IActionVatPham2(cell), 1));
		}
		if (command != null)
		{
			myVector2.addElement(command);
		}
		for (int i = 0; i < listItemFarm.size(); i++)
		{
			Item item = (Item)listItemFarm.elementAt(i);
			FarmItem farmItem = getFarmItem(item.ID);
			if (farmItem.type == 0 && ((farmItem.action == 3 && cell.isGrass) || (farmItem.action == 7 && cell.isWorm) || (farmItem.action != 3 && farmItem.action != 7)))
			{
				string caption = farmItem.des + "(" + item.number + ")";
				myVector2.addElement(new iCommandItemFarm(caption, 6, i, farmItem, this));
			}
		}
		startMenuFarm(myVector2);
	}

	private void doVatPhamAnimal()
	{
		Animal animalByIndex = getAnimalByIndex(((Base)LoadMap.focusObj).IDDB);
		AnimalInfo animalByID = FarmData.getAnimalByID(animalByIndex.species);
		bool flag = false;
		if (false || action != -1)
		{
			return;
		}
		MyVector myVector = new MyVector();
		for (int i = 0; i < listItemFarm.size(); i++)
		{
			Item item = (Item)listItemFarm.elementAt(i);
			FarmItem farmItem = getFarmItem(item.ID);
			if (farmItem.action != 5 && farmItem.type != 0 && (farmItem.type == animalByID.area || farmItem.type == 101 || (farmItem.type == 100 && animalByID.area != 4)))
			{
				myVector.addElement(new CommandThuoc(farmItem.des + "(" + item.number + ")", new IActionThuoc(farmItem, item), farmItem));
			}
		}
		for (int j = 0; j < listItemFarm.size(); j++)
		{
			Item item2 = (Item)listItemFarm.elementAt(j);
			FarmItem farmItem2 = getFarmItem(item2.ID);
			if (farmItem2.type == animalByID.area && farmItem2.action == 5 && (animalByID.area == 4 || animalByID.area == 1))
			{
				int num = item2.number;
				if (animalByID.area == 4)
				{
					num -= listFood[1].size();
				}
				else if (animalByID.area == 1)
				{
					num -= listFood[0].size();
				}
				if (farmItem2.action != 4 || animalByIndex.disease[0] || animalByIndex.disease[1])
				{
					myVector.addElement(new CommandItem5(farmItem2.des + "(" + num + ")", new IActionItem5(farmItem2, animalByID, item2), farmItem2));
				}
			}
		}
		if (idFarm == GameMidlet.avatar.IDDB)
		{
			myVector.addElement(new CommandSellAnimal(T.sell, 3, this));
		}
		int num2 = MyScreen.hText + 10;
		Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - num2 - 10, num2, num2);
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		switch (index)
		{
		case 0:
			doExit();
			break;
		case 1:
			FarmService.gI().doOpenLand(idFarm, 1);
			curTime = Environment.TickCount;
			doJoinFarm(idFarm, true);
			break;
		case 2:
			FarmService.gI().doOpenLand(idFarm, 2);
			curTime = Environment.TickCount;
			doJoinFarm(idFarm, true);
			break;
		case 3:
			if (LoadMap.focusObj != null)
			{
				Canvas.endDlg();
				FarmService.gI().doRequestPriceAnimal(idFarm, ((Base)LoadMap.focusObj).IDDB);
			}
			break;
		case 5:
		{
			if (LoadMap.focusObj == null)
			{
				break;
			}
			Animal animalByIndex = getAnimalByIndex(((Base)LoadMap.focusObj).IDDB);
			AnimalInfo animalByID = FarmData.getAnimalByID(animalByIndex.species);
			for (int k = 0; k < listItemFarm.size(); k++)
			{
				if (subIndex == k)
				{
					Item item2 = (Item)listItemFarm.elementAt(k);
					doUsingVatPhamAnimal(item2, (animalByID.area != 1) ? 1 : 0);
				}
			}
			break;
		}
		case 6:
		{
			for (int i = 0; i < listItemFarm.size(); i++)
			{
				if (i == subIndex)
				{
					Item item = (Item)listItemFarm.elementAt(i);
					if (item.number > 0)
					{
						doUsingVatPham(item.ID, item);
					}
					else
					{
						Canvas.startOKDlg(T.empty + item.name);
					}
				}
			}
			break;
		}
		case 7:
			FarmService.gI().doUpdateStarFruil(1);
			break;
		case 8:
			FarmService.gI().doUpdateStarFruitByMoney(1);
			break;
		case 9:
			FarmService.gI().doUpdateLand(1, 1);
			break;
		case 10:
			FarmService.gI().doUpdateLand(1, 2);
			break;
		case 11:
			FarmService.gI().nauNhanh(1);
			break;
		case 12:
			Canvas.msgdlg.setInfoLR("Bạn có muốn nâng cấp kho hàng không ?", new Command(T.yes, 15, this), Canvas.cmdEndDlg);
			break;
		case 13:
			FarmService.gI().doUpdateStore(1, 1);
			break;
		case 14:
			FarmService.gI().doUpdateStore(1, 2);
			break;
		case 15:
			FarmService.gI().doUpdateStore(0, -1);
			break;
		case 16:
			ListScr.gI().setFriendList(true);
			break;
		case 17:
			FarmService.gI().doStealInfo();
			break;
		case 18:
			gI().doGoFarmWay();
			break;
		case 19:
			FarmService.gI().doLichSuAnTrom();
			break;
		case 20:
			isAbleSteal = true;
			left = null;
			center = null;
			break;
		case 21:
			FarmService.gI().doSteal(0);
			CustomTab.gI().close();
			Canvas.startWaitDlg();
			break;
		case 22:
			PopupShop.gI().close();
			if (remainTime == 0)
			{
				FarmService.gI().doHarvestCook();
			}
			else
			{
				FarmService.gI().nauNhanh(0);
			}
			break;
		case 23:
			Canvas.startOKDlg(T.doUWantCancel, 24, this);
			break;
		case 24:
			FarmService.gI().doCooking(-1);
			PopupShop.gI().close();
			break;
		case 25:
			commandTab(5);
			doGieoHat();
			break;
		case 26:
			isChamSoc = true;
			setAuto(0);
			break;
		case 27:
		{
			isAutoVatNuoi = true;
			for (int j = indexAuto; j < animalLists.size(); j++)
			{
				Animal pet = (Animal)animalLists.elementAt(j);
				if (doAutoVatNuoi(pet))
				{
					return;
				}
				indexAuto++;
				if (indexAuto == animalLists.size())
				{
					indexAuto = 0;
				}
			}
			commandActionPointer(29, -1);
			Canvas.startOKDlg("Không có vật nuôi nào bị bệnh");
			break;
		}
		case 28:
			indexAuto++;
			if (indexAuto == animalLists.size())
			{
				indexAuto = 0;
			}
			commandActionPointer(27, -1);
			break;
		case 29:
			isAutoVatNuoi = false;
			right = null;
			center = null;
			left = null;
			indexAuto = 0;
			AvCamera.isFollow = false;
			break;
		case 4:
			break;
		}
	}

	private void setActionAnimal(FarmItem fItem, short ID, Animal ani)
	{
		setAction(new IActionSetAnimal(fItem, ID, ani));
	}

	protected void doUsingVatPhamAnimal(Item item, int typeItem)
	{
		int num = ((GameMidlet.avatar.direct == Base.RIGHT) ? 1 : (-1));
		int num2 = listFood[typeItem].size();
		if (item.number - num2 <= 0)
		{
			Canvas.startOKDlg(T.foodForEmpty);
			return;
		}
		for (int i = 0; i < 3 && i < item.number - num2; i++)
		{
			Point point = new Point(GameMidlet.avatar.x, GameMidlet.avatar.y - 40);
			FarmItem farmItem = getFarmItem(item.ID);
			point.itemID = item.ID;
			point.w = (point.h = 2);
			point.g = -(4 + CRes.rnd(3));
			point.v = num * (2 + CRes.rnd(3));
			point.limitY = GameMidlet.avatar.y - 20 + CRes.rnd(4) * 5;
			if (farmItem.type == 4)
			{
				int num3 = LoadMap.getposMap(GameMidlet.avatar.x, GameMidlet.avatar.y + 23);
				if (LoadMap.map[num3] == 14)
				{
					point.limitY = 50 + CRes.rnd(50);
					point.v = num * CRes.rnd(3);
				}
			}
			point.layer = new PoLayer(point);
			listFood[typeItem].addElement(point);
			LoadMap.dynamicLists.addElement(point);
		}
	}

	protected void doUsingVatPham(int index, Item it)
	{
		int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
		if (posTreeByFocus < cell.size() && listItemFarm.size() != 0)
		{
			FarmItem farmItem = getFarmItem(it.ID);
			sbyte b = farmItem.action;
			if (b == 7)
			{
				b = 3;
			}
			setAction(b, farmItem.ID);
			FarmService.gI().doUsingItem(idFarm, posTreeByFocus, farmItem.ID);
		}
	}

	public static void startTextSmall(int fir, int cur, CellFarm c, Animal ani)
	{
		if (LoadMap.TYPEMAP != 25 && fir != cur)
		{
			string text = string.Empty;
			if (cur - fir > 0)
			{
				text += "+";
			}
			int x;
			int y;
			if (c != null)
			{
				x = c.xCell * LoadMap.w + LoadMap.w / 2;
				y = c.yCell * LoadMap.w - LoadMap.w / 2;
			}
			else
			{
				x = ani.x;
				y = ani.y - 30;
			}
			Canvas.addFlyTextSmall(text + (cur - fir), x, y, -1, 0, -1);
		}
	}

	protected void doHarvest()
	{
		if (isSteal || GameMidlet.avatar.IDDB == idFarm)
		{
			int posTreeByFocus = getPosTreeByFocus(focusCell.x, focusCell.y);
			CellFarm cellFarm = (CellFarm)cell.elementAt(posTreeByFocus);
			SoundManager.playSound(6);
			FarmService.gI().doHervest(idFarm, posTreeByFocus);
		}
	}

	private void doPlantSeed(int index, int pos)
	{
		if (Canvas.isInitChar)
		{
			Welcome.goFarm();
		}
		Item item = (Item)itemSeed.elementAt(index);
		FarmService.gI().doPlantSeed(idFarm, pos, item.ID);
		if (item.number <= 1)
		{
			Canvas.startOKDlg("Bạn đã hết hạt giống, xin vào cửa hàng đễ mua.");
			commandTab(5);
		}
	}

	public int getPosTreeByFocus(int x, int y)
	{
		for (int i = 0; i < posTree.Length; i++)
		{
			for (int j = 0; j < numO; j++)
			{
				int num = posTree[i].x + j / numH;
				int num2 = posTree[i].y + j % numH;
				if (x == num && y == num2)
				{
					return i * numO + j;
				}
			}
		}
		return -1;
	}

	private void setAction(sbyte ac, int idItemUsing)
	{
		FarmScr.idItemUsing = idItemUsing;
		action = ac;
		GameMidlet.avatar.task = -1;
		GameMidlet.avatar.idFrom = -1;
		GameMidlet.avatar.idTo = -1;
		if (action == 4)
		{
			posDoing = new AvPosition(LoadMap.focusObj.x / LoadMap.w, LoadMap.focusObj.y / LoadMap.w);
		}
		else
		{
			posDoing = new AvPosition(focusCell.x, focusCell.y);
		}
		GameMidlet.avatar.yCur = posDoing.y * LoadMap.w + LoadMap.w / 2;
		GameMidlet.avatar.xCur = posDoing.x * LoadMap.w;
		if (GameMidlet.avatar.direct == Base.LEFT)
		{
			GameMidlet.avatar.xCur += LoadMap.w;
		}
	}

	private void doLamDat(CellFarm c)
	{
		IAction action = new IActionCuocDat(this);
		if (c.idTree == -1 || c.statusTree >= 6)
		{
			action.perform();
			SoundManager.playSound(4);
		}
		else
		{
			Canvas.startOKDlg(T.youWantBreakTree, action);
		}
	}

	private void updateDoing()
	{
		if (action != -1 && timeDoing == -1 && GameMidlet.avatar.action == 0)
		{
			timeDoing = Environment.TickCount / 100;
			int num = -1;
			if (posDoing != null)
			{
				num = getPosTreeByFocus(posDoing.x, posDoing.y);
			}
			if (action == 4)
			{
				num = 0;
			}
			if (posDoing.x * LoadMap.w < GameMidlet.avatar.x)
			{
				GameMidlet.avatar.direct = Base.LEFT;
			}
			else
			{
				GameMidlet.avatar.direct = Base.RIGHT;
			}
			GameMidlet.avatar.dirFirst = GameMidlet.avatar.direct;
			if (aniDoing != null)
			{
				aniDoing.isStand = false;
				aniDoing = null;
			}
			if (num == -1)
			{
				resetAction();
				return;
			}
			SubObject subObject = new SubObject(-2, GameMidlet.avatar.x, GameMidlet.avatar.y - 5, img.frameWidth, img.frameHeight);
			LoadMap.treeLists.addElement(subObject);
			int num2 = 0;
			if (action == 0)
			{
				num2 = 5;
				subObject.y = GameMidlet.avatar.y - 8;
			}
			if (GameMidlet.avatar.direct == Base.RIGHT)
			{
				subObject.x = GameMidlet.avatar.x + 10 + num2;
			}
			else
			{
				subObject.x = GameMidlet.avatar.x - 10 - num2;
			}
		}
		if (timeDoing != -1 && (action == 1 || action == 0 || action == 2) && Environment.TickCount / 100 - timeDoing > 2)
		{
			timeDoing = Environment.TickCount / 100;
			if (GameMidlet.avatar.action == 6)
			{
				GameMidlet.avatar.setAction(0);
			}
			else
			{
				GameMidlet.avatar.setAction(6);
			}
		}
	}

	public void reset()
	{
		focusCell = new AvPosition();
		action = -1;
		timeLimit = 0;
		Cattle.itemID = -1;
		Dog.itemID = -1;
	}

	public void setCellAll()
	{
		for (int i = 0; i < posTree.Length; i++)
		{
			for (int j = 0; j < numO; j++)
			{
				int num = posTree[i].x + j / numH;
				int num2 = posTree[i].y + j % numH;
				if (i * numO + j < cell.size())
				{
					LoadMap.setType(num, num2, 51);
					CellFarm cellFarm = (CellFarm)cell.elementAt(i * numO + j);
					cellFarm.xCell = num;
					cellFarm.yCell = num2;
					cellFarm.x = num * LoadMap.w + LoadMap.w / 2;
					cellFarm.y = num2 * LoadMap.w + 18;
					setInfoCell(i * numO + j);
					LoadMap.treeLists.addElement(cellFarm);
					continue;
				}
				if (i * numO + j == cell.size())
				{
					LoadMap.treeLists.addElement(new SubObject(-3, num * LoadMap.w + 20, num2 * LoadMap.w + 20, imgBuyLant.getWidth()));
					LoadMap.setType(num, num2, 51);
					LoadMap.orderVector(LoadMap.treeLists);
				}
				if (LoadMap.map[num2 * LoadMap.wMap + num] == (byte)typeCell[0])
				{
					LoadMap.orderVector(LoadMap.treeLists);
					return;
				}
				if (num == posTree[i].x && num2 == posTree[i].y)
				{
					LoadMap.map[num2 * LoadMap.wMap + num] = 4;
				}
			}
		}
		LoadMap.orderVector(LoadMap.treeLists);
	}

	public int setCell(int x, int y)
	{
		int num = cell.size();
		for (int i = 0; i < num; i++)
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(i);
			if (cellFarm.xCell == x && cellFarm.yCell == y)
			{
				return i;
			}
		}
		return -1;
	}

	public override void update()
	{
		Canvas.paint.setSoundAnimalFarm();
		t++;
		if (t >= 10)
		{
			t = 0;
		}
		if (n >= 8)
		{
			n = 0;
		}
		n++;
		if (action != -1)
		{
			frame = FRAME[action][t];
			timeLimit++;
			if (timeLimit > 20)
			{
				timeLimit = 0;
				resetAction();
			}
		}
		updateTime();
		Canvas.loadMap.update();
		if (!isAutoVatNuoi && !isSelected)
		{
			setFocus();
		}
		updateDoing();
		if ((LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53) && animalLists.size() > 0)
		{
			updateStatusAnimal();
		}
	}

	public void updateStatusAnimal()
	{
		if (OptionScr.instance.volume > 0 && animalLists.size() > 0)
		{
			tempTime++;
			if (tempTime >= repeatTime)
			{
				tempTime = 0;
				int i = CRes.rnd(animalLists.size());
				Animal animal = (Animal)animalLists.elementAt(i);
				if (animalLists.size() < 4)
				{
					repeatTime = 500;
				}
				else
				{
					repeatTime = 350;
				}
				switch (animal.species)
				{
				case 51:
					SoundManager.playSound(70);
					break;
				case 53:
					SoundManager.playSound(71);
					break;
				case 52:
					SoundManager.playSound(72);
					break;
				default:
					SoundManager.playSound(73);
					break;
				}
			}
		}
		numStatusAnimal++;
		if (numStatusAnimal <= 250)
		{
			return;
		}
		numStatusAnimal = 0;
		int i2 = CRes.rnd(animalLists.size());
		Animal animal2 = (Animal)animalLists.elementAt(i2);
		string text = string.Empty;
		if (animal2.disease[0])
		{
			text += T.diarrhea;
		}
		if (animal2.disease[1])
		{
			if (!text.Equals(string.Empty))
			{
				text += ", ";
			}
			text += T.flu;
		}
		if (animal2.hunger)
		{
			if (!text.Equals(string.Empty))
			{
				text += ", ";
			}
			text += T.hunger;
		}
		if (animal2.health < 20)
		{
			if (!text.Equals(string.Empty))
			{
				text += ", ";
			}
			text += T.tire;
		}
		if (!text.Equals(string.Empty))
		{
			animal2.chat = new ChatPopup(25, text, 0);
			animal2.chat.setPos(animal2.x, animal2.y - 45);
		}
	}

	public void resetAction()
	{
		for (int i = 0; i < LoadMap.treeLists.size(); i++)
		{
			SubObject subObject = (SubObject)LoadMap.treeLists.elementAt(i);
			if (subObject.type == -2)
			{
				LoadMap.treeLists.removeElementAt(i);
				if (i > 0)
				{
					i--;
				}
			}
		}
		timeDoing = -1L;
		int num = -1;
		if (posDoing != null)
		{
			num = setCell(posDoing.x, posDoing.y);
		}
		if (num == -1)
		{
			action = -1;
			GameMidlet.avatar.action = 0;
			GameMidlet.avatar.task = 0;
			doAction();
			return;
		}
		if (idItemUsing == -1)
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(num);
			switch (action)
			{
			case 0:
				setStatusCell(cellFarm, 1);
				cellFarm.statusTree = 0;
				LoadMap.map[cellFarm.yCell * LoadMap.wMap + cellFarm.xCell] = cellFarm.status;
				if (cellFarm.idTree != -1)
				{
					FarmService.gI().doPlantSeed(idFarm, num, -1);
				}
				cellFarm.idTree = -1;
				if (Canvas.isInitChar)
				{
					Canvas.welcome = new Welcome();
					Canvas.welcome.initFarm();
				}
				break;
			case 1:
				setStatusCell(cellFarm, 4);
				cellFarm.isArid = false;
				LoadMap.map[cellFarm.yCell * LoadMap.wMap + cellFarm.xCell] = cellFarm.status;
				FarmService.gI().doUsingItem(idFarm, num, 100);
				break;
			}
		}
		idItemUsing = -1;
		posDoing = null;
		action = -1;
		GameMidlet.avatar.task = 0;
		GameMidlet.avatar.action = 0;
		doAction();
	}

	private void doAction()
	{
		if (isAutoVatNuoi)
		{
			commandAction(10);
		}
		else if (listAction.size() > 0)
		{
			IAction action = (IAction)listAction.elementAt(0);
			action.perform();
			listAction.removeElement(action);
		}
		else if (isChamSoc)
		{
			setGieoHat();
		}
	}

	private void setFocus()
	{
		if (LoadMap.TYPEMAP == 25)
		{
			return;
		}
		int num = ((GameMidlet.avatar.direct != Base.LEFT) ? (GameMidlet.avatar.x + 23) : (GameMidlet.avatar.x - 23));
		num /= LoadMap.w;
		int num2 = GameMidlet.avatar.y / LoadMap.w;
		int num3 = LoadMap.type[num2 * LoadMap.wMap + num];
		int posTreeByFocus = getPosTreeByFocus(num, num2);
		if (num3 == 51 && posTreeByFocus <= cell.size())
		{
			focusCell.x = num;
			focusCell.y = num2;
			if (action != 0 && action != 1)
			{
				cmdSelected = cmdMenu;
			}
			else
			{
				cmdSelected = null;
			}
			return;
		}
		if (cmdSelected == cmdMenu || cmdSelected == cmdFeeding)
		{
			cmdSelected = null;
		}
		focusCell.x = -1;
		focusCell.y = -1;
		if (LoadMap.focusObj != null || !setFeeding())
		{
			if (LoadMap.focusObj != null && cmdSelected == null)
			{
				cmdSelected = cmdFocusBet;
			}
			if (LoadMap.focusObj == null && cmdSelected == cmdFocusBet)
			{
				cmdSelected = null;
				right = null;
			}
		}
	}

	private bool setFeeding()
	{
		int num = LoadMap.getposMap(GameMidlet.avatar.x + 12, GameMidlet.avatar.y);
		int num2 = LoadMap.getposMap(GameMidlet.avatar.x, GameMidlet.avatar.y + 12);
		if ((LoadMap.map[num] == 100 && GameMidlet.avatar.direct == Base.RIGHT) || LoadMap.map[num2] == 14)
		{
			cmdSelected = cmdFeeding;
			return true;
		}
		cmdSelected = null;
		return false;
	}

	public override void updateKey()
	{
		updatePoint();
		base.updateKey();
		Canvas.loadMap.updateKey();
		if (action == -1)
		{
			GameMidlet.avatar.updateKey();
		}
	}

	private void updatePoint()
	{
		if (isTrans && GameMidlet.avatar.action == 0 && GameMidlet.avatar.task == 0 && GameMidlet.avatar.x == GameMidlet.avatar.xCur && GameMidlet.avatar.y == GameMidlet.avatar.yCur)
		{
			isTrans = false;
			GameMidlet.avatar.direct = Base.RIGHT;
			setFocus();
			if (action == -1)
			{
				if (indexItem != -1)
				{
					setDoing();
				}
				else
				{
					indexItem = -1;
					doSellect();
				}
			}
		}
		if (Canvas.isPointerClick)
		{
			int num = (int)((float)Canvas.px / AvMain.zoom + AvCamera.gI().xCam);
			int num2 = (int)((float)Canvas.py / AvMain.zoom + AvCamera.gI().yCam);
			int num3 = LoadMap.w * AvMain.hd;
			if (num2 / num3 * LoadMap.wMap + num / num3 >= 0 && num2 / num3 * LoadMap.wMap + num / num3 <= LoadMap.type.Length)
			{
				int num4 = LoadMap.type[num2 / num3 * LoadMap.wMap + num / num3];
				if (num4 == 51)
				{
					int posTreeByFocus = getPosTreeByFocus(num / num3, num2 / num3);
					if (posTreeByFocus == cell.size())
					{
						doRequestPricePlant();
						return;
					}
					isTran = true;
					isSelected = true;
					if (posTreeByFocus >= 0 && posTreeByFocus < cell.size())
					{
						CellFarm cellFarm = (CellFarm)cell.elementAt(posTreeByFocus);
						focusCell.x = cellFarm.x / LoadMap.w;
						focusCell.y = cellFarm.y / LoadMap.w;
					}
				}
			}
		}
		if (!isTran || !Canvas.isPointerRelease)
		{
			return;
		}
		isTran = false;
		isSelected = false;
		int num5 = (int)((float)Canvas.px / AvMain.zoom + AvCamera.gI().xCam);
		int num6 = (int)((float)Canvas.py / AvMain.zoom + AvCamera.gI().yCam);
		int num7 = LoadMap.w * AvMain.hd;
		if (isAbleSteal && center != null && focusCell != null && num5 / num7 == focusCell.x && num6 / num7 == focusCell.y)
		{
			center.perform();
		}
		else
		{
			if (num6 / num7 * LoadMap.wMap + num5 / num7 < 0 || num6 / num7 * LoadMap.wMap + num5 / num7 > LoadMap.type.Length)
			{
				return;
			}
			int num8 = LoadMap.type[num6 / num7 * LoadMap.wMap + num5 / num7];
			if (num8 != 51)
			{
				return;
			}
			int posTreeByFocus2 = getPosTreeByFocus(num5 / num7, num6 / num7);
			if (posTreeByFocus2 < 0 || posTreeByFocus2 >= cell.size())
			{
				return;
			}
			CellFarm cellFarm2 = null;
			cellFarm2 = (CellFarm)cell.elementAt(posTreeByFocus2);
			focusCell.x = cellFarm2.x / LoadMap.w;
			focusCell.y = cellFarm2.y / LoadMap.w;
			if (isSelectedCell && posTreeByFocus2 >= 0 && posTreeByFocus2 < cell.size())
			{
				idSelected = posTreeByFocus2;
				if (cellFarm2.idTree == -1 || cellFarm2.statusTree == 5 || cellFarm2.statusTree >= 6)
				{
					Canvas.isPointerRelease = false;
					if (isChamSoc && cellFarm2.statusTree != 5)
					{
						Canvas.startOKDlg("ô này không có cây.");
						return;
					}
					if (!cellFarm2.isSelected)
					{
						listSelectedCell.addElement(new AvPosition(num5 / LoadMap.w, num6 / LoadMap.w, posTreeByFocus2));
					}
					cellFarm2.isSelected = true;
					setGieoHat();
					return;
				}
				Canvas.isPointerRelease = false;
				if (isChamSoc)
				{
					if (!cellFarm2.isSelected)
					{
						listSelectedCell.addElement(new AvPosition(num5 / LoadMap.w, num6 / LoadMap.w, posTreeByFocus2));
					}
					cellFarm2.isSelected = true;
					setGieoHat();
				}
				else if (cellFarm2.statusTree != 5)
				{
					Canvas.startOKDlg("Hãy chọn ô không có cây hoặc cây đã chết.");
				}
			}
			else
			{
				Canvas.px -= (int)((float)(LoadMap.w * AvMain.hd) * AvMain.zoom);
				Canvas.pxLast = Canvas.px;
				isTrans = true;
			}
		}
	}

	private void setGieoHat()
	{
		if (listSelectedCell.size() > 0 && indexItem != -1)
		{
			isTrans = true;
			AvPosition avPosition = (AvPosition)listSelectedCell.elementAt(0);
			LoadMap.posFocus.x = avPosition.x * 24 - 24;
			LoadMap.posFocus.y = avPosition.y * 24 + 12;
			GameMidlet.avatar.task = -5;
			GameMidlet.avatar.isJumps = -1;
			GameMidlet.avatar.xCur = GameMidlet.avatar.x;
			GameMidlet.avatar.yCur = GameMidlet.avatar.y;
			Canvas.loadMap.change();
		}
	}

	private void setDoing()
	{
		if (listSelectedCell.size() <= 0 || indexItem == -1)
		{
			return;
		}
		AvPosition avPosition = (AvPosition)listSelectedCell.elementAt(0);
		CellFarm cellFarm = (CellFarm)cell.elementAt(avPosition.anchor);
		cellFarm.isSelected = false;
		focusCell.x = cellFarm.x / LoadMap.w;
		focusCell.y = cellFarm.y / LoadMap.w;
		if (isChamSoc)
		{
			if (cellFarm.statusTree == 5)
			{
				doHarvest();
				setGieoHat();
			}
			else
			{
				bool flag = false;
				if (cellFarm.idTree != -1 && cellFarm.statusTree < 6 && cellFarm.status == 36)
				{
					setAction(new IActionSet1(cellFarm));
					flag = true;
				}
				if (cellFarm.idTree != -1 && cellFarm.statusTree < 6)
				{
					if (avPosition.anchor >= cell.size())
					{
						return;
					}
					setAction(new IActionVatPham1());
					if (cellFarm.isWorm && setBonPhan(cellFarm, avPosition.anchor, 7))
					{
						flag = true;
					}
					if (cellFarm.isGrass && setBonPhan(cellFarm, avPosition.anchor, 3))
					{
						flag = true;
					}
					if (cellFarm.vitalityPer < 50)
					{
						bool flag2 = false;
						for (int i = 0; i < listItemFarm.size(); i++)
						{
							Item item = (Item)listItemFarm.elementAt(i);
							FarmItem farmItem = getFarmItem(item.ID);
							if (farmItem.action == 2)
							{
								FarmService.gI().doUsingItem(idFarm, avPosition.anchor, farmItem.ID);
								break;
							}
							flag2 = true;
						}
						if (!flag2)
						{
							Canvas.startOKDlg("Kho của bạn đã hết phân bón, xin vào cửa hàng đễ mua.");
						}
					}
				}
				if (!flag)
				{
					setGieoHat();
				}
			}
		}
		else if (cellFarm.statusTree == 5)
		{
			doHarvest();
			setGieoHat();
		}
		else
		{
			setAction(new IActionSet2(cellFarm));
			setAction(new IActionSet3(avPosition));
		}
		listSelectedCell.removeElement(avPosition);
	}

	private void setAction(IAction ac)
	{
		if (action != -1)
		{
			listAction.addElement(ac);
		}
		else
		{
			ac.perform();
		}
	}

	public bool setBonPhan(CellFarm c, int pos, int action)
	{
		bool flag = false;
		for (int i = 0; i < listItemFarm.size(); i++)
		{
			Item item = (Item)listItemFarm.elementAt(i);
			FarmItem farmItem = getFarmItem(item.ID);
			if (farmItem.type == 0 && farmItem.action == action)
			{
				setAction(new IActionBonPhan(pos, farmItem));
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Canvas.startOKDlg("Kho của bạn đã hết thuốc diệt cỏ, xin vào cửa hàng đễ mua.");
		}
		return flag;
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

	public override void paintMain(MyGraphics g)
	{
		GUIUtility.ScaleAroundPivot(Vector2.one * AvMain.zoom, Vector2.zero);
		Canvas.resetTrans(g);
		Canvas.paint.paintBGCMD(g, 0, Canvas.h, Canvas.w, Canvas.hTab);
		Canvas.resetTrans(g);
		g.translate(0f - AvCamera.gI().xCam, 0f - AvCamera.gI().yCam);
		Canvas.loadMap.paintBackGround(g);
		if (LoadMap.imgCreateMap != null)
		{
			int num = 0;
			int num2 = numBarn;
			int num3 = numPond;
			for (int i = 0; i < LoadMap.imgCreateMap.Length; i++)
			{
				if (AvCamera.gI().xCam + (float)Canvas.w > (float)num && AvCamera.gI().xCam < (float)(num + LoadMap.imgCreateMap[i].w))
				{
					g.drawImage(LoadMap.imgCreateMap[i], num - 1, LoadMap.Hmap * LoadMap.w * AvMain.hd - LoadMap.imgCreateMap[i].h, 0);
				}
				num += LoadMap.imgCreateMap[i].w - 2;
				if (i == 1 && num2 > 0)
				{
					i--;
					num2--;
				}
				else if (i == 4 && num3 > 0)
				{
					i--;
					num3--;
				}
			}
		}
		for (int j = 0; j < cell.size(); j++)
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(j);
			g.drawImage(imgCell[cellFarm.status], cellFarm.x * AvMain.hd - 14 * AvMain.hd, cellFarm.y * AvMain.hd - 20 * AvMain.hd, 0);
		}
		Canvas.loadMap.paintTouchMap(g);
		Canvas.loadMap.paintObject(g);
		paintFocus(g);
		paintName(g);
		Canvas.resetTrans(g);
		GUIUtility.ScaleAroundPivot(Vector2.one / AvMain.zoom, Vector2.zero);
		Canvas.loadMap.paintEffectCamera(g);
	}

	private void paintName(MyGraphics g)
	{
		if (LoadMap.TYPEMAP != 25)
		{
			Canvas.smallFontYellow.drawString(g, nameFarm, (posName.x + 27) * AvMain.hd, (posName.y - 14) * AvMain.hd + (AvMain.hd - 1) * 7 - 2, 2);
		}
	}

	public void paintFocus(MyGraphics g)
	{
		if (idSelected == -1)
		{
			if (focusCell != null && focusCell.x != -1 && LoadMap.TYPEMAP == 25)
			{
			}
		}
		else if (focusCell != null && focusCell.x != -1 && LoadMap.TYPEMAP != 25)
		{
			g.drawImage(MapScr.imgFocusP, (focusCell.x * LoadMap.w + LoadMap.w / 2) * AvMain.hd, (focusCell.y * LoadMap.w - 4 + n / 2) * AvMain.hd, 3);
		}
	}

	public void onInventory(MyVector itemMua, MyVector itemProduct1, MyVector itemVP, MyVector itemFarm, MyVector listFarmProdut, sbyte levelStore, int capacity, bool isNew)
	{
		itemSeed = itemMua;
		FarmScr.isNew = isNew;
		FarmScr.levelStore = levelStore;
		capacityStore = capacity;
		int num = itemSeed.size();
		for (int i = 0; i < num; i++)
		{
			Item item = (Item)itemSeed.elementAt(i);
			TreeInfo treeByID = FarmData.getTreeByID(item.ID);
			if (treeByID != null)
			{
				item.name = treeByID.name;
			}
		}
		itemProduct = itemProduct1;
		int num2 = itemProduct.size();
		for (int j = 0; j < num2; j++)
		{
			Item nameItem = (Item)itemProduct.elementAt(j);
			setNameItem(nameItem);
		}
		listItemFarm = itemFarm;
		listFarmProduct = listFarmProdut;
		initImg();
	}

	private static void setNameItem(Item item)
	{
		if (item.ID < 50)
		{
			item.price[0] = FarmData.getTreeByID(item.ID).priceProduct;
			item.name = FarmData.getTreeByID(item.ID).name;
		}
		else
		{
			if (item.ID >= 100)
			{
				return;
			}
			item.price[0] = FarmData.getAnimalByID(item.ID).priceProduct;
			if (FarmData.getAnimalByID(item.ID).area == 1)
			{
				item.name = T.egg + " " + FarmData.getAnimalByID(item.ID).name;
			}
			else if (FarmData.getAnimalByID(item.ID).area == 2)
			{
				if (item.ID == 55)
				{
					item.name = "Lông " + FarmData.getAnimalByID(item.ID).name;
				}
				else
				{
					item.name = T.milk + " " + FarmData.getAnimalByID(item.ID).name;
				}
			}
		}
	}

	public static FarmItem getFarmItem(int id)
	{
		for (int i = 0; i < FarmData.listItemFarm.size(); i++)
		{
			FarmItem farmItem = (FarmItem)FarmData.listItemFarm.elementAt(i);
			if (farmItem.ID == id)
			{
				return farmItem;
			}
		}
		return null;
	}

	public void onBuyItem(Item item, int newMoney, sbyte typeBuy, int xu, int luong, int luongK)
	{
		GameMidlet.avatar.updateMoney(xu, luong, luongK);
		PopupShop.isTransFocus = true;
		SoundManager.playSound(2);
		if (item.number <= 0)
		{
			return;
		}
		if (item.ID >= 111)
		{
			Item itemByList = Item.getItemByList(listItemFarm, item.ID);
			if (itemByList != null)
			{
				itemByList.number += item.number;
				return;
			}
			FarmItem farmItem = getFarmItem(item.ID);
			item.name = farmItem.des;
			listItemFarm.addElement(item);
		}
		else if (item.ID <= 100 && item.ID < 50)
		{
			Item itemByList2 = Item.getItemByList(itemSeed, item.ID);
			if (itemByList2 != null)
			{
				itemByList2.number += item.number;
			}
			else
			{
				itemSeed.addElement(item);
				item.name = FarmData.getTreeByID(item.ID).name;
			}
			if (itemSeed.size() == 0)
			{
				itemSeed.addElement(item);
			}
		}
	}

	private void updateTime()
	{
		if (remainTime > 0 && Canvas.getTick() / 1000 - curTimeCooking >= 1)
		{
			remainTime--;
			curTimeCooking = (int)(Canvas.getTick() / 1000);
		}
		if (LoadMap.TYPEMAP == 24 && LoadMap.TYPEMAP == 53 && (Environment.TickCount - curTime) / 1000 > 300)
		{
			curTime = Environment.TickCount;
			doJoinFarm(idFarm, true);
		}
	}

	public void onJoin(int idFarm1, MyVector cell1, MyVector listAni, sbyte numBarn, sbyte numPond, short foodID, int remainTime)
	{
		FarmScr.foodID = foodID;
		FarmScr.remainTime = remainTime;
		FarmScr.numBarn = numBarn;
		FarmScr.numPond = numPond;
		idFarm = idFarm1;
		Dog.isCan = false;
		if (idFarm1 != GameMidlet.avatar.IDDB)
		{
			if (!isSteal)
			{
				Avatar avatar = ListScr.getAvatar(idFarm1);
				if (avatar == null)
				{
					Canvas.startOKDlg(T.notOnFarm);
					return;
				}
				if (avatar.showName == null)
				{
					avatar.setName(avatar.name);
				}
				nameFarm = avatar.showName;
			}
			else
			{
				LoadMap.TYPEMAP = 25;
				nameFarm = nameTemp;
			}
			listFood[0].removeAllElements();
			listFood[1].removeAllElements();
		}
		else
		{
			nameFarm = GameMidlet.avatar.showName;
		}
		cell = cell1;
		if (LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53 && animalLists.size() == 0)
		{
			animalLists = listAni;
		}
		setAnimal();
		if (isJoin)
		{
			if (isSteal || isReSize || (LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53))
			{
				isReSize = false;
				reset();
				posTree = new AvPosition[4];
				Canvas.loadMap.load(25, false);
				Canvas.load = 0;
				resizeMap(numBarn, numPond);
				listNest = new MyVector();
				listBucket = new MyVector();
				setChickNest(1, Chicken.posNest, 87, -8, listNest);
				setChickNest(2, Cattle.posBucket, 86, -7, listBucket);
				int num = animalLists.size();
				for (int i = 0; i < num; i++)
				{
					Animal animal = (Animal)animalLists.elementAt(i);
					animal.setInit();
					LoadMap.playerLists.addElement(animal);
				}
				Canvas.load = 1;
				Canvas.endDlg();
			}
			for (int j = 0; j < LoadMap.treeLists.size(); j++)
			{
				SubObject subObject = (SubObject)LoadMap.treeLists.elementAt(j);
				if ((subObject.type < 800 && subObject.type >= 100) || subObject.type == -3 || subObject is CellFarm)
				{
					LoadMap.treeLists.removeElement(subObject);
					j--;
				}
			}
			GameMidlet.avatar.xCur = GameMidlet.avatar.x;
			GameMidlet.avatar.yCur = GameMidlet.avatar.y;
			setCellAll();
			curTime = Environment.TickCount;
			if (Canvas.currentMyScreen != this)
			{
				switchToMe();
			}
			if (Canvas.isInitChar)
			{
				Canvas.welcome = new Welcome();
				Canvas.welcome.initFarm();
			}
		}
		isJoin = true;
		if (xRemember != -1)
		{
			GameMidlet.avatar.x = (GameMidlet.avatar.xCur = xRemember);
			GameMidlet.avatar.y = (GameMidlet.avatar.yCur = yRemember);
			xRemember = -1;
			yRemember = -1;
		}
		if (isSteal)
		{
			left = cmdNextSteal;
			right = cmdCloseSteal;
			center = cmdStreal;
			if (priceSteal != 0)
			{
				GameMidlet.avatar.money[1] -= priceSteal;
				Canvas.addFlyText(-priceSteal, GameMidlet.avatar.x, GameMidlet.avatar.y, -1, -1);
				priceSteal = 0;
			}
		}
		else
		{
			left = null;
			right = null;
			center = null;
		}
		commandTab(5);
		AvCamera.gI().setPosFollowPlayer();
	}

	private void setChickNest(int type, AvPosition pos, sbyte typeMap, int typeObj, MyVector listPos)
	{
		int num = 0;
		for (int i = 0; i < animalLists.size(); i++)
		{
			Animal animal = (Animal)animalLists.elementAt(i);
			AnimalInfo animalByID = FarmData.getAnimalByID(animal.species);
			if (animalByID.area != type || animalByID.iconProduct == -1)
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < listPos.size(); j++)
			{
				AvPosition avPosition = (AvPosition)listPos.elementAt(j);
				if (avPosition.anchor == animal.species)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				listPos.addElement(new AvPosition(pos.x + num * 24, pos.y, animal.species));
				int num2 = LoadMap.getposMap(pos.x + num * 24, pos.y);
				LoadMap.type[num2] = typeMap;
				LoadMap.addObjTree(typeObj, pos.x + num * 24, pos.y, -1);
				if (GameMidlet.avatar.IDDB != idFarm && isSteal && animal.numEggOne > 0)
				{
					LoadMap.addPopup("trom", pos.x + num * 24, pos.y + 12, (typeMap != 87) ? (-51) : (-50));
				}
				num++;
			}
		}
	}

	public void resizeMap(sbyte numBarn, sbyte numPond)
	{
		try
		{
			numTilePond = FishFarm.WTile + numPond;
			numTileBarn = Cattle.numTileW + numBarn;
			int num = posPond.x / 24;
			int num2 = posBarn.x / 24 + 2;
			DataInputStream dataInputStream = LoadMap.loadDataMap(25);
			LoadMap.map = new short[dataInputStream.available()];
			for (int i = 0; i < LoadMap.map.Length; i++)
			{
				LoadMap.map[i] = (byte)dataInputStream.readByte();
			}
			short[] array = new short[LoadMap.map.Length + LoadMap.Hmap * (numPond + numBarn)];
			int num3 = 0;
			for (int j = 0; j < LoadMap.map.Length; j++)
			{
				array[num3] = LoadMap.map[j];
				num3++;
				if (j % LoadMap.wMap == num)
				{
					for (int k = 0; k < numPond; k++)
					{
						array[num3] = LoadMap.map[j];
						num3++;
					}
				}
				if (j % LoadMap.wMap == num2)
				{
					for (int l = 0; l < numBarn; l++)
					{
						array[num3] = LoadMap.map[j];
						num3++;
					}
				}
			}
			LoadMap.wMap += (short)(numPond + numBarn);
			LoadMap.map = array;
			LoadMap.treeLists.removeAllElements();
			Canvas.loadMap.setMap(null, LoadMap.TYPEMAP + 1, true);
			GameMidlet.avatar.x += numBarn * 24;
			LoadMap.addObjTree(849, posPond.x + 12 + CRes.rnd(numTilePond - 2) * 24, posPond.y + 12 + CRes.rnd(3) * 24, -1);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void setAnimal()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < animalLists.size(); i++)
		{
			Animal animal = (Animal)animalLists.elementAt(i);
			AnimalInfo animalByID = FarmData.getAnimalByID(animal.species);
			if (animal is AnimalDan)
			{
				bool flag = false;
				for (int j = 0; j < myVector.size(); j++)
				{
					AvPosition avPosition = (AvPosition)myVector.elementAt(j);
					if (avPosition.anchor == animal.species)
					{
						((AnimalDan)animal).captainID = avPosition.x;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					((AnimalDan)animal).captainID = animal.IDDB;
					myVector.addElement(new AvPosition(animal.IDDB, 0, animal.species));
				}
			}
			int num = animalByID.harvestTime * 60 / 3;
			if (num > 0)
			{
				animal.period = animal.bornTime / num;
			}
			if (animal.period > 2)
			{
				animal.period = 2;
			}
			if (animal.bornTime == -1 || animalByID.area == 3)
			{
				animal.period = 0;
			}
			Canvas.paint.setAnimalSound(animalLists);
		}
	}

	public void onPlantSeed(int idUser, int indexCell, int idSeed)
	{
		if (LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53)
		{
			return;
		}
		Item itemByList = Item.getItemByList(itemSeed, idSeed);
		if (itemByList != null)
		{
			CellFarm cellFarm = (CellFarm)cell.elementAt(indexCell);
			cellFarm.idTree = idSeed;
			setStatusCell(cellFarm, 4);
			LoadMap.map[cellFarm.yCell * LoadMap.wMap + cellFarm.xCell] = cellFarm.status;
			cellFarm.statusTree = 0;
			cellFarm.isGrass = false;
			cellFarm.isWorm = false;
			cellFarm.time = 0;
			cellFarm.tempTime = 0L;
			cellFarm.vitalityPer = 100;
			cellFarm.hervestPer = 0;
			itemByList.number--;
			if (itemByList.number <= 0)
			{
				itemSeed.removeElement(itemByList);
			}
		}
	}

	public void setInfoCell(int index)
	{
		CellFarm cellFarm = (CellFarm)cell.elementAt(index);
		if (cellFarm.idTree == -1)
		{
			setStatusCell(cellFarm, 2);
		}
		else
		{
			TreeInfo treeInfoByID = FarmData.getTreeInfoByID(cellFarm.idTree);
			int num = treeInfoByID.harvestTime * 60 / 5;
			cellFarm.statusTree = cellFarm.time / num;
			if (cellFarm.statusTree >= 5)
			{
				cellFarm.statusTree = 5;
			}
			if (cellFarm.time < 0 || (treeInfoByID.dieTime != -1 && cellFarm.time - treeInfoByID.harvestTime * 60 > treeInfoByID.dieTime * 60) || cellFarm.hervestPer == 100 || cellFarm.statusTree < 0)
			{
				cellFarm.statusTree = 6;
			}
			if (cellFarm.isArid)
			{
				setStatusCell(cellFarm, 3);
			}
			else
			{
				setStatusCell(cellFarm, 4);
			}
		}
		LoadMap.map[cellFarm.yCell * LoadMap.wMap + cellFarm.xCell] = cellFarm.status;
	}

	public void onHarvestTree(int indexCell2, int number)
	{
		CellFarm cellFarm = (CellFarm)cell.elementAt(indexCell2);
		if (idFarm == GameMidlet.avatar.IDDB)
		{
			cellFarm.statusTree = 6;
			cellFarm.hervestPer = 100;
			cellFarm.isGrass = false;
			cellFarm.isWorm = false;
		}
		if (number > 0)
		{
			TreeInfo treeByID = FarmData.getTreeByID(cellFarm.idTree);
			if (treeByID.isDynamic)
			{
				Canvas.addFlyText(number, cellFarm.xCell * LoadMap.w + 11, cellFarm.yCell * LoadMap.w, -1, 0, treeByID.idImg[cellFarm.statusTree], -1);
			}
			else
			{
				ImageInfo imageInfo = FarmData.listImgInfo[treeByID.idImg[5]];
				Canvas.addFlyText(number, cellFarm.xCell * LoadMap.w + 11, cellFarm.yCell * LoadMap.w, -1, CRes.createImgByImg(imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd, FarmData.imgBig[imageInfo.bigID]), -1);
			}
		}
		TreeInfo treeByID2 = FarmData.getTreeByID(cellFarm.idTree);
		addProductTree(treeByID2, number);
	}

	public static void addProductTree(TreeInfo tree, int numSt)
	{
		if (tree.isDynamic)
		{
			Item itemProductByID = getItemProductByID(tree.productID);
			if (itemProductByID != null)
			{
				itemProductByID.number += numSt;
				return;
			}
			itemProductByID = new Item();
			itemProductByID.ID = tree.productID;
			itemProductByID.number = numSt;
			itemProductByID.price[0] = tree.priceProduct;
			itemProductByID.name = tree.name;
			listFarmProduct.addElement(itemProductByID);
		}
		else
		{
			Item itemByList = Item.getItemByList(itemProduct, tree.ID);
			if (itemByList != null)
			{
				itemByList.number += numSt;
				return;
			}
			itemByList = new Item();
			itemByList.ID = tree.ID;
			itemByList.number = numSt;
			itemByList.price[0] = FarmData.getTreeByID(tree.ID).priceProduct;
			itemByList.name = FarmData.getTreeByID(tree.ID).name;
			itemProduct.addElement(itemByList);
		}
	}

	public static void addProductAnimal(AnimalInfo ani, int numSt)
	{
		Item itemByList = Item.getItemByList(itemProduct, ani.species);
		if (itemByList != null)
		{
			itemByList.number += numSt;
			return;
		}
		Item item = new Item();
		item.ID = ani.species;
		item.number = numSt;
		item.name = ani.name;
		item.price[0] = ani.priceProduct;
		setNameItem(item);
		itemProduct.addElement(item);
	}

	public void onHarvestAnimal(int indexCell3, int number4)
	{
		Animal animalByIndex = getAnimalByIndex(indexCell3);
		if (number4 <= 0 || animalByIndex == null)
		{
			return;
		}
		AnimalInfo animalByID = FarmData.getAnimalByID(animalByIndex.species);
		addProductAnimal(animalByID, number4);
		if (AvatarData.getImgIcon(animalByID.iconProduct) != null)
		{
			AvPosition avPosition = null;
			if (animalByID.area == 1)
			{
				avPosition = getPosO(listNest, animalByIndex.species);
			}
			else if (animalByID.area == 2)
			{
				avPosition = getPosO(listBucket, animalByIndex.species);
			}
			if (avPosition != null)
			{
				Canvas.addFlyText(number4, avPosition.x, avPosition.y - 25, -1, AvatarData.getImgIcon(animalByID.iconProduct).img, -1);
			}
		}
	}

	public static AvPosition getPosO(MyVector list, int type)
	{
		for (int i = 0; i < list.size(); i++)
		{
			AvPosition avPosition = (AvPosition)list.elementAt(i);
			if (avPosition.anchor == type)
			{
				return avPosition;
			}
		}
		return null;
	}

	public void onOpenLand(int idfarm, int curMoney, sbyte typeBuy, string text, int xu1, int luong1, int luongKhoa1)
	{
		if (idfarm == idFarm)
		{
			GameMidlet.avatar.updateMoney(xu1, luong1, luongKhoa1);
			Canvas.startOKDlg(text);
		}
	}

	public void doJoinFarm(int idFarm, bool join)
	{
		isJoin = join;
		FarmService.gI().doJoinFarm(idFarm);
	}

	private void doSellProduct(short idItem)
	{
		Canvas.startOKDlg(T.youWantBuyPro, new IActionSellProduct(idItem));
	}

	protected void doBuyAnimal(AnimalInfo animal)
	{
		Canvas.getTypeMoney(animal.price[0], animal.price[1], new IActionBuyAnimalXu(animal), new IActionBuyAnimalLuong(animal), null);
	}

	public void doOpenCuaHang()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < FarmData.treeInfo.Length; i++)
		{
			int num = i;
			Command o = new CommandBuyItemCuaHang(T.buy, new IactionBuyTreeInput(FarmData.treeInfo[num].ID), num);
			myVector.addElement(o);
		}
		int num2 = FarmData.listAnimalInfo.size();
		for (int j = 0; j < num2; j++)
		{
			AnimalInfo ani = (AnimalInfo)FarmData.listAnimalInfo.elementAt(j);
			int num3 = j;
			Command o2 = new CommandBuyAnimalCuaHang(T.buy, new IActionBuyAnimalCuaHang(this, ani, num3), ani, num3);
			myVector.addElement(o2);
		}
		PopupShop.gI().switchToMe();
		PopupShop.isHorizontal = true;
		PopupShop.gI().addElement(new string[3]
		{
			T.seed,
			T.item,
			T.storePro
		}, new MyVector[3]
		{
			myVector,
			goVatPham(),
			goKhoHang()
		}, null, null);
		if (Canvas.isInitChar && !Welcome.isOut)
		{
			Canvas.welcome = new Welcome();
			Canvas.welcome.initFarmPath(PopupShop.me);
		}
	}

	public MyVector goVatPham()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < FarmData.listItemFarm.size(); i++)
		{
			FarmItem farmItem = (FarmItem)FarmData.listItemFarm.elementAt(i);
			if (farmItem.isItem && (farmItem.priceLuong > 0 || farmItem.priceXu > 0))
			{
				int num = i;
				myVector.addElement(new CommandGoVatPham(T.selectt, new IActionGoVatPham(this, farmItem), farmItem, num));
			}
		}
		return myVector;
	}

	public MyVector goKhoHang()
	{
		MyVector myVector = new MyVector();
		int num = itemProduct.size();
		for (int i = 0; i < num; i++)
		{
			int i2 = i;
			Item item = (Item)itemProduct.elementAt(i2);
			Command o = new CommandGoKhoHang1(T.sell, new IActionGoKhoHang1(this, item), item, i2);
			myVector.addElement(o);
		}
		for (int j = 0; j < listFarmProduct.size(); j++)
		{
			int num2 = j;
			Item item2 = (Item)listFarmProduct.elementAt(j);
			FarmItem farmItem = getFarmItem(item2.ID);
			myVector.addElement(new CommandGoKhoHang2(T.sell, new IActionGoKhoHang2(this, farmItem), farmItem, num2, item2));
		}
		return myVector;
	}

	public void doOpenKhoHang()
	{
		if (isSteal)
		{
			if (isAbleSteal)
			{
				FarmService.gI().doStealStore();
				Canvas.startWaitDlg();
			}
			return;
		}
		if (GameMidlet.avatar.IDDB != idFarm)
		{
			Canvas.startOKDlg(T.notOnFarmOther);
			return;
		}
		MyVector myVector = new MyVector();
		for (int i = 0; i < itemSeed.size(); i++)
		{
			int num = i;
			Command o = new CommandOpenKhoHang1(string.Empty, new IActionEmpty(), num);
			myVector.addElement(o);
		}
		for (int j = 0; j < listItemFarm.size(); j++)
		{
			int num2 = j;
			Command o2 = new CommandOpenKhoHang2(string.Empty, new IActionEmpty(), num2);
			myVector.addElement(o2);
		}
		PopupShop.gI().switchToMe();
		int num3 = 0;
		for (int k = 0; k < itemProduct.size(); k++)
		{
			Item item = (Item)itemProduct.elementAt(k);
			num3 += item.number;
		}
		for (int l = 0; l < listFarmProduct.size(); l++)
		{
			Item item2 = (Item)listFarmProduct.elementAt(l);
			num3 += item2.number;
		}
		PopupShop.gI().addElement(new string[2]
		{
			T.storePro,
			T.StoreSeed
		}, new MyVector[2]
		{
			goKhoHang(),
			myVector
		}, null, null);
		PopupShop.isHorizontal = true;
		PopupShop.gI().setCmyLim();
	}

	public override void commandAction(int index)
	{
		switch (index)
		{
		case 1:
			setAction(1, idItemUsing);
			break;
		case 10:
		{
			isAutoVatNuoi = true;
			for (int i = indexAuto; i < animalLists.size(); i++)
			{
				Animal pet = (Animal)animalLists.elementAt(i);
				if (doAutoVatNuoi(pet))
				{
					return;
				}
				indexAuto++;
			}
			commandTab(8);
			Canvas.startOKDlg("Không có vật nuôi nào bị bệnh");
			break;
		}
		case 11:
			FarmService.gI().doUpdateLand(0, 0);
			break;
		case 12:
			FarmService.gI().doHarvestStarFruit();
			break;
		case 13:
			if (starFruil.timeFinish > 0)
			{
				FarmService.gI().doUpdateStarFruitByMoney(0);
			}
			else
			{
				FarmService.gI().doUpdateStarFruil(0);
			}
			break;
		case 14:
			FarmService.gI().getInfoStarFruit();
			break;
		}
	}

	private void setAuto(int subIndex)
	{
		idSelected = 0;
		left = new Command(T.finish, 5);
		right = null;
		AvCamera.isFollow = true;
		center = null;
		isSelectedCell = true;
		indexItem = subIndex;
	}

	private void doGieoHat()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < itemSeed.size(); i++)
		{
			int i2 = i;
			Item item = (Item)itemSeed.elementAt(i2);
			myVector.addElement(new CommandGieoHat1(item.name + "(" + item.number + ")", new IActionHieoHat1(i), item));
		}
		int num = MyScreen.hText + 10;
		Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - num - 10, num, num);
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 0:
			doSellect();
			break;
		case 1:
			if ((!Dog.isHound || (LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53)) && (Canvas.welcome == null || Welcome.isPaintArrow))
			{
				MenuCenter.gI().startAt(listLeftMenu);
			}
			break;
		case 2:
			doVatPhamAnimal();
			break;
		case 3:
			doFeeding();
			break;
		case 5:
		{
			left = null;
			isSelectedCell = false;
			AvCamera.isFollow = false;
			isChamSoc = false;
			listSelectedCell.removeAllElements();
			for (int i = 0; i < cell.size(); i++)
			{
				CellFarm cellFarm = (CellFarm)cell.elementAt(i);
				cellFarm.isSelected = false;
			}
			idSelected = -1;
			indexItem = -1;
			isSelected = false;
			break;
		}
		case 51:
			FarmService.gI().doOpenLand(idFarm, 1);
			doJoinFarm(idFarm, true);
			break;
		case 52:
			FarmService.gI().doOpenLand(idFarm, 2);
			doJoinFarm(idFarm, true);
			break;
		case 53:
			setAction(0, -1);
			Canvas.endDlg();
			break;
		case 54:
			doGoFarmWay();
			break;
		}
	}

	private bool doAutoVatNuoi(Animal pet)
	{
		if (pet.disease[1])
		{
			LoadMap.focusObj = pet;
			AvCamera.gI().setToPos(pet.x * Main.hdtype, pet.y * Main.hdtype);
			AvCamera.isFollow = true;
			center = new Command("Trị bệnh 1", new IActionTriBenh1(pet));
			left = cmdFinishAuto;
			right = cmdNextAuto;
			return true;
		}
		if (pet.disease[0])
		{
			LoadMap.focusObj = pet;
			AvCamera.gI().setToPos(pet.x * Main.hdtype, pet.y * Main.hdtype);
			AvCamera.isFollow = true;
			center = new Command("Trị bệnh 2", new IActionTriBenh2(pet));
			left = cmdFinishAuto;
			right = cmdNextAuto;
			return true;
		}
		if (pet.hunger)
		{
			LoadMap.focusObj = pet;
			AvCamera.gI().setToPos(pet.x * AvMain.hd, pet.y * AvMain.hd);
			AvCamera.isFollow = true;
			center = new Command("Cho ăn", new IActionEat(pet));
			left = cmdFinishAuto;
			right = cmdNextAuto;
			return true;
		}
		if (pet.health < 80)
		{
			LoadMap.focusObj = pet;
			AvCamera.gI().setToPos(pet.x * Main.hdtype, pet.y * Main.hdtype);
			AvCamera.isFollow = true;
			center = new Command("Thuốc bổ", new IActionTriBenh3(pet));
			left = cmdFinishAuto;
			right = cmdNextAuto;
			return true;
		}
		return false;
	}

	public void onKick(int idFarm1)
	{
		if (LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53)
		{
			Canvas.menuMain = null;
			Canvas.msgdlg.setInfoC(T.youAreBittenByDogByHound, new Command(T.OK, 8, this));
		}
	}

	public bool doEat(short itemID, int index)
	{
		Item itemByList = Item.getItemByList(listItemFarm, itemID);
		if (itemByList == null)
		{
			return false;
		}
		FarmService.gI().doUsingItem(idFarm, index, itemID);
		return false;
	}

	public void doCattleFeeding(sbyte type, sbyte action)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < listItemFarm.size(); i++)
		{
			Item item = (Item)listItemFarm.elementAt(i);
			FarmItem farmItem = getFarmItem(item.ID);
			if ((farmItem.type == type || farmItem.type == 101) && farmItem.action == action)
			{
				myVector.addElement(new CommandCattleFeeding(farmItem.des + "(" + item.number + ")", new IActionCattleFeeding(type, item), farmItem));
			}
		}
		int num = MyScreen.hText + 10;
		Menu.gI().startMenuFarm(myVector, Canvas.hw, Canvas.h - LoadMap.w * AvMain.hd - 10, num, num);
	}

	private void sendHarvestAnimal(Animal pet, AvPosition pos)
	{
		if (Dog.isHound && idFarm != GameMidlet.avatar.IDDB)
		{
			Canvas.addFlyTextSmall(T.theft, pos.x, pos.y - 25, -1, 1, -1);
			if (Dog.numBer > 0)
			{
				listHound.addElement(new IActionSendHarvestAnimal(pet));
			}
			else
			{
				FarmService.gI().doHarvestAnimal(idFarm, pet.IDDB);
			}
		}
		else
		{
			FarmService.gI().doHarvestAnimal(idFarm, pet.IDDB);
		}
	}

	public void doHarvestAnimal(int type, int index, MyVector list)
	{
		if ((isSteal && !isAbleSteal) || (!isSteal && GameMidlet.avatar.IDDB != idFarm) || index < 0 || index >= list.size())
		{
			return;
		}
		if (GameMidlet.avatar.IDDB != idFarm)
		{
			Dog.isHound = true;
		}
		if (Dog.isHound && listHound == null)
		{
			listHound = new MyVector();
		}
		AvPosition avPosition = (AvPosition)list.elementAt(index);
		for (int i = 0; i < animalLists.size(); i++)
		{
			Animal animal = (Animal)animalLists.elementAt(i);
			AnimalInfo animalByID = FarmData.getAnimalByID(animal.species);
			if (animal.numEggOne > 0 && avPosition.anchor == animal.species)
			{
				animal.numEggOne = 0;
				if (type == 1 && animalByID.area == type)
				{
					sendHarvestAnimal(animal, Chicken.posNest);
					removePopup(-50);
				}
				if (type == 2 && animalByID.area == type)
				{
					sendHarvestAnimal(animal, Cattle.posBucket);
					removePopup(-51);
				}
			}
		}
	}

	public void onSell(int sellMoney, int curMoney, short idItem)
	{
		GameMidlet.avatar.money[0] = curMoney;
		PopupShop.isTransFocus = true;
		Canvas.startOKDlg(T.moneySellPro + sellMoney + T.xu);
		Item itemByList = Item.getItemByList(itemProduct, idItem);
		if (itemByList == null)
		{
			itemByList = Item.getItemByList(listFarmProduct, idItem);
			listFarmProduct.removeElement(itemByList);
		}
		else
		{
			itemProduct.removeElement(itemByList);
		}
		if (Canvas.currentMyScreen == PopupShop.gI())
		{
			PopupShop.gI().closeTabAll();
			if (LoadMap.TYPEMAP == 25)
			{
				doOpenCuaHang();
				PopupShop.gI().setTap(2);
			}
			else
			{
				doOpenKhoHang();
			}
		}
		Canvas.endDlg();
		SoundManager.playSound(3);
	}

	public void onSellAnimal(int idFarm, int index, int curMoney1)
	{
		Animal animalByIndex = getAnimalByIndex(index);
		if (animalByIndex != null)
		{
			int text = curMoney1 - GameMidlet.avatar.money[1];
			Canvas.addFlyText(text, animalByIndex.x, animalByIndex.y - 30, -1, -1);
			LoadMap.focusObj = null;
			animalLists.removeElement(animalByIndex);
			LoadMap.playerLists.removeElement(animalByIndex);
			SoundManager.playSound(3);
		}
		GameMidlet.avatar.money[1] = curMoney1;
	}

	public void onPriceAnimal(sbyte index, string str1)
	{
		Canvas.startOKDlg(str1, new IActionPriceAnimal(index));
	}

	public void doGoFarmWay()
	{
		isSteal = false;
		isAbleSteal = false;
		if (listHound != null)
		{
			for (int i = 0; i < listHound.size(); i++)
			{
				((IAction)listHound.elementAt(i)).perform();
			}
		}
		Cattle.itemID = -1;
		Dog.itemID = -1;
		listHound = null;
		right = null;
		ParkService.gI().doJoinPark(25, 0);
	}

	public void doExitBus()
	{
		resetImg();
	}

	public void onBittenByDog()
	{
		listHound = null;
		doGoFarmWay();
	}

	public static Animal getAnimalByIndex(int index)
	{
		for (int i = 0; i < animalLists.size(); i++)
		{
			Animal animal = (Animal)animalLists.elementAt(i);
			if (animal.IDDB == index)
			{
				return animal;
			}
		}
		return null;
	}

	public void doMenuStarFruit()
	{
		if (isSteal)
		{
			if (isAbleSteal && starFruil.numberFruit > 0)
			{
				FarmService.gI().doHarvestStarFruit();
				Canvas.startWaitDlg();
				removePopup(-2);
			}
		}
		else if (GameMidlet.avatar.IDDB == idFarm)
		{
			MyVector myVector = new MyVector();
			if (starFruil.numberFruit > 0)
			{
				myVector.addElement(new CommandMenuStarFruit1(T.harvest + "(" + starFruil.numberFruit + ")", 12, 62));
			}
			myVector.addElement(new CommandMenuStarFruit1((starFruil.timeFinish <= 0) ? T.update : T.QuickUpgrade, 13, (starFruil.timeFinish <= 0) ? 63 : 64));
			myVector.addElement(new CommandMenuStarFruit1(T.info, 14, 61));
			startMenuFarm(myVector);
		}
	}

	public void startMenuFarm(MyVector menu)
	{
		int num = MyScreen.hText + 10;
		Menu.gI().startMenuFarm(menu, Canvas.hw, Canvas.h - num - 10, num, num);
	}

	public void doOpenCooking()
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < FarmData.listFood.size(); i++)
		{
			Food food = (Food)FarmData.listFood.elementAt(i);
			int num = i;
			myVector.addElement(new CommandCooking1(T.cook, new IActionCooking1(food), num, food));
		}
		MyVector myVector2 = new MyVector();
		if (foodID > 0)
		{
			myVector2.addElement(null);
			Command o = new CommandCooking((remainTime != 0) ? T.QuickCooking : T.done, 22, this);
			myVector2.addElement(o);
		}
		PopupShop.gI().switchToMe();
		PopupShop.isHorizontal = true;
		if (foodID > 0)
		{
			PopupShop.gI().addElement(new string[2]
			{
				T.cook,
				T.cooking
			}, new MyVector[2] { myVector, null }, myVector2, null);
			PopupShop.gI().setCmdLeft(new Command(T.cancel, 23, this), 1);
			PopupShop.focusTap = 1;
			PopupShop.gI().setTap(0);
			PopupShop.gI().setCmyLim();
			PopupShop.gI().setCaption();
		}
		else
		{
			PopupShop.gI().addElement(new string[1] { T.cook }, new MyVector[1] { myVector }, null, null);
		}
	}

	public void onHarvestStarFruit(short productIDH, short numberPro)
	{
		for (int i = 0; i < starFruil.xFruit.Length; i++)
		{
			Canvas.addFlyText(0, starFruil.x + starFruil.xFruit[i], starFruil.y - 45 + starFruil.yFruit[i], -1, 0, starFruil.fruitID, -1);
		}
		Canvas.addFlyText(numberPro, GameMidlet.avatar.x, GameMidlet.avatar.y - GameMidlet.avatar.height, -1, 10);
		starFruil.numberFruit = 0;
		Item itemProductByID = getItemProductByID(productIDH);
		if (itemProductByID != null)
		{
			itemProductByID.number += numberPro;
		}
		else
		{
			itemProductByID = new Item();
			itemProductByID.ID = productIDH;
			itemProductByID.number = numberPro;
			listFarmProduct.addElement(itemProductByID);
		}
		Canvas.endDlg();
	}

	public static Item getItemProductByID(int id)
	{
		for (int i = 0; i < listFarmProduct.size(); i++)
		{
			Item item = (Item)listFarmProduct.elementAt(i);
			if (item.ID == id)
			{
				return item;
			}
		}
		return null;
	}

	public static Item getProductByID(int id)
	{
		for (int i = 0; i < itemProduct.size(); i++)
		{
			Item item = (Item)itemProduct.elementAt(i);
			if (item.ID == id)
			{
				return item;
			}
		}
		return null;
	}

	public void doMenuFarmFriend()
	{
		ListScr.gI().setFriendList(true);
	}

	public static void removePopup(int type)
	{
		for (int i = 0; i < LoadMap.treeLists.size(); i++)
		{
			SubObject subObject = (SubObject)LoadMap.treeLists.elementAt(i);
			if (subObject.catagory == 9 && subObject.type == type)
			{
				LoadMap.treeLists.removeElement(subObject);
				break;
			}
		}
	}
}
