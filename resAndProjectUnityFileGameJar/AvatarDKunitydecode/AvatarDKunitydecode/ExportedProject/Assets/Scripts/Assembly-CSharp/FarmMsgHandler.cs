using System;

public class FarmMsgHandler : IMiniGameMsgHandler
{
	private class IActionSteal : IAction
	{
		public void perform()
		{
			FarmService.gI().doSteal(1);
			Canvas.startWaitDlg();
		}
	}

	private class IActionCattle : IAction
	{
		private int typeMoney;

		public IActionCattle(int type)
		{
			typeMoney = type;
		}

		public void perform()
		{
			FarmService.gI().doUpdateFarm(1, typeMoney);
		}
	}

	private class IActionFish : IAction
	{
		private int typeMoney;

		public IActionFish(int type)
		{
			typeMoney = type;
		}

		public void perform()
		{
			FarmService.gI().doUpdateFish(1, typeMoney);
		}
	}

	private static FarmMsgHandler instance;

	private int aa;

	public static void onHandler()
	{
		if (instance == null)
		{
			instance = new FarmMsgHandler();
		}
		GlobalMessageHandler.gI().miniGameMessageHandler = instance;
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case 51:
			{
				sbyte b19 = msg.reader().readByte();
				short[] array2 = new short[b19];
				short[] array3 = new short[b19];
				for (int num13 = 0; num13 < b19; num13++)
				{
					array2[num13] = msg.reader().readShort();
					array3[num13] = msg.reader().readShort();
				}
				int verBigImg = msg.reader().readInt();
				int verPart = msg.reader().readInt();
				FarmData.checkDataFarm(b19, array2, array3, verBigImg, verPart);
				break;
			}
			case 54:
			{
				short index3 = msg.reader().readShort();
				short version = msg.reader().readShort();
				int num11 = msg.reader().readUnsignedShort();
				sbyte[] array = new sbyte[num11];
				for (int num12 = 0; num12 < num11; num12++)
				{
					array[num12] = msg.reader().readByte();
				}
				FarmData.addDataBig(index3, version, array);
				break;
			}
			case 55:
			{
				sbyte[] data2 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data2);
				FarmData.saveImageData(data2);
				break;
			}
			case 56:
			{
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().read(ref data);
				FarmData.saveTreeInfo(data);
				break;
			}
			case 60:
			{
				sbyte b9 = msg.reader().readByte();
				MyVector myVector = new MyVector();
				MyVector myVector2 = new MyVector();
				for (int i = 0; i < b9; i++)
				{
					Item item3 = new Item();
					item3.ID = msg.reader().readByte();
					item3.number = msg.reader().readShort();
					if (item3.ID > 100)
					{
						myVector2.addElement(item3);
					}
					else
					{
						myVector.addElement(item3);
					}
				}
				sbyte b10 = msg.reader().readByte();
				MyVector myVector3 = new MyVector();
				for (int j = 0; j < b10; j++)
				{
					Item item4 = new Item();
					item4.ID = msg.reader().readByte();
					item4.number = msg.reader().readShort();
					myVector3.addElement(item4);
				}
				GameMidlet.avatar.money[1] = msg.reader().readInt();
				GameMidlet.avatar.lvFarm = msg.reader().readByte();
				GameMidlet.avatar.perLvFarm = msg.reader().readByte();
				sbyte b11 = msg.reader().readByte();
				MyVector myVector4 = new MyVector();
				for (int k = 0; k < b11; k++)
				{
					Item item5 = new Item();
					item5.ID = msg.reader().readShort();
					item5.number = msg.reader().readShort();
					myVector4.addElement(item5);
				}
				sbyte b12 = msg.reader().readByte();
				MyVector myVector5 = new MyVector();
				for (int l = 0; l < b12; l++)
				{
					Item item6 = new Item();
					item6.ID = msg.reader().readShort();
					item6.number = msg.reader().readShort();
					myVector5.addElement(item6);
				}
				sbyte levelStore = msg.reader().readByte();
				int capacity = msg.reader().readInt();
				bool isNew = msg.reader().readBoolean();
				GameMidlet.avatar.lvFarm = msg.reader().readShort();
				GameMidlet.avatar.perLvFarm = msg.reader().readByte();
				sbyte b13 = msg.reader().readByte();
				myVector3.removeAllElements();
				for (int m = 0; m < b13; m++)
				{
					Item item7 = new Item();
					item7.ID = msg.reader().readShort();
					item7.number = msg.reader().readInt();
					myVector3.addElement(item7);
				}
				myVector5.removeAllElements();
				b12 = msg.reader().readByte();
				for (int n = 0; n < b12; n++)
				{
					Item item8 = new Item();
					item8.ID = msg.reader().readShort();
					item8.number = msg.reader().readInt();
					myVector5.addElement(item8);
				}
				FarmScr.gI().onInventory(myVector, myVector3, myVector2, myVector4, myVector5, levelStore, capacity, isNew);
				if (FarmData.playing == 0 && LoadMap.TYPEMAP != 24 && LoadMap.TYPEMAP != 53 && LoadMap.TYPEMAP != 25)
				{
					FarmData.saveVersion();
					ParkService.gI().doJoinPark(25, 0);
					FarmScr.init();
					FarmScr.gI().doJoinFarm(GameMidlet.avatar.IDDB, false);
				}
				break;
			}
			case 61:
				readFarmData(msg);
				break;
			case 62:
			{
				Item item2 = new Item();
				item2.ID = msg.reader().readShort();
				item2.number = msg.reader().readByte();
				int newMoney2 = msg.reader().readInt();
				sbyte typeBuy3 = msg.reader().readByte();
				int xu4 = msg.reader().readInt();
				int luong5 = msg.reader().readInt();
				int luongK4 = msg.reader().readInt();
				FarmScr.gI().onBuyItem(item2, newMoney2, typeBuy3, xu4, luong5, luongK4);
				break;
			}
			case 71:
			{
				Item item = new Item();
				item.ID = msg.reader().readByte();
				int newMoney = msg.reader().readInt();
				sbyte typeBuy2 = msg.reader().readByte();
				int xu3 = msg.reader().readInt();
				int luong4 = msg.reader().readInt();
				int luongK3 = msg.reader().readInt();
				FarmScr.gI().onBuyItem(item, newMoney, typeBuy2, xu3, luong4, luongK3);
				break;
			}
			case 63:
			{
				int sellMoney = msg.reader().readInt();
				int curMoney3 = msg.reader().readInt();
				short idItem = msg.reader().readShort();
				FarmScr.gI().onSell(sellMoney, curMoney3, idItem);
				break;
			}
			case 73:
			{
				int idFarm = msg.reader().readInt();
				int index = msg.reader().readByte();
				int curMoney2 = msg.reader().readInt();
				FarmScr.gI().onSellAnimal(idFarm, index, curMoney2);
				break;
			}
			case 64:
			{
				int idUser = msg.reader().readInt();
				int indexCell = msg.reader().readByte();
				int idSeed = msg.reader().readByte();
				FarmScr.gI().onPlantSeed(idUser, indexCell, idSeed);
				break;
			}
			case 66:
			{
				int indexCell3 = msg.reader().readByte();
				int number2 = msg.reader().readShort();
				FarmScr.gI().onHarvestTree(indexCell3, number2);
				break;
			}
			case 74:
			{
				int indexCell2 = msg.reader().readByte();
				int number = msg.reader().readShort();
				FarmScr.gI().onHarvestAnimal(indexCell2, number);
				break;
			}
			case 70:
			{
				int idfarm = msg.reader().readInt();
				int curMoney = msg.reader().readInt();
				sbyte typeBuy = msg.reader().readByte();
				string text = msg.reader().readUTF();
				int xu = msg.reader().readInt();
				int luong = msg.reader().readInt();
				int luongKhoa = msg.reader().readInt();
				FarmScr.gI().onOpenLand(idfarm, curMoney, typeBuy, text, xu, luong, luongKhoa);
				break;
			}
			case 65:
			{
				msg.reader().readByte();
				int id = msg.reader().readShort();
				FarmItem farmItem2 = FarmScr.getFarmItem(id);
				if (farmItem2 == null)
				{
					break;
				}
				Item itemByList = Item.getItemByList(FarmScr.listItemFarm, id);
				if (itemByList != null)
				{
					itemByList.number--;
					if (itemByList.number <= 0)
					{
						FarmScr.listItemFarm.removeElement(itemByList);
					}
				}
				break;
			}
			case 69:
				FarmScr.gI().onPricePlant(msg.reader().readUTF());
				break;
			case 72:
			{
				sbyte index2 = msg.reader().readByte();
				string str = msg.reader().readUTF();
				FarmScr.gI().onPriceAnimal(index2, str);
				break;
			}
			case 67:
				FarmScr.gI().onKick(msg.reader().readInt());
				break;
			case 75:
			{
				int money = msg.reader().readInt();
				int num6 = msg.reader().readInt();
				GameMidlet.avatar.setMoney(money);
				GameMidlet.avatar.money[1] = num6;
				string info4 = msg.reader().readUTF();
				Canvas.startOKDlg(info4);
				break;
			}
			case 76:
				GlobalMessageHandler.readMove(msg);
				break;
			case 77:
				GlobalMessageHandler.readChat(msg);
				break;
			case 78:
				if (LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53)
				{
					sbyte b8 = msg.reader().readByte();
					CellFarm cellFarm = (CellFarm)FarmScr.cell.elementAt(b8);
					cellFarm.idTree = msg.reader().readByte();
					readInfoCell(cellFarm, msg);
					FarmScr.gI().setInfoCell(b8);
				}
				break;
			case 79:
				if (LoadMap.TYPEMAP == 24 || LoadMap.TYPEMAP == 53)
				{
					sbyte b4 = msg.reader().readByte();
					sbyte b5 = msg.reader().readByte();
					if (b5 != -1)
					{
						Animal animalByIndex = FarmScr.getAnimalByIndex(b4);
						animalByIndex.species = b5;
						readInfoAnimal(animalByIndex, msg);
						FarmScr.gI().setAnimal();
					}
				}
				break;
			case 80:
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					string info = msg.reader().readUTF();
					Canvas.msgdlg.setInfoLCR(info, new Command(T.xu, new IActionCattle(0)), new Command(T.gold, new IActionCattle(1)), Canvas.cmdEndDlg);
				}
				else
				{
					sbyte b2 = msg.reader().readByte();
					int num = msg.reader().readInt();
					Canvas.load = 1;
					FarmScr.gI().doJoinFarm(GameMidlet.avatar.IDDB, true);
					FarmScr.isReSize = true;
				}
				int xu2 = msg.reader().readInt();
				int luong2 = msg.reader().readInt();
				int luongK = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(xu2, luong2, luongK);
				break;
			}
			case 81:
			{
				sbyte b20 = msg.reader().readByte();
				if (b20 == 0)
				{
					string info8 = msg.reader().readUTF();
					Canvas.msgdlg.setInfoLCR(info8, new Command(T.xu, new IActionFish(0)), new Command(T.gold, new IActionFish(1)), Canvas.cmdEndDlg);
				}
				else
				{
					sbyte b21 = msg.reader().readByte();
					int num16 = msg.reader().readInt();
					Canvas.load = 1;
					FarmScr.gI().doJoinFarm(GameMidlet.avatar.IDDB, true);
					FarmScr.isReSize = true;
				}
				int xu5 = msg.reader().readInt();
				int luong8 = msg.reader().readInt();
				int luongK7 = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(xu5, luong8, luongK7);
				break;
			}
			case 82:
			{
				short num14 = msg.reader().readShort();
				short num15 = msg.reader().readShort();
				sbyte[] data3 = new sbyte[num15];
				msg.reader().read(ref data3);
				FarmData.listImgIcon.put(string.Empty + num14, new ImageIcon(CRes.createImgByByteArray(ArrayCast.cast(data3))));
				break;
			}
			case 84:
			{
				sbyte b18 = msg.reader().readByte();
				if (b18 == 0)
				{
					string info7 = msg.reader().readUTF();
					Canvas.startOKDlg(info7, 7, FarmScr.instance);
					break;
				}
				int num9 = msg.reader().readInt();
				short num10 = msg.reader().readShort();
				GameMidlet.avatar.money[1] -= num9;
				FarmScr.starFruil.timeFinish = num10 * 60;
				FarmScr.starFruil.time = Canvas.getTick();
				Canvas.addFlyText(-num9, GameMidlet.avatar.x, GameMidlet.avatar.y, -1, -1);
				break;
			}
			case 85:
			{
				short productIDH = msg.reader().readShort();
				short numberPro = msg.reader().readShort();
				FarmScr.gI().onHarvestStarFruit(productIDH, numberPro);
				break;
			}
			case 86:
			{
				sbyte b17 = msg.reader().readByte();
				if (b17 == 0)
				{
					string info6 = msg.reader().readUTF();
					Canvas.startOKDlg(info6, 8, FarmScr.instance);
					break;
				}
				int num8 = msg.reader().readInt();
				StarFruitObj.imgID = msg.reader().readShort();
				FarmScr.starFruil.timeFinish = 0;
				FarmScr.starFruil.lv++;
				int luong7 = msg.reader().readInt();
				int luongK6 = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(GameMidlet.avatar.money[0], luong7, luongK6);
				break;
			}
			case 90:
			{
				sbyte b14 = msg.reader().readByte();
				if (b14 == 0)
				{
					string info5 = msg.reader().readUTF();
					Canvas.msgdlg.setInfoLCR(info5, new Command(T.xu, 9, FarmScr.instance), new Command(T.gold, 10, FarmScr.instance), Canvas.cmdEndDlg);
					break;
				}
				sbyte b15 = msg.reader().readByte();
				int num7 = msg.reader().readInt();
				sbyte b16 = msg.reader().readByte();
				Canvas.startOKDlg(msg.reader().readUTF());
				CellFarm cellFarm2 = (CellFarm)FarmScr.cell.elementAt(b16);
				cellFarm2.level++;
				int luong6 = msg.reader().readInt();
				int luongK5 = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(GameMidlet.avatar.money[0], luong6, luongK5);
				FarmScr.gI().onJoin(FarmScr.idFarm, FarmScr.cell, FarmScr.animalLists, FarmScr.numBarn, FarmScr.numPond, FarmScr.foodID, FarmScr.remainTime);
				break;
			}
			case 94:
			{
				if (aa == 1)
				{
					break;
				}
				sbyte b6 = msg.reader().readByte();
				if (b6 == 0)
				{
					string info3 = msg.reader().readUTF();
					Canvas.msgdlg.setInfoLCR(info3, new Command(T.xu, 13, FarmScr.instance), new Command(T.gold, 14, FarmScr.instance), Canvas.cmdEndDlg);
				}
				else
				{
					sbyte b7 = msg.reader().readByte();
					int num5 = msg.reader().readInt();
					if (b7 == 1)
					{
						GameMidlet.avatar.money[1] -= num5;
					}
					else
					{
						GameMidlet.avatar.money[2] -= num5;
					}
					FarmScr.capacityStore = msg.reader().readInt();
					Canvas.startOKDlg(msg.reader().readUTF());
					FarmScr.gI().onJoin(FarmScr.idFarm, FarmScr.cell, FarmScr.animalLists, FarmScr.numBarn, FarmScr.numPond, FarmScr.foodID, FarmScr.remainTime);
				}
				aa = 1;
				break;
			}
			case 91:
			{
				short num3 = msg.reader().readShort();
				if (num3 == -1)
				{
					FarmScr.foodID = 0;
				}
				else
				{
					short num4 = msg.reader().readShort();
					FarmScr.foodID = num3;
					FarmScr.remainTime = num4 * 60;
					FarmScr.curTimeCooking = (int)(Canvas.getTick() / 1000);
				}
				Canvas.endDlg();
				break;
			}
			case 93:
			{
				sbyte b3 = msg.reader().readByte();
				if (b3 == 0)
				{
					string info2 = msg.reader().readUTF();
					Canvas.startOKDlg(info2, 11, FarmScr.instance);
					break;
				}
				int num2 = msg.reader().readInt();
				FarmScr.remainTime = 0;
				int luong3 = msg.reader().readInt();
				int luongK2 = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(GameMidlet.avatar.money[0], luong3, luongK2);
				break;
			}
			case 92:
			{
				Food foodByID = FarmData.getFoodByID(FarmScr.foodID);
				FarmItem farmItem = FarmScr.getFarmItem(foodByID.productID);
				Item itemProductByID = FarmScr.getItemProductByID(foodByID.productID);
				if (itemProductByID != null)
				{
					itemProductByID.number++;
				}
				else
				{
					itemProductByID = new Item();
					itemProductByID.ID = foodByID.productID;
					itemProductByID.number = 1;
					FarmScr.listFarmProduct.addElement(itemProductByID);
				}
				Canvas.addFlyText(0, FarmScr.xPosCook, FarmScr.yPosCook, -1, 0, farmItem.IDImg, -1);
				FarmScr.foodID = 0;
				break;
			}
			case 83:
				if (msg.reader().readBoolean())
				{
					StarFruitObj.imgID = msg.reader().readShort();
					FarmScr.starFruil.lv++;
				}
				break;
			case 52:
			case 53:
			case 57:
			case 58:
			case 59:
			case 68:
			case 87:
			case 88:
			case 89:
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	public static void readInfoCell(CellFarm c, Message msg)
	{
		short num = msg.reader().readShort();
		FarmScr.startTextSmall(c.time, num, c, null);
		c.time = num;
		c.tempTime = c.time * 60;
		sbyte b = msg.reader().readByte();
		FarmScr.startTextSmall(c.vitalityPer, b, c, null);
		c.vitalityPer = b;
		c.hervestPer = msg.reader().readByte();
		c.isArid = msg.reader().readBoolean();
		c.isWorm = msg.reader().readBoolean();
		c.isGrass = msg.reader().readBoolean();
	}

	public static void readInfoAnimal(Animal ani, Message msg)
	{
		ani.bornTime = msg.reader().readInt();
		sbyte b = msg.reader().readByte();
		FarmScr.startTextSmall(ani.health, b, null, ani);
		ani.health = b;
		ani.harvestPer = msg.reader().readByte();
		ani.numEggOne = msg.reader().readByte();
		ani.hunger = msg.reader().readBoolean();
		ani.disease[0] = msg.reader().readBoolean();
		ani.disease[1] = msg.reader().readBoolean();
	}

	public static void readFarmData(Message msg)
	{
		try
		{
			int num = msg.reader().readInt();
			MyVector myVector = new MyVector();
			MyVector myVector2 = new MyVector();
			short num2 = 0;
			if (num != -1)
			{
				num2 = msg.reader().readByte();
				for (int i = 0; i < num2; i++)
				{
					CellFarm cellFarm = new CellFarm();
					cellFarm.idTree = msg.reader().readByte();
					if (cellFarm.idTree == -1)
					{
						myVector.addElement(cellFarm);
						continue;
					}
					readInfoCell(cellFarm, msg);
					myVector.addElement(cellFarm);
				}
				short num3 = msg.reader().readByte();
				if (LoadMap.TYPEMAP != 24 || GameMidlet.avatar.IDDB != num)
				{
					Cattle.numPig = 0;
					Dog.numBer = 0;
					Chicken.numChicken = 0;
					FarmScr.animalLists.removeAllElements();
				}
				for (int j = 0; j < num3; j++)
				{
					Animal animal = null;
					sbyte b = msg.reader().readByte();
					int num4 = FarmScr.animalLists.size();
					if (LoadMap.TYPEMAP != 24 || num4 == 0 || num4 != num3)
					{
						AnimalInfo animalByID = FarmData.getAnimalByID(b);
						if (b != -1)
						{
							switch (animalByID.area)
							{
							case 4:
								animal = new FishFarm(j, b, 0);
								break;
							case 2:
								animal = new Cattle(j, b);
								break;
							case 3:
								animal = new Dog(j, b);
								break;
							case 1:
								animal = new Chicken(j, b, 0);
								break;
							}
						}
					}
					else
					{
						animal = FarmScr.getAnimalByIndex(j);
						animal = (Animal)FarmScr.animalLists.elementAt(j);
					}
					if (b != -1 && animal != null)
					{
						animal.species = b;
						readInfoAnimal(animal, msg);
						myVector2.addElement(animal);
					}
				}
			}
			sbyte numBarn = msg.reader().readByte();
			sbyte numPond = msg.reader().readByte();
			FarmScr.starFruil = new StarFruitObj();
			FarmScr.starFruil.lv = msg.reader().readShort();
			StarFruitObj.imgID = msg.reader().readShort();
			FarmScr.starFruil.fruitID = msg.reader().readShort();
			FarmScr.starFruil.numberFruit = msg.reader().readShort();
			msg.reader().readShort();
			FarmScr.starFruil.anTrom = msg.reader().readShort();
			FarmScr.starFruil.timeFinish = msg.reader().readShort() * 60;
			FarmScr.starFruil.time = Canvas.getTick();
			for (int k = 0; k < num2; k++)
			{
				CellFarm cellFarm2 = (CellFarm)myVector.elementAt(k);
				cellFarm2.level = msg.reader().readByte();
			}
			short foodID = 0;
			int remainTime = 0;
			if (msg.reader().available() > 0)
			{
				foodID = msg.reader().readShort();
				remainTime = msg.reader().readShort() * 60;
			}
			FarmScr.gI().onJoin(num, myVector, myVector2, numBarn, numPond, foodID, remainTime);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
