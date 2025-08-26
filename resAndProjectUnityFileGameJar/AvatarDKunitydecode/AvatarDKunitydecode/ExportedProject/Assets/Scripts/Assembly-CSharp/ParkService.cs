using System;
using System.IO;

public class ParkService
{
	protected static ParkService instance;

	public static ParkService gI()
	{
		if (instance == null)
		{
			instance = new ParkService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void doJoinPark(int roomID, int boardID)
	{
		Canvas.startWaitDlg();
		Message message = new Message((sbyte)50);
		try
		{
			message.writer().writeByte((sbyte)roomID);
			message.writer().writeByte((sbyte)boardID);
			message.writer().writeShort((short)LoadMap.xJoinCasino);
			message.writer().writeShort((short)LoadMap.yJoinCasino);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doMove(int x, int y, int direct)
	{
		Message message = new Message((sbyte)54);
		try
		{
			message.writer().writeShort((short)x);
			message.writer().writeShort((short)y);
			message.writer().writeByte((sbyte)direct);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void chatToBoard(string text)
	{
		Message message = null;
		message = ((GameMidlet.CLIENT_TYPE != 10) ? new Message((sbyte)55) : new Message((sbyte)77));
		try
		{
			message.writer().writeUTF(text);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestAddFriend(int idUser)
	{
		Message message = new Message((sbyte)(-21));
		try
		{
			message.writer().writeInt(idUser);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doAddFriend(int iddb, bool b)
	{
		Message message = new Message((sbyte)(-19));
		try
		{
			message.writer().writeInt(iddb);
			message.writer().writeBoolean(b);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doGiftGiving(int iddb, int i, int typeBuy)
	{
		Message message = new Message((sbyte)58);
		try
		{
			message.writer().writeInt(iddb);
			message.writer().writeShort((short)i);
			message.writer().writeByte((sbyte)typeBuy);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doGivingDeferrent(int idTo, int id)
	{
		Out.println("doGivingDeferrent: " + id);
		Message message = new Message((sbyte)59);
		try
		{
			message.writer().writeInt(idTo);
			message.writer().writeShort((short)id);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestYourInfo(int iddb)
	{
		Message message = new Message((sbyte)(-22));
		try
		{
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestBoardList(sbyte roomId)
	{
		Message message = new Message((sbyte)60);
		try
		{
			message.writer().writeByte(roomId);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doBuyItem(short id)
	{
		Message message = new Message((sbyte)(-38));
		try
		{
			message.writer().writeShort(id);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doKick(sbyte roomId, sbyte boardId, int iddb)
	{
		Message message = new Message((sbyte)78);
		try
		{
			message.writer().writeByte(roomId);
			message.writer().writeByte(boardId);
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doQuanCau()
	{
		Message m = new Message((sbyte)82);
		send(m);
	}

	public void doFinishFishing(bool isF, sbyte[] index)
	{
		Message message = new Message((sbyte)84);
		try
		{
			message.writer().writeBoolean(isF);
			message.writer().writeByte((sbyte)index.Length);
			for (int i = 0; i < index.Length; i++)
			{
				Canvas.test1 = Canvas.test1 + index[i] + ", ";
				message.writer().writeByte(index[i]);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doCauCaXong()
	{
		Message m = new Message((sbyte)85);
		send(m);
	}

	public void doStartFishing()
	{
		Message m = new Message((sbyte)86);
		send(m);
	}

	public void doInviteToMyHome(int type, int iddb)
	{
		Message message = new Message((sbyte)(-68));
		try
		{
			message.writer().writeByte((sbyte)type);
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doCustomPopup(int idBoss, int idPopup, int ii)
	{
		Message message = new Message((sbyte)(-77));
		try
		{
			message.writer().writeInt(idBoss);
			message.writer().writeByte((sbyte)idPopup);
			message.writer().writeByte((sbyte)ii);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doBossShop(int idBoss1, int idShop, int ii)
	{
		Message message = new Message((sbyte)(-78));
		try
		{
			message.writer().writeInt(idBoss1);
			message.writer().writeByte((sbyte)idShop);
			message.writer().writeShort((short)ii);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doGetDropPart(int idDrop)
	{
		Message message = new Message((sbyte)89);
		try
		{
			message.writer().writeByte(0);
			message.writer().writeInt(idDrop);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestWedding(int roomID, int boardID)
	{
		Message message = new Message((sbyte)93);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
