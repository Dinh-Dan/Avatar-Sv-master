public abstract class Face : AvMain
{
	public void show()
	{
		Canvas.currentFace = this;
	}

	public virtual void init(int h)
	{
	}

	public void close()
	{
		Canvas.currentFace = null;
	}

	public override void updateKey()
	{
		if (Canvas.currentDialog == null)
		{
			base.updateKey();
		}
	}

	public override void paint(MyGraphics g)
	{
		if (Canvas.currentDialog == null)
		{
			base.paint(g);
		}
	}
}
