public class EffectManager
{
	public short ID;

	public short loop;

	public short loopLimit;

	public short radius;

	public short x;

	public short y;

	public short count;

	public short indexLoop;

	public short indexPos;

	public int idPlayer;

	public sbyte style;

	public sbyte loopType;

	public short[] xLoop;

	public short[] yLoop;

	public EffectManager()
	{
		count = 0;
	}

	public void update()
	{
		EffectData effect = AvatarData.getEffect(ID);
		if (effect == null)
		{
			return;
		}
		if (style == 0)
		{
			Avatar avatar = LoadMap.getAvatar(idPlayer);
			if (avatar == null)
			{
				LoadMap.effManager.removeElement(this);
				return;
			}
			x = (short)avatar.x;
			y = (short)avatar.y;
		}
		if (count == loopLimit)
		{
			count = 0;
			EffectObj effectObj = new EffectObj();
			effectObj.ID = ID;
			effectObj.idPlayer = idPlayer;
			effectObj.style = style;
			switch (loopType)
			{
			case 0:
				effectObj.x = x;
				effectObj.y = y;
				break;
			case 1:
			{
				int num = CRes.rnd(radius);
				int angle = CRes.rnd(360);
				int num2 = num * CRes.cos(CRes.fixangle(angle)) >> 10;
				int num3 = -(num * CRes.sin(CRes.fixangle(angle))) >> 10;
				effectObj.x = x;
				effectObj.y = y;
				effectObj.dx = (short)num2;
				effectObj.dy = (short)num3;
				break;
			}
			case 2:
				effectObj.x = x;
				effectObj.y = y;
				if (style == 0)
				{
					effectObj.dx = xLoop[indexPos];
					effectObj.dy = yLoop[indexPos];
				}
				else
				{
					effectObj.x += xLoop[indexPos];
					effectObj.y += yLoop[indexPos];
				}
				break;
			}
			indexLoop++;
			indexPos++;
			if (xLoop != null && indexPos >= xLoop.Length)
			{
				indexPos = 0;
			}
			if (loop != -1 && indexLoop >= loop)
			{
				LoadMap.effManager.removeElement(this);
			}
			switch (style)
			{
			case 0:
				LoadMap.playerLists.addElement(effectObj);
				LoadMap.playerLists = LoadMap.orderVector(LoadMap.playerLists);
				break;
			case 1:
				LoadMap.treeLists.addElement(effectObj);
				LoadMap.treeLists = LoadMap.orderVector(LoadMap.treeLists);
				break;
			case 2:
				if (LoadMap.effBgList == null)
				{
					LoadMap.effBgList = new MyVector();
				}
				LoadMap.effBgList.addElement(effectObj);
				break;
			case 3:
				if (LoadMap.effCameraList == null)
				{
					LoadMap.effCameraList = new MyVector();
				}
				LoadMap.effCameraList.addElement(effectObj);
				break;
			}
		}
		count++;
	}
}
