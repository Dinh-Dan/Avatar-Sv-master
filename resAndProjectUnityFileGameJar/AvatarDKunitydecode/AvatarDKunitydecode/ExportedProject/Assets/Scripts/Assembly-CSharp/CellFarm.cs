public class CellFarm : SubObject
{
	public int xCell;

	public int yCell;

	public int idTree;

	public short time;

	public long tempTime;

	public bool isArid;

	public bool isWorm;

	public bool isGrass;

	public bool isSelected;

	public sbyte hervestPer;

	public sbyte vitalityPer;

	public sbyte status;

	public sbyte level;

	public int statusTree;

	public override void paint(MyGraphics g)
	{
		if ((float)(x * MyObject.hd) < AvCamera.gI().xCam - 10f || (float)(xCell * MyObject.hd) > AvCamera.gI().xCam + (float)Canvas.w + 10f)
		{
			return;
		}
		if (isSelected)
		{
			if (MyObject.hd == 2)
			{
				g.drawImage(FarmScr.imgFocusCel, (x - 13) * MyObject.hd, (y - 18) * MyObject.hd, 0);
			}
			else
			{
				g.drawImage(FarmScr.imgFocusCel, x - 11, y - 15, 0);
			}
		}
		int num = FarmScr.focusCell.x;
		int num2 = FarmScr.focusCell.y;
		if (xCell == num && yCell == num2)
		{
			Canvas.smallFontYellow.drawString(g, "lv" + level, x * MyObject.hd, (y - 44) * MyObject.hd, 2);
		}
		if (idTree == -1)
		{
			return;
		}
		TreeInfo treeByID = FarmData.getTreeByID(idTree);
		treeByID.paint(g, statusTree, x * MyObject.hd, y * MyObject.hd, MyGraphics.BOTTOM | MyGraphics.HCENTER);
		int num3 = treeByID.harvestTime * 60 + treeByID.dieTime * 60;
		if ((time > num3 && treeByID.dieTime > 0) || hervestPer == 100 || time < 0)
		{
			return;
		}
		if (isGrass)
		{
			g.drawImage(FarmScr.imgWorm_G[1], (x + 5) * MyObject.hd, (y - 2) * MyObject.hd, 3);
		}
		if (isWorm)
		{
			g.drawImage(FarmScr.imgWorm_G[0], (x - 7) * MyObject.hd, y * MyObject.hd, 3);
		}
		if (xCell == num && yCell == num2)
		{
			treeByID.paint(g, 7, num * 24 * MyObject.hd - 3, (num2 * 24 - 40) * MyObject.hd, MyGraphics.BOTTOM | MyGraphics.HCENTER);
			g.setColor(1);
			g.fillRect((num * 24 - 4) * MyObject.hd, (num2 * 24 - 38) * MyObject.hd, 31 * MyObject.hd, 4 * MyObject.hd);
			g.setColor(65280);
			g.fillRect((num * 24 - 3) * MyObject.hd, (num2 * 24 - 37) * MyObject.hd, vitalityPer * 30 / 100 * MyObject.hd, 3 * MyObject.hd);
			g.setColor(2512938);
			g.drawRect((num * 24 - 4) * MyObject.hd, (num2 * 24 - 38) * MyObject.hd, 31 * MyObject.hd, 4 * MyObject.hd);
			long num4 = treeByID.harvestTime * 60 * 60 - tempTime;
			long num5 = treeByID.harvestTime * 60 - time;
			string empty = string.Empty;
			if (num4 < 0)
			{
				num4 = 0L;
			}
			long num6 = num4 / 60 / 60;
			long num7 = num4 / 60 % 60;
			string text = empty;
			empty = text + num6 + ":" + num7;
			if (num5 <= 0)
			{
				empty = T.canHarvest;
			}
			Canvas.smallFontYellow.drawString(g, empty, (num * 24 + 5) * MyObject.hd, (num2 * 24 - 40) * MyObject.hd - AvMain.hSmall, 0);
			int num8 = time * 100 / (treeByID.harvestTime * 60);
			num8 = num8 * 30 / 100;
			if (num8 == 0)
			{
				num8 = 1;
			}
			if (num8 >= 30)
			{
				num8 = 29;
			}
			if (treeByID.harvestTime * 60 - time < 0)
			{
				num8 = 30;
			}
			g.setColor(1);
			g.fillRect((num * 24 - 4) * MyObject.hd, (num2 * 24 - 32) * MyObject.hd, 31 * MyObject.hd, 4 * MyObject.hd);
			g.setColor(16776960);
			g.fillRect((num * 24 - 3) * MyObject.hd, (num2 * 24 - 31) * MyObject.hd, num8 * MyObject.hd, 3 * MyObject.hd);
			g.setColor(2512938);
			g.drawRect((num * 24 - 4) * MyObject.hd, (num2 * 24 - 32) * MyObject.hd, 31 * MyObject.hd, 4 * MyObject.hd);
			int num9 = 0;
			if (isGrass)
			{
				num9 = 1;
				FarmScr.imgWormAndGrass.drawFrame(1, (num * 24 + 5 + (isWorm ? 6 : 0)) * MyObject.hd, (num2 * 24 - 22) * MyObject.hd, 0, g);
			}
			if (isWorm)
			{
				FarmScr.imgWormAndGrass.drawFrame(0, (num * 24 + 4 - num9 * 6) * MyObject.hd, (num2 * 24 - 22) * MyObject.hd, 0, g);
			}
		}
	}
}
