using System;

public class ParkMsgHandler : IMiniGameMsgHandler
{
	private class IActionKickOutHome : IAction
	{
		public void perform()
		{
			ParkService.gI().doJoinPark(21, 0);
		}
	}

	public static ParkMsgHandler instance;

	public static void onHandler()
	{
		if (instance == null)
		{
			instance = new ParkMsgHandler();
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
				playerJoinBoard(msg);
				break;
			case 54:
				GlobalMessageHandler.readMove(msg);
				break;
			case 55:
				GlobalMessageHandler.readChat(msg);
				break;
			case 53:
			{
				int id = msg.reader().readInt();
				MapScr.gI().onPlayerLeave(id);
				break;
			}
			case 57:
			{
				int idUser5 = msg.reader().readInt();
				sbyte idFeel = msg.reader().readByte();
				MapScr.gI().onFeel(idUser5, idFeel);
				break;
			}
			case 58:
			{
				int idFrom2 = msg.reader().readInt();
				int idTo2 = msg.reader().readInt();
				int num4 = msg.reader().readShort();
				string des = string.Empty;
				if (num4 == -1)
				{
					des = msg.reader().readUTF();
				}
				int curMoney = msg.reader().readInt();
				int typeBuy = msg.reader().readByte();
				int xu = msg.reader().readInt();
				int luong = msg.reader().readInt();
				int luongK = msg.reader().readInt();
				MapScr.gI().onGiftGiving(idFrom2, idTo2, num4, des, curMoney, typeBuy, xu, luong, luongK);
				break;
			}
			case 59:
			{
				int idFrom = msg.reader().readInt();
				int idTo = msg.reader().readInt();
				int num2 = msg.reader().readShort();
				string text = string.Empty;
				int time = 0;
				if (num2 == -1)
				{
					text = msg.reader().readUTF();
				}
				else
				{
					time = msg.reader().readShort();
				}
				MapScr.gI().onGivingDefferent(idFrom, idTo, num2, text, time);
				break;
			}
			case 60:
			{
				sbyte b2 = msg.reader().readByte();
				int[] array2 = new int[b2];
				for (int j = 0; j < b2; j++)
				{
					array2[j] = msg.reader().readByte();
				}
				MapScr.gI().onParkList(array2);
				Canvas.endDlg();
				break;
			}
			case 82:
			{
				int idUser8 = msg.reader().readInt();
				FishingScr.gI().onQuanCau(idUser8);
				break;
			}
			case 84:
			{
				int idUser7 = msg.reader().readInt();
				int idFish2 = msg.reader().readShort();
				FishingScr.gI().onFinish(idUser7, idFish2);
				break;
			}
			case 85:
			{
				int idUser6 = msg.reader().readInt();
				FishingScr.gI().onCauCaXong(idUser6);
				break;
			}
			case 91:
			{
				int idUser4 = msg.reader().readInt();
				int idFish = msg.reader().readShort();
				short timeDelay = msg.reader().readShort();
				sbyte b = msg.reader().readByte();
				sbyte[][] array = new sbyte[b][];
				for (int i = 0; i < b; i++)
				{
					short num3 = msg.reader().readShort();
					array[i] = new sbyte[num3];
					msg.reader().read(ref array[i]);
				}
				FishingScr.gI().onCaCanCau(idUser4, idFish, timeDelay, array);
				break;
			}
			case 86:
			{
				bool flag2 = msg.reader().readBoolean();
				string error = string.Empty;
				if (!flag2)
				{
					error = msg.reader().readUTF();
				}
				FishingScr.gI().onStartFishing(flag2, error);
				break;
			}
			case 87:
			{
				int idUser3 = msg.reader().readInt();
				int status = msg.reader().readByte();
				FishingScr.gI().onStatus(idUser3, status);
				break;
			}
			case 88:
			{
				int idUser2 = msg.reader().readInt();
				sbyte lv = msg.reader().readByte();
				sbyte perLv = msg.reader().readByte();
				int numFish = msg.reader().readInt();
				short idPart = msg.reader().readShort();
				FishingScr.gI().onInfo(idUser2, lv, perLv, numFish, idPart);
				break;
			}
			case -68:
			{
				sbyte type = msg.reader().readByte();
				int idUser = msg.reader().readInt();
				MapScr.gI().onInviteToMyHome(type, idUser);
				break;
			}
			case -69:
				Canvas.startOK("Bạn bị chủ nhà đuổi.", new IActionKickOutHome());
				break;
			case 92:
			{
				bool flag = msg.reader().readBoolean();
				short num = 0;
				if (flag)
				{
					num = msg.reader().readShort();
				}
				break;
			}
			case 93:
			{
				int userIDBoy = msg.reader().readInt();
				int userIDGirl = msg.reader().readInt();
				MapScr.gI().onWeddingStart(userIDBoy, userIDGirl);
				break;
			}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public static void playerJoinBoard(Message msg)
	{
		Avatar avatar = new Avatar();
		avatar.IDDB = msg.reader().readInt();
		avatar.setName(msg.reader().readUTF());
		sbyte b = msg.reader().readByte();
		for (int i = 0; i < b; i++)
		{
			avatar.addSeri(new SeriPart(msg.reader().readShort()));
		}
		avatar.x = msg.reader().readShort();
		avatar.y = msg.reader().readShort();
		avatar.blogNews = msg.reader().readByte();
		avatar.hungerPet = (sbyte)(100 - msg.reader().readByte());
		avatar.idImg = msg.reader().readShort();
		avatar.idWedding = msg.reader().readShort();
		MapScr.gI().onPlayerJoinPark(avatar);
	}
}
