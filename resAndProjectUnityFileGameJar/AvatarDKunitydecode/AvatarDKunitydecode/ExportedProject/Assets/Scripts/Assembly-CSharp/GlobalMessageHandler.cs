using System;
using System.IO;

public class GlobalMessageHandler : IMessageHandler
{
	private class CommandFlower : Command
	{
		private short idImg;

		public CommandFlower(string caption, IAction ac, short idImg)
			: base(caption, ac)
		{
			this.idImg = idImg;
		}

		public override void paint(MyGraphics g, int x, int y)
		{
			AvatarData.paintImg(g, idImg, x, y, 3);
		}
	}

	private class IActionFlower : IAction
	{
		private sbyte ii;

		public IActionFlower(sbyte i)
		{
			ii = i;
		}

		public void perform()
		{
			GlobalService.gI().doFlowerLoveSelected(ii);
		}
	}

	private class IActionMiniMap : IAction
	{
		private sbyte idCityMap;

		public IActionMiniMap(sbyte id)
		{
			idCityMap = id;
		}

		public void perform()
		{
			PositionMap positionMap = (PositionMap)MiniMap.gI().listPos.elementAt(MiniMap.gI().selected);
			Canvas.startWaitDlg();
			MapScr.idSelectedMini = positionMap.id;
			MapScr.idCityMap = idCityMap;
			GlobalService.gI().doSelectedMiniMap(idCityMap, positionMap.id);
		}
	}

	public GlobalLogicHandler globalLogicHandler = new GlobalLogicHandler();

	public static GlobalMessageHandler me;

	public IMiniGameMsgHandler miniGameMessageHandler;

	public static GlobalMessageHandler gI()
	{
		if (me == null)
		{
			me = new GlobalMessageHandler();
		}
		return me;
	}

	public void onConnectOK()
	{
		globalLogicHandler.onConnectOK();
	}

	public void onConnectionFail()
	{
		globalLogicHandler.onConnectFail();
	}

	public void onDisconnected()
	{
		GlobalLogicHandler.onDisconnect();
	}

	public void onMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case -106:
			{
				sbyte b29 = msg.reader().readByte();
				Out.println("CREATE_CHAR_INFO: " + b29);
				if (b29 == 0)
				{
					Out.println("CREATE_CHAR_INFOaaaaaaaaa: " + msg.reader().available());
					bool isTrue = msg.reader().readBoolean();
					RegisterInfoScr.isTrue = isTrue;
					RegisterInfoScr.isCreate = true;
				}
				else if (b29 == 1)
				{
					RegisterInfoScr.gI().onCreate();
				}
				else if (b29 == 3)
				{
					Canvas.isPaint18 = true;
				}
				break;
			}
			case -62:
				Out.println("CHANGE_PASS");
				LoginScr.gI().tfPass.setText(msg.reader().readUTF());
				LoginScr.gI().saveLogin();
				break;
			case -12:
			{
				Out.println("LOGIN_NEW_GAME");
				string nameNewGame = msg.reader().readUTF();
				string passNewGame = msg.reader().readUTF();
				LoginScr.gI().onLoginNewGame(nameNewGame, passNewGame);
				break;
			}
			case -25:
			{
				sbyte b18 = msg.reader().readByte();
				string des = null;
				string name2 = null;
				string pass = null;
				if (b18 == 2)
				{
					name2 = msg.reader().readUTF();
					pass = msg.reader().readUTF();
				}
				else
				{
					des = msg.reader().readUTF();
				}
				MiniMap.gI().onRegisterByEmail(b18, des, name2, pass);
				break;
			}
			case -30:
			{
				string userName = msg.reader().readUTF();
				string pass2 = msg.reader().readUTF();
				RegisterScr.gI().onRegister(userName, pass2);
				return;
			}
			case -7:
				globalLogicHandler.onVersion(msg.reader().readUTF(), msg.reader().readUTF());
				return;
			case -9:
				globalLogicHandler.onServerMessage(msg.reader().readUTF());
				return;
			case -8:
				globalLogicHandler.onServerInfo(msg.reader().readUTF());
				return;
			case -10:
			{
				string error = msg.reader().readUTF();
				bool boo = false;
				if (msg.reader().available() > 0)
				{
					boo = msg.reader().readBoolean();
				}
				globalLogicHandler.onSetMoneyError(error, boo);
				return;
			}
			case -23:
			{
				MyVector myVector5 = new MyVector();
				int num28 = 0;
				while (msg.reader().available() > 0)
				{
					MoneyInfo moneyInfo = new MoneyInfo();
					moneyInfo.info = msg.reader().readUTF();
					moneyInfo.smsContent = msg.reader().readUTF();
					moneyInfo.smsTo = msg.reader().readUTF();
					moneyInfo.strID = msg.reader().readUTF();
					num28++;
					myVector5.addElement(moneyInfo);
				}
				globalLogicHandler.onMoneyInfo(myVector5);
				return;
			}
			case -21:
			{
				Avatar avatar6 = new Avatar();
				avatar6.IDDB = msg.reader().readInt();
				avatar6.name = msg.reader().readUTF();
				string text3 = msg.reader().readUTF();
				MapScr.gI().onRequestAddFriend(avatar6, text3);
				return;
			}
			case -19:
			{
				Avatar avatar5 = new Avatar();
				avatar5.IDDB = msg.reader().readInt();
				avatar5.name = msg.reader().readUTF();
				bool tr = msg.reader().readBoolean();
				string text2 = msg.reader().readUTF();
				MapScr.gI().onAddFriend(avatar5, tr, text2);
				return;
			}
			case -35:
			{
				bool isCreaCha = msg.reader().readBoolean();
				RegisterScr.gI().onCreaCharacter(isCreaCha);
				return;
			}
			case -36:
			{
				int idUser3 = msg.reader().readInt();
				int idPart = msg.reader().readShort();
				MapScr.gI().onRemoveItem(idUser3, idPart);
				return;
			}
			case -6:
			{
				int id = msg.reader().readInt();
				string name = msg.reader().readUTF();
				string info = msg.reader().readUTF();
				globalLogicHandler.onChatFrom(id, name, info);
				return;
			}
			case -22:
			{
				int id3 = msg.reader().readInt();
				Avatar avatar2 = LoadMap.getAvatar(id3);
				if (avatar2 == null)
				{
					Canvas.endDlg();
					return;
				}
				avatar2.indexP = new sbyte[5];
				avatar2.lvMain = msg.reader().readByte();
				avatar2.perLvMain = msg.reader().readByte();
				avatar2.indexP[Avatar.I_FRIENDLY] = msg.reader().readByte();
				avatar2.indexP[Avatar.I_CRAZY] = msg.reader().readByte();
				avatar2.indexP[Avatar.I_STYLISH] = msg.reader().readByte();
				avatar2.indexP[Avatar.I_HAPPY] = msg.reader().readByte();
				avatar2.indexP[Avatar.I_HUNGER] = msg.reader().readByte();
				Avatar avatar3 = null;
				int num22 = msg.reader().readInt();
				string sologan = string.Empty;
				string tenQuanHe = string.Empty;
				short idImage = 0;
				sbyte lv = 0;
				sbyte perLv = 0;
				short num23 = -1;
				string nameAction = string.Empty;
				if (num22 != -1)
				{
					avatar3 = new Avatar();
					avatar3.IDDB = num22;
					avatar3.setName(msg.reader().readUTF());
					sbyte b12 = msg.reader().readByte();
					for (int num24 = 0; num24 < b12; num24++)
					{
						avatar3.addSeri(new SeriPart(msg.reader().readShort()));
					}
					sologan = msg.reader().readUTF();
					idImage = msg.reader().readShort();
					lv = msg.reader().readByte();
					perLv = msg.reader().readByte();
					tenQuanHe = msg.reader().readUTF();
					num23 = msg.reader().readShort();
					if (num23 != -1)
					{
						nameAction = msg.reader().readUTF();
					}
				}
				if (msg.reader().available() > 0)
				{
					GameMidlet.avatar.lvMain = msg.reader().readShort();
				}
				if (MapScr.isOpenInfo)
				{
					MapScr.isOpenInfo = false;
					MapScr.gI().onInfoPlayer(avatar2, avatar3, sologan, idImage, lv, perLv, tenQuanHe, num23, nameAction);
				}
				return;
			}
			case 34:
			{
				int num38 = msg.reader().readInt();
				if (num38 != -1)
				{
					string text4 = msg.reader().readUTF();
					int num39 = msg.reader().readInt();
					msg.reader().readShort();
					int exp = msg.reader().readInt();
					int num40 = msg.reader().readInt();
					int num41 = msg.reader().readInt();
					int num42 = msg.reader().readInt();
					int num43 = msg.reader().readInt();
					Avatar avatar8 = new Avatar();
					avatar8.setExp(exp);
					string info3 = T.nameStr + text4 + ". " + T.moneyStr + num39 + "$. Level: " + avatar8.lvMain + "+" + avatar8.perLvMain + "%. " + T.win + ": " + num40 + ". " + T.lose + ": " + num41 + ". " + T.draw + ": " + num42 + ". " + T.give + ": " + num43;
					Canvas.startOKDlg(info3);
				}
				return;
			}
			case -42:
			{
				MyVector myVector2 = new MyVector();
				int num11 = msg.reader().readByte();
				for (int num12 = 0; num12 < num11; num12++)
				{
					ObjAd objAd = new ObjAd();
					objAd.id = msg.reader().readShort();
					objAd.title = msg.reader().readUTF();
					objAd.text = msg.reader().readUTF();
					objAd.url = msg.reader().readUTF();
					objAd.sms = msg.reader().readUTF();
					objAd.to = msg.reader().readUTF();
					objAd.listPoint = new MyVector();
					int num13 = msg.reader().readByte();
					for (int num14 = 0; num14 < num13; num14++)
					{
						AvPosition avPosition = new AvPosition();
						avPosition.anchor = msg.reader().readByte();
						avPosition.x = msg.reader().readByte();
						avPosition.y = msg.reader().readByte();
						objAd.listPoint.addElement(avPosition);
					}
					myVector2.addElement(objAd);
				}
				for (int num15 = 0; num15 < num11; num15++)
				{
					ObjAd objAd2 = (ObjAd)myVector2.elementAt(num15);
					objAd2.typeShop = msg.reader().readByte();
				}
				AvatarData.onMapAd(myVector2);
				return;
			}
			case -33:
			{
				int num16 = msg.reader().readInt();
				int num17 = msg.reader().readByte();
				if (num17 == 1)
				{
				}
				if (num17 == 5)
				{
					GameMidlet.avatar.setMoneyNew(GameMidlet.avatar.money[3] + num16);
					Canvas.addFlyTextSmall(num16 + "xeng", GameMidlet.avatar.x, GameMidlet.avatar.y, -1, 0, -1);
				}
				int xu = msg.reader().readInt();
				int luong = msg.reader().readInt();
				int luongK = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(xu, luong, luongK);
				return;
			}
			case -1:
				globalLogicHandler.doGetHandler(msg.reader().readByte());
				return;
			case -47:
			{
				MyVector myVector11 = new MyVector();
				short num67 = msg.reader().readShort();
				for (int num68 = 0; num68 < num67; num68++)
				{
					SeriPart seriPart = new SeriPart();
					seriPart.idPart = msg.reader().readShort();
					seriPart.time = msg.reader().readByte();
					seriPart.expireString = msg.reader().readUTF();
					myVector11.addElement(seriPart);
				}
				MapScr.gI().onContainer(myVector11);
				return;
			}
			case -48:
			{
				int idUser7 = msg.reader().readInt();
				short idP = msg.reader().readShort();
				MapScr.gI().onUsingPart(idUser7, idP);
				return;
			}
			case -49:
			{
				short idShop = msg.reader().readByte();
				string nameShop2 = msg.reader().readUTF();
				short[] array12 = null;
				short num55 = msg.reader().readShort();
				if (num55 > 0)
				{
					array12 = new short[num55];
					for (int num56 = 0; num56 < num55; num56++)
					{
						array12[num56] = msg.reader().readShort();
					}
				}
				MapScr.gI().onOpenShop(0, idShop, nameShop2, array12, -1, null, null);
				return;
			}
			case -50:
			{
				Out.println("REQUEST_SOUND");
				string text5 = msg.reader().readUTF();
				sbyte b27 = msg.reader().readByte();
				return;
			}
			case -51:
			{
				Out.println("SOUND_DATA");
				sbyte b22 = msg.reader().readByte();
				sbyte[] data5 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data5);
				byte[] dst = new byte[data5.Length];
				Buffer.BlockCopy(data5, 0, dst, 0, data5.Length);
				return;
			}
			case -52:
			{
				string numSup = msg.reader().readUTF();
				msg.reader().readInt();
				LoginScr.gI().onNumSupport(numSup);
				return;
			}
			case -53:
			{
				sbyte index = msg.reader().readByte();
				string str = msg.reader().readUTF();
				globalLogicHandler.onUpdateContainer(index, str);
				return;
			}
			case -90:
			{
				sbyte index2 = msg.reader().readByte();
				string str2 = msg.reader().readUTF();
				globalLogicHandler.onUpdateCHest(1, index2, str2);
				return;
			}
			case -54:
			{
				string info2 = msg.reader().readUTF();
				string syst = msg.reader().readUTF();
				string number = msg.reader().readUTF();
				globalLogicHandler.onSms(info2, syst, number);
				break;
			}
			case 50:
			{
				if (!(miniGameMessageHandler is FarmMsgHandler) && !(miniGameMessageHandler is ParkMsgHandler) && !(miniGameMessageHandler is HomeMsgHandler))
				{
					break;
				}
				sbyte roomID2 = msg.reader().readByte();
				sbyte b34 = msg.reader().readByte();
				if (b34 == -1)
				{
					Canvas.startOKDlg(msg.reader().readUTF());
					return;
				}
				short x = 0;
				short y = 0;
				MyVector myVector14 = new MyVector();
				if (b34 != -1 && b34 != -2)
				{
					x = msg.reader().readShort();
					y = msg.reader().readShort();
					myVector14 = readListPlayer(msg);
				}
				short num74 = msg.reader().readShort();
				MyVector mapItemType3 = null;
				MyVector mapItem4 = null;
				if (num74 > 0)
				{
					mapItemType3 = readMapItemType(msg);
					mapItem4 = readMapItem(msg);
				}
				if (GameMidlet.CLIENT_TYPE == 9)
				{
					for (int num75 = 0; num75 < myVector14.size(); num75++)
					{
						Avatar avatar9 = (Avatar)myVector14.elementAt(num75);
						avatar9.idWedding = msg.reader().readShort();
					}
				}
				MapScr.gI().onJoinPark(roomID2, b34, x, y, myVector14, mapItemType3, mapItem4);
				if (LoadMap.TYPEMAP == 21)
				{
					Canvas.load = 0;
					HomeMsgHandler.onHandler();
					AvatarService.gI().getTypeHouse(0);
					Canvas.startWaitDlg();
				}
				break;
			}
			case -58:
			{
				int num59 = msg.reader().readByte();
				MyHashTable myHashTable = new MyHashTable();
				for (int num60 = 0; num60 < num59; num60++)
				{
					int num61 = msg.reader().readShort();
					int num62 = msg.reader().readShort();
					sbyte[] data11 = new sbyte[num62];
					msg.reader().read(ref data11);
					Image v = CRes.createImgByByteArray(ArrayCast.cast(data11));
					data11 = null;
					myHashTable.put(string.Empty + num61, v);
				}
				string title = msg.reader().readUTF();
				string str3 = msg.reader().readUTF();
				sbyte idAction = -1;
				if (msg.reader().available() > 0)
				{
					idAction = msg.reader().readByte();
				}
				CustomTab.me = null;
				CustomTab.gI().setInfo(myHashTable, title, str3, idAction);
				CustomTab.gI().show();
				return;
			}
			case -59:
			{
				if (Canvas.currentDialog == Canvas.msgdlg)
				{
					Canvas.currentDialog = null;
				}
				if (Canvas.currentDialog != null)
				{
					return;
				}
				int userID = msg.reader().readInt();
				sbyte iDMenu = msg.reader().readByte();
				int num29 = msg.reader().readByte();
				string[] array8 = new string[num29];
				short[] array9 = new short[num29];
				for (int num30 = 0; num30 < num29; num30++)
				{
					array8[num30] = msg.reader().readUTF();
				}
				if (msg.reader().available() > 0)
				{
					for (int num31 = 0; num31 < num29; num31++)
					{
						array9[num31] = msg.reader().readShort();
					}
				}
				string nameNPC = null;
				string textChat = null;
				bool[] array10 = null;
				if (msg.reader().available() > 0)
				{
					nameNPC = msg.reader().readUTF();
					textChat = msg.reader().readUTF();
					array10 = new bool[num29];
					for (int num32 = 0; num32 < num29; num32++)
					{
						array10[num32] = msg.reader().readBoolean();
					}
				}
				globalLogicHandler.onMenuOption(userID, iDMenu, array8, array9, nameNPC, textChat, array10);
				return;
			}
			case -60:
			{
				int userID2 = msg.reader().readInt();
				sbyte idMenu = msg.reader().readByte();
				string nameText = msg.reader().readUTF();
				int typeInput = msg.reader().readByte();
				sbyte[] data4 = null;
				if (msg.reader().available() > 0)
				{
					short num33 = msg.reader().readShort();
					data4 = new sbyte[msg.reader().available()];
					msg.reader().read(ref data4);
				}
				globalLogicHandler.onTextBox(userID2, idMenu, nameText, typeInput);
				if (data4 != null)
				{
					Canvas.inputDlg.setImg(CRes.createImgByByteArray(ArrayCast.cast(data4)));
				}
				return;
			}
			case -63:
			{
				sbyte b14 = msg.reader().readByte();
				Out.println("WEATHER: " + b14);
				LoadMap.onWeather(b14);
				return;
			}
			case -64:
			{
				int idUser5 = msg.reader().readInt();
				int degree = msg.reader().readShort();
				sbyte b16 = msg.reader().readByte();
				MyVector myVector4 = new MyVector();
				for (int num27 = 0; num27 < b16; num27++)
				{
					Gift gift = new Gift();
					gift.type = msg.reader().readByte();
					switch (gift.type)
					{
					case 1:
					{
						gift.idPart = msg.reader().readShort();
						sbyte b17 = msg.reader().readByte();
						if (b17 == -1)
						{
							gift.expire = "(" + T.forever + ")";
							break;
						}
						gift.expire = "(" + b17 + " " + T.day + ")";
						break;
					}
					case 2:
						gift.xu = msg.reader().readInt();
						break;
					case 3:
						gift.xp = msg.reader().readInt();
						break;
					case 4:
						gift.luong = msg.reader().readInt();
						break;
					}
					myVector4.addElement(gift);
				}
				DialLuckyScr.gI().onStart(idUser5, degree, myVector4);
				return;
			}
			case -70:
			{
				int idUser4 = msg.reader().readInt();
				sbyte expice = (sbyte)(100 - msg.reader().readByte());
				MapScr.gI().onRequestExpicePet(idUser4, expice);
				return;
			}
			case -80:
			{
				short num20 = msg.reader().readShort();
				short num21 = msg.reader().readShort();
				sbyte[] data3 = new sbyte[num21];
				msg.reader().read(ref data3);
				AvatarData.listImgIcon.put(string.Empty + num20, new ImageIcon(CRes.createImgByByteArray(ArrayCast.cast(data3))));
				return;
			}
			case -81:
			{
				string text = msg.reader().readUTF();
				int num3 = 0;
				for (int l = 0; l < text.Length; l++)
				{
					if (text[l] == '-')
					{
						num3++;
					}
				}
				sbyte[] data = new sbyte[msg.reader().available()];
				msg.reader().read(ref data);
				if (num3 == 2 || text.Equals(ListScr.idFriendList))
				{
					ListScr.hList.put(text, data);
					ListScr.gI().setList(text);
				}
				else
				{
					ListScr.gI().readList(data, text);
					Canvas.endDlg();
				}
				return;
			}
			case -82:
			{
				int idUser2 = msg.reader().readInt();
				short idImg = msg.reader().readShort();
				MapScr.gI().onChangeClan(idUser2, idImg);
				return;
			}
			case -83:
			{
				sbyte b35 = msg.reader().readByte();
				MyVector myVector15 = new MyVector();
				for (int num76 = 0; num76 < b35; num76++)
				{
					StringObj stringObj3 = new StringObj();
					stringObj3.anthor = msg.reader().readShort();
					stringObj3.str = msg.reader().readUTF();
					stringObj3.dis = msg.reader().readShort();
					myVector15.addElement(stringObj3);
				}
				MapScr.gI().onMenuRotate(myVector15);
				return;
			}
			case -77:
			{
				int idBoss2 = msg.reader().readInt();
				sbyte b32 = msg.reader().readByte();
				string text6 = msg.reader().readUTF();
				sbyte b33 = msg.reader().readByte();
				string[] array17 = new string[b33];
				for (int num69 = 0; num69 < b33; num69++)
				{
					array17[num69] = msg.reader().readUTF();
				}
				if (PopupShop.me != Canvas.currentMyScreen)
				{
					MapScr.gI().onCustomPopup(idBoss2, b32, text6, array17);
				}
				return;
			}
			case -78:
			{
				sbyte b30 = msg.reader().readByte();
				int idBoss = msg.reader().readInt();
				short idShop2 = msg.reader().readByte();
				string nameShop3 = msg.reader().readUTF();
				short num57 = msg.reader().readShort();
				if (num57 <= 0)
				{
					return;
				}
				short[] array13 = new short[num57];
				string[] array14 = new string[num57];
				string[] array15 = null;
				if (b30 == 1)
				{
					array15 = new string[num57];
				}
				for (int num58 = 0; num58 < num57; num58++)
				{
					array13[num58] = msg.reader().readShort();
					array14[num58] = msg.reader().readUTF();
					if (b30 == 1)
					{
						array15[num58] = msg.reader().readUTF();
					}
				}
				MapScr.gI().onOpenShop(b30, idShop2, nameShop3, array13, idBoss, array14, array15);
				return;
			}
			case 89:
			{
				sbyte b28 = msg.reader().readByte();
				if (b28 == 0)
				{
					sbyte typeDrop = msg.reader().readByte();
					short idDrop = msg.reader().readShort();
					int id5 = msg.reader().readInt();
					int idPlayer = msg.reader().readInt();
					short xDrop = msg.reader().readShort();
					short yDrop = msg.reader().readShort();
					MapScr.gI().onDropPark(typeDrop, idPlayer, idDrop, id5, xDrop, yDrop);
				}
				else
				{
					int id6 = msg.reader().readInt();
					int idUser6 = msg.reader().readInt();
					MapScr.gI().onGetPart(id6, idUser6);
				}
				return;
			}
			case -84:
			{
				sbyte b4 = msg.reader().readByte();
				short num4 = msg.reader().readByte();
				Out.println("EFFECT_OBJ: " + num4);
				if (num4 == 5 || num4 == 2)
				{
					return;
				}
				if (b4 == 0)
				{
					EffectData effect = AvatarData.getEffect(num4);
					if (effect == null)
					{
						AvatarService.gI().doRequestEffectData(num4);
					}
					EffectManager effectManager = new EffectManager();
					effectManager.ID = num4;
					effectManager.style = msg.reader().readByte();
					effectManager.loopLimit = (effectManager.count = msg.reader().readByte());
					if (effectManager.style == 4)
					{
						int num5 = msg.reader().readShort();
						sbyte b5 = msg.reader().readByte();
						if (Canvas.currentEffect.size() > 0)
						{
							for (int m = 0; m < Canvas.currentEffect.size(); m++)
							{
								Effect effect2 = (Effect)Canvas.currentEffect.elementAt(m);
								if (effect2.IDAction == num4)
								{
									return;
								}
							}
						}
						AnimateEffect animateEffect = new AnimateEffect(2, true, num5);
						animateEffect.timeStop = b5;
						animateEffect.IDAction = num4;
						animateEffect.show();
						return;
					}
					effectManager.loop = msg.reader().readShort();
					effectManager.loopType = msg.reader().readByte();
					if (effectManager.loopType == 1)
					{
						effectManager.radius = msg.reader().readShort();
					}
					else if (effectManager.loopType == 2)
					{
						sbyte b6 = msg.reader().readByte();
						effectManager.xLoop = new short[b6];
						effectManager.yLoop = new short[b6];
						for (int n = 0; n < b6; n++)
						{
							effectManager.xLoop[n] = msg.reader().readShort();
							effectManager.yLoop[n] = msg.reader().readShort();
						}
					}
					if (effectManager.style == 0)
					{
						effectManager.idPlayer = msg.reader().readInt();
					}
					else
					{
						effectManager.x = msg.reader().readShort();
						effectManager.y = msg.reader().readShort();
					}
					MapScr.gI().onEffect(effectManager);
					return;
				}
				EffectData effectData = new EffectData();
				effectData.ID = num4;
				short num6 = msg.reader().readShort();
				sbyte[] data2 = new sbyte[num6];
				msg.reader().read(ref data2);
				effectData.img = CRes.createImgByByteArray(ArrayCast.cast(data2));
				sbyte b7 = msg.reader().readByte();
				effectData.imgImfo = new ImageInfo[b7];
				for (int num7 = 0; num7 < b7; num7++)
				{
					effectData.imgImfo[num7] = new ImageInfo();
					effectData.imgImfo[num7].ID = msg.reader().readByte();
					effectData.imgImfo[num7].x0 = msg.reader().readByte();
					effectData.imgImfo[num7].y0 = msg.reader().readByte();
					effectData.imgImfo[num7].w = msg.reader().readByte();
					effectData.imgImfo[num7].h = msg.reader().readByte();
				}
				sbyte b8 = msg.reader().readByte();
				effectData.frame = new Frame[b8];
				for (int num8 = 0; num8 < b8; num8++)
				{
					effectData.frame[num8] = new Frame();
					sbyte b9 = msg.reader().readByte();
					effectData.frame[num8].dx = new short[b9];
					effectData.frame[num8].dy = new short[b9];
					effectData.frame[num8].idImg = new sbyte[b9];
					for (int num9 = 0; num9 < b9; num9++)
					{
						effectData.frame[num8].dx[num9] = msg.reader().readByte();
						effectData.frame[num8].dy[num9] = msg.reader().readByte();
						effectData.frame[num8].idImg[num9] = msg.reader().readByte();
					}
				}
				sbyte b10 = msg.reader().readByte();
				effectData.arrFrame = new sbyte[b10];
				for (int num10 = 0; num10 < b10; num10++)
				{
					effectData.arrFrame[num10] = msg.reader().readByte();
				}
				AvatarData.effectList.addElement(effectData);
				return;
			}
			case -85:
			{
				int idUser = msg.reader().readInt();
				sbyte b3 = msg.reader().readByte();
				MyVector myVector = new MyVector();
				for (int k = 0; k < b3; k++)
				{
					Emotion emotion = new Emotion();
					emotion.type = msg.reader().readByte();
					emotion.id = msg.reader().readShort();
					emotion.time = msg.reader().readShort();
					myVector.addElement(emotion);
				}
				MapScr.gI().onEmotionList(idUser, myVector);
				return;
			}
			case -24:
			{
				short num77 = msg.reader().readShort();
				int newMoney = 0;
				sbyte typeBuy = 0;
				if (num77 != -1)
				{
					newMoney = msg.reader().readInt();
					typeBuy = msg.reader().readByte();
				}
				string text7 = msg.reader().readUTF();
				int xu3 = msg.reader().readInt();
				int luong3 = msg.reader().readInt();
				int luongKhoa = msg.reader().readInt();
				MapScr.gI().onBuyItem(num77, newMoney, typeBuy, text7, xu3, luong3, luongKhoa);
				return;
			}
			case -87:
			{
				short num70 = msg.reader().readShort();
				MyVector myVector12 = new MyVector();
				for (int num71 = 0; num71 < num70; num71++)
				{
					SeriPart seriPart2 = new SeriPart();
					seriPart2.idPart = msg.reader().readShort();
					seriPart2.time = msg.reader().readByte();
					seriPart2.expireString = msg.reader().readUTF();
					myVector12.addElement(seriPart2);
				}
				int moneyOnChest = msg.reader().readInt();
				sbyte levelChest = msg.reader().readByte();
				short num72 = msg.reader().readShort();
				MyVector myVector13 = new MyVector();
				for (int num73 = 0; num73 < num72; num73++)
				{
					SeriPart seriPart3 = new SeriPart();
					seriPart3.idPart = msg.reader().readShort();
					seriPart3.time = msg.reader().readByte();
					seriPart3.expireString = msg.reader().readUTF();
					myVector13.addElement(seriPart3);
				}
				HouseScr.gI().onCustomChest(myVector12, myVector13, moneyOnChest, levelChest);
				return;
			}
			case -88:
				HouseScr.gI().onEnterPass();
				return;
			case -89:
				HouseScr.gI().onTransChestPart(msg.reader().readBoolean(), msg.reader().readUTF());
				return;
			case -92:
			{
				sbyte id7 = msg.reader().readByte();
				int num63 = msg.reader().readInt();
				sbyte[] data12 = new sbyte[num63];
				msg.reader().read(ref data12);
				int num64 = msg.reader().readInt();
				sbyte wMn = msg.reader().readByte();
				sbyte[] array16 = new sbyte[num64];
				for (int num65 = 0; num65 < num64; num65++)
				{
					array16[num65] = msg.reader().readByte();
				}
				sbyte b31 = msg.reader().readByte();
				MyVector myVector10 = new MyVector();
				for (int num66 = 0; num66 < b31; num66++)
				{
					PositionMap positionMap = new PositionMap();
					positionMap.id = msg.reader().readByte();
					positionMap.idImg = msg.reader().readShort();
					positionMap.text = msg.reader().readUTF();
					positionMap.x = msg.reader().readByte() * 16 * AvMain.hd;
					positionMap.y = msg.reader().readByte() * 16 * AvMain.hd;
					myVector10.addElement(positionMap);
				}
				MiniMap.isCityMap = false;
				MiniMap.isChange = true;
				MiniMap.gI().setInfo(new FrameImage(CRes.createImgByByteArray(ArrayCast.cast(data12)), 16 * AvMain.hd, 16 * AvMain.hd), array16, myVector10, wMn, 16 * AvMain.hd, new Command(T.selectRegion, new IActionMiniMap(id7)));
				MiniMap.gI().switchToMe(MapScr.instance);
				LoadMap.TYPEMAP = -1;
				LoadMap.typeAny = -108;
				LoadMap.typeTemp = -1;
				return;
			}
			case -93:
			{
				sbyte idMap2 = msg.reader().readByte();
				sbyte idTileImg = msg.reader().readByte();
				short num47 = msg.reader().readShort();
				sbyte wMap = msg.reader().readByte();
				int num48 = msg.reader().readShort();
				sbyte[] data9 = new sbyte[num48];
				msg.reader().read(ref data9);
				short[] array11 = null;
				sbyte b23 = msg.reader().readByte();
				if (b23 > 0)
				{
					array11 = new short[b23];
					for (int num49 = 0; num49 < b23; num49++)
					{
						array11[num49] = msg.reader().readShort();
					}
				}
				short num50 = msg.reader().readShort();
				Image img = null;
				if (num50 > 0)
				{
					sbyte[] data10 = new sbyte[num50];
					msg.reader().read(ref data10);
					img = CRes.createImgByByteArray(ArrayCast.cast(data10));
				}
				short num51 = msg.reader().readShort();
				MyVector myVector8 = null;
				MyVector myVector9 = null;
				if (num51 > 0)
				{
					sbyte b24 = msg.reader().readByte();
					myVector8 = new MyVector();
					for (int num52 = 0; num52 < b24; num52++)
					{
						MapItemType mapItemType2 = new MapItemType();
						mapItemType2.idType = msg.reader().readByte();
						mapItemType2.imgID = msg.reader().readShort();
						mapItemType2.iconID = msg.reader().readByte();
						mapItemType2.dx = msg.reader().readShort();
						mapItemType2.dy = msg.reader().readShort();
						sbyte b25 = msg.reader().readByte();
						mapItemType2.listNotTrans = new MyVector();
						for (int num53 = 0; num53 < b25; num53++)
						{
							AvPosition avPosition2 = new AvPosition();
							avPosition2.x = msg.reader().readByte();
							avPosition2.y = msg.reader().readByte();
							mapItemType2.listNotTrans.addElement(avPosition2);
						}
						myVector8.addElement(mapItemType2);
					}
					sbyte b26 = msg.reader().readByte();
					myVector9 = new MyVector();
					for (int num54 = 0; num54 < b26; num54++)
					{
						MapItem mapItem3 = new MapItem();
						mapItem3.type = msg.reader().readByte();
						mapItem3.typeID = msg.reader().readByte();
						mapItem3.x = msg.reader().readByte();
						mapItem3.y = msg.reader().readByte();
						mapItem3.isGetImg = true;
						myVector9.addElement(mapItem3);
					}
				}
				MapScr.gI().onSelectedMiniMap(data9, idMap2, idTileImg, wMap, img, array11, myVector8, myVector9);
				return;
			}
			case -94:
			{
				sbyte idTileMap = msg.reader().readByte();
				sbyte[] data8 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data8);
				Canvas.loadMap.onTileImg(idTileMap, data8);
				return;
			}
			case -96:
				Canvas.endDlg();
				MapScr.gI().move();
				OnSplashScr.gI().switchToMe();
				OnSplashScr.gI().splashScrStat = 0;
				return;
			case -97:
			{
				sbyte[] data7 = new sbyte[msg.reader().available()];
				msg.reader().read(ref data7);
				MyVector myVector7 = AvatarData.readAvatarPart(data7, true);
				Part part = (Part)myVector7.elementAt(0);
				AvatarData.listPartDynamic.put(string.Empty + part.IDPart, part);
				return;
			}
			case -98:
			{
				short num45 = msg.reader().readShort();
				short num46 = msg.reader().readShort();
				sbyte[] data6 = new sbyte[num46];
				msg.reader().read(ref data6);
				AvatarData.listImgPart.put(string.Empty + num45, new ImageIcon(CRes.createImgByByteArray(ArrayCast.cast(data6))));
				return;
			}
			case -38:
			{
				short num44 = msg.reader().readShort();
				int price = 0;
				if (num44 != -1)
				{
					price = msg.reader().readInt();
				}
				int xu2 = msg.reader().readInt();
				int luong2 = msg.reader().readInt();
				int luongK2 = msg.reader().readInt();
				GameMidlet.avatar.updateMoney(xu2, luong2, luongK2);
				MapScr.gI().onBuyIceDream(num44, price);
				return;
			}
			case -99:
			{
				sbyte idMap = msg.reader().readByte();
				sbyte b19 = msg.reader().readByte();
				MyVector myVector6 = new MyVector();
				for (int num34 = 0; num34 < b19; num34++)
				{
					Avatar avatar7 = new Avatar();
					avatar7.IDDB = msg.reader().readInt();
					avatar7.setName(msg.reader().readUTF());
					sbyte b20 = msg.reader().readByte();
					for (int num35 = 0; num35 < b20; num35++)
					{
						avatar7.addSeri(new SeriPart(msg.reader().readShort()));
					}
					avatar7.x = msg.reader().readShort();
					avatar7.y = msg.reader().readShort();
					avatar7.blogNews = msg.reader().readByte();
					avatar7.hungerPet = (sbyte)(100 - msg.reader().readByte());
					avatar7.idImg = msg.reader().readShort();
					sbyte b21 = msg.reader().readByte();
					avatar7.textChat = new string[b21];
					for (int num36 = 0; num36 < b21; num36++)
					{
						avatar7.textChat[num36] = msg.reader().readUTF();
					}
					myVector6.addElement(avatar7);
				}
				short num37 = msg.reader().readShort();
				MyVector mapItemType = null;
				MyVector mapItem2 = null;
				if (num37 > 0)
				{
					mapItemType = readMapItemType(msg);
					mapItem2 = readMapItem(msg);
				}
				MapScr.gI().onJoinOfflineMap(idMap, myVector6, mapItemType, mapItem2);
				break;
			}
			case -101:
			{
				sbyte b15 = msg.reader().readByte();
				short num25 = msg.reader().readShort();
				if (b15 == 1)
				{
					StringObj stringObj = new StringObj();
					stringObj.anthor = num25;
					stringObj.str = msg.reader().readUTF();
					stringObj.dis = msg.reader().readShort();
					stringObj.type = msg.reader().readByte();
					MapScr.listCmdRotate.addElement(stringObj);
					if (Canvas.currentMyScreen == PopupShop.gI())
					{
						PopupShop.gI().close();
					}
					if (LoadMap.focusObj != null)
					{
						MainMenu.gI().doExchange();
					}
					break;
				}
				for (int num26 = 0; num26 < MapScr.listCmdRotate.size(); num26++)
				{
					StringObj stringObj2 = (StringObj)MapScr.listCmdRotate.elementAt(num26);
					if (stringObj2.anthor == num25)
					{
						MapScr.listCmdRotate.removeElementAt(num26);
						break;
					}
				}
				break;
			}
			case -103:
			{
				int id4 = msg.reader().readInt();
				Avatar avatar4 = LoadMap.getAvatar(id4);
				sbyte b13 = msg.reader().readByte();
				if (b13 == 0)
				{
					avatar4.idImg = msg.reader().readShort();
				}
				else
				{
					avatar4.idWedding = msg.reader().readShort();
				}
				break;
			}
			case -102:
			{
				int id2 = msg.reader().readInt();
				int num19 = msg.reader().readInt();
				Avatar avatar = null;
				avatar = ((!onMainMenu.isOngame) ? LoadMap.getAvatar(id2) : BoardScr.getAvatarByID(id2));
				if (avatar != null)
				{
					avatar.money[3] = num19;
				}
				break;
			}
			case -105:
			{
				sbyte b11 = msg.reader().readByte();
				MyVector myVector3 = new MyVector();
				for (int num18 = 0; num18 < b11; num18++)
				{
					short idImg2 = msg.reader().readShort();
					string caption = msg.reader().readUTF();
					Command o = new CommandFlower(caption, new IActionFlower((sbyte)num18), idImg2);
					myVector3.addElement(o);
				}
				Canvas.endDlg();
				FarmScr.gI().startMenuFarm(myVector3);
				break;
			}
			case -107:
			{
				sbyte b2 = msg.reader().readByte();
				string nameShop = null;
				string[] array = null;
				string[] array2 = null;
				string[] array3 = null;
				short[] array4 = null;
				short[] array5 = null;
				short[] array6 = null;
				int[] array7 = null;
				if (b2 == 0)
				{
					nameShop = msg.reader().readUTF();
					short num = msg.reader().readShort();
					array = new string[num];
					array4 = new short[num];
					array2 = new string[num];
					array3 = new string[num];
					array5 = new short[num];
					array6 = new short[num];
					for (int i = 0; i < num; i++)
					{
						array5[i] = msg.reader().readShort();
						array4[i] = msg.reader().readShort();
						array6[i] = msg.reader().readShort();
						array[i] = msg.reader().readUTF();
						array2[i] = msg.reader().readUTF();
						array3[i] = msg.reader().readUTF();
					}
				}
				else if (b2 == 1)
				{
					nameShop = msg.reader().readUTF();
					short num2 = msg.reader().readShort();
					array5 = new short[num2];
					array = new string[num2];
					array4 = new short[num2];
					array7 = new int[num2];
					array3 = new string[num2];
					array6 = new short[num2];
					array2 = new string[num2];
					for (int j = 0; j < num2; j++)
					{
						array5[j] = msg.reader().readShort();
						array[j] = msg.reader().readUTF();
						array2[j] = msg.reader().readUTF();
						array4[j] = msg.reader().readShort();
						array6[j] = msg.reader().readShort();
						array7[j] = msg.reader().readInt();
						array3[j] = msg.reader().readUTF();
					}
				}
				HouseScr.gI().onOpenShop(b2, nameShop, array, array4, array5, array2, array3, array7, array6);
				return;
			}
			case -74:
			{
				MapItem mapItem = new MapItem();
				mapItem.typeID = msg.reader().readShort();
				mapItem.x = (mapItem.xTo = 24 * msg.reader().readByte());
				mapItem.y = (mapItem.yTo = 24 * msg.reader().readByte());
				HouseScr.gI().onBuyItemHouse(mapItem);
				return;
			}
			case 122:
			{
				Out.println("DICH_CHUYEN");
				sbyte b = msg.reader().readByte();
				sbyte roomID = msg.reader().readByte();
				sbyte boardID = msg.reader().readByte();
				int xTe = msg.reader().readShort();
				int yTe = msg.reader().readShort();
				Canvas.loadMap.onDichChuyen(roomID, boardID, xTe, yTe);
				return;
			}
			}
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		if (miniGameMessageHandler != null)
		{
			miniGameMessageHandler.onMessage(msg);
			return;
		}
		try
		{
			switch (msg.command)
			{
			case -5:
				globalLogicHandler.onLoginFail(msg.reader().readUTF());
				break;
			case -4:
			{
				LoginScr.gI().saveLogin();
				if (!RegisterScr.isCreateChar || Canvas.currentMyScreen != RegisterScr.instance)
				{
					GameMidlet.avatar = new Avatar();
				}
				GameMidlet.avatar.IDDB = msg.reader().readInt();
				GameMidlet.avatar.setName(LoginScr.gI().tfUser.getText().ToLower());
				sbyte b36 = msg.reader().readByte();
				MyVector myVector16 = new MyVector();
				for (int num78 = 0; num78 < b36; num78++)
				{
					SeriPart seriPart4 = new SeriPart();
					seriPart4.idPart = msg.reader().readShort();
					myVector16.addElement(seriPart4);
				}
				if (!RegisterScr.isCreateChar || Canvas.currentMyScreen != RegisterScr.instance)
				{
					GameMidlet.avatar.seriPart = myVector16;
				}
				sbyte gender = msg.reader().readByte();
				if (!RegisterScr.isCreateChar || Canvas.currentMyScreen != RegisterScr.instance)
				{
					GameMidlet.avatar.gender = gender;
				}
				GameMidlet.avatar.lvMain = msg.reader().readByte();
				GameMidlet.avatar.perLvMain = msg.reader().readByte();
				GameMidlet.avatar.setMoney(msg.reader().readInt());
				GameMidlet.avatar.indexP = new sbyte[5];
				GameMidlet.avatar.indexP[Avatar.I_FRIENDLY] = msg.reader().readByte();
				GameMidlet.avatar.indexP[Avatar.I_CRAZY] = msg.reader().readByte();
				GameMidlet.avatar.indexP[Avatar.I_STYLISH] = msg.reader().readByte();
				GameMidlet.avatar.indexP[Avatar.I_HAPPY] = msg.reader().readByte();
				GameMidlet.avatar.indexP[Avatar.I_HUNGER] = msg.reader().readByte();
				GameMidlet.avatar.money[2] = msg.reader().readInt();
				GameMidlet.avatar.blogNews = msg.reader().readByte();
				for (int num79 = 0; num79 < GameMidlet.avatar.seriPart.size(); num79++)
				{
					SeriPart seriPart5 = (SeriPart)GameMidlet.avatar.seriPart.elementAt(num79);
					seriPart5.time = msg.reader().readByte();
					seriPart5.expireString = msg.reader().readUTF();
				}
				GameMidlet.avatar.idImg = msg.reader().readShort();
				MapScr.listCmd = new MyVector();
				sbyte b37 = msg.reader().readByte();
				for (int num80 = 0; num80 < b37; num80++)
				{
					StringObj stringObj4 = new StringObj();
					stringObj4.str = msg.reader().readUTF();
					stringObj4.dis = msg.reader().readShort();
					MapScr.listCmd.addElement(stringObj4);
				}
				MapScr.listCmdRotate = new MyVector();
				sbyte b38 = msg.reader().readByte();
				for (int num81 = 0; num81 < b38; num81++)
				{
					StringObj stringObj5 = new StringObj();
					stringObj5.anthor = msg.reader().readShort();
					stringObj5.str = msg.reader().readUTF();
					stringObj5.dis = msg.reader().readShort();
					MapScr.listCmdRotate.addElement(stringObj5);
				}
				MapScr.gI().isTour = msg.reader().readBool();
				if (msg.reader().available() > 0)
				{
					for (int num82 = 0; num82 < b38; num82++)
					{
						StringObj stringObj6 = (StringObj)MapScr.listCmdRotate.elementAt(num82);
						stringObj6.type = msg.reader().readByte();
					}
				}
				if (msg.reader().available() > 0)
				{
					Canvas.iOpenOngame = msg.reader().readByte();
				}
				GameMidlet.avatar.lvMain = (GameMidlet.myIndexP.level = msg.reader().readShort());
				if (Canvas.iOpenOngame == 1 || Canvas.iOpenOngame == 2)
				{
					T.nameCasino = T.nameCasino1;
				}
				GameMidlet.avatar.idWedding = msg.reader().readShort();
				if (msg.reader().available() > 0)
				{
					MapScr.isNewVersion = msg.reader().readBoolean();
				}
				if (MapScr.isNewVersion)
				{
					GameMidlet.avatar.money[3] = msg.reader().readInt();
				}
				sbyte b39 = msg.reader().readByte();
				MapScr.listItemEffect = new MyVector();
				for (int num83 = 0; num83 < b39; num83++)
				{
					ItemEffectInfo itemEffectInfo = new ItemEffectInfo();
					itemEffectInfo.IDAction = msg.reader().readShort();
					itemEffectInfo.name = msg.reader().readUTF();
					itemEffectInfo.IDIcon = msg.reader().readShort();
					itemEffectInfo.money = msg.reader().readInt();
					itemEffectInfo.typeMoney = msg.reader().readByte();
					MapScr.listItemEffect.addElement(itemEffectInfo);
				}
				GameMidlet.avatar.setGold(msg.reader().readInt());
				GameMidlet.avatar.luongKhoa = msg.reader().readInt();
				sbyte b40 = msg.reader().readByte();
				string name3 = msg.reader().readUTF();
				GameMidlet.avatar.setName(name3);
				Out.println("Money: " + GameMidlet.avatar.money[2] + "    " + GameMidlet.avatar.luongKhoa);
				globalLogicHandler.onLoginSuccess();
				break;
			}
			}
		}
		catch (Exception e2)
		{
			Out.logError(e2);
		}
	}

	public static MyVector readListPlayer(Message msg)
	{
		MyVector myVector = new MyVector();
		try
		{
			sbyte b = msg.reader().readByte();
			for (int i = 0; i < b; i++)
			{
				Avatar avatar = new Avatar();
				avatar.IDDB = msg.reader().readInt();
				avatar.setName(msg.reader().readUTF());
				sbyte b2 = msg.reader().readByte();
				for (int j = 0; j < b2; j++)
				{
					short idP = msg.reader().readShort();
					avatar.addSeri(new SeriPart(idP));
				}
				avatar.x = msg.reader().readShort();
				avatar.y = msg.reader().readShort();
				avatar.blogNews = msg.reader().readByte();
				myVector.addElement(avatar);
			}
			for (int k = 0; k < b; k++)
			{
				Avatar avatar2 = (Avatar)myVector.elementAt(k);
				avatar2.direct = msg.reader().readByte();
			}
			for (int l = 0; l < b; l++)
			{
				Avatar avatar3 = (Avatar)myVector.elementAt(l);
				avatar3.hungerPet = (sbyte)(100 - msg.reader().readByte());
			}
			for (int m = 0; m < b; m++)
			{
				Avatar avatar4 = (Avatar)myVector.elementAt(m);
				avatar4.idImg = msg.reader().readShort();
			}
			sbyte b3 = msg.reader().readByte();
			for (int n = 0; n < b3; n++)
			{
				Drop_Part drop_Part = new Drop_Part();
				drop_Part.type = msg.reader().readByte();
				drop_Part.idDrop = msg.reader().readShort();
				drop_Part.ID = msg.reader().readInt();
				drop_Part.x = msg.reader().readShort();
				drop_Part.y = msg.reader().readShort();
				myVector.addElement(drop_Part);
			}
			LoadMap.listImgAD = null;
			sbyte b4 = 0;
			if (msg.reader().available() > 0)
			{
				b4 = msg.reader().readByte();
			}
			if (b4 > 0)
			{
				LoadMap.listImgAD = new MyVector();
				for (int num = 0; num < b4; num++)
				{
					AvPosition avPosition = new AvPosition();
					avPosition.anchor = msg.reader().readShort();
					avPosition.x = msg.reader().readShort();
					avPosition.y = msg.reader().readShort();
					avPosition.depth = msg.reader().readByte();
					LoadMap.listImgAD.addElement(avPosition);
				}
			}
		}
		catch (Exception)
		{
			return new MyVector();
		}
		return myVector;
	}

	public static MyVector readMapItem(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			MyVector myVector = new MyVector();
			for (int i = 0; i < b; i++)
			{
				MapItem mapItem = new MapItem();
				mapItem.type = msg.reader().readByte();
				mapItem.typeID = msg.reader().readByte();
				mapItem.x = msg.reader().readByte();
				mapItem.y = msg.reader().readByte();
				mapItem.isGetImg = true;
				myVector.addElement(mapItem);
			}
			return myVector;
		}
		catch (Exception e)
		{
			Out.logError(e);
		}
		return null;
	}

	public static MyVector readMapItemType(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			MyVector myVector = new MyVector();
			for (int i = 0; i < b; i++)
			{
				MapItemType mapItemType = new MapItemType();
				mapItemType.idType = msg.reader().readByte();
				mapItemType.imgID = msg.reader().readShort();
				mapItemType.iconID = msg.reader().readByte();
				mapItemType.dx = msg.reader().readShort();
				mapItemType.dy = msg.reader().readShort();
				sbyte b2 = msg.reader().readByte();
				mapItemType.listNotTrans = new MyVector();
				for (int j = 0; j < b2; j++)
				{
					AvPosition avPosition = new AvPosition();
					avPosition.x = msg.reader().readByte();
					avPosition.y = msg.reader().readByte();
					mapItemType.listNotTrans.addElement(avPosition);
				}
				myVector.addElement(mapItemType);
			}
			return myVector;
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		return null;
	}

	public static void readMove(Message msg)
	{
		int id = msg.reader().readInt();
		int xM = msg.reader().readShort();
		int yM = msg.reader().readShort();
		int direct = msg.reader().readByte();
		MapScr.gI().onMovePark(id, xM, yM, direct);
	}

	public static void readChat(Message msg)
	{
		int num = msg.reader().readInt();
		string text = msg.reader().readUTF();
		if (num != GameMidlet.avatar.IDDB)
		{
			MapScr.gI().onChatFrom(num, text);
		}
	}
}
