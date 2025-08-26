using System;

public class HomeMsgHandler : IMiniGameMsgHandler
{
	private class IActionSetPass : IKbAction
	{
		private int idUser5;

		public IActionSetPass(int idUser)
		{
			idUser5 = idUser;
		}

		public void perform(string text)
		{
			AvatarService.gI().doSetPassMyHouse(text, idUser5, 1);
			Canvas.endDlg();
		}
	}

	private static HomeMsgHandler instance = new HomeMsgHandler();

	public static void onHandler()
	{
		GlobalMessageHandler.gI().miniGameMessageHandler = instance;
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case -43:
			{
				short num6 = msg.reader().readShort();
				Tile[] array2 = new Tile[num6];
				for (int l = 0; l < num6; l++)
				{
					array2[l] = new Tile();
					array2[l].name = msg.reader().readUTF();
					array2[l].priceXu = msg.reader().readInt();
					array2[l].priceLuong = msg.reader().readInt();
				}
				HouseScr.gI().onGetTileInfo(array2);
				break;
			}
			case -65:
			{
				sbyte houseType2 = msg.reader().readByte();
				int idUser2 = msg.reader().readInt();
				short[] array = null;
				short num4 = msg.reader().readShort();
				array = new short[num4];
				for (int j = 0; j < num4; j++)
				{
					array[j] = msg.reader().readByte();
				}
				sbyte w = msg.reader().readByte();
				MyVector myVector2 = new MyVector();
				short num5 = msg.reader().readShort();
				for (int k = 0; k < num5; k++)
				{
					MapItem mapItem = new MapItem();
					mapItem.typeID = msg.reader().readShort();
					mapItem.x = (mapItem.xTo = msg.reader().readByte() * 24);
					mapItem.y = (mapItem.yTo = msg.reader().readByte() * 24);
					mapItem.dir = msg.reader().readByte();
					myVector2.addElement(mapItem);
				}
				MyVector listPlayer = GlobalMessageHandler.readListPlayer(msg);
				ParkMsgHandler.onHandler();
				HouseScr.gI().onJoin(houseType2, idUser2, array, w, myVector2, listPlayer);
				break;
			}
			case 51:
				ParkMsgHandler.playerJoinBoard(msg);
				break;
			case 54:
				GlobalMessageHandler.readMove(msg);
				break;
			case 55:
				GlobalMessageHandler.readChat(msg);
				break;
			case -46:
			{
				short type = msg.reader().readShort();
				string str = msg.reader().readUTF();
				HouseScr.gI().onCreateHome(type, str);
				break;
			}
			case -66:
			{
				MapItem mapItem2 = new MapItem();
				mapItem2.typeID = msg.reader().readShort();
				mapItem2.x = msg.reader().readByte();
				mapItem2.y = msg.reader().readByte();
				HouseScr.gI().onRemoveItem(mapItem2);
				break;
			}
			case -67:
			{
				int num2 = msg.reader().readByte();
				int houseType = -1;
				short verTile = 0;
				MyVector myVector = null;
				if (num2 == 0)
				{
					verTile = msg.reader().readShort();
					houseType = msg.reader().readByte();
				}
				else
				{
					myVector = new MyVector();
					short num3 = msg.reader().readShort();
					for (int i = 0; i < num3; i++)
					{
						Avatar avatar = new Avatar();
						avatar.IDDB = msg.reader().readInt();
						avatar.typeHome = msg.reader().readByte();
						myVector.addElement(avatar);
					}
				}
				HouseScr.gI().onGetTypeHouse(num2, houseType, verTile, myVector);
				break;
			}
			case -73:
			{
				int wNum = msg.reader().readShort();
				int num = msg.reader().readInt();
				sbyte[] data = new sbyte[num];
				msg.reader().read(ref data);
				HouseScr.gI().saveTileMap(data, wNum);
				break;
			}
			case 76:
				GlobalMessageHandler.readMove(msg);
				break;
			case 77:
				GlobalMessageHandler.readChat(msg);
				break;
			case -75:
			{
				int idUser = msg.reader().readInt();
				ipKeyboard.openKeyBoard(T.setPass, ipKeyboard.PASS, string.Empty, new IActionSetPass(idUser), false);
				break;
			}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
