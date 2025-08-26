public class DiamondMessageHandler : IMiniGameMsgHandler
{
	private static DiamondMessageHandler instance = new DiamondMessageHandler();

	public static void onHandler()
	{
		BoardScr.numPlayer = 2;
		BoardListOnScr.type = BoardListOnScr.STYLE_2PLAYER;
		if (onMainMenu.isOngame)
		{
			RoomListOnScr.setName(2, DiamondScr.gI());
		}
		else
		{
			RoomListOnScr.setName((Canvas.iOpenOngame == 0) ? 2 : 0, DiamondScr.gI());
		}
		CasinoMsgHandler.me.miniGameMessageHandler = instance;
	}

	public void onConnectOK()
	{
	}

	public void onConnectionFail()
	{
	}

	public void onDisconnected()
	{
	}

	public void onMessage(Message msg)
	{
		Out.println("msg: " + msg.command);
		sbyte b = -1;
		sbyte b2 = -1;
		b = msg.reader().readByte();
		b2 = msg.reader().readByte();
		if (!BoardScr.setR_B(b, b2))
		{
			return;
		}
		switch (msg.command)
		{
		case 20:
		{
			sbyte b3 = msg.reader().readByte();
			int whoMoveFirst = msg.reader().readInt();
			sbyte[][] array2 = new sbyte[8][];
			for (int l = 0; l < 8; l++)
			{
				array2[l] = new sbyte[8];
			}
			for (int m = 0; m < 8; m++)
			{
				for (int n = 0; n < 8; n++)
				{
					array2[m][n] = msg.reader().readByte();
				}
			}
			for (int num = 0; num < 2; num++)
			{
				int id = msg.reader().readInt();
				Avatar avatarByID = BoardScr.getAvatarByID(id);
				avatarByID.defence = msg.reader().readShort();
				avatarByID.plusHP = (avatarByID.plusMP = 0);
				avatarByID.hp = (avatarByID.maxHP = msg.reader().readShort());
				avatarByID.mp = msg.reader().readShort();
				avatarByID.maxMP = msg.reader().readShort();
				avatarByID.v *= 2;
				avatarByID.setFeel(4);
			}
			DiamondScr.gI().start(whoMoveFirst, b3, array2);
			break;
		}
		case 64:
		{
			sbyte b6 = msg.reader().readByte();
			sbyte[] array4 = new sbyte[b6];
			AvPosition[] array5 = new AvPosition[b6];
			for (int num7 = 0; num7 < b6; num7++)
			{
				array5[num7] = new AvPosition();
				array4[num7] = msg.reader().readByte();
				array5[num7].anchor = msg.reader().readByte();
				array5[num7].depth = msg.reader().readByte();
			}
			sbyte countCombo = msg.reader().readByte();
			sbyte b7 = msg.reader().readByte();
			MyVector myVector2 = new MyVector();
			for (int num8 = 0; num8 < b7; num8++)
			{
				string o = msg.reader().readUTF();
				myVector2.addElement(o);
			}
			for (int num9 = 0; num9 < 2; num9++)
			{
				int id3 = msg.reader().readInt();
				Avatar avatarByID3 = BoardScr.getAvatarByID(id3);
				avatarByID3.fight = msg.reader().readByte();
				avatarByID3.defence = msg.reader().readShort();
				avatarByID3.plusHP = (short)(msg.reader().readShort() - avatarByID3.hp);
				avatarByID3.plusMP = (short)(msg.reader().readShort() - avatarByID3.mp);
				avatarByID3.isNo = msg.reader().readBoolean();
				if (avatarByID3.isNo)
				{
					DiamondScr.gI().isNo = true;
				}
			}
			DiamondScr.gI().onCreateCell(array4, array5, countCombo, myVector2);
			break;
		}
		case 21:
		{
			int whoMove3 = msg.reader().readInt();
			sbyte b4 = msg.reader().readByte();
			sbyte b5 = msg.reader().readByte();
			DiamondScr.gI().move(whoMove3, b4, b5);
			break;
		}
		case 49:
		{
			int whoMove2 = msg.reader().readInt();
			DiamondScr.gI().onSkip(whoMove2);
			break;
		}
		case 24:
		{
			int whoMove = msg.reader().readInt();
			sbyte[][] array3 = new sbyte[8][];
			for (int num2 = 0; num2 < 8; num2++)
			{
				array3[num2] = new sbyte[8];
			}
			for (int num3 = 0; num3 < 8; num3++)
			{
				for (int num4 = 0; num4 < 8; num4++)
				{
					array3[num3][num4] = msg.reader().readByte();
				}
			}
			DiamondScr.gI().onOutPath(whoMove, array3);
			break;
		}
		case 51:
		{
			MyVector myVector = new MyVector();
			for (int num5 = 0; num5 < 2; num5++)
			{
				int id2 = msg.reader().readInt();
				int num6 = msg.reader().readInt();
				Avatar avatarByID2 = BoardScr.getAvatarByID(id2);
				avatarByID2.v /= 2;
				avatarByID2.action = 0;
				avatarByID2.setMoneyNew(avatarByID2.getMoneyNew() + num6);
				if (num6 != 0)
				{
					Canvas.addFlyText(num6, avatarByID2.x, avatarByID2.y, -1, 30);
					string text = avatarByID2.name + ": ";
					if (num6 > 0)
					{
						DiamondScr.gI().idWin = avatarByID2.IDDB;
						string text2 = text;
						text = text2 + T.win + "   +" + num6 + T.xu;
					}
					else
					{
						string text2 = text;
						text = text2 + T.lose + "  " + num6 + T.xu;
					}
					myVector.addElement("  ");
					myVector.addElement(text);
				}
			}
			DiamondScr.gI().onFinish(myVector);
			break;
		}
		case 71:
		{
			sbyte[][] array = new sbyte[8][];
			for (int i = 0; i < 8; i++)
			{
				array[i] = new sbyte[8];
			}
			for (int j = 0; j < 8; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					array[j][k] = msg.reader().readByte();
				}
			}
			DiamondScr.gI().onData(array);
			break;
		}
		}
	}
}
