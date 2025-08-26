using System;

public class Animal : Base
{
	public const sbyte CHICKEN = 50;

	public const sbyte COW = 51;

	public const sbyte PIG = 52;

	public const sbyte DOG = 53;

	public const sbyte ANY_GO = 1;

	public const sbyte LIMIT_CHUONG = 2;

	public const sbyte LIMIT_WATER = 3;

	public int disTrans;

	public int angle;

	public int distant;

	public int period;

	public int cycle;

	public bool isEat;

	public int bornTime;

	public int health;

	public bool isHarvest;

	public bool hunger;

	public bool[] disease = new bool[2];

	public sbyte species;

	public sbyte harvestPer;

	public sbyte hDelta;

	public sbyte indexFr;

	public AvPosition posNext;

	public int numEggOne;

	public bool isStand;

	public int timeStand;

	public Animal()
	{
		catagory = 2;
	}

	public Animal(int x, int y, int id, sbyte species)
	{
		AnimalInfo animalByID = FarmData.getAnimalByID(species);
		name = animalByID.name;
		catagory = 2;
		setPos(x, y);
		direct = 0;
		action = 0;
		IDDB = id;
		period = 0;
		g = 4;
		vy = g;
		v = 1;
		this.species = species;
		frame = CRes.rnd(12);
	}

	public virtual void setInit()
	{
	}

	public override void paint(MyGraphics g)
	{
		if ((float)(x * MyObject.hd + 30) < AvCamera.gI().xCam || (float)(x * MyObject.hd - 30) > AvCamera.gI().xCam + (float)Canvas.w || Canvas.currentMyScreen == MainMenu.gI() || (Canvas.menuMain != null && Canvas.menuMain == Menu.me))
		{
			return;
		}
		AnimalInfo animalByID = FarmData.getAnimalByID(species);
		ImageIcon imgIcon = AvatarData.getImgIcon(animalByID.idImg[period]);
		if (imgIcon.count != -1)
		{
			if (height == 0)
			{
				height = (short)(imgIcon.h / animalByID.frame);
			}
			if (catagory != 7)
			{
				indexFr = animalByID.arrFrame[action][frame];
			}
			g.drawRegion(imgIcon.img, 0f, indexFr * height, imgIcon.w, height, direct, x * MyObject.hd, y * MyObject.hd + hDelta, (animalByID.area == 4) ? 17 : 33);
			base.paint(g);
			paintFocus(g, imgIcon.w, height + 2, x * MyObject.hd, y * MyObject.hd, LoadMap.focusObj);
		}
	}

	public override void paintIcon(MyGraphics g, int x, int y, bool isName)
	{
		AnimalInfo animalByID = FarmData.getAnimalByID(species);
		ImageIcon imgIcon = AvatarData.getImgIcon(animalByID.idImg[period]);
		if (imgIcon.count != -1)
		{
			if (height == 0)
			{
				height = (short)(imgIcon.h / animalByID.frame);
			}
			if (catagory != 7)
			{
				indexFr = animalByID.arrFrame[action][frame];
			}
			g.drawRegion(imgIcon.img, 0f, indexFr * height, imgIcon.w, height, direct, x, y + hDelta, (animalByID.area == 4) ? 17 : 33);
			paintFocus(g, imgIcon.w, height + 2, x, y, this);
		}
	}

	public void paintFocus(MyGraphics g, int ww, int hh, int x, int y, MyObject obj)
	{
		if (catagory == 7)
		{
			hh = 10;
		}
		int num = period * 5;
		if (disease[0])
		{
			FarmScr.imgBenh.drawFrame(0, x - 10 * MyObject.hd, y - (24 + hDelta) * MyObject.hd - hh, 0, 3, g);
		}
		if (disease[1])
		{
			FarmScr.imgBenh.drawFrame(1, x + 10 * MyObject.hd, y - (24 + hDelta) * MyObject.hd - hh, 0, 3, g);
		}
		PaintPopup.fill(x - (22 + num) * MyObject.hd / 2, y - (18 + hDelta) * MyObject.hd - hh, (22 + num) * MyObject.hd, 4 * MyObject.hd, 1, g);
		PaintPopup.fill(x - (21 + num) * MyObject.hd / 2, y - (17 + hDelta) * MyObject.hd - hh, health * (20 + num) / 100 * MyObject.hd, 2 * MyObject.hd, 65280, g);
		int num2 = FarmData.getAnimalByID(species).harvestTime * 60 - bornTime;
		if (bornTime <= FarmData.getAnimalByID(species).harvestTime * 60)
		{
			Canvas.smallFontYellow.drawString(g, num2 / 60 + ":" + (num2 - num2 / 60 * 60), x, y - (13 + hDelta) * MyObject.hd - hh, 2);
		}
	}

	public override void update()
	{
		base.update();
		if (isStand)
		{
			if (Environment.TickCount / 1000 - timeStand > 10)
			{
				isStand = false;
			}
			return;
		}
		frame++;
		if (frame >= 12)
		{
			frame = 0;
		}
		updateEat();
		if (action != 1)
		{
			if (frame == 0)
			{
				action = (sbyte)CRes.rnd(5 + (species - 50) * 5);
				if (action != 2)
				{
					action = 0;
				}
				else
				{
					direct = (sbyte)CRes.rnd(Base.RIGHT, Base.LEFT);
				}
			}
			if (cycle > 0)
			{
				cycle--;
				return;
			}
			updatePos();
			if (posNext.x > x)
			{
				direct = Base.RIGHT;
			}
			else
			{
				direct = Base.LEFT;
			}
			setAngleAndDis();
			action = 1;
		}
		else
		{
			move();
		}
	}

	public virtual void updatePos()
	{
	}

	public virtual void updateEat()
	{
	}

	public virtual void move()
	{
		int num = v * (disTrans * CRes.cos(CRes.fixangle(angle)) >> 10);
		int num2 = -v * (disTrans * CRes.sin(CRes.fixangle(angle))) >> 10;
		if (detectCollision(num, num2))
		{
			if (setWay(num, num2))
			{
				x += vx;
				y += vy;
			}
			reset();
			return;
		}
		x = xCur + num;
		y = yCur + num2;
		int num3 = CRes.distance(xCur, yCur, x, y);
		disTrans++;
		if (num3 > distant)
		{
			reset();
		}
	}

	public void setPosNext(AvPosition pos)
	{
		posNext = pos;
	}

	public virtual void setAngleAndDis()
	{
		distant = CRes.distance(x, y, posNext.x, posNext.y);
		angle = CRes.angle(posNext.x - x, -(posNext.y - y));
	}

	public virtual void setPos()
	{
		setPosNext(new AvPosition(CRes.rnd(LoadMap.wMap * 6) * 4, CRes.rnd(LoadMap.Hmap * 6) * 4));
	}

	public virtual void reset()
	{
		action = 0;
		xCur = x;
		yCur = y;
		vx = 0;
		vy = 0;
		disTrans = 0;
	}
}
