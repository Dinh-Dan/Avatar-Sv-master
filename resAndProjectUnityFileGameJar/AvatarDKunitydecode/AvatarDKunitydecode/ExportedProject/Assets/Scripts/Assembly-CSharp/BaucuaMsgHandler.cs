public class BaucuaMsgHandler : IMiniGameMsgHandler
{
	public static BaucuaMsgHandler me;

	public static void init()
	{
		me = new BaucuaMsgHandler();
	}

	public static void onHandler()
	{
		BoardScr.numPlayer = 5;
		BoardListOnScr.type = BoardListOnScr.STYLE_5PLAYER;
		if (onMainMenu.isOngame)
		{
			RoomListOnScr.setName(3, BCBoardScr.gI());
		}
		else
		{
			RoomListOnScr.setName((Canvas.iOpenOngame != 0) ? 1 : 3, BCBoardScr.gI());
		}
		if (me == null)
		{
			me = new BaucuaMsgHandler();
		}
		CasinoMsgHandler.me.miniGameMessageHandler = me;
	}

	public void onMessage(Message msg)
	{
		sbyte b = msg.reader().readByte();
		sbyte b2 = msg.reader().readByte();
		if (!BoardScr.setR_B(b, b2))
		{
			return;
		}
		switch (msg.command)
		{
		case 20:
		{
			sbyte b3 = msg.reader().readByte();
			BCBoardScr.me.saveTime = b3;
			BCBoardScr.me.onStartGame(b, b2, b3);
			break;
		}
		case 21:
		{
			sbyte b4 = msg.reader().readByte();
			if (b4 == -1)
			{
				BCBoardScr.me.setBotCmd();
				BCBoardScr.me.canpointer = false;
			}
			else if (b4 != -1)
			{
				for (int k = 0; k < 6; k++)
				{
					BCBoardScr.me.moneySV[b4][k] = msg.reader().readByte();
				}
				BCBoardScr.me.onMove(b4);
			}
			break;
		}
		case 37:
		{
			sbyte[] array2 = new sbyte[3];
			for (int m = 0; m < 3; m++)
			{
				array2[m] = msg.reader().readByte();
			}
			BCBoardScr.me.onResult(array2);
			BCBoardScr.me.setCmdWaiting();
			break;
		}
		case 100:
		{
			sbyte seatI = msg.reader().readByte();
			BCBoardScr.me.onSetTurn(seatI);
			break;
		}
		case 65:
		{
			sbyte b7 = msg.reader().readByte();
			sbyte b8 = msg.reader().readByte();
			sbyte b9 = msg.reader().readByte();
			sbyte b10 = msg.reader().readByte();
			if (b8 != b9 && b10 > 0)
			{
				BCBoardScr.me.moneySV[b7][b9] = b10;
				BCBoardScr.me.onHaphom(b7, b8, b9);
			}
			break;
		}
		case 60:
		{
			sbyte b5 = 0;
			sbyte b6 = 0;
			b5 = msg.reader().readByte();
			b6 = msg.reader().readByte();
			int moneyValue = msg.reader().readInt();
			BCBoardScr.me.onSetPlayer(b5, b6, moneyValue);
			break;
		}
		case 51:
		{
			int[] array = new int[BoardScr.avatarInfos.size()];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = msg.reader().readInt();
			}
			BCBoardScr.me.onFinish(array);
			break;
		}
		case 62:
		{
			sbyte b3 = msg.reader().readByte();
			BCBoardScr.me.saveTime = b3;
			for (int i = 0; i < BoardScr.avatarInfos.size(); i++)
			{
				for (int j = 0; j < 6; j++)
				{
					BCBoardScr.me.moneySV[i][j] = msg.reader().readByte();
				}
			}
			BCBoardScr.me.onPlaying(b3);
			break;
		}
		}
	}
}
