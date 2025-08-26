public class AnimalDan : Animal
{
	public int captainID;

	public sbyte indexFood;

	public AnimalDan(int x, int y, int id, sbyte species)
		: base(x, y, id, species)
	{
	}

	public override void update()
	{
		base.update();
	}

	public override void setAngleAndDis()
	{
		base.setAngleAndDis();
		if (!isEat && IDDB == captainID && distant > 150)
		{
			distant = 150;
		}
	}

	public virtual Point getPosEat()
	{
		return (Point)FarmScr.listFood[indexFood].elementAt(CRes.rnd(FarmScr.listFood[indexFood].size()));
	}

	public override void updatePos()
	{
		if (!isEat && captainID == IDDB)
		{
			setPos();
			return;
		}
		AvPosition avPosition = new AvPosition();
		if (isEat && FarmScr.listFood[indexFood].size() > 0)
		{
			Point posEat = getPosEat();
			if (posEat != null)
			{
				avPosition.x = posEat.x;
				avPosition.y = posEat.y;
				v = 2;
				setPosNext(avPosition);
			}
			else
			{
				setPos();
			}
			return;
		}
		int num = LoadMap.playerLists.size();
		for (int i = 0; i < num; i++)
		{
			Base @base = (Base)LoadMap.playerLists.elementAt(i);
			if (@base is AnimalDan && @base.IDDB == captainID)
			{
				avPosition = new AvPosition(@base.x, @base.y);
				break;
			}
		}
		if (indexFood != 1 && !LoadMap.isTrans(x, y))
		{
			setPos();
		}
		else
		{
			setFollowPos(avPosition);
		}
	}

	public virtual void setFollowPos(AvPosition pos)
	{
	}

	public override void reset()
	{
		int num = FarmScr.listFood[indexFood].size();
		if (hunger && isEat && num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				Point point = (Point)FarmScr.listFood[indexFood].elementAt(i);
				if (CRes.abs(point.x - x) <= 2 && CRes.abs(point.y - y) <= 2)
				{
					FarmScr.listFood[indexFood].removeElement(point);
					LoadMap.dynamicLists.removeElement(point);
					hunger = false;
					isEat = false;
					v = 1;
					FarmScr.gI().doEat(point.itemID, IDDB);
					break;
				}
			}
		}
		base.reset();
		cycle = 100 - ((captainID != IDDB) ? (indexFood * CRes.rnd(70)) : 0);
	}

	public override void updateEat()
	{
		if (FarmScr.listFood[indexFood].size() == 0)
		{
			isEat = false;
		}
		else if (hunger && !isEat)
		{
			isEat = true;
		}
	}
}
