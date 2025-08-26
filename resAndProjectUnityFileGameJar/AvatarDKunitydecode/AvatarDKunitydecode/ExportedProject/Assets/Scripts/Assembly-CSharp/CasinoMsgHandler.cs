using System;

public class CasinoMsgHandler : IMiniGameMsgHandler
{
	private class IActionKick : IAction
	{
		public void perform()
		{
			Canvas.startWaitDlg();
			CasinoService.gI().requestBoardList(BoardScr.roomID);
		}
	}

	public static CasinoMsgHandler me = new CasinoMsgHandler();

	public IMiniGameMsgHandler miniGameMessageHandler;

	public static BoardScr curScr;

	public static void onHandler()
	{
		GlobalMessageHandler.gI().miniGameMessageHandler = me;
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case 61:
				switch (msg.reader().readByte())
				{
				default:
					return;
				case 3:
					TienLenMsgHandler.onHandler();
					break;
				case 7:
					PhomMsgHandler.onHandler();
					break;
				case 21:
					DiamondMessageHandler.onHandler();
					break;
				case 22:
					BaucuaMsgHandler.onHandler();
					break;
				}
				CasinoService.gI().requestRoomList();
				break;
			case 6:
			{
				MyVector myVector3 = new MyVector();
				while (msg.reader().available() > 0)
				{
					RoomInfo roomInfo = new RoomInfo();
					roomInfo.id = msg.reader().readByte();
					roomInfo.roomFree = msg.reader().readByte();
					roomInfo.roomWait = msg.reader().readByte();
					roomInfo.lv = msg.reader().readByte();
					myVector3.addElement(roomInfo);
				}
				RoomListOnScr.gI().setRoomList(myVector3);
				RoomListOnScr.gI().switchToMe();
				Canvas.load = -1;
				if (!onMainMenu.isOngame)
				{
					onMainMenu.isOngame = true;
					onMainMenu.initSize();
					Canvas.paint.initOngame();
				}
				Canvas.endDlg();
				break;
			}
			case 7:
			{
				MyVector myVector = new MyVector();
				sbyte roomID = msg.reader().readByte();
				while (msg.reader().available() > 0)
				{
					BoardInfo boardInfo = new BoardInfo();
					boardInfo.boardID = msg.reader().readByte();
					int num3 = msg.reader().readUnsignedByte();
					boardInfo.nPlayer = (sbyte)(num3 % 16);
					boardInfo.maxPlayer = (sbyte)(num3 / 16);
					int num4 = msg.reader().readUnsignedByte();
					boardInfo.isPass = (num4 & 1) != 0;
					boardInfo.isPlaying = (num4 & 2) != 0;
					boardInfo.money = msg.reader().readInt();
					boardInfo.strMoney = Canvas.getMoneys(boardInfo.money);
					myVector.addElement(boardInfo);
				}
				BoardListOnScr.gI().roomID = roomID;
				BoardListOnScr.gI().setBoardList(myVector);
				BoardListOnScr.gI().setCam();
				Canvas.load = -1;
				BoardListOnScr.gI().switchToMe();
				Canvas.endDlg();
				break;
			}
			case 8:
			{
				Canvas.load = 0;
				sbyte b = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				int num7 = msg.reader().readInt();
				int money2 = msg.reader().readInt();
				MyVector myVector2 = new MyVector();
				while (msg.reader().available() > 0)
				{
					Avatar avatar2 = new Avatar();
					avatar2.IDDB = msg.reader().readInt();
					if (avatar2.IDDB == -1)
					{
						avatar2.setName(string.Empty);
					}
					else
					{
						if (avatar2.IDDB == GameMidlet.avatar.IDDB)
						{
							avatar2 = GameMidlet.avatar;
						}
						avatar2.setName(msg.reader().readUTF());
						avatar2.setMoneyNew(msg.reader().readInt());
						sbyte b4 = msg.reader().readByte();
						for (int j = 0; j < b4; j++)
						{
							SeriPart seri = new SeriPart(msg.reader().readShort());
							if (avatar2.IDDB != GameMidlet.avatar.IDDB)
							{
								avatar2.addSeri(seri);
							}
						}
						int exp = msg.reader().readInt();
						avatar2.setExp(exp);
						avatar2.isReady = msg.reader().readBoolean();
						avatar2.setExp(exp);
						avatar2.setMoneyNew(avatar2.getMoneyNew());
						avatar2.idImg = msg.reader().readShort();
					}
					myVector2.addElement(avatar2);
				}
				curScr.setPlayers(b, b2, num7, money2, myVector2);
				TLBoardScr.gI().isFirstMatch = true;
				BoardScr.disableReady = false;
				int num8 = myVector2.size();
				for (int k = 0; k < num8; k++)
				{
					Avatar avatar3 = (Avatar)myVector2.elementAt(k);
					if (avatar3.IDDB == num7)
					{
						avatar3.isReady = true;
					}
					if (avatar3.IDDB == GameMidlet.avatar.IDDB)
					{
						GameMidlet.avatar.setMoneyNew(avatar3.getMoneyNew());
					}
				}
				if (BoardListOnScr.type == BoardListOnScr.STYLE_2PLAYER)
				{
					curScr.loadMap(60);
				}
				else if (BoardListOnScr.type == BoardListOnScr.STYLE_4PLAYER)
				{
					curScr.loadMap(61);
				}
				else
				{
					curScr.loadMap(65);
				}
				curScr.switchToMe();
				TLBoardScr.gI().setMode(false);
				Canvas.endDlg();
				Canvas.load = 1;
				break;
			}
			case 12:
			{
				Avatar avatar = new Avatar();
				int seat = msg.reader().readByte();
				avatar.IDDB = msg.reader().readInt();
				avatar.setName(msg.reader().readUTF());
				avatar.setMoneyNew(msg.reader().readInt());
				sbyte b3 = msg.reader().readByte();
				for (int i = 0; i < b3; i++)
				{
					avatar.addSeri(new SeriPart(msg.reader().readShort()));
				}
				avatar.direct = Base.RIGHT;
				avatar.setExp(msg.reader().readInt());
				avatar.idImg = msg.reader().readShort();
				avatar.isReady = false;
				TLBoardScr.gI().isFirstMatch = true;
				avatar.isReady = false;
				curScr.setAt(seat, avatar);
				break;
			}
			case 14:
			{
				int leaveID = msg.reader().readInt();
				int owner = msg.reader().readInt();
				if (BoardScr.isStartGame && BoardScr.numPlayer == 2)
				{
					curScr.closeBoard(T.opponentQuit);
				}
				TLBoardScr.gI().isFirstMatch = true;
				BoardScr.me.playerLeave(leaveID);
				BoardScr.setOwner(owner);
				break;
			}
			case 16:
			{
				int num5 = msg.reader().readInt();
				bool isReady = msg.reader().readBoolean();
				if (num5 == GameMidlet.avatar.IDDB)
				{
					Canvas.endDlg();
				}
				curScr.setReady(num5, isReady);
				break;
			}
			case 19:
			{
				sbyte b = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				int money = msg.reader().readInt();
				if (BoardScr.setR_B(b, b2))
				{
					curScr.setMoney(money);
				}
				break;
			}
			case 9:
			{
				sbyte b = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				int fromID = msg.reader().readInt();
				string text2 = msg.reader().readUTF();
				if (BoardScr.setR_B(b, b2))
				{
					BoardScr.showChat(fromID, text2);
				}
				break;
			}
			case 11:
			{
				sbyte b = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				int num6 = msg.reader().readInt();
				Canvas.currentDialog = null;
				if (BoardScr.setR_B(b, b2))
				{
					if (num6 == GameMidlet.avatar.IDDB)
					{
						Canvas.startOK(T.youAreKicked, new IActionKick());
					}
					else
					{
						BoardScr.me.playerLeave(num6);
					}
				}
				break;
			}
			case 52:
			{
				sbyte b = msg.reader().readByte();
				sbyte b2 = msg.reader().readByte();
				int num = msg.reader().readInt();
				int num2 = msg.reader().readInt();
				string text = msg.reader().readUTF();
				Avatar avatarByID = BoardScr.getAvatarByID(num);
				if (num2 != 0 && avatarByID != null)
				{
					avatarByID.setMoneyNew(avatarByID.getMoneyNew() + num2);
					if (GameMidlet.avatar.IDDB == num)
					{
						GameMidlet.avatar.setMoneyNew(avatarByID.getMoneyNew());
					}
					BoardScr.showChat(num, text);
					BoardScr.showFlyText(num, num2);
				}
				break;
			}
			default:
				miniGameMessageHandler.onMessage(msg);
				break;
			}
		}
		catch (Exception)
		{
		}
	}
}
