public class OnScreen : MyScreen
{
	public static FrameImage imgButtomSmall;

	public static FrameImage imgIconButton;

	public MyVector listCmd = new MyVector();

	private bool isTranCmd;

	public void addCmd(int type, int indexImg)
	{
		Command command = new Command(string.Empty, type);
		command.subIndex = (sbyte)indexImg;
		listCmd.addElement(command);
	}

	public override void updateKey()
	{
		base.updateKey();
		Canvas.paint.updateKeyOn(left, center, right);
		if (Canvas.isPointerClick)
		{
			for (int i = 0; i < listCmd.size(); i++)
			{
				if (Canvas.isPoint(4 + imgButtomSmall.frameWidth / 2 + (imgButtomSmall.frameHeight + 4) * i - imgButtomSmall.frameWidth / 2, Canvas.hCan - PaintPopup.hButtonSmall / 2 + 1 - imgButtomSmall.frameHeight / 2, imgButtomSmall.frameWidth, imgButtomSmall.frameHeight))
				{
					Command command = (Command)listCmd.elementAt(i);
					command.indexImage = 1;
					Canvas.isPointerClick = false;
					isTranCmd = true;
					break;
				}
			}
		}
		if (!isTranCmd)
		{
			return;
		}
		if (Canvas.isPointerDown)
		{
			for (int j = 0; j < listCmd.size(); j++)
			{
				Command command2 = (Command)listCmd.elementAt(j);
				if (command2.indexImage == 1 && !Canvas.isPoint(4 + imgButtomSmall.frameWidth / 2 + (imgButtomSmall.frameHeight + 4) * j - imgButtomSmall.frameWidth / 2, Canvas.hCan - PaintPopup.hButtonSmall / 2 + 1 - imgButtomSmall.frameHeight / 2, imgButtomSmall.frameWidth, imgButtomSmall.frameHeight))
				{
					command2.indexImage = 0;
					break;
				}
			}
		}
		if (!Canvas.isPointerRelease)
		{
			return;
		}
		for (int k = 0; k < listCmd.size(); k++)
		{
			Command command3 = (Command)listCmd.elementAt(k);
			if (command3.indexImage == 1 && Canvas.isPoint(4 + imgButtomSmall.frameWidth / 2 + (imgButtomSmall.frameHeight + 4) * k - imgButtomSmall.frameWidth / 2, Canvas.hCan - PaintPopup.hButtonSmall / 2 + 1 - imgButtomSmall.frameHeight / 2, imgButtomSmall.frameWidth, imgButtomSmall.frameHeight))
			{
				command3.indexImage = 0;
				command3.perform();
				Canvas.isPointerRelease = false;
				isTranCmd = false;
				break;
			}
		}
	}

	public override void update()
	{
	}

	public override void paint(MyGraphics g)
	{
		paintBar(g, left, center, right, listCmd);
	}

	public static void paintBar(MyGraphics g, Command left, Command center, Command right, MyVector listCmd)
	{
		Canvas.resetTrans(g);
		Canvas.paint.paintTabSoft(g);
		if (((Canvas.currentDialog != null || ChatTextField.isShow) && Canvas.currentDialog != TransMoneyDlg.me) || ChatTextField.isShow)
		{
			return;
		}
		Canvas.paint.paintCmdBar(g, left, center, right);
		if (listCmd != null)
		{
			for (int i = 0; i < listCmd.size(); i++)
			{
				Command command = (Command)listCmd.elementAt(i);
				imgButtomSmall.drawFrame(command.indexImage, 4 + imgButtomSmall.frameWidth / 2 + (imgButtomSmall.frameHeight + 4) * i, Canvas.hCan - PaintPopup.hButtonSmall / 2 + 1, 0, 3, g);
				imgIconButton.drawFrame(command.subIndex, 4 + imgButtomSmall.frameWidth / 2 + (imgButtomSmall.frameHeight + 4) * i, Canvas.hCan - PaintPopup.hButtonSmall / 2 + 1, 0, 3, g);
			}
		}
	}
}
