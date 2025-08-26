using System;
using System.IO;

public class CasinoService
{
	protected static CasinoService instance;

	public static CasinoService gI()
	{
		if (instance == null)
		{
			instance = new CasinoService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void requestRoomList()
	{
		Message m = new Message((sbyte)6);
		send(m);
	}

	public void requestBoardList(sbyte id)
	{
		Message message = new Message((sbyte)7);
		try
		{
			message.writer().writeByte(id);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void setMaxPlayer(int max)
	{
		Message message = new Message((sbyte)56);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte((sbyte)max);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void joinBoard(sbyte roomID, sbyte boardID, string pass)
	{
		Message message = new Message((sbyte)8);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeUTF(pass);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void joinAnyBoard()
	{
		Message m = new Message((sbyte)28);
		send(m);
	}

	public void requestStrongest(int page)
	{
		Message message = new Message((sbyte)30);
		try
		{
			message.writer().writeByte(page);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void requestFriendList()
	{
		Message m = new Message((sbyte)(-18));
		send(m);
	}

	public void requestRichest(int page)
	{
		Message message = new Message((sbyte)31);
		try
		{
			message.writer().writeByte(page);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void move(sbyte roomID, sbyte boardID, sbyte[] cards)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeByte((sbyte)cards.Length);
			for (int i = 0; i < cards.Length; i++)
			{
				message.writer().writeByte(cards[i]);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void move(sbyte[] cards)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte((sbyte)cards.Length);
			for (int i = 0; i < cards.Length; i++)
			{
				message.writer().writeByte(cards[i]);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void skip()
	{
		Message message = new Message((sbyte)49);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void forceFinish()
	{
		Message message = new Message((sbyte)53);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void moveCo(sbyte x, sbyte y)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte(x);
			message.writer().writeByte(y);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void moveAndWin(int winx, int winy, int windx, int windy, int x, int y)
	{
		Message message = new Message((sbyte)22);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte((sbyte)x);
			message.writer().writeByte((sbyte)y);
			message.writer().writeByte((sbyte)winx);
			message.writer().writeByte((sbyte)winy);
			message.writer().writeByte((sbyte)windx);
			message.writer().writeByte((sbyte)windy);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void firePhom(sbyte cardID)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte(cardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void GetCardPhom()
	{
		Message message = new Message((sbyte)63);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void eatCardPhom(int[] card, sbyte index)
	{
		Message message = new Message((sbyte)64);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			for (int i = 0; i < card.Length; i++)
			{
				message.writer().writeByte((sbyte)card[i]);
			}
			message.writer().writeByte(index);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void HaPhomPhom(Card[] myCard)
	{
		Message message = new Message((sbyte)65);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			int num = -1;
			for (int i = 0; i < 10; i++)
			{
				if (myCard[i].phom != 0)
				{
					if (myCard[i].phom != num && num != -1)
					{
						message.writer().writeByte(-1);
					}
					num = myCard[i].phom;
					message.writer().writeByte(myCard[i].cardID);
				}
				else if (num != -1)
				{
					message.writer().writeByte(-1);
					num = -1;
				}
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doAddCardPhom(int[] card)
	{
		Message message = new Message((sbyte)67);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			for (int i = 0; i < card.Length; i++)
			{
				if (card[i] != -1)
				{
					message.writer().writeByte((sbyte)card[i]);
				}
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doResetPhomEatPhom(int[] cardID, int cardEat)
	{
		Message message = new Message((sbyte)68);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte((sbyte)cardEat);
			for (int i = 0; i < 5 && cardID[i] != -1; i++)
			{
				message.writer().writeByte((sbyte)cardID[i]);
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void chatToBoard(string text)
	{
		Message message = new Message((sbyte)9);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
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
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void createCell(Point[][] array)
	{
		int num = 0;
		Message message = new Message((sbyte)64);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (array[i][j].isRemove)
					{
						array[i][j].isRemove = false;
						message.writer().writeByte((sbyte)(i * 8 + j));
						num++;
					}
				}
			}
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void doMoveDiamond(int iSelected, int selected)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte((sbyte)iSelected);
			message.writer().writeByte((sbyte)selected);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doSkipDaimond()
	{
		Message message = new Message((sbyte)49);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void doOutPath()
	{
		Message message = new Message((sbyte)24);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public void PutMoneyOk(MyVector info, sbyte room, sbyte board)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			if (info.size() > 0)
			{
				for (int i = 0; i < info.size(); i++)
				{
					PimgBC pimgBC = (PimgBC)info.elementAt(i);
					message.writer().writeByte((sbyte)pimgBC.moneyPut);
					pimgBC.moneyPut = 0;
				}
			}
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e, "PutMoneyOkzz()");
		}
	}

	public void ta(sbyte room, sbyte board, sbyte indexFrom, sbyte indexTo)
	{
		Message message = new Message((sbyte)65);
		try
		{
			message.writer().writeByte(BoardScr.roomID);
			message.writer().writeByte(BoardScr.boardID);
			message.writer().writeByte(room);
			message.writer().writeByte(board);
			message.writer().writeByte(indexFrom);
			message.writer().writeByte(indexTo);
			send(message);
		}
		catch (IOException e)
		{
			Out.logError(e, "ta()");
		}
	}

	public void skip(sbyte roomID, sbyte boardID)
	{
		Message m = new Message((sbyte)49);
		send(m);
	}
}
