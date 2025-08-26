using System;

public class PhomService
{
	protected static PhomService instance;

	public static PhomService gI()
	{
		if (instance == null)
		{
			instance = new PhomService();
		}
		return instance;
	}

	public void send(Message m)
	{
		Session_ME.gI().sendMessage(m);
		m.cleanup();
	}

	public void fire(sbyte roomID, sbyte boardID, sbyte cardID)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
			message.writer().writeByte(cardID);
			send(message);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public void GetCard(sbyte roomID, sbyte boardID)
	{
		Message message = new Message((sbyte)63);
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

	public void eatCard(sbyte roomID, sbyte boardID, int[] card, sbyte index)
	{
		Message message = new Message((sbyte)64);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
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

	public void HaPhom(int roomID, int boardID, Card[] myCard)
	{
		Message message = new Message((sbyte)65);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
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

	public void doAddCard(sbyte roomID, sbyte boardID, int[] card)
	{
		Message message = new Message((sbyte)67);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
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

	public void doResetPhomEat(sbyte roomID, sbyte boardID, int[] cardID, int cardEat)
	{
		Message message = new Message((sbyte)68);
		try
		{
			message.writer().writeByte(roomID);
			message.writer().writeByte(boardID);
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
}
