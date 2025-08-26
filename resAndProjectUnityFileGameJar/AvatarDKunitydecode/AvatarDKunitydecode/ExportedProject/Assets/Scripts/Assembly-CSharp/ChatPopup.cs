public class ChatPopup
{
	public int timeOut;

	public int xc;

	public int yc;

	public int h;

	public int w;

	public int hCount;

	public static short wPop;

	public static short hText;

	public string[] chats;

	public static FrameImage[] imgPopup = new FrameImage[3];

	public static Image[] imgArrow = new Image[2];

	private sbyte iNPC;

	public int timeCur;

	public int countDown;

	public ChatPopup()
	{
	}

	public ChatPopup(int time, string chat, sbyte NPC)
	{
		iNPC = NPC;
		prepareData(time, chat);
		hCount = h - imgPopup[0].frameHeight * 2;
		hText = (short)(Canvas.fontChatB.getHeight() - 5);
	}

	public void setPos(int x, int y)
	{
		xc = x;
		yc = y;
		if (onMainMenu.isOngame)
		{
			yc -= 20 * AvMain.hd;
		}
	}

	public bool setOut()
	{
		if (countDown > 1)
		{
			countDown /= 2;
		}
		if (Canvas.getTick() / 1000 - timeCur >= 5)
		{
			return true;
		}
		recalculatePos();
		return false;
	}

	public void prepareData(int time, string chat)
	{
		hText = (short)(Canvas.fontChatB.getHeight() - 5);
		w = 80 * AvMain.hd;
		chats = Canvas.fontChatB.splitFontBStrInLine(chat, w - 25 * AvMain.hd);
		h = hText * chats.Length + 4 + 4;
		if (h < wPop * 2 || chats.Length == 1)
		{
			h = wPop * 2;
		}
		w = 0;
		for (int i = 0; i < chats.Length; i++)
		{
			int num = Canvas.fontChatB.getWidth(chats[i]) + 25 * AvMain.hd;
			if (num > w)
			{
				w = num;
			}
		}
		if (w < 30 * AvMain.hd)
		{
			w = 30 * AvMain.hd;
		}
		timeOut = time;
		timeCur = (int)(Canvas.getTick() / 1000);
		if (countDown <= 1)
		{
			countDown = w / 4;
		}
	}

	public void recalculatePos()
	{
		if (Canvas.currentMyScreen is BoardScr && (onMainMenu.isOngame || BoardScr.isStartGame || BoardScr.disableReady))
		{
			if (yc - h < 0)
			{
				yc = h + 10;
			}
			if (xc - w / 2 < 0)
			{
				xc = w / 2;
			}
			if (xc + w / 2 > Canvas.w)
			{
				xc = Canvas.w - w / 2;
			}
		}
	}

	public void paintAnimal(MyGraphics g)
	{
		int num = AvMain.hd;
		if (Canvas.currentMyScreen == BoardScr.me && (onMainMenu.isOngame || BoardScr.isStartGame || BoardScr.disableReady))
		{
			num = 1;
		}
		paintRoundRect(g, xc * num - (w - countDown) / 2, yc * num - (h - countDown), w - countDown, h - countDown, (iNPC != 1) ? 16777215 : 16768679, (iNPC != 1) ? 14145495 : 13940870, iNPC);
		g.drawImage(imgArrow[iNPC], xc * num - ((iNPC == 0) ? (3 * AvMain.hd) : 0), yc * num - 2, MyGraphics.TOP | MyGraphics.HCENTER);
		for (int i = 0; i < chats.Length; i++)
		{
			Canvas.fontChatB.drawString(g, chats[i], xc * num - w / 2 + w / 2, yc * num - h / 2 + i * (hText - countDown / 2) - chats.Length * hText / 2 - hText / 4, 2);
		}
	}

	public static void paintRoundRect(MyGraphics g, int x, int y, int w, int h, int color1, int color2, sbyte iNPC)
	{
		imgPopup[iNPC].drawFrame(0, x, y, 0, g);
		imgPopup[iNPC].drawFrame(1, x + w - wPop, y, 0, g);
		imgPopup[iNPC].drawFrame(3, x, y + h - wPop, 0, g);
		imgPopup[iNPC].drawFrame(2, x + w - wPop, y + h - wPop, 0, g);
		g.setColor(color1);
		g.fillRect(x + wPop, y, w - wPop * 2, wPop);
		g.fillRect(x + wPop, y + h - wPop, w - wPop * 2, wPop);
		g.fillRect(x, y + wPop, w, h - wPop * 2);
		g.setColor(color2);
		g.fillRect(x + wPop, y, w - wPop * 2, 1f);
		g.fillRect(x + wPop, y + h - 1, w - wPop * 2, 1f);
		g.fillRect(x, y + wPop, 1f, h - wPop * 2);
		g.fillRect(x + w - 1, y + wPop, 1f, h - wPop * 2);
	}
}
