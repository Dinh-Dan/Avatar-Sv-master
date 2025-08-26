using System;
using UnityEngine;

public class AvCamera
{
	public static AvCamera instance;

	public float xCam;

	public float yCam;

	public float xTo;

	public float yTo;

	public float xLimit;

	public float yLimit;

	public long timeDelay;

	private float cmvx;

	private float cmdx;

	private float cmvy;

	private float cmdy;

	public static int distance;

	public static int w;

	public static bool disable;

	public static bool isFollow;

	public static bool isMove;

	public Base followPlayer;

	public float hdFarm = 1f;

	public float vY;

	public float vX;

	public static AvCamera gI()
	{
		if (instance == null)
		{
			instance = new AvCamera();
		}
		return instance;
	}

	public static void setDistance(int dis)
	{
		distance = dis;
	}

	public void init(int index)
	{
		if (followPlayer == null)
		{
			return;
		}
		hdFarm = 1f;
		isFollow = false;
		hdFarm = AvMain.zoom;
		w = (int)((float)(LoadMap.w * AvMain.hd) * hdFarm);
		distance = Canvas.w / 20;
		if (distance < 20)
		{
			distance = 20;
		}
		if ((float)(followPlayer.x * AvMain.hd) * hdFarm > (float)Canvas.hw)
		{
			if ((float)(followPlayer.x * AvMain.hd) * hdFarm < (float)(LoadMap.wMap * w - Canvas.hw - w))
			{
				xTo = (int)((float)(followPlayer.x * AvMain.hd) * hdFarm - (float)Canvas.hw);
			}
			else
			{
				xTo = LoadMap.wMap * w - Canvas.w;
				if (xTo < 0f)
				{
					xTo = 0f;
				}
			}
		}
		else
		{
			xTo = 0f;
		}
		int hCan = Canvas.hCan;
		if (Canvas.w > LoadMap.wMap * w)
		{
			xTo = -(Canvas.w - LoadMap.wMap * w) / 2;
		}
		if (hCan > LoadMap.Hmap * w && (index - 1 == 57 || index - 1 == 58 || index - 1 == 59 || index - 1 == 108))
		{
			yTo = -(hCan - LoadMap.Hmap * w) / 2;
		}
		else
		{
			yTo = LoadMap.Hmap * w - hCan;
		}
		xLimit = (int)((float)(LoadMap.wMap * w - Canvas.w) / hdFarm);
		yLimit = (int)((float)(LoadMap.Hmap * w - hCan) / hdFarm);
		xCam = xTo;
		if (yTo > yLimit)
		{
			yTo = yLimit;
		}
		setPosFollowPlayer();
		xCam = xTo;
		yCam = yTo;
		if (xCam < 0f)
		{
			xCam = 0f;
		}
		if (xCam > xLimit)
		{
			xCam = xLimit;
		}
		if (yCam > yLimit)
		{
			yCam = yLimit;
		}
	}

	public void notTrans()
	{
		xCam = xTo;
		yCam = yTo;
	}

	public void updateTo()
	{
		float zoom = AvMain.zoom;
		if (!disable)
		{
			if (xCam != xTo)
			{
				cmvx = (int)(xTo - xCam) << 1;
				cmdx += cmvx;
				xCam += (int)cmdx >> 4;
				cmdx = (int)cmdx & 0xF;
				if (xCam < 0f)
				{
					xCam = 0f;
				}
				if (xCam > xLimit)
				{
					xCam = xLimit;
				}
			}
		}
		else
		{
			if (xCam < 0f)
			{
				xCam = 0f;
			}
			if (xCam > (float)(LoadMap.wMap * LoadMap.w * AvMain.hd) * zoom - (float)Canvas.w)
			{
				xCam = (int)((float)(LoadMap.wMap * LoadMap.w * AvMain.hd) * zoom - (float)Canvas.w);
			}
		}
		if (yCam != yTo)
		{
			cmvy = (int)(yTo - yCam) << 1;
			cmdy += cmvy;
			yCam += (int)cmdy >> 4;
			cmdy = (int)cmdy & 0xF;
			if (yCam > yLimit)
			{
				yCam = yLimit;
			}
		}
	}

	public void setToPos(float x, float y)
	{
		float zoom = AvMain.zoom;
		timeDelay = 0L;
		xTo = x - (float)Canvas.hw;
		yTo = y - (float)(Canvas.hCan / 2);
		if (xTo < 0f)
		{
			xTo = 0f;
		}
		if (xTo > (float)(LoadMap.wMap * w - Canvas.w))
		{
			xTo = LoadMap.wMap * w - Canvas.w;
		}
		if (yTo < 0f)
		{
			yTo = 0f;
		}
		int num = Canvas.hCan;
		if (TouchScreenKeyboard.visible)
		{
			num = Canvas.h - MyScreen.hTab - 17 * AvMain.hd;
		}
		if (yTo > (float)(LoadMap.Hmap * w - num))
		{
			yTo = LoadMap.Hmap * w - num;
		}
		setLimit();
	}

	public void setToPos(float x, float y, int ih)
	{
		timeDelay = 0L;
		xTo = x - (float)Canvas.hw;
		yTo = y - (float)(Canvas.hCan / 2);
		if (xTo < 0f)
		{
			xTo = 0f;
		}
		if (xTo > (float)(LoadMap.wMap * w - Canvas.w))
		{
			xTo = LoadMap.wMap * w - Canvas.w;
		}
		if (yTo < 0f)
		{
			yTo = 0f;
		}
		int num = ih - MyScreen.hTab - 17 * AvMain.hd;
		if (yTo > (float)(LoadMap.Hmap * w - num))
		{
			yTo = LoadMap.Hmap * w - num;
		}
		setLimit();
	}

	public void setPos(int x, int y)
	{
		xCam = (xTo = x);
		yCam = (yTo = y);
	}

	public void update()
	{
		if (Canvas.isZoom)
		{
			return;
		}
		if (Canvas.currentMyScreen != RaceScr.me && Canvas.welcome == null)
		{
			isFollow = true;
		}
		if (followPlayer != null && MapScr.isWedding)
		{
			isFollow = false;
		}
		if (isMove && CRes.abs((int)vX) <= 1 && CRes.abs((int)vY) <= 1)
		{
			isMove = false;
			timeDelay = Environment.TickCount / 100;
			vX = (vY = 0f);
		}
		moveCamera();
		if (vX != 0f || vY != 0f)
		{
			return;
		}
		updateTo();
		if (!isFollow)
		{
			int num = 0;
			num = ((followPlayer.direct != Base.RIGHT) ? (followPlayer.x * AvMain.hd - distance) : (followPlayer.x * AvMain.hd + distance));
			xTo = (int)((float)num - (float)Canvas.hw / hdFarm);
			int hCan = Canvas.hCan;
			yTo = (int)((float)(followPlayer.y * AvMain.hd) - ((float)hCan / hdFarm - ((float)(hCan / 2) / hdFarm - (float)(w / 3))));
			if (followPlayer.direct == Base.LEFT)
			{
				if ((float)followPlayer.x < (float)Canvas.hw / ((float)AvMain.hd * hdFarm))
				{
					xTo = 0f;
				}
			}
			else if (xTo > xLimit)
			{
				xTo = xLimit;
			}
		}
		setLimit();
	}

	public void setPosFollowPlayer()
	{
		int num = 0;
		num = ((followPlayer.direct != Base.RIGHT) ? (followPlayer.x * AvMain.hd - distance) : (followPlayer.x * AvMain.hd + distance));
		xTo = (int)((float)num - (float)Canvas.hw / hdFarm);
		int hCan = Canvas.hCan;
		yTo = (int)((float)(followPlayer.y * AvMain.hd) - ((float)hCan / hdFarm - ((float)(hCan / 2) / hdFarm - (float)(w / 3))));
		if (followPlayer.direct == Base.LEFT)
		{
			if ((float)followPlayer.x < (float)Canvas.hw / ((float)AvMain.hd * hdFarm))
			{
				xTo = 0f;
			}
		}
		else if (xTo > xLimit)
		{
			xTo = xLimit;
		}
	}

	public void setLimit()
	{
		int hCan = Canvas.hCan;
		if (LoadMap.TYPEMAP >= 0 && LoadMap.TYPEMAP < LoadMap.bg.Length && LoadMap.bg[LoadMap.TYPEMAP] == -1 && LoadMap.imgBG == null)
		{
			if (hCan > LoadMap.Hmap * w)
			{
				yCam = (yTo = -(hCan - LoadMap.Hmap * w) / 2);
			}
		}
		else if (yCam > yLimit)
		{
			yCam = (yTo = yLimit);
		}
		if (Canvas.w > LoadMap.wMap * w)
		{
			xCam = (xTo = -(Canvas.w - LoadMap.wMap * w) / 2);
		}
		else if (xCam < 0f)
		{
			xCam = (xTo = 0f);
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
			float num = -Canvas.hCan;
			if (yCam + vY / 15f < num + 100f)
			{
				if (yCam + vY / 15f < num)
				{
					yCam = (yTo = -Canvas.hCan);
					vY /= 6f;
					vY *= -1f;
				}
				else
				{
					vY -= vY / 20f;
				}
			}
			if (yCam + vY / 15f > yLimit - (float)(100 * AvMain.hd))
			{
				if (yCam + vY / 15f >= yLimit)
				{
					yCam = (yTo = yLimit);
					vY /= 6f;
					vY *= -1f;
				}
				else
				{
					vY -= vY / 20f;
				}
			}
			yCam += vY / 15f;
			yTo = yCam;
			vY -= vY / 20f;
		}
		if (vX == 0f)
		{
			return;
		}
		if (xCam + vX / 15f < (float)(100 * AvMain.hd))
		{
			if (xCam + vX / 15f < 0f)
			{
				xCam = (xTo = 0f);
				vX /= 6f;
				vX *= -1f;
			}
			else
			{
				vX -= vX / 20f;
			}
		}
		if (xCam + vX / 15f > xLimit - (float)(100 * AvMain.hd))
		{
			if (xCam + vX / 15f >= xLimit)
			{
				xCam = (xTo = xLimit);
				vX /= 6f;
				vX *= -1f;
			}
			else
			{
				vX -= vX / 20f;
			}
		}
		xCam += vX / 15f;
		xTo = xCam;
		vX -= vX / 20f;
	}
}
