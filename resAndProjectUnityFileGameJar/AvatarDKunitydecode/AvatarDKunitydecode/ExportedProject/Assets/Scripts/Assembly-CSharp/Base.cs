public class Base : MyObject
{
	public const sbyte DOING = -1;

	public const sbyte STAND = 0;

	public const sbyte RUN = 1;

	public const sbyte SAT_UP_STAND = 2;

	public const sbyte JUMPS = 10;

	public const sbyte AvatarRace = 11;

	public const sbyte TAM = 14;

	public int IDDB;

	public string name = string.Empty;

	public int frame;

	public sbyte g = 7;

	public sbyte vhy;

	public sbyte vh;

	public int xCur;

	public int yCur;

	public int vx;

	public int vy;

	public int v = 4;

	public sbyte action;

	public static sbyte RIGHT;

	public static sbyte LEFT = 2;

	public sbyte direct = LEFT;

	public ChatPopup chat;

	public MyVector listChat = new MyVector();

	public bool ableShow;

	protected void getChat()
	{
		if (chat == null && listChat.size() > 0)
		{
			chat = (ChatPopup)listChat.elementAt(0);
			chat.setPos(x, y - 45);
			listChat.removeElementAt(0);
		}
	}

	public void addChat(int time, string text, sbyte boss)
	{
		listChat.addElement(new ChatPopup(time, text, boss));
		getChat();
	}

	public override void paint(MyGraphics g)
	{
		if (chat != null && Canvas.currentMyScreen != MainMenu.me)
		{
			chat.paintAnimal(g);
		}
	}

	public void setPos(int x, int y)
	{
		base.x = (xCur = x);
		base.y = (yCur = y);
	}

	public override void update()
	{
		if (chat != null)
		{
			chat.setPos(x, y - 45);
			if (chat.setOut())
			{
				chat = null;
				getChat();
			}
		}
	}

	public virtual bool detectCollision(int vX, int vY)
	{
		if (action == -1 || action == 14)
		{
			vx = 0;
			vy = 0;
			return true;
		}
		if (action != 10 && action != 2 && action != 4)
		{
			action = 0;
		}
		if (action != 0 && action != 1)
		{
			vx = 0;
			vy = 0;
			return true;
		}
		action = 1;
		int num = x;
		int num2 = y;
		if (catagory == 2)
		{
			num = xCur;
			num2 = yCur;
		}
		if (LoadMap.isTrans(num + vX, num2 + vY))
		{
			if (vX != 0)
			{
				if (vX > 0)
				{
					vx = v;
				}
				else
				{
					vx = -v;
				}
			}
			if (vY != 0)
			{
				if (vY > 0)
				{
					vy = v;
				}
				else
				{
					vy = -v;
				}
			}
			return false;
		}
		vx = 0;
		vy = 0;
		return true;
	}

	public bool setWay(int vX, int vY)
	{
		if (action != 0 && action != 1)
		{
			return false;
		}
		int num = x;
		if (catagory == 0)
		{
			num += ((vX >= 0) ? 7 : (-7));
		}
		if (vX != 0)
		{
			int typeMap = LoadMap.getTypeMap(num + vX, y - 24);
			int typeMap2 = LoadMap.getTypeMap(num, y - 24);
			if (typeMap == 80 && typeMap2 == 80)
			{
				vy = -v;
				xCur = num;
				MapScr.gI().move();
				return true;
			}
			int typeMap3 = LoadMap.getTypeMap(num + vX, y + 24);
			int typeMap4 = LoadMap.getTypeMap(num, y + 24);
			if (typeMap3 == 80 && typeMap4 == 80)
			{
				vy = v;
				xCur = num;
				MapScr.gI().move();
				return true;
			}
		}
		else if (vY != 0)
		{
			int typeMap5 = LoadMap.getTypeMap(num - 24, y + vY);
			int typeMap6 = LoadMap.getTypeMap(num - 24, y);
			if (typeMap5 == 80 && typeMap6 == 80)
			{
				vx = -v;
				yCur = y;
				MapScr.gI().move();
				return true;
			}
			int typeMap7 = LoadMap.getTypeMap(num + 24, y + vY);
			int typeMap8 = LoadMap.getTypeMap(num + 24, y);
			if (typeMap7 == 80 && typeMap8 == 80)
			{
				vx = v;
				yCur = y;
				MapScr.gI().move();
				return true;
			}
		}
		return false;
	}

	public virtual void paintIcon(MyGraphics g2, int x, int y, bool isName)
	{
	}
}
