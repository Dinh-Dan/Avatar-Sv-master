public class Dog : Animal
{
	private class IActionBitten : IAction
	{
		public void perform()
		{
			FarmScr.gI().onBittenByDog();
			Canvas.endDlg();
		}
	}

	public static bool isHound;

	public static bool isCan;

	public static AvPosition posDosTr;

	public static sbyte numBer;

	public static short itemID = -1;

	public Dog()
	{
	}

	public Dog(int id, sbyte species)
		: base(0, 0, id, species)
	{
		numBer++;
	}

	public override void setInit()
	{
		setPos((FarmScr.numTileBarn + 3) * 24 + setX(), 48 + CRes.rnd(30) * 4);
	}

	public int setX()
	{
		int num = LoadMap.wMap - FarmScr.numTilePond - FarmScr.numTileBarn - 5;
		return CRes.rnd(num * 6) * 4;
	}

	public override void updateEat()
	{
		if (isHound && FarmScr.idFarm != GameMidlet.avatar.IDDB)
		{
			if (v < 3)
			{
				reset();
			}
			if (!isCan && CRes.abs(GameMidlet.avatar.x - x) <= 24 && CRes.abs(GameMidlet.avatar.y - y) <= 24)
			{
				isCan = true;
				Canvas.startOK(T.youAreBittenByDog, new IActionBitten());
				Canvas.msgdlg.setDelay(20);
			}
		}
		if (itemID == -1)
		{
			isEat = false;
		}
		else if (hunger && !isEat)
		{
			isEat = true;
		}
	}

	public override void updatePos()
	{
		posNext = new AvPosition();
		setPos();
	}

	public override void setPos()
	{
		if (isHound && FarmScr.idFarm != GameMidlet.avatar.IDDB)
		{
			if (health > 20 && !hunger)
			{
				v = 8;
			}
			else
			{
				v = 6;
			}
			setPosNext(new AvPosition(GameMidlet.avatar.x, GameMidlet.avatar.y));
		}
		else if (isEat)
		{
			v = 2;
			setPosNext(posDosTr);
		}
		else
		{
			setPosNext(new AvPosition(288 + CRes.rnd(126) * 4, 24 + CRes.rnd(36) * 4));
		}
	}

	public override void reset()
	{
		if (!isEat && CRes.random(2) == 0)
		{
			cycle = 200;
		}
		if (isEat && CRes.distance(posDosTr.x, posDosTr.y, x, y) < 18)
		{
			isEat = false;
			hunger = false;
			cycle = 200;
			if (FarmScr.gI().doEat(itemID, IDDB))
			{
				itemID = -1;
			}
		}
		base.reset();
		if (isHound && FarmScr.idFarm != GameMidlet.avatar.IDDB)
		{
			cycle = 0;
		}
	}
}
