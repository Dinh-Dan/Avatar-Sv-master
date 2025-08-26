using System;

public class EffectObj : Base
{
	public short ID;

	public short dx;

	public short dy;

	public int idPlayer;

	public sbyte style;

	public new sbyte index;

	public EffectObj()
	{
		dx = (dy = 0);
		catagory = 6;
		index = 0;
	}

	public override void update()
	{
		try
		{
			EffectData effect = AvatarData.getEffect(ID);
			if (effect != null)
			{
				index++;
				if (index >= effect.arrFrame.Length)
				{
					removee();
				}
			}
			else
			{
				removee();
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public override void paint(MyGraphics g)
	{
		if (Canvas.currentMyScreen == MainMenu.gI())
		{
			return;
		}
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
				removee();
				return;
			}
			x = avatar.x + dx;
			y = avatar.y + dy;
		}
		effect.paint(g, x, y, index);
	}

	private void removee()
	{
		switch (style)
		{
		case 0:
			LoadMap.playerLists.removeElement(this);
			break;
		case 1:
			LoadMap.treeLists.removeElement(this);
			break;
		case 2:
			LoadMap.effBgList.removeElement(this);
			break;
		case 3:
			LoadMap.effCameraList.removeElement(this);
			break;
		}
	}
}
