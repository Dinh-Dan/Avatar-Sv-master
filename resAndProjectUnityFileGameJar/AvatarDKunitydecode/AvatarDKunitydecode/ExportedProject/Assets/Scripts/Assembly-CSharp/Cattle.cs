public class Cattle : Animal
{
	public static AvPosition posPigTr;

	public static AvPosition posBucket;

	public static sbyte numPig;

	public static sbyte numTileW = 5;

	public static short itemID = -1;

	public Cattle()
	{
	}

	public Cattle(int id, sbyte species)
		: base(0, 0, id, species)
	{
		numPig++;
	}

	public override void setInit()
	{
		setPos(FarmScr.posBarn.x + 48 + CRes.rnd((FarmScr.numTileBarn - 2) * 6) * 4, FarmScr.posBarn.y + 24 + CRes.rnd(12) * 4);
	}

	public override void updatePos()
	{
		posNext = new AvPosition();
		if (!isEat)
		{
			setPosNext(new AvPosition(FarmScr.posBarn.x + 12 + CRes.rnd(FarmScr.numTileBarn * 6) * 4, FarmScr.posBarn.y + 12 + CRes.rnd(18) * 4));
		}
		else
		{
			doTranToFood();
		}
	}

	private void doTranToFood()
	{
		setPosNext(posPigTr);
	}

	public override void updateEat()
	{
		if (hunger && !isEat && itemID != -1)
		{
			isEat = true;
		}
	}

	public override void reset()
	{
		base.reset();
		if (isEat && CRes.abs(posPigTr.x - x) < 20 && CRes.abs(posPigTr.y - y) < 15)
		{
			isEat = false;
			hunger = false;
			if (FarmScr.gI().doEat(itemID, IDDB))
			{
				itemID = -1;
			}
		}
		cycle = 100 + 50 * (species - 50);
	}
}
