using System;

public class TLBoardScr : BoardScr
{
	public static TLBoardScr instance;

	public MyVector currentCards;

	public sbyte[] currentCardsValue;

	public sbyte currentCardsType;

	public MyVector cardShows;

	private sbyte[] selectedCards;

	private sbyte selectedCardsType;

	public MyVector cards;

	private Command cmdSkip;

	private bool isFlying;

	public new static int wCard;

	public new static int hcard;

	private int xShow;

	private int yShow;

	private bool isDown;

	private bool trans;

	private int pa;

	private new bool isTran;

	private int duX;

	private int duY;

	private int indexTran;

	private bool forceMove3Bich;

	public bool isFirstMatch = true;

	public TLBoardScr()
	{
		cmdSkip = new Command(T.skip, 7);
		initYShow();
		if (Canvas.w < 200)
		{
			wCard = 26;
			hcard = 32;
		}
		else
		{
			wCard = 72;
			hcard = 97;
		}
		if (AvMain.hd == 2)
		{
			wCard = 144;
			hcard = 194;
		}
	}

	public static TLBoardScr gI()
	{
		return (instance != null) ? instance : (instance = new TLBoardScr());
	}

	public override void resetCard()
	{
		currentCards = new MyVector();
		currentCardsType = -1;
		currentCardsValue = new sbyte[0];
		selectedCard = -1;
		selectedCards = new sbyte[0];
		selectedCardsType = -1;
		currentPlayer = -1;
		cardShows = new MyVector();
		base.resetCard();
	}

	private static void swap(MyVector actors, int dex1, int dex2)
	{
		object o = actors.elementAt(dex2);
		actors.setElementAt(actors.elementAt(dex1), dex2);
		actors.setElementAt(o, dex1);
	}

	private void sort(MyVector cards)
	{
		int num = cards.size();
		for (int i = 0; i < num - 1; i++)
		{
			for (int j = i + 1; j < num; j++)
			{
				Card card = (Card)cards.elementAt(i);
				Card card2 = (Card)cards.elementAt(j);
				if (card.cardID > card2.cardID)
				{
					swap(cards, i, j);
				}
			}
		}
	}

	public override void commandTab(int index)
	{
		if (index == 7)
		{
			doSkip();
		}
		else
		{
			base.commandTab(index);
		}
	}

	public void initYShow()
	{
		yShow = MyScreen.getHTF();
	}

	public override void init()
	{
		base.init();
		initYShow();
		if (BoardScr.isStartGame)
		{
			setPosCard(false);
		}
		currentCards = null;
	}

	public override void doContinue()
	{
		resetCard();
		base.doContinue();
	}

	protected void doSkip()
	{
		Canvas.msgdlg.setInfoLR(T.doYouWantSkip, new Command(T.yes, 0, this), Canvas.cmdEndDlg);
	}

	public override void commandActionPointer(int index, int subIndex)
	{
		if (index == 0)
		{
			currentPlayer = -1;
			forceMove3Bich = false;
			CasinoService.gI().skip();
			Canvas.endDlg();
		}
		else
		{
			base.commandActionPointer(index, subIndex);
		}
	}

	protected void doSelect()
	{
		((Card)cards.elementAt(selectedCard)).isSelected = !((Card)cards.elementAt(selectedCard)).isSelected;
		selectedCards = getSelectedCardsValue();
		selectedCardsType = CardUtils.getType(selectedCards);
		if (selectedCardsType != -1)
		{
			BoardScr.addInfo(T.cardTypeName[selectedCardsType], 10, -1);
		}
	}

	public void setCurrentCards(sbyte[] cardIDs, int fromSeat)
	{
		int num = 0;
		int y = 0;
		switch (fromSeat)
		{
		case 2:
		{
			num = Canvas.hw;
			y = Canvas.h + Canvas.hTab - 20;
			for (int num2 = cards.size() - 1; num2 >= 0; num2--)
			{
				Card card = (Card)cards.elementAt(num2);
				for (int i = 0; i < cardIDs.Length; i++)
				{
					if (card.cardID == cardIDs[i])
					{
						num = card.x;
						y = card.y;
						break;
					}
				}
			}
			break;
		}
		case 0:
			num = Canvas.hw;
			y = -27;
			break;
		case 3:
			num = Canvas.w + 10;
			y = (Canvas.h + Canvas.hTab) / 2 - 20;
			break;
		case 1:
			num = -10;
			y = (Canvas.h + Canvas.hTab) / 2 - 20;
			break;
		}
		int num3 = Canvas.hw + CRes.r.nextInt(20);
		int num4 = Canvas.hh - 20 * AvMain.hd + CRes.r.nextInt(25);
		if (Canvas.w < 150)
		{
			num4 += 20;
		}
		int num5 = cardIDs.Length;
		int num6 = (Canvas.w - 60) / num5 + 1;
		if (num6 > 12)
		{
			num6 = 12;
		}
		int num7 = (num6 * num5 >> 1) + 6;
		isFlying = true;
		currentCards = new MyVector();
		currentCardsValue = cardIDs;
		for (int j = 0; j < num5; j++)
		{
			Card card2 = new Card(cardIDs[j]);
			card2.x = num + j * disCard;
			card2.y = y;
			card2.xTo = num3 - num7;
			card2.yTo = num4;
			num7 -= num6 * AvMain.hd;
			currentCards.addElement(card2);
		}
		currentCardsType = CardUtils.getType(currentCardsValue);
	}

	public override void doFire()
	{
		base.doFire();
		if (forceMove3Bich)
		{
			bool flag = false;
			for (int i = 0; i < selectedCards.Length; i++)
			{
				if (selectedCards[i] == 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				BoardScr.showChat(GameMidlet.avatar.IDDB, T.youMustFire3Bich);
				return;
			}
			forceMove3Bich = false;
		}
		if (currentCards != null && currentCards.size() != 0 && !CardUtils.available(selectedCards, selectedCardsType, currentCardsValue, currentCardsType))
		{
			BoardScr.showChat(GameMidlet.avatar.IDDB, T.notSameOrSmaller);
			return;
		}
		CasinoService.gI().move(BoardScr.roomID, BoardScr.boardID, selectedCards);
		currentPlayer = -1;
		right = null;
	}

	private void setUpDown(bool iss)
	{
		((Card)cards.elementAt(selectedCard)).isSelected = iss;
		selectedCards = getSelectedCardsValue();
		selectedCardsType = CardUtils.getType(selectedCards);
		setPosCard(false);
	}

	public override void updateKey()
	{
		base.updateKey();
		if (Canvas.isPointerClick)
		{
			indexTran = -1;
			if (BoardScr.isStartGame && cards != null && cards.size() > 0)
			{
				for (int num = cards.size() - 1; num >= 0; num--)
				{
					Card card = (Card)cards.elementAt(num);
					if (card.setCollision())
					{
						isTran = true;
						indexTran = num;
						duX = Canvas.px - card.x;
						duY = Canvas.py - card.y;
						break;
					}
				}
			}
			Canvas.isPointerClick = false;
		}
		if (!isTran)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			if (CRes.abs(Canvas.dx()) > 5 * AvMain.hd && CRes.abs(Canvas.dy()) > 5 * AvMain.hd && indexTran != -1)
			{
				Card card2 = (Card)cards.elementAt(indexTran);
				card2.x = Canvas.px - duX;
				card2.y = Canvas.py - duY;
			}
		}
		else
		{
			if (!Canvas.isPointerRelease)
			{
				return;
			}
			if (CRes.abs(Canvas.dx()) < 10 * AvMain.hd && CRes.abs(Canvas.dy()) < 10 * AvMain.hd)
			{
				if (indexTran != -1)
				{
					selectedCard = indexTran;
					Card card3 = (Card)cards.elementAt(indexTran);
					setUpDown(!((Card)cards.elementAt(selectedCard)).isSelected);
					if (card3.isSelected)
					{
						card3.yTo = yShow - 15 * AvMain.hd;
					}
					else
					{
						card3.yTo = yShow;
					}
				}
			}
			else if (indexTran != -1 && cards != null && cards.size() > 0)
			{
				Card card4 = (Card)cards.elementAt(indexTran);
				for (int num2 = cards.size() - 1; num2 >= 0; num2--)
				{
					Card card5 = (Card)cards.elementAt(num2);
					if (num2 != indexTran && card5.setCollision())
					{
						Card card6 = (Card)cards.elementAt(indexTran);
						card6.x = Canvas.px - duX;
						card6.y = Canvas.py - duY;
						setChangeMyCard(indexTran, num2);
						break;
					}
				}
			}
			indexTran = -1;
			isTran = false;
			Canvas.isPointerRelease = false;
		}
	}

	private void setChangeMyCard(int indexStart, int indexEnd)
	{
		if (indexStart < indexEnd)
		{
			Card card = (Card)cards.elementAt(indexStart);
			int x = card.x;
			int y = card.y;
			Card card2 = (Card)cards.elementAt(indexEnd);
			Card card3 = new Card(-1);
			Card.copyData(card3, card);
			for (int i = indexStart; i < indexEnd; i++)
			{
				Card card4 = (Card)cards.elementAt(i);
				int xTo = card4.xTo;
				int yTo = card4.yTo;
				Card card5 = (Card)cards.elementAt(i + 1);
				int x2 = card5.x;
				int y2 = card5.y;
				Card.copyData(card4, card5);
				card4.xTo = xTo;
				card4.yTo = yTo;
				card4.x = x2;
				card4.y = y2;
			}
			Card.copyData(card2, card3);
			card2.x = x;
			card2.y = y;
		}
		else if (indexStart > indexEnd)
		{
			Card card6 = (Card)cards.elementAt(indexStart);
			int x3 = card6.x;
			int y3 = card6.y;
			Card card7 = (Card)cards.elementAt(indexEnd);
			Card card8 = new Card(-1);
			Card.copyData(card8, card6);
			for (int num = indexStart; num > indexEnd; num--)
			{
				Card card9 = (Card)cards.elementAt(num);
				int xTo2 = card9.xTo;
				int yTo2 = card9.yTo;
				Card card10 = (Card)cards.elementAt(num - 1);
				int x4 = card10.x;
				int y4 = card10.y;
				Card.copyData(card9, card10);
				card9.xTo = xTo2;
				card9.yTo = yTo2;
				card9.x = x4;
				card9.y = y4;
			}
			Card.copyData(card7, card8);
			card7.x = x3;
			card7.y = y3;
		}
	}

	public override void update()
	{
		base.update();
		if (BoardScr.isStartGame && cards != null && cards.size() > 0)
		{
			for (int num = cards.size() - 1; num >= 0; num--)
			{
				Card card = (Card)cards.elementAt(num);
				switch (card.translate())
				{
				case -1:
					card.isShow = false;
					continue;
				default:
					continue;
				case 1:
					break;
				}
				break;
			}
		}
		if (BoardScr.dieTime != 0)
		{
			BoardScr.currentTime = Environment.TickCount;
			if (BoardScr.currentTime > BoardScr.dieTime)
			{
				if (currentPlayer == GameMidlet.avatar.IDDB)
				{
					CasinoService.gI().skip();
					currentPlayer = -1;
				}
				BoardScr.dieTime = 0L;
			}
		}
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			if (BoardScr.isGameEnd)
			{
				left = null;
				center = BoardScr.cmdBack;
				right = null;
			}
			else if (currentPlayer == GameMidlet.avatar.IDDB)
			{
				right = cmdSkip;
				if (getSelectedCardsValue().Length > 0)
				{
					if (selectedCardsType != -1)
					{
						center = BoardScr.cmdFire;
					}
					else
					{
						center = null;
					}
				}
				else
				{
					center = null;
				}
			}
			else
			{
				right = null;
				center = null;
			}
			updateCurCard();
		}
		else
		{
			updateReady();
			right = null;
		}
	}

	private void setPosCard(bool isTran)
	{
		if (cards.size() > 0 && !isTran)
		{
			int num = 12;
			num = (Canvas.w - wCard / 2) / cards.size();
			if (num > wCard / 3 * 2)
			{
				num = wCard / 3 * 2;
			}
			disCard = (Canvas.w - 60) / cards.size() + 1;
			if (disCard > num)
			{
				disCard = num;
			}
			if (disCard < 9)
			{
				disCard = 9;
			}
			disCard = num;
			xShow = (Canvas.w - (disCard * cards.size() + (wCard - disCard)) >> 1) + wCard / 2;
			if (xShow < wCard / 2)
			{
				xShow = wCard / 2;
			}
		}
		int num2 = cards.size();
		int num3 = xShow;
		for (int i = 0; i < num2; i++)
		{
			Card card = (Card)cards.elementAt(i);
			int num4 = 0;
			if (card.isSelected)
			{
				num4 = -15 * AvMain.hd;
			}
			card.setPosTo(num3, yShow + num4);
			num3 += disCard;
			if (isTran)
			{
				card.x = card.xTo;
				card.y = card.yTo;
			}
		}
	}

	public void updateCurCard()
	{
		if (currentCards == null || !isFlying)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < currentCards.size(); i++)
		{
			Card card = (Card)currentCards.elementAt(i);
			if (card.translate() == -1)
			{
				num++;
			}
		}
		if (num == currentCards.size())
		{
			isFlying = false;
		}
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		base.paint(g);
	}

	public override void paintMain(MyGraphics g)
	{
		base.paintMain(g);
		Canvas.resetTrans(g);
		paintCurrentCards(g);
		paintNamePlayers(g);
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			paintMoney(g);
		}
		if (BoardScr.isStartGame)
		{
			paintCard(g);
		}
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			paintShowCards(g);
		}
		paintChat(g);
		Canvas.resetTrans(g);
	}

	private void paintMoney(MyGraphics g)
	{
		for (int i = 0; i < 4; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB == -1)
			{
				continue;
			}
			int num = 0;
			int num2 = 0;
			if (BoardScr.indexPlayer[i] == 2)
			{
				num = -60;
			}
			if (BoardScr.indexPlayer[i] == 1)
			{
				num2 = -10;
			}
			else if (BoardScr.indexPlayer[i] == 3)
			{
				num2 = 10;
			}
			if (Canvas.w > 160)
			{
				Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), BoardScr.posAvatar[BoardScr.indexPlayer[i]].x + num2, BoardScr.posAvatar[BoardScr.indexPlayer[i]].y + 5 + num, BoardScr.posAvatar[BoardScr.indexPlayer[i]].anchor);
			}
			if (avatar.IDDB == currentPlayer && center != BoardScr.cmdBack)
			{
				string text = string.Empty;
				if (BoardScr.dieTime != 0)
				{
					long num3 = (BoardScr.currentTime - BoardScr.dieTime) / 1000;
					text += -num3;
				}
				int x = BoardScr.posAvatar[BoardScr.indexPlayer[i]].x;
				int num4 = BoardScr.posAvatar[BoardScr.indexPlayer[i]].y + AvMain.hBlack;
				if (BoardScr.indexPlayer[i] == 2)
				{
					num4 = yShow - hcard / 2 - (Canvas.blackF.getHeight() + 4);
				}
				PaintPopup.fill(x - 10 * AvMain.hd, num4, 22 * AvMain.hd, Canvas.blackF.getHeight() + 2, 16776365, g);
				g.setColor(332544);
				g.drawRect(x - 10 * AvMain.hd, num4, 22 * AvMain.hd, Canvas.blackF.getHeight() + 2);
				Canvas.blackF.drawString(g, text, x, num4 + 1, 2);
			}
		}
	}

	public void paintCard(MyGraphics g)
	{
		if (!BoardScr.isStartGame || cards == null || cards.size() <= 0)
		{
			return;
		}
		int num = cards.size();
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			Card card = (Card)cards.elementAt(i);
			Card card2 = new Card(-1, false);
			card2.x = card.x;
			card2.y = card.y;
			if (!card.isShow)
			{
				card2 = (Card)cards.elementAt(i);
			}
			if (Canvas.w < 150)
			{
				card2.paintSmall(g, false);
			}
			else if (i == num - 1 || card.isSelected || (card2 != null && card2.isSelected))
			{
				card2.paintFull(g);
			}
			else if (disCard > 14 || card2.x != card2.xTo || card2.y != card2.yTo)
			{
				card2.paintHalfBackFull(g);
			}
			else
			{
				card2.paintHalf(g);
			}
			if (i == selectedCard)
			{
				num3 = card2.y - hcard / 2 - 2 + ((Canvas.gameTick % 10 > 4) ? 2 : 0);
				num2 = card2.x - wCard / 2 + 5 * AvMain.hd;
			}
		}
	}

	private void paintShowCards(MyGraphics g)
	{
		if (cardShows == null || cardShows.size() == 0)
		{
			return;
		}
		int num = cardShows.size();
		int num2 = (Canvas.w - 60) / num + 1;
		if (num2 > 12)
		{
			num2 = 12;
		}
		int num3 = Canvas.hw - (num2 * num >> 1) + 6;
		int y = (Canvas.h + Canvas.hTab) / 2;
		for (int i = 0; i < num; i++)
		{
			Card card = (Card)cardShows.elementAt(i);
			card.x = num3;
			card.y = y;
			num3 += num2 * AvMain.hd;
			if (Canvas.w < 150)
			{
				card.paintSmall(g, false);
			}
			else if (i == num - 1)
			{
				card.paintFull(g);
			}
			else
			{
				card.paintHalf(g);
			}
		}
	}

	private void paintCurrentCards(MyGraphics g)
	{
		if (currentCards != null && currentCards.size() != 0)
		{
			paintCCard(g);
			if (isFlying)
			{
			}
		}
	}

	private void paintCCard(MyGraphics g)
	{
		int num = currentCards.size();
		for (int i = 0; i < num; i++)
		{
			Card card = (Card)currentCards.elementAt(i);
			if (Canvas.w < 150)
			{
				card.paintSmall(g, false);
			}
			else if (i == num - 1)
			{
				card.paintFull(g);
			}
			else
			{
				card.paintHalf(g);
			}
		}
	}

	public void start(int whoMoveFirst, sbyte interval1, MyVector myCards)
	{
		base.start(whoMoveFirst, interval1);
		BoardScr.isStartGame = true;
		forceMove3Bich = false;
		if (isFirstMatch && whoMoveFirst == GameMidlet.avatar.IDDB)
		{
			for (int i = 0; i < myCards.size(); i++)
			{
				Card card = (Card)myCards.elementAt(i);
				if (card.cardID == 0)
				{
					forceMove3Bich = true;
					break;
				}
			}
		}
		cardShows = null;
		currentCards = new MyVector();
		currentCardsType = -1;
		currentCardsValue = new sbyte[0];
		BoardScr.isGameEnd = false;
		cards = myCards;
		sort(myCards);
		for (int j = 0; j < cards.size(); j++)
		{
			Card card2 = (Card)cards.elementAt(j);
			card2.x = Canvas.hw;
			card2.y = (Canvas.h + Canvas.hTab) / 2;
			card2.isShow = true;
		}
		int num = 0;
		for (int k = 0; k < BoardScr.numPlayer; k++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(k);
			if (avatar.IDDB != -1)
			{
				num++;
			}
		}
		BoardScr.interval = interval1;
		BoardScr.dieTime = Environment.TickCount + interval1 * 1000;
		if (whoMoveFirst == GameMidlet.avatar.IDDB)
		{
			right = cmdSkip;
		}
		Avatar avatarByID = BoardScr.getAvatarByID(whoMoveFirst);
		BoardScr.addInfo(avatarByID.name + T.firstFire, 20, avatarByID.IDDB);
		currentCardsType = -1;
		currentCardsValue = new sbyte[0];
		selectedCard = 2;
		currentPlayer = whoMoveFirst;
		setPosPlaying();
		setPosCard(false);
	}

	public void move(int whoMove, sbyte[] movedCards, int nextMove)
	{
		forceMove3Bich = false;
		if (whoMove != -1)
		{
			int indexByID = BoardScr.getIndexByID(whoMove);
			setCurrentCards(movedCards, BoardScr.indexPlayer[indexByID]);
		}
		if (whoMove == GameMidlet.avatar.IDDB)
		{
			removeCards(movedCards);
			selectedCard = 0;
			setPosCard(false);
		}
		currentPlayer = nextMove;
		if (currentPlayer == GameMidlet.avatar.IDDB)
		{
			if (getSelectedCardsValue().Length == 0)
			{
				right = cmdSkip;
			}
			else
			{
				right = BoardScr.cmdFire;
			}
		}
		else
		{
			right = null;
		}
		if (BoardScr.interval == 0)
		{
			BoardScr.interval = 30;
		}
		BoardScr.dieTime = Environment.TickCount + BoardScr.interval * 1000;
	}

	public void skip(int whoSkip, int nextMove, bool isClearCurrentCards)
	{
		if (isClearCurrentCards)
		{
			repaint();
		}
		Avatar avatarByID = BoardScr.getAvatarByID(whoSkip);
		string empty = string.Empty;
		empty = ((!avatarByID.name.Equals(string.Empty)) ? T.skip : T.exitBoard);
		BoardScr.addInfo(empty, 60, avatarByID.IDDB);
		currentPlayer = nextMove;
		if (isClearCurrentCards)
		{
			currentCards = new MyVector();
			currentCardsType = -1;
			currentCardsValue = new sbyte[0];
		}
		if (currentPlayer == GameMidlet.avatar.IDDB)
		{
			if (getSelectedCardsValue().Length == 0)
			{
				right = cmdSkip;
			}
			else
			{
				right = BoardScr.cmdFire;
			}
		}
		else
		{
			right = null;
		}
		BoardScr.dieTime = Environment.TickCount + BoardScr.interval * 1000;
	}

	public void showCards(int whoShow, sbyte[] card)
	{
		Avatar avatarByID = BoardScr.getAvatarByID(whoShow);
		CardUtils.sort(card);
		cardShows = new MyVector();
		for (int i = 0; i < card.Length; i++)
		{
			cardShows.addElement(new Card(card[i]));
		}
		if (avatarByID != null && avatarByID.IDDB == whoShow && cards != null)
		{
			cards.removeAllElements();
		}
	}

	public void finish(int whoFinish, sbyte finishPosition, int dMoney, int dExp)
	{
		Avatar avatarByID = BoardScr.getAvatarByID(whoFinish);
		if (avatarByID != null)
		{
			avatarByID.isReady = false;
			int num = avatarByID.exp + dExp;
			if (num < 0)
			{
				num = 0;
			}
			avatarByID.setExp(num);
			avatarByID.setMoneyNew(avatarByID.getMoneyNew() + dMoney);
			if (avatarByID.IDDB == GameMidlet.avatar.IDDB)
			{
				GameMidlet.avatar.setMoneyNew(avatarByID.getMoneyNew());
			}
		}
		BoardScr.showChat(whoFinish, T.goad + (finishPosition + 1));
	}

	public void stopGame()
	{
		BoardScr.isGameEnd = true;
	}

	public void moveError(string info)
	{
		BoardScr.addInfo(info, 100, GameMidlet.avatar.IDDB);
		currentPlayer = GameMidlet.avatar.IDDB;
	}

	public void setMode(bool hasStartGame)
	{
		repaint();
		BoardScr.isStartGame = hasStartGame;
	}

	public void removeCards(sbyte[] removeCards)
	{
		int num = cards.size();
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			Card card = (Card)cards.elementAt(num2);
			for (int i = 0; i < removeCards.Length; i++)
			{
				if (card.cardID == removeCards[i])
				{
					cards.removeElementAt(num2);
				}
			}
		}
	}

	public sbyte[] getSelectedCardsValue()
	{
		MyVector myVector = new MyVector();
		int num = cards.size();
		for (int i = 0; i < num; i++)
		{
			Card card = (Card)cards.elementAt(i);
			if (card.isSelected)
			{
				myVector.addElement(card);
			}
		}
		int num2 = myVector.size();
		sbyte[] array = new sbyte[num2];
		for (int j = 0; j < num2; j++)
		{
			array[j] = ((Card)myVector.elementAt(j)).cardID;
		}
		CardUtils.sort(array);
		return array;
	}
}
