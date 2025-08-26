public class ElementPlayer
{
	public int IDPlayer;

	public string name;

	public string subText;

	public MyVector text;

	public IAction action;

	public short numSMS;

	public ElementPlayer(int ID, string name, string text)
	{
		IDPlayer = ID;
		this.name = name;
		subText = text;
		this.text = new MyVector();
		addText(text, false);
	}

	public ElementPlayer(string name, string text)
	{
		IDPlayer = -1;
		this.name = name;
		subText = text;
	}

	public void addText(string te, bool isO)
	{
		subText = te;
		if (Canvas.fontChat.getWidth(subText) > Canvas.w / 3 * 2)
		{
			string[] array = Canvas.fontChat.splitFontBStrInLine(subText, Canvas.w / 3 * 2);
			subText = array[0];
		}
		if (!te.Equals(string.Empty))
		{
			TextMsg o = new TextMsg(te, isO);
			text.addElement(o);
		}
		if (numSMS < 99)
		{
			numSMS++;
		}
	}
}
