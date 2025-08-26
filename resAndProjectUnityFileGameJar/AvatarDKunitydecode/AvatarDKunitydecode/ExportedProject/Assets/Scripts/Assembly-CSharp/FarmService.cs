using System;
using System.IO;

public class FarmService
{
	private static FarmService instance;

	public static FarmService gI()
	{
		if (instance == null)
		{
			instance = new FarmService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void setBigData()
	{
		Message m = new Message((sbyte)51);
		send(m);
	}

	public void getBigImage(short id)
	{
		Message message = new Message((sbyte)54);
		try
		{
			message.writer().writeShort(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		Canvas.startWaitDlg(T.getFarmData);
	}

	public void getImageData()
	{
		Message m = new Message((sbyte)55);
		send(m);
		Canvas.startWaitDlg(T.getFarmData);
	}

	public void getTreeInfo()
	{
		Message m = new Message((sbyte)56);
		send(m);
		Canvas.startWaitDlg(T.getFarmData);
	}

	public void getInventory()
	{
		Message m = new Message((sbyte)60);
		send(m);
	}

	public void doJoinFarm(int id)
	{
		Message message = new Message((sbyte)61);
		try
		{
			message.writer().writeInt(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doBuyItem(short id, sbyte n, int typeShop, int money)
	{
		Message message = new Message((sbyte)62);
		try
		{
			message.writer().writeShort(id);
			message.writer().writeByte(n);
			message.writer().writeByte((sbyte)typeShop);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		Canvas.endDlg();
	}

	public void doSellItem(short idItem)
	{
		Message message = new Message((sbyte)63);
		try
		{
			message.writer().writeShort(idItem);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doPlantSeed(int idUser, int indexCell, int idSeed)
	{
		Message message = new Message((sbyte)64);
		try
		{
			message.writer().writeInt(idUser);
			message.writer().writeByte((sbyte)indexCell);
			message.writer().writeByte((sbyte)idSeed);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doUsingItem(int idUser, int indexCell, int idItem)
	{
		Message message = new Message((sbyte)65);
		try
		{
			message.writer().writeInt(idUser);
			message.writer().writeByte((sbyte)indexCell);
			message.writer().writeShort((short)idItem);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doHervest(int idFarm, int indexCell)
	{
		Message message = new Message((sbyte)66);
		try
		{
			message.writer().writeInt(idFarm);
			message.writer().writeByte((sbyte)indexCell);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doOpenLand(int idFarm, int typeBuy)
	{
		Message message = new Message((sbyte)70);
		try
		{
			message.writer().writeInt(idFarm);
			message.writer().writeByte((sbyte)typeBuy);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestPricePlant(int idFarm)
	{
		Message message = new Message((sbyte)69);
		try
		{
			message.writer().writeInt(idFarm);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doHarvestAnimal(int idFarm, int index)
	{
		Message message = new Message((sbyte)74);
		try
		{
			message.writer().writeInt(idFarm);
			message.writer().writeByte((sbyte)index);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doSellAnimal(int idFarm, sbyte iddb)
	{
		Message message = new Message((sbyte)73);
		try
		{
			message.writer().writeInt(idFarm);
			message.writer().writeByte(iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doBuyAnimal(AnimalInfo animal, int typeBuy)
	{
		FarmScr.cell = null;
		Canvas.endDlg();
		Message message = new Message((sbyte)71);
		try
		{
			message.writer().writeByte(animal.species);
			message.writer().writeByte((sbyte)typeBuy);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestPriceAnimal(int idFarm, int iddb)
	{
		Message message = new Message((sbyte)72);
		try
		{
			message.writer().writeInt(idFarm);
			message.writer().writeByte((sbyte)iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doTransMoney(int money, int type)
	{
		Message message = new Message((sbyte)75);
		try
		{
			message.writer().writeInt(money);
			message.writer().writeByte((sbyte)type);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateFarm(int type, int typeMoney)
	{
		Message message = new Message((sbyte)80);
		try
		{
			message.writer().writeByte(type);
			if (type == 1)
			{
				message.writer().writeByte(typeMoney);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateFish(int type, int typeMoney)
	{
		Message message = new Message((sbyte)81);
		try
		{
			message.writer().writeByte(type);
			if (type == 1)
			{
				message.writer().writeByte(typeMoney);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doGetImgIcon(short id)
	{
		Message message = new Message((sbyte)82);
		try
		{
			message.writer().writeShort(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void getInfoStarFruit()
	{
		Message m = new Message((sbyte)87);
		send(m);
	}

	public void doUpdateStarFruil(int i)
	{
		Message message = new Message((sbyte)84);
		try
		{
			message.writer().writeByte(i);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doHarvestStarFruit()
	{
		Message m = new Message((sbyte)85);
		send(m);
	}

	public void doUpdateStarFruitByMoney(int i)
	{
		Message message = new Message((sbyte)86);
		try
		{
			message.writer().writeByte(i);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateLand(int step, int typeMoney)
	{
		Message message = new Message((sbyte)90);
		try
		{
			message.writer().writeByte(step);
			if (step == 1)
			{
				message.writer().writeByte(typeMoney);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateStore(int step, int typeMoney)
	{
		Message message = new Message((sbyte)94);
		try
		{
			message.writer().writeByte(step);
			if (step == 1)
			{
				message.writer().writeByte(typeMoney);
			}
		}
		catch (Exception)
		{
		}
		send(message);
	}

	public void doCooking(short iD)
	{
		Canvas.startWaitDlg();
		Message message = new Message((sbyte)91);
		try
		{
			message.writer().writeShort(iD);
			send(message);
		}
		catch (Exception)
		{
		}
	}

	public void nauNhanh(int step)
	{
		Message message = new Message((sbyte)93);
		try
		{
			message.writer().writeByte(step);
			send(message);
		}
		catch (Exception)
		{
		}
	}

	public void doHarvestCook()
	{
		Message m = new Message((sbyte)92);
		send(m);
	}

	public void doFinishStarFruit()
	{
		Message m = new Message((sbyte)83);
		send(m);
	}

	public void doStealInfo()
	{
		Message m = new Message((sbyte)95);
		send(m);
	}

	public void doSteal(int step)
	{
		Message message = new Message((sbyte)96);
		try
		{
			message.writer().writeByte(step);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doStealStore()
	{
		Message m = new Message((sbyte)97);
		send(m);
	}

	public void doLichSuAnTrom()
	{
		Message m = new Message((sbyte)98);
		send(m);
	}
}
