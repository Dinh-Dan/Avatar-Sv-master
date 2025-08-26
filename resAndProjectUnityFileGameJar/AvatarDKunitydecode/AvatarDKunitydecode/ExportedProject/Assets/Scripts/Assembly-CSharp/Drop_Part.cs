public class Drop_Part : Base
{
	public short idDrop;

	public short deltaH;

	public int x0;

	public int y0;

	public int ID;

	private new sbyte g;

	private sbyte dir;

	private sbyte yDrop;

	private sbyte yRung;

	public sbyte type;

	public sbyte state;

	public const sbyte STATE_DROP = 0;

	public const sbyte STATE_FLY = 1;

	public const sbyte STATE_STAND = 2;

	public const sbyte STATE_FLY_UP = 3;

	public const sbyte STATE_DOWN = 4;

	public const sbyte DROP_POWER = 6;

	public Drop_Part()
	{
		catagory = 5;
	}

	public Drop_Part(sbyte typeDrop, short idDrop1, int id)
	{
		ID = id;
		catagory = 5;
		type = typeDrop;
		idDrop = idDrop1;
		dir = 0;
	}

	public override void update()
	{
		switch (state)
		{
		case 0:
		case 1:
			x += (short)(x0 - x >> 2);
			y += (short)(y0 - y >> 2);
			if (g >= -6)
			{
				deltaH += g;
				g--;
			}
			if ((CRes.abs(x - x0) < 4 || CRes.abs(y - y0) < 4) && deltaH <= 1)
			{
				x = x0;
				y = y0;
				deltaH = 0;
				g = 0;
				if (state == 1)
				{
					LoadMap.removePlayer(this);
				}
				state = 2;
			}
			break;
		case 3:
			deltaH += 3;
			if (deltaH > 50)
			{
				LoadMap.removePlayer(this);
			}
			break;
		case 4:
			if (deltaH > 0)
			{
				deltaH -= g;
				g++;
			}
			else
			{
				deltaH = 0;
				state = 2;
			}
			break;
		case 2:
			break;
		}
	}

	public override void paint(MyGraphics g)
	{
		g.drawImage(LoadMap.imgShadow, x, y + 1, MyGraphics.BOTTOM | MyGraphics.HCENTER);
		if (type == 0)
		{
			Part part = AvatarData.getPart(idDrop);
			part.paintIcon(g, x, y + yRung / 20 - deltaH, 0, MyGraphics.BOTTOM | MyGraphics.HCENTER);
		}
		else
		{
			AvatarData.paintImg(g, idDrop, x, y + yRung / 20 - deltaH, MyGraphics.BOTTOM | MyGraphics.HCENTER);
		}
		yRung -= 2;
		if (yRung < -20)
		{
			yRung = 0;
		}
	}

	public void startFlyTo(int idUser)
	{
		Avatar avatar = LoadMap.getAvatar(idUser);
		if (avatar != null)
		{
			x0 = avatar.x;
			y0 = avatar.y;
			state = 1;
			deltaH = 0;
		}
		else
		{
			deltaH = 0;
			state = 3;
		}
		g = 6;
	}

	public void startDropFrom(int idPlayer, short xTo, short yTo)
	{
		if (idPlayer == -2)
		{
			x = xTo;
			y = yTo;
			state = 2;
		}
		else
		{
			Avatar avatar = LoadMap.getAvatar(idPlayer);
			if (avatar != null)
			{
				x = avatar.x;
				y = avatar.y;
				state = 0;
				g = 6;
				deltaH = 0;
			}
			else
			{
				state = 4;
				x = xTo;
				y = yTo;
				deltaH = 100;
				g = 0;
			}
		}
		x0 = xTo;
		y0 = yTo;
	}
}
