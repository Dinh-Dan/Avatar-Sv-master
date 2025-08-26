public class CameraList
{
	public int cmdy;

	public int cmvy;

	public int cmyLim;

	public int xL;

	public int h;

	public int w;

	public int disY;

	public int disX;

	public int sizeW;

	public int sizeH;

	public int wOne;

	public int hOne;

	public int x;

	public int y;

	public int size;

	public int cmdx;

	public int cmvx;

	public int cmxLim;

	public static float cmy;

	public static float cmtoY;

	public static float cmx;

	public static float cmtoX;

	private int selected;

	public bool isShow;

	public bool isQuaTrang;

	private long timeDelay;

	private long count;

	public float pa;

	public float pb;

	public float vY;

	public float vX;

	public float dyTran;

	public float dxTran;

	public float timeOpen;

	public float xCamLast;

	public bool transY;

	public bool transX;

	public bool isSel;

	public bool isG;

	private long timePointY;

	private long timePointX;

	private int pxLast;

	private int pyLast;

	public void setInfo(int x, int y, int wOne, int hOne, int w, int h, int limX, int limY, int size)
	{
		isQuaTrang = false;
		this.x = x;
		this.y = y + Canvas.transTab;
		sizeH = h / hOne;
		sizeW = w / wOne;
		this.size = size;
		this.wOne = wOne;
		this.hOne = hOne;
		this.h = h;
		this.w = w;
		disY = limY;
		disX = limX;
		selected = 0;
		cmy = (cmtoY = 0f);
		cmyLim = h - disY;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmx = (cmtoX = 0f);
		cmxLim = w - disX;
		if (cmxLim < 0)
		{
			cmxLim = 0;
		}
		isShow = true;
		count = 0L;
	}

	public void close()
	{
		isShow = false;
	}

	public void setSelect(int se)
	{
		selected = se;
		setCam();
	}

	public void updateKey()
	{
		count++;
		if (Canvas.menuMain != null || Canvas.currentDialog != null)
		{
			return;
		}
		if (timeOpen > 0f)
		{
			timeOpen -= 1f;
			if (timeOpen == 0f && Canvas.currentMyScreen != PopupShop.me)
			{
				Canvas.currentMyScreen.setSelected(selected, true);
			}
			return;
		}
		if (Canvas.isPointerClick && Canvas.isPointer(x, y, w, disY))
		{
			pyLast = Canvas.pyLast;
			pxLast = Canvas.pxLast;
			Canvas.isPointerClick = false;
			timeDelay = count;
			pa = cmy;
			pb = cmx;
			xCamLast = cmx;
			transY = true;
			isG = false;
			if (vY != 0f || vX != 0f)
			{
				isG = true;
			}
			vY = 0f;
			vX = 0f;
		}
		if (!transY)
		{
			return;
		}
		long num = count - timeDelay;
		int num2 = pyLast - Canvas.py;
		pyLast = Canvas.py;
		int num3 = pxLast - Canvas.px;
		pxLast = Canvas.px;
		if (Canvas.isPointerDown)
		{
			if (count % 2 == 0)
			{
				dyTran = Canvas.py;
				dxTran = Canvas.px;
				timePointY = count;
				timePointX = count;
			}
			vY = 0f;
			vX = 0f;
			if (cmtoY <= 0f || cmtoY >= (float)cmyLim)
			{
				cmtoY = pa + (float)(num2 / 2);
				pa = cmtoY;
			}
			else
			{
				cmtoY = pa + (float)num2;
				pa = cmtoY;
			}
			if (cmtoX <= 0f || cmtoX >= (float)cmxLim)
			{
				cmtoX = pb + (float)(num3 / 2);
				pb = cmtoX;
			}
			else
			{
				cmtoX = pb + (float)num3;
				pb = cmtoX;
			}
			cmy = cmtoY;
			cmx = cmtoX;
			if (num < 20)
			{
				int num4 = (int)(cmtoY + (float)Canvas.py - (float)y) / hOne;
				int num5 = (int)((cmtoX + (float)Canvas.px - (float)x) / (float)wOne);
				selected = num4 * sizeW + num5;
				if (selected < 0)
				{
					selected = 0;
				}
				if (selected >= sizeH * sizeW)
				{
					selected = sizeH * sizeW - 1;
				}
				isSel = true;
				Canvas.currentMyScreen.setSelected(selected, false);
			}
			if (CRes.abs(Canvas.dy()) >= 10 * AvMain.hd || CRes.abs(Canvas.dx()) >= 10 * AvMain.hd)
			{
				Canvas.currentMyScreen.setHidePointer(true);
			}
			else if (num > 3 && num < 8 && !isG)
			{
				isSel = false;
				Canvas.currentMyScreen.setHidePointer(false);
			}
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		isG = false;
		transY = false;
		int num6 = (int)(count - timePointY);
		float num7 = dyTran - (float)Canvas.py;
		float num8 = dxTran - (float)Canvas.px;
		if (CRes.abs((int)num7) > 40 && num6 < 20 && cmtoY > 0f && cmtoY < (float)cmyLim)
		{
			vY = num7 / (float)num6 * 10f;
		}
		int num9 = (int)(count - timePointX);
		if (CRes.abs((int)num8) > 40 && num9 < 20 && cmtoX > 0f && cmtoX < (float)cmxLim)
		{
			vX = num8 / (float)num9 * 10f;
		}
		if (isQuaTrang)
		{
			if (Canvas.dx() > 20 * AvMain.hd)
			{
				if (Canvas.dx() > disX / 3)
				{
					int num10 = (int)(cmx / (float)disX) + 1;
					cmtoX = num10 * disX;
					vX = 0f;
				}
				else
				{
					int num11 = (int)(cmx / (float)disX);
					cmtoX = num11 * disX;
					vX = 0f;
				}
			}
			if (Canvas.dx() < -20 * AvMain.hd)
			{
				if (CRes.abs(Canvas.dx()) > disX / 3)
				{
					int num12 = (int)(cmx / (float)disX);
					cmtoX = num12 * disX;
					vX = 0f;
				}
				else
				{
					int num13 = (int)(cmx / (float)disX) + 1;
					cmtoX = num13 * disX;
					vX = 0f;
				}
			}
		}
		timePointY = -1L;
		timePointX = -1L;
		if (CRes.abs(Canvas.dy()) < 10 * AvMain.hd && CRes.abs(Canvas.dx()) < 10 * AvMain.hd)
		{
			if (num <= 4)
			{
				timeOpen = 5f;
				Canvas.currentMyScreen.setHidePointer(false);
			}
			else
			{
				Canvas.currentMyScreen.setSelected(selected, true);
				if (Canvas.currentMyScreen != PopupShop.me)
				{
					Canvas.currentMyScreen.setHidePointer(true);
				}
			}
			isSel = false;
			Canvas.paint.clickSound();
		}
		transX = false;
	}

	private void setCam()
	{
		if (!Canvas.isPointerDown)
		{
			cmtoY = selected / sizeW * hOne - disY / 2 + hOne / 2;
			if (cmtoY < 0f)
			{
				cmtoY = 0f;
			}
			if (cmtoY > (float)cmyLim)
			{
				cmtoY = cmyLim;
			}
			if (selected / sizeW > sizeH - 1 || selected / sizeW == 0)
			{
				cmy = cmtoY;
			}
			cmtoX = selected % sizeW * wOne - disX / 2 + wOne / 2;
			if (cmtoX < 0f)
			{
				cmtoX = 0f;
			}
			if (cmtoX > (float)cmxLim)
			{
				cmtoX = cmxLim;
			}
			if (selected % sizeW > sizeW - 1 || selected % sizeW == 0)
			{
				cmx = cmtoX;
			}
		}
	}

	public void moveCamera()
	{
		if (Canvas.menuMain != null || Canvas.currentDialog != null)
		{
			return;
		}
		if (vY != 0f)
		{
			if (cmy < 0f || cmy > (float)cmyLim)
			{
				if (vY > 500f)
				{
					vY = 500f;
				}
				else if (vY < -500f)
				{
					vY = -500f;
				}
				vY -= vY / 5f;
				if (CRes.abs((int)(vY / 10f)) <= 10)
				{
					vY = 0f;
				}
			}
			cmy += vY / 15f;
			cmtoY = cmy;
			vY -= vY / 20f;
		}
		else if (cmy < 0f)
		{
			cmtoY = 0f;
		}
		else if (cmy > (float)cmyLim)
		{
			cmtoY = cmyLim;
		}
		if (vX != 0f)
		{
			if (cmx < 0f || cmx > (float)cmxLim)
			{
				if (vX > 500f)
				{
					vX = 500f;
				}
				else if (vX < -500f)
				{
					vX = -500f;
				}
				vX -= vX / 5f;
				if (CRes.abs((int)(vX / 10f)) <= 10)
				{
					vX = 0f;
				}
			}
			cmx += vX / 15f;
			cmtoX = cmx;
			vX -= vX / 20f;
		}
		else if (cmx < 0f)
		{
			cmtoX = 0f;
		}
		else if (cmx > (float)cmxLim)
		{
			cmtoX = cmxLim;
		}
		if (cmy != cmtoY)
		{
			cmvy = (int)(cmtoY - cmy) << 2;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
		}
		if (cmx != cmtoX)
		{
			cmvx = (int)(cmtoX - cmx) << 2;
			cmdx += cmvx;
			cmx += cmdx >> 4;
			cmdx &= 15;
		}
	}
}
