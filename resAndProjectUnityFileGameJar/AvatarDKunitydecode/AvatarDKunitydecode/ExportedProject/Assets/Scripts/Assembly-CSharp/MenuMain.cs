public class MenuMain : AvMain
{
	public virtual void update()
	{
	}

	public new virtual void updateKey()
	{
		base.updateKey();
	}

	public new virtual void paint(MyGraphics g)
	{
		base.paint(g);
	}

	public void show()
	{
		Canvas.menuMain = this;
	}
}
