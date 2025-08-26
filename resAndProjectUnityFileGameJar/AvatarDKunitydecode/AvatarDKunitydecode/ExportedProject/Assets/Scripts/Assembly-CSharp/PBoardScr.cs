using System;

public class PBoardScr : BoardScr
{
	public static PBoardScr instance;

	public static int distant = 16;

	private int[] numC = new int[4];

	private Card[][] cardShow;

	public static AvPosition posCard;

	public static AvPosition[] posName;

	public static AvPosition[] posNamePlaying;

	public static AvPosition[] posFinish;

	public static AvPosition[] posCardShow;

	public static AvPosition[] posCardEat;

	private AvPosition arror;

	private Card card;

	private Card[] myCard = new Card[10];

	private Card[] cardEat = new Card[3];

	private int[][] ShowHaPhom;

	private Card[][] showCardEat;

	private int[] numCardEat = new int[4];

	private int[] numCardPhom = new int[4];

	private int[][] cardRac;

	private sbyte[] numCardRac = new sbyte[4];

	private sbyte[] numberCard = new sbyte[4];

	private int numPhom;

	private int phomRandom;

	private int phomHa;

	private int firstPlayer;

	private int cardCurrent;

	private int firstHa;

	private Card hCard;

	private int assetChange;

	private bool finish;

	private int winer;

	private int denBai;

	private bool isU;

	private bool isHaPhom;

	private int[] scorePlayer = new int[4];

	private int key;

	private bool pause;

	private bool isEatCard;

	private AvPosition getC;

	private Card cardE;

	private int colorPhom_1 = 473848;

	private int colorPhom_2 = 517368;

	private int colorPhom_3 = 522270;

	public new static int disCard = 12;

	public static int disShow = 12;

	public int distantCard;

	private Command cmdEat;

	private Command cmdGet;

	private Command cmdHaPhom;

	private int xShow;

	private int yShow;

	private int remem;

	private bool trans;

	private new bool isTran;

	private int duX;

	private int duY;

	private int indexTran;

	private int pos = -2;

	private int count;

	private Card ca;

	private int[] cardToEat = new int[5];

	public PBoardScr()
	{
		cardShow = new Card[4][];
		for (int i = 0; i < cardShow.Length; i++)
		{
			cardShow[i] = new Card[4];
		}
		ShowHaPhom = new int[4][];
		for (int j = 0; j < ShowHaPhom.Length; j++)
		{
			ShowHaPhom[j] = new int[12];
		}
		showCardEat = new Card[4][];
		for (int k = 0; k < showCardEat.Length; k++)
		{
			showCardEat[k] = new Card[3];
		}
		cardRac = new int[4][];
		for (int l = 0; l < cardRac.Length; l++)
		{
			cardRac[l] = new int[11];
		}
		reset();
		cmdEat = new Command(T.eat, 7);
		cmdGet = new Command(T.gett, 8);
		cmdHaPhom = new Command(T.haPhom, 9);
		Canvas.paint.initPosPhom();
		distantCard = 35 * AvMain.hd;
	}

	public static PBoardScr gI()
	{
		if (instance == null)
		{
			instance = new PBoardScr();
		}
		return instance;
	}

	public override void commandTab(int index)
	{
		switch (index)
		{
		case 7:
			doEat();
			break;
		case 8:
			doGet();
			break;
		case 9:
			doHaPhom();
			break;
		default:
			base.commandTab(index);
			break;
		}
	}

	public override void resetCard()
	{
		reset();
		base.resetCard();
	}

	public void reset()
	{
		for (int i = 0; i < 10; i++)
		{
			if (i < 3)
			{
				cardEat[i] = new Card(-1, true);
			}
		}
		cardShow = new Card[4][];
		for (int j = 0; j < cardShow.Length; j++)
		{
			cardShow[j] = new Card[4];
		}
		selectedCard = 0;
		cardCurrent = -1;
		for (int k = 0; k < 4; k++)
		{
			for (int l = 0; l < 4; l++)
			{
				if (l < 3)
				{
					showCardEat[k][l] = null;
				}
			}
			for (int m = 0; m < 12; m++)
			{
				ShowHaPhom[k][m] = -1;
				if (m < 11)
				{
					cardRac[k][m] = -1;
				}
			}
			scorePlayer[k] = -1;
			numC[k] = 0;
			numCardEat[k] = 0;
			numCardPhom[k] = 0;
			numCardRac[k] = 0;
			numberCard[k] = 0;
		}
		numPhom = 0;
		phomRandom = 3;
		phomHa = 0;
		hCard = new Card(-1, true);
		assetChange = -1;
		key = 1;
		finish = false;
		winer = -1;
		isU = false;
		isHaPhom = false;
		pos = -2;
		pause = false;
		denBai = -1;
		firstHa = -1;
		isEatCard = false;
		getC = new AvPosition(Canvas.hw, Canvas.hh, 3);
		cardE = new Card(-1, true);
	}

	public override void init()
	{
		base.init();
		initP();
		if (BoardScr.isStartGame)
		{
			setPosCard(false);
		}
		getC = new AvPosition(Canvas.hw, Canvas.hh, 3);
	}

	public void initP()
	{
		int h = Canvas.h;
		posCard = new AvPosition();
		arror = new AvPosition();
		posCard.x = Canvas.hw - 27;
		posCard.y = MyScreen.getHTF() - AvMain.hFillTab;
		arror.x = posCard.x - 24;
		arror.y = posCard.y - BoardScr.hcard / 2 - 4;
		Canvas.paint.initPosPhom();
	}

	public void setCmdEatAndGet()
	{
		center = cmdEat;
		right = cmdGet;
	}

	public void setCmdFire()
	{
		center = BoardScr.cmdFire;
		right = null;
	}

	public void resetCmd()
	{
		center = null;
		right = null;
	}

	public void setcmdHaPhom()
	{
		center = cmdHaPhom;
		right = null;
	}

	public void setContinue()
	{
		center = BoardScr.cmdBack;
		right = null;
	}

	public override void doContinue()
	{
		finish = false;
		resetCard();
		base.doContinue();
	}

	private void resetChangeCard()
	{
		if (assetChange != -1 && hCard.cardID != -1)
		{
			cleanCard(selectedCard);
			cleanUp(assetChange);
			myCard[assetChange] = hCard;
			hCard = new Card(-1, true);
		}
	}

	private void resetCell(int id)
	{
		myCard[id] = new Card(-1, true);
	}

	private void resetCurrentTime()
	{
		BoardScr.currentTime = Environment.TickCount / 1000;
	}

	public void resetOrderCard()
	{
		if (trans && remem == 2)
		{
			remem = 0;
			trans = false;
			if (hCard.cardID != -1)
			{
				checkCardChange();
			}
			resetUpMyCard(selectedCard);
			Canvas.isPointerDown = false;
		}
	}

	public override void updateKey()
	{
		base.updateKey();
		if (Canvas.isPointerClick)
		{
			indexTran = -1;
			if (BoardScr.isStartGame && myCard != null)
			{
				for (int num = myCard.Length - 1; num >= 0; num--)
				{
					if (myCard[num] != null && myCard[num].cardID != -1 && myCard[num].setCollision())
					{
						isTran = true;
						indexTran = num;
						duX = Canvas.px - myCard[num].x;
						duY = Canvas.py - myCard[num].y;
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
		if (Canvas.isPointerDown && CRes.abs(Canvas.dx()) > 5 * AvMain.hd && CRes.abs(Canvas.dy()) > 5 * AvMain.hd && indexTran != -1)
		{
			myCard[indexTran].x = Canvas.px - duX;
			myCard[indexTran].y = Canvas.py - duY;
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		if (CRes.abs(Canvas.dx()) < 10 * AvMain.hd && CRes.abs(Canvas.dy()) < 10 * AvMain.hd)
		{
			if (indexTran != -1)
			{
				if (!myCard[indexTran].isUp)
				{
					myCard[indexTran].isUp = true;
					myCard[indexTran].yTo = posCard.y - 15 * AvMain.hd;
				}
				else
				{
					myCard[indexTran].isUp = false;
					myCard[indexTran].yTo = posCard.y;
				}
			}
		}
		else if (indexTran != -1 && myCard != null)
		{
			for (int num2 = myCard.Length - 1; num2 >= 0; num2--)
			{
				if (num2 != indexTran && myCard[num2].cardID != -1 && myCard[num2].setCollision())
				{
					setChangeMyCard(indexTran, num2);
					cleanPhomRandom();
					findPhom();
					break;
				}
			}
		}
		indexTran = -1;
		isTran = false;
		Canvas.isPointerRelease = false;
	}

	private void setChangeMyCard(int indexStart, int indexEnd)
	{
		if (indexStart < indexEnd)
		{
			Card card = myCard[indexStart];
			int x = myCard[indexEnd].x;
			int y = myCard[indexEnd].y;
			int x2 = card.x;
			int y2 = card.y;
			myCard[indexStart].x = myCard[indexStart].xTo;
			myCard[indexStart].y = myCard[indexStart].yTo;
			for (int i = indexStart; i < indexEnd; i++)
			{
				int x3 = myCard[i].x;
				int y3 = myCard[i].y;
				myCard[i] = myCard[i + 1];
				myCard[i].xTo = x3;
				myCard[i].yTo = y3;
			}
			myCard[indexEnd] = card;
			myCard[indexEnd].x = x2;
			myCard[indexEnd].y = y2;
			myCard[indexEnd].yTo = y;
			myCard[indexEnd].xTo = x;
		}
		else if (indexStart > indexEnd)
		{
			Card card2 = myCard[indexStart];
			int x4 = myCard[indexEnd].x;
			int y4 = myCard[indexEnd].y;
			int x5 = card2.x;
			int y5 = card2.y;
			myCard[indexStart].x = myCard[indexStart].xTo;
			myCard[indexStart].y = myCard[indexStart].yTo;
			for (int num = indexStart; num > indexEnd; num--)
			{
				int x6 = myCard[num].x;
				int y6 = myCard[num].y;
				myCard[num] = myCard[num - 1];
				myCard[num].xTo = x6;
				myCard[num].yTo = y6;
			}
			myCard[indexEnd] = card2;
			myCard[indexEnd].x = x5;
			myCard[indexEnd].y = y5;
			myCard[indexEnd].yTo = y4;
			myCard[indexEnd].xTo = x4;
		}
	}

	public void resetUpMyCard(int index)
	{
		if (myCard[index].cardID != -1 && !myCard[index].isUp)
		{
			myCard[index].yTo = posCard.y;
		}
		myCard[index].isUp = false;
	}

	public override void update()
	{
		base.update();
		if (BoardScr.isStartGame || BoardScr.disableReady)
		{
			if (BoardScr.isStartGame && !isTran && myCard != null)
			{
				for (int num = myCard.Length - 1; num >= 0; num--)
				{
					if (myCard[num] != null)
					{
						int num2 = myCard[num].translate();
						if (num2 == -1)
						{
							myCard[num].isShow = false;
						}
					}
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (cardShow[i] == null)
				{
					continue;
				}
				for (int j = 0; j < cardShow[i].Length; j++)
				{
					if (cardShow[i][j] != null)
					{
						cardShow[i][j].translate();
					}
				}
			}
			for (int k = 0; k < showCardEat.Length; k++)
			{
				for (int l = 0; l < showCardEat[k].Length; l++)
				{
					if (showCardEat[k][l] != null)
					{
						showCardEat[k][l].translate();
					}
				}
			}
			checkTimeLimit();
		}
		else
		{
			updateReady();
		}
	}

	private void setPosCard(bool isT)
	{
		if (BoardScr.disableReady)
		{
			return;
		}
		int num = getAssetCard();
		if (num == -1)
		{
			num = 10;
		}
		if ((BoardScr.isStartGame || BoardScr.disableReady) && num != 0)
		{
			disCard = (Canvas.w - BoardScr.wCard / 2) / num;
			if (disCard > BoardScr.wCard / 3 * 2)
			{
				disCard = BoardScr.wCard / 3 * 2;
			}
		}
		disShow = disCard;
		if (disShow > BoardScr.wCard / 4)
		{
			disShow = BoardScr.wCard / 4;
		}
		if (!isT)
		{
			xShow = (Canvas.w - (disCard * num + (BoardScr.wCard - disCard)) >> 1) + BoardScr.wCard / 2;
			if (xShow < BoardScr.wCard / 2)
			{
				xShow = BoardScr.wCard / 2;
			}
		}
		for (int i = 0; i < 10; i++)
		{
			int num2 = 0;
			if (myCard[i].isUp)
			{
				num2 = 15 * AvMain.hd;
			}
			myCard[i].setPosTo(xShow + i * disCard, posCard.y - num2);
			yShow = myCard[i].yTo - BoardScr.hcard / 2 - 20;
			if (isT)
			{
				myCard[i].x = myCard[i].xTo;
				myCard[i].y = myCard[i].yTo;
			}
		}
	}

	public void checkTimeLimit()
	{
		BoardScr.dieTime = (int)(Environment.TickCount / 1000 - BoardScr.currentTime);
		if (BoardScr.interval - BoardScr.dieTime >= 0)
		{
			return;
		}
		if (center == cmdEat && right == cmdGet)
		{
			doGet();
			resetCmd();
		}
		else if (center == BoardScr.cmdFire)
		{
			int num = 0;
			for (int i = 1; i < 10; i++)
			{
				if (myCard[i].phom == 0 && myCard[i].cardID > myCard[num].cardID)
				{
					num = i;
				}
			}
			resetChangeCard();
			CasinoService.gI().firePhom(myCard[num].cardID);
		}
		else if (center == cmdHaPhom)
		{
			resetChangeCard();
			cmdHaPhom.action.perform();
		}
	}

	private void checkCardChange()
	{
		int num = -1;
		for (int i = 0; i < 3; i++)
		{
			if (hCard.phom == cardEat[i].phom)
			{
				num = cardEat[i].cardID;
			}
		}
		myCard[selectedCard] = hCard;
		if (num != -1)
		{
			if (!checkCardToEat(selectedCard))
			{
				hCard = myCard[selectedCard];
				resetCell(selectedCard);
				resetChangeCard();
				return;
			}
			resetCell(selectedCard);
		}
		if (hCard.phom != 0 && ((selectedCard > 0 && myCard[selectedCard - 1].phom == hCard.phom) || (selectedCard < 9 && myCard[selectedCard + 1].phom == hCard.phom)))
		{
			myCard[selectedCard] = hCard;
			hCard = new Card(-1, true);
		}
		else if (num == -1 || hCard.phom == 0)
		{
			if (selectedCard < 9)
			{
				for (int j = 0; j < 3; j++)
				{
					if (cardEat[j].phom == 0 || myCard[selectedCard + 1].phom != cardEat[j].phom)
					{
						continue;
					}
					int[] array = new int[10];
					int num2 = 0;
					for (int k = 0; k < 10; k++)
					{
						if (myCard[k].phom == cardEat[j].phom)
						{
							array[k] = myCard[k].cardID;
							continue;
						}
						array[k] = -1;
						if (num2 == 0)
						{
							num2 = 1;
							array[k] = hCard.cardID;
						}
					}
					array = orderArrayIncrease(array);
					if (checkPhomBaLa(array) || checkPhomSanh(array))
					{
						pos = hCard.phom;
						hCard.phom = cardEat[j].phom;
						myCard[selectedCard] = hCard;
						if (assetChange != selectedCard)
						{
							doResetPhomEat(cardEat[j], cardEat[j].cardID);
						}
					}
					else
					{
						resetChangeCard();
					}
					return;
				}
			}
			int num3 = setCardEaTrue();
			if (selectedCard >= num3 && num3 != -1)
			{
				resetChangeCard();
				return;
			}
			myCard[selectedCard] = hCard;
			hCard = new Card(-1, true);
			if (assetChange != selectedCard)
			{
				myCard[selectedCard].phom = 0;
				cleanPhomRandom();
				findPhom();
			}
		}
		else if (num != -1)
		{
			pos = -1;
			if (assetChange != selectedCard)
			{
				doResetPhomEat(hCard, num);
			}
		}
	}

	private int setCardEaTrue()
	{
		for (int i = 0; i < 10; i++)
		{
			if (myCard[i].cardID == -1)
			{
				return i;
			}
			if (searchCardEat(myCard[i].phom))
			{
				return i;
			}
		}
		return -1;
	}

	private void doResetPhomEat(Card cardID, int cardeat)
	{
		resetOrderCard();
		pause = true;
		int[] array = new int[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = -1;
		}
		int[] array2 = new int[6];
		for (int j = 0; j < 6; j++)
		{
			array2[j] = -1;
		}
		int num = 0;
		for (int k = 0; k < 10; k++)
		{
			if (myCard[k].phom == cardID.phom)
			{
				array2[num] = myCard[k].cardID;
				num++;
			}
		}
		if (array2[5] != -1)
		{
			orderArrayIncrease(array2);
			int num2 = 0;
			for (int l = 0; l < array2.Length; l++)
			{
				if (array2[l] == cardeat)
				{
					num2 = l;
				}
			}
			int num3 = 0;
			if (num2 < 3)
			{
				for (int m = 0; m < array2.Length; m++)
				{
					if (m > 2)
					{
						for (int n = 0; n < 10; n++)
						{
							if (array2[m] == myCard[n].cardID)
							{
								myCard[n].phom = 0;
							}
						}
						continue;
					}
					array[num3] = array2[m];
					num3++;
					for (int num4 = 0; num4 < 10; num4++)
					{
						if (array2[m] == myCard[num4].cardID)
						{
							Card card = myCard[num4];
							cleanCard(num4);
							myCard[getAssetCard()] = card;
						}
					}
				}
			}
			else
			{
				for (int num5 = 0; num5 < array2.Length; num5++)
				{
					if (num5 < 3)
					{
						for (int num6 = 0; num6 < 10; num6++)
						{
							if (array2[num5] == myCard[num6].cardID)
							{
								myCard[num6].phom = 0;
							}
						}
						continue;
					}
					array[num3] = array2[num5];
					num3++;
					for (int num7 = 0; num7 < 10; num7++)
					{
						if (array2[num5] == myCard[num7].cardID)
						{
							Card card2 = myCard[num7];
							cleanCard(num7);
							myCard[getAssetCard()] = card2;
						}
					}
				}
			}
		}
		else
		{
			int num8 = 0;
			for (int num9 = 0; num9 < 10; num9++)
			{
				if (myCard[num9].phom == cardID.phom)
				{
					array[num8] = myCard[num9].cardID;
					num8++;
				}
			}
		}
		orderArrayIncrease(array);
		CasinoService.gI().doResetPhomEatPhom(array, cardeat);
	}

	public void onResetPhomEat(sbyte isReset)
	{
		resetOrderCard();
		pause = false;
		if (isReset == 0)
		{
			if (pos == -1)
			{
				myCard[selectedCard] = hCard;
				myCard[selectedCard].phom = 0;
				hCard = new Card(-1, true);
				if (assetChange != selectedCard)
				{
					cleanPhomRandom();
					findPhom();
				}
				assetChange = -1;
			}
			else if (pos >= 0)
			{
				hCard = new Card(-1, true);
				if (assetChange != selectedCard)
				{
					myCard[selectedCard].phom = 0;
					cleanPhomRandom();
					findPhom();
				}
			}
		}
		else if (pos == -1)
		{
			resetCell(selectedCard);
			resetChangeCard();
		}
		else if (pos >= 0)
		{
			resetCell(selectedCard);
			resetChangeCard();
			myCard[assetChange].phom = (sbyte)pos;
		}
		pos = -2;
		setPosCard(false);
	}

	private void cleanUp(int id)
	{
		for (int num = 9; num > id; num--)
		{
			myCard[num] = myCard[num - 1];
		}
		resetCell(id);
	}

	public void cleanCard(int card)
	{
		for (int i = card; i < 9; i++)
		{
			myCard[i] = myCard[i + 1];
		}
		resetCell(9);
		resetUpMyCard(card);
	}

	public override void paint(MyGraphics g)
	{
		paintMain(g);
		base.paint(g);
	}

	public override void paintMain(MyGraphics g)
	{
		base.paintMain(g);
		paintNamePlayers(g);
		paintTime(g);
		if (BoardScr.isStartGame)
		{
			paintMoneys(g);
			paintMyCard(g);
			paintEatCard(g);
			if (!finish)
			{
				int indexByID = BoardScr.getIndexByID(firstHa);
				switch (BoardScr.indexPlayer[indexByID])
				{
				case 0:
					g.drawImage(BoardScr.imgBan, posNamePlaying[0].x, posNamePlaying[0].y + 13 * AvMain.hd + AvMain.hSmall, 3);
					break;
				case 1:
					g.drawImage(BoardScr.imgBan, posNamePlaying[1].x, posNamePlaying[1].y + AvMain.hSmall, 0);
					break;
				case 2:
					g.drawImage(BoardScr.imgBan, posNamePlaying[2].x, posNamePlaying[2].y - 13 * AvMain.hd, 3);
					break;
				case 3:
					g.drawImage(BoardScr.imgBan, posNamePlaying[3].x - 13 * AvMain.hd, posNamePlaying[3].y + 13 * AvMain.hd + AvMain.hSmall, 3);
					break;
				}
			}
		}
		paintChat(g);
	}

	public void paintMoneys(MyGraphics g)
	{
		for (int i = 0; i < 4; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar.IDDB != -1 && (Canvas.w >= 160 || avatar.IDDB == currentPlayer))
			{
				int indexByID = BoardScr.getIndexByID(currentPlayer);
				if (i != indexByID || (Canvas.gameTick % 20 > 5 && i == indexByID) || finish)
				{
					Canvas.smallFontYellow.drawString(g, avatar.getMoneyNew() + " " + T.getMoney(), posNamePlaying[BoardScr.indexPlayer[i]].x, posNamePlaying[BoardScr.indexPlayer[i]].y, posNamePlaying[BoardScr.indexPlayer[i]].anchor);
				}
			}
		}
	}

	private void paintTime(MyGraphics g)
	{
		if (BoardScr.isStartGame && !BoardScr.isGameEnd)
		{
			long num = BoardScr.interval - BoardScr.dieTime;
			if (num > 0 && posNamePlaying != null && posNamePlaying[0] != null)
			{
				int indexByID = BoardScr.getIndexByID(currentPlayer);
				Canvas.numberFont.drawString(g, num + string.Empty, Canvas.hw, posCardShow[0].y + (posCardShow[2].y - posCardShow[0].y) / 2 - Canvas.numberFont.getHeight() / 2, 2);
			}
		}
	}

	private void paintEatCard(MyGraphics g)
	{
		if (isEatCard)
		{
			if (Canvas.w > 176)
			{
				cardE.paintFull(g);
			}
			else
			{
				cardE.paintSmall(g, false);
			}
		}
	}

	private void paintMyCard(MyGraphics g)
	{
		this.card = new Card(-1, true);
		int num = disShow;
		if (num <= 12 && Canvas.w > 200)
		{
			num = 20;
		}
		for (int i = 0; i < 4; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar == null || avatar.IDDB == -1)
			{
				continue;
			}
			int num2 = 0;
			for (int j = 0; j < 3 && showCardEat[i][j] != null; j++)
			{
				num2++;
			}
			if (BoardScr.indexPlayer[i] == 1)
			{
				for (int k = 0; k < 3 && showCardEat[i][k] != null; k++)
				{
					Card card = showCardEat[i][k];
					card.phom = 1;
					card.paintFull(g);
					PaintLineColor(1, card.x, card.y, BoardScr.indexPlayer[i], g);
				}
			}
		}
		for (int l = 0; l < 4; l++)
		{
			if (BoardScr.indexPlayer[l] != 1)
			{
				continue;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = distantCard / 2;
			for (int m = 0; m < 12; m++)
			{
				if (ShowHaPhom[l][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[l][m] == -1)
				{
					if (num4 == 1)
					{
						num3--;
						break;
					}
					num4 = 1;
				}
				else
				{
					num4 = 0;
				}
				num3++;
			}
			if (num3 > 0)
			{
				num5 = Canvas.hh / num3;
				if (num5 > distantCard)
				{
					num5 = distantCard;
				}
				else if (num5 < distantCard / 2)
				{
					num5 = distantCard / 2;
				}
			}
			for (int n = 0; n < 12; n++)
			{
				if (ShowHaPhom[l][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[l][n] != -1)
				{
					this.card = new Card((sbyte)ShowHaPhom[l][n], true);
					this.card.x = posCardEat[BoardScr.indexPlayer[l]].x;
					this.card.y = posCardEat[BoardScr.indexPlayer[l]].y - (num3 - 1) * num5 / 2 + n * num5;
					this.card.paintFull(g);
				}
			}
		}
		paintcardShow(g);
		for (int num6 = 0; num6 < 4; num6++)
		{
			Avatar avatar2 = (Avatar)BoardScr.avatarInfos.elementAt(num6);
			if (avatar2 == null || avatar2.IDDB == -1)
			{
				continue;
			}
			int num7 = 0;
			for (int num8 = 0; num8 < 11 && cardRac[num6][num8] != -1; num8++)
			{
				num7++;
			}
			if (BoardScr.indexPlayer[num6] == 1 || BoardScr.indexPlayer[num6] == 3)
			{
				for (int num9 = 0; num9 < 11 && cardRac[num6][num9] != -1; num9++)
				{
					this.card = new Card((sbyte)cardRac[num6][num9], true);
					this.card.x = posCardShow[BoardScr.indexPlayer[num6]].x;
					this.card.y = posCardShow[BoardScr.indexPlayer[num6]].y - (num7 - 1) * num / 2 + num9 * num;
					this.card.paintFull(g);
				}
			}
			if (BoardScr.indexPlayer[num6] == 0)
			{
				for (int num10 = 0; num10 < 11 && cardRac[num6][num10] != -1; num10++)
				{
					this.card = new Card((sbyte)cardRac[num6][num10], true);
					this.card.x = posCardShow[BoardScr.indexPlayer[num6]].x + (num7 - 1) * disShow / 2 - num10 * disShow;
					this.card.y = posCardShow[BoardScr.indexPlayer[num6]].y;
					this.card.paintFull(g);
				}
			}
		}
		for (int num11 = 0; num11 < 4; num11++)
		{
			Avatar avatar3 = (Avatar)BoardScr.avatarInfos.elementAt(num11);
			if (avatar3 == null || avatar3.IDDB == -1)
			{
				continue;
			}
			int num12 = 0;
			for (int num13 = 0; num13 < 3 && showCardEat[num11][num13] != null; num13++)
			{
				num12++;
			}
			if (BoardScr.indexPlayer[num11] == 0)
			{
				for (int num14 = 0; num14 < 3 && showCardEat[num11][num14] != null; num14++)
				{
					Card card2 = showCardEat[num11][num14];
					card2.phom = 1;
					card2.paintFull(g);
					PaintLineColor(1, card2.x, card2.y, 0, g);
				}
			}
		}
		for (int num15 = 0; num15 < 4; num15++)
		{
			int num16 = 0;
			int num17 = 0;
			int num18 = distantCard / 2;
			for (int num19 = 0; num19 < 12; num19++)
			{
				if (ShowHaPhom[num15][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[num15][num19] == -1)
				{
					if (num17 == 1)
					{
						num16--;
						break;
					}
					num17 = 1;
				}
				else
				{
					num17 = 0;
				}
				num16++;
			}
			if (num16 > 0)
			{
				num18 = Canvas.hw / num16;
				if (num18 > distantCard)
				{
					num18 = distantCard;
				}
				else if (num18 < distantCard / 2)
				{
					num18 = distantCard / 2;
				}
			}
			if (BoardScr.indexPlayer[num15] != 0)
			{
				continue;
			}
			if (ShowHaPhom[num15][0] == -1)
			{
				break;
			}
			for (int num20 = 0; num20 < 12; num20++)
			{
				if (ShowHaPhom[num15][num20] != -1)
				{
					this.card = new Card((sbyte)ShowHaPhom[num15][num20], true);
					this.card.x = posCardEat[BoardScr.indexPlayer[num15]].x + (num16 - 1) * num18 / 2 - num20 * num18;
					this.card.y = posCardEat[BoardScr.indexPlayer[num15]].y;
					this.card.paintFull(g);
				}
			}
		}
		for (int num21 = 0; num21 < 4; num21++)
		{
			Avatar avatar4 = (Avatar)BoardScr.avatarInfos.elementAt(num21);
			if (avatar4 != null && avatar4.IDDB != -1 && BoardScr.indexPlayer[num21] == 3)
			{
				int num22 = 0;
				for (int num23 = 0; num23 < 3 && showCardEat[num21][num23] != null; num23++)
				{
					num22++;
				}
				for (int num24 = 0; num24 < 3 && showCardEat[num21][num24] != null; num24++)
				{
					Card card3 = showCardEat[num21][num24];
					card3.phom = 1;
					card3.paintFull(g);
					PaintLineColor(1, card3.x, card3.y, BoardScr.indexPlayer[num21], g);
				}
			}
		}
		for (int num25 = 0; num25 < 4; num25++)
		{
			Avatar avatar5 = (Avatar)BoardScr.avatarInfos.elementAt(num25);
			if (avatar5 == null || avatar5.IDDB == -1 || BoardScr.indexPlayer[num25] != 3)
			{
				continue;
			}
			int num26 = 0;
			int num27 = 0;
			int num28 = distantCard / 2;
			for (int num29 = 0; num29 < 12; num29++)
			{
				if (ShowHaPhom[num25][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[num25][num29] == -1)
				{
					if (num27 == 1)
					{
						num26--;
						break;
					}
					num27 = 1;
				}
				else
				{
					num27 = 0;
				}
				num26++;
			}
			if (num26 > 0)
			{
				num28 = Canvas.hh / num26;
				if (num28 > distantCard)
				{
					num28 = distantCard;
				}
				else if (num28 < distantCard / 2)
				{
					num28 = distantCard / 2;
				}
			}
			for (int num30 = 0; num30 < 12; num30++)
			{
				if (ShowHaPhom[num25][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[num25][num30] != -1)
				{
					this.card = new Card((sbyte)ShowHaPhom[num25][num30], true);
					this.card.y = posCardEat[BoardScr.indexPlayer[num25]].y - (num26 - 1) * num28 / 2 + num30 * num28;
					this.card.x = posCardEat[BoardScr.indexPlayer[num25]].x;
					this.card.paintFull(g);
				}
			}
		}
		for (int num31 = 0; num31 < 4; num31++)
		{
			if (BoardScr.indexPlayer[num31] != 2)
			{
				continue;
			}
			int num32 = 0;
			int num33 = 0;
			int num34 = distantCard / 2;
			for (int num35 = 0; num35 < 12; num35++)
			{
				if (ShowHaPhom[num31][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[num31][num35] == -1)
				{
					if (num33 == 1)
					{
						num32--;
						break;
					}
					num33 = 1;
				}
				else
				{
					num33 = 0;
				}
				num32++;
			}
			if (num32 > 0)
			{
				num34 = Canvas.hw / num32;
				if (num34 > distantCard)
				{
					num34 = distantCard;
				}
				else if (num34 < distantCard / 2)
				{
					num34 = distantCard / 2;
				}
			}
			for (int num36 = 0; num36 < 12; num36++)
			{
				if (ShowHaPhom[num31][0] == -1)
				{
					break;
				}
				if (ShowHaPhom[num31][num36] != -1)
				{
					this.card = new Card((sbyte)ShowHaPhom[num31][num36], true);
					if (BoardScr.indexPlayer[num31] == 2)
					{
						this.card.x = posCardEat[BoardScr.indexPlayer[num31]].x - (num32 - 1) * num34 / 2 + num36 * num34;
						this.card.y = posCardEat[BoardScr.indexPlayer[num31]].y;
					}
					this.card.paintFull(g);
				}
			}
		}
		if (myCard != null)
		{
			paintCard(g);
		}
	}

	private void paintcardShow(MyGraphics g)
	{
		if (finish)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(i);
			if (avatar == null || avatar.IDDB == -1)
			{
				continue;
			}
			int num = 0;
			if (cardShow[i] != null)
			{
				for (int j = 0; j < 4; j++)
				{
					if (cardShow[i][j] != null)
					{
						num++;
					}
				}
			}
			if (BoardScr.indexPlayer[i] == 0 || BoardScr.indexPlayer[i] == 2)
			{
				for (int k = 0; k < 4; k++)
				{
					if (cardShow[i] == null)
					{
						break;
					}
					if (cardShow[i][k] == null)
					{
						break;
					}
					if (BoardScr.indexPlayer[i] == 2)
					{
						cardShow[i][k].paintFull(g);
					}
					else
					{
						cardShow[i][k].paintFull(g);
					}
				}
			}
			else
			{
				for (int l = 0; l < 4 && cardShow[i][l] != null; l++)
				{
					cardShow[i][l].paintFull(g);
				}
			}
		}
	}

	private void paintCard(MyGraphics g)
	{
		for (int i = 0; i < 10; i++)
		{
			int num = 0;
			if (myCard[i] != null && myCard[i].cardID != -1)
			{
				ca = new Card(-1, true);
				ca.x = myCard[i].x;
				ca.y = myCard[i].y;
				if (!myCard[i].isShow)
				{
					ca = myCard[i];
				}
				if (num == 0 && i < 9 && myCard[i + 1].cardID != -1 && i != selectedCard)
				{
					if (disCard > 14 || myCard[i + 1].x != myCard[i + 1].xTo)
					{
						ca.paintHalfBackFull(g);
					}
					else
					{
						ca.paintHalf(g);
					}
					if (myCard[i].phom != 0)
					{
						PaintLineColor(myCard[i].phom, myCard[i].x, myCard[i].y, 2, g);
					}
				}
				else
				{
					ca.paintFull(g);
					if (myCard[i].phom != 0)
					{
						PaintLineColor(myCard[i].phom, myCard[i].x, myCard[i].y, 2, g);
					}
				}
			}
			if (i == selectedCard)
			{
				paintCardChange(g);
			}
		}
	}

	private void PaintLineColor(int phom, int x, int y, int i, MyGraphics g)
	{
		int num = 0;
		switch (phom)
		{
		case 1:
		case 4:
			num = colorPhom_1;
			break;
		case 2:
		case 5:
			num = colorPhom_2;
			break;
		case 3:
		case 6:
			num = colorPhom_3;
			break;
		}
		g.setColor(num);
		g.fillRect(x - BoardScr.wCard / 2 + 2, y - BoardScr.hcard / 2 + 22, 7f, 2f);
	}

	private void paintCardChange(MyGraphics g)
	{
		if (hCard.cardID == -1)
		{
			return;
		}
		hCard.x = xShow + selectedCard * disCard;
		hCard.y = posCard.y + ((!trans) ? 10 : (-5));
		if (Canvas.w > 176)
		{
			if (selectedCard < 9)
			{
				hCard.paintFull(g);
			}
			else
			{
				hCard.paintFull(g);
			}
		}
		else
		{
			hCard.paintSmall(g, false);
		}
	}

	private void detectPhom()
	{
		int num = -1;
		for (int i = 0; i < 8 && !searchCardEat(myCard[i].phom); i++)
		{
			int[] array = new int[6];
			for (int j = 0; j < 6; j++)
			{
				array[j] = -1;
			}
			for (int k = 0; k < 3; k++)
			{
				if (myCard[i + k].phom != 0)
				{
					num = k + 1;
					return;
				}
				array[k] = myCard[i + k].cardID;
			}
			if (num != -1)
			{
				i = num;
				num = -1;
			}
			else if (checkPhomSanh(array) || checkPhomBaLa(array))
			{
				phomRandom++;
				for (int l = 0; l < 3; l++)
				{
					myCard[i + l].phom = (sbyte)phomRandom;
				}
				i += 2;
			}
		}
	}

	private void addCard(int index)
	{
		if (myCard[index].phom != 0 || myCard[index].cardID == -1)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		if (index > 0 && myCard[index - 1].phom != 0 && myCard[index - 1].cardID != -1)
		{
			int[] array = new int[10];
			for (int i = 0; i < 10; i++)
			{
				if (myCard[i].phom == myCard[index - 1].phom)
				{
					array[i] = myCard[i].cardID;
					num += array[i] / 4 + 1;
				}
				else
				{
					array[i] = -1;
				}
			}
			array = orderArrayIncrease(array);
			array[9] = myCard[index].cardID;
			array = orderArrayIncrease(array);
			if (!checkPhomSanh(array) && !checkPhomBaLa(array))
			{
				num = 0;
			}
		}
		if (index < 9 && (index == 0 || (index != 0 && myCard[index + 1].phom != myCard[index - 1].phom)) && myCard[index + 1].phom != 0 && myCard[index + 1].cardID != -1)
		{
			int[] array2 = new int[10];
			for (int j = 0; j < 10; j++)
			{
				if (myCard[j].phom == myCard[index + 1].phom)
				{
					array2[j] = myCard[j].cardID;
					num2 += array2[j] / 4 + 1;
				}
				else
				{
					array2[j] = -1;
				}
			}
			array2 = orderArrayIncrease(array2);
			array2[9] = myCard[index].cardID;
			array2 = orderArrayIncrease(array2);
			if (!checkPhomSanh(array2) && !checkPhomBaLa(array2))
			{
				num2 = 0;
			}
		}
		if (num < num2)
		{
			myCard[index].phom = myCard[index + 1].phom;
		}
		else if (num > 0)
		{
			myCard[index].phom = myCard[index - 1].phom;
		}
	}

	private void findPhom()
	{
		phomRandom = 3;
		detectPhom();
		for (int i = 0; i < 10; i++)
		{
			if (!searchCardEat(myCard[i].phom) && myCard[i].phom == 0 && myCard[i].cardID != -1)
			{
				addCard(i);
			}
		}
		checkU();
	}

	private void cleanPhomRandom()
	{
		int num = 0;
		for (int i = 0; i < 10 && !searchCardEat(myCard[i].phom); i++)
		{
			if (myCard[i].phom != 0 && num != myCard[i].phom)
			{
				num = myCard[i].phom;
				phomRandom--;
			}
			myCard[i].phom = 0;
		}
	}

	private bool searchCardEat(int phom)
	{
		for (int i = 0; i < 3; i++)
		{
			if (cardEat[i].phom == 0)
			{
				return false;
			}
			if (cardEat[i].phom == phom)
			{
				return true;
			}
		}
		return false;
	}

	public void start(sbyte interval2, MyVector myCard, int firstMove, int firHa)
	{
		base.start(firstMove, interval2);
		reset();
		resetCurrentTime();
		currentPlayer = firstMove;
		firstPlayer = currentPlayer;
		BoardScr.interval = interval2;
		firstHa = firHa;
		BoardScr.isStartGame = true;
		int num = myCard.size();
		for (int i = 0; i < 10; i++)
		{
			this.myCard[i] = new Card(-1, true);
			this.myCard[i].x = Canvas.hw;
			this.myCard[i].y = Canvas.hh;
			this.myCard[i].isShow = true;
		}
		for (int j = 0; j < num; j++)
		{
			Card card = (Card)myCard.elementAt(j);
			this.myCard[j] = new Card(card.cardID, true);
			this.myCard[j].x = Canvas.hw;
			this.myCard[j].y = Canvas.hh;
			this.myCard[j].isShow = true;
		}
		orderCardIncrease(this.myCard);
		findPhom();
		if (currentPlayer != GameMidlet.avatar.IDDB)
		{
			resetCmd();
		}
		setPosPlaying();
		setPosCard(false);
	}

	public override void doFire()
	{
		resetOrderCard();
		base.doFire();
		resetChangeCard();
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < 10; i++)
		{
			if (myCard[i].cardID != -1 && myCard[i].isUp)
			{
				num++;
				num2 = i;
			}
		}
		if (num > 1)
		{
			Canvas.startOKDlg(T.canYouOnceOnly);
		}
		else if (num == 0)
		{
			Canvas.startOKDlg(T.yetSellectCard);
		}
		else if (checkCardToFire(num2))
		{
			resetCmd();
			setCmdWaiting();
			CasinoService.gI().firePhom(myCard[num2].cardID);
		}
	}

	private bool checkCardToFire(int sellectAsset)
	{
		if (myCard[sellectAsset].phom == 0)
		{
			return true;
		}
		if (!checkCardToEat(sellectAsset))
		{
			Canvas.startOKDlg(T.ifFireBreakPhom);
			return false;
		}
		return true;
	}

	public void setPosCardFire(int sNum)
	{
		int num = 0;
		if (cardShow[sNum] != null)
		{
			for (int i = 0; i < 4; i++)
			{
				if (cardShow[sNum][i] != null)
				{
					num++;
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (cardShow[sNum][j] != null)
			{
				switch (BoardScr.indexPlayer[sNum])
				{
				case 0:
					cardShow[sNum][j].xTo = posCardShow[BoardScr.indexPlayer[sNum]].x + (num - 1) * distantCard / 2 - j * distantCard;
					cardShow[sNum][j].yTo = posCardShow[BoardScr.indexPlayer[sNum]].y;
					break;
				case 2:
					cardShow[sNum][j].xTo = posCardShow[BoardScr.indexPlayer[sNum]].x - (num - 1) * distantCard / 2 + j * distantCard;
					cardShow[sNum][j].yTo = posCardShow[BoardScr.indexPlayer[sNum]].y;
					break;
				default:
					cardShow[sNum][j].xTo = posCardShow[BoardScr.indexPlayer[sNum]].x;
					cardShow[sNum][j].yTo = posCardShow[BoardScr.indexPlayer[sNum]].y - (num - 1) * distantCard / 2 + j * distantCard;
					break;
				}
			}
		}
	}

	public void onFire(int currentP, int firstP, int cardFire, sbyte numberCard)
	{
		if (cardFire == -1)
		{
			setCmdFire();
			Canvas.startOKDlg(T.cardToMiss);
			return;
		}
		int num = disShow;
		if (num < 35 * AvMain.hd)
		{
			num = 35 * AvMain.hd;
		}
		if (currentP == GameMidlet.avatar.IDDB)
		{
			resetOrderCard();
		}
		resetCmd();
		resetCurrentTime();
		int indexByID = BoardScr.getIndexByID(firstP);
		if (indexByID == -1)
		{
			return;
		}
		if (currentP == GameMidlet.avatar.IDDB)
		{
			if (getAssetCard(cardShow[BoardScr.indexOfMe]) != -1 && getAssetCard(cardShow[BoardScr.indexOfMe]) <= 3)
			{
				setCmdEatAndGet();
			}
			cleanPhomRandom();
			findPhom();
		}
		cardCurrent = cardFire;
		currentPlayer = currentP;
		firstPlayer = firstP;
		this.numberCard[indexByID] = numberCard;
		numC[indexByID]++;
		cardShow[indexByID][this.numberCard[indexByID]] = new Card((sbyte)cardCurrent, true);
		int num2 = 0;
		if (cardShow[indexByID] != null)
		{
			for (int i = 0; i < 4; i++)
			{
				if (cardShow[indexByID][i] != null)
				{
					num2++;
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (cardShow[indexByID][j] == null)
			{
				continue;
			}
			switch (BoardScr.indexPlayer[indexByID])
			{
			case 0:
				cardShow[indexByID][this.numberCard[indexByID]].x = posCardShow[BoardScr.indexPlayer[indexByID]].x;
				cardShow[indexByID][this.numberCard[indexByID]].y = -BoardScr.hcard / 2;
				continue;
			case 2:
				continue;
			}
			if (BoardScr.indexPlayer[indexByID] == 1)
			{
				cardShow[indexByID][this.numberCard[indexByID]].x = -BoardScr.wCard / 2;
			}
			else
			{
				cardShow[indexByID][this.numberCard[indexByID]].x = Canvas.w + BoardScr.wCard / 2;
			}
			cardShow[indexByID][this.numberCard[indexByID]].y = posCardShow[BoardScr.indexPlayer[indexByID]].y;
		}
		setPosCardFire(indexByID);
		if (firstPlayer == GameMidlet.avatar.IDDB)
		{
			for (int k = 0; k < 10; k++)
			{
				if (myCard[k].cardID == cardCurrent)
				{
					cardShow[indexByID][this.numberCard[indexByID]].x = myCard[k].x;
					cardShow[indexByID][this.numberCard[indexByID]].y = myCard[k].y;
					cleanCard(k);
					break;
				}
			}
			if (myCard[selectedCard].cardID == -1)
			{
				selectedCard = getAssetCard() - 1;
			}
		}
		resetUpCard();
		setPosCard(false);
	}

	public void onSkipPlayer(int curPlayer, int firHa)
	{
		int indexByID = BoardScr.getIndexByID(curPlayer);
		if (indexByID != -1)
		{
			firstHa = firHa;
			if (curPlayer == GameMidlet.avatar.IDDB && currentPlayer != curPlayer)
			{
				setCmdEatAndGet();
				setPosCard(false);
			}
			currentPlayer = curPlayer;
			resetCurrentTime();
		}
	}

	protected void doHaPhom()
	{
		resetOrderCard();
		if (GameMidlet.avatar.IDDB != currentPlayer)
		{
			Canvas.startOKDlg(T.waitToCurrent);
			return;
		}
		resetChangeCard();
		int[] array = new int[12];
		int num = -1;
		for (int i = 0; i < 10; i++)
		{
			if (myCard[i].phom != 0 && (num == -1 || num == myCard[i].phom))
			{
				num = myCard[i].phom;
				array[i] = myCard[i].cardID;
			}
			else
			{
				array[i] = -1;
			}
		}
		resetCmd();
		setCmdWaiting();
		CasinoService.gI().HaPhomPhom(myCard);
	}

	private void resetUpCard()
	{
		if (BoardScr.disableReady)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			if (myCard[i].cardID != -1 && myCard[i].isUp)
			{
				resetUpMyCard(i);
			}
		}
	}

	public void onHaPhom(bool isHa, int[] mCard, bool isU2, int[] orderMoney, int curID)
	{
		int indexByID = BoardScr.getIndexByID(curID);
		if (!isHa)
		{
			Canvas.startOKDlg(T.notSamePhom);
			return;
		}
		if (curID == GameMidlet.avatar.IDDB)
		{
			resetOrderCard();
		}
		resetUpCard();
		isU = isU2;
		int num = 1;
		for (int i = 0; i < ShowHaPhom[indexByID].Length; i++)
		{
			if (ShowHaPhom[indexByID][i] == -1)
			{
				if (num == 1)
				{
					num = i;
					break;
				}
				num = 1;
			}
			else
			{
				num = 0;
			}
		}
		for (int j = num; j < mCard.Length; j++)
		{
			if (mCard[j - num] != -1)
			{
				ShowHaPhom[indexByID][j] = mCard[j - num];
			}
		}
		if (GameMidlet.avatar.IDDB == curID)
		{
			isHaPhom = true;
			for (int k = 0; k < 10; k++)
			{
				for (int l = 0; l < mCard.Length; l++)
				{
					if (myCard[k].cardID == mCard[l])
					{
						resetCell(k);
						break;
					}
				}
			}
			setCmdFire();
			myCard = orderCardIncrease(myCard);
			if (myCard[selectedCard].cardID == -1)
			{
				selectedCard = getAssetCard() - 1;
			}
			setPosCard(false);
		}
		if (isU)
		{
			finish = true;
			setContinue();
		}
		numCardPhom[indexByID] = 0;
		for (int m = 0; m < ShowHaPhom[indexByID].Length; m++)
		{
			if (ShowHaPhom[indexByID][m] != -1)
			{
				numCardPhom[indexByID]++;
			}
		}
	}

	public void onFinish(int[] score, int[][] cardRac, int[] moneyPlayers)
	{
		resetOrderCard();
		int num = 1000;
		for (int i = 0; i < 4; i++)
		{
			if (score[i] >= 0 && score[i] < num)
			{
				num = score[i];
				winer = i;
			}
			scorePlayer[i] = score[i];
		}
		this.cardRac = cardRac;
		for (int j = 0; j < 4; j++)
		{
			numCardRac[j] = (sbyte)getAssetArray(cardRac[j]);
		}
		finish = true;
		setContinue();
		for (int k = 0; k < 4; k++)
		{
			Avatar avatar = (Avatar)BoardScr.avatarInfos.elementAt(k);
			if (avatar != null && avatar.IDDB != -1 && scorePlayer[k] != -1)
			{
				if (scorePlayer[k] == -2)
				{
					BoardScr.showChat(avatar.IDDB, "Cháy");
				}
				else if (k == winer)
				{
					BoardScr.showChat(avatar.IDDB, "Thắng");
				}
				else
				{
					BoardScr.showChat(avatar.IDDB, "Thua");
				}
				avatar.isReady = false;
			}
		}
		GameMidlet.avatar.isReady = false;
		setPosCard(false);
	}

	public void onOnceWin(int orderMoney3)
	{
		finish = true;
		setContinue();
		if (!BoardScr.disableReady)
		{
			winer = BoardScr.indexOfMe;
			BoardScr.showChat(((Avatar)BoardScr.avatarInfos.elementAt(winer)).IDDB, "Thắng");
		}
		BoardScr.isStartGame = true;
	}

	public void onDenBai(int winID, int[] orderMoney)
	{
		finish = true;
		setContinue();
		winer = winID;
		isU = true;
		denBai = BoardScr.getIndexByID(firstPlayer);
		BoardScr.showChat(winer, "Ù");
		BoardScr.showChat(firstPlayer, "Đền ù");
	}

	private void checkU()
	{
		if (numPhom + (phomRandom - 3) + phomHa == 3)
		{
			center = cmdHaPhom;
		}
	}

	private bool checkCardToEat(int id)
	{
		int num = -1;
		for (int i = 0; i < 3; i++)
		{
			if (cardEat[i].phom != 0 && cardEat[i].phom == myCard[id].phom)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			if (myCard[id].phom != 0)
			{
				int[] array = new int[10];
				for (int j = 0; j < 10; j++)
				{
					if (myCard[j].phom == myCard[id].phom && !myCard[j].isUp && j != id)
					{
						array[j] = myCard[j].cardID;
					}
					else
					{
						array[j] = -1;
					}
				}
				array = orderArrayIncrease(array);
				if (!checkPhomSanh(array) && !checkPhomBaLa(array))
				{
					for (int k = 0; k < 10; k++)
					{
						if (k != id && myCard[k].phom == myCard[id].phom)
						{
							myCard[k].phom = 0;
						}
					}
					myCard[id].phom = 0;
				}
			}
			return true;
		}
		int[] array2 = new int[10];
		for (int l = 0; l < 10; l++)
		{
			if (myCard[l].phom == cardEat[num].phom && !myCard[l].isUp && l != id)
			{
				array2[l] = myCard[l].cardID;
			}
			else
			{
				array2[l] = -1;
			}
		}
		array2 = orderArrayIncrease(array2);
		int num2 = -1;
		int num3 = 0;
		for (int m = 0; m < 10; m++)
		{
			if (array2[m] == cardEat[num].cardID)
			{
				num2 = m;
			}
		}
		for (int n = 0; n < 9 && array2[n + 1] != -1 && (array2[n + 1] / 4 == array2[n] / 4 || (array2[n + 1] / 4 - array2[n] / 4 == 1 && array2[n] % 4 == array2[n + 1] % 4)); n++)
		{
			num3 = n + 1;
		}
		if (num2 > num3 && num3 > 1)
		{
			return false;
		}
		if (num3 > 1)
		{
			for (int num4 = num3 + 1; num4 < 10; num4++)
			{
				for (int num5 = 0; num5 < 10; num5++)
				{
					if (array2[num4] == myCard[num5].cardID)
					{
						myCard[num5].phom = 0;
					}
				}
			}
			return true;
		}
		int[] array3 = new int[3];
		for (int num6 = 0; num6 < 3; num6++)
		{
			array3[num6] = -1;
		}
		for (int num7 = 0; num7 <= num3; num7++)
		{
			array3[num7] = array2[num7];
			array2[num7] = -1;
		}
		array2 = orderArrayIncrease(array2);
		if (checkPhomSanh(array2) || checkPhomBaLa(array2))
		{
			for (int num8 = 0; num8 < 3; num8++)
			{
				for (int num9 = 0; num9 < 10; num9++)
				{
					if (array3[num8] == myCard[num9].cardID)
					{
						myCard[num9].phom = 0;
					}
				}
			}
			return true;
		}
		return false;
	}

	private void doEat()
	{
		resetOrderCard();
		resetChangeCard();
		for (int i = 0; i < cardToEat.Length; i++)
		{
			cardToEat[i] = -1;
		}
		int num = 0;
		for (int j = 0; j < 10; j++)
		{
			if (myCard[j].cardID != -1 && myCard[j].isUp)
			{
				if (num == 5)
				{
					Canvas.startOKDlg(T.youSelect);
					return;
				}
				cardToEat[num] = myCard[j].cardID;
				num++;
			}
		}
		if (num < 2)
		{
			Canvas.startOKDlg(T.upTwoCard);
			return;
		}
		int[] array = new int[6];
		for (int k = 0; k < 5; k++)
		{
			if (cardToEat[k] != -1)
			{
				array[k] = cardToEat[k];
			}
			else
			{
				array[k] = -1;
			}
		}
		array[5] = cardCurrent;
		sbyte b = -1;
		if (checkPhomSanh(array))
		{
			b = 1;
		}
		if (checkPhomBaLa(array))
		{
			b = 0;
		}
		if (b == -1)
		{
			Canvas.startOKDlg(T.notPhom);
			return;
		}
		for (int l = 0; l < array.Length; l++)
		{
			if (array[l] == -1)
			{
				continue;
			}
			for (int m = 0; m < 10; m++)
			{
				if (array[l] == myCard[m].cardID)
				{
					if (!checkCardToEat(m))
					{
						Canvas.startOKDlg(T.notPhom);
						return;
					}
					break;
				}
			}
		}
		resetCmd();
		setCmdWaiting();
		CasinoService.gI().eatCardPhom(cardToEat, b);
	}

	public void onEatCard(bool isEat, int numEat, int firHa, sbyte nCard)
	{
		int indexByID = BoardScr.getIndexByID(currentPlayer);
		if (indexByID == -1)
		{
			return;
		}
		if (currentPlayer == GameMidlet.avatar.IDDB)
		{
			resetOrderCard();
		}
		if (isEat)
		{
			int indexByID2 = BoardScr.getIndexByID(firstPlayer);
			int num = 0;
			int num2 = 0;
			if (indexByID2 != -1)
			{
				num = cardShow[indexByID2][numberCard[indexByID2]].x;
				num2 = cardShow[indexByID2][numberCard[indexByID2]].y;
				int indexByID3 = BoardScr.getIndexByID(firstHa);
				if (indexByID2 != indexByID3)
				{
					cardShow[indexByID2][numberCard[indexByID2]] = cardShow[indexByID3][numberCard[indexByID3]];
					cardShow[indexByID2][numberCard[indexByID2]].xTo = num;
					cardShow[indexByID2][numberCard[indexByID2]].yTo = num2;
					cardShow[indexByID3][numberCard[indexByID3]] = null;
				}
				cardShow[indexByID3][numberCard[indexByID3]] = null;
				firstHa = firHa;
				numberCard[indexByID3] = nCard;
				numC[indexByID] = numberCard[indexByID];
				numC[indexByID3] = numberCard[indexByID3];
			}
			numCardEat[indexByID]++;
			if (currentPlayer == GameMidlet.avatar.IDDB)
			{
				numPhom++;
				if (getAssetCard(cardShow[BoardScr.indexOfMe]) == 3)
				{
					setcmdHaPhom();
				}
				else
				{
					setCmdFire();
				}
				for (int num3 = numEat - 1; num3 >= 0; num3--)
				{
					for (int i = 0; i < 10; i++)
					{
						if (myCard[i].cardID == cardToEat[num3])
						{
							myCard[i].phom = (sbyte)numPhom;
							myCard[getAssetCard()] = myCard[i];
							cleanCard(i);
						}
					}
				}
				int assetCard = getAssetCard();
				myCard[assetCard] = new Card((sbyte)cardCurrent, true);
				myCard[assetCard].phom = (sbyte)numPhom;
				myCard[assetCard].x = num;
				myCard[assetCard].y = num2;
				for (int j = 0; j < 3; j++)
				{
					if (cardEat[j].cardID == -1)
					{
						cardEat[j] = myCard[assetCard];
						break;
					}
				}
				cleanPhomRandom();
				findPhom();
			}
			int assetArray = getAssetArray(showCardEat[indexByID]);
			showCardEat[indexByID][assetArray] = new Card((sbyte)cardCurrent, true);
			showCardEat[indexByID][assetArray].x = num;
			showCardEat[indexByID][assetArray].y = num2;
			setPosCardEat(indexByID);
			resetUpCard();
		}
		else if (currentPlayer == GameMidlet.avatar.IDDB)
		{
			Canvas.startOKDlg(T.notPhom);
			setCmdEatAndGet();
		}
		if (GameMidlet.avatar.IDDB == currentPlayer || GameMidlet.avatar.IDDB == firstPlayer)
		{
			setPosCard(false);
		}
		for (int k = 0; k < 4; k++)
		{
			setPosCardFire(k);
		}
	}

	private void setPosCardEat(int sNum)
	{
		int num = 0;
		for (int i = 0; i < 3 && showCardEat[sNum][i] != null; i++)
		{
			num++;
		}
		for (int j = 0; j < 3; j++)
		{
			if (showCardEat[sNum][j] != null)
			{
				if (BoardScr.indexPlayer[sNum] == 0)
				{
					showCardEat[sNum][j].xTo = posCardEat[BoardScr.indexPlayer[sNum]].x + (num - 1) * distantCard / 2 - j * distantCard;
					showCardEat[sNum][j].yTo = posCardEat[BoardScr.indexPlayer[sNum]].y;
				}
				else
				{
					showCardEat[sNum][j].xTo = posCardEat[BoardScr.indexPlayer[sNum]].x;
					showCardEat[sNum][j].yTo = posCardEat[BoardScr.indexPlayer[sNum]].y - (num - 1) * distantCard / 2 + j * distantCard;
				}
			}
		}
	}

	private void resetCardEat()
	{
		isEatCard = true;
		cardE = new Card((sbyte)cardCurrent, true);
		int indexByID = BoardScr.getIndexByID(firstPlayer);
		cardE.x = BoardScr.posAvatar[BoardScr.indexPlayer[indexByID]].x;
		cardE.y = BoardScr.posAvatar[BoardScr.indexPlayer[indexByID]].y;
	}

	private void eatCard()
	{
		if (isEatCard)
		{
			int indexByID = BoardScr.getIndexByID(currentPlayer);
			int num = (BoardScr.posAvatar[BoardScr.indexPlayer[indexByID]].x - cardE.x) / 2;
			int num2 = (BoardScr.posAvatar[BoardScr.indexPlayer[indexByID]].y - cardE.y) / 2;
			cardE.x += num;
			cardE.y += num2;
			if (Math.abs(num) <= 1 && Math.abs(num2) <= 1)
			{
				isEatCard = false;
			}
		}
	}

	private void doGet()
	{
		resetOrderCard();
		resetChangeCard();
		resetCmd();
		setCmdWaiting();
		CasinoService.gI().GetCardPhom();
	}

	public void onGetCard(int cardGet)
	{
		resetOrderCard();
		int num = setAssetCard();
		myCard[num] = new Card((sbyte)cardGet, true);
		setPosCard(false);
		myCard[num].x = Canvas.hw;
		myCard[num].y = posCardShow[0].y + (posCardShow[2].y - posCardShow[0].y) / 2 - Canvas.numberFont.getHeight() / 2;
		if (getAssetCard(cardShow[BoardScr.indexOfMe]) == 3)
		{
			if (GameMidlet.avatar.IDDB == currentPlayer)
			{
				setcmdHaPhom();
			}
		}
		else if (GameMidlet.avatar.IDDB == currentPlayer)
		{
			setCmdFire();
		}
		if (!isHaPhom)
		{
			cleanPhomRandom();
			findPhom();
		}
		else
		{
			addCardToHaPhom(myCard[num].cardID);
		}
	}

	private void addCardToHaPhom(int id)
	{
		int[] array = new int[6];
		for (int i = 0; i < 5; i++)
		{
			array[i] = -1;
		}
		int num = 0;
		for (int j = 0; j < 12; j++)
		{
			if (ShowHaPhom[BoardScr.indexOfMe][j] != -1)
			{
				array[num] = ShowHaPhom[BoardScr.indexOfMe][j];
				num++;
				continue;
			}
			num = 0;
			array[5] = id;
			orderArrayIncrease(array);
			if (checkPhomSanh(array) || checkPhomBaLa(array))
			{
				doAddCardToPhom(array);
				break;
			}
			for (int k = 0; k < 6; k++)
			{
				array[k] = -1;
			}
		}
	}

	private void doAddCardToPhom(int[] card)
	{
		resetOrderCard();
		CasinoService.gI().doAddCardPhom(card);
	}

	public void onAddCardToPhom(bool isAdd, sbyte card1)
	{
		int indexByID = BoardScr.getIndexByID(currentPlayer);
		if (indexByID == -1)
		{
			return;
		}
		if (currentPlayer == GameMidlet.avatar.IDDB)
		{
			resetOrderCard();
		}
		if (!isAdd)
		{
			return;
		}
		if (GameMidlet.avatar.IDDB == currentPlayer)
		{
			for (int i = 0; i < 10 && myCard[i].cardID != -1; i++)
			{
				if (card1 == myCard[i].cardID)
				{
					resetCell(i);
					break;
				}
			}
			setPosCard(false);
		}
		int num = 0;
		int[] array = new int[6];
		for (int j = 0; j < 6; j++)
		{
			array[j] = -1;
		}
		for (int k = 0; k < ShowHaPhom[indexByID].Length; k++)
		{
			if (ShowHaPhom[indexByID][k] == -1)
			{
				num = 0;
				array[5] = card1;
				orderArrayIncrease(array);
				if (checkPhomSanh(array) || checkPhomBaLa(array))
				{
					for (int num2 = 11; num2 > k; num2--)
					{
						if (num2 - 1 >= 0)
						{
							ShowHaPhom[indexByID][num2] = ShowHaPhom[indexByID][num2 - 1];
						}
					}
					ShowHaPhom[indexByID][k] = card1;
				}
				for (int l = 0; l < 6; l++)
				{
					array[l] = -1;
				}
			}
			else
			{
				array[num] = ShowHaPhom[indexByID][k];
				num++;
			}
		}
	}

	private int setAssetCard()
	{
		for (int i = 0; i < 10; i++)
		{
			if (myCard[i].cardID == -1)
			{
				return i;
			}
			if (searchCardEat(myCard[i].phom))
			{
				for (int num = 9; num > i; num--)
				{
					myCard[num] = myCard[num - 1];
				}
				return i;
			}
		}
		return -1;
	}

	private int getAssetCard(Card[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	private int getAssetCard()
	{
		for (int i = 0; i < 10; i++)
		{
			if ((hCard.cardID == -1 || i != selectedCard) && myCard[i].cardID == -1)
			{
				return i;
			}
		}
		return -1;
	}

	private int getAssetArray(Card[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	private int getAssetArray(int[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == -1)
			{
				return i;
			}
		}
		return -1;
	}

	public override void setPlayers(sbyte roomID, sbyte boardID, int ownerID, int money, MyVector playerInfos)
	{
		base.setPlayers(roomID, boardID, ownerID, money, playerInfos);
		GameMidlet.avatar.isReady = false;
		BoardScr.notReadyDelay = 0;
	}

	private int[] orderArrayIncrease(int[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[j] != -1)
				{
					int num = array[i];
					if (num > array[j] || num == -1)
					{
						array[i] = array[j];
						array[j] = num;
					}
				}
			}
		}
		return array;
	}

	private Card[] orderCardIncrease(Card[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[j].cardID != -1)
				{
					Card card = array[i];
					if (card.cardID > array[j].cardID || card.cardID == -1)
					{
						array[i] = array[j];
						array[j] = card;
					}
				}
			}
		}
		return array;
	}

	private bool checkPhomSanh(int[] vCard)
	{
		int[] array = vCard;
		array = orderArrayIncrease(array);
		int num = 0;
		for (int i = 0; i < array.Length - 1 && array[i + 1] != -1; i++)
		{
			if (array[i] - array[i + 1] != 0 && array[i + 1] / 4 - array[i] / 4 == 1 && array[i] % 4 - array[i + 1] % 4 == 0)
			{
				num++;
				continue;
			}
			return false;
		}
		if (num > 1)
		{
			return true;
		}
		return false;
	}

	private bool checkPhomBaLa(int[] vCard)
	{
		int[] array = vCard;
		array = orderArrayIncrease(array);
		int num = 0;
		for (int i = 0; i < array.Length - 1 && array[i + 1] != -1; i++)
		{
			if (array[i] / 4 - array[i + 1] / 4 == 0 && array[i] - array[i + 1] != 0)
			{
				num++;
				continue;
			}
			return false;
		}
		if (num > 1)
		{
			return true;
		}
		return false;
	}

	public void onPlaying(int interval1, int curPlayer, int firPlayer, int[][] cardShow2, int[][] cardEat2, int firHa)
	{
		reset();
		BoardScr.disableReady = true;
		resetCurrentTime();
		for (int i = 0; i < cardShow2.Length; i++)
		{
			int num = 0;
			if (cardShow2[i] != null)
			{
				for (int j = 0; j < 4; j++)
				{
					num++;
				}
			}
			for (int k = 0; k < cardShow2[i].Length; k++)
			{
				if (cardShow2[i][k] != -1)
				{
					cardShow[i][k] = new Card((sbyte)cardShow2[i][k], true);
					switch (BoardScr.indexPlayer[i])
					{
					case 0:
						cardShow[i][k].y = (cardShow[i][k].yTo = posCardShow[BoardScr.indexPlayer[i]].y);
						cardShow[i][k].x = (cardShow[i][k].xTo = posCardShow[BoardScr.indexPlayer[i]].x + (num - 1) * distantCard / 2 - k * distantCard);
						break;
					case 2:
						cardShow[i][k].y = (cardShow[i][k].yTo = posCardShow[BoardScr.indexPlayer[i]].y);
						cardShow[i][k].x = (cardShow[i][k].xTo = posCardShow[BoardScr.indexPlayer[i]].x - (num - 1) * distantCard / 2 + k * distantCard);
						break;
					default:
						cardShow[i][k].x = (cardShow[i][k].xTo = posCardShow[BoardScr.indexPlayer[i]].x);
						cardShow[i][k].y = (cardShow[i][k].yTo = posCardShow[BoardScr.indexPlayer[i]].y - (num - 1) * distantCard / 2 + k * distantCard);
						break;
					}
				}
			}
		}
		for (int l = 0; l < cardEat2.Length; l++)
		{
			int num2 = 0;
			for (int m = 0; m < 3; m++)
			{
				num2++;
			}
			for (int n = 0; n < cardEat2[l].Length; n++)
			{
				if (cardEat2[l][n] != -1)
				{
					showCardEat[l][n] = new Card((sbyte)cardEat2[l][n], true);
					if (BoardScr.indexPlayer[l] == 0)
					{
						showCardEat[l][n].xTo = (showCardEat[l][n].x = posCardEat[BoardScr.indexPlayer[l]].x + (num2 - 1) * distantCard / 2 - n * distantCard);
						showCardEat[l][n].yTo = (showCardEat[l][n].y = posCardEat[BoardScr.indexPlayer[l]].y);
					}
					else
					{
						showCardEat[l][n].xTo = (showCardEat[l][n].x = posCardEat[BoardScr.indexPlayer[l]].x);
						showCardEat[l][n].yTo = (showCardEat[l][n].y = posCardEat[BoardScr.indexPlayer[l]].y - (num2 - 1) * distantCard / 2 + n * distantCard);
					}
				}
			}
		}
		BoardScr.interval = interval1;
		currentPlayer = curPlayer;
		firstPlayer = firPlayer;
		firstHa = firHa;
		BoardScr.isStartGame = true;
		for (int num3 = 0; num3 < 4; num3++)
		{
			cardShow[BoardScr.indexOfMe][num3] = null;
		}
		for (int num4 = 0; num4 < 3; num4++)
		{
			showCardEat[BoardScr.indexOfMe][num4] = null;
		}
		for (int num5 = 0; num5 < ShowHaPhom.Length; num5++)
		{
			ShowHaPhom[BoardScr.indexOfMe][num5] = -1;
		}
		for (int num6 = 0; num6 < cardRac.Length; num6++)
		{
			cardRac[BoardScr.indexOfMe][num6] = -1;
		}
		setPosPlaying();
		repaint();
		disCard = (Canvas.w - BoardScr.wCard / 2) / 10;
		if (disCard > BoardScr.wCard / 3 * 2)
		{
			disCard = BoardScr.wCard / 3 * 2;
		}
	}

	public void setAvatar(MyVector listAva)
	{
		BoardScr.avatarInfos = listAva;
		for (int i = 0; i < BoardScr.numPlayer; i++)
		{
			Avatar avatar = (Avatar)listAva.elementAt(i);
			if (avatar.IDDB == GameMidlet.avatar.IDDB)
			{
				avatar.seriPart = GameMidlet.avatar.seriPart;
			}
			avatar.direct = Base.RIGHT;
			avatar.setAction(2);
			avatar.setFrame(avatar.action);
		}
	}
}
