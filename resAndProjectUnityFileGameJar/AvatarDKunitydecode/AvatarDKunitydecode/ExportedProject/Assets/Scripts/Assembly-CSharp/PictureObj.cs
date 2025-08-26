public class PictureObj
{
	public int x;

	public int y;

	public int orthor;

	public int ID;

	public int h;

	public int w;

	public bool linebreak;

	public PictureObj(int id, int x, int y, int or, bool line)
	{
		ID = id;
		this.x = x;
		this.y = y;
		orthor = or;
		linebreak = line;
	}
}
