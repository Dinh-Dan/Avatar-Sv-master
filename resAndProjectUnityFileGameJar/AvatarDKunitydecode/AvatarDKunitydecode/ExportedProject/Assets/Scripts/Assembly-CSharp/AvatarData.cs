using System;
using System.Collections;
using UnityEngine;

public class AvatarData
{
	public static string sAvaPart = "avatarPart";

	public static string sImage = "avatarImgData";

	public static string sData = "avatarData";

	public static string sItemInfo = "avatarItemInfo";

	public static string sImgBig = "avatarImgBig";

	public static string sMapItemType = "avatarMapItemType";

	public static string sMapType = "avatarMapType";

	public static string sObject = "avatarObject";

	public static string sTile = "avatarTile";

	public static string sVer = "avatarVs";

	public static int verImg;

	public static int verPart;

	public static int verItemImg;

	public static int verObj;

	public static ImageInfo[] listImgInfo;

	public static Part[] listPart;

	public static MyVector listItemInfo;

	private static MyVector bigImgInfo = new MyVector();

	public static MyHashTable listBigImg = new MyHashTable();

	public static MyHashTable listBigImgBB;

	public static int playing = -1;

	public static MyVector listMapItemType = new MyVector();

	public static int verItemType;

	public static int verItem;

	public static MyVector listMapItem = new MyVector();

	public static MyVector listAd;

	public static MyHashTable listImgIcon = new MyHashTable();

	public static MyHashTable listImgPart = new MyHashTable();

	public static MyHashTable listPartDynamic = new MyHashTable();

	public static MyVector effectList = new MyVector();

	public static void delRMS()
	{
		RMS.deleteAll();
	}

	public void checkDataAvatar(MyVector bigInfo, int verBigImg, int verPart2, int verBigItemImg, int vItemType, int vItem, int vObj)
	{
		Out.println("checkDataAvatar");
		try
		{
			playing = 0;
			loadVersion();
			if (!loadImgBig())
			{
				AvatarData.bigImgInfo = bigInfo;
				int num = bigInfo.size();
				for (int i = 0; i < num; i++)
				{
					BigImgInfo bigImgInfo = (BigImgInfo)bigInfo.elementAt(i);
					AvatarService.gI().getBigImage(bigImgInfo.id);
					playing++;
				}
			}
			else
			{
				int num2 = bigInfo.size();
				for (int j = 0; j < num2; j++)
				{
					BigImgInfo bigImgInfo2 = (BigImgInfo)bigInfo.elementAt(j);
					BigImgInfo bigImgInfoList = getBigImgInfoList(bigImgInfo2.id);
					if (bigImgInfoList == null)
					{
						AvatarData.bigImgInfo.addElement(bigImgInfo2);
						AvatarService.gI().getBigImage(bigImgInfo2.id);
						playing++;
					}
					else if (bigImgInfo2.ver != bigImgInfoList.ver)
					{
						AvatarService.gI().getBigImage(bigImgInfo2.id);
						playing++;
					}
				}
			}
			if (!loadImageData())
			{
				verImg = verBigImg;
				AvatarService.gI().getImageData();
				playing++;
			}
			else if (verImg != verBigImg)
			{
				verImg = verBigImg;
				AvatarService.gI().getImageData();
				playing++;
			}
			if (!loadAvatarPart())
			{
				verPart = verPart2;
				AvatarService.gI().getAvatarPart();
				playing++;
			}
			else if (verPart != verPart2)
			{
				verPart = verPart2;
				AvatarService.gI().getAvatarPart();
				playing++;
			}
			else
			{
				setFollowAvatarPart();
			}
			if (!loadItemInfo())
			{
				verItemImg = verBigItemImg;
				AvatarService.gI().getItemInfo();
				playing++;
			}
			else if (verItemImg != verBigItemImg)
			{
				verItemImg = verBigItemImg;
				AvatarService.gI().getItemInfo();
				playing++;
			}
			if (!loadMapItemType())
			{
				verItemType = vItemType;
				AvatarService.gI().getMapItemType();
				playing++;
			}
			else if (verItemType != vItemType)
			{
				verItemType = vItemType;
				AvatarService.gI().getMapItemType();
				playing++;
			}
			if (!loadMapItem())
			{
				verItem = vItem;
				AvatarService.gI().getMapItem();
				playing++;
			}
			else if (verItem != vItem)
			{
				verItem = vItem;
				AvatarService.gI().getMapItem();
				playing++;
			}
			setPlaying();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public static void addDataBig(BigImgInfo big)
	{
		playing--;
		int num = AvatarData.bigImgInfo.size();
		for (int i = 0; i < num; i++)
		{
			BigImgInfo bigImgInfo = (BigImgInfo)AvatarData.bigImgInfo.elementAt(i);
			if (bigImgInfo.id == big.id)
			{
				bigImgInfo.data = big.data;
				bigImgInfo.ver = big.ver;
				bigImgInfo.follow = big.follow;
				break;
			}
		}
		setPlaying();
	}

	public static void saveImgBig()
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeShort((short)AvatarData.bigImgInfo.size());
			for (int i = 0; i < AvatarData.bigImgInfo.size(); i++)
			{
				BigImgInfo bigImgInfo = (BigImgInfo)AvatarData.bigImgInfo.elementAt(i);
				dataOutputStream.writeShort(bigImgInfo.id);
				dataOutputStream.writeShort(bigImgInfo.follow);
				dataOutputStream.writeInt(bigImgInfo.data.Length);
				dataOutputStream.write(bigImgInfo.data);
				dataOutputStream.writeShort(bigImgInfo.ver);
			}
			RMS.saveRMS(sImgBig, dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public static bool loadImgBig()
	{
		DataInputStream dataInputStream = initLoad(sImgBig);
		if (dataInputStream == null)
		{
			return false;
		}
		try
		{
			int num = dataInputStream.readShort();
			AvatarData.bigImgInfo = new MyVector();
			for (int i = 0; i < num; i++)
			{
				BigImgInfo bigImgInfo = new BigImgInfo();
				bigImgInfo.id = dataInputStream.readShort();
				bigImgInfo.follow = dataInputStream.readShort();
				int num2 = dataInputStream.readInt();
				bigImgInfo.data = new sbyte[num2];
				dataInputStream.read(ref bigImgInfo.data);
				bigImgInfo.ver = dataInputStream.readShort();
				AvatarData.bigImgInfo.addElement(bigImgInfo);
			}
			dataInputStream.close();
		}
		catch (Exception)
		{
			delErrorRms(sImgBig);
		}
		return true;
	}

	private static Part[] setArrayPart(MyVector list)
	{
		int num = 0;
		for (int i = 0; i < list.size(); i++)
		{
			Part part = (Part)list.elementAt(i);
			if (part.IDPart > num)
			{
				num = part.IDPart;
			}
		}
		Part[] array = new Part[num + 1];
		for (int j = 0; j < list.size(); j++)
		{
			Part part2 = (Part)list.elementAt(j);
			array[part2.IDPart] = part2;
		}
		return array;
	}

	public static MyVector readAvatarPart(sbyte[] array, bool isSimple)
	{
		DataInputStream dataInputStream = new DataInputStream(array);
		short num = 1;
		if (!isSimple)
		{
			num = dataInputStream.readShort();
		}
		MyVector myVector = new MyVector();
		for (int i = 0; i < num; i++)
		{
			short iDPart = dataInputStream.readShort();
			int num2 = dataInputStream.readInt();
			int num3 = dataInputStream.readShort();
			short num4 = dataInputStream.readShort();
			switch (num4)
			{
			case -2:
			{
				PartSmall partSmall = new PartSmall();
				partSmall.IDPart = iDPart;
				partSmall.price[0] = num2;
				partSmall.price[1] = num3;
				partSmall.follow = num4;
				partSmall.name = dataInputStream.readUTF();
				partSmall.sell = dataInputStream.readByte();
				partSmall.idIcon = dataInputStream.readShort();
				myVector.addElement(partSmall);
				break;
			}
			case -1:
			{
				APartInfo aPartInfo = new APartInfo();
				aPartInfo.IDPart = iDPart;
				aPartInfo.price[0] = num2;
				aPartInfo.price[1] = num3;
				aPartInfo.follow = num4;
				aPartInfo.name = dataInputStream.readUTF();
				aPartInfo.sell = dataInputStream.readByte();
				aPartInfo.zOrder = dataInputStream.readByte();
				aPartInfo.gender = dataInputStream.readByte();
				aPartInfo.level = dataInputStream.readByte();
				aPartInfo.idIcon = dataInputStream.readShort();
				aPartInfo.imgID = new short[15];
				aPartInfo.dx = new sbyte[15];
				aPartInfo.dy = new sbyte[15];
				for (int j = 0; j < 15; j++)
				{
					aPartInfo.imgID[j] = dataInputStream.readShort();
					aPartInfo.dx[j] = dataInputStream.readByte();
					aPartInfo.dy[j] = dataInputStream.readByte();
				}
				myVector.addElement(aPartInfo);
				break;
			}
			default:
			{
				PartFollow partFollow = new PartFollow();
				partFollow.IDPart = iDPart;
				partFollow.price[0] = num2;
				partFollow.price[1] = num3;
				partFollow.follow = num4;
				partFollow.color = dataInputStream.readShort();
				myVector.addElement(partFollow);
				break;
			}
			}
		}
		return myVector;
	}

	public static void saveAvatarPart(sbyte[] arr)
	{
		playing--;
		listPart = setArrayPart(readAvatarPart(arr, false));
		RMS.saveRMS(sAvaPart, arr);
		setFollowAvatarPart();
		setPlaying();
	}

	public static bool loadAvatarPart()
	{
		sbyte[] array = RMS.loadRMS(sAvaPart);
		if (array == null)
		{
			return false;
		}
		listPart = setArrayPart(readAvatarPart(array, false));
		return true;
	}

	private static void saveVersion()
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeInt(verPart);
			dataOutputStream.writeInt(verItemType);
			dataOutputStream.writeInt(verImg);
			dataOutputStream.writeInt(verItemImg);
			dataOutputStream.writeInt(verItem);
			RMS.saveRMS(sVer, dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception)
		{
		}
	}

	private static void loadVersion()
	{
		sbyte[] array = RMS.loadRMS(sVer);
		if (array != null)
		{
			DataInputStream dataInputStream = new DataInputStream(array);
			verPart = dataInputStream.readInt();
			verItemType = dataInputStream.readInt();
			verImg = dataInputStream.readInt();
			verItemImg = dataInputStream.readInt();
			verItem = dataInputStream.readInt();
		}
	}

	public static void setFollowAvatarPart()
	{
		for (int i = 0; i < listPart.Length; i++)
		{
			if (listPart[i].follow >= 0)
			{
				Part part = listPart[listPart[i].follow];
				Part part2 = listPart[i];
				part2.name = part.name;
				part2.sell = part.sell;
				part2.zOrder = part.zOrder;
				part2.idIcon = part.idIcon;
			}
		}
	}

	private static void readItemDataInfo(sbyte[] arr)
	{
		DataInputStream dataInputStream = new DataInputStream(arr);
		short num = dataInputStream.readShort();
		listItemInfo = new MyVector();
		for (int i = 0; i < num; i++)
		{
			Item item = new Item();
			item.ID = dataInputStream.readShort();
			item.name = dataInputStream.readUTF();
			item.des = dataInputStream.readUTF();
			item.price[0] = dataInputStream.readInt();
			item.shopType = dataInputStream.readByte();
			item.idIcon = dataInputStream.readShort();
			listItemInfo.addElement(item);
		}
	}

	public static void saveItemData(sbyte[] arr)
	{
		playing--;
		readItemDataInfo(arr);
		RMS.saveRMS(sItemInfo, arr);
		setPlaying();
	}

	public static bool loadItemInfo()
	{
		sbyte[] array = RMS.loadRMS(sItemInfo);
		if (array == null)
		{
			return false;
		}
		readItemDataInfo(array);
		return true;
	}

	private static ImageInfo[] readImageData(sbyte[] arr)
	{
		DataInputStream dataInputStream = new DataInputStream(arr);
		short num = dataInputStream.readShort();
		MyVector myVector = new MyVector();
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			ImageInfo imageInfo = new ImageInfo();
			imageInfo.ID = dataInputStream.readShort();
			if (imageInfo.ID > num2)
			{
				num2 = imageInfo.ID;
			}
			imageInfo.bigID = dataInputStream.readShort();
			imageInfo.x0 = (short)dataInputStream.readUnsignedByte();
			imageInfo.y0 = (short)dataInputStream.readUnsignedByte();
			imageInfo.w = dataInputStream.readByte();
			imageInfo.h = dataInputStream.readByte();
			myVector.addElement(imageInfo);
		}
		ImageInfo[] array = new ImageInfo[num2 + 1];
		for (int j = 0; j < myVector.size(); j++)
		{
			ImageInfo imageInfo2 = (ImageInfo)myVector.elementAt(j);
			array[imageInfo2.ID] = imageInfo2;
		}
		return array;
	}

	public static void saveImageData(sbyte[] arr)
	{
		playing--;
		listImgInfo = readImageData(arr);
		RMS.saveRMS(sImage, arr);
		setPlaying();
	}

	public static bool loadImageData()
	{
		sbyte[] array = RMS.loadRMS(sImage);
		if (array == null)
		{
			return false;
		}
		listImgInfo = readImageData(array);
		return true;
	}

	private static MyVector readMapItemType(sbyte[] arr)
	{
		DataInputStream dataInputStream = new DataInputStream(arr);
		short num = dataInputStream.readShort();
		MyVector myVector = new MyVector();
		for (sbyte b = 0; b < num; b++)
		{
			MapItemType mapItemType = new MapItemType();
			mapItemType.idType = dataInputStream.readShort();
			mapItemType.name = dataInputStream.readUTF();
			mapItemType.des = dataInputStream.readUTF();
			mapItemType.imgID = dataInputStream.readShort();
			mapItemType.iconID = dataInputStream.readShort();
			mapItemType.dx = dataInputStream.readByte();
			mapItemType.dy = dataInputStream.readByte();
			mapItemType.priceXu = dataInputStream.readShort();
			if (mapItemType.priceXu == 32767)
			{
				mapItemType.priceXu = -1;
			}
			if (mapItemType.priceXu > -1)
			{
				mapItemType.priceXu *= 1000;
			}
			mapItemType.priceLuong = dataInputStream.readShort();
			mapItemType.buy = dataInputStream.readByte();
			mapItemType.listNotTrans = new MyVector();
			sbyte b2 = dataInputStream.readByte();
			for (sbyte b3 = 0; b3 < b2; b3++)
			{
				AvPosition avPosition = new AvPosition();
				avPosition.x = dataInputStream.readByte();
				avPosition.y = dataInputStream.readByte();
				mapItemType.listNotTrans.addElement(avPosition);
			}
			myVector.addElement(mapItemType);
		}
		return myVector;
	}

	public static void saveMapItemType(sbyte[] arr)
	{
		playing--;
		listMapItemType.removeAllElements();
		listMapItemType = readMapItemType(arr);
		RMS.saveRMS(sMapItemType, arr);
		setPlaying();
	}

	public static bool loadMapItemType()
	{
		sbyte[] array = RMS.loadRMS(sMapItemType);
		if (array == null)
		{
			return false;
		}
		listMapItemType = readMapItemType(array);
		return true;
	}

	public static void loadMyAccount()
	{
		sbyte[] array = RMS.loadRMS("my_account");
		if (array != null)
		{
			DataInputStream dataInputStream = new DataInputStream(array);
			Canvas.user = dataInputStream.readUTF();
			Canvas.pass = dataInputStream.readUTF();
			ServerListScr.selected = dataInputStream.readByte();
		}
	}

	public static void saveMyAccount()
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeUTF(Canvas.user);
			dataOutputStream.writeUTF(Canvas.pass);
			dataOutputStream.writeByte(ServerListScr.selected);
			RMS.saveRMS("my_account", dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private static void readMapItem(sbyte[] arr)
	{
		DataInputStream dataInputStream = new DataInputStream(arr);
		listMapItem = new MyVector();
		short num = dataInputStream.readShort();
		for (byte b = 0; b < num; b++)
		{
			MapItem mapItem = new MapItem();
			mapItem.ID = dataInputStream.readShort();
			mapItem.typeID = dataInputStream.readShort();
			mapItem.type = dataInputStream.readByte();
			mapItem.x = dataInputStream.readByte();
			mapItem.y = dataInputStream.readByte();
			listMapItem.addElement(mapItem);
		}
	}

	public static void saveMapItem(sbyte[] arr)
	{
		playing--;
		listMapItem.removeAllElements();
		readMapItem(arr);
		RMS.saveRMS(sMapType, arr);
		setPlaying();
	}

	public static bool loadMapItem()
	{
		sbyte[] array = RMS.loadRMS(sMapType);
		if (array == null)
		{
			return false;
		}
		readMapItem(array);
		return true;
	}

	public static void setPlaying()
	{
		if (playing != 0)
		{
			return;
		}
		saveVersion();
		saveImgBig();
		int num = AvatarData.bigImgInfo.size();
		for (int i = 0; i < num; i++)
		{
			BigImgInfo bigImgInfo = (BigImgInfo)AvatarData.bigImgInfo.elementAt(i);
			if (bigImgInfo.follow != -1)
			{
				sbyte[] data = getBigImgInfoList(bigImgInfo.follow).data;
				Array.Copy(bigImgInfo.data, 0, data, 0, bigImgInfo.data.Length);
				bigImgInfo.data = data;
			}
			bigImgInfo.img = CRes.createImgByByteArray(ArrayCast.cast(bigImgInfo.data));
		}
		for (int j = 0; j < AvatarData.bigImgInfo.size(); j++)
		{
			BigImgInfo bigImgInfo2 = (BigImgInfo)AvatarData.bigImgInfo.elementAt(j);
			bigImgInfo2.data = null;
			listBigImg.put(string.Empty + bigImgInfo2.id, bigImgInfo2);
		}
		AvatarData.bigImgInfo.removeAllElements();
		AvatarData.bigImgInfo = null;
		GameMidlet.avatar.orderSeriesPath();
		MapScr.gI().joinCitymap();
	}

	private static void setBigImgBB(BigImgInfo big)
	{
		Image image = new Image();
		image.w = big.img.getWidth();
		image.h = big.img.getHeight();
		image.texture = new Texture2D(image.w, image.h);
		for (int i = 0; i < listImgInfo.Length; i++)
		{
			if (big.id != listImgInfo[i].bigID)
			{
				continue;
			}
			Color[] pixels = getBigImgInfo(big.id).img.texture.GetPixels(listImgInfo[i].x0 * AvMain.hd, listImgInfo[i].y0 * AvMain.hd, listImgInfo[i].w * AvMain.hd, listImgInfo[i].h * AvMain.hd);
			int num = listImgInfo[i].w * AvMain.hd;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < listImgInfo[i].h * AvMain.hd; k++)
				{
					Color color = new Color(pixels[k * num + j].a, pixels[k * num + j].b, pixels[k * num + j].g);
					pixels[k * num + j] = new Color(pixels[num - j].a, pixels[num - j].b, pixels[num - j].g);
					pixels[num - j] = new Color(color.a, color.b, color.g);
					pixels[k * num + j] = new Color(0f, 0f, 0f, 0f);
				}
			}
			image.texture.SetPixels(listImgInfo[i].x0 * AvMain.hd, listImgInfo[i].y0 * AvMain.hd, listImgInfo[i].w * AvMain.hd, listImgInfo[i].h * AvMain.hd, pixels);
		}
		for (int l = 0; l < listPart.Length; l++)
		{
			if (listPart[l].follow <= -1)
			{
				continue;
			}
			APartInfo aPartInfo = (APartInfo)getPart(listPart[l].follow);
			for (int m = 0; m < aPartInfo.imgID.Length; m++)
			{
				ImageInfo imageInfo = listImgInfo[aPartInfo.imgID[m]];
				if (((PartFollow)listPart[l]).color == big.id)
				{
					Color[] pixels2 = getBigImgInfo(big.id).img.texture.GetPixels(imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd);
					image.texture.SetPixels(imageInfo.x0 * AvMain.hd, imageInfo.y0 * AvMain.hd, imageInfo.w * AvMain.hd, imageInfo.h * AvMain.hd, pixels2);
				}
			}
		}
		image.texture.Apply(false, false);
		BigImgInfo bigImgInfo = new BigImgInfo();
		bigImgInfo.follow = big.follow;
		bigImgInfo.id = big.id;
		bigImgInfo.img = image;
		bigImgInfo.ver = big.ver;
		listBigImgBB.put(string.Empty + bigImgInfo.id, bigImgInfo);
	}

	public static BigImgInfo getBigImgInfoBB(int id)
	{
		return (BigImgInfo)listBigImgBB.get(string.Empty + id);
	}

	public static void paintPart(MyGraphics g, int bigID, int x0, int y0, int w, int h, int x, int y, int direct, int arthor)
	{
		g.drawRegion(getBigImgInfo(bigID).img, x0 * AvMain.hd, y0 * AvMain.hd, w * AvMain.hd, h * AvMain.hd, direct, x, y, arthor);
	}

	public static DataInputStream initLoad(string name)
	{
		sbyte[] array = RMS.loadRMS(name);
		if (array == null)
		{
			return null;
		}
		return new DataInputStream(array);
	}

	public static void saveIP()
	{
	}

	public static void loadIP()
	{
	}

	public static BigImgInfo getBigImgInfoList(int id)
	{
		int num = AvatarData.bigImgInfo.size();
		for (int i = 0; i < num; i++)
		{
			BigImgInfo bigImgInfo = (BigImgInfo)AvatarData.bigImgInfo.elementAt(i);
			if (bigImgInfo.id == id)
			{
				return bigImgInfo;
			}
		}
		return null;
	}

	public static BigImgInfo getBigImgInfo(int id)
	{
		return (BigImgInfo)listBigImg.get(string.Empty + id);
	}

	public static MapItemType getMapItemTypeByID(int idType)
	{
		int num = listMapItemType.size();
		for (int i = 0; i < num; i++)
		{
			if (((MapItemType)listMapItemType.elementAt(i)).idType == idType)
			{
				return (MapItemType)listMapItemType.elementAt(i);
			}
		}
		return null;
	}

	public static void onMapAd(MyVector listAd)
	{
		AvatarData.listAd = listAd;
	}

	public static bool isZOrderMain(int zOrder)
	{
		if (zOrder != 10 && zOrder != 20 && zOrder != 30 && zOrder != 40 && zOrder != 50)
		{
			return false;
		}
		return true;
	}

	public static APartInfo getPartByZ(MyVector seri, int z)
	{
		if (seri != null)
		{
			for (int i = 0; i < seri.size(); i++)
			{
				SeriPart seriPart = (SeriPart)seri.elementAt(i);
				Part part = getPart(seriPart.idPart);
				if (seriPart != null && part is APartInfo && ((APartInfo)part).zOrder == z)
				{
					return (APartInfo)part;
				}
			}
		}
		return null;
	}

	public static SeriPart getSeriByIdPart(MyVector listSeri, int idPart)
	{
		int num = listSeri.size();
		for (int i = 0; i < num; i++)
		{
			SeriPart seriPart = (SeriPart)listSeri.elementAt(i);
			if (seriPart.idPart == idPart)
			{
				return seriPart;
			}
		}
		return null;
	}

	public static SeriPart getSeriByZ(int zOrder, MyVector listSeri)
	{
		int num = listSeri.size();
		for (int i = 0; i < num; i++)
		{
			SeriPart seriPart = (SeriPart)listSeri.elementAt(i);
			Part part = getPart(seriPart.idPart);
			if (part.zOrder == zOrder)
			{
				return seriPart;
			}
		}
		return null;
	}

	public static Part getPartDinamic(short idPart)
	{
		Part part = (Part)listPartDynamic.get(string.Empty + idPart);
		if (part == null)
		{
			part = new APartInfo();
			part.IDPart = -1;
			listPartDynamic.put(string.Empty + idPart, part);
			GlobalService.gI().requestPartDynaMic(idPart);
		}
		return part;
	}

	public static Part getPart(short idPart)
	{
		if (idPart >= 2000)
		{
			return getPartDinamic(idPart);
		}
		return listPart[idPart];
	}

	public static string getName(Part part)
	{
		if (part.follow >= 0)
		{
			return getPart(part.follow).name;
		}
		return part.name;
	}

	public static void paintImg(MyGraphics g, int id, int x, int y, int anthor)
	{
		if (getImgIcon((short)id).count != -1)
		{
			g.drawImage(getImgIcon((short)id).img, x, y, anthor);
		}
	}

	public static ImageIcon getImagePart(short id)
	{
		ImageIcon imageIcon = (ImageIcon)listImgPart.get(string.Empty + id);
		if (imageIcon == null)
		{
			imageIcon = new ImageIcon();
			listImgPart.put(string.Empty + id, imageIcon);
			GlobalService.gI().requestImagePart(id);
		}
		else if (imageIcon.count >= 0)
		{
			imageIcon.count = Environment.TickCount / 1000;
		}
		return imageIcon;
	}

	public static ImageIcon getImgIcon(short id)
	{
		ImageIcon imageIcon = (ImageIcon)listImgIcon.get(string.Empty + id);
		if (imageIcon == null)
		{
			imageIcon = new ImageIcon();
			listImgIcon.put(string.Empty + id, imageIcon);
			AvatarService.gI().doGetImgIcon(id);
		}
		else if (imageIcon.count >= 0)
		{
			imageIcon.count = Environment.TickCount / 1000;
		}
		return imageIcon;
	}

	public static void setLimitImage()
	{
		try
		{
			if (listImgIcon.size() > 5)
			{
				foreach (DictionaryEntry item in listImgIcon)
				{
					ImageIcon imageIcon = (ImageIcon)item.Value;
					if (imageIcon.count != -1 && Canvas.getTick() / 1000 - imageIcon.count > 200)
					{
						string key = (string)item.Key;
						listImgIcon.h.Remove(key);
						return;
					}
				}
			}
			if (listImgPart.size() <= 5)
			{
				return;
			}
			foreach (DictionaryEntry item2 in listImgPart)
			{
				ImageIcon imageIcon2 = (ImageIcon)item2.Value;
				if (imageIcon2.count != -1 && Environment.TickCount / 1000 - imageIcon2.count > 200)
				{
					string k = (string)item2.Key;
					listImgPart.remove(k);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public static int getLevel(Part part)
	{
		int num = 0;
		if (part.follow >= 0)
		{
			return ((APartInfo)getPart(part.follow)).level;
		}
		return ((APartInfo)part).level;
	}

	public static EffectData getEffect(short id)
	{
		for (int i = 0; i < effectList.size(); i++)
		{
			EffectData effectData = (EffectData)effectList.elementAt(i);
			if (effectData.ID == id)
			{
				return effectData;
			}
		}
		return null;
	}

	public static void delErrorRms(string path)
	{
		try
		{
			PlayerPrefs.DeleteKey("2.5.8" + path);
		}
		catch (Exception)
		{
		}
	}
}
