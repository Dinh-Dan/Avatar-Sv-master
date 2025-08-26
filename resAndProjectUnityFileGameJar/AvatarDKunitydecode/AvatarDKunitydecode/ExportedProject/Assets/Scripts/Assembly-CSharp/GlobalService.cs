using System;
using System.IO;
using UnityEngine;

public class GlobalService
{
	protected static GlobalService instance;

	public static GlobalService gI()
	{
		if (instance == null)
		{
			instance = new GlobalService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void chatToBoard(sbyte roomID, sbyte boardID, string text)
	{
		Message message = new Message((sbyte)9);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeUTF(text);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void leaveBoard(sbyte roomID, sbyte boardID)
	{
		Message message = new Message((sbyte)15);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void ready(sbyte roomID, sbyte boardID, bool isReady)
	{
		Message message = new Message((sbyte)16);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeBoolean(isReady);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void setMoney(sbyte roomID, sbyte boardID, int money)
	{
		Message message = new Message((sbyte)19);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeInt(money);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void setPassword(sbyte roomID, sbyte boardID, string pass)
	{
		Message message = new Message((sbyte)18);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeUTF(pass);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void kick(sbyte roomID, sbyte boardID, int kickID)
	{
		Message message = new Message((sbyte)11);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeInt(kickID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void startGame(sbyte roomID, sbyte boardID)
	{
		Message message = new Message((sbyte)20);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void requestService(sbyte service, string arg)
	{
		if (arg == null)
		{
			arg = string.Empty;
		}
		Message message = new Message((sbyte)(-55));
		try
		{
			message.writer().writeByte(service);
			message.writer().writeUTF(arg);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void setProviderAndClientType()
	{
		Message message = new Message((sbyte)(-1));
		message.writer().writeByte(GameMidlet.CLIENT_TYPE);
		send(message);
		Message message2 = new Message((sbyte)(-17));
		try
		{
			message2.writer().writeByte(GameMidlet.PROVIDER);
			message2.writer().writeInt(10000);
			string value = "uni";
			message2.writer().writeUTF(value);
			message2.writer().writeInt(10000);
			message2.writer().writeInt(Canvas.w);
			message2.writer().writeInt(Canvas.h);
			message2.writer().writeBoolean(true);
			message2.writer().writeByte((sbyte)(AvMain.hd - 1));
			message2.writer().writeUTF("2.5.8");
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		send(message2);
		Message message3 = new Message((sbyte)(-79));
		message3.writer().writeUTF(GameMidlet.AGENT);
		send(message3);
		Message message4 = new Message(-86);
		message4.writer().writeByte(2);
		if (!ScaleGUI.isAndroid)
		{
			message4.writer().writeByte(GameMidlet.VERSIONCODE);
			message4.writer().writeUTF(SystemInfo.deviceUniqueIdentifier + string.Empty);
		}
		send(message4);
	}

	public void requestInfoOf(int iDDB)
	{
		Message message = new Message((sbyte)34);
		try
		{
			message.writer().writeInt(iDDB);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void login(string username, string pass, string version)
	{
		Out.println("login: -" + username + "------" + pass + "-");
		Message message = new Message((sbyte)(-2));
		try
		{
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			message.writer().writeUTF(version);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void setGameType(int gameType)
	{
		Message message = new Message((sbyte)61);
		try
		{
			message.writer().writeByte(gameType);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void requestRegister(string username, string pass, string introductionCode, string email)
	{
		Message message = new Message((sbyte)(-30));
		try
		{
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			message.writer().writeUTF(introductionCode);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void requestRegister(string username, string pass)
	{
		Message message = new Message((sbyte)(-30));
		try
		{
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void addFriend(int id)
	{
		Message message = new Message((sbyte)(-19));
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

	public void requestAvatar(short avatar)
	{
		Message message = new Message((sbyte)38);
		try
		{
			message.writer().writeShort(avatar);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void chatTo(int iddb, string text)
	{
		Out.println("chatTo: " + iddb + "    " + text);
		Message message = new Message((sbyte)(-6));
		try
		{
			message.writer().writeInt(iddb);
			message.writer().writeUTF(text);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void requestAvatarShop()
	{
		Message m = new Message((sbyte)39);
		send(m);
	}

	public void requestChargeMoneyInfo()
	{
		Message m = new Message((sbyte)(-23));
		send(m);
	}

	public void sendSMSSuccess(string content, string smsTo)
	{
		Message message = new Message((sbyte)57);
		try
		{
			message.writer().writeUTF(content);
			message.writer().writeUTF(smsTo);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestCreCharacter()
	{
		Message message = new Message((sbyte)(-35));
		try
		{
			message.writer().writeByte(GameMidlet.avatar.gender);
			int num = GameMidlet.avatar.seriPart.size();
			message.writer().writeByte((sbyte)num);
			for (int i = 0; i < num; i++)
			{
				SeriPart seriPart = (SeriPart)GameMidlet.avatar.seriPart.elementAt(i);
				message.writer().writeShort(seriPart.idPart);
			}
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRemoveItem(int part, int type)
	{
		Message message = new Message((sbyte)(-36));
		try
		{
			Out.println("doRemoveItem: " + part + "    " + type);
			message.writer().writeShort((short)part);
			message.writer().writeByte((sbyte)type);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestContainer(int iddb)
	{
		Message message = new Message((sbyte)(-47));
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

	public void doUsingItem(short idPart, sbyte type)
	{
		Message message = new Message((sbyte)(-48));
		try
		{
			message.writer().writeShort(idPart);
			message.writer().writeByte(type);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void getHandler(int index)
	{
		Message message = new Message((sbyte)(-1));
		try
		{
			message.writer().writeByte(index);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestSoundData(sbyte id)
	{
		Message message = new Message((sbyte)(-51));
		try
		{
			message.writer().writeByte(id);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void requestShop(int typeShop)
	{
		Message message = new Message((sbyte)(-49));
		try
		{
			message.writer().writeByte(typeShop);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doRequestNumSupport(int hashCode)
	{
		Message message = new Message((sbyte)(-52));
		try
		{
			message.writer().writeInt(hashCode);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateContainer(int index)
	{
		Message message = new Message((sbyte)(-53));
		try
		{
			message.writer().writeByte(index);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doMenuOption(int userID, sbyte idMenu, int i)
	{
		Message message = new Message((sbyte)(-59));
		try
		{
			message.writer().writeInt(userID);
			message.writer().writeByte(idMenu);
			message.writer().writeByte((sbyte)i);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doTextBox(int userID, sbyte idMenu, string text)
	{
		Message message = new Message((sbyte)(-60));
		try
		{
			message.writer().writeInt(userID);
			message.writer().writeByte(idMenu);
			message.writer().writeUTF(text);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doCommunicate(int idUser)
	{
		Message message = new Message((sbyte)(-61));
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

	public void doLoadCard(string text, string text2, string link)
	{
		Message message = new Message((sbyte)(-56));
		try
		{
			message.writer().writeUTF(text);
			message.writer().writeUTF(text2);
			message.writer().writeUTF(link);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doChangePass(string PassOld, string passNew_1)
	{
		Message message = new Message((sbyte)(-62));
		try
		{
			message.writer().writeUTF(PassOld);
			message.writer().writeUTF(passNew_1);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doDialLucky(short part, int selectedNumber)
	{
		Message message = new Message((sbyte)(-64));
		try
		{
			message.writer().writeShort(part);
			message.writer().writeShort((short)selectedNumber);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doServerKick(int iddb, string text)
	{
		Message message = new Message((sbyte)(-72));
		try
		{
			message.writer().writeInt(iddb);
			message.writer().writeUTF(text);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doListCustom(int idList, sbyte page, int selected, sbyte idmenu)
	{
		Message message = new Message((sbyte)(-81));
		try
		{
			message.writer().writeInt(idList);
			message.writer().writeByte(page);
			message.writer().writeShort((short)selected);
			message.writer().writeByte(idmenu);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestCmdRotate(int anthor, int iddb)
	{
		Message message = new Message((sbyte)(-83));
		try
		{
			message.writer().writeShort((short)anthor);
			message.writer().writeInt(iddb);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doCustomTab(sbyte idAction)
	{
		Message message = new Message((sbyte)(-58));
		try
		{
			message.writer().writeByte(idAction);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doUpdateChest(int i)
	{
		Out.println("doUpdateChest: " + i);
		Message message = new Message((sbyte)(-90));
		try
		{
			message.writer().writeByte(i);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doEnterPass(string text, sbyte type)
	{
		Message message = new Message((sbyte)(-88));
		try
		{
			Out.println("enter Pass: " + text + " type: " + type);
			message.writer().writeByte(type);
			message.writer().writeUTF(text);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestChest()
	{
		Message m = new Message((sbyte)(-87));
		send(m);
	}

	public void doChangeChestPass(string text, string text2)
	{
		Message message = new Message((sbyte)(-88));
		try
		{
			message.writer().writeByte(1);
			message.writer().writeUTF(text);
			message.writer().writeUTF(text2);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doTransChestPart(int i, int ii, short idPart)
	{
		Message message = new Message((sbyte)(-89));
		try
		{
			message.writer().writeByte((sbyte)i);
			message.writer().writeShort((short)ii);
			message.writer().writeShort(idPart);
			Out.println("send get chest part: bytetype: " + i + " idIndex: " + ii + " idPart: " + idPart);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void joinAvatar()
	{
		Message m = new Message((sbyte)(-96));
		send(m);
	}

	public void requestCityMap(sbyte idMini)
	{
		Message message = new Message((sbyte)(-92));
		try
		{
			if (idMini != -1)
			{
				message.writer().writeByte(idMini);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void requestTileMap(sbyte idTileImg)
	{
		Message message = new Message((sbyte)(-94));
		try
		{
			message.writer().writeByte(idTileImg);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doSelectedMiniMap(sbyte idCityMap, sbyte id)
	{
		Message message = new Message((sbyte)(-93));
		try
		{
			message.writer().writeByte(idCityMap);
			message.writer().writeByte(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void requestJoinAny(short idJoin)
	{
		Message message = new Message((sbyte)(-95));
		try
		{
			message.writer().writeByte(MapScr.idCityMap);
			message.writer().writeByte(MapScr.idSelectedMini);
			message.writer().writeShort(idJoin);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void requestPartDynaMic(short idPart)
	{
		Message message = new Message((sbyte)(-97));
		try
		{
			message.writer().writeShort(idPart);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void requestImagePart(short id)
	{
		Message message = new Message((sbyte)(-98));
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

	public void doRequestMoneyLoad(string iD)
	{
		Message message = new Message((sbyte)(-91));
		try
		{
			message.writer().writeUTF(iD);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRemoveFriend(int id)
	{
		Message message = new Message((sbyte)(-20));
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

	public void doJoinOfflineMap(int i)
	{
		Canvas.startWaitDlg();
		Message message = new Message((sbyte)(-99));
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

	public void doJoinRoomRace()
	{
		Canvas.startWaitDlg();
		Message m = new Message((sbyte)1);
		send(m);
	}

	public void doDatCuoc(int idPet, int money)
	{
		Canvas.startWaitDlg();
		Message message = new Message((sbyte)5);
		try
		{
			message.writer().writeByte(idPet);
			message.writer().writeInt(money);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doPetInfo(int idPet)
	{
		Message message = new Message((sbyte)2);
		try
		{
			message.writer().writeByte(idPet);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doHistoryRace()
	{
		Message m = new Message((sbyte)8);
		send(m);
	}

	public void chatToBoard(string text)
	{
		Message message = new Message((sbyte)9);
		try
		{
			message.writer().writeUTF(text);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRequestSky()
	{
		Message m = new Message((sbyte)92);
		send(m);
	}

	public void transXeng(int money)
	{
		Message message = new Message((sbyte)(-102));
		try
		{
			message.writer().writeInt(money);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doFlowerLove()
	{
		Message m = new Message((sbyte)(-105));
		send(m);
	}

	public void doFlowerLoveSelected(int ii)
	{
		Message message = new Message((sbyte)(-106));
		try
		{
			message.writer().writeByte(ii);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doSendOpenShopHouse(sbyte typeShop, short idItem)
	{
		Message message = new Message((sbyte)(-107));
		try
		{
			message.writer().writeByte(typeShop);
			message.writer().writeShort(idItem);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doRegisterByEmail(string name, string pass, string email)
	{
		Out.println("doRegisterByEmail: " + name + "   " + pass + "   " + email);
		Message message = new Message((sbyte)(-25));
		try
		{
			message.writer().writeUTF(name);
			message.writer().writeUTF(pass);
			message.writer().writeUTF(email);
			message.writer().writeByte(0);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doLoginNewGame()
	{
		Out.println("doLoginNewGame");
		Message m = new Message((sbyte)(-12));
		send(m);
	}

	public void createCharInfo(string name, int ngay, int thang, int nam, string address, string cmnd, string ngayCap, string noiCap, string sdt)
	{
		Out.println("createCharInfo: " + name + "  " + ngay + "/" + thang + "/" + nam + "   " + address + "   " + cmnd + "   " + ngayCap + "   " + noiCap + "   " + sdt);
		Message message = new Message((sbyte)(-106));
		try
		{
			message.writer().writeUTF(name);
			message.writer().writeByte(ngay);
			message.writer().writeByte(thang);
			message.writer().writeShort(nam);
			message.writer().writeUTF(address);
			message.writer().writeUTF(cmnd);
			message.writer().writeUTF(ngayCap);
			message.writer().writeUTF(noiCap);
			message.writer().writeUTF(sdt);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
