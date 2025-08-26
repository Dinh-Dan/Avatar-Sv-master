using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class FarmData
{
	public const string dataBigFarm = "avatarDataFarm";

	public const string imageData = "avatarImgFarm";

	public const string sTreeInfoFarm = "avatarTreeInfoFarm";

	public const string sImgBigFarm = "avatarImgBigFarm";

	public const string sVer = "avatarVSFarm";

	public static sbyte numImgBig;

	public static short[] versionBig;

	public static int verImg;

	public static int verPart;

	public static ImageInfo[] listImgInfo;

	public static TreeInfo[] treeInfo;

	public static Image[] imgBig;

	public static Item[] vatPhamInfo;

	public static MyVector listAnimalInfo = new MyVector();

	public static MyVector listItemFarm = new MyVector();

	public static MyVector listFood = new MyVector();

	public static int playing = -1;

	public static MyHashTable listImgIcon = new MyHashTable();

	public static void init()
	{
		playing = -1;
	}

	public static void checkDataFarm(sbyte number1, short[] id, short[] version, int verBigImg, int verPart2)
	{
		loadVersion();
		playing = 0;
		imgBig = new Image[number1];
		if (!loadDataBig())
		{
			numImgBig = number1;
			versionBig = version;
			verImg = -1;
			verPart = -1;
			for (int i = 0; i < number1; i++)
			{
				FarmService.gI().getBigImage((short)i);
				playing++;
			}
		}
		else if (numImgBig > 0)
		{
			for (int j = 0; j < numImgBig; j++)
			{
				sbyte[] data = loadImgBig(j);
				imgBig[j] = CRes.createImgByByteArray(ArrayCast.cast(data));
			}
		}
		for (int k = 0; k < numImgBig; k++)
		{
			if (version[k] != versionBig[k])
			{
				FarmService.gI().getBigImage((short)k);
				playing++;
			}
		}
		int num = number1 - numImgBig;
		if (num > 0)
		{
			short[] array = versionBig;
			versionBig = new short[version.Length];
			for (int l = 0; l < array.Length; l++)
			{
				versionBig[l] = array[l];
			}
			for (int m = numImgBig; m < number1; m++)
			{
				FarmService.gI().getBigImage((short)m);
				playing++;
			}
		}
		if (!loadImageData())
		{
			verImg = verBigImg;
			FarmService.gI().getImageData();
			playing++;
		}
		else if (verImg != verBigImg)
		{
			verImg = verBigImg;
			FarmService.gI().getImageData();
			playing++;
		}
		if (!loadTreeInfo())
		{
			verPart = verPart2;
			FarmService.gI().getTreeInfo();
			playing++;
		}
		else if (verPart != verPart2)
		{
			verPart = verPart2;
			FarmService.gI().getTreeInfo();
			playing++;
		}
		if (playing == 0)
		{
			FarmService.gI().getInventory();
		}
		CRes.rndaaa();
	}

	public static void addDataBig(short index, short version, sbyte[] data)
	{
		playing--;
		versionBig[index] = version;
		imgBig[index] = CRes.createImgByByteArray(ArrayCast.cast(data));
		saveImgBig(data, index);
		saveDataBig(numImgBig, versionBig, verImg, verPart);
		if (playing == 0)
		{
			FarmService.gI().getInventory();
		}
	}

	public static void saveImgBig(sbyte[] imgData, int index)
	{
		try
		{
			RMS.saveRMS("avatarImgBigFarm" + index, imgData);
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}

	public static sbyte[] loadImgBig(int index)
	{
		return RMS.loadRMS("avatarImgBigFarm" + index);
	}

	public static void saveVersion()
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeInt(verPart);
			dataOutputStream.writeInt(verImg);
			RMS.saveRMS("avatarVSFarm", dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
	}

	private static void loadVersion()
	{
		sbyte[] array = RMS.loadRMS("avatarVSFarm");
		if (array == null)
		{
			return;
		}
		DataInputStream dataInputStream = new DataInputStream(array);
		try
		{
			verPart = dataInputStream.readInt();
			verImg = dataInputStream.readInt();
		}
		catch (IOException ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
	}

	private static void readTreeInfo(sbyte[] arr)
	{
		DataInputStream dataInputStream = new DataInputStream(arr);
		short num = dataInputStream.readShort();
		TreeInfo[] array = new TreeInfo[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new TreeInfo();
			array[i].ID = dataInputStream.readByte();
			array[i].name = dataInputStream.readUTF();
			array[i].name1 = array[i].name.ToLower();
			array[i].Phase = new sbyte[2];
			array[i].Phase[0] = dataInputStream.readByte();
			array[i].Phase[1] = dataInputStream.readByte();
			array[i].harvestTime = dataInputStream.readShort();
			array[i].dieTime = dataInputStream.readShort();
			array[i].priceSeed[0] = dataInputStream.readShort();
			array[i].priceProduct = dataInputStream.readShort();
			array[i].numProduct = dataInputStream.readShort();
			array[i].idImg = new short[8];
			for (int j = 0; j < array[i].idImg.Length; j++)
			{
				array[i].idImg[j] = dataInputStream.readShort();
			}
		}
		short num2 = dataInputStream.readShort();
		vatPhamInfo = new Item[num2];
		for (int k = 0; k < num2; k++)
		{
			vatPhamInfo[k] = new Item();
			vatPhamInfo[k].ID = dataInputStream.readByte();
			vatPhamInfo[k].price[0] = dataInputStream.readShort();
		}
		for (int l = 0; l < num; l++)
		{
			array[l].priceSeed[1] = dataInputStream.readShort();
		}
		for (int m = 0; m < num2; m++)
		{
			vatPhamInfo[m].price[1] = dataInputStream.readShort();
		}
		short num3 = dataInputStream.readShort();
		listAnimalInfo = new MyVector();
		for (int n = 0; n < num3; n++)
		{
			AnimalInfo animalInfo = new AnimalInfo();
			animalInfo.species = dataInputStream.readByte();
			animalInfo.name = dataInputStream.readUTF();
			animalInfo.des = dataInputStream.readUTF();
			animalInfo.price[0] = dataInputStream.readInt();
			animalInfo.price[1] = dataInputStream.readShort();
			animalInfo.harvestTime = dataInputStream.readShort();
			animalInfo.priceProduct = dataInputStream.readShort();
			for (int num4 = 0; num4 < 3; num4++)
			{
				animalInfo.idImg[num4] = dataInputStream.readShort();
			}
			animalInfo.frame = dataInputStream.readByte();
			for (int num5 = 0; num5 < 3; num5++)
			{
				for (int num6 = 0; num6 < 12; num6++)
				{
					animalInfo.arrFrame[num5][num6] = dataInputStream.readByte();
				}
			}
			animalInfo.area = dataInputStream.readByte();
			animalInfo.iconID = dataInputStream.readShort();
			animalInfo.iconProduct = dataInputStream.readShort();
			animalInfo.iconO = dataInputStream.readShort();
			listAnimalInfo.addElement(animalInfo);
		}
		listItemFarm = new MyVector();
		sbyte b = dataInputStream.readByte();
		for (int num7 = 0; num7 < b; num7++)
		{
			FarmItem farmItem = new FarmItem();
			farmItem.isItem = true;
			farmItem.ID = dataInputStream.readShort();
			farmItem.IDImg = dataInputStream.readShort();
			farmItem.type = dataInputStream.readByte();
			farmItem.action = dataInputStream.readByte();
			farmItem.des = dataInputStream.readUTF();
			farmItem.priceXu = dataInputStream.readShort();
			farmItem.priceLuong = dataInputStream.readShort();
			listItemFarm.addElement(farmItem);
		}
		sbyte b2 = dataInputStream.readByte();
		for (int num8 = 0; num8 < b2; num8++)
		{
			FarmItem farmItem2 = new FarmItem();
			farmItem2.isItem = false;
			farmItem2.ID = dataInputStream.readShort();
			farmItem2.IDImg = dataInputStream.readShort();
			farmItem2.des = dataInputStream.readUTF();
			farmItem2.priceXu = dataInputStream.readShort();
			farmItem2.priceLuong = dataInputStream.readShort();
		}
		short num9 = dataInputStream.readByte();
		TreeInfo[] array2 = new TreeInfo[num9];
		for (int num10 = 0; num10 < num9; num10++)
		{
			array2[num10] = new TreeInfo();
			array2[num10].isDynamic = true;
			array2[num10].ID = dataInputStream.readShort();
			array2[num10].name = dataInputStream.readUTF();
			array2[num10].name1 = array2[num10].name.ToLower();
			array2[num10].harvestTime = dataInputStream.readShort();
			array2[num10].priceSeed[0] = dataInputStream.readShort();
			array2[num10].priceSeed[1] = dataInputStream.readShort();
			array2[num10].productID = dataInputStream.readShort();
			array2[num10].numProduct = dataInputStream.readShort();
			array2[num10].lv = dataInputStream.readByte();
			array2[num10].idImg = new short[8];
			for (int num11 = 0; num11 < array2[num10].idImg.Length; num11++)
			{
				array2[num10].idImg[num11] = dataInputStream.readShort();
			}
		}
		short num12 = dataInputStream.readShort();
		for (int num13 = 0; num13 < num12; num13++)
		{
			Food food = new Food();
			food.ID = dataInputStream.readShort();
			food.text = dataInputStream.readUTF();
			food.productID = dataInputStream.readShort();
			food.cookTime = dataInputStream.readShort();
			short num14 = dataInputStream.readShort();
			food.material = new short[num14];
			food.numberMaterial = new short[num14];
			for (int num15 = 0; num15 < num14; num15++)
			{
				food.material[num15] = dataInputStream.readShort();
				food.numberMaterial[num15] = dataInputStream.readShort();
			}
			listFood.addElement(food);
		}
		sbyte b3 = dataInputStream.readByte();
		for (int num16 = 0; num16 < b3; num16++)
		{
			FarmItem farmItem3 = new FarmItem();
			farmItem3.isItem = false;
			farmItem3.ID = dataInputStream.readShort();
			farmItem3.IDImg = dataInputStream.readShort();
			farmItem3.des = dataInputStream.readUTF();
			farmItem3.priceXu = dataInputStream.readInt();
			farmItem3.priceLuong = dataInputStream.readInt();
			listItemFarm.addElement(farmItem3);
		}
		treeInfo = new TreeInfo[num + num9];
		for (int num17 = 0; num17 < num; num17++)
		{
			treeInfo[num17] = array[num17];
		}
		for (int num18 = num; num18 < num9 + num; num18++)
		{
			treeInfo[num18] = array2[num18 - num];
		}
	}

	public static void saveTreeInfo(sbyte[] arr)
	{
		playing--;
		readTreeInfo(arr);
		RMS.saveRMS("avatarTreeInfoFarm", arr);
		if (playing == 0)
		{
			FarmService.gI().getInventory();
		}
	}

	public static bool loadTreeInfo()
	{
		sbyte[] array = RMS.loadRMS("avatarTreeInfoFarm");
		if (array == null)
		{
			return false;
		}
		try
		{
			readTreeInfo(array);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
		return true;
	}

	private static void readImageData(sbyte[] arr)
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
			imageInfo.x0 = dataInputStream.readByte();
			imageInfo.y0 = dataInputStream.readByte();
			imageInfo.w = dataInputStream.readByte();
			imageInfo.h = dataInputStream.readByte();
			myVector.addElement(imageInfo);
		}
		listImgInfo = new ImageInfo[num2 + 1];
		for (int j = 0; j < num; j++)
		{
			ImageInfo imageInfo2 = (ImageInfo)myVector.elementAt(j);
			listImgInfo[imageInfo2.ID] = imageInfo2;
		}
	}

	public static void saveImageData(sbyte[] arr)
	{
		playing--;
		readImageData(arr);
		RMS.saveRMS("avatarImgFarm", arr);
		if (playing == 0)
		{
			FarmService.gI().getInventory();
		}
	}

	public static bool loadImageData()
	{
		sbyte[] array = RMS.loadRMS("avatarImgFarm");
		if (array == null)
		{
			return false;
		}
		try
		{
			readImageData(array);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
		return true;
	}

	public static void saveDataBig(sbyte number1, short[] version, int verImg, int verPart)
	{
		DataOutputStream dataOutputStream = new DataOutputStream();
		try
		{
			dataOutputStream.writeByte(number1);
			dataOutputStream.writeInt(verImg);
			dataOutputStream.writeInt(verPart);
			for (int i = 0; i < number1; i++)
			{
				dataOutputStream.writeShort(version[i]);
			}
			sbyte[] data = dataOutputStream.toByteArray();
			try
			{
				RMS.saveRMS("avatarDataFarm", data);
			}
			catch (Exception e)
			{
				Out.logError(e);
			}
			dataOutputStream.close();
		}
		catch (IOException e2)
		{
			Out.logError(e2);
		}
	}

	public static bool loadDataBig()
	{
		DataInputStream dataInputStream = AvatarData.initLoad("avatarDataFarm");
		if (dataInputStream == null)
		{
			return false;
		}
		try
		{
			numImgBig = dataInputStream.readByte();
			verImg = dataInputStream.readInt();
			verPart = dataInputStream.readInt();
			versionBig = new short[numImgBig];
			for (int i = 0; i < numImgBig; i++)
			{
				versionBig[i] = dataInputStream.readShort();
			}
			dataInputStream.close();
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		return true;
	}

	public static Item getVPbyID(int id)
	{
		for (int i = 0; i < vatPhamInfo.Length; i++)
		{
			if (vatPhamInfo[i].ID == id)
			{
				return vatPhamInfo[i];
			}
		}
		return null;
	}

	public static TreeInfo getTreeByID(int id)
	{
		for (int i = 0; i < treeInfo.Length; i++)
		{
			if (id == treeInfo[i].ID)
			{
				return treeInfo[i];
			}
		}
		return null;
	}

	public static AnimalInfo getAnimalByID(int id)
	{
		int num = listAnimalInfo.size();
		for (int i = 0; i < num; i++)
		{
			AnimalInfo animalInfo = (AnimalInfo)listAnimalInfo.elementAt(i);
			if (animalInfo.species == id)
			{
				return animalInfo;
			}
		}
		return null;
	}

	public static TreeInfo getTreeInfoByID(int id)
	{
		for (int i = 0; i < treeInfo.Length; i++)
		{
			if (treeInfo[i].ID == id)
			{
				return treeInfo[i];
			}
		}
		return null;
	}

	public static void paintImg(MyGraphics g, int id, int x, int y, int anthor)
	{
		if (getImgIcon((short)id).count != -1)
		{
			g.drawImage(getImgIcon((short)id).img, x, y, anthor);
		}
	}

	public static ImageIcon getImgIcon(short id)
	{
		ImageIcon imageIcon = (ImageIcon)listImgIcon.get(string.Empty + id);
		if (imageIcon == null)
		{
			imageIcon = new ImageIcon();
			listImgIcon.put(string.Empty + id, imageIcon);
			FarmService.gI().doGetImgIcon(id);
		}
		else if (imageIcon.count >= 0)
		{
			imageIcon.count = Environment.TickCount / 1000;
		}
		return imageIcon;
	}

	public static void setLimitImage()
	{
		if (listImgIcon.size() <= 50)
		{
			return;
		}
		foreach (DictionaryEntry item in listImgIcon)
		{
			ImageIcon imageIcon = (ImageIcon)item.Value;
			if (imageIcon.count != -1 && Canvas.getTick() / 1000 - imageIcon.count > 200)
			{
				string key = (string)item.Key;
				listImgIcon.h.Remove(key);
				break;
			}
		}
	}

	public static Food getFoodByID(short id)
	{
		for (int i = 0; i < listFood.size(); i++)
		{
			Food food = (Food)listFood.elementAt(i);
			if (food.ID == id)
			{
				return food;
			}
		}
		return null;
	}
}
