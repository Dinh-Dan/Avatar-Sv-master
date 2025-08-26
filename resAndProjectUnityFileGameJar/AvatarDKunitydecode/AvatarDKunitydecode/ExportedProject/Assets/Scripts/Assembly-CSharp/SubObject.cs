public class SubObject : MyObject
{
	public int type;

	public SubObject()
	{
		catagory = 1;
	}

	public SubObject(int type, int x, int y, int w)
	{
		catagory = 1;
		this.type = type;
		base.x = x;
		base.y = y;
		base.w = (short)w;
	}

	public SubObject(int type, int x, int y, int w, int h)
	{
		catagory = 1;
		this.type = type;
		base.x = x;
		base.y = y;
		base.w = (short)w;
		base.h = (short)h;
	}

	public SubObject(int i, int j)
	{
		catagory = 1;
		x = i;
		y = j;
	}

	public override void paint(MyGraphics g)
	{
		if (type < 0 && ((float)(x * MyObject.hd + w / 2) < AvCamera.gI().xCam || (float)(x * MyObject.hd - w / 2) > AvCamera.gI().xCam + (float)Canvas.w))
		{
			return;
		}
		int num = x * MyObject.hd;
		int num2 = y * MyObject.hd;
		switch (type)
		{
		case 0:
			AvatarData.paintImg(g, 243, num, num2, 33);
			break;
		case -2:
			paintDoing(g, num, num2);
			break;
		case -10:
		case -3:
			g.drawImage(FarmScr.imgBuyLant, num, num2, MyGraphics.BOTTOM | MyGraphics.RIGHT);
			break;
		case -5:
			paintTrough(g, num, num2, type);
			break;
		case -6:
			paintDogTr(g, num, num2);
			break;
		case -7:
			paintNest(g, num, num2, FarmScr.listBucket);
			break;
		case -8:
			paintNest(g, num, num2, FarmScr.listNest);
			break;
		case -9:
			if (Canvas.welcome != null)
			{
				g.drawImage(LoadMap.imgShadow, num, num2, 3);
				AvatarData.paintImg(g, 900, num, num2 + Canvas.welcome.num - 10, MyGraphics.BOTTOM | MyGraphics.HCENTER);
			}
			break;
		case -4:
		case -1:
			break;
		}
	}

	private static void paintNest(MyGraphics g, int x, int y, MyVector list)
	{
		for (int i = 0; i < list.size(); i++)
		{
			AvPosition avPosition = (AvPosition)list.elementAt(i);
			if (avPosition.x * MyObject.hd != x || avPosition.y * MyObject.hd != y)
			{
				continue;
			}
			AnimalInfo animalByID = FarmData.getAnimalByID(avPosition.anchor);
			if (animalByID.iconO != -1)
			{
				AvatarData.paintImg(g, animalByID.iconO, x, y, 3);
			}
			for (int j = 0; j < FarmScr.animalLists.size(); j++)
			{
				Animal animal = (Animal)FarmScr.animalLists.elementAt(j);
				if (animal.species == avPosition.anchor && animal.numEggOne > 0)
				{
					AvatarData.paintImg(g, animalByID.iconProduct, x, y, 3);
					return;
				}
			}
		}
	}

	private static void paintDogTr(MyGraphics g, int x, int y)
	{
		FarmScr.imgDogTr.drawFrame(0, x, y, 0, 3, g);
		if (Dog.itemID != -1)
		{
			FarmScr.imgDogTr.drawFrame(1, x, y, 0, 3, g);
		}
	}

	private static void paintTrough(MyGraphics g, int x, int y, int type)
	{
		FarmScr.imgTrough.drawFrame(0, x, y, 0, 3, g);
		if (Cattle.itemID != -1)
		{
			FarmScr.imgTrough.drawFrame(2, x, y, 0, 3, g);
		}
	}

	public static void paintDoing(MyGraphics g, int x, int y)
	{
		if (FarmScr.action != -1)
		{
			FarmScr.img.drawFrame(FarmScr.frame, x, y, (GameMidlet.avatar.direct == Base.LEFT) ? 2 : 0, 3, g);
		}
	}
}
