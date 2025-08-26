using System.Collections;

public class MyVector
{
	private ArrayList a;

	public MyVector()
	{
		a = new ArrayList();
	}

	public MyVector(ArrayList a)
	{
		this.a = a;
	}

	public void addElement(object o)
	{
		a.Add(o);
	}

	public int size()
	{
		return a.Count;
	}

	public object elementAt(int i)
	{
		return a[i];
	}

	public void removeElementAt(int i)
	{
		a.RemoveAt(i);
	}

	public void removeElement(object o)
	{
		a.Remove(o);
	}

	public void setElementAt(object o, int i)
	{
		a[i] = o;
	}

	public void removeAllElements()
	{
		a.Clear();
	}

	public void insertElementAt(object o, int i)
	{
		a.Insert(i, o);
	}
}
