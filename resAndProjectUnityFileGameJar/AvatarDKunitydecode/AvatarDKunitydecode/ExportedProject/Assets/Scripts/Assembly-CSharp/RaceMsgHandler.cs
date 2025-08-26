using System;

public class RaceMsgHandler : IMiniGameMsgHandler
{
	public class dialogWin : Face
	{
		public string name;

		public sbyte idPet;

		public sbyte ratePet;

		private int wPopupWin;

		private int hPopupWin;

		public int tienCuoc;

		public int tienAn;

		public int tienThue;

		public int tienNhanDuoc;

		public dialogWin()
		{
			wPopupWin = (short)(200 * AvMain.hd);
			hPopupWin = (short)(AvMain.hNormal * 11);
		}

		public override void commandActionPointer(int index, int subIndex)
		{
			if (index == 0)
			{
				RaceScr.gI().left = null;
				RaceScr.gI().listPet = null;
				Canvas.currentFace = null;
			}
		}

		public override void paint(MyGraphics g)
		{
			if (Canvas.currentDialog != null)
			{
				return;
			}
			Canvas.paint.paintPopupBack(g, (Canvas.w - wPopupWin) / 2, (Canvas.h - hPopupWin) / 2, wPopupWin, hPopupWin, -1, false);
			g.translate((Canvas.w - wPopupWin) / 2, (Canvas.h - hPopupWin) / 2);
			int num = 0;
			Canvas.normalFont.drawString(g, RaceScr.gI().timeStart + string.Empty, wPopupWin / 2, (num += AvMain.hNormal) - AvMain.hNormal / 2 - AvMain.hd, 2);
			Canvas.normalFont.drawString(g, "Thú đua chiến thắng", wPopupWin / 2, (num += AvMain.hNormal) - AvMain.hNormal / 2, 2);
			Canvas.borderFont.drawString(g, name, wPopupWin / 2, num += AvMain.hNormal - 3 * AvMain.hd, 2);
			num += AvMain.hNormal * 2;
			for (int i = 0; i < 6; i++)
			{
				if (idPet == RaceScr.gI().listPet[i].IDDB)
				{
					ImageIcon imgIcon = AvatarData.getImgIcon(RaceScr.gI().listPet[i].idImg);
					if (imgIcon.count != -1)
					{
						int num2 = imgIcon.h / 5;
						g.drawRegion(imgIcon.img, 0f, RaceScr.FRAME[0][0] * num2, imgIcon.w, num2, 0, wPopupWin / 2, num + AvMain.hNormal / 2, 3);
					}
				}
			}
			num += AvMain.hNormal / 2;
			Canvas.normalFont.drawString(g, "Tiền cược: ", 10 * AvMain.hd, num += AvMain.hNormal, 0);
			Canvas.normalFont.drawString(g, string.Empty + tienCuoc, wPopupWin - 20 * AvMain.hd, num, 1);
			Canvas.normalFont.drawString(g, "Tiền ăn: ", 10 * AvMain.hd, num += AvMain.hNormal, 0);
			Canvas.normalFont.drawString(g, string.Empty + tienAn, wPopupWin - 20 * AvMain.hd, num, 1);
			Canvas.normalFont.drawString(g, "Tiền thuế: ", 10 * AvMain.hd, num += AvMain.hNormal, 0);
			Canvas.normalFont.drawString(g, string.Empty + tienThue, wPopupWin - 20 * AvMain.hd, num, 1);
			Canvas.normalFont.drawString(g, "Tiền nhận được: ", 10 * AvMain.hd, num += AvMain.hNormal, 0);
			Canvas.normalFont.drawString(g, string.Empty + tienNhanDuoc, wPopupWin - 20 * AvMain.hd, num, 1);
			base.paint(g);
		}
	}

	public class HistoryPopup : Dialog
	{
		private short[] idPet;

		private string[] time;

		private int w;

		private int h;

		private int hCell;

		private int cmtoY;

		private int cmy;

		private int cmdy;

		private int cmvy;

		private int cmyLim;

		private int pa;

		private bool trans;

		private int vY;

		private int count;

		private int timePoint;

		private int dyTran;

		private bool isTranKey;

		private sbyte countClose;

		public HistoryPopup(short[] idPet, string[] time)
		{
			this.idPet = idPet;
			this.time = time;
			h = 140 * AvMain.hd;
			w = 0;
			for (int i = 0; i < time.Length; i++)
			{
				int num = Canvas.normalFont.getWidth(time[i]) + 40 * AvMain.hd;
				if (num > w)
				{
					w = num + 10 * AvMain.hd;
				}
			}
			hCell = AvMain.hBorder + 5 * AvMain.hd;
			cmyLim = idPet.Length * hCell - (h - 10 * AvMain.hd);
			if (cmyLim < 0)
			{
				cmyLim = 0;
			}
		}

		public override void show()
		{
		}

		public override void updateKey()
		{
			count++;
			bool flag = false;
			if (Canvas.isPointerClick && Canvas.isPoint((Canvas.w - w) / 2 + w - 8 * AvMain.hd, (Canvas.hCan - h) / 2 - 24 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
			{
				Canvas.isPointerClick = false;
				countClose = 5;
				isTranKey = true;
			}
			if (isTranKey)
			{
				if (countClose == 5 && Canvas.isPointerDown && !Canvas.isPoint((Canvas.w - w) / 2 + w - 8 * AvMain.hd, (Canvas.hCan - h) / 2 - 24 * AvMain.hd, 40 * AvMain.hd, 40 * AvMain.hd))
				{
					countClose = 0;
				}
				if (Canvas.isPointerRelease)
				{
					if (countClose == 5)
					{
						countClose = 0;
						Canvas.currentDialog = null;
					}
					Canvas.isPointerRelease = false;
				}
			}
			if (Canvas.isPointerClick && Canvas.isPointer((Canvas.w - w) / 2, (Canvas.h - h) / 2, w, h))
			{
				Canvas.isPointerClick = false;
				if (!trans)
				{
					pa = cmy;
					trans = true;
					vY = 0;
				}
			}
			if (trans)
			{
				int num = Canvas.dy();
				if (Canvas.isPointerDown)
				{
					if (Canvas.gameTick % 3 == 0)
					{
						dyTran = Canvas.py;
						timePoint = count;
					}
					cmtoY = pa + num;
					vY = 0;
					if (cmtoY < 0 || cmtoY > cmyLim)
					{
						cmtoY = pa + num / 2;
					}
					cmy = cmtoY;
				}
				if (Canvas.isPointerRelease)
				{
					trans = false;
					int num2 = count - timePoint;
					int num3 = dyTran - Canvas.py;
					if (CRes.abs(num3) > 40 && num2 < 10 && cmtoY > 0 && cmtoY < cmyLim)
					{
						vY = num3 / num2 * 10;
					}
					timePoint = -1;
					if (Math.abs(num) < 10)
					{
						cmtoY = pa + num;
					}
				}
			}
			if (Canvas.keyHold[2])
			{
				cmtoY -= AvMain.hBorder;
				flag = true;
			}
			else if (Canvas.keyHold[8])
			{
				flag = true;
				cmtoY += AvMain.hBorder;
			}
			if (flag)
			{
				if (cmtoY < 0)
				{
					cmtoY = 0;
				}
				if (cmtoY > cmyLim)
				{
					cmtoY = cmyLim;
				}
			}
			if (vY != 0)
			{
				if (cmy < 0 || cmy > cmyLim)
				{
					vY -= vY / 4;
					cmy += vY / 20;
					if (vY / 10 <= 1)
					{
						vY = 0;
					}
				}
				if (cmy < 0)
				{
					if (cmy < -h / 2)
					{
						cmy = -h / 2;
						cmtoY = 0;
						vY = 0;
					}
				}
				else if (cmy > cmyLim)
				{
					if (cmy < cmyLim + h / 2)
					{
						cmy = cmyLim + h / 2;
						cmtoY = cmyLim;
						vY = 0;
					}
				}
				else
				{
					cmy += vY / 10;
				}
				cmtoY = cmy;
				vY -= vY / 10;
				if (vY / 10 == 0)
				{
					vY = 0;
				}
			}
			else if (cmy < 0)
			{
				cmtoY = 0;
			}
			else if (cmy > cmyLim)
			{
				cmtoY = cmyLim;
			}
			if (cmy != cmtoY)
			{
				cmvy = cmtoY - cmy << 2;
				cmdy += cmvy;
				cmy += cmdy >> 4;
				cmdy &= 15;
			}
			base.updateKey();
		}

		public override void paint(MyGraphics g)
		{
			Canvas.resetTrans(g);
			Canvas.paint.paintPopupBack(g, (Canvas.w - (w + 10 * AvMain.hd)) / 2, (Canvas.hCan - (h + 10 * AvMain.hd)) / 2, w + 20 * AvMain.hd, h + 20 * AvMain.hd, countClose / 3, false);
			g.translate((Canvas.w - w) / 2, (Canvas.hCan - h) / 2);
			g.setClip(0f, 7 * AvMain.hd, w, h - 10 * AvMain.hd);
			g.translate(0f, -cmy);
			for (int i = 0; i < idPet.Length; i++)
			{
				AvatarData.paintImg(g, idPet[i], 15 * AvMain.hd, 15 * AvMain.hd + i * hCell, 3);
				Canvas.normalFont.drawString(g, time[i], 30 * AvMain.hd, 15 * AvMain.hd + i * hCell - AvMain.hBorder / 2, 0);
			}
			base.paint(g);
		}
	}

	public class PetRace : Base
	{
		public sbyte rate;

		public sbyte count;

		public sbyte win = 10;

		public sbyte countWater = -1;

		public sbyte countFire = -1;

		public sbyte indexBui;

		public sbyte indexTe = 6;

		public short idImg;

		public short idIcon;

		public short[] numTick;

		public short[] vTick;

		public new string name = string.Empty;

		public int money;

		private int numF;

		public override void update()
		{
			indexBui++;
			if (indexBui >= 10)
			{
				indexBui = 0;
			}
			if (indexTe < 9)
			{
				indexTe++;
			}
			numF++;
			if (numF >= 6)
			{
				numF = 0;
			}
			frame++;
			if (frame == 24)
			{
				frame = 0;
			}
			if (x >= (LoadMap.wMap + 1) * LoadMap.w)
			{
				return;
			}
			if (numTick != null && count < numTick.Length && RaceScr.gI().countStart <= 0)
			{
				x += vTick[count];
				if (vTick[count] == 0)
				{
					action = 2;
				}
				else
				{
					action = 1;
				}
				numTick[count]--;
				if (numTick[count] <= 0)
				{
					count++;
					if (count < vTick.Length)
					{
						if (indexTe == 9 && vTick[count] == 0)
						{
							indexTe = 0;
						}
						else if (countWater == -1 && vTick[count] == 2)
						{
							countWater = 20;
						}
						else if (countFire == -1 && vTick[count] == 5)
						{
							countFire = 20;
						}
					}
				}
			}
			else
			{
				action = 0;
				if (vTick != null && RaceScr.gI().countStart <= 0)
				{
					x += vTick[vTick.Length - 1];
				}
				if (win == 10 && numTick != null && count >= numTick.Length)
				{
					RaceScr raceScr = RaceScr.gI();
					sbyte nWin;
					raceScr.nWin = (sbyte)((nWin = raceScr.nWin) + 1);
					win = nWin;
				}
			}
			if (countWater >= 0)
			{
				countWater--;
			}
			if (countFire >= 0)
			{
				countFire--;
			}
		}

		public override void paint(MyGraphics g)
		{
			ImageIcon imgIcon = AvatarData.getImgIcon(idImg);
			if (imgIcon.count != -1)
			{
				int num = imgIcon.h / 5;
				g.drawRegion(imgIcon.img, 0f, RaceScr.FRAME[action][frame / 2] * num, imgIcon.w, num, 0, x * MyObject.hd, y * MyObject.hd, MyGraphics.HCENTER | MyGraphics.BOTTOM);
				if (RaceScr.gI().isStart && money > 0)
				{
					Canvas.arialFont.drawString(g, string.Empty + money, x * MyObject.hd - imgIcon.w / 2 - 12 * MyObject.hd, y * MyObject.hd - AvMain.hBlack / 2 - MyObject.hd, 1);
				}
				if (countWater >= 0)
				{
					g.drawImage(RaceScr.imgWater, x * MyObject.hd + imgIcon.w / 2, y * MyObject.hd - num, MyGraphics.BOTTOM | MyGraphics.HCENTER);
				}
				if (indexTe < 9)
				{
					g.drawImage(RaceScr.imgTe[indexTe / 3], x * MyObject.hd, y * MyObject.hd, 3);
				}
				if (countFire >= 0)
				{
					g.drawImage(RaceScr.imgFire, x * MyObject.hd + imgIcon.w / 2, y * MyObject.hd - num, MyGraphics.BOTTOM | MyGraphics.HCENTER);
					g.drawImage(RaceScr.imgBui[indexBui / 2], x * MyObject.hd - imgIcon.w / 2, y * MyObject.hd, 3);
				}
				if (IDDB == AvCamera.gI().followPlayer.IDDB)
				{
					g.drawImage(MapScr.imgFocusP, x * MyObject.hd, y * MyObject.hd - num - numF / 2 - 10 * MyObject.hd, 3);
				}
			}
		}
	}

	public static RaceMsgHandler instance;

	public static void onHandler()
	{
		if (instance == null)
		{
			instance = new RaceMsgHandler();
		}
		GlobalMessageHandler.gI().miniGameMessageHandler = instance;
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case 1:
			{
				sbyte b3 = msg.reader().readByte();
				if (b3 == 0)
				{
					PetRace[] array3 = new PetRace[6];
					for (int k = 0; k < 6; k++)
					{
						array3[k] = new PetRace();
						array3[k].money = 0;
						array3[k].IDDB = msg.reader().readByte();
						array3[k].rate = msg.reader().readByte();
						array3[k].idImg = msg.reader().readShort();
						array3[k].idIcon = msg.reader().readShort();
					}
					short timeRemain = msg.reader().readShort();
					RaceScr.gI().doOpenRace(array3, timeRemain, false, true);
					break;
				}
				if (!msg.reader().readBoolean())
				{
					PetRace[] array4 = new PetRace[6];
					for (int l = 0; l < 6; l++)
					{
						array4[l] = new PetRace();
						array4[l].money = 0;
						array4[l].IDDB = msg.reader().readByte();
						array4[l].idImg = msg.reader().readShort();
						sbyte b4 = msg.reader().readByte();
						array4[l].numTick = new short[b4];
						array4[l].vTick = new short[b4];
						int num = 0;
						for (int m = 0; m < b4; m++)
						{
							array4[l].numTick[m] = msg.reader().readShort();
							array4[l].vTick[m] = msg.reader().readShort();
							num += array4[l].numTick[m];
						}
					}
					short timeRemain2 = msg.reader().readShort();
					RaceScr.gI().timeStart = msg.reader().readShort();
					RaceScr.gI().curTimeStart = Environment.TickCount;
					RaceScr.gI().doOpenRace(array4, timeRemain2, false, false);
					break;
				}
				for (int n = 0; n < 6; n++)
				{
					sbyte b5 = msg.reader().readByte();
					RaceScr.gI().listPet[n].numTick = new short[b5];
					RaceScr.gI().listPet[n].vTick = new short[b5];
					int num2 = 0;
					for (int num3 = 0; num3 < b5; num3++)
					{
						RaceScr.gI().listPet[n].numTick[num3] = msg.reader().readShort();
						RaceScr.gI().listPet[n].vTick[num3] = msg.reader().readShort();
						num2 += RaceScr.gI().listPet[n].numTick[num3];
					}
				}
				short timeRemain3 = msg.reader().readShort();
				RaceScr.gI().timeStart = msg.reader().readShort();
				RaceScr.gI().curTimeStart = Environment.TickCount;
				RaceScr.gI().doOpenRace(null, timeRemain3, true, false);
				break;
			}
			case 8:
			{
				sbyte b2 = msg.reader().readByte();
				short[] array = new short[b2];
				string[] array2 = new string[b2];
				for (int j = 0; j < b2; j++)
				{
					array[j] = msg.reader().readShort();
					array2[j] = msg.reader().readUTF();
				}
				if (b2 > 0)
				{
					Canvas.currentDialog = new HistoryPopup(array, array2);
				}
				else
				{
					Canvas.endDlg();
				}
				break;
			}
			case 2:
			{
				short idImg = msg.reader().readShort();
				string namePet = msg.reader().readUTF();
				short numWin = msg.reader().readShort();
				sbyte tile = msg.reader().readByte();
				sbyte phongDo = msg.reader().readByte();
				sbyte sucKhoe = msg.reader().readByte();
				RaceScr.gI().onPetInfo(idImg, namePet, numWin, tile, phongDo, sucKhoe);
				break;
			}
			case 5:
			{
				sbyte b = msg.reader().readByte();
				int money = msg.reader().readInt();
				for (int i = 0; i < RaceScr.gI().listPet.Length; i++)
				{
					if (b == RaceScr.gI().listPet[i].IDDB)
					{
						RaceScr.gI().listPet[i].money = money;
						RaceScr.gI().indexFocus = (sbyte)i;
						break;
					}
				}
				Canvas.endDlg();
				break;
			}
			case 9:
			{
				string text = msg.reader().readUTF();
				RaceScr.gI().onChat(text);
				break;
			}
			case 10:
				Canvas.currentDialog = null;
				RaceScr.gI().diaWin = new dialogWin();
				RaceScr.gI().diaWin.idPet = msg.reader().readByte();
				RaceScr.gI().diaWin.name = msg.reader().readUTF();
				RaceScr.gI().diaWin.ratePet = msg.reader().readByte();
				RaceScr.gI().diaWin.tienCuoc = msg.reader().readInt();
				RaceScr.gI().diaWin.tienAn = msg.reader().readInt();
				RaceScr.gI().diaWin.tienThue = msg.reader().readInt();
				RaceScr.gI().diaWin.tienNhanDuoc = msg.reader().readInt();
				break;
			case 3:
			case 4:
			case 6:
			case 7:
				break;
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
