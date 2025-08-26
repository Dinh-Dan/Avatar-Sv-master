using System;

public class RegisterInfoScr : MyScreen
{
	private class IActionTao : IAction
	{
		public void perform()
		{
			gI().create();
		}
	}

	private class IActionOk : IAction
	{
		public void perform()
		{
			Out.println("IActionOk");
		}
	}

	public static RegisterInfoScr me;

	private int x;

	private int y;

	private int w;

	private int h;

	public TField tfUser;

	public TField tfNgay;

	public TField tfThang;

	public TField tfnam;

	public TField tfAddress;

	public TField tfCMND;

	public TField tfNgayCap;

	public TField tfNoiCap;

	public TField tfNumberPhone;

	private int cmtoY;

	private int cmy;

	private int cmdy;

	private int cmvy;

	private int cmyLim;

	public static bool isCreate;

	public static bool isTrue;

	private string[] strInfo;

	private int pa;

	private int dyTran;

	private int timeOpen;

	private int vY;

	private int pyLast;

	private int yCamLast;

	private bool trans;

	private bool isClick;

	private new bool isHide;

	private bool changeFocus;

	private bool changeIndex;

	private bool transY;

	private bool tranImage;

	private long timeDelay;

	private long count;

	private long timePoint;

	private long timePointY;

	public static RegisterInfoScr gI()
	{
		return (me != null) ? me : (me = new RegisterInfoScr());
	}

	public void start(bool isTrue)
	{
		w = 240 * AvMain.hd;
		if (w > Canvas.w)
		{
			w = Canvas.w;
		}
		h = 210 * AvMain.hd;
		x = (Canvas.w - w) / 2;
		y = (Canvas.h - h) / 2 - PaintPopup.hButtonSmall / 2 + 10 * AvMain.hd;
		int num = y + 5 * AvMain.hd;
		tfUser = new TField(string.Empty, null, new IActionOk());
		tfUser.name = "tfUser";
		tfUser.sDefaust = "Họ và tên";
		tfUser.isChangeFocus = false;
		tfUser.width = w - 22 * AvMain.hd;
		if (isTrue)
		{
			tfUser.setText("Nguyển Văn A");
		}
		else
		{
			tfUser.setText(string.Empty);
		}
		tfUser.setIputType(3);
		tfUser.x = x + 14 * AvMain.hd;
		tfUser.y = num;
		num += tfUser.height + 2 * AvMain.hd;
		int width = (w - 40 * AvMain.hd) / 3;
		tfnam = new TField(string.Empty, null, new IActionOk());
		tfnam.name = "tfnam";
		tfnam.sDefaust = "Năm";
		tfnam.isChangeFocus = false;
		tfnam.width = width;
		if (isTrue)
		{
			tfnam.setText("1987");
		}
		else
		{
			tfnam.setText(string.Empty);
		}
		tfnam.setIputType(1);
		tfnam.x = x + w - 8 * AvMain.hd - tfnam.width;
		tfnam.y = num;
		tfThang = new TField(string.Empty, null, new IActionOk());
		tfThang.name = "tfThang";
		tfThang.sDefaust = "Tháng";
		tfThang.isChangeFocus = false;
		tfThang.width = width;
		if (isTrue)
		{
			tfThang.setText("1");
		}
		else
		{
			tfThang.setText(string.Empty);
		}
		tfThang.setIputType(1);
		tfThang.x = x + w / 2 - tfThang.width / 2 + AvMain.hd;
		tfThang.y = num;
		tfNgay = new TField(string.Empty, null, new IActionOk());
		tfNgay.name = "tfNgay";
		tfNgay.sDefaust = "Ngày";
		tfNgay.isChangeFocus = false;
		tfNgay.width = width;
		if (isTrue)
		{
			tfNgay.setText("1");
		}
		else
		{
			tfNgay.setText(string.Empty);
		}
		tfNgay.setIputType(1);
		tfNgay.x = x + 13 * AvMain.hd;
		tfNgay.y = num;
		num += tfUser.height + 2 * AvMain.hd;
		tfAddress = new TField(string.Empty, null, new IActionOk());
		tfAddress.name = "tfAddress";
		tfAddress.sDefaust = "Địa chỉ";
		tfAddress.isChangeFocus = false;
		tfAddress.width = w - 22 * AvMain.hd;
		if (isTrue)
		{
			tfAddress.setText("Ho Chi Minh");
		}
		else
		{
			tfAddress.setText(string.Empty);
		}
		tfAddress.setIputType(0);
		tfAddress.x = x + 14 * AvMain.hd;
		tfAddress.y = num;
		num += tfUser.height + 2 * AvMain.hd;
		tfCMND = new TField(string.Empty, null, new IActionOk());
		tfCMND.name = "tfCMND";
		tfCMND.sDefaust = "Số CMND hoặc hộ chiếu";
		tfCMND.isChangeFocus = false;
		tfCMND.width = w - 22 * AvMain.hd;
		if (isTrue)
		{
			tfCMND.setText("0123456789");
		}
		else
		{
			tfCMND.setText(string.Empty);
		}
		tfCMND.setIputType(1);
		tfCMND.x = x + 14 * AvMain.hd;
		tfCMND.y = num;
		num += tfUser.height + 2 * AvMain.hd;
		tfNgayCap = new TField(string.Empty, null, new IActionOk());
		tfNgayCap.name = "tfNgayCap";
		tfNgayCap.sDefaust = "Ngày cấp";
		tfNgayCap.isChangeFocus = false;
		tfNgayCap.width = (w - 42 * AvMain.hd) / 2;
		if (isTrue)
		{
			tfNgayCap.setText("1/1/2010");
		}
		else
		{
			tfNgayCap.setText(string.Empty);
		}
		tfNgayCap.setIputType(0);
		tfNgayCap.x = x + 14 * AvMain.hd;
		tfNgayCap.y = num;
		tfNoiCap = new TField(string.Empty, null, new IActionOk());
		tfNoiCap.name = "tfNoiCap";
		tfNoiCap.sDefaust = "Nơi cấp";
		tfNoiCap.isChangeFocus = false;
		tfNoiCap.width = (w - 22 * AvMain.hd) / 2;
		if (isTrue)
		{
			tfNoiCap.setText("Ho Chi Minh");
		}
		else
		{
			tfNoiCap.setText(string.Empty);
		}
		tfNoiCap.setIputType(0);
		tfNoiCap.x = x + w - (w - 22 * AvMain.hd) / 2 - 10 * AvMain.hd;
		tfNoiCap.y = num;
		num += tfUser.height + 2 * AvMain.hd;
		tfNumberPhone = new TField(string.Empty, null, new IActionOk());
		tfNumberPhone.name = "tfNumberPhone";
		tfNumberPhone.sDefaust = "Số điện thoại";
		tfNumberPhone.isChangeFocus = false;
		tfNumberPhone.width = w - 22 * AvMain.hd;
		if (isTrue)
		{
			tfNumberPhone.setText("0123456789");
		}
		else
		{
			tfNumberPhone.setText(string.Empty);
		}
		tfNumberPhone.setIputType(0);
		tfNumberPhone.x = x + 14 * AvMain.hd;
		tfNumberPhone.y = num;
		strInfo = Canvas.fontChatB.splitFontBStrInLine("Dưới 18 tuổi chỉ có thể chơi 180 phút 1 ngày.", w - 20 * AvMain.hd);
		h = num + Canvas.fontChatB.getHeight() * strInfo.Length + 5 * AvMain.hd;
		if (h > Canvas.hCan - PaintPopup.hButtonSmall)
		{
			h = Canvas.hCan - PaintPopup.hButtonSmall;
		}
		switchToMe();
		cmyLim = num - w;
		if (cmyLim < 0)
		{
			cmyLim = 0;
		}
		cmtoY = 0;
		cmy = 0;
		center = new Command("Tạo", new IActionTao());
		center.x = x + w / 2;
		center.y = y + h + 2 * AvMain.hd;
		Canvas.endDlg();
	}

	public override void update()
	{
		updateText();
		moveCamera();
	}

	private void updateText()
	{
		if (!tfNgay.getText().Equals(string.Empty))
		{
			string text = tfNgay.getText();
			int num = 1;
			try
			{
				num = int.Parse(text);
			}
			catch (Exception)
			{
				tfNgay.setText("1");
			}
		}
		if (!tfThang.getText().Equals(string.Empty))
		{
			string text2 = tfThang.getText();
			int num2 = 1;
			try
			{
				num2 = int.Parse(text2);
			}
			catch (Exception)
			{
				tfThang.setText("1");
			}
		}
		if (!tfnam.getText().Equals(string.Empty))
		{
			string text3 = tfnam.getText();
			int num3 = 1987;
			try
			{
				num3 = int.Parse(text3);
			}
			catch (Exception)
			{
				tfnam.setText("1987");
			}
		}
	}

	public void create()
	{
		if (tfUser.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập họ và tên");
			return;
		}
		if (tfNgay.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập ngày sinh");
			return;
		}
		if (tfThang.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập tháng sinh");
			return;
		}
		if (tfnam.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập năm sinh");
			return;
		}
		if (tfAddress.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập địa chỉ");
			return;
		}
		if (tfCMND.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập CMND");
			return;
		}
		if (tfNgayCap.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập ngày cấp CMND");
			return;
		}
		if (tfNoiCap.Equals(string.Empty))
		{
			Canvas.startOKDlg("Bạn chưa nhập nơi cấp CMND");
			return;
		}
		Canvas.startWaitDlg();
		GlobalService.gI().createCharInfo(tfUser.getText(), int.Parse(tfNgay.getText()), int.Parse(tfThang.getText()), int.Parse(tfnam.getText()), tfAddress.getText(), tfCMND.getText(), tfNgayCap.getText(), tfNoiCap.getText(), tfNumberPhone.getText());
	}

	public void moveCamera()
	{
		if (timeOpen > 0)
		{
			timeOpen--;
		}
		if (vY != 0)
		{
			if (cmy < 0 || cmy > cmyLim)
			{
				if (vY > 500)
				{
					vY = 500;
				}
				else if (vY < -500)
				{
					vY = -500;
				}
				vY -= vY / 5;
				if (CRes.abs(vY / 10) <= 10)
				{
					vY = 0;
				}
			}
			cmy += vY / 15;
			cmtoY = cmy;
			if (vY != 0)
			{
				vY -= vY / 20 + 1;
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
	}

	public override void keyPress(int keyCode)
	{
		if (tfUser.isFocused())
		{
			tfUser.keyPressed(keyCode);
		}
		else if (tfnam.isFocused())
		{
			tfnam.keyPressed(keyCode);
		}
		else if (tfThang.isFocused())
		{
			tfThang.keyPressed(keyCode);
		}
		else if (tfNgay.isFocused())
		{
			tfNgay.keyPressed(keyCode);
		}
		else if (tfAddress.isFocused())
		{
			tfAddress.keyPressed(keyCode);
		}
		else if (tfCMND.isFocused())
		{
			tfCMND.keyPressed(keyCode);
		}
		if (tfNgayCap.isFocused())
		{
			tfNgayCap.keyPressed(keyCode);
		}
		else if (tfNoiCap.isFocused())
		{
			tfNoiCap.keyPressed(keyCode);
		}
		else if (tfNumberPhone.isFocused())
		{
			tfNumberPhone.keyPressed(keyCode);
		}
	}

	public override void updateKey()
	{
		base.updateKey();
		tfUser.update();
		tfnam.update();
		tfThang.update();
		tfNgay.update();
		tfAddress.update();
		tfCMND.update();
		tfNgayCap.update();
		tfNoiCap.update();
		tfNumberPhone.update();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.loadMap.paint(g);
		Canvas.loadMap.paintObject(g);
		Canvas.resetTrans(g);
		if (Canvas.currentDialog == null)
		{
			Canvas.paint.paintPopupBack(g, x, y, w, h, -1, false);
			for (int i = 0; i < strInfo.Length; i++)
			{
				Canvas.fontChatB.drawString(g, strInfo[i], x + w / 2, y + h - 5 * AvMain.hd - (Canvas.fontChatB.getHeight() - 2 * AvMain.hd) * strInfo.Length + i * (Canvas.fontChatB.getHeight() - 2), 2);
			}
			g.setClip(x, y + 4 * AvMain.hd, w, h - 8 * AvMain.hd);
			g.translate(0f, -cmy);
			tfUser.paint(g);
			tfnam.paint(g);
			tfThang.paint(g);
			tfNgay.paint(g);
			tfAddress.paint(g);
			tfCMND.paint(g);
			tfNgayCap.paint(g);
			tfNoiCap.paint(g);
			tfNumberPhone.paint(g);
			Canvas.resetTrans(g);
			base.paint(g);
		}
	}

	public void onCreate()
	{
		Out.println("onCreate");
		Canvas.endDlg();
		MapScr.gI().joinCitymap();
	}
}
