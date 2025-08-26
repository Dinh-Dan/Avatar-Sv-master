using System;
using System.IO;

public class AvatarService
{
	protected static AvatarService instance;

	public static AvatarService gI()
	{
		if (instance == null)
		{
			instance = new AvatarService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void getBigData()
	{
		Message m = new Message((sbyte)(-11));
		send(m);
	}

	public void getBigImage(short id)
	{
		Message message = new Message((sbyte)(-14));
		try
		{
			message.writer().writeShort(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		Canvas.startWaitDlg(T.getData);
		MsgDlg.isBack = false;
	}

	public void getImageData()
	{
		Message m = new Message((sbyte)(-15));
		send(m);
		MsgDlg.isBack = false;
	}

	public void getAvatarPart()
	{
		Message m = new Message((sbyte)(-16));
		send(m);
		MsgDlg.isBack = false;
	}

	public void getItemInfo()
	{
		Message m = new Message((sbyte)(-37));
		send(m);
		MsgDlg.isBack = false;
	}

	public void getMapItemType()
	{
		Message m = new Message((sbyte)(-40));
		send(m);
	}

	public void getMapItem()
	{
		Message m = new Message((sbyte)(-41));
		send(m);
	}

	public void doFeel(int focusFeel)
	{
		if (GameMidlet.CLIENT_TYPE != 9 && GameMidlet.CLIENT_TYPE != 11)
		{
			return;
		}
		Message message = new Message((sbyte)57);
		try
		{
			message.writer().writeByte(focusFeel);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doBuyItem(int part, int typeBuy)
	{
		Message message = new Message((sbyte)(-24));
		try
		{
			message.writer().writeShort((short)part);
			message.writer().writeByte((sbyte)typeBuy);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doGetTileInfo()
	{
		Message m = new Message((sbyte)(-43));
		send(m);
	}

	public void doCreateHome(short[] map, int type)
	{
		Message message = new Message((sbyte)(-46));
		try
		{
			message.writer().writeShort((short)type);
			message.writer().writeShort((short)map.Length);
			for (int i = 0; i < map.Length; i++)
			{
				message.writer().writeByte((sbyte)map[i]);
			}
			message.writer().writeShort(0);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doBuyItemHouse(MapItem map)
	{
		Message message = new Message((sbyte)(-74));
		try
		{
			message.writer().writeShort(map.typeID);
			message.writer().writeByte((sbyte)(map.x / 24));
			message.writer().writeByte((sbyte)(map.y / 24));
			message.writer().writeByte((sbyte)map.type);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doJoinHouse(int iddb)
	{
		Canvas.startWaitDlg();
		Message message = new Message((sbyte)(-65));
		try
		{
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void dodelItem(MapItem pos)
	{
		Message message = new Message((sbyte)(-66));
		try
		{
			message.writer().writeShort(pos.typeID);
			message.writer().writeByte((sbyte)(pos.x / 24));
			message.writer().writeByte((sbyte)(pos.y / 24));
			message.writer().writeByte(pos.dir);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void getTypeHouse(int type)
	{
		Message message = new Message((sbyte)(-67));
		try
		{
			message.writer().writeByte(type);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doKickOutHome(int iddb)
	{
		Message message = new Message((sbyte)(-69));
		try
		{
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestExpicePet(int idUser)
	{
		Message message = new Message((sbyte)(-70));
		try
		{
			message.writer().writeInt(idUser);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doSortItem(int anchor, int x, int y, int x2, int y2, int dir)
	{
		Message message = new Message((sbyte)(-71));
		try
		{
			message.writer().writeShort((short)anchor);
			message.writer().writeByte((sbyte)x);
			message.writer().writeByte((sbyte)y);
			message.writer().writeByte((sbyte)x2);
			message.writer().writeByte((sbyte)y2);
			message.writer().writeByte((sbyte)dir);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doGetTileMap()
	{
		Message m = new Message((sbyte)(-73));
		send(m);
	}

	public void doSetPassMyHouse(string text, int idUser, int type)
	{
		Message message = new Message((sbyte)(-75));
		try
		{
			message.writer().writeByte((sbyte)type);
			message.writer().writeUTF(text);
			if (type == 1)
			{
				message.writer().writeInt(idUser);
			}
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doSMSServerLoad(string link)
	{
		Message message = new Message((sbyte)(-76));
		try
		{
			message.writer().writeUTF(link);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doGetImgIcon(short id)
	{
		Message message = new Message((sbyte)(-80));
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

	public void doRequestEffectData(short id3)
	{
		Message message = new Message((sbyte)(-84));
		try
		{
			message.writer().writeByte(id3);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doJoinHouse4(int id)
	{
		Message message = new Message((sbyte)(-104));
		try
		{
			message.writer().writeInt(id);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}
}
