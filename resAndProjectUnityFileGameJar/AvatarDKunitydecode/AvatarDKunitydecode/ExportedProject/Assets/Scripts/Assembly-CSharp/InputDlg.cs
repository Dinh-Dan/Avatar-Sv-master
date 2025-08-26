public class InputDlg : Dialog
{
	private class IActionOk : IAction
	{
		public void perform()
		{
			Canvas.inputDlg.center.perform();
		}
	}

	protected string[] info;

	public TField tfInput;

	public IAction okAction;

	private Image img;

	private int x;

	private int y;

	private int w;

	private int h;

	public InputDlg()
	{
		tfInput = new TField(string.Empty, null, new IActionOk());
		tfInput.isChangeFocus = false;
		tfInput.showSubTextField = false;
		tfInput.autoScaleScreen = true;
	}

	public override void commandTab(int index)
	{
		Canvas.currentMyScreen.commandTab(index);
	}

	public string getText()
	{
		return tfInput.getText();
	}

	public override void init(int hCan)
	{
		y = hCan - h - 50;
		tfInput.x = Canvas.hw - tfInput.width / 2;
		tfInput.y = y + h - tfInput.height - AvMain.hCmd / 2 - 10;
		if (center != null)
		{
			center.x = Canvas.w / 2;
			center.y = y + h - PaintPopup.hButtonSmall / 2;
		}
	}

	public void setImg(Image img)
	{
		this.img = img;
		h += img.getHeight();
		init(Canvas.hCan);
	}

	public void setInfo(string info, int index, int type, MyScreen parent)
	{
		initInfo(info, type);
		show();
		tfInput.parent = parent;
		tfInput.setFocus(true);
		tfInput.showSubTextField = false;
		center = new Command(T.OK, index, Canvas.hw, y + h - AvMain.hCmd / 2);
	}

	public void initInfo(string info, int type)
	{
		img = null;
		w = Canvas.w - 40;
		if (Canvas.normalFont.getWidth(info) + 20 < w)
		{
			w = Canvas.normalFont.getWidth(info) + 20;
		}
		if (w < Canvas.w / 2)
		{
			w = Canvas.w / 2;
		}
		this.info = Canvas.normalFont.splitFontBStrInLine(info, w - 20);
		h = AvMain.hCmd / 2 + 10 + tfInput.height + this.info.Length * AvMain.hNormal + 60;
		x = (Canvas.w - w) / 2;
		y = Canvas.hCan - h - 50;
		if (center != null)
		{
			center.x = Canvas.w / 2;
			center.y = y + h - PaintPopup.hButtonSmall / 2;
		}
		tfInput.isChangeFocus = false;
		tfInput.width = w - 20 * AvMain.hd;
		init(Canvas.hCan);
		tfInput.setText(string.Empty);
		tfInput.setIputType(ipKeyboard.TEXT);
		show();
	}

	public void setInfoIA(string info, IAction ok, int type, MyScreen me)
	{
		initInfo(info, type);
		okAction = ok;
		tfInput.parent = me;
		center = new Command(T.OK, okAction);
		tfInput.showSubTextField = false;
		show();
	}

	public void setInfoIkb(string info, IKbAction ac, int type, MyScreen me)
	{
		initInfo(info, type);
		tfInput.parent = me;
		center = new Command(T.OK, ac);
		init(Canvas.hCan);
		tfInput.showSubTextField = false;
		show();
	}

	public override void paint(MyGraphics g)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintPopupBack(g, x, y, w, h, -1, false);
		int num = y + (tfInput.y - y) / 2 - info.Length * AvMain.hNormal / 2 + 4 * AvMain.hd;
		if (img != null)
		{
			g.drawImage(img, x + w / 2, tfInput.y - img.getHeight() / 2 - 5 * AvMain.hd, 3);
			num -= img.getHeight() / 2;
		}
		int num2 = 0;
		int num3 = num;
		while (num2 < info.Length)
		{
			Canvas.normalFont.drawString(g, info[num2], Canvas.hw, num3, 2);
			num2++;
			num3 += AvMain.hNormal;
		}
		tfInput.paint(g);
		base.paint(g);
	}

	public override void keyPress(int keyCode)
	{
		tfInput.keyPressed(keyCode);
		if (keyCode == -5)
		{
			tfInput.action.perform();
		}
	}

	public override void updateKey()
	{
		tfInput.update();
		if (tfInput.isFocused())
		{
			right = null;
		}
		base.updateKey();
	}

	public override void show()
	{
		Canvas.currentDialog = this;
	}

	public void showWithCaption(string text)
	{
		tfInput.name = text;
		show();
	}
}
