using System;

public class AvatarMsgHandler : IMiniGameMsgHandler
{
	private static AvatarMsgHandler instance = new AvatarMsgHandler();

	public static void onHandler()
	{
		GlobalMessageHandler.gI().miniGameMessageHandler = instance;
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case -11:
			{
				MyVector myVector = new MyVector();
				sbyte b = msg.reader().readByte();
				for (int j = 0; j < b; j++)
				{
					BigImgInfo bigImgInfo2 = new BigImgInfo();
					bigImgInfo2.id = msg.reader().readShort();
					bigImgInfo2.ver = msg.reader().readShort();
					myVector.addElement(bigImgInfo2);
				}
				int verBigImg = msg.reader().readShort();
				int verPart = msg.reader().readShort();
				int verBigItemImg = msg.reader().readShort();
				int vItemType = msg.reader().readShort();
				int vItem = msg.reader().readShort();
				sbyte b2 = msg.reader().readByte();
				for (int k = 0; k < b2; k++)
				{
					BigImgInfo bigImgInfo3 = new BigImgInfo();
					bigImgInfo3.id = msg.reader().readShort();
					bigImgInfo3.ver = msg.reader().readShort();
					myVector.addElement(bigImgInfo3);
				}
				int vObj = msg.reader().readInt();
				Canvas.avataData.checkDataAvatar(myVector, verBigImg, verPart, verBigItemImg, vItemType, vItem, vObj);
				break;
			}
			case -14:
			{
				BigImgInfo bigImgInfo = new BigImgInfo();
				bigImgInfo.id = msg.reader().readShort();
				bigImgInfo.ver = msg.reader().readShort();
				int num = msg.reader().readUnsignedShort();
				bigImgInfo.data = new sbyte[num];
				for (int i = 0; i < num; i++)
				{
					bigImgInfo.data[i] = msg.reader().readByte();
				}
				bigImgInfo.follow = -1;
				if (msg.reader().available() >= 2)
				{
					bigImgInfo.follow = msg.reader().readShort();
				}
				AvatarData.addDataBig(bigImgInfo);
				break;
			}
			case -15:
			{
				sbyte[] data5 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data5);
				AvatarData.saveImageData(data5);
				break;
			}
			case -16:
			{
				sbyte[] data4 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data4);
				AvatarData.saveAvatarPart(data4);
				break;
			}
			case -37:
			{
				sbyte[] data3 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data3);
				AvatarData.saveItemData(data3);
				break;
			}
			case -40:
			{
				sbyte[] data2 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data2);
				AvatarData.saveMapItemType(data2);
				break;
			}
			case -41:
			{
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().read(ref data);
				AvatarData.saveMapItem(data);
				break;
			}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
	}
}
