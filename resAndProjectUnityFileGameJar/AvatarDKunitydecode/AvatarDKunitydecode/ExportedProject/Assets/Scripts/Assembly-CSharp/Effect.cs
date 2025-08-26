public abstract class Effect
{
	public bool isStop;

	public short IDAction = -1;

	public abstract void update();

	public abstract void paint(MyGraphics g);

	public void show()
	{
		Canvas.currentEffect.addElement(this);
	}

	public virtual void close()
	{
		Canvas.currentEffect.removeElement(this);
	}

	public virtual void paintBack(MyGraphics g)
	{
	}
}
