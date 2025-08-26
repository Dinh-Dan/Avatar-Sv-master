public class Chicken : AnimalDan
{
	public static int numChicken;

	public static AvPosition posNest;

	public Chicken(int id, sbyte species, sbyte cap)
		: base(0, 0, id, species)
	{
		captainID = cap;
		indexFood = 0;
		numChicken++;
	}

	public override void setInit()
	{
		posNext = new AvPosition();
		if (captainID == IDDB)
		{
			x = (xCur = (posNext.x = (FarmScr.numTileBarn + 3) * 24 + setX()));
			y = (yCur = (posNext.y = 72 + CRes.rnd(24) * 4));
			return;
		}
		updatePos();
		if (!LoadMap.isTrans(x, y))
		{
			setPosNext(new AvPosition((FarmScr.numTileBarn + 3) * 24 + setX(), 72 + CRes.rnd(24) * 4));
		}
		x = (xCur = posNext.x);
		y = (yCur = posNext.y);
	}

	public int setX()
	{
		int num = LoadMap.wMap - FarmScr.numTilePond - FarmScr.numTileBarn - 5;
		return CRes.rnd(num * 6) * 4;
	}

	public override void setFollowPos(AvPosition pos)
	{
		setPosNext(new AvPosition(pos.x - 48 + setX(), pos.y - 48 + CRes.rnd(24) * 4));
	}

	public override void setPos()
	{
		base.setPos();
	}
}
