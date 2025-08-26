public class Command
{
	public string caption;

	public IAction action;

	public IKbAction ipaction;

	public sbyte indexMenu;

	public sbyte subIndex;

	public sbyte indexImage;

	public int x = -1;

	public int y = -1;

	public AvMain pointer;

	public Command()
	{
	}

	public Command(string caption, IAction action)
	{
		this.caption = caption;
		this.action = action;
	}

	public Command(string caption, IKbAction action)
	{
		this.caption = caption;
		ipaction = action;
	}

	public Command(string caption, int type)
	{
		this.caption = caption;
		indexMenu = (sbyte)type;
	}

	public Command(string caption, int type, AvMain pointer)
	{
		this.caption = caption;
		indexMenu = (sbyte)type;
		this.pointer = pointer;
	}

	public Command(string caption, int type, int sub)
	{
		this.caption = caption;
		indexMenu = (sbyte)type;
		subIndex = (sbyte)sub;
	}

	public Command(string caption, int type, int subIndex, AvMain pointer)
	{
		this.caption = caption;
		indexMenu = (sbyte)type;
		this.pointer = pointer;
		this.subIndex = (sbyte)subIndex;
	}

	public Command(string caption, int type, int x, int y)
	{
		this.caption = caption;
		indexMenu = (sbyte)type;
		this.x = x;
		this.y = y;
	}

	public Command(string caption, IAction action, int x, int y)
	{
		this.caption = caption;
		this.action = action;
		this.x = x;
		this.y = y;
	}

	public void perform()
	{
		Out.println("command gettext: " + Canvas.inputDlg.tfInput.getText());
		if (action != null)
		{
			action.perform();
		}
		else if (ipaction != null)
		{
			ipaction.perform(Canvas.inputDlg.tfInput.getText());
		}
		else if (pointer != null)
		{
			pointer.commandActionPointer(indexMenu, subIndex);
		}
		else if (Canvas.currentDialog != null)
		{
			Canvas.currentDialog.commandTab(indexMenu);
		}
		else
		{
			Canvas.currentMyScreen.commandTab(indexMenu);
		}
	}

	public virtual void update()
	{
	}

	public virtual void paint(MyGraphics g, int x, int y)
	{
	}
}
