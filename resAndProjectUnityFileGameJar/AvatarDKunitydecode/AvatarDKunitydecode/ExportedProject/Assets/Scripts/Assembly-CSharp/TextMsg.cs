public class TextMsg
{
	public string[] text;

	public bool isOwner;

	public short wPopup;

	public TextMsg(string t, bool isO)
	{
		text = Canvas.fontChat.splitFontBStrInLine(t, Canvas.w / 2);
		for (int i = 0; i < text.Length; i++)
		{
			int num = Canvas.fontChat.getWidth(text[i]) + 10 * AvMain.hd;
			if (wPopup < num)
			{
				wPopup = (short)num;
			}
		}
		if (wPopup < 30 * AvMain.hd)
		{
			wPopup = (short)(40 * AvMain.hd);
		}
		isOwner = isO;
	}
}
