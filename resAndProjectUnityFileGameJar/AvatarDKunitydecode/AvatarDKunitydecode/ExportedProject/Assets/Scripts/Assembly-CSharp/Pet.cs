public class Pet : Animal
{
	public Avatar follow;

	private MyVector listMove = new MyVector();

	private int xFir;

	private int yFir;

	private int quich;

	private int yFly;

	private int dir;

	private bool isFly;

	public static Image[] imgShadow;

	private static sbyte[][] FRAME;

	static Pet()
	{
		imgShadow = new Image[2];
		FRAME = new sbyte[3][];
		FRAME[0] = new sbyte[12]
		{
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3
		};
		FRAME[1] = new sbyte[12]
		{
			0, 0, 0, 1, 1, 1, 0, 0, 0, 1,
			1, 1
		};
		FRAME[2] = new sbyte[12]
		{
			2, 2, 2, 3, 3, 3, 2, 2, 2, 3,
			3, 3
		};
	}

	public Pet(Avatar fo)
	{
		catagory = 4;
		follow = fo;
		posNext = new AvPosition();
		posNext.x = follow.x - 40 + CRes.rnd(80);
		posNext.y = follow.y - 20 + CRes.rnd(40);
		xCur = (x = posNext.x);
		yCur = (y = posNext.y);
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart(follow.idPet);
		quich = aPartInfo.level;
	}

	public override void setPos()
	{
		if (listMove.size() > 0)
		{
			AvPosition avPosition = (AvPosition)listMove.elementAt(0);
			posNext.x = avPosition.x;
			posNext.y = avPosition.y;
			listMove.removeElementAt(0);
		}
		else
		{
			int num = CRes.rnd(20) - 10;
			if (CRes.abs(posNext.x + num - GameMidlet.avatar.x) >= 35)
			{
				num = 0;
			}
			posNext.x += num;
			posNext.y = y;
		}
		if (posNext.x < 0)
		{
			posNext.x = 5;
		}
		else if (posNext.x > LoadMap.wMap * 24)
		{
			posNext.x = LoadMap.wMap * 24 - 5;
		}
		else if (posNext.y < 0)
		{
			posNext.y = 5;
		}
		else if (posNext.y > LoadMap.Hmap * 24 - 24)
		{
			posNext.y = LoadMap.Hmap * 24 - 30;
		}
	}

	public override void updatePos()
	{
		setPos();
	}

	public override void update()
	{
		if (Canvas.gameTick % (3 - quich) == 0)
		{
			frame++;
		}
		if (Canvas.gameTick % 1 == 0 && action == 1 && y == yCur && isFly)
		{
			if (dir == 1)
			{
				yFly++;
				if (yFly > 3)
				{
					dir = -1;
				}
			}
			else
			{
				yFly--;
				if (yFly < -3)
				{
					dir = 1;
				}
			}
		}
		if (frame >= 12)
		{
			frame = 0;
		}
		setPosNext();
		if (action != 1)
		{
			if (cycle > 0)
			{
				if (frame == 0)
				{
					action = (sbyte)CRes.rnd(3 + quich * 2);
					if (action != 2)
					{
						action = 0;
					}
					else
					{
						direct = (sbyte)CRes.rnd(Base.RIGHT, Base.LEFT);
					}
					if (isFly)
					{
						action = 2;
					}
				}
				cycle--;
				if (CRes.distance(x, y, follow.x, follow.y) > 35)
				{
					base.reset();
					cycle = 0;
					v = 4;
				}
			}
			else
			{
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
		}
		else
		{
			move();
		}
	}

	private void setPosNext()
	{
		if (xFir == follow.x && yFir == follow.y)
		{
			return;
		}
		int num = CRes.distance(xFir, yFir, follow.x, follow.y);
		if (num > 40)
		{
			int num2 = 10 + CRes.rnd(20);
			if (follow.direct == Base.RIGHT)
			{
				num2 = -(10 + CRes.rnd(20));
			}
			if (LoadMap.getTypeMap(follow.x + num2, follow.y) != 80)
			{
				num2 = 0;
			}
			listMove.addElement(new AvPosition(follow.x + num2, follow.y));
			xFir = follow.x + num2;
			yFir = follow.y;
		}
	}

	public override void reset()
	{
		base.reset();
		cycle = 50 + CRes.rnd(100);
		if (listMove.size() > 0)
		{
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
			cycle = 0;
			disTrans = 1;
			v = 2 + quich;
		}
		else
		{
			v = 1 + CRes.rnd(quich);
		}
	}

	public override void paint(MyGraphics g)
	{
		if ((float)((x + 15) * MyObject.hd) < AvCamera.gI().xCam || (float)((x - 15) * MyObject.hd) > AvCamera.gI().xCam + (float)Canvas.w || follow.ableShow || (Canvas.stypeInt > 0 && Canvas.currentMyScreen == MainMenu.gI()))
		{
			return;
		}
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart(follow.idPet);
		if (aPartInfo.IDPart == -1)
		{
			return;
		}
		if (aPartInfo.IDPart >= 2000)
		{
			ImageIcon imagePart = AvatarData.getImagePart(aPartInfo.imgID[FRAME[action][frame]]);
			if (!isFly && aPartInfo.dy[0] + imagePart.h < -10 && imagePart.h > 0)
			{
				isFly = true;
				dir = 1;
			}
			if (imagePart.count != -1)
			{
				g.drawImage(imgShadow[(!isFly) ? 1u : 0u], x * MyObject.hd, (y - 1) * MyObject.hd, 3);
				g.drawRegion(imagePart.img, 0f, 0f, imagePart.w, imagePart.h, direct, x * MyObject.hd - imagePart.img.w / 2, (y - yFly) * MyObject.hd + aPartInfo.dy[FRAME[action][frame]] * MyObject.hd, 0);
			}
		}
		else
		{
			ImageInfo imageInfo = AvatarData.listImgInfo[aPartInfo.imgID[FRAME[action][frame]]];
			if (!isFly && aPartInfo.dy[0] + imageInfo.h * MyObject.hd < -10 && imageInfo.h > 0)
			{
				isFly = true;
				dir = 1;
			}
			g.drawImage(imgShadow[(!isFly) ? 1u : 0u], x * MyObject.hd, (y - 1) * MyObject.hd, 3);
			g.drawRegion(AvatarData.getBigImgInfo(imageInfo.bigID).img, imageInfo.x0 * MyObject.hd, imageInfo.y0 * MyObject.hd, imageInfo.w * MyObject.hd, imageInfo.h * MyObject.hd, direct, x * MyObject.hd + aPartInfo.dx[FRAME[action][frame]] * MyObject.hd - ((direct == Base.LEFT) ? (aPartInfo.dx[FRAME[action][frame]] * AvMain.hd * 2 + imageInfo.w * AvMain.hd) : 0), (y + yFly) * MyObject.hd + aPartInfo.dy[FRAME[action][frame]] * MyObject.hd, 0);
		}
	}

	public void paintIcon(MyGraphics g, int x, int y, int hunger)
	{
		APartInfo aPartInfo = (APartInfo)AvatarData.getPart(follow.idPet);
		if (aPartInfo.IDPart == -1)
		{
			return;
		}
		int num = y + aPartInfo.dy[FRAME[action][frame]];
		PaintPopup.fill(x - 10, num - 10, 20, 3, 11381824, g);
		g.setColor(11072024);
		g.drawRect(x - 10, num - 10, 20f, 3f);
		PaintPopup.fill(x - 9, num - 9, hunger * 20 / 100, 2, 16644608, g);
		if (aPartInfo.IDPart >= 2000)
		{
			ImageIcon imagePart = AvatarData.getImagePart(aPartInfo.imgID[FRAME[action][frame]]);
			if (imagePart.count != -1)
			{
				g.drawImage(imgShadow[(!isFly) ? 1u : 0u], x, y - 1, 3);
				g.drawRegion(imagePart.img, 0f, 0f, imagePart.w, imagePart.h, direct, x + aPartInfo.dx[FRAME[action][frame]] * MyObject.hd - ((direct == Base.LEFT) ? (aPartInfo.dx[FRAME[action][frame]] * AvMain.hd * 2 + imagePart.w * AvMain.hd) : 0), num + yFly, 0);
			}
		}
		else
		{
			ImageInfo imageInfo = AvatarData.listImgInfo[aPartInfo.imgID[FRAME[action][frame]]];
			g.drawImage(imgShadow[(!isFly) ? 1u : 0u], x, y - 1, 3);
			g.drawRegion(AvatarData.getBigImgInfo(imageInfo.bigID).img, imageInfo.x0 * MyObject.hd, imageInfo.y0 * MyObject.hd, imageInfo.w * MyObject.hd, imageInfo.h * MyObject.hd, direct, x + aPartInfo.dx[FRAME[action][frame]] * MyObject.hd - ((direct == Base.LEFT) ? (aPartInfo.dx[FRAME[action][frame]] * AvMain.hd * 2 + imageInfo.w * AvMain.hd) : 0), num + yFly, 0);
		}
	}

	public override void move()
	{
		int num = v * follow.hungerPet / 100;
		if (follow.hungerPet >= 70)
		{
			num = v;
		}
		if (num < 1)
		{
			num = 1;
		}
		int num2 = num * (disTrans * CRes.cos(CRes.fixangle(angle)) >> 10);
		int num3 = -num * (disTrans * CRes.sin(CRes.fixangle(angle))) >> 10;
		x = xCur + num2;
		y = yCur + num3;
		int num4 = CRes.distance(xCur, yCur, x, y);
		disTrans++;
		if (num4 > distant)
		{
			reset();
		}
	}
}
