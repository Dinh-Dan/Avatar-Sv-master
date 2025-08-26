using System;

public class TienLenMsgHandler : IMiniGameMsgHandler
{
	public static TienLenMsgHandler instance = new TienLenMsgHandler();

	public static void onHandler()
	{
		BoardScr.numPlayer = 4;
		BoardListOnScr.type = BoardListOnScr.STYLE_4PLAYER;
		RoomListOnScr.title = T.nameCasinoOngame[0];
		CasinoMsgHandler.curScr = TLBoardScr.gI();
		CasinoMsgHandler.me.miniGameMessageHandler = instance;
	}

	public void onMessage(Message msg)
	{
		try
		{
			sbyte roomID = msg.reader().readByte();
			sbyte boardID = msg.reader().readByte();
			if (!BoardScr.setR_B(roomID, boardID))
			{
				return;
			}
			switch (msg.command)
			{
			case 51:
			{
				int whoFinish = msg.reader().readInt();
				sbyte finishPosition = msg.reader().readByte();
				int dMoney = msg.reader().readInt();
				int dExp = msg.reader().readInt();
				TLBoardScr.gI().finish(whoFinish, finishPosition, dMoney, dExp);
				break;
			}
			case 50:
				TLBoardScr.gI().isFirstMatch = false;
				TLBoardScr.gI().stopGame();
				if (msg.reader().available() > 0)
				{
					int whoShow = msg.reader().readInt();
					sbyte b = msg.reader().readByte();
					sbyte[] array2 = new sbyte[b];
					for (int j = 0; j < b; j++)
					{
						array2[j] = msg.reader().readByte();
					}
					TLBoardScr.gI().showCards(whoShow, array2);
				}
				break;
			case 20:
			{
				sbyte interval = msg.reader().readByte();
				MyVector myVector = new MyVector();
				for (int k = 0; k < 13; k++)
				{
					myVector.addElement(new Card(msg.reader().readByte()));
				}
				int whoMoveFirst = msg.reader().readInt();
				Canvas.endDlg();
				BoardScr.me.resetReady();
				TLBoardScr.gI().start(whoMoveFirst, interval, myVector);
				CasinoService.gI().forceFinish();
				break;
			}
			case 21:
			{
				int whoMove = msg.reader().readInt();
				sbyte b2 = msg.reader().readByte();
				sbyte[] array3 = new sbyte[b2];
				for (int l = 0; l < b2; l++)
				{
					array3[l] = msg.reader().readByte();
				}
				int nextMove2 = msg.reader().readInt();
				BoardScr.disableReady = true;
				TLBoardScr.gI().move(whoMove, array3, nextMove2);
				BoardScr.me.setPosPlaying();
				break;
			}
			case 49:
			{
				int whoSkip = msg.reader().readInt();
				int nextMove = msg.reader().readInt();
				bool isClearCurrentCards = msg.reader().readBoolean();
				TLBoardScr.gI().skip(whoSkip, nextMove, isClearCurrentCards);
				break;
			}
			case 53:
			{
				int num = msg.reader().readInt();
				sbyte[] array = new sbyte[13];
				try
				{
					for (int i = 0; i < 13; i++)
					{
						array[i] = msg.reader().readByte();
					}
				}
				catch (Exception e)
				{
					Out.logError(e);
					array = null;
				}
				Canvas.endDlg();
				TLBoardScr.gI().stopGame();
				if (array != null)
				{
					TLBoardScr.gI().showCards(num, array);
				}
				BoardScr.showChat(num, T.forceFinish);
				break;
			}
			case 54:
			{
				string info = msg.reader().readUTF();
				TLBoardScr.gI().moveError(info);
				break;
			}
			}
		}
		catch (Exception e2)
		{
			Out.logError(e2);
		}
	}
}
