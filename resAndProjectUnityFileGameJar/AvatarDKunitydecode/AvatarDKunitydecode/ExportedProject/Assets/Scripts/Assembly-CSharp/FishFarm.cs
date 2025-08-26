public class FishFarm : AnimalDan
{
	public new sbyte index;

	public static int WTile = 5;

	private AvPosition waves;

	public sbyte zump;

	public FishFarm(int id, sbyte species, sbyte cap)
		: base(0, 0, id, species)
	{
		captainID = cap;
		indexFood = 1;
		catagory = 7;
		waves = new AvPosition(-10, 0, CRes.rnd(8));
	}

	public override void update()
	{
		if (period == 2)
		{
			if (waves.anchor == 6 || waves.x == -10)
			{
				waves.x = x + ((direct != Base.RIGHT) ? (-3) : 3);
				waves.y = y + 2;
			}
			waves.anchor++;
			if (waves.anchor > 17)
			{
				waves.anchor = 6;
			}
		}
		AnimalInfo animalByID = FarmData.getAnimalByID(species);
		indexFr = animalByID.arrFrame[action][frame];
		int num = CRes.rnd(100);
		if (num == 2 && zump <= 0 && action == 0)
		{
			zump = 8;
		}
		if (zump > 0)
		{
			indexFr = (sbyte)(2 - zump / 3 + 2);
			zump--;
			hDelta = zump;
			if (hDelta >= 4)
			{
				hDelta = (sbyte)(4 - zump % 4);
			}
			hDelta += 5;
			hDelta *= -1;
		}
		else
		{
			hDelta = 0;
		}
		base.update();
	}

	public override void paint(MyGraphics g)
	{
		base.paint(g);
		if (period == 2 && waves.anchor < 16)
		{
			g.setColor(Fish.color[LoadMap.status]);
		}
	}

	public override void setInit()
	{
		posNext = new AvPosition();
		x = (xCur = (posNext.x = FarmScr.posPond.x + CRes.rnd(FarmScr.numTilePond - 1) * 24));
		y = (yCur = (posNext.y = FarmScr.posPond.y + 12 + CRes.rnd(3) * 24));
	}

	public override void setPos()
	{
		setPosNext(new AvPosition(FarmScr.posPond.x + 30 + CRes.rnd(FarmScr.numTilePond - 2) * 24, FarmScr.posPond.y + 12 + CRes.rnd(3) * 24));
	}

	public override void setFollowPos(AvPosition pos)
	{
		setPosNext(new AvPosition(pos.x - 10 + CRes.rnd(20), pos.y - 10 + CRes.rnd(20)));
	}

	public override bool detectCollision(int vX, int vY)
	{
		if (action == -1)
		{
			vx = 0;
			vy = 0;
			return true;
		}
		if (action != 0 && action != 1)
		{
			vx = 0;
			vy = 0;
			return true;
		}
		action = 1;
		int num = xCur;
		int num2 = yCur;
		if (!LoadMap.isTrans(num + vX, num2 + vY))
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

	public override Point getPosEat()
	{
		Point point = (Point)FarmScr.listFood[indexFood].elementAt(CRes.rnd(FarmScr.listFood[indexFood].size()));
		if (LoadMap.isTrans(point.x, point.y) || point.g != 0)
		{
			return null;
		}
		return point;
	}
}
