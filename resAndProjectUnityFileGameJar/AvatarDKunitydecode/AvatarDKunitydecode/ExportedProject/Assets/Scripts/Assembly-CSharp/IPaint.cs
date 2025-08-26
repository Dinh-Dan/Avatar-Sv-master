public interface IPaint
{
	void paintTextBox(MyGraphics g, int x, int y, int width, int height, TField tf, bool isFocus, sbyte indexEraser);

	void paintBoxTab(MyGraphics g, int x, int y, int h, int w, int focusTab, int wSub, int wTab, int hTab, int numTab, int maxTab, int[] count, int[] colorTab, string name, sbyte countCloseAll, sbyte countCloseSmall, bool isMenu, bool isFull, string[] subName, float cmx, Image[][] imgIcon);

	void paintCmd(MyGraphics g, Command left, Command center, Command right);

	void init();

	void initImgCard();

	void paintHalf(MyGraphics g, Card c);

	void paintHalfBackFull(MyGraphics g, Card c);

	void paintFull(MyGraphics g, Card c);

	void paintSmall(MyGraphics g, Card c, bool isCh);

	void paintMSG(MyGraphics g);

	void initPos();

	int collisionCmdBar(Command left, Command center, Command right);

	void initPosLogin(LoginScr lg, int h);

	void getSound(string path, int loop);

	void setAnimalSound(MyVector animalLists);

	void setSoundAnimalFarm();

	void clickSound();

	void paintBGCMD(MyGraphics g, int x, int y, int w, int h);

	void paintCheckBox(MyGraphics g, int x, int y, int focus, bool isCheck);

	void paintSelected(MyGraphics g, int x, int y, int w, int h);

	void paintArrow(MyGraphics g, int index, int x, int y, int w, int indLeft, int indRight);

	void paintNormalFont(MyGraphics g, string str, int x, int y, int anthor, bool isSelect);

	int getWNormalFont(string str);

	void paintSelected_2(MyGraphics g, int x, int y, int w, int h);

	void paintTransBack(MyGraphics g);

	void paintKeyArrow(MyGraphics g, int x, int y);

	int updateKeyArr(int x, int y);

	void setVirtualKeyFish(int index);

	void initPosPhom();

	int initShop();

	bool selectedPointer(int xF, int yF);

	string doJoinGo(int x, int y);

	void setDrawPointer(Command left, Command center, Command right);

	void setBack();

	void paintList(MyGraphics g, int w, int maxW, int maxH, bool isHide, int selected, int[] listBoard);

	void setLanguage();

	void paintDefaultBg(MyGraphics g);

	void paintLogo(MyGraphics g, int x, int y);

	void paintDefaultScrList(MyGraphics g, string title, string subTitle, string check);

	void initImgBoard(int type);

	void setColorBar();

	void initOngame();

	void resetOngame();

	void initString(int type);

	void paintRoomList(MyGraphics g, MyVector roomList, int hSmall, int cmy);

	void setName(int index, BoardScr board);

	void paintPlayer(MyGraphics g, int index, int male, int countLeft, int countRight);

	void updateKeyRegister();

	void initReg();

	void paintPopupBack(MyGraphics g, int x, int y, int w, int h, int countClose, bool isFull);

	void paintButton(MyGraphics g, int x, int y, int index, string text);

	void resetCasino();

	void paintMoney(MyGraphics g, int x, int y);

	void paintTabSoft(MyGraphics g);

	void paintCmdBar(MyGraphics g, Command left, Command center, Command right);

	void paintDefaultPopup(MyGraphics g, int x, int y, int w, int h);

	void paintLineRoom(MyGraphics g, int x, int y, int xTo, int yTo);

	void paintSelect(MyGraphics g, int x, int y, int w, int h);

	void updateKeyOn(Command left, Command center, Command right);

	void paintBorderTitle(MyGraphics g, int x, int y, int w, int h);

	void clearImgAvatar();

	void loadImgAvatar();

	void paintTransMoney(MyGraphics g, int x, int y, int w, int h);
}
