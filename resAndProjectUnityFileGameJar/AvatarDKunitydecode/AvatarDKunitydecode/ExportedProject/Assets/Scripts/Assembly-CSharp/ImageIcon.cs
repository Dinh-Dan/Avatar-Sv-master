public class ImageIcon
{
	public Image img;

	public short w;

	public short h;

	public int count = -1;

	public ImageIcon()
	{
	}

	public ImageIcon(Image im)
	{
		img = im;
		count = 0;
		w = (short)im.getWidth();
		h = (short)im.getHeight();
	}
}
